
namespace ASCOM.CelestronAdvancedBlueTooth.CelestroneDriver.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Collections;

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
                this.traceState = Convert.ToBoolean(driverProfile.GetValue(driverID, this.traceStateProfileName, string.Empty, this.traceStateDefault));
                this.comPort = driverProfile.GetValue(driverID, this.comPortProfileName, string.Empty, this.comPortDefault);
                var val = driverProfile.GetValue(driverID, this.bluetoothDeviceProfileName, string.Empty, string.Empty);
                BluetoothAddress.TryParse(val, out this.bluetoothDevice);
                val = driverProfile.GetValue(driverID, this.isBluetoothProfileName, string.Empty, "false");
                bool.TryParse(val, out this.isBluetooth);
                val = driverProfile.GetValue(driverID, this.latitudeProfileName, string.Empty, this.coordinateDefaultValue);
                double.TryParse(val, out this.latitude);
                val = driverProfile.GetValue(driverID, this.longitudeProfileName, string.Empty, this.coordinateDefaultValue);
                double.TryParse(val, out this.longitude);
                val = driverProfile.GetValue(driverID, this.TrackingModeProfileName, string.Empty, this.TrackingModeDefault.ToString());
                int.TryParse(val, out this.trackingMode);
                this.TelescopeModel = driverProfile.GetValue(driverID, this.TelescopeModelProfileName, string.Empty, string.Empty);
                val = driverProfile.GetValue(driverID, this.hasGPSProfileName, string.Empty, this.hasGPSDefault);
                int.TryParse(val, out this.hasGPS);
                val = driverProfile.GetValue(driverID, this.elevationProfileName, string.Empty, this.defaultDouble);
                double.TryParse(val, out this.elevation);
                val = driverProfile.GetValue(driverID, this.appertureProfileName, string.Empty, this.defaultDouble);
                double.TryParse(val, out this.apperture);
                val = driverProfile.GetValue(driverID, this.focalProfileName, string.Empty, this.defaultDouble);
                double.TryParse(val, out this.focal);
                val = driverProfile.GetValue(driverID, this.obstructionProfileName, string.Empty, this.defaultDouble);
                double.TryParse(val, out this.obstruction);
                val = driverProfile.GetValue(driverID, this.showControlProfileName, string.Empty, "false");
                bool.TryParse(val, out this.showControl);

                var posValid = true;
                val = driverProfile.GetValue(driverID, this.HomeAltProfileName, string.Empty, string.Empty);
                posValid &= double.TryParse(val, out this.HomeAlt);
                val = driverProfile.GetValue(driverID, this.HomeAzmProfileName, string.Empty, string.Empty);
                posValid &= double.TryParse(val, out this.HomeAzm);
                if (!posValid) 
                {
                    this.HomeAlt = 90;
                    this.HomeAzm = 90;
                }
                posValid = true;
                val = driverProfile.GetValue(driverID, this.ParkAltProfileName, string.Empty, string.Empty);
                posValid &= double.TryParse(val, out this.ParkAlt);
                val = driverProfile.GetValue(driverID, this.ParkAzmProfileName, string.Empty, string.Empty);
                posValid &= double.TryParse(val, out this.ParkAzm);
                if (!posValid)
                {
                    this.ParkAlt = 0;
                    this.ParkAzm = 90;
                }

                val = driverProfile.GetValue(driverID, this.AtParkProfileName, string.Empty, this.defaultDouble);
                bool.TryParse(val, out this.IsAtPark);

            }
#if DEBUG
            //return;
            if (this.bluetoothDevice == null && this.TelescopeModel.Length == 0)
            {
                this.traceState = true;
                this.comPort = "COM1";
                BluetoothAddress.TryParse("001112280143", out this.bluetoothDevice);
                //bluetoothDevice = new BluetoothAddress(001112280143);
                this.isBluetooth = true;
                this.trackingMode = (int) TrackingMode.EQN;
                this.TelescopeModel = "Advanced C8-NGT";
                this.hasGPS = 0;
                this.latitude = 53.9175;
                this.longitude = 27.529722222;
                this.elevation = 295;
                this.apperture = 0.200;
                this.focal = 1d;
                this.obstruction = 28d;
                this.showControl = true;
            }
#endif 
        }

        public void WriteProfile(string driverID)
        {
            using (Profile driverProfile = new Profile())
            {
                driverProfile.DeviceType = "Telescope";
                driverProfile.WriteValue(driverID, this.traceStateProfileName, this.traceState.ToString());
                driverProfile.WriteValue(driverID, this.comPortProfileName, this.comPort);
                driverProfile.WriteValue(driverID, this.bluetoothDeviceProfileName, this.bluetoothDevice != null ? this.bluetoothDevice.ToString() : string.Empty);
                driverProfile.WriteValue(driverID, this.isBluetoothProfileName, this.isBluetooth.ToString());
                driverProfile.WriteValue(driverID, this.TrackingModeProfileName, this.trackingMode.ToString());
                driverProfile.WriteValue(driverID, this.TelescopeModelProfileName,this.TelescopeModel);
                driverProfile.WriteValue(driverID, this.hasGPSProfileName, this.hasGPS.ToString());
                driverProfile.WriteValue(driverID, this.latitudeProfileName, this.latitude.ToString());
                driverProfile.WriteValue(driverID, this.longitudeProfileName, this.longitude.ToString());
                driverProfile.WriteValue(driverID, this.elevationProfileName, this.elevation.ToString());
                driverProfile.WriteValue(driverID, this.appertureProfileName, this.apperture.ToString());
                driverProfile.WriteValue(driverID, this.focalProfileName, this.focal.ToString());
                driverProfile.WriteValue(driverID, this.obstructionProfileName, this.obstruction.ToString());
                driverProfile.WriteValue(driverID, this.showControlProfileName, this.showControl.ToString());

                driverProfile.WriteValue(driverID, this.HomeAltProfileName, this.HomeAlt.ToString());
                driverProfile.WriteValue(driverID, this.HomeAzmProfileName, this.HomeAzm.ToString());
                driverProfile.WriteValue(driverID, this.ParkAltProfileName, this.ParkAlt.ToString());
                driverProfile.WriteValue(driverID, this.ParkAzmProfileName, this.ParkAzm.ToString());
                driverProfile.WriteValue(driverID, this.AtParkProfileName, this.IsAtPark.ToString());

            }

        }


        public IProfileRecord this[string name]
        {
            get
            {
                if (this.properties.ContainsKey(name)) return this.properties[name];
                else return null;
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
