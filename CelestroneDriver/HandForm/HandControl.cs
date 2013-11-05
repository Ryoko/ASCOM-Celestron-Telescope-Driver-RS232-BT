//using ASCOM.DeviceInterface;

namespace ASCOM.CelestronAdvancedBlueTooth.CelestroneDriver.HandForm
{
    using System;
    using System.ComponentModel;
    using System.Threading;
    using System.Windows.Forms;

    using ASCOM.CelestronAdvancedBlueTooth.CelestroneDriver.TelescopeWorker;
    using ASCOM.CelestronAdvancedBlueTooth.CelestroneDriver.Utils;
    using ASCOM.DeviceInterface;

    public partial class HandControl : Form, IHandControl
    {
        //private ITelescopeV3 _driver;

        private TelescopeWorker _tw;

        public HandControl()
        {
            this.InitializeComponent();
            this.SetGuideRates();
        }

        public void ShowForm(bool show)
        {
            if (show)
            {
                this.Show();
                this.bgw.RunWorkerAsync();
            }
            else
            {
                this.bgw.CancelAsync();
                this.Hide();
            }
        }

        public void SetForm(TelescopeWorker worker)
        {
            //_driver = driver;
            this._tw = worker;
            //bgw.RunWorkerAsync();
        }

        private bool _connectionState = false;
        private void bgw_DoWork(object sender, DoWorkEventArgs e)
        {
            while (!this.bgw.CancellationPending)
            {
                Thread.Sleep(100);
                if (this.bgw.CancellationPending) return;
                if (this._tw != null && this._tw.TelescopeInteraction != null && this._tw.TelescopeProperties != null && this._tw.TelescopeInteraction.isConnected)
                {
                    if (!this._connectionState)
                    {
                        this._connectionState = true;
                        this.bgw.ReportProgress(10);
                    }
                    this.bgw.ReportProgress(50, this._tw);
                }
                else
                {
                    if (this._connectionState)
                    {
                        this._connectionState = false;
                        this.bgw.ReportProgress(10);
                    }
                }
            }
        }

        private void bgw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage == 50)
            {
                try
                {

                    var tp = this._tw.TelescopeProperties;
                    this.Ra.Text = new DMS(tp.RaDec.Ra, true).ToString();
                    this.Dec.Text = new DMS(tp.RaDec.Dec).ToString();
                    this.Alt.Text = new DMS(tp.AltAzm.Alt).ToString();
                    this.Azm.Text = new DMS(tp.AltAzm.Azm).ToString();
                    var mode = tp.TrackingMode;
                    this.Mode.Text = mode.ToString();
                }catch{}
            }
            if (e.ProgressPercentage == 10)
            {
                try
                {
                    this.SetUIState();
                }catch{}
            }

        }

        private void bgw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!this.bgw.CancellationPending)
            {
                this.bgw.RunWorkerAsync();
            }
        }

        private void SetUIState()
        {
            this.Coordinates.Enabled = this._connectionState;
            this.ControlButtons.Enabled = this._connectionState;
        }

        private void Control_MouseDown(object sender, MouseEventArgs e)
        {
            if (!(sender is Button)) return;
            var b = (Button)sender;
            SlewAxes axis;
            var rate = (double)this.RateBar.Value * 10;
            switch (b.Name)
            {
                case "Ra_p":
                    axis = SlewAxes.RaAzm;
                    rate *= 1d;
                    break;
                case "Ra_n":
                    axis = SlewAxes.RaAzm;
                    rate *= -1d;
                    break;
                case "Dec_p":
                    axis = SlewAxes.DecAlt;
                    rate *= 1d;
                    break;
                case "Dec_n":
                    axis = SlewAxes.DecAlt;
                    rate *= -1d;
                    break;
                default:
                    return;
            }

            if (this._tw == null || !this._tw.IsConnected) return;
            this._tw.MoveAxis(axis, rate);
        }

        private void Control_MouseUp(object sender, MouseEventArgs e)
        {
            if (this.ConstMove.Checked) return;
            if (!(sender is Button)) return;
            var b = (Button)sender;
            SlewAxes axis;
            var rate = 0d;
            switch (b.Name)
            {
                case "Ra_p":
                case "Ra_n":
                    axis = SlewAxes.RaAzm;
                    break;
                case "Dec_p":
                case "Dec_n":
                    axis = SlewAxes.DecAlt;
                    break;
                default:
                    return;
            }

            if (this._tw == null || !this._tw.IsConnected) return;
            this._tw.MoveAxis(axis, 0);
        }

        private void Stop_Click(object sender, EventArgs e)
        {
            if (this._tw == null || !this._tw.IsConnected) return;
            this._tw.MoveAxis(SlewAxes.RaAzm, 0);
            this._tw.MoveAxis(SlewAxes.DecAlt, 0);
        }

        private void GuideBtn_Click(object sender, EventArgs e)
        {
            if (!(sender is Button)) return;
            var btn = (Button) sender;
            GuideDirections dir;
            switch (btn.Name)
            {
                case "GuideN":
                    dir = GuideDirections.guideNorth;
                    break;
                case "GuideS":
                    dir = GuideDirections.guideSouth;
                    break;
                case "GuideE":
                    dir = GuideDirections.guideEast;
                    break;
                case "GuideW":
                    dir = GuideDirections.guideWest;
                    break;
                default:
                    return;
            }

            if (this._tw == null || !this._tw.IsConnected || !this._tw.TelescopeInteraction.CanSlewHighRate) return;
            var durat = this.GuideItvl.Value;
            this._tw.PulseGuide(dir, (int)durat);
        }

        private void SetGuideRates()
        {
            if (this._tw == null || !this._tw.IsConnected || !this._tw.TelescopeInteraction.CanSlewHighRate) return;
            this._tw.TelescopeProperties.PulseRateAlt = (double)(this.GuideRate.Value / 100) * (Const.TRACKRATE_SIDEREAL) / 15;
            this._tw.TelescopeProperties.PulseRateAzm = (double)(this.GuideRate.Value / 100) * (Const.TRACKRATE_SIDEREAL);
        }

        private void GideRate_ValueChanged(object sender, EventArgs e)
        {
            this.SetGuideRates();
        }
    }
}
