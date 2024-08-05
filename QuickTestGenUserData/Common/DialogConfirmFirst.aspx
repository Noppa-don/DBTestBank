<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="DialogConfirmFirst.aspx.vb"
    Inherits="QuickTestGenUserData.DialogConfirmFirst" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
    <script type="text/javascript">

        function GetRadWindow() {
            var oWindow = null;
            if (window.radWindow) oWindow = window.radWindow;
            else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
            return oWindow;
        }

        function openWin2() {
            var parentPage = GetRadWindow().BrowserWindow;
            var parentRadWindowManager = parentPage.GetRadWindowManager();
            var msg2 = document.getElementById("<%=HfMsg2.ClientID %>").value;
            var oWnd2 = parentRadWindowManager.open("../common/DialogConfirmSecond.aspx?msg=" + msg2 , "RadDialogConfirmSecond");
            window.setTimeout(function () {
                oWnd2.setActive(true);
            }, 0);
            closeWindows();
        }

        function closeWindows() {
            var oWnd = GetRadWindow();
            oWnd.close();
        }

    </script>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" Skin="Outlook" DecorationZoneID="p"
        DecoratedControls="All" />
    <asp:HiddenField ID="HfMsg2" runat="server" />
    <asp:HiddenField ID="HfId" runat="server" />
    <div>
        <fieldset id="p" style="height: 100px">
            <table width="100%">
                <tr>
                    <td colspan="2">
                        <div id="PartMsg" runat="server" style="display: block; width: 420px">
                        </div>
                        <br />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: left">
                        <button id="BtnCancel" style="width: 100px" onclick="closeWindows(); ">
                            ยกเลิก</button>
                    </td>
                    <td style="text-align: right">
                        <button id="BtnOk" style="width: 100px" onclick="openWin2(); return false;">
                            แน่ใจ</button>
                        <%--<button id="BtnOk" style="width: 100px" onclick="OpenDialogConfirm2">
                            ตกลง</button>--%>
                    </td>
                </tr>
            </table>
        </fieldset>
    </div>
    </form>
</body>
</html>
