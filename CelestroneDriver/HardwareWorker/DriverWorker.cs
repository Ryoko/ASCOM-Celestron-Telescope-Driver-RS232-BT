﻿namespace ASCOM.CelestronAdvancedBlueTooth.CelestroneDriver.HardwareWorker
{
    using System;

    using ASCOM.CelestronAdvancedBlueTooth.CelestroneDriver.TelescopeWorker;
    using ASCOM.CelestronAdvancedBlueTooth.CelestroneDriver.Utils;

    internal interface IDriverWorker
    {
        DriverWorker.CheckConnectedDelegate CheckConnected { get; set; }
        IDeviceWorker dw { get; set; }
        void CommandBlind(string command, bool raw);
        bool CommandBool(string command, bool raw);
        string CommandString(string command, bool raw);
        bool GetPairValues(string command, out int val1, out int val2);
        byte[] CommandBytes(byte[] command);
    }

    class DriverWorker : IDriverWorker
    {
        public delegate void CheckConnectedDelegate(string message);
        public CheckConnectedDelegate CheckConnected { get; set; }
        public IDeviceWorker dw { get; set; }
        private object _lockConnection;

        public DriverWorker(CheckConnectedDelegate checkConnected, IDeviceWorker deviceWorker)
        {
            this.dw = deviceWorker;
            this.CheckConnected = checkConnected;
        }

        public void CommandBlind(string command, bool raw)
        {
            this.CheckConnected("CommandBlind");
            // Call CommandString and return as soon as it finishes
            this.CommandString(command, raw);
            return;
        }

        public bool CommandBool(string command, bool raw)
        {
            this.CheckConnected("CommandBool");
            string ret = this.CommandString(command, raw);
            return ret.EndsWith("#");
            // TODO decode the return string and return true or false
            // or
        }

        public string CommandString(string command, bool raw)
        {
            this.CheckConnected("CommandString");
            // it's a good idea to put all the low level communication with the device here,
            // then all communication calls this function
            // you need something to ensure that only one command is in progress at a time
            try
            {
                return this.dw.Transfer(command);
            }
            catch (Exception err)
            {
                throw new ASCOM.DriverException(err.Message, err);
            }
        }

        public byte[] CommandBytes(byte[] command)
        {
            try
            {
                return this.dw.Transfer(command);
            }
            catch (Exception err)
            {
                throw new ASCOM.DriverException(err.Message, err);
            }
        }

        public bool GetPairValues(string command, out int val1, out int val2)
        {
            val1 = val2 = 0;
            var r = this.CommandString(command, false);
            if (!r.EndsWith(GeneralCommands.TERMINATOR.AsString())) return false;
            var val = r.TrimEnd((char)GeneralCommands.TERMINATOR).Split(new[] { ',' });
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
