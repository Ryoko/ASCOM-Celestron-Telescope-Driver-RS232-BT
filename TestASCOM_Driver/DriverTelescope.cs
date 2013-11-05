using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using ASCOM.DeviceInterface;

namespace ASCOM.CelestronAdvancedBlueTooth
{
    using ASCOM.CelestronAdvancedBlueTooth.CelestroneDriver.TelescopeWorker;
    using ASCOM.CelestronAdvancedBlueTooth.CelestroneDriver.Utils;

    public partial class Telescope : ITelescopeV3
    {
        #region ITelescope Implementation

        public void AbortSlew()
        {
            tl.LogMessage("AbortSlew", "set");
            telescopeInteraction.CancelGoTo();
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
                    if (telescopeProperties == null || telescopeProperties.AltAzm == null || !telescopeProperties.IsReady) return 0;
                    var alt = telescopeProperties.AltAzm.Alt - telescopeProperties.SyncAltAzmOffset.Alt;
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
                    if (telescopeProperties == null || telescopeProperties.AltAzm == null || !telescopeProperties.IsReady) return 0;
                    var azm = telescopeProperties.AltAzm.Azm - telescopeProperties.SyncAltAzmOffset.Azm;
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

        public bool CanMoveAxis(TelescopeAxes Axis)
        {
            bool res = false;
            if (telescopeProperties.IsAtPark) return false;
            switch (Axis)
            {
                case TelescopeAxes.axisPrimary:
                    res = telescopeInteraction.CanSlewVariableRate;
                    break;
                case TelescopeAxes.axisSecondary:
                    res = telescopeInteraction.CanSlewVariableRate;
                    break;
                case TelescopeAxes.axisTertiary:
                    break;
                default:
                    throw new InvalidValueException("CanMoveAxis", Axis.ToString(), "0 to 2");
            }
            tl.LogMessage("CanMoveAxis " + Axis.ToString() + " Get - ", res.ToString());
            return res;
        }

        public bool CanPulseGuide
        {
            get
            {
                if (telescopeProperties.IsAtPark) return false;
                var res = telescopeInteraction.CanSlewHighRate;
                tl.LogMessage("CanPulseGuide", "Get - " + res.ToString());
                return res;
            }
        }

        public bool CanSetDeclinationRate
        {
            get
            {
                tl.LogMessage("CanSetDeclinationRate", "Get - " + telescopeInteraction.CanSlewVariableRate.ToString());
                return telescopeInteraction.CanSlewVariableRate;
            }
        }

        public bool CanSetGuideRates
        {
            get
            {
                tl.LogMessage("CanSetGuideRates", "Get - " + telescopeInteraction.CanSlewVariableRate.ToString());
                return telescopeInteraction.CanSlewVariableRate;
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
                tl.LogMessage("CanSetRightAscensionRate", "Get - " + telescopeInteraction.CanSlewVariableRate.ToString());
                return telescopeInteraction.CanSlewVariableRate;
            }
        }

        public bool CanSetTracking
        {
            get
            {
                tl.LogMessage("CanSetTracking", "Get - " + telescopeInteraction.CanSetTracking.ToString());
                return telescopeInteraction.CanSetTracking;
            }
        }

        public bool CanSlew
        {
            get
            {
                if (telescopeProperties.IsAtPark) return false;
                tl.LogMessage("CanSlew", "Get - " + true.ToString());
                return true;
            }
        }

        public bool CanSlewAltAz
        {
            get
            {
                if (telescopeProperties.IsAtPark) return false;
                tl.LogMessage("CanSlewAltAz", "Get - " + true.ToString());
                return true;
            }
        }

        public bool CanSlewAltAzAsync
        {
            get
            {
                if (telescopeProperties.IsAtPark) return false;
                tl.LogMessage("CanSlewAltAzAsync", "Get - " + true.ToString());
                return true;
            }
        }

        public bool CanSlewAsync
        {
            get
            {
                if (telescopeProperties.IsAtPark) return false;
                tl.LogMessage("CanSlewAsync", "Get - " + true.ToString());
                return true;
            }
        }

        public bool CanSync
        {
            get
            {
                if (telescopeProperties.IsAtPark) return false;
                var res = true;// telescopeInteraction.CanSyncRaDec;
                tl.LogMessage("CanSync", "Get - " + res.ToString());
                return res;
            }
        }

        public bool CanSyncAltAz
        {
            get
            {
                if (telescopeProperties.IsAtPark) return false;
                var res = true;// telescopeInteraction.CanSyncAltAzm;
                tl.LogMessage("CanSyncAltAz", "Get - " + res.ToString());
                return res;
            }
        }



        public double Declination
        {
            get
            {
                try
                {
                    if (telescopeProperties == null || telescopeProperties.RaDec == null || !telescopeProperties.IsReady) return 0;
                    var dec = telescopeProperties.RaDec.Dec - telescopeProperties.SyncRaDecOffset.Dec;
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
                if (CanSetDeclinationRate && telescopeInteraction.CanSlewHighRate)
                {
                    //var val = Math.Abs(value)*Const.SiderealRate;
                    //telescopeInteraction.SlewHighRate(SlewAxes.DecAlt, value * 3600);
                    TelescopeProperties.DeclinationRateOffset = value;
                    telescopeWorker.SetTrackingDec();
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
                
                tl.LogMessage("GuideRateDeclination Get", telescopeProperties.PulseRateAzm.ToString());
                return telescopeProperties.PulseRateAzm;
            }
            set
            {
                var rate = telescopeProperties.DeclinationRateOffset + value;
                if (!CheckRate(TelescopeAxes.axisPrimary, rate)) throw new ArgumentOutOfRangeException("value for Dec guide rate out of range");
                tl.LogMessage("GuideRateDeclination Set", (value).ToString());
                telescopeProperties.PulseRateAlt = value;
            }
        }

        public double GuideRateRightAscension
        {
            get
            {
                
                tl.LogMessage("GuideRateRightAscension Get", telescopeProperties.PulseRateAzm.ToString());
                return telescopeProperties.PulseRateAzm;
            }
            set
            {
                var rate =
                    telescopeWorker.GetRateRa(telescopeProperties.TrackingRate, telescopeProperties.TrackingMode) +
                    value;
                if (!CheckRate(TelescopeAxes.axisPrimary, rate)) throw new ArgumentOutOfRangeException("value for RA guide rate out of range");
                tl.LogMessage("GuideRateRightAscension Set", (value).ToString());
                telescopeProperties.PulseRateAzm = value;
            }
        }

        public bool IsPulseGuiding
        {
            get
            {
                bool res = telescopeWorker.IsPulsGuiding();
                tl.LogMessage("IsPulseGuiding Get", res.ToString());
                return res;
                //throw new ASCOM.PropertyNotImplementedException("IsPulseGuiding", false);
            }
        }

        /// <summary>
        /// Set rate for specified axis (deg/sec)
        /// </summary>
        /// <param name="Axis">Axis to specified rate</param>
        /// <param name="Rate">Rate of movement in deg per sec</param>
        public void MoveAxis(TelescopeAxes Axis, double Rate)
        {
            telescopeWorker.CheckPark();
            SlewAxes axs = Axis == TelescopeAxes.axisPrimary ? SlewAxes.RaAzm : SlewAxes.DecAlt;
            if (Math.Abs(Rate) > 10)
            {
                var rate = (int) (Rate/10);
                var absRate = Math.Abs(rate);
                if (absRate > 9)
                    throw new ArgumentOutOfRangeException("Fixed rate " + absRate + " for MovingRate is Out of range");
                telescopeWorker.MoveAxis(axs, rate, true);
                tl.LogMessage("MoveAxis ", Axis + "to fixed rate " + rate);
            }
            else
            {
                if (!CheckRate(Axis, Math.Abs(Rate)))
                    throw new ArgumentOutOfRangeException("Rate " + Rate.ToString() + " for MovingRate is Out of range");
                telescopeWorker.MoveAxis(axs, Rate);
                tl.LogMessage("MoveAxis ", Axis + "to rate " + Rate);
            }
            //throw new ASCOM.MethodNotImplementedException("MoveAxis");
        }

        public void PulseGuide(GuideDirections Direction, int Duration)
        {
            telescopeWorker.CheckPark();
            if (!CanPulseGuide || !telescopeInteraction.CanSlewHighRate) throw new NotSupportedException("Puls guiding is not supported");
            telescopeWorker.PulseGuide(Direction, Duration);
            tl.LogMessage("PulseGuide", string.Format("{0} {1}sec", Direction.ToString(), Duration));
        }

        public double RightAscension
        {
            get
            {
                try
                {
                    if (telescopeProperties == null || telescopeProperties.RaDec == null || !telescopeProperties.IsReady) return 0;
                    var val = telescopeProperties.RaDec.Ra - telescopeProperties.SyncRaDecOffset.Ra;
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
                if (CanSetRightAscensionRate && telescopeInteraction.CanSlewVariableRate)
                {
                    tl.LogMessage("Right Ascesion Rate Set - ", value.ToString());
                    TelescopeProperties.RightAscensionRateOffset = value;
                    //var rate = (Const.SiderealRateDegPerSec + value * Const.SiderealRate) * ((TelescopeProperties.TrackingMode == TrackingMode.EQS) ? -1 : 1);
                    //var val =  rate * 3600;
                    //telescopeInteraction.SlewHighRate(SlewAxes.RaAzm, val);
                    telescopeWorker.SetTrackingRate(telescopeProperties.TrackingRate, telescopeProperties.TrackingMode);
                }
            }
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
                profile.elevation = (int)value;
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
                if (telescopeInteraction.CanWorkLocation)
                {
                    if (telescopeProperties == null || telescopeProperties.Location == null) return;
                    telescopeProperties.Location.Lat = value;
                    telescopeInteraction.TelescopeLocation = telescopeProperties.Location;
                }
                tl.LogMessage("SiteLatitude Set", new DMS(value).ToString(":"));
                profile.latitude = value;
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
                if (telescopeInteraction.CanWorkLocation)
                {
                    if (telescopeProperties == null || telescopeProperties.Location == null) return;
                    telescopeProperties.Location.Lon = value;
                    telescopeInteraction.TelescopeLocation = telescopeProperties.Location;
                }
                tl.LogMessage("SiteLongitude Set", new DMS(value).ToString(":"));
                profile.longitude = value;
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
            telescopeWorker.CheckPark();
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
            telescopeWorker.CheckPark();
            var altaz = new AltAzm(Altitude + telescopeProperties.SyncAltAzmOffset.Alt, Azimuth + telescopeProperties.SyncAltAzmOffset.Azm);
            if (telescopeWorker.Slew(altaz))
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
            telescopeWorker.CheckPark();
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
            telescopeWorker.CheckPark();
            var coord = new Coordinates(RightAscension + telescopeProperties.SyncRaDecOffset.Ra, Declination + telescopeProperties.SyncRaDecOffset.Dec);
            if (telescopeWorker.Slew(coord))
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
            telescopeWorker.CheckPark();
            if (!telescopeProperties.Target.IsRaDec) throw new Astrometry.Exceptions.ValueNotSetException("Target coordinate not setted");
            tl.LogMessage("SlewToTarget", 
                string.Format("Target Ra:{0} Dec:{1}", telescopeProperties.Target.Ra.ToString(":"), telescopeProperties.Target.Dec.ToString(":")));
            SlewToCoordinates((double)telescopeProperties.Target.Ra.Deg, (double)telescopeProperties.Target.Dec.Deg);
        }

        public void SlewToTargetAsync()
        {
            telescopeWorker.CheckPark();
            if (!telescopeProperties.Target.IsRaDec) throw new Astrometry.Exceptions.ValueNotSetException("Target coordinate not setted");
            tl.LogMessage("SlewToTargetAsync",
                string.Format("Target Ra:{0} Dec:{1}", telescopeProperties.Target.Ra.ToString(":"), telescopeProperties.Target.Dec.ToString(":")));
            SlewToCoordinatesAsync((double)telescopeProperties.Target.Ra.Deg, (double)telescopeProperties.Target.Dec.Deg);
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
            telescopeWorker.CheckPark();
            if (telescopeInteraction.CanSyncAltAzm)
            {
                telescopeInteraction.SyncAltAz(new AltAzm(Altitude, Azimuth));
            }
            else
            {
                // rc = 3 sc = 4 ; c = 3; cc = c - dc => dc = -1; dc = rc - dc => cc = 4; rc = cc + dc 
                telescopeProperties.SyncAltAzmOffset.Alt = telescopeProperties.AltAzm.Alt - Altitude;
                telescopeProperties.SyncAltAzmOffset.Azm = telescopeProperties.AltAzm.Azm - Azimuth;
            }
            tl.LogMessage("SyncToAltAz", "Setted");
        }

        public void SyncToCoordinates(double RightAscension, double Declination)
        {
            telescopeWorker.CheckPark();
            if (telescopeInteraction.CanSyncRaDec)
            {
                telescopeInteraction.SyncRaDec(new Coordinates(RightAscension, Declination));
            }
            else
            {
                // rc = 3 sc = 4 ; c = 3; cc = c - dc => dc = -1; dc = rc - dc => cc = 4; rc = cc + dc 
                telescopeProperties.SyncRaDecOffset.Ra = telescopeProperties.RaDec.Ra - RightAscension;
                telescopeProperties.SyncRaDecOffset.Dec = telescopeProperties.RaDec.Dec - Declination;
            }
            tl.LogMessage("SyncToCoordinates", "Synced");
        }

        public void SyncToTarget()
        {
            telescopeWorker.CheckPark();
            if (!telescopeProperties.Target.IsRaDec) throw new ASCOM.ValueNotSetException("Target not set");

            if (telescopeInteraction.CanSyncRaDec)
            {
                telescopeInteraction.SyncRaDec(new Coordinates((double)telescopeProperties.Target.Ra.Deg, (double)telescopeProperties.Target.Dec.Deg));
            }
            else
            {
                // rc = 3 sc = 4 ; c = 3; cc = c - dc => dc = -1; dc = rc - dc => cc = 4; rc = cc + dc 
                telescopeProperties.SyncRaDecOffset.Ra = telescopeProperties.RaDec.Ra - (double)telescopeProperties.Target.Ra.Deg;
                telescopeProperties.SyncRaDecOffset.Dec = telescopeProperties.RaDec.Dec - (double)telescopeProperties.Target.Dec.Deg;
            }
            tl.LogMessage("SyncToTarget", "Setted");
        }

        public double TargetDeclination
        {
            get
            {
                if (telescopeProperties.Target.Dec  == null) throw new ASCOM.ValueNotSetException("Target not set");
                tl.LogMessage("TargetDeclination Get", telescopeProperties.Target.Dec.ToString(":"));
                return (double)telescopeProperties.Target.Dec.Deg;
            }
            set
            {
                var val = new DMS(value);
                tl.LogMessage("TargetDeclination Set - ", val.ToString(":"));
                telescopeProperties.Target.Dec =  val;
            }
        }

        public double TargetRightAscension
        {
            get
            {
                if (telescopeProperties.Target.Ra == null) throw new ASCOM.ValueNotSetException("Target not set");
                tl.LogMessage("TargetRightAscension Get", telescopeProperties.Target.Ra.ToString(":"));
                return (double)telescopeProperties.Target.Ra.Deg;
            }
            set
            {
                var val = new DMS(value, true);
                tl.LogMessage("TargetRightAscension Set", val.ToString(":"));
                telescopeProperties.Target.Ra = val;
            }
        }

        //private TrackingMode lastTrackingMode = TrackingMode.Unknown;

        public bool Tracking
        {
            get
            {
                if (!telescopeInteraction.CanGetTracking) throw new NotSupportedException("Getting tracking mode is not supported");
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
                if (!telescopeInteraction.CanSetTrackingRates) throw new NotSupportedException("Setting tracking rate is not supported");
                if (telescopeInteraction.CanSlewHighRate)
                {
                    telescopeWorker.SetTrackingRate(value, telescopeProperties.TrackingMode);
                    telescopeProperties.TrackingRate = value;
                    tl.LogMessage("TrackingRate Set", value.ToString());
                    return;
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
                if (telescopeInteraction.CanWorkDateTime)
                {
                    try
                    {
                        utcDate = telescopeInteraction.TelescopeDateTime.ToUniversalTime();
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
                
                if (telescopeInteraction.CanWorkDateTime)
                {
                    try
                    {
                        telescopeInteraction.TelescopeDateTime = DateTime.SpecifyKind(value, DateTimeKind.Utc);
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

        public bool AtHome
        {
            get
            {
                var res = telescopeWorker.IsAthome;
                tl.LogMessage("AtHome", "Get - " + res.ToString());
                return res;
            }
        }

        public bool AtPark
        {
            get
            {
                var res = telescopeProperties.IsAtPark;
                tl.LogMessage("AtPark", "Get - " + res.ToString());
                return res;
            }
        }

        public bool CanSetPark
        {
            get
            {
                var res = telescopeInteraction.CanWorkPosition;
                tl.LogMessage("CanSetPark", "Get - " + res.ToString());
                return res;
            }
        }
        public bool CanUnpark
        {
            get
            {
                var res = telescopeInteraction.CanWorkPosition;// && telescopeProperties.IsAtPark;
                tl.LogMessage("CanUnpark", "Get - " + res.ToString());
                return res;
            }
        }
        public bool CanPark
        {
            get
            {
                var res = telescopeInteraction.CanWorkPosition && telescopeProperties.ParkPosition != null;// && !telescopeProperties.IsAtPark;
                tl.LogMessage("CanPark", "Get - " + true.ToString());
                return res;
            }
        }

        public bool CanFindHome
        {
            get
            {
                var res = telescopeInteraction.CanWorkPosition && telescopeProperties.HomePozition != null;
                tl.LogMessage("CanFindHome", "Get - " + res.ToString());
                return res;
            }
        }

        public void SetPark()
        {
            var pos = telescopeInteraction.GetPosition();
            telescopeProperties.ParkPosition = pos;
            telescopeProperties.HomePozition = pos;
            profile.ParkAlt = pos.Alt;
            profile.ParkAzm = pos.Azm;
            WriteProfile();
            tl.LogMessage("SetPark", string.Format("Setted at Azm={0:f6} Alt={1:f6}", pos.Azm, pos.Alt));
        }

        public void FindHome()
        {
            if (telescopeInteraction.CanWorkPosition && telescopeProperties.HomePozition != null)
            {
                var pos = telescopeProperties.HomePozition;
                try
                {
                    telescopeWorker.GoHome();
                }
                catch (Exception err)
                {
                    tl.LogMessage("FindHome", string.Format("Timeout error while finding home position: Azm={0:f6} Alt={1:f6}", pos.Azm, pos.Alt));
                    throw new DriverException("Timeout error while parking");
                }
                tl.LogMessage("FindHome", string.Format("Driving to: Azm={0:f6} Alt={1:f6}", pos.Azm, pos.Alt));
            }
        }


        public void Park()
        {
            if (telescopeInteraction.CanWorkPosition && telescopeProperties.ParkPosition != null)
            {
                var pos = telescopeProperties.HomePozition;
                tl.LogMessage("Parking", string.Format("Driving to: Azm={0:f6} Alt={1:f6}", pos.Azm, pos.Alt));
                try
                {
                    telescopeWorker.Park();
                }
                catch (Exception err)
                {
                    tl.LogMessage("Parking", string.Format("Timeout error while parking at position: Azm={0:f6} Alt={1:f6}", pos.Azm, pos.Alt));
                    throw new DriverException("Timeout error while parking");
                }
                tl.LogMessage("Parked", string.Format("at position: Azm={0:f6} Alt={1:f6}", pos.Azm, pos.Alt));
                profile.IsAtPark = true;
                WriteProfile();
                return;
            }

            tl.LogMessage("Park", "Can't park");
            throw new NotSupportedException("Parking is not supported or position is not setted");
        }

        public void Unpark()
        {
            if (telescopeInteraction.CanWorkPosition && telescopeProperties.IsAtPark)
            {
                telescopeProperties.IsAtPark = false;
                telescopeWorker.SetTracking(true);
                profile.IsAtPark = false;
                WriteProfile();
                tl.LogMessage("Unpark", "unparked");
                return;
            }
            tl.LogMessage("Unpark", "Not supported");
            throw new NotSupportedException("Unparking is not supported");
        }

        #endregion
        /// <summary>
        /// Check rate on specified axis (deg/sec)
        /// </summary>
        /// <param name="axis">TelescopeAxes</param>
        /// <param name="rate">Rate (deg/sec)</param>
        /// <returns></returns>
        public bool CheckRate(TelescopeAxes axis, double rate)
        {
            foreach (IRate trackingRate in AxisRates(axis))
            {
                if (rate >= trackingRate.Minimum && rate <= trackingRate.Maximum) return true;
            }
            return false;
        }
    }
}
