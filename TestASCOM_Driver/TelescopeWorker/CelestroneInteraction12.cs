using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASCOM.CelestronAdvancedBlueTooth.Utils;
using ASCOM.Utilities;
using ASCOM.Utilities.Interfaces;

namespace ASCOM.CelestronAdvancedBlueTooth.TelescopeWorker
{
    /// <summary>
    /// The celestrone interaction 12.
    /// </summary>
    internal class CelestroneInteraction12 : ATelescopeInteraction
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CelestroneInteraction12"/> class.
        /// </summary>
        /// <param name="_driverWorker">
        /// The _driver worker.
        /// </param>
        public CelestroneInteraction12(IDriverWorker _driverWorker) : base(_driverWorker)
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
                var res = this.GetValues("Z", 4);
                var alt = res[0] * 360d;
                var azm = res[1] * 360d;
                if (alt > 180) alt -= 360;
                if (azm < 0) azm += 360;
                return new AltAzm(alt, azm);
            }

            set
            {
                var az = (value.Azm > 180) ? value.Azm - 360 : value.Azm;
                var al = (value.Alt < 0) ? value.Alt + 360 : value.Alt;
                if (
                    driverWorker.CommandBool(
                        string.Format("B{0},{1}#", Utils.Utils.Deg2HEX16(az), Utils.Utils.Deg2HEX16(al)), false))
                {
                    return;
                }

                throw new Exception("Error setting parameters");
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
                var res = this.GetValues("E", 4);
                var ra = res[0] * 24;
                var dec = res[1] * 360;

                if (dec > 180) dec -= 360;

                return new Coordinates(ra, dec);
            }
            set
            {
                if (driverWorker.CommandBool(string.Format("R{0},{1}#",
                    Utils.Utils.RADeg2HEX16(value.Ra), Utils.Utils.Deg2HEX16(value.Dec)), false))
                {
                    return;
                }
                throw new Exception("Error setting parameters");
            }
        }


        public override double FirmwareVersion
        {
            get
            {
                //var com = new[] {(byte) 'V'};
                var res = driverWorker.CommandString("V", false);// SendCommand(com);
                _firmwareVersion = res[0] + (double)res[1] / 10;
                return _firmwareVersion;
            }
        }

        public override bool IsAlignmentComplete
        {
            get
            {
                //var com = new[] {(byte) 'J'};
                var res = driverWorker.CommandString("J", false);//SendCommand(com);
                return res[0] == 1;
            }
        }

        public override bool IsGoToInProgress
        {
            get
            {
                //var com = new[] {(byte) 'L'};
                var res = driverWorker.CommandString("L", false);//SendCommand(com);
                return res[0] == (byte) '1';
            }
        }

        public override void CancelGoTo()
        {
            //var com = new[] {(byte) 'M'};
            driverWorker.CommandString("M", false);//SendCommand(com);
        }

        public override double VersionRequired
        {
            get { return 1.2; }
        }

    }
}
