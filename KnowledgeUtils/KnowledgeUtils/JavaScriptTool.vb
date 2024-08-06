Imports System.Web.UI
Imports System.Text

Namespace Web.JavaScript

    ''' <summary>
    ''' คลาสจัดการ JavaScript
    ''' </summary>
    ''' <remarks></remarks>
    Public Class JavaScriptTool

        ''' <summary>
        ''' ใช้แสดง Message Box
        ''' </summary>
        ''' <param name="Page">หน้า UI</param>
        ''' <param name="Word">ข้อความที่ใช้แสดง</param>
        ''' <remarks></remarks>
        Public Shared Sub ScriptShowMsgBox(ByVal Page As Page, ByVal Word As String)
            Page.ClientScript.RegisterStartupScript(Page.GetType, "msg1", "alert('" & Word & "');", True)
        End Sub

        ''' <summary>
        ''' ใช้ทำให้ปุ่มมีการกดยืนยันก่อนทำงาน
        ''' </summary>
        ''' <param name="Button">ปุ่มที่จะให้ทำงาน</param>
        ''' <param name="Word">ข้อความที่ใช้แสดง</param>
        ''' <remarks></remarks>
        Public Shared Sub ScriptConfirmBox(ByVal Button As WebControls.Button, ByVal Word As String)
            Button.Attributes("OnClick") = "return confirm('" & Word & "?')"
        End Sub

        ''' <summary>
        ''' สร้าง javascript เผื่อ ระบุค่าให้กับคอนโทรนที่ต้องการ
        ''' </summary>
        ''' <param name="Page">Page ที่ ControlName อยู่</param>
        ''' <param name="ControlName">ชื่อ Control ที่จะระบุค่าให้ในฟังชั้นจะแปลงเป็นชื่อ ClientID เอง</param>
        ''' <param name="JavaName">ชื่อ Function Javascript</param>
        ''' <remarks></remarks>
        Public Shared Sub ScriptAssignValueByEvent(ByVal Page As Page, ByVal ControlName As String, ByVal JavaName As String)
            Dim C As Control = Page.FindDeepControl(ControlName)
            Dim Sb As New StringBuilder
            Sb.Append("<script type=""text/javascript"">" & ControlChars.CrLf)
            Sb.Append("function " & JavaName & "(s, e) {" & ControlChars.CrLf)
            Sb.Append("document.getElementById('" & C.ClientID & "').value = e.get_value();" & ControlChars.CrLf)
            Sb.Append("}" & ControlChars.CrLf & "</script>")

            Page.ClientScript.RegisterClientScriptBlock(Page.GetType, JavaName, Sb.ToString, False)
        End Sub

    End Class

End Namespace



