<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="TestGenQuestion.aspx.vb" Inherits="QuickTest.TestGenQuestion" %>

<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>
<%@ Register assembly="CrystalDecisions.Web, Version=12.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager> 
    <div>
        <asp:Button ID="btnGenPDF" runat="server" Text="สร้างข้อสอบ" 
            style="height: 26px" />
    
        <asp:Button ID="btnGenKey" runat="server" Text="สร้างเฉลย" 
            style="height: 26px" />
    
    </div>
    </form>
</body>
</html>
