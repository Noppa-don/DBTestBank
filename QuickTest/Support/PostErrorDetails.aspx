<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="PostErrorDetails.aspx.vb" Inherits="QuickTest.PostErrorDetails" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        #DivContent {
        width:600px;height:500px;
        margin-left:auto;margin-right:auto;text-align:center;
        }
        table {
        width:100%;
        margin-left:auto;margin-right:auto;
        }
        td:first-child {
        text-align:right;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div id="DivContent">
            <h3>แจ้งปัญหา เหตุขัดข้อง</h3>

            <div id="DivInput">
                <table>
                    <tr>
                        <td>ชื่อผู้แจ้ง</td>
                        <td>
                            <asp:TextBox ID="txtName" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="txtName" runat="server" ForeColor="Red" ErrorMessage="*"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>เบอร์โทรกลับ</td>
                        <td>
                            <asp:TextBox ID="txtPhone" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="txtPhone" runat="server" ForeColor="Red" ErrorMessage="*"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>รายละเอียดปัญหาที่พบ</td>
                        <td>
                            <asp:TextBox ID="txtMessage" runat="server" MaxLength="5000" TextMode="MultiLine"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ControlToValidate="txtMessage" runat="server" ForeColor="Red" ErrorMessage="*"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                </table>
            </div>

        </div>
    </form>
</body>
</html>
