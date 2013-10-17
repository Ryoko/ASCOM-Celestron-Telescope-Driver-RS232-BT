using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using ASCOM.Astrometry.Exceptions;
using ASCOM.CelestronAdvancedBlueTooth.Utils;
using ASCOM.DeviceInterface;
using ASCOM.Utilities;
using ASCOM.Utilities.Interfaces;

namespace ASCOM.CelestronAdvancedBlueTooth.TelescopeWorker
{
    class CelestroneInteraction16 : CelestroneInteraction12
    {
        public CelestroneInteraction16(IDriverWorker _driverWorker) : base(_driverWorker)
        {
        }

        /// <summary>
        /// Gets or sets the ra dec.
        /// </summary>
        /// <exception cref="Exception">
        /// </exception>
        public override Coordinates RaDec
        {
            get
            {
                try
                {
                    var res = this.GetValues("e", 6);
                    var ra = res[0] / 15d;
                    var dec = res[1];

                    if (dec > 180) dec -= 360;

                    return new Coordinates(ra, dec);
                }
                catch (Exception err)
                {
                    throw new Exception("Error getting Ra/Dec values", err);
                }
            }
            set
            {
                try
                {
                    var ra = value.Ra * 15d;
                    var dec = value.Dec < 0 ? value.Dec + 360 : value.Dec;
                    SetValues("r", new[] { ra, dec }, 6, 8);
                }
                catch (Exception err)
                {
                    throw new Exception("Error setting Ra/Dec values", err);
                }
            }
        }

        public override TrackingMode TrackingMode
        {
            get { throw new System.NotImplementedException(); }
            set
            {
                var com = new[] {(byte)'T', (byte)value};
                SendBytes(com);
            }
        }

        public override void SlewFixedRate(SlewAxes axis, int rate)
        {
            var r = (byte)Math.Abs(rate);
            if (r > 9) throw new Exception("Wrong parameter");
            var devId = GetDeviceId(axis);
            var command = rate < 0 ? DeviceCommands.MC_SET_NEG_FIXED_GUIDERATE : DeviceCommands.MC_SET_POS_FIXED_GUIDERATE;
            //var com = new byte[] { (byte)'P', 2, DevId, (byte)dir, (byte)rate, 0, 0, 0 };
            SendCommandToDevice(devId, command, 0, r);
        }

        public override void SlewVariableRate(SlewAxes axis, double rate)
        {
            var r = (int)Math.Abs(rate*4*3600);
            var devId = GetDeviceId(axis);
            //var com = new byte[] { (byte)'P', 3, devId, (byte)dir, (byte) (r/256), (byte) (r%256), 0, 0 };
            //SendCommand(com);
            var command = rate < 0 ? DeviceCommands.MC_SET_NEG_VARIABLE_GUIDERATE : DeviceCommands.MC_SET_POS_VARIABLE_GUIDERATE;
            SendCommandToDevice(devId, command, 0, (byte) (r/256), (byte) (r%256));
        }

        public override void SlewHighRate(SlewAxes axis, double rate)
        {
            var r = Math.Abs((long)(rate*3600*1024));
            var h = (byte) ((r/0x10000) & 0xff);
            var m = (byte) ((r/0x100) & 0xff);
            var l = (byte) (r & 0xff);
            var devId = GetDeviceId(axis);
            var command = rate < 0 ? DeviceCommands.MC_SET_NEG_VARIABLE_GUIDERATE : DeviceCommands.MC_SET_POS_VARIABLE_GUIDERATE;
            SendCommandToDevice(devId, command, 0, h, m, l);
        }

/*        public override bool SetTrackingRate(DriveRates rate, TrackingMode mode)
        {
            byte hRate;
            switch (rate)
            {
                case DriveRates.driveSidereal:
                    hRate = 0xff;
                    break;
                case DriveRates.driveSolar:
                    hRate = 0xfe;
                    break;
                case DriveRates.driveLunar:
                    hRate = 0xfd;
                    break;
                default:
                    throw new ValueNotAvailableException("Wring tracking rate value");
            }
            if (mode > TrackingMode.AltAzm)
            {
                var command = mode == TrackingMode.EQS
                    ? DeviceCommands.MC_SET_NEG_VARIABLE_GUIDERATE
                    : DeviceCommands.MC_SET_POS_VARIABLE_GUIDERATE;
                SendCommandToDevice(GetDeviceId(SlewAxes.RaAzm), command, 0, ()0xff, hRate);
            }
            else
            {
                SendCommandToDevice(GetDeviceId(SlewAxes.RaAzm), DeviceCommands.MC_SET_POS_VARIABLE_GUIDERATE, 0, 0, 0);
            }
            return true;
        }
*/
        public override bool IsGPS
        {
            get
            {
                var com = new byte[] { (byte)'P', 1, 176, 55, 0, 0, 0, 1 };
                var res = SendBytes(com);
                if (res.Length < 2 || res[res.Length - 1] != '#') throw new ProtocolViolationException("Error receiving isGPS property");
                return res[0] > 0;
            }
        }

        public override DateTime GpsDateTime
        {
            get
            {
                var com = new byte[] { (byte)'P', 1, 176, 3, 0, 0, 0, 2 };
                var res = SendCommand(com); 
                var month = (int)res[0];
                var day = (int)res[1];

                com = new byte[] { (byte)'P', 1, 176, 4, 0, 0, 0, 2 };
                res = SendCommand(com); 
                var year = (int)res[0] * 256 + res[1];

                com = new byte[] { (byte)'P', 1, 176, 51, 0, 0, 0, 3 };
                res = SendCommand(com);

                return new DateTime(year, month, day, res[2], res[1], res[0]);
            }
        }

        public override LatLon GPSLocation
        {
            get
            {
                var com = new byte[] { (byte)'P', 1, 176, 1, 0, 0, 0, 3 };
                var res = SendCommand(com);
                var lat = ((
                    ((double) res[0])*65536 +
                    ((double) res[1])*256 +
                    res[2])/Math.Pow(2, 24))*360;
                com[3] = 2;
                res = SendCommand(com);
                var lon = ((
                    ((double)res[0]) * 65536 +
                    ((double)res[1]) * 256 +
                    res[2]) / Math.Pow(2, 24)) * 360;
                return new LatLon(lat, lon);
            }
        }

        public override DateTime RTCDateTime
        {
            get
            {
                var com = new byte[] { (byte)'P', 1, 178, 3, 0, 0, 0, 2 };
                var res = SendCommand(com); 
                var month = (int)res[0];
                var day = (int)res[1];

                com = new byte[] { (byte)'P', 1, 178, 4, 0, 0, 0, 2 };
                res = SendCommand(com); 
                var year = (int)res[0] * 256 + res[1];

                com = new byte[] { (byte)'P', 1, 178, 51, 0, 0, 0, 3 };
                res = SendCommand(com);

                return new DateTime(year, month, day, res[2], res[1], res[0]);
            }
            set { throw new System.NotImplementedException(); }
        }

        public override double GetDeviceVersion(DeviceID device)
        {
//            var com = new byte[] { (byte)'P', 1, (byte)device, 254, 0, 0, 0, 2 };
            var ans = SendCommandToDevice(device, DeviceCommands.GET_VER, 2);
//            var res = SendCommand(com);
            var low = (double)ans[1];
            low = low / (low < 10 ? 10 : low < 100 ? 100 : 1000);
            return ans[0] + low;
        }

        public override double VersionRequired
        {
            get { return 1.6; }
        }

        public override void GoToPosition(AltAzm pos)
        {
            var buf = DoubleToBytes(pos.Alt);
            if (pos.Azm < 0 || pos.Azm > 180)//Unsafe Azm position
            {
                if (pos.Alt < 80 || pos.Alt > 100)
                    throw new ArgumentOutOfRangeException("Azimuth position is out of safety range");
                
                // Move Alt axis to safe position before moving Azm axis to unsafe position
                SendCommandToDevice(DeviceID.DecAltMotor, DeviceCommands.MC_GOTO_FAST, 0, buf);
                var tBeginMove = Environment.TickCount;
                while (true)
                {
                    Thread.Sleep(100);
                    var p = GetPosition();
                    if (p.Alt > 80 && p.Alt < 100) break;
                    if (tBeginMove + 60000 < Environment.TickCount)
                        throw new DriverException("Can not move Alt axis to save position");
                }
            }
            else
            {
                SendCommandToDevice(DeviceID.DecAltMotor, DeviceCommands.MC_GOTO_FAST, 0, buf);
            }
            buf = DoubleToBytes(pos.Azm);
            SendCommandToDevice(DeviceID.RaAzmMotor, DeviceCommands.MC_GOTO_FAST, 0, buf);
        }

        public override AltAzm GetPosition()
        {
            var res = SendCommandToDevice(DeviceID.DecAltMotor, DeviceCommands.MC_GET_POSITION, 3);
            var alt = BytesToDouble(res.Take(res.Length - 1).ToArray());
            res = SendCommandToDevice(DeviceID.RaAzmMotor, DeviceCommands.MC_GET_POSITION, 3);
            var azm = BytesToDouble(res.Take(res.Length - 1).ToArray());
            var rest = "";
            foreach (var re in res)
            {
                rest += string.Format(" {0:x}", re);
            }
            //Debug.WriteLine(string.Format("[{0}] DevID={1} Com={2} res={3}",
            //    DateTime.Now, (byte)DeviceID.RaAzmMotor, (byte)DeviceCommands.MC_GET_POSITION, rest));

            return new AltAzm(alt, azm);
        }

        public override bool IsSlewDone(DeviceID deviceId)
        {
            var res = SendCommandToDevice(deviceId, DeviceCommands.MC_SLEW_DONE, 1);
            return res[0] != 0;
        }

        public override bool CanWorkPosition
        {
            get { return true; }
        }

        public override bool CanSetTracking
        {
            get { return true; }
        }

        public override bool CanSlewFixedRate
        {
            get { return true; }
        }

        public override bool CanSlewVariableRate
        {
            get { return true; }
        }

        public override bool CanWorkGPS
        {
            get { return true; }
        }

        public override bool CanGetRTC
        {
            get { return true; }
        }

        public override bool CanGetDeviceVersion
        {
            get { return true; }
        }

        public override bool CanSetTrackingRates
        {
            get { return true; }
        }

        public override bool CanSlewHighRate
        {
            get { return true; }
        }
    }
}
