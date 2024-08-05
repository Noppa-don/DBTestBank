Imports System.Runtime.CompilerServices
Public Module PPExtensionMethods

    Dim _DB As New ClassConnectSql

    ''' <summary>
    ''' function ในการแปลงเลข GUID ของ GroupSujectId เป็นชื่อกลุ่มภาษาไทย เช่น GUID => กลุ่มสาระการเรียนรู้ศิลปะ
    ''' </summary>
    ''' <param name="value"></param>
    ''' <returns>ชื่อกลุ่มสาระการเรียนรู้ของแต่ละวิชา</returns>
    <Extension()>
    Public Function ToGroupSubjectThName(value As String) As String
        Dim newGuid As Guid
        If Guid.TryParse(value, newGuid) Then
            Select Case value.ToString().ToUpper()
                Case CommonSubjectsText.ArtSubjectId
                    Return CommonSubjectsText.GroupSubjectName.Art
                Case CommonSubjectsText.EnglishSubjectId
                    Return CommonSubjectsText.GroupSubjectName.English
                Case CommonSubjectsText.HealthSubjectId
                    Return CommonSubjectsText.GroupSubjectName.Health
                Case CommonSubjectsText.HomeSubjectId
                    Return CommonSubjectsText.GroupSubjectName.Home
                Case CommonSubjectsText.MathSubjectId
                    Return CommonSubjectsText.GroupSubjectName.Math
                Case CommonSubjectsText.ScienceSubjectId
                    Return CommonSubjectsText.GroupSubjectName.Science
                Case CommonSubjectsText.SocialSubjectId
                    Return CommonSubjectsText.GroupSubjectName.Social
                Case CommonSubjectsText.ThaiSubjectId
                    Return CommonSubjectsText.GroupSubjectName.Thai
            End Select
        End If
        Return value
    End Function

    ''' <summary>
    ''' function ในการแปลงเลข GUID GroupsubjectId เป็นชื่อวิชา(ไทย) แบบสั้น เช่น GUID => ศิลปะ
    ''' </summary>
    ''' <param name="value"></param>
    ''' <returns>ชื่อวิชา(ไทย) แบบยาว</returns>
    <Extension()>
    Public Function ToSubjectShortThName(value As String) As String
        Dim newGuid As Guid
        If Guid.TryParse(value, newGuid) Then
            Select Case value.ToString().ToUpper()
                Case CommonSubjectsText.ArtSubjectId
                    Return CommonSubjectsText.SubjectBookName.Art
                Case CommonSubjectsText.EnglishSubjectId
                    Return CommonSubjectsText.SubjectBookName.English
                Case CommonSubjectsText.HealthSubjectId
                    Return CommonSubjectsText.SubjectBookName.Health
                Case CommonSubjectsText.HomeSubjectId
                    Return CommonSubjectsText.SubjectBookName.Home
                Case CommonSubjectsText.MathSubjectId
                    Return CommonSubjectsText.SubjectBookName.Math
                Case CommonSubjectsText.ScienceSubjectId
                    Return CommonSubjectsText.SubjectBookName.Science
                Case CommonSubjectsText.SocialSubjectId
                    Return CommonSubjectsText.SubjectBookName.Social
                Case CommonSubjectsText.ThaiSubjectId
                    Return CommonSubjectsText.SubjectBookName.Thai
            End Select
        End If
        Return value
    End Function

    ''' <summary>
    ''' extension ในการแปลงชื่อกลุ่มสาระการเรียนรู้เป็นชื่ออังกฤษแบบสั้น เช่น "กลุ่มสาระการเรียนรู้ภาษาไทย" = thai
    ''' </summary>
    ''' <param name="value">GroupSubjectName(ชื่อกลุ่มสาระการเรียนรู้เป็นภาษาไทย)</param>
    ''' <returns>value</returns>
    <Extension()>
    Public Function ToSubjShortEngName(value As String) As String
        Dim tempValue As String
        Select Case value.ToString().ToUpper()
            Case CommonSubjectsText.GroupSubjectName.Art
                tempValue = CommonSubjectsText.SubjectShortEngName.Art
            Case CommonSubjectsText.GroupSubjectName.English
                tempValue = CommonSubjectsText.SubjectShortEngName.English
            Case CommonSubjectsText.GroupSubjectName.Health
                tempValue = CommonSubjectsText.SubjectShortEngName.Health
            Case CommonSubjectsText.GroupSubjectName.Home
                tempValue = CommonSubjectsText.SubjectShortEngName.Home
            Case CommonSubjectsText.GroupSubjectName.Math
                tempValue = CommonSubjectsText.SubjectShortEngName.Math
            Case CommonSubjectsText.GroupSubjectName.Science
                tempValue = CommonSubjectsText.SubjectShortEngName.Science
            Case CommonSubjectsText.GroupSubjectName.Social
                tempValue = CommonSubjectsText.SubjectShortEngName.Social
            Case CommonSubjectsText.GroupSubjectName.Thai
                tempValue = CommonSubjectsText.SubjectShortEngName.Thai
            Case "กลุ่มสาระการเรียนรู้สุขศึกษาและพละศึกษา"
                tempValue = CommonSubjectsText.SubjectShortEngName.Health
            Case Else
                tempValue = CommonSubjectsText.SubjectShortEngName.PISA
        End Select
        Return tempValue
    End Function

    ''' <summary>
    ''' extension ในการแปลงชื่อกลุ่มสาระการเรียนรู้เป็นชื่อไทยแบบสั้น เช่น "กลุ่มสาระการเรียนรู้ภาษาไทย" = ไทย
    ''' </summary>
    ''' <param name="value">GroupSubjectName(ชื่อกลุ่มสาระการเรียนรู้เป็นภาษาไทย)</param>
    ''' <returns>value</returns>
    <Extension()>
    Public Function ToSubjShortThName(value As String) As String
        Dim tempValue As String
        Select Case value.ToString().ToUpper()
            Case CommonSubjectsText.GroupSubjectName.Art Or "กลุ่่มสาระการเรียนรู้ศิลปะ"
                tempValue = CommonSubjectsText.SubjectShortThaiName.Art
            Case CommonSubjectsText.GroupSubjectName.English
                tempValue = CommonSubjectsText.SubjectShortThaiName.English
            Case CommonSubjectsText.GroupSubjectName.Health
                tempValue = CommonSubjectsText.SubjectShortThaiName.Health
            Case CommonSubjectsText.GroupSubjectName.Home
                tempValue = CommonSubjectsText.SubjectShortThaiName.Home
            Case CommonSubjectsText.GroupSubjectName.Math
                tempValue = CommonSubjectsText.SubjectShortThaiName.Math
            Case CommonSubjectsText.GroupSubjectName.Science
                tempValue = CommonSubjectsText.SubjectShortThaiName.Science
            Case CommonSubjectsText.GroupSubjectName.Social
                tempValue = CommonSubjectsText.SubjectShortThaiName.Social
            Case CommonSubjectsText.GroupSubjectName.Thai
                tempValue = CommonSubjectsText.SubjectShortThaiName.Thai
            Case "กลุ่มสาระการเรียนรู้สุขศึกษาและพละศึกษา"
                tempValue = CommonSubjectsText.SubjectShortThaiName.Health
            Case Else
                tempValue = CommonSubjectsText.SubjectShortThaiName.PISA
        End Select
        Return tempValue
    End Function

    ''' <summary>
    ''' function ในการแปลง เลข Guid SubjectId เป็นชื่อไทยสั้นๆ เช่น GUID --> ไทย
    ''' </summary>
    ''' <param name="value"></param>
    ''' <returns></returns>
    <Extension()>
    Public Function SubjectIdToShortThName(value As String) As String
        Dim tempValue As String = value
        Dim newGuid As Guid
        If Guid.TryParse(value, newGuid) Then
            Select Case value.ToString().ToUpper()
                Case CommonSubjectsText.ArtSubjectId
                    tempValue = CommonSubjectsText.SubjectShortThaiName.Art
                Case CommonSubjectsText.EnglishSubjectId
                    tempValue = CommonSubjectsText.SubjectShortThaiName.English
                Case CommonSubjectsText.HealthSubjectId
                    tempValue = CommonSubjectsText.SubjectShortThaiName.Health
                Case CommonSubjectsText.HomeSubjectId
                    tempValue = CommonSubjectsText.SubjectShortThaiName.Home
                Case CommonSubjectsText.MathSubjectId
                    tempValue = CommonSubjectsText.SubjectShortThaiName.Math
                Case CommonSubjectsText.ScienceSubjectId
                    tempValue = CommonSubjectsText.SubjectShortThaiName.Science
                Case CommonSubjectsText.SocialSubjectId
                    tempValue = CommonSubjectsText.SubjectShortThaiName.Social
                Case CommonSubjectsText.ThaiSubjectId
                    tempValue = CommonSubjectsText.SubjectShortThaiName.Thai
                Case Else
                    tempValue = CommonSubjectsText.SubjectShortThaiName.PISA
            End Select
        End If
        Return tempValue
    End Function

    <Extension()>
    Public Function ToSubjectBookThName(value As String) As String
        Dim newGuid As Guid
        If Guid.TryParse(value, newGuid) Then
            Select Case value.ToString().ToUpper()
                Case CommonSubjectsText.ArtSubjectId
                    Return CommonSubjectsText.SubjectBookName.Art
                Case CommonSubjectsText.EnglishSubjectId
                    Return CommonSubjectsText.SubjectBookName.English
                Case CommonSubjectsText.HealthSubjectId
                    Return CommonSubjectsText.SubjectBookName.Health
                Case CommonSubjectsText.HomeSubjectId
                    Return CommonSubjectsText.SubjectBookName.Home
                Case CommonSubjectsText.MathSubjectId
                    Return CommonSubjectsText.SubjectBookName.Math
                Case CommonSubjectsText.ScienceSubjectId
                    Return CommonSubjectsText.SubjectBookName.Science
                Case CommonSubjectsText.SocialSubjectId
                    Return CommonSubjectsText.SubjectBookName.Social
                Case CommonSubjectsText.ThaiSubjectId
                    Return CommonSubjectsText.SubjectBookName.Thai
            End Select
        End If
        Return value
    End Function

    ''' <summary>
    ''' ใช้แปลงค่าจาก LevelId(GUID) เป็น text ธรรมดา เช่น GUID ---> ป.1
    ''' </summary>
    ''' <param name="value"></param>
    ''' <returns></returns>
    <Extension()>
    Public Function ToLevelShortName(value As String) As String
        Dim newGuid As Guid
        If Guid.TryParse(value, newGuid) Then
            Return GetLevelShortname(value)
            'Select Case value.ToString().ToUpper()
            '    Case CommonLevelsText.K4Id
            '        'Return CommonLevelsText.k4LevelShortName
            '    Case CommonLevelsText.K5Id
            '        Return CommonLevelsText.k5LevelShortName
            '    Case CommonLevelsText.K6Id
            '        Return CommonLevelsText.k6LevelShortName
            '    Case CommonLevelsText.K7Id
            '        Return CommonLevelsText.k7LevelShortName
            '    Case CommonLevelsText.K8Id
            '        Return CommonLevelsText.k8LevelShortName
            '    Case CommonLevelsText.K9Id
            '        Return CommonLevelsText.k9LevelShortName
            '    Case CommonLevelsText.K10Id
            '        Return CommonLevelsText.k10LevelShortName
            '    Case CommonLevelsText.K11Id
            '        Return CommonLevelsText.k11LevelShortName
            '    Case CommonLevelsText.K12Id
            '        Return CommonLevelsText.k12LevelShortName
            '    Case CommonLevelsText.K13Id
            '        Return CommonLevelsText.k13LevelShortName
            '    Case CommonLevelsText.K14Id
            '        Return CommonLevelsText.k14LevelShortName
            '    Case CommonLevelsText.K15Id
            '        Return CommonLevelsText.k15LevelShortName
            '    Case Else
            '        Return "ไม่มีชื่อชั้น"
            'End Select
        End If
        Return value
    End Function

    ''' <summary>
    ''' แปลง ClassId เป็นชื่อชั้นเลขไทย
    ''' </summary>
    ''' <param name="value"></param>
    ''' <returns></returns>
    <Extension()>
    Public Function ToLevelShortThaiName(value As String) As String
        Dim newGuid As Guid
        If Guid.TryParse(value, newGuid) Then
            Select Case value.ToString().ToUpper()
                Case CommonLevelsText.K4Id
                    Return CommonLevelsText.k4LevelShortThName
                Case CommonLevelsText.K5Id
                    Return CommonLevelsText.k5LevelShortThName
                Case CommonLevelsText.K6Id
                    Return CommonLevelsText.k6LevelShortThName
                Case CommonLevelsText.K7Id
                    Return CommonLevelsText.k7LevelShortThName
                Case CommonLevelsText.K8Id
                    Return CommonLevelsText.k8LevelShortThName
                Case CommonLevelsText.K9Id
                    Return CommonLevelsText.k9LevelShortThName
                Case CommonLevelsText.K10Id
                    Return CommonLevelsText.k10LevelShortThName
                Case CommonLevelsText.K11Id
                    Return CommonLevelsText.k11LevelShortThName
                Case CommonLevelsText.K12Id
                    Return CommonLevelsText.k12LevelShortThName
                Case CommonLevelsText.K13Id
                    Return CommonLevelsText.k13LevelShortThName
                Case CommonLevelsText.K14Id
                    Return CommonLevelsText.k14LevelShortThName
                Case CommonLevelsText.K15Id
                    Return CommonLevelsText.k15LevelShortThName
                Case Else
                    Return "ไม่มีชื่อชั้น"
            End Select
        End If
        Return value
    End Function

    ''' <summary>
    ''' แปลง classid เป็นรูปแปปที่ตามด้วย K ตัวอย่างเช่น เลข Class GUID ป.1 จะได้ K4
    ''' </summary>
    ''' <param name="value"></param>
    ''' <returns></returns>
    <Extension()>
    Public Function ToClassKFormat(value As String) As String
        Dim newGuid As Guid
        If Guid.TryParse(value, newGuid) Then
            Select Case value.ToString().ToUpper()
                Case CommonLevelsText.K4Id
                    Return "k4"
                Case CommonLevelsText.K5Id
                    Return "k5"
                Case CommonLevelsText.K6Id
                    Return "k6"
                Case CommonLevelsText.K7Id
                    Return "k7"
                Case CommonLevelsText.K8Id
                    Return "k8"
                Case CommonLevelsText.K9Id
                    Return "k9"
                Case CommonLevelsText.K10Id
                    Return "k10"
                Case CommonLevelsText.K11Id
                    Return "k11"
                Case CommonLevelsText.K12Id
                    Return "k12"
                Case CommonLevelsText.K13Id
                    Return "k13"
                Case CommonLevelsText.K14Id
                    Return "k14"
                Case CommonLevelsText.K15Id
                    Return "k15"
                Case Else
                    Return ""
            End Select
        End If
        Return value
    End Function

    ''' <summary>
    ''' function ในการแปลงชื่อวิชาอังกฤษ เป็น subjectID
    ''' </summary>
    ''' <param name="value"></param>
    ''' <returns>subjectID</returns>
    <Extension()>
    Public Function ToSubjectId(value As String) As String
        Select Case value
            Case CommonSubjectsText.SubjectShortEngName.Art
                Return CommonSubjectsText.ArtSubjectId
            Case CommonSubjectsText.SubjectShortEngName.English
                Return CommonSubjectsText.EnglishSubjectId
            Case CommonSubjectsText.SubjectShortEngName.Health
                Return CommonSubjectsText.HealthSubjectId
            Case CommonSubjectsText.SubjectShortEngName.Home
                Return CommonSubjectsText.HomeSubjectId
            Case CommonSubjectsText.SubjectShortEngName.Math
                Return CommonSubjectsText.MathSubjectId
            Case CommonSubjectsText.SubjectShortEngName.Science
                Return CommonSubjectsText.ScienceSubjectId
            Case CommonSubjectsText.SubjectShortEngName.Social
                Return CommonSubjectsText.SocialSubjectId
            Case CommonSubjectsText.SubjectShortEngName.Thai
                Return CommonSubjectsText.ThaiSubjectId
            Case CommonSubjectsText.SubjectShortEngName.PISA.ToLower
                Return CommonSubjectsText.PisaSubjectId
            Case Else
                Return value
        End Select
    End Function

    ''' <summary>
    ''' function สำหรับการหาที่อยู่ของ file ซึ่งมาจากการแปลง qsetId เช่น /e/f/g/f/4/{qsetId}/
    ''' </summary>
    ''' <param name="value"></param>
    ''' <returns>string File Path</returns>
    <Extension()>
    Public Function ToFolderFilePath(value As String) As String
        If Not Guid.TryParse(value, New Guid) Then Return value

        'Dim PathProJect As String = HttpContext.Current.Server.MapPath("../file/")
        Dim subFolder As New ArrayList
        subFolder.Add(value.Substring(0, 1))
        subFolder.Add(value.Substring(1, 1))
        subFolder.Add(value.Substring(2, 1))
        subFolder.Add(value.Substring(3, 1))
        subFolder.Add(value.Substring(4, 1))
        subFolder.Add(value.Substring(5, 1))
        subFolder.Add(value.Substring(6, 1))
        subFolder.Add(value.Substring(7, 1))
        subFolder.Add("{" & value & "}")

        Dim path As String = "../file/"
        For Each i In subFolder
            path &= i & "/"
        Next
        Return path
    End Function

    Private Function GetLevelShortname(LevelId As String) As String
        Dim sql As String = "select Level_ShortName from tblLevel where Level_Id = '" & LevelId & "'"
        Dim LevelShortname As String = _DB.ExecuteScalar(sql)
        Return LevelShortname
    End Function
End Module
