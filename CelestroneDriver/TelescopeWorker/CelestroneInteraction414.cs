namespace ASCOM.CelestronAdvancedBlueTooth.CelestroneDriver.TelescopeWorker
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using ASCOM.CelestronAdvancedBlueTooth.CelestroneDriver.HardwareWorker;
    using ASCOM.CelestronAdvancedBlueTooth.CelestroneDriver.Utils;

    [TelescopeInteraction(4.14)]
    internal class CelestroneInteraction414 : CelestroneInteraction41
    {
        public CelestroneInteraction414(IDeviceWorker deviceWorker) : base(deviceWorker)
        {}

        public override SiteOfPier GetSiteOfPier()
        {
            var res = this.SendCommand(GeneralCommands.GET_PIER_SIDE);
            if (res.Length < 2)
                throw new Exception("Error setting parameters");
            return this.DecodePierSite(res[0]);
        }

        public override SiteOfPier GetDestinationSiteOfPier(Coordinates coord)
        {
            var par = new List<byte>();
            par.AddRange(this.DoubleToBytes(coord.Ra));
            par.Add((byte)GeneralCommands.COMMA);
            par.AddRange(this.DoubleToBytes(coord.Dec));

            var res = this.SendCommand(GeneralCommands.GET_DEST_PIER_SIDE, par.ToArray());
            if (res.Length < 2)
                throw new Exception("Error setting parameters");
            return this.DecodePierSite(res[0]);
        }

        public override bool CanGetSiteOfPier
        {
            get
            {
                return true;
            }
        }

        private SiteOfPier DecodePierSite(byte site)
        {
            switch (site)
            {
                case (byte)GeneralCommands._E:
                    return SiteOfPier.East;
                case (byte)GeneralCommands._W:
                    return SiteOfPier.West;
                default:
                    return SiteOfPier.Unknown;
            }
        }

        public override double VersionRequired
        {
            get { return 4.14; }
        }
    }
}
