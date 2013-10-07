using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using ASCOM.CelestronAdvancedBlueTooth.TelescopeWorker;
using ASCOM.CelestronAdvancedBlueTooth.Utils;
using ASCOM.DeviceInterface;

namespace ASCOM.CelestronAdvancedBlueTooth.HandForm
{
    public partial class HandControl : Form, IHandControl
    {
        private ITelescopeV3 _driver;

        public HandControl()
        {
            InitializeComponent();
            SetGuideRates();
        }

        public void ShowForm(bool show)
        {
            if (show)
            {
                this.Show();
                bgw.RunWorkerAsync();
            }
            else
            {
                bgw.CancelAsync();
                this.Hide();
            }
        }

        public void SetForm(Telescope driver)
        {
            _driver = driver;
            //bgw.RunWorkerAsync();
        }

        private bool _connectionState = false;
        private void bgw_DoWork(object sender, DoWorkEventArgs e)
        {
            while (!bgw.CancellationPending)
            {
                Thread.Sleep(100);
                if (bgw.CancellationPending) return;
                if (this._driver != null && this._driver.Connected)
                {
                    if (!_connectionState)
                    {
                        _connectionState = true;
                        bgw.ReportProgress(10);
                    }
                    bgw.ReportProgress(50, _driver);
                }
                else
                {
                    if (_connectionState)
                    {
                        _connectionState = false;
                        bgw.ReportProgress(10);
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
                    Ra.Text = new DMS(_driver.RightAscension, true).ToString();
                    Dec.Text = new DMS(_driver.Declination).ToString();
                    Alt.Text = new DMS(_driver.Altitude).ToString();
                    Azm.Text = new DMS(_driver.Azimuth).ToString();
                    var m = _driver.Action("GetTrackingMode", "");
                    int mode;
                    if (int.TryParse(m, out mode))
                    {
                        Mode.Text = ((TrackingMode)mode).ToString();
                    }
                }catch{}
            }
            if (e.ProgressPercentage == 10)
            {
                try
                {
                    SetUIState();
                }catch{}
            }

        }

        private void bgw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!bgw.CancellationPending)
            {
                bgw.RunWorkerAsync();
            }
        }

        private void SetUIState()
        {
            Coordinates.Enabled = _connectionState;
            ControlButtons.Enabled = _connectionState;
        }

        private void Control_MouseDown(object sender, MouseEventArgs e)
        {
            if (!(sender is Button)) return;
            var b = (Button)sender;
            TelescopeAxes axis;
            var rate = (double)RateBar.Value * 10;
            switch (b.Name)
            {
                case "Ra_p":
                    axis = TelescopeAxes.axisPrimary;
                    rate *= 1d;
                    break;
                case "Ra_n":
                    axis = TelescopeAxes.axisPrimary;
                    rate *= -1d;
                    break;
                case "Dec_p":
                    axis = TelescopeAxes.axisSecondary;
                    rate *= 1d;
                    break;
                case "Dec_n":
                    axis = TelescopeAxes.axisSecondary;
                    rate *= -1d;
                    break;
                default:
                    return;
            }

            if (_driver == null || !_driver.Connected) return;
            _driver.MoveAxis(axis, rate);
        }

        private void Control_MouseUp(object sender, MouseEventArgs e)
        {
            if (ConstMove.Checked) return;
            if (!(sender is Button)) return;
            var b = (Button)sender;
            TelescopeAxes axis;
            var rate = 0d;
            switch (b.Name)
            {
                case "Ra_p":
                case "Ra_n":
                    axis = TelescopeAxes.axisPrimary;
                    break;
                case "Dec_p":
                case "Dec_n":
                    axis = TelescopeAxes.axisSecondary;
                    break;
                default:
                    return;
            }

            if (_driver == null || !_driver.Connected) return;
            _driver.MoveAxis(axis, 0);
        }

        private void Stop_Click(object sender, EventArgs e)
        {
            if (_driver == null || !_driver.Connected) return;
            _driver.MoveAxis(TelescopeAxes.axisPrimary, 0);
            _driver.MoveAxis(TelescopeAxes.axisSecondary, 0);
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

            if (_driver == null || !_driver.Connected || !_driver.CanPulseGuide) return;
            var durat = GuideItvl.Value;
            _driver.PulseGuide(dir, (int)durat);
        }

        private void SetGuideRates()
        {
            if (_driver == null || !_driver.Connected || !_driver.CanSetGuideRates) return;
            _driver.GuideRateDeclination = (double)(GuideRate.Value / 100) * (Const.TRACKRATE_SIDEREAL) / 15;
            _driver.GuideRateRightAscension = (double)(GuideRate.Value / 100) * (Const.TRACKRATE_SIDEREAL);
        }

        private void GideRate_ValueChanged(object sender, EventArgs e)
        {
            SetGuideRates();
        }
    }
}
