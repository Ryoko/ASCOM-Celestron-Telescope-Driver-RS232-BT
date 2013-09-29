using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASCOM.CelestronAdvancedBlueTooth.Utils;

namespace ASCOM.CelestronAdvancedBlueTooth.TelescopeWorker
{
    abstract class ATelescopeInteraction : ITelescopeInteraction
    {
        protected IDriverWorker driverWorker;
        protected double _firmwareVersion = -1;
        protected TelescopeModel _telescopeModel = TelescopeModel.Unknown;

        public ATelescopeInteraction(IDriverWorker _driverWorker)
        {
            driverWorker = _driverWorker;
        }

        public bool isConnected { get; set; }
        
        public abstract AltAzm AltAzm
        {
            get; set;
        }

        public abstract Coordinates RaDec
        {
            get; set;
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
        public abstract double FirmwareVersion
        {
            get;
        }

        public virtual double GetDeviceVersion(DeviceID device)
        {
            throw new System.NotImplementedException();
        }

        public virtual TelescopeModel GetModel
        {
            get { throw new System.NotImplementedException(); }
        }

        public abstract bool IsAlignmentComplete
        {
            get;
        }

        public abstract bool IsGoToInProgress
        {
            get;
        }

        public abstract void CancelGoTo();

        public virtual double VersionRequired
        {
            get { return 0; }
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
            var res = driverWorker.CommandString(Encoding.ASCII.GetString(com), false);
            if (res.Length < len || !res[len - 1].Equals('#'))
                throw new Exception("Error in protocol");
            return Encoding.ASCII.GetBytes(res);//res.ToCharArray().Cast<byte>().ToArray();
        }

        protected byte[] SendBytes(byte[] com)
        {
            var res = driverWorker.CommandString(Encoding.ASCII.GetString(com), false);
            if (res.Length == 0 || !res[res.Length - 1].Equals('#'))
                throw new Exception("Error in protocol");
            return Encoding.ASCII.GetBytes(res);//  res.ToCharArray().Cast<byte>().ToArray();
        }
    }
}
