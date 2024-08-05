
Public Class WindowsTelerik

    Public Shared Sub ShowAlert(ByVal Msg As String, CurrentPage As System.Web.UI.Page, DialogAlert As Telerik.Web.UI.RadWindow)
        Dim script As String = "function f(){ " & _
                               "var oWnd = $find(""" + DialogAlert.ClientID + """); " & _
                               "oWnd.setUrl('../common/DialogAlert.aspx?msg=" + Msg + "' ); " & _
                               "oWnd.show(); " & _
                               "Sys.Application.remove_load(f);} " & _
                               "Sys.Application.add_load(f);"
        ScriptManager.RegisterStartupScript(CurrentPage, CurrentPage.GetType(), "key", script, True)
    End Sub

    ''' <summary>
    ''' โชว์ข้อความถามรอบเดียว ควรส่ง Msg อันเดียว
    ''' </summary>
    ''' <param name="Msg"></param>
    ''' <param name="CurrentPage"></param>
    ''' <param name="DialogConfirm"></param>
    ''' <remarks></remarks>
    Public Shared Sub ShowConfirmSingle(ByVal Msg As String, ByVal CurrentPage As System.Web.UI.Page, ByVal DialogConfirm As Telerik.Web.UI.RadWindow)
        Dim script As String = "function f(){ " & _
                               "var oWnd = $find(""" + DialogConfirm.ClientID + """); " & _
                               "oWnd.setUrl('../common/DialogMsg.aspx?msg=" + Msg + "' ); " & _
                               "oWnd.show(); " & _
                               "Sys.Application.remove_load(f);} " & _
                               "Sys.Application.add_load(f);"
        ScriptManager.RegisterStartupScript(CurrentPage, CurrentPage.GetType(), "key", script, True)
    End Sub

    ''' <summary>
    ''' โชว์ข้อความถามสองรอบ ควรส่ง Msg สองข้อความ โดยเชื่อมด้วย ","
    ''' </summary>
    ''' <param name="Msg"></param>
    ''' <param name="CurrentPage"></param>
    ''' <param name="DialogConfirmFirst"></param>
    ''' <remarks></remarks>
    Public Shared Sub ShowConfirmDouble(ByVal Msg As String, ByVal CurrentPage As System.Web.UI.Page, ByVal DialogConfirmFirst As Telerik.Web.UI.RadWindow)
        Dim script As String = "function f(){ " & _
                               "var oWnd = $find(""" + DialogConfirmFirst.ClientID + """); " & _
                               "oWnd.setUrl('../common/DialogConfirmFirst.aspx?msg=" + Msg + "' ); " & _
                               "oWnd.show(); " & _
                               "Sys.Application.remove_load(f);} " & _
                               "Sys.Application.add_load(f);"
        ScriptManager.RegisterStartupScript(CurrentPage, CurrentPage.GetType(), "key", script, True)
    End Sub

    ''' <summary>
    ''' โชว์หน้าต่างเล็กๆไว้เก็บ Input กลับมา ควรส่ง msg 3 parameter 'title','label1','textbox1'
    ''' </summary>
    ''' <param name="Msg"></param>
    ''' <param name="CurrentPage"></param>
    ''' <param name="DialogInput"></param>
    ''' <remarks></remarks>
    Public Shared Sub ShowInput(ByVal Msg As String, ByVal CurrentPage As System.Web.UI.Page, ByVal DialogInput As Telerik.Web.UI.RadWindow)
        Dim script As String = "function f(){ " & _
                               "var oWnd = $find(""" + DialogInput.ClientID + """); " & _
                               "oWnd.setUrl('../common/DialogInput.aspx?msg=" + Msg + "' ); " & _
                               "oWnd.show(); " & _
                               "Sys.Application.remove_load(f);} " & _
                               "Sys.Application.add_load(f);"
        ScriptManager.RegisterStartupScript(CurrentPage, CurrentPage.GetType(), "key", script, True)
    End Sub

    ''' <summary>
    ''' โชว์หน้าต่างเล็กๆไว้เก็บ Input สอง Input กลับมา ควรส่ง msg 5 parameter 'title','label1','textbox1','label2','textbox2'
    ''' </summary>
    ''' <param name="Msg"></param>
    ''' <param name="CurrentPage"></param>
    ''' <param name="DialogInput"></param>
    ''' <remarks></remarks>
    Public Shared Sub ShowTwoInput(ByVal Msg As String, ByVal CurrentPage As System.Web.UI.Page, ByVal DialogInput As Telerik.Web.UI.RadWindow)
        Dim script As String = "function f(){ " & _
                               "var oWnd = $find(""" + DialogInput.ClientID + """); " & _
                               "oWnd.setUrl('../common/DialogTwoInput.aspx?msg=" + Msg + "' ); " & _
                               "oWnd.show(); " & _
                               "Sys.Application.remove_load(f);} " & _
                               "Sys.Application.add_load(f);"
        ScriptManager.RegisterStartupScript(CurrentPage, CurrentPage.GetType(), "key", script, True)
    End Sub

    ''' <summary>
    ''' โชว์หน้าเลือกปี ถึง ปี
    ''' </summary>
    ''' <param name="CurrentPage"></param>
    ''' <param name="DialogYearToYear"></param>
    ''' <remarks></remarks>
    Public Shared Sub ShowYearToYear(ByVal Msg As String, ByVal CurrentPage As System.Web.UI.Page, ByVal DialogYearToYear As Telerik.Web.UI.RadWindow)
        Dim script As String = "function f(){ " & _
                                       "var oWnd = $find(""" + DialogYearToYear.ClientID + """); " & _
                                       "oWnd.setUrl('../common/DialogYearToYear.aspx' ); " & _
                                       "oWnd.show(); " & _
                                       "Sys.Application.remove_load(f);} " & _
                                       "Sys.Application.add_load(f);"
        ScriptManager.RegisterStartupScript(CurrentPage, CurrentPage.GetType(), "key", script, True)
    End Sub

    Public Shared Sub ShowTabletStatus(ByVal Msg As String, ByVal CurrentPage As System.Web.UI.Page, ByVal RadTabletStatus As Telerik.Web.UI.RadWindow)
        Dim script As String = "function f(){ " & _
                                       "var oWnd = $find(""" + RadTabletStatus.ClientID + """); " & _
                                       "oWnd.setUrl('../common/DialogTabletStatus.aspx' ); " & _
                                       "oWnd.show(); " & _
                                       "Sys.Application.remove_load(f);} " & _
                                       "Sys.Application.add_load(f);"
        ScriptManager.RegisterStartupScript(CurrentPage, CurrentPage.GetType(), "key", script, True)
    End Sub

End Class
