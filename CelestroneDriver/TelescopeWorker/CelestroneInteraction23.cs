namespace ASCOM.CelestronAdvancedBlueTooth.CelestroneDriver.TelescopeWorker
{
    using System;

    using ASCOM;
    using ASCOM.CelestronAdvancedBlueTooth.CelestroneDriver.HardwareWorker;
    using ASCOM.CelestronAdvancedBlueTooth.CelestroneDriver.Utils;

    [TelescopeInteraction(2.3)]
    class CelestroneInteraction23 : CelestroneInteraction22
    {
        public CelestroneInteraction23(IDeviceWorker deviceWorker) : base(deviceWorker)
        {}

        public override TrackingMode TrackingMode
        {
            get
            {
                var com = new[] { (byte)'t' };
                var res = this.SendBytes(com);
                return (TrackingMode) res[0];
            }
            set
            {
                var com = new[] { (byte)'T', (byte)value };
                this.SendBytes(com);
            }
        }

        public override DateTime TelescopeDateTime
        {
            get
            {
                var res = this.SendCommand(GeneralCommands.GET_TIME);
                if (res.Length < 6) throw new DriverException("Wrong answer");
                var dt = new DateTime(res[5] + 2000, res[3], res[4], res[0], res[1], res[2], DateTimeKind.Unspecified);
                var offset = res[6] < 100 ? res[6] : 256 - res[6];
                var dtUTC = DateTime.SpecifyKind(dt.AddHours(-offset).AddHours(-res[7]), DateTimeKind.Utc);
                return dtUTC.ToLocalTime();
            }
            set
            {
                DateTime dt = value;
                if (value.Kind == DateTimeKind.Utc)
                {
                    dt = value.ToLocalTime();
                }
                var tz = (int)(TimeZone.CurrentTimeZone.GetUtcOffset(dt).TotalHours + 0.5);
                var dlst = TimeZone.CurrentTimeZone.IsDaylightSavingTime(DateTime.Now) ? 1 : 0;
                
                if (tz < 0) tz = 256 + tz;
                var com = new byte[]
                {
                    (byte)GeneralCommands.SET_TIME,
                    (byte)dt.Hour, (byte)dt.Minute, (byte)dt.Second,
                    (byte)dt.Month, (byte)dt.Day, (byte)(dt.Year - 2000),
                    (byte)tz, (byte)dlst
                };
                this.SendBytes(com);
            }
        }

        public override LatLon TelescopeLocation
        {
            get
            {
                var com = new[] { (byte)'w' };
                var res = this.SendBytes(com);
                if (res.Length < 8) return null;
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
                this.SendBytes(com);
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
