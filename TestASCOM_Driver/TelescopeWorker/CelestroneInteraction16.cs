using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
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

        public override bool SetTrackingRate(DriveRates rate, TrackingMode mode)
        {
            int hRate;
            switch (rate)
            {
                case DriveRates.driveSidereal:
                    hRate = 0xffff;
                    break;
                case DriveRates.driveSolar:
                    hRate = 0xfffe;
                    break;
                case DriveRates.driveLunar:
                    hRate = 0xfffd;
                    break;
                default:
                    throw new ValueNotAvailableException("Wring tracking rate value");
            }
            var dir = mode == TrackingMode.EQS ? Direction.Negative : Direction.Positive;
            SlewVariableRate(dir, SlewAxes.RaAzm, hRate);
            return true;
        }

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

        public override bool CanSetTrackingRates
        {
            get { return true; }
        }
    }
}
