<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="DialogConfirmSecond.aspx.vb"
    Inherits="QuickTest.DialogConfirmSecond" %>

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

        function returnToParent(IsOk) {
            //create the argument that will be returned to the parent page
            var oArg = new Object();

            //get value
            oArg.IsOk = IsOk;

            //get a reference to the current RadWindow
            var oWnd = GetRadWindow();

            //Close the RadWindow and send the argument to the parent page
            oWnd.close(oArg);
        }

    </script>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" Skin="Outlook" DecorationZoneID="p"
        DecoratedControls="All" />
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
                        &nbsp;</td>
                    <td style="text-align: right">
                        <button id="BtnOk" style="width: 100px" onclick="returnToParent('yes'); return false;">
                            ตกลง</button>
                    </td>
                </tr>
            </table>
        </fieldset>
    </div>
    </form>
</body>
</html>
