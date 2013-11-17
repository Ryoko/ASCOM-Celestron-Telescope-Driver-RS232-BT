using System.Diagnostics;
using ASCOM.Utilities;

namespace ASCOM.CelestronAdvancedBlueTooth.CelestroneDriver.HardwareWorker
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using ASCOM.CelestronAdvancedBlueTooth.CelestroneDriver.TelescopeWorker;
    using ASCOM.CelestronAdvancedBlueTooth.CelestroneDriver.Utils;

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
        private TraceLogger tl = null;

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
                //if (peerStream.CanTimeout) this.peerStream.ReadTimeout = TIMEOUT;
            }
            catch (Exception err)
            {
                return false;
            }
            this.IsConnected = true;
            return true;
        }

        public byte[] Transfer(byte[] send, int rLength = -1)
        {

            if (this.peerStream == null) return new byte[0];
            byte[] receive = new byte[1024];
            int offset = 0;


            lock (this.lockObj)
            {
//                for (int j = 0; j < 3; j++)
//                {
                    this.peerStream.Write(send, 0, send.Length);
                    var begin = -1;
                    for (int i = 0; i < 10; i++)
                    {
                        Thread.Sleep(10);
                        int lenRepl = 0;
                        try
                        {
                            lenRepl = this.peerStream.Read(receive, offset, 1024 - offset);
                        }
                        catch
                        {
                            
                        }
                        offset += lenRepl;

                        if ( (rLength > 0 && offset >= rLength) || ( /*rLength < 0 &&*/ offset > 0 && receive[offset - 1] == (byte) GeneralCommands.TERMINATOR) )
                        {
                            if (tl != null)
                                tl.LogMessage("Bluetooth transmit", string.Format("Send:{0} Recived:{1}", Utils.Bytes2Dump(send), Utils.Bytes2Dump(receive.Take(offset))));
                            return receive.Take(offset).ToArray();
                        }
                        //if (rLength > 0 && offset >= rLength) return receive.Take(offset).ToArray();

                        if (begin < 0) begin = Environment.TickCount;
                        if (Environment.TickCount > begin + TIMEOUT) break;
                    }
//                }
            }
            throw new Exception("Error send command to device");
        }

        public byte[] Transfer(GeneralCommands command, int rLen = -1)
        {
            return Transfer(command.ToBytes(), rLen);
        }

        public void CheckConnected(string message)
        {
            if (!this.IsConnected)
            {
                throw new ASCOM.NotConnectedException(message);
            }
        }

        public TraceLogger TraceLogger
        {
            set
            {
                tl = value;
            }
        }

        public string Transfer(string command, int rLen = -1)
        {
            try{
                var send = Encoding.ASCII.GetBytes(command);
                var receive = this.Transfer(send, rLen);
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
