<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="WaitPageTeacherPad.aspx.vb"
    Inherits="QuickTest.WaitPageTeacherPad" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <script src="../js/jquery-1.7.1.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.signalR-2.0.2.min.js" type="text/javascript"></script>
   <script src='<%=ResolveClientUrl("~/signalr/hubs") %>' type="text/javascript"></script>
    <script type="text/javascript">

        $(function () {

            var groupname = '<%=GroupName %>';

            SignalRCheck = $.connection.hubSignalR;
            // alert(groupname);

            SignalRCheck.client.send = function (message) {
                if (message == '<%=ResolveUrl("~")%>testset/step1.aspx') {
                    window.location = '<%=ResolveUrl("~")%>TestSet/Step1_PadTeacher.aspx';
                }
                else if (message == '<%=ResolveUrl("~")%>activity/settingactivity.aspx') {
                    window.location = '<%=ResolveUrl("~")%>Activity/SettingActivity_PadTeacher.aspx';
                }
                else if (message == '<%=ResolveUrl("~")%>activity/activitypage.aspx') {
                    window.location = '<%=ResolveUrl("~")%>Activity/ActivityPage_PadTeacher.aspx';
                }
            };

            $.connection.hub.start().done(function () {
                SignalRCheck.server.addToGroup(groupname);
                //SignalRCheck.server.sendCommand(groupname, '../TestSet/Step1.aspx');

                $('#gotoStep1').click(function () {
                    setCurrentPage('<%=ResolveUrl("~")%>testset/step1.aspx');
                    window.location = '<%=ResolveUrl("~")%>TestSet/Step1_PadTeacher.aspx';
                });
            });

        });
        function setCurrentPage(thisPage) {
            $.ajax({ type: "POST",
                  url: "<%=ResolveUrl("~")%>SelectSessionSignalR.aspx/setCurrentPage",
                  data:"{ thisPage : '" + thisPage + "'}",
                  async: false,                              
                  contentType: "application/json; charset=utf-8", dataType: "json",
                  success: function (data) {                                              
                  },
                  error: function myfunction(request, status) {   
                        alert(status);                      
                  }
            });
        }
    </script>
    <title></title>
</head>
<body style='background-color: Orange'>
    <form id="form1" runat="server">
    <div style='margin-left: auto; margin-right: auto; width: 800px; height: 300px;'>

        <span style='font-size: 67px; color: White; top:70px; left:5px; position: relative;'>
            กำลังจัดชุดควิซอยู่..รอก่อนค่ะ</span>
        <a id='gotoStep1' style='font-size: 69px; color: White;
            top: 135px; left: 85px; position: relative;cursor:pointer;'>หรือ<u>คลิก</u>เพื่อไปจัดชุดค่ะ</a>
        <img style='position: relative; top:150px; right: -180px;' src="../Images/LogoQ.png" />

    </div>
    </form>
</body>
</html>
