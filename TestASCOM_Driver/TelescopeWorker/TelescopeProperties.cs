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
        public Target Target { get; private set; }
        public double FirmwareVersion { get; set; }
        public double MotorsFirmvareVersion { get; set; }
        public TrackingMode TrackingMode { get; set; }
        public DateTime TelescopeTime { get; set; }
        public int SlewSteeleTime { get; set; }
        public bool HasGPS { get; set; }
        public DriveRates TrackingRate { get; set; }
        public bool IsTracking { get; set; }
        public TrackingMode DefaultTrackingMode { get; set; }
        /// <summary>
        /// deg/sec
        /// </summary>
        public double DeclinationRateOffset { get; set; }
        /// <summary>
        /// deg/sec
        /// </summary>
        public double RightAscensionRateOffset { get; set; }
        public bool MovingAltAxes { get; set; }
        public bool MovingAzmAxes { get; set; }
        public AltAzm ParkPosition { get; set; }
        public AltAzm HomePozition { get; set; }
        public bool IsAtPark { get; set; }
        public bool IsAtHome { get; set; }
        public bool IsRateTracked { get; set; }
        /// <summary>
        /// deg/sec
        /// </summary>
        public double PulseRateAlt { get; set; }
        /// <summary>
        /// deg/sec
        /// </summary>
        public double PulseRateAzm { get; set; }
        public AltAzm Position { get; set; }

        //public static TelescopeProperties Properties { get { return _properties; } }

        public TelescopeProperties(ITelescopeInteraction ti)
        {
            _ti = ti;
            TrackingRate = DriveRates.driveSidereal;
            Target = new Target();
            IsRateTracked = false;
            PulseRateAlt = PulseRateAzm = Const.TRACKRATE_SIDEREAL/2d;
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
            this.HomePozition = new AltAzm(Telescope.HomeAlt, Telescope.HomeAzm);
            this.ParkPosition = Telescope.ParkAlt.Equals(double.NaN) || Telescope.ParkAzm.Equals(double.NaN) ? null : new AltAzm(Telescope.ParkAlt, Telescope.ParkAzm);
            this.IsAtPark = Telescope.IsAtPark;
            this.IsReady = true;
        }
    }

    public class Target
    {
        public DMS Ra { get; set; }
        public DMS Dec { get; set; }
        public DMS Alt { get; set; }
        public DMS Azm { get; set; }

        public bool IsRaDec
        {
            get
            {
                return (Ra != null && Dec != null);
            }
        }

        public bool IsAltAzm
        {
            get
            {
                return (Alt != null && Azm != null);
            }
        }
    }
}
