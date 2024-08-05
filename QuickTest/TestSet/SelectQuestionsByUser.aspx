<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="SelectQuestionsByUser.aspx.vb" Inherits="QuickTest.SelectQuestionsByUser" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .divSubjectClass {
            /*border: 1px dashed;*/
            padding: 10px;
            margin-top: 10px;
        }

            .divSubjectClass > span.spanSubjectClass {
                font-size: 25px;
                font-weight: bold;
                background-color: yellow;
                padding: 0 10px;
            }

        .divQset {
            border: 1px solid;
            padding: 10px;
            margin: 10px 0;
        }

        .divQuestion {
            padding: 10px;
            border-radius: 5px;
        }

        ul li {
            padding: 5px 0 5px 30px !important;
            position: relative;
        }

            ul li label {
                position: absolute;
                left: 0;
            }

        .selected {
            background: rgba(47, 204, 47, 0.43);
        }

        .notselect {
            background: initial;
            border: 1px dashed;
        }

        span.correct {
            background-color: greenyellow;
        }

        .submitChangeFontSize {
            width: 250px !important;
            position: relative !important;
            height: 40px !important;
            font-size: 130% !important;
            margin: 0 !important;
        }

        table tr td {
            background: initial !important;
            color: initial !important;
            padding: initial !important;
        }

        .txtRandom {
            padding: 1px;
            width: 60px;
            font: 80% 'THSarabunNew';
            border: 1px solid #C6E7F0;
            background: #EFF8FB;
            color: #47433F;          
            border-radius: 7px;         
            text-align:center;  
        }
        .divQuestionSelected{
            position:fixed;
            width:100px;
            height:100px;
            background-color:#faa51a;
            top:20px;
            margin-left:-120px;
            border-radius:5px;
            text-align: center;
    line-height: 30px;
        }
        .divQuestionSelected .lblQuestionAmount{
            font-size:30px;
            font-weight:bold;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadStyleContent" runat="server">
    <form id="toNextPage" runat="server">
        <div id="main" style="background-color: white; padding-bottom: 10px;">
            <header style="height: 120px;">
      <div id="logo" class="slogantext">
        <div id="logo_text">
          <h2>ครบถ้วน ถูกต้อง ฉับไว </h2>		 		
		         </div>
      </div>
      <nav>              
           <ul class="sf-menu" id="nav" style="font-size:20px;text-align:center;">		    
            <li style="width:100%;"><span>เลือกข้อสอบทีละข้อ หรือจะสุ่มจากจำนวนก็ได้ค่ะ</span></li>             
          </ul>       
      </nav>
    </header>
            <div id="site_content" style="padding: 0; margin: auto;">
                <div class="content" style="width: 930px; margin: 0 0 5px 0;">
                    <table>
                        <tr>
                            <td style="width: 50%; text-align: center;">
                                <input type="checkbox" id="chkAllQuestions" onclick="selectedAllQuestion(this);" /><label for="chkAllQuestions">เลือกใช้ทั้งหมด</label>
                            </td>
                            <td >
                                <label>สุ่มข้อสอบ</label>
                                <input type="text" class="txtRandom" maxlength="3" onkeypress='return event.charCode >= 48 && event.charCode <= 57' />
                                <label>ข้อ</label>
                            </td>
                        </tr>
                    </table>
                    <div runat="server" id="divContent">
                    </div>
                </div>
                <div>
                    <asp:Button ID="btnBack" ClientIDMode="Static" runat="server" class="submitChangeFontSize" Text="<< ไปหน้าเลือกชุดข้อสอบ"></asp:Button>
                    <asp:Button ID="btnNext" ClientIDMode="Static" runat="server" style="float: right;" class="submitChangeFontSize" Text="ไปหน้าสรุปชุดข้อสอบ >>"></asp:Button>
                </div>

                 <div class="divQuestionSelected">
                <span>เลือกมาแล้ว</span><br />
                <span class="lblQuestionAmount"></span><br />
                <span>ข้อ</span>
            </div>
            </div>

            <footer style="margin-top: 15px">สงวนลิขสิทธิ์ &copy; บริษัท สำนักพิมพ์วัฒนาพานิช จำกัด</footer>
        </div>


        <div id="dialog"></div>
    </form>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ExtraScript" runat="server">
    <script type="text/javascript">

        

        $(function () {
            if (isAllQuestionSelected()) $('#chkAllQuestions').attr('checked', true);

            setQuestionAmountTxt();

            var delayID = null;
            $('.txtRandom').keyup(function (e) {
                var rdAmount = parseInt($(this).val());
               
                if (delayID == null) {
                    delayID = setTimeout(function () {
                        randomQuestion(rdAmount);
                    }, 1000);
                }
                else if (delayID != null) {
                    clearTimeout(delayID);                   
                    delayID = setTimeout(function () {
                        randomQuestion(rdAmount);
                    }, 1000);
                }
            });
            
            $('#btnNext').click(function (e) {
                var questionSeletedAmount = $('.chkSelected:checked').length;
                if (questionSeletedAmount == 0) {
                    e.preventDefault();
                    callAlertDialog('ต้องเลือกข้อสอบก่อนค่ะ');
                }
            });
        });

        function callAlertDialog(txt) {
            var $d = $('#dialog');
            var myBtn = {};
            myBtn["ตกลง"] = function () {
                $d.dialog('close');
            };
            $d.dialog({ buttons: myBtn, draggable: false, resizable: false, modal: true }).dialog('option', 'title', txt);
        }

        function selectedAllQuestion(chk) {                    
            if (chk.checked) {               
                $('.chkSelected:not(:checked)').each(function () {
                    $(this).attr('checked', true);
                    $(this).triggerHandler('click');
                });               
            } else {
                $('.chkSelected:checked').each(function () {
                    $(this).attr('checked', false);
                    $(this).triggerHandler('click');
                });
            }
        }

        function setQuestionAmountTxt() {
            //var questionAmount = $('input[type="checkbox"]:checked').length;
            var questionSeletedAmount = $('.chkSelected:checked').length;
            $('.lblQuestionAmount').text(questionSeletedAmount);

            if (isAllQuestionSelected()) {
                $('#chkAllQuestions').attr('checked', true);
            } else {
                
                $('#chkAllQuestions').attr('checked', false);
            }
        }

        function randomQuestion(rdAmount) {
            var qAmount = $('.chkSelected').length;
            if (rdAmount > qAmount) {
                $('.txtRandom').val('');
                return 0;
            }
            var arrCheckbox = new Array();
            $('.chkSelected').each(function () {
                if ($(this).attr('checked') == 'checked') {
                    $(this).attr('checked', false);
                    $(this).triggerHandler('click');
                }
                arrCheckbox.push($(this));
            });

            //var arrSelected = new Array();
            // loop random chkbox
            console.log(rdAmount);
            for (var i = 1; i <= rdAmount; i++) {
                var rd = (Math.floor(Math.random() * (arrCheckbox.length)));               
                arrCheckbox[rd].attr('checked', true);
                arrCheckbox[rd].triggerHandler('click');
                arrCheckbox.splice(rd, 1);
            }
            $('.txtRandom').val('');
        }

        function isAllQuestionSelected() {
            var questionSeletedAmount = $('.chkSelected:checked').length;
            var questionAmount = $('.chkSelected').length;
            if (questionAmount == questionSeletedAmount) return true;
            return false;
        }

        function setQuestionIsActive(chkBox, subjectId, classId, qsetId, questionId) {
           // console.log(questionId + ' --> ' + chkBox.checked);
            var dataToPost = "{ subjectId : '" + subjectId + "', classId : '" + classId + "', qsetId : '" + qsetId + "', questionId : '" + questionId + "', isActive : " + chkBox.checked + "}";
            var methodName = "SetQuestionIsActive";
            var className = (chkBox.checked) ? "selected" : "notselect";

            postToServer(methodName, dataToPost);

            //console.log(className);
            var divQuestion = $(chkBox).next().next();
            divQuestion.removeClass('selected');
            divQuestion.removeClass('notselect');
            divQuestion.addClass(className);

            setQuestionAmountTxt();
        }

        function setQuestionsIsActive(chkBox, subjectId, classId, qsetId) {
            //console.log(qsetId + ' --> ' + chkBox.checked);

            var dataToPost = "{ subjectId : '" + subjectId + "', classId : '" + classId + "', qsetId : '" + qsetId + "', isActive : " + chkBox.checked + "}";
            var methodName = "SetQuestionsIsActive";
            var className = (chkBox.checked) ? "selected" : "notselect";

            postToServer(methodName, dataToPost);

            var divQuestion = $(chkBox).next().next();
            divQuestion.removeClass('selected');
            divQuestion.removeClass('notselect');
            divQuestion.addClass(className);

            setQuestionAmountTxt();
        }

        function postToServer(methodName, dataToPost) {
            var val;
            $.ajax({
                type: "POST",
                url: "<%=ResolveUrl("~")%>WebServices/QuestionService.asmx/" + methodName,
                data: dataToPost,  //" 
                contentType: "application/json; charset=utf-8", dataType: "json",
                async: false,
                success: function (msg) {
                    val = msg.d;
                },
                error: function myfunction(request, status) {
                }
            });
            return val;
        }
    </script>
</asp:Content>
