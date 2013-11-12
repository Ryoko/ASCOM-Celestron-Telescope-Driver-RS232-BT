namespace ASCOM.CelestronAdvancedBlueTooth.CelestroneDriver.TelescopeWorker
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using ASCOM.CelestronAdvancedBlueTooth.CelestroneDriver.HardwareWorker;
    using ASCOM.CelestronAdvancedBlueTooth.CelestroneDriver.Utils;

    [TelescopeInteraction(4.21)]
    internal class CelestroneInteraction421 : CelestroneInteraction414
    {
        public CelestroneInteraction421(IDeviceWorker deviceWorker)
            : base(deviceWorker)
        { }

        public override DateTime TelescopeDateTime
        {
            get
            {
                var res = this.SendCommand(GeneralCommands.GET_TIME_NEW);
                if (res.Length < 6) throw new DriverException("Wrong answer");
                var dt = new DateTime(res[5] + 2000, res[3], res[4], res[0], res[1], res[2], DateTimeKind.Unspecified);
                var offset = res[6] < 100 ? res[6] : 256 - res[6];
                var dtUTC = DateTime.SpecifyKind(dt.AddHours(-(offset/4)).AddHours(-res[7]), DateTimeKind.Utc);
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
                tz *= 4;
                var com = new byte[]
                {
                    (byte)GeneralCommands.SET_TIME_NEW,
                    (byte)dt.Hour, (byte)dt.Minute, (byte)dt.Second,
                    (byte)dt.Month, (byte)dt.Day, (byte)(dt.Year - 2000),
                    (byte)tz, (byte)dlst
                };
                this.SendBytes(com);

            }
        }

        public override void Hibernate()
        {
            this.SendCommand(GeneralCommands.HIBERNATE);
        }

        public override void WakeUp()
        {
            this.SendCommand(GeneralCommands.WAKEUP, (byte)GeneralCommands._1);
        }

        public override void SetHome()
        {
            this.SendCommand(GeneralCommands.SET_HOME);
        }

        public override void GoHome()
        {
            this.SendCommand(GeneralCommands.GOTO_HOME);
        }

        public override bool CanHibernate
        {
            get
            {
                return true;
            }
        }

        public override bool CanWorkHome
        {
            get
            {
                return true;
            }
        }
        
        public override double VersionRequired
        {
            get { return 4.21; }
        }
    }
}
