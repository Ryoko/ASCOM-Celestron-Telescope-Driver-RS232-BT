namespace ASCOM.CelestronAdvancedBlueTooth.CelestroneDriver.HandForm
{
    using global::CelestroneDriver.TelescopeWorker;

    public interface IHandControl
    {
        void ShowForm(bool show);
        void SetForm(TelescopeWorker worker);
    }
}
