namespace ASCOM.CelestronAdvancedBlueTooth.CelestroneDriver.HardwareWorker
{
    using System;
    using System.Linq;

    using ASCOM.CelestronAdvancedBlueTooth.CelestroneDriver.TelescopeWorker;
    using ASCOM.CelestronAdvancedBlueTooth.CelestroneDriver.Utils;
    using ASCOM.Utilities;

    public class ComPortWorker : IDeviceWorker
    {
        private Serial _port = new Serial();
        public bool IsConnected { get; private set; }

        public bool Connect(object connectionInfo)
        {
            try
            {
                if (this._port.Connected)
                {
                    this._port.Connected = false;
                }
                if (connectionInfo is string)
                {
                    this._port.PortName = (string)connectionInfo;
                }
                else if (connectionInfo is int)
                {
                    this._port.Port = (int)connectionInfo;
                }
                else
                {
                    return false;
                }
                if (!this._port.AvailableCOMPorts.Contains(this._port.PortName)) return false;

                this._port.Speed = SerialSpeed.ps9600;
                this._port.DataBits = 8;
                this._port.Parity = SerialParity.None;
                this._port.StopBits = SerialStopBits.One;
                this._port.RTSEnable = false;
                this._port.Handshake = SerialHandshake.None;

                this._port.Connected = true;
                this.IsConnected = this._port.Connected;
                return this.IsConnected;
            }
            catch (Exception err)
            {
                return false;
            }
        }

        public void Disconnect()
        {
            try
            {
                if (this._port.Connected)
                {
                    this._port.Connected = false;
                }
            }catch(Exception err){}
            this.IsConnected = false;
        }

        public string Transfer(string command, int rLen = -1)
        {
            try
            {
                this._port.Transmit(command);

                var res = rLen < 0 ? this._port.ReceiveTerminated(GeneralCommands.TERMINATOR.ToString()) : this._port.ReceiveCounted(rLen);
                return res;
            }
            catch (Exception err)
            {
                return "";
            }
        }

        public byte[] Transfer(byte[] send, int rLen = -1)
        {
            try
            {
                this._port.TransmitBinary(send);
                var res = rLen < 0 ? this._port.ReceiveTerminatedBinary(new[]{(byte)GeneralCommands.TERMINATOR}) :
                    this._port.ReceiveCountedBinary(rLen);
                return res;
            }
            catch (Exception err)
            {
                return new byte[0];
            }
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

        public TraceLogger TraceLogger { set; private get; }
    }
}
