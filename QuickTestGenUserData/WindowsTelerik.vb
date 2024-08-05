Imports System.Web.UI

Public Class WindowsTelerik

    Public Shared Sub ShowAlert(ByVal Msg As String, CurrentPage As System.Web.UI.Page, DialogAlert As Telerik.Web.UI.RadWindow)
        Dim script As String = "function callShowAlert(){ " & _
                               "var oWnd = $find(""" + DialogAlert.ClientID + """); " & _
                               "oWnd.setUrl('" & HttpContext.Current.Request.ApplicationPath & "/Common/DialogAlert.aspx?msg=" + Msg + "' ); " & _
                               "oWnd.show(); " & _
                               "Sys.Application.remove_load(callShowAlert);} " & _
                               "Sys.Application.add_load(callShowAlert);"
        ScriptManager.RegisterStartupScript(CurrentPage, CurrentPage.GetType(), "key", Script, True)
    End Sub

    Public Shared Sub ShowAlertLong(ByVal Msg As String, CurrentPage As System.Web.UI.Page, DialogAlert As Telerik.Web.UI.RadWindow)
        Dim script As String = "function callShowAlertLong(){ " & _
                               "var oWnd = $find(""" + DialogAlert.ClientID + """); " & _
                               "oWnd.setUrl('" & HttpContext.Current.Request.ApplicationPath & "/Common/DialogLongMsg.aspx?msg=" + Msg + "' ); " & _
                               "oWnd.show(); " & _
                               "Sys.Application.remove_load(callShowAlertLong);} " & _
                               "Sys.Application.add_load(callShowAlertLong);"
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
        Dim script As String = "function callShowConfirmSingle(){ " & _
                               "var oWnd = $find(""" + DialogConfirm.ClientID + """); " & _
                               "oWnd.setUrl('" & HttpContext.Current.Request.ApplicationPath & "/Common/DialogMsg.aspx?msg=" + Msg + "' ); " & _
                               "oWnd.show(); " & _
                               "Sys.Application.remove_load(callShowConfirmSingle);} " & _
                               "Sys.Application.add_load(callShowConfirmSingle);"
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
        Dim script As String = "function callShowConfirmDouble(){ " & _
                               "var oWnd = $find(""" + DialogConfirmFirst.ClientID + """); " & _
                               "oWnd.setUrl('" & HttpContext.Current.Request.ApplicationPath & "/Common/DialogConfirmFirst.aspx?msg=" + Msg + "' ); " & _
                               "oWnd.show(); " & _
                               "Sys.Application.remove_load(callShowConfirmDouble);} " & _
                               "Sys.Application.add_load(callShowConfirmDouble);"
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
        Dim script As String = "function callDialogInput(){ " & _
                               "var oWnd = $find(""" + DialogInput.ClientID + """); " & _
                               "oWnd.setUrl('" & HttpContext.Current.Request.ApplicationPath & "/Common/DialogInput.aspx?msg=" + Msg + "' ); " & _
                               "oWnd.show(); " & _
                               "Sys.Application.remove_load(callDialogInput);} " & _
                               "Sys.Application.add_load(callDialogInput);"
        ScriptManager.RegisterStartupScript(CurrentPage, CurrentPage.GetType(), "key", script, True)
    End Sub

    ''' <summary>
    ''' Dialog หน้า tabletdesk
    ''' </summary>
    ''' <param name="Msg"></param>
    ''' <param name="CurrentPage"></param>
    ''' <param name="DialogTabletDesk"></param>
    ''' <remarks></remarks>
    Public Shared Sub ShowTabletDesk(ByVal Msg As String, ByVal CurrentPage As System.Web.UI.Page, ByVal DialogTabletDesk As Telerik.Web.UI.RadWindow)
        Dim script As String = "function callDialogTabletDesk(){ " & _
                               "var oWnd = $find(""" + DialogTabletDesk.ClientID + """); " & _
                               "oWnd.setUrl('" & HttpContext.Current.Request.ApplicationPath & "/Common/DialogTabletDesk.aspx?msg=" + Msg + "' ); " & _
                               "oWnd.show(); " & _
                               "Sys.Application.remove_load(callDialogTabletDesk);} " & _
                               "Sys.Application.add_load(callDialogTabletDesk);"
        ScriptManager.RegisterStartupScript(CurrentPage, CurrentPage.GetType(), "key", script, True)
    End Sub

    ''' <summary>
    ''' Dialog ของการเลือกห้องครู
    ''' </summary>
    ''' <param name="Msg">roomid ต่อกันด้วย ","</param>
    ''' <param name="CurrentPage"></param>
    ''' <param name="DialogTeacherRoom"></param>
    ''' <remarks></remarks>
    Public Shared Sub ShowTeacherRoom(ByVal Msg As String, ByVal CurrentPage As System.Web.UI.Page, ByVal DialogTeacherRoom As Telerik.Web.UI.RadWindow)
        Dim script As String = "function callShowTeacherRoom(){ " & _
                               "var oWnd = $find(""" + DialogTeacherRoom.ClientID + """); " & _
                               "oWnd.setUrl('" & HttpContext.Current.Request.ApplicationPath & "/Common/DialogTeacherRoom.aspx?msg=" + Msg + "' ); " & _
                               "oWnd.show(); " & _
                               "Sys.Application.remove_load(callShowTeacherRoom);} " & _
                               "Sys.Application.add_load(callShowTeacherRoom);"
        ScriptManager.RegisterStartupScript(CurrentPage, CurrentPage.GetType(), "key", script, True)
    End Sub

    ''' <summary>
    ''' Dialog ของการเลือกครูหลัก
    ''' </summary>
    ''' <param name="Msg"></param>
    ''' <param name="CurrentPage"></param>
    ''' <param name="DialogTeacherRoom"></param>
    ''' <remarks></remarks>
    Public Shared Sub ShowTeacherTeacher(ByVal Msg As String, ByVal CurrentPage As System.Web.UI.Page, ByVal DialogTeacherRoom As Telerik.Web.UI.RadWindow)
        Dim script As String = "function callShowTeacherTeacher(){ " & _
                               "var oWnd = $find(""" + DialogTeacherRoom.ClientID + """); " & _
                               "oWnd.setUrl('" & HttpContext.Current.Request.ApplicationPath & "/Common/DialogTeacherAssistant.aspx?msg=" + Msg + "' ); " & _
                               "oWnd.show(); " & _
                               "Sys.Application.remove_load(callShowTeacherTeacher);} " & _
                               "Sys.Application.add_load(callShowTeacherTeacher);"
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
                               "oWnd.setUrl('" & HttpContext.Current.Request.ApplicationPath & "/Common/DialogTwoInput.aspx?msg=" + Msg + "' ); " & _
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
                                       "oWnd.setUrl('" & HttpContext.Current.Request.ApplicationPath & "/Common/DialogYearToYear.aspx' ); " & _
                                       "oWnd.show(); " & _
                                       "Sys.Application.remove_load(f);} " & _
                                       "Sys.Application.add_load(f);"
        ScriptManager.RegisterStartupScript(CurrentPage, CurrentPage.GetType(), "key", script, True)
    End Sub

    Public Shared Sub ShowTabletStatus(ByVal Msg As String, ByVal CurrentPage As System.Web.UI.Page, ByVal RadTabletStatus As Telerik.Web.UI.RadWindow)
        Dim script As String = "function f(){ " & _
                                       "var oWnd = $find(""" + RadTabletStatus.ClientID + """); " & _
                                       "oWnd.setUrl('" & HttpContext.Current.Request.ApplicationPath & "/Common/DialogTabletStatus.aspx' ); " & _
                                       "oWnd.show(); " & _
                                       "Sys.Application.remove_load(f);} " & _
                                       "Sys.Application.add_load(f);"
        ScriptManager.RegisterStartupScript(CurrentPage, CurrentPage.GetType(), "key", script, True)
    End Sub

    Public Shared Sub ShowTabletLost(ByVal Msg As String, ByVal CurrentPage As System.Web.UI.Page, ByVal RadTabletLost As Telerik.Web.UI.RadWindow, ByVal Id As Guid)
        Dim script As String = "function f(){ " & _
                                       "var oWnd = $find(""" + RadTabletLost.ClientID + """); " & _
                                       "oWnd.setUrl('" & HttpContext.Current.Request.ApplicationPath & "/Common/DialogTabletLost.aspx?id=" & Id.ToString & "'); " & _
                                       "oWnd.show(); " & _
                                       "Sys.Application.remove_load(f);} " & _
                                       "Sys.Application.add_load(f);"
        ScriptManager.RegisterStartupScript(CurrentPage, CurrentPage.GetType(), "key", script, True)
    End Sub

    Public Shared Sub ShowTabletRepair(ByVal Msg As String, ByVal CurrentPage As System.Web.UI.Page, ByVal RadTabletRepair As Telerik.Web.UI.RadWindow, ByVal Id As Guid)
        Dim script As String = "function f(){ " & _
                                       "var oWnd = $find(""" + RadTabletRepair.ClientID + """); " & _
                                       "oWnd.setUrl('" & HttpContext.Current.Request.ApplicationPath & "/Common/DialogTabletRepair.aspx?id=" & Id.ToString & "'); " & _
                                       "oWnd.show(); " & _
                                       "Sys.Application.remove_load(f);} " & _
                                       "Sys.Application.add_load(f);"
        ScriptManager.RegisterStartupScript(CurrentPage, CurrentPage.GetType(), "key", script, True)
    End Sub

    Public Shared Sub ShowConfirmGenPasswordMobile(ByVal Msg As String, ByVal CurrentPage As System.Web.UI.Page, ByVal RadDialog As Telerik.Web.UI.RadWindow)
        Dim script As String = "function f(){ " & _
                               "var oWnd = $find(""" + RadDialog.ClientID + """); " & _
                               "oWnd.setUrl('" & HttpContext.Current.Request.ApplicationPath & "/Common/DialogMobileGenPassword.aspx?msg=" + Msg + "' ); " & _
                               "oWnd.show(); " & _
                               "Sys.Application.remove_load(f);} " & _
                               "Sys.Application.add_load(f);"
        ScriptManager.RegisterStartupScript(CurrentPage, CurrentPage.GetType(), "key", script, True)
    End Sub

    Public Shared Sub ShowSelectRunNo(ByVal CurrentPage As System.Web.UI.Page, ByVal DialogInput As Telerik.Web.UI.RadWindow)
        Dim script As String = "function callDialogSelectRunNo(){ " & _
                               "var oWnd = $find(""" + DialogInput.ClientID + """); " & _
                               "oWnd.setUrl('" & HttpContext.Current.Request.ApplicationPath & "/Common/DialogSelectRunNo.aspx' ); " & _
                               "oWnd.show(); " & _
                               "Sys.Application.remove_load(callDialogSelectRunNo);} " & _
                               "Sys.Application.add_load(callDialogSelectRunNo);"
        ScriptManager.RegisterStartupScript(CurrentPage, CurrentPage.GetType(), "key", script, True)
    End Sub

End Class
