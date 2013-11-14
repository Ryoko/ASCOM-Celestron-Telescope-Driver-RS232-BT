namespace InputControl
{
    using System;
    using System.Threading;

    //public interface IInputControl
    //{
    //    int RefreshRate { get; set; }
    //    Action<ControllerState> OnUpdate { get; set; }
    //    AInputcontrol GetInstance { get; }
    //}

    public class AInputcontrol// : IInputControl
    {
        protected bool active;
        protected readonly Thread workThread;
        protected static volatile AInputcontrol instance;
//        protected static readonly object Syncroot = new object();
        protected readonly AutoResetEvent autoReset = new AutoResetEvent(false);
        protected ControllerState State;

        //public static AInputcontrol GetInstance(Type T)
        //{
        //        if (instance == null)
        //        {
        //            lock (Syncroot)
        //            {
        //                if (instance == null)
        //                {
        //                    instance = (AInputcontrol)Activator.CreateInstance(T);
        //                }
        //            }
        //        }
        //        return instance;
        //}

        public int RefreshRate { get; set; }
        public bool Active {
            get
            {
                return this.active;
            }
            set
            {
                if (value == this.active) return;
                if (value)
                {
                    this.autoReset.Set();
                }
                this.active = value;
            }
        }

        public Action<ControllerState> OnUpdate;

        public AInputcontrol()
        {
            this.RefreshRate = 100;
            this.Active = false;
            this.workThread = new Thread(this.DoWork);
            this.workThread.Start();
        }

        private void DoWork()
        {
            this.autoReset.WaitOne();
            while (true)
            {
                if (this.active)
                {
                    this.ProcessController();
                    Thread.Sleep(this.RefreshRate);
                }
                else
                {
                    this.autoReset.WaitOne();
                }
            }
        }

        protected virtual void ProcessController()
        {
            
        }
    }

    public class ControllerState
    {
        public double X { get; set; }
        public double Y { get; set; }
        public bool[] Buttons { get; set; }
        public bool Active { get; set; }
    }
}
