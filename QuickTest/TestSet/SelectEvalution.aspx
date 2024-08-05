<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="SelectEvalution.aspx.vb" Inherits="QuickTest.SelectEvalution" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        input[type=checkbox] + label
            {
                height: 21px;
                padding-left: 30px;
                background: url(../images/bullet.gif) center left no-repeat;
                margin-left: -18px;
            }
        input[type=checkbox]:checked + label {
                background-image: url(../images/bullet_checked.gif);
            }
        #divSelectEvalutionIndex {
            text-align: left; 
            width: 100%;
            font-family: 'THSarabunNew'; 
            color: #474748;
        }

        tr {
            background-color: #D3F2F7;
        }
    </style>

    <script type="text/javascript" src="../js/jquery-1.7.1.js"></script>
    <script type="text/javascript">

        $(function () {

            var GroupSubjectId = '<%=GroupSubjectId%>';
            var LevelId = '<%=LevelId%>';

            var getLengthChk = document.getElementsByName("test[]").length;

            $("#chkAll").click(function () {
                if ($("#chkAll").attr('checked') == 'checked') {
                    for (var i = 0; i < getLengthChk; i++) {
                        $('#divSelectEvalutionIndex input:checkbox').attr('checked', 'checked');
                    }
                } else {
                    for (var i = 0; i < getLengthChk; i++) {
                        $('#divSelectEvalutionIndex input:checkbox').removeAttr('checked');
                    }
                }

                $.ajax({
                    type: "POST",
                    url: "<%=ResolveUrl("~")%>testset/SelectEvalution.aspx/DeleteTestsetEvalutionIndex",
                    data: "{ TestsetId : '',GroupSubjectid : '" + GroupSubjectId + "',LevelId : '" + LevelId + "' }",
                    contentType: "application/json; charset=utf-8", dataType: "json",
                    success: function (msg) {
                        if (msg.d != 0) {
                        }
                    },
                    error: function myfunction(request, status) { 
                    }
                }); 

            });

        });

        function SaveTestsetEvalutionIndex(testsetId, EIId) {
            var IsCheck = ($('#' + EIId).is(":checked"));

            $.ajax({
                type: "POST",
                url: "<%=ResolveUrl("~")%>testset/SelectEvalution.aspx/SaveTestsetEvalutionIndex",
                data: "{ TestsetId : '" + testsetId + "', EIId : '" + EIId + "', IsCheck : '" + IsCheck + "' }",
                contentType: "application/json; charset=utf-8", dataType: "json",
                success: function (msg) {
                    if (msg.d != 0) {
                    }
                },
                error: function myfunction(request, status) { 
                }
            });             
        }

        function DeleteAllTestsetEvalutionIndex(testsetId) {
            $.ajax({
                type: "POST",
                url: "<%=ResolveUrl("~")%>testset/SelectEvalution.aspx/DeleteTestsetEvalutionIndex",
                data: "{ TestsetId : '" + testsetId + "',GroupSubjectid : '" + GroupSubjectId + "',LevelId : '" + LevelId + "' }",
                contentType: "application/json; charset=utf-8", dataType: "json",
                success: function (msg) {
                    if (msg.d != 0) {
                    }
                },
                error: function myfunction(request, status) { 
                }
            });             
        }
        
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <table>
            <tr>
                <td style="font-weight: bold; font-size: 23px; color: #474748; line-height: 55px; padding-left:10px;">เลือกตัวชี้วัดเพื่อกรองข้อสอบ</td>
                <td style="text-align: right; padding-right: 10px;">
                    <span style="padding-left:10px;font-size:18px;">
                        <input id="chkAll" class="ck" style="display: none;" type="checkbox" />
                        <label for="chkAll">เลือกทั้งหมด</label>
                    </span>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <div id="divSelectEvalutionIndex" runat="server"></div> 
                </td>
            </tr>
        </table>

    </form>
</body>
</html>
