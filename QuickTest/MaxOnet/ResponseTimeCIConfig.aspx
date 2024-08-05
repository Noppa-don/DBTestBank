<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ResponseTimeCIConfig.aspx.vb" Inherits="QuickTest.ResponseTimeConfig" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Label ID="lblResponseTime" runat="server" ></asp:Label>
            <br />
            <asp:Button ID="btnTimeDefault" runat="server" Text="Default" />
            <asp:Button ID="btnTimeThirty" runat="server" Text="30 Secounds" />
            <asp:Button ID="btnTimeSixty" runat="server" Text="60 Secounts" />
        </div>
    </form>
</body>
</html>
