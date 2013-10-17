using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using ASCOM.Astrometry.Exceptions;
using ASCOM.CelestronAdvancedBlueTooth.Utils;
using ASCOM.DeviceInterface;

namespace ASCOM.CelestronAdvancedBlueTooth.TelescopeWorker
{
    enum TelescopeMode {Initial, Normal, Slewing, MovingAxis, Guiding}
    enum TelescopeOperationMode {Natural, Rate}

    class TelescopeWorker
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
        private Telescope _driver;
        private ITelescopeWorkerOperations two;
        private KalmanFilterSimple1D kfilt;

        private TelescopeWorker(Telescope driver)
        {
            _driver = driver;
            two = new TelescopeWorkerOperationsRateMode();
            setFilter();
            
            bgWorker.DoWork += BgWorkerOnDoWork;
            bgWorker.ProgressChanged += BgWorkerOnProgressChanged;
            bgWorker.RunWorkerCompleted += BgWorkerOnRunWorkerCompleted;
            bgWorker.WorkerSupportsCancellation = true;
            bgWorker.WorkerReportsProgress = true;

            bgWorker.RunWorkerAsync();
        }


        public static TelescopeWorker GetWorker(Telescope driver)
        {
            if (_worker == null)
            {
                _worker = new TelescopeWorker(driver);
            }
            return _worker;
        }

        public TelescopeProperties TelescopeProperties
        {
            get { return tp; }
            //private set { tp = value; }
        }

        public ITelescopeInteraction TelescopeInteraction
        {
            set
            {
                ti = value;
                tp = new TelescopeProperties(value);
                two.SegProperties(tp, ti);
            }
            get
            {
                return ti;
            }
        }

        public TelescopeOperationMode OperationMode
        {
            get
            {
                return two is TelescopeWorkerOperationsNaturalMode
                    ? TelescopeOperationMode.Natural
                    : TelescopeOperationMode.Rate;
            }
            set
            {
                two = value == TelescopeOperationMode.Natural
                    ? (ITelescopeWorkerOperations)new TelescopeWorkerOperationsNaturalMode()
                    : new TelescopeWorkerOperationsRateMode();
                two.SegProperties(tp, ti);
            }
        }

        public bool IsSlewing
        {
            get
            {
                return telescopeMode == TelescopeMode.Slewing || telescopeMode == TelescopeMode.MovingAxis;
            }
        }

        public bool Slew(Coordinates coord)
        {
            CheckPark();
            if (telescopeMode != TelescopeMode.Normal) return false;
            ti.RaDec = coord;
            telescopeMode = TelescopeMode.Slewing;
            return true;
        }

        public bool Slew(AltAzm coord)
        {
            CheckPark();
            if (telescopeMode != TelescopeMode.Normal) return false;
            ti.AltAzm = coord;
            telescopeMode = TelescopeMode.Slewing;
            return true;
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
            if (value) CheckPark();
            if (!ti.CanSetTrackingRates) throw new NotSupportedException("Setting tracking rate is not supported");
            if (telescopeMode == TelescopeMode.MovingAxis) return;

            if (tp.TrackingMode > TrackingMode.Off) tp.DefaultTrackingMode = tp.TrackingMode;
            if (value)
            {
                if (tp.TrackingMode > TrackingMode.Off) return;
                //ti.TrackingMode = tp.DefaultTrackingMode;
                SetTrackingRate(tp.TrackingRate, tp.DefaultTrackingMode);
                tp.TrackingMode = tp.DefaultTrackingMode;
            }
            else
            {
                if (tp.TrackingMode < TrackingMode.AltAzm) return;
                //ti.TrackingMode = TrackingMode.Off;
                SetTrackingRate(tp.TrackingRate, TrackingMode.Off);
                tp.TrackingMode = TrackingMode.Off;
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
            return two.GetRateRa(rate, mode);
        }

        public void SetTrackingRate(DriveRates rate, TrackingMode mode)
        {
            two.SetTrackingRate(rate, mode);
        }

        public void SetTrackingDec()
        {
            two.SetTrackingDec();
        }

        private void CheckRateTrackingState()
        {
            two.CheckRateTrackingState();
        }

        private PulsState ps = new PulsState();
        public void PulseGuide(GuideDirections dir, int duration)
        {
            CheckPark();
            if(telescopeMode != TelescopeMode.Normal && telescopeMode != TelescopeMode.Guiding) return;
            two.PulseGuide(dir, duration, ps);
            telescopeMode = TelescopeMode.Guiding;
        }

        public bool IsPulsGuiding()
        {
            return telescopeMode == TelescopeMode.Guiding;
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
            CheckPark();
            if (telescopeMode == TelescopeMode.Normal || telescopeMode == TelescopeMode.MovingAxis)
            {
                if (axis == SlewAxes.DecAlt) tp.MovingAltAxes = !rate.Equals(0);
                if (axis == SlewAxes.RaAzm) tp.MovingAzmAxes = !rate.Equals(0);
                if (!rate.Equals(0))
                {
                    if (axis == SlewAxes.RaAzm)
                    {
                        if (telescopeMode != TelescopeMode.MovingAxis) moveAxisTrackingMode = tp.TrackingMode;
                        CheckRateTrackingState();
                        //SetTracking(TrackingMode.Off);
                        //tp.TrackingMode = TrackingMode.Off;
                    }
                    two.MoveAxis(axis, rate, isFixed);
                    telescopeMode = TelescopeMode.MovingAxis;
                }
                else // rate == 0
                {
                    if (axis == SlewAxes.RaAzm)
                    {
                        SetTrackingRate(tp.TrackingRate, moveAxisTrackingMode);
                        tp.TrackingMode = moveAxisTrackingMode;
                    }
                    else
                    {
                        SetTrackingDec();
                    }
                    if (!tp.MovingAltAxes && !tp.MovingAzmAxes)
                    {
                        telescopeMode = TelescopeMode.Normal;
                    }
                }
                
            }
        }

        public void GoHome()
        {
           // ti.SendCommandToDevice()
        }

        public bool IsAthome
        {
            get
            {
                var p = ti.GetPosition();
                var dAlt = Math.Abs(p.Alt - tp.HomePozition.Alt);
                var dAzm = Math.Abs(p.Azm - tp.HomePozition.Azm);
                return (dAlt < 0.001 && dAzm < 0.001);
            }
        }

        public void CheckPark()
        {
            if (tp.IsAtPark)
            {
                throw new ParkedException("Parked");
            }
        }

        public void StopWorking()
        {
            if (tp.MovingAltAxes)
            {
                MoveAxis(SlewAxes.DecAlt, 0);
            }
            if (tp.MovingAzmAxes)
            {
                MoveAxis(SlewAxes.RaAzm, 0);
            }
            two.StopWorking();
        }

        public void Disconnect()
        {
            ti = null;
            telescopeMode = TelescopeMode.Initial;
        }

        private void BgWorkerOnRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs runWorkerCompletedEventArgs)
        {
            if (bgWorker.CancellationPending)
            {
                return;
            }
            if (runWorkerCompletedEventArgs.Error != null)
            {
                throw new DriverException("Error in processing: " + runWorkerCompletedEventArgs.Error.Message, runWorkerCompletedEventArgs.Error);
            }
            bgWorker.RunWorkerAsync();
        }

        private void BgWorkerOnProgressChanged(object sender, ProgressChangedEventArgs progressChangedEventArgs)
        {
            throw new System.NotImplementedException();
        }

        private void BgWorkerOnDoWork(object sender, DoWorkEventArgs doWorkEventArgs)
        {
            while (!bgWorker.CancellationPending)
            {
                if (ti != null && ti.isConnected && !tp.IsAtPark)
                {
                    switch (telescopeMode)
                    {
                        case TelescopeMode.Initial:
                            InitialMode();
                            break;
                        case TelescopeMode.Normal:
                            NormalMode();
                            break;
                        case TelescopeMode.Slewing:
                            SlewingMode();
                            break;
                        case TelescopeMode.MovingAxis:
                            MovingAxisMode();
                            break;
                        case TelescopeMode.Guiding:
                            GuidingMode();
                            break;

                    }
                }
                if (bgWorker.CancellationPending) break;
                if (telescopeMode == TelescopeMode.Guiding)
                {
                    Thread.Sleep(20);
                    continue;
                }
                Thread.Sleep(100);
            }
        }

        private void GuidingMode()
        {
            if (ps.Ra != null && ps.Ra.isExpired)
            {
                ps.Ra = null;
                SetTrackingRate(tp.TrackingRate, tp.TrackingMode);
            }

            if (ps.Dec != null && ps.Dec.isExpired)
            {
                ps.Dec = null;
                SetTrackingDec();
            }

            if (ps.Dec == null && ps.Ra == null)
            {
                telescopeMode = TelescopeMode.Normal;
            }
        }

        private void MovingAxisMode()
        {
            CheckCoordinates();
        }

        private void NormalMode()
        {
            CheckCoordinates();
        }

        private void InitialMode()
        {
            var isReady = false;
            for (int i = 0; i < 5; i++)
            {
                try
                {
                    tp.GetTelescopeProperties();
                    isReady = true;
                    break;
                }
                catch (Exception err)
                {

                }
                Thread.Sleep(1000);    
            }
            if (!isReady) throw new DriverException("Can't get propertyes of telescope");
            CheckCoordinates(true);
            telescopeMode = tp.SlewState == SlewState.Slewing ? TelescopeMode.Slewing : TelescopeMode.Normal;
            CheckRateTrackingState();
            SetTrackingRate(tp.TrackingRate, tp.TrackingMode);
        }

        private int slewEndTime = int.MinValue;
        private bool isSlewSetteled = false;
        private void SlewingMode(bool Sync = false)
        {
            if (!isSlewSetteled)
            {
                if (!ti.IsGoToInProgress)
                {
                    slewEndTime = Environment.TickCount;
                    isSlewSetteled = true;
                }
                CheckCoordinates();
            }
            if (slewEndTime + tp.SlewSteeleTime < Environment.TickCount)
            {
                isSlewSetteled = false;
                telescopeMode = TelescopeMode.Normal;
            }
        }

        private double dRateRa = 0, dRateDec = 0, sRa = 0;
        private int lastCalcTime = 0, tCalc = 0;

        private void CheckTracking(double dRa, double dDec)
        {
            double rRa, rDec;
            if (lastCalcTime > 0)
            {
                var RaRate = GetRateRa(tp.TrackingRate, tp.TrackingMode);
                var dT = Environment.TickCount - lastCalcTime;
                var ra = (15 * dRa * 1000) / dT - Const.TRACKRATE_SIDEREAL;
                var dec = (dDec*1000)/dT;

                //var csRa = Math.Pow(ra - RaRate, 2);
                //sRa =  Math.Sqrt((Math.Pow(sRa, 2) * tCalc + csRa) / (tCalc + 1));
                sRa = Math.Abs(RaRate + ra);
                if (tp.TrackingMode > TrackingMode.AltAzm && Math.Abs(ra) < 0.0003)
                {
                    SetTrackingRate(tp.TrackingRate, tp.TrackingMode);
//                    sRa = 0;
                }

                if (sRa > 0.0005)
                {
                    tCalc = 0;
                }

                dRateRa = dRateRa*tCalc/(tCalc+1) + ra/(tCalc + 1);
                dRateDec = dRateDec*tCalc/(tCalc +1) + dec/(tCalc + 1);
                tCalc++;
                Debug.WriteLine(string.Format("dRateRA = {0:f6}, dRa = {1:f6}, errRa = {2:f6}, n={3}", dRateRa, ra, sRa, tCalc));
            }
            lastCalcTime = Environment.TickCount;

        }

        private void CheckCoordinates(bool now = false)
        {
            if (!now && lastGetCoordiante + CoordinatesGetInterval > Environment.TickCount) return;
            
            if (now || lastGetCoordinateMode)
            {
                try
                {
                    var c = ti.RaDec;
                    if (tp.RaDec != null)
                    {
                        var dRa = c.Ra - tp.RaDec.Ra;
                        var dDec = c.Dec - tp.RaDec.Dec;
                        //CheckTracking(dRa, dDec);
                    }
                    tp.RaDec = c;
                }
                catch (Exception err)
                {
                    tp.RaDec = new Coordinates(double.NaN, double.NaN);
                }
            }

            if (now || !lastGetCoordinateMode)
            {
                try
                {
                    var val = ti.AltAzm;
                    //ProcessAltAzm(val, tp.AltAzm);
                    tp.AltAzm = val;
                }
                catch (Exception err)
                {
                    tp.AltAzm = new AltAzm(double.NaN, double.NaN);
                }
            }
            if (telescopeMode == TelescopeMode.Normal)
            {
                try
                {
                    var pos = ti.GetPosition();
                    ProcessAltAzm(pos, tp.Position);
                    tp.Position = pos;
                }
                catch (Exception err)
                {
                    tp.Position = new AltAzm(double.NaN, double.NaN);
                }
            }
            lastGetCoordinateMode = !lastGetCoordinateMode;
            lastGetCoordiante = Environment.TickCount;
        }

        private void setFilter()
        {
            kfilt = new KalmanFilterSimple1D(0.0001, 0.00003);
        }

        private int lastAltAzm = 0;
        private void ProcessAltAzm(AltAzm newVal, AltAzm oldVal)
        {
            if (lastAltAzm > 0)
            {
                var RaRate = GetRateRa(tp.TrackingRate, tp.TrackingMode);
                var dT = Environment.TickCount - lastAltAzm;
                var azm = Const.TRACKRATE_SIDEREAL - (newVal.Azm - oldVal.Azm)*1000/dT;
                var dec = (newVal.Alt - oldVal.Alt)*1000/dT;

                var azmf = kfilt.Correct(azm);

                Debug.WriteLine(string.Format("[{4}] azm = {0,14}, AzmV = {1:f8}, AzmVcorr = {2:f8}, Covar = {3:f8}", 
                    DMS.FromDeg(newVal.Azm).ToString(":"), 
                    azm, azmf, kfilt.Covariance, DateTime.Now));

            }
            else
            {
                kfilt.SetState(newVal.Azm, 0.0);
            }
            lastAltAzm = Environment.TickCount;
        }

    }
}
