Imports System.Web
Public Class CheckPhotoApprovalStatusPage
    Inherits System.Web.UI.Page
    Dim _DB As New ClassConnectSql()

    ''' <summary>
    ''' ทำการสร้าง div นักเรียนที่อัพรูปเข้ามาเพื่อให้ ครู หรือ admin อนุมัติ
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ClsSecurity.CheckConnectionIsSecure()
        'Dim StrDateTest As String = Date.Now.Year.ToString() & Date.Now.Month.ToString() & Date.Now.Day.ToString() & "-" & TimeOfDay.Hour.ToString() & ":" & TimeOfDay.Minute.ToString()

        If Not Page.IsPostBack Then
            If HttpContext.Current.Session("UserId") IsNot Nothing And HttpContext.Current.Session("SchoolCode") IsNot Nothing Then
                GenHtmlData()
            End If
        End If

    End Sub

    ''' <summary>
    ''' ทำการต่อสตริงสร้าง div รูปภาพที่นักเรียน upload เข้ามา
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub GenHtmlData()

        Dim sql As String = " SELECT tblStudentPhoto.Student_Id, tblStudentPhoto.IsFromParentTablet, tblStudentPhoto.StudentPhoto_Id,t360_tblStudent.School_Code " & _
                            " ,dbo.t360_tblStudent.Student_FirstName + ' ' + dbo.t360_tblStudent.Student_LastName AS StudentName, " & _
                            " dbo.t360_tblStudent.Student_CurrentNoInRoom,dbo.t360_tblStudent.Student_CurrentClass + dbo.t360_tblStudent.Student_CurrentRoom AS ClassRoom " & _
                            " FROM t360_tblStudent INNER JOIN t360_tblTeacherRoom ON t360_tblStudent.School_Code = t360_tblTeacherRoom.School_Code " & _
                            " AND t360_tblStudent.Student_CurrentClass = t360_tblTeacherRoom.Class_Name AND " & _
                            " t360_tblStudent.Student_CurrentRoom = t360_tblTeacherRoom.Room_Name INNER JOIN " & _
                            " tblStudentPhoto ON t360_tblStudent.Student_Id = tblStudentPhoto.Student_Id " & _
                            " WHERE (t360_tblTeacherRoom.Teacher_Id = '" & HttpContext.Current.Session("UserId").ToString() & "') AND " & _
                            " (t360_tblStudent.Student_IsActive = 1) AND (tblStudentPhoto.Approval_Status = 0) AND (t360_tblTeacherRoom.TR_IsActive = 1) ORDER BY dbo.tblStudentPhoto.Received_Date DESC "
        Dim dt As New DataTable
        dt = _DB.getdata(sql)
        If dt.Rows.Count > 0 Then
            Dim Sb As New StringBuilder
            'ตัวแปรที่เช็คว่าอัพโหลดรูปมาจาก tablet ของผู้ปกครองรึเปล่า
            Dim IsFromParentTablet As Boolean = False
            Dim StudentPhotoId As String = ""
            Dim StudentFullName As String = ""
            Dim StudentInfo As String = ""
            Dim ParentName As String = ""
            'loop เพื่อต่อสตริงสร้าง div รูปภาพที่ upload เข้ามา , เงื่อนไขการจบ loop คือ วนจนครบทุกรูปที่ upload เข้ามา
            For index = 0 To dt.Rows.Count - 1
                StudentPhotoId = dt.Rows(index)("StudentPhoto_Id").ToString()
                IsFromParentTablet = dt.Rows(index)("IsFromParentTablet")
                StudentFullName = dt.Rows(index)("StudentName")
                StudentInfo = "เลขที่ " & dt.Rows(index)("Student_CurrentNoInRoom").ToString() & " ชั้น " & dt.Rows(index)("ClassRoom").ToString()
                'ถ้ารูปถูกอัพมาจาก Tablet ของผู้ปกครองต้องแสดงข้อมูลอีกอย่าง
                If IsFromParentTablet = False Then
                    Sb.Append("<div id='" & StudentPhotoId & "' class='frame'>")
                    Sb.Append("<div class='Top'>")
                    Sb.Append("<span class='SpnName'>" & StudentFullName & "</span>")
                    Sb.Append("<br />")
                    Sb.Append("<span class='SpnDetail'>" & StudentInfo & "</span>")
                    Sb.Append("<br />")
                    Sb.Append("<img class='Photo' src='../UserData/" & dt.Rows(index)("School_Code").ToString() & "/{" & dt.Rows(index)("Student_Id").ToString() & "}/Id_tmp.jpg' />")
                    Sb.Append("</div>")
                    Sb.Append("<div class='Bottom'>")
                    Sb.Append("<img class='Approve' stdPtId='" & StudentPhotoId & "' src='../Images/ApproveButton.png' />")
                    Sb.Append("<img class='NotApprove' stdPtId='" & StudentPhotoId & "' src='../Images/NotApproveButton.png' />")
                    Sb.Append("</div>")
                    Sb.Append("</div>")
                Else 'ถ้าไม่ใช่ก็แสดงปกติ
                    ParentName = GetParentNameByStudentId(dt.Rows(index)("Student_Id").ToString())
                    Sb.Append("<div id='" & StudentPhotoId & "' class='frame'>")
                    Sb.Append("<div class='Top'>")
                    Sb.Append("<span class='SpnParent'>แจ้งเป็นผู้ปกครอง</span>")
                    Sb.Append("<span style='margin-left:5px;'>" & StudentInfo & "</span>")
                    Sb.Append("<br />")
                    Sb.Append("<span class='SpnDetail'>" & StudentFullName & "</span>")
                    Sb.Append("<br />")
                    Sb.Append("<span style='font-size:17px;'>โดย " & ParentName & "</span>")
                    Sb.Append("<br />")
                    Sb.Append("<img class='Photo' src='../UserData/" & dt.Rows(index)("School_Code").ToString() & "/{" & dt.Rows(index)("Student_Id").ToString() & "}/Id_tmp.jpg' />")
                    Sb.Append("</div>")
                    Sb.Append("<div class='Bottom'>")
                    Sb.Append("<img class='Approve' stdPtId='" & StudentPhotoId & "' src='../Images/ApproveButton.png' />")
                    Sb.Append("<img class='NotApprove' stdPtId='" & StudentPhotoId & "' src='../Images/NotApproveButton.png' />")
                    Sb.Append("</div>")
                    Sb.Append("</div>")
                End If
            Next
            DivListApprove.InnerHtml = Sb.ToString()
        End If

    End Sub

    ''' <summary>
    ''' หาชื่อ/นามสกุล ผู้ปกครอง
    ''' </summary>
    ''' <param name="StudentId">รหัสนักเรียน</param>
    ''' <returns>String,PR_First + PR_LastName</returns>
    ''' <remarks></remarks>
    Private Function GetParentNameByStudentId(ByVal StudentId As String) As String
        Dim sql As String = " SELECT PR_FirstName + ' ' + PR_LastName AS ParentFullName FROM dbo.tblParent INNER JOIN dbo.tblStudentParent " & _
                            " ON dbo.tblParent.PR_Id = dbo.tblStudentParent.PR_Id WHERE Student_Id = '" & StudentId & "' AND dbo.tblParent.IsActive = 1 "
        Dim ParentName As String = _DB.ExecuteScalar(sql)
        Return ParentName
    End Function

    ''' <summary>
    ''' เมื่อกดปุ่มอนุมัติรูป ต้องทำการ update ข้อมูล และย้ายรูป
    ''' </summary>
    ''' <param name="StudentPhotoId">id ของรูปที่อนุมัติ</param>
    ''' <returns>String:Complete,Error</returns>
    ''' <remarks></remarks>
    <Services.WebMethod()>
    Public Shared Function ApprovePhoto(ByVal StudentPhotoId As String) As String
        If HttpContext.Current.Session("UserId") Is Nothing OrElse (Guid.TryParse(HttpContext.Current.Session("UserId").ToString(), New Guid) = False) Then
            Return 0
        End If

        Dim _DB As New ClassConnectSql()
        Dim sql As String = ""

        'Update Database ว่าอนุมัติรุปนี้
        Try
            _DB.OpenWithTransection()
            sql = " UPDATE dbo.tblStudentPhoto SET Approval_Status = 1 , " &
                  " Approve_By = '" & HttpContext.Current.Session("UserId").ToString() & "',LastUpdate = dbo.GetThaiDate(),ClientId = NULL WHERE StudentPhoto_Id = '" & StudentPhotoId & "' "


            _DB.ExecuteWithTransection(sql)


            sql = " SELECT dbo.t360_tblStudent.Student_Id,School_Code FROM dbo.tblStudentPhoto " &
                  " INNER JOIN dbo.t360_tblStudent ON dbo.tblStudentPhoto.Student_Id = dbo.t360_tblStudent.Student_Id " &
                  " WHERE dbo.tblStudentPhoto.StudentPhoto_Id = '" & StudentPhotoId & "' "

            Dim dt As DataTable = _DB.getdataWithTransaction(sql)

            'Update ที่ t360_tblStudent
            sql = "update t360_tblstudent set Student_HasPhoto = '1',lastupdate = dbo.GetThaiDate(),ClientId = Null where Student_Id = '" & dt.Rows(0)("Student_Id").ToString() & "'"

            _DB.ExecuteWithTransection(sql)

            If dt.Rows.Count > 0 Then
                'Path ของรูปของนักเรียนคนนี้
                Dim StrPath As String = HttpContext.Current.Server.MapPath("../UserData/" & dt.Rows(0)("School_Code").ToString() & "/{" & dt.Rows(0)("Student_Id").ToString() & "}")
                'สตริงที่เป็นรูปแบบ วันที่/เวลา เพื่อจะนำไปเป็นส่วนนึงในชื่อรูป
                Dim StrDatetimeinfo As String = Date.Now.Year.ToString() & Date.Now.Month.ToString() & Date.Now.Day.ToString() & "-" & TimeOfDay.Hour.ToString() & "_" & TimeOfDay.Minute.ToString()
                'Path รูปเก่า
                Dim OldImg As String = ""
                'Path รูปใหม่
                Dim NewImg As String = ""
                'ต้องเช็คก่อนว่ามีไฟล์ Id.jpg,IdFullSize.jpg อยู่แล้วหรือเปล่า ถ้ามีอยู่แล้วต้อง Copy มาเป็นชื่อใหม่ที่ต้อด้วย วันที่ แล้วค่อยลบไฟล์ต้นฉบับทิ้ง
                If System.IO.File.Exists(StrPath & "/Id.jpg") = True Then
                    'ทำ Id.jpg ก่อน
                    OldImg = StrPath & "\Id.jpg"
                    NewImg = StrPath & "\Id" & StrDatetimeinfo & ".jpg"
                    'Copy รูปที่ upload ไว้ก่อนไปเป็นชื่อใหม่
                    System.IO.File.Copy(OldImg, NewImg)
                    System.IO.File.SetAttributes(OldImg, IO.FileAttributes.Normal)
                    'ลบรูปเก่าทิ้งเลย
                    System.IO.File.Delete(OldImg)

                    'ทำ IdFullSize.Ext 
                    Dim dr As System.IO.DirectoryInfo = New System.IO.DirectoryInfo(StrPath)
                    Dim aFile As System.IO.FileInfo() = dr.GetFiles("*IdFullSize*")
                    If aFile.Length > 0 Then
                        Dim StrExtension As String = aFile(0).Extension
                        System.IO.File.Copy(StrPath & "\IdFullSize" & StrExtension, StrPath & "\Id_FullSize" & StrDatetimeinfo & StrExtension)
                        System.IO.File.SetAttributes(StrPath & "\IdFullSize" & StrExtension, IO.FileAttributes.Normal)
                        System.IO.File.Delete(StrPath & "\IdFullSize" & StrExtension)
                    End If

                End If
                'Rename Id_tmp.jpg ให้เป็น Id.jpg
                System.IO.File.Copy(StrPath & "\Id_tmp.jpg", StrPath & "\Id.jpg")
                System.IO.File.SetAttributes(StrPath & "\Id_tmp.jpg", IO.FileAttributes.Normal)
                System.IO.File.Delete(StrPath & "\Id_tmp.jpg")

                'Rename IdFullSize_tmp.Ext ให้เป็น IdFullSize.Ext
                Dim drInfo As System.IO.DirectoryInfo = New System.IO.DirectoryInfo(StrPath)
                Dim bFile As System.IO.FileInfo() = drInfo.GetFiles("*IdFullSize*")
                If bFile.Length > 0 Then
                    Dim StrExtension As String = bFile(0).Extension
                    System.IO.File.Copy(StrPath & "\IdFullSize_tmp" & StrExtension, StrPath & "\IdFullSize" & StrExtension)
                    System.IO.File.SetAttributes(StrPath & "\IdFullSize_tmp" & StrExtension, IO.FileAttributes.Normal)
                    System.IO.File.Delete(StrPath & "\IdFullSize_tmp" & StrExtension)
                End If

                _DB.CommitTransection()
                _DB = Nothing
                Return "Complete"
            End If
        Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
            Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
            _DB.RollbackTransection()
            _DB = Nothing
            Return "Error"
        End Try
    End Function

    ''' <summary>
    ''' เมื่อกดปุ่มไม่อนุมัติรูปภาพ ให้ทำการ update ข้อมูล
    ''' </summary>
    ''' <param name="StudentPhotoId">Id ของรุปภาพที่ไม่อนุมัติ</param>
    ''' <returns>String:Complete,Error</returns>
    ''' <remarks></remarks>
    <Services.WebMethod()>
    Public Shared Function NotApprovePhoto(ByVal StudentPhotoId As String) As String
        If HttpContext.Current.Session("UserId") Is Nothing OrElse (Guid.TryParse(HttpContext.Current.Session("UserId").ToString(), New Guid) = False) Then
            Return 0
        End If
        Try
            Dim _DB As New ClassConnectSql()
            Dim sql As String = " UPDATE dbo.tblStudentPhoto SET Approval_Status = 2, " &
                                " Approve_By = '" & HttpContext.Current.Session("UserId").ToString() & "' " &
                                " ,LastUpdate = dbo.GetThaiDate(),ClientId = NULL WHERE StudentPhoto_Id = '" & StudentPhotoId & "' "
            _DB.Execute(sql)
            Return "Complete"
        Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
            Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
            Return "Error"
        End Try
    End Function

    ''' <summary>
    ''' เมื่อกดปุ่ม อนุมัติทั้งหมด ทำการ update ข้อมูล และย้ายรูป เหมือนที่ทำรูปเดียว แต่อันนี้ต้องวน loop เอา
    ''' </summary>
    ''' <returns>String:Error,Complete,0</returns>
    ''' <remarks></remarks>
    <Services.WebMethod()>
    Public Shared Function ApproveAll() As String
        If HttpContext.Current.Session("UserId") Is Nothing OrElse (Guid.TryParse(HttpContext.Current.Session("UserId").ToString(), New Guid) = False) Then
            Return 0
        End If

        Dim _DB As New ClassConnectSql()

        If HttpContext.Current.Session("UserId").ToString() IsNot Nothing Then
            Try
                'ทำการหาข้อมูล รูปทั้งหมด 
                Dim sql As String = " SELECT tblStudentPhoto.Student_Id, tblStudentPhoto.StudentPhoto_Id,t360_tblStudent.School_Code " &
                                    " FROM t360_tblStudent INNER JOIN t360_tblTeacherRoom ON t360_tblStudent.School_Code = t360_tblTeacherRoom.School_Code " &
                                    " AND t360_tblStudent.Student_CurrentClass = t360_tblTeacherRoom.Class_Name AND t360_tblStudent.Student_CurrentRoom = " &
                                    " t360_tblTeacherRoom.Room_Name INNER JOIN tblStudentPhoto ON t360_tblStudent.Student_Id = tblStudentPhoto.Student_Id " &
                                    " WHERE (t360_tblTeacherRoom.Teacher_Id = '" & HttpContext.Current.Session("UserId").ToString() & "') AND (t360_tblStudent.Student_IsActive = 1) " &
                                    " AND (tblStudentPhoto.Approval_Status = 0) AND (t360_tblTeacherRoom.TR_IsActive = 1) ORDER BY dbo.tblStudentPhoto.Received_Date DESC "
                Dim dt As New DataTable
                dt = _DB.getdata(sql)
                If dt.Rows.Count > 0 Then
                    'Open Transaction
                    _DB.OpenWithTransection()
                    Dim StudentId As String = ""
                    Dim SchoolId As String = ""
                    Dim StudentPhotoId As String = ""
                    Dim StrPath As String = ""
                    Dim StrDatetimeinfo As String = Date.Now.Year.ToString() & Date.Now.Month.ToString() & Date.Now.Day.ToString() & "-" & TimeOfDay.Hour.ToString() & "_" & TimeOfDay.Minute.ToString()
                    'loop เพื่อ update ข้อมูล และย้ายไฟล์รูปภาพตาม Pattern , เงื่อนไขที่กำหนด ทำจนครบทุกรูป
                    For index = 0 To dt.Rows.Count - 1
                        StudentId = dt.Rows(index)("Student_Id").ToString()
                        SchoolId = dt.Rows(index)("School_Code").ToString()
                        StudentPhotoId = dt.Rows(index)("StudentPhoto_Id").ToString()

                        'Update tblStdentPhoto ก่อน
                        sql = " UPDATE dbo.tblStudentPhoto SET Approval_Status = 1 , " &
                              " Approve_By = '" & HttpContext.Current.Session("UserId").ToString() & "',LastUpdate = dbo.GetThaiDate(),ClientId = NULL WHERE StudentPhoto_Id = '" & StudentPhotoId & "' "
                        _DB.ExecuteWithTransection(sql)

                        StrPath = HttpContext.Current.Server.MapPath("../UserData/" & dt.Rows(index)("School_Code").ToString() & "/{" & dt.Rows(index)("Student_Id").ToString() & "}")
                        'ต้องเช็คก่อนว่ามีไฟล์ Id.jpg,IdFullSize.jpg อยู่แล้วหรือเปล่า ถ้ามีอยู่แล้วต้อง Copy มาเป็นชื่อใหม่ที่ต้อด้วย วันที่ แล้วค่อยลบไฟล์ต้นฉบับทิ้ง
                        If System.IO.File.Exists(StrPath & "/Id.jpg") = True Then
                            'ทำ Id.jpg ก่อน
                            System.IO.File.Copy(StrPath & "\Id.jpg", StrPath & "\Id" & StrDatetimeinfo & ".jpg")
                            System.IO.File.SetAttributes(StrPath & "\Id.jpg", IO.FileAttributes.Normal)
                            System.IO.File.Delete(StrPath & "\Id.jpg")

                            'ทำ IdFullSize.Ext 
                            Dim dr As System.IO.DirectoryInfo = New System.IO.DirectoryInfo(StrPath)
                            Dim aFile As System.IO.FileInfo() = dr.GetFiles("*IdFullSize*")
                            If aFile.Length > 0 Then
                                Dim StrExtension As String = aFile(0).Extension
                                System.IO.File.Copy(StrPath & "\IdFullSize" & StrExtension, StrPath & "\Id_FullSize" & StrDatetimeinfo & StrExtension)
                                System.IO.File.SetAttributes(StrPath & "\IdFullSize" & StrExtension, IO.FileAttributes.Normal)
                                System.IO.File.Delete(StrPath & "\IdFullSize" & StrExtension)
                            End If
                        End If

                        'Rename Id_tmp.jpg ให้เป็น Id.jpg
                        System.IO.File.Copy(StrPath & "\Id_tmp.jpg", StrPath & "\Id.jpg")
                        System.IO.File.SetAttributes(StrPath & "\Id_tmp.jpg", IO.FileAttributes.Normal)
                        System.IO.File.Delete(StrPath & "\Id_tmp.jpg")

                        'Rename IdFullSize_tmp.Ext ให้เป็น IdFullSize.Ext
                        Dim drInfo As System.IO.DirectoryInfo = New System.IO.DirectoryInfo(StrPath)
                        Dim bFile As System.IO.FileInfo() = drInfo.GetFiles("*IdFullSize*")
                        If bFile.Length > 0 Then
                            Dim StrExtension As String = bFile(0).Extension
                            System.IO.File.Copy(StrPath & "\IdFullSize_tmp" & StrExtension, StrPath & "\IdFullSize" & StrExtension)
                            System.IO.File.SetAttributes(StrPath & "\IdFullSize_tmp" & StrExtension, IO.FileAttributes.Normal)
                            System.IO.File.Delete(StrPath & "\IdFullSize_tmp" & StrExtension)
                        End If
                    Next
                    _DB.CommitTransection()
                    _DB = Nothing
                    Return "Complete"
                Else
                    Return "Error"
                End If
            Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
                _DB.RollbackTransection()
                Return "Error"
            End Try
        End If

    End Function


End Class