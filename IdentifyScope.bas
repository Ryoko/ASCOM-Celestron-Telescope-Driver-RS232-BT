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