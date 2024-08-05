<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="purgeClass.aspx.vb" Inherits="QuickTest.purgeClass" %>

<%@ Register Assembly="Telerik.web.ui" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <script src="../js/jquery-1.7.1.js" type="text/javascript"></script>
    <script src="../js/jquery-ui-1.8.18.js" type="text/javascript"></script>
    <script src="../js/jquery.qtip-1.0.0-rc3.js" type="text/javascript"></script>
    <script src="../js/jquery.prettyLoader.js" type="text/javascript"></script>
    <link href="../css/prettyLoader.css" rel="stylesheet" type="text/css" />
    <link href="../css/styleQuiz.css" rel="stylesheet" type="text/css" />
    <link href="../css/jquery-ui-1.8.18.custom.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        $(document).ready(function () {

            $('div .roomInClass').click(function () {
                $(this).toggleClass('deleteClass');
                var roomSelected = '';
                $('.roomInClass.deleteClass').each(function () {
                    var id = $(this).attr('id');
                    roomSelected += ',' + id;
                });
                $('#hidRoomSelected').val(roomSelected);

            });


            $('.divBackToStep1').click(function () {
                window.parent.location.href = '/testset/step1.aspx';
            });

            //            $('#btnSave').click(function () {
            //                $('#dialog').dialog('open');
            //            });

            $('.headClass').click(function () {
                // $(this).toggleClass('deleteClass');
                var t = $(this);
                //for (var i = 1; i < 5; i++) {
                //  $(t).next().toggleClass('deleteClass');
                //  t = $(t).next();
                //}

                $(t).nextAll().removeClass('deleteClass');

                if ($(t).hasClass('CheckDelete')) {
                    $(t).removeClass('CheckDelete');
                }
                else {
                    $(t).addClass('CheckDelete');
                    $(t).nextAll().addClass('deleteClass');
                }
                var roomSelected = '';
                $('.roomInClass.deleteClass').each(function () {
                    var id = $(this).attr('id');
                    roomSelected += ',' + id;
                });
                $('#hidRoomSelected').val(roomSelected);
                //alert($('#hidRoomSelected').val());
            });

            $('#rdSomeClass').change(function () {

                if ($('#rdAllYear').attr('Checked') == 'checked') {


                    $('#divGradient').show();
                }
                else {

                    $('#divGradient').hide();
                }
            });

            $('#rdSelectedYear').change(function () {

                if ($('#rdSomeClass').attr('Checked') == 'checked') {

                    $('#divGradient').hide();
                }
                else {

                    $('#divGradient').show();
                }
            });

            $('#rdAllClass').change(function () {
                $('#divGradient').show();
            });

            $('#rdAllYear').change(function () {
                $('#divGradient').show();
            });

            $('#dialog').dialog({
                autoOpen: false,

                draggable: false,
                resizable: false,
                modal: true,
                height: "190",
                open: function () {
                    $(this).parent().appendTo("form");
                }
            });

            $('#Save').click(function (e) {
                    e.preventDefault();
                    $('#dialog').dialog('open');
                });
         
        });
    </script>
    <style type="text/css">
        .classRoom
        {
            position: relative;
            background-color: #96ceed;
            width: 678px;
            overflow: auto;
            margin-top: 2px;
            -webkit-border-radius: 0.5em;
        }
        .roomInClass
        {
            background-color: #fff;
            height: 40px;
            text-align: center;
            position: relative;
            float: left;
            margin: 5px;
            line-height: 2.2;
            -webkit-border-radius: 0.5em;
            cursor: pointer;
        }
        .headClass
        {
            background-color: #96ceed;
            width: 80px;
            height: 50px;
            text-align: center;
            float: left;
            line-height: 2.7;
            -webkit-border-radius: 0.5em;
            cursor: pointer;
        }
        .deleteClass
        {
            background-image: url('../images/upgradeClass/eraser.png');
            background-repeat: no-repeat;
            background-position: center
        }
        
        .btnSave
        {
            width: 100px;
            height: 50px;
            position: absolute;
            right: 65px;
            bottom: 10px;
        }
        
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
    </telerik:RadScriptManager>
    <div id="main">
        <div id="site_content">
            <div class="content" style="width: 930px;">
                <center>
                    <h2>
                        ล้างข้อมูลนักเรียน และประวัติการควิซ
                    </h2>
                    <div id="div-1" style="color: #47433F">
                        <asp:HiddenField ID="hidRoomSelected" runat="server" Value="" />
                        <%--  <div style="width: 60%; height: 90%; left: 20%; position: absolute; text-align: center;
        border: 1px solid #aaa;">--%>
                        <%--<div class="divBackToStep1" style="background-color: Orange;">
        </div>--%>
                        <div style="text-align: left; width: 580px; left: 18%; position: relative;">
                            <table>
                                <tr>
                                    <td>
                                        <asp:RadioButton runat="server" Text="" ID="rdSelectedYear" GroupName="year" Checked="true" />
                                        ล้างเฉพาะปีการศึกษา
                                   <asp:DropDownList ID="cbAcademicYear" runat="server" AutoPostBack="True" Font-Names="THSarabunNew" Height="40px" >
                                      
                                        </asp:DropDownList>
                                       <%-- <telerik:RadComboBox ID="cbAcademicYear" runat="server" AutoPostBack="true" Width="70px"
                                            Height="80px">
                                        </telerik:RadComboBox>--%>
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:RadioButton ID="rdAllYear" runat="server" Text=" ล้างข้อมูลทั้งหมด" GroupName="year" /><br />
                                    </td>
                                </tr>
                            </table>
                            <asp:RadioButton ID="rdAllClass" runat="server" Text=" ทั้งโรงเรียน" GroupName="class"
                                Checked="true" />
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:RadioButton ID="rdSomeClass" runat="server" Text=" บางชั้นบางห้อง" GroupName="class" />
                            <br />
                        </div>
                        <br />
                        <div id="ClassAndRoom" runat="server" style="left: 80px; text-align: center; position: relative;
                            width: auto; height: auto; top: auto; overflow:auto">
                        </div>
                        <br />
                       
                    </div>
                     <div class="form_settings">
                            <asp:Button ID="BtnBack" runat="server" Text="กลับ" class="submit" Style="margin: 0 0 0 -283px;
                                width: 200px; position: relative;" />
                            <asp:Button ID="Save" runat="server" Text="บันทึก" class="submit" Style="float: Right;
                                right: 20px; width: 200px; position: relative;" />
                            <%--Style="float: right; height: 50px;width: 80px;"--%>
                        </div>
                    <%--</div>--%>
                    <footer style="margin-top: 40px">
                            <a href="http://www.wpp.co.th">สงวนลิขสิทธิ์ &copy; บริษัท สำนักพิมพ์วัฒนาพานิช จำกัด </footer>
                </center>
            </div>
        </div>
    </div>
    <div id="dialog" title="ถ้าทำการลบข้อมูล ข้อมูลการควิซจะถูกลบทั้งหมด แน่ใจนะคะ? พิมพ์ แน่ใจ ในช่องว่างเพื่อยืนยันค่ะ" style="height:100px;">
        <asp:TextBox ID="txtConfirm" runat="server" Style="margin-left: 30px;margin-top: 2px;"></asp:TextBox>
        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtConfirm" ValidationExpression="แน่ใจ" ErrorMessage="พิมพ์คำว่า แน่ใจ นะคะ"></asp:RegularExpressionValidator>
        <asp:Button ID="btnSave" runat="server" Text="ตกลง" class="submit" Style="float: left;
            left: 10px; top: 33px; width: 60px; position: relative;" />
        <asp:Button ID="btnCancel" runat="server" Text="ยกเลิก" class="submit" Style="float: Right;
            right: 10px; width: 60px; top: 7px; position: relative;" />
    </div>

    <%--  <asp:Button ID="btnSave" runat="server" CssClass="btnSave" Text="บันทึก"></asp:Button>--%>
    </form>
</body>
</html>
