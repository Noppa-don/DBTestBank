<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="SelectSession_Pad.aspx.vb"
    Inherits="QuickTest.SelectSession_Pad" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <script src="js/jquery-1.7.1.js" type="text/javascript"></script>
    <script src="js/jquery-ui-1.8.18.js" type="text/javascript"></script>
    <link href="css/jquery-ui-1.8.18.custom.css" rel="stylesheet" type="text/css" />
    <telerik:RadScriptBlock ID="RadScriptBlock1" runat="server">
        <script type="text/javascript">
   
        $(function () {
        
        $('#Dialog-Confirm').dialog({
        autoOpen:false,
        resizable:false,
        modal:true,
        buttons:{
        "ตกลง":function(){
        UseSessionIsSelected();
        },
        "ยกเลิก":function(){
        $(this).dialog('close');
        }
        }

        });
     
    //เมื่อเรากดคลิกปุ่มเข้าห้องเดิมต้องไปยังหน้าที่อยู่ใน Session ที่เลือกตอนนั้น
        $('#btnUseOldSession').click(function () {
        $('#Dialog-Confirm').dialog('open');
        });

    });
        //Get ค่าว่าหน้าปัจจุบันของ Session นั้น 
        function SendRowSelected(send, args) {
            $('#HdScreenName').val(args.getDataKeyValue("หน้าจอ"));
            $('#HdPkInfo').val(args.getDataKeyValue("PK"));
        }
        
        function UseSessionIsSelected() {
             var isUseTablet = '<%=IsUseTablet %>';
             var DVID = '<%=DeviceId %>';
             var UrlLocation = $('#HdScreenName').val();
            var PkData = $('#HdPkInfo').val();
            //alert(isUseTablet);
            if (UrlLocation !== '') {
            //////////////////////////////////////////////////////// Set ค่าให้ Session
               $.ajax({ type: "POST",
	            url: "<%=ResolveUrl("~")%>SelectSession_Pad.aspx/SetSession",
	            data: "{ PkInFo:'" + PkData + "'}",
	            contentType: "application/json; charset=utf-8", dataType: "json",   
	            success: function (msg) {
                //เช็คก่อนว่าเป็นการเข้าหน้านี้จาก Tablet ครูหรือเปล่า
                if (isUseTablet == 'True') {//ถ้าเขาจาก Tablet ต้องดัก URL เพราะบางหน้า Tablet ไม่สามารถไปได้
                    var TabcurrentPage = msg.d;
                    
                    TabcurrentPage = TabcurrentPage.toLowerCase();
                    
                    //ถ้า URL เป็นหน้า Step1 ต้องไปหน้า Step1 สำหรับ Tablet 
                    var step1Page = '<%=ResolveUrl("~")%>testset/step1.aspx'; 
                    var settingPage = '<%=ResolveUrl("~")%>activity/settingactivity.aspx';      
                    //var activityPage = '<%=ResolveUrl("~")%>activity/activitypage.aspx';            
                    var activityPage = '/activity/activitypage.aspx'; 
                     
                    if (TabcurrentPage == step1Page.toLowerCase()) {
                        window.location = '<%=ResolveUrl("~")%>TestSet/Step1_PadTeacher.aspx?DeviceId=' + DVID;
                    }
                    //ถ้า URL เป็นหน้า Setting ต้องไปหน้า Setting สำหรับ Tablet                    
                    else if (TabcurrentPage == settingPage.toLowerCase()) {
                        window.location = '<%=ResolveUrl("~")%>Activity/SettingActivity_PadTeacher.aspx?DeviceId=' + DVID;
                    }
                    //ถ้า URL เป็นหน้า ActivityPage ต้องไปหน้า Activity สำหรับ Tablet
                    else if (TabcurrentPage == activityPage.toLowerCase()) {
                        window.location = '<%=ResolveUrl("~")%>Activity/ActivityPage_PadTeacher.aspx?DeviceId=' + DVID;
                    }
                    //ถ้า URL ไม่เข้าเงื่อนไขอะไรเลยให้ไปหน้า รอจัดข้อสอบ
                    else
                    {
                        //window.location = '<%=ResolveUrl("~")%>Activity/WaitPageTeacherPad.aspx?DeviceId=' + DVID;
                        window.location = TabcurrentPage;
                    }
                }
                },
	            error: function myfunction(request, status)  {
                alert('บางอย่างผิดพลาดโปรดลองอีกครั้ง');
	            }
	        });
            ///////////////////////////////////////////////////////
            }
            else {
                alert('โปรดเลือก Session ก่อน');
            }
        }


        </script>
    </telerik:RadScriptBlock>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
    </telerik:RadScriptManager>
    <div>
        <telerik:RadGrid ID="GVSession" runat="server" GridLines="Vertical" PagerStyle-VerticalAlign="Middle"
            Skin="Hay" AutoGenerateColumns="false">
            <ClientSettings>
                <Selecting AllowRowSelect="true" />
                <ClientEvents OnRowClick='SendRowSelected' />
            </ClientSettings>
            <MasterTableView ClientDataKeyNames='หน้าจอ,PK'>
            </MasterTableView>
        </telerik:RadGrid>
    </div>
    <div style='width: 100%; margin-left: auto; margin-right: auto; margin-top: 50px;'>
        <%-- <asp:Button ID="BtnNewSession" runat="server" Text="เปิดของใหม่ +" Style='width: 100px;
            height: 50px; font-size: 15px; background-color: #5ECE0D; border-radius: 15px;
            color: White;' />
        <input type="button" id='btnUseOldSession' value='เข้าของเดิม' style='float: right;
            width: 100px; height: 50px; font-size: 15px; background-color: #5ECE0D; border-radius: 15px;
            color: White;' />--%>
        <asp:Button ID="BtnNewSession" runat="server" Text="เปิดห้องใหม่" Style='margin-left: 100px;'
            CssClass="btnSelectSession" />
        <input type="button" id='btnUseOldSession' value='เข้าห้องเดิม' style='float: right;
            margin-right: 100px;' class="btnSelectSession" />
    </div>
    <input type="hidden" id='HdScreenName' />
    <input type='hidden' id='HdPkInfo' />
    <div id='Dialog-Confirm' title='ยืนยัน Session'>
        <p>
            ต้องการใช้ Session นี้ใช่ไหม ?</p>
    </div>
    </form>
</body>
</html>
<style type="text/css">
    .RadGrid ,.RadGrid table
    {
        border-radius: .5em;
    }
    .RadGrid table th:first-child
    {
        border-radius: 5px 0 0 0;
    }
    .RadGrid table th:last-child
    {
        border-radius:  0 5px 0 0;
    }
    .RadGrid table tr:last-child td:first-child
    {
        border-radius:  0 0  0 5px;
    }
    .RadGrid table tr:last-child td:last-child
    {
        border-radius:  0 0 5px 0 ;
    }
    .RadGrid table th, .RadGrid table tr
    {
        height: 70px;
        font-size: 25px;
    }
    .RadGrid_Hay .rgHeader
    {
        text-align: center;
    }
    .btnSelectSession
    {
        width: 170px;
        height: 70px;
        font-size: 25px;
        background-color: #5ECE0D;
        border-radius: 15px;
        color: White;
    }
    body
    {
        background-color: rgb(216, 255, 216);
    }
     form
    {
        margin-top:5%;
    }
</style>
