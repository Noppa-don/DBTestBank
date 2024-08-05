<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="MoveEngExam.aspx.vb" Inherits="QuickTest.MoveEngExam" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <script src="<%=ResolveUrl("~")%>js/jquery-1.7.1.min.js"></script>
    <script src="<%=ResolveUrl("~")%>js/LayoutConfirmed.js"></script>
    <title></title>
    <script type="text/javascript">
        var ResolveURL;
        $(function () {
            ResolveURL = '<%=ResolveUrl("~")%>';

            GetAllQuestionSelected();

            $('.LevelType').change(function () {

                var QuestionNo = $(this).attr('id');
                BindQCat('', QuestionNo);
            });

            $('.SelectedQCat').change(function () {
                var QuestionNo = $(this).attr('QNo');
                var QCatId = $('#' + QuestionNo + 'SelectedQCat').val();
                if (QCatId != '0' && QCatId != 'undefined') {
                    BindQSet('', QuestionNo, QCatId);
                }
                
            });

            $('.SelectedQset').change(function () {

                var QuestionNo = $(this).attr('QNo');
                var QId = $(this).attr('qId');
                var QSetId = $('#' + QuestionNo + 'SelectedQset').val();

                $.ajax({
                    type: "POST",
                    url: "<%=ResolveUrl("~")%>testset/MoveEngExam.aspx/UpdateQset",
                    data: "{QSetId : '" + QSetId + "',QuestionId : '" + QId + "'}",
                    contentType: "application/json; charset=utf-8", dataType: "json",
                    success: function (msg) {
                    }, error: function myfunction(request, status) {
                    }
                });
            });
        });

        function GetAllQuestionSelected() {
            $('.LevelType').each(function (i, obj) {

                $(this).empty();

                var _select = $('<select>');
                _select.append($('<option></option>').val("0").html("เลือกชุดข้อสอบ"));
                _select.append($('<option></option>').val("E5DBFA06-C4CE-4CE2-9F47-60E9CB99A38C").html("Foundation"));
                _select.append($('<option></option>').val("DB95E7F8-7BF3-468D-AD9E-0AAF1B328D45").html("Pre-intermediate"));
                _select.append($('<option></option>').val("14A28F3D-1AFF-429D-B7A1-927A28E010BD").html("Intermediate"));
                _select.append($('<option></option>').val("2E0FFC04-BCEE-45BE-9C0C-B40742523F43").html("Upper-Intermediat"));
                _select.append($('<option></option>').val("6736D029-6B78-4570-9DBB-991217DA8FEE").html("Advance"));

                $(this).append(_select.html());

                var QId = $(this).attr('qId');
                var QNo = $(this).attr('id');

                $.ajax({
                    type: "POST",
                    url: "<%=ResolveUrl("~")%>testset/MoveEngExam.aspx/CheckSeletedLevel",
                    data: "{QuestionId : '" + QId + "'}",
                    contentType: "application/json; charset=utf-8", dataType: "json",
                    success: function (msg) {
                        var sLevel = msg.d;
                        if (sLevel != '0') {
                            $('#' + QNo + ' option[value=' + sLevel + ']').attr('selected', 'selected');
                            BindQCat(QId, QNo);
                        }
                    }, error: function myfunction(request, status) {
                    }
                });
            });
        }

        function BindQCat(QId, QNo) {
            var levelId = $("#" + QNo).val();

            $('#' + QNo + 'SelectedQCat').empty();

            $.ajax({
                type: "POST",
                url: "<%=ResolveUrl("~")%>testset/MoveEngExam.aspx/GetQCat",
                data: "{LevelId : '" + levelId + "',QuestionId : '" + QId + "'}",
                contentType: "application/json; charset=utf-8", dataType: "json",
                success: function (msg) {
                    var QcatAll = jQuery.parseJSON(msg.d);
                    var _select = $('<select>');
                    var selectedId;
                    selectedId = '';
                    _select.append($('<option></option>').val("0").html("เลือกบทเรียน"));

                    $.each(QcatAll, function (val, text) {

                        _select.append($('<option></option>').val(text.val).html(text.text));
                        if (text.selected != '') {
                            selectedId = text.val;
                        }
                    });

                    $('#' + QNo + 'SelectedQCat').append(_select.html());
   
                    if (selectedId != '') {
                        $('#' + QNo + 'SelectedQCat option[value=' + selectedId + ']').attr('selected', 'selected');
                        BindQSet(QId, QNo, selectedId)
                    } else {
                        var select2 = $('<select>');
                        select2.append($('<option></option>').val("0").html("เลือกชุดข้อสอบ"));
                        $('#' + QNo + 'SelectedQset').append(select2.html());
                    }

                },
                error: function myfunction(request, status) {
                }
            });
        }

        function BindQSet(QId, QNo, QCatId) {

            $('#' + QNo + 'SelectedQset').empty();

            $.ajax({
                type: "POST",
                url: "<%=ResolveUrl("~")%>testset/MoveEngExam.aspx/GetQSet",
                    data: "{QCatId : '" + QCatId + "',QuestionId : '" + QId + "'}",
                    contentType: "application/json; charset=utf-8", dataType: "json",
                    success: function (msg) {
                        var QSetAll = jQuery.parseJSON(msg.d);
                        var _select = $('<select>');
                        var selectedId;
                        _select.append($('<option></option>').val("0").html("เลือกชุดข้อสอบ"));

                        $.each(QSetAll, function (val, text) {
                            _select.append($('<option></option>').val(text.val).html(text.text));
                            if (text.selected != '') {
                                selectedId = text.val;
                            }
                        });

                        $('#' + QNo + 'SelectedQset').append(_select.html());

                        if (selectedId != '') {
                            $('#' + QNo + 'SelectedQset option[value=' + selectedId + ']').attr('selected', 'selected');
                        }
                    }, error: function myfunction(request, status) {
                    }
             });
            }
    </script>
    <style type="text/css">
        #MainDiv {
            width: 895px;
            height: 520px;
            margin-left: auto;
            margin-right: auto;
            padding: 10px;
            border: 1px solid;
            border-radius: 6px;
            text-align: center;
        }

        table {
            width: 810px;
            margin-left: auto;
            margin-right: auto;
            text-align: center;
            margin-top: 10px;
            padding: 5px;
            border: 1px solid;
            border-radius: 6px;
        }

        td {
            padding: 5px;
        }

        #DivContent {
            height: 465px;
            overflow: auto;
        }

        h3 {
            margin-top: 0px;
            margin-bottom: 5px;
        }

        #spnQsetName {
            font-size: 20px;
        }

        th {
            border-bottom: 1px solid;
        }

        td {
            border-right: 1px solid;
            /*border-bottom:1px solid;*/
        }

        .allowHover:hover {
            background-color: #FFDDA9;
        }

        td:last-child {
            border-right: none;
        }

        .imgPreview {
            width: 20px;
            height: 20px;
            cursor: pointer;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div id="MainDiv">
            <span id="spnQsetName" runat="server"></span>
            <div id="DivContent">
                <table>
                    <tr>
                        <th style="width: 5%">ข้อที่</th>
                        <th style="width: 35%">ตัวอย่างคำถาม</th>
                        <th style="width: 20%">ระดับชั้น</th>
                        <th style="width: 20%">บทเรียน</th>
                        <th style="width: 20%">ชุดข้อสอบ</th>
                    </tr>
                    <asp:Repeater ID="rptListQuestion" runat="server">
                        <ItemTemplate>
                            <tr class="allowHover">
                                <td><%# Container.DataItem("Question_No").ToString() %>.</td>

                                <td style="text-align: left;"><%# clslayoutcheck.ReplaceModuleURL(Container.DataItem("Question_Name"), Qset_Id)%></td>

                                <td>
                                    <select id="<%# Container.DataItem("Question_No").ToString() %>" class="LevelType" qid="<%# Container.DataItem("Question_Id").ToString() %>" style="font-size: 16px; height: 40px;">
                                        <option value="0">เลือกระดับชั้น</option>
                                </td>
                                <td>
                                    <select class="SelectedQCat" id="<%# Container.DataItem("Question_No").ToString() %>SelectedQCat"
                                        qno="<%# Container.DataItem("Question_No").ToString() %>" style="font-size: 16px; height: 40px;" />
                                    <option value="0">เลือกบทเรียน</option>
                                </td>
                                <td>
                                    <select class="SelectedQset" id="<%# Container.DataItem("Question_No").ToString() %>SelectedQset"
                                        qno="<%# Container.DataItem("Question_No").ToString() %>" qid="<%# Container.DataItem("Question_Id").ToString() %>"
                                        style="font-size: 16px; height: 40px;">
                                        <option value="0">เลือกชุดข้อสอบ</option>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </table>
            </div>
        </div>
    </form>
</body>
</html>
