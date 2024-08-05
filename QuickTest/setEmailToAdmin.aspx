<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="setEmailToAdmin.aspx.vb"
    Inherits="QuickTest.setEmailToAdmin" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
    </telerik:RadScriptManager>
    <asp:LinkButton ID="lnkHome" runat="server">กลับเมนูหลัก</asp:LinkButton>
    <div>
        <table>
            <tr>
                <td>
                    Server
                </td>
                <td>
                    <telerik:RadTextBox ID="txtServerName" runat="server" Width="125px">
                    </telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Email
                </td>
                <td>
                    <telerik:RadTextBox ID="txtEmail" runat="server">
                    </telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    
                </td>
                <td>
                    <telerik:RadButton ID="btnSubmit" runat="server" Text="ยืนยัน" Width="80px">
                    </telerik:RadButton>
                    
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
