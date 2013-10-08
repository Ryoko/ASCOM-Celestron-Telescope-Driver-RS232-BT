using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ASCOM.CelestronAdvancedBlueTooth.HardwareWorker
{
    using ASCOM.Utilities;

    class ComPortWorker : IDeviceWorker
    {
        private Serial _port = new Serial();


        public bool Connect(object connectionInfo)
        {
            try
            {
                if (_port.Connected)
                {
                    _port.Connected = false;
                }
                if (connectionInfo is string)
                {
                    _port.PortName = (string)connectionInfo;
                }
                else if (connectionInfo is int)
                {
                    _port.Port = (int)connectionInfo;
                }
                else
                {
                    return false;
                }
                if (!_port.AvailableCOMPorts.Contains(_port.PortName)) return false;

                _port.Speed = SerialSpeed.ps9600;
                _port.DataBits = 8;
                _port.Parity = SerialParity.None;
                _port.StopBits = SerialStopBits.One;
                _port.RTSEnable = false;
                _port.Handshake = SerialHandshake.None;

                _port.Connected = true;
                return _port.Connected;
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
                if (_port.Connected)
                {
                    _port.Connected = false;
                }
            }catch(Exception err){}
        }

        public string Transfer(string command)
        {
            try
            {
                _port.Transmit(command);
                var res = _port.ReceiveTerminated("#");
                return res;
            }
            catch (Exception err)
            {
                return "";
            }
        }

        public byte[] Transfer(byte[] send)
        {
            try
            {
                _port.TransmitBinary(send);
                var res = _port.ReceiveTerminatedBinary(new[]{(byte)'#'});
                return res;
            }
            catch (Exception err)
            {
                return new byte[0];
            }
        }
    }
}
