<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="GenUserManagerPage.aspx.vb" Inherits="QuickTestGenUserData.GenUserManagerPage" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="../UserControl/LocalControl.ascx" TagName="LocalControl" TagPrefix="Lc" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
     
    <style type="text/css">
        .style2
        {
            width: 80px;
        }
        .style3
        {
            width: 97px;
        }
        .style4
        {
            width: 184px;
        }
        .style5
        {
            width: 1392px;
        }
        .style6
        {
            width: 80%;
            text-align: right;
        }
        .auto-style1
        {
            width: 40%;
            height: 29px;
        }
        .auto-style2
        {
            width: 237px;
            text-align: center;
        }
        .auto-style3
        {
            width: 156px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">

    <script type="text/javascript" src="../Scripts/jquery-ui-1.8.18.js"></script>

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
        <table>
            <tr>
           
                <td style="padding-left: 40px;">
                    <asp:label ID="HyperLink2" runat="server"  >ค้นหาข้อมูลผู้ใช้ </asp:label>
                </td>
                 <td style="width: 90.5%;text-align: right;">
                   <asp:LinkButton ID="lbAddUser" runat="server">เพิ่มผู้ใช้ใหม่</asp:LinkButton>
                </td> 

              <%--  <td style="text-align: right" colspan="2" class="style5">
                <asp:LinkButton ID="lbtnBackHome" runat="server">ออกจากระบบ</asp:LinkButton>
                </td>--%>
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
              <%If ConfigurationManager.AppSettings("IsQuicktestProduction") = True Then %>   
            <td class="style3">
            เลือกโรงเรียน
            </td>
           
            <td class="style4">
                <telerik:RadComboBox ID="CmbSchool" runat="server">
                </telerik:RadComboBox>
            </td>
                      
              <%End if%>
                <td id="tdSearch" runat="server" class="style2">
                    ค้นหาผู้ใช้
                </td>
                <td class="auto-style2">
                    <telerik:RadTextBox ID="txtName" runat="server" Width="200px">
                    </telerik:RadTextBox>
                </td>
                <%If ConfigurationManager.AppSettings("IsQuicktestProduction") = True Then %>
                <td class="auto-style3">
                    ค้นจากรหัสโรงเรียน
                </td>
                   
                <td>
                    <telerik:RadTextBox ID="txtSchoolCode" runat="server" Width="200px">
                    </telerik:RadTextBox>
                </td>
                 <%End If%>
                <td align="right" class="auto-style1">
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
                                                    CommandName="Update" CommandArgument='<%# Eval("UserId")%>' />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="DeleteColumn" HeaderText="ลบ" ItemStyle-Width="50px">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="BtnDelete" runat="server" ImageUrl="~/Images/eraser.png" ToolTip="ลบ"
                                                    CommandName="Delete" CommandArgument='<%# eval("UserId") %>' />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="ViewLogColumn" HeaderText="ดู Log" ItemStyle-Width="50px">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="BtnViewLog" runat="server" ImageUrl="~/Images/ViewLog.png" ToolTip="ดู Log"
                                                    CommandName="ViewLog" CommandArgument='<%# eval("UserId") %>' />
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
