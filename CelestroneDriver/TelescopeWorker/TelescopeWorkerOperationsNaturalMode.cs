namespace ASCOM.CelestronAdvancedBlueTooth.CelestroneDriver.TelescopeWorker
{
    using System;

    using ASCOM.Astrometry.Exceptions;
    using ASCOM.CelestronAdvancedBlueTooth.CelestroneDriver.Utils;
    using ASCOM.DeviceInterface;

    class TelescopeWorkerOperationsRateMode : ITelescopeWorkerOperations
    {
        private TelescopeProperties tp;
        private ITelescopeInteraction ti;

        public TelescopeWorkerOperationsRateMode()
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

            Rate += this.tp.RightAscensionRateOffset * Const.SiderealRate;
            if (mode == TrackingMode.EQS) Rate = -Rate;
            return Rate;
        }

        public void SetTrackingRate(DriveRates rate, TrackingMode mode)
        {
            double Rate = this.GetRateRa(rate, mode);
            this.CheckRateTrackingState();
            this.ti.SlewHighRate(SlewAxes.RaAzm, Rate);
            this.tp.MovingAzmAxes = false;
        }

        public void SetTrackingDec()
        {
            if (this.tp.TrackingMode <= TrackingMode.AltAzm)
            {
                this.ti.SlewHighRate(SlewAxes.DecAlt, 0);
            }
            else
            {
                //                if (tp.TrackingMode == TrackingMode.EQS) Rate = -Rate;
                var Rate = this.tp.DeclinationRateOffset;
                this.ti.SlewHighRate(SlewAxes.DecAlt, Rate);
            }
            this.tp.MovingAltAxes = false;

        }

        public void CheckRateTrackingState()
        {
            if (this.tp==null || this.tp.IsRateTracked || this.ti == null || !this.ti.CanSlewHighRate || !this.ti.CanSetTracking) return;
            this.ti.TrackingMode = TrackingMode.Off;
            this.tp.IsRateTracked = true;
        }

        public void PulseGuide(GuideDirections dir, int duration, PulsState ps)
        {
            if (!this.ti.CanSlewHighRate) throw new NotSupportedException("Puls guiding is not supported");
            this.CheckRateTrackingState();
            double rate;
            switch (dir)
            {
                case GuideDirections.guideNorth:
                    ps.Dec = new Puls(dir, Environment.TickCount, duration);
                    rate = this.tp.DeclinationRateOffset + this.tp.PulseRateAlt;
                    this.ti.SlewHighRate(SlewAxes.DecAlt, rate);
                    break;
                case GuideDirections.guideSouth:
                    ps.Dec = new Puls(dir, Environment.TickCount, duration);
                    rate = this.tp.DeclinationRateOffset - this.tp.PulseRateAlt;
                    this.ti.SlewHighRate(SlewAxes.DecAlt, rate);
                    break;
                case GuideDirections.guideEast:
                    ps.Ra = new Puls(dir, Environment.TickCount, duration);
                    rate = this.GetRateRa(this.tp.TrackingRate, this.tp.TrackingMode) + this.tp.PulseRateAzm;
                    this.ti.SlewHighRate(SlewAxes.RaAzm, rate);
                    break;
                case GuideDirections.guideWest:
                    ps.Ra = new Puls(dir, Environment.TickCount, duration);
                    rate = this.GetRateRa(this.tp.TrackingRate, this.tp.TrackingMode) - this.tp.PulseRateAzm;
                    this.ti.SlewHighRate(SlewAxes.RaAzm, rate);
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
                    this.ti.SlewHighRate(axis, rate);
                }
                else
                {
                    this.ti.SlewFixedRate(axis, (int)rate);
                }
            }
        }

        public void StopWorking()
        {
            if (this.ti != null && this.tp != null && this.tp.IsRateTracked)
            {
                if (this.ti.CanSlewHighRate)
                    this.ti.SlewHighRate(SlewAxes.DecAlt, 0);
                if (this.ti.CanSetTracking && this.ti.CanSlewHighRate)
                {
                    this.ti.SlewHighRate(SlewAxes.RaAzm, 0);
                    this.ti.TrackingMode = this.tp.TrackingMode;
                    this.tp.IsRateTracked = false;
                }
            }
        }


    }
}
