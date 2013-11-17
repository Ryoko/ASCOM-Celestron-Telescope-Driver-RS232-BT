using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using ASCOM.CelestronAdvancedBlueTooth.SetupProperties;
using ASCOM.DeviceInterface;
using ASCOM.Utilities;
using ASCOM.CelestronAdvancedBlueTooth;

using InTheHand.Net;
using InTheHand.Net.Sockets;
using System.Diagnostics;
using System.Runtime.Serialization.Formatters;
using TelescopeModel = ASCOM.CelestronAdvancedBlueTooth.SetupProperties.TelescopeModel;

namespace ASCOM.CelestronAdvancedBlueTooth
{
    using CelestroneDriver.HardwareWorker;
    using CelestroneDriver.Utils;

    [ComVisible(false)]					// Form not registered for COM!
    public partial class SetupDialogForm : Form
    {
        private BluetoothAddress selDeviceAddress;
        private BluetoothDeviceInfo selDeviceInfo;
        private TelescopeModels models = new TelescopeModels();
        private bool isInit = false;
        private bool isHasGPSsetted = false;

        private TelescopeSettingsProfile profile;

        public SetupDialogForm(TelescopeSettingsProfile Profile)
        {
            profile = Profile;
            InitializeComponent();
            // Initialise current values of user settings from the ASCOM Profile 
            SelectedComPort.Text = profile.ComPort;
            chkTrace.Checked = profile.TraceState;
            selDeviceAddress = profile.BluetoothDevice;
            try
            {
                if (selDeviceAddress != null)
                {
                    //                selDeviceInfo = BlueToothDiscover.GetDevice(selDeviceAddress);
                    selDeviceInfo = new BluetoothDeviceInfo(selDeviceAddress);
                    SelectedBluetooth.Text = selDeviceInfo != null ? selDeviceInfo.DeviceName : "";
                }
            }catch{}
            tabControl1.SelectedIndex = profile.IsBluetooth ? 1 : 0;

            LatSuff.SelectedIndex = 0;
            if (profile.Latitude > -1000)
            {
                var latitude = new DMS(profile.Latitude);
                Latitude.Text = latitude.ToString();
                LatSuff.SelectedIndex = latitude.Sign > 0 ? 0 : 1;
            }
            LonSuff.SelectedIndex = 0;
            if (profile.Longitude > -1000)
            {
                var longitude = new DMS(profile.Longitude);
                Longitude.Text = longitude.ToString();
                LonSuff.SelectedIndex = longitude.Sign > 0 ? 0 : 1;
            }
            Elevation.Value = (decimal)profile.Elevation;

            Apperture.Value = (decimal)profile.Apperture * 1000;
            Focal.Value = (decimal)profile.Focal * 1000;
            Obstruction.Value = (decimal)profile.Obstruction;

            TrackingMode.SelectedIndex = profile.TrackingMode;
            HasGPS.CheckState = profile.HasGps < 0 ? CheckState.Indeterminate : profile.HasGps > 0 ? CheckState.Checked : CheckState.Unchecked;

            ShowHandControl.Checked = profile.ShowControl;

            ScopeSelection.Items.Clear();
            foreach (var model in models)
            {
                ScopeSelection.Items.Add(model);
                if (model.Name.Equals(profile.TelescopeModel))
                    ScopeSelection.SelectedIndex = ScopeSelection.Items.Count - 1;
            }

            isInit = true;
            setButtons();
        }

        private void cmdOK_Click(object sender, EventArgs e) // OK button event handler
        {
            // Place any validation constraint checks here
            var ts = new TelescopeSettings();
            if (!GetFromForm(ts))
            {
                throw new Exception("Error(s) in value(s)");
            }
            profile.ComPort = ts.ComPort; // Update the state variables with results from the dialogue
            profile.TraceState = chkTrace.Checked;

            profile.BluetoothDevice = ts.BluetoothAddr;
            profile.IsBluetooth = ts.IsBluetooth;

            profile.Latitude = ts.Latitude;
            profile.Longitude = ts.Longitude;
            profile.Elevation = ts.Elevation;

            profile.Apperture = ts.Apperture;
            profile.Focal = ts.FocalRate;
            profile.Obstruction = ts.Obstruction;

            profile.TelescopeModel = ts.ModelName;
            profile.TrackingMode = ts.TrackingMode;
            profile.HasGps = ts.HasGPS;

            profile.ShowControl = ts.ShowHandControl;
        }

        private void cmdCancel_Click(object sender, EventArgs e) // Cancel button event handler
        {
            Close();
        }

        private void BrowseToAscom(object sender, EventArgs e) // Click on ASCOM logo event handler
        {
            try
            {
                System.Diagnostics.Process.Start("http://ascom-standards.org/");
            }
            catch (System.ComponentModel.Win32Exception noBrowser)
            {
                if (noBrowser.ErrorCode == -2147467259)
                    MessageBox.Show(noBrowser.Message);
            }
            catch (System.Exception other)
            {
                MessageBox.Show(other.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SelectedBluetooth_Click(sender, e);
        }

        private void GPS_Validated(object sender, EventArgs e)
        {
            if (!isInit) return;
            isInit = false;
            if (!(sender is TextBox)) return;
            var tb = sender as TextBox;
            DMS val;
            if (DMS.TryParse(tb.Text, out val))
            {
                tb.Text = val.ToString();
            }
            isInit = true;
            setButtons();            
        }

        private void ScopeSelection_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!isInit) return;
            var model = (TelescopeModel) ScopeSelection.SelectedItem;
            isInit = false;
            Apperture.Value = (decimal)model.Apperture;
            Focal.Value = (decimal)model.FocalLenth;
            Obstruction.Value = (decimal)model.ObstructionPercent;
            switch (model.GPS)
            {
                case GPSmode.No:
                    HasGPS.Checked = false;
                    HasGPS.Enabled = false;
                    break;
                case GPSmode.Yes:
                    HasGPS.Checked = true;
                    HasGPS.Enabled = false;
                    break;
                default:
                    if (!isHasGPSsetted)
                    {
                        HasGPS.CheckState = CheckState.Indeterminate;
                    }
                    HasGPS.Enabled = true;
                    break;
            }
            isInit = true;
            setButtons();
        }

        private void SelectedBluetooth_Click(object sender, EventArgs e)
        {
            selDeviceInfo = BlueToothDiscover.GetDeviceDialog();
            if (selDeviceInfo != null)
            {
                SelectedBluetooth.Text = selDeviceInfo.DeviceName;
                setButtons();
            }
        }

        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            setButtons();
        }

        private void SelectedBluetooth_TextChanged(object sender, EventArgs e)
        {
            setButtons();
        }
        
        private TelescopeSettings ts = new TelescopeSettings();
        private void setButtons()
        {
            if (!isInit) return;
            //cmdOK.Enabled = (tabControl1.SelectedIndex != 1) || !string.IsNullOrEmpty(SelectedBluetooth.Text);
            CheckScope.Enabled = ((tabControl1.SelectedIndex != 1) || !string.IsNullOrEmpty(SelectedBluetooth.Text)) && ScopeSelection.SelectedIndex >= 0;
            cmdOK.Enabled = GetFromForm(ts);
        }

        private AutoResetEvent lockEvent = new AutoResetEvent(false);
        private BackgroundWorker bgWorker;
        private void CheckScope_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Processing";
            toolStripProgressBar1.Visible = true;
            toolStripProgressBar1.Style = ProgressBarStyle.Blocks;
            tabControl1.Enabled = false;
            panel1.Enabled = false;
            panel2.Enabled = false;

            profile.BluetoothDevice = selDeviceInfo != null ? selDeviceInfo.DeviceAddress : null;
            profile.IsBluetooth = tabControl1.SelectedIndex == 1 && selDeviceInfo != null;
            
            bgWorker = new BackgroundWorker();
            bgWorker.DoWork += bgWorker_DoWork;
            bgWorker.RunWorkerCompleted += bgWorker_RunWorkerCompleted;
            bgWorker.RunWorkerAsync();
            int per = 0;
            while (bgWorker.IsBusy)
            {
                //if (lockEvent.)
                Thread.Sleep(100);
                per += 1;
                if (per > 100) per = 0;
                toolStripProgressBar1.Value = per;
                Application.DoEvents();
            }
            toolStripStatusLabel1.Text = "Ready";
            toolStripProgressBar1.Visible = false;
            toolStripProgressBar1.Value = 0;
            tabControl1.Enabled = true;
            panel1.Enabled = true;
            panel2.Enabled = true;
            var prop = Telescope.TelescopeProperties;
            if (prop.IsReady)
            {
                isInit = false;
                Latitude.Text = new DMS(prop.Location.Lat).ToString();
                Longitude.Text = new DMS(prop.Location.Lon).ToString();
                TrackingMode.SelectedIndex = (byte)prop.TrackingMode;
                HasGPS.Checked = prop.HasGPS;
                isInit = true;
                setButtons();
            }
        }

        void bgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
        }

        void bgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                var scope = (Telescope) Telescope.TelescopeV3;
                scope.Initialize();
                scope.Connected = true;
                var tBegin = Environment.TickCount;
                while (!Telescope.TelescopeProperties.IsReady)
                {
                    Thread.Sleep(100);
                    if (tBegin + 60000 < Environment.TickCount) break;
                }
                scope.Connected = false;
            }
            catch (Exception err)
            {

            }
            finally
            {
//                bgWorker.Dispose();
            }
        }

        private void HasGPS_CheckedChanged(object sender, EventArgs e)
        {
            isHasGPSsetted = true;
        }

        public bool GetFromForm(TelescopeSettings set)
        {
            bool res = true;
            set.ComPort = SelectedComPort.Text; // Update the state variables with results from the dialogue

            set.BluetoothAddr = selDeviceInfo != null ? selDeviceInfo.DeviceAddress : null;
            set.IsBluetooth = tabControl1.SelectedIndex > 0;

            DMS lat, lon;

            if (DMS.TryParse(Latitude.Text, out lat) && DMS.TryParse(Longitude.Text, out lon))
            {
                lat.Sign = LatSuff.SelectedIndex > 0 ? -1 : 1;
                set.Latitude = (double)lat.Deg;
                lon.Sign = LonSuff.SelectedIndex > 0 ? -1 : 1;
                set.Longitude = (double)lon.Deg;
            }
            else
            {
                res = false;
                set.Latitude = -1000;
                set.Longitude = -1000;
            }
            double val;
            set.Elevation = (double)Elevation.Value;
            res &= !(set.Apperture = (double)Apperture.Value / 1000).Equals(0);
            res &= !(set.FocalRate = (double)Focal.Value / 1000).Equals(0);
            set.Obstruction = (double)Obstruction.Value;
            if (ScopeSelection.SelectedIndex >= 0) { set.ModelName = ScopeSelection.Text; } else { res = false; }
            if (TrackingMode.SelectedIndex >= 0) {set.TrackingMode = TrackingMode.SelectedIndex;} else { res = false; }
            set.HasGPS = HasGPS.CheckState == CheckState.Indeterminate ? -1 : HasGPS.Checked ? 1 : 0;
            res &= set.HasGPS >= 0;
            set.ShowHandControl = ShowHandControl.Checked;
            if (set.IsBluetooth && selDeviceInfo == null) res = false;
            return res;
        }

        private void Field_ValueChanged(object sender, EventArgs e)
        {
            setButtons();
        }

    
    }

    public class TelescopeSettings
    {
        public string ModelName;
        public double Apperture;
        public double FocalRate;
        public double Obstruction;
        public int HasGPS;
        public double Latitude;
        public double Longitude;
        public double Elevation;
        public int TrackingMode;
        public bool IsBluetooth;
        public BluetoothAddress BluetoothAddr;
        public string ComPort;
        public bool ShowHandControl;

    }
}