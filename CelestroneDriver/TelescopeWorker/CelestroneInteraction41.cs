namespace ASCOM.CelestronAdvancedBlueTooth.CelestroneDriver.TelescopeWorker
{
    using System;

    using ASCOM.CelestronAdvancedBlueTooth.CelestroneDriver.HardwareWorker;
    using ASCOM.CelestronAdvancedBlueTooth.CelestroneDriver.Utils;

    [TelescopeInteraction(4.1)]
    internal class CelestroneInteraction41 : CelestroneInteraction31
    {
        public CelestroneInteraction41(IDeviceWorker deviceWorker) : base(deviceWorker)
        {}

        public override void SyncRaDec(Coordinates coordinates)
        {
            if (this.CommandBool(string.Format("s{0},{1}#",
                Utils.Deg2HEX32(coordinates.Ra), Utils.Deg2HEX32(coordinates.Dec)), false))
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
