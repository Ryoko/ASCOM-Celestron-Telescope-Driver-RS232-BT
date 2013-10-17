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



}




