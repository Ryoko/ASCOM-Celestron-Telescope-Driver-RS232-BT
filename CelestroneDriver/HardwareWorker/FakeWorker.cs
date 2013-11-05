using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ASCOM.CelestronAdvancedBlueTooth.CelestroneDriver.HardwareWorker
{
    using ASCOM.CelestronAdvancedBlueTooth.CelestroneDriver.TelescopeWorker;

    public class FakeWorker : IDeviceWorker
    {
        private bool _connected = false;
        private MockTelescope _telescope;

        public FakeWorker(double firmwareVersion, TelescopeType type)
        {
            FirmvareVersion = firmwareVersion;
            ModelType = type;
        }

        public double FirmvareVersion
        {
            set
            {
                _telescope.FirmwareVersion = value;
            }
        }

        public TelescopeType ModelType
        {
            set
            {
                _telescope.TelescopeType = value;
            }
        }

        public bool Connect(object connectionInfo)
        {
            _connected = true;
            return _connected;
        }

        public void Disconnect()
        {
            _connected = false;
        }

        public string Transfer(string command)
        {
            return _telescope.exchange(command);
        }

        public byte[] Transfer(byte[] send)
        {
            return _telescope.exchange(send);
        }

        public bool IsConnected
        {
            get
            {
                return _connected;
            }
            private set
            {
                _connected = value;
            }
        }

        public void CheckConnected(string message)
        {
            if (!_connected) throw new NotConnectedException(message);
        }
    }
}
