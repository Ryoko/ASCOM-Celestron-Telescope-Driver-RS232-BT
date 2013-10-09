using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASCOM.Astrometry.Exceptions;
using ASCOM.CelestronAdvancedBlueTooth.Utils;
using ASCOM.DeviceInterface;

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

        public virtual void SlewFixedRate(SlewAxes axis, int rate)
        {
            throw new System.NotImplementedException();
        }

        public virtual void SlewVariableRate(SlewAxes axis, double rate)
        {
            throw new System.NotImplementedException();
        }

        public virtual void SlewHighRate(SlewAxes axis, double rate)
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

        public virtual bool SetTrackingRate(DriveRates rate, TrackingMode mode)
        {
            throw new System.NotImplementedException();
        }

        public virtual double VersionRequired
        {
            get { return 0; }
        }

        public virtual void GoToPosition(double azm, double alt)
        {
            throw new System.NotImplementedException();
        }

        public virtual double[] GetPosition()
        {
            throw new System.NotImplementedException();
        }

        public virtual bool CanWorkPosition
        {
            get { return false; }
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

        public virtual bool CanSetTrackingRates
        {
            get { return false; }
        }

        public virtual bool CanSlewHighRate
        {
            get { return false; }
        }

        protected byte[] SendCommand(byte[] com)
        {
            var len = com[com.Length - 1] + 1;
            var res = this.driverWorker.CommandString(Encoding.ASCII.GetString(com), false);
            if (res.Length < len || !res[len - 1].Equals('#'))
            {
                throw new Exception("Error in protocol");
            }

            return Encoding.ASCII.GetBytes(res); //res.ToCharArray().Cast<byte>().ToArray();
        }

        protected byte[] SendBytes(byte[] com)
        {
            var res = this.driverWorker.CommandString(Encoding.ASCII.GetString(com), false);
            if (res.Length == 0 || !res[res.Length - 1].Equals('#'))
            {
                throw new Exception("Error in protocol");
            }

            return Encoding.ASCII.GetBytes(res); //  res.ToCharArray().Cast<byte>().ToArray();
        }

        protected double[] GetValues(string command, int nOfDigits)
        {
            var res = this.driverWorker.CommandString(command, false);
            if (res.Length == 0 || !res[res.Length - 1].Equals('#'))
            {
                throw new Exception("Error in protocol");
            }

            var vals = res.TrimEnd('#').Split(new[] { ',' });
            if (vals.Length != 2)
            {
                throw new Exception("Error in protocol");
            }

            var coeff = 360d / Math.Pow(2, nOfDigits * 4);
            var outs = vals.Select(val => val.Length > nOfDigits ? val.Substring(0, nOfDigits) : val).Select(v => Convert.ToInt32(v, 16) * coeff).ToArray();
            return outs;
        }

        public virtual byte[] SendCommandToDevice(DeviceID DeviceId, DeviceCommands Command, byte NoOfAnsvers, params byte[] args)
        {
            var argLen = (byte)(args.Length + 1);
            if (argLen > 4) argLen = 4;
            var argums = new byte[] {(byte) 'P', 0, (byte) DeviceId, (byte) Command, 0, 0, 0, NoOfAnsvers};
            for (var i = 0; i < 3; i++)
            {
                if (args.Length <= i) continue;
                argums[i + 4] = args[i];
                if (args[i] == 0x3B) argLen = 4;
            }
            argums[1] = argLen;
            var res = SendBytes(argums);
            if (res.Length < NoOfAnsvers + 1) 
                throw new Exception(string.Format("Error in protocol: {0} bytes reply expected, {1} bytes recived", NoOfAnsvers, res.Length - 1));
            return res;
        }

        protected void SetValues(string command, IEnumerable<double> values, int nOfDigits, int nDigitsInParam = 0)
        {
            string com = command;
            foreach (var val in values)
            {
                var iVal = (int)(val*(Math.Pow(2, nOfDigits*4)/360) + 0.5);
                var format = string.Format("{{0:X{0}}}", nOfDigits);
                var v = string.Format(format, iVal);
                if (nDigitsInParam > nOfDigits) v += new string('0', nDigitsInParam - nOfDigits);
                com += com.Length > command.Length ? "," + v : v;
            }
            com += "#";

            driverWorker.CommandBool(com, false);
        }

        protected DeviceID GetDeviceId(SlewAxes axis)
        {
            if (axis == SlewAxes.DecAlt) return DeviceID.DecAltMotor;
            if (axis == SlewAxes.RaAzm) return DeviceID.RaAzmMotor;
            throw new ValueNotAvailableException("Wrong axis value: " + axis.ToString());
        }

        /// <summary>
        /// Fraction of full rotation to 3 bytes
        /// </summary>
        /// <param name="value">0-360</param>
        /// <returns></returns>
        protected byte[] DoubleToBytes(double value)
        {
            if (value < 0) value += 360;

            var iVal = (int)(value * (Math.Pow(2, 24) / 360) + 0.5);
            var h = (byte)((iVal/0x10000) & 0xff);
            var m = (byte)((iVal / 0x100) & 0xff);
            var l = (byte) (iVal & 0xff);
            return (new[] {h, m, l});
        }

        protected double BytesToDouble(byte[] buf)
        {
            int res = 0;
            foreach (var b in buf)
            {
                res = res*0x100 + b;
            }
            var val = 360*res/Math.Pow(2, buf.Length*8);
            return val;
        }
    }
}
