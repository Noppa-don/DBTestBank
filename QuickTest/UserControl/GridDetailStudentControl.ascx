<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="GridDetailStudentControl.ascx.vb"
    Inherits="QuickTest.GridDetailStudentControl" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<style type="text/css">
    .ForMainDiv {
        width: 750px;
        height: 300px;
        margin-left: auto;
        margin-right: auto;
    }

    .ForMainDivNoData {
        width: auto;
        height: 350px;
        overflow-y: auto;
        border: 3px dashed #FFA032;
        border-radius: 5px;
        color: #444;
        margin: 10px; 
    }

    .ForSpanNoData {
        font-size: 40px;
        position: relative;
        top: 135px;
    }

    .ForDivShowInFo {
        text-align: center;
    }

    table {
        margin: 0;
        font: normal 0.95em 'THSarabunNew' !important;
        color: #444;
    }
    /*.rgRow td:first-child,.rgAltRow td:first-child, th.rgHeader:first-child, .rgFilterRow td:first-child
    {
        border-right: 1px solid #ccc !important;width:65%;border-radius:5px 0 0 0;      
    }
    .rgRow td:last-child,.rgAltRow td:last-child,th.rgHeader:last-child, .rgFilterRow td:last-child
    {
        border-left: 1px solid #ccc !important;width:20%; text-align:center;border-radius: 0 5px 0 0;  
    }*/

    .RadGrid_Default .rgRow td, .RadGrid_Default .rgAltRow td, .RadGrid_Default .rgEditRow td, .RadGrid_Default .rgFooter td {
        border-right: 1px solid #ccc !important;
    }

    .RadGrid_Default .rgHeader, .RadGrid_Default .rgHeader a {
        border-right: 1px solid #ccc !important;
        /*width:150px;*/
    }

    tr.rgRow {
        background-color: #FFFFCC !important;
        height: 45px;
        font-size: 18px;
    }

    tr.rgAltRow {
        background-color: #FFFFFF !important;
        height: 45px;
        font-size: 18px;
    }

        tr.rgRow td, tr.rgAltRow td {
            border-color: #ccc !important;
        }

            tr.rgRow td a, tr.rgAltRow td a {
                color: #09D4FF !important;
            }

    thead tr {
        height: 55px;
    }

        thead tr th {
            
            background: #839b59 !important;
            /*background: #dce9f9 !important;*/
            border-bottom: 1px solid #ccc !important;
            font-size: 20px !important;
        }

    .ForImg {
        float: right;
        margin-right: 20px;
        cursor: pointer;
        width: 30px;
        height: 30px;
    }

    .ForDivLogType {
        width: 50px;
        height: 30px;
        float: left;
        margin-left: 10px;
        margin-right: 10px;
        border-radius: 5px;
    }

    .ForMaindivLogType {
        line-height: 30px;
    }

    .DivForPractice {
        display: inline-block;
        width: 445px;
    }

    .DivForQuiz {
        display: inline-block;
        width: 385px;
    }

    .DivForHomework {
        display: inline-block;
        width: 270px;
    }

    .TestForIcon {
        display: inline-block;
        margin-left: 10px;
        vertical-align: top;
    }

    #RadGrid1 table tr td:first-child {
        text-align: left !important;
        padding-left: 10px;
    }
    .imgPreview{
        width:60px;
        cursor:pointer;
    }
</style>

<telerik:RadCodeBlock ID="RadcodeBlockStyle" runat="server">
    <%If IsAndroid = True Then%>
    <style type="text/css">
        table tr td {
            font-size: 28px !important;
        }

        #DivBtnChooseMode {
            font-size: 25px !important;
        }

        .ForImg {
            width: 50px !important;
            height: 50px !important;
        }
    </style>
    <%End If%>
</telerik:RadCodeBlock>

<telerik:RadCodeBlock ID='RadCodeBlockGrid' runat="server">

    <script type="text/javascript">
        var ua = navigator.userAgent.toLowerCase();
        var isAndroid = ua.indexOf("android") > -1;

        var deviceid = '<%=DeviceuniqueId%>';
        var token = '<%=Token%>';

        $(function () {
            
            //ดักถ้าเข้าจาก Tablet ครู
            //if (isAndroid) {
            //    $('table tr td').css('font-size', '28px');
            //    $('#DivBtnChooseMode').css('font-size', '25px');
            //    $('.ForImg').css({ 'width': '50px' ,'height':'50px'});
            //}

            $('.imgPreview').click(function () {
                var quizid = $(this).attr('id');
                window.location.href = '../Activity/ActivityPage_Pad.aspx?quizid=' + quizid + '&deviceuniqueid=' + deviceid + '&token=' + token + '&ispreview=true';
                //alert(id);
            });
        });


        // กดเปิด LightBox หน้า UsageScore เมื่อกดจากรูปที่ column TestSetname
        function ShowUsageScorePage(InputId, Mode) {
            var Path = '';
            if (Mode == 'Quiz') {
                //alert('เข้า Quiz');
                var splitstr = InputId.split('|');
                Path = '<%=ResolveUrl("~")%>Student/UsageScore.aspx?QuizId=' + splitstr[0] + '&StudentId=' + splitstr[1];
        }
        else if (Mode == 'Practice') {
            //alert('เข้า Practice');
            Path = '<%=ResolveUrl("~")%>Student/UsageScore.aspx?TestSetId=' + InputId;
        }
        else if (Mode == 'Homework') {
            //alert('เข้า Homework');
            var splitstr = InputId.split('|');
            Path = '<%=ResolveUrl("~")%>Student/UsageScore.aspx?MA_Id=' + splitstr[0] + '&StudentId=' + splitstr[1] + '&HwQuizId=' + splitstr[2];
        }
    if (isAndroid) {
        $.fancybox({
            'autoScale': true,
            'transitionIn': 'none',
            'transitionOut': 'none',
            'href': Path,
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
            'href': Path,
            'type': 'iframe',
            'width': 850,
            'minHeight': 500
        });
    }

}

// กดเปิด LightBox หน้า StudentListPage เมื่อกดจาก ชื่อ TestsetName
function ShowTestSetDetailPage(QuizId, StudentId) {
    if (isAndroid) {
        $.fancybox({
            'autoScale': true,
            'transitionIn': 'none',
            'transitionOut': 'none',
            'href': '<%=ResolveUrl("~")%>TestSet/ShowTestSetDetailPage.aspx?QuizID=' + QuizId + '&StudentID=' + StudentId,
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
                'href': '<%=ResolveUrl("~")%>TestSet/ShowTestSetDetailPage.aspx?QuizID=' + QuizId + '&StudentID=' + StudentId,
                'type': 'iframe',
                'width': 850,
                'minHeight': 500
            });
        }

    }

    //Function เปิดหน้า กราฟเมื่อกดจาก column คะแนน
    function ShowChartStudentInfo(Mode, StudentId, QuizId) {

        var Path = '';
        if (Mode == 'Quiz') {
            Path = '<%=ResolveUrl("~")%>Student/ChartStudentInfo.aspx?Mode=Quiz&QuizId=' + QuizId + '&StudentId=' + StudentId;
        }
        else if (Mode == 'Practice') {
            Path = '<%=ResolveUrl("~")%>Student/ChartStudentInfo.aspx?Mode=Practice&QuizId=' + QuizId + '&StudentId=' + StudentId;
        }
        else if (Mode == 'Homework') {
            Path = '<%=ResolveUrl("~")%>Student/ChartStudentInfo.aspx?Mode=Homework&QuizId=' + QuizId + '&StudentId=' + StudentId;
        }

    if (isAndroid) {
        $.fancybox({
            'autoScale': true,
            'transitionIn': 'none',
            'transitionOut': 'none',
            'href': Path,
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
            'href': Path,
            'type': 'iframe',
            'width': 850,
            'minHeight': 540
        });
    }

}

    </script>

</telerik:RadCodeBlock>


<div id='MainDiv' runat="server" style="height: 360px; overflow-y: auto;">
    <%--<telerik:RadAjaxPanel ID="RadAjaxPanel1" LoadingPanelID="RadAjaxLoadingPanel1" runat="server">--%>
    <telerik:RadGrid runat="server" ID='RadGrid1' HeaderStyle-HorizontalAlign="Center"
        HeaderStyle-Font-Size="Medium" AllowPaging="false" AutoGenerateColumns="false" Style="border-radius: 5px;" ClientIDMode="Static">
        <MasterTableView Font-Size="Medium">
        </MasterTableView>
    </telerik:RadGrid>
    <%--</telerik:RadAjaxPanel>--%>
</div>
<div id="ForDivNodata" runat="server" style="overflow-y: auto; display: none;">
</div>
