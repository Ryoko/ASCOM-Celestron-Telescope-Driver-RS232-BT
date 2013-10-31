namespace CelestroneDriver.TelescopeWorker
{
    using System;

    using ASCOM.DeviceInterface;

    class Puls
    {
        int TimeBegin { get; set; }
        int Duration { get; set; }
        GuideDirections Direction { get; set; }

        public Puls(GuideDirections dir, int timeNow, int duration)
        {
            this.Direction = dir;
            this.TimeBegin = timeNow;
            this.Duration = duration;
        }

        public bool isExpired 
        {
            get
            {
                return this.TimeBegin + this.Duration < Environment.TickCount;
            }
        }
    }

    class PulsState
    {
        public Puls Dec { get; set; }
        public Puls Ra { get; set; }
    }
}
