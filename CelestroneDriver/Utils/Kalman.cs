namespace ASCOM.CelestronAdvancedBlueTooth.CelestroneDriver.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal class KalmanFilterSimple1D
    {
        public double X0 { get; private set; } // predicted state
        public double P0 { get; private set; } // predicted covariance

        public double F { get; private set; } // factor of real value to previous real value
        public double Q { get; private set; } // measurement noise
        public double H { get; private set; } // factor of measured value to real value
        public double R { get; private set; } // environment noise

        public double State { get; private set; }
        public double Covariance { get; private set; }

        /// <summary>
        /// Kalman Filter
        /// </summary>
        /// <param name="q">measurement noise</param>
        /// <param name="r">environment noise</param>
        /// <param name="f">factor of real value to previous real value</param>
        /// <param name="h">factor of measured value to real value</param>
        public KalmanFilterSimple1D(double q, double r, double f = 1, double h = 1)
        {
            this.Q = q;
            this.R = r;
            this.F = f;
            this.H = h;
        }

        /// <summary>
        /// Set State
        /// </summary>
        /// <param name="state"></param>
        /// <param name="covariance"></param>
        public void SetState(double state, double covariance)
        {
            this.State = state;
            this.Covariance = covariance;
        }

        public double Correct(double data)
        {
            //time update - prediction
            this.X0 = this.F*this.State;
            this.P0 = this.F*this.Covariance*this.F + this.Q;

            //measurement update - correction
            var K = this.H*this.P0/(this.H*this.P0*this.H + this.R);
            this.State = this.X0 + K*(data - this.H*this.X0);
            this.Covariance = (1 - K*this.H)*this.P0;
            return this.State;
        }
    }

    public class StatisticValue
    {
        public int Time { get; set; }
        public double Value { get; set; }
        public double Error { get; set; }

        public StatisticValue(int time, double value, double error = 0)
        {
            this.Time = time;
            this.Value = value;
            this.Error = error;
        }
    }

    public class Statistic
    {

        public List<StatisticValue> values { get; set; }
        public double cSco = 0;
        public double cMed = 0;

        public Statistic(int medApperture)
        {
            this.values = new List<StatisticValue>();
            this.MedApperture = medApperture;
        }

        public void Add(double value)
        {
            this.values.Add(new StatisticValue(Environment.TickCount, value));
        }

        public void Add(double value, double modelValue)
        {
            var err = Math.Abs(value - modelValue);
            var val = new StatisticValue(Environment.TickCount, value, err);
            this.values.Add(val);
            var n = this.values.Count;
            this.cMed = (this.cMed*(n - 1) + value)/n;
            this.cSco = (this.cSco*(n - 1) + err)/n;
        }

        public double sco
        {
            get
            {
                if (this.values.Count < 4) return 0;
                var med = this.values.Sum(value => value.Value) / this.values.Count;
                this.values.ForEach(v => v.Error = v.Error.Equals(0) ? Math.Abs(v.Value - med): v.Error);
                return this.values.Sum(v => v.Error)/this.values.Count;
            }
        }

        public int MedApperture { get; set; }
        public double Median
        {
            get
            {
                var n = this.values.Count;
                if (n < this.MedApperture) return double.NaN;
                var v = new List<double>();
                var med = 0d;
                for (int i = n - this.MedApperture; i < n; i++)
                {
                    med += this.values[i].Value;
                    //v.Add(values[i].Value);
                }
                //v.Sort();
                return med/this.MedApperture; //v[MedApperture/2];
            }
        }
    }



}




