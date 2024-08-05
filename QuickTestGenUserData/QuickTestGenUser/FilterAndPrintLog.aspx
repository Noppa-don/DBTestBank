<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="FilterAndPrintLog.aspx.vb" Inherits="QuickTestGenUserData.FilterAndPrintLog" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
         <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
    </telerik:RadScriptManager>
          <div id="DialogFilterLog">
            <table>
                 <tr>
                    <td colspan="5" style="height:50px;">
                        <label id="lblLogName" runat="server">ดูข้อมูลการเข้าใช้งานของ วารี โตพันธ์</label>
                    </td>
                  </tr>
                 <tr>
                    <td style="width: 80px;">
                        <label>เริ่มวันที่</label>
                    </td>
                    <td>
                        <telerik:RadDatePicker ID="DpkStartDate" runat="server"></telerik:RadDatePicker>
                    </td>
                    <td>
                        <label>ถึงวันที่</label>
                    </td>
                    <td>
                        <telerik:RadDatePicker ID="DpkEndDate" runat="server"></telerik:RadDatePicker>
                    </td>
                    <td>
                        <telerik:RadButton ID="btnShowLog" runat="server" Width="100" Text="ค้นหา" Skin="Outlook"></telerik:RadButton>
                    </td>
                    <td>
                        <telerik:RadButton ID="btnExport" runat="server" Visible="false" Width="100" Text="พิมพ์ Excel" Skin="Outlook"></telerik:RadButton>
                    </td>
                     <td style="text-align: right; width: 57%;">
                        <telerik:RadButton ID="btnBack" runat="server" Width="100" Text="กลับ" Skin="Outlook"></telerik:RadButton>
                    </td>
                </tr>
            </table>
        </div>
        <div>
            <telerik:RadGrid ID="GvLog" runat="server" AutoGenerateColumns="False" AllowPaging="True" BorderStyle="Solid"
                 PageSize="5000" AllowMultiRowSelection="true">
                <MasterTableView DataKeyNames="LogDate" AllowSorting="true" AllowMultiColumnSorting="true"
                    NoMasterRecordsText="ไม่พบข้อมูล" HeaderStyle-ForeColor="Black">
                    <Columns>

                        <telerik:GridBoundColumn DataField="LogDate" HeaderStyle-HorizontalAlign="Center" HeaderText="วันที่"
                            ItemStyle-Width="300">
                            <HeaderStyle HorizontalAlign="Center" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="LevelName" HeaderStyle-HorizontalAlign="Center" HeaderText="ชั้น"
                            ItemStyle-Width="50">
                            <HeaderStyle HorizontalAlign="Center" />
                        </telerik:GridBoundColumn>
                          <telerik:GridBoundColumn DataField="QCatName" HeaderStyle-HorizontalAlign="Center" HeaderText="บท"
                            ItemStyle-Width="300">
                            <HeaderStyle HorizontalAlign="Center" />
                        </telerik:GridBoundColumn>
                          <telerik:GridBoundColumn DataField="QSetName" HeaderStyle-HorizontalAlign="Center" HeaderText="ชุดข้อสอบ"
                            ItemStyle-Width="300">
                            <HeaderStyle HorizontalAlign="Center" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="QNo" HeaderStyle-HorizontalAlign="Center" HeaderText="เลขข้อ"
                            ItemStyle-Width="50">
                            <HeaderStyle HorizontalAlign="Center" />
                        </telerik:GridBoundColumn>
                          <telerik:GridBoundColumn DataField="LogDetail" HeaderStyle-HorizontalAlign="Center" HeaderText="รายละเอียด"
                            ItemStyle-Width="1000">
                            <HeaderStyle HorizontalAlign="Center" />
                        </telerik:GridBoundColumn>
                                                                                                                       
                    </Columns>
                </MasterTableView>
                <HeaderContextMenu CssClass="GridContextMenu GridContextMenu_Default" EnableImageSprites="True">
                </HeaderContextMenu>
            </telerik:RadGrid>
        </div>
    </form>
</body>
</html>
