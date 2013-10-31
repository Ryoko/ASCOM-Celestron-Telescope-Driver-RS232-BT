using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CelestroneDriverTestProject
{
    using CelestroneDriver.TelescopeWorker;
    using NUnit.Framework;

    public class MockTelescope
    {
        private double _firmwareVersion;
        private TelescopeType _telescopeType;
        
        public MockTelescope(double firmwareVersion, TelescopeType telescopeType)
        {
            _firmwareVersion = firmwareVersion;
            _telescopeType = telescopeType;
        }

        public byte[] exchange(byte[] input)
        {
            var ans = new List<byte>();
            if(input == null || !input.Any()) throw new Exception("Empty transfer");

            var t = input[0];
            switch (t)
            {
                case (byte)'v':
                    return this.makeVersion();
                    break;
            }

            ans.Add((byte)'#');
            return ans.ToArray();
        }

        public byte[] makeVersion()
        {
            var v1 = (byte)_firmwareVersion;
            var v = _firmwareVersion - v1;
            while (v % 10 > 0) v *= 10;
            var v2 = (byte)v;
            return new byte[]{v1, v2};
        }
    }
}
