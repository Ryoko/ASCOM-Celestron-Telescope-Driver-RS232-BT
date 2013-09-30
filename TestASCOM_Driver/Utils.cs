using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace ASCOM.CelestronAdvancedBlueTooth.Utils
{
    class DMS 
    {
        public int D { get; set; }
        public int M { get; set; }
        public decimal S { get; set; }
        public int Sign { get; set; }
        public bool isRA { get; set; }
        public decimal Deg
        {
            set 
            {
                if (value < 0){
                    Sign = -1;
                    value = value * -1;
                }else{
                    Sign = 1;
                }
                D = (int)value;
                value = (value - D) * 60;
                M = (int)value;
                S = (value - M) * 60;
            }
            get
            {
                return (D + (decimal)M / 60 + S / 3600) * Sign;
            }
        }

        public DMS(decimal deg)
        {
            Deg = deg;
        }
        
        public DMS(double deg, bool isRA = false)
        {
            Deg = (decimal)deg;
        }

        public DMS(int d, int m, decimal s)
        {
            D = d;
            M = m;
            S = s;
        }

        public static bool TryParse(string coordinates, out DMS value)
        {
            value = new DMS(0m);
            decimal val;

            //Regex r = new Regex(@"(\d+)[°\s]+(\d+)['\s]+(\d+)[\.\,]?(\d*)['\s]*");
            //var m = r.Match(coordinates);
            var c = coordinates.Split(new[] { ' ', '°', '\'', '.', ',', '"' });

            try
            {
                //if (m.Success)
                //{
                //    value.D = int.Parse(m.Groups[1].Value);
                //    value.M = int.Parse(m.Groups[2].Value);
                //    var dig = m.Groups[4].Length > 0 ? decimal.Parse("0" + Telescope.decimalSeparator + m.Groups[4].Value) : 0;
                //    value.S = int.Parse(m.Groups[3].Value) + dig;
                if (c.Length > 2)
                {
                    value.D = int.Parse(c[0]);
                    value.M = int.Parse(c[1]);
                    var dig = c.Length > 3 ? decimal.Parse("0" + Telescope.decimalSeparator + c[3]) : 0;
                    value.S = int.Parse(c[2]) + dig;

                    return true;
                }
            }
            catch
            {
                return false;
            }
            if (decimal.TryParse(coordinates, out val))
            {
                value.Deg = val;
                return true;
            } 
            return false;
        }

        override public string ToString()
        {
            return string.Format("{0:d2}°{1:d2}'{2,2:f1}\"", this.D, this.M, this.S);
        }
        
        public string ToString(string del)
        {
            return string.Format("{0:d2}{3}{1:d2}{3}{2,2:f1}", this.D, this.M, this.S, del);
        }
        
        public string ToString(string del1, string del2)
        {
            return string.Format("{0:d2}{3}{1:d2}{4}{2,2:f1}", this.D, this.M, this.S, del1, del2);
        }
    }

    class Utils
    {
        static public string Deg2HEX32(double val)
        {
            var v = (Int32)((val / 360) * 4294967296);
            return v.ToString("X");
        }
        static public string Deg2HEX16(double val)
        {
            var v = (Int16)((val / 360) * 65536);
            return v.ToString("X");

        }
        static public string RADeg2HEX32(double val)
        {
            var v = (Int32)((val / 24) * 4294967296);
            return v.ToString("X");
        }
        static public string RADeg2HEX16(double val)
        {
            var v = (Int16)((val / 24) * 65536);
            return v.ToString("X");

        }

    }

    public class Coordinates
    {
        public Coordinates(double ra, double dec)
        {
            Ra = ra;
            Dec = dec;
        }
        public double Ra { get; set; }
        public double Dec { get; set; }
    }
    public class AltAzm
    {
        public AltAzm(double alt, double azm)
        {
            Alt = alt;
            Azm = azm;
        }
        public double Alt { get; set; }
        public double Azm { get; set; }
    }
    public class LatLon
    {
        public LatLon(double lat, double lon)
        {
            Lat = lat;
            Lon = lon;
        }
        public LatLon(decimal lat, decimal lon)
        {
            Lat = (double)lat;
            Lon = (double)lon;
        }
        public double Lat { get; set; }
        public double Lon { get; set; }
    }
}
