﻿using System;
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

        public override AltAzm AltAzm
        {
            get
            {
                int Alt, Azm;
                if (driverWorker.GetPairValues("z", out Alt, out Azm))
                {
                    return new AltAzm(((double)Alt / 4294967296) * 360, ((double)Azm / 4294967296) * 360);
                }
                throw new Exception("Error getting parameters");
            }
            set
            {
                if (driverWorker.CommandBool(string.Format("b{0},{1}#",
                    Utils.Utils.Deg2HEX32(value.Azm), Utils.Utils.Deg2HEX32(value.Alt)), false))
                {
                    return;
                }
                throw new Exception("Error setting parameters");
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
