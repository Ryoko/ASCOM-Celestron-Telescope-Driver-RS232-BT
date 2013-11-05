namespace ASCOM.CelestronAdvancedBlueTooth.CelestroneDriver.HandForm
{
    using ASCOM.CelestronAdvancedBlueTooth.CelestroneDriver.TelescopeWorker;

    public interface IHandControl
    {
        void ShowForm(bool show);
        void SetForm(TelescopeWorker worker);
    }
}
