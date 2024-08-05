<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="LoginProblemQuestion.aspx.vb" Inherits="QuickTest.LoginProblemQuestion" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
        <link href="../css/jquery-ui-1.8.18.custom.min.css" rel="stylesheet" />
    <style type="text/css">
        .txtStyle {
                margin-left: 20px;
   margin-bottom: 28px;
    border-radius: 7px;
    border-style: solid;
    border-color: #a3d2d4;
    width: 250px;
    height: 30px;
     color: #025458;
         font-size: 16px;
         padding-left:5px;
        }

        #btnLogin {
               border-radius: 15px;
    width: 252px;
    height: 55px;
    background-color: #025458;
    color: #e3f4f5;
    font-size: 30px;
    margin-top: 15px;
        }
        .ui-widget-header {
    border: 1px solid #0c4649!important;
    background: #0c4649!important;
        }
        .ui-dialog .ui-dialog-buttonpane button {
            color: #0c4649!important;
        }
    </style>

        <script type="text/javascript" src="../js/jquery-1.7.1.js"></script>
    <script type="text/javascript" src="../js/jquery.blockUI.js"></script>
    <script type="text/javascript" src="../js/jquery-ui-1.8.18.min.js"></script>
    <script type="text/javascript" src="../js/GFB.js"></script>
    <script type="text/javascript">

        $(document).ready(function () {
            function callDialog(txt) {
                var $d = $('#dialog');
                var myBtn = {};
                myBtn["ตกลง"] = function () {
                    $d.dialog('close');
                };
                $d.dialog({ buttons: myBtn, draggable: false, resizable: false, modal: true }).dialog('option', 'title', txt);
            }

            function readApproveFile(UserId) {
                var rawFile = new XMLHttpRequest();
                rawFile.open("GET", '<%=ResolveUrl("~")%>Support/ApproveUser.txt' , false);
                rawFile.onreadystatechange = function () {
                    if (rawFile.readyState === 4) {
                        if (rawFile.status === 200 || rawFile.status == 0) {
                            var allText = rawFile.responseText;
                            var n = allText.toLowerCase().indexOf(UserId);
                            if (n == -1) {
                                readEditorFile(UserId)
                            } else {
                                window.location = '<%=ResolveUrl("~")%>Support/ProblemQuestionSummary.aspx?pmt=1&usid=' + UserId;
                            }
                          
                        }
                    }
                }
                rawFile.send(null);
            }

            function readEditorFile(UserId) {
                var rawFile = new XMLHttpRequest();
                rawFile.open("GET", '<%=ResolveUrl("~")%>Support/EditorUser.txt', false);
                rawFile.onreadystatechange = function () {
                    if (rawFile.readyState === 4) {
                        if (rawFile.status === 200 || rawFile.status == 0) {
                            var allText = rawFile.responseText;
                            //alert(allText);
                            var n = allText.toLowerCase().indexOf(UserId);
                            if (n == -1) {
                                callDialog('User นี้ยังไม่ได้รับสิทธิ์เข้าระบบ กรุณาติดต่อ Support ค่ะ');
                            } else {
                                window.location = '<%=ResolveUrl("~")%>Support/ProblemQuestionSummary.aspx?pmt=2&usid=' + UserId;
                            }
                          
                        }
                    }
                }
                rawFile.send(null);
            }

            $('#btnLogin').click(function () {
             
                var UserName;
                UserName = $('#txtUer').val();
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
                    url: "<%=ResolveUrl("~")%>WebServices/UserPwdService.asmx/CheckMaxOnetUser",
                    async: false,
                    data: "{ UserName :  '" + UserName + "',Password :  '" + Password + "'}",
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
                                readApproveFile(StudentId);
                                //window.location = '<%=ResolveUrl("~")%>Support/ProblemQuestionSummary.aspx';
                            }
                        }
                    },
                    error: function myfunction(request, status) {
                    }
                });
            });
        })
    </script>
</head>
<body style="background-color:#025458;">
    <form id="form1" runat="server">
        <div style="    background-color: #e3f4f5;
    padding: 30px;
    width: 45%;
    border-radius: 15px;
    margin-left: auto;
    margin-right: auto;
    margin-top: 40px; color: #025458;">
            <table style=" margin-left: auto;
     font-size: 20px;
    margin-right: auto;">
                <tr><td colspan="2" style="text-align: center;
    padding-bottom: 30px;
    font-size: 30px;"><span>ระบบแจ้ง / บันทึก ปัญหาข้อสอบ</span></td></tr>
                <tr>
                    <td style="padding-bottom: 22px;"><asp:Label ID="lblUser" runat="server" Text="User"></asp:Label></td>
                    <td><asp:TextBox ID="txtUer" runat="server" CssClass="txtStyle"></asp:TextBox></td>
                </tr>
                <tr>
                    <td style="padding-bottom: 22px;"><asp:Label ID="lblPassword" runat="server" Text="Password"></asp:Label></td>
                    <td><asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="txtStyle"></asp:TextBox></td>
                </tr>
                <tr>
                    <td colspan="2" style="text-align: center;"><input type="button" id="btnLogin" value="Login" /></td>
                </tr>
            </table>
        </div>
          <div id="dialog"></div>
    </form>
</body>
</html>
