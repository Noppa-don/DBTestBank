<%@ Page Title="QuickTest - ขั้นที่ 3: เลือกหน่วยการเรียนรู้" Language="vb" AutoEventWireup="false"
    MasterPageFile="~/Site.Master" CodeBehind="NewStep3.aspx.vb" Inherits="QuickTest.NewStep3" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content4" ContentPlaceHolderID="HeadStyleContent" runat="server">
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <%-- <link rel="stylesheet" type="text/css" href="<%=ResolveUrl("~")%>shadowbox/shadowbox.css" />
    <link charset="utf-8" media="screen" type="text/css" href="<%=ResolveUrl("~")%>css/aa.css"
        rel="stylesheet" />
    <link type="text/css" rel="stylesheet" href="<%=ResolveUrl("~")%>css/general.css" />  --%>
    <script src="../js/json2.js" type="text/javascript"></script>   
    <script src="../Scripts/jquery.signalR-2.0.2.min.js" type="text/javascript"></script>
    <script src='<%=ResolveClientUrl("~/signalr/hubs") %>' type="text/javascript"></script>
    <script type="text/javascript">var baseUrl = "<%= ResolveUrl("~/") %>";</script>
    <script src="../js/DashboardSignalR.js" type="text/javascript"></script>
   
    <style type="text/css">
        tbody.on {
            display: table-row-group;
        }

        tbody.off {
            display: none;
        }

        .toolsTip {
            position: absolute;
            border: 1px solid #FFCC66;
            background-color: #FFFFCC;
            color: #000000;
            display: none;
            padding: 5px;
            width: 700px;
            font-size: 16px;
            margin: 0px auto;
        }

        .TopRight {
            position: fixed;
            border: 3px solid #FFCC66;
            background-color: #FFFFCC;
            color: #000000;
            display: none;
            top: 0px;
            right: 0px;
            padding: 10px;
            padding-bottom: 5px;
            padding-top: 5px;
            margin: 3px;
            border-radius: 15px;
        }

        .ForDescription {
            position: fixed;
            border: 3px solid #FFCC66;
            background-color: #FFFFCC;
            color: #000000;
            padding: 10px;
            padding-bottom: 5px;
            padding-top: 5px;
            display: none;
            margin: 3px;
            top: 0px;
            left: 0px;
            border-radius: 15px;
        }

        .ForRadEditor {
            background-image: none !important;
            background-color: White;
        }

        .MainDivSummary {
            width: 180px;
            padding: 2px;
            display: inline-block;
            margin-left: 20px;
            border-radius: 6px;
            cursor: pointer;
        }

        .MainW {
            border: 2px solid #0EAE2F;
        }

        .MainQ {
            border: 2px solid #FF7400;
        }

        .MainW:hover {
            background-color: #FCF8E9;
        }

        .MainQ:hover {
            background-color: #FCF8E9;
        }

        .MainDivSummary div {
            font: initial;
        }

        .divLeft {
            width: 130px;
            display: inline-block;
        }

        .MainW .divLeft span {
            font-size: 12px;
            color: #0D7C24;
        }

        .MainQ .divLeft span {
            font-size: 12px;
            color: #FF5B14;
        }

        .divRight {
            display: inline-block;
        }

        .IconRight {
            width: 8px;
            height: 8px;
        }
    </style>
    <style type="text/css">
        .divClassQuestions {
            padding: 10px;
            background-color: white;
        }

            .divClassQuestions span {
                font-weight: bold;
                color: black;
            }

            .divClassQuestions .divQuestions {
                border: 1px solid #f8971c;
                border-radius: 5px;
                padding: 10px;
                background-color: whitesmoke;
            }

                .divClassQuestions .divQuestions table {
                    margin: 0 !important;
                }

                    .divClassQuestions .divQuestions table td {
                        background: initial !important;
                    }

                        .divClassQuestions .divQuestions table td input[type=text] {
                            height: 35px;
                            width: 350px;
                            border: none;
                            border: solid 1px #ccc;
                            border-radius: 10px;
                            /*เติ่มเพิ่ม*/
                            margin-left: 25px;
                            width: 600px;
                        }

                .divClassQuestions .divQuestions .addQuestion {
                    width: 90%;
                    position: relative;
                    background: #D3F2F7;
                    padding: 5px;
                    color: black;
                    height: 40px;
                    font-weight: bold;
                    border-radius: 10px;
                    text-align: center;
                    margin: auto;
                    margin-top: 5px;
                    cursor: pointer;
                }

                    .divClassQuestions .divQuestions .addQuestion h3 {
                        cursor: pointer;
                    }

        .qtipQcategory , .qtipQcategoryByUser {
            padding-left: 30px!important;
            font: inherit!important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <form id="toNextPage" runat="server">
        <div id="main" style="background-color: white; padding-bottom: 10px;">
            <input type="hidden" id="CountQtip" value="0" />
            <header class="thinheader">
      <div id="logo" class="slogantext">
        <div id="logo_text">
          <h2>ครบถ้วน ถูกต้อง ฉับไว </h2>		 		
		         </div>
      </div>
      <nav>
        <div id="menu_container">                 
           <ul class="sf-menu" id="nav" style="font-size:20px;text-align:center;">
		    <li><a href="../Testset/DashboardSetupPage.aspx"><img id="imgBack" src="../Images/Home.png" style="position: absolute; margin-left: 5px; margin-top: -8px; cursor: pointer;"></a></li> 
            <li style="margin-left:45px;"><a href="../Testset/Step2.aspx" >ขั้นที่ 1 เลือกวิชา --></a></li> 
            <li style="width:290px;"><a href="#" class="current">ขั้นที่ 2 เลือกหน่วยการเรียนรู้ --></a></li> 
            <li><a href="#" style="cursor:not-allowed;">ขั้นที่ 3 บันทึก</a></li>
          </ul>
        </div>
      </nav>
    </header>
            <div id="site_content" style="padding: 0; margin: auto;">
                <%If HttpContext.Current.Session("EditID") IsNot Nothing And HttpContext.Current.Session("EditID") <> "" Then %>
                <div style="text-align: center;">
                    <span style="color: red; font-size: 20px;"><%= EditTestSetWarningText %></span>
                </div>
                <%End If %>
                <div class="content ListingFixedHeightContainer" style="width: 930px; margin: 0 0 5px 0;">
                    <asp:Repeater ID="Repeater1" runat="server">
                        <ItemTemplate>
                            <h3 id="Subject_<%# Container.DataItem("SubjectId") %>" style="margin: 0 0 0 15px;"><%# Container.DataItem("SubjectName")%></h3>
                            <center>
                    <div id="div-1" style='text-align:left' class="ListingContent"> 
                        <asp:Repeater id="ListingClass" runat="server">
                            <ItemTemplate>
                                <div class="divClassQuestions">
                                    
                                    <div class="divQuestions">     
                                        <span>ชั้น <%# Container.DataItem("ClassName") %></span>                             
                                    <table>
                                        <tr>
                                            <td colspan="2" style="text-align:left;"><input type="text" class="qtipQcategory" subjectid="<%# Container.DataItem("SubjectId") %>" classid="<%# Container.DataItem("ClassId") %>" placeholder="เลือกหน่วยการเรียนรู้ (นำเมาส์มาลอยเพื่อเลือกค่ะ)" /></td>
                                         
                                        </tr>
                                        <tr><td colspan="2" style="padding-left:25px;"><span class="spnSelectQuestion <%# Container.DataItem("SubjectId") %><%# Container.DataItem("ClassId") %>" selectQuestionAmount="<%#GetQuestionAmount(Eval("SubjectId"), Eval("ClassId"), True) %>"><%#CreateQuestionAmount(Eval("ClassId"), Eval("SubjectId")) %></span></td></tr>
                                    </table>                                    
                                        <%#CreateContentQuestionByUser(Eval("ClassId"), Eval("SubjectId")) %>
                                    <div class="addQuestion" subjectid="<%# Container.DataItem("SubjectId") %>" classid="<%# Container.DataItem("ClassId") %>">
                        <img src="../Images/plus_circle.png" style="position:absolute;left:35%;width:40px;" alt="" />                        
                        <span>เพิ่มข้อสอบ</span>
                    </div>
                                    </div>  
                                </div>		                      
                            </ItemTemplate>
                        </asp:Repeater>
		            </div>                  
		        </center>
                        </ItemTemplate>
                    </asp:Repeater>

                    <div style="text-align: right; padding-right: 40px; margin-top: 10px;">รวมทั้งหมด <span id='spnTotalQuestionsSelected'><%= GetQuestionAmount()%></span> ข้อ</div>
                      <asp:Button ID="btnNextStep4" ClientIDMode="Static" runat="server" class="submitChangeFontSize" Text="ไปหน้าเลือกข้อสอบแบบละเอียด >>"
                Style="float:right; width: 300px; position: relative; height: 40px;"></asp:Button>
                </div>
            </div>

          
            
            <footer style="margin-top: 15px">
      สงวนลิขสิทธิ์ &copy; บริษัท สำนักพิมพ์วัฒนาพานิช จำกัด
    </footer>
        </div>
        <span class="TopRight" id="SpanFullDetail"></span>
         <div id="dialog"></div>
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
        </telerik:RadScriptManager>
    </form>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ExtraScript" runat="server">
    <script src="../js/GFB.js" type="text/javascript"></script>
    <script src="../js/jquery-ui-1.8.18.js" type="text/javascript"></script>
    <script src="../js/jquery.ui.touch-punch.min.js" type="text/javascript"></script>
    <script src="../js/facescroll.js" type="text/javascript"></script>
    <link href="../css/jquery.fancybox.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery.fancybox.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            //new FastButton(document.getElementById('btnNextStep4'), TriggerServerButton);

            $('#btnNextStep4').click(function (e) {
                var checkedAmount = $('#spnTotalQuestionsSelected').text();
                if (parseInt(checkedAmount) == 0) {
                    e.preventDefault();
                    callAlertDialog('เลือกข้อสอบก่อนค่ะ!');
                    return false;
                }
            });

            //redirect to add questions
            $('.addQuestion').click(function () {
                var subjectId = $(this).attr('subjectid');
                var levelId = $(this).attr('classid');
                window.location = "<%=ResolveUrl("~")%>testset/addQuestionsetPage.aspx?subjectid=" + subjectId + "&levelid=" + levelId;
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
    </script>
    <script type="text/javascript">
        $(function () {
            var styleQtipCategory = { width: 800, padding: 5, background: 'whitesmoke', color: 'black', textAlign: 'left', border: { width: 4, radius: 5, color: '#b92e7f' }, tip: 'leftMiddle', name: 'dark', 'font-size': '14px', 'line-height': '2em', height: 400 };
            $('input[type="text"].qtipQcategory').each(function () {
                $(this).qtip({
                    //show :{ready:true},
                    show: { event: 'mouseover' },
                    content: showQsetInBook(this, true),
                    style: styleQtipCategory,
                    hide: false,
                    position: { corner: { tooltip: 'leftMiddle', target: 'leftMiddle' } },
                    hide: { when: { event: 'mouseout' }, fixed: true }
                });
                var subjectId = $(this).attr("subjectid");
                var classId = $(this).attr("classid");
                var data = "{subjectId : '" + subjectId + "',classId : '" + classId + "'}"
                var listQset = JSON.parse(getQsetSelected(data));
                var qsetName = "";
                $.each(listQset, function (i, item) {
                    //console.log($('#' + item.qsetId).next().val());
                    //console.log($('label[for="' + item.qsetId + '"]').hide());
                    console.log($('#' + item.qsetId).attr('checked'));
                    qsetName += $('#' + item.qsetId).next().text();
                });
                $(this).val(qsetName);
            });
            $('input[type="text"].qtipQcategoryByUser').each(function () {
                $(this).qtip({
                    show: { event: 'mouseover' },
                    content: showQsetInBook(this, false),
                    style: styleQtipCategory,
                    hide: false,
                    position: { corner: { tooltip: 'leftMiddle', target: 'leftMiddle' } },
                    hide: { when: { event: 'mouseout' }, fixed: true }
                });
            });
            $('input[type="text"].qtipEvalution').each(function () {
                $(this).qtip({
                    show: { event: 'mouseover' },
                    content: showEvalutionInBook(this),
                    style: {
                        width: 650, padding: 5, background: 'whitesmoke', color: 'black', textAlign: 'left', border: { width: 4, radius: 5, color: '#b92e7f' }, tip: 'leftMiddle', name: 'dark', 'font-size': '14px', 'line-height': '2em', height: 350, 'overflow': 'auto'
                    }, hide: false,
                    position: { corner: { tooltip: 'leftMiddle', target: 'leftMiddle' } },
                    hide: { when: { event: 'mouseout' }, fixed: true }
                });
            });

            //span show choose question amount
            //$('.spnSumQuestion').each(function () {
            //    var txt = "จากทั้งหมด 500 ข้อ เลือกมาแล้ว 0 ข้อ";
            //    var subjectId = $(this).attr('subjectid');
            //    var sum = 0;
            //    $("input[value='" + subjectId + "']").each(function () {
            //        //if (parseInt($(this).attr('questionAmount')) == undefined) { console.log($(this).attr('id')); }
            //        sum = sum + parseInt($(this).attr('questionAmount'));
            //        console.log(parseInt($(this).attr('questionAmount')));
            //    });
            //    $(this).text("จากทั้งหมด " + sum + " ข้อ เลือกมาแล้ว " + sum + " ข้อ");
            //});
        });

        function getQsetSelected(data) {
            var result;
            $.ajax({
                type: "POST",
                url: "../WebServices/TestsetService.asmx/GetQSetSelected",
                data: data,
                async: false,
                contentType: "application/json; charset=utf-8", dataType: "json",
                success: function (data) {
                    result = data.d;
                },
                error: function (xhr, status, text) {
                    alert(status);
                }
            });
            return result;
        }

        function ajaxPostToServer(methodName, data) {
            var result;
            $.ajax({
                type: "POST",
                url: "../WebServices/QuestionService.asmx/" + methodName,
                data: data,
                async: false,
                contentType: "application/json; charset=utf-8", dataType: "json",
                success: function (data) {
                    result = data.d;
                },
                error: function (xhr, status, text) {
                    alert(status);
                }
            });
            return result;
        }

        function showQsetInBook(sender, isWpp) {
            var subjectId = $(sender).attr("subjectid");
            var classId = $(sender).attr("classid");
            if (subjectId == undefined) return "";
            var methodName = "GetContentQsetInBook";
            var data = "{ subjectId : '" + subjectId + "', classId : '" + classId + "',isWpp : '" + isWpp + "'}";
            return ajaxPostToServer(methodName, data);
        }

        //function getContentQsetInBook(subjectId, classId) {
        //    var result;
        //    $.ajax({
        //        type: "POST",
        //        url: "../WebServices/QuestionService.asmx/GetContentQsetInBook",
        //        data: "{ subjectId : '" + subjectId + "', classId : '" + classId + "'}",
        //        async: false,
        //        contentType: "application/json; charset=utf-8", dataType: "json",
        //        success: function (data) {
        //            result = data.d;
        //        },
        //        error: function (xhr, status, text) {
        //            alert(status);
        //        }
        //    });
        //    return result;
        //}
        function editQuestion(qsetId) {
            window.location = "../testset/addquestionpage.aspx?qsetId=" + qsetId;
        }
        
        function deleteQset(classId, subjectId, qsetId) {
            //เช็คว่า qset นี้ได้ถูกนำไปจัดชุดหรือเปล่า ถ้ามี ขึ้นเตือนว่าจะลบหรือไม่ลบ
            //แล้วบอกด้วยว่า ถ้าลบข้อสอบนี้ ชุดที่เคยมีข้อสอบนี้ จะถูกลบออกไปด้วย    
            var dataToPost = "{ classId : '" + classId + "', subjectId : '" + subjectId + "', qsetId : '" + qsetId + "'}";
            if (isQsetExistInTestset(qsetId)) {
                callDialog("showDialogConfirm", dataToPost, "ข้อสอบนี้ได้ถูกจัดเป็นชุดข้อสอบแล้ว ถ้าต้องการลบ ชุดที่มีข้อสอบชุดนี้ จะถูกลบออกไปด้วย")
            } else {
                showDialogConfirm(dataToPost);
            }
        }

        function showDialogConfirm(data) {
            callDialog("confirmdeleteQset", data, "ยืนยันการลบข้อสอบ ?");
        }

        function confirmdeleteQset(data) {
            var methodName = "DeleteQset";
            //var dataToPost = "{ qsetId : '" + qsetId + "'}";
            var isDeleted = postToServer(methodName, data);            
            if (isDeleted) {
                callAlertDialog("ลบเรียบร้อยแล้วค่ะ");
                location.reload();
            } else {
                callAlertDialog("ไม่สามารถลบได้ค่ะ!!")
            }
        }

        function showEvalutionInBook(sender) {
            var subjectId = $(sender).attr("subjectid");
            var classId = $(sender).attr("classid");
            if (subjectId == undefined) return "";
            var methodName = "GetContentEvalutionIndexInBook";
            var data = "{ subjectId : '" + subjectId + "', classId : '" + classId + "'}";
            return GetContentEvalutionIndexInBook(subjectId, classId);
        }
        //function GetContentEvalutionIndexInBook(subjectId, classId) {
        //    var result;
        //    $.ajax({
        //        type: "POST",
        //        url: "../WebServices/QuestionService.asmx/GetContentEvalutionIndexInBook",
        //        data: "{ subjectId : '" + subjectId + "', classId : '" + classId + "'}",
        //        async: false,
        //        contentType: "application/json; charset=utf-8", dataType: "json",
        //        success: function (data) {
        //            result = data.d;
        //        },
        //        error: function (xhr, status, text) {
        //            alert(status);
        //        }
        //    });
        //    return result;
        //}

        function isQsetExistInTestset(qsetId) {
            var methodName = "IsQsetInTestset";
            var dataToPost = "{ qsetId : '" + qsetId + "'}";
            return postToServer(methodName, dataToPost);
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

        function saveQsetToTestset(chkBox, userId, qSetId, qsetType, classId, subjectId, isWpp) {
            var dataToPost = "{ addById : '" + userId + "', qSetId : '" + qSetId + "', qsetType : '" + qsetType + "', classId : '" + classId + "', subjectId : '" + subjectId + "',isWpp : " + isWpp + "}";
            var methodName = (chkBox.checked) ? "AddQuestionsTestset" : "RemoveQuestionsTestset";
            saveQuestionsTestset(dataToPost, methodName);
            updateQuestionSeleted(chkBox);
            countQuestionSelected();

            //alert($(chkBox).parent());
            var span = $(chkBox).parent().find('span');
            if (span != undefined) $(span).remove();
            //$(chkBox).parent().find('span').remove();
            // var qsetName = $(chkBox).next().text();
            //$('input[subjectid="' + subjectId + '"][classid="' + classId + '"]').val(qsetName);
        }

        function saveQuestionsTestset(dataToPost, methodName) {
            $.ajax({
                type: "POST",
                url: "<%=ResolveUrl("~")%>WebServices/QuestionService.asmx/" + methodName,
                data: dataToPost,  //" 
                contentType: "application/json; charset=utf-8", dataType: "json",
                async: false,
                success: function (msg) {
                },
                error: function myfunction(request, status) {
                }
            });
        }

        function updateQuestionSeleted(e, isWpp) {
            var subjectId = $(e).attr('value');

            var sum = 0;
            $("input[value='" + subjectId + "']").each(function () {
                sum = sum + parseInt($(this).attr('questionAmount'));
            });
            var sumSelected = 0;
            $("input[value='" + subjectId + "']:checked").each(function () {
                sumSelected = sumSelected + parseInt($(this).attr('questionAmount'));
            });
            sumSelected = numberWithCommas(sumSelected);
            //console.log(sum + " - " + sumSelected);
            $('span.' + subjectId).text("จากทั้งหมด " + numberWithCommas(sum) + " ข้อ เลือกมาแล้ว " + sumSelected + " ข้อ");
            $('span.' + subjectId).attr('selectQuestionAmount', sumSelected);

            //var sumAll = 0;
            //$('.spnSelectQuestion').each(function () {
            //    var thisVal = $(this).attr("selectQuestionAmount") === undefined ? 0 : $(this).attr("selectQuestionAmount");
            //    sumAll = sumAll + parseInt(thisVal);
            //});
            //sumAll = numberWithCommas(sumAll);
            //$('#spnTotalQuestionsSelected').text(sumAll);
            //$('#spnTotalQuestionsSelected1').text(sumAll);
        }

        function countQuestionSelected() {
            var sumAll = 0;
            $('.spnSelectQuestion').each(function () {
                var thisVal = $(this).attr("selectQuestionAmount") === undefined ? 0 : $(this).attr("selectQuestionAmount");
                sumAll = sumAll + parseInt(thisVal);
            });
            sumAll = numberWithCommas(sumAll);
            //console.log(sumAll);
            $('#spnTotalQuestionsSelected').text(sumAll);
            $('#spnTotalQuestionsSelected1').text(sumAll);
        }
        function numberWithCommas(x) {
            return x.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
        }

        countQuestionSelected();
    </script>
</asp:Content>
