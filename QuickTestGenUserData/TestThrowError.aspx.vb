Public Class WebForm1
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try

            If CheckIfRequestIsFromLocalMachine() Then
                ErrorLabel.Text = "เดี๋ยวจะทำการ throwerror แล้ว-> ให้ไปรอ รับเมล์ที่ elmah-pointplus@klnetwork.com ได้เลย, ถ้าเมล์ไม่มาก็เพราะว่า elmah config ไม่เรียบร้อย"
                Throw New Exception("Test message ทดสอบการส่งเมสเสจ ผ่าน elmah DLL จาก Localhost เท่านั้น, เมื่อ :" + Now.ToString)
            Else
                ErrorLabel.Text = " ไม่ได้เรียก จาก Localhost จะไม่สามารถใช้งานได้ , ต้อง remote ไปที่ server console แล้วค่อยเรียกเท่านั้น = " & HttpContext.Current.Request.UserHostAddress
            End If
        Catch ex As Exception
             Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
        End Try
    End Sub
    Private Function CheckIfRequestIsFromLocalMachine() As Boolean

        Dim clientAddress As String = HttpContext.Current.Request.UserHostAddress
 
        ' make sure the request is for localhost

        ' user host address of the user to determine where the call is made from

        Dim bReturn As Boolean = False

        If clientAddress = "::1" Or clientAddress = "127.0.0.1" Or clientAddress = "203.150.224.93" Then
            bReturn = True
        End If

        Return bReturn

    End Function
End Class