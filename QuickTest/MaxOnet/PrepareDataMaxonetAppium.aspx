<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="PrepareDataMaxonetAppium.aspx.vb" Inherits="QuickTest.PrepareDataMaxonetAppium" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <asp:Label ID="lblDeviceName" Text="Device Name = " runat="server"></asp:Label><asp:TextBox ID="txtDeviceName" runat="server" ></asp:TextBox><br /><br />
        <asp:Label ID="lblKeyName" Text="KeyCode Name = " runat="server"></asp:Label><asp:TextBox ID="txtKeyCodeName" runat="server"></asp:TextBox><br /><br />
        <asp:Button ID="btnOk" runat="server" Text="OK" />
    </div>
    </form>
</body>
</html>
