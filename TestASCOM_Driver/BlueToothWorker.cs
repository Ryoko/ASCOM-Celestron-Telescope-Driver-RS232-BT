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

namespace ASCOM.CelestronAdvancedBlueTooth
{
    class BlueToothDiscover
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

        public bool Connect(object connectionInfo)
        {
            if (connectionInfo is BluetoothAddress)
            {
                di = new BluetoothDeviceInfo(connectionInfo as BluetoothAddress);
            }
            else
            {
                if (!(connectionInfo is BluetoothDeviceInfo)) return false;
                di = (BluetoothDeviceInfo) connectionInfo;
            }
            BluetoothAddress addr = di.DeviceAddress; //BluetoothAddress.Parse("001122334455");
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
            return true;
        }

        public byte[] Transfer(byte[] send) {
            this.peerStream.Write(send, 0, send.Length);
            byte[] receive = new byte[1024];

            int offset = 0;
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
            return receive.Take(offset).ToArray();
        }

        public string Transfer(string command)
        {
            try{
                var send = Encoding.ASCII.GetBytes(command);
                var receive = Transfer(send);
                return Encoding.ASCII.GetString(receive, 0, receive.Length);
            }
            catch (Exception err)
            {
                return "";
            }
        }

        public void Disconnect()
        {
            this.cli.Close();
        }

    }
}
