namespace CelestroneDriver.TelescopeWorker
{
    using ASCOM.DeviceInterface;

    internal interface ITelescopeWorkerOperations
    {
        void SegProperties(TelescopeProperties telescopeProperties, ITelescopeInteraction telescopeInteraction);
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
        /// Move axis with fixed or variable rate
        /// </summary>
        /// <param name="axis">Slewing axis</param>
        /// <param name="rate">Rate (deg/sec) or fixed rate * 10</param>
        /// <param name="isFixed"></param>
        void MoveAxis(SlewAxes axis, double rate, bool isFixed = false);

        void StopWorking();
    }
}