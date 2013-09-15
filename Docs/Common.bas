Attribute VB_Name = "Common"
'   ============
'   Common.bas
'   ============
'
' Things that are common to the whole driver
'
' Written:  07-Feb-04   Chris Rowland
'
' Edits:
'
' When      Who     What
' --------- ---     --------------------------------------------------
' 07-Feb-04 cdr     Initial edit.
' 21-Feb-04 cdr     Use constant strings to persist data
' 13-May-04 cdr     Convert to hub
' 29-Sep-12 cdr     Reverse the Aux guide direction for East and West

#Const BETA_MODE = False

#If BETA_MODE Then
Private m_AltAzRaDec As Boolean
#End If

Public g_serial As DriverHelper.Serial
Public g_util As DriverHelper.Util

' ------------------------
' connectivity management
' ------------------------

Public g_Profile As DriverHelper.Profile
Public g_sScopeID As String
Public g_sScopeName As String

Public g_iConnections As Integer
Public g_bConnected As Boolean

Public g_bInitComplete As Boolean

'==========================================
' Constants
'------------------------------------------
#If SECOND_DRIVER Then
Public Const DESC As String = "Second Celestron Scope Driver"
Public Const g_DriverID As String = "Celestron2.Telescope"
#Else
Public Const DESC As String = "Celestron Scope Driver"
Public Const g_DriverID As String = "Celestron.Telescope"
#End If

' telescope interface version
Public Const g_InterfaceVersion = 2

' timings for scope events in milliseconds
Public Const COORD_UPD_INTERVAL As Long = 1000
Public Const SHORT_UPD_INTERVAL As Long = 500
Public Const SHORT_WAIT         As Long = 200
Public Const LONG_WAIT          As Long = 1800

Public Const MAX_SLEW_TIME = 100   ' in seconds

Public Const GPS_LINK_RETRY_INTERVAL As Double = 10   'pwgs: Seconds between retests of GPS time syncd status when not syncd.
Public Const GPS_RESYNC_INTERVAL As Double = 3600     'pwgs: Seconds between GPS time resynchs when synchronised.

Public Const INVALID_COORDINATE As Double = 100000#
'--------------------------------

' Constant strings used to persist data
Public Const SITE_LATITUDE = "Site Latitude"
Public Const SITE_LONGITUDE = "Site Longitude"
Public Const SITE_ELEVATION = "Site Elevation"
Public Const COM_PORT = "COM Port"
Public Const SHOW_HC = "Show Hand Control"
Public Const PARK_ALTITUDE = "Park Altitude"
Public Const PARK_AZIMUTH = "Park Azimuth"
Public Const IS_PARKED = "IsParked"
Public Const IS_TRACKING = "Tracking"
Public Const FOCAL_LENGTH = "Focal Length"
Public Const APERTURE = "ApertureDiameter"
Public Const AREA = "ApertureArea"
Public Const HAS_GPS = "Has GPS"
Public Const IS_GEM = "Is Gem"
Public Const TRACK_MODE = "Track Mode"
Public Const SCOPE_INDEX = "Scope Index"
Public Const AXIS_RATE = "AxisRate"
Public Const ON_TOP = "OnTop"
Public Const ENABLE_PEC = "EnablePEC"

'
'==========================================
' private parameters
'------------------------------------------

Private m_Coord_Upd_Interval As Long

Private Enum E_SCOPE_TYPE
    E_UNKNOWN = 0
    E_ULTIMA = 1        ' Ultima 2000
    E_NEXSTAR58 = 2     ' the original NexStar 5 and 8
    E_NEXSTAR_GPS = 3   ' the GPS, 5i and 8i, CGE and ASC
    E_GT = 4            ' the original GT controller and - maybe - Tascos
End Enum

Private m_ScopeType As E_SCOPE_TYPE

Public g_ScopeTypeStr  As String

Public g_iSettleTime As Integer

Public g_dRA As Double
Public g_dDec As Double

Public g_dAlt As Double
Public g_dAz As Double
Public g_ScopeAlt As Double    ' value as read from the scope
Public g_ScopeAz As Double     ' value as read from the scope

Public g_CanPulseGuide As Boolean
Public g_CanSetTracking As Boolean
Public g_CanSlewAsync As Boolean

Public g_dTargetRA As Double
Public g_dTargetDec As Double

Public g_bTargetRaValid As Boolean
Public g_bTargetDecValid As Boolean

Public g_dLatitude As Double
Public g_dLongitude As Double
Public g_dElevation As Double
Public g_dAperture As Double
Public g_dFocalLength As Double
Public g_dApertureArea As Double

Public g_dTargetAltitude As Double
Public g_dTargetAzimuth As Double

Public g_AtHome As Boolean
Public g_AtPark As Boolean
Private useScopeTime As Boolean     ' if true try to get the time from the RTC or GPS

' these values are set according to the scope we are connected to
Public g_eAlignMode As AlignmentModes      ' cdr for NSGPS
' offset values for sync
Public g_OffsetRa As Double
Public g_OffsetDec As Double
Public g_Tracking As Boolean
'Public g_Guiding As Boolean        ' set while guiding or pulse guiding
Public g_EnablePEC As Boolean       ' enable PEC if requested.

' rates collections

Public g_AxisRates As AxisRates         ' rates available for MoveAxis

Public g_GuideRates As AxisRates    ' rates available for PulseGuide

Public g_GuideRateDec As Double    ' PulseGuide rate in degrees per second
Public g_GuideRateRa As Double     ' PulseGuide rate in degrees per second

Public g_HCRates As AxisRates          ' 9 HC rates, used for the HC dialog

Public g_DriveRates As TrackingRates

Public g_DriveRate As DriveRates

Public g_PulseGuide As clsPulseGuide
Public g_IsGuiding As Boolean           ' this is only used by clsPulseGuide, if the
                                        ' aux control guiding is used it isn't set
Private isGuidingReturnsFalse As Boolean
Private asyncGuiding As Boolean

Private Enum E_SLEW_STATE
    eInitSlew = -1
    eNoSlew = 0
    eSettling = 1
    eSlewing = 2
    eMoveAxis = 3
End Enum

Private m_lNextAltAz As Long
Private m_Extended As Boolean   ' uses the extended command set
Private m_lNextRADec As Long

Private m_bAbortSlew As Boolean
Private m_eSlewing As E_SLEW_STATE
Private m_SlewStart As Single
Public m_bActive As Boolean        ' set while sending and receiving a command

Private m_ParkAlt As Double
Private m_ParkAz As Double

Private m_Version As Long             'cdr  HC version is available from 1.6 onwards for NexStarGPS
' store the version as 2 bytes to avoid rounding problems
Const Version_16 = &H106        ' 1.06 first to use "P" commands
Const Version_22 = &H202        ' 2.02 GPS and i series scopes
Const Version_23 = &H203        ' 2.03 iSeries SE
Const Version_30 = &H300        ' 3.00 CGE scopes
Const Version_34 = &H304        ' 3.04 ASC scopes
Const Version_40 = &H400        ' 4.00 CPC and SLT scopes
Const Version_41 = &H40A        ' 4.10 scopes with sync command
Const Version_412 = &H40C       ' 4.12 scopes
Const Version_413 = &H40D       ' 4.13 scopes
Const Version_414 = &H40E       ' 4.14 scopes
'Const Version_420 = &H414       ' 4.20 scopes
'Const Version_421 = &H415       ' 4.21 scopes
Const Version_Beta = &H2800     ' beta versions

' Version information
' 1.6   original GPS, no 'm' command
' 2.2   GPS and i Series, 'm' command works
' 3.01  CGE, no 'm' command
' 3.04  ASC, no 'm' command
' 2.3   NXS for GPS and i series
' 3.05  NXS for CGE and AGT
' 4.01  CPC and maybe others
' 4.21  hibernate/wakeup (originally 4.20 but didn't make it to the released code)
' 40.xx beta versions
' 100.07 to 102.x original GT

Const Version_GT_LO = &H6400
Const Version_GT_HI = &H66FF

Private Enum E_SCOPE_ID
    SCOPE_UNKNOWN = 0
    SCOPE_GPS = 1
    SCOPE_GPS_SA = 2
    SCOPE_ISERIES = 3
    SCOPE_ISERIES_SE = 4
    SCOPE_CGE = 5
    SCOPE_ASC = 6
    SCOPE_SLT = 7
    SCOPE_C20 = 8
    SCOPE_CPC = 9
    SCOPE_GT = 10
    SCOPE_4_5_SE = 11
    SCOPE_6_8_SE = 12
    SCOPE_CGE2 = 13
    SCOPE_EQ6 = 14
    SCOPE_AVX = 20
End Enum
  
Private m_Scope_ID As E_SCOPE_ID

Private m_ApertureDiameter As Double    'cdr
Private m_FocalLength As Double
Private m_HasGPS As Boolean
Private m_HasPEC As Boolean

' cdr we need to determine and keep track of the current tracking mode for the NexStar
Public Enum E_TRACK_MODE
    E_TRACK_OFF = 0
    E_TRACK_ALTAZ = 1
    E_TRACK_EQ_N = 2
    E_TRACK_EQ_S = 3
End Enum

Public g_TrackMode As E_TRACK_MODE

Private m_IsGem As Boolean

Private m_Aligned As Boolean    ' set when the scope reports that it is aligned

Private m_AllowAllSlews As Boolean  ' If set this will allow slews regardless of the state of tracking
Private m_SlewIfNotAligned As Boolean    ' allow slews when not aligned

' Alt and Azm motor controller versions
Private m_AltMcVer As Long
Private m_AzmMcVer As Long

Private Const MIN_GUIDE_VER_CGE2 = &H60C         ' 6.12 onwards
Private Const MIN_GUIDE_VER_ASC = &H512         ' 5.18 onwards

' tracking rates in the scope motor controller units of 1/1024 arc sec/second
Private Const SIDEREAL_MC_RATE = &H3C29
Private Const LUNAR_MC_RATE = &H39CD
Private Const SOLAR_MC_RATE = &H3C00

Public g_DeclinationRate As Double      ' arcsec/sec
Public g_RightAscensionRate As Double

Private m_RaRateSet As Boolean        ' the Ra rate has been set, the scope may not track

Private m_dlgHC As dlgHandControl

'Private CorrectForPrecession As Boolean
' this was added to correct the positions from J2000 to current.
' The scopes before V4.13 don't correct which makes the deep sky positions approximately
' J2000 but the planet positions JNow. It might be worth adding this, depending on the HC
' version because the error will be increasing as we get further from J2000.


' Commands expressed as byte constants
Private Const NS_CR As Byte = &HD               ' Carriage return
Private Const NS_COMMA As Byte = &H2C           ' comma
Private Const NS_QUESTION As Byte = &H3F        ' ? command initialise for NS 5 and 8
Private Const NS_TERMINATOR As Byte = &H23      ' # command terminator

Private Const NS_0 As Byte = &H30               ' "0"
Private Const NS_1 As Byte = &H31               ' "1"
Private Const NS_9 As Byte = &H39               ' "9"
Private Const NS_A As Byte = &H41               ' "A"
Private Const NS_E As Byte = &H45               ' "E"
Private Const NS_F As Byte = &H46               ' "F"
Private Const NS_W As Byte = &H57               ' "W"

Private Const NS_SET_ALTAZ As Byte = &H41       ' A Slew AltAz, Nexstar 5 and 8 only
Private Const NS_SET_ALTAZ_LP As Byte = &H42    ' B Slew AltAz Low precision
Private Const NS_SET_CONFIG_ITEM As Byte = &H43 ' C Set configuration item
Private Const NS_GET_RADEC_LP As Byte = &H45    ' E Get Ra Dec Low precision
Private Const NS_SYNC As Byte = &H46            ' F Sync, Ultima2K only
Private Const NS_SET_TIME As Byte = &H48        ' H Set local time
Private Const NS_SET_TIME_NEW As Byte = &H49    ' I Set local time, 4.21 onwards
Private Const NS_IS_ALIGNED As Byte = &H4A      ' J get scope aligned state
Private Const NS_ECHO_CMD As Byte = &H4B        ' K echo command
Private Const NS_IS_SLEWING As Byte = &H4C      ' L get slewing state
Private Const NS_ABORT_SLEW As Byte = &H4D      ' M Abort Slew
Private Const NS_SET_RADEC_LP As Byte = &H52    ' R set Ra Dec Low precision
Private Const NS_SYNC_LP As Byte = &H53         ' S sync low precision
Private Const NS_SET_TRACKING As Byte = &H54    ' T Set Tracking
Private Const NS_GET_VERSION As Byte = &H56     ' V get Version command
Private Const NS_AUX_CMD As Byte = &H50         ' P Aux command
Private Const NS_SET_LOCATION As Byte = &H57    ' W set Location
Private Const NS_LAST_ALIGN As Byte = &H59      ' Y Last Align or Quick Align
Private Const NS_GET_ALTAZ_LP As Byte = &H5A    ' Z Get AltAz Low precision

Private Const NS_SET_ALTAZ_HP As Byte = &H62    ' b Slew AltAz high precision
Private Const NS_GET_CONFIG_ITEM As Byte = &H63 ' c Get Confguration item
Private Const NS_GET_RADEC_HP As Byte = &H65    ' e Get Ra Dec high precision

Private Const NS_GET_DEST_PIER_SIDE As Byte = &H67  ' g Get the destination side of pier
Private Const NS_GET_TIME As Byte = &H68        ' h get local time
Private Const NS_GET_TIME_NEW As Byte = &H69    ' i get local time, 4.21 onwards
Private Const NS_GET_LST As Byte = &H6C         ' l get local sidereal time
Private Const NS_GET_MODEL As Byte = &H6D       ' m Get scope model
Private Const NS_SET_HOME As Byte = &H6E        ' n Set home position, 4.21 onwards
Private Const NS_GOTO_HOME As Byte = &H6F       ' o goto home position, 4.21 onwards
Private Const NS_GET_PIER_SIDE As Byte = &H70   ' p Get Pier Side
Private Const NS_SET_RADEC_HP As Byte = &H72    ' r set Ra Dec High precision
Private Const NS_SYNC_HP As Byte = &H73         ' s sync high precision
Private Const NS_GET_TRACKING As Byte = &H74    ' t get Tracking state
Private Const NS_UNDO_SYNC As Byte = &H75       ' u undo sync
Private Const NS_GET_LOCATION As Byte = &H77    ' w Get location
Private Const NS_HIBERNATE As Byte = &H78       ' x Hibernate, 4.21 onwards
Private Const NS_WAKEUP As Byte = &H79          ' y Wake up, parameter 0 or 1, 4.21 onwards
Private Const NS_GET_ALTAZ_HP As Byte = &H7A    ' z Get Alt Az High precision

' configuration items
Private Enum E_CONFIG_ITEM
    MERIDIAN_MODE = 1
End Enum

' meridian mode enumeration
Private Enum E_MERIDIAN_MODE
    FAVOUR_CURRENT = 0
    FAVOUR_WEST = 1
    FAVOUR_EAST = 2
    USE_MERIDIAN = 3
End Enum

' AUX commands for the NexStar scopes
Public Enum E_DEVICE_ID
    ID_MAIN = 1
    ID_HC = 4
    ID_AZM = 16
    ID_ALT = 17
    ID_GPS = 176
    ID_RTC = &HB2
End Enum

Private Enum E_MESSAGE_ID
'                                   Send    Receive
'   motor controller commands
    MC_GET_POSITION = 1         '    n/a    24 bits
    MC_GOTO_FAST = 2            '   16/24 bits Ack
    
    MC_SET_AXIS = 4             '   24 bits Ack     24 bits for a rotation

    ' continuously variable move rate in units of 0.25 arc sec per second
    MC_SET_POS_GUIDERATE = 6    '   24 bits n/a
    MC_SET_NEG_GUIDERATE = 7    '   24 bits n/a
    
    MC_SEEK_SWITCH = &HB        '   n/a     Ack     Start the move to the switch position
    MC_PEC_RECORD_START = &HC   '   n/a     Ack     Start recording PEC position
    MC_PEC_PLAYBACK = &HD       '   8 bits  Ack     Start(01)/stop(00) PEC playback
    
    MC_AT_SWITCH = &H12         '   n/a     8 bits, FFH is seek switch completed
    MC_SLEW_DONE = &H13         '    n/a    8 bits, FFH is slew completed
    
    MC_PEC_RECORD_DONE = &H15   '   n/a     8 bits  FFH ia PEC record completed
    MC_PEC_RECORD_STOP = &H16   '   n/a     n/a     Stop PEC recording
    
    MC_GOTO_SLOW = &H17
    MC_AT_INDEX = &H18          '   n/a     8 bits  FFH at index, 00H not
    MC_SEEK_INDEX = &H19        '   n/a     n/a     Seek PEC Index
    
    ' emulate the HC buttons, value is rate 0-9 where 0 is stop
    MC_MOVE_POS = &H24          '      8    n/a     move positive (up/right)
    MC_MOVE_NEG = &H25          '      8    n/a     Move negative (down/left)
    
    ' the new hardware based guide commands
    MTR_AUX_GUIDE = &H26
'        Command is followed by a signed char: velocity [% sidereal]; and
'        unsigned char: duration [10 ms units].

    MTR_IS_AUX_GUIDE_ACTIVE = &H27
        'MC returns TRUE(1) if last AUX_GUIDE command's is still in effect.
        'MC returns FALSE(0) if last AUX_GUIDE command has expired.

    ' set/get the autoguide rates as a percentage of sidereal
    MC_SET_AUTOGUIDE_RATE = &H46 ' 8    n/a     Set percentage of sidereal
    MC_GET_AUTOGUIDE_RATE = &H47 ' 8    n/a     % = 100 * val/256
    
'   GPS commands
    GPS_GET_LAT = 1         '   n/a     24 Bits
    GPS_GET_LONG = 2        '   n/a     24 Bits
    GPS_GET_DATE = 3        '   n/a     16 Bits month|day
    GPS_GET_YEAR = 4        '   n/a     16 Bits
    GPS_GET_TIME = &H33     '   n/a     24 bits hours|minutes|seconds
    GPS_TIME_VALID = &H36   '   n/a     8 bits  0=False, 1=True
    GPS_LINKED = &H37       '   n/a     8 bits  0=False, 1=True
    
'   All modules
    GET_VER = &HFE          '   n/a     16 bits major|minor
End Enum

Private Enum E_TRACK_RATES
    ' 16 bit guide rates are for special rates, these are for the Az axis in an EQ mode only
    ' use MC_SET_POS_GUIDERATE for EQ_N or MC_SET_NEG_GUIDERATE for EQ_S
    RATE_SIDEREAL = &HFFFF
    RATE_SOLAR = &HFFFE
    RATE_LUNAR = &HFFFD
End Enum

Private Const SIDEREAL_RATE_DEG_SEC = (360# / SPD) / SIDRATE

' specifies the maximum rate for a Move Axis command
Private Const MAX_MOVE_RATE_DEG_SEC = 4.5

' trace commands
Private m_fTracing As Boolean
Private m_TraceFile As Scripting.TextStream
Private FSO As Scripting.FileSystemObject

' comproxy stuff for NexRemote
'Private nexRemote  As COMproxy.Application
'Private isUsingNexRemote As Boolean

Private Declare Function GetTickCount Lib "kernel32" () As Long
Public Declare Sub Sleep Lib "kernel32" (ByVal dwMilliseconds As Long)

Public Sub Main()
    Debug.Print "Sub Main"
    g_bInitComplete = False                 ' 1-Time initialization not done

    If App.StartMode = vbSModeStandalone Then
        MsgBox "This driver can only be run from an ASCOM application", , "ASCOM Celestron Driver"
        End
    End If
End Sub

Public Sub InitOnce()
    Debug.Print "InitOnce g_bInitComplete " & g_bInitComplete
    If g_bInitComplete Then Exit Sub       ' Initialize only once

    Set g_Profile = New DriverHelper.Profile
    g_Profile.DeviceType = "Telescope"              ' We're a Telescope driver
        
    Set g_util = New DriverHelper.Util
    g_util.LoadDLL App.Path & "\Astro32.dll"                           ' Load the astronomy functions DLL
    
    Set g_serial = New DriverHelper.Serial

    g_iConnections = 0                              ' zero connections currently
    g_bConnected = False                            ' Not yet connected

    g_Tracking = True                               ' Drive is tracking
    g_dTargetRA = INVALID_COORDINATE                ' No target coordinates
    g_dTargetDec = INVALID_COORDINATE
    g_iSettleTime = 0                               ' No settling time
    
    g_Profile.Register g_DriverID, DESC             ' Self reg (skips if already reg)

    m_lNextRADec = 0                                ' Not received RA/Dec yet
    m_lNextAltAz = 0                                ' Not received Alt/Az yet
    m_eSlewing = eNoSlew
    m_bAbortSlew = False
    m_bActive = False
    g_AtPark = False
    g_AtHome = False
    m_ScopeType = E_UNKNOWN
    g_ScopeTypeStr = "Unknown"
    Set g_AxisRates = New AxisRates
    Set g_GuideRates = New AxisRates
    Set g_HCRates = New AxisRates
    Set g_DriveRates = New TrackingRates
    g_DriveRates.Add driveSidereal
    g_DriveRate = driveSidereal
    m_Coord_Upd_Interval = COORD_UPD_INTERVAL   ' default to 1 second

    Set g_PulseGuide = New clsPulseGuide
    
    g_RightAscensionRate = 0#
    g_DeclinationRate = 0#
    
    g_bInitComplete = True                 ' 1-Time initialization completed

End Sub

Public Function ScopeCanPulseGuide() As Boolean
    CheckConnected
    Select Case m_ScopeType
    Case E_NEXSTAR_GPS
        g_CanPulseGuide = (m_Version >= Version_16) And (g_eAlignMode <> algAltAz)
'    Case E_ULTIMA      ' this seems potentially to be possible.
'        CanPulseGuide = True
    Case Else
        g_CanPulseGuide = False
    End Select
    ScopeCanPulseGuide = g_CanPulseGuide
    Trace "ScopeCanPulseGuide " & g_CanPulseGuide
End Function

Public Sub ScopeCommandBlind(ByVal Command As String, Optional Raw As Boolean)
    Select Case m_ScopeType
    Case E_NEXSTAR_GPS, E_ULTIMA
        Dim buf As String
        buf = ScopeCommandString(Command)
    Case Else
        RaiseError SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
                    "Method Command Blind()" & MSG_NOT_IMPLEMENTED
    End Select
End Sub

Public Function ScopeCommandString(ByVal Command As String, Optional Raw As Boolean) As String
    Dim buf As String
    'On Error Resume Next    'pwgs: Added to ensure that comms errors are propogated correctly up the application
    Select Case m_ScopeType
    Case E_NEXSTAR_GPS, E_ULTIMA
        CheckConnected
        ' all replies are terminated with  a # char
        SyncLock
        g_serial.ClearBuffers
        g_serial.Transmit Command
        ' some replies may contain the terminator character so we have to
        ' handle these by receiving the appropriate count
        Select Case Mid$(Command, 1, 1)
        Case ""     ' handle an empty string by returning nothing
            buf = ""
        Case "K"                            ' send "Kx"
            buf = g_serial.ReceiveCounted(2)    ' expect "x#"
        Case "V"                            ' send "V"
            buf = g_serial.ReceiveCounted(3)    ' expect "mn#"
        Case "P"                            ' send "P..."
            buf = g_serial.ReceiveCounted(Asc(Mid$(Command, 8, 1)) + 1) ' expect responsebytes chars and "#"
            ' new HC will return extra 0 character if the command times out
            If Right$(buf, 1) <> "#" Then
                buf = buf & g_serial.ReceiveCounted(1)
            End If
        Case "w", "h"
            buf = g_serial.ReceiveCounted(8)    ' expect 8 bytes, then "#"
        Case Else
            buf = g_serial.ReceiveTerminated("#")
        End Select
        Trace "ScopeCommandString " & Command & "=" & buf
        ScopeCommandString = buf
        m_bActive = False
    Case Else
        RaiseError SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
                    "Method CommandString()" & MSG_NOT_IMPLEMENTED
        m_bActive = False
    End Select
End Function

Private Function ScopeCommandBinary(Command() As Byte) As Byte()
    Dim buf() As Byte
    'On Error Resume Next    'pwgs: Added to ensure that comms errors are propogated correctly up the application
    Select Case m_ScopeType
    Case E_NEXSTAR_GPS
        CheckConnected
        ' all replies are terminated with  a # char
        SyncLock
'        Trace "Active is true"

        g_serial.ClearBuffers
        g_serial.TransmitBinary Command
        ' some replies may contain the terminator character so we have to
        ' handle these by receiving the appropriate count
        Select Case Command(0)
        Case NS_ECHO_CMD                    ' send "Kx"
            buf = g_serial.ReceiveCountedBinary(2)    ' expect "x#"
        Case NS_GET_VERSION                            ' send "V"
            buf = g_serial.ReceiveCountedBinary(3)    ' expect "mn#"
        Case NS_AUX_CMD                            ' send "P..."
            buf = g_serial.ReceiveCountedBinary(Command(7) + 1) ' expect responsebytes chars and "#"
            ' new HC will return extra 0 character if the command times out
            If buf(UBound(buf)) <> NS_TERMINATOR Then
                Dim bx() As Byte
                ReDim Preserve buf(0 To UBound(buf))
                bx = g_serial.ReceiveCountedBinary(1)
                buf(UBound(buf)) = bx(0)
                m_bActive = False
                RaiseError Err.Number, ERR_SOURCE, "Aux Command " & Command(3) & "-" & Err.Description
            End If
        Case NS_GET_LOCATION, NS_GET_TIME, NS_GET_TIME       ' w, h, i
            buf = g_serial.ReceiveCountedBinary(9)    ' expect 8 bytes, then "#"
        Case Else
            buf = g_serial.ReceiveTerminatedBinary(ToByte(NS_TERMINATOR))
        End Select
        ScopeCommandBinary = buf
        m_bActive = False
'        Trace "Active - false"
    Case E_ULTIMA
        CheckConnected
        ' all replies are terminated with  a # char
        SyncLock
'        Trace "Active is true"
        g_serial.ClearBuffers
        g_serial.TransmitBinary Command
        buf = g_serial.ReceiveTerminatedBinary(ToByte(NS_TERMINATOR))
        ScopeCommandBinary = buf
        m_bActive = False
'        Trace "Active - false"
    Case E_NEXSTAR58
        ' sends the initialization command for the NexStar 5 & 8
        'Returns an empty buffer if it fails
        ' we should not send commands while slewing as we may miss the '@' character
        If m_eSlewing <> eSlewing Then
            SyncLock
            g_serial.ClearBuffers
            On Error GoTo InitFail
            g_serial.TransmitBinary ToByte(NS_QUESTION)
            buf = g_serial.ReceiveTerminatedBinary(ToByte(NS_TERMINATOR))
            If (buf(0) = NS_TERMINATOR) Then
                g_serial.TransmitBinary Command
                ' use the length of the command to decide if there's a reply
                If Length(Command) = 1 Then
                    buf = g_serial.ReceiveCountedBinary(4)
                End If
            End If
InitFail:
            m_bActive = False
        End If
        ScopeCommandBinary = buf
        
    Case E_GT
        SyncLock
        g_serial.TransmitBinary Command
        If Length(Command) = 1 Then
            buf = g_serial.ReceiveCountedBinary(6)
        End If
        m_bActive = False
        ScopeCommandBinary = buf
        
    Case Else
        RaiseError SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
                    "Method ScopeCommandBinary()" & MSG_NOT_IMPLEMENTED
        m_bActive = False
    End Select
End Function

Public Sub ScopeMoveAxis(ByVal Axis As TelescopeAxes, Rate As Double)
    ' This will command the scope to move in the specified directions
    'at the specified rates in Degrees per second.
    
    CheckConnected
    CheckParked
    
'    Wait ' for any command to finish
    Trace "ScopeMoveAxis " & Axis & ", " & Rate
    Select Case m_ScopeType
    Case E_NEXSTAR_GPS
        If m_Version >= Version_16 Then
            If RateIsInRange(Rate, g_AxisRates) Then
                Dim gr As Long
                Dim msgID As E_MESSAGE_ID
                Dim devID As E_DEVICE_ID
                gr = Rate * 3600# * 1024# ' convert to pulses
                Select Case Axis
                Case TelescopeAxes.axisPrimary
                    devID = ID_AZM
                Case TelescopeAxes.axisSecondary
                    devID = ID_ALT
                Case Else
                    RaiseError SCODE_PROP_RANGE_ERROR, ERR_SOURCE, _
                            "Method MoveAxis() " & MSG_PROP_RANGE_ERROR
                    Exit Sub
                End Select
                
                If g_TrackMode = E_TRACK_EQ_S Then gr = -gr
                
                If gr = 0 Then
                    m_eSlewing = eNoSlew
                    If g_Tracking Then
                        If g_eAlignMode = algAltAz Then
                            SetTracking g_Tracking
                        ElseIf devID = ID_AZM Then
                            SetTrackingRate
                        Else
                            MCRate devID, gr
                        End If
                    Else
                        MCRate devID, gr
                        If Axis = axisPrimary Then m_RaRateSet = True
                    End If
                Else
                    MCRate devID, gr
                    m_eSlewing = eMoveAxis
                    If Axis = axisPrimary Then m_RaRateSet = True
                End If
            Else
                RaiseError SCODE_PROP_RANGE_ERROR, ERR_SOURCE, _
                        "Method MoveAxis() " & MSG_PROP_RANGE_ERROR
                    Exit Sub
            End If
        Else
            RaiseError SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
                    "Method MoveAxis() " & MSG_NOT_IMPLEMENTED
        End If
    Case Else
        RaiseError SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
                    "Method MoveAxis()" & MSG_NOT_IMPLEMENTED
    End Select
Exit Sub

ErrNXGPS:
    ' catch the NXGPS error during tracking
    ' try to restore tracking
    Sleep SHORT_WAIT
    SetTracking g_Tracking
End Sub

Public Sub GetAltAz()
    Dim t As Long
    Dim buf() As Byte
    Dim sep1 As Integer, sep2 As Integer
    Dim Alt As Double, Az As Double
    CheckConnected
    t = GetTickCount()
    ' Throttle scope traffic
    If t < m_lNextAltAz Or g_IsGuiding Then
        Exit Sub
    End If
    If m_bActive Then Exit Sub
    On Error GoTo ErrGetAltAz
          
    Select Case m_ScopeType
    Case E_NEXSTAR_GPS
        If m_Version >= Version_22 Then
'            buf = ScopeCommandString("z")
            buf = ScopeCommandBinary(ToByte(NS_GET_ALTAZ_HP))
            ' 32 bit high precision
            If buf(8) = NS_COMMA And buf(17) = NS_TERMINATOR Then
                g_ScopeAz = HexToDegB(buf, 0, 5)
                g_ScopeAlt = HexToDegB(buf, 9, 14)
            End If
        Else
'            buf = ScopeCommandString("Z")
            buf = ScopeCommandBinary(ToByte(NS_GET_ALTAZ_LP))
            If buf(4) = NS_COMMA And buf(9) = NS_TERMINATOR Then
                g_ScopeAz = HexToDegB(buf, 0, 3)
                g_ScopeAlt = HexToDegB(buf, 5, 8)
            End If
        End If
    
        If g_ScopeAlt > 180# Then
            g_ScopeAlt = g_ScopeAlt - 360#
        End If
        If g_ScopeAz < 0# Then
            g_ScopeAz = g_ScopeAz + 360#
        End If
    Case E_NEXSTAR58
        buf = ScopeCommandBinary(ToByte(NS_GET_ALTAZ_LP))
        If Length(buf) = 4 Then
            g_ScopeAz = ByteToDegB(buf, 0, 1)
            g_ScopeAlt = ByteToDegB(buf, 2, 3)
        End If
    Case E_ULTIMA
'        buf = ScopeCommandString("Z" & vbCr)
        buf = ScopeCommandBinary(ToByte(NS_GET_ALTAZ_LP, NS_CR))
        sep1 = InByt(buf, NS_COMMA)
        sep2 = InByt(buf, NS_TERMINATOR)
        If sep1 >= 4 And sep2 >= 9 Then
            g_ScopeAz = HexToDegB(buf, sep1 - 4, sep1 - 1)
            g_ScopeAlt = HexToDegB(buf, sep2 - 4, sep2 - 1)
        End If
        If g_ScopeAlt > 180# Then
           g_ScopeAlt = g_ScopeAlt - 360#
        End If
        If g_ScopeAz < 0# Then
            g_ScopeAz = g_ScopeAz + 360#
        End If
    Case E_GT
        buf = ScopeCommandBinary(ToByte(NS_GET_ALTAZ_LP))    ' "bb0bb0"
        g_ScopeAz = ByteToDegB(buf, 0, 1)
        g_ScopeAlt = ByteToDegB(buf, 2, 3)
    Case Else
        RaiseError SCODE_NOT_IMPLEMENTED, _
                  ERR_SOURCE, _
                  "GetAltAz() " & MSG_NOT_IMPLEMENTED
    End Select
    
    Trace "GetAltAz " & g_ScopeAlt & ", " & g_ScopeAz
    
    Select Case g_eAlignMode
    Case algAltAz
        g_dAlt = g_ScopeAlt
        g_dAz = g_ScopeAz
    Case algPolar, algGermanPolar
        ' use the Ra and dec values
        GetRaDec
        Dim ha As Double
        Dim st As Double
        st = now_lst(degrad(g_dLongitude))
        ha = st - g_dRA     ' hours
        hadec_aa degrad(g_dLatitude), hrrad(ha), degrad(g_dDec), Alt, Az
        g_dAlt = raddeg(Alt)
        g_dAz = raddeg(Az)
    End Select
    
    m_lNextAltAz = t + m_Coord_Upd_Interval          ' Success, update timer
Exit Sub

ErrGetAltAz:                                       ' Here if scope doesn't respond
    m_lNextAltAz = t + m_Coord_Upd_Interval
    m_bActive = False
End Sub

Public Function ScopeCanSetTracking() As Boolean
    Select Case m_ScopeType
    Case E_NEXSTAR_GPS
        g_CanSetTracking = (m_Version >= Version_16)
    Case Else
        g_CanSetTracking = False
    End Select
    Trace "ScopeCanSetTracking " & g_CanSetTracking
    ScopeCanSetTracking = g_CanSetTracking
End Function

Public Function ScopeCanSetPierSide() As Boolean
    If m_Version > Version_414 And m_Version < Version_GT_LO And _
        (m_Scope_ID = SCOPE_CGE2) Or (m_Scope_ID = SCOPE_EQ6) Or _
        (m_Scope_ID = SCOPE_AVX) _
    Then
        ScopeCanSetPierSide = m_IsGem
    Else
        ScopeCanSetPierSide = False
    End If
End Function

Public Function ScopeCanSlewAsync() As Boolean
    Select Case m_ScopeType
    Case E_NEXSTAR_GPS
        g_CanSlewAsync = True   '(m_Version >= Version_16)
    Case E_ULTIMA
        g_CanSlewAsync = True
    Case Else
        g_CanSlewAsync = False
    End Select
    Trace "ScopeCanSlewAsync " & g_CanSlewAsync
    ScopeCanSlewAsync = g_CanSlewAsync
End Function

Public Function ScopeCanSlewAltAz()
    Select Case m_ScopeType
    Case E_NEXSTAR_GPS
        ScopeCanSlewAltAz = (m_Version >= Version_16)
        Trace "ScopeCanSlewAltAz " & (m_Version >= Version_16)
    Case Else
        ScopeCanSlewAltAz = False
        Trace "ScopeCanSlewAltAz " & False
    End Select
End Function

Public Sub ScopeConnected(ByVal newVal As Boolean)
    Dim buf As String, bufcomio As String
    Dim tmo As Long
    Dim wrk As Integer
    
    If newVal Then                                      ' CONNECTING
'        g_util.LoadDLL App.Path & "\astro32.dll"        ' Assure this is loaded and available
        
        On Error GoTo N0_SERIAL
        If Not g_serial.Connected Then
            '
            ' (1) Set up the communications link. Default to COM1.
            '
            buf = g_Profile.GetValue(g_DriverID, COM_PORT)
            If buf = "" Then                                ' Default to COM1
                buf = "1"
                g_Profile.WriteValue g_DriverID, COM_PORT, buf
            End If
            g_serial.port = CInt(Trim$(buf))                ' Set port
            g_serial.Speed = ps9600                         ' 9600
            g_serial.ReceiveTimeout = 1                     ' 1 second timeouts
            g_serial.Connected = True                       ' Grab the serial port
            g_bConnected = True
        
            On Error GoTo NO_SCOPE
            ' determine which scope we are connected to
            If IdentifyScope Then
                If m_ScopeType = E_GT Then
                    On Error GoTo 0
                    g_serial.ClearBuffers
                    g_serial.Connected = False                          ' Release the port
                    RaiseError SCODE_NO_CELESTRON, ERR_SOURCE, _
                        "This driver does not control the Original GT or Tasco scopes."
                    g_bConnected = False
                    Exit Sub
                End If
                
                g_serial.ReceiveTimeout = 4     ' Apparently scopes can take 3.5 secs to respond.
            
                ' Get the scope dependent parameters
                GetScopeParameters
            
                ' show the HC dialog
                On Error GoTo 0
                If m_ScopeType = E_NEXSTAR_GPS And m_Version >= Version_16 Then
                    buf = g_Profile.GetValue(g_DriverID, SHOW_HC)
                    If buf = "" Then buf = "0"
                    If CInt(Trim$(buf)) > 0 Then
                        Set m_dlgHC = New dlgHandControl
                        m_dlgHC.Show
                    End If
                End If
            Else
                g_serial.ClearBuffers
                g_serial.Connected = False
            End If
        End If  ' not g_serial.Connected
    Else                                                ' DISCONNECTING
        If g_iConnections <= 1 Then
            If g_serial.Connected And m_RaRateSet And g_eAlignMode <> algAltAz Then SetTrackingRate
            If Not m_dlgHC Is Nothing Then
                Unload m_dlgHC
            End If
            On Error Resume Next                            ' Best efforts...
            g_serial.ClearBuffers                           ' Clear serial buffers
            g_serial.Connected = False                      ' Release COM port
            ' set parameters back to uninitialised state
            m_ScopeType = E_UNKNOWN
            g_ScopeTypeStr = "Unknown"
            m_Version = 0
            m_Aligned = False
            g_bTargetDecValid = False
            g_bTargetRaValid = False
            m_eSlewing = eInitSlew
            If m_fTracing Then m_TraceFile.Close
        End If
    End If

    g_bConnected = g_serial.Connected
    Exit Sub
    
NO_SCOPE:
    g_serial.ClearBuffers
N0_SERIAL:
    g_serial.Connected = False                          ' Release the port
    g_bConnected = False
    On Error GoTo 0                                     ' Avoid infinite loop rbd 10/
    RaiseError SCODE_NO_CELESTRON, ERR_SOURCE, MSG_NO_CELESTRON
End Sub

Public Sub GetRaDec()
    ' use the scope type to decide how to proceed
    Dim buf() As Byte
    Dim t As Long
    Dim sep1 As Integer, sep2 As Integer
    
    t = GetTickCount
    ' throttle getting the RA dec
    If t < m_lNextRADec Or g_IsGuiding Then
        Debug.Print "GetRaDec throttled"
        Exit Sub
    End If
    Debug.Print "GetRaDec m_bActive " & m_bActive
    If m_bActive Then Exit Sub
    
    On Error GoTo ErrGetRaDec
    
    Select Case m_ScopeType
    Case E_NEXSTAR_GPS
        If m_Version >= Version_16 Then
#If BETA_MODE Then
            AltAzToRaDec
#Else
'            buf = ScopeCommandString("e")
            buf = ScopeCommandBinary(ToByte(NS_GET_RADEC_HP))
            
            If buf(8) = NS_COMMA And buf(17) = NS_TERMINATOR Then
                g_dRA = HexToDegB(buf, 0, 5) / 15#
                g_dDec = HexToDegB(buf, 9, 14)
            End If
#End If
        Else
'            buf = ScopeCommandString("E")
            buf = ScopeCommandBinary(ToByte(NS_GET_RADEC_LP))
            If buf(4) = NS_COMMA And buf(9) = NS_TERMINATOR Then
                g_dRA = HexToDegB(buf, 0, 3) / 15#
                g_dDec = HexToDegB(buf, 5, 8)
            End If
        End If
        If g_dDec > 180# Then
           g_dDec = g_dDec - 360#
        End If
    Case E_NEXSTAR58
        buf = ScopeCommandBinary(ToByte(NS_GET_RADEC_LP))
        If Length(buf) = 4 Then
            g_dRA = ByteToDegB(buf, 0, 1) / 15#
            g_dDec = ByteToDegB(buf, 2, 3)
            If g_dDec > 180# Then
                g_dDec = g_dDec - 360#
            End If
        End If
    Case E_ULTIMA
'        buf = ScopeCommandString("E" & vbCr)
        buf = ScopeCommandBinary(ToByte(NS_GET_RADEC_LP, NS_CR))

        sep1 = InByt(buf, NS_COMMA)
        sep2 = InByt(buf, NS_TERMINATOR)
        If sep1 >= 4 And sep2 >= 9 Then
            g_dRA = HexToDegB(buf, sep1 - 4, sep1 - 1) / 15#
            g_dDec = HexToDegB(buf, sep2 - 4, sep2 - 1)
        End If
    Case E_GT
'        g_serial.Transmit "E"
        buf = ScopeCommandBinary(ToByte(NS_GET_RADEC_LP))    ' "bb0bb0"
        If Length(buf) <> 6 Then
            Exit Sub
        End If
        If buf(2) = 0 And buf(5) = 0 Then
            g_dRA = ByteToDegB(buf, 0, 1) / 15#
            g_dDec = ByteToDegB(buf, 3, 4)
        End If
    Case Else
        RaiseError SCODE_NOT_IMPLEMENTED, _
                  ERR_SOURCE, _
                  "GetRaDec() " & MSG_NOT_IMPLEMENTED
    End Select
    m_lNextRADec = t + m_Coord_Upd_Interval            ' Success, update the timer
    Trace "GetRaDec " & g_dRA & ", " & g_dDec
    
'    If CorrectForPrecession Then
'        Dim dRA As Double, dDec As Double
'        PrecessDelta g_dRA, g_dDec, dRA, dDec
'        g_dRA = g_dRA + dRA
'        g_dDec = g_dDec + dDec
'    End If
    
Exit Sub

ErrGetRaDec:
    m_lNextRADec = t + m_Coord_Upd_Interval
    m_bActive = False
End Sub

Public Function GetSiderealTime() As Double

    If m_Version > Version_412 And m_Version < Version_GT_LO Then
        ' get the sidereal time from the HC
        CheckConnected
        Dim buf() As Byte, lst As Double
        buf = ScopeCommandBinary(ToByte(NS_GET_LST))
        lst = HexToDegB(buf, 0, 5)
'        Dim ss As String, i As Integer
'        For i = 0 To UBound(buf)
'            ss = ss & Chr(buf(i))
'        Next
'        Debug.Print ss
        GetSiderealTime = lst / 15#        ' return as hours
    Else
        ' use the UTCDate property to read the UTC from the GPS or PC clock
        Dim utc As Date, gst As Double, j2k As Double
        Dim y As Double, m As Double, d As Double
        Dim h As Double, mn As Double, s As Double
        Dim mjd As Double
        
        utc = GetUTCDate  ' read from the GPS if valid or the PC clock if not
        
        ' the cal_mjd or utc_gst subroutines in Astro32
        ' don't seem to give the right answers
        
        y = Year(utc)
        m = Month(utc)
        d = Day(utc)
        h = Hour(utc)
        mn = Minute(utc)
        s = Second(utc)
        
    ''    cal_mjd m, d, y, mjd
    '    utc_gst mjd, (utc - Int(utc)), gst
        
        ' days since J2000.0 - Keith Burnett
        j2k = 367# * y - Int(7# * (y + Int((m + 9#) / 12#)) / 4#) + _
                Int(275# * m / 9#) + d - 730531.5 + _
                (h + mn / 60# + s / 3600#) / 24#
        
        gst = 280.46061837 + 360.98564736629 * j2k
        gst = (gst + g_dLongitude)         ' local sidereal time
        
        range gst, 360#
        GetSiderealTime = gst / 15#        ' return as hours
    End If
End Function

Public Sub ScopeDeclinationRate(ByVal newVal As Double)
    ' rate now ArcSec/sec
    If m_Version >= Version_16 And g_eAlignMode <> algAltAz Then
        If RateIsInRange(newVal / 3600#, g_AxisRates) Then
            Dim mr As Long
            g_DeclinationRate = newVal
            mr = g_DeclinationRate * 1024#
            If g_TrackMode = E_TRACK_EQ_S Then
                mr = -mr
            End If
            MCRate ID_ALT, mr
        Else
        End If
        Trace "ScopeDeclinationRate " & g_DeclinationRate
    Else
        RaiseError SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
                    "Property DeclinationRate" & MSG_NOT_IMPLEMENTED
    End If
End Sub

Public Function ScopeDestinationSideOfPier(RA As Double, dec As Double) As PierSide
    If m_Version > Version_414 And m_Version < Version_GT_LO Then
        Dim buf() As Byte
        buf = ScopeCommandBinary(ToByte(NS_GET_DEST_PIER_SIDE, DegToHexB(RA * 15, True), NS_COMMA, DegToHexB(dec, True)))
        Select Case buf(0)
        Case NS_W
            ScopeDestinationSideOfPier = PierSide.pierEast 'looking West
        Case NS_E
            ScopeDestinationSideOfPier = PierSide.pierWest 'looking East
        Case Else
            ScopeDestinationSideOfPier = PierSide.pierUnknown '???
        End Select
    Else
        ' a very simple test, uses the current hour angle calculated from the Ra
        Dim ha As Double
        ha = now_lst(degrad(g_dLongitude)) - RA
        range ha, 24#
        If ha < 12# Then
            ScopeDestinationSideOfPier = PierSide.pierEast 'looking West
        Else
            ScopeDestinationSideOfPier = PierSide.pierWest 'looking East
        End If
    End If
End Function

Public Function ScopeEquatorialSystem() As EquatorialCoordinateType
    Select Case m_Version
    Case Is <= Version_413
        ScopeEquatorialSystem = equJ2000
    Case Is >= Version_GT_LO
        ScopeEquatorialSystem = equJ2000
    Case Else
        ScopeEquatorialSystem = equLocalTopocentric
    End Select
End Function

Public Function GetGuideRate(id As E_DEVICE_ID) As Double
    Dim pcg As Integer
    Dim buf() As Byte
    If ScopeCanPulseGuide Then
        ' read the rate from the Alt or Azm axis
        buf = AUXCommand(id, MC_GET_AUTOGUIDE_RATE, 1)
        pcg = Int(buf(0))
        If Not CanAuxGuide Then
            ' report rates of 50% or 99% only
            pcg = IIf(pcg > 192, 255, 128)
        End If
        GetGuideRate = (pcg / 256) * SIDEREAL_RATE_DEG_SEC
        Trace "GetGuideRate " & (pcg / 256) * SIDEREAL_RATE_DEG_SEC
    Else
        GetGuideRate = 0#
    End If
End Function

Public Sub SetGuideRate(id As E_DEVICE_ID, RateDegPerSec As Double)
    If RateIsInRange(RateDegPerSec, g_GuideRates) Then
        Dim pcg As Integer
        If ScopeCanPulseGuide Then
            ' set the rate for the alt or azm axis
            pcg = RateDegPerSec / SIDEREAL_RATE_DEG_SEC * 256
            ' set rates of 50% or 99% only
            pcg = IIf(pcg > 192, 255, 128)
            AUXCommand id, MC_SET_AUTOGUIDE_RATE, 0, pcg
            Trace "SetGuideRate " & id & "," & RateDegPerSec
        Else
            RaiseError SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
                        "Property SetGuideRate" & MSG_NOT_IMPLEMENTED
        End If
    Else
        RaiseError SCODE_PROP_RANGE_ERROR, ERR_SOURCE, _
                "Property SetGuideRate() " & MSG_PROP_RANGE_ERROR
    End If
End Sub

Public Sub SetRightAscensionRate(ByVal newVal As Double)
    'Rate is in seconds/sidereal second, convert to arcsec/sec
    Dim SecUTCSec As Double
    SecUTCSec = newVal * 0.9972695677 * 15
    If m_Version >= Version_16 And g_eAlignMode <> algAltAz Then
        If RateIsInRange(SecUTCSec / 3600#, g_AxisRates) Then
            Dim mr As Long
            g_RightAscensionRate = newVal
            Trace "SetRightAscensionRate " & newVal
            mr = SecUTCSec * 1024# + SIDEREAL_MC_RATE
            If g_TrackMode = E_TRACK_EQ_S Then
                mr = -mr
            End If
            If g_RightAscensionRate <> 0 Then
                MCRate ID_AZM, mr
                m_RaRateSet = True
            Else
                SetTrackingRate
            End If
        Else
            RaiseError SCODE_PROP_RANGE_ERROR, ERR_SOURCE, "Property RightAscensionRate" & MSG_PROP_RANGE_ERROR
        End If
    Else
        RaiseError SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
                    "Property RightAscensionRate" & MSG_NOT_IMPLEMENTED
    End If
End Sub

Public Function ScopeSlewing() As Boolean
' check if the scope is slewing.
' slewing will be assumed to be stopped after the max slew time has been exceeded and
' the GPS and Ultima will send the abort slew command.

    Dim buf() As Byte
    Dim byt As Byte
    Dim t As Single
    Dim sep1 As Integer
    
    Static dSettleTime As Date    ' timer time when the settle time has finished
    Static dNextSlewUpdate As Date    ' time when a new slew command can be issued
    
    ScopeSlewing = False

    If g_IsGuiding Then Exit Function
    
    Select Case m_eSlewing
    Case eNoSlew
        ' early exit if we believe we are not slewing
        dSettleTime = Now
        dNextSlewUpdate = Now
    Case eSettling
        ' delay reporting the slew has finished until after the settle time
        If Now >= dSettleTime Then
            m_eSlewing = eNoSlew
        End If
    Case eSlewing, eInitSlew
        ' throttle checking the slew state
        If Now < dNextSlewUpdate Then
            ScopeSlewing = True
            Exit Function
        End If
        ' check the scope for slewing,
        ' if we get a response that it has finished then set the state to Settling
        Select Case m_ScopeType
        Case E_NEXSTAR_GPS
            If Not m_bActive Then
                CheckConnected
                On Error Resume Next                    ' if there is no response then continue
'                buf = ScopeCommandString("L")
                buf = ScopeCommandBinary(ToByte(NS_IS_SLEWING))
                m_bActive = False
                On Error GoTo 0                         ' turn error trapping back on
                ' the reply should be "0#" or "1#"
                If buf(0) = NS_1 Then
                    m_eSlewing = eSlewing
                ElseIf buf(0) = NS_0 Then
                    m_eSlewing = IIf((m_eSlewing = eInitSlew), eNoSlew, eSettling)
                    Sleep SHORT_WAIT            ' delay to allow the slew to finish
                Else        ' no valid reply, assume still slewing
                    ' check to see if the max time for the slew has been exceeded
                    t = Timer - m_SlewStart
                    If (t > MAX_SLEW_TIME) Or _
                        (t < 0 And t + SPD > MAX_SLEW_TIME) _
                    Then
                        ScopeAbortSlew
                    End If
                End If
            End If
        Case E_NEXSTAR58
            On Error Resume Next       ' force errors to be ignored
            DoEvents
            ' this is based on what is seen by monitoring a N8
            ' it returns a '@' character when the slew is finished
            ' don't clear the buffers, we don't want to miss the '@'
            byt = g_serial.ReceiveByte
            If byt = &H40 Then
                m_eSlewing = IIf((m_eSlewing = eInitSlew), eNoSlew, eSettling)
            Else
                ' check to see if the max time for the slew has been exceeded
                t = Timer - m_SlewStart
                If (t > MAX_SLEW_TIME) Or _
                    (t < 0 And t + SPD > MAX_SLEW_TIME) _
                Then
                    m_eSlewing = eSettling
                End If
            End If
    
        Case E_ULTIMA
            CheckConnected
            On Error Resume Next                        ' if there is no response then continue
'            buf = ScopeCommandString("L" & vbCr)
            buf = ScopeCommandBinary(ToByte(NS_IS_SLEWING, NS_CR))
            m_bActive = False
            On Error GoTo 0                             ' turn error trapping back on
            sep1 = InByt(buf, NS_TERMINATOR)
            If buf(sep1 - 1) = NS_0 Then
                m_eSlewing = IIf((m_eSlewing = eInitSlew), eNoSlew, eSettling)
            ElseIf buf(sep1 - 1) = NS_1 Then
                m_eSlewing = eSlewing
            Else
                ' check to see if the max time for the slew has been exceeded
                t = Timer - m_SlewStart
                If (t > MAX_SLEW_TIME) Or _
                    (t < 0 And t + SPD > MAX_SLEW_TIME) _
                Then
                    ScopeAbortSlew
                End If
            End If
        Case Else
            m_eSlewing = IIf((m_eSlewing = eInitSlew), eNoSlew, eSettling)
        End Select
        
        ' go straight to NoSlew if its the first time
'        If dNextSlewUpdate = 0# And m_eSlewing = eSettling Then
'            m_eSlewing = eNoSlew
'        End If
        
'        dNextSlewUpdate = Timer + m_Coord_Upd_Interval / 1000#
'        range dNextSlewUpdate, SPD
        dNextSlewUpdate = Now + m_Coord_Upd_Interval / (SPD * 1000#)
        
        ' set the minimum time when the slew will have finished.
'        dSettleTime = Timer + g_iSettleTime
'        range dSettleTime, SPD
        dSettleTime = Now + g_iSettleTime / SPD
        
    End Select  ' m_eSlewing
    
    Trace "ScopeSlewing " & Not (m_eSlewing = eNoSlew)

    ScopeSlewing = Not (m_eSlewing = eNoSlew)
End Function

Public Sub ScopePark()
    Select Case m_ScopeType
    Case E_NEXSTAR_GPS
        Dim buf() As Byte
        CheckConnected
'        If m_Version >= Version_421 Then
'            ' go to the scope home position
'            buf = ScopeCommandBinary(ToByte(NS_GOTO_HOME))
'            ' wait for the slew to stop
'            Do
'                Sleep LONG_WAIT
'                buf = ScopeCommandBinary(ToByte(NS_IS_SLEWING))
'            Loop Until buf(0) = NS_0
'            ' hibernate
'            buf = ScopeCommandBinary(ToByte(NS_HIBERNATE))
'            g_AtPark = True
'        Else
            ' try to use NR
            If GotoHome Then
                Hibernate
                Exit Sub
            End If
            ' save the current tracking state
            Dim tt As Boolean
            tt = g_Tracking
            ' turn tracking off
            SetTracking False
            
            ' use the MC slew
            On Error GoTo AuxErr
            buf = DegToByteB(m_ParkAlt)
            AUXCommand ID_ALT, MC_GOTO_FAST, 0, buf(0), buf(1)
            buf = DegToByteB(m_ParkAz)
            AUXCommand ID_AZM, MC_GOTO_FAST, 0, buf(0), buf(1)
    '        Dim buf() As Byte
            Do
                Sleep LONG_WAIT
                DoEvents
                buf = AUXCommand(ID_ALT, MC_SLEW_DONE, 1, 0)
            Loop Until buf(0) <> &H0
            Do
                Sleep LONG_WAIT
                DoEvents
                buf = AUXCommand(ID_AZM, MC_SLEW_DONE, 1, 0)
            Loop Until buf(0) <> &H0
        
    '        SlewAltAz m_ParkAz, m_ParkAlt
    '        While ScopeSlewing
    '            g_util.WaitForMilliseconds LONG_WAIT
    '            DoEvents
    '        Wend
            g_AtPark = True
            g_Tracking = tt     ' value of tracking reflects what it would be when the scope is unparked
            ' save the values of parked and tracking
            g_Profile.WriteValue g_DriverID, IS_PARKED, CInt(g_AtPark)
            g_Profile.WriteValue g_DriverID, IS_TRACKING, CInt(g_Tracking)
'        End If
        m_Aligned = False
    Case Else
        RaiseError SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
                    "Method Park()" & MSG_NOT_IMPLEMENTED
    End Select
    Trace "ScopePark"
Exit Sub

AuxErr:
    RaiseError SCODE_IMPLEMENTATION_ERROR, ERR_SOURCE, _
                "Method Park() AuxCommand" & MSG_IMPLEMENTATION_ERROR
End Sub

Public Sub ScopeSetPark()
    Dim buf() As Byte
    Select Case m_ScopeType
    Case E_NEXSTAR_GPS
'        GetAltAz
'        If m_Version >= Version_421 And m_Version < Version_GT_LO Then
'            ' set the scope home position
'            buf = ScopeCommandBinary(ToByte(NS_SET_HOME))
'        Else
            ' try to use NR
            'If SetHome Then Exit Sub
            ' use MC commands
            On Error GoTo AuxErr
            buf = AUXCommand(ID_ALT, MC_GET_POSITION, 3, 0)
            m_ParkAlt = ByteToDegB(buf, 0, 2)
            buf = AUXCommand(ID_AZM, MC_GET_POSITION, 3, 0)
            m_ParkAz = ByteToDegB(buf, 0, 2)
    '        m_ParkAlt = g_dAlt
    '        m_ParkAz = g_dAz
            g_Profile.WriteValue g_DriverID, PARK_ALTITUDE, Str(m_ParkAlt)
            g_Profile.WriteValue g_DriverID, PARK_AZIMUTH, Str(m_ParkAz)
'        End If
    Case Else
        RaiseError SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
                    "Method SetPark()" & MSG_NOT_IMPLEMENTED
    End Select
    Trace "ScopeSetPark"
Exit Sub
AuxErr:
    RaiseError SCODE_IMPLEMENTATION_ERROR, ERR_SOURCE, _
                "Method SetPark() AuxCommand" & MSG_IMPLEMENTATION_ERROR
End Sub

Public Function ScopeSideOfPier() As PierSide
    If m_Version > Version_414 And m_Version < Version_GT_LO Then
        Dim buf() As Byte
        buf = ScopeCommandBinary(ToByte(NS_GET_PIER_SIDE))
        Select Case buf(0)
        Case NS_W
            ScopeSideOfPier = PierSide.pierEast 'looking West
        Case NS_E
            ScopeSideOfPier = PierSide.pierWest 'looking East
        Case Else
            ScopeSideOfPier = PierSide.pierUnknown 'Who knows?
        End Select
    Else
        ' tested on 3.01 with HcAnywhere
        GetAltAz
        Select Case g_ScopeAlt
        Case -90 To 90
            ScopeSideOfPier = PierSide.pierWest 'looking East
        Case Else
            ScopeSideOfPier = PierSide.pierEast 'looking West
        End Select
    End If
End Function

'Here 's my take on side of pier, as a diagram.

' SOP Side of Pier
' DSOP destination side of pier
' |---M---| the zone either side of the meridian where the scope can be
' on either side of the pier.

' position --E--|---M---|--W--

' Set SOP    E    E   E    E
' SOP =      E    E   W    W
' set mode            E    E
' DSOP =     E    E   E    W
' Action             flip
' return     OK   OK  OK   Error

' Set SOP    W    W   W    W
' SOP =      E    E   W    W
' set mode   W    W
' DSOP =     E    W   W    W
' Action         flip
' return    Error OK  OK   OK

' What this means is:
' If the scope is already on the side of pier requested then nothing is
' done.
' If the scope is on the "wrong" side of the pier then an attempt is
' made to switch it to the side requested. I can do this by setting the
' meridian mode and getting the destination side of pier for the current
' position.
' If the scope can't change to the side requested then an error is raised.
' If the scope can slew to the requested side then a slew is done to put
' the scope on the right side.

Public Sub SetSideOfPier(Side As PierSide)
    Dim mm As Long, newmm As Long, sop As PierSide, dsop As PierSide
    If m_Version > Version_414 And m_Version < Version_GT_LO Then
        ' read the side of pier
        sop = ScopeSideOfPier
        If sop = Side Then
            ' no action, the scope is on the correct side
            Exit Sub
        End If
        ' set the scope meridian mode to favour the side requested
        Select Case Side
        Case PierSide.pierWest
            newmm = FAVOUR_EAST
        Case PierSide.pierEast
            newmm = FAVOUR_WEST
        Case Else
            Exit Sub
        End Select
        SetMeridianMode newmm
        'Get the DSOP for the current position now the meridian mode has been set
        GetRaDec
        dsop = ScopeDestinationSideOfPier(g_dRA, g_dDec)
        ' if the DSOP is different to the side to be set then raise
        ' an error because the side can't be reached
        If dsop <> Side Then
            RaiseError SCODE_PROP_RANGE_ERROR, ERR_SOURCE, _
                "Property Let SideOfPier" & MSG_PROP_RANGE_ERROR
        Else
            ' the DSOP is the same as the side so do a slew to the current position, this should do the flip.
            SlewAsync g_dRA, g_dDec, True
        End If
    Else
        RaiseError SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
                    "Property Let SideOfPier" & MSG_NOT_IMPLEMENTED
    End If
End Sub

Public Sub ScopeSync()
        
    CheckSlewAllowed False
    Select Case m_ScopeType
    Case E_ULTIMA
'        ScopeCommandString "F" & DegToHex(g_dTargetRA * 15#) & "," & DegToHex(g_dTargetDec) & vbCr
        ScopeCommandBinary ToByte(NS_SYNC, DegToHexB(g_dTargetRA * 15#), NS_COMMA, DegToHexB(g_dTargetDec), NS_CR)
    Case Else
        If m_Version >= Version_41 And m_Version < Version_GT_LO Then
            ' these scopes support the new sync command
            ScopeCommandBinary ToByte(NS_SYNC_HP, DegToHexB(g_dTargetRA * 15#, True), NS_COMMA, DegToHexB(g_dTargetDec, True))
            Trace "new Scope Sync"
        Else
            ' use the driver based "sync"
            GetRaDec
            g_OffsetRa = g_dTargetRA + g_OffsetRa - g_dRA
            g_OffsetDec = g_dTargetDec + g_OffsetDec - g_dDec
            Trace "ScopeSync"
        End If
        m_lNextRADec = 0    ' force read of RaDec
        
    End Select
End Sub

Public Sub ScopeUnpark()
    g_AtPark = False
    g_Profile.WriteValue g_DriverID, IS_PARKED, Int(g_AtPark)
    Select Case m_ScopeType
    Case E_NEXSTAR_GPS
        CheckConnected
'        If m_Version >= Version_421 And m_Version < Version_GT_LO Then
'            If useScopeTime Then
'            Else
'                ' set the time from the PC
'                ' get the time zone, this includes DST
'                Dim buf() As Byte
'                Dim tz As Integer, dst As Integer
'                Dim dt As Date
'                tz = -utc_offs / (60# * 15#)      ' in 1/4 hours
'                buf = Space(20)
'                ' get the DST in hours, 0 or 1
'                dst = tz_name(buf, DATE_LOCALTZ)
'
'                ' remove the DST offset from the time zone
'                tz = tz - (dst * 4)
'                dt = Now
'                ' use the new I command
'                ScopeCommandBinary ToByte(NS_SET_TIME_NEW, _
'                                    Hour(dt), Minute(dt), Second(dt), _
'                                    Month(dt), Day(dt), Year(dt) - 2000, tz And &HFF, dst)
'                Sleep 100
'            End If
'            ' wake up using the HC command
'            buf = ScopeCommandBinary(ToByte(NS_WAKEUP, NS_1))
'            m_Aligned = (buf(0) = 1)
'        Else
'            ' wakeup using the NR command
'            WakeUp
            If g_Tracking Then
                SetTracking True   ' tracking is only started if required
            End If
'        End If
        Trace "ScopeUnpark " & g_Tracking
    Case Else
        RaiseError SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
                    "Method Unpark()" & MSG_NOT_IMPLEMENTED
    End Select
End Sub

Public Sub ScopePulseGuide(ByVal Direction As GuideDirections, _
                        ByVal Duration As Long)
    ' guide at the specified rate for the time set
    ' make the rate relative to the sidereal rate
    ' it will only work for a polar mount.
    ' This version is synchronous
    CheckConnected
    CheckParked
    If Duration > 32500 Or Duration < 0 Then
        RaiseError SCODE_PROP_RANGE_ERROR, ERR_SOURCE, "Method PulseGuide Duration " & _
                MSG_PROP_RANGE_ERROR
    End If
    Debug.Print "ScopePulseGuide m_bActive " & m_bActive
'    Wait
    Select Case m_ScopeType
    Case E_NEXSTAR_GPS
    
        If m_Version >= Version_16 And (g_eAlignMode <> algAltAz) Then
            g_DeclinationRate = 0
            g_RightAscensionRate = 0
            
            Select Case Direction
            Case GuideDirections.guideNorth, GuideDirections.guideSouth, _
                GuideDirections.guideEast, GuideDirections.guideWest
                
                ' try to aux guide
                If AuxGuide(Direction, Duration) Then
                    Exit Sub
                Else    ' use the PC controlled guiding
                    g_PulseGuide.PulseGuide Direction, Duration
                    If asyncGuiding Then
                        Sleep Duration
                    End If
                End If

            Case Else
                RaiseError SCODE_PROP_RANGE_ERROR, ERR_SOURCE, _
                        "Method PulseGuide() " & MSG_PROP_RANGE_ERROR
                Exit Sub
            End Select
        Else
            RaiseError SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
                    "Method PulseGuide() " & MSG_NOT_IMPLEMENTED
        End If
    Case Else
        RaiseError SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
                    "Method PulseGuide()" & MSG_NOT_IMPLEMENTED
    End Select
Exit Sub

ErrNXGPS:
    ' catch the NXGPS error during tracking
    ' try to stop guiding
    Trace "PulseGuide error " & Err.Description
    Sleep SHORT_WAIT
    SetTracking g_Tracking
End Sub

Public Sub SetTracking(ByVal newVal As Boolean)
    Select Case m_ScopeType
    Case E_NEXSTAR_GPS
        g_Tracking = newVal ' allow the tracking state to change while parked
        If Not g_AtPark Then
'            ScopeCommandBlind "T" & Chr$(IIf(g_Tracking, TrackMode, E_TRACK_OFF))
            ScopeCommandBinary ToByte(NS_SET_TRACKING, IIf(g_Tracking, TrackMode, E_TRACK_OFF))
        End If
        g_Profile.WriteValue g_DriverID, IS_TRACKING, CInt(g_Tracking)
        Trace "SetTracking " & g_Tracking
    Case Else
        RaiseError SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
                    "Property Tracking" & MSG_NOT_IMPLEMENTED
    End Select
End Sub

Public Sub ScopeTrackingRate(value As DriveRates)
' implemented for equatorial mounts only
' untested
    If m_ScopeType = E_NEXSTAR_GPS And m_Version >= Version_16 Then
        Select Case g_eAlignMode
        Case algPolar, algGermanPolar
            CheckConnected
            
            Select Case value
            Case driveSidereal, driveSolar, driveLunar
                g_DriveRate = value
                SetTrackingRate
                g_DeclinationRate = 0
                g_RightAscensionRate = 0
                Trace "ScopeTrackingRate " & value
            Case Else
                RaiseError SCODE_PROP_RANGE_ERROR, ERR_SOURCE, "Property Let TrackingRate " & MSG_PROP_RANGE_ERROR
            End Select
        Case Else
            If value <> driveSidereal Then
                RaiseError SCODE_PROP_RANGE_ERROR, ERR_SOURCE, "Property Let TrackingRate " & MSG_PROP_RANGE_ERROR
            End If
        End Select
    Else
        If value <> driveSidereal Then
            RaiseError SCODE_PROP_RANGE_ERROR, ERR_SOURCE, "Property Let TrackingRate " & MSG_PROP_RANGE_ERROR
        End If
    End If
End Sub

Public Sub GetExtLocation()
    Dim buf() As Byte
    If m_Extended Then
'        buf = ScopeCommandString("w")
        buf = ScopeCommandBinary(ToByte(NS_GET_LOCATION))
        g_dLatitude = DecodeDegreesB(buf, 0)
        g_dLongitude = DecodeDegreesB(buf, 4)
        Trace "GetExtLocation " & g_dLatitude & "," & g_dLongitude
    End If
End Sub

Public Sub SetExtLocation()
    If m_Extended Then
        ' write the value to the scope
        ScopeCommandBinary ToByte(NS_SET_LOCATION, EncodeDegreesB(g_dLatitude), EncodeDegreesB(g_dLongitude))
        Trace "SetExtLocation " & g_dLatitude & "," & g_dLongitude
    End If
End Sub

Public Function GetUTCDate() As Date
    ' Get the UTC date using the GPS if it is present and has a valid time
    Dim buf() As Byte
    Static yr As Integer, mn As Integer, dy As Integer  'pwgs: Changed to static to retain values for testing between calls
    Dim h As Integer, m As Integer, s As Integer
    Dim tz As Integer, dst As Integer
    
    Static LastUTCDate As Date, LastUTCDateTimeStamp As Date, GPSResyncTime As Date, GPSLinkRetryTime As Date, GPSTimeLinked As Variant 'pwgs: Extra variables for time cacheing
    Dim OffSet As Date  'pwgs: Work variable to hold calculated time offset
    Dim utc As Date
    
    On Error GoTo ErrUTC
    
    CheckConnected  'pwgs: Added to stop time appearing in Maxim when the scope is not connected - this approach is used in the Meade and Simulator scope drivers
    
    If m_Extended Then
        ' read the local time from the HC
'        buf = ScopeCommandString("h")
'        If m_Version >= Version_421 And m_Version < Version_GT_LO Then
'            ' use the new i command
'            buf = ScopeCommandBinary(ToByte(NS_GET_TIME_NEW))
'            tz = buf(6)     ' time zone offset in 1/4 hours
'            If tz > &H7F Then tz = tz - &H100
'            tz = tz * 15    ' offset in minutes
'        Else
            ' use the h commnd
            buf = ScopeCommandBinary(ToByte(NS_GET_TIME))
            tz = buf(6)     ' time zone offset in hours
            If tz > &H7F Then tz = tz - &H100
            tz = tz * 60    ' offset in minutes
'        End If
        h = buf(0)
        m = buf(1)
        s = buf(2)
        mn = buf(3)
        dy = buf(4)
        yr = buf(5) + 2000
        dst = buf(7)
        ' local time
        utc = DateSerial(yr, mn, dy) + TimeSerial(h, m, s)
        ' allow for dst and time zone
        utc = utc - TimeSerial(dst, 0, 0)
        utc = utc - TimeSerial(0, tz, 0)
        Trace "Extended UTCDate: " & utc
        GetUTCDate = utc
        Exit Function
    ElseIf m_HasGPS Then
       
        If (Not GPSTimeLinked) And (Now() > GPSLinkRetryTime) Then
            buf = AUXCommand(ID_GPS, GPS_TIME_VALID, 1)
            If buf(0) = 1 Then 'pwgs: We have GPS time link!
                GPSTimeLinked = True
                Trace "UTCDate: GPS Time Synchronised"
            Else 'pwgs: No GPS time link
                GPSLinkRetryTime = Now() + GPS_LINK_RETRY_INTERVAL / SPD
                Trace "UTCDate: GPS time not synchronised, trying again in " & GPSLinkRetryTime
            End If
        End If
        
        If GPSTimeLinked Then
            If Now() > GPSResyncTime Then
                If CInt(Format(Now() - LastUTCDateTimeStamp + LastUTCDate, "yyyy")) <> yr Then
                    Trace "UTCDate YEAR Refresh: " & CInt(Format(Now() - LastUTCDateTimeStamp + LastUTCDate, "yyyy")) & "#" & yr
                    buf = AUXCommand(ID_GPS, GPS_GET_YEAR, 2)
                    yr = buf(0) * &H100 + buf(1)
                End If
                If CInt(Format(Now() - LastUTCDateTimeStamp + LastUTCDate, "dd")) <> dy Then
                    Trace "UTCDate DATE Refresh: " & CInt(Format(Now() - LastUTCDateTimeStamp + LastUTCDate, "d")) & "#" & dy
                    buf = AUXCommand(ID_GPS, GPS_GET_DATE, 2)
                    mn = buf(0)
                    dy = buf(1)
                End If
                
                buf = AUXCommand(ID_GPS, GPS_GET_TIME, 3)
                h = buf(0)
                m = buf(1)
                s = buf(2)
                utc = DateSerial(yr, mn, dy) + TimeSerial(h, m, s)
                LastUTCDate = utc
                LastUTCDateTimeStamp = Now()
                GPSResyncTime = Now() + GPS_RESYNC_INTERVAL / SPD
                Trace "GPSSync UTCDate: " & utc & " Resync Time: " & GPSResyncTime
                GetUTCDate = utc
                Exit Function
            Else
                OffSet = CDate((Now() - LastUTCDateTimeStamp))
                utc = LastUTCDate + OffSet
                OffSet = CDate(CDate(GPS_RESYNC_INTERVAL / SPD) - OffSet)
                Trace "GPSCalc UTCDate: " & utc & " Time to resync: " & OffSet
                GetUTCDate = utc
                Exit Function
            End If
        End If
    End If
    ' if all else fails, drop through and
    ' read the date from the PC clock
ErrUTC:
    On Error GoTo 0
    CheckConnected  'pwgs: Added to stop time appearing in Maxim when the scope is not connected - this approach is used in the Meade and Simulator scope drivers
    m_bActive = False
    utc = CDate(CDbl(Now()) + (CDbl(utc_offs()) / SPD)) 'pwgs: Changed sign to create correct UTC time
    Trace "PC UTCDate: " & utc
    GetUTCDate = utc
End Function

Public Sub LetUTCDate(ByVal newVal As Date)
    If m_Extended Then
        Dim buf As String, tz As Integer, dst As Integer
        Dim dt As Date
        
        ' get the time zone from the PC, this includes DST
        tz = -utc_offs / 60#       ' in minutes
        buf = Space(20)
        ' get the DST in hours, 0 or 1
        dst = tz_name(buf, DATE_LOCALTZ)
        
        ' convert UTC to local time
        dt = newVal + CDate(tz / (24# * 60#))
        ' remove the DST offset (in minutes) from the time zone
        tz = tz - dst * 60
        ' set the time in the scope, use the PC time zone and DST
'        If m_Version >= Version_421 And m_Version < Version_GT_LO Then
'            ' use the new I command
'            ' tz in units of 15 minutes
'            tz = tz / 15
'            ScopeCommandBinary ToByte(NS_SET_TIME_NEW, _
'                                Hour(dt), Minute(dt), Second(dt), _
'                                Month(dt), Day(dt), Year(dt) - 2000, tz And &HFF, dst)
'        Else
            ' use the H command
            ' tz in hours
            tz = tz / 60
            ScopeCommandBinary ToByte(NS_SET_TIME, _
                                Hour(dt), Minute(dt), Second(dt), _
                                Month(dt), Day(dt), Year(dt) - 2000, tz And &HFF, dst)
'        End If
        Trace "LetUTCDate " & newVal
    Else
        RaiseError SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
                    "Property Let UTCDate" & MSG_NOT_IMPLEMENTED
    End If
    
End Sub

Public Sub ScopeAbortSlew()
    If g_AtPark Then
        RaiseError SCODE_SCOPE_PARKED, ERR_SOURCE, "Method AbortSlew() " & MSG_SCOPE_PARKED
    Else
        Select Case m_ScopeType
        Case E_NEXSTAR_GPS
'            ScopeCommandBlind "M"
            ScopeCommandBinary ToByte(NS_ABORT_SLEW)
            m_bAbortSlew = True
            g_util.WaitForMilliseconds SHORT_WAIT
'            If m_Version >= Version_16 And (g_eAlignMode <> algAltAz) Then
            If m_Version >= Version_16 Then
                On Error Resume Next
                MoveHC axisPrimary, 0
                MoveHC axisSecondary, 0
                ' there's a problem with this, it seems to cause a slow runaway in Ra
                ' on the AS-GT, CGEM and CGE Pro scopes with HC version 4.21.
                ' it only seems to affect version 5.0.29, no idea why.
                ' try removing the HC commands
'                AUXCommand ID_ALT, MC_MOVE_POS, 0
'                Sleep SHORT_WAIT
'                AUXCommand ID_AZM, MC_MOVE_POS, 0
'                Sleep LONG_WAIT
'                SetTracking g_Tracking
                On Error GoTo 0
            End If
        Case E_ULTIMA
'            ScopeCommandBlind "M" & vbCr
            ScopeCommandBinary ToByte(NS_ABORT_SLEW, NS_CR)
            m_bAbortSlew = True
            g_util.WaitForMilliseconds LONG_WAIT
        Case Else
            RaiseError SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
                        "Method AbortSlew()" & MSG_NOT_IMPLEMENTED
        End Select
        m_eSlewing = eNoSlew
    End If
    Trace "ScopeAbortSlew"
End Sub

Public Sub MoveHC(Axis As TelescopeAxes, HCButton As Integer)
    ' This will command the scope to move in the specified directions
    'at the specified HC button rate.
    
    CheckConnected
    CheckParked
    
'    Wait ' for any command to finish
    
    Select Case m_ScopeType
    Case E_NEXSTAR_GPS
        If m_Version >= Version_16 Then
            If HCButton >= -9 And HCButton <= 9 Then
                Dim msgID As E_MESSAGE_ID
                Dim devID As E_DEVICE_ID
                Dim br As Integer
                Select Case Axis
                Case TelescopeAxes.axisPrimary
                    devID = ID_AZM
                Case TelescopeAxes.axisSecondary
                    devID = ID_ALT
                Case Else
                    RaiseError SCODE_PROP_RANGE_ERROR, ERR_SOURCE, _
                            "Method MoveAxis() " & MSG_PROP_RANGE_ERROR
                    Exit Sub
                End Select
                On Error GoTo ErrNXGPS
                Select Case HCButton
                Case Is > 0
                    msgID = MC_MOVE_POS
                    br = HCButton
                Case Is < 0
                    msgID = MC_MOVE_NEG
                    br = -HCButton
                Case Else
                    ' set 0 rate restores tracking
                    If g_Tracking Then
                        ' this should set this axis back to the normal tracking rate
                        AUXCommand devID, MC_MOVE_POS, 0, 0
                        ' required to restore tracking for the AltAz scopes
                        If g_eAlignMode = algAltAz Then
                            SetTracking True
                        End If
                    Else
                        AUXCommand devID, MC_SET_POS_GUIDERATE, 0, 0, 0, 0
                    End If
                    Trace "MoveHC " & Axis & ", stop"
                    Exit Sub
                End Select
                CheckAligned        ' check if the scope is aligned
                AUXCommand devID, msgID, 0, br
                Trace "MoveHC " & Axis & "," & HCButton
            Else
                RaiseError SCODE_PROP_RANGE_ERROR, ERR_SOURCE, _
                        "Method MoveHC() " & MSG_PROP_RANGE_ERROR
                    Exit Sub
            End If
        Else
            RaiseError SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
                    "Method MoveHC() " & MSG_NOT_IMPLEMENTED
        End If
    Case Else
        RaiseError SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
                    "Method MoveHC()" & MSG_NOT_IMPLEMENTED
    End Select
Exit Sub

ErrNXGPS:
    ' catch the NXGPS error during tracking
    ' try to restore tracking
    Sleep SHORT_WAIT
    SetTracking g_Tracking

End Sub

Public Function IsGuiding() As Boolean
    If isGuidingReturnsFalse Then
        IsGuiding = False
        Exit Function
    End If
    ' Check isGuiding unconditionally because
    ' a long guide command may be being done using timers.
    If g_IsGuiding Then
        IsGuiding = g_IsGuiding
        Exit Function
    End If
    
    If CanAuxGuide Then
        ' read the guide state from the motor controllers
        ' read both because we don't know which is active
        Dim buf() As Byte
        On Error GoTo Err
        buf = AUXCommand(ID_ALT, MTR_IS_AUX_GUIDE_ACTIVE, 1, 0)
        If buf(0) = 1 Then
            IsGuiding = True
            Exit Function
        End If
        buf = AUXCommand(ID_AZM, MTR_IS_AUX_GUIDE_ACTIVE, 1, 0)
        If buf(0) = 1 Then
            IsGuiding = True
            Exit Function
        End If
        IsGuiding = False
    End If
Exit Function
Err:
    IsGuiding = False
End Function

Public Sub MCRate(devID As E_DEVICE_ID, Rate As Long)
' starts the axis moving at the specified rate
' If this is done to the AZM axis then the Tracking enabled flag in the MC
' will be disabled. Use the 2 byte MC_SET_NEG/POS_GUIDERATE to set tracking to
' the sidereal rate and allow tracking to be resumed.

    Dim gr As Long
    Dim msgID As E_MESSAGE_ID
    Dim h As Byte, m As Byte, l As Byte
    
    On Error GoTo Err
    ' check if the scope is aligned
    ' send commands to check the align and tracking states if necessary
    CheckAligned
    Select Case Rate
    Case Is >= 0
        msgID = MC_SET_POS_GUIDERATE
    Case Is < 0
        msgID = MC_SET_NEG_GUIDERATE
    End Select
    gr = Abs(Rate)
    h = CByte(gr / &H10000)
    m = CByte((gr / &H100) And &HFF)
    l = CByte(gr Mod &H100)
    AUXCommand devID, msgID, 0, h, m, l   ' start the move
    Trace "MCRate " & devID & "," & Rate
Exit Sub

Err:
    ' catch the NXGPS error during tracking
    ' try to restore tracking
    Sleep SHORT_WAIT
    SetTracking g_Tracking
End Sub

Private Function AUXCommand(DeviceID As E_DEVICE_ID, msgID As E_MESSAGE_ID, responseBytes As Integer, ParamArray data() As Variant) As Byte()
' send AUX command and get a reply
' NexStar GPS protocol only
' raises an error if there is a timeout on the command
    Dim d(0 To 2) As Byte, i As Integer
    Dim Is3B As Boolean
    
    For i = 0 To 2
        If i <= UBound(data) Then
            d(i) = CByte(data(i) And &HFF)
            If d(i) = &H3B Then Is3B = True
        Else
            d(i) = 0
        End If
    Next
    
    ' set the length
    i = UBound(data) + 2
    If i > 4 Then i = 4
    If Is3B Then i = 4
    
    AUXCommand = ScopeCommandBinary(ToByte(NS_AUX_CMD, i, DeviceID, msgID, d, responseBytes))
End Function

Public Function VerStr() As String
    VerStr = Int(m_Version / &H100) & "." & val(m_Version Mod &H100)
End Function

Public Sub Trace(msg As String)
    If m_fTracing Then
        m_TraceFile.WriteLine "[" & Format$(Now(), "Hh:Nn:Ss") & "] " & msg
    End If
End Sub

' set the Track rate for the GPS scopes
Private Sub GPSTrackRate(ByVal Rate As Long, UDNotLR As Boolean)
    ' Rate is in the GPS units of 1/1024 ArcSec per sec.
    Dim rh As Integer, rm As Integer, rl As Integer, s As E_MESSAGE_ID, ud As E_DEVICE_ID, buf As String
    
    ' determine the high, mid and low bytes of the rate
    rh = Int(Abs(Rate) \ &H10000)
    rm = Int((Abs(Rate) \ &H100) And &HFF)
    rl = Abs(Rate) Mod &H100
    ' left/right or Up/down?
    ud = IIf(UDNotLR, E_DEVICE_ID.ID_ALT, E_DEVICE_ID.ID_AZM)
    ' Positive or negative move?
    s = IIf(Rate >= 0, E_MESSAGE_ID.MC_SET_POS_GUIDERATE, E_MESSAGE_ID.MC_SET_NEG_GUIDERATE)
    On Error GoTo ErrTRate
    AUXCommand ud, s, 0, rh, rm, rl
Exit Sub

ErrTRate:
    m_bActive = False
End Sub

'
' CheckConnected() - Raise an error if the scope is not connected
'
Public Sub CheckConnected()
    If Not g_bConnected Then
        RaiseError SCODE_NOT_CONNECTED, _
                  ERR_SOURCE, _
                  MSG_NOT_CONNECTED
    End If
End Sub

Private Sub CheckParked()
' raise an error if the scope is parked
    If g_AtPark Then
        RaiseError SCODE_SCOPE_PARKED, _
                  ERR_SOURCE, _
                  MSG_SCOPE_PARKED
    End If
End Sub

Private Sub CheckTracking(AltAzSlew As Boolean)
' check the state of tracking and raise an error if it is in the wrong state.
    If m_AllowAllSlews Then
        Exit Sub
    End If
    If AltAzSlew And g_Tracking Then
        RaiseError SCODE_ALTAZ_SLEW_ERROR, _
            ERR_SOURCE, _
            MSG_ALTAZ_SLEW_ERROR
    ElseIf Not AltAzSlew And Not g_Tracking Then
        RaiseError SCODE_RADEC_SLEW_ERROR, _
            ERR_SOURCE, _
            MSG_RADEC_SLEW_ERROR
    End If
End Sub

Public Sub CheckSlewAllowed(AltAzSlew As Boolean)
'combine all checks that a slew is allowed
' and raise an error if it is in the wrong state for the slew
    CheckConnected
    CheckParked
    CheckTracking AltAzSlew
    If (Not AltAzSlew And Not (g_bTargetDecValid And g_bTargetRaValid)) Then
        RaiseError SCODE_PROP_NOT_SET, ERR_SOURCE, MSG_PROP_NOT_SET
    End If
    
    If Not AltAzSlew And Not CheckAligned Then
        RaiseError SCODE_NOT_VALID, ERR_SOURCE, "Slew is not possible if the scope is not aligned"
    End If

    If m_RaRateSet And g_eAlignMode <> algAltAz Then
        ' restore tracking
        SetTrackingRate
        m_RaRateSet = False
    End If
    ' sets offset rates to zero
    g_DeclinationRate = 0
    g_RightAscensionRate = 0
End Sub

Private Function IdentifyScope() As Boolean
' this is the original scope identification code
    Dim bIn As Byte, buf() As Byte
    Dim sep1 As Integer, sep2 As Integer
    Dim retry As Integer
    
    retry = 0
    IdentifyScope = True
    
TryAgain:
    g_serial.ClearBuffers
    ' sending "?E" as one string doesn't seem to work with the NS11
    ' so try sending as two characters separated by a short pause
    On Error GoTo NoResponse
    g_serial.TransmitBinary ToByte(NS_QUESTION)
    g_util.WaitForMilliseconds 100   ' shorten this as much as possible
    g_serial.TransmitBinary ToByte(NS_GET_RADEC_LP)
    g_util.WaitForMilliseconds 100   ' shorten this as much as possible
    ' it WBN to read the whole string and make deductions based on its length.
    ' this really needs a count property on g_serial
    bIn = g_serial.ReceiveByte
'    buf = g_serial.Receive
    Select Case bIn
'    Select Case Asc(Mid$(buf, 1, 1))
    Case NS_TERMINATOR
        ' we would expect "#bbbb"
        m_ScopeType = E_NEXSTAR58
        g_ScopeTypeStr = "NexStar 5 or 8"
        Exit Function
'    Case Asc("0") To Asc("9"), Asc("A") To Asc("F")
    Case &H30 To &H39, &H41 To &H46
        ' we would expect hhhh,hhhh#
        m_ScopeType = E_NEXSTAR_GPS
        g_ScopeTypeStr = "NexStar GPS or i"     ' it can be GT, GPS, i, CGE or ASC
        Exit Function
    Case 1
        ' we would expect 1bb0bb0
        ' This is the early GT and Tasco scopes, they are not supported
        m_ScopeType = E_GT
        g_ScopeTypeStr = "NexStar GT"
        Exit Function
    Case Else
        m_ScopeType = E_UNKNOWN
        g_ScopeTypeStr = "Unknown"
    End Select
    retry = retry + 1
    If retry < 3 Then
        GoTo TryAgain
    End If
    IdentifyScope = False
Exit Function

NoResponse:
    ' try sending a CR to trigger the Ultima into responding
    Err.Clear
'    g_serial.Transmit vbCr
    g_serial.TransmitBinary ToByte(NS_CR)
    buf = g_serial.ReceiveTerminatedBinary(ToByte(NS_TERMINATOR))
    ' we expect <A>hhhh,hhhh#
    sep1 = InByt(buf, NS_COMMA)
    sep2 = InByt(buf, NS_TERMINATOR)
    If sep1 >= 4 And sep2 >= sep1 + 5 Then
        m_ScopeType = E_ULTIMA
        g_ScopeTypeStr = "Ultima 2000"
        g_dRA = HexToDegB(buf, sep1 - 4, sep1 - 1) / 15#
        g_dDec = HexToDegB(buf, sep1 + 1, sep2 - 1)
        If g_dDec > 180# Then g_dDec = g_dDec - 360#
        Exit Function
    End If
    m_ScopeType = E_UNKNOWN
    g_ScopeTypeStr = "Unknown"
    retry = retry + 1
    If retry < 3 Then
        GoTo TryAgain
    End If
    IdentifyScope = False
End Function

Public Sub SlewAsync(RightAscension As Double, Declination As Double, Optional noChecks As Boolean = False)
    Dim RaH As Integer, RaL As Integer, DecH As Integer, DecL As Integer
    Dim RA As Long, dec As Long
'    Dim buf As String
    Dim hp As Boolean
    Dim cmd As Byte
    
'    Wait
    If Not noChecks Then
        CheckSlewAllowed False
    End If
    If g_IsGuiding Then
        g_PulseGuide.Halt
    End If
    
'    If CorrectForPrecession Then
'        Dim dRA As Double, dDec As Double
'        PrecessDelta rightAscension, declination, dRA, dDec
'        rightAscension = rightAscension - dRA
'        declination = declination - dDec
'    End If
    
    Select Case m_ScopeType
    Case E_NEXSTAR_GPS
        On Error GoTo ERR_SLEW        ' ignore any error
        If m_Version >= Version_16 Then
'            buf = "r"       ' high precision 24 bit slew
            cmd = NS_SET_RADEC_HP
            hp = True
        Else
'            buf = "R"       ' 16 bit slew
            cmd = NS_SET_RADEC_LP
            hp = False
        End If
        
'        buf = buf & DegToHex(g_dTargetRA * 15#, hp) & "," & DegToHex(g_dTargetDec, hp)
        '
        ' Send the command
        '
        ScopeCommandBinary ToByte(cmd, DegToHexB(RightAscension * 15#, hp), NS_COMMA, DegToHexB(Declination, hp))
        
        m_eSlewing = eSlewing               ' Init Slewing state
        m_bAbortSlew = False                ' Set off abort slew
        m_SlewStart = Timer
    Case E_NEXSTAR58
        ' this is actually a synchronous slew, we don't return until it is completed
        On Error GoTo ERR_SLEW        ' ignore any error
        On Error GoTo 0     ' we want to see the errors for testing
        ScopeCommandBinary ToByte(NS_SET_RADEC_LP, DegToByteB(RightAscension * 15#), DegToByteB(Declination))
        m_eSlewing = eSlewing
        m_SlewStart = Timer
        
    Case E_ULTIMA
        On Error GoTo ERR_SLEW        ' ignore any error
        ' Send the command
'        ScopeCommandString "R" & DegToHex(g_dTargetRA * 15#) & "," & DegToHex(g_dTargetDec) & vbCr
        ScopeCommandBinary ToByte(NS_SET_RADEC_LP, DegToHexB(RightAscension * 15#), NS_COMMA, DegToHexB(Declination), NS_CR)
        m_eSlewing = eSlewing
        m_bAbortSlew = False
        m_SlewStart = Timer
    Case E_GT
        On Error GoTo ERR_SLEW        ' ignore any error
'        g_serial.Transmit "R" & DegToByte(g_dTargetRA * 15#) & Chr(0) & DegToByte(g_dTargetDec) & Chr(0)
        g_serial.TransmitBinary ToByte(NS_SET_RADEC_LP, DegToByteB(RightAscension * 15#), 0, DegToByteB(Declination), 0)
        m_eSlewing = eSlewing
        m_bAbortSlew = False
        m_SlewStart = Timer
    Case Else
        RaiseError SCODE_NOT_IMPLEMENTED, _
                  ERR_SOURCE, _
                  "SlewAsync " & MSG_NOT_IMPLEMENTED
    End Select
    Trace "SlewAsync " & RightAscension & "," & Declination
Exit Sub

ERR_SLEW:
    m_eSlewing = eNoSlew
    m_bActive = False
End Sub

Public Sub SlewAltAz(ByVal Azimuth As Double, ByVal Altitude As Double)
'    Dim aH As Integer, aL As Integer, zH As Integer, zL As Integer
    Dim Alt As Double, Az As Double, ha As Double, dec As Double
'    Dim buf As String
'    Dim hp As Boolean
'    Dim cmd As Byte
    
    If g_IsGuiding Then g_PulseGuide.Halt

    CheckSlewAllowed True
    
    ' The Alt Az slew commands seem to be flaky as follows:
    ' CGE  "B" command, not "b"
    ' NXS  will not slew to altitudes less than zero, regardless of the slew limit.
    ' so convert the Alt and Az to Ra and Dec, then do a RaDec slew.
    Alt = degrad(Altitude)
    Az = degrad(Azimuth)
    aa_hadec degrad(g_dLatitude), Alt, Az, ha, dec
    ha = radhr(ha)
    dec = raddeg(dec)
    
    ha = now_lst(degrad(g_dLongitude)) - ha
    
    On Error GoTo errAZslew
    SlewAsync ha, dec, True
    
    Trace "SlewAltAz " & Az & ", " & Alt
Exit Sub

errAZslew:
    m_eSlewing = eNoSlew
    m_bActive = False
End Sub

' gets and sets the MeridianMode
Public Function GetMeridianMode() As Long
    Dim buf As String
    If m_Version > Version_414 And m_Version < Version_GT_LO Then
'        buf = ScopeCommandString("c0001")
        GetMeridianMode = GetConfigItem(MERIDIAN_MODE)
    Else
        RaiseError SCODE_NOT_IMPLEMENTED, _
                  ERR_SOURCE, _
                  "GetMeridianMode " & MSG_NOT_IMPLEMENTED
    End If
End Function

Public Sub SetMeridianMode(mode As Long)
    Dim buf As String, sbuf As String
    If m_Version > Version_412 And m_Version < Version_GT_LO Then
'        sbuf = "C0001,0000000" & Hex$(mode)
'        buf = ScopeCommandString(sbuf)
        SetConfigItem MERIDIAN_MODE, mode
    Else
        RaiseError SCODE_NOT_IMPLEMENTED, _
                  ERR_SOURCE, _
                  "SetMeridianMode " & MSG_NOT_IMPLEMENTED
    End If
End Sub

' checks that a guiding rate commanded is in range
Private Function RateIsInRange(Rate As Double, Rates As AxisRates) As Boolean
    Dim r As Rate
    
    For Each r In Rates
        If Abs(Rate) <= r.Maximum And Abs(Rate) >= r.Minimum Then
            RateIsInRange = True
            Exit Function
        End If
    Next
    RateIsInRange = False
End Function

Private Sub GetScopeParameters()
    Dim buf As String, strVersion As String
    
    ' read the new parameters from the registry
    buf = g_Profile.GetValue(g_DriverID, SITE_LATITUDE)
    If Len(buf) > 0 Then g_dLatitude = val(Trim$(buf))
    buf = g_Profile.GetValue(g_DriverID, SITE_LONGITUDE)
    If Len(buf) > 0 Then g_dLongitude = val(Trim$(buf))
    buf = g_Profile.GetValue(g_DriverID, SITE_ELEVATION)
    If Len(buf) > 0 Then g_dElevation = val(Trim$(buf))
    buf = g_Profile.GetValue(g_DriverID, FOCAL_LENGTH)
    If Len(buf) > 0 Then g_dFocalLength = val(Trim$(buf)) / 1000#  ' convert from mm to metres
    buf = g_Profile.GetValue(g_DriverID, APERTURE)
    If Len(buf) > 0 Then g_dAperture = val(Trim$(buf)) / 1000#  ' convert from mm to metres
    buf = g_Profile.GetValue(g_DriverID, AREA)
    If Len(buf) > 0 Then
        g_dApertureArea = val(Trim$(buf)) / 1000000#   ' convert from mm^2 to m^2
    Else
        g_dApertureArea = (g_dAperture / 2) ^ 2 * PI
    End If
        
    buf = g_Profile.GetValue(g_DriverID, HAS_GPS)
    If Len(buf) > 0 Then m_HasGPS = CBool(Trim$(buf))
    
    ' Non ASCOM property to allow slews regardless of the state of tracking
    buf = g_Profile.GetValue(g_DriverID, "AllowAllSlews")
    If Len(buf) > 0 Then m_AllowAllSlews = CBool(Trim$(buf))
    
    ' non ASCOM property to allow slews when not aligned - useful for off-line testing of SkyAlign
    buf = g_Profile.GetValue(g_DriverID, "SlewIfNotAligned")
    If Len(buf) > 0 Then m_SlewIfNotAligned = CBool(Trim$(buf))
    
    ' non ASCOM property to enable PEC
'    buf = g_Profile.GetValue(g_DriverID, ENABLE_PEC)
'    If Len(buf) > 0 Then g_EnablePEC = CBool(buf)
    g_EnablePEC = False
    
    #If BETA_MODE Then
    buf = g_Profile.GetValue(g_DriverID, "AltAzRaDec")
    If Len(buf) > 0 Then m_AltAzRaDec = CBool(Trim$(buf))
    #End If
    
    buf = g_Profile.GetValue(g_DriverID, "SerTraceFile")
    If buf <> "" Then                       ' Set up tracking
        m_fTracing = True
        Set FSO = New Scripting.FileSystemObject
        Set m_TraceFile = FSO.CreateTextFile(Trim$(buf), True)
    Else
        m_fTracing = False
    End If
    
    ' check for the guiding controls
    buf = g_Profile.GetValue(g_DriverID, "AsyncGuiding")
    If buf <> "" Then
        asyncGuiding = CBool(Trim$(buf))
    Else
        asyncGuiding = False
    End If
    buf = g_Profile.GetValue(g_DriverID, "IsGuidingReturnsFalse")
    If buf <> "" Then
        isGuidingReturnsFalse = CBool(Trim$(buf))
    Else
        isGuidingReturnsFalse = False
    End If
    
    Trace ("Serial trace file:")

    Dim IsGem As Boolean
    buf = g_Profile.GetValue(g_DriverID, IS_GEM)
    If Len(buf) > 0 Then
        IsGem = CBool(Trim$(buf))
    End If
    ' get the track mode
    buf = g_Profile.GetValue(g_DriverID, TRACK_MODE)
    If buf = "" Then buf = "1"              ' default is Alt/Az
    g_TrackMode = val(Trim$(buf))
    Select Case g_TrackMode
    Case E_TRACK_ALTAZ
        g_eAlignMode = algAltAz
    Case E_TRACK_EQ_N, E_TRACK_EQ_S
        g_eAlignMode = algPolar
        If IsGem Then g_eAlignMode = algGermanPolar
    Case Else
        g_eAlignMode = algAltAz
    End Select
    
    Select Case m_ScopeType
    Case E_NEXSTAR_GPS
        m_Coord_Upd_Interval = SHORT_UPD_INTERVAL
        
        ' read the park position
        buf = g_Profile.GetValue(g_DriverID, PARK_ALTITUDE)
        If buf = "" Then buf = "0"
        m_ParkAlt = val(Trim$(buf))
        buf = g_Profile.GetValue(g_DriverID, PARK_AZIMUTH)
        If buf = "" Then buf = "0"
        m_ParkAz = val(Trim$(buf))
        
        buf = g_Profile.GetValue(g_DriverID, IS_PARKED)
        If Len(buf) > 0 Then g_AtPark = CBool(Trim$(buf))
        
        ' any error will cause the connection to fail
        On Error GoTo Err
        
        ' use the V command to get the HC version
        m_Version = 0
        On Error Resume Next        ' ignore errors reading the version
        buf = ""
        Dim byt() As Byte
'        buf = ScopeCommandString("V")
        byt = ScopeCommandBinary(ToByte(NS_GET_VERSION))
        
        m_bActive = False
        ' The 80 GT returns "e<0>#"
        ' Also seen "d<7>#" with a 114 GT
        ' GT versions are 100.7, 101.0 and 102.x
        ' new GT versions will be 104.x
        
        
        'The NS11 returns "<1><6>#" for version 1.6
        ' the beta versions have high version numbers
        If Length(byt) >= 2 Then
            m_Version = byt(0) * &H100 + byt(1)
            strVersion = Int(m_Version / &H100) & "." & Int(m_Version Mod &H100)
            ' force the GT versions to be 0
            If m_Version >= Version_GT_LO And m_Version <= Version_GT_HI Then m_Version = 0
        End If
        Trace "Version " & strVersion
        
        ' test code for trying different versions
'        m_Version = 0               ' GT
'        m_Version = Version_22      ' GPS or i
'        m_Version = Version_30      ' CGE
'        m_Version = Version_Beta + 64   ' beta version 40.64
        
        If m_Version >= Version_16 Then
            ' get guide rates from the HC
            g_GuideRateRa = GetGuideRate(ID_AZM)
            g_GuideRateDec = GetGuideRate(ID_ALT)
            ' set the guide rates collection (no longer public)
            Set g_GuideRates = New AxisRates
            g_GuideRates.Add SIDEREAL_RATE_DEG_SEC, 0#
            ' set the private HC rates
            ' set 9 ranges, 0.5 sidereal to 3 degrees per second
            Set g_HCRates = New AxisRates
            g_HCRates.Add 0.5 * SIDEREAL_RATE_DEG_SEC, 0.5 * SIDEREAL_RATE_DEG_SEC
            g_HCRates.Add SIDEREAL_RATE_DEG_SEC, SIDEREAL_RATE_DEG_SEC
            g_HCRates.Add 4 * SIDEREAL_RATE_DEG_SEC, 4 * SIDEREAL_RATE_DEG_SEC
            g_HCRates.Add 8 * SIDEREAL_RATE_DEG_SEC, 8 * SIDEREAL_RATE_DEG_SEC
            g_HCRates.Add 16 * SIDEREAL_RATE_DEG_SEC, 16 * SIDEREAL_RATE_DEG_SEC
            g_HCRates.Add 64 * SIDEREAL_RATE_DEG_SEC, 64 * SIDEREAL_RATE_DEG_SEC
            g_HCRates.Add 0.5, 0.5
            g_HCRates.Add 2#, 2#
            g_HCRates.Add 3#, 3#
            
            ' set the axis rates collection
            ' this is one range, the GPS scopes are 0 to 3.0 degrees per second
            ' the i series scopes are said to slew up to 6 deg per second.
            ' we don't have a way to tell so use the lowest common denominator for now.
            ' 5.0.2 increase the rate to 4.5 deg/sec
            Set g_AxisRates = New AxisRates
            g_AxisRates.Add MAX_MOVE_RATE_DEG_SEC, 0#
            
            Select Case g_eAlignMode
            Case algPolar, algGermanPolar
                Set g_DriveRates = New TrackingRates
                g_DriveRates.Add driveSidereal
                g_DriveRates.Add driveLunar
                g_DriveRates.Add driveSolar
            End Select
            
        End If
        '
        ' The J command used by NexStar to check if alignment complete
        
        ' 3.2.6 we allow connection while not aligned
        ' 4.2.3 move to CheckAligned
'        buf = ""
'        buf = ScopeCommandString("J")
'        m_bActive = False
'        If AscB((Mid(buf, 1, 1))) = 1 Then
'            m_eSlewing = eSlewing  ' force a check the first time Slewing is called
'            ' assume we are tracking
'            g_Tracking = True
'        Else
'            g_Tracking = False
'        End If
        ' we could determine if we are tracking by checking if Alt, Az or Ra are changing
        
        'sort out the version dependent stuff
        Select Case m_Version
        Case Is < Version_16
            g_ScopeTypeStr = "NexStar GT " & strVersion
            m_Extended = False
        Case Version_16 To Version_22 - 1
            ' assume GPS, get the latitude and longitude from the GPS module
            g_ScopeTypeStr = "NexStar GPS " & strVersion
            m_Extended = False
        Case Version_22
            ' GPS and iSeries
            g_ScopeTypeStr = "NexStar " & GetScopeType & " " & strVersion
            m_Extended = False
        Case Version_22 + 1 To Version_30 - 1
            ' NexStar iSeries SE
            g_ScopeTypeStr = GetScopeType & " " & strVersion
            m_Extended = True
        Case Version_30 To Version_34 - 1
            ' CGE scope
            g_ScopeTypeStr = "CGE " & strVersion
            m_Extended = False
        Case Version_34
            ' asc scope
            g_ScopeTypeStr = "ASC " & strVersion
            m_Extended = False
        Case Version_34 + 1 To Version_40 - 1
            ' nxs scope
            g_ScopeTypeStr = GetScopeType & " " & strVersion
            m_Extended = True
        Case Version_40 To Version_Beta - 1, Is > Version_GT_HI
            'SLT and CPC scopes, maybe C20 as well
            ' new GT scopes
            g_ScopeTypeStr = GetScopeType & " " & strVersion
            m_Extended = True
        Case Version_Beta To Version_GT_HI
            ' beta versions
            g_ScopeTypeStr = "Beta " & GetScopeType & " " & strVersion
            m_Extended = True
        Case Else
            g_ScopeTypeStr = "Unrecognised " & strVersion
            m_Extended = False
        End Select
        Trace "Scope Type " & g_ScopeTypeStr
        ' Check if the scope is aligned
        CheckAligned
        
        ' check the MC version if possible, all version 4 HCs
        If m_Version > Version_40 And m_Version < Version_GT_LO Then
            GetMotorVersions
        End If
        
        ' Read tracking state from the profile
        buf = g_Profile.GetValue(g_DriverID, IS_TRACKING)
        If Len(buf) > 0 Then g_Tracking = val(Trim$(buf))
        
        ' read the guide backlash parameters
        buf = g_Profile.GetValue(g_DriverID, "GuideBacklashRate")
        If Len(buf) > 0 Then g_PulseGuide.GuideBacklashRate = val(Trim$(buf))
        buf = g_Profile.GetValue(g_DriverID, "GuideBacklashTime")
        If Len(buf) > 0 Then g_PulseGuide.GuideBacklashTime = val(Trim$(buf))
        
        ' do we use the scope time for unpark and Find Home?
        useScopeTime = False        ' default, use the PC
        buf = g_Profile.GetValue(g_DriverID, "UseScopeTime")
        If Len(buf) > 0 Then useScopeTime = val(Trim$(buf))
        
        ' check the GPS actually exists
        If m_HasGPS Then
            g_serial.ReceiveTimeout = 5         'pwgs: Increase Timeout to 5 seconds if a GPS module is connected
            m_HasGPS = DeviceExists(ID_GPS)     'pwgs: Moved these lines outside the g_Extended section
        End If
        If m_HasGPS Then GetAUXparameters   'pwgs: Added test to only call if the GPS is actually there
        m_eSlewing = eInitSlew      ' force a check the first time slewing is called
    Case E_ULTIMA
        m_Coord_Upd_Interval = COORD_UPD_INTERVAL
        g_serial.ReceiveTimeout = 5
        m_eSlewing = eInitSlew      ' force a check the first time Slewing is called
        m_Aligned = True
        g_Tracking = True
    Case Else
        g_Tracking = True
        m_Aligned = True
    End Select
    Trace "Driver: " & g_DriverID
Exit Sub

Err:
    ' any error will cause connection to fail
    g_serial.ClearBuffers
    g_serial.Connected = False
    g_bConnected = False
    m_ScopeType = E_UNKNOWN
    RaiseError SCODE_NO_CELESTRON, ERR_SOURCE, MSG_NO_CELESTRON

End Sub

Private Sub GetMotorVersions()
' reads the motor versions
    Dim buf() As Byte
    
    On Error GoTo Err       ' silently fail
    buf = AUXCommand(ID_ALT, GET_VER, 2)
    m_AltMcVer = buf(0) * 256 + buf(1)
    buf = AUXCommand(ID_AZM, GET_VER, 2)
    m_AzmMcVer = buf(0) * 256 + buf(1)
Err:
End Sub

Private Function CanAuxGuide() As Boolean
    CanAuxGuide = False     ' assume it can't
    Select Case m_Scope_ID
    Case E_SCOPE_ID.SCOPE_CGE2
        If m_AzmMcVer >= MIN_GUIDE_VER_CGE2 And m_AltMcVer >= MIN_GUIDE_VER_CGE2 Then
            CanAuxGuide = True
        End If
    Case E_SCOPE_ID.SCOPE_EQ6
        If m_AzmMcVer >= MIN_GUIDE_VER_ASC And m_AltMcVer >= MIN_GUIDE_VER_ASC Then
            CanAuxGuide = True
        End If
    Case E_SCOPE_ID.SCOPE_AVX
        CanAuxGuide = True
    End Select
End Function

Private Function AuxGuide(Direction As GuideDirections, Duration As Long) As Boolean
' use the Mc Guide command.
' Returns True if successful, False if not

    AuxGuide = False      ' assume failure
    On Error GoTo Err       ' any error will return False
    If Not CanAuxGuide Then
        Exit Function
    End If
    
    ' check the duration is in range
    If Duration > 2550 Then
        Exit Function
    End If
    Debug.Print "AuxGuide"
    ' we can do it!
    ' determine the motor ID and the guide rate
    ' adjust the guide rate downwards to allow for the
    Dim id As E_DEVICE_ID
    Dim counts As Integer
    counts = Int((Duration + 5) / 10)
    Dim Rate As Integer
    Rate = Duration * 10 / counts    ' percent rate allowing for duration to count conversion
    
'    Dim Rate As Integer
    Select Case Direction
    Case GuideDirections.guideEast
        id = ID_AZM
        Rate = -Rate * g_GuideRateRa / SIDEREAL_RATE_DEG_SEC
    Case GuideDirections.guideWest
        id = ID_AZM
        Rate = Rate * g_GuideRateRa / SIDEREAL_RATE_DEG_SEC
    Case GuideDirections.guideNorth
        id = ID_ALT
        Rate = Rate * g_GuideRateDec / SIDEREAL_RATE_DEG_SEC
    Case GuideDirections.guideSouth
        id = ID_ALT
        Rate = -Rate * g_GuideRateDec / SIDEREAL_RATE_DEG_SEC
    End Select
    ' send the command
    Trace "Mc Guide, MC " & id & ", rate " & Rate & ", counts " & counts
    AUXCommand id, MTR_AUX_GUIDE, 0, Rate, counts
    AuxGuide = True
    If asyncGuiding Then
        Sleep Duration
    End If
Err:
End Function

Private Function ByteToDegB(buf() As Byte, first As Integer, last As Integer) As Double
' converts a two or three byte array to degrees
    Dim h As Long, m As Long, l As Long, d As Double
    
    h = buf(first)
    m = buf(first + 1)
    If last > first + 1 Then
        l = buf(last)
        d = CDbl(h * &H10000 + m * &H100 + l) * CDbl(360# / &H1000000)
    Else
        d = CDbl(h * &H100 + m) * CDbl(360# / &H10000)
    End If
    If d > 360# Then
       d = d - 360#
    End If
    If d < -180# Then
        d = d + 360#
    End If
    ByteToDegB = d
End Function

Private Function DegToByteB(deg As Double) As Byte()
' convert the degrees in deg to A 2 byte array
    Dim d As Long, ba(0 To 1) As Byte
    
    If deg >= 0 Then
        d = deg * (&H10000 / 360#)
    Else
        d = (360 + deg) * (&H10000 / 360#)
    End If
    d = d Mod &H10000
    ba(0) = Int(d / &H100)
    ba(1) = CByte(d Mod &H100)
    DegToByteB = ba
End Function

Private Function DegToHexB(deg As Double, Optional HiPrecision As Boolean = False) As Byte()
' returns a hex byte array that is the degrees encoded in the Celestron way
' If HiPrecision is false then returns a 4 byte array, otherwise an 8 byte array.
    Dim d As Currency
    
    If deg >= 0 Then
        d = deg * CCur(&H1000000 / 360#)
    Else
        d = (360 + deg) * CCur(&H1000000 / 360#)
    End If
    
    If HiPrecision Then
        DegToHexB = HexB(d, 6)
    Else
        d = d / &H100
        DegToHexB = HexB(d, 4)
    End If
End Function

Private Function HexB(ByVal d As Currency, nb As Integer) As Byte()
    Dim ba() As Byte, i As Integer, X As Integer
    
    Select Case nb
    Case 8
        ReDim ba(0 To 7) As Byte
        i = 7
    Case 6
        ReDim ba(0 To 7) As Byte
        i = 5
        ba(7) = NS_0
        ba(6) = NS_0
    Case 4
        ReDim ba(0 To 3) As Byte
        i = 3
    End Select
    Do
        X = d And &HF
        Select Case X
        Case 0 To 9
            ba(i) = X + NS_0        'convert to ascii "0" - "9"
        Case 10 To 15
            ba(i) = X - 10 + NS_A   ' or "A" to "F"
        End Select
        d = Fix(d / &H10)   ' shift right, fix is required to keep the sums correct
        i = i - 1       ' from LS end
    Loop Until i < 0
    HexB = ba
End Function

Private Function ByteHex(hb() As Byte, first As Integer, last As Integer) As Long
' converts a hex byte string to a long
    Dim l As Currency
    Dim i As Integer, j As Integer
    
    For i = first To last
        j = hb(i)
        Select Case j
        Case NS_0 To NS_9
            j = j - NS_0
        Case NS_A To NS_F
            j = j - &H40 + 9
        End Select
        l = l * &H10 + j
    Next
    ByteHex = l
End Function

Private Function DeviceExists(Device As E_DEVICE_ID) As Boolean
' Checks for the presence of the Device by attempting to read its version
    Dim buf() As Byte
    DeviceExists = False
    On Error GoTo Err
    ' read the version and expect 2 bytes + "#"
    buf = AUXCommand(Device, GET_VER, 2)
    DeviceExists = (UBound(buf) = 2)
Err:
End Function

Private Function HexToDegB(hb() As Byte, first As Integer, last As Integer) As Double
' converts a Celestron hex byte array to degrees
' if 4 bytes then 16 bits,
' if 6 or 8 bytes then 32 bits
    Dim h As Currency, l As Currency, d As Double
    Dim i As Integer, j As Integer
    
    For i = first To last
        j = hb(i)
        Select Case j
        Case &H30 To &H39
            j = j - &H30
        Case &H41 To &H46
            j = j - &H40 + 9
        End Select
        l = l * &H10 + j
    Next
    
    Select Case last - first + 1
    Case 4
        d = CDbl(l) * CDbl(360# / &H10000)
    Case 6
        d = CDbl(l) * CDbl(360# / &H1000000)
    Case 8
        d = CDbl(l) * CDbl(360# / CCur(&H1000000 * &H100))
    End Select
    If d > 360# Then
       d = d - 360#
    End If
    If d < -180# Then
        d = d + 360#
    End If
    HexToDegB = d
End Function

Private Function CheckRate(Rate As Double, Rates As AxisRates) As Integer
' returns the rate (0 to 9) appropriate to the rate in deg per seconds supplied.
    Dim i As Integer
    
    ' check for zero, allowing for a bit of rounding errors
    If Abs(Rate) < Rates.Item(1).Maximum / 2 Then
        CheckRate = 0
        Exit Function
    End If
    ' check for other rates
    For i = 1 To Rates.Count
        If Abs(Rate) <= Rates.Item(i).Maximum Then
            CheckRate = i
            Exit Function
        End If
    Next
    
    ' if we get here the rate is not supported, raise an error
    RaiseError SCODE_PROP_RANGE_ERROR, ERR_SOURCE, _
            "Function CheckRate() " & MSG_PROP_RANGE_ERROR
End Function

Private Function EncodeDegreesB(ByVal deg As Double) As Byte()
    ' encodes the position in degrees to the Celestron string for the "W" command
    Dim dg As Double, lg As Long
    Dim d As Integer, m As Integer, s As Integer, sn As Integer
    Dim ba(0 To 3) As Byte
    
    ' force deg to the range -180 to 180
    While deg < -180
        deg = deg + 360
    Wend
    While deg > 180
        deg = deg - 360
    Wend
    If deg < 0 Then
        dg = -deg
        sn = 1
    Else
        dg = deg
        sn = 0
    End If
    ' 4.2.14 really avoid rounding errors
    lg = dg * 3600#     ' convert to seconds
    s = lg Mod 60
    lg = (lg - s) / 60  ' remove seconds and convert to minutes
    m = lg Mod 60
    d = (lg - m) / 60   ' remove seconds and convert to degrees
    
'    d = CInt(dg)
'    m = CInt((dg - d) * 60#)
'    s = CInt((((dg - d) * 60#) - m) * 60#)      ' 4.2.5 avoid rounding errors
    ba(0) = d
    ba(1) = m
    ba(2) = s
    ba(3) = sn
    EncodeDegreesB = ba
End Function

Private Function DecodeDegreesB(bdeg() As Byte, os As Integer) As Double
    Dim d As Double, m As Integer, s As Integer, sn As Integer
    
    d = bdeg(os + 0)
    m = bdeg(os + 1)
    s = bdeg(os + 2)
    sn = bdeg(os + 3)
    d = d + m / 60# + s / 3600#
    If sn = 1 Then d = -d
    DecodeDegreesB = d
End Function

Private Sub GetAUXparameters()
    ' read various parameters from the GPS scopes
    Dim buf() As Byte

    If m_HasGPS Then
        On Error GoTo Err
        buf = AUXCommand(ID_GPS, GPS_LINKED, 1)
        If buf(0) = 1 Then
            buf = AUXCommand(ID_GPS, GPS_GET_LAT, 3)
            g_dLatitude = ByteToDegB(buf, 0, 2)
            buf = AUXCommand(ID_GPS, GPS_GET_LONG, 3)
            g_dLongitude = ByteToDegB(buf, 0, 2)
            If g_dLongitude > 180 Then g_dLongitude = g_dLongitude - 360#
            g_Profile.WriteValue g_DriverID, SITE_LATITUDE, Str(g_dLatitude)
            g_Profile.WriteValue g_DriverID, SITE_LONGITUDE, Str(g_dLongitude)
        End If
    End If
Err:
End Sub

'Private Function Initialize() As Boolean
'    ' sends the initialization command for the NexStar 5 & 8
'    'Returns true if successful
'    Dim buf() As Byte
'
'    ' we should not send commands while slewing as we may miss the '@' character
'    If m_eSlewing = eSlewing Then
'        Initialize = False
'    Else
'        g_serial.ClearBuffers
'        On Error GoTo InitFail
'        g_serial.TransmitBinary ToByte(NS_QUESTION)
'        buf = g_serial.ReceiveTerminatedBinary(ToByte(NS_TERMINATOR))
'        Initialize = (buf(0) = NS_TERMINATOR)
'    End If
'Exit Function

'InitFail:
'    Initialize = False
'End Function


Public Sub SetTrackingRate()
' sets the tracking rate to the current rate with no checks
    Dim msgID As E_MESSAGE_ID
    Select Case g_TrackMode
    Case E_TRACK_EQ_N
        msgID = MC_SET_POS_GUIDERATE
    Case E_TRACK_EQ_S
        msgID = MC_SET_NEG_GUIDERATE
    End Select
    Dim tr As E_TRACK_RATES
    Select Case g_DriveRate
    Case driveSidereal
        tr = RATE_SIDEREAL
    Case driveSolar
        tr = RATE_SOLAR
    Case driveLunar
        tr = RATE_LUNAR
    End Select
    On Error Resume Next
    AUXCommand ID_AZM, msgID, 0, Int(tr / &H100), tr Mod &H100
    m_RaRateSet = False
End Sub

Private Function GetScopeType() As String
' use the 'm' command to read the scope type
' update the IsGem parameter
    Dim buf() As Byte, OldGemType As Boolean
    
    OldGemType = m_IsGem
'    buf = ScopeCommandString("m")
    buf = ScopeCommandBinary(ToByte(NS_GET_MODEL))
    m_Scope_ID = buf(0)
    Select Case m_Scope_ID
    Case E_SCOPE_ID.SCOPE_GPS
        GetScopeType = "GPS"
        m_IsGem = False
        m_HasPEC = True
    Case SCOPE_GPS_SA
        GetScopeType = "GPS SA"
        m_IsGem = False
        m_HasPEC = True
    Case SCOPE_ISERIES
        GetScopeType = "iSeries"
        m_IsGem = False
    Case SCOPE_ISERIES_SE
        GetScopeType = "iSeries SE"
        m_IsGem = False
    Case SCOPE_CGE
        GetScopeType = "CGE"
        m_IsGem = True
        m_HasPEC = True
    Case SCOPE_ASC
        GetScopeType = "ASC"
        m_IsGem = True
    Case SCOPE_SLT
        GetScopeType = "SLT"
        m_IsGem = False
    Case SCOPE_C20
        GetScopeType = "C20"
        m_IsGem = True
        m_HasPEC = True
    Case SCOPE_CPC
        GetScopeType = "CPC"
        m_IsGem = False
        m_HasPEC = True
    Case SCOPE_GT
        GetScopeType = "GT"
        m_IsGem = False
    Case SCOPE_4_5_SE
        GetScopeType = "4/5 SE"
        m_IsGem = False
    Case SCOPE_6_8_SE
        GetScopeType = "6/8 SE"
        m_IsGem = False
    Case SCOPE_CGE2
        GetScopeType = "CGE Pro"
        m_IsGem = True
        m_HasPEC = True
    Case SCOPE_EQ6
        GetScopeType = "CGEM"
        m_IsGem = True
        m_HasPEC = True
    Case SCOPE_AVX
        GetScopeType = "AVX"
        m_IsGem = True
        m_HasPEC = True
    Case Else
        GetScopeType = "Type " & Asc(buf)
    End Select
    If m_IsGem <> OldGemType Then
        g_Profile.WriteValue g_DriverID, IS_GEM, CInt(m_IsGem)
    End If
End Function

Private Function TrackMode() As E_TRACK_MODE
' this handles the track mode allowing for the
' nonsense with the modes being different for the early CGE and AS scopes
    Dim tm As E_TRACK_MODE
    tm = g_TrackMode
    If (m_Version >= Version_30 And m_Version <= Version_34) And _
        (g_TrackMode = E_TRACK_EQ_N Or g_TrackMode = E_TRACK_EQ_S) _
    Then
        ' these are the CGE and AS scopes, reduce the mode by 1
        tm = tm - 1
    End If
    TrackMode = tm
End Function

Private Function CheckAligned() As Boolean
' if the scope is not aligned then it sends a command to check it
' if the scope has become aligned and it has the extended command set then
' it checks the tracking mode and sets the state of Tracking and the track mode
    Dim byt() As Byte, buf As String

    If Not m_Aligned Then
        ' ask the scope if it is aligned
'        buf = ScopeCommandString("J")
        byt = ScopeCommandBinary(ToByte(NS_IS_ALIGNED))
        If byt(0) = 1 Then
            ' the scope is now aligned
            m_Aligned = True
            If m_Extended Then
                ' check the track mode
'                buf = ScopeCommandString("t")
                byt = ScopeCommandBinary(ToByte(NS_GET_TRACKING))
                Dim tm As E_TRACK_MODE
                tm = byt(0)
                
                ' set the tracking and align mode states
                Select Case tm
                Case E_TRACK_OFF
                    g_Tracking = False
                    ' don't set the track mode or align mode
                Case E_TRACK_ALTAZ
                    g_Tracking = True
                    g_TrackMode = tm
                    g_eAlignMode = algAltAz
                    g_Profile.WriteValue g_DriverID, TRACK_MODE, Str(g_TrackMode)
                Case E_TRACK_EQ_N, E_TRACK_EQ_S
                    g_Tracking = True
                    g_TrackMode = tm
                    g_eAlignMode = algPolar
                    Dim IsGem As Boolean
                    buf = g_Profile.GetValue(g_DriverID, IS_GEM)
                    If Len(buf) > 0 Then IsGem = CBool(Trim$(buf))
                    If IsGem Then g_eAlignMode = algGermanPolar
                    g_Profile.WriteValue g_DriverID, TRACK_MODE, Str(g_TrackMode)
                    EnablePEC
                End Select
                Trace "Tracking " & g_Tracking & ", Track mode " & g_TrackMode
                ' 5.0.20 set AtPark to false if it's tracking
                If g_AtPark And g_Tracking Then
                    g_AtPark = False
                    g_Profile.WriteValue g_DriverID, IS_PARKED, CInt(g_AtPark)
                End If
            Else
                ' inherit track and align modes from the registry values set by the user
                ' assume we are tracking
                g_Tracking = True
            End If
        End If
    End If
    CheckAligned = IIf(m_SlewIfNotAligned, True, m_Aligned)
End Function

'Public Sub Wait()
'    Exit Sub
'    ' wait for any send/receive process to finish before sending another
'    Dim Count As Integer
'    Count = 0
'    While m_bActive
'        Sleep 100
'        DoEvents
'        Count = Count + 1
'        If Count > 100 Then
'            Debug.Print "Wait Timeout"
'            m_bActive = False   ' 10 second timeout
'        End If
'        DoEvents
'    Wend
'End Sub

Public Sub RaiseError(Number As Long, Optional Source, Optional Description)
    Trace "Error " & Number & " Src:" & Source & " - " & Description
    Err.Raise Number, Source, Description
End Sub

Private Function ToByte(ParamArray data() As Variant) As Byte()
' takes a list of byte values and returns them as a byte array
    Dim Command() As Byte, bDat As Byte, i As Integer, j As Integer
    Dim vDat As Variant

    i = LBound(data)
    ReDim Command(0 To 255)
    For Each vDat In data
        If IsArray(vDat) Then
            For j = LBound(vDat) To UBound(vDat)
                Command(i) = vDat(j)
                i = i + 1
            Next
        Else
            Command(i) = CByte(vDat)
            i = i + 1
        End If
    Next
    ReDim Preserve Command(0 To i - 1)
    ToByte = Command
End Function

Private Function InByt(buf() As Byte, byt As Byte, Optional start As Integer = 0) As Integer
' returns the position of byt in buf starting at start

    Dim i As Integer
    For i = start To UBound(buf)
        If buf(i) = byt Then
            InByt = i
            Exit Function
        End If
    Next
    InByt = -1      ' error, byte not found
End Function

Private Sub SetConfigItem(Item As E_CONFIG_ITEM, mode As Long)
        Dim byt() As Byte
        byt = ScopeCommandBinary(ToByte(NS_SET_CONFIG_ITEM, HexB(Item, 4), NS_COMMA, HexB(mode, 8)))
End Sub

Private Function GetConfigItem(Item As E_CONFIG_ITEM) As Long
        Dim byt() As Byte
        byt = ScopeCommandBinary(ToByte(NS_GET_CONFIG_ITEM, HexB(Item, 4)))
        GetConfigItem = ByteHex(byt, 0, 7)
End Function

'Private Sub PrecessDelta(RA As Double, dec As Double, dRra As Double, dDec As Double)
'    ' precession correction from http://www.cv.nrao.edu/~rfisher/Ephemerides/earth_rot.html
'    ' see http://www.stargazing.net/kepler/b1950.html for a method that may be better near the poles.
'    Dim y As Double
'
'    y = (now_mjd - mjd_2000) / 365.25    ' time in years from 2000.0
'
'    dRra = ((3.075 + 1.3362 * Sin(degrad(RA * 15#)) * Tan(degrad(dec))) * y) / (3600#)
'    dDec = (20.043 * Cos(degrad(RA * 15#)) * y) / 3600#
'
'End Sub

' enable PEC if required
Private Sub EnablePEC()
    Dim buf() As Byte
    If g_EnablePEC Then
        ' must be equatorially mounted and CGE, CPC or GPS scope
        If g_eAlignMode <> algAltAz And m_HasPEC Then
            ' have we seen the index?
            ' Apparently there's no need to seek the index because
            ' the scope will find it if it needs to
'            buf = AUXCommand(ID_AZM, MC_AT_INDEX, 1, 0)
'            If buf(0) <> &HFF Then
'                ' no, go to it
'                AUXCommand ID_AZM, MC_SEEK_INDEX, 0, 0
'                Do
'                    g_util.WaitForMilliseconds 100
'                    ' wait until we have seen the index
'                    buf = AUXCommand(ID_AZM, MC_AT_INDEX, 1, 0)
'                Loop Until buf(0) = &HFF
'            End If
            On Error Resume Next
            AUXCommand ID_AZM, MC_PEC_PLAYBACK, 0, 1
        End If
    End If
End Sub

Private Sub SyncLock()
    ' wait for any send/receive process to finish before sending another
    Dim Count As Integer
    Count = 0
    While m_bActive
        Sleep 100
        DoEvents
        Count = Count + 1
        If Count > 100 Then
            Debug.Print "SyncLock Timeout"
            m_bActive = False   ' 10 second timeout
        End If
        DoEvents
    Wend
    m_bActive = True
End Sub

Private Function Length(buf() As Byte) As Integer
    Length = UBound(buf) - LBound(buf) + 1
End Function



' Send to NexRemote
' return reply
' if NR is not present return empty string
Private Function SendNR(ByVal Message As String) As String
    SendNR = ""
    Exit Function
    Dim COMproxy As Object
    On Local Error Resume Next
    Set COMproxy = GetObject(Class:="COMproxy.Application")
    If Not COMproxy Is Nothing Then
       If COMproxy.IsConnected Then
          SendNR = COMproxy.Send(Message)
       Else
          'Not Connected
       End If
       Set COMproxy = Nothing
    Else
       'Not Connected
    End If
End Function

' selects a menu item.
' returns false if it fails for any reason.
Private Function SelectMenuItem(menuItems As String) As Boolean
    Dim buf As String
    Dim items() As String
    items = Split(menuItems, "|")
    Dim i As Integer
    SelectMenuItem = False
    ' undo a few times to get back to the root
    For i = 0 To 3
        buf = SendNR("$X")
        Sleep 200
    Next
    ' Select Menu
    buf = SendNR("$3")
    If buf = "" Then Exit Function      ' no NR present
    For i = 0 To UBound(items)
        ' select the item
        buf = SendNR("!L2")
        While InStr(buf, items(i)) <> 1
            buf = SendNR("$6")    ' UP
            Sleep 200
            buf = SendNR("!L2")
        Wend
        buf = SendNR("$E")      ' selects the item.  This will do the action for the last item
        Sleep 200
    Next
    SelectMenuItem = True
End Function


' Home commands
' applicable to the GEM scopes with switches, the CGE and CGE Pro at present.
' and HC version 4.21 or better or NexRemote

' returns True if the scope can find the home position
Public Function ScopeCanFindHome() As Boolean
    ScopeCanFindHome = False
    If Not g_bConnected Then
        Exit Function
    End If
    Exit Function   ' never tested
'    If Not isUsingNexRemote Or m_Version < Version_421 Or m_Version >= Version_GT_LO Then
'        Exit Function
'    End If
'    Select Case m_Scope_ID
'    Case E_SCOPE_ID.SCOPE_CGE, E_SCOPE_ID.SCOPE_CGE2
'        ScopeCanFindHome = True
''    Case E_SCOPE_ID.SCOPE_ASC
''        ScopeCanFindHome = True
'    End Select
End Function

' Handles the FindHome command.
' This moves to the switch position and does a last align.
Public Sub ScopeFindHome()
    If Not ScopeCanFindHome Then
        ' raise an error
        RaiseError SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
                    "Method FindHome()" & MSG_NOT_IMPLEMENTED
    End If
    ' use NexRemote if pre version 4.21
    If Not LastAlign Then
        ' raise an error
        RaiseError SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
                    "Method FindHome()" & MSG_NOT_IMPLEMENTED
    End If
    Dim buf() As Byte
    On Error GoTo Err
    ' execute the switch home move via P command with 0x0B.
    buf = AUXCommand(ID_ALT, MC_SEEK_SWITCH, 0)
    buf = AUXCommand(ID_AZM, MC_SEEK_SWITCH, 0)
    ' Monitor switch home move completion via P command with 0x12.
    Dim atSwitches As Boolean
    Do
        Sleep LONG_WAIT
        buf = AUXCommand(ID_ALT, MC_AT_SWITCH, 1)
        atSwitches = (buf(0) = 1)
        buf = AUXCommand(ID_AZM, MC_AT_SWITCH, 1)
        atSwitches = atSwitches Or (buf(0) = 1)
        ' do we need a timeout?
    Loop Until atSwitches
    ' RESET the encoder counts via P command with 0x04.
    buf = AUXCommand(ID_ALT, MC_SET_AXIS, 0, &H40, 0, 0)
    buf = AUXCommand(ID_AZM, MC_SET_AXIS, 0, &H40, 0, 0)
    ' Set the time with I
    Dim utc As Date
    utc = GetUTC
    LetUTCDate utc
    ' Send rs232 serial command Y.  'Y' will execute "Last Align" to
    ' restore the rest of the alignment from EEPROM parameters.
    buf = ScopeCommandBinary(ToByte(NS_LAST_ALIGN))
    Sleep SHORT_WAIT
    ' tidy up
    m_Aligned = False   ' force an align check
Exit Sub

Err:
    RaiseError Err.Number, ERR_SOURCE, _
            "FindHome execution error: " & Err.Description
    m_Aligned = False   ' force an align check
End Sub

' gets the UTC from the GPS or RTC if required or the PC clock if it isn't available
Private Function GetUTC() As Date
    If useScopeTime Then
        ' get the time from the GPS or RTC if it's present
        Dim id As E_DEVICE_ID
        Dim buf() As Byte
        On Error GoTo Err
        If DeviceExists(ID_GPS) Then
            id = ID_GPS
            ' wait for the time to be valid
            buf = AUXCommand(id, GPS_TIME_VALID, 1)
            While buf(0) <> 1
                Sleep LONG_WAIT
                buf = AUXCommand(id, GPS_TIME_VALID, 1)
            Wend
        ElseIf DeviceExists(ID_RTC) Then
            id = ID_RTC
        End If
        If id > 0 Then
            ' we have something, read the UTC
            Dim yr As Integer, mn As Integer, dy As Integer
            Dim h As Integer, m As Integer, s As Integer
            buf = AUXCommand(id, GPS_GET_YEAR, 2)
            yr = buf(0) * &H100 + buf(1)
            buf = AUXCommand(id, GPS_GET_DATE, 2)
            mn = buf(0)
            dy = buf(1)
            buf = AUXCommand(id, GPS_GET_TIME, 3)
            h = buf(0)
            m = buf(1)
            s = buf(2)
            GetUTC = DateSerial(yr, mn, dy) + TimeSerial(h, m, s)
            Exit Function
        End If
    End If
    ' otherise get the time from the PC
Err:
    GetUTC = CDate(CDbl(Now()) + (CDbl(utc_offs()) / SPD)) 'pwgs: Changed sign to create correct UTC time
End Function

' Hibernate using NexRemote commands
' returns True if successful
Private Function Hibernate() As Boolean
    Dim buf As String
    Hibernate = False
    If SelectMenuItem("Utilities|Hibernate") Then
        ' Send ENTER to hibernate
        buf = SendNR("$E")
        Sleep 200
        buf = SendNR("!L1")
        If InStr(buf, "Power Off") = 1 Then
            Hibernate = True
        End If
    End If
End Function

' wake up using NR commands
Private Function WakeUp() As Boolean
    Dim buf As String
    Dim i As Integer
    WakeUp = False
    ' Check for Wake Up prompt
    i = 0
    buf = SendNR("!L1")
    If buf = "" Then Exit Function  ' NR not running
    While InStr(buf, "Wake Up") <> 1
        Sleep 1000
        buf = SendNR("!L1")
        If i > 10 Then
            Exit Function
        End If
        i = i + 1
    Wend
    SendNR "$E"
    Sleep 500
    buf = SendNR("!L1")
    ' get the time prompt
    While InStr(buf, "Time hh:mm:ss") <> 1
        Sleep 500
        buf = SendNR("!L1")
    Wend
    ' assume the time is OK
    SendNR "$E"
    Sleep 200
    ' check for the AM prompt
    buf = SendNR("!L1")
    If InStr(buf, "Select One") = 1 Then
        SendNR "$E"
        Sleep 200
    End If
    ' check for the DST prompt, assume it's OK
    SendNR "$E"
    Sleep 200
    ' check for the time zone prompt, assume it's OK
    SendNR "$E"
    Sleep 200
    ' check for the date prompt, assume it's OK
    SendNR "$E"
    Sleep 200
    ' That's it
    WakeUp = True
End Function

' NexRemote Command to set the home position
' return False if it fails (probably because it isn't using NR)
Private Function SetHome() As Boolean
    Dim buf As String
    SetHome = False
    If SelectMenuItem("Utilities|Home|Set") Then
        ' Send ENTER to set the position
''        buf = SendNR("$E")
'        Sleep 200
        SetHome = True
    End If
End Function

' NexRemote function to go to the home position
' return False if it fails (probably because it isn't using NR)
Private Function GotoHome() As Boolean
    Dim buf As String
    GotoHome = False
    Exit Function       ' not imlemented
    If SelectMenuItem("Utilities|Home|Goto") Then
        ' Send ENTER to move to the home position
'        buf = SendNR("$E")
'        Sleep 200
        ' wait for the move to complete
        Do
            Sleep 200
            buf = SendNR("!L1")
        Loop While Mid$(buf, 16, 1) <> " "
        GotoHome = True
    End If
End Function

' NexRemote function to do a last align
' return False if it fails
Private Function LastAlign() As Boolean
    Dim buf As String
    Dim i As Integer
    LastAlign = False
    Exit Function       ' disable for now, never got it tested
    ' Check for Wake Up prompt
    i = 0
    buf = SendNR("!L1")
    If buf = "" Then Exit Function  ' NR not running
    ' look for CGE Pro Ready or CGE Ready
    While InStr(buf, "Ready") <= 0
        Sleep 1000
        buf = SendNR("!L1")
        If i > 10 Then
            Exit Function
        End If
        i = i + 1
    Wend
    SendNR "$E"     ' Enter
    Sleep 500
    buf = SendNR("!L1")
    If InStr(buf, "SetSwitch Pos") <> 1 Then Exit Function
    SendNR "$E"         ' goto switch position
    Sleep 500
    ' wait until the scope has reached the switch position
    buf = SendNR("!L1")
    ' get the time prompt
    While InStr(buf, "Time hh:mm:ss") <> 1
        Sleep 500
        buf = SendNR("!L1")
    Wend
    ' assume the time is OK
    SendNR "$E"
    Sleep 200
    ' check for the AM prompt
    buf = SendNR("!L1")
    If InStr(buf, "Select One") = 1 Then
        SendNR "$E"
        Sleep 200
    End If
    ' check for the DST prompt, assume it's OK
    SendNR "$E"
    Sleep 200
    ' check for the time zone prompt, assume it's OK
    SendNR "$E"
    Sleep 200
    ' check for the date prompt, assume it's OK
    SendNR "$E"
    Sleep 200
    ' Check the alignment prompt
    buf = SendNR("!L1")
    If InStr(buf, "Select Alignment") <> 1 Then Exit Function
    If SelectMenuItem("Last Alignment") Then
        SendNR "$E"
        LastAlign = True
    End If
End Function

#If BETA_MODE Then

' code to read the Alt Az and return Ra and Dec
' used for pointing testing
Private Sub AltAzToRaDec()
    Dim buf() As Byte
    
    ' no checks, just get on with it
    If m_AltAzRaDec Then
        Dim ha As Double, dec As Double, lst As Double
        buf = ScopeCommandBinary(ToByte(NS_GET_ALTAZ_HP))
        ha = HexToDegB(buf, 0, 5) / 15#
        dec = HexToDegB(buf, 9, 14)
        lst = now_lst(degrad(g_dLongitude))
        If dec < 90 And dec > -90 Then
            g_dDec = dec
            g_dRA = lst - ha
        Else
            g_dDec = 180 - dec
            g_dRA = lst - ha
        End If
    Else
        buf = ScopeCommandBinary(ToByte(NS_GET_RADEC_HP))
        g_dRA = HexToDegB(buf, 0, 5) / 15#
        g_dDec = HexToDegB(buf, 9, 14)
    End If

End Sub

#End If
