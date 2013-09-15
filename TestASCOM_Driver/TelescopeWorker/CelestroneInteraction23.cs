using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using ASCOM.CelestronAdvancedBlueTooth.Utils;

namespace ASCOM.CelestronAdvancedBlueTooth.TelescopeWorker
{
    class CelestroneInteraction23 : CelestroneInteractin22
    {
        public CelestroneInteraction23(IDriverWorker _driverWorker) : base(_driverWorker)
        {
        }

        public override TrackingMode TrackingMode
        {
            get
            {
                var com = new[] { (byte)'t' };
                var res = SendCommand(com);
                return (TrackingMode) res[0];
            }
            set
            {
                var com = new[] { (byte)'T', (byte)value };
                SendCommand(com);
            }
        }

        public override DateTime TelescopeDateTime
        {
            get
            {
                var com = new[] { (byte)'h' };
                var res = SendCommand(com);
                var dt = new DateTime(res[5] + 2000, res[4], res[3], res[0], res[1], res[2], DateTimeKind.Unspecified);
                var offset = res[6] < 100 ? res[6] : 256 - res[6];
                var dtUTC = DateTime.SpecifyKind(dt.AddHours(-offset).AddHours(-res[7]), DateTimeKind.Utc);
                return dtUTC.ToLocalTime();
            }
            set
            {
                var dt = value.ToUniversalTime();
                var tz = (int)(TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now).TotalHours + 0.5);
                var dlst = TimeZone.CurrentTimeZone.IsDaylightSavingTime(DateTime.Now) ? 1 : 0;
                if (tz < 0) tz = 256 + tz;
                var com = new byte[]
                {
                    (byte)'H',
                    (byte)dt.Hour, (byte)dt.Minute, (byte)dt.Second,
                    (byte)dt.Month, (byte)dt.Day, (byte)(dt.Year - 2000),
                    (byte)tz, (byte)dlst
                };
                SendCommand(com);
            }
        }

        public override LatLon TelescopeLocation
        {
            get
            {
                var com = new[] { (byte)'w' };
                var res = SendCommand(com);
                var lat = new DMS(res[0], res[1], res[2]) { Sign = res[3] == 0 ? 1 : -1 };
                var lon = new DMS(res[4], res[5], res[6]) { Sign = res[7] == 0 ? 1 : -1 };
                return new LatLon((double)lat.Deg, (double)lon.Deg);
            }
            set
            {
                var lat = new DMS((decimal) value.Lat);
                var lon = new DMS((decimal) value.Lon);
                var com = new[]
                {
                    (byte)'W', 
                    (byte)lat.D, (byte)lat.M, (byte)(lat.S + .5M), (byte)(lat.Sign > 0 ? 0 : 1),
                    (byte)lon.D, (byte)lon.M, (byte)(lon.S + .5M), (byte)(lon.Sign > 0 ? 0 : 1)
                };
                SendCommand(com);
            }
        }

        public override double VersionRequired
        {
            get { return 2.3; }
        }

        public override bool CanGetTracking
        {
            get { return true; }
        }

        public override bool CanWorkDateTime
        {
            get { return true; }
        }

        public override bool CanWorkLocation
        {
            get { return true; }
        }

    }
}
