<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="AdminManagerPage.aspx.vb" Inherits="QuickTest.AdminManagerPage" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">


<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

       <style type="text/css">
        .style1
        {
            width: 100%;
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
        <telerik:RadWindowManager ID="RadWindowManager1" VisibleStatusbar="False" runat="server"
            Skin="Outlook" EnableShadow="True">
            <Windows>
                <telerik:RadWindow ID="RadDialogConfirmFirst" runat="server" Behaviors="Move" Modal="false"
                    Height="200" Width="500" EnableShadow="true">
                </telerik:RadWindow>
                <telerik:RadWindow ID="RadDialogConfirmSecond" runat="server" Behaviors="Move" OnClientClose="OnClientCloseDelete"
                    Modal="false" Height="200" Width="500" EnableShadow="true">
                </telerik:RadWindow>
                <telerik:RadWindow ID="RadDialogAlert" runat="server" Behaviors="Move" Modal="false"
                    Height="200" Width="500" EnableShadow="true" VisibleOnPageLoad="false">
                </telerik:RadWindow>
            </Windows>
        </telerik:RadWindowManager>
        <table class="style1">
            <tr>
                <td>
                    <asp:LinkButton ID="lbtnHome" runat="server">กลับหน้าหลัก</asp:LinkButton>
                </td>
                <td style="text-align: right">
                    <asp:LinkButton ID="lbtnInsert" runat="server">เพิ่มผู้ใช้ใหม่</asp:LinkButton>
                </td>
            </tr>
        </table>
        <br />
    </div>
    <telerik:RadGrid ID="GvUserAdmin" runat="server" AutoGenerateColumns="False" AllowPaging="True"
        GridLines="None" PageSize="20" AllowMultiRowSelection="True" 
        ShowFooter="True">
        <MasterTableView DataKeyNames="a" AllowSorting="true" AllowMultiColumnSorting="true"
            NoMasterRecordsText="ไม่พบข้อมูล" HeaderStyle-ForeColor="Black">
<CommandItemSettings ExportToPdfText="Export to Pdf"></CommandItemSettings>
            <Columns>
                <telerik:GridBoundColumn DataField="a" HeaderStyle-HorizontalAlign="Center" HeaderText="ลำดับ"
                    ItemStyle-Width="30" ItemStyle-HorizontalAlign="Center">
                    <HeaderStyle HorizontalAlign="Center" />

<ItemStyle HorizontalAlign="Center" Width="30px"></ItemStyle>
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="FirstName" HeaderStyle-HorizontalAlign="Center"
                    HeaderText="ชื่อ" ItemStyle-Width="600">
                    <HeaderStyle HorizontalAlign="Center" />

<ItemStyle Width="600px"></ItemStyle>
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="LastName" HeaderStyle-HorizontalAlign="Center"
                    HeaderText="นามสกุล" ItemStyle-Width="600">
                    <HeaderStyle HorizontalAlign="Center" />

<ItemStyle Width="600px"></ItemStyle>
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="UserName" HeaderStyle-HorizontalAlign="Center"
                    HeaderText="ชื่อผู้ใช้" ItemStyle-Width="600">
                    <HeaderStyle HorizontalAlign="Center" />

<ItemStyle Width="600px"></ItemStyle>
                </telerik:GridBoundColumn>
                <telerik:GridTemplateColumn UniqueName="EditColumn" HeaderText="แก้ไข" ItemStyle-Width="30px">
                    <ItemTemplate>
                        <asp:ImageButton ID="BtnEdit" runat="server" ImageUrl="~/Images/freehand.png" ToolTip="แก้ไข"
                            CommandName="Update" CommandArgument='<%# eval("UserId") %>' />
                    </ItemTemplate>

<ItemStyle Width="30px"></ItemStyle>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn UniqueName="DeleteColumn" HeaderText="ลบ" ItemStyle-Width="50px">
                    <ItemTemplate>
                        <asp:ImageButton ID="BtnDelete" runat="server" ImageUrl="~/Images/eraser.png" ToolTip="ลบ"
                            CommandName="Delete" CommandArgument='<%# eval("UserId") %>' />
                    </ItemTemplate>

<ItemStyle Width="50px"></ItemStyle>
                </telerik:GridTemplateColumn>
            </Columns>

<HeaderStyle ForeColor="Black"></HeaderStyle>
        </MasterTableView>
        <FooterStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" 
            Font-Strikeout="False" Font-Underline="False" ForeColor="Black" Wrap="True" />
        <PagerStyle NextPageText="Next" PrevPageText="Previous" />
        <HeaderContextMenu CssClass="GridContextMenu GridContextMenu_Default" EnableImageSprites="True">
        </HeaderContextMenu>
    </telerik:RadGrid>
    </form>
</body>
</html>
