<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="LoginGenUser.aspx.vb" Inherits="QuickTestGenUserData.LoginGenUser" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div style="width: 100%; margin-left: auto; margin-right: auto">
       <table style="width: 50%; margin-left: auto; margin-right: auto; margin-top: 100px">
             <tr>
                            <td colspan="3" class="style7">
                                <asp:Label ID="txtvalidate" runat="server" Text="* ชื่อผู้ใช้หรือรหัสผ่านผิด ลองอีกครั้งนะคะ"
                                    Width="482px" ForeColor="#FF3300"></asp:Label>
                            </td>
                        </tr>
        <tr>
                            <td class="style3">
                                ชื่อผู้ใช้
                            </td>
                            <td class="style5" style="display: block;">
                                <asp:TextBox ID="txtusername" runat="server" class="conta ct" value="" Width="249px"></asp:TextBox>
                            </td>
                            <td class="style4" style="vertical-align: middle;">
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="TxtUserName"
                                    ErrorMessage="*" ForeColor="#FF3300" Display="Dynamic" Style="font-weight: 700"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="style3">
                                รหัสผ่าน
                            </td>
                            <td class="style5">
                                <asp:TextBox ID="txtpassword" runat="server" class="contact" value="" Width="249px"
                                    TextMode="Password" Text="cat"></asp:TextBox>
                            </td>
                            <td class="style4" style="vertical-align: middle;">
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="TxtUserName"
                                    ErrorMessage="*" ForeColor="#FF3300" Display="Dynamic" Style="font-weight: 700"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="style3">
                            </td>
                            <td colspan="2" class="style6">
                                <asp:Button ID="BtnSubmit" runat="server" Width="100px" Text="เข้าระบบ" class="submit" />
                            </td>
                        </tr>
           </table>
       </div>
    </form>
</body>
</html>
