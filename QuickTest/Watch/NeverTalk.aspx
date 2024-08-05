<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="NeverTalk.aspx.vb" Inherits="QuickTest.NeverTalk" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        html, body, form {
            width: 100%;
            height: 100%;
            margin:0;
        }

        .warning {
            width: 80%;
            height: 60%;
            margin:10px auto 0;
            background-color: red;
            text-align:center;
            font-size:30px;
        }

        .refresh {
            text-align: center;
            margin-top: 20px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="warning">
            ยังไม่มีการพูดคุยกับครูในเทอมนี้ค่ะ
        </div>
        <div class="refresh">
            <asp:Button ID="btnRefresh" runat="server" Text="Refresh" />
        </div>
    </form>
</body>
</html>
