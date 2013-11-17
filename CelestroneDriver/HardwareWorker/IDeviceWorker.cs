using ASCOM.Utilities;

namespace ASCOM.CelestronAdvancedBlueTooth.CelestroneDriver.HardwareWorker
{
    using ASCOM.CelestronAdvancedBlueTooth.CelestroneDriver.TelescopeWorker;

    public interface IDeviceWorker
    {
        bool Connect(object connectionInfo);
        void Disconnect();
        string Transfer(string command, int rLen = -1);
        byte[] Transfer(byte[] send, int rLen = -1);
        byte[] Transfer(GeneralCommands command, int rLen = -1);
        bool IsConnected { get; }
        void CheckConnected(string message);
        TraceLogger TraceLogger { set; }
    }
}
