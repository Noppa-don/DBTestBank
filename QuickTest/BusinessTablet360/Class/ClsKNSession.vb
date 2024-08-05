Imports System.Web

Public Class ClsKNSession

    'Dim _IsClsSessApp As Boolean
    'Dim SA As New Object

    'Public Sub New(ByVal IsClsSessApp As Boolean)
    '    _IsClsSessApp = IsClsSessApp
    '    If IsClsSessApp = True Then
    '        SA = New ClsSessApp
    '    Else
    '        SA = New ClsSessSetting
    '    End If
    'End Sub

    Default Public Property StoredValue(ByVal Key As String) As Object
        Get
            Return HttpContext.Current.Application(Key)
        End Get
        Set(ByVal value As Object)
            HttpContext.Current.Application(Key) = value
        End Set
    End Property

    ''' <summary>
    ''' global property ช่วยให้ code ในหน้าหลักๆ สั้นลง อ่านเข้าใจง่ายขึ้น
    ''' </summary>
    ''' <returns></returns>
    Public Shared ReadOnly Property RunMode As String
        Get
            If Not IsNothing(HttpContext.Current.Application("runmode")) Then
                Return HttpContext.Current.Application("runmode").ToString().Trim().ToLower()
            End If
            Return ""
        End Get
    End Property

    ''' <summary>
    ''' global property อ่านค่าจาก runmode มาเป็น boolean ให้เรียบร้อย / ช่วยให้ code ในหน้าหลักๆ สั้นลง อ่านเข้าใจง่ายขึ้น
    ''' </summary>
    ''' <returns></returns>
    Public Shared ReadOnly Property IsMaxONet As Boolean
        Get
            If Not IsNothing(HttpContext.Current.Application("runmode")) Then
                Dim Mode As String = HttpContext.Current.Application("runmode").ToString.Trim().ToLower()
                If Mode = "maxonet" Or Mode = "wetest" Then Return True
            End If
            Return False
        End Get
    End Property
    ''' <summary>
    ''' กรณี maxonet ไม่ต้องใช้ signalR, (น่าจะ ต้อง รวมกรณี standalone ด้วย, รอถาม เดก่อน)
    ''' (global property อ่านค่าจาก runmode มาเป็น boolean ให้เรียบร้อย / ช่วยให้ code ในหน้าหลักๆ สั้นลง อ่านเข้าใจง่ายขึ้น)
    ''' </summary>
    ''' <returns></returns>
    Public Shared ReadOnly Property NeedSignalR As Boolean
        Get
            'กรณี maxonet ไม่ต้องใช้ signalR, (น่าจะ ต้อง รวมกรณี standalone ด้วย, รอถาม เดก่อน)
            If Not IsNothing(HttpContext.Current.Application("runmode")) Then
                Dim runMode As String = HttpContext.Current.Application("runmode").ToString.Trim().ToLower()
                If runMode = "maxonet" OrElse runMode = "standalonenotablet" Then
                    Return False
                End If
            End If
            Return True
        End Get
    End Property

    ''' <summary>
    ''' get value schoolcode from langset.bin
    ''' </summary>
    ''' <returns></returns>
    Public Shared ReadOnly Property DefaultSchoolCode As String
        Get
            If Not IsNothing(HttpContext.Current.Application("defaultschoolcode")) Then
                'Return HttpContext.Current.Application("defaultschoolcode").ToString()
                Return HttpContext.Current.Session("schoolid")
            End If
            Return ""
        End Get
    End Property

    Public Shared ReadOnly Property IsAddQuestionBySchool As Boolean
        Get
            Return HttpContext.Current.Application("isaddquestionbyschool")
        End Get
    End Property

    Public Shared ReadOnly Property EnableReportQuestion As Boolean
        Get
            Return HttpContext.Current.Application("EnableReportQuestion")
        End Get
    End Property

    Public Shared ReadOnly Property CurrentCalendarId As String
        Get
            If Not IsNothing(HttpContext.Current.Application("CurrentCalendarId")) Then

            End If
            Return HttpContext.Current.Application("CurrentCalendarId").ToString()
        End Get
    End Property

    Public Shared ReadOnly Property IsSelectByEvalutionIndex As Boolean
        Get
            Return HttpContext.Current.Application("IsSelectByEvalutionIndex")
        End Get
    End Property

    'Public Function AddValueForClsSess(ByVal KeyUniqueID As String, ByVal PropertyName As String, ByVal InputValue As Object)
    '    'ต้องดักตอนเอาค่าลงเพราะมีหลาย Cls
    '    If KeyUniqueID IsNot Nothing Or KeyUniqueID <> "" Or PropertyName IsNot Nothing Or PropertyName <> "" Then
    '        Try
    '            Dim SAtype As Type = SA.GetType
    '            Dim SApt = SAtype.GetProperty(PropertyName)
    '            If SApt IsNot Nothing Then
    '                SApt.SetValue(SA, InputValue, Nothing)
    '            Else
    '                Return "ไม่มีชื่อตัวแปรนี้"
    '            End If
    '        Catch ex As FormatException 'Erro เมื่อค่าที่ส่งไป Add ผิด Format
    '            Return "-1"
    '        End Try
    '        If _IsClsSessApp = True Then 'ดักว่าเป็นการ Add ค่าให้กับ Cls ไหน
    '            HttpContext.Current.Application("Sess" & KeyUniqueID) = SA
    '        Else
    '            HttpContext.Current.Application("Quiz" & KeyUniqueID) = SA
    '        End If
    '        Return "Complete"
    '    Else
    '        Return "ข้อมูลที่ส่งเข้ามาผิดเงื่อนไข If แรก"
    '    End If
    'End Function

    'Public Function GetValueFromClsSess(ByVal KeyUniqueID As String, ByVal PropertyName As String)

    '    If KeyUniqueID IsNot Nothing Or KeyUniqueID <> "" Or PropertyName IsNot Nothing Or PropertyName <> "" Then

    '        If GetClsFromApplication(KeyUniqueID) Is Nothing Or GetClsFromApplication(KeyUniqueID) = "ไม่สามารถแปลงค่าจากตัวแปร Application ได้" Then
    '            Return Nothing
    '        End If

    '        Dim SAtype As Type = SA.GetType
    '        Dim SApt = SAtype.GetProperty(PropertyName)
    '        Dim ReturnValue As Object
    '        If SApt IsNot Nothing Then
    '            ReturnValue = SApt.GetValue(SA, Nothing)
    '            Return ReturnValue
    '        Else
    '            Return Nothing
    '        End If
    '    Else
    '        Return "ข้อมูลที่ส่งเข้ามาผิดเงื่อนไข If แรก"
    '    End If

    'End Function

    'Public Function GetClsFromApplication(ByVal KeyUniqueID As String) As String
    '    'รอเพิ่ม If ให้ดึงค่าจากอีก Cls นึงเพราะมี Cls Setting เพิ่ีมเข้ามา
    '    If KeyUniqueID IsNot Nothing Or KeyUniqueID <> "" Then
    '        If _IsClsSessApp = True Then
    '            If HttpContext.Current.Application("Sess" & KeyUniqueID) Is Nothing Then
    '                Return Nothing
    '            End If
    '        Else
    '            If HttpContext.Current.Application("Quiz" & KeyUniqueID) Is Nothing Then
    '                Return Nothing
    '            End If
    '        End If
    '        Try
    '            If _IsClsSessApp = True Then 'ดักว่าต้องการดึงค่าจาก Cls ไหน
    '                SA = CType(HttpContext.Current.Application("Sess" & KeyUniqueID), ClsSessApp)
    '            Else
    '                SA = CType(HttpContext.Current.Application("Quiz" & KeyUniqueID), ClsSessSetting)
    '            End If

    '        Catch ex As Exception
    '            Return "ไม่สามารถแปลงค่าจากตัวแปร Application ได้"
    '        End Try
    '        Return "Complete"
    '    Else
    '        Return "ไม่มี KeyUniqueID"
    '    End If
    'End Function

End Class

Public Class KNAppSession

    Default Public Property StoredValue(ByVal Key As String) As Object
        Get
            If HttpContext.Current.Session("UserId") IsNot Nothing And HttpContext.Current.Session("selectedSession") IsNot Nothing Then
                If Key Is Nothing Then Throw New Exception("Key Nothing")
                If Key.Trim() = String.Empty Then Throw New Exception("Key Empty")
                If HttpContext.Current.Application(HttpContext.Current.Session("UserId").ToString() & "|" & HttpContext.Current.Session("selectedSession").ToString() & "|" & Key) IsNot Nothing Then
                    Return HttpContext.Current.Application(HttpContext.Current.Session("UserId").ToString() & "|" & HttpContext.Current.Session("selectedSession").ToString() & "|" & Key)
                Else
                    Return Nothing
                End If
            Else
                Throw New Exception("Session 'UserId' และ Session 'SelectedSession' ไม่มีค่า ")
            End If
        End Get
        Set(ByVal value As Object)
            If HttpContext.Current.Session("UserId") IsNot Nothing And HttpContext.Current.Session("selectedSession") IsNot Nothing Then
                If Key Is Nothing Then Throw New Exception("Key Nothing")
                If Key.Trim() = String.Empty Then Throw New Exception("Key Empty")
                HttpContext.Current.Application(HttpContext.Current.Session("UserId").ToString() & "|" & HttpContext.Current.Session("selectedSession").ToString() & "|" & Key) = value
            Else
                Throw New Exception("Session 'UserId' และ Session 'SelectedSession' ไม่มีค่า ")
            End If
        End Set
    End Property

End Class
