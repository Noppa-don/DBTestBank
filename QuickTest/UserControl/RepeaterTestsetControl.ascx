<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="RepeaterTestsetControl.ascx.vb"
    Inherits="QuickTest.RepeaterTestsetControl" %>

<style type="text/css">
    #TestsetMainDiv .TestsetMenu {
        margin: 5px;
        text-align: center;
        width: 80%;
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
        height: 300px;
    }

        #ManageTestset div table {
            width: 100%;
            border-spacing: 4;
            margin-top: 0px;
        }

    span.icon_clear {
        position: absolute;
        right: 10px;
        display: none;
        cursor: pointer;
        font: bold 1em;
        color: Red;
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
        top: 50%;
        margin-top: -13px;
        left: 5px;
        -ms-behavior: url('../css/backgroundsize.min.htc');
    }

    #txtSearchTestset, #txtSearchOtherTestset {
        width: 140px;
        padding-left: 30px;
        padding-right: 30px;
        height: 30px;
        font: 20px 'THSarabunNew';
    }

    a, tr, label, .TestsetMenu {
        cursor: pointer;
    }

    span.MsgNoTestset, span.hint {
        top: 20px;
        position: relative;
    }

        span.hint, span.hint > a {
            color: grey;
            font-size: 14px;
            font-weight: normal;
        }

            span.hint > a:hover {
                color: #09D4FF;
                cursor: pointer;
            }

    .cancleDelTestset {
        background-image: url("../images/smalliconMode/btnCancleDialog.png") !important;
        border: 0 !important;
        background-color: initial !important;
        height: 44px;
    }

    .confirmDelTestset {
        background-image: url("../images/smalliconMode/btnDeleteDialog.png") !important;
        border: 0 !important;
        background-color: initial !important;
        height: 44px;
    }
</style>
<link href="../css/jquery-ui-1.8.18.custom.css" rel="stylesheet" type="text/css" />
<link href="../css/jquery.fancybox.css" rel="stylesheet" type="text/css" />
<%--<link href="../css/prettyPhoto.css" rel="stylesheet" type="text/css" />--%>
<%--<script src="../js/jquery-1.7.1.js" type="text/javascript"></script>--%>
<script src="../js/GFB.js" type="text/javascript"></script>
<script src="../js/jquery-ui-1.8.18.js" type="text/javascript"></script>
<script src="../js/jquery.ui.touch-punch.min.js" type="text/javascript"></script>
<script src="../js/jquery.placeholder.js"></script>
<%--<script src="../js/jquery.prettyPhoto.js" type="text/javascript"></script>--%>
<script src="../js/jquery.fancybox.js" type="text/javascript"></script>
<style type="text/css">
    #dialogCopyTestset table tr td {
        background-color: white;
        text-align: center;
        padding: 0;
    }

    label.viewResult {
        background: url(../images/ResultPracticeTab.png) center left no-repeat;
        background-size: contain;
        padding-left: 70px !important;
    }

    label.viewHomeworkResult {
        background: url(../images/homework/ResultHomeWorkTab.png) center left no-repeat;
        background-size: contain;
        padding-left: 70px !important;
    }

    label.studentHomeworkTab {
        background: url(../images/homework/StudentHomeWorkTab.png) center left no-repeat;
        background-size: contain;
        padding-left: 70px !important;
    }

    label.homeworkManage {
        background: url(../images/homework/DocumentBag.png) center left no-repeat;
        background-size: contain;
        padding-left: 70px !important;
    }
</style>
<script type="text/javascript">
    var ua = navigator.userAgent.toLowerCase();
    var isAndroid = ua.indexOf("android") > -1;

    $(function () {

        //Bar1
        $('.Bar1').each(function () {
            new FastButton(this, TriggerClickBar1);
        });

        //Bar2
        $('.Bar2').each(function (e) {
            new FastButton(this, TriggerClickBar2);
        });

        //Bar3
        $('.Bar3').each(function () {
            new FastButton(this, TriggerClickBar3);
        });

        //Bar4
        $('.Bar4').each(function () {
            new FastButton(this, TriggerClickBar4);
        });

        //Bar5
        $('.Bar5').each(function () {
            new FastButton(this, TriggerClickBar5);
        });

        //ดักถ้าเข้าจาก Tablet ของครู
        if (isAndroid) {
            $('table tr td').css('font-size', '30px');
        }

        if ($.browser.msie && $.browser.version <= 9.0) {
            $('#txtSearchTestset').placeholder();
        }


        toggleMenuTestset();
        toggleMenuOtherTestset();

        $('.icon_clear').click(function () {
            $(this).delay(300).fadeTo(300, 0).prev().val('');//'#txtSearchTestset'
            //console.log($(this).prev().is($('#txtSearchTestset')));
            var table = ($(this).prev().is($('#txtSearchTestset'))) ? $('.TestsetDiv').children('.bordered') : $('.OtherTestsetDiv').children('.bordered');
            ShowAllMatchSearch(table);
        });

        setImgMenuViewResult();
    });

    function setImgMenuViewResult() {
        var dashboardMode = '<%=_DashboardMode%>';
        var className = (dashboardMode == '<%=BusinessTablet360.EnumDashBoardType.Homework %>') ? 'viewHomeworkResult' : 'viewResult';
        $('#ctl00_MainContent_MyCtlTestset_lblViewResult').addClass(className);
        var classNameManageMenu = (dashboardMode == '<%=BusinessTablet360.EnumDashBoardType.Homework %>') ? 'homeworkManage' : 'old';
        $('#ctl00_MainContent_MyCtlTestset_lblManageTestset').addClass(classNameManageMenu);
    }

    function ClearNotMatchSearch(txtSearch, table) {
        //var table = $('.bordered');
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
                ClearNotMatchSearch(t.toLowerCase(), $('.TestsetDiv').children('.bordered'));
                prevValue = t;
            }
            if (prevValue == '') {
                $('#txtSearchTestset').next('.icon_clear').delay(300).fadeTo(300, 0);
            }
            else {
                $('#txtSearchTestset').next('.icon_clear').stop().fadeTo(300, 1);
            }
        }, 500);
    }

    function SearchOtherTestset() {
        if (!($('.OtherTestsetDiv').hasClass('slide_True'))) {
            $(".OtherTestsetDiv").slideToggle();
            $(".OtherTestsetDiv").addClass('slide_True');
        }
        var prevValue = '';
        if (delayID != null) {
            clearInterval(delayID);
        }
        delayID = setInterval(function () {
            var t = $('#txtSearchOtherTestset').val();
            if (prevValue != t) {
                ClearNotMatchSearch(t.toLowerCase(), $('.OtherTestsetDiv').children('.bordered'));
                prevValue = t;
            }
            if (prevValue == '') {
                $('#txtSearchOtherTestset').next('.icon_clear').delay(300).fadeTo(300, 0);
            }
            else {
                $('#txtSearchOtherTestset').next('.icon_clear').stop().fadeTo(300, 1);
            }
        }, 500);
    }

    function ClearTimeInterval() {
        clearInterval(delayID);
    }
    function ShowAllMatchSearch(table) {
        //var table = $('.bordered');
        table.children('tbody').children('tr').each(function () {
            var IsShow = $(this).css('display');
            if (IsShow == 'none') {
                $(this).show();
            }
        });
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

    function toggleMenuOtherTestset(e) {
        if ($(".OtherTestsetDiv").hasClass('slide_True')) {
            $(".OtherTestsetDiv").removeClass('slide_True');
            $(".OtherTestsetDiv").slideToggle();

        } else {
            $(".OtherTestsetDiv").addClass('slide_True');
            $(".OtherTestsetDiv").slideToggle();
        }
    }

    // function กดจาก testset ที่จัดไว้แล้ว
    var DashboardMode = '<%= _DashboardMode %>';
    //QUIZ = 1,Homework = 2,Practice = 3,PrintTestset = 4,SetUp = 5
    function EditTestset(TestSetID, e) {
        if (navigator.userAgent.indexOf('Firefox') > -1) {
            var a = '';
        } else {
            var a = e.srcElement.getAttribute('rel');
        }
        if (a != 'PrettyTestset') {
            if (!($(e.target).is('a')) && !($(e.target).is('.RubberHide'))) {
                if (!isAndroid) {
                    FadePageTransitionOut();
                }
                if (DashboardMode == 1) {
                    window.location = '<%=ResolveUrl("~")%>activity/settingactivity.aspx?TestsetID=' + TestSetID;
                }
                else if (DashboardMode == 2) {
                    window.location = '<%=ResolveUrl("~")%>Module/HomeworkAssignPage.aspx?TestsetID=' + TestSetID + '&IsNew=True&PageName=Homework/DashboardHomeworkPage.aspx';
                }
                else if (DashboardMode == 3) {
                    //window.location = '<%=ResolveUrl("~")%>activity/activitypage.aspx?TestsetID=' + TestSetID + '&DashBoardMode=6';
                    window.location = '<%=ResolveUrl("~")%>activity/activitypage.aspx?TestsetID=' + TestSetID;
                }
                else if (DashboardMode == 4) {
                    window.location = '<%=ResolveUrl("~")%>testset/genpdf.aspx?TestsetID=' + TestSetID + '&FromDashboard=True';
                }
                else if (DashboardMode == 5) {
                    window.location = '<%=ResolveUrl("~")%>testset/step2.aspx?editid=' + TestSetID;
                }
}
}
    }

    function ClickEditTestset(TestSetID) {
        window.location = '<%=ResolveUrl("~")%>testset/step2.aspx?editid=' + TestSetID;
    }

        function ClickPrintTestset(TestSetID) {
                      window.location = '<%=ResolveUrl("~")%>testset/genpdf.aspx?TestsetID=' + TestSetID + '&FromDashboard=True';
        }

// function กดปุ่ม จัดชุดใหม่
function NewTestset() {
    if (DashboardMode == 5) {
        window.location = '<%=ResolveUrl("~")%>TestSet/Step2.aspx';
    }
    else {
        window.location = '<%=ResolveUrl("~")%>PracticeMode_Pad/ChooseClass.aspx?DashboardMode=' + DashboardMode;
    }
}
// function กด ดูผลการ ควิซ การบ้าน ฝึกใน
function ViewReport() {
    OpenLightBoxReport(DashboardMode);
}

function OpenLightBoxReport(inputMode) {
    var urlPage;
    if (inputMode == 1) {
        urlPage = '<%=ResolveUrl("~")%>Quiz/RepeaterReportQuiz.aspx';
    }
    else if (inputMode == 2) {
        urlPage = '<%=ResolveUrl("~")%>Homework/RepeaterReportHomework.aspx';
    }
    else if (inputMode == 3) {
        urlPage = '<%=ResolveUrl("~")%>Practice/RepeaterReportPractice.aspx';
    }
    if (isAndroid) {
        $.fancybox({
            'autoScale': true,
            'transitionIn': 'none',
            'transitionOut': 'none',
            'href': urlPage,
            'type': 'iframe',
            'width': 750,
            'minHeight': 450
        });
    }
    else {
        $.fancybox({
            'autoScale': true,
            'transitionIn': 'none',
            'transitionOut': 'none',
            'href': urlPage,
            'type': 'iframe',
            'width': 900,
            'minHeight': 600
        });
    }

}

// function ลบ ไปอัพเดท isactive = 0
function UpdateTestSetId(TestsetID, s) {
    $(s).closest("tr").remove();
    $.ajax({
        type: "POST",
        url: "<%=ResolveUrl("~")%>WebServices/TestsetService.asmx/UpdateTestSetIdCodeBehind",
        data: "{ TestsetId : '" + TestsetID + "'}",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (data.d != 0) {//session(UserId) หายจะ return 0 - Sefety 
                $('#dialogConfirm').children('p').html("บันทึกแล้วค่ะ");
                $('#dialogConfirm').dialog({
                    buttons: {
                        'ตกลง': function () {
                            $(this).dialog('close');
                            ChengeColorTr($('.TestsetDiv').children('.bordered'));
                        }
                    },
                    draggable: false,
                    resizable: false,
                    modal: true
                }).dialog('open').dialog('option', 'title', 'ลบชุดเรียบร้อยแล้วค่ะ');
            }
        },
        error: function myfunction() {
            //alert("เกิดข้อผิดพลาด!");
        }
    });
}
// check ก่อนว่า มีใครทำอะไรเกี่ยวกับ testset อยู่หรือเปล่า
function CheckTestsetUseNow(TestsetID, s) {
    $.ajax({
        type: "POST",
        url: "<%=ResolveUrl("~")%>WebServices/TestsetService.asmx/CheckTestsetUseNow",
        data: "{ TestsetId : '" + TestsetID + "'}",
        contentType: "application/json; charset=utf-8",
        success: function (msg) {
            if (msg.d != 0) {//session(UserId) หายจะ return 0 - Sefety 
                //$('#dialogConfirm').children('p').html(msg.d);
                $('#dialogConfirm').dialog({
                    buttons: [{
                        text: "",//"ไม่ลบ"
                        "class": "cancleDelTestset",
                        click: function () {
                            $(this).dialog('close');
                        }
                    },
                        {
                            text: "",//"ลบ"
                            "class": "confirmDelTestset",
                            click: function () {
                                UpdateTestSetId(TestsetID, s);
                            }
                        }],
                    draggable: false,
                    resizable: false,
                    modal: true
                }).dialog('open').dialog('option', 'title', msg.d);
            }
        },
        error: function myfunction() {
        }
    });
}
function ChengeColorTr(table) {
    var i = 1;
    $(table).find('tr').each(function () {
        if (i > 1) {
            var td = $(this).find("td");
            if (i % 2 == 0) {
                $(td).css('background', '#FFFFCC');
            } else {
                $(td).css('background', '#FFFFFF');
            }
        }
        i = i + 1;
    });
}

// กดเปิด LightBox หน้า StudentListPage เมื่อกดจาก ชื่อ TestsetName
function ShowTestSetDetailPage(TestsetID, isTestset) {
    var param = isTestset ? 'TestsetID=' + TestsetID : 'qsetid=' + TestsetID;
    $.fancybox({
        'autoScale': true,
        'transitionIn': 'none',
        'transitionOut': 'none',
        'href': '<%=ResolveUrl("~")%>TestSet/ShowTestSetDetailPage.aspx?' + param,
        'type': 'iframe',
        'width': 864,
        'minHeight': 540
    });
}
 

function GoHomeworkNowAndHistoryPage() {
    window.location = '<%=ResolveUrl("~")%>Student/HomewokNowAndHistoryPage.aspx?IsCurrent=True'
}


function TriggerClickBar1(e) {
    NewTestset();
}

function TriggerClickBar2(e) {
    if (e.target.id != "txtSearchTestset" && e.target.id != 'spnClear') {
        toggleMenuTestset();
    }
}


function TriggerClickBar3(e) {
    GoHomeworkNowAndHistoryPage();
}

function TriggerClickBar4() {
    ViewReport();
}


function TriggerClickBar5(e) {
    if (e.target.id != "txtSearchOtherTestset" && e.target.id != 'spnOtherClear') {
        toggleMenuOtherTestset(e);
    }
}


</script>
<div id="TestsetMainDiv">
    <span id="spanTest"></span>
    <div id="NewTestset" class="TestsetMenu Bar1" <%--onclick="NewTestset();"--%>>
        <h3>
            <label id="lblNewTestset" class="new" runat="server">
            </label>
        </h3>
    </div>
    <div id="ManageTestset" class="TestsetMenu" <%--onclick="toggleMenuTestset();"--%>>
        <h3 class="Bar2">
            <label id="lblManageTestset" runat="server">
            </label>
            <%If IsEnabledSearch = True Then%>
            <span style="position: relative;"><span class="imgFind"></span>
                <input type="text" id="txtSearchTestset" placeholder="ค้นหา" onfocus="SearchTestset();"
                    onblur="ClearTimeInterval();" />
                <span class="icon_clear" id="spnClear">X</span> </span>
            <%End If%>
        </h3>
        <div id="repeaterTestsetDiv" class="TestsetDiv slide_True" runat="server">
            <asp:Repeater ID="Listing" runat="server">
                <HeaderTemplate>
                    <table class="bordered">
                        <thead>
                            <tr>
                                <th style="width: 50%;">ชื่อ
                                </th>
                                <th style="width: 20%;">วิชา
                                </th>
                                <th style="width: 30%;">วันที่จัดทำ
                                </th>
                            </tr>
                        </thead>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr tag="<%# Container.DataItem("TestSet_Name")%>,<%# Container.DataItem("SubjectName")%>">
                        <td style="background: #FFFFCC;">
                            <%--<a class="aTestset" rel="PrettyTestset" href="<%=ResolveUrl("~")%>testset/ShowTestsetDetailPage.aspx?TestsetID=<%# Container.DataItem("TestSet_Id")%>?iframe=true&width=850&height=600">--%>
                            <a class="aTestset" rel="PrettyTestset" onclick="ShowTestSetDetailPage('<%# Container.DataItem("TestSet_Id")%>',true);">
                                <%# Container.DataItem("TestSet_Name")%></a>
                            <% If _DashboardMode = 5 Then%>
                            <img id='imdDeleteTestSet' class='RubberHide' alt="" src="../Images/Delete-icon.png"
                                onclick='CheckTestsetUseNow("<%# Container.DataItem("TestSet_Id")%>",this)' style='float: right; cursor: pointer' />

                             <img id='imgEditTestSet' rel="mmm" class='RubberHide' alt="" src="../Images/freehand.png"
                                onclick='ClickEditTestset("<%# Container.DataItem("TestSet_Id")%>")' style='float: right; cursor: pointer;padding-right: 15px;' />
                            <%End If%>
                            <% If _DashboardMode = 4 Then%>
                            <img id='imgPrintTestSet' class='RubberHide' alt="" src="../Images/Printer.png"
                                 onclick='ClickPrintTestset("<%# Container.DataItem("TestSet_Id")%>")' style='float: right; cursor: pointer;padding-right: 25px;' />
                            <%End If%>
                        </td>
                        <td style="background: #FFFFCC;">
                            <%# Container.DataItem("SubjectName")%>
                        </td>
                        <td style="background: #FFFFCC;">
                            <%# Container.DataItem("TimeAgo")%>
                        </td>
                    </tr>
                </ItemTemplate>
                <AlternatingItemTemplate>
                    <tr tag="<%# Container.DataItem("TestSet_Name")%>,<%# Container.DataItem("SubjectName")%>">
                        <td style="background: #FFFFFF;">
                            <%--<a class="aTestset" rel="PrettyTestset" href="<%=ResolveUrl("~")%>testset/ShowTestsetDetailPage.aspx?TestsetID=<%# Container.DataItem("TestSet_Id")%>?iframe=true&width=850&height=600">--%>
                            <a class="aTestset" rel="PrettyTestset" onclick="ShowTestSetDetailPage('<%# Container.DataItem("TestSet_Id")%>',true);">
                                <%# Container.DataItem("TestSet_Name")%></a>
                            <% If _DashboardMode = 5 Then%>
                            <img id='imdDeleteTestSet' class='RubberHide' alt="" src="../Images/Delete-icon.png"
                                onclick='CheckTestsetUseNow("<%# Container.DataItem("TestSet_Id")%>",this)' style='float: right; cursor: pointer' />

                            <img id='imgEditTestSet' class='RubberHide' alt="" src="../Images/freehand.png"
                                onclick='ClickEditTestset("<%# Container.DataItem("TestSet_Id")%>")' style='float: right; cursor: pointer;padding-right: 15px;' />
                            <%End If%>

                            <% If _DashboardMode = 4 Then%>
                            <img id='imgPrintTestSet' class='RubberHide' alt="" src="../Images/Printer.png"
                                 onclick='ClickPrintTestset("<%# Container.DataItem("TestSet_Id")%>")' style='float: right; cursor: pointer;padding-right: 25px;' />
                            <%End If%>

                        </td>
                        <td style="background: #FFFFFF;">
                            <%# Container.DataItem("SubjectName")%>
                        </td>
                        <td style="background: #FFFFFF;">
                            <%# Container.DataItem("TimeAgo")%>
                        </td>
                    </tr>
                </AlternatingItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </asp:Repeater>
        </div>
    </div>
    <div id="HW_Student" class="TestsetMenu Bar3" runat="server">
        <h3>
            <label id="lblHW_Student" class="studentHomeworkTab" runat="server">
            </label>
        </h3>
    </div>
    <div id="ViewResult" class="TestsetMenu Bar4" runat="server">
        <h3>
            <label id="lblViewResult" runat="server">
            </label>
        </h3>
    </div>
    <%If _DashboardMode = BusinessTablet360.EnumDashBoardType.SetUp And BusinessTablet360.ClsKNSession.IsAddQuestionBySchool Then %>
    <div id="CopyTestset" class="TestsetMenu">
        <h3 class="Bar5">
            <label id="Label1" class="old" runat="server">
                ดูชุดข้อสอบของคนอื่น
            </label>
            <span style="position: relative;"><span class="imgFind"></span>
                <input type="text" id="txtSearchOtherTestset" placeholder="ค้นหา" onfocus="SearchOtherTestset();"
                    onblur="ClearTimeInterval();" />
                <span class="icon_clear" id="spnOtherClear">X</span> </span>

        </h3>
        <div id="Div1" class="OtherTestsetDiv slide_True" runat="server" style="height: 300px; overflow-y: auto;">
            <asp:Repeater ID="RepeaterOtherUserTestset" runat="server">
                <HeaderTemplate>
                    <table class="bordered">
                        <thead>
                            <tr>
                                <th>ชื่อ
                                </th>
                                <th style="width: 35px;">คัดลอก
                                </th>
                                <th style="width: 80px">วิชา
                                </th>
                                <th style="width: 130px;">โดย
                                </th>
                                <th style="width: 130px;">วันที่จัด
                                </th>
                            </tr>
                        </thead>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr tag="<%# Container.DataItem("QSet_Name")%>,<%# Container.DataItem("SubjectName")%>">
                        <td style="background: #FFFFCC;">
                            <a class="aTestset" rel="PrettyTestset" onclick="ShowTestSetDetailPage('<%# Container.DataItem("QSet_Id")%>',false);">
                                <%# Container.DataItem("QSet_Name")%></a>
                        </td>
                        <td style="background: #FFFFCC;">
                            <img src="../Images/download.png" width="32" onclick="CopyTestset('<%# Container.DataItem("QSet_Id")%>');" />
                        </td>
                        <td style="background: #FFFFCC;">
                            <%# Container.DataItem("SubjectName")%>
                        </td>
                        <td style="background: #FFFFCC;"><%# Container.DataItem("Teacher_FirstName")%>
                        </td>
                        <td style="background: #FFFFCC;">
                            <%# Container.DataItem("TimeAgo")%>
                        </td>
                    </tr>
                </ItemTemplate>
                <AlternatingItemTemplate>
                    <tr tag="<%# Container.DataItem("QSet_Name")%>,<%# Container.DataItem("SubjectName")%>">
                        <td style="background: #FFFFFF;">
                            <a class="aTestset" rel="PrettyTestset" onclick="ShowTestSetDetailPage('<%# Container.DataItem("QSet_Id")%>',false);">
                                <%# Container.DataItem("QSet_Name")%></a>
                        </td>
                        <td style="background: #FFFFFF;">
                            <img src="../Images/download.png" width="32" onclick="CopyTestset('<%# Container.DataItem("QSet_Id")%>');" />
                        </td>
                        <td style="background: #FFFFFF;">
                            <%# Container.DataItem("SubjectName")%>
                        </td>
                        <td style="background: #FFFFFF;"><%# Container.DataItem("Teacher_FirstName")%>
                        </td>
                        <td style="background: #FFFFFF;">
                            <%# Container.DataItem("TimeAgo")%>
                        </td>
                    </tr>
                </AlternatingItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </asp:Repeater>
        </div>
    </div>
    <%End If %>
</div>
<div id="dialogConfirm" title="">
</div>
<div id="dialogCopyTestset" style="display: none; width: 600px!important;">
    <div style="width: 478px; background-color: #FEFEFE; border-radius: 5px; padding: 10px; margin: auto; margin-top: 5px; border: 1px solid silver; font-size: 18px;">
        <table style="margin: auto;">
            <tr>
                <td colspan="2" style="text-align: center; font-size: 120%; font-weight: bold;">ใช้เป็น 
                </td>
            </tr>
            <tr>
                <td style="width: 50%; border-right: 1px solid silver; border-bottom: 1px solid silver;">
                    <asp:CheckBox ID="ChkIsQuiz" runat="server" ClientIDMode="Static" Text="ควิซ" />
                    <label for="ChkIsQuiz"></label>
                    <img src="../Images/SmallIconMode/IconQuiz.png" style="width: 60px; height: 60px; margin-left: 9px;" alt="" />
                </td>
                <td style="border-bottom: 1px solid silver;">
                    <asp:CheckBox ID="ChkIsHomework" ClientIDMode="Static" runat="server" Text="การบ้าน" />
                    <label id="lblForChkIsHomework" for="ChkIsHomework">
                    </label>
                    <img src="../Images/SmallIconMode/IconHomework.png" style="width: 60px; height: 60px;" alt="" />
                </td>
            </tr>
            <tr>
                <td style="width: 50%; border-right: 1px solid silver;">
                    <asp:CheckBox ID="ChkIsPractice" ClientIDMode="Static" runat="server" Text="ฝึกฝน" />
                    <label id="lblForChkIsPractice" for="ChkIsPractice">
                    </label>
                    <img src="../Images/SmallIconMode/IconPractice.png" style="width: 60px; height: 60px; margin-left: -4px;" alt="" />
                </td>
                <td>
                    <asp:CheckBox ID="ChkIsPrintTestset" ClientIDMode="Static" runat="server" Text="ใบงาน" />
                    <label id="lblForChkIsPrintTestset" for="ChkIsPrintTestset">
                    </label>
                    <img src="../Images/SmallIconMode/IconPrint.png" style="width: 60px; height: 60px; margin-left: 12px;" alt="" />
                </td>
            </tr>
            <tr>
                <td colspan="2" style="padding-top: 15px;"><span>ตั้งชื่อว่า : </span>
                    <input type="text" id="txtTestsetName" style="width: 340px; padding: 1px; font: 100% 'THSarabunNew'; border: 1px solid #C6E7F0; background: #EFF8FB; color: #47433F; border-radius: 5px;" /></td>
            </tr>
        </table>
    </div>
</div>
<script type="text/javascript">
    function CopyTestset(qsetId) {
        console.log(qsetId);
        $('#ChkIsQuiz').attr('checked', false);
        $('#ChkIsHomework').attr('checked', false);
        $('#ChkIsPractice').attr('checked', false);
        $('#ChkIsPrintTestset').attr('checked', false);
        $('#txtTestsetName').val('');
        callDialog(qsetId);
    }

    function callDialog(testsetId) {
        //$('#dialogCopyTestset').children('p').html(msg.d);
        $('#dialogCopyTestset').dialog({
            buttons: {
                'ยกเลิก': function () {
                    $(this).dialog('close');
                },
                'บันทึก': function () {
                    var newTestsetName = $('#txtTestsetName').val();
                    if (newTestsetName == "") { callAlertDialog("ตั้งชื่อชุดก่อนค่ะ"); return 0; }// ไม่ได้ตั้งชื่อชุด
                    var isQuiz = $('#ChkIsQuiz').attr('checked') == undefined ? false : true;
                    var isHomework = $('#ChkIsHomework').attr('checked') == undefined ? false : true;
                    var isPractice = $('#ChkIsPractice').attr('checked') == undefined ? false : true;
                    var isReport = $('#ChkIsPrintTestset').attr('checked') == undefined ? false : true;
                    if (!isQuiz) { if (!isHomework) { if (!isPractice) { if (!isReport) { callAlertDialog("เลือกประเภทของชุดที่จะนำไปใช้ด้วยค่ะ"); return 0; } } } } // check ว่าได้เลือกอะไรมั้ย
                    saveCopyTestset(testsetId, newTestsetName, isQuiz, isHomework, isPractice, isReport);
                    $(this).dialog('close');
                }
            },
            draggable: false,
            resizable: false,
            modal: true
        }).dialog('open').dialog('option', 'title', 'ต้องการคัดลอกชุด ');
    }

    function saveCopyTestset(otherQsetId, newTestsetName, isQuiz, isHomework, isPractice, isReport) {
        $.ajax({
            type: "POST",
            url: "<%=ResolveUrl("~")%>WebServices/TestsetService.asmx/CopyOtherTestset",
            data: "{ qsetId : '" + otherQsetId + "', newTestsetName : '" + newTestsetName + "', isQuiz : " + isQuiz + ", isHomework : " + isHomework + ", isPractice : " + isPractice + ", isReport : " + isReport + "}",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data.d == true) {
                    $('#dialogConfirm').dialog({
                        buttons: {
                            'ตกลง': function () {
                                $(this).dialog('close');
                                location.reload();
                            }
                        },
                        draggable: false,
                        resizable: false,
                        modal: true
                    }).dialog('open').dialog('option', 'title', 'คัดลอกข้อสอบเรียบร้อยแล้วค่ะ');
                } else {
                    callAlertDialog("ไม่สามารถบันทึกชุดได้ค่ะ");
                }
            },
            error: function myfunction() {
                callAlertDialog("ไม่สามารถบันทึกชุดได้ค่ะ");
            }
        });
    }

    function callAlertDialog(txtAlert) {
        $('#dialogConfirm').dialog({
            buttons: {
                'ตกลง': function () {
                    $(this).dialog('close');                    
                }
            },
            draggable: false,
            resizable: false,
            modal: true
        }).dialog('open').dialog('option', 'title', txtAlert);
    }
</script>
