using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASCOM.CelestronAdvancedBlueTooth.Utils;
using ASCOM.DeviceInterface;

namespace ASCOM.CelestronAdvancedBlueTooth.TelescopeWorker
{
    public enum SlewState { InitSlew = -1, NoSlew = 0, Slewing, SlewRest, MoveAxis };
    public enum TelescopeType { Unknown, NextStar, NextStar58, GT, Ultima }


    public class TelescopeProperties
    {
        //private static readonly TelescopeProperties _properties = new TelescopeProperties();
        private ITelescopeInteraction _ti;

        public bool IsReady { get; private set; }
        public string TelescopeName { get; set; }
        public string TelescopeType { get; set; }
        public TelescopeModel TelescopeModel { get; set; }
        public Coordinates RaDec { get; set; }
        public AltAzm AltAzm { get; set; }
        public DateTime DateTime { get; set; }
        public SlewState SlewState { get; set; }
        public LatLon Location { get; set; }
        public double Elevation { get; set; }
        public double Apperture { get; set; }
        public double FocalLength { get; set; }
        public double AppertureArea { get; set; }
        public double ObstructionPercent { get; set; }
        public Coordinates TargetCoordinates { get; set; }
        public AltAzm TargetAltAzm { get; set; }
        public double FirmwareVersion { get; set; }
        public double MotorsFirmvareVersion { get; set; }
        public TrackingMode TrackingMode { get; set; }
        public DateTime TelescopeTime { get; set; }
        public int SlewSteeleTime { get; set; }
        public bool HasGPS { get; set; }
        public DriveRates TrackingRate { get; set; }
        public bool IsTracking { get; set; }
        public TrackingMode DefaultTrackingMode { get; set; }
        public double DeclinationRateOffset { get; set; }
        public double RightAscensionRateOffset { get; set; }
        public bool MovingAltAxes { get; set; }
        public bool MovingAzmAxes { get; set; }

        //public static TelescopeProperties Properties { get { return _properties; } }

        public TelescopeProperties(ITelescopeInteraction ti)
        {
            _ti = ti;
            TrackingRate = DriveRates.driveSidereal;
        }

        public void GetTelescopeProperties()
        {
            this.FirmwareVersion = _ti.FirmwareVersion;
            if (_ti.CanGetModel) this.TelescopeModel = _ti.GetModel;
            if (_ti.CanGetDeviceVersion) this.MotorsFirmvareVersion = _ti.GetDeviceVersion(DeviceID.DecAltMotor);
            if (_ti.CanGetTracking)
            {
                this.TrackingMode = _ti.TrackingMode;
            }
            else
            {
                this.TrackingMode = (TrackingMode)Telescope.trackingMode;
            }
            this.IsTracking = TrackingMode > TrackingMode.AltAzm;

            this.Location = _ti.CanWorkLocation ? 
                _ti.TelescopeLocation 
                : _ti.CanWorkGPS ? 
                    _ti.GPSLocation 
                    : new LatLon(Telescope.latitude, Telescope.longitude);
            this.TelescopeTime = _ti.CanWorkDateTime ? _ti.TelescopeDateTime : DateTime.Now;
            this.HasGPS = (_ti.CanWorkGPS) && _ti.IsGPS;

            this.SlewState = _ti.IsGoToInProgress ? SlewState.Slewing : SlewState.NoSlew;
            this.Elevation = Telescope.elevation;
            this.Apperture = Telescope.apperture;
            this.FocalLength = Telescope.focal;
            this.ObstructionPercent = Telescope.obstruction;
            this.AppertureArea = (Math.Pow((Apperture / 2), 2) * Math.PI) * ((100 - Telescope.obstruction) / 100);

            this.RightAscensionRateOffset = 0;
            this.DeclinationRateOffset = 0;
            this.DefaultTrackingMode = TrackingMode > TrackingMode.Off ? TrackingMode : Location.Lat > 0 ? TrackingMode.EQN : TrackingMode.EQS;
            this.IsReady = true;
        }
    }
}
