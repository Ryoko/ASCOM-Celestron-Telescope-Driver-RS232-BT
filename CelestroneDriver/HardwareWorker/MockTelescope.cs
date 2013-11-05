namespace ASCOM.CelestronAdvancedBlueTooth.CelestroneDriver.HardwareWorker
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using ASCOM.CelestronAdvancedBlueTooth.CelestroneDriver.TelescopeWorker;
    using ASCOM.CelestronAdvancedBlueTooth.CelestroneDriver.Utils;

    public class MockTelescope
    {
        private double _firmwareVersion;
        private TelescopeType _telescopeType;
        private LatLon Location = new LatLon(45d, 45d);
        private TrackingMode _tracking;

        public MockTelescope(double firmwareVersion, TelescopeType telescopeType)
        {
            this.FirmwareVersion = firmwareVersion;
            this.TelescopeType = telescopeType;
            _tracking = TrackingMode.EQN;
        }

        public double FirmwareVersion
        {
            get
            {
                return this._firmwareVersion;
            }
            set
            {
                this._firmwareVersion = value;
            }
        }

        public TelescopeType TelescopeType
        {
            get
            {
                return this._telescopeType;
            }
            set
            {
                this._telescopeType = value;
            }
        }

        public string exchange(string input)
        {
            var cBuff = input.ToCharArray();
            var buff = new byte[input.Length];
            for (int i = 0; i < cBuff.Length; i++)
            {
                buff[i] = (byte)cBuff[i];
            }
            var ret = this.exchange(buff);
            var bRet = new char[ret.Length];
            for (int i = 0; i < ret.Length; i++)
            {
                bRet[i] = (char)ret[i];
            }
            return new string(bRet);
        }

        public byte[] exchange(byte[] input)
        {
            var ans = new List<byte>();
            if(input == null || !input.Any()) throw new Exception("Empty transfer");

            var t = input[0];
            switch (t)
            {
                case (byte)'V':
                    return this.makeVersion();
                case (byte)'Z':
                    return "12AB,4000#".ToBytes();
                case (byte)'z':
                    return "12AB0500,40000500#".ToBytes();
                case (byte)'E':
                    return "34AB,12CE#".ToBytes();
                case (byte)'e':
                    return "34AB0500,12CE0500#".ToBytes();
                case (byte)'S': //Sync
                case (byte)'s':
                case (byte)'B': //Slew AltAzm
                case (byte)'b':
                case (byte)'R': //Slew RaDec
                case (byte)'r':
                    return "#".ToBytes();
                case (byte)'t':
                    return new byte[]{(byte)_tracking, (byte)'#'};
                case (byte)'T':
                    _tracking = (TrackingMode)input[1];
                    return new byte[] { (byte)'#' };
                case (byte)'P':
                    return this.SendCommand(input[1], (DeviceID)input[2], (DeviceCommands)input[3], input.Skip(4).ToArray());
                case (byte)'w':
                    {
                        var lat = Location.Lat.ToDMS();
                        var lon = Location.Lon.ToDMS();

                        return new byte[]
                                   {
                                       (byte)lat.D, (byte)lat.M, (byte)lat.S, (byte)(lat.Sign > 0 ? 0 : 1), (byte)lon.D,
                                       (byte)lon.M, (byte)lon.S, (byte)(lon.Sign > 0 ? 0 : 1), (byte)'#'
                                   };
                    }
                case (byte)'W':
                    {
                        var lat = new DMS(input[1], input[2], input[3], input[4] > 0 ? -1 : 1);
                        var lon = new DMS(input[5], input[6], input[7], input[8] > 0 ? -1 : 1);
                        Location = new LatLon(lat.Deg, lon.Deg);
                        return "#".ToBytes();
                    }
                case (byte)'h':
                    {
                        var tm = DateTime.Now;
                        var tz = (int)(TimeZone.CurrentTimeZone.GetUtcOffset(tm).TotalHours + 0.5);
                        var dlst = TimeZone.CurrentTimeZone.IsDaylightSavingTime(DateTime.Now) ? 1 : 0; 
                        return new byte[]
                                   {
                                       (byte)tm.Hour, (byte)tm.Minute, (byte)tm.Second, (byte)tm.Month, (byte)tm.Day,
                                       (byte)(tm.Year - 2000), (byte)tz, (byte)dlst, (byte)'#'
                                   };
                    }
                case (byte)'H':
                    return "#".ToBytes();
                default:
                    return "#".ToBytes();
            }

            ans.Add((byte)'#');
            return ans.ToArray();
        }

        private byte[] SendCommand(byte nOfParameters, DeviceID deviceId, DeviceCommands command, byte[] par)
        {
            switch (deviceId)
            {
                case DeviceID.DecAltMotor:
                case DeviceID.RaAzmMotor:
                    return this.MotorCommands(nOfParameters, command, par);
                case DeviceID.HC:

                default:
                    return "#".ToBytes();
            }
        }

        private byte[] MotorCommands(byte nOfParameters, DeviceCommands command, byte[] par)
        {
            switch (command)
            {
                case DeviceCommands.GET_VER:
                    return "12#".ToBytes();
                case DeviceCommands.MC_GET_POSITION:
                    return new byte[] { 0, 1, 2, 4, (byte)'#' };
                case DeviceCommands.MC_GOTO_FAST:
                case DeviceCommands.MC_GOTO_SLOW:
                    return "#".ToBytes();
                case DeviceCommands.MC_SET_POS_FIXED_GUIDERATE:
                case DeviceCommands.MC_SET_NEG_FIXED_GUIDERATE:
                    return "#".ToBytes();
                case DeviceCommands.MC_SET_POS_VARIABLE_GUIDERATE:
                case DeviceCommands.MC_SET_NEG_VARIABLE_GUIDERATE:
                    return "#".ToBytes();
                case DeviceCommands.MC_SET_POSITION:
                    return "#".ToBytes();
                case DeviceCommands.MC_SET_AUTOGUIDE_RATE:
                    return "#".ToBytes();
                case DeviceCommands.MC_GET_AUTOGUIDE_RATE:
                    return new byte[] { 0x0, 0x1, 0x2, 0x3, (byte)'#' };
                case DeviceCommands.MC_SLEW_DONE:
                    return new byte[] { 0xff, (byte)'#' };
                default:
                    return "#".ToBytes();
            }
        }

        public byte[] makeVersion()
        {
            var ver = (decimal)this.FirmwareVersion;
            var v1 = (byte)ver;
            var v = ver - v1;
            while (v % 1 > 0) v *= 10;
            var v2 = (byte)v;
            return new byte[]{v1, v2};
        }
    }

    public static class Myextensions
    {
        public static byte[] ToBytes(this string val)
        {
            return val.Select((c, i) => (byte)c).ToArray();
        }
    }
}
