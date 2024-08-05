<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="PreviewAllLogPage.aspx.vb" Inherits="QuickTest.PreviewAllLogPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        div {
            margin: 30px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Label runat="server" Text="tblExternalApp"></asp:Label>
            <asp:GridView ID="GridtblExternallApp" runat="server"></asp:GridView>
        </div>
        <hr />

        <div>
            <asp:Label runat="server" Text="tblExternalAppStation"></asp:Label>
            <asp:GridView ID="GridtblExternalAppStation" runat="server"></asp:GridView>
        </div>
        <hr />

        <div>
            <asp:Label runat="server" Text="tblExternalLogAppStation"></asp:Label>
            <asp:GridView ID="GridtblExternalLogAppStation" runat="server"></asp:GridView>
        </div>
        <hr />

        <div>
            <asp:Label runat="server" Text="tblExternalLogApp"></asp:Label>
            <asp:GridView ID="GridtblExternalLogApp" runat="server"></asp:GridView>
        </div>
        <hr />

        <div>
            <asp:Label runat="server" Text="tblExternalLogAction"></asp:Label>
            <asp:GridView ID="GridtblExternalLogAction" runat="server"></asp:GridView>
        </div>
        <hr />

        <div>
            <asp:Label runat="server" Text="tblExternalAppQuestion"></asp:Label>
            <asp:GridView ID="GridtblExternalAppQuestion" runat="server"></asp:GridView>
        </div>
        <hr />

        <div>
            <asp:Label runat="server" Text="tblExternalAppAnswer"></asp:Label>
            <asp:GridView ID="GridtblExternalAppAnswer" runat="server"></asp:GridView>
        </div>
        <hr />

    </form>
</body>
</html>
