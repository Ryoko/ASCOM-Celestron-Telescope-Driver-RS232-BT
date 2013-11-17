using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using ASCOM.CelestronAdvancedBlueTooth;
using ASCOM.DeviceInterface;

namespace ASCOM.CelestronAdvancedBluetooth
{
    using ASCOM.CelestronAdvancedBlueTooth.CelestroneDriver.Utils;

    using InputControl;

    public partial class Form1 : Form
    {

        private ASCOM.DriverAccess.Telescope driver;
        //private AInputcontrol pad;

        public Form1()
        {
            InitializeComponent();
            SetUIState();
            SetActions();
            //pad = new GamePad();
            //pad.OnUpdate += OnUpdate;
            //this.test();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            backgroundWorker1.CancelAsync();
            if (IsConnected)
                driver.Connected = false;
            driver.Dispose();
            driver = null;
            //pad = null;
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
                catch(Exception err)
                {
                    driver.Connected = false;
                    MessageBox.Show(
                        string.Format("Connection to device failed"),
                        "Connection error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
            SetUIState();
        }

        private void SetUIState()
        {
            buttonConnect.Enabled = !string.IsNullOrEmpty(Properties.Settings.Default.DriverId);
            buttonChoose.Enabled = !IsConnected;
            buttonConnect.Text = IsConnected ? "Disconnect" : "Connect";
            var park = false;
            if (IsConnected)
            {
                park = driver.AtPark;
            }
            Coordinates.Enabled = IsConnected;
            actions.Enabled = IsConnected;
            ControlButtons.Enabled = IsConnected && !park;
            Park.Text = park ? "Unpark" : "Park";
            goHome.Enabled = !park;
            setPark.Enabled = !park;
            TrMode.Enabled = !park;
        }

        private bool IsConnected
        {
            get
            {
                return ((this.driver != null) && driver.Connected);
            }
        }


        private double pAlt = 0, pAzm = 0;
        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            while (!backgroundWorker1.CancellationPending)
            {
                if (this.driver != null && this.driver.Connected)
                {
                    try
                    {
                        var pos = driver.Action("GetPosition", "").Split(';');
                        //double alt, azm;
                        double.TryParse(pos[0], out pAzm);
                        double.TryParse(pos[1], out pAlt);
                    }catch{}
                    backgroundWorker1.ReportProgress(50, driver);
                }
                Thread.Sleep(100);
            }
        }

        private bool isSetting;
        private void backgroundWorker1_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            if (backgroundWorker1.CancellationPending) return;
            if (e.ProgressPercentage == 50)
            {
                try
                {
                    isSetting = true;
                    Ra.Text = new DMS(driver.RightAscension, true).ToString();
                    Dec.Text = new DMS(driver.Declination).ToString();
                    Alt.Text = new DMS(driver.Altitude).ToString();
                    Azm.Text = new DMS(driver.Azimuth).ToString();
                    positionAzm.Text = new DMS(pAzm).ToString();
                    positionAlt.Text = new DMS(pAlt).ToString();
                    slewState.Text = driver.Slewing ? "Slewing" : "";

                    var mode = driver.Action("GetTrackingMode", "");
                    int trMode;
                    if (int.TryParse(mode, out trMode) && TrMode.SelectedIndex != trMode)
                    {
                        TrMode.SelectedIndex = trMode;
                    }
                    isSetting = false;
                }catch{}
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

        private void setPark_Click(object sender, EventArgs e)
        {
            if (driver != null && driver.Connected)
            {
                driver.SetPark();
            }
        }

        private void goHome_Click(object sender, EventArgs e)
        {
            if (driver != null && driver.Connected)
            {
                driver.FindHome();
            }
        }

        private void Park_Click(object sender, EventArgs e)
        {
            if (driver != null && driver.Connected)
            {
                if (driver.AtPark)
                {
                    driver.Unpark();
                }
                else
                {
                    driver.Park();
                }
                SetUIState();
            }
        }
    
        private void test()
        {
            var trf = new ASCOM.Astrometry.Transform.Transform();
            var jd = trf.JulianDateTT;
            console.Text += string.Format("JD value = {0}\n", jd);
        }


        private void useGamePad_CheckedChanged(object sender, EventArgs e)
        {
            //pad.Active = useGamePad.Checked;
        }

        private void OnUpdate(ControllerState controllerState)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<ControllerState>(OnUpdate), controllerState);
                return;
            }
            gamepadX.Text = controllerState.X.ToString();
            gamepadY.Text = controllerState.Y.ToString();
            var rateX = ((int) (controllerState.X*9)) * 10;
            var rateY = ((int) (controllerState.Y*9)) * 10;
            if (driver == null || !driver.Connected) return;
            driver.MoveAxis(TelescopeAxes.axisPrimary, rateX);
            driver.MoveAxis(TelescopeAxes.axisSecondary, rateY);
        }

        private List<string> actList = new List<string>() {"SlewSync", "SiteOfPier", "Destination SiteOfPier"}; 

        private void SetActions()
        {
            actionList.Items.Clear();
            actionList.Items.AddRange(actList.ToArray());
            actionList.SelectedIndex = 0;
        }

        private void slewTo_Click(object sender, EventArgs e)
        {
            if (driver.Connected)
            {
                switch (actionList.SelectedIndex)
                {
                    case 0:
                        ConsoleAdd(string.Format("Value: {0}", "Slewing"));
                        driver.SlewToCoordinates(12, 45);
                        ConsoleAdd(string.Format("Value: {0}", "Slewed"));
                        return;
                    case 1:
                        var res = driver.SideOfPier;
                        ConsoleAdd(string.Format("SOP: {0}", res.ToString()));
                        return;
                    case 2:
                        ConsoleAdd(string.Format("Dest SOP: {0}", driver.DestinationSideOfPier(15, 12)));
                        return;

                }
                //var sop = driver.SideOfPier; //driver.DestinationSideOfPier(90.55555, 90.555555);
            }
        }

        private void ConsoleAdd(string val)
        {
            var l = new List<string>(console.Lines);
            l.Add(string.Format("[{0}] {1}", DateTime.Now.TimeOfDay.ToString("t"), val));
            console.Lines = l.ToArray();
        }
    }

}
