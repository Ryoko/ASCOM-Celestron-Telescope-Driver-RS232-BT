﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASCOM.CelestronAdvancedBlueTooth.Utils;
using ASCOM.Utilities;
using ASCOM.Utilities.Interfaces;

namespace ASCOM.CelestronAdvancedBlueTooth.TelescopeWorker
{
    internal class CelestroneWorker41 : CelestroneWorker31
    {
        public CelestroneWorker41(IDriverWorker _driverWorker)
            : base(_driverWorker)
        {
        }

        public override void SyncRaDec(Coordinates coordinates)
        {
            if (driverWorker.CommandBool(string.Format("s{0},{1}#",
                Utils.Utils.Deg2HEX32(coordinates.Ra), Utils.Utils.Deg2HEX32(coordinates.Dec)), false))
            {
                return;
            }
            throw new Exception("Error setting parameters");
        }

        public override bool CanSyncRaDec
        {
            get { return true; }
        }

        public override double VersionRequired
        {
            get { return 4.1; }
        }
    }
}
