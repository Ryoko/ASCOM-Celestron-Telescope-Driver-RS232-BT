using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        private TelescopeWorker(Telescope driver)
        {
            _driver = driver;
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
            private set { tp = value; }
        }

        public ITelescopeInteraction TelescopeInteraction
        {
            set
            {
                ti = value;
                tp = new TelescopeProperties(value);
            }
            get
            {
                return ti;
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
            if (telescopeMode != TelescopeMode.Normal) return false;
            ti.RaDec = coord;
            telescopeMode = TelescopeMode.Slewing;
            return true;
        }

        public bool Slew(AltAzm coord)
        {
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
            if (mode <= TrackingMode.AltAzm)
            {
                return 0;
            }

            double Rate;
            switch (rate)
            {
                case DriveRates.driveSidereal:
                    Rate = Const.TRACKRATE_SIDEREAL;
                    break;
                case DriveRates.driveSolar:
                    Rate = Const.TRACKRATE_SOLAR;
                    break;
                case DriveRates.driveLunar:
                    Rate = Const.TRACKRATE_LUNAR;
                    break;
                default:
                    throw new ValueNotAvailableException("Wring tracking rate value");
            }

            Rate += tp.RightAscensionRateOffset * Const.SiderealRate;
            if (mode == TrackingMode.EQS) Rate = -Rate;
            return Rate;
        }

        public void SetTrackingRate(DriveRates rate, TrackingMode mode)
        {
            double Rate = GetRateRa(rate, mode);
            CheckRateTrackingState();
            ti.SlewHighRate(SlewAxes.RaAzm, Rate);
            tp.MovingAzmAxes = false;
        }

        public void SetTrackingDec()
        {
            if (tp.TrackingMode <= TrackingMode.AltAzm)
            {
                ti.SlewHighRate(SlewAxes.DecAlt, 0);
            }
            else
            {
//                if (tp.TrackingMode == TrackingMode.EQS) Rate = -Rate;
                var Rate = tp.DeclinationRateOffset;
                ti.SlewHighRate(SlewAxes.DecAlt, Rate);
            }
            tp.MovingAltAxes = false;
        
        }

        private void CheckRateTrackingState()
        {
            if (tp.IsRateTracked || ti == null || !ti.CanSlewHighRate || !ti.CanSetTracking) return;
            ti.TrackingMode = TrackingMode.Off;
            tp.IsRateTracked = true;
        }

        private PulsState ps = new PulsState();
        public void PulseGuide(GuideDirections dir, int duration)
        {
            if(telescopeMode != TelescopeMode.Normal && telescopeMode != TelescopeMode.Guiding) return;
            if (!ti.CanSlewHighRate) throw new NotSupportedException("Puls guiding is not supported");
            CheckRateTrackingState();
            double rate;
            switch (dir)
            {
                case GuideDirections.guideNorth:
                    ps.Dec = new Puls(dir, Environment.TickCount, duration);
                    rate = tp.DeclinationRateOffset + tp.PulseRateAlt;
                    ti.SlewHighRate(SlewAxes.DecAlt, rate);
                    break;
                case GuideDirections.guideSouth:
                    ps.Dec = new Puls(dir, Environment.TickCount, duration);
                    rate = tp.DeclinationRateOffset - tp.PulseRateAlt;
                    ti.SlewHighRate(SlewAxes.DecAlt, rate);
                    break;
                case GuideDirections.guideEast:
                    ps.Ra = new Puls(dir, Environment.TickCount, duration);
                    rate = GetRateRa(tp.TrackingRate, tp.TrackingMode) + tp.PulseRateAzm;
                    ti.SlewHighRate(SlewAxes.RaAzm, rate);
                    break;
                case GuideDirections.guideWest:
                    ps.Ra = new Puls(dir, Environment.TickCount, duration);
                    rate = GetRateRa(tp.TrackingRate, tp.TrackingMode) - tp.PulseRateAzm;
                    ti.SlewHighRate(SlewAxes.RaAzm, rate);
                    break;
            }
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
                    if (!isFixed)
                    {
                        ti.SlewHighRate(axis, rate);
                    }
                    else
                    {
                        ti.SlewFixedRate(axis, (int)rate);
                    }
                    telescopeMode = TelescopeMode.MovingAxis;
                }
                else // rate == 0
                {
                    //ti.SlewHighRate(axis, 0);
                    if (axis == SlewAxes.RaAzm)
                    {
                        //if (tp.TrackingMode > TrackingMode.Off) moveAxisTrackingMode = tp.TrackingMode;
                        //ti.SetTrackingRate(tp.TrackingRate, moveAxisTrackingMode);
                        SetTrackingRate(tp.TrackingRate, moveAxisTrackingMode);
                        //ti.TrackingMode = moveAxisTrackingMode;
                        tp.TrackingMode = moveAxisTrackingMode;
                    }
                    else
                    {
                        SetTrackingDec();
                    }
                    if (!tp.MovingAltAxes && !tp.MovingAzmAxes)
                    {
                        //SetTracking(moveAxisTrackingMode);
                        telescopeMode = TelescopeMode.Normal;
                    }
                }
                
            }
        }

        public void GoHome()
        {
           // ti.SendCommandToDevice()
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

            if (ti != null && tp != null && tp.IsRateTracked)
            {
                if (ti.CanSlewHighRate)
                    ti.SlewHighRate(SlewAxes.DecAlt, 0);
                if (ti.CanSetTracking && ti.CanSlewHighRate)
                {
                    ti.SlewHighRate(SlewAxes.RaAzm, 0);
                    ti.TrackingMode = tp.TrackingMode;
                    tp.IsRateTracked = false;
                }
            }
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
                if (ti != null && ti.isConnected)
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
                var rate = GetRateRa(tp.TrackingRate, tp.TrackingMode);
                ti.SlewHighRate(SlewAxes.RaAzm, rate);
            }

            if (ps.Dec != null && ps.Dec.isExpired)
            {
                ps.Dec = null;
                var rate = tp.DeclinationRateOffset;
                ti.SlewHighRate(SlewAxes.DecAlt, rate);
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
                CheckCoordinates(true);
            }
            if (slewEndTime + tp.SlewSteeleTime < Environment.TickCount)
            {
                isSlewSetteled = false;
                telescopeMode = TelescopeMode.Normal;
            }
        }

        private void CheckCoordinates(bool now = false)
        {
            if (!now && lastGetCoordiante + CoordinatesGetInterval > Environment.TickCount) return;
            
            if (now || lastGetCoordinateMode)
            {
                try
                {
                    tp.RaDec = ti.RaDec;
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
                    tp.AltAzm = ti.AltAzm;
                }
                catch (Exception err)
                {
                    tp.AltAzm = new AltAzm(double.NaN, double.NaN);
                }
            }
            
            lastGetCoordinateMode = !lastGetCoordinateMode;
            lastGetCoordiante = Environment.TickCount;
        }

    }
}
