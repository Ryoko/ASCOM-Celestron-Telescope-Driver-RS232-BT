using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using ASCOM.DeviceInterface;
using ASCOM.Utilities;

namespace ASCOM.CelestronAdvancedBlueTooth
{
    class TAdvancedGT : ITelescopeWorker
    {
        private IDeviceWorker dw;
        private TraceLogger tl;
        private IDriverWorker driverWorker;

        public TAdvancedGT(IDeviceWorker Dw, TraceLogger Tl, IDriverWorker DriverWorker)
        {
            dw = Dw;
            tl = Tl;
            driverWorker = DriverWorker;
        }
        
        #region ITelescope Implementation

        public void AbortSlew()
        {
            tl.LogMessage("AbortSlew", "Not implemented");

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
                int Alt, Azm;
                if (driverWorker.GetPairValues("z", out Alt, out Azm))
                {
                    double val = (Alt / 4294967296) * 360;
                    tl.LogMessage("Altitude Get", val.ToString());

                    return val;
                }
                tl.LogMessage("Altitude Get", "Error geting altitude");
                throw new ASCOM.DriverException("Error geting altitude");
            }
        }

        public double ApertureArea
        {
            get
            {
                tl.LogMessage("ApertureArea Get", "Not implemented");
                return Telescope.apperture;
            }
        }

        public double ApertureDiameter
        {
            get
            {
                tl.LogMessage("ApertureDiameter Get", "Not implemented");
                return Telescope.apperture;
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

                int Alt, Azm;
                if (driverWorker.GetPairValues("z", out Alt, out Azm))
                {
                    double val = (Azm / 4294967296) * 360;
                    tl.LogMessage("Azimuth Get", val.ToString());
                    return val;
                }
                tl.LogMessage("Azimuth Get", "Error geting azimuth");
                throw new ASCOM.DriverException("Error geting azimuth");
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
            tl.LogMessage("CanMoveAxis", "Get - " + Axis.ToString());
            switch (Axis)
            {
                case TelescopeAxes.axisPrimary:
                    return true;
                case TelescopeAxes.axisSecondary:
                    return true;
                case TelescopeAxes.axisTertiary:
                    return false;
                default:
                    throw new InvalidValueException("CanMoveAxis", Axis.ToString(), "0 to 2");
            }
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
                tl.LogMessage("CanSetDeclinationRate", "Get - " + false.ToString());
                return false;
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
                tl.LogMessage("CanSetRightAscensionRate", "Get - " + false.ToString());
                return false;
            }
        }

        public bool CanSetTracking
        {
            get
            {
                tl.LogMessage("CanSetTracking", "Get - " + false.ToString());
                return false;
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
                return false;
            }
        }

        public bool CanSyncAltAz
        {
            get
            {
                tl.LogMessage("CanSyncAltAz", "Get - " + false.ToString());
                return false;
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
                int Ra, Dec;
                if (driverWorker.GetPairValues("e", out Ra, out Dec))
                {
                    double val = (Dec / 4294967296) * 360;
                    tl.LogMessage("Declination", "Get - " + utilities.DegreesToDMS(val, ":", ":"));
                    return val;
                }
                tl.LogMessage("Declination Get", "Error geting Declination");
                throw new ASCOM.DriverException("Error geting Declination");

                //double declination = 0.0;
                //tl.LogMessage("Declination", "Get - " + utilities.DegreesToDMS(declination, ":", ":"));
                //return declination;
            }
        }

        public double DeclinationRate
        {
            get
            {
                double declination = 0.0;
                tl.LogMessage("DeclinationRate", "Get - " + declination.ToString());
                return declination;
            }
            set
            {
                tl.LogMessage("DeclinationRate Set", "Not implemented");
                throw new ASCOM.PropertyNotImplementedException("DeclinationRate", true);
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
                tl.LogMessage("FocalLength Get", Telescope.focal.ToString());
                return Telescope.focal;
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
                throw new ASCOM.PropertyNotImplementedException("IsPulseGuiding", false);
            }
        }

        public void MoveAxis(TelescopeAxes Axis, double Rate)
        {
            tl.LogMessage("MoveAxis", "Not implemented");
            throw new ASCOM.MethodNotImplementedException("MoveAxis");
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
                int Ra, Dec;
                if (driverWorker.GetPairValues("e", out Ra, out Dec))
                {
                    double val = (Ra / 4294967296) * 360;
                    tl.LogMessage("RightAscension", "Get - " + utilities.DegreesToDMS(val, ":", ":"));
                    return val;
                }
                tl.LogMessage("RightAscension Get", "Error getting RightAscension");
                throw new ASCOM.DriverException("Error getting RightAscension");
            }
        }

        public double RightAscensionRate
        {
            get
            {
                double rightAscensionRate = 0.0;
                tl.LogMessage("RightAscensionRate", "Get - " + rightAscensionRate.ToString());
                return rightAscensionRate;
            }
            set
            {
                tl.LogMessage("RightAscensionRate Set", "Not implemented");
                throw new ASCOM.PropertyNotImplementedException("RightAscensionRate", true);
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
                                       24.065709824419081 * (utilities.DateLocalToJulian(DateTime.Now) - 2451545.0)) % 24.0;
                tl.LogMessage("SiderealTime", "Get - " + siderealTime.ToString());
                return siderealTime;
            }
        }

        public double SiteElevation
        {
            get
            {
                tl.LogMessage("SiteElevation Get", Telescope.elevation.ToString());
                return Telescope.elevation;
            }
            set
            {
                tl.LogMessage("SiteElevation Set", value.ToString());
                Telescope.elevation = (int)value;
            }
        }

        public double SiteLatitude
        {
            get
            {
                tl.LogMessage("SiteLatitude Get", "Not implemented");
                throw new ASCOM.PropertyNotImplementedException("SiteLatitude", false);
            }
            set
            {
                tl.LogMessage("SiteLatitude Set", "Not implemented");
                throw new ASCOM.PropertyNotImplementedException("SiteLatitude", true);
            }
        }

        public double SiteLongitude
        {
            get
            {
                tl.LogMessage("SiteLongitude Get", "Not implemented");
                throw new ASCOM.PropertyNotImplementedException("SiteLongitude", false);
            }
            set
            {
                tl.LogMessage("SiteLongitude Set", "Not implemented");
                throw new ASCOM.PropertyNotImplementedException("SiteLongitude", true);
            }
        }

        public short SlewSettleTime
        {
            get
            {
                tl.LogMessage("SlewSettleTime Get", "Not implemented");
                throw new ASCOM.PropertyNotImplementedException("SlewSettleTime", false);
            }
            set
            {
                tl.LogMessage("SlewSettleTime Set", "Not implemented");
                throw new ASCOM.PropertyNotImplementedException("SlewSettleTime", true);
            }
        }

        public void SlewToAltAz(double Azimuth, double Altitude)
        {
            SlewToAltAzAsync(Azimuth, Altitude);
            while (true)
            {
                Thread.Sleep(100);
                var r = driverWorker.CommandString("L", false);
                if (r.Equals("0#")) break;
            }
            tl.LogMessage("SlewingToAltAz", string.Format("Alt:{0}, Azm:{1}", Altitude, Azimuth));
        }

        public void SlewToAltAzAsync(double Azimuth, double Altitude)
        {
            if (driverWorker.CommandBool(string.Format("b{0},{1}#",
                Utils.Utils.Deg2HEX32(Azimuth), Utils.Utils.Deg2HEX32(Altitude)), false))
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
                var r = driverWorker.CommandString("L", false);
                if (r.Equals("0#")) break;
            }
            tl.LogMessage("Slewed ToToCoordinates", string.Format("RA:{0}, Dec:{1}", RightAscension, Declination));
        }

        public void SlewToCoordinatesAsync(double RightAscension, double Declination)
        {
            if (driverWorker.CommandBool(string.Format("b{0},{1}#",
                Utils.Utils.Deg2HEX32(RightAscension), Utils.Utils.Deg2HEX32(Declination)), false))
            {
                tl.LogMessage("Slewing ToToCoordinates", string.Format("RA:{0}, Dec:{1}", RightAscension, Declination));
            }
            else
            {
                tl.LogMessage("Error SlewToToCoordinates", string.Format("RA:{0}, Dec:{1}", RightAscension, Declination));
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
                var r = driverWorker.CommandString("L", false);
                if (r.Equals("0#")) return false;
                if (r.Equals("1#")) return true;
                throw new ASCOM.DriverException("Error getting slewing state");
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

        private int TrackingMode = -1;

        public bool Tracking
        {
            get
            {
                bool tracking = true;
                var res = dw.Transfer("t");
                var trMode = Encoding.ASCII.GetBytes(res)[0];
                tl.LogMessage("Tracking", "Get - " + trMode.ToString());
                if (trMode > 0) TrackingMode = trMode;
                return trMode > 0;
            }
            set
            {
                int trMode = -1;
                if (value)
                {
                    if (TrackingMode < 0)
                    {
                        var ct = this.Tracking;
                    }
                    if (TrackingMode < 1) throw new ASCOM.InvalidOperationException("Tracking mode not set yet");
                }
                trMode = value ? TrackingMode : 0;
                var command = new[] { (byte)'T', (byte)trMode };
                var res = dw.Transfer(command);
                if (res[0] != (byte)'#') throw new ASCOM.InvalidOperationException("Execution Error");
                tl.LogMessage("Tracking Set", trMode.ToString());
            }
        }

        public DriveRates TrackingRate
        {
            get
            {
                tl.LogMessage("TrackingRate Get", "Not implemented");
                throw new ASCOM.PropertyNotImplementedException("TrackingRate", false);
            }
            set
            {
                tl.LogMessage("TrackingRate Set", "Not implemented");
                throw new ASCOM.PropertyNotImplementedException("TrackingRate", true);
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
                DateTime utcDate = DateTime.UtcNow;
                tl.LogMessage("TrackingRates", "Get - " + String.Format("MM/dd/yy HH:mm:ss", utcDate));
                return utcDate;
            }
            set
            {
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

    }
}
