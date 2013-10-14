using System;
using System.Threading;
using System.Windows.Forms;
using ASCOM.CelestronAdvancedBlueTooth.TelescopeWorker;
using ASCOM.CelestronAdvancedBlueTooth.Utils;
using ASCOM.DeviceInterface;

namespace ASCOM.CelestronAdvancedBluetooth
{
    public partial class Form1 : Form
    {

        private ASCOM.DriverAccess.Telescope driver;

        public Form1()
        {
            InitializeComponent();
            SetUIState();
            this.test();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (IsConnected)
                driver.Connected = false;

            Properties.Settings.Default.Save();
        }

        private void buttonChoose_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.DriverId = ASCOM.DriverAccess.Telescope.Choose(Properties.Settings.Default.DriverId);
            SetUIState();
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            if (IsConnected)
            {
                driver.Connected = false;
            }
            else
            {
                try
                {
                    driver = new ASCOM.DriverAccess.Telescope(Properties.Settings.Default.DriverId);
                    driver.Connected = true;
                    if (!backgroundWorker1.IsBusy)
                        backgroundWorker1.RunWorkerAsync();
                }
                catch
                {
                    
                }
            }
            SetUIState();
        }

        private void SetUIState()
        {
            buttonConnect.Enabled = !string.IsNullOrEmpty(Properties.Settings.Default.DriverId);
            buttonChoose.Enabled = !IsConnected;
            buttonConnect.Text = IsConnected ? "Disconnect" : "Connect";
            Coordinates.Enabled = IsConnected;
            ControlButtons.Enabled = IsConnected;
        }

        private bool IsConnected
        {
            get
            {
                return ((this.driver != null) && (driver.Connected == true));
            }
        }

        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            while (!backgroundWorker1.CancellationPending)
            {
                if (this.driver != null && this.driver.Connected)
                {
                    backgroundWorker1.ReportProgress(50, driver);
                }
                Thread.Sleep(100);
            }
        }

        private bool isSetting;
        private void backgroundWorker1_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage == 50)
            {
                isSetting = true;
                Ra.Text = new DMS(driver.RightAscension, true).ToString();
                Dec.Text = new DMS(driver.Declination).ToString();
                Alt.Text = new DMS(driver.Altitude).ToString();
                Azm.Text = new DMS(driver.Azimuth).ToString();
                var mode = driver.Action("GetTrackingMode", "");
                int trMode;
                if (int.TryParse(mode, out trMode) && TrMode.SelectedIndex != trMode)
                {
                    TrMode.SelectedIndex = trMode;
                }
                isSetting = false;
            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {

        }

        private void Control_MouseDown(object sender, MouseEventArgs e)
        {
            if (!(sender is Button)) return;
            var b = (Button) sender;
            TelescopeAxes axis;
            var rate = 0d;
            switch (b.Name)
            {
                case "Ra_p":
                    axis = TelescopeAxes.axisPrimary;
                    rate = 1d;
                    break;
                case "Ra_n":
                    axis = TelescopeAxes.axisPrimary;
                    rate = -1d;
                    break;
                case "Dec_p":
                    axis = TelescopeAxes.axisSecondary;
                    rate = 1d;
                    break;
                case "Dec_n":
                    axis = TelescopeAxes.axisSecondary;
                    rate = -1d;
                    break;
                default:
                    return;
            }

            if (driver == null || !driver.Connected) return;
            driver.MoveAxis(axis, rate);
        }

        private void Control_MouseUp(object sender, MouseEventArgs e)
        {
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

            if (driver == null || !driver.Connected) return;
            driver.MoveAxis(axis, 0);
        }

        private void Rate_Click(object sender, EventArgs e)
        {
            if (!(sender is Button)) return;
            var bt = (Button) sender;
            switch(bt.Name)
            {
                case "Sidereal":
                    driver.TrackingRate = DriveRates.driveSidereal;
                    break;
                case "Solar":
                    driver.TrackingRate = DriveRates.driveSolar;
                    break;
                case "Lunar":
                    driver.TrackingRate = DriveRates.driveLunar;
                    break;
            }
        }

        private void TrackinMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isSetting) return;
            driver.Action("SetTrackingMode", TrMode.SelectedIndex.ToString());
        }

        private void getHome_Click(object sender, EventArgs e)
        {
            if (driver != null && driver.Connected)
            {
                //driver.FindHome();
                var pos = driver.Action("GetPosition", "").Split(';');
                double alt, azm;
                if (double.TryParse(pos[0], out azm) && double.TryParse(pos[1], out alt))
                {
                    positionAzm.Text = new DMS(azm).ToString();
                    positionAlt.Text = new DMS(alt).ToString();
                }
            }
        }

        private void goHome_Click(object sender, EventArgs e)
        {
            if (driver != null && driver.Connected)
            {
                driver.FindHome();
            }
        }
    
        private void test()
        {
            var trf = new ASCOM.Astrometry.Transform.Transform();
            var jd = trf.JulianDateTT;
            console.Text += string.Format("JD value = {0}\n", jd);
        }
    }

}
