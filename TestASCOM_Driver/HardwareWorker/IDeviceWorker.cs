﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ASCOM.CelestronAdvancedBlueTooth
{
    interface IDeviceWorker
    {
        bool Connect(object connectionInfo);
        void Disconnect();
        string Transfer(string command);
        byte[] Transfer(byte[] send);

    }
}