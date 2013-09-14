using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Windows.Forms;
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
            if (int.TryParse(Obstruction.Text, out val)) { Telescope.obstruction = val; }
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
            //BlueToothDevices.Items.Clear();
            //var devices = BlueToothWorker.GetDevicesinRange();
            //foreach (var item in devices)
            //{
            //    BlueToothDevices.Items.Add(item);
            //}
            selDeviceInfo = BlueToothDiscover.GetDeviceDialog();
            SelectedBluetooth.Text = selDeviceInfo.DeviceName;
            //var services = selDeviceInfo.InstalledServices;
            //var records = new Dictionary<Guid, InTheHand.Net.Bluetooth.ServiceRecord[]>();
            //foreach (var serv in services)
            //{
            //    records[serv] = selDeviceInfo.GetServiceRecords(serv);
                
            //}

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
    }
}