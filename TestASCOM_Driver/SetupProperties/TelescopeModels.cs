using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;

namespace ASCOM.CelestronAdvancedBlueTooth.SetupProperties
{
    enum MountType { Fork, Gem}
    enum GPSmode { Unknown = -1, No = 0, Maybe, Yes}
    class TelescopeModel
    {
        public string Name { get; set; }
        public double Apperture { get; set; }
        public double FocalLenth { get; set; }
        public double ObstructionPercent { get; set; }
        public MountType Mount { get; set; }
        public GPSmode GPS { get; set; }
 
        public TelescopeModel(string name, double apperture, double focla, double obstruction)
        {
            Name = name;
            Apperture = apperture;
            FocalLenth = focla;
            ObstructionPercent = obstruction;
        }

        public override string ToString()
        {
            return Name;
        }
    }

    class TelescopeModels : IEnumerable<TelescopeModel>
    {
        public List<TelescopeModel> models = new List<TelescopeModel>();

        public TelescopeModels()
        {
            SetModels();
        }

        public TelescopeModel this[int i]
        {
            get { return models[i]; }
        }

        public void SetModels()
        {
            models.Add(new TelescopeModel("NexStar 60GT or SLT", 60, 700, 0){GPS = GPSmode.No, Mount = MountType.Fork});
            models.Add(new TelescopeModel("NexStar 76GT", 76, 700, 28){GPS = GPSmode.No, Mount = MountType.Fork});
            models.Add(new TelescopeModel("NexStar 80GT", 80, 400, 0){GPS = GPSmode.No, Mount = MountType.Fork});
            models.Add(new TelescopeModel("NexStar 80SLT", 80, 900, 0){GPS = GPSmode.No, Mount = MountType.Fork});
            models.Add(new TelescopeModel("NexStar 102GT", 102, 500, 0){GPS = GPSmode.No, Mount = MountType.Fork});
            models.Add(new TelescopeModel("NexStar 102SLT", 102, 660, 0){GPS = GPSmode.No, Mount = MountType.Fork});
            models.Add(new TelescopeModel("NexStar 114GT or SLT", 114, 1000, 36){GPS = GPSmode.No, Mount = MountType.Fork});
            models.Add(new TelescopeModel("NexStar 130GT or SLT", 130, 650, 29){GPS = GPSmode.No, Mount = MountType.Fork});
            models.Add(new TelescopeModel("NexStar 4GT", 102, 1325, 35){GPS = GPSmode.No, Mount = MountType.Fork});
            models.Add(new TelescopeModel("NexStar 5", 127, 1250, 35){GPS = GPSmode.No, Mount = MountType.Fork});
            models.Add(new TelescopeModel("NexStar 8", 203, 2032, 31){GPS = GPSmode.No, Mount = MountType.Fork});
            models.Add(new TelescopeModel("NexStar 5i", 127, 1250, 35){GPS = GPSmode.Maybe, Mount = MountType.Fork});
            models.Add(new TelescopeModel("NexStar 6SE", 150, 1500, 37){GPS = GPSmode.Maybe, Mount = MountType.Fork});
            models.Add(new TelescopeModel("NexStar 8i or 8i SE", 203, 2032, 31){GPS = GPSmode.Maybe, Mount = MountType.Fork});
            models.Add(new TelescopeModel("NexStar 8 GPS or CPC", 203, 2032, 34){GPS = GPSmode.Yes, Mount = MountType.Fork});
            models.Add(new TelescopeModel("NexStar 9.25 GPS or CPC", 235, 2350, 36){GPS = GPSmode.Yes, Mount = MountType.Fork});
            models.Add(new TelescopeModel("NexStar 11 GPS or CPC", 280, 2800, 34){GPS = GPSmode.Yes, Mount = MountType.Fork});
            models.Add(new TelescopeModel("CGE 800", 203, 2032, 34){GPS = GPSmode.Maybe, Mount = MountType.Gem});
            models.Add(new TelescopeModel("CGE or CGE Pro 925", 235, 2350, 36){GPS = GPSmode.Maybe, Mount = MountType.Gem});
            models.Add(new TelescopeModel("CGE or CGE Pro 1100", 280, 2800, 34){GPS = GPSmode.Maybe, Mount = MountType.Gem});
            models.Add(new TelescopeModel("CGE or CGE Pro 1400", 356, 3910, 32){GPS = GPSmode.Maybe, Mount = MountType.Gem});
            models.Add(new TelescopeModel("Advanced C5-SGT", 127, 1250, 35){GPS = GPSmode.Maybe, Mount = MountType.Gem});
            models.Add(new TelescopeModel("Advanced C6-NGT", 150, 750, 29){GPS = GPSmode.Maybe, Mount = MountType.Gem});
            models.Add(new TelescopeModel("Advanced C6-RGT", 150, 1200, 0){GPS = GPSmode.Maybe, Mount = MountType.Gem});
            models.Add(new TelescopeModel("Advanced C6-SGT", 150, 1500, 31){GPS = GPSmode.Maybe, Mount = MountType.Gem});
            models.Add(new TelescopeModel("Advanced C8-SGT", 203, 2032, 31){GPS = GPSmode.Maybe, Mount = MountType.Gem});
            models.Add(new TelescopeModel("Advanced C8-NGT", 200, 1000, 28){GPS = GPSmode.Maybe, Mount = MountType.Gem});
            models.Add(new TelescopeModel("Advanced C9 1/4-SGT", 235, 2350, 36){GPS = GPSmode.Maybe, Mount = MountType.Gem});
            models.Add(new TelescopeModel("Advanced C10-NGT", 254, 1200, 23){GPS = GPSmode.Maybe, Mount = MountType.Gem});
            models.Add(new TelescopeModel("Advanced C11-SGT", 279, 2800, 34){GPS = GPSmode.Maybe, Mount = MountType.Gem});
            models.Add(new TelescopeModel("CGEM 800", 200, 2032, 31){GPS = GPSmode.Maybe, Mount = MountType.Gem});
            models.Add(new TelescopeModel("CGEM 925", 235, 2350, 36){GPS = GPSmode.Maybe, Mount = MountType.Gem});
            models.Add(new TelescopeModel("CGEM 1100", 280, 2800, 34){GPS = GPSmode.Maybe, Mount = MountType.Gem});
            models.Add(new TelescopeModel("Advanced VX Mount", 80, 500, 0){GPS = GPSmode.Maybe, Mount = MountType.Gem});
            models.Add(new TelescopeModel("C20", 500, 3454, 39) { GPS = GPSmode.Maybe, Mount = MountType.Fork });
            models.Add(new TelescopeModel("Ultima 2000", 203, 2032, 34) { GPS = GPSmode.No, Mount = MountType.Fork });
            models.Add(new TelescopeModel("Synta SkyWatcher Mount", 80, 500, 0) { GPS = GPSmode.No, Mount = MountType.Gem });
            //        .AddItem "CGEM 800"
//        .ItemData(.NewIndex) = EncodeData(200, 2032, True, True, False, True, True)
//        m_Obstruction(.NewIndex) = 31
//        .AddItem "CGEM 925"
//        .ItemData(.NewIndex) = EncodeData(235, 2350, True, True, False, True, True)
//        m_Obstruction(.NewIndex) = 36
//        .AddItem "CGEM 1100"
//        .ItemData(.NewIndex) = EncodeData(280, 2800, True, True, False, True, True)
//        m_Obstruction(.NewIndex) = 34
//        .AddItem "Advanced VX Mount"
//        .ItemData(.NewIndex) = EncodeData(80, 500, True, True, False, True, True, 0)
//        m_Obstruction(.NewIndex) = 0
        }

        public IEnumerator<TelescopeModel> GetEnumerator()
        {
            return models.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
