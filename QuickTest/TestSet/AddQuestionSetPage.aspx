<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="AddQuestionSetPage.aspx.vb" Inherits="QuickTest.AddQuestionSetPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../css/AddQuestionStyle.css" rel="stylesheet" />
    <style type="text/css">
        #selectQcatName {
            width: 255px;
            font: 100% 'THSarabunNew';
            border: 1px solid #C6E7F0;
            background: #EFF8FB;
            color: #47433F;
            border-radius: 7px;
        }

            #selectQcatName:disabled ,#txtQCategory:disabled{
                background: #d6d6d6;
                color: #9e9e9e;
                cursor: no-drop!important;
            }

        #rdAddQcat {
            margin-top: 20px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadStyleContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <div id="main" style="background-color: white; padding: 10px 0;">
        <h3>เพิ่มหน่วยข้อสอบ</h3>
        <div class="form">
            <span style="font-weight: bold; font-size: 18px;">ใส่ชื่อหน่วย เลือกประเภทข้อสอบและใส่ประเภทของคำสั่ง</span>
            <hr />
            <div class="form2">

                <table>
                    <tr>
                        <td colspan="2" style="text-align: center; font-size: 20px;"><b><span><%=Book.ToString() %></span></b><br />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span>ชื่อหน่วยการเรียนรู้</span>
                        </td>
                        <td>
                            <input type="radio" name="qCategory" id="rdExistQcat" /><label for="rdExistQcat">เลือกที่มีอยู่แล้ว</label>
                            <select id="selectQcatName" style="width: 255px;">
                            </select>
                            <br />
                            <input type="radio" name="qCategory" id="rdAddQcat" checked="checked" /><label for="rdAddQcat">เพิ่มใหม่</label>
                            <input type="text" id="txtQCategory" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span>ประเภทข้อสอบ</span>
                        </td>
                        <td>
                            <input type="radio" name="qsettype" value="1" id="rdChioce" checked="checked" /><label for="rdChioce">ปรนัย</label>
                            <input type="radio" name="qsettype" value="9" id="rdExplain" /><label for="rdExplain">อัตนัย</label>
                            <input type="radio" name="qsettype" value="2" id="rdTrueFalse" /><label for="rdTrueFalse">ถูกผิด</label>
                            <input type="radio" name="qsettype" value="3" id="rdPair" /><label for="rdPair">จับคู่</label>
                            <input type="radio" name="qsettype" value="6" id="rdSort" /><label for="rdSort">เรียงลำดับ</label></td>
                    </tr>
                    <tr>
                        <td>
                            <span>ประเภทคำสั่ง</span>
                        </td>
                        <td>
                            <textarea id="txtQset" name="txtQset" cols="80" rows="2"></textarea>
                        </td>
                    </tr>
                </table>
            </div>
            <div style="position: relative; height: 30px; margin-top: 10px;">
                <input type="button" id="btnBack" value="กลับ" class="btn" style="position: absolute; left: 0;" />
                <input type="button" id="btnNext" value="ไปต่อ" class="btn" style="position: absolute; right: 0;" />
            </div>
        </div>
    </div>
    <div id="dialog"></div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ExtraScript" runat="server">
    <%--<script src="../js/jquery-1.7.1.min.js" type="text/javascript"></script>--%>
    <script type="text/javascript">

        var bookId = '<%=Book.Id%>';
        var groupSubjectId = '<%=Book.GroupSubjectId%>';
        var levelId = '<%=Book.LevelId%>';

        var tempCategory = '<%=QuestionCategoryJSONStr%>';
        var questionCategoryUser = (tempCategory == "") ? null : JSON.parse('<%=QuestionCategoryJSONStr%>');
        console.log(questionCategoryUser);
        $(function () {
            if (questionCategoryUser == null) {
                $('#rdExistQcat').hide();
                $('#rdExistQcat').next().hide();
                $('#rdExistQcat').next().next().hide();
                $('#rdExistQcat').next().next().next('br').remove();
            } else {
                if (questionCategoryUser.length > 0) {
                    $('#rdExistQcat').attr("checked", true);
                    $('#txtQCategory').attr('disabled', true);
                } else {
                    $('#rdExistQcat').hide();
                    $('#rdExistQcat').next().hide();
                    $('#rdExistQcat').next().next().hide();
                    $('#rdExistQcat').next().next().next('br').remove();
                    //hide radio 
                    $('#rdAddQcat').attr('checked', true);
                    $('#rdAddQcat').hide();
                }

                $.each(questionCategoryUser, function (i, item) {
                    $('#selectQcatName').append($('<option>', {
                        value: item.Id,
                        text: item.Name
                    }));
                });
            }

            //$('#selectQcatName').change(function () {
            //    alert($(this).find(":selected").attr("value"));
            //    alert($(this).find(":selected").text());
            //});

            $('input[name="qCategory"]').change(function () {
                if ($(this).is($('#rdExistQcat'))) {
                    $('#selectQcatName').attr('disabled', false);
                    $('#txtQCategory').attr('disabled', 'disabled');
                } else {
                    $('#selectQcatName').attr('disabled', 'disabled');
                    $('#txtQCategory').attr('disabled', false);
                }
            });

            $('#btnBack').click(function () {
                window.location = "<%=ResolveUrl("~")%>testset/newstep3.aspx";
            });

            $('#btnNext').click(function () {
                console.log($('input[name="qsettype"]:checked').attr('value'));
                if (true) { // add new qset
                    var questionCategoryObj;
                    var qCategoryId = "";
                    var qCategoryName = "";
                    var ele = $('input[name="qCategory"]:checked');
                    if ($(ele).is($('#rdExistQcat'))) {
                        qCategoryId = $('#selectQcatName').find(":selected").attr("value");// get value from ddl
                        qCategoryName = $('#selectQcatName').find(":selected").text();
                        questionCategoryObj = { Id: qCategoryId, Name: qCategoryName, BookId: bookId };
                    } else {
                        qCategoryName = $('#txtQCategory').val();
                        questionCategoryObj = { Id: '', Name: qCategoryName, BookId: bookId };
                        if (questionCategoryObj.Name == '') {
                            callDialog('ใส่ชื่อหน่วยการเรียนรู้ก่อนค่ะ!');
                            return 0;
                        }                        
                    }

                    var qSetType = $('input[name="qsettype"]:checked').attr('value');
                    var qSetName = $('#txtQset').val();

                    var questionSetObj = { Id: '', Name: qSetName, Type: qSetType };
                    if (questionSetObj.Name == '') {
                        callDialog('ใส่ประเภทคำสั่งก่อนค่ะ!');
                        return 0;
                    }
                    if (questionCategoryObj.Id == '') {
                        var val = addQuestionCategory(questionCategoryObj);
                        if (val == null) { console.log("save qcate ไม่ได้หรือว่าใส่ชื่อที่มีอยู่แล้ว"); return 0; }
                        questionCategoryObj = val;
                    }
                    questionSetObj = addQuestionSet(questionCategoryObj.Id, questionSetObj);
                    if (questionSetObj == null) { console.log("ไม่สามารถ save ชุดได้"); return 0; }
                    window.location = "<%=ResolveUrl("~")%>testset/AddQuestionPage.aspx?qsetId=" + questionSetObj.Id;
                } else {
                    //update categoryandquestionset
                    editQuestionCategoryAndQuestionSet(obj);
                }
                //window.location = "<%=ResolveUrl("~")%>testset/AddQuestionPage.aspx";
            });
        });

        function ajaxPostToWebService(functionName, obj) {
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

        function addQuestionCategory(questionCategoryObj) {
            var functionName = "AddQuestionCategory";
            var obj = "{ objStr : '" + JSON.stringify(questionCategoryObj) + "'}"
            return ajaxPostToWebService(functionName, obj);
        }
        function addQuestionSet(questionCategoryId, questionSetObj) {
            var functionName = "AddQuestionSet";
            var obj = "{questionCategoryId: '" + questionCategoryId + "', objStr : '" + JSON.stringify(questionSetObj) + "'}"
            return ajaxPostToWebService(functionName, obj);
        }
        function editQuestionSet(questionSetObj) {

        }

        function callDialog(txt) {
            var $d = $('#dialog');
            var myBtn = {};
            myBtn["ตกลง"] = function () {
                $d.dialog('close');
            };
            $d.dialog({ buttons: myBtn, draggable: false, resizable: false, modal: true }).dialog('option', 'title', txt);
        }
        //function addQuestionCategoryAndQuestionSet(obj) {
        //    $.ajax({
        //        type: "POST",
        //        url: "../WebServices/QuestionService.asmx/AddQuestionCategoryAndQuestionSet",
        //        data: "{ obj : '" + JSON.stringify(obj) + "'}",
        //        async: false,
        //        contentType: "application/json; charset=utf-8", dataType: "json",
        //        success: function (data) {
        //            if (data.d == true) {
        //            }
        //        },
        //        error: function (xhr, status, text) {
        //            alert(status);
        //        }
        //    });
        //}

        //function editQuestionCategoryAndQuestionSet(obj) {
        //    $.ajax({
        //        type: "POST",
        //        url: "../WebServices/QuestionService.asmx/EditQuestionCategoryAndQuestionSet",
        //        data: "{ obj : '" + JSON.stringify(obj) + "'}",
        //        async: false,
        //        contentType: "application/json; charset=utf-8", dataType: "json",
        //        success: function (data) {
        //            if (data.d == true) {
        //            }
        //        },
        //        error: function (xhr, status, text) {
        //            alert(status);
        //        }
        //    });
        //}
    </script>
</asp:Content>
