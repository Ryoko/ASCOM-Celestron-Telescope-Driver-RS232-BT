namespace ASCOM.CelestronAdvancedBlueTooth.CelestroneDriver.TelescopeWorker
{
    using System;

    using ASCOM.CelestronAdvancedBlueTooth.CelestroneDriver.Utils;
    using ASCOM.DeviceInterface;

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
        public Coordinates SyncRaDecOffset { get; set; }
        public AltAzm SyncAltAzmOffset { get; set; }
        public bool HasRTC { get; private set; }

        private TelescopeSettingsProfile profile;
        //public static TelescopeProperties Properties { get { return _properties; } }

        public TelescopeProperties(ITelescopeInteraction ti, TelescopeSettingsProfile Profile)
        {
            this._ti = ti;
            this.TrackingRate = DriveRates.driveSidereal;
            this.Target = new Target();
            this.IsRateTracked = false;
            this.PulseRateAlt = this.PulseRateAzm = Const.TRACKRATE_SIDEREAL/2d;
            this.profile = Profile;
        }

        public void GetTelescopeProperties()
        {
            this.FirmwareVersion = this._ti.FirmwareVersion;
            if (this._ti.CanGetModel) this.TelescopeModel = this._ti.GetModel;
            if (this._ti.CanGetDeviceVersion) this.MotorsFirmvareVersion = this._ti.GetDeviceVersion(DeviceID.DecAltMotor);
            if (this._ti.CanGetTracking)
            {
                this.TrackingMode = this._ti.TrackingMode;
                if (this.TrackingMode >= TrackingMode.LENGTH)
                    this.TrackingMode = (TrackingMode) this.profile.TrackingMode;
            }
            else
            {
                this.TrackingMode = (TrackingMode)this.profile.TrackingMode;
            }
            this.IsTracking = this.TrackingMode > TrackingMode.AltAzm;

            this.Location = this._ti.CanWorkLocation ? 
                this._ti.TelescopeLocation 
                : this._ti.CanWorkGPS ? 
                    this._ti.GPSLocation
                    : new LatLon(this.profile.Latitude, this.profile.Longitude);
            this.TelescopeTime = this._ti.CanWorkDateTime ? this._ti.TelescopeDateTime : DateTime.Now;
            this.HasGPS = (this._ti.CanWorkGPS) && this._ti.IsGPS;
            if (this.HasGPS)
                try
                {
                    var gpsVer = _ti.GetDeviceVersion(DeviceID.GPSUnit);
                    this.HasGPS = gpsVer > 0;
                }
                catch
                {
                    this.HasGPS = false;
                }
            this.SlewState = this._ti.IsGoToInProgress ? SlewState.Slewing : SlewState.NoSlew;
            this.Elevation = this.profile.Elevation;
            this.Apperture = this.profile.Apperture;
            this.FocalLength = this.profile.Focal;
            this.ObstructionPercent = this.profile.Obstruction;
            this.AppertureArea = (Math.Pow((this.Apperture / 2), 2) * Math.PI) * ((100 - this.profile.Obstruction) / 100);

            this.RightAscensionRateOffset = 0;
            this.DeclinationRateOffset = 0;
            this.DefaultTrackingMode = this.TrackingMode > TrackingMode.Off ? this.TrackingMode : this.Location.Lat > 0 ? TrackingMode.EQN : TrackingMode.EQS;
            this.HomePozition = new AltAzm(this.profile.HomeAlt, this.profile.HomeAzm);
            this.ParkPosition = this.profile.ParkAlt.Equals(double.NaN) || this.profile.ParkAzm.Equals(double.NaN) ? null : new AltAzm(this.profile.ParkAlt, this.profile.ParkAzm);
            this.IsAtPark = this.profile.IsAtPark;
            try
            {
                var rtc = _ti.GetDeviceVersion(DeviceID.RTC);
                this.HasRTC = rtc > 0;
            }
            catch
            {
                this.HasRTC = false;
            }
            this.IsReady = true;
            this.SyncAltAzmOffset = new AltAzm(0, 0);
            this.SyncRaDecOffset = new Coordinates(0, 0);
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
                return (this.Ra != null && this.Dec != null);
            }
        }

        public bool IsAltAzm
        {
            get
            {
                return (this.Alt != null && this.Azm != null);
            }
        }
    }
}
