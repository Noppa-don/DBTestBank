<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="TestsetSummaryPage.aspx.vb" Inherits="QuickTest.TestsetSummaryPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadStyleContent" runat="server">
    <style type="text/css">
        .divTestsetSummary h1.testsetSummary {
            font-size: 20px;
            font-weight: bold;
            margin: 0;
        }

        .divClassName, .divTestsetCategory {
            width: 790px;
            margin: auto;
            padding: 10px;
        }

        .divClassName {
            border: 1px dotted;
            margin-top: 10px;
            background-color:white;
        }

        .divTestsetCategory > h1 {
            font-weight: bold;
            text-decoration: underline;
            margin: 0;
        }

        .divClassName > span {
            font-weight: bold;
            font-size: 18px;
        }

        .divAllQset .divQset:not(:first-child) {
            margin-top: 5px;
        }

        .divQset {
            border: 1px solid #000;
            min-height: 60px;
            margin: auto;
            position: relative;
            border-radius: 5px;
            padding-left: 70px;
            padding-right: 70px;
            width: 610px;
            background-color: whitesmoke;
            font-size: 16px;
            cursor: pointer;            
        }

            .divQset img {
                height: 50px;
                position: absolute;
                top: 50%;
                margin-top: -25px;
                left: 0;
            }

            /*.divQset span.qsetName {
                position: absolute;
                top: 50%;
                margin-top: -18px;
                margin-left: 75px;
                font-size: 18px;
            }*/

            .divQset span.qsetQuestionAmount {
                position: absolute;
                right: 10px;
                font-weight: bold;
                top: 0;
            }

        .divTestsetCategory div input[type=radio]:not(:first-child) {
            margin-left: 30px;
        }

        .divTestsetCategory div label {
            margin-left: 5px;
        }

        .divTestsetCategory input[type=text] {
            padding: 1px;
            font: 100% 'THSarabunNew';
            border: 1px solid #C6E7F0;
            background: #EFF8FB;
            color: #47433F;
            border-radius: 7px;
        }

        #divShowQset ul {
            padding: 0;
            margin: 0!important;
        }

        #divShowQset li {
            list-style-type: none;
            line-height:initial;
        }
         .ui-state-highlight{ height: 1.5em; line-height: 50em;width:inherit;margin:auto;margin-top:10px;margin-bottom:10px; border:1px dotted;}
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <form id="toNextPage" runat="server">
        <div id="main" style="background-color: white; padding-bottom: 10px;">
            <header class="thinheader">
                <div id="logo" class="slogantext">
                    <div id="logo_text">
                        <h2>ครบถ้วน ถูกต้อง ฉับไว </h2>
                    </div>
                </div>
                <nav>
                    <div id="menu_container">
                        <ul class="sf-menu" id="nav" style="font-size: 20px; text-align: center;">
                            <li><a href="../Testset/DashboardSetupPage.aspx">
                                <img alt="" id="imgBack" src="../Images/Home.png" style="position: absolute; margin-left: 5px; margin-top: -8px; cursor: pointer;">
                            </a></li>
                            <li style="margin-left: 45px;"><a>สรุปข้อสอบที่ทำการเลือกมา</a></li>
                        </ul>
                    </div>
                </nav>
            </header>
            <div id="site_content" style="padding: 0; margin: auto;">
                <div class="divTestsetSummary">
                    <center><h1 class="testsetSummary"></h1></center>
                    <div runat="server" id="divShowQset" clientidmode="Static"></div>
                    <%-- <div class="divClassName">
                        <span>ภาษาไทย ป.1</span>
                        <div class="divAllQset">
                            <div class="divQset">
                                <img src="../Images/WaterMark/WatermarkWPP.jpg" alt="" />
                                <span class="qsetName">พัฒนาการอ่าน - เลือกคำตอบที่ถูกต้องที่สุด</span>
                                <span class="qsetQuestionAmount">10 ข้อ</span>
                            </div>
                            <div class="divQset">
                                <img src="../Images/WaterMark/WatermarkWPP.jpg" alt="" />
                                <span class="qsetName">พัฒนาการอ่าน - เลือกคำตอบที่ถูกต้องที่สุด</span>
                                <span class="qsetQuestionAmount">10 ข้อ</span>
                            </div>
                            <div class="divQset">
                                <img src="../Images/WaterMark/WatermarkWPP.jpg" alt="" />
                                <span class="qsetName">พัฒนาการอ่าน - เลือกคำตอบที่ถูกต้องที่สุด</span>
                                <span class="qsetQuestionAmount">10 ข้อ</span>
                            </div>
                        </div>
                    </div>--%>
                    <div class="divTestsetCategory" style="display:none;" >
                        <h1>เลือกข้อสอบตามความต้องการ</h1>
                        <div style="text-align: center;">
                            <asp:RadioButton ID="rdUseAll" runat="server" Text="ใช้ทั้งหมด" GroupName="TestsetCategory" Checked="true" />
                            <asp:RadioButton ID="rdManual" runat="server" Text="เลือกข้อ เข้า/ออก เอง" GroupName="TestsetCategory" />
                            <asp:RadioButton ID="rdRandom" runat="server" Text="สุ่มให้เหลือ" GroupName="TestsetCategory" />
                            <asp:TextBox ID="txtRandom" runat="server" Width="40px"></asp:TextBox><label>ข้อ</label>
                        </div>
                    </div>
                    <div id="divButton" style="position: relative; height: 40px; width: 850px; margin: 10px auto 0 auto">
                        <asp:Button Style="float: left; width: 150px; position: absolute; left: 0; margin: 0; font-size: 130%; height: 40px;"
                            ID="btnBack" ClientIDMode="Static" runat="server" Text="กลับ" class="submitChangeFontSize" />
                        <asp:Button Style="float: right; width: 200px; position: absolute; right: 0; margin: 0; font-size: 130%; height: 40px;"
                            ID="btnSave" ClientIDMode="Static" runat="server" Text="ไปหน้าบันทึกชุด" class="submitChangeFontSize" />
                    </div>
                </div>
            </div>
            <footer style="margin-top: 10px">สงวนลิขสิทธิ์ &copy; บริษัท สำนักพิมพ์วัฒนาพานิช จำกัด</footer>
        </div>
    </form>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ExtraScript" runat="server">
    <%-- <script type="text/javascript">
        onKeyDown = "limitText(this.form.limitedtextarea,this.form.countdown,2);"
        function limitText(limitField, limitCount, limitNum) {
            if (limitField.value.length > limitNum) {
                limitField.value = limitField.value.substring(0, limitNum);
            } else {
                limitCount.value = limitNum - limitField.value.length;
            }
        }
    </script>--%>

    <link href="../css/jquery.fancybox.css" rel="stylesheet" />
    <script src="../js/jquery.fancybox.js" type="text/javascript"></script>
    <script src="../js/jquery-ui-1.8.18.min.js"></script>
    <script type="text/javascript">
        $(function () {
            $('.divQset').click(function () {
                var qsetId = $(this).attr('id');
                OpenFancybox(qsetId);
            });

            $('#divShowQset').sortable();
            $("ul").sortable({ placeholder: "ui-state-highlight", axis: "y" });
            $("ul").disableSelection();
            
            var qsetAmount = $('.divQset').length;
            var questionAmount = 0;
            $('span.qsetQuestionAmount').each(function () {
                console.log($(this).text())
                var q = $(this).text().replace(" ข้อ", "");
                questionAmount = questionAmount + parseInt(q);
            });
            var txt = "เลือกข้อสอบมา " + questionAmount + " ข้อ (" + qsetAmount + " หน่วยการเรียนรู้)";
            $('h1.testsetSummary').text(txt);
        });

        function OpenFancybox(qsetId) {
            $.fancybox({
                'autoScale': true,
                'transitionIn': 'none',
                'transitionOut': 'none',
                'href': '<%=ResolveUrl("~")%>TestSet/QsetDetailPage.aspx?qsetId=' + qsetId,
                'type': 'iframe',
                'width': 864,
                'minHeight': 540
            });
        }

        function editQuestion(qsetId) {
            window.location = "../testset/addquestionpage.aspx?qsetId=" + qsetId;
        }
    </script>
</asp:Content>
