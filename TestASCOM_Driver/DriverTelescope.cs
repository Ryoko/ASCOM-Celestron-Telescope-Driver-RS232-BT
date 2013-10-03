using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using ASCOM.CelestronAdvancedBlueTooth.TelescopeWorker;
using ASCOM.CelestronAdvancedBlueTooth.Utils;
using ASCOM.DeviceInterface;

namespace ASCOM.CelestronAdvancedBlueTooth
{
    public partial class Telescope : ITelescopeV3
    {
        #region ITelescope Implementation

        public void AbortSlew()
        {
            tl.LogMessage("AbortSlew", "set");
            tw.CancelGoTo();
        }

        public AlignmentModes AlignmentMode
        {
            get
            {
                tl.LogMessage("AlignmentMode Get", "Not implemented");
                return AlignmentModes.algGermanPolar;
            }
        }

        public double Altitude
        {
            get
            {
                try
                {
                    if (telescopeProperties == null || telescopeProperties.AltAzm == null) return 0;
                    var alt = telescopeProperties.AltAzm.Alt;
                    tl.LogMessage("Altitude Get - ", alt.ToString());
                    return alt;
                }
                catch (Exception err)
                {
                    tl.LogMessage("Altitude Get - ", "Error geting altitude");
                    throw new ASCOM.DriverException("Error geting altitude", err);
                }
            }
        }

        public double ApertureArea
        {
            get
            {
                var val = telescopeProperties.AppertureArea;

                tl.LogMessage("ApertureArea Get - ", val.ToString());
                return val;
            }
        }

        public double ApertureDiameter
        {
            get
            {
                tl.LogMessage("ApertureDiameter Get - ", telescopeProperties.Apperture.ToString());
                return telescopeProperties.Apperture;
            }
        }

        public bool AtHome
        {
            get
            {
                tl.LogMessage("AtHome", "Get - " + false.ToString());
                return false;
            }
        }

        public bool AtPark
        {
            get
            {
                tl.LogMessage("AtPark", "Get - " + false.ToString());
                return false;
            }
        }

        public IAxisRates AxisRates(TelescopeAxes Axis)
        {

            tl.LogMessage("AxisRates", "Get - " + Axis.ToString());
            return new AxisRates(Axis);
        }

        public double Azimuth
        {
            get
            {
                try
                {
                    if (telescopeProperties == null || telescopeProperties.AltAzm == null) return 0;
                    var azm = telescopeProperties.AltAzm.Azm;
                    tl.LogMessage("Azimuth Get", new DMS(azm).ToString(":"));
                    return azm;
                }
                catch (Exception err)
                {
                    tl.LogMessage("Azimuth Get", "Error geting altitude");
                    throw new ASCOM.DriverException("Error geting azimuth", err);
                }
            }
        }

        public bool CanFindHome
        {
            get
            {
                tl.LogMessage("CanFindHome", "Get - " + false.ToString());
                return false;
            }
        }

        public bool CanMoveAxis(TelescopeAxes Axis)
        {
            bool res = false;
           
            switch (Axis)
            {
                case TelescopeAxes.axisPrimary:
                    res = tw.CanSlewVariableRate;
                    break;
                case TelescopeAxes.axisSecondary:
                    res = tw.CanSlewVariableRate;
                    break;
                case TelescopeAxes.axisTertiary:
                    break;
                default:
                    throw new InvalidValueException("CanMoveAxis", Axis.ToString(), "0 to 2");
            }
            tl.LogMessage("CanMoveAxis " + Axis.ToString() + " Get - ", res.ToString());
            return res;
        }

        public bool CanPark
        {
            get
            {
                tl.LogMessage("CanPark", "Get - " + false.ToString());
                return false;
            }
        }

        public bool CanPulseGuide
        {
            get
            {
                tl.LogMessage("CanPulseGuide", "Get - " + false.ToString());
                return false;
            }
        }

        public bool CanSetDeclinationRate
        {
            get
            {
                tl.LogMessage("CanSetDeclinationRate", "Get - " + tw.CanSlewVariableRate.ToString());
                return tw.CanSlewVariableRate;
            }
        }

        public bool CanSetGuideRates
        {
            get
            {
                tl.LogMessage("CanSetGuideRates", "Get - " + false.ToString());
                return false;
            }
        }

        public bool CanSetPark
        {
            get
            {
                tl.LogMessage("CanSetPark", "Get - " + false.ToString());
                return false;
            }
        }

        public bool CanSetPierSide
        {
            get
            {
                tl.LogMessage("CanSetPierSide", "Get - " + false.ToString());
                return false;
            }
        }

        public bool CanSetRightAscensionRate
        {
            get
            {
                tl.LogMessage("CanSetRightAscensionRate", "Get - " + tw.CanSlewVariableRate.ToString());
                return tw.CanSlewVariableRate;
            }
        }

        public bool CanSetTracking
        {
            get
            {
                tl.LogMessage("CanSetTracking", "Get - " + false.ToString());
                return tw.CanSetTracking;
            }
        }

        public bool CanSlew
        {
            get
            {
                tl.LogMessage("CanSlew", "Get - " + false.ToString());
                return true;
            }
        }

        public bool CanSlewAltAz
        {
            get
            {
                tl.LogMessage("CanSlewAltAz", "Get - " + false.ToString());
                return true;
            }
        }

        public bool CanSlewAltAzAsync
        {
            get
            {
                tl.LogMessage("CanSlewAltAzAsync", "Get - " + false.ToString());
                return true;
            }
        }

        public bool CanSlewAsync
        {
            get
            {
                tl.LogMessage("CanSlewAsync", "Get - " + false.ToString());
                return true;
            }
        }

        public bool CanSync
        {
            get
            {
                tl.LogMessage("CanSync", "Get - " + false.ToString());
                return tw.CanSyncRaDec;
            }
        }

        public bool CanSyncAltAz
        {
            get
            {
                tl.LogMessage("CanSyncAltAz", "Get - " + false.ToString());
                return tw.CanSyncRaDec;
            }
        }

        public bool CanUnpark
        {
            get
            {
                tl.LogMessage("CanUnpark", "Get - " + false.ToString());
                return false;
            }
        }

        public double Declination
        {
            get
            {
                try
                {
                    if (telescopeProperties == null || telescopeProperties.RaDec == null) return 0;
                    var dec = telescopeProperties.RaDec.Dec;
                    tl.LogMessage("Declination Get", new DMS(dec).ToString());
                    return dec;
                }
                catch (Exception err)
                {
                    tl.LogMessage("Declination Get", "Error geting declination");
                    throw new ASCOM.DriverException("Error geting declination", err);
                }
            }
        }

//        private double declinationRate = double.NaN;
        public double DeclinationRate
        {
            get
            {
                var val = 0d;
                if (CanSetDeclinationRate)
                {
                    val = telescopeProperties.DeclinationRateOffset;
                }
                tl.LogMessage("DeclinationRate Get - ", val.ToString());
                return val;
            }
            set
            {
                if (CanSetDeclinationRate && tw.CanSlewHighRate)
                {
                    //var val = Math.Abs(value)*Const.SiderealRate;
                    tw.SlewHighRate(SlewAxes.DecAlt, value * 3600);
                    TelescopeProperties.DeclinationRateOffset = value;
                    tl.LogMessage("DeclinationRate Set", value.ToString());
                }
//                if (tw.CanSlewVariableRate) throw new ASCOM.InvalidOperationException("Error setting declination rate");
            }
        }

        public PierSide DestinationSideOfPier(double RightAscension, double Declination)
        {
            tl.LogMessage("DestinationSideOfPier Get", "Not implemented");
            throw new ASCOM.PropertyNotImplementedException("DestinationSideOfPier", false);
        }

        public bool DoesRefraction
        {
            get
            {

                tl.LogMessage("DoesRefraction Get", "Not implemented");
                throw new ASCOM.PropertyNotImplementedException("DoesRefraction", false);
            }
            set
            {
                tl.LogMessage("DoesRefraction Set", "Not implemented");
                throw new ASCOM.PropertyNotImplementedException("DoesRefraction", true);
            }
        }

        public EquatorialCoordinateType EquatorialSystem
        {
            get
            {
                EquatorialCoordinateType equatorialSystem = EquatorialCoordinateType.equLocalTopocentric;
                tl.LogMessage("DeclinationRate", "Get - " + equatorialSystem.ToString());
                return equatorialSystem;
            }
        }

        public void FindHome()
        {
            tl.LogMessage("FindHome", "Not implemented");
            throw new ASCOM.MethodNotImplementedException("FindHome");
        }

        public double FocalLength
        {
            get
            {
                var fLen = telescopeProperties.FocalLength;
                tl.LogMessage("FocalLength Get", fLen.ToString());
                return fLen;
            }
        }

        public double GuideRateDeclination
        {
            get
            {
                tl.LogMessage("GuideRateDeclination Get", "Not implemented");
                throw new ASCOM.PropertyNotImplementedException("GuideRateDeclination", false);
            }
            set
            {
                tl.LogMessage("GuideRateDeclination Set", "Not implemented");
                throw new ASCOM.PropertyNotImplementedException("GuideRateDeclination", true);
            }
        }

        public double GuideRateRightAscension
        {
            get
            {
                tl.LogMessage("GuideRateRightAscension Get", "Not implemented");
                throw new ASCOM.PropertyNotImplementedException("GuideRateRightAscension", false);
            }
            set
            {
                tl.LogMessage("GuideRateRightAscension Set", "Not implemented");
                throw new ASCOM.PropertyNotImplementedException("GuideRateRightAscension", true);
            }
        }

        public bool IsPulseGuiding
        {
            get
            {
                tl.LogMessage("IsPulseGuiding Get", "Not implemented");
                return false;
                throw new ASCOM.PropertyNotImplementedException("IsPulseGuiding", false);
            }
        }

        /// <summary>
        /// Set rate for specified axis
        /// </summary>
        /// <param name="Axis">Axis to specified rate</param>
        /// <param name="Rate">Rate of movement in deg per sec</param>
        public void MoveAxis(TelescopeAxes Axis, double Rate)
        {
            if (!CheckRate(Axis, Math.Abs(Rate))) throw new ArgumentOutOfRangeException("Rate for MovingRate is Out of range");
            SlewAxes a = Axis == TelescopeAxes.axisPrimary ? SlewAxes.RaAzm : SlewAxes.DecAlt;
            telescopeWorker.MoveAxis(a, Rate);
            tl.LogMessage("MoveAxis ", Axis + "to rate " + Rate);
            //throw new ASCOM.MethodNotImplementedException("MoveAxis");
        }

        public void Park()
        {
            tl.LogMessage("Park", "Not implemented");
            throw new ASCOM.MethodNotImplementedException("Park");
        }

        public void PulseGuide(GuideDirections Direction, int Duration)
        {
            tl.LogMessage("PulseGuide", "Not implemented");
            throw new ASCOM.MethodNotImplementedException("PulseGuide");
        }

        public double RightAscension
        {
            get
            {
                try
                {
                    if (telescopeProperties == null || telescopeProperties.RaDec == null) return 0;
                    var val = telescopeProperties.RaDec.Ra;
                    tl.LogMessage("RightAscension Get", new DMS(val).ToString());
                    return val;
                }
                catch (Exception err)
                {
                    tl.LogMessage("RightAscension Get", "Error getting RightAscension");
                    throw new ASCOM.DriverException("Error getting RightAscension");
                }
            }
        }

        public double RightAscensionRate
        {
            get
            {
                double val = 0;
                if (CanSetRightAscensionRate)
                {
                    val = TelescopeProperties.RightAscensionRateOffset;
                }
                tl.LogMessage("AscensionRateRate Get - ", val.ToString());
                return val;
            }
            set
            {
                if (CanSetRightAscensionRate && tw.CanSlewVariableRate)
                {
                    tl.LogMessage("Right Ascesion Rate Set - ", value.ToString());
                    TelescopeProperties.RightAscensionRateOffset = value;
                    var rate = (Const.SiderealRateDegPerSec + value * Const.SiderealRate) * ((TelescopeProperties.TrackingMode == TrackingMode.EQS) ? -1 : 1);
                    var val =  rate * 3600;
                    tw.SlewHighRate(SlewAxes.RaAzm, val);
                }
            }
        }

        public void SetPark()
        {
            tl.LogMessage("SetPark", "Not implemented");
            throw new ASCOM.MethodNotImplementedException("SetPark");
        }

        public PierSide SideOfPier
        {
            get
            {
                tl.LogMessage("SideOfPier Get", "Not implemented");
                throw new ASCOM.PropertyNotImplementedException("SideOfPier", false);
            }
            set
            {
                tl.LogMessage("SideOfPier Set", "Not implemented");
                throw new ASCOM.PropertyNotImplementedException("SideOfPier", true);
            }
        }

        public double SiderealTime
        {
            get
            {
                double siderealTime = (18.697374558 +
                                       24.065709824419081*(utilities.DateLocalToJulian(DateTime.Now) - 2451545.0))%24.0;
                tl.LogMessage("SiderealTime", "Get - " + siderealTime.ToString());
                return siderealTime;
            }
        }

        public double SiteElevation
        {
            get
            {
                var val = telescopeProperties.Elevation;
                tl.LogMessage("SiteElevation Get", val.ToString());
                return val;
            }
            set
            {
                tl.LogMessage("SiteElevation Set", value.ToString());
                telescopeProperties.Elevation = value;
                Telescope.elevation = (int) value;
                WriteProfile();
            }
        }

        public double SiteLatitude
        {
            get
            {
                if (telescopeProperties == null || telescopeProperties.Location == null) return 0;
                var val = telescopeProperties.Location.Lat;
                tl.LogMessage("SiteLatitude Get", new DMS(val).ToString(":"));
                return val;
            }
            set
            {
                if (tw.CanWorkLocation)
                {
                    if (telescopeProperties == null || telescopeProperties.Location == null) return;
                    telescopeProperties.Location.Lat = value;
                    tw.TelescopeLocation = telescopeProperties.Location;
                }
                tl.LogMessage("SiteLatitude Set", new DMS(value).ToString(":"));
                Telescope.latitude = value;
                WriteProfile();
            }
        }

        public double SiteLongitude
        {
            get
            {
                if (telescopeProperties == null || telescopeProperties.Location == null) return 0;
                var val = telescopeProperties.Location.Lon;
                tl.LogMessage("SiteLongitude Get", new DMS(val).ToString(":"));
                return val;
            }
            set
            {
                if (tw.CanWorkLocation)
                {
                    if (telescopeProperties == null || telescopeProperties.Location == null) return;
                    telescopeProperties.Location.Lon = value;
                    tw.TelescopeLocation = telescopeProperties.Location;
                }
                tl.LogMessage("SiteLongitude Set", new DMS(value).ToString(":"));
                Telescope.longitude = value;
                WriteProfile();
            }
        }

        public short SlewSettleTime
        {
            get
            {
                var val = (short)(telescopeProperties.SlewSteeleTime/1000); 
                tl.LogMessage("SlewSettleTime Get", val.ToString());
                return val;
            }
            set
            {
                telescopeProperties.SlewSteeleTime = value*1000;
                tl.LogMessage("SlewSettleTime Set", value.ToString());
            }
        }

        public void SlewToAltAz(double Azimuth, double Altitude)
        {
            
            SlewToAltAzAsync(Azimuth, Altitude);
            while (true)
            {
                Thread.Sleep(100);
                if (!telescopeWorker.IsSlewing) break;
            }
            tl.LogMessage("SlewingToAltAz", string.Format("Alt:{0}, Azm:{1}", Altitude, Azimuth));
        }

        public void SlewToAltAzAsync(double Azimuth, double Altitude)
        {
            if (telescopeWorker.Slew(new AltAzm(Altitude, Azimuth)))
            {
                tl.LogMessage("SlewingToAltAz", string.Format("Alt:{0}, Azm:{1}", Altitude, Azimuth));
            }
            else
            {
                tl.LogMessage("Error SlewToAltAz", string.Format("Alt:{0}, Azm:{1}", Altitude, Azimuth));
            }
        }

        public void SlewToCoordinates(double RightAscension, double Declination)
        {
            SlewToCoordinatesAsync(RightAscension, Declination);
            while (true)
            {
                Thread.Sleep(100);
                if(!telescopeWorker.IsSlewing) break;
            }
            tl.LogMessage("Slewed ToToCoordinates Complete", string.Format("RA:{0}, Dec:{1}", new DMS(RightAscension).ToString(), new DMS(Declination).ToString()));
        }

        public void SlewToCoordinatesAsync(double RightAscension, double Declination)
        {
            if (telescopeWorker.Slew(new Coordinates(RightAscension, Declination)))
            {
                tl.LogMessage("Slewing ToToCoordinates", string.Format("RA:{0}, Dec:{1}", new DMS(RightAscension).ToString(), new DMS(Declination).ToString()));
            }
            else
            {
                tl.LogMessage("Error SlewToToCoordinates", string.Format("RA:{0}, Dec:{1}", new DMS(RightAscension).ToString(), new DMS(Declination).ToString()));
            }
        }

        public void SlewToTarget()
        {

            tl.LogMessage("SlewToTarget", "Not implemented");
            throw new ASCOM.MethodNotImplementedException("SlewToTarget");
        }

        public void SlewToTargetAsync()
        {
            tl.LogMessage("SlewToTargetAsync", "Not implemented");
            throw new ASCOM.MethodNotImplementedException("SlewToTargetAsync");
        }

        public bool Slewing
        {
            get
            {
                return telescopeWorker.IsSlewing;
            }
        }

        public void SyncToAltAz(double Azimuth, double Altitude)
        {
            tl.LogMessage("SyncToAltAz", "Not implemented");
            throw new ASCOM.MethodNotImplementedException("SyncToAltAz");
        }

        public void SyncToCoordinates(double RightAscension, double Declination)
        {
            tl.LogMessage("SyncToCoordinates", "Not implemented");
            throw new ASCOM.MethodNotImplementedException("SyncToCoordinates");
        }

        public void SyncToTarget()
        {
            tl.LogMessage("SyncToTarget", "Not implemented");
            throw new ASCOM.MethodNotImplementedException("SyncToTarget");
        }

        public double TargetDeclination
        {
            get
            {
                if (target == null) throw new ASCOM.ValueNotSetException("Target not set");
                tl.LogMessage("TargetDeclination Get", target.Dec.ToString());
                return target.Dec;
            }
            set
            {
                tl.LogMessage("TargetDeclination Set", value.ToString());
                target.Dec = value;
            }
        }

        public double TargetRightAscension
        {
            get
            {
                if (target == null) throw new ASCOM.ValueNotSetException("Target not set");
                tl.LogMessage("TargetRightAscension Get", target.Ra.ToString());
                return target.Dec;
            }
            set
            {
                tl.LogMessage("TargetRightAscension Set", value.ToString());
                target.Ra = value;
            }
        }

        //private TrackingMode lastTrackingMode = TrackingMode.Unknown;

        public bool Tracking
        {
            get
            {
                if (!tw.CanGetTracking) throw new NotSupportedException("Getting tracking mode is not supported");
//                var tm = telescopeProperties.TrackingMode;
                bool tr = (telescopeProperties.TrackingMode > TrackingMode.Off);
                tl.LogMessage("Tracking Mode - Get:", telescopeProperties.TrackingMode.ToString());
                return tr;
            }

            set
            {
                //TrackingMode trMode = TrackingMode.Unknown;
                telescopeWorker.SetTracking(value);
                tl.LogMessage("Tracking Set", telescopeProperties.TrackingMode.ToString());
            }
        }

        //private DriveRates currentDriveRate = DriveRates.driveSidereal; 
        public DriveRates TrackingRate
        {
            get
            {
                tl.LogMessage("TrackingRate Get", telescopeProperties.TrackingRate.ToString());
                return telescopeProperties.TrackingRate;
            }
            set
            {
                if (tw.CanSetTrackingRates)
                {
                    telescopeProperties.TrackingRate = value;
                    tw.SetTrackingRate(value, telescopeProperties.TrackingMode);
                    tl.LogMessage("TrackingRate Set", value.ToString());
                }
                throw new NotSupportedException("TrackingRate");
            }
        }

        public ITrackingRates TrackingRates
        {
            get
            {
                ITrackingRates trackingRates = new TrackingRates();
                tl.LogMessage("TrackingRates", "Get - ");
                foreach (DriveRates driveRate in trackingRates)
                {
                    tl.LogMessage("TrackingRates", "Get - " + driveRate.ToString());
                }
                return trackingRates;
            }
        }

        public DateTime UTCDate
        {
            get
            {
                DateTime utcDate;
                if (tw.CanWorkDateTime)
                {
                    try
                    {
                        utcDate = tw.TelescopeDateTime.ToUniversalTime();
                        tl.LogMessage("Telescope UTCDate", "Get - " + utcDate.ToString("MM/dd/yy HH:mm:ss"));
                        return utcDate;
                    }
                    catch (Exception err)
                    {
                        throw new DriverException("Error gettingtime from telescope", err);
                    }
                }
                utcDate = DateTime.UtcNow;
                tl.LogMessage("Computer UTCDate", "Get - " + utcDate.ToString("MM/dd/yy HH:mm:ss"));
                return utcDate;

            }
            set
            {
                
                if (tw.CanWorkDateTime)
                {
                    try
                    {
                        tw.TelescopeDateTime = DateTime.SpecifyKind(value, DateTimeKind.Utc);
                        tl.LogMessage("Telescope UTCDate", "Set - " + value.ToString("MM/dd/yy HH:mm:ss"));
                        return;
                    }
                    catch (Exception err)
                    {
                        throw new DriverException("Error settingtime to telescope", err);
                    }
                }
                tl.LogMessage("UTCDate Set", "Not implemented");
                throw new ASCOM.PropertyNotImplementedException("UTCDate", true);
            }
        }

        public void Unpark()
        {
            tl.LogMessage("Unpark", "Not implemented");
            throw new ASCOM.MethodNotImplementedException("Unpark");
        }

        #endregion

        private bool CheckRate(TelescopeAxes axis, double rate)
        {
            foreach (IRate trackingRate in AxisRates(axis))
            {
                if (rate >= trackingRate.Minimum && rate <= trackingRate.Maximum) return true;
            }
            return false;
        }
    }
}
