<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master"
    CodeBehind="EditHomeworkPage.aspx.vb" Inherits="QuickTest.EditHomeworkPage" %>

<%@ Register Src="../UserControl/TestsetHeaderControl.ascx" TagName="TestsetHeaderControl"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="../js/jquery-1.7.1.js" type="text/javascript"></script>
    <script src="../js/GFB.js" type="text/javascript"></script>
    <script src="../js/jquery-ui-1.8.18.js" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" href="../css/jquery-ui-1.8.18.custom.css" />
    <script type="text/javascript">
        var JSMAId = '<%=MAID %>';

        var ua = navigator.userAgent.toLowerCase();
        var isAndroid = ua.indexOf("android") > -1;

        $(function () {

            //hover animation
            InjectionHover('.Forbtn', 5);
            InjectionHover('.imgManageHomework', 5);

            //ปุ่ม,รูป แก้ไข
            $('.EditHomework').each(function () {
                new FastButton(this, TriggerEditClick);
            });

            //ปุ่ม,รูป คัดลอก
            $('.CopyHomework').each(function () {
                new FastButton(this, TriggerCopyClick);
            });

            //ปุ่ม,รูป ลบ
            $('.DeleteHomework').each(function () {
                new FastButton(this, TriggerDeleteClick);
            });

            //ดักถ้าเข้าจาก Tablet ครู
            if (isAndroid) {
                $('#main').css('width', '740px');
                $('#DivBottom').css('margin-top', '10px');
            }

            //$('.EditHomework').click(function () {
            //    parent.GotoHomeworkAssignmentPage(JSMAId, false);
            //});

            //$('.CopyHomework').click(function () {
            //    parent.GotoHomeworkAssignmentPage(JSMAId, true);
            //});

            //$('.DeleteHomework').click(function () {
            //    $('#DialogDeletHomework').dialog('open');
            //});

            $('#navigation').hide();
            $('.ulTopMenu').remove();


            $('#DialogDeletHomework').dialog({
                autoOpen: false,
                height: 350,
                width: 450,
                modal: true,
                buttons: {
                    'ตกลง': function () {
                        //alert('ลบการบ้าน');
                        $.ajax({
                            type: "POST",
                            url: "<%=ResolveUrl("~")%>Module/EditHomeworkPage.aspx/DeleteHomeworkCodeBehind",
                            data: "{ MAID: '" + JSMAId + "'}",
                            async: false,
                            contentType: "application/json; charset=utf-8", dataType: "json",
                            success: function (msg) {
                                if (msg.d != 0) {//session(UserId) หายจะ return 0 - Sefety function
                                    if (msg.d == 'Complete') {
                                        //window.location = '../TestSet/Step3.aspx';
                                        parent.CloseFancyBoxEditHomework();
                                    }
                                }
                            },
                            error: function myfunction(request, status) {
                                alert('บางอย่างผิดพลาดโปรดลองอีกครั้ง')
                            }
                        });
                    },
                    'ยกเลิก': function () {
                        $(this).dialog('close');
                    }
                }
            });

        });

        function TriggerEditClick(e) {
            parent.GotoHomeworkAssignmentPage(JSMAId, false);
        }

        function TriggerCopyClick() {
            parent.GotoHomeworkAssignmentPage(JSMAId, true);
        }

        function TriggerDeleteClick() {
            $('#DialogDeletHomework').dialog('open');
        }

    </script>
    <style type="text/css">
        #DivTop {
            padding-top: 25px;
        }

        #main {
            background-color: #D3F2F7!important;
        }

        #DivBottom, #DivTop {
            margin-top: 30px;
            padding: 10px;
            width: 95.5%;
            margin-left: 1%;
            border: 2px solid #FFA032;
            background-color: wheat;
            border-radius: 5px;
        }

        table tr td {
            width: 280px;
            text-align: center;
            background: none;
            background-color: wheat!important;
        }

        img {
            cursor: pointer;
        }

        .Forbtn {
            font: 100% 'THSarabunNew';
            border: 0;
            padding: 2px 0 3px 0;
            cursor: pointer;
            background: #1EC9F4;
            -moz-border-radius: .5em;
            -webkit-border-radius: .5em;
            border-radius: .5em; /*behavior:url(border-radius.htc);*/
            color: #444;
            font-weight: bold;
            background: #FFA032;
            behavior: url('../css/PIE.htc');
            -pie-track-active: false;
            width: 190px;
            height: 50px;
            font-size: 25px;
            border: 3px solid white;
            background: linear-gradient(to bottom, #faa51a 0%,#f47a20 100%);
            color: white;
        }

        table tr th, table tr td {
            text-align: center !important;
        }

        body, html {
            background: initial!important;
            background-color: #D3F2F7!important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id='main' style='width: 850px;'>
        <uc1:TestsetHeaderControl ID="TestsetHeaderControl1" runat="server" />

        <div id='DivBottom'>
            <table>
                <tr>
                    <td>
                        <img id='EditHomeWork' class='EditHomework imgManageHomework' src="../images/Homework/btnEditHomework-edit.png" />
                    </td>
                    <td>
                        <img id='CopyHomework' class='CopyHomework imgManageHomework' src="../images/Homework/btnEditHomework-copy.png" />
                    </td>
                    <td>
                        <img id='DeleteHomework' class='DeleteHomework imgManageHomework' src="../images/Homework/btnEditHomework-delete.png" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <input type='button' class='EditHomework Forbtn' id='btnEditHomework' value='แก้ไข' />
                        <%--<span class='ForSpn'>แก้ไข</span>--%>
                    </td>
                    <td>
                        <input type="button" class='CopyHomework Forbtn' id='btnCopyHomework' value='คัดลอก' />
                        <%--<span class='ForSpn'>คัดลอก</span>--%>
                    </td>
                    <td>
                        <input type="button" class='DeleteHomework Forbtn' id='btnDeleteHomework' value='ลบ' />
                        <%--<span class='ForSpn'>ลบ</span>--%>
                    </td>
                </tr>
            </table>
        </div>
        <div id='DialogDeletHomework' title='ลบการบ้าน'>
            <p style='font-weight: bold;'>
                ถ้าทำการลบการบ้านนี้จะทำให้ข้อมูลการบ้านของนักเรียนหายไปแน่ใจนะค่ะ ?
            </p>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ExtraScript" runat="server">
</asp:Content>
