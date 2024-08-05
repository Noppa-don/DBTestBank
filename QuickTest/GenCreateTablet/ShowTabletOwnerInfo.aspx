<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ShowTabletOwnerInfo.aspx.vb"
    Inherits="QuickTest.ShowTabletOwnerInfo" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <script src="../js/jquery-1.7.1.js" type="text/javascript"></script>
    <script src="../js/jquery.fancybox.js" type="text/javascript"></script>
    <link href="../css/jquery.fancybox.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">


        function ShowActivitypagePad(InputDeviceId) {
            $.fancybox({
                'autoScale': true,
                'transitionIn': 'none',
                'transitionOut': 'none',
                'href': '../Activity/ActivityPage_Pad.aspx?DeviceUniqueID=' + InputDeviceId,
                'type': 'iframe',
                'width': 1024,
                'minHeight': 600
            });
        }

        function ShowChooseTestSet(InputDeviceId) {
            $.fancybox({
                'autoScale': true,
                'transitionIn': 'none',
                'transitionOut': 'none',
                'href': '../PracticeMode_Pad/ChooseTestset.aspx?DeviceUniqueID=' + InputDeviceId,
                'type': 'iframe',
                'width': 1024,
                'minHeight': 600
            });
        }

    </script>

    <style type="text/css">
    
    .ForbtnOpenQuiz
    {
        width:90px;
        height:30px;
        margin:5px 5px 5px 5px;
        }
    
    </style>

    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
    </telerik:RadScriptManager>
    <telerik:RadAjaxManager runat="server" ID="RadAjaxManager1">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="GridShowInfo">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="GridShowInfo" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <div style='width: 100%; height: 500px; overflow: auto; margin-left: auto; margin-right: auto;'>
        <telerik:RadGrid runat="server" ID="GridShowInfo" Width='1350' AutoGenerateColumns="false"
            AllowFilteringByColumn="true" AllowSorting="true">
            <MasterTableView AllowFilteringByColumn="true">
                <Columns>
                    <telerik:GridBoundColumn DataField="Tablet_SerialNumber" HeaderText="DeviceIdTabletSerialNumber" AutoPostBackOnFilter="true"
                        CurrentFilterFunction="Contains" />
                    <telerik:GridBoundColumn DataField="TYPE" HeaderText="Type" AutoPostBackOnFilter="true"
                        CurrentFilterFunction="Contains" />
                    <telerik:GridBoundColumn DataField="School_Code" HeaderText="School_ID" AutoPostBackOnFilter="true"
                        CurrentFilterFunction="Contains" />
                    <telerik:GridBoundColumn DataField="Student_Code" HeaderText="StudentCode" AutoPostBackOnFilter="true"
                        CurrentFilterFunction="Contains" />
                    <telerik:GridBoundColumn DataField="StudentOrTeacherId" HeaderText="Teacher/Student_ID"
                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" />
                    <telerik:GridBoundColumn DataField="TeacherOrStudentName" HeaderText="Teacher/Student_Name"
                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" />
                    <telerik:GridBoundColumn DataField="Student_CurrentClass" HeaderText="ชั้น" AutoPostBackOnFilter="true"
                        CurrentFilterFunction="Contains" />
                    <telerik:GridBoundColumn DataField="Student_CurrentRoom" HeaderText="ห้อง" AutoPostBackOnFilter="true"
                        CurrentFilterFunction="Contains" />
                    <telerik:GridBoundColumn DataField="Student_CurrentNoInRoom" HeaderText="เลขที่"
                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" />
                    <telerik:GridTemplateColumn HeaderText="ปุ่มเปิดหน้าเด็ก">
                        <ItemTemplate>

                        <%# TestTestTest(Eval("TYPE").ToString(),Eval("Tablet_SerialNumber"))%>

                         <%--     <input type="button" value='ควิซ' onclick="ShowActivitypagePad('<%#Eval("Tablet_SerialNumber") %>')" />
                            <input type="button" value='การบ้าน/ฝึกฝน' onclick="ShowChooseTestSet('<%#Eval("Tablet_SerialNumber") %>')" />--%>
 
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
            </MasterTableView>
        </telerik:RadGrid>
    </div>
    <div style='width: 250px; margin-left: auto; margin-right: auto;'>
        <asp:Button ID="btnNew" runat="server" Text="+" Style='width: 220px; height: 80px;
            margin-top: 20px; font-size: 70px;' />
    </div>
    </form>
</body>
</html>
