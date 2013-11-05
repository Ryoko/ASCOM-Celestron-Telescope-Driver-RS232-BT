namespace ASCOM.CelestronAdvancedBlueTooth.CelestroneDriver.TelescopeWorker
{
    using System;

    using ASCOM.CelestronAdvancedBlueTooth.CelestroneDriver.HardwareWorker;

    [TelescopeInteraction(3.1)]
    internal class CelestroneInteraction31 : CelestroneInteraction23
    {
        public CelestroneInteraction31(IDeviceWorker deviceWorker) : base(deviceWorker)
        {}

        public override DateTime RTCDateTime
        {
            get
            {
                return base.RTCDateTime;
            }
            set
            {

                var com = new byte[]
                {
                    (byte) 'P', 3, 178, 131,
                    (byte)value.Month, (byte)value.Day, 0, 0
                };
                this.SendBytes(com);
                
                com = new byte[]
                {
                    (byte) 'P', 3, 178, 132,
                    (byte)(value.Year / 256), (byte)(value.Year % 256), 0, 0
                };
                this.SendBytes(com);

                com = new byte[]
                {
                    (byte) 'P', 4, 178, 179,
                    (byte)value.Hour, (byte)value.Minute, (byte)value.Second, 0
                };
                this.SendBytes(com);

            }
        }

        public override TrackingMode TrackingMode
        {
            get
            {
                var mode = base.TrackingMode;
                if (this._telescopeModel == TelescopeModel.AdvancedGT || this._telescopeModel == TelescopeModel.CGE)
                    if(this._firmwareVersion >= 3.01 && this._firmwareVersion <= 3.04)
                        if (mode > TrackingMode.Off) mode = mode + 1;
                return mode;
            }
            set
            {
                var mode = value;
                if (this._telescopeModel == TelescopeModel.AdvancedGT || this._telescopeModel == TelescopeModel.CGE)
                    if (this._firmwareVersion >= 3.01 && this._firmwareVersion <= 3.04)
                        if (mode > TrackingMode.Off) mode = mode - 1;
                base.TrackingMode = mode;
            }
        }

        public override double VersionRequired
        {
            get { return 3.01; }
        }

        public override bool CanSetRTC
        {
            get { return true; }
        }

    }
}