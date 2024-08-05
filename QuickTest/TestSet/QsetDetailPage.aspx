<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="QsetDetailPage.aspx.vb" Inherits="QuickTest.QsetDetailPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        @import url(../fonts/thsarabunnew.css);

        body {
            font: normal 100% 'THSarabunNew';
            color: #444;
            background-color: antiquewhite;
        }

        .main {
            width: 800px;
            margin: auto;
        }

        .divQsetHeader {
            position: relative;
            padding-bottom: 25px;
        }

            .divQsetHeader h1 {
                margin: 0;
            }

            .divQsetHeader #lblQsetName {
                font-size: 22px;
            }

            .divQsetHeader #lblQuestionAmount {
                position: absolute;
                right: 0;
                top: 20px;
                font-size: 20px;
                font-weight: bold;
            }

            .divQsetHeader img {
                position: absolute;
                width: 30px;
                right: 10px;
                bottom: 5px;
                cursor: pointer;
            }

        .divAllQuestion {
            border: 1px solid;
            border-radius: 5px;
            padding-bottom: 10px;
        }

            .divAllQuestion ul {
                padding: 0;
                margin: 0;
            }

            .divAllQuestion li {
                list-style-type: none;
            }

            .divAllQuestion .divQuestion {
                width: 90%;
                height: auto;
                margin: auto;
                position: relative;
            }

        .divQuestion .divQuestionName {
            border: 1px solid;
            margin: auto;
            margin-top: 10px;
            padding: 5px;
            background-color: whitesmoke;
        }

        .divQuestion .divAnswer {
            width: 95%;
            border: 1px solid;
            margin: auto;
            margin-top: 5px;
            padding: 5px;
            display: none;
        }

            .divQuestion .divAnswer div {
                display: inline-block;
                padding: 5px;
            }

                .divQuestion .divAnswer div.correct {
                    background-color: rgb(44, 165, 5);
                    color: white;
                }

        /*.ui-sortable-helper { height: 1.5em; line-height: 50em;background-color:rebeccapurple; width:90px;}*/
        .ui-state-highlight {
            height: 1.5em;
            line-height: 50em;
            width: 90%;
            margin: auto;
            margin-top: 10px;
            border: 1px dotted;
        }
        
        
    </style>
    <link href="../css/jquery-ui-1.8.18.custom.min.css" rel="stylesheet" />
    <script src="../js/jquery-1.7.1.min.js"></script>
    <script src="../js/jquery-ui-1.8.18.min.js"></script>
    <style type="text/css">
        .btnReportQuestion {
            position: absolute;
            cursor: pointer;
        }

        .divQuestion .btnReportQuestion {
            right: 35px;
            top: 5px;
        }
    </style>
    <link href="../css/jquery.fancybox.css" rel="stylesheet" />
    <script src="../js/jquery.fancybox.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="main">
            <div class="divQsetHeader">
                <h1>
                    <asp:Label ID="lblSubjectName" runat="server"></asp:Label></h1>
                <asp:Label ID="lblQsetName" runat="server"></asp:Label>
                <asp:Label ID="lblQuestionAmount" runat="server"></asp:Label>
                <img src="../Images/table.png" alt="" onclick="editMode(this);" />
            </div>

            <div class="divAllQuestion" id="divQuestions" runat="server">
                <%--<div class="divQuestion">
                    <div class="divQuestionName"></div>
                    <div class="divAnswer"></div>
                </div>
                <div class="divQuestion">
                    <div class="divQuestionName"></div>
                    <div class="divAnswer"></div>
                </div>
                <div class="divQuestion">
                    <div class="divQuestionName"></div>
                    <div class="divAnswer"></div>
                </div>
                <div class="divQuestion">
                    <div class="divQuestionName"></div>
                    <div class="divAnswer"></div>
                </div>
                <div class="divQuestion">
                    <div class="divQuestionName"></div>
                    <div class="divAnswer"></div>
                </div>
                <div class="divQuestion">
                    <div class="divQuestionName"></div>
                    <div class="divAnswer"></div>
                </div>--%>
            </div>
        </div>
    </form>
    <script type="text/javascript">

        $(function () {
          

        });

        //function ReporterQuestion(qsetId, questionId) {
        //    $.fancybox({
        //        fitToView: true,
        //        'autoScale': true,
        //        'transitionIn': 'none',
        //        'transitionOut': 'none',
        //        'href': '../support/reporterproblemQuestion.aspx?qsetId=' + qsetId + '&questionId=' + questionId,
        //        'type': 'iframe',
        //        'width': 600,
        //        'minHeight': 400
        //    });
        //}

        var isEditMode = false;
        function editMode(e) {
            var icon;
            isEditMode = !isEditMode;
            if (isEditMode) {
                icon = '../images/freehand.png';
                // add btn for show all answer
                if ($('.divAnswer').length > 0) {
                    $('.divQsetHeader')
                    .append(
                    $('<img>', { src: '../images/user-group-icon.png', style: 'width: 30px;position: absolute;right: 50px;cursor:pointer;', 'class': 'toggleAns', onclick: 'toggleAllAnswer();' })
                );
                    // add btn for show answer
                    $('.divQuestion')
                        .append(
                        $('<img>', { src: '../images/eye.jpg', style: 'width: 40px;position: absolute;right: 5px;top: 5px;cursor:pointer;', 'class': 'toggleAns', onclick: 'toggleAnswer(this);' })
                    );
                }

                // sortable question
                $("ul").sortable({ placeholder: "ui-state-highlight", axis: "y" });
                $("ul").disableSelection();

                 <%If BusinessTablet360.ClsKNSession.EnableReportQuestion Then %> 
                // add hover
                $('.divQuestion').each(function () {
                    $(this).hover(function () {
                        var qsetId = $(this).attr('qsetId');
                        var questionId = $(this).attr('id');
                        $(this).append($('<img>', { src: '../Images/reporter.png', 'class': 'btnReportQuestion', onclick: 'ReporterQuestion("' + qsetId + '","' + questionId + '");' }));
                    }, function () {
                        $(this).find('img.btnReportQuestion').remove();
                    });
                });
                <%End If%>
            } else {
                icon = '../images/table.png';
                $('.divQsetHeader').find('.toggleAns').remove();
                $('.divQuestion').find('.toggleAns').remove();
                $('.divAnswer').hide();

                //remove hover
                $('.divQuestion').each(function () {
                    $(this).unbind('mouseenter mouseleave');
                });


                $("ul").sortable("destroy");
            }
            $(e).attr('src', icon);
        }

        function toggleAnswer(e) {
            var display = $(e).prev().css('display');
            if (display === 'none') {
                $(e).prev().show();
            } else {
                $(e).prev().hide();
            }
        }

        function toggleAllAnswer() {
            $('.divAnswer').show();
        }
    </script>
    <%If BusinessTablet360.ClsKNSession.EnableReportQuestion Then %>    
    <script src="../js/reportProblemQuestion.js" type="text/javascript"></script>
    <%End If %>
</body>
</html>
