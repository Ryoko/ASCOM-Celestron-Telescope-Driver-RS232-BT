using ASCOM.DeviceInterface;

namespace ASCOM.CelestronAdvancedBlueTooth.TelescopeWorker
{
    internal interface ITelescopeWorkerOperations
    {
        /// <summary>
        /// Get rate on Azm axis in (deg/sec)
        /// </summary>
        /// <param name="rate">Curent DriveRate</param>
        /// <param name="mode">Current TrackingMode</param>
        /// <returns></returns>
        double GetRateRa(DriveRates rate, TrackingMode mode);

        void SetTrackingRate(DriveRates rate, TrackingMode mode);
        void SetTrackingDec();
        void CheckRateTrackingState();
        void PulseGuide(GuideDirections dir, int duration, PulsState ps);

        /// <summary>
        /// Move Azm axis with fixed or variable rate
        /// </summary>
        /// <param name="rate">Rate (deg/sec) or fixed rate * 10</param>
        /// <param name="isFixed"></param>
        void MoveAxisAzm(double rate, bool isFixed = false);

        void StopWorking();
    }
}