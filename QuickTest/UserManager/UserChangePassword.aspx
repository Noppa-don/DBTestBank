<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="UserChangePassword.aspx.vb"
    Inherits="QuickTest.UserChangePassword" %>

<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
    <title></title>
    
    <style type="text/css">
        .body
        {
            padding: "10px" font-family: "Arial" Font-size: "10pt";
        }
    </style>
    
</head>
<body>
    <form id="form1" runat="server">
    <script type="text/javascript" src="../Scripts/jquery-1.4.1.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
//            $('#txtOldPassword').blur(function () {
//                if (validatePassword('txtOldPassword')) {
//                    //alert('รหัสถูกต้อง');
//                }
//                else { alert('รหัสผ่านเดิมไม่ถูกต้อง'); }
//            });

            $("#txtNewPasswordConfirm").blur(function () {
                if (validateNewPassword('txtNewPasswordConfirm')) {
                    //alert('รหัสถูกต้อง');
                }
                else { alert('กรุณากรอกรหัสผ่านใหม่ให้เหมือนกัน'); }
            });
        });

        function validatePassword(txtOldPassword) {
//            var oldPasswordInTxt = document.getElementById("txtOldPassword").value;
//            var oldPasswordInDB = "<%=getPassword()%>";  // ถ้า error ใช้ระหว่าง # กับ =
//            if (oldPasswordInDB == oldPasswordInTxt) { return true; }
//            else { return false }
        }

        function validateNewPassword(txtNewPassword) {
            var newPassword = document.getElementById("txtNewPassword").value;
            var confirmNewPassword = document.getElementById("txtNewPasswordConfirm").value;
            if (newPassword == confirmNewPassword) { return true; }
            else { return false }
        }
    </script>

    
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
    </telerik:RadScriptManager>
    <asp:LinkButton ID="lbtnHome" runat="server">กลับเมนูหลัก</asp:LinkButton>
    <div style="text-align: left">
        <table>
            <tr>
                <td>
                    ชื่อผู้ใช้
                </td>
                <td>
                    <asp:Label ID="lblUserName" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    รหัสโรงเรียน
                </td>
                <td>
                    <asp:Label ID="lblSchoolID" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    รหัสผ่านเดิม
                </td>
                <td>
                    <telerik:RadTextBox ID="txtOldPassword" Runat="server" TextMode="Password" 
                        Width="125px" >
                    </telerik:RadTextBox>
                    <asp:Label ID="lblCheck" runat="server" ForeColor="Red" 
                        Text="รหัสผ่านเดิมไม่ถูกต้อง" Visible="False"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    รหัสผ่านใหม่</td>
                <td>
                    <telerik:RadTextBox ID="txtNewPassword" Runat="server" TextMode="Password">
                    </telerik:RadTextBox>
                    <asp:Label ID="lblCheckPassword" runat="server" ForeColor="Red" 
                        Text="กรุณากรอกรหัส" Visible="False"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    ยืนยัน</td>
                <td>
                    <telerik:RadTextBox ID="txtNewPasswordConfirm" Runat="server" 
                        TextMode="Password">
                    </telerik:RadTextBox>
                    <asp:CompareValidator ID="CompareValidator1" runat="server" 
                        ControlToCompare="txtNewPassword" ControlToValidate="txtNewPasswordConfirm" 
                        ErrorMessage="กรุณากรอกรหัสผ่านให้เหมือนกัน" ForeColor="Red"></asp:CompareValidator>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;</td>
                <td>
                    <telerik:RadButton ID="btnConfirm" runat="server" Text="ตกลง" Width="80px">
                    </telerik:RadButton>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
