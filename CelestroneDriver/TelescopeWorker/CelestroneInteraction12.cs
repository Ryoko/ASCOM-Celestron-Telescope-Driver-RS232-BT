namespace ASCOM.CelestronAdvancedBlueTooth.CelestroneDriver.TelescopeWorker
{
    using System;

    using ASCOM.CelestronAdvancedBlueTooth.CelestroneDriver.HardwareWorker;
    using ASCOM.CelestronAdvancedBlueTooth.CelestroneDriver.Utils;

    /// <summary>
    /// The celestrone interaction 12.
    /// </summary>
    [TelescopeInteraction(0)]
    internal class CelestroneInteraction12 : ATelescopeInteraction
    {
        public CelestroneInteraction12(IDeviceWorker deviceWorker) : base(deviceWorker)
        {}

        public override double FirmwareVersion
        {
            get
            {
                //var com = new[] {(byte) 'V'};
                var res = this.DeviceWorker.Transfer(GeneralCommands.GET_VERSION); //Transfer("V");// SendBytes(com);
                if (res.Length < 2) throw new Exception("Wrong answer");
                var low = (double)res[1];
                low = low / (low < 10 ? 10 : low < 100 ? 100 : 1000);
                this._firmwareVersion = res[0] + low;
                return this._firmwareVersion;
            }
        }

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
                    var res = this.GetValues(GeneralCommands.GET_ALTAZ_LP, 4);
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
                    this.SetValues(GeneralCommands.SET_ALTAZ_LP, new[]{az, al}, 4);
                }
                catch (Exception err)
                {
                    throw new Exception("Error setting Alt/Azm values", err);
                }
            }
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
                    var res = this.GetValues(GeneralCommands.GET_RADEC_LP, 4);
                    var ra = res[0]/15d;
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
                    this.SetValues(GeneralCommands.SET_RADEC_LP, new []{ra, dec}, 4);
                }
                catch (Exception err)
                {
                    throw new Exception("Error setting Ra/Dec values", err);
                }
            }
        }

        public override bool IsAlignmentComplete
        {
            get
            {
                //var com = new[] {(byte) 'J'};
                var res = this.DeviceWorker.Transfer(GeneralCommands.IS_ALIGNED);//SendBytes(com);
                return res[0] == 1;
            }
        }

        public override bool IsGoToInProgress
        {
            get
            {
                //var com = new[] {(byte) 'L'};
                var res = this.DeviceWorker.Transfer(GeneralCommands.IS_SLEWING);//SendBytes(com);
                return res[0] == (byte) '1';
            }
        }

        public override void CancelGoTo()
        {
            //var com = new[] {(byte) 'M'};
            this.DeviceWorker.Transfer(GeneralCommands.ABORT_SLEW);//SendBytes(com);
        }

        public override double VersionRequired
        {
            get { return 0; }
        }

    }
}
