using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using ASCOM.DeviceInterface;

namespace ASCOM.CelestronAdvancedBlueTooth.TelescopeWorker
{
    class Puls
    {
        int TimeBegin { get; set; }
        int Duration { get; set; }
        GuideDirections Direction { get; set; }

        public Puls(GuideDirections dir, int timeNow, int duration)
        {
            Direction = dir;
            TimeBegin = timeNow;
            Duration = duration;
        }

        public bool isExpired 
        {
            get
            {
                return TimeBegin + Duration < Environment.TickCount;
            }
        }
    }

    class PulsState
    {
        public Puls Dec { get; set; }
        public Puls Ra { get; set; }
    }
}
