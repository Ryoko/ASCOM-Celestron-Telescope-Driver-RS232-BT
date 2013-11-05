namespace ASCOM.CelestronAdvancedBlueTooth.CelestroneDriver.TelescopeWorker
{
    using System;

    using ASCOM.CelestronAdvancedBlueTooth.CelestroneDriver.HardwareWorker;
    using ASCOM.CelestronAdvancedBlueTooth.CelestroneDriver.Utils;

    [TelescopeInteraction(2.2)]
    class CelestroneInteraction22 : CelestroneInteraction16
    {
        public CelestroneInteraction22(IDeviceWorker deviceWorker) : base(deviceWorker)
        {}

        /// <summary>
        /// Gets or sets the alt azm.
        /// </summary>
        /// <exception cref="Exception">
        /// </exception>
        public override AltAzm AltAzm
        {
            get
            {
                try
                {
                    var res = this.GetValues("z", 6);
                    var alt = res[0];
                    var azm = res[1];
                    if (alt > 180) alt -= 360;
                    if (azm < 0) azm += 360;
                    return new AltAzm(alt, azm);
                }
                catch (Exception err)
                {
                    throw new Exception("Error getting Alt/Azm values", err);
                }
            }

            set
            {
                try
                {
                    var az = (value.Azm > 180) ? value.Azm - 360 : value.Azm;
                    var al = (value.Alt < 0) ? value.Alt + 360 : value.Alt;
                    this.SetValues("b", new[] { az, al }, 6, 8);
                }
                catch (Exception err)
                {
                    throw new Exception("Error setting Alt/Azm values", err);
                }
            }
        }

        public override TelescopeModel GetModel
        {
            get
            {
                var com = new[] { (byte)'m' };
                var res = this.DeviceWorker.Transfer("m");//SendBytes(com);
                this._telescopeModel = (TelescopeModel) res[0];
                return this._telescopeModel;
            }
        }

        public override double VersionRequired
        {
            get { return 2.2; }
        }

        public override bool CanGetModel
        {
            get { return true; }
        }
    }
}
