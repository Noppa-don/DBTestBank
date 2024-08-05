<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="RegisterDevice.aspx.vb" Inherits="QuickTest.RegisterMaxonet" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" style="background-color: #00bff3;">
<head runat="server">
    <title></title>
    <link href="../css/jquery-ui-1.8.18.custom.min.css" rel="stylesheet" />
    <style type="text/css">
        #DivMain {
            border-radius: 1em; background-color: #00bff3; position: absolute; padding: 15px; margin-left: 50%; left: -150px; margin-top: 10%; 
            border-style: solid; border-color: white; border-width: 9px; color: white;
        }
        #btnRegister{
            width: 50%; height: 45px; border-radius: 0.5em; font-size: 20px; background-color: #e81062; margin-top: 10px; color:white;
        }

        .trHeader {
            height: 70px; text-align: center; font-size: 30px; font-weight:bold;
        }
        .trDetail {
            height: 40px; font-size: 20px;
        }
        .txtInput {
            border-radius: 10px; height: 22px; width: 250px;padding-left: 10px;
        }
    </style>

    <script src="../js/jquery-1.7.1.js" type="text/javascript"></script>
    <script src="<%=ResolveUrl("~")%>js/jquery-ui-1.8.18.min.js" type="text/javascript"></script>
    <script type="text/javascript">

        function callDialog(txt) {
            var $d = $('#dialog');
            var myBtn = {};
            myBtn["ตกลง"] = function () {
                $d.dialog('close');
            };
            $d.dialog({ buttons: myBtn, draggable: false, resizable: false, modal: true }).dialog('option', 'title', txt);
        }

        $(document).ready(function () {
            $('#btnRegister').click(function () {
                var UserName;
                UserName = $('#txtUsername').val();
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
                        window.location = msg.d;
                    }
                },
                error: function myfunction(request, status) {

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
                    <td colspan="2">ลงทะเบียนเข้าใช้งาน</td>
                </tr>

                <tr class="trDetail">
                    <td>
                        <table>
                            <tr>
                                <td style="padding-right: 25px;">ชื่อผู้ใช้</td>
                                <td><input type="text" id="txtUsername" runat="server" class="txtInput" /></td>
                            </tr>
                            <tr class="trDetail">
                                <td>รหัสผ่าน</td>
                                <td><input type="password" id="txtPassword" runat="server" class="txtInput" /></td>
                            </tr>
                        </table>
                      </td>  
                    </tr>
                    <tr class="trHeader">
                        <td colspan="2">
                            <input id="btnRegister" runat="server" type="button" value="ลงทะเบียน" />
                        </td>
                    </tr>
                </table>
            </div>
            <div>
                <img class="imgChild" src="../images/kidStandPointL.png" style="position: fixed; margin-top: 27%; margin-left: 82%;" />
            </div>
            <div id="dialog"></div>
        </form>
    </body>
</html>
