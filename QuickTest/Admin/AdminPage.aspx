<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="AdminPage.aspx.vb" Inherits="QuickTest.AdminPage1" %>
<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
     <style type="text/css">
        .style1
        {
            width: 100%;
        }
        .style2
        {
            width: 183px;
        }
        .style3
        {
            text-align: right;
        }
        .style4
        {
            width: 63px;
        }
        .style5
        {
            background-color: #CCCCCC;
        }
        .style6
        {
            width: 63px;
            background-color: #FFFFFF;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <script type="text/javascript">
        function OnClientCloseDelete(s, e) {
            //get the transferred arguments
            var arg = e.get_argument();
            if (arg.IsOk == 'yes') {
                var pageId = '<%= Page.ClientID %>';
                __doPostBack(pageId, "delete");
            }
        }

    </script>
     <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
    </telerik:RadScriptManager>
    <div>
    <telerik:RadWindowManager ID="RadWindowManager1" VisibleStatusbar="false" runat="server"
                    Skin="Outlook" EnableShadow="true">
                    <Windows>
                        <telerik:RadWindow ID="RadDialogConfirmFirst" runat="server" Behaviors="move" Modal="false"
                            Height="200" Width="500" EnableShadow="true">
                        </telerik:RadWindow>
                        <telerik:RadWindow ID="RadDialogConfirmSecond" runat="server" Behaviors="move" OnClientClose="OnClientCloseDelete"
                            Modal="false" Height="200" Width="500" EnableShadow="true">
                        </telerik:RadWindow>
                        <telerik:RadWindow ID="RadDialogAlert" runat="server" Behaviors="move" Modal="false"
                            Height="200" Width="500" EnableShadow="true" VisibleOnPageLoad="false">
                        </telerik:RadWindow>
                    </Windows>
                </telerik:RadWindowManager>
        <table class="style1">
            <tr>
                <td colspan="5">
                    <asp:LinkButton ID="lbtnAdminManagePage" runat="server">ค้นหาข้อมูลผู้ใช้ &gt;</asp:LinkButton>
                    เพิ่ม/ แก้ไขข้อมูลผู้ใช้<br />
                    <br />
                </td>
            </tr>
            <tr>
                <td colspan="3" class="style5" style="text-align: center">
                    กรอกข้อมูลผู้ใช้งาน</td>
                                     <td class="style6">
                    &nbsp;
                </td>
                <td class="style5" style="text-align: center">
                    หน้าจอที่ใช้ได้</td>

            </tr>
            <tr>
                <td class="style3">
                    ชื่อ</td>
                <td class="style2">
                    <telerik:RadTextBox ID="txtFirstName" Runat="server" Width="200px">
                    </telerik:RadTextBox>
                </td>
                <td>
                        <asp:RequiredFieldValidator ID="ChkFirstName" runat="server" 
                            ControlToValidate="txtFirstName" ErrorMessage="*" ForeColor="#FF3300" 
                            Display="Dynamic"></asp:RequiredFieldValidator>
               </td>
                               <td class="style4">
                    &nbsp;
                </td>
                <td>
                    <asp:CheckBox ID="cbManageUserShool" runat="server" 
                        Text="เมนูจัดการข้อมูลผู้ใช้(โรงเรียน)" />
                    </td>
            </tr>
            <tr>
                <td class="style3">
                    นามสกุล</td>
                <td class="style2">
                    <telerik:RadTextBox ID="txtLastName" Runat="server" Width="200px">
                    </telerik:RadTextBox>
                </td>
                <td>
                    &nbsp;</td>
                 <td class="style4">
                    &nbsp;
                </td>
                <td>
                    <asp:CheckBox ID="cbManageUserAdmin" runat="server" 
                        Text="เมนูจัดการข้อมูลผู้ใช้(Admin)" /></td>
            </tr>
            <tr>
                <td class="style3">
                    ชื่อผู้ใช้</td>
                <td class="style2">
                    <telerik:RadTextBox ID="txtUserName" Runat="server">
                    </telerik:RadTextBox>
                </td>
                <td>
                   <asp:RequiredFieldValidator ID="ChkUsername" runat="server" 
                            ControlToValidate="txtUsername" ErrorMessage="*" ForeColor="#FF3300" 
                            Display="Dynamic"></asp:RequiredFieldValidator></td>
                <td class="style4">
                    &nbsp;</td>
                                    
                 <td>
                  <asp:CheckBox ID="cbAdminLog" runat="server" Text="เมนูดูข้อมูลการใช้งาน" />
                </td>
            </tr>
            <tr>
                <td class="style3">
                    รหัสผ่าน</td>
                <td class="style2">
                    <telerik:RadTextBox ID="txtPassword" Runat="server" >
                    </telerik:RadTextBox>
                </td>
                <td>
                   <asp:RequiredFieldValidator ID="ChkPassword" runat="server" 
                            ControlToValidate="txtPassword" ErrorMessage="*" ForeColor="#FF3300"></asp:RequiredFieldValidator>
                </td>
                <td class="style4">
                    &nbsp;</td>
                 <td>
                    <asp:CheckBox ID="cbAdminContact" runat="server" Text="เมนูคำถามเพิ่มเติม" />
                </td>
            </tr>
            <tr>
                <td class="style3">
                    ยืนยันรหัสผ่าน</td>
                <td class="style2">
                    <telerik:RadTextBox ID="txtCPassword" Runat="server">
                    </telerik:RadTextBox>
                </td>
                <td>
                   <asp:RequiredFieldValidator ID="ChkCPassword" runat="server" 
                            ControlToValidate="txtCPassword" ErrorMessage="*" ForeColor="#FF3300"></asp:RequiredFieldValidator>
                </td>
                <td class="style4">
                    &nbsp;</td>
                 <td>
                   <asp:CheckBox ID="cbSetEmail" runat="server" Text="ตั้งค่าEmailรับข้อความจากหน้าเว็บ" />
                </td>
            </tr>
                        <tr>
                <td class="style3" colspan="5">
                    <br />
                    <br />
                    <br />
                    <asp:Button ID="btnSave" runat="server" Text="บันทึก" Width="79px" />
                            </td>
            </tr>
        </table>
    
    </div>
    </form>
</body>
</html>
