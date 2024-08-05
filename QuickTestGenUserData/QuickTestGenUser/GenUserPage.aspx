<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="GenUserPage.aspx.vb" Inherits="QuickTestGenUserData.GenUserPage" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="../UserControl/LocalControl.ascx" TagName="LocalControl" TagPrefix="Lc" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">

    

   
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
 <script type="text/javascript">

     function OnClientCloseDelete(s, e) {
         //get the transferred arguments
         var arg = e.get_argument();
         if (arg.IsOk == 'yes') {
             var pageId = '<%= Page.ClientID %>';
                 __doPostBack(pageId, "delete");
             }
         }

         function onAllClassCheck(i) {
             var chk = document.getElementById(i.id);
             var isChecked = chk.checked;
             var pageId = '<%= Page.ClientID %>';
             __doPostBack(pageId, "AllClassCheck," + isChecked + "," + i.id);
         }
    </script>


    </telerik:RadCodeBlock>
   
    
</head>
<body>
    <form id="form1" runat="server">

    <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
    </telerik:RadScriptManager>

       <div>
        <telerik:RadWindowManager ID="RadWindowManager1" VisibleStatusbar="False" runat="server"
            Skin="Outlook" EnableShadow="True">
            <Windows>
                <telerik:RadWindow ID="RadDialogConfirmFirst" runat="server" Behaviors="move" Modal="false"
                    Height="200" Width="500" EnableShadow="true">
                </telerik:RadWindow>
     
                <telerik:RadWindow ID="RadDialogAlert" runat="server" Behaviors="move" Modal="false"
                    Height="200" Width="500" EnableShadow="true" VisibleOnPageLoad="false">
                </telerik:RadWindow>
            </Windows>
            
        </telerik:RadWindowManager>
        
        <table>
            <tr>
                <td>&nbsp;</td>
                <td>
                    <asp:LinkButton ID="lbtnSearchUser" runat="server">ค้นหาข้อมูลผู้ใช้ &gt;</asp:LinkButton>
                &nbsp;
                    <asp:LinkButton ID="lbtnAddUser" runat="server" Width="70px">เพิ่มผู้ใช้   </asp:LinkButton>
                &nbsp;
                    <%If ConfigurationManager.AppSettings("IsQuicktestProduction") = True Then %>
                    <asp:Label ID="lblSchool" runat="server" Text="Label"></asp:Label>
                    <%End If%>
                </td>
               
            </tr>
        </table>
        <table>
            <tr>
                <td>
                    ชื่อ
                </td>
                <td>
                    <telerik:RadTextBox ID="txtFirstName" runat="server" Width="200">
                    </telerik:RadTextBox>
                </td>
                <td valign="top">
                    นามสกุล
                </td>
                <td>
                    <telerik:RadTextBox ID="txtLastName" runat="server" Width="200">
                    </telerik:RadTextBox>
                </td>
                <td></td>
            </tr>
            <tr>
                <td valign="top">
                    ชื่อผู้ใช้
                </td>
                <td>
                    <telerik:RadTextBox ID="txtUserName" runat="server" Width="200">
                    </telerik:RadTextBox>
                </td>
                <td valign="top">
                    รหัสผ่าน
                </td>
                <td>
                    <asp:TextBox ID="txtPSW" Width="200" TextMode="Password" runat="server"></asp:TextBox>
                </td>
                 <td valign="top">
                    ยืนยันรหัสผ่าน
                </td>
                <td>
                    <asp:TextBox ID="txtCFPSW" Width="200" TextMode="Password" runat="server"></asp:TextBox>
                   
                </td>
                  <td>
                        <asp:Label ID="lblNotChangePassword" runat="server" Text="* ถ้าไม่ต้องการเปลี่ยน Password ไม่ต้องกรอก รหัสผ่าน และ ยืนยันรหัสผ่าน นะคะ" ForeColor="#FF3300" Visible="false"></asp:Label>
                  </td>
                
            </tr>


          <%If ConfigurationManager.AppSettings("IsQuicktestProduction") = True Then %>
             <tr>
                <td style="width: 80px;">
                    <label>เริ่มวันที่</label>
                </td>
                <td>
                    <telerik:RadDatePicker ID="DpkStartDate" runat="server"></telerik:RadDatePicker>
                </td>
             </tr>
             <br />
             <tr>
                <td>
                    <label>ถึงวันที่</label>
                </td>
                <td>
                    <telerik:RadDatePicker ID="DpkEndDate" runat="server"></telerik:RadDatePicker>
                </td>
              </tr>
            <%End If%>

        </table>

        <div>
            <telerik:RadGrid ID="GvCondition" runat="server" AllowPaging="True"
                GridLines="None" PageSize="5000" AllowMultiRowSelection="True" 
                AllowMultiRowEdit="True"  AutoGenerateColumns="false">
                <MasterTableView DataKeyNames="a" AllowSorting="True" AllowMultiColumnSorting="True"
                    NoMasterRecordsText="ไม่พบข้อมูล" HeaderStyle-ForeColor="Black">
                    <Columns>
                                        

                        <telerik:GridBoundColumn DataField="a" HeaderStyle-HorizontalAlign="Center" HeaderText=""  
                            ItemStyle-Width="100" ItemStyle-HorizontalAlign="Left">
                            <HeaderStyle HorizontalAlign="Center" />
                        </telerik:GridBoundColumn>

                        <telerik:GridTemplateColumn DataField="cbAll" HeaderStyle-HorizontalAlign="Center" uniqueName="ทั้งหมด" HeaderText="ทั้งหมด"
                            ItemStyle-Width="50" HeaderStyle-Width="50" >
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <asp:CheckBox ID="cbAll" runat="server" onclick="onAllClassCheck(this)" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                       <%-- <telerik:GridTemplateColumn DataField="01" HeaderStyle-HorizontalAlign="Center" HeaderText="อ.1"
                            ItemStyle-Width="50">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <asp:CheckBox ID="C1" runat="server" onclick="onAllClassCheck(this)"/>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn DataField="02" HeaderStyle-HorizontalAlign="Center" HeaderText="อ.2"
                            ItemStyle-Width="50">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <asp:CheckBox ID="C2" runat="server" onclick="onAllClassCheck(this)"/>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                          <telerik:GridTemplateColumn DataField="03" HeaderStyle-HorizontalAlign="Center" HeaderText="อ.3"
                            ItemStyle-Width="50">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <asp:CheckBox ID="C3" runat="server" onclick="onAllClassCheck(this)"/>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>--%>

                         <telerik:GridTemplateColumn DataField="04" HeaderStyle-HorizontalAlign="Center" HeaderText="ป.1"
                            ItemStyle-Width="50">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <asp:CheckBox ID="C04" runat="server" onclick="onAllClassCheck(this)"/>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn DataField="05" HeaderStyle-HorizontalAlign="Center" HeaderText="ป.2"
                            ItemStyle-Width="50">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemTemplate>  
                                <asp:CheckBox ID="C05" runat="server" onclick="onAllClassCheck(this)"/>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn DataField="06" HeaderStyle-HorizontalAlign="Center" HeaderText="ป.3"
                            ItemStyle-Width="50">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <asp:CheckBox ID="C06" runat="server" onclick="onAllClassCheck(this)"/>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn DataField="07" HeaderStyle-HorizontalAlign="Center" HeaderText="ป.4"
                            ItemStyle-Width="50">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <asp:CheckBox ID="C07" runat="server" onclick="onAllClassCheck(this)" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn DataField="08" HeaderStyle-HorizontalAlign="Center" HeaderText="ป.5"
                            ItemStyle-Width="50">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemTemplate>   
                                <asp:CheckBox ID="C08" runat="server" onclick="onAllClassCheck(this)" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn DataField="09" HeaderStyle-HorizontalAlign="Center" HeaderText="ป.6"
                            ItemStyle-Width="50">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <asp:CheckBox ID="C09" runat="server" onclick="onAllClassCheck(this)" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>                        
                        <telerik:GridTemplateColumn DataField="10" HeaderStyle-HorizontalAlign="Center" HeaderText="ม.1"
                            ItemStyle-Width="50">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <asp:CheckBox ID="C10" runat="server" onclick="onAllClassCheck(this)"/>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn DataField="11" HeaderStyle-HorizontalAlign="Center" HeaderText="ม.2"
                            ItemStyle-Width="50">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <asp:CheckBox ID="C11" runat="server" onclick="onAllClassCheck(this)"/>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn DataField="12" HeaderStyle-HorizontalAlign="Center" HeaderText="ม.3"
                            ItemStyle-Width="50">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <asp:CheckBox ID="C12" runat="server" onclick="onAllClassCheck(this)"/>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn DataField="13" HeaderStyle-HorizontalAlign="Center" HeaderText="ม.4"
                            ItemStyle-Width="50">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <asp:CheckBox ID="C13" runat="server" onclick="onAllClassCheck(this)"/>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                         <telerik:GridTemplateColumn DataField="14" HeaderStyle-HorizontalAlign="Center" HeaderText="ม.5"
                            ItemStyle-Width="50">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <asp:CheckBox ID="C14" runat="server" onclick="onAllClassCheck(this)" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                         <telerik:GridTemplateColumn DataField="15" HeaderStyle-HorizontalAlign="Center" HeaderText="ม.6"
                            ItemStyle-Width="50">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <asp:CheckBox ID="C15" runat="server" onclick="onAllClassCheck(this)" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
                <HeaderContextMenu CssClass="GridContextMenu GridContextMenu_Default" EnableImageSprites="True">
                </HeaderContextMenu>
            </telerik:RadGrid>
        </div>

        <%--<table style="width: 100%">
            <tr>
               
                <td align="right">
                <telerik:RadButton ID="btnAddDetail" runat="server" Text="เพิ่มเงื่อนไข">
                    </telerik:RadButton>
                   
                </td>
            </tr>
        </table>
        <div>
            <telerik:RadGrid ID="GvBudgetClass" runat="server" AutoGenerateColumns="False" AllowPaging="True"
                GridLines="None" PageSize="5000" AllowMultiRowSelection="true">
                <MasterTableView  DataKeyNames = "a" AllowSorting="true" AllowMultiColumnSorting="true"
                    NoMasterRecordsText="ไม่พบข้อมูล" HeaderStyle-ForeColor="Black">
                    <Columns>
                        <telerik:GridBoundColumn DataField="a" HeaderStyle-HorizontalAlign="Center" HeaderText="ลำดับ"
                            ItemStyle-Width="30" ItemStyle-HorizontalAlign="Center">
                            <HeaderStyle HorizontalAlign="Center" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Cls" HeaderStyle-HorizontalAlign="Center" HeaderText="ระดับชั้น"
                            ItemStyle-Width="100">
                            <HeaderStyle HorizontalAlign="Center" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Subject" HeaderStyle-HorizontalAlign="Center" HeaderText="กลุ่มสาระ"
                            ItemStyle-Width="200">
                            <HeaderStyle HorizontalAlign="Center" />
                        </telerik:GridBoundColumn>
                        <telerik:GridTemplateColumn UniqueName="DeleteColumn" HeaderText="" ItemStyle-Width="30px">
                            <ItemTemplate>
                                <asp:ImageButton ID="BtnDelete" runat="server" ImageUrl="~/Images/eraser.png" ToolTip="ลบ"
                                    CommandName="Delete" CommandArgument='<%# eval("a") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>  
                        <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Select" HeaderStyle-HorizontalAlign="Center"
                            ImageUrl="~\Images\freehand.png" UniqueName="SelectColumn">
                            <HeaderStyle Width="30px" />
                        </telerik:GridButtonColumn>
                    </Columns>
                </MasterTableView>
                <HeaderContextMenu CssClass="GridContextMenu GridContextMenu_Default" EnableImageSprites="True">
                </HeaderContextMenu>
            </telerik:RadGrid>
        </div>--%>

         <table width="100%">
            <tr>
                <td align="right">
                    <telerik:RadButton ID="BtnSave" runat="server" Width="100" Text="บันทึก">
                    </telerik:RadButton>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>