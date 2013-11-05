namespace ASCOM.CelestronAdvancedBlueTooth.CelestroneDriver.HardwareWorker
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using InTheHand.Net.Sockets;

    using System.Windows.Forms;

    using InTheHand.Windows.Forms;
    using InTheHand.Net;
    using InTheHand.Net.Bluetooth;

    using System.IO;
    using System.Threading;

    public class BlueToothDiscover
    {
        
        //private static BlueToothWorker _worker = new BlueToothWorker();
        public static string[] GetDevicesinRange()
        {
            var result = new List<string>();
            var cli = new InTheHand.Net.Sockets.BluetoothClient();
            
            //cli.BeginDiscoverDevices(10, true, true, false, false, DeviceDiscovered, null);
            var peers = cli.DiscoverDevices();
            foreach (var device in peers)
            {
                result.Add(device.DeviceName);
            }

            return result.ToArray();
        }

        public static BluetoothDeviceInfo GetDeviceDialog()
        {
            var dlg = new SelectBluetoothDeviceDialog();
            DialogResult result = dlg.ShowDialog();
            if (result != DialogResult.OK)
            {
                return null;
            }
            BluetoothDeviceInfo device = dlg.SelectedDevice;
            BluetoothAddress addr = device.DeviceAddress;
            return device;
        }
    }

    public class BluetoothWorker : IDeviceWorker
    {
        private const int TIMEOUT = 1000;

        private BluetoothEndPoint ep;
        private BluetoothClient cli;
        private Stream peerStream;
        private BluetoothDeviceInfo di;
        private object lockObj = new object();
        public bool IsConnected { get; private set; }

        public bool Connect(object connectionInfo)
        {
            if (connectionInfo is BluetoothAddress)
            {
                this.di = new BluetoothDeviceInfo(connectionInfo as BluetoothAddress);
            }
            else
            {
                if (!(connectionInfo is BluetoothDeviceInfo)) return false;
                this.di = (BluetoothDeviceInfo) connectionInfo;
            }
            BluetoothAddress addr = this.di.DeviceAddress; //BluetoothAddress.Parse("001122334455");
            Guid serviceClass = BluetoothService.SerialPort;
            // - or - etc
            // serviceClass = MyConsts.MyServiceUuid
            //
            try
            {
                this.ep = new BluetoothEndPoint(addr, serviceClass);
                this.cli = new BluetoothClient();
                this.cli.Connect(this.ep);
                this.peerStream = this.cli.GetStream();
            }
            catch (Exception err)
            {
                return false;
            }
            this.IsConnected = true;
            return true;
        }

        public byte[] Transfer(byte[] send)
        {

            if (this.peerStream == null) return new byte[0];
            byte[] receive = new byte[1024];
            int offset = 0;


            lock (this.lockObj)
            {
                this.peerStream.Write(send, 0, send.Length);
                var begin = -1;
                for (int i = 0; i < 10; i++)
                {
                    Thread.Sleep(10);
                    var lenRepl = this.peerStream.Read(receive, offset, 1024 - offset);
                    offset += lenRepl;
                    if (receive[offset - 1] == 35) break;

                    if (begin < 0) begin = Environment.TickCount;
                    if (Environment.TickCount > begin + TIMEOUT) break;
                }
            }
            //Debug.WriteLine(string.Format("Send:{0} Recived:{1}", Utils.Utils.Bytes2Dump(send), Utils.Utils.Bytes2Dump(receive.Take(offset))));
            return receive.Take(offset).ToArray();
        }

        public void CheckConnected(string message)
        {
            if (!this.IsConnected)
            {
                throw new ASCOM.NotConnectedException(message);
            }
        }

        public string Transfer(string command)
        {
            try{
                var send = Encoding.ASCII.GetBytes(command);
                var receive = this.Transfer(send);
                return Encoding.ASCII.GetString(receive, 0, receive.Length);
            }
            catch (Exception err)
            {
                return "";
            }
        }

        public void Disconnect()
        {
            if (this.peerStream != null) this.peerStream.Dispose();
            //if (this.ep != null) this.ep.
            if (this.cli != null)
            {
                this.cli.Close();
                this.cli.Dispose();
            }
            this.IsConnected = false;
        }

    }
}
