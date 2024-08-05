<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="UserManagerPage.aspx.vb" Inherits="QuickTest.UserManagerPage" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="../UserControl/LocalControl.ascx" TagName="LocalControl" TagPrefix="Lc" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
     
    <style type="text/css">
        .style2
        {
            width: 65px;
        }
        .style3
        {
            width: 81px;
        }
        .style4
        {
            width: 184px;
        }
        .style5
        {
            width: 1392px;
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
      <div>
     <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
    </telerik:RadScriptManager>
    <div>
        <telerik:RadWindowManager ID="RadWindowManager1" VisibleStatusbar="False" runat="server"
            Skin="Outlook" EnableShadow="True">
            <Windows>
            <telerik:RadWindow ID="RadWindow1" runat="server" Behaviors="Move" Modal="false"
                    Height="200" Width="500" EnableShadow="true" NavigateUrl="    ">
                </telerik:RadWindow>

                <telerik:RadWindow ID="RadDialogConfirmFirst" runat="server" Behaviors="Move" Modal="false"
                    Height="200" Width="500" EnableShadow="true">
                </telerik:RadWindow>
                <telerik:RadWindow ID="RadDialogConfirmSecond" runat="server"  Behaviors="Move" OnClientClose="OnClientCloseDelete"
                    Modal="false" Height="200" Width="500" EnableShadow="true">
                </telerik:RadWindow>
                <telerik:RadWindow ID="RadDialogAlert" runat="server" Behaviors="Move" Modal="false"
                    Height="200" Width="500" EnableShadow="true" VisibleOnPageLoad="false">
                </telerik:RadWindow>
            </Windows>
        </telerik:RadWindowManager>
        <table>
            <tr>
           
                <td>
                    <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="~/UserManager/UserManagerPage.aspx">ค้นหาข้อมูลผู้ใช้ </asp:HyperLink>
                </td> 

                <td style="text-align: right" colspan="2" class="style5">
                    <asp:LinkButton ID="lbtnHome" runat="server">กลับเมนูหลัก</asp:LinkButton>
                </td>
            </tr>
        </table>

        <table>
            <tr>
                <td>
                     <Lc:LocalControl ID="Lc1" runat="server" />
                </td>
            </tr>
        </table>

        <table style='width: 100%'>
            <tr>
            <td class="style3">
            เลือกโรงเรียน
            </td>
           
            <td class="style4">
                <telerik:RadComboBox ID="CmbSchool" runat="server">
                </telerik:RadComboBox>
            
            </td>
                <td class="style2">
                    ค้นหาผู้ใช้
                </td>
                <td>
                    <telerik:RadTextBox ID="txtName" runat="server" Width="200px">
                    </telerik:RadTextBox>
                </td>
                <td align="right">
                   <asp:LinkButton ID="lbAddUser" runat="server">เพิ่มผู้ใช้ใหม่</asp:LinkButton>
                </td>
               
            </tr>
        </table>
        <table width="100%">
            <tr>
                <td align="right" class="style1">
                    <telerik:RadButton ID="BtnFind" runat="server" Width="100" Text="ค้นหา" Skin="Outlook">
                    </telerik:RadButton>
                </td>
            </tr>
        </table>
        <div>
            <telerik:RadGrid ID="GvUser" runat="server" AutoGenerateColumns="False" AllowPaging="True"
                GridLines="None" PageSize="5000" AllowMultiRowSelection="true">
                <MasterTableView DataKeyNames="a" AllowSorting="true" AllowMultiColumnSorting="true"
                    NoMasterRecordsText="ไม่พบข้อมูล" HeaderStyle-ForeColor="Black">
                    <Columns>

                        <telerik:GridBoundColumn DataField="a" HeaderStyle-HorizontalAlign="Center" HeaderText="ลำดับ"
                            ItemStyle-Width="30" ItemStyle-HorizontalAlign="Center">
                            <HeaderStyle HorizontalAlign="Center" />
                        </telerik:GridBoundColumn>

                        <telerik:GridBoundColumn DataField="FirstName" HeaderStyle-HorizontalAlign="Center" HeaderText="ชื่อ"
                            ItemStyle-Width="600">
                            <HeaderStyle HorizontalAlign="Center" />
                        </telerik:GridBoundColumn>

                          <telerik:GridBoundColumn DataField="LastName" HeaderStyle-HorizontalAlign="Center" HeaderText="นามสกุล"
                            ItemStyle-Width="600">
                            <HeaderStyle HorizontalAlign="Center" />
                        </telerik:GridBoundColumn>

                          <telerik:GridBoundColumn DataField="UserName" HeaderStyle-HorizontalAlign="Center" HeaderText="ชื่อผู้ใช้"
                            ItemStyle-Width="600">
                            <HeaderStyle HorizontalAlign="Center" />
                        </telerik:GridBoundColumn>
                          
                                           <telerik:GridTemplateColumn UniqueName="EditColumn" HeaderText="แก้ไข" ItemStyle-Width="30px">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="BtnEdit" runat="server" ImageUrl="~/Images/freehand.png" ToolTip="แก้ไข"
                                                    CommandName="Update" CommandArgument='<%# eval("UserId") %>' />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="DeleteColumn" HeaderText="ลบ" ItemStyle-Width="50px">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="BtnDelete" runat="server" ImageUrl="~/Images/eraser.png" ToolTip="ลบ"
                                                    CommandName="Delete" CommandArgument='<%# eval("UserId") %>' />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        
                    </Columns>
                </MasterTableView>
                <HeaderContextMenu CssClass="GridContextMenu GridContextMenu_Default" EnableImageSprites="True">
                </HeaderContextMenu>
            </telerik:RadGrid>
        </div>
    </div>
    </div>
    </form>
</body>
</html>
