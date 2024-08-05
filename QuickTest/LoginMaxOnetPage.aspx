<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="LoginMaxOnetPage.aspx.vb" Inherits="QuickTest.LoginMaxOnetPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server" id="HeaderPage">
    <title>
   
    </title>
 
    <link href="css/jquery-ui-1.8.18.custom.min.css" rel="stylesheet" />

    <style type="text/css">
        #DivMain {
            border-radius: 1em;
            position: absolute;
            padding: 20px 60px 20px 60px;
            margin-left: 50%;
            left: -257px;
            margin-top: 10%;
            border-style: solid;
            border-color: white;
            border-width: 9px;
        }
        #btnRegister{
            width: 150px; height: 100px; border-radius: 0.5em; font-size: 20px;  margin-top: 10px;color: white; font-weight: bold;
        }

        #btnLogin {
            width: 200px; height: 60px; border-radius: 0.5em; font-size: 25px; margin-top: 30px;color: white;
        }

        .trHeader {
            height: 70px; text-align: center; font-size: 25px;
        }
        .trDetail {
            height: 40px;
        }
        .txtInput {
            border-radius: 10px; height: 22px; width: 250px;padding-left: 10px;
        }
        .blockUI blockMsg blockPage,blockUI blockOverlay {
            display:none!important;
        }
        td {
                padding-top: 20px;
        }
    </style>

    <script type="text/javascript" src="js/jquery-1.7.1.js"></script>
    <script type="text/javascript" src="js/jquery.blockUI.js"></script>
    <script type="text/javascript" src="js/jquery-ui-1.8.18.min.js"></script>
    <script type="text/javascript" src="js/CheckAndSetNowDevice.js"></script>
    <script type="text/javascript" src="js/GFB.js"></script>

    <script type="text/javascript">

        var DeviceId
        var Token

        function NewFastButton() {
            window.location = '<%=ResolveUrl("~")%>practicemode_pad/ChooseTestsetMaxOnet.aspx?deviceUniqueId=' + DeviceId + '&token=' + Token;
        }

        function DeleteFastButton() { }

        function callDialog(txt) {
            var $d = $('#dialog');
            var myBtn = {};
            myBtn["ตกลง"] = function () {
                $d.dialog('close');
            };
            $d.dialog({ buttons: myBtn, draggable: false, resizable: false, modal: true }).dialog('option', 'title', txt);
        }

        $(document).ready(function () {
            var baseURL = "<%=ResolveUrl("~")%>";
            var cssName = '<%=cssName%>';

            $('head').append(cssName);

            $.unblockUI();

            $('#btnRegister').click(function () {
                window.location = '<%=ResolveUrl("~")%>MaxOnet/RegisterDevice.aspx';
            });

            $('#btnLogin').click(function () {
                var UserName;
                UserName = $('#txtUserName').val(); 
                if (UserName == '') {
                    callDialog('กรอกชื่อผู้ใช้ก่อนนะคะ');
                    return 0;
                }

                var Password;
                Password = $('#txtPassword').val();
                if (Password == '') {
                    callDialog('กรอกรหัสผ่านก่อนนะคะ');
                    return 0;
                }

                var USPW = JSON.stringify({ "UserName": UserName, "Password": Password });

                $.ajax({
                    type: "POST",
                    url: "<%=ResolveUrl("~")%>WebServices/UserPwdService.asmx/CheckMaxOnetUser",
                    async: false,
                    //data: "{ UserName :  '" + UserName + "',Password :  '" + Password + "'}",
                    data : USPW,
                    contentType: "application/json; charset=utf-8", dataType: "json",      
                    success: function (data) {
                        var a = data.d;
                        if (a == 'False') {
                            callDialog('ชื่อผู้ใช้หรือรหัสผ่านผิด ลองอีกครั้งนะคะ'); 
                        } else {
                            var arr = data.d.split(',');
                            var UserStatus = arr[0]
                            DeviceId = arr[1];
                            Token = arr[2];
                            var StudentId = arr[3];

                            if (UserStatus == 'Expired') {
                                window.location = '<%=ResolveUrl("~")%>MaxOnet/KeyCodeExpiredPage.aspx?DeviceId=' + DeviceId + '&TokenId=' + Token + '&SubjectsIdStr=' + StudentId;
                            } else if (UserStatus == 'OK') {
                                 window.location = '<%=ResolveUrl("~")%>practicemode_pad/ChooseTestsetMaxOnet.aspx?deviceUniqueId=' + DeviceId + '&token=' + Token;
                                <%--var baseURL = "<%=ResolveUrl("~")%>";
                                CheckIsNowDevice(DeviceId, Token, baseURL, StudentId)--%>
                            } else if (UserStatus == 'InstallDevice') {
                                $.ajax({
                                    type: "POST",
                                    url: "<%=ResolveUrl("~")%>MaxOnet/RegisterDevice.aspx/InstallDevice",
                                    async: false,
                                    data: "{ txtUserName :  '" + UserName + "', txtPassword :  '" + Password + "'}",
                                    contentType: "application/json; charset=utf-8", dataType: "json",
                                    success: function (msg) {
                                        if (msg.d == -1) {
                                            callDialog('ลงทะเบียนไม่สำเร็จค่ะ');
                                        } else {
                                            //alert('window.location = "MaxOnet/RegisterStudent.aspx?Token=' + msg.d + '&DeviceId=' + DeviceId + '&keycode=' + Password);
                                            window.location = "<%=ResolveUrl("~")%>MaxOnet/" + msg.d;
                                        }
                                    },
                                    error: function myfunction(request, status) {

                                    }
                                });
                            }
                        }
                    },
                    error: function myfunction(request, status) {
                        console.log(status);
                    }
                });
            });
        });

    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div id="DivMain">
            <table>
                <tr class="trHeader">
                    <td colspan="2">
                        <img class="imgLogo" src="./images/<%=runmode%>/AppLogo.png" style="width:144px;" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <table style="color: white; font-size: 20px;">
                            <tr class="trDetail">
                                <td>ชื่อผู้ใช้</td>
                                <td><input type="text" id="txtUserName" class="txtInput" /></td>
                            </tr>
                            <tr class="trDetail">
                                <td>รหัสผ่าน</td>
                                <td><input type="password" id="txtPassword" class="txtInput" /></td>
                            </tr>
                            <tr class="trHeader">
                                <%--537740781555--%>
                                <td colspan="2"><input type="button" class="RegisterPageButton" id="btnLogin" value="เข้าสู่ระบบ" /></td>
                            </tr>
                            <tr class="trDetail">
                         	    <td colspan="2">Last Update : 30/09/2563</td>
                            </tr>
                        </table>
                    </td>
                    <td style="padding-left: 25px; padding-bottom: 20px;">
                        <input type="button" id="btnRegister" class="RegisterPageButton" value="ลงทะเบียน" />
                    </td>
                </tr>
            </table> 
        </div>
        <div>
            <img class="imgChild" src="./images/<%=runmode%>/kidStanding.png" style="position: fixed;  margin-top: 27%; margin-left: 4%;" />
        </div>
        <div id="dialog"></div>
        <div id="dialog2" class="dialogSession"></div>
    </form>
</body>
</html>
