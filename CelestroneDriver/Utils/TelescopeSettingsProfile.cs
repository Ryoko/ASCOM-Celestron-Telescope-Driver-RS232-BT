
namespace CelestroneDriver.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Collections;
    using ASCOM.Utilities;
    using CelestroneDriver.TelescopeWorker;

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
            Value = Default = defaultValue;
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
        public string comPortProfileName = "COM Port"; // Constants used for Profile persistence
        public string comPortDefault = "COM1";
        public string comPort; // Variables to hold the currrent device configuration

        public string traceStateProfileName = "Trace Level";
        public string traceStateDefault = "false";
        public bool traceState = true;

        public string bluetoothDeviceProfileName = "BlueTooth Device";
        public BluetoothAddress bluetoothDevice;

        public string isBluetoothProfileName = "IsBluetooth";
        public bool isBluetooth;

        public string coordinateDefaultValue = "-1000";

        public string longitudeProfileName = "Longitude";
        public double longitude;
        public string latitudeProfileName = "Latitude";
        public double latitude;

        public string elevationProfileName = "Elevation Level";
        public double elevation;

        public string hasGPSProfileName = "Telescope has GPS";
        public string hasGPSDefault = "-1";
        public int hasGPS;

        public string defaultDouble = "0";
        public string appertureProfileName = "Telescope Apperture";
        public double apperture;

        public string focalProfileName = "Telescope Focal Length";
        public double focal;

        public string obstructionProfileName = "Telescope Obstruction Percent";
        public double obstruction;

        public string TelescopeModelProfileName = "Telescope Model";
        public string TelescopeModel;

        public string TrackingModeProfileName = "Tracking Mode";
        public int TrackingModeDefault = -1;
        public int trackingMode;

        public string showControlProfileName = "Show Control OnConnect";
        public bool showControl = false;

        public string ParkAltProfileName = "Park Altitude";
        public double ParkAlt = 0;
        public string ParkAzmProfileName = "Park Azimuth";
        public double ParkAzm = 0;
        public string HomeAltProfileName = "Home Altitude";
        public double HomeAlt = 0;
        public string HomeAzmProfileName = "Home Alzimuth";
        public double HomeAzm = 0;
        public string AtParkProfileName = "Is At Park";
        public bool IsAtPark;

        public Coordinates target;

        public TelescopeSettingsProfile()
        {

        }

        public void ReadProfile(string driverID)
        {
            using (Profile driverProfile = new Profile())
            {
                traceState = Convert.ToBoolean(driverProfile.GetValue(driverID, traceStateProfileName, string.Empty, traceStateDefault));
                comPort = driverProfile.GetValue(driverID, comPortProfileName, string.Empty, comPortDefault);
                var val = driverProfile.GetValue(driverID, bluetoothDeviceProfileName, string.Empty, string.Empty);
                BluetoothAddress.TryParse(val, out bluetoothDevice);
                val = driverProfile.GetValue(driverID, isBluetoothProfileName, string.Empty, "false");
                bool.TryParse(val, out isBluetooth);
                val = driverProfile.GetValue(driverID, latitudeProfileName, string.Empty, coordinateDefaultValue);
                double.TryParse(val, out latitude);
                val = driverProfile.GetValue(driverID, longitudeProfileName, string.Empty, coordinateDefaultValue);
                double.TryParse(val, out longitude);
                val = driverProfile.GetValue(driverID, TrackingModeProfileName, string.Empty, TrackingModeDefault.ToString());
                int.TryParse(val, out trackingMode);
                TelescopeModel = driverProfile.GetValue(driverID, TelescopeModelProfileName, string.Empty, string.Empty);
                val = driverProfile.GetValue(driverID, hasGPSProfileName, string.Empty, hasGPSDefault);
                int.TryParse(val, out hasGPS);
                val = driverProfile.GetValue(driverID, elevationProfileName, string.Empty, defaultDouble);
                double.TryParse(val, out elevation);
                val = driverProfile.GetValue(driverID, appertureProfileName, string.Empty, defaultDouble);
                double.TryParse(val, out apperture);
                val = driverProfile.GetValue(driverID, focalProfileName, string.Empty, defaultDouble);
                double.TryParse(val, out focal);
                val = driverProfile.GetValue(driverID, obstructionProfileName, string.Empty, defaultDouble);
                double.TryParse(val, out obstruction);
                val = driverProfile.GetValue(driverID, showControlProfileName, string.Empty, "false");
                bool.TryParse(val, out showControl);

                var posValid = true;
                val = driverProfile.GetValue(driverID, HomeAltProfileName, string.Empty, string.Empty);
                posValid &= double.TryParse(val, out HomeAlt);
                val = driverProfile.GetValue(driverID, HomeAzmProfileName, string.Empty, string.Empty);
                posValid &= double.TryParse(val, out HomeAzm);
                if (!posValid) 
                {
                    HomeAlt = 90;
                    HomeAzm = 90;
                }
                posValid = true;
                val = driverProfile.GetValue(driverID, ParkAltProfileName, string.Empty, string.Empty);
                posValid &= double.TryParse(val, out ParkAlt);
                val = driverProfile.GetValue(driverID, ParkAzmProfileName, string.Empty, string.Empty);
                posValid &= double.TryParse(val, out ParkAzm);
                if (!posValid)
                {
                    ParkAlt = 0;
                    ParkAzm = 90;
                }

                val = driverProfile.GetValue(driverID, AtParkProfileName, string.Empty, defaultDouble);
                bool.TryParse(val, out IsAtPark);

            }
#if DEBUG
            //return;
            if (bluetoothDevice == null && TelescopeModel.Length == 0)
            {
                traceState = true;
                comPort = "COM1";
                BluetoothAddress.TryParse("001112280143", out bluetoothDevice);
                //bluetoothDevice = new BluetoothAddress(001112280143);
                isBluetooth = true;
                trackingMode = (int) TrackingMode.EQN;
                TelescopeModel = "Advanced C8-NGT";
                hasGPS = 0;
                latitude = 53.9175;
                longitude = 27.529722222;
                elevation = 295;
                apperture = 0.200;
                focal = 1d;
                obstruction = 28d;
                showControl = true;
            }
#endif 
        }

        public void WriteProfile(string driverID)
        {
            using (Profile driverProfile = new Profile())
            {
                driverProfile.DeviceType = "Telescope";
                driverProfile.WriteValue(driverID, traceStateProfileName, traceState.ToString());
                driverProfile.WriteValue(driverID, comPortProfileName, comPort);
                driverProfile.WriteValue(driverID, bluetoothDeviceProfileName, bluetoothDevice != null ? bluetoothDevice.ToString() : string.Empty);
                driverProfile.WriteValue(driverID, isBluetoothProfileName, isBluetooth.ToString());
                driverProfile.WriteValue(driverID, TrackingModeProfileName, trackingMode.ToString());
                driverProfile.WriteValue(driverID, TelescopeModelProfileName,TelescopeModel);
                driverProfile.WriteValue(driverID, hasGPSProfileName, hasGPS.ToString());
                driverProfile.WriteValue(driverID, latitudeProfileName, latitude.ToString());
                driverProfile.WriteValue(driverID, longitudeProfileName, longitude.ToString());
                driverProfile.WriteValue(driverID, elevationProfileName, elevation.ToString());
                driverProfile.WriteValue(driverID, appertureProfileName, apperture.ToString());
                driverProfile.WriteValue(driverID, focalProfileName, focal.ToString());
                driverProfile.WriteValue(driverID, obstructionProfileName, obstruction.ToString());
                driverProfile.WriteValue(driverID, showControlProfileName, showControl.ToString());

                driverProfile.WriteValue(driverID, HomeAltProfileName, HomeAlt.ToString());
                driverProfile.WriteValue(driverID, HomeAzmProfileName, HomeAzm.ToString());
                driverProfile.WriteValue(driverID, ParkAltProfileName, ParkAlt.ToString());
                driverProfile.WriteValue(driverID, ParkAzmProfileName, ParkAzm.ToString());
                driverProfile.WriteValue(driverID, AtParkProfileName, IsAtPark.ToString());

            }

        }


        public IProfileRecord this[string name]
        {
            get
            {
                if (properties.ContainsKey(name)) return properties[name];
                else return null;
            }
        }

        public IEnumerator<string> GetEnumerator()
        {
            return properties.Keys.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }

}
