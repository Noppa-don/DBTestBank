<%@ Page Title="QuickTest - ขั้นที่ 3: เลือกหน่วยการเรียนรู้" Language="vb" AutoEventWireup="false"
    MasterPageFile="~/Site.Master" CodeBehind="Step3.aspx.vb" Inherits="QuickTest.Step3" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content4" ContentPlaceHolderID="HeadStyleContent" runat="server">
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">

    <link rel="stylesheet" type="text/css" href="<%=ResolveUrl("~")%>shadowbox/shadowbox.css" />
    <link charset="utf-8" media="screen" type="text/css" href="<%=ResolveUrl("~")%>css/aa.css" rel="stylesheet" />
    <link type="text/css" rel="stylesheet" href="<%=ResolveUrl("~")%>css/general.css" />
    <script src="../js/json2.js" type="text/javascript"></script>

    <%If Not IE = "1" Then%>
    <%--        <script src="../Scripts/jquery.signalR-2.0.2.min.js" type="text/javascript"></script>
        <script src='<%=ResolveClientUrl("~/signalr/hubs") %>' type="text/javascript"></script>--%>
    <script type="text/javascript">var baseUrl = "<%= ResolveUrl("~/") %>";</script>
    <%--       <script src="../js/DashboardSignalR.js" type="text/javascript"></script>--%>
    <%End If%>

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
            -moz-border-radius: 15px;
            padding: 10px;
            padding-bottom: 5px;
            padding-top: 5px;
            margin: 3px;
            -webkit-border-radius: 15px;
            border-radius: 15px;
            behavior: url(/css/PIE.htc);
        }

        .ForDescription {
            position: fixed;
            border: 3px solid #FFCC66;
            background-color: #FFFFCC;
            -moz-border-radius: 15px;
            color: #000000;
            padding: 10px;
            padding-bottom: 5px;
            padding-top: 5px;
            display: none;
            margin: 3px;
            top: 0px;
            left: 0px;
            -webkit-border-radius: 15px;
            border-radius: 15px;
            behavior: url(/css/PIE.htc);
        }

        .ForRadEditor {
            background-image: none !important;
            background-color: White;
        }

        .MainDivSummary {
            width: 160px;
            padding: 2px 10px 2px 10px;
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

        .MainM {
            margin-left: 365px !important;
        }

        .MainEM {
            width: 0px!important;
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
            width: 125px;
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

        body {
            background:none!important;
        }
    </style>

    <%If IsAndroid = True Then%>
    <style type="text/css">
        #main, #site_content, .content {
            width: 880px !important;
        }

        nav, footer, #logo, .slogantext {
            width: 860px !important;
        }

        #btnNextStep4 {
            margin-left: 670px !important;
            height: 70px !important;
        }

        .bordered {
            width: 98% !important;
            margin-left: auto !important;
            margin-right: auto !important;
        }

        p label {
            margin-left: 10px !important;
        }

        table tr td {
            font-size: 25px !important;
        }
    </style>
    <%End If%>
    <%If BusinessTablet360.ClsKNSession.RunMode.ToLower = "twotests" Then %>
    <style type="text/css">
        html {
            background-image: none !important;
        }
    </style>
    <%End If %>
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

        .SelectEvalution {
            font: 100% 'THSarabunNew';
            border: 0;
            width: 99px;
            margin: 0 0 0 212px;
            height: 33px;
            padding: 2px 0 3px 0;
            cursor: pointer;
            background: #1EC9F4;
            -webkit-box-shadow: 0 1px 2px rgba(0,0,0,.2);
            -moz-box-shadow: 0 1px 2px rgba(0,0,0,.2);
            box-shadow: 0 1px 2px rgba(0,0,0,.2);
            color: #FFF;
            border: solid 1px #0D8AA9;
            background: #46C4DD;
            background: -webkit-gradient(linear, left top, left bottom, from(#63CFDF), to(#17B2D9));
            background: -moz-linear-gradient(top, #63CFDF, #17B2D9);
            background: linear-gradient(#63CFDF, #17B2D9);
            -pie-background: linear-gradient(#63CFDF, #17B2D9);
            text-shadow: 1px 1px #178497;
            position: absolute;
            background-color: #B6B6B6;
            -webkit-border-radius: 0.5em;
            -moz-border-radius: 0.5em;
            border-radius: 0.5em;
            behavior: url(../css/PIE.htc);
        }
    </style>

    <script type="text/javascript">

        var ua = navigator.userAgent.toLowerCase();
        var isAndroid = ua.indexOf("android") > -1;
        var maxQuestionInTestset = '<%= CInt(ConfigurationManager.AppSettings("MaxQuestionInTestset"))%>';

        $(function () {
            //$('#btnNextStep4').click(function (e) {
            //    var sumOfQuestion = parseInt($('#spnTotalQuestionsSelected1').text());
            //    if (sumOfQuestion > maxQuestionInTestset) {
            //        e.preventDefault();
            //        //call dialog
            //        $('#DialogWarning').dialog("option", "title", "จำนวนข้อสอบในชุด ห้ามเกิน " + maxQuestionInTestset + " ข้อค่ะ <br />เลือกออกบางส่วนนะคะ").dialog('open');
            //        return false;
            //    }
            //    return true;
            //});

            $('.SelectEvalution').click(function () {

                return false;
            });

            $('#DialogWarning').dialog({
                autoOpen: false, draggable: false, resizable: false, modal: true, width: 'auto', buttons: {
                    //"ยกเลิก": function () {
                    //    $(this).dialog('close');
                    //},
                    "ตกลง": function () {
                        IsCanQuiz = true;
                        $(this).dialog('close');
                        $("#btnOK").trigger('click');
                    }
                }
            });

        });

        function toggleNumQstn(mid, classid, subjectid) {
            //              if (document.getElementById('MID' + mid).checked) {
            //                  document.getElementById('spnSelected_' + mid).innerHTML = document.getElementById('spnTotal_' + mid).innerHTML;//                  
            //              }
            //              else if(document.getElementById('MID' + mid).checked == false){
            //              var text = '';
            //                  document.getElementById('spnSelected_' + mid).innerHTML = text;                 
            //              }                
            SumQuestionSelected();
            //              document.getElementById('spnTotalSubjectsSelected').innerHTML = String($.unique(selectedSubject).length);
            //              document.getElementById('spnTotalClassesSelected').innerHTML = String($.unique(selectedClass).length);
        };
        function SumQuestionSelected() {
            //              var sum = 0;
            //              //iterate through each textboxes and add the values
            //              $(".spnSelectedQuestions").each(function () {
            //                  //add only if the value is number
            //                  if (!isNaN($(this).text()) && $(this).text().length != 0) {
            //                      sum += parseFloat($(this).text());
            //                  }
            //              });
            //               var spnSelectLegth = document.getElementsByName('spnSelec').length;
            //                    var spnSelect = document.getElementsByName('spnSelec');
            //                    var sumSpnSelect = 0;
            //                    for (i = 0; i < spnSelectLegth; i++) {
            //                        var s = parseInt($(spnSelect.item(i)).text(), 10);
            //                        sumSpnSelect = sumSpnSelect + s;
            //                    }
            //                    $('#spnTotalQuestionsSelected').html(sumSpnSelect);
            //                    $('#spnTotalQuestionsSelected1').html(sumSpnSelect);
            if ($.browser.msie) {//IE
                var inputs = document.getElementsByTagName('span');
                var spanA = [];
                for (var i = 0; i < inputs.length; i++) {
                    if (inputs.item(i).getAttribute('name') == 'spnSelec') {
                        spanA.push(inputs.item(i));
                    }
                }
                var sumspan = 0;
                for (i = 0; i < spanA.length; i++) {
                    var s3 = parseInt($(spanA[i]).text(), 10);
                    sumspan = sumspan + s3;
                }
                $('#spnTotalQuestionsSelected').html(sumspan);
                $('#spnTotalQuestionsSelected1').html(sumspan);
            }
            else {//FF CHROME
                var spnSelectLegth = document.getElementsByName('spnSelec').length;
                var spnSelect = document.getElementsByName('spnSelec');
                var sumSpnSelect = 0;
                for (i = 0; i < spnSelectLegth; i++) {
                    var s = parseInt($(spnSelect.item(i)).text(), 10);
                    sumSpnSelect = sumSpnSelect + s;
                }
                console.log('sum = ' + sumSpnSelect);
                $('#spnTotalQuestionsSelected').html(sumSpnSelect);
                $('#spnTotalQuestionsSelected1').html(sumSpnSelect);
            }
            //.toFixed() method will roundoff the final sum to 2 decimal places
            //$("#spnTotalQuestionsSelected").html(sum);
        }
        function SaveLog(LogType, LogText) {

            $.ajax({
                type: "POST",
                url: "<%=ResolveUrl("~")%>testset/Step3.aspx/SetLog",
                data: "{ LogText : '" + LogText + "', LogType:'" + LogType + "' }",  //" 
                contentType: "application/json; charset=utf-8", dataType: "json",
                success: function (msg) {
                },
                error: function myfunction(request, status) {
                }
            });
        }
        function onSave(chkBox, qSetId, testSetId, userId, classId) {
            var valOftotal = $('#spnTotal_' + qSetId).html(); console.log('valOftotal = ' + valOftotal);
            var CreateStr = "ชุดนี้เลือกมาแล้ว <span id='spnSelected_" + qSetId + "' name='spnSelec'>" + valOftotal + "</span> จาก <span id='spnTotal_" + qSetId + "'>" + valOftotal + "</span> ข้อ";
            var parentTd = $(chkBox).parent('td');
            if (chkBox.checked) {
                //$(chkBox).next().next().next().html(CreateStr);                
                $(parentTd).find('a.aTag').html(CreateStr);
            }
            else {
                var CreateStr = "ชุดนี้ยังไม่ถูกเลือก (มีทั้งหมด <span id='spnTotal_" + qSetId + "'>" + valOftotal + "</span> ข้อ)";
                //$(chkBox).next().next().next().html(CreateStr);
                $(parentTd).find('a.aTag').html(CreateStr);
            }
            SumQuestionSelected();
            var valReturnFromCodeBehide;
            $.ajax({
                type: "POST",
                url: "<%=ResolveUrl("~")%>testset/Step3.aspx/OnSaveCodeBehide",
                data: "{ needRemove : '" + !(chkBox.checked) + "', qSetId : '" + qSetId + "', testSetId : '" + testSetId + "', userId : '" + userId + "', classId : '" + classId + "'}",  //" 
                contentType: "application/json; charset=utf-8", dataType: "json",
                success: function (msg) {
                    if (msg.d != 0) {
                        valReturnFromCodeBehide = msg.d;
                        if (valReturnFromCodeBehide == "1") {
                            if (chkBox.checked && $('#CountQtip').val() < 4) {
                                callQtip(chkBox.id, 'ข้อสอบที่เลือกมาไม่ใช่แบบตัวเลือกหรือถูกผิด จะใช้ร่วมกับกระดาษคำตอบไม่ได้นะคะ');
                            }
                        }
                        else if (valReturnFromCodeBehide == "0") {
                            if (chkBox.checked) {
                                checkOverQuestion(chkBox.id);
                            }
                        }
                    }
                },
                error: function myfunction(request, status) {
                }
            });
        }
        function checkOverQuestion(id) {
            var sumQuestion = $('#spnTotalQuestionsSelected1').text();
            if (parseInt(sumQuestion) > 120) {
                callQtip(id, 'จำนวนข้อสอบที่เลือกมาเกิน 120 ข้อ จะใช้ร่วมกับกระดาษคำตอบไม่ได้นะคะ');
                setTimeout(function () { $('label[for="' + id + '"]').qtip('destroy'); }, 4000);
            }
        }
        function callQtip(id, content) {
            $('label[for="' + id + '"]').stop().qtip({
                content: content.toString(),
                show: { ready: true },
                style: {
                    width: 250, padding: 5, background: '#F68500', color: 'white', textAlign: 'center', border: { width: 7, radius: 5, color: '#F68500' }, tip: 'leftMiddle', name: 'dark', 'font-weight': 'bold'
                },
                position: { corner: { tooltip: 'leftMiddle', target: 'rightMiddle' } },
                hide: false
            });
        }
        function destroyQtip(id) {
            setTimeout(function () { $('label[for="' + id + '"]').qtip('destroy'); checkOverQuestion(id); }, 4000);

            var countQtip = $('#CountQtip').val();
            $('#CountQtip').val(parseInt(countQtip) + 1);
            //alert($('#CountQtip').val());            
        }

        function AddQcatOrQset(inputSubjectId, InputClassId) {

            $.ajax({
                type: "POST",
                url: "<%=ResolveUrl("~")%>testset/Step3.aspx/SetLogAddQcatOrQset",
                data: "{ SubjectId : '" + inputSubjectId + "', LevelId:'" + InputClassId + "' }",  //" 
                contentType: "application/json; charset=utf-8", dataType: "json",
                success: function (msg) {
                },
                error: function myfunction(request, status) {
                }
            });
            window.location = '../TestSet/AddNewQuestionCatAndSet.aspx?Subjectid=' + inputSubjectId + '&LevelId=' + InputClassId;
        }
        function EditQCatName(QsetId, QCatName) {
            $('#txtEditQCatName').val(QCatName);
            $('#HDEditQCatName').val(QsetId);
            $('#DivEditQCatName').dialog('open');

            $.ajax({
                type: "POST",
                url: "<%=ResolveUrl("~")%>testset/Step3.aspx/SetLogEditQCatName",
                data: "{ QsetId : '" + QsetId + "' }",  //" 
                contentType: "application/json; charset=utf-8", dataType: "json",
                success: function (msg) {
                },
                error: function myfunction(request, status) {
                }
            });
        }


    </script>
    <link href="../css/jquery-ui-1.8.18.custom.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        tr.qsetHeader .boxRadio {
            width: 160px;
            margin-left: 15px;
        }

            tr.qsetHeader .boxRadio input[type=radio] {
                position: absolute;
                visibility: hidden;
            }

            tr.qsetHeader .boxRadio label {
                display: block;
                position: relative;
                z-index: 9;
                cursor: pointer;
                padding-left: 25px;
            }

            tr.qsetHeader .boxRadio:hover label, .check {
                font-weight: bold;
                color: #FFFFFF;
            }


            tr.qsetHeader .boxRadio .check {
                display: block;
                position: absolute;
                border: 3px solid #AAAAAA;
                border-radius: 100%;
                height: 15px;
                width: 15px;
                margin-top: -30px;
                z-index: 9;
            }

            tr.qsetHeader .boxRadio:hover input[type=radio]:not(:checked) ~ .check {
                border: 3px solid #FFFFFF !important;
            }
            /*tr.qsetHeader .boxRadio:hover input[type=radio]:checked ~ .check{
                  border:3px solid red;
              }*/

            tr.qsetHeader .boxRadio .check::before {
                display: block;
                position: absolute;
                content: '';
                border-radius: 100%;
                height: 11px;
                width: 11px;
                top: 2px;
                left: 2px;
                margin: auto;
            }

            tr.qsetHeader .boxRadio input[type=radio]:checked ~ .check {
                border: 3px solid #f47a20;
            }

                tr.qsetHeader .boxRadio input[type=radio]:checked ~ .check::before {
                    background: #f47a20;
                }

            tr.qsetHeader .boxRadio input[type=radio]:checked ~ label {
                color: #f47a20;
                font-weight: bold;
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

            <div id="site_content" style="padding: 0; margin: auto;    display: inline-block;">
                <div>
                    <img style="float: left; vertical-align: middle; margin: 0 10px 0 0;"
                        src="<%=ResolveUrl("~")%>images/another_page.png" alt="another page" />
                    <h2>เลือกหน่วยการเรียนรู้/ข้อสอบ</h2>
                    <asp:Button ID="btnNextStep4" ClientIDMode="Static" runat="server" class="submitChangeFontSize" Text="Next - ไปต่อขั้น 3 >>"
                        Style="width: 200px; position: initial; height: 40px; float: right; margin-top: -66px; font-size: 120%;"></asp:Button>
                </div>
                <div style="font-size: 120%;">
                    <p>
                        <label>เลือกหน่วยการเรียนรู้หรือข้อสอบที่ต้องการจัดชุดค่ะ</label>
                    </p>
                </div>
                <%If HttpContext.Current.Session("EditID") IsNot Nothing And HttpContext.Current.Session("EditID") <> "" Then %>
                <div style="text-align: center;">
                    <span style="color: red; font-size: 20px;"><%= EditTestSetWarningText %></span>
                </div>
                <%End If %>
                <div class="content ListingFixedHeightContainer" style="width: 930px; margin: 0 0 5px 0; padding: 0;">
                    <section id="select">	
                        <%If Not BusinessTablet360.ClsKNSession.IsAddQuestionBySchool Then %>	
          <asp:Repeater id="ListingSubject" runat="server">
            <ItemTemplate>
                <h3 id="Subject_<%# Container.DataItem("SubjectId") %>" style="margin: 0px 0 0 15px;"><%# Container.DataItem("SubjectName")%></h3>
		        <center>
                    <div id="div-1" style='text-align:left' class="ListingContent"> 
                        <asp:Repeater id="ListingClass" runat="server">
                            <ItemTemplate>
		                       <table style="width:100%; border-spacing:0;">
                                <tbody id="ShowHide" >
                                    <tr onclick="return toggleTbody('<%# Container.DataItem("SubjectId") & "_" &  Container.DataItem("ClassId")%>',this);" class="qsetHeader" >
                                        <td style="display:flex;">
                                            <b><%# Container.DataItem("ClassName")%></b>
                                         
                                            <%If HttpContext.Current.Application("runmode") <> "twotests" And HttpContext.Current.Application("runmode") <> "wordonly" Then%>
                                                <div class="boxRadio">                                        
                                                    <input type="radio" name="<%# "sortQset" & Container.DataItem("SubjectId") & Container.DataItem("ClassId")%>" 
                                                        checked="checked" id="<%# "sort_" & Container.DataItem("SubjectId") & "_" & Container.DataItem("ClassId") & "_normal"%>" />
                                                    <label for="<%# "sort_" & Container.DataItem("SubjectId") & "_" & Container.DataItem("ClassId") & "_normal"%>" 
                                                        onclick="sortQset('<%# Container.DataItem("SubjectId") & "_" & Container.DataItem("ClassId")%>',1);">เรียงตามตัวอักษร</label>
                                                    <div class="check"></div>    
                                                </div> 
                                                <div class="boxRadio">                                       
                                                    <input type="radio" name="<%# "sortQset" & Container.DataItem("SubjectId") & Container.DataItem("ClassId")%>" 
                                                        id="<%# "sort_" & Container.DataItem("SubjectId") & "_" & Container.DataItem("ClassId") & "_qset"%>" />
                                                    <label for="<%# "sort_" & Container.DataItem("SubjectId") & "_" & Container.DataItem("ClassId") & "_qset"%>" 
                                                        onclick="sortQset('<%# Container.DataItem("SubjectId") & "_" & Container.DataItem("ClassId")%>',2);">เรียงตามบทเรียน</label>
                                                    <div class="check"><div class="inside"></div></div>    
                                                </div>
                                            <%End If%>

                                            <%If HttpContext.Current.Application("NeedAddNewQCatAndQsetButton") = True Then%>
                                                <img src="../Images/New.png" style='margin-left: 84%;cursor: pointer; width: 50px; padding-bottom: 10px;' title='เพิ่มหน่วยการเรียนรู้/ชุดข้อสอบ' 
                                                    onclick="AddQcatOrQset('<%# Container.DataItem("SubjectId")%>','<%# Container.DataItem("ClassId")%>')" />
                                            <%End If%>
                                    </td>
                                    <td>
                                        <%If HttpContext.Current.Application("IsSelectByEvalutionIndex") = True Then%>
                                        <input type="button" id="btnSelectEvalution" 
                                             onclick="OpenSelectEvalutionIndex('<%# Container.DataItem("SubjectId")%>    ','<%# Container.DataItem("ClassId")%>    ')" />
                                            class="SelectEvalution" value="เลือกตามตัวชี้วัด" 
                                            style="width: 130px; position: initial; font-size:90%; float:right;"></input>
                                        <%End If%>
                                    </td>
                                </tr>
                                </tbody>
                                <tbody  class='off' id="Notice<%# Container.DataItem("SubjectId") & "_" & Container.DataItem("ClassId")%>"><tr><td colspan="'2'">มีรายชื่อหน่วยการเรียนรู้ซ่อนอยู่</td></tr></tbody>
                                <tbody id="<%# Container.DataItem("SubjectId") & "_" &  Container.DataItem("ClassId")%>">
                                <%# CreateTestUnitList(Eval("ClassId"), Eval("SubjectId"))%>
		                        </tbody>
                              </table>
                            </ItemTemplate>
                        </asp:Repeater>
		            </div>                  
		        </center>
            </ItemTemplate>
          </asp:Repeater>
                        <%Else %>
                         <asp:Repeater id="Repeater1" runat="server">
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
                                            <td style="text-align:center;"><input type="text" class="qtipQcategory" subjectid="<%# Container.DataItem("SubjectId") %>" classid="<%# Container.DataItem("ClassId") %>" /></td>
                                            <td style="text-align:center;"><input type="text" class="qtipEvalution" subjectid="<%# Container.DataItem("SubjectId") %>" classid="<%# Container.DataItem("ClassId") %>"/></td>
                                        </tr>
                                        <tr><td colspan="2" style="padding-left:25px;"><span class="spnSelectQuestion <%# Container.DataItem("SubjectId") %><%# Container.DataItem("ClassId") %>"><%#CreateQuestionAmount(Eval("ClassId"), Eval("SubjectId")) %></span></td></tr>
                                    </table>

                                       <%--<%#CreateQuestionBySchool(Eval("ClassId"), Eval("SubjectId"), Eval("ClassName")) %>--%>

                                    <div class="addQuestion" subjectid="<%# Container.DataItem("SubjectId") %>" classid="<%# Container.DataItem("ClassId") %>">
                        <img src="../Images/New-.png" style="position:absolute;left:35%;width:40px;" alt="" />                        
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
                        <%End If %>
<div style="text-align: right;padding-right: 40px;display:none;    margin-top: 10px;">รวมทั้งหมด <span id='spnTotalQuestionsSelected'><%= GetQuestionAmount()%></span> ข้อ</div>
		 </section>
                </div>
            </div>
            <%--<script type="text/javascript">                    var c = document.getElementById("select"); function resizeText(multiplier, e) { e.preventDefault(); if (c.style.fontSize == "") { c.style.fontSize = "1.0em"; } c.style.fontSize = parseFloat(c.style.fontSize) + (multiplier * 0.2) + "em"; } </script>--%>
            <%--<a href="SaveSet.aspx?iframe=true&width=800&height=600"" rel="prettyPhoto" title="ตั้งชื่อแบบทดสอบที่จัดชุดมาค่ะ" style="text-decoration:none">
            <input class="submitChangeFontSize" type="submit" name="smallerFont" value="บันทึก -> พิมพ์"  style="margin-left: 670px; width:220px; margin-top: 15px" />
        </a>--%>

            <!--[if lte IE 7]><br /><![endif]-->
            <a id="footer-back-to-top" style='cursor: not-allowed' class="WhiteButton badge-back-to-top ">
                <strong style="font-size: 18px;">รวม <span id='spnTotalQuestionsSelected1'>0</span> ข้อ</strong></a>
            <footer style="margin-top: 15px">
      สงวนลิขสิทธิ์ &copy; บริษัท สำนักพิมพ์วัฒนาพานิช จำกัด
    </footer>
        </div>
        <div id="tip">
        </div>
        <div id='DescriptionDiv' class="ForDescription">
            เมื่อคลิกสามารถดูรายละเอียดข้อสอบและ<br />
            &nbspเลือกข้อสอบเฉพาะบางข้อที่ต้องการได้
        </div>
        <%-- <ul id="HelpSelect">
                <li class="about2" style="z-index: 99;"><a title="สงสัยในการใช้งาน ทำตามขั้นตอนตัวอย่างนี้นะคะ"
                    id="HelpSelect" onclick="ShowHelpSelect();">ช่วย<br />
                    เหลือ<br />
                </a></li>
            </ul>--%>
        <div id='HowToDialog' style="display: none; width: 100%; height: 100%; z-index: 0; position: fixed; top: 0px; left: 0px; background-color: Black">
            <iframe id="FrameShowHowTo" scrolling="no" style="overflow: hidden; white-space: nowrap; width: 100%; height: 100%; position: relative; margin-left: auto; margin-right: auto; z-index: 0;"
                frameborder="0"></iframe>
            <ul id="CloseHelp">
                <li class="about1" style="z-index: 999;"><a title="จบการฝึกฝน" id="CloseHelp" onclick="CloseHelpSelect();">จบการ<br />
                    ฝึกฝน </a></li>
            </ul>
        </div>
        <div id='dialogDeleteQcatOrSet' title='ลบหน่วยการเรียนรู้หรือชุดข้อสอบ'>
            <br />
            <span style='font-size: 25px;'><u>ลบหน่วยการเรียนรู้</u></span>
            <br />
            <br />
            "<b><span id='spnQCatName'></span></b>"
        <br />
            <br />
            <input type="button" id='btnDeleteQCat' onclick='DeleteQCategory()' style='margin-left: 390px; width: 70px; height: 35px;'
                value='ตกลง' />
            <br />
            <br />
            <hr />
            <br />
            <span style='font-size: 25px;'><u>ลบขุดข้อสอบ</u></span>
            <br />
            <br />
            "<b><span id='spnQsetName'></span></b>"
        <br />
            <br />
            <input type="button" id='btnDeleteQset' onclick='DeleteQset()' style='margin-left: 390px; width: 70px; height: 35px;'
                value='ตกลง' />
        </div>
        <input type="hidden" id='HdQsetid' />
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
        </telerik:RadScriptManager>
        <div id='DivEditQsetName' style='z-index: 99; text-align: center; display: none; width: 660px; height: 350px; margin-left: auto; margin-right: auto; margin-top: -555px; position: relative; background-color: #4f674f; border-radius: 5px;'>
            <table>
                <tr>
                    <td style="background-color: #4f674f; text-align: center;">
                        <span style='font-size: 20px; font-weight: bold; color:white;'>แก้ไขชื่อชุดข้อสอบ</span>
                    </td>
                </tr>
            </table>

            <telerik:RadEditor Style='margin-left: 30px;' EditModes="Design" ContentAreaCssFile="~/css/ForRadEditor.css"
                Width='600' Height='200' ID="txtEditQsetName" runat="server">
                <Tools>
                    <telerik:EditorToolGroup>
                        <telerik:EditorTool Name="Bold" />
                        <telerik:EditorTool Name="Cut" />
                        <telerik:EditorTool Name="Copy" />
                    </telerik:EditorToolGroup>
                </Tools>
                <Content></Content>
            </telerik:RadEditor>
            <br />
            <%--<asp:Button style='width:100px;height:40px;margin-right:35px;' ID="btnSaveEditQsetName" runat="server" Text="ตกลง" />--%>
            <input type="button" style='width: 100px; height: 40px; margin-right: 35px; font-size: 20px;'
                id='btnSaveEditQsetName' value='ตกลง' />
            <input type="button" id='btnCloseDiv' style='margin-right: -360px; width: 100px; height: 40px; font-size: 20px;'
                value='ยกเลิก' />
        </div>
        <div id='DivEditQCatName' title='แก้ไขชื่อหน่วยการเรียนรู้'>
            <input type="text" id='txtEditQCatName' style='width: 445px;' />
        </div>
        <div id="DialogWarning" title=""></div>
        <input type="hidden" id='HDEditQsetName' value='' />
        <input type="hidden" id='HDEditQCatName' value='' />
    </form>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ExtraScript" runat="server">
    <script src="../js/jquery-ui-1.8.18.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            //ดักถ้าเข้าจาก Tablet ของครู
            //if (isAndroid) {
            //$('#main').css('width', '880px');
            //$('nav').css('width', '860px');
            //$('#site_content').css('width', '880px');
            //$('.content').css('width', '880px');
            //$('#btnNextStep4').css({'margin-left': '670px','height':'70px'});
            //$('footer').css('width', '860px');
            //$('#logo').css('width', '860px');
            //$('.slogantext').css('width', '860px');
            //$('.bordered').css({'width': '98%','margin-left': 'auto','margin-right': 'auto'});
            ////$('.bordered').css('margin-left', 'auto');
            ////$('.bordered').css('margin-right', 'auto');
            //$('p label').css('margin-left', '10px');
            //$('table tr td').css('font-size', '25px');
            //}

            //เมื่อคลิกที่ Icon W
            $('.MainW').click(function () {
                var qsetid = $(this).attr('qsetid');
                $.fancybox({
                    'autoScale': true,
                    'transitionIn': 'none',
                    'transitionOut': 'none',
                    'href': '<%=ResolveUrl("~")%>testset/WordLayoutConfirmed.aspx?qsetid=' + qsetid,
                    'type': 'iframe',
                    'width': 980,
                    'height': 600
                });

                SaveLog("13", "ไปหน้าแก้ไขข้อสอบฝั่ง Word (QsetId=" + qsetid + ")");
            });

            //เมื่อคลิกที่ Icon Q
            $('.MainQ').click(function () {
                var qsetid = $(this).attr('qsetid');
                $.fancybox({
                    'autoScale': true,
                    'transitionIn': 'none',
                    'transitionOut': 'none',
                    'href': '<%=ResolveUrl("~")%>testset/QuizLayoutConfirmed.aspx?qsetid=' + qsetid,
                    'type': 'iframe',
                    'width': 980,
                    'height': 600
                });
                SaveLog("13", "ไปหน้าแก้ไขข้อสอบฝั่ง Quiz (QsetId=" + qsetid + ")");
            });

            $('.MainM').click(function () {
                var qsetid = $(this).attr('qsetid');
                $.fancybox({
                    'autoScale': true,
                    'transitionIn': 'none',
                    'transitionOut': 'none',
                    'href': '<%=ResolveUrl("~")%>testset/MoveExamPage.aspx?qsetid=' + qsetid,
                    'type': 'iframe',
                    'width': 980,
                    'height': 600
                });
                SaveLog("13", "ไปหน้าย้ายข้อสอบ (QsetId=" + qsetid + ")");
            });

             $('.MainEM').click(function () {
                var qsetid = $(this).attr('qsetid');
                $.fancybox({
                    'autoScale': true,
                    'transitionIn': 'none',
                    'transitionOut': 'none',
                    'href': '<%=ResolveUrl("~")%>testset/MoveEngExam.aspx?qsetid=' + qsetid,
                    'type': 'iframe',
                    'width': 980,
                    'height': 600
                });
                SaveLog("13", "ไปหน้าย้ายข้อสอบภาษาอังกฤษ (QsetId=" + qsetid + ")");
            });

            $('#dialogDeleteQcatOrSet').dialog({
                autoOpen: false,
                height: 550,
                width: 500
            });
            $('#DivEditQCatName').dialog({
                autoOpen: false,
                height: 250,
                width: 500,
                modal: true,
                buttons: {
                    'ตกลง': function () {
                        //                alert($('#HDEditQCatName').val());
                        //                alert($('#txtEditQCatName').val());
                        var QsetId = $('#HDEditQCatName').val()
                        var newQCatName = $('#txtEditQCatName').val();
                        if (newQCatName.match(/^\s+$/) === null) {
                            if (newQCatName == '') {
                                alert('ห้ามใส่ค่าว่าง');
                            } else {
                                $.ajax({
                                    type: "POST",
                                    url: "<%=ResolveUrl("~")%>TestSet/Step3.aspx/EditQCatName",
                                    data: "{ QsetId: '" + QsetId + "',QCatName: '" + newQCatName + "'}",
                                    contentType: "application/json; charset=utf-8", dataType: "json",
                                    success: function (msg) {

                                        if (msg.d == 'Complete') {
                                            window.location = '../TestSet/Step3.aspx';
                                        }
                                    },
                                    error: function myfunction(request, status) {
                                        alert('บางอย่างผิดพลาดโปรดลองอีกครั้ง')
                                    }
                                });
                            }
                        }
                        else {
                            alert('ห้ามใส่ค่าว่าง');
                        }
                    },
                    'ยกเลิก': function () {
                        var QsetId = $('#HDEditQCatName').val();
                        $.ajax({
                            type: "POST",
                            url: "<%=ResolveUrl("~")%>TestSet/Step3.aspx/EditQCatName",
                            data: "{ QsetId: '" + QsetId + "',QCatName: 'CancelEditQCat'}",
                            contentType: "application/json; charset=utf-8", dataType: "json",
                            success: function (msg) {

                                if (msg.d == 'Complete') {
                                    //$(this).dialog('close');
                                    $('#DivEditQCatName').dialog('close');
                                }
                            },
                            error: function myfunction(request, status) {
                                alert('บางอย่างผิดพลาดโปรดลองอีกครั้ง')
                            }
                        });

                    }
                }
            });
            //$(this).dialog('close');
            $('#btnCloseDiv').click(function () {
                var QsetId = $('#HDEditQsetName').val();
                $.ajax({
                    type: "POST",
                    url: "<%=ResolveUrl("~")%>TestSet/Step3.aspx/EditQCatName",
                    data: "{ QsetId: '" + QsetId + "',QCatName: 'CancelEditQSet'}",
                    contentType: "application/json; charset=utf-8", dataType: "json",
                    success: function (msg) {

                        if (msg.d == 'Complete') {
                            //$(this).dialog('close');
                            $('#DivEditQsetName').hide();
                        }
                    },
                    error: function myfunction(request, status) {
                        alert('บางอย่างผิดพลาดโปรดลองอีกครั้ง')
                    }
                });

            });
            $('#btnSaveEditQsetName').click(function () {
                if (confirm('ต้องการแก้ไขชื่อชุดข้อสอบนี้ ?') == true) {
                    var QsetId = $('#HDEditQsetName').val();
                    SaveEditQSetName(QsetId);
                }
            });
        });
        function DeleteQcatOrQset(InputQsetId, InputQsetName, InputQcatName) {
            $('#dialogDeleteQcatOrSet').dialog('open');
            $('#spnQCatName').html(InputQcatName);
            $('#spnQsetName').html(InputQsetName);
            $('#HdQsetid').val(InputQsetId);
            SaveLog("13", "ไปหน้าลบหน่วยการเรียนรู้/ชุดข้อสอบ (QsetId=" + InputQsetId + ")")
        }
        function DeleteQCategory() {
            if (confirm('ต้องการลบหน่วยการเรียนรู้นี้ ?') == true) {
                var QsetId = $('#HdQsetid').val();
                $.ajax({
                    type: "POST",
                    url: "<%=ResolveUrl("~")%>TestSet/Step3.aspx/DeleteQuestionCategoryCB",
                    data: "{ VbQsetId: '" + QsetId + "'}",
                    contentType: "application/json; charset=utf-8", dataType: "json",
                    success: function (msg) {
                        if (msg.d == 'Complete') {
                            window.location = '../TestSet/Step3.aspx';
                        }
                    },
                    error: function myfunction(request, status) {
                        alert('บางอย่างผิดพลาดโปรดลองอีกครั้ง')
                    }
                });
            }
        }
        function DeleteQset() {
            if (confirm('ต้องการลบชุดข้อสอบนี้ ?') == true) {
                var QsetId = $('#HdQsetid').val();
                $.ajax({
                    type: "POST",
                    url: "<%=ResolveUrl("~")%>TestSet/Step3.aspx/DeleteQuestionSet",
                    data: "{ VbQsetId: '" + QsetId + "'}",
                    contentType: "application/json; charset=utf-8", dataType: "json",
                    success: function (msg) {
                        if (msg.d == 'Complete') {
                            window.location = '../TestSet/Step3.aspx';
                        }
                    },
                    error: function myfunction(request, status) {
                        alert('บางอย่างผิดพลาดโปรดลองอีกครั้ง')
                    }
                });
            }
        }
        function UpdateBookId(InputQsetId, InputGroupsubjectId) {
            //window.location = '../testset/EditBookQuestionCategory.aspx?QsetId=' + InputQsetId + '&GroupSubjectId=' + InputGroupsubjectId;
        }
        function GoToHomeWork(QsetId) {
            window.location = '../Module/HomeWorkManagerPage.aspx?QsetId=' + QsetId;
            //                $.ajax({ type: "POST",
            //	            url: "<%=ResolveUrl("~")%>TestSet/Step3.aspx/InsertNetTestSetForHomeWork",
            //	            data: "{ VbQsetId: '" + QsetId + "'}",
            //	            contentType: "application/json; charset=utf-8", dataType: "json",   
            //	            success: function (msg) {
            //                if (msg.d != '') {
            //                    var TestSetId = msg.d.TestSetId;
            //                    var TestSetName = msg.d.CategoryName;
            //                    window.location = '../Module/HomeWorkPage.aspx?Id=' + TestSetId + '&Name=' + TestSetName; 
            //                }
            //	            },
            //	            error: function myfunction(request, status)  {
            //                alert('บางอย่างผิดพลาดโปรดลองอีกครั้ง')
            //	            }
            //	        });                
        }
        function EditQsetName(QsetId, QsetName) {
            $('#DivEditQsetName').show();
            $('#HDEditQsetName').val(QsetId);
            $find('<%=txtEditQsetName.ClientID %>').set_html(QsetName);
            SaveLog("13", "ไปหน้าจอแก้ไขชุดข้อสอบ (QsetId=" + QsetId + ")")
        }
        function SaveEditQSetName(QsetId) {
            var editor = $find('<%=txtEditQsetName.ClientID %>');
            var QsetName = editor.get_html();
            /////////////////////////////////////////////////////////////////////
            $.ajax({
                type: "POST",
                url: "<%=ResolveUrl("~")%>TestSet/Step3.aspx/SaveNewQsetNameCodeBehind",
                data: "{ QsetId: '" + QsetId + "',QsetName:'" + QsetName + "' }",
                contentType: "application/json; charset=utf-8", dataType: "json",
                success: function (msg) {
                    if (msg.d == 'Complete') {
                        window.location = '../TestSet/Step3.aspx'
                    }
                    else if (msg.d == 'False') {
                        alert('ไม่สามารถบันทึกชื่อชุดข้อสอบที่เป็นค่าว่างได้');
                    }
                },
                error: function myfunction(request, status) {
                    alert('บางอย่างผิดพลาดโปรดลองอีกครั้ง')
                }
            });
        }
    </script>
    <script type="text/javascript">
        function toggleTbody(id, ele) {           
            if (document.getElementById) {
                var tbod = document.getElementById(id);
                var tbodNotice = document.getElementById('Notice' + id);
                if (tbod && typeof tbod.className == 'string') {
                    if (tbod.className == 'off') {
                        tbod.className = 'on';
                        tbodNotice.className = 'off';
                    } else {
                        tbod.className = 'off';
                        tbodNotice.className = 'on';
                    }
                }
            }
            return false;
        }

        function sortQset(subjectClassId, sortType) {
            $('#' + subjectClassId).html('');
            $.ajax({
                type: "POST",
                url: "<%=ResolveUrl("~")%>webservices/questionservice.asmx/GetQsetContentBySort",
                data: "{ subjectClassId : '" + subjectClassId + "', sortType : '" + sortType + "' }",
                contentType: "application/json; charset=utf-8", dataType: "json",
                async:false,
                success: function (msg) {                    
                    if (msg.d != "") {
                        $('#' + subjectClassId).html(msg.d);
                    }
                },
                error: function myfunction(request, status) {                      
                }
            });
        }
        function ShowHelpSelect() {
            //$.fancybox.close();
            $('#FrameShowHowTo').attr('src', '../HowTo/HowToSelectExam/HowToSelectExam.htm');
            $('#HowToDialog').show();
            $('#Help').hide();
            $('.pp_overlay').hide();
            $('.pp_pic_holder').hide();
            $('.pp_default').hide();
        }
        function CloseHelpSelect() {
            $('#HowToDialog').hide();
            $('#Help').show();
            $('.pp_overlay').show();
            $('.pp_pic_holder').show();
            $('.pp_default').show();
        }
        $(function () {
            $('#CloseHelp a').stop().animate({ 'marginLeft': '-10px' }, 1000);
            $('#CloseHelp > li').hover(function () {
                $('a', $(this)).stop().animate({ 'marginLeft': '-66px' }, 200);
            }, function () {
                $('a', $(this)).stop().animate({ 'marginLeft': '-10px' }, 200);
            });
        });
    </script>
    <script src="../js/GFB.js" type="text/javascript"></script>
    <script src="../js/jquery-ui-1.8.18.js" type="text/javascript"></script>
    <script src="../js/jquery.ui.touch-punch.min.js" type="text/javascript"></script>
    <script src="../js/facescroll.js" type="text/javascript"></script>
    <link href="../css/jquery.fancybox.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery.fancybox.js" type="text/javascript"></script>
    <script type="text/javascript">

        $(function () {
            new FastButton(document.getElementById('btnNextStep4'), TriggerServerButton);

            //redirect to add questions
            $('.addQuestion').click(function () {
                var subjectId = $(this).attr('subjectid');
                var levelId = $(this).attr('classid');
                window.location = "<%=ResolveUrl("~")%>testset/addQuestionsetPage.aspx?subjectid=" + subjectId + "&levelid=" + levelId;
            });
        });

        function OpenSelectEachQuestion(qSetId, classId) {
            $.fancybox({
                'autoScale': true,
                'transitionIn': 'none',
                'transitionOut': 'none',
                'href': '<%=ResolveUrl("~")%>testset/SelectEachQuestion.aspx?qSetId=' + qSetId + "&classId=" + classId,
                'type': 'iframe',
                'width': 980,
                'height': 600,
                'onClose': SumQuestionSelected()
            });
            SaveLog('11', 'ไปหน้าจอเลือกข้อสอบทีละข้อ (QsetId=' + qSetId + ')')
        }

        function OpenSelectEvalutionIndex(subjectId,levelId) {
            $.fancybox({
                'autoScale': true,
                'transitionIn': 'none',
                'transitionOut': 'none',
                'href': '<%=ResolveUrl("~")%>testset/SelectEvalution.aspx?subjectid=' + subjectId + '&levelid=' + levelId,
                'type': 'iframe',
                'width': 980,
                'height': 600,
                afterClose: function () {
                    location.reload();
                }
            });
        }

        if (!isAndroid) {
            $(".ListingContent").alternateScroll();
        }
        //  $("#Testdoo").alternateScroll();  
        $(document).ready(function () {
            $('.alt-scroll-vertical-bar').html('<img src="../Images/sprite_On.PNG" /><img src="../Images/sprite_Down.PNG" style="margin-top: 52px;" />')
            SumQuestionSelected();
            //alert(sumSpnSelect);
            // toolstip
            $('label').hover(function (e) {
                callTooltip('#SpanFullDetail', e);
                var id = $(this).prev().attr('id');
                if (id.indexOf('sort') > -1) return 0;
                id = id.substring(3, id.length);
                //$('#SpanFullDetail').html(id);
                $.ajax({
                    type: "POST",
                    url: "<%=ResolveUrl("~")%>testset/Step3.aspx/getQuestionSetName",
                    data: "{ qSetId : '" + id + "' }",  //" 
                    contentType: "application/json; charset=utf-8", dataType: "json",
                    success: function (msg) {
                        if (msg.d != 0) {//session(UserId) หายจะ return 0 - Sefety function
                            $('#SpanFullDetail').html(msg.d);
                            var widthSpan = $('#SpanFullDetail').width();
                            widthSpan = widthSpan / 2;
                            var heightSpan = $('#SpanFullDetail').height();
                            heightSpan = heightSpan / 2;
                            $('#SpanFullDetail').css('left', '50%').css('margin-left', -widthSpan + 'px').css('top', '50%').css('margin-top', -heightSpan + 'px');
                            $('#SpanFullDetail').css('width', (widthSpan * 2) + 'px');
                            //                      $('#SpanFullDetail').html(msg.d);
                            //                      $('#SpanFullDetail').show();
                            //alert('success'+valReturnFromCodeBehide);
                        }
                    },
                    error: function myfunction(request, status) {
                        //alert('shin' + request.statusText + status);    
                    }
                });
            }, function () {
                $("#SpanFullDetail").mouseleave(function () {
                    $('#SpanFullDetail').stop(true, true).fadeOut('slow');
                });
                $('#SpanFullDetail').stop(true, true).fadeOut('slow');
            });
            $(window).scroll(function () {
                //$('#SpanFullDetail').stop(true,true).fadeOut('slow');
                //alert('sdgsd');
            });
            //            $('body').click(function(){
            //                var d = $(this).find('qtip');
            //                alert(d);
            //            });
        });
        function callTooltip(obj, e) {
            var tip = $(this).find('toolsTip');
            var locateX = e.pageX + 20;
            var locateY = e.pageY + 20;
            locateX += 10;
            locateY -= 50;
            if (!isAndroid) {

            }
            //$(obj).css({ left: locateX, top: locateY }).delay(1000).fadeIn();
            $("#SpanFullDetail").mouseenter(function () {
                $('#SpanFullDetail').stop(true, true).delay(800).fadeIn('slow');
            });
            $(obj).stop(true, true).delay(800).fadeIn('slow');
            //$(obj).css({ left: locateX, top: locateY }).delay(1000).show();     
        }
        $('.aTag').hover(function () {
            if (!isAndroid) {
                $("#DescriptionDiv").stop(true, true).delay(800).fadeIn('slow');
            }
        },
        function () {
            if (!isAndroid) {
                $("#DescriptionDiv").stop(true, true).fadeOut('slow');
            }
        });

    </script>
    <script type="text/javascript">
        $(function () {
            $('input[type="text"].qtipQcategory').each(function () {
                $(this).qtip({
                    //show :{ready:true},
                    show: { event: 'mouseover' },
                    content: showQsetInBook(this),
                    style: {
                        width: 800, padding: 5, background: 'whitesmoke', color: 'black', textAlign: 'left', border: { width: 4, radius: 5, color: '#b92e7f' }, tip: 'leftMiddle', name: 'dark', 'font-size': '14px', 'line-height': '2em', height: 400
                    }, hide: false,
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
        function showQsetInBook(sender) {
            var subjectId = $(sender).attr("subjectid");
            var classId = $(sender).attr("classid");
            if (subjectId == undefined) return "";
            return getContentQsetInBook(subjectId, classId);
        }

        function getContentQsetInBook(subjectId, classId) {
            var result;
            $.ajax({
                type: "POST",
                url: "../WebServices/QuestionService.asmx/GetContentQsetInBook",
                data: "{ subjectId : '" + subjectId + "', classId : '" + classId + "'}",
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

        function showEvalutionInBook(sender) {
            var subjectId = $(sender).attr("subjectid");
            var classId = $(sender).attr("classid");
            if (subjectId == undefined) return "";
            return GetContentEvalutionIndexInBook(subjectId, classId);
        }
        function GetContentEvalutionIndexInBook(subjectId, classId) {
            var result;
            $.ajax({
                type: "POST",
                url: "../WebServices/QuestionService.asmx/GetContentEvalutionIndexInBook",
                data: "{ subjectId : '" + subjectId + "', classId : '" + classId + "'}",
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

        function saveQsetToTestset(chkBox, qSetId, testSetId, userId, classId) {
            $.ajax({
                type: "POST",
                url: "<%=ResolveUrl("~")%>testset/Step3.aspx/OnSaveCodeBehide",
                data: "{ needRemove : '" + !(chkBox.checked) + "', qSetId : '" + qSetId + "', testSetId : '" + testSetId + "', userId : '" + userId + "', classId : '" + classId + "'}",  //" 
                contentType: "application/json; charset=utf-8", dataType: "json",
                async: false,
                success: function (msg) {
                    updateQuestionSeleted(chkBox);
                },
                error: function myfunction(request, status) {
                }
            });
        }

        function updateQuestionSeleted(e) {
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
            $('span.' + subjectId).text("จากทั้งหมด " + numberWithCommas(sum) + " ข้อ เลือกมาแล้ว " + sumSelected + " ข้อ");
            $('span.' + subjectId).attr('selectQuestionAmount', sumSelected);

            var sumAll = 0;
            $('.spnSelectQuestion').each(function () {
                var thisVal = $(this).attr("selectQuestionAmount") === undefined ? 0 : $(this).attr("selectQuestionAmount");
                sumAll = sumAll + parseInt(thisVal);
            });
            sumAll = numberWithCommas(sumAll);
            $('#spnTotalQuestionsSelected').text(sumAll);
            $('#spnTotalQuestionsSelected1').text(sumAll);
        }
        function numberWithCommas(x) {
            return x.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
        }
    </script>
    <span class="TopRight" id="SpanFullDetail"></span>
</asp:Content>
