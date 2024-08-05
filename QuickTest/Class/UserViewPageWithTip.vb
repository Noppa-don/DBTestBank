Imports KnowledgeUtils

Public Class UserViewPageWithTip
    Dim redis As New RedisStore()
    Dim db As New ClassConnectSql()
    Private UserId As String

    Sub New(ByVal UserId As String)
        Me.UserId = UserId
        If redis.Getkey(Of Dictionary(Of Guid, String))("PageWithTip") Is Nothing Then
            Dim collPages As New Dictionary(Of Guid, String)
            Dim dt As DataTable = db.getdata("SELECT * FROM tblPageWithTip;")
            For Each r As DataRow In dt.Rows
                If Not collPages.Keys.Contains(r("PWTId")) Then
                    collPages.Add(r("PWTId"), r("PWTName"))
                End If
            Next
            redis.SetKey(Of Dictionary(Of Guid, String))("PageWithTip", collPages)
        End If
    End Sub

    Public Sub CheckIsViewAllTips(ByVal IsViewAllTips As Boolean)
        'Dim IsViewAllTips As Boolean = True
        If Not IsViewAllTips Then ' Or (Not dt.Rows(0)("IsViewAllTips"))
            Dim sqlSelectUserViewPage As String = String.Format("SELECT p.PWTName,u.PageView FROM tblUserViewPageWithTip u INNER JOIN tblPageWithTip p ON u.PWTId = p.PWTId WHERE u.UserId = '{0}';", UserId)
            Dim dtUserViewPageWithTip As DataTable = db.getdata(sqlSelectUserViewPage)

            If dtUserViewPageWithTip.Rows.Count = 0 Then
                db.Execute(String.Format("INSERT INTO tblUserViewPageWithTip SELECT NEWID(),PWTId,'{0}',0,1,dbo.GetThaiDate() FROM tblPageWithTip;", UserId))
                dtUserViewPageWithTip = db.getdata(sqlSelectUserViewPage)
            End If

            Dim dictViewPage As New Dictionary(Of String, Integer)
            Dim countUserView As Integer = 0
            For Each r As DataRow In dtUserViewPageWithTip.Rows
                If Not dictViewPage.Keys.Contains(r("PWTName")) Then
                    dictViewPage.Add(r("PWTName"), r("PageView"))
                End If
                countUserView += r("PageView")
            Next

            If (dtUserViewPageWithTip.Rows.Count * 3) = countUserView Then
                db.Execute(String.Format("UPDATE tblUser SET IsViewAllTips = 1 WHERE GUID = '{0}';", UserId))
                IsViewAllTips = True
            Else
                redis.SetKey(Of Dictionary(Of String, Integer))(String.Format("{0}_TipViewCount", UserId), dictViewPage) ' redis key เก็บ dictionary จำนวนของการเยี่ยมชวนแต่ละหน้าของ user
            End If
        End If
        redis.SetKey(Of Boolean)(String.Format("{0}_IsViewAllTips", UserId), IsViewAllTips) 'default values
    End Sub


    Public Function CheckUpdateUserViewPageWithTip(ByVal pageName As String, Optional ByVal IsStudent As Boolean = False) As Boolean ' teacher
        If IsStudent Then
            Dim IsShowTip As Boolean = UpdateUserViewPageWithTip(pageName)
            Dim collUserTipViewCount As Dictionary(Of String, Integer) = GetUserTipViewCount()
            Dim countUserView As Integer = 0
            For Each k As KeyValuePair(Of String, Integer) In collUserTipViewCount
                countUserView += k.Value
            Next
            Dim collPageWithTip As Dictionary(Of Guid, String) = GetPageWithTips()
            If (collPageWithTip.Count * 3) = countUserView Then ' นับจำนวนหน้าใน tblpagewithtips
                db.Execute(String.Format("UPDATE t360_tblStudent SET IsViewAllTips = 1 WHERE Student_Id = '{0}';", UserId))
                redis.SetKey(Of Boolean)(String.Format("{0}_IsViewAllTips", UserId), True)
            End If
            Return IsShowTip
        End If
        Return UpdateUserViewPageWithTip(pageName)
    End Function

    Private Function UpdateUserViewPageWithTip(ByVal pageName As String) As Boolean
        Dim collectionUserViewPage As Dictionary(Of String, Integer) = GetUserTipViewCount()
        If collectionUserViewPage IsNot Nothing Then
            If collectionUserViewPage.ContainsKey(pageName) Then
                If collectionUserViewPage.Item(pageName) < 3 Then
                    Dim PWTId As String = (From p In redis.Getkey(Of Dictionary(Of Guid, String))("PageWithTip") Where p.Value.ToLower = pageName.ToLower Select p.Key).Take(1).SingleOrDefault().ToString()
                    db.Execute(String.Format("UPDATE tblUserViewPageWithTip SET PageView = PageView + 1 WHERE UserId = '{0}' AND PWTId = '{1}';", UserId, PWTId))
                    collectionUserViewPage.Item(pageName) = collectionUserViewPage.Item(pageName) + 1
                    SetUserTipViewCount(collectionUserViewPage)
                    Return True
                End If
                Return False
            End If
        End If
        Return False
    End Function

    Public Function CheckUserViewPageWithTip(ByVal pageName As String) As Boolean ' For Student
        If redis.Getkey(Of Dictionary(Of Guid, String))("PageWithTip") IsNot Nothing Then

            Dim page As String = (From p In redis.Getkey(Of Dictionary(Of Guid, String))("PageWithTip") Where p.Value.ToLower = pageName.ToLower Select p.Value).Take(1).SingleOrDefault()
            If page Is Nothing OrElse page = "" Then
                ' insert หน้าที่ยังไม่เคยอยู่ใน db - tblPageWithTip
                Dim PWTId As String = db.ExecuteScalar("SELECT NEWID();")
                db.Execute(String.Format("INSERT INTO tblPageWithTip VALUES ('{0}','{1}',1,dbo.GetThaiDate());", PWTId, pageName.ToLower))
                ' เพิ่มหน้าเข้าไปใน Redis ด้วย
                Dim collPages As Dictionary(Of Guid, String) = redis.Getkey(Of Dictionary(Of Guid, String))("PageWithTip")
                If Not collPages.Keys.Contains(Guid.Parse(PWTId)) Then
                    collPages.Add(Guid.Parse(PWTId), pageName.ToLower)
                End If
                redis.SetKey(Of Dictionary(Of Guid, String))("PageWithTip", collPages)
            End If

            If GetUserTipViewCount() Is Nothing Then
                Dim dtUserViewPageWithTip As DataTable = db.getdata(String.Format("SELECT p.PWTName,u.PageView FROM tblUserViewPageWithTip u INNER JOIN tblPageWithTip p ON u.PWTId = p.PWTId WHERE u.UserId = '{0}';", UserId))
                Dim dictViewPage As New Dictionary(Of String, Integer)
                For Each r As DataRow In dtUserViewPageWithTip.Rows
                    If Not dictViewPage.Keys.Contains(r("PWTName")) Then
                        dictViewPage.Add(r("PWTName"), r("PageView"))
                    End If
                Next
                SetUserTipViewCount(dictViewPage)
            End If

            Dim currentPageExist As String = (From p In GetUserTipViewCount() Where p.Key.ToLower = pageName.ToLower Select p.Key).SingleOrDefault()
            If currentPageExist Is Nothing Or currentPageExist = "" Then 'ถ้าหน้าที่กำลังอยู่ไม่มีอยู่ใน Redis
                ' เพิ่มเข้าไปใน tblUserViewPageWithTip
                Dim PWTId As String = (From p In redis.Getkey(Of Dictionary(Of Guid, String))("PageWithTip") Where p.Value.ToLower = pageName.ToLower Select p.Key).Take(1).SingleOrDefault().ToString()
                db.Execute(String.Format("INSERT INTO tblUserViewPageWithTip VALUES (NEWID(),'{0}','{1}',0,1,dbo.GetThaiDate());", PWTId, UserId))
                ' เพิ่มเข้าไปใน Redis ของ User ด้วย
                Dim collUserView As Dictionary(Of String, Integer) = GetUserTipViewCount()
                If Not collUserView.Keys.Contains(pageName.ToLower) Then
                    collUserView.Add(pageName.ToLower, 0)
                End If
                SetUserTipViewCount(collUserView)
            End If

            Return CheckUpdateUserViewPageWithTip(pageName.ToLower, True)
        End If
        Return False
    End Function

    Private Function GetUserTipViewCount() As Dictionary(Of String, Integer)
        Return redis.Getkey(Of Dictionary(Of String, Integer))(String.Format("{0}_TipViewCount", UserId))
    End Function
    Private Sub SetUserTipViewCount(ByVal coll As Dictionary(Of String, Integer))
        redis.SetKey(Of Dictionary(Of String, Integer))(String.Format("{0}_TipViewCount", UserId), coll)
    End Sub

    Private Function GetPageWithTips() As Dictionary(Of Guid, String)
        Return redis.Getkey(Of Dictionary(Of Guid, String))("PageWithTip")
    End Function
End Class
