<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ResetGroupSubjectPassword.aspx.vb" Inherits="QuickTest.ResetGroupSubjectPassword" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../css/jquery-ui-1.8.18.custom.css" rel="stylesheet" />
    <script src="../js/jquery-1.7.1.js"></script>
    <script src="../js/jquery-ui-1.8.18.js"></script>
    <style type="text/css">
        @import url(../fonts/thsarabunnew.css);

        table td {
            text-align: center !important;
            background: #D3F2F7;
            color: #47433F;
            border-bottom: 1px solid #FFF;
            padding: 7px 0px;
            font-size: 16px;
        }        
        table tr:nth-child(odd) td {
        background: #F1F8F9;
        }

        table tr:first-child td {
            font-weight: bold;
            font-size: 18px;
            background: #65E4F9;
        }

        table tr td:first-child {
            border-right: 1px solid #FFF;
            font-weight: bold;
            background: #65E4F9;
        }

        table tr td:last-child {
            border-left: 1px solid #FFF;
        }

        #site_content {
            padding: initial;
        }

        .form_settings span {
            font-size: 25px!important;
            float: initial;
            width: initial;
        }

        .ui-dialog .ui-dialog-buttonpane button {
            margin-left: 5px !important;
            margin-right: 10px !important;
        }

        .ui-dialog-title {
            font: bold 24px 'THSarabunNew';
        }

        .form_settings input {
            width: 100px;
        }

        form, body {
            background: white;
            font: normal 0.95em 'THSarabunNew';
        }

        input {
            padding: 1px;
            width: 100px;
            font: 80% 'THSarabunNew';
            border: 1px solid #C6E7F0;
            background: #EFF8FB;
            color: #47433F;
            border-radius: 7px;
            cursor:pointer;
        }
            input:hover {
                background: #83DAF7;
            }
    </style>
    <script type="text/javascript">
        var userNameToResetPwd;
        $(function () {
            $('#dialogConfirm1').dialog({
                autoOpen: false,
                resizable: false,
                modal: true,
                buttons: {
                    "ยกเลิก": function () {
                        $(this).dialog("close");
                    },
                    'ตกลง': function () {
                        $(this).dialog("close");
                        ResetUserPwd(userNameToResetPwd);
                    }
                }
            });
            $('#dialogSuccess').dialog({
                autoOpen: false,
                resizable: false,
                modal: true,
                buttons: {
                    'ตกลง': function () {
                        $(this).dialog("close");
                    }
                }
            });
        });
        function CallDialog(userName) {
            userNameToResetPwd = userName;
            $('#spnDialogConfirm1').text("ต้องการ Reset รหัสผ่าน ให้กับชื่อผู้ใช้ " + userName + " แน่นะคะ?");
            $('#dialogConfirm1').dialog('open');
        }

        function ResetUserPwd(userName) {
            $.ajax({
                type: "POST",
                url: "<%=ResolveUrl("~")%>WebServices/UserPwdService.asmx/ResetUserPwd",
                data: "{ UserName: '" + userName + "'}",
                contentType: "application/json; charset=utf-8", dataType: "json",
                success: function (msg) {
                    if (msg.d == 'True') {
                        // show dialog success redirect to loginapage
                        $('#dialogSuccess').dialog('open');
                    }
                    else {
                        alert('บางอย่างผิดพลาดโปรดลองอีกครั้ง');
                    }
                },
                error: function myfunction(request, status) {
                    //alert('บางอย่างผิดพลาดโปรดลองอีกครั้ง')
                }
            });
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">       
        <div style="padding-top: 5px; text-align: center;">
            <span style="font-size: 20px; font-weight: bold;">Reset รหัสผ่าน ของแต่ละหมวด เป็นค่าเริ่มต้น</span>
            <div>
                <table style="width: 650px; margin-left: auto; margin-right: auto;">
                    <tr>
                        <td style="width: 250px;">กล่มสาระการเรียนรู้</td>
                        <td style="width: 250px;">ชื่อผู้ใช้</td>
                        <td>รหัสผ่าน</td>
                    </tr>
                    <tr>
                        <td>ภาษาไทย</td>
                        <td>thai</td>
                        <td>
                            <input type="button" value="reset" onclick="CallDialog('thai')" /></td>
                    </tr>
                    <tr>
                        <td>คณิตศาสตร์</td>
                        <td>math</td>
                        <td>
                            <input type="button" value="reset" onclick="CallDialog('math')" /></td>
                    </tr>
                    <tr>
                        <td>วิทยาศาสตร์</td>
                        <td>science</td>
                        <td>
                            <input type="button" value="reset" onclick="CallDialog('science')" /></td>
                    </tr>
                    <tr>
                        <td>สังคมศึกษาฯ</td>
                        <td>social</td>
                        <td>
                            <input type="button" value="reset" onclick="CallDialog('social')" /></td>
                    </tr>
                    <tr>
                        <td>สุขศึกษาฯ</td>
                        <td>health</td>
                        <td>
                            <input type="button" value="reset" onclick="CallDialog('health')" /></td>
                    </tr>
                    <tr>
                        <td>ศิลปะ</td>
                        <td>art</td>
                        <td>
                            <input type="button" value="reset" onclick="CallDialog('art')" /></td>
                    </tr>
                    <tr>
                        <td>การงานอาชีพฯ</td>
                        <td>home</td>
                        <td>
                            <input type="button" value="reset" onclick="CallDialog('home')" /></td>
                    </tr>
                    <tr>
                        <td>ภาษาต่างประเทศ</td>
                        <td>english</td>
                        <td>
                            <input type="button" value="reset" onclick="CallDialog('english')" /></td>
                    </tr>
                </table>
            </div>

        </div>
        <div id="dialogConfirm1" title="ต้องการ Reset รหัสผ่าน ?">
            <div style="padding: 10px;">
                <span id="spnDialogConfirm1">ต้องการ Reset รหัสผ่าน admin เป็น network</span><br />
                <br />
                <br />
                <span>รหัสผ่านใหม่จะเป็น&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;1234&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;ค่ะ</span>
            </div>
        </div>
        <div id="dialogSuccess" title="Reset รหัสผ่านเรียบร้อย">
            <div style="padding: 10px;">
                <span>Reset รหัสผ่านเรียบร้อยแล้วค่ะ</span>
            </div>
        </div>
    </form>
</body>
</html>
