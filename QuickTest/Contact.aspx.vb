Imports System.Net
Imports System.Net.Mail


Public Class Contact
    Inherits System.Web.UI.Page
    Dim conDB As New ClassConnectSql

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        radioMail.Checked = True
    End Sub

    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSubmit.Click


        ' request dropdownlist
        Dim title As String = Request.Form("dropdown")
        If title = 1 Then
            title = "เข้าไม่ได้"
        ElseIf title = 2 Then
            title = "เลือกข้อสอบไม่ได้"
        ElseIf title = 3 Then
            title = "พิมพ์ออกไม่ได้ ตกขอบกระดาษ"
        ElseIf title = 4 Then
            title = "สับสน ไม่เข้าใจว่าใช้งานยังไง"
        ElseIf title = 5 Then
            title = "บางจุดกดแล้วไม่ทำงาน หรือ น่าจะเสีย"
        ElseIf title = 6 Then
            title = "แนะนำเพิ่มเติม เสนอแนะไอเดีย"
        ElseIf title = 7 Then
            title = "อยากใช้บ้างต้องทำอย่างไร"
        ElseIf title = 8 Then
            title = "ลืมรหัสผ่าน"
        ElseIf title = 9 Then
            title = "เรื่องอื่นๆ"
        Else
            title = "ไม่ได้ระบุ"
        End If

        ' request textbox
        Dim description As String = conDB.CleanString(Request.Form("descript"))
        Dim nameTel As String = conDB.CleanString(Request.Form("descriptName"))
        Dim email As String = conDB.CleanString(Request.Form("Email"))
        Dim Tel As String = conDB.CleanString(Request.Form("Tel"))

        If description <> "" And nameTel <> "" And (email <> "" Or Tel <> "") Then

            ' request checkbox
            Dim Istel As Boolean = radioTel.Checked

            ' insert to table
            Dim sql As String = "INSERT INTO tblContact(Title,Description,Tel,Email,IsTel) VALUES ('" & title & "','" & description & "','" & nameTel & "','" & email & "','" & Istel.ToString & "');"
            conDB.Execute(sql)

            'send email
            Dim strBody As String = "สอบถามเกี่ยวกับเรื่อง : " & title & "   รายละเอียด  :  " & description & "    ชื่อ : " & nameTel & "    อีเมล์ : " & email & "    โทร : " & Tel

            If Istel Then
                strBody &= "    สะดวกให้ติดต่อกลับทางโทรศัพท์"
            Else
                strBody &= "    สะดวกให้ติดต่อกลับทางอีเมล์"
            End If

            Dim clsTS As New ClsTestSet("")
            If clsTS.sendEmailToAdmin("แจ้งปัญหาการเข้าใช้งาน 2Tests", strBody) Then
                DisplayAlert("ส่งอีเมลล์เรียบร้อยค่ะ")
            Else
                lblError.Visible = True
            End If

        Else
            DisplayAlert("กรุณากรอกข้อมูลให้ครบก่อนนะคะ")
        End If
    End Sub

    Protected Overridable Sub DisplayAlert(message As String)
        ClientScript.RegisterStartupScript(Me.[GetType](), Guid.NewGuid().ToString(), String.Format("alert('{0}');window.location.href = 'contact.aspx'", message.Replace("'", "\'").Replace(vbLf, "\n").Replace(vbCr, "\r")), True)
    End Sub




End Class