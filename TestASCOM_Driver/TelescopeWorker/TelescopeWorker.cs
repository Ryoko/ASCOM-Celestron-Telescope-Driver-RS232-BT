using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using ASCOM.CelestronAdvancedBlueTooth.Utils;

namespace ASCOM.CelestronAdvancedBlueTooth.TelescopeWorker
{
    enum TelescopeMode {Initial, Normal, Slewing, MovingAxis, Guiding}
    class TelescopeWorker
    {
        public readonly static TelescopeWorker Worker = new TelescopeWorker();
        private BackgroundWorker bgWorker = new BackgroundWorker();
        //private TelescopeWorker tw;
        //private Queue<ITelescopeOperator> 
        private TelescopeMode telescopeMode = TelescopeMode.Initial;
        private ITelescopeInteraction ti = null;
        private TelescopeProperties tp;
        public static int CoordinatesGetInterval = 200;
        private int lastGetCoordiante = 0;
        private bool lastGetCoordinateMode = false;

        public static TelescopeProperties TelescopeProperties
        {
            get { return Worker.tp; }
            private set { Worker.tp = value; }
        }

        public static ITelescopeInteraction TelescopeInteraction
        {
            set
            {
                Worker.ti = value;
                Worker.tp = new TelescopeProperties(value);
            }
            get
            {
                return Worker.ti;
            }
        }

        private TelescopeWorker()
        {
            //tw = Worker.tw;

            bgWorker.DoWork += BgWorkerOnDoWork;
            bgWorker.ProgressChanged += BgWorkerOnProgressChanged;
            bgWorker.RunWorkerCompleted += BgWorkerOnRunWorkerCompleted;
            bgWorker.WorkerSupportsCancellation = true;
            bgWorker.WorkerReportsProgress = true;
            bgWorker.RunWorkerAsync();
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

        public void SetTracking(bool value)
        {
            if (!ti.CanSetTracking) throw new NotSupportedException("Setting tracking mode is not supported");
            if (value)
            {
                if (tp.TrackingMode > TrackingMode.AltAzm) return;
                tp.TrackingMode = tp.DefaultTrackingMode;
                ti.TrackingMode = tp.TrackingMode;
            }
            else
            {
                var tm = tp.TrackingMode;
                if (tm > TrackingMode.AltAzm) tp.DefaultTrackingMode = tm;
                ti.TrackingMode = TrackingMode.Off;
                tp.TrackingMode = TrackingMode.Off;
            }
        }

        public void MoveAxis(SlewAxes axis, double rate)
        {
            if (telescopeMode == TelescopeMode.Normal || telescopeMode == TelescopeMode.MovingAxis)
            {
                if (axis == SlewAxes.DecAlt) tp.MovingAltAxes = !rate.Equals(0);
                if (axis == SlewAxes.RaAzm) tp.MovingAzmAxes = !rate.Equals(0);

                if (!rate.Equals(0))
                {
                    SetTracking(false);
                    ti.SlewHighRate(axis, rate * 3600d);
                    telescopeMode = TelescopeMode.MovingAxis;
                }
                else
                {
                    ti.SlewHighRate(axis, 0);
                    if (!tp.MovingAltAxes && !tp.MovingAzmAxes)
                    {
                        SetTracking(true);
                        telescopeMode = TelescopeMode.Normal;
                    }
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
            //TODO: do something    
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
                Thread.Sleep(100);
            }
        }

        private void GuidingMode()
        {
            throw new System.NotImplementedException();
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
            tp.GetTelescopeProperties();
            CheckCoordinates(true);
            telescopeMode = tp.SlewState == SlewState.Slewing ? TelescopeMode.Slewing : TelescopeMode.Normal;

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
