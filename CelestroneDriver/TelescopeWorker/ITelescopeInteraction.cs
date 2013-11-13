namespace ASCOM.CelestronAdvancedBlueTooth.CelestroneDriver.TelescopeWorker
{
    using System;

    using ASCOM.CelestronAdvancedBlueTooth.CelestroneDriver.Utils;

    public enum Direction { Positive = 6, Negative = 7};
    public enum SlewAxes { RaAzm = 16, DecAlt = 17 };
    public enum DeviceID { Main = 1, HC = 4, RaAzmMotor = 16, DecAltMotor = 17, GPSUnit = 176, RTC = 178 }
    public enum TelescopeModel { Unknown = 0, GPSSeries = 1, iSeries = 3, iSeriesSE, CGE, AdvancedGT, SLT, CPC = 9, GT, SE45, SE68 }
    public enum TrackingMode { Unknown = -1, Off = 0, AltAzm, EQN, EQS }

    public enum SiteOfPier {Unknown = -1, East = 0, West}

    public enum DeviceCommands
    {
        MC_GET_POSITION = 1, //    n/a    24 bits
        MC_GOTO_FAST = 2, //   16/24 bits Ack

        MC_SET_POSITION = 4, //   24 bits Ack     24 bits for a rotation

        // continuously variable move rate in units of 0.25 arc sec per second
        MC_SET_POS_VARIABLE_GUIDERATE = 6, //   24 bits n/a
        MC_SET_NEG_VARIABLE_GUIDERATE = 7, //   24 bits n/a

        MC_SEEK_SWITCH = 0xB, //   n/a     Ack     Start the move to the switch position
        MC_PEC_RECORD_START = 0xC, //   n/a     Ack     Start recording PEC position
        MC_PEC_PLAYBACK = 0xD, //   8 bits  Ack     Start(01)/stop(00) PEC playback

        MC_AT_SWITCH = 0x12, //   n/a     8 bits, FFH is seek switch completed
        MC_SLEW_DONE = 0x13, //    n/a    8 bits, FFH is slew completed

        MC_PEC_RECORD_DONE = 0x15, //   n/a     8 bits  FFH ia PEC record completed
        MC_PEC_RECORD_STOP = 0x16, //   n/a     n/a     Stop PEC recording

        MC_GOTO_SLOW = 0x17,
        MC_AT_INDEX = 0x18, //   n/a     8 bits  FFH at index, 00H not
        MC_SEEK_INDEX = 0x19, //   n/a     n/a     Seek PEC Index

        // emulate the HC buttons, value is rate 0-9 where 0 is stop
        MC_SET_POS_FIXED_GUIDERATE = 0x24, //      8    n/a     move positive (up/right)
        MC_SET_NEG_FIXED_GUIDERATE = 0x25, //      8    n/a     Move negative (down/left)

        // the new hardware based guide commands
        MTR_AUX_GUIDE = 0x26,
//        Command is followed by a signed char: velocity [% sidereal]; and
//        unsigned char: duration [10 ms units].

        MTR_IS_AUX_GUIDE_ACTIVE = 0x27,
        //MC returns TRUE(1) if last AUX_GUIDE command//s is still in effect.
        //MC returns FALSE(0) if last AUX_GUIDE command has expired.

        // set/get the autoguide rates as a percentage of sidereal
        MC_SET_AUTOGUIDE_RATE = 0x46, // 8    n/a     Set percentage of sidereal
        MC_GET_AUTOGUIDE_RATE = 0x47, // 8    n/a     % = 100, * val/256

//   GPS commands
        GPS_GET_LAT = 1, //   n/a     24 Bits
        GPS_GET_LONG = 2, //   n/a     24 Bits
        GPS_GET_DATE = 3, //   n/a     16 Bits month|day
        GPS_GET_YEAR = 4, //   n/a     16 Bits
        GPS_GET_TIME = 0x33, //   n/a     24 bits hours|minutes|seconds
        GPS_TIME_VALID = 0x36, //   n/a     8 bits  0=False, 1=True
        GPS_LINKED = 0x37, //   n/a     8 bits  0=False, 1=True

//   RTS commands
        RTS_SET_DATE = 0x83,
        RTS_SET_YEAR = 0x84,
        RTS_SET_TIME = 0xB3,

//   All modules
        GET_VER = 0xFE, //   n/a     16 bits major|minor

    }

    public enum GeneralCommands
    {
        CR = 0xD,			    // Carriage return
        COMMA = 0x2C,			// comma
        QUESTION = 0x3F,			// ? command initialise for NS 5 and 8
        TERMINATOR = 0x23,		// # command terminator

        _0 = 0x30,			    // "0"
        _1 = 0x31,			    // "1"
        _9 = 0x39,			    // "9"
        _A = 0x41,			    // "A"
        _E = 0x45,			    // "E"
        _F = 0x46,			    // "F"
        _W = 0x57,			    // "W"

        SET_ALTAZ = 0x41,		// A Slew AltAz, Nexstar 5 and 8 only
        SET_ALTAZ_LP = 0x42,	// B Slew AltAz Low precision
        SET_CONFIG_ITEM = 0x43,	// C Set configuration item
        GET_RADEC_LP = 0x45,	// E Get Ra Dec Low precision
        SYNC = 0x46,			// F Sync, Ultima2K only
        SET_TIME = 0x48,		// H Set local time
        SET_TIME_NEW = 0x49,	// I Set local time, 4.21 onwards
        IS_ALIGNED = 0x4A,		// J get scope aligned state
        ECHO_CMD = 0x4B,		// K echo command
        IS_SLEWING = 0x4C,		// L get slewing state
        ABORT_SLEW = 0x4D,		// M Abort Slew
        SET_RADEC_LP = 0x52,	// R set Ra Dec Low precision
        SYNC_LP = 0x53,			// S sync low precision
        SET_TRACKING = 0x54,	// T Set Tracking
        GET_VERSION = 0x56,		// V get Version command
        AUX_CMD = 0x50,			// P Aux command
        SET_LOCATION = 0x57,	// W set Location
        LAST_ALIGN = 0x59,		// Y Last Align or Quick Align
        GET_ALTAZ_LP = 0x5A,	// Z Get AltAz Low precision

        SET_ALTAZ_HP = 0x62,	// b Slew AltAz high precision
        GET_CONFIG_ITEM = 0x63,	// c Get Confguration item
        GET_RADEC_HP = 0x65,	// e Get Ra Dec high precision

        GET_DEST_PIER_SIDE = 0x67,	    // g Get the destination side of pier
        GET_TIME = 0x68,			    // h get local time
        GET_TIME_NEW = 0x69,			// i get local time, 4.21 onwards
        GET_LST = 0x6C,			        // l get local sidereal time
        GET_MODEL = 0x6D,			    // m Get scope model
        SET_HOME = 0x6E,			    // n Set home position, 4.21 onwards
        GOTO_HOME = 0x6F,			    // o goto home position, 4.21 onwards
        GET_PIER_SIDE = 0x70,		    // p Get Pier Side      4.14
        SET_RADEC_HP = 0x72,			// r set Ra Dec High precision
        SYNC_HP = 0x73,			        // s sync high precision
        GET_TRACKING = 0x74,			// t get Tracking state
        UNDO_SYNC = 0x75,			    // u undo sync
        GET_LOCATION = 0x77,			// w Get location
        HIBERNATE = 0x78,			    // x Hibernate, 4.21 onwards
        WAKEUP = 0x79,			        // y Wake up, parameter 0 or 1, 4.21 onwards
        GET_ALTAZ_HP = 0x7A,			// z Get Alt Az High precision
    }

    //public enum DeviceIds { Main = 1, HC = 4, AzmDrive = 16, AltDrive = 17, GPS = 176, RTC = 0xB2 }
    public interface ITelescopeInteraction
    {
        //ICelestroneTelescopeWorker(IDeviceWorker dw);
        bool isConnected { get; set; }
        AltAzm AltAzm { get; set; }
        Coordinates RaDec { get; set; }
        void SyncAltAz(AltAzm coordinates);
        void SyncRaDec(Coordinates coordinates);
        TrackingMode TrackingMode { get; set; }
        /// <summary>
        /// Slew with Variable Rate (deg/sec)
        /// </summary>
        /// <param name="dir">Direction</param>
        /// <param name="axis">Axis</param>
        /// <param name="rate">Rate (deg/second)</param>
        void SlewVariableRate(SlewAxes axis, double rate);
        void SlewFixedRate(SlewAxes axis, int rate);
        /// <summary>
        /// Slew with Variable Rate High Precision (deg/sec)
        /// </summary>
        /// <param name="dir">Direction</param>
        /// <param name="axis">Axis</param>
        /// <param name="rate">Rate (deg/second)</param>
        void SlewHighRate(SlewAxes axis, double rate);
        DateTime TelescopeDateTime { get; set; }
        LatLon TelescopeLocation { get; set; }
        bool IsGPS { get; }
        DateTime GpsDateTime { get; }
        LatLon GPSLocation { get; }
        DateTime RTCDateTime { get; set; }
        double FirmwareVersion { get; }
        double GetDeviceVersion(DeviceID device);
        TelescopeModel GetModel { get; }
        bool IsAlignmentComplete { get; }
        bool IsGoToInProgress { get; }
        void CancelGoTo();
        //bool SetTrackingRate(DriveRates rate, TrackingMode mode);
        double VersionRequired { get; }
        void GoToPosition(AltAzm position);
        /// <summary>
        /// Get Position {Azm, Alt} 
        /// </summary>
        /// <returns>{Azm, Alt}</returns>
        AltAzm GetPosition();
        void SetHome();
        void GoHome();

        SiteOfPier GetSiteOfPier();
        SiteOfPier GetDestinationSiteOfPier(Coordinates coord);

        void Hibernate();
        void WakeUp();

        bool IsSlewDone(DeviceID deviceId);

        bool CanSyncAltAzm { get; }
        bool CanSyncRaDec { get; }
        bool CanSetTracking { get; }
        bool CanGetTracking { get; }
        bool CanSlewFixedRate { get; }
        bool CanSlewVariableRate { get; }
        bool CanWorkDateTime { get; }
        bool CanWorkLocation { get; }
        bool CanWorkGPS { get; }
        bool CanGetRTC { get; }
        bool CanSetRTC { get; }
        bool CanGetModel { get; }
        bool CanGetDeviceVersion { get; }
        bool CanSetTrackingRates { get; }
        bool CanSlewHighRate { get; }
        bool CanWorkPosition { get; }
        bool CanGetSiteOfPier { get; }
        bool CanHibernate { get; }
        bool CanWorkHome { get; }

        byte[] SendCommandToDevice(DeviceID DeviceId, DeviceCommands Command, byte NoOfAnsvers, params byte[] args);

        bool CommandBool(string command, bool raw);
        void CommandBlind(string command, bool raw);
        string CommandString(string command, bool raw);

    }

    public class TelescopeInteractionAttribute : Attribute
    {
        public TelescopeInteractionAttribute(double versionRequired)
        {
            this.RequiredVersion = versionRequired;
        }
        public double RequiredVersion { get; private set; }
        public TelescopeModel[] TelescopeModels { get; set; } 
    }
}
