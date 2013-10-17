using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASCOM.Astrometry.Exceptions;
using ASCOM.CelestronAdvancedBlueTooth.Utils;
using ASCOM.DeviceInterface;

namespace ASCOM.CelestronAdvancedBlueTooth.TelescopeWorker
{
    class TelescopeWorkerOperationsNaturalMode : ITelescopeWorkerOperations
    {
        private TelescopeProperties tp;
        private ITelescopeInteraction ti;

        public TelescopeWorkerOperationsNaturalMode()
        {
        }

        public void SegProperties(TelescopeProperties telescopeProperties, ITelescopeInteraction telescopeInteraction)
        {
            this.tp = telescopeProperties;
            this.ti = telescopeInteraction;
        }        
        /// <summary>
        /// Get rate on Azm axis in (deg/sec)
        /// </summary>
        /// <param name="rate">Curent DriveRate</param>
        /// <param name="mode">Current TrackingMode</param>
        /// <returns></returns>
        public double GetRateRa(DriveRates rate, TrackingMode mode)
        {
            if (mode <= TrackingMode.AltAzm)
            {
                return 0;
            }

            double Rate;
            switch (rate)
            {
                case DriveRates.driveSidereal:
                    Rate = Const.TRACKRATE_SIDEREAL;
                    break;
                case DriveRates.driveSolar:
                    Rate = Const.TRACKRATE_SOLAR;
                    break;
                case DriveRates.driveLunar:
                    Rate = Const.TRACKRATE_LUNAR;
                    break;
                default:
                    throw new ValueNotAvailableException("Wring tracking rate value");
            }

            Rate += tp.RightAscensionRateOffset * Const.SiderealRate;
            if (mode == TrackingMode.EQS) Rate = -Rate;
            return Rate;
        }

        public void SetTrackingRate(DriveRates rate, TrackingMode mode)
        {
            double Rate = GetRateRa(rate, mode);
            CheckRateTrackingState();
            if (!tp.IsAtPark)
                ti.SlewHighRate(SlewAxes.RaAzm, Rate);
            tp.MovingAzmAxes = false;
        }

        public void SetTrackingDec()
        {
            if (tp.TrackingMode <= TrackingMode.AltAzm)
            {
                ti.SlewHighRate(SlewAxes.DecAlt, 0);
            }
            else
            {
                //                if (tp.TrackingMode == TrackingMode.EQS) Rate = -Rate;
                var Rate = tp.DeclinationRateOffset;
                if (!tp.IsAtPark)
                    ti.SlewHighRate(SlewAxes.DecAlt, Rate);
            }
            tp.MovingAltAxes = false;

        }

        public void CheckRateTrackingState()
        {
            if (tp.IsRateTracked || ti == null || !ti.CanSlewHighRate || !ti.CanSetTracking) return;
            ti.TrackingMode = TrackingMode.Off;
            tp.IsRateTracked = true;
        }

        public void PulseGuide(GuideDirections dir, int duration, PulsState ps)
        {
            if (!ti.CanSlewHighRate) throw new NotSupportedException("Puls guiding is not supported");
            CheckRateTrackingState();
            double rate;
            switch (dir)
            {
                case GuideDirections.guideNorth:
                    ps.Dec = new Puls(dir, Environment.TickCount, duration);
                    rate = tp.DeclinationRateOffset + tp.PulseRateAlt;
                    ti.SlewHighRate(SlewAxes.DecAlt, rate);
                    break;
                case GuideDirections.guideSouth:
                    ps.Dec = new Puls(dir, Environment.TickCount, duration);
                    rate = tp.DeclinationRateOffset - tp.PulseRateAlt;
                    ti.SlewHighRate(SlewAxes.DecAlt, rate);
                    break;
                case GuideDirections.guideEast:
                    ps.Ra = new Puls(dir, Environment.TickCount, duration);
                    rate = GetRateRa(tp.TrackingRate, tp.TrackingMode) + tp.PulseRateAzm;
                    ti.SlewHighRate(SlewAxes.RaAzm, rate);
                    break;
                case GuideDirections.guideWest:
                    ps.Ra = new Puls(dir, Environment.TickCount, duration);
                    rate = GetRateRa(tp.TrackingRate, tp.TrackingMode) - tp.PulseRateAzm;
                    ti.SlewHighRate(SlewAxes.RaAzm, rate);
                    break;
            }
        }

        /// <summary>
        /// Move Azm axis with fixed or variable rate
        /// </summary>
        /// <param name="rate">Rate (deg/sec) or fixed rate * 10</param>
        /// <param name="isFixed"></param>
        public void MoveAxis(SlewAxes axis, double rate, bool isFixed = false)
        {
            if (!rate.Equals(0))
            {
                if (!isFixed)
                {
                    ti.SlewHighRate(axis, rate);
                }
                else
                {
                    ti.SlewFixedRate(axis, (int) rate);
                }
            }
        }

        public void StopWorking()
        {
            if (ti != null && tp != null && tp.IsRateTracked && !tp.IsAtPark)
            {
                if (ti.CanSlewHighRate)
                    ti.SlewHighRate(SlewAxes.DecAlt, 0);
                if (ti.CanSetTracking && ti.CanSlewHighRate)
                {
                    ti.SlewHighRate(SlewAxes.RaAzm, 0);
                    ti.TrackingMode = tp.TrackingMode;
                    tp.IsRateTracked = false;
                }
            }
        }


    }
}
