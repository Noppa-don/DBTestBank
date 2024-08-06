Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.HtmlControls
Imports System.IO
Imports KnowledgeUtils.System

Namespace Web

    Public Interface IWebUserControl

        WriteOnly Property Bind() As IEnumerable
        Function RenderUserControl(ByVal Properties As ListProperty) As String

    End Interface

    ''' <summary>
    ''' คลาสช่วยจัดการ User Control ของ Web
    ''' </summary>
    ''' <remarks></remarks>
    Public Class WebUserControlManager

        ''' <summary>
        ''' ฟังชั่น Generate User Control ในรูปแบบ Html เหมาะกับการใช้สร้าง User Control เรียกผ่าน Service
        ''' </summary>
        ''' <param name="UserControlPath"></param>
        ''' <param name="Properties"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function CreateUserControl(UserControlPath As String, Properties As ListProperty) As String
            Using Page As New Page
                Dim Uc = Page.LoadControl(UserControlPath)

                If Uc IsNot Nothing Then
                    Dim TypeUc As Type = Uc.GetType
                    For Each p In Properties
                        Dim Pt = TypeUc.GetProperty(p.PropertyName)
                        If Pt IsNot Nothing Then
                            Pt.SetValue(Uc, p.PropertyValue, Nothing)
                        End If
                    Next

                    Dim Hf As New HtmlForm
                    Using writer As New StringWriter()
                        Hf.Controls.Add(Uc)
                        Page.Controls.Add(Hf)
                        HttpContext.Current.Server.Execute(Page, writer, False)
                        Return writer.ToString()
                    End Using
                Else
                    Return Nothing
                End If
            End Using
        End Function

    End Class

End Namespace




