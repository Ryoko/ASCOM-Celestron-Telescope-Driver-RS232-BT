using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASCOM.CelestronAdvancedBlueTooth.Utils;
using ASCOM.Utilities;
using ASCOM.Utilities.Interfaces;

namespace ASCOM.CelestronAdvancedBlueTooth.TelescopeWorker
{
    class CelestroneInteraction12 : ITelescopeInteraction
    {
        protected IDriverWorker driverWorker;
        public CelestroneInteraction12(IDriverWorker _driverWorker)
        {
            driverWorker = _driverWorker;
        }

        public virtual AltAzm AltAzm
        {
            get
            {
                int Alt, Azm;
                if (driverWorker.GetPairValues("Z", out Alt, out Azm))
                {
                    return new AltAzm((Alt / 65536) * 360, (Azm / 65536) *360);
                }
                throw new Exception("Error getting parameters");
            }
            set
            {
                if (driverWorker.CommandBool(string.Format("B{0},{1}#",
                    Utils.Utils.Deg2HEX16(value.Azm), Utils.Utils.Deg2HEX16(value.Alt)), false))
                {
                    return;
                }
                throw new Exception("Error setting parameters");
            }
        }

        public virtual Coordinates RaDec
        {
            get
            {
                int Ra, Dec;
                if (driverWorker.GetPairValues("E", out Ra, out Dec))
                {
                    return new Coordinates((Ra / 65536) * 360, (Dec / 65536) * 360);
                }
                throw new Exception("Error getting parameters");
            }
            set
            {
                if (driverWorker.CommandBool(string.Format("R{0},{1}#",
                    Utils.Utils.Deg2HEX16(value.Ra), Utils.Utils.Deg2HEX16(value.Dec)), false))
                {
                    return;
                }
                throw new Exception("Error setting parameters");
            }
        }

        public virtual void SyncAltAz(AltAzm coordinates)
        {
            throw new System.NotImplementedException();
        }

        public virtual void SyncRaDec(Coordinates coordinates)
        {
            throw new System.NotImplementedException();
        }

        public virtual TrackingMode TrackingMode
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public virtual void SlewFixedRate(Direction dir, SlewAxes axis, int rate)
        {
            throw new System.NotImplementedException();
        }

        public virtual void SlewVariableRate(Direction dir, SlewAxes axis, double rate)
        {
            throw new System.NotImplementedException();
        }

        public virtual DateTime TelescopeDateTime
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public virtual LatLon TelescopeLocation
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public virtual bool IsGPS
        {
            get { throw new System.NotImplementedException(); }
        }

        public virtual DateTime GpsDateTime
        {
            get { throw new System.NotImplementedException(); }
        }

        public virtual LatLon GPSLocation
        {
            get { throw new System.NotImplementedException(); }
        }

        public virtual DateTime RTCDateTime
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }
        public double FirmwareVersion
        {
            get
            {
                var com = new[] { (byte)'V' };
                var res = SendCommand(com);
                return res[0] + (double) res[1]/10;
            }
        }

        public virtual double GetDeviceVersion(DeviceID device)
        {
            throw new System.NotImplementedException();
        }

        public virtual TelescopeModel GetModel
        {
            get { throw new System.NotImplementedException(); }
        }

        public bool IsAlignmentComplete
        {
            get
            {
                var com = new[] {(byte) 'J'};
                var res = SendCommand(com);
                return res[0] == 1;
            }
        }

        public bool IsGoToInProgress
        {
            get
            {
                var com = new[] { (byte)'L' };
                var res = SendCommand(com);
                return res[0] == (byte)'1';
            }
        }

        public void CancelGoTo()
        {
            var com = new[] { (byte)'M' };
            SendCommand(com);
        }

        public virtual double VersionRequired
        {
            get { return 1.2; }
        }

        public virtual bool CanSyncAltAzm
        {
            get { return false; }
        }

        public virtual bool CanSyncRaDec
        {
            get { return false; }
        }

        public virtual bool CanSetTracking
        {
            get { return false; }
        }

        public virtual bool CanGetTracking
        {
            get { return false; }
        }

        public virtual bool CanSlewFixedRate
        {
            get { return false; }
        }

        public virtual bool CanSlewVariableRate
        {
            get { return false; }
        }

        public virtual bool CanWorkDateTime
        {
            get { return false; }
        }

        public virtual bool CanWorkLocation
        {
            get { return false; }
        }

        public virtual bool CanWorkGPS
        {
            get { return false; }
        }

        public virtual bool CanGetRTC
        {
            get { return false; }
        }

        public virtual bool CanSetRTC
        {
            get { return false; }
        }

        public virtual bool CanGetModel
        {
            get { return false; }
        }

        public virtual bool CanGetDeviceVersion
        {
            get { return false; }
        }

        protected byte[] SendCommand(byte[] com)
        {
            var len = com[com.Length - 1] + 1;
            var res = driverWorker.CommandString(com.ToString(), false);
            if (res.Length < len || !res[len - 1].Equals('#'))
                throw new Exception("Error in protocol");
            return res.ToCharArray().Cast<byte>().ToArray();
        }
    }
}
