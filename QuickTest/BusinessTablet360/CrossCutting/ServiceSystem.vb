Imports KnowledgeUtils.Database.DBFactory.LinqToSql(Of BusinessTablet360.DataClassesTablet360DataContext)
Imports KnowledgeUtils.Database.DBFactory
Imports KnowledgeUtils.Database
Imports System.Web

Public Class ServiceSystem

    Public Shared Function HomeworkTimeToText(ByVal StartDate As Date, ByVal EndDate As Date) As String
        If StartDate.Month = EndDate.Month Then
            Return "ทำในช่วง " & StartDate.Day & " - " & EndDate.Day & "/" & EndDate.Month & "/" & EndDate.Year + 543
        Else
            Return "ทำในช่วง " & StartDate.Day & "/" & StartDate.Month & " - " & EndDate.Day & "/" & EndDate.Month & "/" & EndDate.Year
        End If
    End Function

    Public Shared Function RandomNumber() As String
        Dim Rd As New Random()
        Dim RdInt = Rd.Next(0, 999999)
        Return Strings.Format(RdInt, "000000")
    End Function

    Public Shared Function GetIPAddress() As String
        Dim context As System.Web.HttpContext = System.Web.HttpContext.Current
        Dim sIPAddress As String = context.Request.ServerVariables("HTTP_X_FORWARDED_FOR")
        If String.IsNullOrEmpty(sIPAddress) Then
            Return context.Request.ServerVariables("REMOTE_ADDR")
        Else
            Dim ipArray As String() = sIPAddress.Split(New [Char]() {","c})
            Return ipArray(0)
        End If
    End Function

    Public Shared Function CalculateHeight(ByVal h As Integer, ByVal NumOfPiece As Integer) As Integer
        'min-45 , max-400
        Dim m As Integer = (h / NumOfPiece)
        If m < 45 Then
            m = 45
        ElseIf m > 400 Then
            m = 400
        Else
            m = m
        End If
        CalculateHeight = m * NumOfPiece
    End Function

    Public Shared Function ConvertForOrderRoom(ByVal Source As String) As String
        Dim Result As String
        Result = Strings.Replace(Source, "/", "")
        Result = Strings.Replace(Result, " ", "")
        If IsNumeric(Result) Then
            Result = Format(CType(Result, Integer), "000")
        End If
        If Result = Source Then
            Result = Source
        End If
        Return Result
    End Function

    ''' <summary>
    ''' คำนวนให้เป็นวันเดียวกับที่ส่งมา แต่จะได้เป็นเวลา 23 นาฬิการ 59 นาที 59 วินาที
    ''' </summary>
    ''' <param name="StartDate"></param>
    ''' <param name="DiffDate"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function CalEndDateForSync(ByVal StartDate As Date, DiffDate As Integer) As Date
        Return StartDate.AddDays(DiffDate)
    End Function

    Public Shared Sub CheckData()
        Using Ctx = GetLinqToSql.GetDataContext
            Dim Data = Ctx.t360_tblMenuItems.Where(Function(q) q.MenuItem_Code = 61).SingleOrDefault
            If Data Is Nothing Then
                Dim NewData As New t360_tblMenuItem
                With NewData
                    .MenuItem_Code = 61
                    .MenuItem_Parent = 1
                    .MenuItem_Name = "ปรับเลื่อนชั้นนักเรียน"
                    .MenuItem_Page = "StudentManagerPage.aspx"
                    .MenuItem_Type = 1
                    .MenuItem_Order = 7
                    .MenuItem_IsActive = 1
                    .LastUpdate = Now
                End With
                Ctx.t360_tblMenuItems.InsertOnSubmit(NewData)
            End If

            Data = Ctx.t360_tblMenuItems.Where(Function(q) q.MenuItem_Code = 62).SingleOrDefault
            If Data Is Nothing Then
                Dim NewData As New t360_tblMenuItem
                With NewData
                    .MenuItem_Code = 62
                    .MenuItem_Parent = 1
                    .MenuItem_Name = "ตรวจสอบปรับปรุง"
                    .MenuItem_Page = "StudentManagerPage.aspx"
                    .MenuItem_Type = 1
                    .MenuItem_Order = 8
                    .MenuItem_IsActive = 1
                    .LastUpdate = Now
                End With
                Ctx.t360_tblMenuItems.InsertOnSubmit(NewData)
            End If

            'Load วันที่เลื่อนชั้นที่ตั้งไว้ถ้ามี
            HttpContext.Current.Application("DateSchedulerUplevel") = Ctx.t360_tblUpLevels _
            .Where(Function(q) q.School_Code = WebApplicationManager.GetSchoolId And q.IsActive = True) _
            .Select(Function(q) q.ScheduleDate).SingleOrDefault

            'เพิ่มเด็กถือแท็บเล็ต
            'With GetLinqToSql
            '    .MainSql = "select * from t360_tblStudent {f} "
            '    .ApplyTextPart("f", " WHERE Student_IsActive = 1 and School_Code='1000007' and Student_Status=1  and Student_id not in (select Owner_Id from t360_tblTabletOwner)")
            '    Dim StudentNotTab = .DataContextExecuteObjects(Of t360_tblStudent).ToArray

            '    For Each Student In StudentNotTab
            '        .MainSql = "select TOP 1 * from t360_tblTablet {f} "
            '        .ApplyTextPart("f", "  where Tablet_IsActive = 1 and Tablet_IsOwner=0")
            '        Dim Tab = .DataContextExecuteObjects(Of t360_tblTablet).SingleOrDefault
            '        If Tab Is Nothing Then
            '            Exit For
            '        End If
            '        Dim NewOwner As New t360_tblTabletOwner
            '        NewOwner.TON_Id = Guid.NewGuid
            '        NewOwner.Tablet_Id = Tab.Tablet_id
            '        NewOwner.Owner_Id = Student.Student_Id
            '        NewOwner.School_Code = Student.School_Code
            '        NewOwner.Owner_Type = EnTabletOwnerType.Student
            '        NewOwner.TON_ReceiveDate = Now
            '        NewOwner.TabletOwner_IsActive = True
            '        NewOwner.LastUpdate = Now
            '        Ctx.t360_tblTabletOwners.InsertOnSubmit(NewOwner)

            '        Dim TabUpdate = Ctx.t360_tblTablets.Where(Function(q) q.Tablet_id = Tab.Tablet_id).SingleOrDefault
            '        TabUpdate.Tablet_IsOwner = True
            '        Ctx.SubmitChanges()
            '    Next
            'End With


            'เพิ่มเครื่องหาย
            'For index = 1 To 45
            '    Dim NewTab As New t360_tblTablet
            '    NewTab.LastUpdate = Now
            '    NewTab.Tablet_LastUpdate = Now
            '    NewTab.School_Code = "1000007"
            '    NewTab.Tablet_IsOwner = False
            '    NewTab.Tablet_id = Guid.NewGuid
            '    NewTab.Tablet_SerialNumber = Guid.NewGuid.ToString
            '    NewTab.Tablet_Status = EnTabletStatus.Normal
            '    NewTab.Tablet_TabletName = "T" & index
            '    NewTab.Tablet_IsActive = True
            '    Ctx.t360_tblTablets.InsertOnSubmit(NewTab)
            'Next


            'เพิ่มแจ้งหาย
            'With GetLinqToSql
            '    .MainSql = "select TOP 40 * from t360_tblTablet {f} "
            '    .ApplyTextPart("f", " WHERE Tablet_IsActive = 1 and Tablet_IsOwner=0 and School_Code='1000007'")
            '    Dim Tabs = .DataContextExecuteObjects(Of t360_tblStudent).ToArray

            '    For Each Row In Tabs

            '        Dim NewData As New t360_tblTabletLost
            '        With NewData
            '            .IsActive = True
            '            .LastUpdate = Now
            '            .School_Code = "1000007"
            '            .TLT_DocNumber = "10000Test"
            '            .TLT_LostDate = Now
            '            .TLT_Station = "สน. บางรัก"
            '            .TON_Id
            '        End With
            '        NewOwner.TON_Id = Guid.NewGuid
            '        NewOwner.Tablet_Id = Tab.Tablet_id
            '        NewOwner.Owner_Id = Student.Student_Id
            '        NewOwner.School_Code = Student.School_Code
            '        NewOwner.Owner_Type = EnTabletOwnerType.Student
            '        NewOwner.TON_ReceiveDate = Now
            '        NewOwner.TabletOwner_IsActive = True
            '        NewOwner.LastUpdate = Now
            '        Ctx.t360_tblTabletOwners.InsertOnSubmit(NewOwner)

            '        Dim TabUpdate = Ctx.t360_tblTablets.Where(Function(q) q.Tablet_id = Tab.Tablet_id).SingleOrDefault
            '        TabUpdate.Tablet_IsOwner = True
            '        Ctx.SubmitChanges()
            '    Next
            'End With


            Ctx.SubmitChanges()
        End Using
    End Sub

    Public Shared Function TranformFormatStrToGUID(ByVal InputStr As String)
        If InputStr IsNot Nothing And InputStr.Trim() <> "" Then
            InputStr = InputStr.Insert(8, "-")
            InputStr = InputStr.Insert(13, "-")
            InputStr = InputStr.Insert(18, "-")
            InputStr = InputStr.Insert(23, "-")
            Return InputStr
        Else
            Return ""
        End If
    End Function

    Public Shared Function GetConnStr(ByVal InputKNConfig As String, Optional KeyType As EnLangType = EnLangType.Quick) As String
        InputKNConfig = KNConfigData.DecryptData(InputKNConfig)
        Dim objKnConfig = InputKNConfig.Split("|")
        Dim objEachKey = Nothing
        Dim ConnStr As String = ""
        For Each r In objKnConfig
            objEachKey = r.ToString().Split("*")
            Dim Eachkey As String = objEachKey(0)
            If KeyType = EnLangType.Quick Then
                If Eachkey.ToLower() = "localdb" Then
                    ConnStr = objEachKey(1)
                    Exit For
                ElseIf Eachkey.ToLower() = "centraldb" Then
                    ConnStr = objEachKey(1)
                    Exit For
                End If
            Else
                If Eachkey.ToLower() = "ConnectionString".ToLower Then
                    ConnStr = objEachKey(1)
                    Exit For
                End If
            End If
        Next
        Return ConnStr
    End Function

End Class
