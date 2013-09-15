using System;
using ASCOM.CelestronAdvancedBlueTooth.Utils;

namespace ASCOM.CelestronAdvancedBlueTooth.TelescopeWorker
{
    public enum Direction { Positive, Negative };
    public enum SlewAxes { RaAzm, DecAlt };
    public enum DeviceID { RaAzmMotor = 16, DecAltMotor = 17, GPSUnit = 176, RTC = 178}
    public enum TelescopeModel { GPSSeries = 1, iSeries = 3, iSeriesSE, CGE, AdvancedGT, SLT, CPC = 9, GT, SE45, SE68 }
    public enum TrackingMode { Off, AltAzm, EQN, EQS };

    interface ITelescopeInteraction
    {
        //ICelestroneTelescopeWorker(IDeviceWorker dw);

        AltAzm AltAzm { get; set; }
        Coordinates RaDec { get; set; }
        void SyncAltAz(AltAzm coordinates);
        void SyncRaDec(Coordinates coordinates);
        TrackingMode TrackingMode { get; set; }
        void SlewFixedRate(Direction dir, SlewAxes axis, int rate);
        /// <summary>
        /// Slew with Variable Rate
        /// </summary>
        /// <param name="dir">Direction</param>
        /// <param name="axis">Axis</param>
        /// <param name="rate">Rate (arcseconds/second)</param>
        void SlewVariableRate(Direction dir, SlewAxes axis, double rate);
        DateTime TelescopeDateTime { get; set; }
        LatLon TelescopeLocation { get; set; }
        bool IsGPS { get; }
        DateTime GpsDateTime { get; }
        LatLon GPSLocation { get; }
        DateTime RTCDateTime { get; set; }
        double FirmwareVersion { get; }
        double GetDeviceVersion(DeviceID device);
        TelescopeModel GetModel { get; }
        bool IsAlignmentComplete { get; }
        bool IsGoToInProgress { get; }
        void CancelGoTo();

        double VersionRequired { get; }
        bool CanSyncAltAzm { get; }
        bool CanSyncRaDec { get; }
        bool CanSetTracking { get; }
        bool CanGetTracking { get; }
        bool CanSlewFixedRate { get; }
        bool CanSlewVariableRate { get; }
        bool CanWorkDateTime { get; }
        bool CanWorkLocation { get; }
        bool CanWorkGPS { get; }
        bool CanGetRTC { get; }
        bool CanSetRTC { get; }
        bool CanGetModel { get; }
        bool CanGetDeviceVersion { get; }



    }
}
