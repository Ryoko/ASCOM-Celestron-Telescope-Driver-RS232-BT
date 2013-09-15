using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ASCOM.CelestronAdvancedBlueTooth
{
    internal interface IDriverWorker
    {
        DriverWorker.CheckConnectedDelegate CheckConnected { get; set; }
        IDeviceWorker dw { get; set; }
        void CommandBlind(string command, bool raw);
        bool CommandBool(string command, bool raw);
        string CommandString(string command, bool raw);
        bool GetPairValues(string command, out int val1, out int val2);
    }

    class DriverWorker : IDriverWorker
    {
        public delegate void CheckConnectedDelegate(string message);
        public CheckConnectedDelegate CheckConnected { get; set; }
        public IDeviceWorker dw { get; set; }
        private object _lockConnection

        public DriverWorker(CheckConnectedDelegate checkConnected, IDeviceWorker deviceWorker)
        {
            dw = deviceWorker;
            CheckConnected = checkConnected;
        }

        public void CommandBlind(string command, bool raw)
        {
            CheckConnected("CommandBlind");
            // Call CommandString and return as soon as it finishes
            this.CommandString(command, raw);
            return;
        }

        public bool CommandBool(string command, bool raw)
        {
            CheckConnected("CommandBool");
            string ret = CommandString(command, raw);
            return ret.EndsWith("#");
            // TODO decode the return string and return true or false
            // or
        }

        public string CommandString(string command, bool raw)
        {
            CheckConnected("CommandString");
            // it's a good idea to put all the low level communication with the device here,
            // then all communication calls this function
            // you need something to ensure that only one command is in progress at a time
            try
            {
                return dw.Transfer(command);
            }
            catch (Exception err)
            {
                throw new ASCOM.DriverException(err.Message, err);
            }
        }

        public bool GetPairValues(string command, out int val1, out int val2)
        {
            val1 = val2 = 0;
            var r = CommandString(command, false);
            if (!r.EndsWith("#")) return false;
            var val = r.TrimEnd('#').Split(new[] { ',' });
            try
            {
                val1 = Convert.ToInt32(val[0], 16);
                val2 = Convert.ToInt32(val[1], 16);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
