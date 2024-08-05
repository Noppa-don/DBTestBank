<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="AdminContactPage.aspx.vb"
    Inherits="QuickTest.AdminContactPage" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .style1
        {
            width: 1501px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
    </telerik:RadScriptManager>
    
    <div>
        <br />
    </div>
    <table>
    <tr>
    <td style="text-align: right" colspan="2" class="style1">
                    <asp:LinkButton ID="lbtnHome" runat="server">กลับเมนูหลัก</asp:LinkButton>
                </td>
    </tr>
    </table>
 
    <telerik:RadGrid ID="GvContact" runat="server" AllowPaging="True" BorderColor="White"
        GridLines="None" PageSize="20" HeaderStyle-Font-Bold="true" HeaderStyle-Width="200"
        AutoGenerateColumns="False" EnableHeaderContextAggregatesMenu="True" EnableHeaderContextFilterMenu="True"
        EnableHeaderContextMenu="True" BorderStyle="None" Skin="Telerik">
        <MasterTableView EnableHeaderContextAggregatesMenu="True" GridLines="None">
            <CommandItemSettings ExportToPdfText="Export to Pdf"></CommandItemSettings>
            <Columns>
                <telerik:GridBoundColumn DataField="ReceivedDate" HeaderStyle-HorizontalAlign="Center"
                    HeaderText="แจ้งเมื่อ" ItemStyle-Width="30" ItemStyle-HorizontalAlign="Center">
                    <HeaderStyle HorizontalAlign="Center" Font-Bold="True" />
                    <ItemStyle HorizontalAlign="Center" Width="30px"></ItemStyle>
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="Title" HeaderStyle-HorizontalAlign="Center" HeaderText="สอบถามเกี่ยวกับเรื่อง"
                    ItemStyle-Width="600">
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle Width="600px"></ItemStyle>
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="Description" HeaderStyle-HorizontalAlign="Center"
                    HeaderText="คำถาม,แนะนำ,หรือข้อสงสัย" ItemStyle-Width="600">
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle Width="600px"></ItemStyle>
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="Tel" HeaderStyle-HorizontalAlign="Center" HeaderText="ชื่อ,เบอร์โทร"
                    ItemStyle-Width="600">
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle Width="600px"></ItemStyle>
                </telerik:GridBoundColumn>
                <%--<telerik:GridBoundColumn DataField="IsTel" HeaderStyle-HorizontalAlign="Center" HeaderText="ต้องการให้โทรกลับ ?"
                    ItemStyle-Width="600">
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle Width="600px"></ItemStyle>

                </telerik:GridBoundColumn>--%>
                <telerik:GridCheckBoxColumn DataField="IsTel" DataType="System.Boolean" HeaderText="ต้องการให้โทรกลับ ?"
                    SortExpression="IsTel" UniqueName="IsTel">
                </telerik:GridCheckBoxColumn>
                <%--<telerik:GridTemplateColumn UniqueName="IsTel" HeaderText="ต้องการให้โทรกลับ ?" ItemStyle-Width="30px">
                    <ItemTemplate>
                        <asp:ImageButton ID="BtnEdit" runat="server" ImageUrl="~/Images/bullet_checked.gif" ToolTip="dfhdfh"
                            CommandName="1" CommandArgument= '<%# eval("IsTel") %> ' />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>--%>
            </Columns>
        </MasterTableView>
        <HeaderStyle Font-Bold="True" Width="200px"></HeaderStyle>
        <HeaderContextMenu EnableImageSprites="True" CssClass="GridContextMenu GridContextMenu_Default">
        </HeaderContextMenu>
    </telerik:RadGrid>
    </form>
</body>
</html>