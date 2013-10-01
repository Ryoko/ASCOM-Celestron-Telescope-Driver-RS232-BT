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
                try
                {
                    var res = this.GetValues("Z", 4);
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
                    SetValues("B", new[]{az, al}, 4);
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
                    var res = this.GetValues("E", 4);
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
                    SetValues("R", new []{ra, dec}, 4);
                }
                catch (Exception err)
                {
                    throw new Exception("Error setting Ra/Dec values", err);
                }
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
