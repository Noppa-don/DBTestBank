<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="RepeaterReportPractice.aspx.vb" Inherits="QuickTest.RepeaterReportPractice" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0" name="viewport" />
    <title></title>

    <script src="../js/jquery-1.7.1.js" type="text/javascript"></script>
    <script src="../js/GFB.js" type="text/javascript"></script>
    <script src="../js/jquery-ui-1.8.18.js" type="text/javascript"></script>
    <script src="../js/jquery.placeholder.js" type="text/javascript"></script>
    <link href="../css/newStyle.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        body {
            background: none;
        }

        #main {
            width: 840px;
            background: none;
        }

        #lblManageTestset {
            font-size: 130%;
        }

        #TestsetMainDiv .TestsetMenu {
            margin: 5px;
            text-align: center;
            width: 100%;
            min-height: 60px;
            background: #D3F2F7;
            color: #FFF;
            padding: 5px;
            -webkit-border-radius: 0.5em;
            -moz-border-radius: 0.5em;
            border-radius: 0.5em;
            behavior: url('../css/PIE.htc');
            -pie-track-active: false;
            margin-left: auto;
            margin-right: auto;
        }

        #ManageTestset div {
            text-align: center;
            overflow-y: auto;
            height: 400px;
        }

            #ManageTestset div table {
                width: 100%;
                border-spacing: 4px;
                margin-top: 0px;
            }

        span.icon_clear {
            position: absolute;
            right: 10px;
            display: none;
            cursor: pointer;
            font: bold 1em;
            color: Red;
            top: -3px;
        }

            span.icon_clear:hover {
                color: Blue;
            }

        span.imgFind {
            background-image: url('../images/Search.png');
            display: inline-block;
            width: 25px;
            height: 25px;
            background-size: cover;
            position: absolute;
            top: 5px;
            left: 5px;
             -ms-behavior: url('../css/backgroundsize.min.htc');
        }

        #txtSearchTestset {
            width: 140px;
            padding-left: 30px;
            padding-right: 30px;
            height: 30px;
            font: 20px 'THSarabunNew';
        }

        a, tr, label, .TestsetMenu {
            cursor: pointer;
        }

        .ui-dialog .ui-dialog-buttonpane {
            padding: .3em;
        }

            .ui-dialog .ui-dialog-buttonpane .ui-dialog-buttonset {
                width: 100%;
            }

            .ui-dialog .ui-dialog-buttonpane button {
                float: right;
            }

        .ui-dialog-buttonset > button:first-child {
            float: left;
            margin: .5em 0 .5em .4em;
        }

        .bordered td:first-child, .bordered th:first-child {
            text-align: center;
        }

        table {
            font-size: 120%;
        }
    </style>

    <script type="text/javascript">
        var ua = navigator.userAgent.toLowerCase();
        var isAndroid = ua.indexOf("android") > -1;

        $(function () {
            //tr คลิกไปหน้า รายงาน
            $('tbody tr').each(function () {
                new FastButton(this, TriggerTRClick);
            });

            $('.icon_clear').click(function () {
                $(this).delay(300).fadeTo(300, 0).prev('#txtSearchTestset').val('');
                ShowAllMatchSearch();
            });

            if ($.browser.msie && $.browser.version <= 9.0) {
                $('#txtSearchTestset').placeholder();
            }

            //ดักถ้าเป็น Tablet ของครู
            if (isAndroid) {
                $('#main').css('width', '740px');
                $('#repeaterTestsetDiv').css('height', '280px');
                $('table tr td').css('font-size', '20px');
            }

        });

        function TriggerTRClick(e) {
            var obj = $(e.target).parent();
            var TestsetId = $(obj).attr('TestsetId');
            ShowReport(TestsetId);
        }

        //function toggleMenuTestset() {
        //    if ($(".TestsetDiv").hasClass('slide_True')) {
        //        $(".TestsetDiv").removeClass('slide_True');
        //        $(".TestsetDiv").slideToggle();

        //    } else {
        //        $(".TestsetDiv").addClass('slide_True');
        //        $(".TestsetDiv").slideToggle();
        //    }
        //    if ($.browser.msie) {
        //        if ($.browser.version <= 7) {
        //            $('.TestsetDiv').css('overflow', 'auto');
        //        }
        //    }
        //}

        function ClearNotMatchSearch(txtSearch) {
            var table = $('.bordered');
            table.children('tbody').children('tr').each(function () {
                var tag = $(this).attr('tag');
                var IsShow = $(this).css('display');
                if (tag.toLowerCase().indexOf(txtSearch) == -1) {
                    if (IsShow != "none") {
                        $(this).hide();
                    }
                }
                else {
                    if (IsShow == "none") {
                        $(this).show();
                    }
                }
            });
        }

        var delayID = null;
        function SearchTestset() {
            if (!($('.TestsetDiv').hasClass('slide_True'))) {
                $(".TestsetDiv").slideToggle();
                $(".TestsetDiv").addClass('slide_True');
            }
            var prevValue = '';
            if (delayID != null) {
                clearInterval(delayID);
            }
            delayID = setInterval(function () {
                var t = $('#txtSearchTestset').val();
                if (prevValue != t) {
                    ClearNotMatchSearch(t.toLowerCase());
                    prevValue = t;
                }
                if (prevValue == '') {
                    $('.icon_clear').delay(300).fadeTo(300, 0);
                }
                else {
                    $('.icon_clear').stop().fadeTo(300, 1);
                }
            }, 500);
        }

        function ClearTimeInterval() {
            clearInterval(delayID);
        }

        function ShowAllMatchSearch() {
            var table = $('.bordered');
            table.children('tbody').children('tr').each(function () {
                var IsShow = $(this).css('display');
                if (IsShow == 'none') {
                    $(this).show();
                }
            });
        }

        function ShowReport(InputTestSetId) {
            window.location = '<%=ResolveUrl("~")%>Activity/ActivityReport.aspx?TestsetId=' + InputTestSetId + '&ReportMenu=3&ShowBtnBack=True';
        }

    </script>


</head>
<body>
    <form id="form1" runat="server">
        <div id='main'>
            <div id="TestsetMainDiv" style='padding: 25px;'>
                <div id="ManageTestset" class="TestsetMenu">
                    <h3>
                        <label id="lblManageTestset" class="old" runat="server" onclick="toggleMenuTestset();">
                            ดูผลการฝึกฝน(เฉพาะชุดที่ครูจัด)
                        </label>
                        <span style="position: relative; margin-left: 1em;"><span class="imgFind"></span>
                            <input type="text" id="txtSearchTestset" placeholder="ค้นหา" onfocus="SearchTestset();"
                                onblur="ClearTimeInterval();" />
                            <span class="icon_clear">X</span> </span>
                    </h3>
                    <div id="repeaterTestsetDiv" class="TestsetDiv slide_True" runat="server">
                        <asp:Repeater ID="Listing" runat="server">
                            <HeaderTemplate>
                                <table class="bordered">
                                    <thead>
                                        <tr>
                                            <th style="width: 15%;">เข้าทำ(คน)
                                            </th>
                                            <th style="width: 85%;">ชื่อชุด(เฉพาะชุดที่ครูจัด)
                                            </th>
                                        </tr>
                                    </thead>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr tag="<%# Container.DataItem("TotalStudentPractice")%>,<%# Container.DataItem("TestSet_Name")%>"
                                    TestsetId="<%# Container.DataItem("TestSet_Id")%>">
                                    <td style="background: #FFFFCC;">
                                        <%# Container.DataItem("TotalStudentPractice")%>
                                    </td>
                                    <td style="background: #FFFFCC; text-align: left;padding-left:10px;">
                                        <%# Container.DataItem("TestSet_Name")%>
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <AlternatingItemTemplate>
                                <tr tag="<%# Container.DataItem("TotalStudentPractice")%>,<%# Container.DataItem("TestSet_Name")%>"
                                    TestsetId="<%# Container.DataItem("TestSet_Id")%>">
                                    <td style="background: #FFFFFF;">
                                        <%# Container.DataItem("TotalStudentPractice")%>
                                    </td>
                                    <td style="background: #FFFFFF; text-align: left;padding-left:10px;">
                                        <%# Container.DataItem("TestSet_Name")%>
                                    </td>
                                </tr>
                            </AlternatingItemTemplate>
                            <FooterTemplate>
                                </table>
                            </FooterTemplate>
                        </asp:Repeater>
                    </div>
                </div>

            </div>
        </div>

    </form>
</body>
</html>
