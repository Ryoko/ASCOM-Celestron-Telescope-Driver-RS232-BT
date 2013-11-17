
namespace ASCOM.CelestronAdvancedBlueTooth.CelestroneDriver.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Collections;
    using System.Diagnostics;
    using System.Reflection;

    using ASCOM.CelestronAdvancedBlueTooth.CelestroneDriver.TelescopeWorker;
    using ASCOM.Utilities;

    using InTheHand.Net;

    public interface IProfileRecord
    {
        string Name { get; set; }
        Type ValueType { get; }
        object Value { get; }
    }
    public class ProfileRecord<T> : IProfileRecord
    {
        public Type ValueType
        {
            get
            {
                return typeof(T);
            }
        }

        public object Value { get; set; }
        public string Name { get; set; }
        public T Default { get; set; }

        public ProfileRecord(string name, T defaultValue)
        {
            this.Value = this.Default = defaultValue;
        }
    }
    public class ProfileRecordAttribute : Attribute
    {
        public bool IsStored { get; set; }
        public string DefaultValue { get; set; }
        public string Name { get; set; }

        public ProfileRecordAttribute(string name, string Default = "", bool isStored = true)
        {
            Name = name;
            IsStored = isStored;
            DefaultValue = Default;
        }
    }

    public class TelescopeSettingsProfile : IEnumerable<string>
    {
        private Dictionary<string, IProfileRecord> properties = new Dictionary<string, IProfileRecord>()
                        {
                            {"comPort", new ProfileRecord<string>("COM Port", "COM1")},
                            {"traceState", new ProfileRecord<bool>("Trace Level", true)},
                            {"bluetoothDevice", new ProfileRecord<BluetoothAddress>("BlueTooth Device", null)},
                            {"isBluetooth", new ProfileRecord<bool>("IsBluetooth", true)},
                            {"longitude", new ProfileRecord<double>("Longitude", -1000)},
                            {"latitude", new ProfileRecord<double>("Latitude", -1000)},
                            {"elevation", new ProfileRecord<double>("Elevation Level", 0)},
                            {"hasGPS", new ProfileRecord<int>("Telescope has GPS", -1)},
                            {"apperture", new ProfileRecord<double>("Telescope Apperture", 0)},
                            {"focal", new ProfileRecord<double>("Telescope Focal Length", 0)},
                            {"obstruction", new ProfileRecord<double>("obstruction", 0)},
                            {"TelescopeModel", new ProfileRecord<string>("TelescopeModel", "")},
                            {"TrackingMode", new ProfileRecord<int>("Tracking Mode", -1)},
                            {"showControl", new ProfileRecord<bool>("Show Control OnConnect", false)},
                            {"ParkAlt", new ProfileRecord<double>("Park Altitude", 0)},
                            {"ParkAzm", new ProfileRecord<double>("Park Azimuth", 0)},
                            {"HomeAlt", new ProfileRecord<double>("Home Altitude", 0)},
                            {"HomeAzm", new ProfileRecord<double>("Home Azimuth", 0)},
                            {"IsAtPark", new ProfileRecord<bool>("Is At Park", false)},
                        };

        //public ProfileRecord<string> comPort = ProfileRecord<string>("COM Port", "COM1");
        //public ProfileRecord<bool> traceState = ProfileRecord<bool>("Trace Level", true);
        //public ProfileRecord<BluetoothAddress> bluetoothDevice = ProfileRecord<BluetoothAddress>("BlueTooth Device", null);
        //public ProfileRecord<bool> isBluetooth = ProfileRecord<bool>("IsBluetooth", true);
        //public ProfileRecord<double> longitude = ProfileRecord<double>("Longitude", -1000);
        //public ProfileRecord<double> latitude = ProfileRecord<double>("Latitude", -1000);
        //public ProfileRecord<double> elevation = ProfileRecord<double>("Elevation Level", 0);
        //public ProfileRecord<int> hasGPS = ProfileRecord<int>("Telescope has GPS", -1);
        //public ProfileRecord<double> apperture = ProfileRecord<double>("Telescope Apperture", 0);
        //public ProfileRecord<double> focal = ProfileRecord<double>("Telescope Focal Length", 0);
        //public ProfileRecord<double> obstruction = ProfileRecord<double>("obstruction", 0);
        //public ProfileRecord<string> TelescopeModel = ProfileRecord<string>("TelescopeModel", "");
        //public ProfileRecord<int> TrackingMode = ProfileRecord<int>("Tracking Mode", -1);
        //public ProfileRecord<bool> showControl = ProfileRecord<bool>("Show Control OnConnect", false);
        //public ProfileRecord<double> ParkAlt = ProfileRecord<double>("Park Altitude", 0);
        //public ProfileRecord<double> ParkAzm = ProfileRecord<double>("Park Azimuth", 0);
        //public ProfileRecord<double> HomeAlt = ProfileRecord<double>("Home Altitude", 0);
        //public ProfileRecord<double> HomeAzm = ProfileRecord<double>("Home Azimuth", 0);
        //public ProfileRecord<bool> IsAtPark = ProfileRecord<bool>("Is At Park", false);

        [ProfileRecord("COM Port", "COM1")]
        public string ComPort; // Variables to hold the currrent device configuration

        [ProfileRecord("Trace Level", "false")]
        public bool TraceState = true;

        [ProfileRecord("BlueTooth Device")]
        public BluetoothAddress BluetoothDevice;

        [ProfileRecord("IsBluetooth", "false")]
        public bool IsBluetooth;

        [ProfileRecord("Longitude", "-1000")]
        public double Longitude;

        [ProfileRecord("Latitude", "-1000")]
        public double Latitude;

        [ProfileRecord("Elevation Level", "0")]
        public double Elevation;

        [ProfileRecord("Telescope has GPS", "-1")]
        public int HasGps;

        [ProfileRecord("Telescope Apperture", "0")]
        public double Apperture;

        [ProfileRecord("Telescope Focal Length", "0")]
        public double Focal;

        [ProfileRecord("Telescope Obstruction Percent", "0")]
        public double Obstruction;

        [ProfileRecord("Telescope Model")]
        public string TelescopeModel;

        [ProfileRecord("Tracking Mode", "-1")]
        public int TrackingMode;

        [ProfileRecord("Show Control OnConnect", "0")]
        public bool ShowControl;

        [ProfileRecord("Park Altitude", "90")]
        public double ParkAlt;

        [ProfileRecord("Park Azimuth", "90")]
        public double ParkAzm;

        [ProfileRecord("Home Altitude", "90")]
        public double HomeAlt;

        [ProfileRecord("Home Alzimuth", "90")]
        public double HomeAzm;

        [ProfileRecord("Is At Park", "false")]
        public bool IsAtPark;

        [ProfileRecord("Target Coordinates", "0", false)]
        public Coordinates Target;

        public TelescopeSettingsProfile()
        {

        }

        public void ReadProfile(string driverId)
        {
            //Debugger.Break();
            using (Profile driverProfile = new Profile())
            {
                foreach (var fieldInfo in this.GetType().GetFields())
                {
                    var at = fieldInfo.GetCustomAttributes(typeof(ProfileRecordAttribute), false);
                    if (at.Length == 0) continue;
                    var pra = (ProfileRecordAttribute)at[0];
                    if (!pra.IsStored) continue;
                    try
                    {
                        var value = driverProfile.GetValue(driverId, pra.Name, string.Empty, pra.DefaultValue);
                        fieldInfo.SetValue(this, Convert.ChangeType(value, fieldInfo.FieldType));
                    }
                    catch
                    {
                        
                    }
                }

            }
#if DEBUG
            //return;
            if (this.BluetoothDevice != null || this.TelescopeModel.Length != 0)
            {
                return;
            }
            this.TraceState = true;
            this.ComPort = "COM1";
            BluetoothAddress.TryParse("001112280143", out this.BluetoothDevice);
            //bluetoothDevice = new BluetoothAddress(001112280143);
            this.IsBluetooth = true;
            this.TrackingMode = (int) CelestroneDriver.TelescopeWorker.TrackingMode.EQN;
            this.TelescopeModel = "Advanced C8-NGT";
            this.HasGps = 0;
            this.Latitude = 53.9175;
            this.Longitude = 27.529722222;
            this.Elevation = 295;
            this.Apperture = 0.200;
            this.Focal = 1d;
            this.Obstruction = 28d;
            this.ShowControl = true;
#endif 
        }

        public void WriteProfile(string driverID)
        {
            using (var driverProfile = new Profile())
            {
                driverProfile.DeviceType = "Telescope";
                foreach (var fieldInfo in this.GetType().GetFields())
                {
                    var at = fieldInfo.GetCustomAttributes(typeof(ProfileRecordAttribute), false);
                    if (at.Length == 0) continue;
                    var pra = (ProfileRecordAttribute)at[0];
                    if (!pra.IsStored) continue;
                    try
                    {
                        driverProfile.GetValue(driverID, pra.Name, string.Empty, pra.DefaultValue);
                        var value = fieldInfo.GetValue(this).ToString();
                        driverProfile.WriteValue(driverID, pra.Name, value);
                    }catch{}
                }

                //driverProfile.WriteValue(driverID, this.traceStateProfileName, this.traceState.ToString());
                //driverProfile.WriteValue(driverID, this.comPortProfileName, this.comPort);
                //driverProfile.WriteValue(driverID, this.bluetoothDeviceProfileName, this.bluetoothDevice != null ? this.bluetoothDevice.ToString() : string.Empty);
                //driverProfile.WriteValue(driverID, this.isBluetoothProfileName, this.isBluetooth.ToString());
                //driverProfile.WriteValue(driverID, this.TrackingModeProfileName, this.trackingMode.ToString());
                //driverProfile.WriteValue(driverID, this.TelescopeModelProfileName,this.TelescopeModel);
                //driverProfile.WriteValue(driverID, this.hasGPSProfileName, this.hasGPS.ToString());
                //driverProfile.WriteValue(driverID, this.latitudeProfileName, this.latitude.ToString());
                //driverProfile.WriteValue(driverID, this.longitudeProfileName, this.longitude.ToString());
                //driverProfile.WriteValue(driverID, this.elevationProfileName, this.elevation.ToString());
                //driverProfile.WriteValue(driverID, this.appertureProfileName, this.apperture.ToString());
                //driverProfile.WriteValue(driverID, this.focalProfileName, this.focal.ToString());
                //driverProfile.WriteValue(driverID, this.obstructionProfileName, this.obstruction.ToString());
                //driverProfile.WriteValue(driverID, this.showControlProfileName, this.showControl.ToString());

                //driverProfile.WriteValue(driverID, this.HomeAltProfileName, this.HomeAlt.ToString());
                //driverProfile.WriteValue(driverID, this.HomeAzmProfileName, this.HomeAzm.ToString());
                //driverProfile.WriteValue(driverID, this.ParkAltProfileName, this.ParkAlt.ToString());
                //driverProfile.WriteValue(driverID, this.ParkAzmProfileName, this.ParkAzm.ToString());
                //driverProfile.WriteValue(driverID, this.AtParkProfileName, this.IsAtPark.ToString());

            }

        }

        public IProfileRecord this[string name]
        {
            get
            {
                return this.properties.ContainsKey(name) ? this.properties[name] : null;
            }
        }

        public IEnumerator<string> GetEnumerator()
        {
            return this.properties.Keys.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }

}
