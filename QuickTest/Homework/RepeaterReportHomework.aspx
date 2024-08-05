<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="RepeaterReportHomework.aspx.vb" Inherits="QuickTest.RepeaterReportHomework" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="../js/jquery-1.7.1.js" type="text/javascript"></script>
    <script src="../js/GFB.js" type="text/javascript"></script>
    <script src="../js/jquery-ui-1.8.18.js" type="text/javascript"></script>
    <script src="../js/jquery.placeholder.js" type="text/javascript"></script>
    <link href="../css/newStyle.css" rel="stylesheet" type="text/css" />

    <style type="text/css">
        body {
            background: url('../images/homework/bgResultHomework.png');
                background-repeat: no-repeat;
    background-size: cover;
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
            min-height: 60px;
            background: #D3F2F7;
            color: #FFF;
            padding: 5px;            
            border-radius: 0.5em;            
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
            background-image: url('../images/homework/Search.png');
            display: inline-block;
            width: 25px;
            height: 25px;
            background-size: cover;
            position: absolute;
            top: 8px;
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
            font-size: 100%;
        }

        .TopRight {
            position: fixed;
            border: 3px solid #FFCC66;
            background-color: #FFFFCC;
            color: #000000;
            display: none;
            top: 0px;
            right: 0px;
            -moz-border-radius: 15px;
            padding: 10px;
            padding-bottom: 5px;
            padding-top: 5px;
            margin: 3px;
            -webkit-border-radius: 15px;
            border-radius: 15px;
            behavior: url(/css/PIE.htc);
        }

        .ForDescription {
            position: fixed;
            border: 3px solid #FFCC66;
            background-color: #FFFFCC;
            -moz-border-radius: 15px;
            color: #000000;
            padding: 10px;
            padding-bottom: 5px;
            padding-top: 5px;
            display: none;
            margin: 3px;
            top: 0px;
            left: 0px;
            -webkit-border-radius: 15px;
            border-radius: 15px;
            behavior: url(/css/PIE.htc);
        }

        .toolsTip {
            position: absolute;
            border: 1px solid #FFCC66;
            background-color: #FFFFCC;
            color: #000000;
            display: none;
            padding: 5px;
            width: 700px;
            font-size: 16px;
            margin: 0px auto;
        }
    </style>

    <script type="text/javascript">
        var ua = navigator.userAgent.toLowerCase();
        var isAndroid = ua.indexOf("android") > -1;

        $(function () {
            //tr คลิกไปหน้า รายงาน
            $('tbody tr').each(function (e) {                
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
                $('#repeaterTestsetDiv').css('height', '300px');
                $('table tr td').css('font-size', '20px');
            }

            //ToolTip แสดงชื่อคนที่ต้องทำการบ้าน (เมื่อยาวเกิน 50 ตัวอักษร)
            $('.AssignToIsLong').hover(function (e) {                
                callTooltip('#SpanFullDetail', e);
                var id = $(this).children('span').attr('id');
                $.ajax({
                    type: "POST",
                    url: "<%=ResolveUrl("~")%>Homework/RepeaterReportHomework.aspx/GetFullAssignTo",
                    data: "{ MaId : '" + id + "' }",  //" 
                    contentType: "application/json; charset=utf-8", dataType: "json",
                    success: function (msg) {
                        if (msg.d != 0) {//session(UserId) หายจะ return 0 - Sefety function
                            $('#SpanFullDetail').html(msg.d);
                            var widthSpan = $('#SpanFullDetail').width();
                            widthSpan = widthSpan / 2;
                            var heightSpan = $('#SpanFullDetail').height();
                            heightSpan = heightSpan / 2;
                            $('#SpanFullDetail').css('left', '50%').css('margin-left', -widthSpan + 'px').css('top', '85%').css('margin-top', -heightSpan + 'px');
                            $('#SpanFullDetail').css('width', (widthSpan * 2) + 'px');
                            //                      $('#SpanFullDetail').html(msg.d);
                            //                      $('#SpanFullDetail').show();
                            //alert('success'+valReturnFromCodeBehide);
                        }
                    },
                    error: function myfunction(request, status) {
                        //alert('shin' + request.statusText + status);    
                    }
                });
         }, function () {
             $("#SpanFullDetail").mouseleave(function () {
                 $('#SpanFullDetail').stop(true, true).fadeOut('slow');
             });

             $('#SpanFullDetail').stop(true, true).fadeOut('slow');
         });


    });

        function TriggerTRClick(e) {
            if (!($(e.target).is('span'))) {
                var obj = $(e.target).parent();
                var quizid = $(obj).attr('quizid');
                ShowReport(quizid);
            }            
        }

            function callTooltip(obj, e) {
                var tip = $(this).find('toolsTip');
                var locateX = e.pageX + 20;
                var locateY = e.pageY + 20;
                locateX += 10;
                locateY -= 50;
                //           $(obj).css({ left: locateX, top: locateY }).delay(1000).fadeIn();
                $("#SpanFullDetail").mouseenter(function () {
                    $('#SpanFullDetail').stop(true, true).delay(800).fadeIn('slow');
                });
                $(obj).stop(true, true).delay(800).fadeIn('slow');
                //$(obj).css({ left: locateX, top: locateY }).delay(1000).show();
            }

            function toggleMenuTestset() {
                if ($(".TestsetDiv").hasClass('slide_True')) {
                    $(".TestsetDiv").removeClass('slide_True');
                    $(".TestsetDiv").slideToggle();

                } else {
                    $(".TestsetDiv").addClass('slide_True');
                    $(".TestsetDiv").slideToggle();
                }
                if ($.browser.msie) {
                    if ($.browser.version <= 7) {
                        $('.TestsetDiv').css('overflow', 'auto');
                    }
                }
            }

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

            function ShowReport(InputQuizId) {
                window.location = '<%=ResolveUrl("~")%>Activity/ActivityReport.aspx?QuizId=' + InputQuizId + '&ReportMenu=2&ShowBtnBack=True';
    }




    </script>


</head>
<body>
    <form id="form1" runat="server">
        <div id='main'>
            <div id="TestsetMainDiv" style='padding: 20px;background-color:whitesmoke;'>
                <div id="ManageTestset" class="TestsetMenu">
                    <h3>
                        <label id="lblManageTestset" class="old" runat="server" onclick="toggleMenuTestset();">
                            ดูผลการทำการบ้าน
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
                                            <th style="width: 20%;">สั่งให้
                                            </th>
                                            <th style="width: 20.5%;">สั่งเมื่อ
                                            </th>
                                            <th style="width: 20.5%;">กำหนดส่ง
                                            </th>
                                            <th style="width: 40%;">ชื่อการบ้าน
                                            </th>
                                        </tr>
                                    </thead>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr tag="<%# Container.DataItem("TestSet_Name")%>,<%# Container.DataItem("AssignTo")%>,<%# Container.DataItem("Start_Date")%>"
                                    quizid="<%# Container.DataItem("Quiz_Id")%>">
                                    <td style="background: #FFFFCC;" <%#GetClassForTdAssignTo(Eval("AssignTo")) %>>
                                        <%# CheckAssignToIsLong(Eval("AssignTo"), Eval("MA_Id").ToString())%>
                                    </td>
                                    <td style="background: #FFFFCC; text-align: center;">
                                        <%# Container.DataItem("Start_Date")%>
                                    </td>
                                    <td style="background: #FFFFCC;text-align: center;">
                                        <%# Container.DataItem("End_Date")%>
                                    </td>
                                    <td style="background: #FFFFCC; text-align: left;padding-left:10px;">
                                        <%# Container.DataItem("TestSet_Name")%>
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <AlternatingItemTemplate>
                                <tr tag="<%# Container.DataItem("TestSet_Name")%>,<%# Container.DataItem("AssignTo")%>,<%# Container.DataItem("Start_Date")%>"
                                    quizid="<%# Container.DataItem("Quiz_Id")%>">
                                    <td style="background: #FFFFFF;" <%#GetClassForTdAssignTo(Eval("AssignTo")) %>>
                                        <%# CheckAssignToIsLong(Eval("AssignTo"),Eval("MA_Id").ToString())%>
                                    </td>
                                    <td style="background: #FFFFFF; text-align: center;">
                                        <%# Container.DataItem("Start_Date")%>
                                    </td>
                                    <td style="background: #FFFFFF;text-align: center;">
                                        <%# Container.DataItem("End_Date")%>
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
        <span class="TopRight" id="SpanFullDetail"></span>
    </form>
</body>
</html>
