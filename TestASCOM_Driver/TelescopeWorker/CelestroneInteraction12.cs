using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASCOM.CelestronAdvancedBlueTooth.Utils;
using ASCOM.Utilities;
using ASCOM.Utilities.Interfaces;

namespace ASCOM.CelestronAdvancedBlueTooth.TelescopeWorker
{
    internal class CelestroneInteraction12 : ATelescopeInteraction
    {
        public CelestroneInteraction12(IDriverWorker _driverWorker) : base(_driverWorker)
        {
        }

        public override AltAzm AltAzm
        {
            get
            {
                int Alt, Azm;
                if (driverWorker.GetPairValues("Z", out Alt, out Azm))
                {
                    var az = ((double) Alt/65536)*360;
                    if (az < 0) az += 360;
                    return new AltAzm(az, ((double)Azm/65536)*360);
                }
                throw new Exception("Error getting parameters");
            }
            set
            {
                var az = (value.Azm > 180) ? value.Azm - 360 : value.Azm;
                if (driverWorker.CommandBool(string.Format("B{0},{1}#",
                    Utils.Utils.Deg2HEX16(az), Utils.Utils.Deg2HEX16(value.Alt)), false))
                {
                    return;
                }
                throw new Exception("Error setting parameters");
            }
        }

        public override Coordinates RaDec
        {
            get
            {
                int Ra, Dec;
                if (driverWorker.GetPairValues("E", out Ra, out Dec))
                {
                    return new Coordinates(((double)Ra/65536)*24, ((double)Dec/65536)*360);
                }
                throw new Exception("Error getting parameters");
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
