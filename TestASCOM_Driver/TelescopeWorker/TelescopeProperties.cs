using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASCOM.CelestronAdvancedBlueTooth.Utils;

namespace ASCOM.CelestronAdvancedBlueTooth.TelescopeWorker
{
    enum SlewState { InitSlew = -1, NoSlew = 0, Slewing, SlewRest, MoveAxis };
    enum TelescopeType { Unknown, NextStar, NextStar58, GT, Ultima }


    class TelescopeProperties
    {
        private static TelescopeProperties _properties = new TelescopeProperties();

        public bool IsReady { get; private set; }
        public string TelescopeName { get; set; }
        public string TelescopeType { get; set; }
        public Coordinates RaDec { get; set; }
        public AltAzm AltAzm { get; set; }
        public DateTime DateTime { get; set; }
        public SlewState SlewState { get; set; }
        public LatLon Location { get; set; }
        public double Elevation { get; set; }
        public ITelescopeWorker TelescopeWorker { get; set; }
        public double Apperture { get; set; }
        public double FocalLength { get; set; }
        public double AppertureArea { get; set; }
        public double ObstructionPercent { get; set; }
        public Coordinates TargetCoordinates { get; set; }
        public AltAzm TargetAltAzm { get; set; }
        public double FirmwareVersion { get; set; }

        public static TelescopeProperties GetProperties(ITelescopeWorker tw)
        {
            
        }

        public static TelescopeProperties Properties { get { return _properties; } }
    }
}
