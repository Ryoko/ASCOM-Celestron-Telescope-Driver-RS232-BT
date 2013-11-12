namespace ASCOM.CelestronAdvancedBlueTooth.CelestroneDriver.TelescopeWorker
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using ASCOM.Astrometry.Exceptions;
    using ASCOM.CelestronAdvancedBlueTooth.CelestroneDriver.HardwareWorker;
    using ASCOM.CelestronAdvancedBlueTooth.CelestroneDriver.Utils;
    using ASCOM.DeviceInterface;

    //[TelescopeInteraction(     TelescopeModels = new[]
    //{
    //    TelescopeModel.AdvancedGT, 
    //    TelescopeModel.CGE,
    //    TelescopeModel.CPC,
    //    TelescopeModel.GPSSeries,
    //    TelescopeModel.SE45,
    //    TelescopeModel.SE68,
    //    TelescopeModel.iSeries,
    //    TelescopeModel.iSeriesSE,
    //})]
    public abstract class ATelescopeInteraction : ITelescopeInteraction
    {
        //protected IDriverWorker driverWorker { set; get; }
        protected IDeviceWorker DeviceWorker { get; set; }
        protected double _firmwareVersion = -1;
        protected TelescopeModel _telescopeModel = TelescopeModel.Unknown;

        public static ITelescopeInteraction GeTelescopeInteraction(IDeviceWorker deviceWorker)
        {
         
            var res = deviceWorker.Transfer("V");// SendBytes(com);
            if (res.Length < 2) throw new Exception("Wrong answer");
            var low = (double)res[1];
            low = low / (low < 10 ? 10 : low < 100 ? 100 : 1000);
            var ver = res[0] + low;
            //ver = 1.6;

            var types = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.GetCustomAttributes(typeof (TelescopeInteractionAttribute), false).Any());
            Type tver = null;
            var maxVer = double.MinValue;
            foreach (var type in types)
            {
                var at = type.GetCustomAttributes(typeof (TelescopeInteractionAttribute), false);
                var v = (TelescopeInteractionAttribute)at[0];
                if (v.RequiredVersion <= ver && v.RequiredVersion > maxVer)
                {
                    tver = type;
                    maxVer = v.RequiredVersion;
                }
            }
            if (tver != null) return (ATelescopeInteraction)Activator.CreateInstance(tver, deviceWorker);
            return null;

            //ATelescopeInteraction ti = new CelestroneInteraction41(deviceWorker);
            //while (ti.GetType() != typeof (ATelescopeInteraction))
            //{
            //    var tti = ti.GetType();
            //    var att = tti.GetCustomAttributes(typeof(TelescopeInteractionAttribute), true);
               
            //    if (ver >= ti.VersionRequired) return ti;
            //    var t = ti.GetType();
            //    var tp = t.BaseType;
            //    if (tp == null) return null;
            //    ti = (ATelescopeInteraction)Activator.CreateInstance(tp, deviceWorker);
            //}

            //return null;
        }

        public ATelescopeInteraction(IDeviceWorker deviceWorker)
        {
            this.DeviceWorker = deviceWorker;
        }

        protected ATelescopeInteraction(){}

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

        public virtual double FirmwareVersion
        {
            get
            {
                //var com = new[] {(byte) 'V'};
                var res = this.DeviceWorker.Transfer("V");// SendBytes(com);
                if (res.Length < 2) throw new Exception("Wrong answer");
                var low = (double)res[1];
                low = low / (low < 10 ? 10 : low < 100 ? 100 : 1000);
                this._firmwareVersion = res[0] + low;
                return this._firmwareVersion;
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

        public abstract bool IsAlignmentComplete
        {
            get;
        }

        public abstract bool IsGoToInProgress
        {
            get;
        }

        public virtual bool IsSlewDone(DeviceID deviceId)
        {
            throw new System.NotImplementedException();
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

        public virtual void GoToPosition(AltAzm position)
        {
            throw new System.NotImplementedException();
        }

        public virtual AltAzm GetPosition()
        {
            throw new System.NotImplementedException();
        }

        public virtual void SetHome()
        {
            throw new NotImplementedException();
        }

        public virtual void GoHome()
        {
            throw new NotImplementedException();
        }

        public virtual SiteOfPier GetSiteOfPier()
        {
            throw new System.NotImplementedException();
        }

        public virtual SiteOfPier GetDestinationSiteOfPier(Coordinates coord)
        {
            throw new NotImplementedException();
        }

        public virtual void Hibernate()
        {
            throw new System.NotImplementedException();
        }

        public virtual void WakeUp()
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

        public virtual bool CanGetSiteOfPier
        {
            get { return false; }
        }

        public virtual bool CanHibernate
        {
            get { return false; }
        }

        public virtual bool CanWorkHome
        {
            get { return false; }
        }

        public bool CommandBool(string command, bool raw)
        {
            this.DeviceWorker.CheckConnected("CommandBool");
            string ret = this.DeviceWorker.Transfer(command);
            return ret.EndsWith("#");
        }

        public void CommandBlind(string command, bool raw)
        {
            this.DeviceWorker.CheckConnected("CommandBlind");
            // Call CommandString and return as soon as it finishes
            this.DeviceWorker.Transfer(command);
        }

        public string CommandString(string command, bool raw)
        {
            this.DeviceWorker.CheckConnected("CommandString");
            try
            {
                return this.DeviceWorker.Transfer(command);
            }
            catch (Exception err)
            {
                throw new ASCOM.DriverException(err.Message, err);
            }
        }

        protected byte[] SendBytes(byte[] com)
        {
            var res = this.DeviceWorker.Transfer(com);// CommandString(Encoding.ASCII.GetString(com), false);
            if (res.Length == 0 || res[res.Length - 1] != '#')
            {
                throw new Exception("Error in protocol");
            }

            return res; //  res.ToCharArray().Cast<byte>().ToArray();
        }

        protected double[] GetValues(string command, int nOfDigits)
        {
            var res = this.DeviceWorker.Transfer(command);
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
            var res = this.SendBytes(argums);
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
            this.DeviceWorker.Transfer(com);
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
            int d = 0;
            foreach (var v in buf)
            {
                if (v == 35) break;
                res = res * 0x100 + (v & 0xff);
                d++;
            }
            var val = (res*360d)/Math.Pow(2, d*8);
            if (val < 0) val += 360;
            while (val > 360) val -= 360;
            return val;
        }

        protected byte[] SendCommand(GeneralCommands command, params byte[] par)
        {
            var com = new List<byte>(new[]{(byte)command});
            com.AddRange(par);
            return this.SendBytes(com.ToArray());
        }
    }
}
