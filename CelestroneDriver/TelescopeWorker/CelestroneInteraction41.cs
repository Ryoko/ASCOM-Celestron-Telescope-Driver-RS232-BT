﻿namespace ASCOM.CelestronAdvancedBlueTooth.CelestroneDriver.TelescopeWorker
{
    using System;
    using System.Collections.Generic;

    using ASCOM.CelestronAdvancedBlueTooth.CelestroneDriver.HardwareWorker;
    using ASCOM.CelestronAdvancedBlueTooth.CelestroneDriver.Utils;

    [TelescopeInteraction(4.1)]
    internal class CelestroneInteraction41 : CelestroneInteraction31
    {
        public CelestroneInteraction41(IDeviceWorker deviceWorker) : base(deviceWorker)
        {}

        public override void SyncRaDec(Coordinates coordinates)
        {
            if (this.CommandBool(string.Format("{0}{1}{2}{3}#",
                GeneralCommands.SYNC_HP, Utils.Deg2HEX32(coordinates.Ra),
                GeneralCommands.COMMA, Utils.Deg2HEX32(coordinates.Dec)), false))
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
