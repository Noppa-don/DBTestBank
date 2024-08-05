<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Cleanuppage.aspx.vb" Inherits="QuickTest.Cleanuppage" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>คลังข้อสอบออนไลน์ by วัฒนาพานิช</title>
    <meta name="description" content="website description" />
    <meta name="keywords" content="website keywords, website keywords" />
    <script src="js/jquery-1.7.1.js" type="text/javascript"></script>
    <script src="../js/jquery-ui-1.8.18.js"></script>
    <link href="../css/jquery-ui-1.8.18.custom.css" rel="stylesheet" />
    <link rel="stylesheet" href="./css/reset.css" type="text/css" />
    <link rel="stylesheet" type="text/css" href="./css/style.css" />
    <link rel="stylesheet" type="text/css" href="./css/iframestyle.css" />
    <link rel="stylesheet" type="text/css" href="./css/contactstyle.css" />
    <link href="css/menuFixReviewAns.css" rel="stylesheet" type="text/css" />
    <!--[if lt IE 9]>
        <script src="html5.js"><
        /script>
    <![endif]-->
    <telerik:RadCodeBlock runat="server" ID="Radcodeblock1">


        <script type="text/javascript">
            var IsAll;
            $(function () {

                $('#DivExportBackUpFile').dialog({
                    autoOpen: false,
                    resizable: false,
                    modal: true,
                    buttons: {
                        "ต้องการ": function () {
                            $(this).dialog("close");
                            $.ajax({
                                type: "POST",
                                url: "<%=ResolveUrl("~")%>Cleanuppage.aspx/ExportBackUpFile",
                                data: "{IsAll : '" + IsAll + "'}",
                                contentType: "application/json; charset=utf-8", dataType: "json",
                                success: function (msg) {
                                    if (msg.d == 'Complete') {
                                        $('#DivdialogConfirm1').dialog('open');
                                    }
                                },
                                error: function myfunction(request, status) {
                                    //alert('บางอย่างผิดพลาดโปรดลองอีกครั้ง')
                                }
                            });
                       
                        },
                        'ไม่ต้องการ': function () {
                            $(this).dialog("close");
                            $('#DivdialogConfirm1').dialog('open');
                        }
                    }
                });
                
                $('#DivdialogConfirm1').dialog({
                    autoOpen: false,
                    resizable: false,
                    modal: true,
                    buttons: {
                        "ยืนยันลบทั้งหมด": function () {
                            $(this).dialog("close");
                            $('#DivdialogConfirm2').dialog('open');
                        },
                        'ไม่ลบ': function () {
                            $(this).dialog("close");
                        }
                    }
                });

                $('#DivdialogConfirm2').dialog({
                    autoOpen: false,
                    resizable: false,
                    modal: true,
                    buttons: {
                        "ลบทันที": function () {
                            $(this).dialog("close");
                            $.ajax({
                                type: "POST",
                                url: "<%=ResolveUrl("~")%>Cleanuppage.aspx/DeleteAllData",
                                data: "{IsAll : '" + IsAll + "'}",
                                contentType: "application/json; charset=utf-8", dataType: "json",
                                success: function (msg) {
                                    if (msg.d == 'Complete') {
                                        $('#RadMaskedTextBox1_text').hide();
                                        $('#RadMaskedTextBox1_Label').hide();
                                        $("#chkDeleteAll").next().hide();
                                        $("#chkDeleteAll").hide();
                                        $('#spnWarning').text('ลบข้อมูลเสร็จเรียบร้อยแล้วค่ะ');
                                        $('#spnWarning').css('color', 'green');
                                        $('#spnWarning').show();
                                        setTimeout(function () {
                                            //window.location = '<%=ResolveUrl("~")%>Loginpage.aspx';
                                            window.location = 'http://pointplus.iknow.co.th/Loginpage.aspx';
                                        }, 10000);
                                    }
                                },
                                error: function myfunction(request, status) {
                                    //alert('บางอย่างผิดพลาดโปรดลองอีกครั้ง')
                                }
                            });
                        },
                        'ไม่ลบ': function () {
                            $(this).dialog("close");
                        }
                    }
                });

                $('#BtnSubmit').click(function () {
                    var InputKey = $('#RadMaskedTextBox1').val();
                    var lenInputKey = InputKey.length;
                    if (lenInputKey == 39) {
                        $.ajax({
                            type: "POST",
                            url: "<%=ResolveUrl("~")%>Cleanuppage.aspx/CheckKeyIsCorrect",
                            data: "{ InputKey: '" + $('#RadMaskedTextBox1').val() + "'}",
                            contentType: "application/json; charset=utf-8", dataType: "json",
                            success: function (msg) {
                                if (msg.d == 'Correct') {
                                    IsAll = $("#chkDeleteAll").attr('checked') === 'checked' ? true : false;
                                    $('#DivExportBackUpFile').dialog('open');
                                    $('#spnWarning').hide();
                                }
                                else {
                                    $('#BtnSubmit').attr('disabled', 'disabled');
                                    $('#BtnSubmit').css('background', '-webkit-gradient(linear, left top, left bottom, from(#FFFFFF), to(#999999))');
                                    $('#spnWarning').show();
                                    setTimeout(function () {
                                        window.location = '<%=ResolveUrl("~")%>Loginpage.aspx';
                              }, 10000);
                          }
                      },
                            error: function myfunction(request, status) {
                                //alert('บางอย่างผิดพลาดโปรดลองอีกครั้ง')
                            }
                        });

              }
              else {
                  alert('กรอกรหัสไม่ครบค่ะ');
              }

                });
            });


        </script>

    </telerik:RadCodeBlock>


    <style type="text/css">
        table td {
            padding: 10px !important;
            font-size: 40px;
            text-align: center !important;
            border-radius: 6px;
        }

        .ui-dialog .ui-dialog-buttonpane button {
            margin-left: 5px !important;
            margin-right: 10px !important;
        }

        #chkDeleteAll, label[for="chkDeleteAll"] {
            font-size: 26px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
        </telerik:RadScriptManager>
        <div id="main">
            <header style="height: 300px;">
                <div id="logo" style="padding-top: 5px">
                    <div id="logo_text">
                        <img class="imgLogo" style="vertical-align: middle; margin: 0 50px 0 230px;" src="./images/logoQ.png" alt="logo" />
                    </div>
                </div>
            </header>
            <!--[if lte IE 7]><br /><br /><br /><![endif]-->
            <div id="site_content">

                <div class="content" style="margin-left: 50px;">
                    <div class="form_settings" style='width: 830px;'>
                        <table>
                            <tr>
                                <td>ล้างข้อมูล</td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:RadMaskedTextBox ID="RadMaskedTextBox1" ClientIDMode="Static" runat="server" Width="750" SelectionOnFocus="SelectAll" Label="KEY:" Font-Size="30px"
                                        Mask="aaaa-aaaa-aaaa-aaaa-aaaa-aaaa-aaaa-aaaa">
                                    </telerik:RadMaskedTextBox>
                                    <div>
                                        <%--<asp:Label ID="lblWarning" style="float:initial;" ForeColor="Red" runat="server" Text="" Visible="false"></asp:Label>--%>
                                        <span id="spnWarning" style="float: initial; color: red; display: none;">คีย์ไม่ถูกต้อง - กลับสู่หน้าหลัก</span>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>

                                    <asp:CheckBox ID="chkDeleteAll" ClientIDMode="Static" runat="server" Text="ลบข้อสอบที่จัดชุดไว้ทั้งหมด ?" />
                                </td>
                            </tr>
                        </table>
                        <div style="width: 830px; margin-left: auto; margin-right: auto; margin-top: 30px; text-align: center;">
                            <%--<asp:Button ID="BtnSubmit" runat="server" Width="100px" Text="ตกลง" style="position:relative;width:150px;height:55px;font-size:30px;" class="submit" />--%>
                            <input type="button" id="BtnSubmit" value="ตกลง" style="position: relative; width: 150px; height: 55px; font-size: 30px; background: #46C4DD; background: -webkit-gradient(linear, left top, left bottom, from(#63CFDF), to(#17B2D9)); background: -moz-linear-gradient(top,  #63CFDF,  #17B2D9); border: solid 1px #0D8AA9;" />
                        </div>
                    </div>
                </div>

            </div>
            <%--<asp:Button ID="Button1" runat="server" Text="Button" />--%>
            <footer>
                <%--<a href="http://www.wpp.co.th"></a>--%>สงวนลิขสิทธิ์ &copy; บริษัท สำนักพิมพ์วัฒนาพานิช จำกัด
            </footer>
        </div>
        <div id="DivdialogConfirm1" title="ต้องการล้างข้อมูลที่มีอยู่ทั้งหมด ?">
            ประวัติการใช้งานทุกอย่าง รวมถึงรายชื่อต่างๆ
            <br />
            จะถูกล้างทั้งหมดนะคะ!!
        </div>
        <div id="DivExportBackUpFile" title="โหลดไฟล์ Excel">
            ต้องการโหลดข้อมูลเป็น Excel เก็บไว้หรือไม่
        </div>
        <div id="DivdialogConfirm2" title="การลบข้อมูลจะกู้กลับ <br />หรือย้อนกลับไม่ได้นะคะ ?">
            <br />
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;แน่ใจนะคะ!!<br />
        </div>
    </form>
</body>
</html>
