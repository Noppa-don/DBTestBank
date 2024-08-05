<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master"
    CodeBehind="StudentDetailPage.aspx.vb" Inherits="QuickTest.StudentDetailPage" %>

<%@ Register Src="../UserControl/GridDetailStudentControl.ascx" TagName="GridDetailStudentControl"
    TagPrefix="uc1" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="../js/jquery-1.7.1.js" type="text/javascript"></script>
    <script src="../js/GFB.js" type="text/javascript"></script>
    <script src="../js/jquery.qtip-1.0.0-rc3.js" type="text/javascript"></script>
    <script src="../js/jquery.blockUI.js" type="text/javascript"></script>

    <link rel="stylesheet" type="text/css" href="../shadowbox/shadowbox.css">
    <script type="text/javascript" src="../shadowbox/shadowbox.js"></script>

    <script type="text/javascript">


        var DeviceId = '<%=JSDeviceId %>';
        var ua = navigator.userAgent.toLowerCase();
        var isAndroid = ua.indexOf("android") > -1;
        var isMaxOnet = "<%= IsMaxOnet%>";
        //console.log(isMaxOnet);


        $(function () {

        Shadowbox.init({
            skipSetup: false
        });
            //ปุ่ม การบ้านทั้งหมด
            new FastButton(document.getElementById('btnHomework'), TriggerBtnClick);

            //ปุ่ม ประวัติควิซ
            if ($('#btnQuizHistory').length != 0) {
                new FastButton(document.getElementById('btnQuizHistory'), TriggerBtnClick);
            }

            //ปุ่ม ประวัติฝึกฝน
            new FastButton(document.getElementById('btnPracticeHistory'), TriggerBtnClick);

            //ปุ่ม บ้าน
            new FastButton(document.getElementById('ImgHome'), TriggerHomeClick);

            $('#Help').remove();

            //$('#ImgHome').click(function () {
            //    window.location = '<%=ResolveUrl("~")%>PracticeMode_Pad/ChooseTestset.aspx?DeviceUniqueID=' + DeviceId;
            //});

            $('.GotoContact').click(function () {
                GotoContact();
            });

        });
        var tokenId = '<%=TokenId%>';
        function TriggerHomeClick() {
            if (isMaxOnet === "True") {
                openBlockUI();
                backSound.play();
                refreshUrl = '<%=ResolveUrl("~")%>PracticeMode_Pad/ChooseTestsetMaxOnet.aspx?DeviceUniqueID=' + DeviceId + '&token=' + tokenId;
                if (responseTimeCI != 0) return 0;
                window.location = refreshUrl;// '<%=ResolveUrl("~")%>PracticeMode_Pad/ChooseTestsetMaxOnet.aspx?DeviceUniqueID=' + DeviceId + '&token=' + tokenId;
            } else {
                window.location = '<%=ResolveUrl("~")%>PracticeMode_Pad/ChooseTestset.aspx?DeviceUniqueID=' + DeviceId;
            }

        }

        function TriggerBtnClick(e) {
            if (isMaxOnet === "True") { openBlockUI(); chooseSound.play(); }
            var obj = e.target;
            $(obj).trigger('click');
        }

        function GotoContact() {
                    Shadowbox.open({
                        content: '<div style="overflow: hidden;white-space: nowrap; background-color:white;"><iframe scrolling="no" style="overflow: hidden;white-space: nowrap; " src="<%=ResolveUrl("~")%>Contact.aspx" frameborder="0" width="700" height="570"></iframe></div>',
                        player: "html",
                        title: "ติดต่อเรา",
                        height: 570,
                        width: 700
                    });
         };


    </script>

    <script type="text/javascript">



        var NeedShowTip = '<%=NeedShowTip%>';
        $(function () {
            if (NeedShowTip == 'True') {
                ShowTips();
            }
           
        });
        
        function ShowTips() {
            var elm = ['#DivBtnChooseMode', '#ImgHome'];
            var tipPosition = ['leftMiddle', 'topMiddle'];
            var tipTarget = ['rightMiddle', 'bottomMiddle'];
            var tipContent = ['เลือกดูประวัติ + คะแนนของตัวเอง', 'กลับหน้าแรก'];
            var tipAjust = [0, 0];
            var w = [300, 140];
            for (var i = 0; i < elm.length; i++) {
                $(elm[i]).qtip({
                    content: tipContent[i],
                    show: { ready: true },
                    style: {
                        width: w[i], padding: 5, background: '#F68500', color: 'white', textAlign: 'center', border: { width: 7, radius: 5, color: '#F68500' }, tip: tipPosition[i], name: 'dark', 'font-weight': 'bold', 'font-size': '18px'
                    }, hide: false,
                    position: { corner: { tooltip: tipPosition[i], target: tipTarget[i] }, adjust: { x: tipAjust[i] } },
                    fixed: false
                });
            }
            DestroyTips(elm);
        }
        function DestroyTips(elm) {
            setTimeout(function () {
                for (var i = 0; i < elm.length; i++) {
                    $(elm[i]).qtip('destroy');
                }
            }, 5000);
        }
    </script>
    <style type="text/css">
        #DivStudentInfo {
            width: 650px;
            margin-left: auto;
            margin-right: auto;
        }

        .ForDivBtn, .ForDivBtnBottom {
            width: 520px;
            margin: auto;
            border: solid 1px #DA7C0C;
            border-radius: .5em;
            text-align: center;
            display: table;
            margin-top: 20px;
            background: #F78D1D;
            background: -webkit-gradient(linear, left top, left bottom, from(#FAA51A), to(#F47A20));
            background: -moz-linear-gradient(top, #FAA51A, #F47A20);
        }

        .ForbtnMenu {
            font: 100% 'THSarabunNew';
            width: 160px;
            height: 45px;
            line-height: 45px;
            display: inline-block;
            text-align: center;
            cursor: pointer;
            color: #fff;
            border: 0;
            background: transparent;
            text-transform: uppercase;
            text-decoration: none; /*border-right: 1px solid;*/
            margin: auto;
        }

        .ForImgStudent {
            border: 1px solid #DA7C0C;
            padding: 3px;
            border-radius: 5px;
        }

        table tr td {
            text-align: center !important;
            font-size: 22px;
            font-weight: bold;
            background: #FFEDCB !important;
        }

        #ForUserControl {
            width: 900px;
            height: 360px;
            margin-left: auto;
            margin-right: auto;
            margin-top: 20px;
            margin-bottom: 10px;
        }

        .ForTdNoneBackground {
            background: none;
        }

        .ForHomeworkInfo {
            border: 1px solid #DA7C0C;
            border-radius: 5px;
            background-color: #FFEDCB;
            position: relative;
            top: -8px;
        }

        .RadGrid_Default .rgRow td, .RadGrid_Default .rgAltRow td, .RadGrid_Default .rgEditRow td, .RadGrid_Default .rgFooter td {
            font-weight: normal;
        }

        .RadGrid_Default .rgHeader, .RadGrid_Default .rgHeader a {
            font-size: 25px !important;
            font-weight: bold !important;
        }
    </style>
    <%If IsMaxOnet Then %>
    <style type="text/css">
        .ForbtnMenu {
            width: 260px;
            height: inherit;
            margin: auto;
        }

        #DivBtnChooseMode {
            display: flex !important;
            text-align: center;
        }

        .DivBtnChooseMode {
            height: 70px;
        }

        .ForDivBtn {
            margin-top: 0 !important;
            height: 70px;
            line-height: 70px;
            border: solid 1px #2193FC;
            background: -webkit-gradient(linear, left top, left bottom, from(#D3F2F7), to(#D3F2F7));
        }

        .ForbtnMenu {
            color: #222 !important;
        }
    </style>
    <%Else%>
    <style type="text/css">
        #site_content {
            height: 620px !important;
            border-radius: 10px;
        }
    </style>
    <%End If %>
    <style type="text/css">
        .flex {
            display: -webkit-box;
            display: -moz-box;
            display: -ms-flexbox;
            display: -webkit-flex;
            display: flex;
        }

            .flex.flex--reverse {
                -webkit-box-orient: horizontal;
                -moz-box-orient: horizontal;
                -webkit-box-direction: reverse;
                -moz-box-direction: reverse;
                -webkit-flex-direction: row-reverse;
                -ms-flex-direction: row-reverse;
                flex-direction: row-reverse;
            }

        .flex--row {
            -webkit-box-orient: vertical;
            -moz-box-orient: vertical;
            -webkit-box-direction: normal;
            -moz-box-direction: normal;
            -webkit-flex-direction: column;
            -ms-flex-direction: column;
            flex-direction: column;
        }

            .flex--row.flex--reverse {
                -webkit-box-orient: vertical;
                -moz-box-orient: vertical;
                -webkit-box-direction: reverse;
                -moz-box-direction: reverse;
                -webkit-flex-direction: column-reverse;
                -ms-flex-direction: column-reverse;
                flex-direction: column-reverse;
            }

        .flex--justify-content--space-between {
            -webkit-box-pack: justify;
            -moz-box-pack: justify;
            -ms-flex-pack: justify;
            -webkit-justify-content: space-between;
            justify-content: space-between;
        }

        .flex--justify-content--space-around {
            -webkit-box-pack: justify;
            -moz-box-pack: justify;
            -ms-flex-pack: distribute;
            -webkit-justify-content: space-around;
            justify-content: space-around;
        }

        .flex--justify-content--center {
            -webkit-box-pack: center;
            -moz-box-pack: center;
            -ms-flex-pack: center;
            -webkit-justify-content: center;
            justify-content: center;
        }

        .flex--justify-content--start {
            -webkit-box-pack: start;
            -moz-box-pack: start;
            -ms-flex-pack: start;
            -webkit-justify-content: flex-start;
            justify-content: flex-start;
        }

        .flex--justify-content--end {
            -webkit-box-pack: end;
            -moz-box-pack: end;
            -ms-flex-pack: end;
            -webkit-justify-content: flex-end;
            justify-content: flex-end;
        }

        .flex--align-items--start {
            -webkit-box-align: start;
            -moz-box-align: start;
            -ms-flex-align: start;
            -webkit-align-items: flex-start;
            align-items: flex-start;
        }

        .flex--align-items--end {
            -webkit-box-align: end;
            -moz-box-align: end;
            -ms-flex-align: end;
            -webkit-align-items: flex-end;
            align-items: flex-end;
        }

        .flex--align-items--center {
            -webkit-box-align: center;
            -moz-box-align: center;
            -ms-flex-align: center;
            -webkit-align-items: center;
            align-items: center;
        }

        .flex--align-items--baseline {
            -webkit-box-align: baseline;
            -moz-box-align: baseline;
            -ms-flex-align: baseline;
            -webkit-align-items: baseline;
            align-items: baseline;
        }

        .flex--align-items--stretch {
            -webkit-box-align: stretch;
            -moz-box-align: stretch;
            -ms-flex-align: stretch;
            -webkit-align-items: stretch;
            align-items: stretch;
        }

        .flex--flex-grow-all > * {
            -webkit-box-flex: 1;
            -moz-box-flex: 1;
            -webkit-flex-grow: 1;
            -ms-flex-grow: 1;
            flex-grow: 1;
        }

        .flex--flex-grow-last :last-child {
            -webkit-box-flex: 1;
            -moz-box-flex: 1;
            -webkit-flex-grow: 1;
            -ms-flex-grow: 1;
            flex-grow: 1;
        }

        .flex--flex-grow-middle :nth-child(2) {
            -webkit-box-flex: 1;
            -moz-box-flex: 1;
            -webkit-flex-grow: 1;
            -ms-flex-grow: 1;
            flex-grow: 1;
        }

        .flex-center-wrapper {
            display: -webkit-box;
            display: -moz-box;
            display: -ms-flexbox;
            display: -webkit-flex;
            display: flex;
            -webkit-box-pack: center;
            -moz-box-pack: center;
            -ms-flex-pack: center;
            -webkit-justify-content: center;
            justify-content: center;
            -webkit-box-align: center;
            -moz-box-align: center;
            -ms-flex-align: center;
            -webkit-align-items: center;
            align-items: center;
        }
        div.GotoContact{
                width: 70px;
                height: 70px;
                cursor: pointer;
                background-image: url(../images/maxonet/post.png);
                background-size: 70px;
                display: inline-block;
                float: right;
                margin-top: -70px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <form id='FormStudentDetailpage' runat="server">
        <telerik:RadScriptManager runat="server" ID='RadScriptManager1'>
        </telerik:RadScriptManager>
        <div id='main'>
            <div id='site_content' style="padding: 15px 12px 10px 12px; height: 460px;">
                <div class='content' style='width: 930px; padding: 0;'>
                    <img src="../Images/Arrow back.png" id='ImgHome' style='position: absolute; margin-left: 10px; cursor: pointer; width: 50px; height: 50px;margin-top: 8px;'
                        alt="home" />
                    <div id='DivStudentInfo' clientidmode="Static" runat="server">
                    </div>
                    <div id='DivBtnChooseMode' class='ForDivBtn flex'>
                        <asp:Button ID="btnPracticeHistory" ClientIDMode="Static" runat="server" Text="ทำเพื่อเข้าใจ" CssClass='ForbtnMenu' />
                        <%If BusinessTablet360.ClsKNSession.IsMaxONet Then %>
                        <div style="border: 1px solid; height: 45px; margin-top: 11px; width: 1px;"></div>
                        <%End If %>
                        <asp:Button ID="btnQuizHistory" ClientIDMode="Static" runat="server" Text="ประวัติควิซ"
                            CssClass='ForbtnMenu' Style='border-left: 1px solid; border-right: 1px solid;' />
                        <asp:Button ID="btnHomework" ClientIDMode="Static" runat="server" Text="การบ้านทั้งหมด" CssClass='ForbtnMenu' />

                    </div>
                     <div class="GotoContact"></div>
                    <div id='ForUserControl'>
                        <uc1:GridDetailStudentControl ID="GridDetailStudentControl1" runat="server" />
                    </div>
                </div>
            </div>
        </div>
    </form>
    <style type="text/css">
        thead tr th {
            background: #FF9F0F !important;
            color: white !important;
        }
    </style>
</asp:Content>
