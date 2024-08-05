<<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="PreviewAndEditExam.aspx.vb" Inherits="QuickTest.PreviewAndEditExam" ValidateRequest="false" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script src="<%=ResolveUrl("~")%>js/jquery-1.7.1.min.js"></script>
        <script src="<%=ResolveUrl("~")%>js/ckEditor/ckeditor.js"></script>
        <%--<script src="<%=ResolveUrl("~")%>js/advanced.js"></script>--%>
        <%--<script src="<%=ResolveUrl("~")%>js/wysihtml5-0.3.0.js"></script>--%>
        <%--<script src="<%=ResolveUrl("~")%>js/wysihtml5-0.3.0_rc2.js"></script>--%>
        <link href="<%=ResolveUrl("~")%>css/menuFixReviewAns.css" rel="stylesheet" />
    </telerik:RadCodeBlock>
    <style type="text/css">
        .divQuestionAndAnswer, .divQuestionAndAnswerExplain {
            position: relative;
            margin-left: auto;
            margin-right: auto;
            width: 610px;
            top: 40px;
        }

            .divQuestionAndAnswer td, .divQuestionAndAnswer td span.drag {
                font-size: 24px;fsave
            }

                .divQuestionAndAnswer td span.drag {
                    cursor: pointer;
                }

        .Question {
            font-size: 30px;
            position: relative;
            -webkit-border-radius: 10px 10px 0px 0px;
            background-color: #ffc76f;
            height: auto;
            font-weight: bold;
            width: 650px;
            border: 20px;
            padding: 20px;
            margin-left: -40px;
            border-radius: 10px 10px 0px 0px;
            behavior: url('../css/PIE.htc');
            -pie-track-active: false;
        }

        div#mainAnswer, div#mainAnswerExplain {
            width: 650px;
            position: relative;
            background-color: #F4F7FF;
            padding: 20px;
            -webkit-border-radius: 0px 0px 10px 10px;
            margin-left: -40px;
            border-radius: 0px 0px 10px 10px;
            behavior: url('../css/PIE.htc');
            -pie-track-active: false;
        }

        .Answer {
            width: 283px;
            position: relative;
            float: left;
            height: 80px;
            font-size: 20px;
            border-right: solid 1px #AFAFAF;
            border-left: solid 1px #AFAFAF;
            border-bottom: solid 1px #AFAFAF;
            padding: 20px;
            background-color: #F4F7FF;
        }

        body {
            background: url(../Images/Activity/bg5.png) no-repeat center center fixed;
            height: 100%;
            background-repeat: no-repeat;
            background-size: cover;
            font: normal 0.95em 'THSarabunNew';
            color: #444;
        }

        table {
            display: table;
            border-collapse: separate;
            border-spacing: 2px;
            border-color: gray;
        }

        div.Question p:first-child {
            display: inline;
        }

        #mainAnswer td p:first-child {
            display: inline;
        }

        #DivBtn {
            width: 650px;
            margin-left: auto;
            margin-right: auto;
            margin-top: 60px;
            text-align: center;
        }

            #DivBtn input[type=button] {
                font-size: 20px;
                width: 100px;
                height: 50px;
            }

        li:before {
            content: "ลำดับที่ " counter(li) " ";
            counter-increment: li;
            color: orange;
        }

        li {
            background-color: white;
            margin: 0 5px 5px 5px;
            padding: 5px;
            width: 613px;
            text-align: left;
            padding-left: 20px;
            line-height: 40px;
            -webkit-border-radius: 5px;
            cursor: pointer;
            border: 1px solid gray;
        }

            li div {
                display: inline;
            }

        ul {
            counter-reset: li;
            margin-top: 0px;
            list-style: none;
            margin-left: -40px;
        }
    </style>

    <style type="text/css">
        #imgSwapMode {
            cursor: pointer;
            position: fixed;
            top: 33px;
            width: 50px;
            height: 50px;
        }

        #AnswerExp .ImgCircle {
            top: 30px;
            left: -5px;
        }

        #AnswerExp > div {
            padding: 10px 0;
            border-bottom: 1px solid grey;
            position: relative;
        }

            #AnswerExp > div > div {
                padding: 10px;
                border-radius: 5px;
                font-size: 24px;
            }

                #AnswerExp > div > div > div {
                    border: 1px dashed;
                    border-width: 2px;
                    border-radius: 5px;
                    margin-left: 25px;
                    margin-right: 10px;
                    padding: 10px;
                }

            #AnswerExp > div div.Correct {
                background-color: #2CA505;
                color: white;
            }

                #AnswerExp > div div.Correct div, ul > li > div.Correct {
                    background-color: #77BD60;
                }

            #AnswerExp > div div.InCorrect {
                background-color: #FF0B00;
                color: white;
            }

                #AnswerExp > div div.InCorrect div, ul > li > div.InCorrect {
                    background-color: #FF7A74;
                }

            #AnswerExp > div div.NotAnswered {
                padding: 0 10px;
            }

                #AnswerExp > div div.NotAnswered div {
                    background-color: rgb(192, 226, 255);
                }

        #btnAnswerExp, #btnQuestionExp {
            width: 75px;
            height: 75px;
            display: none;
            position: absolute;
            right: 10px;
            top: 10px;
            z-index: 9;
            background-image: url('../images/activity/btnExplain-85.png');
        }

        div#QuestionExp {
            border: 1px dashed;
            border-width: 2px;
            background-color: #FFE3B6;
            width: 625px;
            padding: 10px;
            word-wrap: break-word;
            border-radius: 5px;
            display: block;
        }

        ul > li > div.Correct, ul > li > div.InCorrect {
            border: 1px dashed;
            padding: 10px;
            border-width: 2px;
            border-radius: 5px;
            margin: 10px 10px 10px 0;
            display: none;
        }
    </style>

</head>
<body>
    <form id="form1" runat="server">

        <img id="imgSwapMode" src="../Images/Activity/swap.png" />

        <div class="divQuestionAndAnswer" style="top: 20px; -webkit-border-radius: 0.5em; margin-left: auto; margin-right: auto; margin-bottom: 20px;">

            <div id="mainQuestion" class="Question">
                <%=CurrentQuestionNo%>.<div class="CanEdit EditQuestion" style="display: inline;" contenteditable="true"><%= CurrentQuestionName %></div>
            </div>
            <div id="qtipp" style="position: relative; width: 650px;">
            </div>

            <div id="mainAnswer" runat="server">
            </div>
        </div>

        <div class="divQuestionAndAnswerExplain" style="top: 20px; -webkit-border-radius: 0.5em; margin-left: auto; margin-right: auto; margin-bottom: 20px; display: none;">
            <div id="mainQuestionExplain" class="Question">
                <%=CurrentQuestionNo%>.<div style="display: inline;"><%= CurrentQuestionName %></div>
                <div id="QuestionExp" class="CanEdit EditQuestionExplain" contenteditable="true">
                    <%= CurrentQuestionExplain %>
                </div>
            </div>
            <div id="qtipExplain" style="position: relative; width: 650px;">
            </div>

            <div id="mainAnswerExplain">
                <div id="AnswerExp" runat="server">
                </div>
            </div>
        </div>

        <div id="DivBtn">
            <input type="button" id="btnCancel" style="margin-right: 100px;" value="ยกเลิก" />
            <input type="button" id="btnSaveType1" style="display: none;" value="ตกลง" />
            <input type="button" id="btnSaveType2" style="display: none;" value="ตกลง" />
            <input type="button" id="btnSaveType3" style="display: none;" value="ตกลง" />
            <input type="button" id="btnSaveType6" style="display: none;" value="ตกลง" />
        </div>


    </form>
    <script type="text/javascript">
        var ResolveURL = '<%=ResolveUrl("~")%>';
        $(function () {
            //swap Mode
            $('#imgSwapMode').click(function () {
                ReCreateCKEditorToControl();
                if ($('.divQuestionAndAnswer').css('display') == 'block') {
                    $('.divQuestionAndAnswer').stop().hide();
                    $('.divQuestionAndAnswerExplain').stop().show();
                }
                else {
                    $('.divQuestionAndAnswerExplain').stop().hide();
                    $('.divQuestionAndAnswer').stop().show();
                }
            });

            //when click cancel button
            $('#btnCancel').click(function () {
                var JVQuestionId = '<%= CurrentQuestionId%>';
                $.ajax({
                    type: "POST",
                    url: "<%=ResolveUrl("~")%>TestSet/editeachquestion.aspx/CancelEdit",
                    data: "{ QuestionId:'" + JVQuestionId + "' }",
                    contentType: "application/json; charset=utf-8", dataType: "json",
                    success: function (msg) {
                        if (msg.d == 'Complete') {
                            window.opener = null;
                            window.close();
                        }
                    },
                    error: function myfunction(request, status) {
                        alert('บางอย่างผิดพลาดโปรดลองอีกครั้ง')
                    }
                });
            });

            var JVQsetType = '<%= CurrentQsetType%>';
            var JVQuestionId = '<%= CurrentQuestionId%>';

            if (JVQsetType == '1') {
                $('#btnSaveType1').show();
            }
            else if (JVQsetType == '2') {
                $('#btnSaveType2').show();
            }
            else if (JVQsetType == '3') {
                $('#btnSaveType3').show();
            }
            else if (JVQsetType == '6') {
                $('#btnSaveType6').show();
            }

            // Turn off automatic editor creation first.
            CKEDITOR.disableAutoInline = true;
            InjectCKEditorToControl();

            //save Type1
            $('#btnSaveType1').click(function () {
                var questionSet = '', answerSet = '', answerExplainSet = '';
                questionSet = GetStrQuestionAndQuestionExplainSet(JVQuestionId);
                if (questionSet == '') {
                    alert('คำถามและคำอธิบายคำถามต้องไม่เป็นค่าว่าง');
                    return;
                }
                answerSet = GetStrAnswerAndAnswerExplainSet('EditAnswer', false);
                if (answerSet == '') {
                    alert('คำตอบต้องไม่เป็นค่าว่าง');
                    return;
                }
                answerExplainSet = GetStrAnswerAndAnswerExplainSet('EditAnswerExplain', true);
                if (answerExplainSet == '') {
                    alert('คำอธิบายคำตอบ ต้องไม่เป็นค่าว่าง');
                    return;
                }
            
                PostToSaveAfterEditType1And2(questionSet, answerSet, answerExplainSet);
            });

            //save Type2
            $('#btnSaveType2').click(function () {
                var questionSet = '', answerExplainSet = '';
                questionSet = GetStrQuestionAndQuestionExplainSet(JVQuestionId);
                if (questionSet == '') {
                    alert('คำถามและคำอธิบายคำถามต้องไม่เป็นค่าว่าง');
                    return;
                }
                answerExplainSet = GetStrAnswerAndAnswerExplainSet('EditAnswerExplain', true);
                if (answerExplainSet == '') {
                    alert('คำอธิบายคำตอบ ต้องไม่เป็นค่าว่าง');
                    return;
                }
                PostToSaveAfterEditType1And2(questionSet, '', answerExplainSet);
            });

            //save Type3
            $('#btnSaveType3').click(function () {
                var qsetName = '', questionSet = '', answerSet = '', answerExplainSet = '';
                //qsetName
                qsetName = GetStrQuestionAndQuestionExplainSet(JVQuestionId); //$('.EditQuestion').html().replace(/\u200B/g, '');
                //validate qsetName(question for type3)
                if (qsetName == '') {
                    alert('คำถาม/คำอธิบายคำถาม ต้องไม่เป็นค่าว่าง');
                    return;
                }
                ////quesitonSet
                questionSet = GetStrAnswerAndAnswerExplainSet('EditQuestionType3', false);
                if (questionSet == '') {
                    alert('ฝั่งคำถามต้องไม่เป็นค่าว่าง(ฝั่งซ้าย)');
                    return;
                }
                //answerSet
                answerSet = GetStrAnswerAndAnswerExplainSet('EditAnswer', false);
                if (answerSet == '') {
                    alert('ฝั่งคำตอบต้องไม่เป็นค่าว่าง(ฝั่งขวา)');
                    return;
                }
                //answerExplainSet
                answerExplainSet = GetStrAnswerAndAnswerExplainSet('EditAnswerExplain', true);
                PostToSaveAfterEditType3(qsetName, questionSet, answerSet, answerExplainSet);
            });
            
            //save Type6
            $('#btnSaveType6').click(function () {
                var qsetName = '', questionSet = '', answerExplainSet;
                //qsetName
                qsetName = GetStrQuestionAndQuestionExplainSet(JVQuestionId);
                if (qsetName == '') {
                    alert('คำถามต้องไม่เป็นค่าว่าง');
                    return;
                }
                //quesitonSet
                questionSet = GetStrAnswerAndAnswerExplainSet('EditQuestionType6', false);
                if (questionSet == '') {
                    alert('คำตอบต้องไม่เป็นค่าว่าง');
                    return;
                }
                answerExplainSet = GetStrAnswerAndAnswerExplainSet('EditAnswerExplainType6', true)
                PostToSaveAfterEditType6(qsetName, questionSet, answerExplainSet);
            });

        });

        function InjectCKEditorToControl() {
            $('.CanEdit').each(function () {
                CKEDITOR.inline(this, {
                    enterMode: Number(2),
                    toolbar: [
                        { name: 'fontstyles', items: ['Bold', 'Italic', 'Underline', 'Strike', 'Subscript', 'Superscript', 'RemoveFormat'] },
                        { name: 'clipboard', items: ['Cut', 'Copy', 'Paste', 'Undo', 'Redo'] },
                        { name: 'paragraph', items: ['JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock'] },
                        '/',
                        { name: 'paragraphstyles', items: ['Styles', 'Format', 'Font', 'FontSize', 'TextColor', 'BGColor', 'Image'] }
                    ]
                });
            });
        }

        function ReCreateCKEditorToControl() {
            $('.CanEdit').each(function () {
                if ($(this).attr('contenteditable') == 'false') {
                    var name;
                    for (name in CKEDITOR.instances) {
                        var instance = CKEDITOR.instances[name];
                        if (this && this == instance.element.$) {
                            instance.destroy();
                            $(this).attr('contenteditable', true);
                            CKEDITOR.inline(this, {
                                enterMode: Number(2),
                                toolbar: [
                                    { name: 'fontstyles', items: ['Bold', 'Italic', 'Underline', 'Strike', 'Subscript', 'Superscript', 'RemoveFormat'] },
                                    { name: 'clipboard', items: ['Cut', 'Copy', 'Paste', 'Undo', 'Redo'] },
                                    { name: 'paragraph', items: ['JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock'] },
                                    '/',
                                    { name: 'paragraphstyles', items: ['Styles', 'Format', 'Font', 'FontSize', 'TextColor', 'BGColor', 'Image'] }
                                ]
                            });
                        }
                    }
                }
            });
        }

        function CheckValidate(InputValidateVariableName, InputValidateVariableExplain) {
            if (InputValidateVariableName.trim() == '' || InputValidateVariableExplain.trim() == '') {
                return false;
            }
            return true;
        }

        function LoopCreateQuestionAndAnswerStr(ClassName) {
            var currentAQNameAndExplain = '', AQSet = '', attrName = '';
            var flag = true;
            //เช็คก่อนว่าเป็น answer หรือ question
            if (ClassName.indexOf('Question') > 0) {
                attrName = 'questionid';
            }
            else {
                attrName = 'answerid';
            }
            $('.' + ClassName).each(function () {
                currentAQNameAndExplain = $(this).html().replace(/\u200B/g, '');
                if (CheckValidate(currentAQNameAndExplain, 'dummy') == false) {
                    flag = false;
                }
                AQSet += $(this).attr(attrName) + '|' + currentAQNameAndExplain + '@~@';
            })
            if (flag == false) {
                return false;
            }
            else {
                return AQSet;
            }
        }

        function GetStrAnswerAndAnswerExplainSet(ClassName, IsExplain) {
            var questionAndAnswerSet = '';
            questionAndAnswerSet = LoopCreateQuestionAndAnswerStr(ClassName);
            if (IsExplain == false) {
                if (questionAndAnswerSet == false) {
                    return '';
                }
            }
            if (questionAndAnswerSet == false) {
                return '';
            }
            if (questionAndAnswerSet.substr(-3) == '@~@') {
                questionAndAnswerSet = questionAndAnswerSet.substr(0, questionAndAnswerSet.length - 3);
            }
            return questionAndAnswerSet;
        }

        function GetStrQuestionAndQuestionExplainSet(QuestionId) {
            var currentQuestionName = $('.EditQuestion').html().replace(/\u200B/g, '');
            var currentQuestionExplain = $('.EditQuestionExplain').html().replace(/\u200B/g, '');
            //currentQuestionExplain = currentQuestionExplain.replace("'", "&#39;");
            //validate question and questionexplain
            if (CheckValidate(currentQuestionName, currentQuestionExplain) == false) {
                return '';
            }
           
            var questionSet = QuestionId + "|" + currentQuestionName + '|' + currentQuestionExplain;
         
            return questionSet
        }

        function PostToSaveAfterEditType1And2(QuestionSet, AnswerSet, AnswerExplainSet) {
           
             $.ajax({
                type: "POST",
                async: false,
                url: ResolveURL + "Testset/PreviewAndEditExam.aspx/SaveAfterEdit",
                data: "{  QuestionSet: '" + escape(QuestionSet) + "', AnswerSet: '" + escape(AnswerSet) + "',AnswerExplainSet: '" + escape(AnswerExplainSet) + "' }",
                contentType: "application/json; charset=utf-8", dataType: "json",
                success: function (data) {
                    if (data.d == 'complete') {
                        window.opener = null;
                        window.close();
                    }
                },
                error: function myfunction(request, status) {
                    alert('ส่งข้อมูลไป CodeBehind ไม่ได้');
                }
            });
        }

        function PostToSaveAfterEditType3(QsetName, QuestionSet, AnswerSet, AnswerExplainSet) {
            $.ajax({
                type: "POST",
                url: ResolveURL + "Testset/PreviewAndEditExam.aspx/SaveAfterEditType3",
                data: "{QsetName:'" + escape(QsetName) + "',QuestionSet: '" + escape(QuestionSet) + "', AnswerSet: '" + escape(AnswerSet) + "',AnswerExplainSet: '" + escape(AnswerExplainSet) + "'}",
                contentType: "application/json; charset=utf-8", dataType: "json",
                success: function (data) {
                    if (data.d == 'complete') {
                        window.opener = null;
                        window.close();
                    }
                },
                error: function myfunction(request, status) {
                    alert('ส่งข้อมูลไป CodeBehind ไม่ได้');
                }
            });
        }

        function PostToSaveAfterEditType6(QsetName, QuestionSet, AnswerExplainSet) {
            $.ajax({
                type: "POST",
                url: ResolveURL + "Testset/PreviewAndEditExam.aspx/SaveAfterEditType6",
                data: "{QsetName:'" + escape(QsetName) + "',QuestionSet: '" + escape(QuestionSet) + "',AnswerExplainSet:'" + escape(AnswerExplainSet) + "'}",
                contentType: "application/json; charset=utf-8", dataType: "json",
                success: function (data) {
                    if (data.d == 'complete') {
                        window.opener = null;
                        window.close();
                    }
                },
                error: function myfunction(request, status) {
                    alert('ส่งข้อมูลไป CodeBehind ไม่ได้');
                }
            });
        }

    </script>
</body>

</html>
