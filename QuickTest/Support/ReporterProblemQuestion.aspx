<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ReporterProblemQuestion.aspx.vb" Inherits="QuickTest.ReporterProblemQuestion" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        @import url(../fonts/thsarabunnew.css);

        body {
            background: wheat;
            font: normal 0.95em 'THSarabunNew';
            color: #444;
        }

        textarea {
            width: 410px;
            height: 60px;
            resize: none;
            font: normal 0.95em 'THSarabunNew';
        }

        .submit {
            font: 100% 'THSarabunNew';
            width: 99px;
            height: 33px;
            padding: 2px 0 3px 0;
            cursor: pointer;
            background: #1EC9F4;
            box-shadow: 0 1px 2px rgba(0,0,0,.2);
            color: #FFF;
            border: solid 1px #0D8AA9;
            background: #46C4DD;
            background: -webkit-gradient(linear, left top, left bottom, from(#63CFDF), to(#17B2D9));
            background: -moz-linear-gradient(top, #63CFDF, #17B2D9);
            text-shadow: 1px 1px #178497;
            border-radius: 0.5em;
        }

        h1 {
            margin: 0;
        }
    </style>
    <link href="../css/jquery-ui-1.8.18.custom.min.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div style="text-align: center; margin-bottom: 10px;">
            <h1>
                <asp:Label ID="lblHeader" runat="server"></asp:Label></h1>
            <div style="text-align: left;">
                <b>
                    <asp:Label ID="lblSubjectName" runat="server"></asp:Label></b><br />
                <b>หน่วยการเรียนรู้ - </b>
                <asp:Label ID="lblQcatName" runat="server"></asp:Label><br />
                <b>คำสั่ง - </b>
                <asp:Label ID="lblQsetName" runat="server"></asp:Label>
                <%If QuestionId <> "" Then %>
                <br />
                <b>โจทย์ - </b>
                <asp:Label ID="lblQuestionName" runat="server"></asp:Label>
                <%End If %>
            </div>
            <br />
            <textarea id="txtAnnotation" placeholder="ใส่หมายเหตุ เพื่อแจ้งปัญหา" maxlength="8000"></textarea>
            <br />
            <input type="button" class="submit" value="แจ้งปัญหา" />
        </div>
    </form>
    <script src="../js/jquery-1.7.1.min.js"></script>
    <script src="../js/jquery-ui-1.8.18.min.js"></script>
    <script type="text/javascript">
        var questionId = '<%=QuestionId%>';
        $(function () {
            var isCloseFancybox;
            
            var $dialog = $('<div>');
            $dialog.dialog({
                autoOpen: false,
                resizable: false,
                draggable:false,
                modal: true,
                buttons: {
                    "ตกลง": function () {
                        $(this).dialog("close");
                        if (isCloseFancybox) parent.$.fancybox.close();
                    }
                }
            });           

            $('.submit').click(function () {                
                var annotation = $('#txtAnnotation').val();
                if (annotation == "") {
                    $dialog.dialog("option", 'title', 'ไม่สามารถแจ้งปัญหาได้ค่ะ ใส่หมายเหตุก่อนค่ะ!!').dialog("open");
                    return 0;
                }
                if (reporterProblemQuestion(annotation)) {
                    isCloseFancybox = true;
                    $dialog.dialog("option", 'title', 'แจ้งปัญหาแล้วค่ะ').dialog("open");                    
                } else {
                    isCloseFancybox = false;
                    $dialog.dialog("option", 'title', 'ไม่สามารถแจ้งปัญหาได้ค่ะ ลองใหม่นะคะ').dialog("open");
                }
            });
        });

        function reporterProblemQuestion(annotation) {
            var returnValue;
            $.ajax({
                type: "POST",
                url: "<%=ResolveUrl("~")%>WebServices/QuestionService.asmx/ReporterProblemQuestion", 
                data: "{  questionId: '" + questionId + "', annotation: '" + annotation + "' }",
                contentType: "application/json; charset=utf-8", dataType: "json",
                async: false,
                success: function (data) {
                    returnValue = data.d;
                },
                error: function myfunction(request, status) {
                    //alert('ส่งข้อมูลไป CodeBehind ไม่ได้');
                }
            });
            return returnValue;
        }
    </script>
</body>
</html>
