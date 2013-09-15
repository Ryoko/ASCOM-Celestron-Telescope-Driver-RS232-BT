using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASCOM.CelestronAdvancedBlueTooth.Utils;
using ASCOM.Utilities;
using ASCOM.Utilities.Interfaces;

namespace ASCOM.CelestronAdvancedBlueTooth.TelescopeWorker
{
    class CelestroneInteraction16 : CelestroneInteraction12
    {
        public CelestroneInteraction16(IDriverWorker _driverWorker) : base(_driverWorker)
        {
        }

        public override Coordinates RaDec
        {
            get
            {
                int Ra, Dec;
                if (driverWorker.GetPairValues("e", out Ra, out Dec))
                {
                    return new Coordinates((Ra / 4294967296) * 360, (Dec / 4294967296) * 360);
                }
                throw new Exception("Error getting parameters");
            }
            set
            {
                if (driverWorker.CommandBool(string.Format("r{0},{1}#",
                    Utils.Utils.Deg2HEX32(value.Ra), Utils.Utils.Deg2HEX32(value.Dec)), false))
                {
                    return;
                }
                throw new Exception("Error setting parameters");
            }
        }

        public override TrackingMode TrackingMode
        {
            get { throw new System.NotImplementedException(); }
            set
            {
                var com = new[] {(byte)'T', (byte)value};
                SendCommand(com);
            }
        }

        public override void SlewFixedRate(Direction dir, SlewAxes axis, int rate)
        {
            if (rate < 0 || rate > 9) throw new Exception("Wrong parameter");
            var com = new byte[] {(byte) 'P', 2, (byte) axis, (byte) dir, (byte) rate, 0, 0, 0};
            SendCommand(com);
        }

        public override void SlewVariableRate(Direction dir, SlewAxes axis, double rate)
        {
            var r = (int)(rate * 4);
            var com = new byte[] { (byte)'P', 3, (byte)axis, (byte)dir, (byte) (r/256), (byte) (r%256), 0, 0 };
            SendCommand(com);
        }

        public override bool IsGPS
        {
            get
            {
                var com = new byte[] { (byte)'P', 1, 176, 55, 0, 0, 0, 1 };
                var res = SendCommand(com);
                return res[0] > 0;
            }
        }

        public override DateTime GpsDateTime
        {
            get { throw new System.NotImplementedException(); }
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
            var com = new byte[] { (byte)'P', 1, (byte)device, 254, 0, 0, 0, 2 };
            var res = SendCommand(com);
            return res[0] + (double)res[1] / 10;
        }

        public override double VersionRequired
        {
            get { return 1.6; }
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
    }
}
