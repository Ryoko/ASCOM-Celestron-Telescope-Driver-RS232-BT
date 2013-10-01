using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using ASCOM.CelestronAdvancedBlueTooth.Utils;

namespace ASCOM.CelestronAdvancedBlueTooth.TelescopeWorker
{
    class CelestroneInteraction22 : CelestroneInteraction16
    {
        public CelestroneInteraction22(IDriverWorker _driverWorker)
            : base(_driverWorker)
        {
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
                    SetValues("b", new[] { az, al }, 6, 8);
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
                var res = driverWorker.CommandString("m", false);//SendCommand(com);
                _telescopeModel = (TelescopeModel) res[0];
                return _telescopeModel;
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
