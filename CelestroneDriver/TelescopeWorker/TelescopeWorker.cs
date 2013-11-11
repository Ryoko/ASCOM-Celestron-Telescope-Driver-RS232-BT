namespace ASCOM.CelestronAdvancedBlueTooth.CelestroneDriver.TelescopeWorker
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Threading;

    using ASCOM;
    using ASCOM.CelestronAdvancedBlueTooth.CelestroneDriver.Utils;
    using ASCOM.DeviceInterface;

    public enum TelescopeMode {Initial, Normal, Slewing, MovingAxis, Guiding}
    public enum TelescopeOperationMode {Natural, Rate}

    public class TelescopeWorker
    {
        private static TelescopeWorker _worker;
        private BackgroundWorker bgWorker = new BackgroundWorker();
        //private TelescopeWorker tw;
        //private Queue<ITelescopeOperator> 
        private TelescopeMode telescopeMode = TelescopeMode.Initial;
        private ITelescopeInteraction ti = null;
        private TelescopeProperties tp;
        public static int CoordinatesGetInterval = 200;
        private int lastGetCoordiante = 0;
        private bool lastGetCoordinateMode = false;
        //private ITelescopeV3 _driver;
        private ITelescopeWorkerOperations two;
        private KalmanFilterSimple1D kfilt;
        private TelescopeSettingsProfile profile;
        private object _sync;

        private TelescopeWorker(TelescopeSettingsProfile Profile /*ITelescopeV3 driver*/)
        {
            //_driver = driver;
            this.profile = Profile;
            this.two = new TelescopeWorkerOperationsRateMode();
            this.setFilter();
            
            this.bgWorker.DoWork += this.BgWorkerOnDoWork;
            this.bgWorker.ProgressChanged += this.BgWorkerOnProgressChanged;
            this.bgWorker.RunWorkerCompleted += this.BgWorkerOnRunWorkerCompleted;
            this.bgWorker.WorkerSupportsCancellation = true;
            this.bgWorker.WorkerReportsProgress = true;

            this.bgWorker.RunWorkerAsync();
        }

        public bool IsConnected
        {
            get
            {
                return this.TelescopeInteraction != null && this.TelescopeProperties != null && this.TelescopeInteraction.isConnected && this.TelescopeProperties.IsReady;
            }
        }

        public static TelescopeWorker GetWorker(TelescopeSettingsProfile Profile)
        {
            if (_worker == null)
            {
                _worker = new TelescopeWorker(Profile);
            }
            return _worker;
        }

        public TelescopeProperties TelescopeProperties
        {
            get { return this.tp; }
            //private set { tp = value; }
        }

        public ITelescopeInteraction TelescopeInteraction
        {
            set
            {
                this.ti = value;
                this.tp = new TelescopeProperties(value, this.profile);
                this.two.SegProperties(this.tp, this.ti);
            }
            get
            {
                return this.ti;
            }
        }

        public TelescopeOperationMode OperationMode
        {
            get
            {
                return this.two is TelescopeWorkerOperationsNaturalMode
                    ? TelescopeOperationMode.Natural
                    : TelescopeOperationMode.Rate;
            }
            set
            {
                this.two = value == TelescopeOperationMode.Natural
                    ? (ITelescopeWorkerOperations)new TelescopeWorkerOperationsNaturalMode()
                    : new TelescopeWorkerOperationsRateMode();
                this.two.SegProperties(this.tp, this.ti);
            }
        }

        public bool IsSlewing
        {
            get
            {
                return this.telescopeMode == TelescopeMode.Slewing || this.telescopeMode == TelescopeMode.MovingAxis;
            }
        }

        public bool Slew(Coordinates coord)
        {
            this.CheckPark();
            if (this.telescopeMode != TelescopeMode.Normal) return false;
            this.tp.IsAtHome = false;
            lock (_sync)
            {
                this.ti.RaDec = coord;
                this.telescopeMode = TelescopeMode.Slewing;
            }
            return true;
        }

        public bool Slew(AltAzm coord)
        {
            this.CheckPark();
            if (this.telescopeMode != TelescopeMode.Normal) return false;
            this.tp.IsAtHome = false;
            lock (_sync)
            {
                this.ti.AltAzm = coord;
                this.telescopeMode = TelescopeMode.Slewing;
            }
            return true;
        }

        public void AbortSlew()
        {
            lock (_sync)
            {
                ti.CancelGoTo();
            }
        }

        public DateTime TelescopeDateTime
        {
            get
            {
                lock (_sync)
                {
                    if (ti.CanWorkGPS) return ti.GpsDateTime;
                    if (ti.CanGetRTC) return ti.RTCDateTime;
                    if (ti.CanWorkDateTime) return ti.TelescopeDateTime;
                    return DateTime.Now;
                }
            }
            set
            {
                lock (_sync)
                {
                    if (ti.CanSetRTC) ti.RTCDateTime = value;
                    if (ti.CanWorkDateTime) ti.TelescopeDateTime = value;
                }
            }
        }

        public void Sync(Coordinates coord)
        {
            lock (_sync)
            {
                ti.SyncRaDec(coord);
            }
        }

        public void Sync(AltAzm coord)
        {
            lock (_sync)
            {
                ti.SyncAltAz(coord);
            }
        }


        //public void SetTracking(TrackingMode mode, DriveRates rate = DriveRates.driveSidereal)
        //{
        //    if(mode < TrackingMode.Off) throw new ValueNotAvailableException("Wrong tracking mode value: " + mode.ToString());
        //    if(!ti.CanSetTrackingRates) throw new NotSupportedException("Setting tracking rate is not supported");
        //    SetTrackingRate(rate, mode);
        //    tp.TrackingRate = rate;
        //}

        public void SetTracking(bool value)
        {
            if (value) this.CheckPark();
            if (!this.ti.CanSetTrackingRates) throw new NotSupportedException("Setting tracking rate is not supported");
            if (this.telescopeMode == TelescopeMode.MovingAxis) return;

            lock (_sync)
            {

                if (this.tp.TrackingMode > TrackingMode.Off) this.tp.DefaultTrackingMode = this.tp.TrackingMode;
                if (value)
                {
                    this.tp.IsAtHome = false;
                    if (this.tp.TrackingMode > TrackingMode.Off) return;
                    //ti.TrackingMode = tp.DefaultTrackingMode;
                    this.SetTrackingRate(this.tp.TrackingRate, this.tp.DefaultTrackingMode);
                    this.tp.TrackingMode = this.tp.DefaultTrackingMode;
                }
                else
                {
                    if (this.tp.TrackingMode < TrackingMode.AltAzm) return;
                    //ti.TrackingMode = TrackingMode.Off;
                    this.SetTrackingRate(this.tp.TrackingRate, TrackingMode.Off);
                    this.tp.TrackingMode = TrackingMode.Off;
                }
            }
        }
        /// <summary>
        /// Get rate on Azm axis in (deg/sec)
        /// </summary>
        /// <param name="rate">Curent DriveRate</param>
        /// <param name="mode">Current TrackingMode</param>
        /// <returns></returns>
        public double GetRateRa(DriveRates rate, TrackingMode mode)
        {
            return this.two.GetRateRa(rate, mode);
        }

        public void SetTrackingRate(DriveRates rate, TrackingMode mode)
        {
            if (mode > TrackingMode.AltAzm) this.tp.IsAtHome = false;
            lock (_sync)
            {
                this.two.SetTrackingRate(rate, mode);
            }
        }

        public void SetTrackingDec()
        {
            lock (_sync)
            {
                this.two.SetTrackingDec();
            }
        }

        private void CheckRateTrackingState()
        {
            lock (_sync)
            {
                this.two.CheckRateTrackingState();
            }
        }

        private PulsState ps = new PulsState();
        public void PulseGuide(GuideDirections dir, int duration)
        {
            this.CheckPark();
            if(this.telescopeMode != TelescopeMode.Normal && this.telescopeMode != TelescopeMode.Guiding) return;
            lock (_sync)
            {
                this.tp.IsAtHome = false;
                this.two.PulseGuide(dir, duration, this.ps);
                this.telescopeMode = TelescopeMode.Guiding;
            }
        }

        public bool IsPulsGuiding()
        {
            return this.telescopeMode == TelescopeMode.Guiding;
        }

        private TrackingMode moveAxisTrackingMode = TrackingMode.Unknown;
        /// <summary>
        /// MoveAxis with fixed or variable rate
        /// </summary>
        /// <param name="axis">SlewAxes</param>
        /// <param name="rate">Rate (deg/sec) or fixed rate * 10</param>
        /// <param name="isFixed"></param>
        public void MoveAxis(SlewAxes axis, double rate, bool isFixed = false)
        {
            this.CheckPark();
            if (this.telescopeMode != TelescopeMode.Normal && this.telescopeMode != TelescopeMode.MovingAxis) return;

            lock (_sync)
            {
                this.tp.IsAtHome = false;
                if (axis == SlewAxes.DecAlt) this.tp.MovingAltAxes = !rate.Equals(0);
                if (axis == SlewAxes.RaAzm) this.tp.MovingAzmAxes = !rate.Equals(0);
                if (!rate.Equals(0))
                {
                    if (axis == SlewAxes.RaAzm)
                    {
                        if (this.telescopeMode != TelescopeMode.MovingAxis) this.moveAxisTrackingMode = this.tp.TrackingMode;
                        this.CheckRateTrackingState();
                        //SetTracking(TrackingMode.Off);
                        //tp.TrackingMode = TrackingMode.Off;
                    }
                    this.two.MoveAxis(axis, rate, isFixed);
                    this.telescopeMode = TelescopeMode.MovingAxis;
                }
                else // rate == 0
                {
                    if (axis == SlewAxes.RaAzm)
                    {
                        this.SetTrackingRate(this.tp.TrackingRate, this.moveAxisTrackingMode);
                        this.tp.TrackingMode = this.moveAxisTrackingMode;
                    }
                    else
                    {
                        this.SetTrackingDec();
                    }
                    if (!this.tp.MovingAltAxes && !this.tp.MovingAzmAxes)
                    {
                        this.telescopeMode = TelescopeMode.Normal;
                    }
                }
            }
        }

        private void WaitGoPosition()
        {
            var tBeginPark = Environment.TickCount;
            bool isAltSlewDone = false, isAzmSlewDone = false;
            while (true)
            {

                isAltSlewDone = isAltSlewDone || this.ti.IsSlewDone(DeviceID.DecAltMotor);
                isAzmSlewDone = isAzmSlewDone || this.ti.IsSlewDone(DeviceID.RaAzmMotor);
                if (isAltSlewDone && isAzmSlewDone)
                {
                    return;
                }
                if (tBeginPark + 60000 < Environment.TickCount)
                {
                    throw new DriverException("Timeout error while parking");
                }
                Thread.Sleep(100);
            }
        }

        public void Park()
        {
            lock (_sync)
            {
                var pos = this.tp.HomePozition;
                this.ti.GoToPosition(pos);
                this.WaitGoPosition();
                this.SetTracking(false);
                this.tp.IsAtPark = true;
            }
        }

        public void GoHome()
        {
            lock (_sync)
            {
                var pos = this.tp.HomePozition;
                this.ti.GoToPosition(pos);
                this.WaitGoPosition();
                this.SetTracking(false);
                this.tp.IsAtHome = true;
            }
        }

        public bool IsAthome
        {
            get
            {
                return this.tp.IsAtHome;
            }
        }

        public void CheckPark()
        {
            if (this.tp.IsAtPark)
            {
                throw new ParkedException("Parked");
            }
        }

        public void StopWorking()
        {
            lock (_sync)
            {
                if (this.tp.MovingAltAxes)
                {
                    this.MoveAxis(SlewAxes.DecAlt, 0);
                }
                if (this.tp.MovingAzmAxes)
                {
                    this.MoveAxis(SlewAxes.RaAzm, 0);
                }
                this.two.StopWorking();
            }
        }

        public void Disconnect()
        {
            this.ti = null;
            this.telescopeMode = TelescopeMode.Initial;
        }

        private void BgWorkerOnRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs runWorkerCompletedEventArgs)
        {
            if (this.bgWorker.CancellationPending)
            {
                return;
            }
            if (runWorkerCompletedEventArgs.Error != null)
            {
                throw new DriverException("Error in processing: " + runWorkerCompletedEventArgs.Error.Message, runWorkerCompletedEventArgs.Error);
            }
            this.bgWorker.RunWorkerAsync();
        }

        private void BgWorkerOnProgressChanged(object sender, ProgressChangedEventArgs progressChangedEventArgs)
        {
            throw new System.NotImplementedException();
        }

        private void BgWorkerOnDoWork(object sender, DoWorkEventArgs doWorkEventArgs)
        {
            while (!this.bgWorker.CancellationPending)
            {
                if (this.ti != null && this.ti.isConnected && !this.tp.IsAtPark)
                {
                    lock (_sync)
                    {

                        switch (this.telescopeMode)
                        {
                            case TelescopeMode.Initial:
                                this.InitialMode();
                                break;
                            case TelescopeMode.Normal:
                                this.NormalMode();
                                break;
                            case TelescopeMode.Slewing:
                                this.SlewingMode();
                                break;
                            case TelescopeMode.MovingAxis:
                                this.MovingAxisMode();
                                break;
                            case TelescopeMode.Guiding:
                                this.GuidingMode();
                                break;

                        }
                    }
                }
                if (this.bgWorker.CancellationPending) break;
                if (this.telescopeMode == TelescopeMode.Guiding)
                {
                    var tm = this.ps.Dec.TimeEnd;
                    if (tm > this.ps.Ra.TimeEnd) tm = this.ps.Ra.TimeEnd;
                    var tw = tm - Environment.TickCount;
                    if (tw > 0)
                        Thread.Sleep(tw > 200 ? 200 : tw);
                    continue;
                }
                Thread.Sleep(200);
            }
        }

        private void GuidingMode()
        {
            if (this.ps.Ra != null && this.ps.Ra.isExpired)
            {
                this.ps.Ra = null;
                this.SetTrackingRate(this.tp.TrackingRate, this.tp.TrackingMode);
            }

            if (this.ps.Dec != null && this.ps.Dec.isExpired)
            {
                this.ps.Dec = null;
                this.SetTrackingDec();
            }

            if (this.ps.Dec == null && this.ps.Ra == null)
            {
                this.telescopeMode = TelescopeMode.Normal;
            }
        }

        private void MovingAxisMode()
        {
            this.CheckCoordinates();
        }

        private void NormalMode()
        {
            this.CheckCoordinates();
        }

        private void InitialMode()
        {
            var isReady = false;
            for (int i = 0; i < 5; i++)
            {
                try
                {
                    this.tp.GetTelescopeProperties();
                    isReady = true;
                    break;
                }
                catch (Exception err)
                {

                }
                Thread.Sleep(1000);    
            }
            if (!isReady) throw new DriverException("Can't get propertyes of telescope");
            this.CheckCoordinates(true);
            this.telescopeMode = this.tp.SlewState == SlewState.Slewing ? TelescopeMode.Slewing : TelescopeMode.Normal;
            this.CheckRateTrackingState();
            this.SetTrackingRate(this.tp.TrackingRate, this.tp.TrackingMode);
        }

        private int slewEndTime = int.MinValue;
        private bool isSlewSetteled = false;
        private void SlewingMode(bool Sync = false)
        {
            if (!this.isSlewSetteled)
            {
                if (!this.ti.IsGoToInProgress)
                {
                    this.slewEndTime = Environment.TickCount;
                    this.isSlewSetteled = true;
                }
            } 
            else if (this.slewEndTime + this.tp.SlewSteeleTime < Environment.TickCount)
            {
                this.isSlewSetteled = false;
                this.telescopeMode = TelescopeMode.Normal;
            }
            this.CheckCoordinates();
        }

        private double dRateRa = 0, dRateDec = 0, sRa = 0;
        private int lastCalcTime = 0, tCalc = 0;

        private void CheckTracking(double dRa, double dDec)
        {
            double rRa, rDec;
            if (this.lastCalcTime > 0)
            {
                var RaRate = this.GetRateRa(this.tp.TrackingRate, this.tp.TrackingMode);
                var dT = Environment.TickCount - this.lastCalcTime;
                var ra = (15 * dRa * 1000) / dT - Const.TRACKRATE_SIDEREAL;
                var dec = (dDec*1000)/dT;

                //var csRa = Math.Pow(ra - RaRate, 2);
                //sRa =  Math.Sqrt((Math.Pow(sRa, 2) * tCalc + csRa) / (tCalc + 1));
                this.sRa = Math.Abs(RaRate + ra);
                if (this.tp.TrackingMode > TrackingMode.AltAzm && Math.Abs(ra) < 0.0003)
                {
                    this.SetTrackingRate(this.tp.TrackingRate, this.tp.TrackingMode);
//                    sRa = 0;
                }

                if (this.sRa > 0.0005)
                {
                    this.tCalc = 0;
                }

                this.dRateRa = this.dRateRa*this.tCalc/(this.tCalc+1) + ra/(this.tCalc + 1);
                this.dRateDec = this.dRateDec*this.tCalc/(this.tCalc +1) + dec/(this.tCalc + 1);
                this.tCalc++;
                Debug.WriteLine(string.Format("dRateRA = {0:f6}, dRa = {1:f6}, errRa = {2:f6}, n={3}", this.dRateRa, ra, this.sRa, this.tCalc));
            }
            this.lastCalcTime = Environment.TickCount;

        }

        private void CheckCoordinates(bool now = false)
        {
            if (!now && this.lastGetCoordiante + CoordinatesGetInterval > Environment.TickCount) return;
            
            if (now || this.lastGetCoordinateMode)
            {
                try
                {
                    var c = this.ti.RaDec;
                    if (this.tp.RaDec != null)
                    {
                        var dRa = c.Ra - this.tp.RaDec.Ra;
                        var dDec = c.Dec - this.tp.RaDec.Dec;
                        //CheckTracking(dRa, dDec);
                    }
                    this.tp.RaDec = c;
                }
                catch (Exception err)
                {
                    this.tp.RaDec = new Coordinates(double.NaN, double.NaN);
                }
            }

            if (now || !this.lastGetCoordinateMode)
            {
                try
                {
                    var val = this.ti.AltAzm;
                    //ProcessAltAzm(val, tp.AltAzm);
                    this.tp.AltAzm = val;
                }
                catch (Exception err)
                {
                    this.tp.AltAzm = new AltAzm(double.NaN, double.NaN);
                }
            }
            if (this.telescopeMode == TelescopeMode.Normal)
            {
                try
                {
                    var pos = this.ti.GetPosition();
                    this.ProcessAltAzm(pos, this.tp.Position, Environment.TickCount);
                    this.tp.Position = pos;
                }
                catch (Exception err)
                {
                    this.tp.Position = new AltAzm(double.NaN, double.NaN);
                }
            }
            this.lastGetCoordinateMode = !this.lastGetCoordinateMode;
            this.lastGetCoordiante = Environment.TickCount;
        }

        private void setFilter()
        {
            this.kfilt = new KalmanFilterSimple1D(0.0001, 0.00003);
        }

        private int lastAltAzm = 0;
        private Statistic AzmValues = new Statistic(3);
        private void ProcessAltAzm(AltAzm newVal, AltAzm oldVal, int time)
        {
            if (this.lastAltAzm > 0)
            {
                var RaRate = this.GetRateRa(this.tp.TrackingRate, this.tp.TrackingMode);
                var dT = time - this.lastAltAzm;
                var azm = /*Const.TRACKRATE_SIDEREAL -*/ (newVal.Azm - oldVal.Azm)*1000/dT;
                var dec = (newVal.Alt - oldVal.Alt)*1000/dT;
                this.AzmValues.Add(azm, Const.TRACKRATE_SIDEREAL - RaRate);
//                var azmf = kfilt.Correct(azm);
                if (azm.Equals(0d)) 
                    this.SetTrackingRate(this.tp.TrackingRate, this.tp.TrackingMode);
                Debug.WriteLine(string.Format("[{0}] azm={1,-12} azmVal={2,9:f6} sco={3,9:f6} sMed={4,9:f6} sSco={5,9:f6} med={6,9:f6}", DateTime.Now,
                    DMS.FromDeg(newVal.Azm).ToString(":"), 
                    azm, this.AzmValues.sco, this.AzmValues.cMed, this.AzmValues.cSco , this.AzmValues.Median));

            }
            else
            {
                //kfilt.SetState(newVal.Azm, 0.0);
            }
            this.lastAltAzm = time;
        }

        private void Synchronize()
        {
            
        }

    }
}
