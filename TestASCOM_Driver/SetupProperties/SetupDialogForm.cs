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
using ASCOM.CelestronAdvancedBlueTooth.Utils;
using InTheHand.Net;
using InTheHand.Net.Sockets;
using System.Diagnostics;
using System.Runtime.Serialization.Formatters;

namespace ASCOM.CelestronAdvancedBlueTooth
{
    [ComVisible(false)]					// Form not registered for COM!
    public partial class SetupDialogForm : Form
    {
        private BluetoothAddress selDeviceAddress;
        private BluetoothDeviceInfo selDeviceInfo;
        private TelescopeModels models = new TelescopeModels();

        public SetupDialogForm()
        {
            InitializeComponent();
            // Initialise current values of user settings from the ASCOM Profile 
            SelectedComPort.Text = Telescope.comPort;
            chkTrace.Checked = Telescope.traceState;
            selDeviceAddress = Telescope.bluetoothDevice;
            if (selDeviceAddress != null)
            {
//                selDeviceInfo = BlueToothDiscover.GetDevice(selDeviceAddress);
                selDeviceInfo = new BluetoothDeviceInfo(selDeviceAddress);
                SelectedBluetooth.Text = selDeviceInfo != null ? selDeviceInfo.DeviceName : "";
            }
            tabControl1.SelectedIndex = Telescope.isBluetooth ? 1 : 0;

            LatSuff.SelectedIndex = 0;
            if (Telescope.latitude > -1000)
            {
                var latitude = new DMS(Telescope.latitude);
                Latitude.Text = latitude.ToString();
                LatSuff.SelectedIndex = latitude.Sign > 0 ? 0 : 1;
            }
            LonSuff.SelectedIndex = 0;
            if (Telescope.longitude > -1000)
            {
                var longitude = new DMS(Telescope.longitude);
                Longitude.Text = longitude.ToString();
                LonSuff.SelectedIndex = longitude.Sign > 0 ? 0 : 1;
            }
            Altitude.Text = Telescope.elevation.ToString();
            
            Apperture.Text = Telescope.apperture.ToString();
            Focal.Text = Telescope.focal.ToString();
            Obstruction.Text = Telescope.obstruction.ToString();
            TrackingMode.SelectedIndex = Telescope.traceMode;
            HasGPS.Checked = Telescope.hasGPS;

            ScopeSelection.Items.Clear();
            foreach (var model in models)
            {
                ScopeSelection.Items.Add(model);
            }
        }

        private void cmdOK_Click(object sender, EventArgs e) // OK button event handler
        {
            // Place any validation constraint checks here

            Telescope.comPort = SelectedComPort.Text; // Update the state variables with results from the dialogue
            Telescope.traceState = chkTrace.Checked;

            Telescope.bluetoothDevice = selDeviceInfo.DeviceAddress;
            //var memoryStream = new MemoryStream();
            //var binaryFormatter = new BinaryFormatter();
            //binaryFormatter.Serialize(memoryStream, selDeviceInfo);

            //String base64String = Convert.ToBase64String(memoryStream.ToArray());
            Telescope.isBluetooth = tabControl1.SelectedIndex > 0;

            DMS lat, lon;

            if (DMS.TryParse(Latitude.Text, out lat) && DMS.TryParse(Longitude.Text, out lon))
            {
                lat.Sign = LatSuff.SelectedIndex > 0 ? -1 : 1;
                Telescope.latitude = lat.Deg;
                lon.Sign = LonSuff.SelectedIndex > 0 ? -1 : 1;
                Telescope.longitude = lon.Deg;
            }
            else
            {
                Telescope.latitude = -1000;
                Telescope.longitude = -1000;
            }
            int val;
            if (int.TryParse(Altitude.Text, out val)) { Telescope.elevation = val; }
            if (int.TryParse(Apperture.Text, out val)) { Telescope.apperture = val; }
            if (int.TryParse(Focal.Text, out val)) { Telescope.focal = val; }
            if (int.TryParse(ObstructionLabel.Text, out val)) { Telescope.obstruction = val; }
            Telescope.traceMode = TrackingMode.SelectedIndex;
            Telescope.hasGPS = HasGPS.Checked;

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

        private void Latitude_Validated(object sender, EventArgs e)
        {
            if (!(sender is TextBox)) return;
            var tb = sender as TextBox;
            Utils.DMS val;
            if (Utils.DMS.TryParse(tb.Text, out val))
            {
                tb.Text = val.ToString();
            }
            
        }

        private void ScopeSelection_SelectedIndexChanged(object sender, EventArgs e)
        {
            var model = (TelescopeModel) ScopeSelection.SelectedItem;
            Apperture.Text = model.Apperture.ToString();
            Focal.Text = model.FocalLenth.ToString();
            Obstruction.Text = model.ObstructionPercent.ToString();
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
                    HasGPS.CheckState = CheckState.Indeterminate;
                    HasGPS.Enabled = true;
                    break;
            }
            setButtons();
        }

        private void SelectedBluetooth_Click(object sender, EventArgs e)
        {
            selDeviceInfo = BlueToothDiscover.GetDeviceDialog();
            SelectedBluetooth.Text = selDeviceInfo.DeviceName;
        }

        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            setButtons();
        }

        private void SelectedBluetooth_TextChanged(object sender, EventArgs e)
        {
            setButtons();
        }

        private void setButtons()
        {
            cmdOK.Enabled = (tabControl1.SelectedIndex != 1) || !string.IsNullOrEmpty(SelectedBluetooth.Text);
            CheckScope.Enabled = ((tabControl1.SelectedIndex != 1) || !string.IsNullOrEmpty(SelectedBluetooth.Text)) && ScopeSelection.SelectedIndex >= 0;
            
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

            Telescope.bluetoothDevice = selDeviceInfo.DeviceAddress;
            Telescope.isBluetooth = tabControl1.SelectedIndex == 1 && selDeviceInfo != null;
            
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
                Latitude.Text = new DMS(prop.Location.Lat).ToString();
                Longitude.Text = new DMS(prop.Location.Lon).ToString();
                TrackingMode.SelectedIndex = (int)prop.TrackingMode;
                HasGPS.Checked = prop.HasGPS;
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
    }
}