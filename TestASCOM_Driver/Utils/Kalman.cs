using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;

namespace ASCOM.CelestronAdvancedBlueTooth.Utils
{
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
            Q = q;
            R = r;
            F = f;
            H = h;
        }

        /// <summary>
        /// Set State
        /// </summary>
        /// <param name="state"></param>
        /// <param name="covariance"></param>
        public void SetState(double state, double covariance)
        {
            State = state;
            Covariance = covariance;
        }

        public double Correct(double data)
        {
            //time update - prediction
            X0 = F*State;
            P0 = F*Covariance*F + Q;

            //measurement update - correction
            var K = H*P0/(H*P0*H + R);
            State = X0 + K*(data - H*X0);
            Covariance = (1 - K*H)*P0;
            return State;
        }
    }

    public class StatisticValue
    {
        public int Time { get; set; }
        public double Value { get; set; }
        public double Error { get; set; }

        public StatisticValue(int time, double value, double error = 0)
        {
            Time = time;
            Value = value;
            Error = error;
        }
    }

    public class Statistic
    {

        public List<StatisticValue> values { get; set; }
        public double cSco = 0;
        public double cMed = 0;

        public Statistic(int medApperture)
        {
            values = new List<StatisticValue>();
            MedApperture = medApperture;
        }

        public void Add(double value)
        {
            values.Add(new StatisticValue(Environment.TickCount, value));
        }

        public void Add(double value, double modelValue)
        {
            var err = Math.Abs(value - modelValue);
            var val = new StatisticValue(Environment.TickCount, value, err);
            values.Add(val);
            var n = values.Count;
            cMed = (cMed*(n - 1) + value)/n;
            cSco = (cSco*(n - 1) + err)/n;
        }

        public double sco
        {
            get
            {
                if (values.Count < 4) return 0;
                var med = values.Sum(value => value.Value) / values.Count;
                values.ForEach(v => v.Error = v.Error.Equals(0) ? Math.Abs(v.Value - med): v.Error);
                return values.Sum(v => v.Error)/values.Count;
            }
        }

        public int MedApperture { get; set; }
        public double Median
        {
            get
            {
                var n = values.Count;
                if (n < MedApperture) return double.NaN;
                var v = new List<double>();
                var med = 0d;
                for (int i = n - MedApperture; i < n; i++)
                {
                    med += values[i].Value;
                    //v.Add(values[i].Value);
                }
                //v.Sort();
                return med/MedApperture; //v[MedApperture/2];
            }
        }
    }



}




