<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ResetAdminPassword.aspx.vb" Inherits="QuickTest.ResetAdminPassword" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../css/reset.css" rel="stylesheet" />
    <link href="../css/style.css" rel="stylesheet" />
    <link href="../css/iframestyle.css" rel="stylesheet" />
    <link href="../css/contactstyle.css" rel="stylesheet" />
    <link href="../css/menuFixReviewAns.css" rel="stylesheet" />
    <link href="../css/jquery-ui-1.8.18.custom.css" rel="stylesheet" />
    <script src="../js/jquery-1.7.1.js"></script>
    <script src="../js/jquery-ui-1.8.18.js"></script>
    <style type="text/css">
        table td {
            padding: 10px !important;
            font-size: 40px;
            text-align: center !important;
            border-radius: 6px;
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
    </style>
    <telerik:RadCodeBlock runat="server" ID="Radcodeblock1">

        <script type="text/javascript">
            $(function () {
                $('#dialogWarning').dialog({
                    autoOpen: false,
                    resizable: false,
                    modal: true,
                    buttons: {
                        'ตกลง': function () {
                            $(this).dialog("close"); //$('#dialogConfirm1').dialog('open');
                        }
                    }
                });
                $('#dialogWarning2').dialog({
                    autoOpen: false,
                    resizable: false,
                    modal: true,
                    buttons: {
                        'ตกลง': function () {
                            $(this).dialog("close");
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
                            //window.location = '<%=ResolveUrl("~")%>Loginpage.aspx';
                            window.location = 'http://pointplus.iknow.co.th/Loginpage.aspx';
                    }
                }
            });
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
                        $('#dialogConfirm2').dialog('open');
                    }
                }
            });
            $('#dialogConfirm2').dialog({
                autoOpen: false,
                resizable: false,
                modal: true,
                buttons: {
                    "ยกเลิก": function () {
                        $(this).dialog("close");
                    },
                    'ตกลง': function () {
                        $(this).dialog("close");
                        ResetUserPwd('admin');
                    }
                }
            });
            $('#BtnSubmit').click(function () {
                var InputKey = $('#RadMaskedTextBox1').val();
                if (InputKey.length == 39) {
                    CheckKeyIsCorrect(InputKey);
                } else {
                    $('#dialogWarning').dialog('open');
                }
            });
        });

        function CheckKeyIsCorrect(key) {
            $.ajax({
                type: "POST",
                url: "<%=ResolveUrl("~")%>WebServices/UserPwdService.asmx/CheckKeyIsCorrect",
                data: "{ InputKey: '" + key + "'}",
                contentType: "application/json; charset=utf-8", dataType: "json",
                success: function (msg) {
                    if (msg.d == 'True') {
                        $('#dialogConfirm1').dialog('open');
                        $('#spnWarning').hide();
                    }
                    else {
                        $('#dialogWarning2').dialog('open');
                    }
                },
                error: function myfunction(request, status) {
                    //alert('บางอย่างผิดพลาดโปรดลองอีกครั้ง')
                }
            });
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
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
        </telerik:RadScriptManager>
        <div id="main">
            <header style="height: 220px;">
                <div id="logo" style="padding-top: 5px">
                    <div id="logo_text">
                        <img class="imgLogo" style="vertical-align: middle; margin: 0 50px 0 230px;" src="../images/logoQ.png" alt="logo" />
                    </div>
                </div>
            </header>
            <!--[if lte IE 7]><br /><br /><br /><![endif]-->
            <div id="site_content">

                <div class="content" style="margin-left: 50px;">
                    <div class="form_settings" style='width: 830px;'>
                        <table>
                            <tr>
                                <td>Reset รหัสผ่าน ของ admin เป็นค่าเริ่มต้น</td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:RadMaskedTextBox ID="RadMaskedTextBox1" ClientIDMode="Static" runat="server" Width="620" SelectionOnFocus="SelectAll" Label="KEY:" Font-Size="30px"
                                        Mask="aaaa-aaaa-aaaa-aaaa-aaaa-aaaa-aaaa-aaaa">
                                    </telerik:RadMaskedTextBox>
                                    <div>
                                        <span id="spnWarning" style="float: initial; color: red; display: none;">คีย์ไม่ถูกต้อง - กลับสู่หน้าหลัก</span>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td><span>( ใช้รหัสลับสำหรับลงทะเบียนซ้ำ จากแผ่นการ์ดในกล่อง DVD โปรแกรมลิขสิทธิ์ )</span></td>
                            </tr>
                        </table>
                        <div style="width: 830px; margin-left: auto; margin-right: auto; margin-top: 30px; text-align: center;">
                            <input type="button" id="BtnSubmit" value="ยืนยัน" style="position: relative; width: 150px; height: 55px; font-size: 30px; background: #46C4DD; background: -webkit-gradient(linear, left top, left bottom, from(#63CFDF), to(#17B2D9)); background: -moz-linear-gradient(top,  #63CFDF,  #17B2D9); border: solid 1px #0D8AA9;" />
                        </div>
                    </div>
                </div>
            </div>
            <footer>
                สงวนลิขสิทธิ์ &copy; บริษัท สำนักพิมพ์วัฒนาพานิช จำกัด
            </footer>
        </div>
        <div id="dialogConfirm1" title="ต้องการ Reset รหัสผ่าน ?">
            <div style="padding: 10px;">
                <span>ต้องการ Reset รหัสผ่าน admin เป็น network</span><br />
                <span>แน่นะคะ!!</span>
            </div>
        </div>
        <div id="dialogConfirm2" title="ยืนยันการ Reset รหัสผ่าน">
            <div style="padding: 10px;">
                <span>ยืนยันเปลี่ยนรหัสผ่านเป็น network แน่นะคะ</span>
                <br />
                <span>ถ้ามี admin หลายคน อย่าลืมบอกกันด้วยนะคะ!!</span>
            </div>
        </div>
        <div id="dialogWarning" title="แจ้งเตือน">
            <div style="padding: 10px;">
                <span>ใส่รหัสไม่ครบค่ะ!!</span>
            </div>
        </div>
        <div id="dialogWarning2" title="แจ้งเตือน">
            <div style="padding: 10px;">
                <span>คีย์ไม่ถูกต้องค่ะ!!</span>
            </div>
        </div>
        <div id="dialogSuccess" title="Reset รหัสผ่านเรียบร้อย">
            <div style="padding: 10px;">
                <span>Reset รหัสผ่านเรียบร้อยแล้วค่ะ</span><br />
                <span>ไปหน้าเข้าใช้งาน Pointplus</span>
            </div>
        </div>
    </form>
</body>
</html>
