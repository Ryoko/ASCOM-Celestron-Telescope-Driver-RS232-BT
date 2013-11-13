namespace ASCOM.CelestronAdvancedBlueTooth.CelestroneDriver.HardwareWorker
{
    using ASCOM.CelestronAdvancedBlueTooth.CelestroneDriver.TelescopeWorker;

    public interface IDeviceWorker
    {
        bool Connect(object connectionInfo);
        void Disconnect();
        string Transfer(string command);
        byte[] Transfer(byte[] send);
        byte[] Transfer(GeneralCommands command);
        bool IsConnected { get; }
        void CheckConnected(string message);
    }
}
