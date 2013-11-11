namespace ASCOM.CelestronAdvancedBlueTooth.CelestroneDriver.TelescopeWorker
{
    using System;

    using ASCOM.DeviceInterface;

    class Puls
    {
        public int TimeEnd { get; private set; }
//        int TimeBegin { get; set; }
        int Duration { get; set; }
        GuideDirections Direction { get; set; }

        public Puls(GuideDirections dir, int timeNow, int duration)
        {
            this.Direction = dir;
//            this.TimeBegin = timeNow;
            this.Duration = duration;
            this.TimeEnd = timeNow + duration;
        }

        public bool  isExpired 
        {
            get
            {
                return this.TimeEnd < Environment.TickCount;
            }
        }
    }

    class PulsState
    {
        public Puls Dec { get; set; }
        public Puls Ra { get; set; }
    }
}
