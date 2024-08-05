<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="AddQuestionPage.aspx.vb" Inherits="QuickTest.AddQuestionPage" %>

<%@ Register Src="~/UserControl/pairQuestionControl.ascx" TagName="ChoiceControl" TagPrefix="uc" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../css/AddQuestionStyle.css" rel="stylesheet" />
    <link href="../css/jquery.qtip.css" rel="stylesheet" />
    <link href="../css/jquery-ui-1.8.18.custom.min.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .qtip {
            max-width: 1000px;
        }

        span.main {
            font-weight: bold;
            font-size: 14px;
        }
    </style>
    <%If QuestionSet.Type = 1 Or QuestionSet.Type = 2 Or QuestionSet.Type = 9 Then %>
    <style type="text/css">
        #divQuestion {
            margin-top: -60px;
            text-indent: 30px;
        }
        .txtIndent{
            text-indent:48px!important;
        }
    </style>
    <%End If %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadStyleContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div id="main" style="background-color: white; padding: 10px 0;">
        <h3>เพิ่มคำถามในหน่วย</h3>
        <div class="form">

            <div style="border-radius: inherit; border: 1px solid; padding: 5px 10px;">
                <span class="main"><%=Book.ToString() %></span><br />
                <span class="main">หน่วยการเรียนรู้  : </span><span><%=Book.QuestionCategories(0).Name %></span><br />
                <span class="main">ประเภทข้อสอบ  : </span><span><%=QuestionSet.TypeName %></span><br />
                <%If QuestionSet.Type = QuickTest.EnumQsetType.Choice Or QuestionSet.Type = QuickTest.EnumQsetType.TrueFalse Or QuestionSet.Type = QuickTest.EnumQsetType.Subjective Then %>
                <span class="main">ประเภทคำสั่ง  : </span><span><%=QuestionSet.Name %></span>
                <%End If %>
            </div>

            <%If QuestionSet.Type = QuickTest.EnumQsetType.Choice OrElse QuestionSet.Type = QuickTest.EnumQsetType.TrueFalse Or QuestionSet.Type = QuickTest.EnumQsetType.Subjective Then%>
            <div style="margin-top: 10px; display: flex;" id="QuestionNumberPanel">
                <span class="main">ข้อ</span>
                <div style="width: 100%; overflow: hidden; margin-left: 5px; display: flex;">
                    <div class="btnScroll" style="width: 43px; background-image: url('../images/left_circle.png'); cursor: pointer; margin-right: 5px; background-size: contain; background-repeat: no-repeat;"
                        onclick="scrollToLeft();">
                    </div>
                    <div class="QuestionNumberBar" style="overflow: hidden;">
                        <div class="btnQuestionNumberBar">
                            <%--<div class="currentQuestion">1</div>
                    <div>+</div>--%>
                        </div>
                    </div>
                    <div class="btnScroll" style="width: 43px; background-image: url('../images/right_circle.png'); cursor: pointer; margin-left: 5px; background-size: contain; background-repeat: no-repeat;" onclick="scrollToRight();"></div>
                    <div class="btnNewQuestion" style="width: 43px; background-image: url('../images/plus_circle.png'); cursor: pointer; margin-left: 5px; background-size: contain; background-repeat: no-repeat;">
                    </div>
                </div>
            </div>
            <%End If %>


            <uc:ChoiceControl runat="server" />

            <div style="position: relative; height: 30px; margin-top: 10px;">
                <input type="button" id="btnBack" value="กลับ" class="btn" style="position: absolute; left: 0;" />
                <input type="button" id="btnNext" value="ไปต่อ ดูสรุปก่อนบันทึก" class="btn" style="position: absolute; right: 0; width: 180px;" />
            </div>
        </div>
    </div>    
    <div id="dialog"></div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ExtraScript" runat="server">

    <script src="../js/jquery-1.7.1.min.js" type="text/javascript"></script>
    <script src="../js/jquery-ui-1.8.18.min.js" type="text/javascript"></script>
    <script src="../js/ckEditor/ckeditor.js" type="text/javascript"></script>
    <script src="<%=ResolveUrl("~")%>js/jquery.blockUI.js" type="text/javascript"></script>
    <script src="../js/jquery.qtip-1.0.0-rc3.js" type="text/javascript"></script>
    <script type="text/javascript">
        var groupSubjectId = '<%=Book.GroupSubjectId%>';
        var levelId = '<%=Book.LevelId%>';
        var qsetId = '<%=QsetId%>';

        var qset = getQuestionSet(qsetId);
        var questions = qset.Questions //[{Id : '352ded77-cd88-41b5-aa1f-a2e8d4b190cd'}];
        var question;

        var isRefreshPage = true;
        $(function () {
            // ปุ่ม back กลับไปยังหน้าเลือกข้อสอบ
            $('#btnBack').click(function () {
                var result = isCompletedQuestionSet(false);
                if (!result) { return 0; }
                window.location = '../testset/newstep3.aspx';
            });
            // ปุ่ม next ก่อนไปดูหน้าสรุป
            $('#btnNext').click(function () {
                // check ข้อสอบก่อนไปหน้าสรุป
                var result = isCompletedQuestionSet(true);
                if (!result) { return 0; }
                // save qset ที่พึ่งเพิ่มเข้าไป เข้า temptestset
                if (saveMyQsetToTestset()) {
                    window.location = '../testset/SelectQuestionsByUser.aspx';
                } else {
                    callAlertDialog("Error.. ไม่สามารถไปต่อหน้าสรุปได้ค่ะ");
                }
            });


            $('.btnQuestionNumberBar > div').live('click', function () {
                if ($(this).hasClass('questionSelected')) { return 0; }
                openBlockUI();
                if (!isRefreshPage) {
                    if (qset.Type !== 9) {
                        var txtIsComplete = isCompleteQuestion(question);
                        if (txtIsComplete !== "") {
                            txtIsComplete = txtIsComplete.replace("ถึงจะเพิ่มข้อใหม่ได้", "ถึงจะเปลี่ยนข้ออื่นได้");
                            callAlertDialog(txtIsComplete); unBlockUI();
                            return 0;
                        }
                    }
                } else {
                    isRefreshPage = !isRefreshPage;
                }
                // ให้เปิด div ที่เป็นข้อสอบไว้
                $('.divQuestionAndAnswerExplain').stop().hide();
                $('.divQuestionAndAnswer').stop().show();

                $('.btnQuestionNumber').removeClass('questionSelected');
                $(this).addClass('questionSelected');
                var questionId = $(this).attr("value");
                question = getQuestion(questionId);
                console.log(question.Answers[0].Name);
                $('.spnQuestionNumber').text(question.No + ".");
                initDataToQuestion();
                initDataToAnswer();
                initialEvalutionQtip();
                setTimeout(function () {
                    unBlockUI();
                }, 350);
                if (question.No >= 10) {
                    $('#divQuestion').addClass('txtIndent');
                } else {
                    $('#divQuestion').removeClass('txtIndent');
                }
            });

            if (questions != null) {
                if (qset.Type == 1 || qset.Type == 2 || qset.Type == 9) {
                    createBtnQuestionNumber();
                    //สั่งให้กดปุ่มข้อที่ 1 แล้วไปทำการ get คำถามในข้อนั้นออกมา  
                    //ลองเปลี่ยนให้คลิกข้อสุดท้ายแทน
                    $('div[value=' + questions[questions.length - 1].Id + ']').trigger("click");
                } else {
                    initDataToQuestion();
                    initDataToAnswer();
                }
            }

            //var contents = $('.EditQuestion').html();
            // function ในการ save ชื่อคำถามใน tab สีเหลือง
            $('.EditQuestion').blur(function () {
                var contents = $(this).html();
                if (qset.Type == 1 || qset.Type == 2 || qset.Type == 9) {
                    if (question.Name != contents) {
                        question.Name = contents;
                        saveQuestion();
                    }
                } else {
                    if (qset.Name != contents) {
                        qset.Name = contents;
                        saveQuestionset();
                    }
                }
            });

            // save คำถาม แบบจับคู่และเรียงลำดับ
            $('.EditQuestionChoice').live('blur', function () {
                var contents = $(this).html();
                var questionId = $(this).attr('value');
                $.each(questions, function (i, item) {
                    if (item.Id == questionId) {
                        if (item.Name != contents) {
                            item.Name = contents;
                            console.log(item);
                            question = item;
                            saveQuestion();
                            return 0;
                        }
                    }
                });
            });

            //hover for show btn delete question
            $('#mainQuestion').hover(function () {
                //$(this).append($('<img>', { src: '../Images/Delete-icon.png', 'class': 'btnDeleteQuestion', onclick: 'deleteQuestion();' }));
                if (questions.length > 1 && (qset.Type == 1 || qset.Type == 2 || qset.Type == 9)) {
                    $(this).append($('<img>', { src: '../Images/Delete-icon.png', 'class': 'btnDeleteQuestion', onclick: 'callDialog("deleteQuestion","","ต้องการลบคำถามข้อนี้นะคะ");' }));
                }
            }, function () {
                $(this).find('img.btnDeleteQuestion').remove();
            });


            // function สำหรับ save คำอธิบายโจทย์
            $('.EditQuestionExplain').blur(function () {
                var contents = $(this).html();
                if (qset.Type == 1 || qset.Type == 2 || qset.Type == 9) {
                    if (question.ExplainName != contents) {
                        question.ExplainName = contents;
                        saveQuestion();
                    }
                } else {
                    $.each(questions, function (i, item) {
                        item.ExplainName = contents;
                    });
                    saveQuestionExplain(contents);
                }
            });

            // กดออกจากช่องที่ กรอกคำตอบเพื่อทำการ save 
            $('.EditAnswer').live('blur', function () {
                var answerId = $(this).attr('value');
                var answerName = $(this).html();
                var answer = { Id: answerId, Name: answerName };
                saveAnswer(answer);
            });

            // กดออกจากช่องที่ กรอกคำอธิบายคำตอบเพื่อทำการ save 
            $('.EditAnswerExplain').live('blur', function () {
                var answerId = $(this).attr('value');
                var explainName = $(this).html();
                var answer = { Id: answerId, ExplainName: explainName };
                saveAnswerExplain(answer);
            });

            $(".btnNewQuestion").click(function () {
                isRefreshPage = true;
                if (qset.Type !== 9) {
                    var txtIsComplete = isCompleteQuestion(question);
                    if (txtIsComplete !== "") {
                        callAlertDialog(txtIsComplete);
                        return 0;
                    }
                }
                question = newQuestion();
                if (question != null) {
                    question = JSON.parse(question);
                    questions.push(question);
                    createBtnQuestionNumber();
                    $('div[value=' + question.Id + ']').trigger("click");
                }
            });
        });

        function isCompletedQuestionSet(isNext) {
            var result = true;
            var txtToPage = (isNext) ? "ถึงจะไปหน้าสรุปข้อสอบได้ค่ะ" : "ถึงจะกลับไปเลือกชุดข้อสอบได้";
            if (qset.Type == 1 || qset.Type == 2) {
                var txtIsComplete = isCompleteQuestion(question);
                if (txtIsComplete !== "") {
                    txtIsComplete = txtIsComplete.replace("ถึงจะเพิ่มข้อใหม่ได้", "ถึงจะไปหน้าสรุปข้อสอบได้ค่ะ");
                    callAlertDialog(txtIsComplete);
                    return 0;
                }
            } else if (qset.Type == 3) {
                $.each(questions, function (i, item) {
                    if (item.Name == "" || item.Answers[0].Name == "") {
                        callAlertDialog("ต้องใส่คำถามและคำตอบจับคู่ให้ครบก่อนค่ะ " + txtToPage);
                        result = false;
                        return false;
                    }
                });
            } else if (qset.Type == 6) {
                $.each(questions, function (i, item) {
                    if (item.Name == "") {
                        callAlertDialog("ต้องใส่ตัวเลือกเรียงลำดับให้ครบก่อนค่ะ " + txtToPage);
                        result = false;
                        return false;
                    }
                });
            }
            return result;
        }

        function isCompleteQuestion(currentQuestion) {
            var alertTxt = "";
            if (currentQuestion.Name == "") {
                alertTxt = "ใส่คำถามก่อนค่ะ ถึงจะเพิ่มข้อใหม่ได้";
            } else {
                var score = 0;
                for (var i = 0; i < currentQuestion.Answers.length; i++) {
                    score = score + currentQuestion.Answers[i].Score;

                    if (currentQuestion.Answers[i].Name == "") {
                        alertTxt = "ใส่คำตอบของ " + prefixAnswerThai[i] + ". ก่อนค่ะ";
                        break;
                    }
                }

                if (alertTxt == "") {
                    alertTxt = (score >= 1) ? alertTxt : "เลือกคำตอบที่ถูกต้องก่อนค่ะ";
                }
            }
            return alertTxt;
        }

        function openBlockUI() {
            $.blockUI({
                css: {
                    backgroundColor: 'initial',
                    border: 0
                },
                message: '<img src="<%=ResolveUrl("~")%>Images/waitspinner.gif" width="100" height="100" /><br /><span style="font-size:35px;">รอสักครู่นะคะ</span>'
            });
        }
        function unBlockUI() {
            $.unblockUI();
        }

        // Turn off automatic editor creation first.
        CKEDITOR.disableAutoInline = true;

        // function ไว้เช็คว่า element นี้ เป็น ckeditor หรือยัง
        function ckeditorExist(eleName) {
            var v = false;
            eleName = eleName.replace('#', '');
            for (var instanceName in CKEDITOR.instances) {
                var tempEle = CKEDITOR.instances[instanceName];
                if (tempEle.name == eleName) {
                    //console.log('เป็น ckeditor อยู่แล้ว'); 
                    v = true;
                }
            }
            return v;
        }

        // นำ element มาแปลงเป็น ckeditor
        function InjectCKEditorToControl(ele) {
            if (ckeditorExist(ele)) return 0;
            $(ele).each(function () {
                //console.log('create ckeditor id = ' + ele);
                CKEDITOR.inline(this, {
                    enterMode: Number(2),
                    toolbar: [
                        { name: 'fontstyles', items: ['Bold', 'Italic', 'Underline', 'Strike'] },
                        //{ name: 'clipboard', items: ['Cut', 'Copy', 'Paste', 'Undo', 'Redo'] },
                        { name: 'paragraph', items: ['JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock'] },
                        '/',
                        { name: 'paragraphstyles', items: ['Styles', 'Format', 'Font', 'FontSize'] },
                        { name: 'image', items: ['TextColor', 'BGColor', 'Image'] }
                    ]
                });
            });
        }

        CKEDITOR.on('fileUploadRequest', function (evt) {
            alert(1);
        });

        var prefixAnswerThai = ["ก", "ข", "ค", "ง", "จ", "ฉ", "ช", "ซ", "ฌ", "ด"];
        var prefixAnswerEng = ["a", "b", "c", "d", "e", "f", "g"];

        // สำหรับ เอาข้อมูลใส่ element ส่วนคำถาม
        function initDataToQuestion() {
            var questionName = (qset.Type == 1 || qset.Type == 2 || qset.Type == 9) ? question.Name : qset.Name;
            InjectCKEditorToControl('#divQuestion');
            CKEDITOR.instances['divQuestion'].setData(questionName);
            //$('.EditQuestion').html(questionName);
            //$('.spnQuestionNumber').next().html(questionName);
        }

        // สำหรับ เอาข้อมูลใส่ element ตอนที่กดปุ่มหลอดไฟ    
        function initDataToQuestionExplain() {
            var questionName = (qset.Type == 1 || qset.Type == 2 || qset.Type == 9) ? question.Name : qset.Name;
            $('#divQuestionExp').html(questionName);
            var questionExplainName = (qset.Type == 1 || qset.Type == 2 || qset.Type == 9) ? question.ExplainName : questions[0].ExplainName;
            InjectCKEditorToControl('#QuestionExp');
            CKEDITOR.instances['QuestionExp'].setData(questionExplainName);
            // $('.EditQuestionExplain').text(questionExplainName);            
        }

        // สำหรับ เอาข้อมูลใส่ element ส่วนคำตอบ
        function initDataToAnswer() {
            if (qset.Type == 1) {
                initDataToChoiceAnswer();
            } else if (qset.Type == 2) {
                initDataToTrueFalseAnswer();
            } else if (qset.Type == 3) {
                initDataToPairAnswer();
            } else if (qset.Type == 6) {
                initDataToSortAnswer();
            } else if (qset.Type == 9) {
                initDataToSubjectiveAnswer();
            }
        }

        // สร้าง ckeditor ขึ้นมาใหม่
        function reCreateCkEditorExplain(eleName, txt) {
            var tempEle = CKEDITOR.instances[eleName];
            if (tempEle != undefined) tempEle.destroy();
            InjectCKEditorToControl('#' + eleName);
            CKEDITOR.instances[eleName].setData(txt);
        }

        // function สำหรับ สร้างส่วนของคำตอบแบบอัตนัย
        function initDataToSubjectiveAnswer() {
            $('#Table1').empty();
            var tr = $('<tr>');
            $.each(question.Answers, function (i, item) {
                var divAnswerId = "divAnswer" + i;
                $(tr).append($('<td style="position:relative;width:100%;">').append($('<div>', {
                    'class': "EditAnswer",
                    value: item.Id,
                    defaultText: "คลิกที่นี่เพื่อใส่คำตอบ",
                    contenteditable: "true",
                    id: divAnswerId
                })));
                $('#Table1').append($(tr));
                InjectCKEditorToControl('#' + divAnswerId);
                CKEDITOR.instances[divAnswerId].setData(item.Name);
            });
        }
        function initDataToSubjectiveAnswerExplain() {
            var table = $('<table>')
            var tr = $('<tr>');
            $.each(question.Answers, function (i, item) {
                var divAnswerId = "divAnswer" + i;
                $(tr).append($('<td style="position:relative;width:100%;">').append($('<div>', {
                    html: item.Name,
                    id: divAnswerId
                })));
                $(table).append($(tr));
            });
            $('#AnswerExp').append($(table));
        }

        // function สำหรับ save คำอธิบายคำตอบ
        function initDataToAnswerExplain() {
            $('#AnswerExp').empty();
            if (qset.Type == 1 || qset.Type == 2) {
                $.each(question.Answers, function (i, item) {
                    var divAnswerId = "divAnswerExp" + i;
                    var checked = (item.Score == 0) ? false : true;
                    var classDivAnswer = (checked) ? "answerExplain Correct" : "answerExplain";
                    $('#AnswerExp').append(
                    $('<div>').append($('<div>', { 'class': classDivAnswer })
                    .append($('<table>').append($('<tr>')
                    .append($('<td>', { 'style': 'width: 45px;' }).append($('<input />', { type: 'checkbox', id: item.Id, 'class': 'rightAnswer', 'checked': checked })).append($('<label>', { 'for': item.Id })).append($('<span>', { text: prefixAnswerThai[i] + '.' })))
                    .append($('<td>').append($('<span>', { html: item.Name })).append($('<div>', { id: divAnswerId, 'class': 'CanEdit EditAnswerExplain', contenteditable: true, defaultText: "คลิกที่นี่เพื่อใส่อธิบายคำตอบ", value: item.Id, text: item.ExplainName, width: 595 })))))));
                });
                // วนใช้งานแปลงเป็น ckeditor
                $.each(question.Answers, function (i, item) {
                    reCreateCkEditorExplain("divAnswerExp" + i, item.ExplainName);
                });
            } else if (qset.Type == 3) {
                initDataToPairAnswerExplain();
            } else if (qset.Type == 6) {
                initDataToSortAnswerExplain();
            } else if (qset.Type == 9) {
                initDataToSubjectiveAnswerExplain();
            }
        }

        // สร้างส้่วนคำตอบแบบ choice
        function initDataToChoiceAnswer() {
            $('#Table1').empty();
            var tr;
            var btnAddNewAnswer = $('<td colspan="2">').append($('<div class="addAnswer" onclick="newAnswer();">').append($('<img>', { src: '../Images/plus_circle.png', width: '30px' })).append($('<span>', { text: "เพิ่มตัวเลือก" })));
            $.each(question.Answers, function (i, item) {
                var divAnswerId = "divAnswer" + i;
                if ((i + 1) % 2 != 0) {
                    tr = $('<tr>');
                }
                var classTd = (item.Score == 0) ? "" : "Correct";
                $(tr).append($('<td>', { text: prefixAnswerThai[i] + ".", "class": classTd }));
                $(tr).append($('<td style="position:relative;width:45%;" class="' + classTd + '">').append($('<div>', {
                    'class': "EditAnswer",
                    value: item.Id,
                    defaultText: "คลิกที่นี่เพื่อใส่คำตอบ",
                    //text: item.Name,
                    contenteditable: "true",
                    id: divAnswerId,
                    width: 292
                })));

                if ((i + 1) % 2 == 0) {
                    $('#Table1').append($(tr));
                }
                if ((i + 1) == question.Answers.length && (i + 1) % 2 != 0) {
                    $(tr).append(btnAddNewAnswer);
                    $('#Table1').append($(tr));
                } else if ((i + 1) == question.Answers.length && (i + 1) % 2 == 0) {
                    tr = $('<tr>');
                    $(tr).append(btnAddNewAnswer);
                    $('#Table1').append($(tr));
                }
                //InjectCKEditorToControl('#divAnswer1');                
            });

            // วนใช้งานแปลงเป็น ckeditor
            $.each(question.Answers, function (i, item) {
                //InjectCKEditorToControl('#divAnswer' + i);
                reCreateCkEditorExplain('divAnswer' + i, item.Name);
                //CKEDITOR.instances['divAnswer' + i].setData(item.Name);
            });

            //delete answer
            if (question.Answers.length > 3) {
                $('.EditAnswer').parent().hover(function () {
                    var c = $(this).children().attr('id');
                    c = c.replace('divAnswer', '');
                    var prefixTxt = prefixAnswerThai[c] + ".";
                    var titleTxt = "ต้องการลบตัวเลือก " + prefixTxt + " ?";
                    var contentTxt = prefixTxt + " " + $(this).children().text();
                    $(this).append($('<img>', { src: '../Images/Delete-icon.png', 'class': 'btnDeleteAnswer', onclick: 'callDialogChoice("deleteAnswer","' + $(this).children().attr('value') + '","' + titleTxt + '","' + contentTxt + '");' }));
                }, function () {
                    $(this).find('img.btnDeleteAnswer').remove();
                });
            }
        }

        // สร้างส่วนคำตอบแบบถูกผิด
        function initDataToTrueFalseAnswer() {
            $('#Table1').empty();
            var tr;
            $.each(question.Answers, function (i, item) {
                if ((i + 1) % 2 != 0) {
                    tr = $('<tr>');
                }
                var classTd = (item.Score == 0) ? "" : "Correct";
                $(tr).append($('<td>', { text: prefixAnswerThai[i] + ".", "class": classTd }));
                $(tr).append($('<td style="position:relative;width:45%;" class="' + classTd + '">').append($('<div>', {
                    value: item.Id,
                    text: item.Name,
                })));
                if ((i + 1) % 2 == 0) {
                    $('#Table1').append($(tr));
                }
            });
        }

        // function ในการเรียนเพื่อ post ไป save หรือ get ข้อมูลจากฝั้ง server
        function ajaxPostToWebService(functionName, obj) {
            console.log('post to server functionName = ' + functionName + ', param = ' + obj);
            var returnValue;
            $.ajax({
                type: "POST",
                url: "../WebServices/QuestionService.asmx/" + functionName,
                data: obj,
                async: false,
                contentType: "application/json; charset=utf-8", dataType: "json",
                success: function (data) {
                    returnValue = data.d;
                },
                error: function (xhr, status, text) {
                    alert(status + " " + text);
                }
            });
            return returnValue;
        }

        // function ในการเพิ่มคำถามแบบ choice และ ถูกผิด
        function newQuestion() {
            var functionName = "NewQuestion";
            var obj = "{ qsetId : '" + qsetId + "', qsetType : '" + qset.Type + "'}";
            var question = ajaxPostToWebService(functionName, obj);
            return question;
        }

        function deleteQuestion() {
            isRefreshPage = true;
            var functionName = "DeleteQuestion";
            var obj = "{ questionId : '" + question.Id + "', qsetId : '" + qsetId + "', qsetType : '" + qset.Type + "'}";
            var result = ajaxPostToWebService(functionName, obj);
            if (result) {
                callAlertDialog("ลบคำถามเรียบร้อยแล้วค่ะ");
                var tempIndex = -1;
                $.each(questions, function (i, item) {
                    if (item.Id == question.Id) { tempIndex = i; }
                });
                questions.splice(tempIndex, 1);

                var tempId = $('div[value=' + question.Id + ']').next().attr('value');
                if (tempId == undefined) {
                    tempId = $('div[value=' + question.Id + ']').prev().attr('value');
                }
                createBtnQuestionNumber();
                if (tempId != undefined) $('div[value=' + tempId + ']').trigger("click");
            } else {
                callAlertDialog("ไมสามารถลบคำถามได้ค่ะ");
            }
        }

        // เพิ่มคำตอบแบบ choice
        function newAnswer() {
            var answerNo = question.Answers.length + 1;
            if (answerNo > 10) { callAlertDialog("ไม่สามารถเพิ่มคำตอบได้แล้วค่ะ เนื่องจากตัวเลือกจะเยอะเกินไป"); return 0; }
            var functionName = "NewAnswer";
            var obj = "{ questionId : '" + question.Id + "', qsetId : '" + qsetId + "', answerNo : '" + answerNo + "'}";
            var answer = ajaxPostToWebService(functionName, obj);
            if (answer != null) {
                $.each(question.Answers, function (i, item) {
                    //var tempEle = CKEDITOR.instances['divAnswer' + i];
                    //tempEle.destroy();
                    CKEDITOR.instances['divAnswer' + i].destroy();
                    //CKEDITOR.instances['divAnswerExp' + i].destroy();
                });
                question.Answers.push(answer);
                initDataToAnswer();
            }
        }
        // ลบคำตอบแบบ choice
        function deleteAnswer(answerId) {
            var functionName = "DeleteAnswer";
            var obj = "{ questionId : '" + question.Id + "', qsetId : '" + qsetId + "', answerId : '" + answerId + "'}";
            var answer = ajaxPostToWebService(functionName, obj);
            if (answer == true) {
                callAlertDialog("ลบตัวเลือกเรียบร้อยแล้ว");
                var tempIndex = -1;
                $.each(question.Answers, function (i, item) {
                    //var tempEle = CKEDITOR.instances['divAnswer' + i].destroy();
                    //tempEle.destroy();
                    CKEDITOR.instances['divAnswer' + i].destroy();
                    //CKEDITOR.instances['divAnswerExp' + i].destroy();                    
                    if (item.Id == answerId) { tempIndex = i; }
                });
                question.Answers.splice(tempIndex, 1);
                initDataToAnswer();
            } else {
                callAlertDialog("ลบตัวเลือกไม่ได้");
            }
        }

        // function สำหรับเพิ่ม คำถามและคำตอบใหม่ จับคู่ และเรียงลำดับ
        function newQuestionAnswer() {
            var functionName = "NewQuestionAnswer";
            var obj = "{ qsetId : '" + qsetId + "',qsetType : '" + qset.Type + "'}";
            var result = ajaxPostToWebService(functionName, obj);
            if (result != null) {
                $.each(questions, function (i, item) {
                    CKEDITOR.instances['divQuestion' + i].destroy();
                    if (qset.Type == 3) CKEDITOR.instances['divAnswer' + i].destroy();
                });
                var q = JSON.parse(result);
                questions.push(q);
                initDataToAnswer();
            }
        }

        // function ลบทั้งคำถามและคำตอบ สำหรับจับคู่และเรียงลำดับ
        function deleteQuestionAnswer(questionId) {
            // to do รอทำ dialog ยืนยันการลบมาแสดง
            var functionName = "DeleteQuestionAnswer";
            var obj = "{ qsetId : '" + qsetId + "', qsetType : " + qset.Type + ", questionId : '" + questionId + "'}";
            var result = ajaxPostToWebService(functionName, obj);
            if (result) {
                var tempIndex = -1;
                $.each(questions, function (i, item) {
                    CKEDITOR.instances['divQuestion' + i].destroy();
                    if (qset.Type == 3) ifCKEDITOR.instances['divAnswer' + i].destroy();
                    if (item.Id == questionId) { tempIndex = i; }
                });
                questions.splice(tempIndex, 1);
                initDataToAnswer();
            }
        }

        // function ในการ save คำถามและคำอธิบายคำถาม
        function saveQuestion() {
            var functionName = "SaveQuestion";
            var tempQuestion = { Id: question.Id, Name: encodeURIComponent(question.Name), ExplainName: encodeURIComponent(question.ExplainName) };
            var obj = "{ objStr : '" + JSON.stringify(tempQuestion) + "'}";
            var result = ajaxPostToWebService(functionName, obj);
            //question.Name = decodeURIComponent(question.Name);
            //question.ExplainName = decodeURIComponent(question.ExplainName);
        }

        // function ในการ save คำถามของแบบจับคู่และเรียงลำดับ
        function saveQuestionset() {
            var functionName = "SaveQuestionset";
            var tempQset = { Id: qset.Id, Name: encodeURIComponent(qset.Name) };
            var obj = "{ objStr : '" + JSON.stringify(tempQset) + "'}";
            var result = ajaxPostToWebService(functionName, obj);
            if (result == false) {
                //alert('save not success');
            }
        }

        // สำหรับ save คำอธิบายคำถาม ข้อสอบแบบจับคู่และเรียงลำดับ
        function saveQuestionExplain(questionExplain) {
            var functionName = "SaveQuestionExplain";
            var tempQset = { Id: qset.Id };
            var obj = "{ objStr : '" + JSON.stringify(tempQset) + "', questionExplain : '" + encodeURIComponent(questionExplain) + "'}";
            var result = ajaxPostToWebService(functionName, obj);
            if (result == false) {
                //alert('save not success');
            }
        }

        // function ในการ save คำตอบ เวลากดแก้ไข
        function saveAnswer(answer) {
            var functionName = "SaveAnswer";
            answer.Name = encodeURIComponent(answer.Name);
            var obj = "{ objStr : '" + JSON.stringify(answer) + "'}";
            var result = ajaxPostToWebService(functionName, obj);
            answer.Name = decodeURIComponent(answer.Name);
            if (result == true) {
                if (qset.Type == 1 || qset.Type == 2) {
                    var tempIndex = -1;
                    $.each(question.Answers, function (i, item) {
                        if (item.Id == answer.Id) { tempIndex = i; }
                    });
                    question.Answers[tempIndex].Name = answer.Name;
                } else {
                    $.each(questions, function (i, item) {
                        if (item.Answers[0].Id == answer.Id) { item.Answers[0].Name = answer.Name; return false; }
                    });
                }
            }
        }

        // function ในการ save คำอธิบายคำตอบ
        function saveAnswerExplain(answer) {
            var functionName = "SaveAnswerExplain";
            answer.ExplainName = encodeURIComponent(answer.ExplainName);
            var obj = "{ objStr : '" + JSON.stringify(answer) + "'}";
            var result = ajaxPostToWebService(functionName, obj);
            answer.ExplainName = decodeURIComponent(answer.ExplainName);
            if (result == true) {
                if (qset.Type == 1 || qset.Type == 2) {
                    var tempIndex = -1;
                    $.each(question.Answers, function (i, item) {
                        if (item.Id == answer.Id) { tempIndex = i; }
                    });
                    question.Answers[tempIndex].ExplainName = answer.ExplainName;
                } else {
                    $.each(questions, function (i, item) {
                        if (item.Answers[0].Id == answer.Id) { item.Answers[0].ExplainName = answer.ExplainName; return false; }
                    });
                }
            }
        }

        // function ในการ get questionset
        function getQuestionSet(qsetId) {
            var functionName = "GetQuestionSet";
            var obj = "{ qsetId : '" + qsetId + "'}";
            var result = ajaxPostToWebService(functionName, obj);
            return JSON.parse(result);
        }

        // function ในการ get คำถามตอนกดเลขข้อ
        function getQuestion(questionId) {
            var functionName = "GetQuestion";
            var obj = "{ questionId : '" + questionId + "'}";
            var result = ajaxPostToWebService(functionName, obj);
            return JSON.parse(result);
        }
    </script>
    <script type="text/javascript">
        // save qset ที่สร้างไว้ เข้า testset
        function saveMyQsetToTestset() {
            var functionName = "SaveMyQsetToTestset";
            var obj = "{ qsetId : '" + qset.Id + "',qsetType : '" + qset.Type + "',subjectId: '" + groupSubjectId + "',classId: '" + levelId + "'}";
            var result = ajaxPostToWebService(functionName, obj);
            return result;
        }

        function callDialog(functionName, param, titleTxt) {
            var $d = $('#dialog');
            var myBtn = {};

            myBtn["ยกเลิก"] = function () {
                $d.dialog('close');
            };
            myBtn["ตกลง"] = function () {
                $d.dialog('close');
                var fn = eval(functionName);
                fn(param);
            };
            $d.html('');
            $d.dialog({ buttons: myBtn, draggable: false, resizable: false, modal: true }).dialog('option', 'title', titleTxt);
        }

        function callDialogChoice(functionName, param, titleTxt, contentTxt) {
            var $d = $('#dialog');
            var myBtn = {};

            myBtn["ยกเลิก"] = function () {
                $d.dialog('close');
            };
            myBtn["ตกลง"] = function () {
                $d.dialog('close');
                var fn = eval(functionName);
                fn(param);
            };            
            $d.html(contentTxt);
            $d.dialog({ buttons: myBtn, draggable: false, resizable: false, modal: true }).dialog('option', 'title', titleTxt);
        }

        function callAlertDialog(titleName) {
            var $d = $('#dialog');
            var myBtn = {};
            myBtn["ตกลง"] = function () {
                $d.dialog('close');
            };
            $d.html('');
            $d.dialog({ buttons: myBtn, draggable: false, resizable: false, modal: true }).dialog('option', 'title', titleName);
        }
    </script>
    <script type="text/javascript">
        /** function ในการ Render Question And Answer **/
        function initDataToPairAnswer() {
            $('#Table1').empty();
            var $tr;
            $.each(questions, function (i, item) {
                var divQuestionId = 'divQuestion' + i;
                var divAnswerId = 'divAnswer' + i;
                $tr = $('<tr>', { "class": "questionTr", style: "position:relative;", value: item.Id });
                $tr.append($('<td style="width:45%;">').append($('<div>', { id: divQuestionId, 'class': 'EditQuestionChoice', contenteditable: "true", defaultText: "คลิกที่นี่เพื่อใส่คำถาม", value: item.Id }))); //, text: item.Name
                $tr.append($('<td style="width:10%;">').append($('<span>', { text: "คู่กับ" })));
                $tr.append($('<td>').append($('<div>', { id: divAnswerId, 'class': "EditAnswer", contenteditable: "true", defaultText: "คลิกที่นี่เพื่อใส่คำตอบ", value: item.Answers[0].Id }))); //text: item.Answers[0].Name
                $('#Table1').append($tr);
                //แปลงเป็น ckeditor
                InjectCKEditorToControl('#' + divQuestionId);
                CKEDITOR.instances[divQuestionId].setData(item.Name);
                InjectCKEditorToControl('#' + divAnswerId);
                CKEDITOR.instances[divAnswerId].setData(item.Answers[0].Name);
            });

            $tr = $('<tr>');
            $tr.append($('<td colspan="3">').append($('<div>', { "class": "addAnswer", onclick: "newQuestionAnswer();" }).append($('<img>', { src: '../Images/plus_circle.png', width: '30px' })).append($('<span>', { text: "เพิ่มคำถามและคำตอบ" }))));
            $('#Table1').append($tr);

            $('.questionTr').hover(function () {
                $(this).children('td').next().next().append($('<img>', { src: '../Images/Delete-icon.png', 'class': 'btnDeleteAnswer', onclick: 'callDialog("deleteQuestionAnswer","' + $(this).attr('value') + '","ต้องการลบตัวเลือกจับคู่ ?");' }));
            }, function () {
                $(this).children('td').next().next().find('img.btnDeleteAnswer').remove();
            });
        }

        function initDataToPairAnswerExplain() {
            console.log("สร้างคำอธิบายคำตอบแบบจับคู่");
            $('#AnswerExp').empty();
            $.each(questions, function (i, item) {
                var divAnswerId = "divAnswerExp" + i;
                var $div = $('<div>', { "class": "answerExplain" });
                var $table = $('<table>');
                var $tr = $('<tr>', { "class": "questionTr", style: "position:relative;", value: item.Id });
                $tr.append($('<td style="width:45%;">').append($('<span>', { html: item.Name })));
                $tr.append($('<td style="width:10%;">').append($('<span>', { text: "คู่กับ" })));
                $tr.append($('<td>').append($('<span>', { html: item.Answers[0].Name })));
                $table.append($tr);
                $div.append($table);
                $div.append($('<div>', { id: divAnswerId, 'class': 'CanEdit EditAnswerExplain', contenteditable: true, defaultText: "คลิกที่นี่เพื่อใส่อธิบายคำตอบ", value: item.Answers[0].Id, text: item.Answers[0].ExplainName }));
                $('#AnswerExp').append($div);

                reCreateCkEditorExplain(divAnswerId, item.Answers[0].ExplainName);
            });
        }

        function initDataToSortAnswer() {
            console.log("สร้างส่วนของคำตอบแบบเรียงลำดับ");
            $('#Table1').empty();
            var $ul = $('<ul>', { id: "sortable" });
            $.each(questions, function (i, item) {
                var divQuestionId = 'divQuestion' + i;
                $ul.append($('<li>', { id: item.Id }).append($('<div>', { id: divQuestionId, 'class': 'CanEdit EditQuestionChoice', contenteditable: true, defaultText: "คลิกที่นี่เพื่อใส่ตัวเลือก", value: item.Id }))); //, text: item.Name
            });
            $('#Table1').append($('<tr>').append($('<td>').append($ul).append($('<div>', { "class": "addAnswer", onclick: "newQuestionAnswer();" }).append($('<img>', { src: '../Images/plus_circle.png', width: '30px' })).append($('<span>', { text: "เพิ่มตัวเลือก" })))));

            // สร้าง ckeditor
            $.each(questions, function (i, item) {
                InjectCKEditorToControl('#divQuestion' + i);
                CKEDITOR.instances['divQuestion' + i].setData(item.Name);
            });

            $('li').hover(function () {
                $(this).append($('<img>', { src: '../Images/Delete-icon.png', 'class': 'btnDeleteAnswer', onclick: 'callDialog("deleteQuestionAnswer","' + $(this).attr('id') + '","ต้องการลบตัวเลือกการเรียงลำดับ ?");' }));
            }, function () {
                $(this).find('img.btnDeleteAnswer').remove();
            });

            //if (questions.length > 1) {
            //    $('#sortable').sortable({
            //        placeholder: "ui-state-highlight",
            //        stop: function (event, ui) {
            //            saveReOrderQuestionAnswer();
            //        }
            //    });
            //}
        }

        function initDataToSortAnswerExplain() {
            $('#AnswerExp').empty();
            var $table = $('<table>');
            $.each(questions, function (i, item) {
                var divQuestionExpId = 'divQuestionExp' + i;
                var $tr = $('<tr>').append($('<td>').append($('<div>', { "class": "answerExplain" }).append($('<span>', { html: (item.Name == "") ? "ยังไม่ได้ใส่ตัวเลือก" : item.Name })).append($('<div>', { id: divQuestionExpId, 'class': 'CanEdit EditAnswerExplain', contenteditable: true, defaultText: "คลิกที่นี่เพื่อใส่อธิบายคำตอบ", value: item.Answers[0].Id })))); //, text: item.Answers[0].ExplainName
                $table.append($tr);
            });
            $('#AnswerExp').append($table);
            $.each(questions, function (i, item) {
                console.log(item.Answers[0].ExplainName);
                reCreateCkEditorExplain('divQuestionExp' + i, item.Answers[0].ExplainName);
            });
        }

        function saveReOrderQuestionAnswer() {
            var tempQuestions = getQuestionInSortable();
            var functionName = "SaveReOrderQuestionAnswer";
            var obj = "{ qsetId : '" + qsetId + "',qsetType : '" + qset.Type + "',tempQuestions : '" + tempQuestions + "'}";
            var result = ajaxPostToWebService(functionName, obj);
            if (result == null) return 0;

            questions = JSON.parse(result);

            initDataToSortAnswer();
            //initDataToSortAnswerExplain();
        }

        function getQuestionInSortable() {
            var arrQuestions = [];
            $('#sortable').children().each(function () {
                var questionId = $(this).attr('id');
                arrQuestions.push(questionId);
            });
            return arrQuestions;
        }
    </script>
    <script type="text/javascript">
        function createBtnQuestionNumber() {
            $('.btnQuestionNumberBar').empty();
            $.each(questions, function (i, item) {
                addBtnQuestionNumber(item.Id, i + 1);
            });

            if (questions.length > 18) {
                $('.btnScroll').show();
            } else {
                $('.btnScroll').hide();
            }

            //if ($('.QuestionNumberBar').hasScrollBar()) {
            //    $('.btnScroll').show();
            //} else {
            //    $('.btnScroll').hide();
            //}
        }

        function addBtnQuestionNumber(value, text) {
            $('.btnQuestionNumberBar').append($('<div>', {
                value: value,
                text: text,
                'class': 'btnQuestionNumber'
            }));
        }

        function scrollToLeft() {
            var currentScroll = $('.QuestionNumberBar').scrollLeft();
            $('.QuestionNumberBar').scrollLeft(currentScroll - 200);
        }
        function scrollToRight() {
            var currentScroll = $('.QuestionNumberBar').scrollLeft();
            $('.QuestionNumberBar').scrollLeft(currentScroll + 200);
        }

        (function ($) {
            $.fn.hasScrollBar = function () {
                return this.get(0).scrollWidth > this.width();
            }
        })(jQuery);
    </script>
    <script type="text/javascript">
        $(function () {
            $('.btnQuestionExplain').click(function () {
                if ($('.divQuestionAndAnswer').css('display') == 'block') {
                    $('.divQuestionAndAnswer').stop().hide();
                    $('.divQuestionAndAnswerExplain').stop().show();

                    // update question & answer for explain show
                    initDataToQuestionExplain();
                    initDataToAnswerExplain();
                }
                else {
                    $('.divQuestionAndAnswerExplain').stop().hide();
                    $('.divQuestionAndAnswer').stop().show();
                }
            });
        });
    </script>
    <script type="text/javascript">
        //function Qtip evalutionindex item
        $(function () {

            //$('input[type=checkbox]').live('click', function () { //evalutionItem
            //    var eiid = $(this).attr('id');
            //    var isActive = ($(this).attr('checked') == 'checked') ? true : false;
            //    var result = setQuestionEvalutionItem(eiid, isActive);
            //    if (!result) {
            //        var checked = (isActive) ? '' : 'checked';
            //        $(this).attr('checked', checked);
            //    }
            //});

            $('input[type=radio]').live('click', function () {
                var eiid = $(this).attr('id');
                var result = setQuestionDifficulty(eiid);
                if (!result) {
                    $('#' + eiid).attr('checked', '');
                } else {

                }
            });
        });

        function initialEvalutionQtip() {
            if ($('.qtip').length > 0) { $('.btnEvalutionQtip').qtip("destroy"); };
            $('.btnEvalutionQtip').qtip({
                content: getHtmlEvalutionIndexItems(),
                show: { ready: true },
                show: { event: 'mouseover' },
                style: {
                    width: 600, padding: 5, background: 'whitesmoke', color: 'black', textAlign: 'left', border: { width: 4, radius: 5, color: '#b92e7f' }, tip: 'rightMiddle', name: 'dark', 'font-size': '14px', 'line-height': '2em', height: 350
                }, hide: false,
                position: { corner: { tooltip: 'rightMiddle', target: 'leftMiddle' } },
                hide: { when: { event: 'mouseout' }, fixed: true }
            });
        }

        function getHtmlEvalutionIndexItems() {
            var functionName = "GetHtmlEvalutionIndexItems";
            var obj = "{ QuestionId : '" + question.Id + "',GroupSubjectId : '" + groupSubjectId + "',LevelId : '" + levelId + "'}";
            var result = ajaxPostToWebService(functionName, obj);
            return result;
            if (result == false) {
                //alert('save not success');
            }
        }

        function setQuestionEvalutionItem(eiid, isActive) {
            var functionName = "SetQuestionEvalutionItem";
            var obj = "{ QuestionId : '" + question.Id + "', EIId : '" + eiid + "', IsActive : '" + isActive + "'}";
            var result = ajaxPostToWebService(functionName, obj);
            return result;
        }

        function setQuestionDifficulty(eiid) {
            var functionName = "SetQuestionDifficulty";
            var obj = "{ QuestionId : '" + question.Id + "', EIId : '" + eiid + "'}";
            var result = ajaxPostToWebService(functionName, obj);
            return result;
        }
    </script>
    <script type="text/javascript">
        // Checkbox เลือกคำตอบที่ถูกต้อง
        $(function () {
            $("input:checkbox[class='rightAnswer']").live('click', function () {
                var $box = $(this);
                var $parent = $(this).parent().parent().parent().parent().parent('div');
                var isScored = true;
                if ($box.is(":checked")) {
                    var group = "input:checkbox[class='" + $box.attr("class") + "']";
                    $(group).prop("checked", false);
                    $box.prop("checked", true);
                    $('div.answerExplain').removeClass("Correct");
                    $parent.addClass("Correct");
                } else {
                    $box.prop("checked", false);
                    $('div.answerExplain').removeClass("Correct");
                    isScored = false;
                }
                var answerId = $(this).attr("id");
                saveRightAnswer(answerId, isScored);
            });
        });

        function saveRightAnswer(answerId, isScored) {
            var functionName = "SaveRightAnswer";
            var obj = "{ questionId : '" + question.Id + "', answerId : '" + answerId + "', isScored : " + isScored + "}";
            var result = ajaxPostToWebService(functionName, obj);
            if (result) {
                $('#Table1 td').removeClass("Correct");
                var tempIndex = -1;
                $.each(question.Answers, function (i, item) {
                    item.Score = 0;
                    if (item.Id == answerId) { tempIndex = i; }
                });

                if (isScored) {
                    var $divAnswer = $('div[value=' + answerId + ']');
                    $divAnswer.parent().addClass("Correct");
                    $divAnswer.parent().prev().addClass("Correct");
                    //update score
                    question.Answers[tempIndex].Score = 1;
                }
            }
        }
    </script>
</asp:Content>
