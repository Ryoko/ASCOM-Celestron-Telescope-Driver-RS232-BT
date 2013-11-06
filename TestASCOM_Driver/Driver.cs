//tabs=4
// --------------------------------------------------------------------------------
// TODO fill in this information for your driver, then remove this line!
//
// ASCOM Telescope driver for CelestronAdvancedBlueTooth
//
// Description:	Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam 
//				nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam 
//				erat, sed diam voluptua. At vero eos et accusam et justo duo 
//				dolores et ea rebum. Stet clita kasd gubergren, no sea takimata 
//				sanctus est Lorem ipsum dolor sit amet.
//
// Implements:	ASCOM Telescope interface version: <To be completed by driver developer>
// Author:		(XXX) Your N. Here <your@email.here>
//
// Edit Log:
//
// Date			Who	Vers	Description
// -----------	---	-----	-------------------------------------------------------
// dd-mmm-yyyy	XXX	6.0.0	Initial edit, created from ASCOM driver template
// --------------------------------------------------------------------------------
//


// This is used to define code in the template that is specific to one class implementation
// unused code canbe deleted and this definition removed.
//#define Telescope

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms.VisualStyles;
using ASCOM;
using ASCOM.Astrometry;
using ASCOM.Astrometry.AstroUtils;
using ASCOM.Astrometry.Exceptions;
using ASCOM.Utilities;
using ASCOM.DeviceInterface;
using System.Globalization;
using System.Collections;
using InTheHand.Net;
using InTheHand.Net.Sockets;
using System.Threading;

namespace ASCOM.CelestronAdvancedBlueTooth
{
    using System.Diagnostics;

    using ASCOM.CelestronAdvancedBlueTooth.CelestroneDriver.TelescopeWorker;
    using ASCOM.CelestronAdvancedBlueTooth.CelestroneDriver.Utils;

    using CelestroneDriver.HandForm;
    using CelestroneDriver.HardwareWorker;

    //
    // Your driver's DeviceID is ASCOM.CelestronAdvancedBlueTooth.Telescope
    //
    // The Guid attribute sets the CLSID for ASCOM.CelestronAdvancedBlueTooth.Telescope
    // The ClassInterface/None addribute prevents an empty interface called
    // _CelestronAdvancedBlueTooth from being created and used as the [default] interface
    //
    // TODO Replace the not implemented exceptions with code to implement the function or
    // throw the appropriate ASCOM exception.
    //

    /// <summary>
    /// ASCOM Telescope Driver for CelestronAdvancedBlueTooth.
    /// </summary>
    [Guid("ed7b0d10-7841-47d5-b9a3-755a00763c28")]
    [ClassInterface(ClassInterfaceType.None)]
    public partial class Telescope : ITelescopeV3
    {
        /// <summary>
        /// ASCOM DeviceID (COM ProgID) for this driver.
        /// The DeviceID is used by ASCOM applications to load the driver at runtime.
        /// </summary>
        internal static string driverID = "ASCOM.CelestronAdvancedBlueTooth.Telescope";
        // TODO Change the descriptive string for your driver then remove this line
        /// <summary>
        /// Driver description that displays in the ASCOM Chooser.
        /// </summary>
        private static string driverDescription = "Bluetooth/COM Driver for Celestron";
        private static TelescopeSettingsProfile profile = new TelescopeSettingsProfile();

        private static Telescope _telescopeV3;
        public static ITelescopeV3 TelescopeV3 {get { return _telescopeV3; }}
        private IHandControl _handControl;

        /// <summary>
        /// Private variable to hold the connected state
        /// </summary>
        private bool connectedState;
        private IDeviceWorker deviceWorker;
        /// <summary>
        /// Private variable to hold an ASCOM Utilities object
        /// </summary>
        private Util utilities;

        /// <summary>
        /// Private variable to hold an ASCOM AstroUtilities object to provide the Range method
        /// </summary>
        private AstroUtils astroUtilities;

        /// <summary>
        /// Private variable to hold the trace logger object (creates a diagnostic log file with information that you specify)
        /// </summary>
        private TraceLogger tl;

        //private DriverWorker driverWorker;
        private ITelescopeInteraction telescopeInteraction;
        private CelestroneDriver.TelescopeWorker.TelescopeWorker telescopeWorker;
        private TelescopeProperties telescopeProperties;
        public static TelescopeProperties TelescopeProperties {get { return _telescopeV3.telescopeProperties; }}
        /// <summary>
        /// Initializes a new instance of the <see cref="CelestronAdvancedBlueTooth"/> class.
        /// Must be public for COM registration.
        /// </summary>
        public Telescope()
        {
            ReadProfile(); // Read device configuration from the ASCOM Profile store
            _telescopeV3 = this;

            tl = new TraceLogger("", "CelestronAdvancedBlueTooth");
            tl.Enabled = profile.TraceState;
            tl.LogMessage("Telescope", "Starting initialisation");

            connectedState = false; // Initialise connected to false
            utilities = new Util(); //Initialise util object
            astroUtilities = new AstroUtils(); // Initialise astro utilities object
            try
            {
                //Initialize();
            }
            catch (Exception err)
            {
                
            }
            tl.LogMessage("Telescope", "Completed initialisation");
            _handControl = new HandControl();
            _handControl.SetForm(telescopeWorker);
        }

        public void Initialize()
        {
        }

        private void StopWorking()
        {
            if (telescopeWorker != null)
            {
                telescopeWorker.Disconnect();
            }
            if (deviceWorker != null)
            {
                deviceWorker.Disconnect();
                deviceWorker = null;
            }            
        }

        //
        // PUBLIC COM INTERFACE ITelescopeV3 IMPLEMENTATION
        //

        #region Common properties and methods.

        /// <summary>
        /// Displays the Setup Dialog form.
        /// If the user clicks the OK button to dismiss the form, then
        /// the new settings are saved, otherwise the old values are reloaded.
        /// THIS IS THE ONLY PLACE WHERE SHOWING USER INTERFACE IS ALLOWED!
        /// </summary>
        public void SetupDialog()
        {
            // consider only showing the setup dialog if not connected
            // or call a different dialog if connected
            if (IsConnected)
                System.Windows.Forms.MessageBox.Show("Already connected, just press OK");

            using (SetupDialogForm F = new SetupDialogForm(profile))
            {
                var result = F.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    WriteProfile(); // Persist device configuration values to the ASCOM Profile store
                }
            }
        }

        public ArrayList SupportedActions
        {
            get
            {
                tl.LogMessage("SupportedActions Get", "Returning empty arraylist");
                return new ArrayList(){"GetTrackingMode", "SetTrackingMode", "GetPosition"};
            }
        }

        public string Action(string actionName, string actionParameters)
        {
            switch (actionName)
            {
                case "GetTrackingMode":
                    return ((int)telescopeProperties.TrackingMode).ToString();
                case "SetTrackingMode":
                {
                    int mode;
                    if (int.TryParse(actionParameters, out mode))
                    {
                        if (mode < 0 || mode > 3) throw new ValueNotAvailableException("Wrong trackingrate: " + mode);
                        telescopeWorker.SetTrackingRate(DriveRates.driveSidereal, (TrackingMode)mode);
                        telescopeProperties.TrackingMode = (TrackingMode) mode;
                    }
                    return "";
                }
                case "GetPosition":
                    var res = telescopeInteraction.GetPosition();
                    return string.Format("{0};{1}", res.Azm, res.Alt);
            }
            throw new ASCOM.ActionNotImplementedException("Action " + actionName + " is not implemented by this driver");
        }

        public void CommandBlind(string command, bool raw)
        {
            telescopeInteraction.CommandBlind(command, raw);
        }

        public bool CommandBool(string command, bool raw)
        {
            return telescopeInteraction.CommandBool(command, raw);
        }

        public string CommandString(string command, bool raw)
        {
            return telescopeInteraction.CommandString(command, raw);
        }
        
        public void Dispose()
        {
            // Clean up the tracelogger and util objects
            StopWorking();
            tl.Enabled = false;
            tl.Dispose();
            tl = null;
            utilities.Dispose();
            utilities = null;
            astroUtilities.Dispose();
            astroUtilities = null;
        }


        public bool Connected
        {
            get
            {
                if(tl != null) tl.LogMessage("Connected Get", IsConnected.ToString());
                return IsConnected;
            }
            set
            {
                tl.LogMessage("Connected Set", value.ToString());
                if (value == IsConnected)
                    return;

                if (value)
                {
                    try
                    {
                        //Initialize();
                        StopWorking();
                        deviceWorker = profile.IsBluetooth ? (IDeviceWorker)new BluetoothWorker() : new ComPortWorker();
                        if (deviceWorker != null)
                        {
                            //driverWorker = new DriverWorker(this.CheckConnected, deviceWorker);
                            //telescopeInteraction = new CelestroneInteraction12(deviceWorker);
                        }

                        connectedState = true;
                        bool res;
                        if (profile.IsBluetooth)
                        {
                            tl.LogMessage(
                                "Connected Set", "Connecting to bluetooth device " + profile.BluetoothDevice.ToString());
                            res = this.deviceWorker.Connect(profile.BluetoothDevice);
                        }
                        else
                        {
                            tl.LogMessage("Connected Set", "Connecting to COM port " + profile.ComPort);
                            res = this.deviceWorker.Connect(profile.ComPort);
                        }
                        if (res)
                        {
                            telescopeWorker = CelestroneDriver.TelescopeWorker.TelescopeWorker.GetWorker(profile);
                            telescopeInteraction = ATelescopeInteraction.GeTelescopeInteraction(deviceWorker);
                            telescopeWorker.TelescopeInteraction = telescopeInteraction;
                            telescopeProperties = telescopeWorker.TelescopeProperties;
                            telescopeInteraction.isConnected = true;
                            var tBegin = Environment.TickCount;
                            while (true)
                            {
                                if (tBegin + 30000 < Environment.TickCount) throw new DriverException("Unable to get telescope parameters");
                                Thread.Sleep(100);
                                if (telescopeProperties.IsReady) break;
                            }
                            _handControl.ShowForm(profile.ShowControl);
                        }
                        else
                        {
                            throw new DriverException("Error connectiong to device");
                        }
                    }
                    catch(Exception err)
                    {
                        connectedState = false;
                        throw err;
                    }
                }
                else
                {
                    telescopeWorker.StopWorking();
                    telescopeInteraction.isConnected = false;
                    connectedState = false;
                    if (profile.IsBluetooth)
                    {
                        tl.LogMessage("Connected Set", "Disconnecting from bluetooth " + profile.BluetoothDevice.ToString());
                    }
                    else
                    {
                        tl.LogMessage("Connected Set", "Disconnecting from port " + profile.ComPort);
                    }
                    _handControl.ShowForm(false);
                    StopWorking();
                }
            }
        }

        public string Description
        {
            // TODO customise this device description
            get
            {
                tl.LogMessage("Description Get", driverDescription);
                return driverDescription;
            }
        }

        public string DriverInfo
        {
            get
            {
                Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                // TODO customise this driver description
                string driverInfo = "Information about the driver itself. Version: " + String.Format(CultureInfo.InvariantCulture, "{0}.{1}", version.Major, version.Minor);
                tl.LogMessage("DriverInfo Get", driverInfo);
                return driverInfo;
            }
        }

        public string DriverVersion
        {
            get
            {
                Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                string driverVersion = String.Format(CultureInfo.InvariantCulture, "{0}.{1}", version.Major, version.Minor);
                tl.LogMessage("DriverVersion Get", driverVersion);
                return driverVersion;
            }
        }

        public short InterfaceVersion
        {
            // set by the driver wizard
            get
            {
                tl.LogMessage("InterfaceVersion Get", "3");
                return Convert.ToInt16("3");
            }
        }

        public string Name
        {
            get
            {
                string name = "Celestrone Bluetooth";
                tl.LogMessage("Name Get", name);
                return name;
            }
        }

        #endregion

        //private string 


        #region Private properties and methods
        // here are some useful properties and methods that can be used as required
        // to help with driver development

        #region ASCOM Registration

        // Register or unregister driver for ASCOM. This is harmless if already
        // registered or unregistered. 
        //
        /// <summary>
        /// Register or unregister the driver with the ASCOM Platform.
        /// This is harmless if the driver is already registered/unregistered.
        /// </summary>
        /// <param name="bRegister">If <c>true</c>, registers the driver, otherwise unregisters it.</param>
        private static void RegUnregASCOM(bool bRegister)
        {
            using (var P = new ASCOM.Utilities.Profile())
            {
                P.DeviceType = "Telescope";
                if (bRegister)
                {
                    P.Register(driverID, driverDescription);
                }
                else
                {
                    P.Unregister(driverID);
                }
            }
        }

        /// <summary>
        /// This function registers the driver with the ASCOM Chooser and
        /// is called automatically whenever this class is registered for COM Interop.
        /// </summary>
        /// <param name="t">Type of the class being registered, not used.</param>
        /// <remarks>
        /// This method typically runs in two distinct situations:
        /// <list type="numbered">
        /// <item>
        /// In Visual Studio, when the project is successfully built.
        /// For this to work correctly, the option <c>Register for COM Interop</c>
        /// must be enabled in the project settings.
        /// </item>
        /// <item>During setup, when the installer registers the assembly for COM Interop.</item>
        /// </list>
        /// This technique should mean that it is never necessary to manually register a driver with ASCOM.
        /// </remarks>
        [ComRegisterFunction]
        public static void RegisterASCOM(Type t)
        {
            RegUnregASCOM(true);
        }

        /// <summary>
        /// This function unregisters the driver from the ASCOM Chooser and
        /// is called automatically whenever this class is unregistered from COM Interop.
        /// </summary>
        /// <param name="t">Type of the class being registered, not used.</param>
        /// <remarks>
        /// This method typically runs in two distinct situations:
        /// <list type="numbered">
        /// <item>
        /// In Visual Studio, when the project is cleaned or prior to rebuilding.
        /// For this to work correctly, the option <c>Register for COM Interop</c>
        /// must be enabled in the project settings.
        /// </item>
        /// <item>During uninstall, when the installer unregisters the assembly from COM Interop.</item>
        /// </list>
        /// This technique should mean that it is never necessary to manually unregister a driver from ASCOM.
        /// </remarks>
        [ComUnregisterFunction]
        public static void UnregisterASCOM(Type t)
        {
            RegUnregASCOM(false);
        }

        #endregion

        /// <summary>
        /// Returns true if there is a valid connection to the driver hardware
        /// </summary>
        private bool IsConnected
        {
            get
            {
                // TODO check that the driver hardware connection exists and is connected to the hardware
                return connectedState;
            }
        }

        /// <summary>
        /// Use this function to throw an exception if we aren't connected to the hardware
        /// </summary>
        /// <param name="message"></param>
        private void CheckConnected(string message)
        {
            if (!IsConnected)
            {
                throw new ASCOM.NotConnectedException(message);
            }
        }

        /// <summary>
        /// Read the device configuration from the ASCOM Profile store
        /// </summary>
        internal void ReadProfile()
        {
            profile.ReadProfile(driverID);
        }

        /// <summary>
        /// Write the device configuration to the  ASCOM  Profile store
        /// </summary>
        public void WriteProfile()
        {
            profile.WriteProfile(driverID);
        }

        #endregion
    }
}
