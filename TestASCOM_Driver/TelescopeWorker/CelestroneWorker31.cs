using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASCOM.CelestronAdvancedBlueTooth.Utils;
using ASCOM.Utilities;
using ASCOM.Utilities.Interfaces;

namespace ASCOM.CelestronAdvancedBlueTooth.TelescopeWorker
{
    internal class CelestroneWorker31 : CelestroneWorker23
    {
        public CelestroneWorker31(IDriverWorker _driverWorker) : base(_driverWorker)
        {
        }

        public override DateTime RTCDateTime
        {
            get
            {
                return base.RTCDateTime;
            }
            set
            {

                var com = new byte[]
                {
                    (byte) 'P', 3, 178, 131,
                    (byte)value.Month, (byte)value.Day, 0, 0
                };
                SendCommand(com);
                
                com = new byte[]
                {
                    (byte) 'P', 3, 178, 132,
                    (byte)(value.Year / 256), (byte)(value.Year % 256), 0, 0
                };
                SendCommand(com);

                com = new byte[]
                {
                    (byte) 'P', 4, 178, 179,
                    (byte)value.Hour, (byte)value.Minute, (byte)value.Second, 0
                };
                SendCommand(com);

            }
        }

        public override double VersionRequired
        {
            get { return 3.01; }
        }

        public override bool CanSetRTC
        {
            get { return true; }
        }

    }
}