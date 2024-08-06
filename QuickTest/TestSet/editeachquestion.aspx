<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="editeachquestion.aspx.vb"
    Inherits="QuickTest.editeachquestion" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <script src="../js/jquery-1.7.1.js" type="text/javascript"></script>
    <script src="../js/jquery-ui-1.8.18.js" type="text/javascript"></script>
    <link href="../css/jquery-ui-1.8.18.custom.css" rel="stylesheet" type="text/css" />
    <script src="../js/CustomEditorControl.js" type="text/javascript"></script>
    <link rel="stylesheet" href="../css/jquery.button-audio-player.css" />
    <%--  <script type="text/javascript" src="../js/jquery.slim.min.js"></script>--%>
    <script type="text/javascript" src="../js/jquery.button-audio-player.js"></script>

    <telerik:RadScriptBlock ID="RadScriptBlock1" runat="server">
        <style type="text/css">
            .reTool .InsertFrontDoubleQuote {
                background-image: url(../images/Editor/extraBtnQuoteLeft.png) !important;
            }

            .reTool .InsertBackDoubleQuote {
                background-image: url(../images/Editor/extraBtnQuoteRight.png) !important;
            }

            .reTool .InsertDash {
                background-image: url(../images/Editor/extraBtnDash.png) !important;
            }

            .reTool .Sara01 {
                background-image: url(../images/Editor/01.png) !important;
            }

            .reTool .Sara02 {
                background-image: url(../images/Editor/02.jpg) !important;
            }

            .reTool .Sara03 {
                background-image: url(../images/Editor/03.jpg) !important;
            }

            .reTool .Sara04 {
                background-image: url(../images/Editor/04.jpg) !important;
            }

            .reTool .Sara05 {
                background-image: url(../images/Editor/05.jpg) !important;
            }

            .reTool .Sara06 {
                background-image: url(../images/Editor/06.jpg) !important;
            }

            .reTool .Sara07 {
                background-image: url(../images/Editor/07.jpg) !important;
            }

            .reTool .Sara08 {
                background-image: url(../images/Editor/08.jpg) !important;
            }

            .reTool .Sara10 {
                background-image: url(../images/Editor/10.jpg) !important;
            }

            .reTool .Sara12 {
                background-image: url(../images/Editor/12.jpg) !important;
            }

            .reTool .Sara15 {
                background-image: url(../images/Editor/15.jpg) !important;
                width: 45px;
            }

            .reTool .Sara19 {
                background-image: url(../images/Editor/19.jpg) !important;
            }

            .reTool {
                width: 45px !important;
            }


            /*.reModule {
                height: 0 !important;
            }*/

            .lblScore {
                padding-right: 8px;
            }

            .ClsScoreTxt {
                width: 35px;
                text-align: right;
            }
        </style>
        <script type="text/javascript">

            $(function () {

                var JVQuestionId = '<%=VBQuestionId %>';
                var JVQsetId = '<%=VBQsetId %>';
                var JVQsetType = '<%=VBQsetType%>';

                if (JVQsetType != 1) {
                    $('#tdScore').hide();
                }


                $('#CheckAllCopy').click(function () {
                    var master = this.checked;
                    $('#chkCopyThisQuestionToQuizMode').attr('checked', master);
                    $('#chkCopyThisQuestionExplainToQuizMode').attr('checked', master);

                    $('.AnswerToQuizMode').each(function () {
                        $(this).children().attr('checked', master);
                    });

                    $('.AnswerExplainToQuizMode').each(function () {
                        $(this).children().attr('checked', master);
                    });

                });

                $('#imgPrint').click(function () {
                    var JVQuestionId = '<%=VBQuestionId %>';
                    var JVQsetId = '<%=VBQsetId %>';
                    window.location = '../TestSet/PrintEditEachQuestion.aspx?QuestionId=' + JVQuestionId + '&QsetId=' + JVQsetId;

                });

                //เมื่อติ๊ก checkbox ที่ choice ต้องปลดทุกอันออกก่อนแล้วค่อยเลือกอันที่กดมาล่าสุด
                $('.chkShowLastRow').click(function () {
                    $('#chkNotAllowShuffleAnswer').attr('checked', false);
                    $('.chkShowLastRow input').attr('checked', false);
                    $(this).find('input').attr('checked', true);
                });

                //เมื่อติ๊กที่ checkbox ที่คำถาม ต้องปลดของ choice ออกให้หมด
                $('#chkNotAllowShuffleAnswer').click(function () {
                    $('.chkShowLastRow input').attr('checked', false);
                });


                var myVideo = document.getElementById("RadQuestion");
                if (myVideo.addEventListener) {
                    myVideo.addEventListener('contextmenu', function (e) {
                        e.preventDefault();
                    }, false);
                } else {
                    myVideo.attachEvent('oncontextmenu', function () {
                        window.event.returnValue = false;
                    });
                }

            });

            function DeleteAnswer(AnswerId) {

                var JVQuestionId = '<%=VBQuestionId %>';
                var JVQsetId = '<%=VBQsetId %>';
                var JVQsetType = '<%=VBQsetType%>';

                if (confirm('ต้องการลบคำตอบนี้ ?') == true) {
                    $.ajax({
                        type: "POST",
                        url: "<%=ResolveUrl("~")%>TestSet/editeachquestion.aspx/DeleteAnswer",
                        data: "{ AnswerId: '" + AnswerId + "',QuestionId:'" + JVQuestionId + "',QsetId:'" + JVQsetId + "',QsetType:" + JVQsetType + "}",
                        contentType: "application/json; charset=utf-8", dataType: "json",
                        success: function (msg) {
                            if (msg.d == 'Complete') {
                                window.location = '../TestSet/editeachquestion.aspx?qid=' + JVQuestionId + '&QsetId=' + JVQsetId;
                            }
                        },
                        error: function myfunction(request, status) {
                            alert('บางอย่างผิดพลาดโปรดลองอีกครั้ง')
                        }
                    });
                }
            }

            function ClosePage() {
                var JVQuestionId = '<%=VBQuestionId %>';
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

            }

            function OnClientPasteHtml(editor, args) {
                var JVAllowPasteHtml = '<%=AllowPasteHtml %>';
                if (JVAllowPasteHtml == "False"){
                    if (args.get_commandName() == "Paste") {
                        args.set_cancel(true);
                        alert('ไม่สามารถคัดลอกข้อความจากเอกสารอื่นมาวางได้นะคะ');
                    }               
                }
            }

        </script>
        <script type="text/javascript">

            function ResetContent(sender,cONnAME,RefId,RefType,MLevel) {
           
                var btnUpload = $(sender);
                var fileUpload = $(sender).prev().get(0);// $(sender).prev().get(0);
                var files = fileUpload.files;
                var fileName;

                var data = new FormData();
                for (var i = 0; i < files.length; i++) {
                    data.append(files[i].name, files[i]);
                    fileName = files[i].name;
                }

                if (RefId == ''){
                    RefId = '<%=Request.QueryString("qid").ToString()%>';
                }

                console.log(RefId);

                var cutFileName = fileName.replace('.mp3','');
                var saveFileResult = saveSoundFile(data,RefId,RefType,MLevel);

                if (saveFileResult != "False") {
                 
                    var setResult = setmu(fileName,cutFileName,saveFileResult,cONnAME,RefId);
                    if (setResult == "1") {
                        location.reload();
                    }
                    
                }else{
                    alert('เกิดข้อผิดพลาด');            
                }
            }

            var _storedRange = {};

            function storeRange(sender) {
                var editor = sender;//$find("<%=RadQuestion.ClientID%>");
                var selection = editor.getSelection();
                _storedRange = selection.getRange();
            }
  
            function saveSoundFile(data,RefId,RefType,MLevel) {
                var qsetId = '<%=Request.QueryString("QSetId").ToString()%>';
                var returnVal;

                $.ajax({
                    url: "../Upload/FileUploadHandler.ashx?qsetId=" + qsetId + '&RefId=' + RefId + '&RefType=' + RefType + '&MLevel=' + MLevel,
                    type: "POST",
                    data: data,
                    contentType: false,
                    processData: false,
                    async: false,
                    success: function (result) {
                        returnVal = result;
                    },
                    error: function (err) {
                        alert(err.statusText)
                    }
                });
                return returnVal;
            }

            function setmu(filename,divname,FilePath,cONnAME,RefId) {
                console.log(filename + ' ' +  divname + ' ' + FilePath + ' ' + cONnAME +  '' + RefId)

                $('#' + cONnAME).append("<br><div style='display:flex;justify-content: center;'><table><tr><td>" + filename + "</td><td><div id='" + divname + "'></td><td><img class='btn_Del' RefId='" + RefId + "' FileName='" + filename + "' style='cursor: pointer;' title='ลบ File' src='../Images/Delete-icon.png' /></td></tr></table></div>");   

                setbuttonAudioPlayer(divname,FilePath);
                return '1'
            }

            function setbuttonAudioPlayer(divname,FilePath) {
                $('#' + divname).buttonAudioPlayer({
                    type: 'default',
                    src: FilePath
                });

            } 
            $(function () {
                $('.btn_Del').click(function(){ 
                    var RefId = $(this).attr('RefId');
                    var filename =$(this).attr('filename');

                    $.ajax({
                        type: "POST",
                        url: "<%=ResolveUrl("~")%>TestSet/editeachquestion.aspx/DeleteMultiFile",
                        data: "{ RefId:'" + RefId + "' ,FileName:'" + filename + "' }",
                        contentType: "application/json; charset=utf-8", dataType: "json",
                        success: function (msg) {
                            if (msg.d == 'Complete') {
                                alert('ลบไฟล์เสียงเรียบร้อย');
                                location.reload();
                            }
                        },
                        error: function myfunction(request, status) {
                            alert('บางอย่างผิดพลาดโปรดลองอีกครั้ง')
                        }
                    });
                });
            });
           
        </script>
    </telerik:RadScriptBlock>

    <title>QuickTest - หน้าแก้ไข</title>
</head>
<body>

    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server"></telerik:RadScriptManager>

        <table id="tQuestion">
            <tr id="UpdatetoQuiz">
                <td colspan="2">
                    <input style="transform: scale(2); margin-left: 95px; margin-right: 15px;" type="checkbox" id="CheckAllCopy" />
                    <span style="font-size: 20px;">เลือกให้คำถาม คำตอบ และคำอธิบายทั้งหมดในข้อนี้ไปทับที่โหมด Quiz ด้วย</span>
                </td>
            </tr>
            <tr id="ImageWarning">
                <td colspan="2">
                    <span style="font-size: 20px; color: darkred;">
                        <br />
                        <br />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;ขนาดภาพที่เหมาะสมในการวางในข้อสอบคือ Width 150px , Height 150px หรือถ้าต้องการใส่รูปขนาดอื่น อย่าลืมตรวจสอบตอนออกใบงานและหน้าทำควิซนะคะ</span>
                </td>
            </tr>

            <tr id="QuestionHeaderAndEditor">
                <td>
                    <b>
                        <asp:Label ID="lblWarn" Font-Size="X-Large" runat="server" Visible="false" ForeColor="Red"></asp:Label></b>
                    <br />
                    <br />
                    <asp:Label ID="lblQuestion" runat="server" Text="คำถาม" Style="font-weight: 700; text-decoration: underline; font-size: x-large"></asp:Label>

                    <asp:CheckBox ID="chkNotAllowShuffleAnswer" ClientIDMode="Static" Visible="false" Style="margin-left: 10px;" ForeColor="Red" Font-Size="15px" runat="server" Text="ข้อนี้ห้ามสลับตัวเลือก(ตัวเลือกจะแสดงเรียงลำดับ ก. ข. ค. ง. ... ตามลำดับนี้เท่านั้น)" />
                    <br />
                    <asp:CheckBox ID="chkCopyThisQuestionToQuizMode" ClientIDMode="Static" Style="margin-left: 88px;" Font-Size="15px" runat="server" Text="เลือกให้คำถามนี้ไปทับที่โหมด Quiz ด้วย" />
                    <br />
                    <br />
                    <asp:Button ID="btnOpenVowel" runat="server" Text="เปิดปุ่มเพิ่มสระ" Style="height: 26px" />
                    <br />
                    <telerik:RadEditor ID="RadQuestion" ClientIDMode="Static" runat="server" Height="220px" OnClientLoad="TelerikDemo.editor_onClientLoad" OnClientPasteHtml="OnClientPasteHtml"
                        AutoResizeHeight="True">
                        <Modules>
                            <telerik:EditorModule Name="RadEditorNodeInspector" />
                        </Modules>
                        <Tools>
                            <telerik:EditorToolGroup>
                                <telerik:EditorSplitButton Name="Undo"></telerik:EditorSplitButton>
                                <telerik:EditorSplitButton Name="Redo"></telerik:EditorSplitButton>
                            </telerik:EditorToolGroup>
                            <telerik:EditorToolGroup>
                                <telerik:EditorTool Name="Bold" />
                                <telerik:EditorTool Name="Italic" />
                                <telerik:EditorTool Name="Underline" />
                                <telerik:EditorTool Name="ImageManager" ShortCut="CTRL+G" />
                                <telerik:EditorSplitButton Name="ForeColor"></telerik:EditorSplitButton>
                                <telerik:EditorTool Name="Subscript" />
                                <telerik:EditorTool Name="Superscript" />
                                <telerik:EditorTool Name="InsertFrontDoubleQuote"></telerik:EditorTool>
                                <telerik:EditorTool Name="InsertBackDoubleQuote"></telerik:EditorTool>
                                <telerik:EditorTool Name="InsertDash"></telerik:EditorTool>
                            </telerik:EditorToolGroup>


                        </Tools>
                        <Content>
                        </Content>

                    </telerik:RadEditor>
                </td>
                <td style="padding-left: 10px;">
                    <asp:Label ID="Label1" runat="server" Text="อธิบายคำถาม"
                        Style="font-weight: 700; text-decoration: underline; font-size: x-large"></asp:Label>
                    <br />
                    <br />
                    <asp:CheckBox ID="chkCopyThisQuestionExplainToQuizMode" ClientIDMode="Static" Font-Size="15px" runat="server" Text="เลือกให้อธิบายคำถามนี้ไปทับที่โหมด Quiz ด้วย" />
                    <br />
                    <br />
                    <telerik:RadEditor ID="RadQuestionExplain" runat="server" Height="220px"
                        Width="680px" AutoResizeHeight="True" OnClientLoad="TelerikDemo.editor_onClientLoad" OnClientPasteHtml="OnClientPasteHtml">
                        <Modules>
                            <telerik:EditorModule Name="RadEditorNodeInspector" />
                        </Modules>
                        <Tools>
                            <telerik:EditorToolGroup>
                                <telerik:EditorSplitButton Name="Undo">
                                </telerik:EditorSplitButton>
                                <telerik:EditorSplitButton Name="Redo">
                                </telerik:EditorSplitButton>
                                <telerik:EditorTool Name="Cut" />
                                <telerik:EditorTool Name="Copy" />
                                <telerik:EditorTool Name="Paste" ShortCut="CTRL+V" />
                            </telerik:EditorToolGroup>
                            <telerik:EditorToolGroup>
                                <telerik:EditorTool Name="Bold" />
                                <telerik:EditorTool Name="Italic" />
                                <telerik:EditorTool Name="Underline" />
                                <telerik:EditorTool Name="ImageManager" ShortCut="CTRL+G" />
                                <telerik:EditorSeparator />
                                <telerik:EditorSplitButton Name="ForeColor">
                                </telerik:EditorSplitButton>
                                <telerik:EditorSeparator />
                                <%--<telerik:EditorDropDown Name="FontName">
                                </telerik:EditorDropDown>
                                <telerik:EditorDropDown Name="FontSize" Width="80px">
                                </telerik:EditorDropDown>--%>
                                <telerik:EditorTool Name="Subscript" />
                                <telerik:EditorTool Name="Superscript" />
                                <telerik:EditorTool Name="InsertFrontDoubleQuote" />
                                <telerik:EditorTool Name="InsertBackDoubleQuote" />
                                <telerik:EditorTool Name="InsertDash"></telerik:EditorTool>
                            </telerik:EditorToolGroup>
                        </Tools>
                        <Content>
                        </Content>
                    </telerik:RadEditor>
                </td>
            </tr>
            <tr id="QuestionMultiFile">
                <td>
                    <div id="divQMulti" style="border: 1px solid orange; padding: 5px; text-align: center; margin-top: 40px;" runat="server">
                        <span style="font-weight: bold; text-decoration: underline;">เพิ่มไฟล์เสียง</span><br />
                        <br />
                        <input type="file" accept=".mp4,.mp3,.wav" />
                        <input type="button" value="Upload" onclick="ResetContent(this,'divQMulti','','1','1');" />
                    </div>
                </td>
                <td>
                    <div id="divQMultiExplain" style="border: 1px solid orange; padding: 5px; text-align: center; margin-top: 40px;" runat="server">
                        <span style="font-weight: bold; text-decoration: underline;">เพิ่มไฟล์เสียง</span><br />
                        <br />
                        <input type="file" accept=".mp4,.mp3,.wav" />
                        <input type="button" value="Upload" onclick="ResetContent(this,'divQMultiExplain','','2','1');" />
                    </div>
                </td>
            </tr>
            <tr id="QuestionMultiFileSlow">
                <td>
                    <div id="divQMultiSlow" style="border: 1px solid orange; padding: 5px; text-align: center; margin-top: 40px;" runat="server">
                        <span style="font-weight: bold; text-decoration: underline;">เพิ่มไฟล์เสียงแบบ Slow</span><br />
                        <br />
                        <input type="file" accept=".mp4,.mp3,.wav" />
                        <input type="button" value="Upload" onclick="ResetContent(this,'divQMultiSlow','','1','2');" />
                    </div>

                </td>
                <td>
                    <div id="divQMultiExplainSlow" style="border: 1px solid orange; padding: 5px; text-align: center; margin-top: 40px;" runat="server">
                        <span style="font-weight: bold; text-decoration: underline;">เพิ่มไฟล์เสียงแบบ Slow</span><br />
                        <br />
                        <input type="file" accept=".mp4,.mp3,.wav" />
                        <input type="button" value="Upload" onclick="ResetContent(this,'divQMultiExplainSlow','','2','2');" />
                    </div>
                </td>
            </tr>
            <tr id="QuestionMultiFiletxt">
                <td>
                    <br />
                    <span style="font-weight: bold; text-decoration: underline;">เพิ่มคำอ่านตามไฟล์เสียง</span><br />
                    <br />
                    <telerik:RadEditor ID="RadQMultiTxt" ClientIDMode="Static" runat="server" Height="220px" OnClientLoad="TelerikDemo.editor_onClientLoad" OnClientPasteHtml="OnClientPasteHtml"
                        AutoResizeHeight="True">
                        <Modules>
                            <telerik:EditorModule Name="RadEditorNodeInspector" />
                        </Modules>
                        <Tools>
                            <telerik:EditorToolGroup>
                                <telerik:EditorSplitButton Name="Undo"></telerik:EditorSplitButton>
                                <telerik:EditorSplitButton Name="Redo"></telerik:EditorSplitButton>
                            </telerik:EditorToolGroup>
                            <telerik:EditorToolGroup>
                                <telerik:EditorTool Name="Bold" />
                                <telerik:EditorTool Name="Italic" />
                                <telerik:EditorTool Name="Underline" />
                                <telerik:EditorTool Name="ImageManager" ShortCut="CTRL+G" />
                                <telerik:EditorSplitButton Name="ForeColor"></telerik:EditorSplitButton>
                                <telerik:EditorTool Name="Subscript" />
                                <telerik:EditorTool Name="Superscript" />
                                <telerik:EditorTool Name="InsertFrontDoubleQuote"></telerik:EditorTool>
                                <telerik:EditorTool Name="InsertBackDoubleQuote"></telerik:EditorTool>
                                <telerik:EditorTool Name="InsertDash"></telerik:EditorTool>
                            </telerik:EditorToolGroup>
                        </Tools>
                        <Content>
                        </Content>

                    </telerik:RadEditor>
                </td>
                <td>
                    <br />
                    <span style="font-weight: bold; text-decoration: underline;">เพิ่มคำอ่านตามไฟล์เสียง</span><br />
                    <br />
                    <telerik:RadEditor ID="RadQMultiExplainTxt" ClientIDMode="Static" runat="server" Height="220px" OnClientLoad="TelerikDemo.editor_onClientLoad" OnClientPasteHtml="OnClientPasteHtml"
                        AutoResizeHeight="True">
                        <Modules>
                            <telerik:EditorModule Name="RadEditorNodeInspector" />
                        </Modules>
                        <Tools>
                            <telerik:EditorToolGroup>
                                <telerik:EditorSplitButton Name="Undo"></telerik:EditorSplitButton>
                                <telerik:EditorSplitButton Name="Redo"></telerik:EditorSplitButton>
                            </telerik:EditorToolGroup>
                            <telerik:EditorToolGroup>
                                <telerik:EditorTool Name="Bold" />
                                <telerik:EditorTool Name="Italic" />
                                <telerik:EditorTool Name="Underline" />
                                <telerik:EditorTool Name="ImageManager" ShortCut="CTRL+G" />
                                <telerik:EditorSplitButton Name="ForeColor"></telerik:EditorSplitButton>
                                <telerik:EditorTool Name="Subscript" />
                                <telerik:EditorTool Name="Superscript" />
                                <telerik:EditorTool Name="InsertFrontDoubleQuote"></telerik:EditorTool>
                                <telerik:EditorTool Name="InsertBackDoubleQuote"></telerik:EditorTool>
                                <telerik:EditorTool Name="InsertDash"></telerik:EditorTool>
                            </telerik:EditorToolGroup>


                        </Tools>
                        <Content>
                        </Content>

                    </telerik:RadEditor>

                </td>
            </tr>

        </table>
        <br /><br />

        <table id="tAnswer">
            <tr>
                <td style='width: 680px'>
                    <asp:Label ID="lblAnswer" runat="server" Text="คำตอบ"
                        Style="font-weight: 700; text-decoration: underline; font-size: x-large"></asp:Label>
                </td>
                <td style='padding-left: 10px'>
                    <asp:Label ID="lblAnswerExplain" runat="server" Text="อธิบายคำตอบ"
                        Style="font-weight: 700; text-decoration: underline; font-size: x-large"></asp:Label>
                </td>
            </tr>
        </table>
        <br />
        <asp:Repeater ID="RptCreateAnswer" runat="server">
            <ItemTemplate>
                <table>
                    <tr>
                        <td>
                            <strong>คำตอบข้อที่ <i><%# Container.DataItem("Answer_No")%></i></strong>
                            <br />
                            <br />
                            <img answerid="<%# Container.DataItem("Answer_Id")%>" id='btn_Del_<%# Container.DataItem("Answer_Id")%>' style='cursor: pointer;' title='ลบคำตอบ' src="../Images/Delete-icon.png" onclick='DeleteAnswer("<%# Container.DataItem("Answer_Id")%>")' />
                            <br />
                            <asp:Label ID="lblChooseThisAnswerIsCorrect" Font-Bold="true" runat="server" Text="เลือกให้เป็นคำตอบที่ถูกต้อง"></asp:Label>
                            <%--<b>เลือกให้เป็นคำตอบที่ถูกต้อง</b>--%>
                            <asp:CheckBox ID="CheckScore" CssClass="ChkScore" ClientIDMode="Static" runat="server" />
                            <br />
                            <asp:CheckBox ID="CheckThisAnswerToShowInLastRow" Visible="false" CssClass="chkShowLastRow" runat="server" Text="แสดงตัวเลือกนี้ในตำแหน่งสุดท้ายเท่านั้น(เช่น ถูกทุกข้อ , ผิดทุกข้อ)" Font-Size="15px" ForeColor="Red" />
                            <br />
                        </td>
                        <td></td>
                    </tr>
                    <%--<strong>คำตอบข้อที่ <i><%# AnswerNumber()%></i></strong>--%>
                </table>
                <table>
                    <tr>
                        <td>
                            <asp:CheckBox ID="chkCopyThisAnswerToQuizMode" CssClass="AnswerToQuizMode" runat="server" Text="เลือกให้คำตอบนี้ไปทับที่โหมด Quiz ด้วย" Font-Size="15px" />
                        </td>
                        <td id="tdScore" style="padding-left: 315px;">
                            <asp:Label ID="lblScore" runat="server" Text="คะแนน" CssClass="lblScore"></asp:Label>
                            <asp:TextBox ID="txtScore" runat="server" Text="0" CssClass="ClsScoreTxt"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <br />
                <br />
                <table>
                    <tr>
                        <td>
                            <telerik:RadEditor ID="RadAnswer" runat="server" Height="220px" Width="680px" EditModes="All" AutoResizeHeight="True"
                                OnClientLoad="TelerikDemo.editor_onClientLoad" OnClientPasteHtml="OnClientPasteHtml">
                                <Tools>
                                    <telerik:EditorToolGroup Tag="MainToolbar">
                                        <telerik:EditorSplitButton Name="Undo">
                                        </telerik:EditorSplitButton>
                                        <telerik:EditorSplitButton Name="Redo">
                                        </telerik:EditorSplitButton>
                                        <telerik:EditorTool Name="Cut" />
                                        <telerik:EditorTool Name="Copy" />
                                        <telerik:EditorTool Name="Paste" ShortCut="CTRL+V" />
                                    </telerik:EditorToolGroup>
                                    <telerik:EditorToolGroup Tag="Formatting">
                                        <telerik:EditorTool Name="Bold" />
                                        <telerik:EditorTool Name="Italic" />
                                        <telerik:EditorTool Name="Underline" />
                                        <telerik:EditorTool Name="ImageManager" ShortCut="CTRL+G" />
                                        <telerik:EditorSeparator />
                                        <telerik:EditorSplitButton Name="ForeColor">
                                        </telerik:EditorSplitButton>
                                        <telerik:EditorSeparator />
                                        <%--<telerik:EditorDropDown Name="FontName">
                                        </telerik:EditorDropDown>
                                        <telerik:EditorDropDown Name="FontSize" Width="80px">
                                        </telerik:EditorDropDown>--%>
                                        <telerik:EditorTool Name="Subscript" />
                                        <telerik:EditorTool Name="Superscript" />
                                        <telerik:EditorTool Name="InsertFrontDoubleQuote" />
                                        <telerik:EditorTool Name="InsertBackDoubleQuote" />
                                        <telerik:EditorTool Name="InsertDash"></telerik:EditorTool>
                                    </telerik:EditorToolGroup>
                                </Tools>
                                <Content>
                                </Content>
                            </telerik:RadEditor>
                        </td>
                        <td style="padding-left: 10px;">
                            <asp:CheckBox ID="chkCopyThisAnswerExplainToQuizMode" CssClass="AnswerExplainToQuizMode" runat="server" Text="เลือกให้คำอธิบายคำตอบนี้ไปทับที่โหมด Quiz ด้วย" Font-Size="15px" />
                            <br />
                            <telerik:RadEditor ID="RadAnswerExplain" runat="server" Height="220px" Width="680px" EditModes="All" AutoResizeHeight="True"
                                OnClientLoad="TelerikDemo.editor_onClientLoad" OnClientPasteHtml="OnClientPasteHtml">
                                <Tools>
                                    <telerik:EditorToolGroup Tag="MainToolbar">
                                        <telerik:EditorSplitButton Name="Undo">
                                        </telerik:EditorSplitButton>
                                        <telerik:EditorSplitButton Name="Redo">
                                        </telerik:EditorSplitButton>
                                        <telerik:EditorTool Name="Cut" />
                                        <telerik:EditorTool Name="Copy" />
                                        <telerik:EditorTool Name="Paste" ShortCut="CTRL+V" />
                                    </telerik:EditorToolGroup>
                                    <telerik:EditorToolGroup Tag="Formatting">
                                        <telerik:EditorTool Name="Bold" />
                                        <telerik:EditorTool Name="Italic" />
                                        <telerik:EditorTool Name="Underline" />
                                        <telerik:EditorTool Name="ImageManager" ShortCut="CTRL+G" />
                                        <telerik:EditorSeparator />
                                        <telerik:EditorSplitButton Name="ForeColor">
                                        </telerik:EditorSplitButton>
                                        <telerik:EditorSeparator />
                                        <%--                                        <telerik:EditorDropDown Name="FontName">
                                        </telerik:EditorDropDown>
                                        <telerik:EditorDropDown Name="FontSize" Width="80px">
                                        </telerik:EditorDropDown>--%>
                                        <telerik:EditorTool Name="Subscript" />
                                        <telerik:EditorTool Name="Superscript" />
                                        <telerik:EditorTool Name="InsertFrontDoubleQuote" />
                                        <telerik:EditorTool Name="InsertBackDoubleQuote" />
                                        <telerik:EditorTool Name="InsertDash"></telerik:EditorTool>
                                    </telerik:EditorToolGroup>
                                </Tools>
                                <Content>
                                </Content>
                            </telerik:RadEditor>

                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div id="divAMulti<%# Container.DataItem("Answer_Id")%>" style="border: 1px solid orange; padding: 5px; text-align: center; margin-top: 40px;">
                                <span style="font-weight: bold; text-decoration: underline;">เพิ่มไฟล์เสียง</span><br />
                                <input type="file" accept=".mp4,.mp3,.wav" />
                                <input type="button" value="Upload" onclick="ResetContent(this,'divAMulti<%# Container.DataItem("Answer_Id")%>    ','<%# Container.DataItem("Answer_Id")%>    ','1','1');" />
                            </div>

                        </td>
                        <td>
                            <div id="divAMultiExplain<%# Container.DataItem("Answer_Id")%>" style="border: 1px solid orange; padding: 5px; text-align: center; margin-top: 40px;">
                                <span style="font-weight: bold; text-decoration: underline;">เพิ่มไฟล์เสียง</span><br />
                                <input type="file" accept=".mp4,.mp3,.wav" />
                                <input type="button" value="Upload" onclick="ResetContent(this,'divAMultiExplain<%# Container.DataItem("Answer_Id")%>    ','<%# Container.DataItem("Answer_Id")%>    ','2','1');" />
                            </div>
                             </td>
                    </tr>
                    <tr>
                        <td>
                            <div id="divAMultiSlow<%# Container.DataItem("Answer_Id")%>" style="border: 1px solid orange; padding: 5px; text-align: center; margin-top: 40px;">
                                <span style="font-weight: bold; text-decoration: underline;">เพิ่มไฟล์เสียงแบบ Slow</span><br />
                                <input type="file" accept=".mp4,.mp3,.wav" />
                                <input type="button" value="Upload" onclick="ResetContent(this,'divAMultiSlow<%# Container.DataItem("Answer_Id")%>    ','<%# Container.DataItem("Answer_Id")%>    ','1','2');" />
                            </div>
                        </td>
                        <td>
                            <div id="divAMultiExplainSlow<%# Container.DataItem("Answer_Id")%>" style="border: 1px solid orange; padding: 5px; text-align: center; margin-top: 40px;">
                                <span style="font-weight: bold; text-decoration: underline;">เพิ่มไฟล์เสียงแบบ Slow</span><br />
                                <input type="file" accept=".mp4,.mp3,.wav" />
                                <input type="button" value="Upload" onclick="ResetContent(this,'divAMultiExplainSlow<%# Container.DataItem("Answer_Id")%>    ','<%# Container.DataItem("Answer_Id")%>    ','2','2');" />
                            </div>
                        </td>
                    </tr>
              <%--      <tr>
                        <td>
                            <br />
                            <span style="font-weight: bold; text-decoration: underline;">เพิ่มคำอ่านตามไฟล์เสียง</span><br />
                            <br />
                            <telerik:RadEditor ID="RadAMultiTxt" ClientIDMode="Static" runat="server" Height="220px" OnClientLoad="TelerikDemo.editor_onClientLoad" OnClientPasteHtml="OnClientPasteHtml"
                                AutoResizeHeight="True">
                                <Modules>
                                    <telerik:EditorModule Name="RadEditorNodeInspector" />
                                </Modules>
                                <Tools>
                                    <telerik:EditorToolGroup>
                                        <telerik:EditorSplitButton Name="Undo"></telerik:EditorSplitButton>
                                        <telerik:EditorSplitButton Name="Redo"></telerik:EditorSplitButton>
                                    </telerik:EditorToolGroup>
                                    <telerik:EditorToolGroup>
                                        <telerik:EditorTool Name="Bold" />
                                        <telerik:EditorTool Name="Italic" />
                                        <telerik:EditorTool Name="Underline" />
                                        <telerik:EditorTool Name="ImageManager" ShortCut="CTRL+G" />
                                        <telerik:EditorSplitButton Name="ForeColor"></telerik:EditorSplitButton>
                                        <telerik:EditorTool Name="Subscript" />
                                        <telerik:EditorTool Name="Superscript" />
                                        <telerik:EditorTool Name="InsertFrontDoubleQuote"></telerik:EditorTool>
                                        <telerik:EditorTool Name="InsertBackDoubleQuote"></telerik:EditorTool>
                                        <telerik:EditorTool Name="InsertDash"></telerik:EditorTool>
                                    </telerik:EditorToolGroup>
                                </Tools>
                                <Content>
                                </Content>

                            </telerik:RadEditor>
                        </td>
                        <td>
                            <br />
                            <span style="font-weight: bold; text-decoration: underline;">เพิ่มคำอ่านตามไฟล์เสียง</span><br />
                            <br />
                            <telerik:RadEditor ID="RadAMultiExplainTxt" ClientIDMode="Static" runat="server" Height="220px" OnClientLoad="TelerikDemo.editor_onClientLoad" OnClientPasteHtml="OnClientPasteHtml"
                                AutoResizeHeight="True">
                                <Modules>
                                    <telerik:EditorModule Name="RadEditorNodeInspector" />
                                </Modules>
                                <Tools>
                                    <telerik:EditorToolGroup>
                                        <telerik:EditorSplitButton Name="Undo"></telerik:EditorSplitButton>
                                        <telerik:EditorSplitButton Name="Redo"></telerik:EditorSplitButton>
                                    </telerik:EditorToolGroup>
                                    <telerik:EditorToolGroup>
                                        <telerik:EditorTool Name="Bold" />
                                        <telerik:EditorTool Name="Italic" />
                                        <telerik:EditorTool Name="Underline" />
                                        <telerik:EditorTool Name="ImageManager" ShortCut="CTRL+G" />
                                        <telerik:EditorSplitButton Name="ForeColor"></telerik:EditorSplitButton>
                                        <telerik:EditorTool Name="Subscript" />
                                        <telerik:EditorTool Name="Superscript" />
                                        <telerik:EditorTool Name="InsertFrontDoubleQuote"></telerik:EditorTool>
                                        <telerik:EditorTool Name="InsertBackDoubleQuote"></telerik:EditorTool>
                                        <telerik:EditorTool Name="InsertDash"></telerik:EditorTool>
                                    </telerik:EditorToolGroup>
                                </Tools>
                                <Content>
                                </Content>

                            </telerik:RadEditor>
                        </td>
                    </tr>--%>
                </table>
                <hr />
                <br />
            </ItemTemplate>
        </asp:Repeater>

        <div id='DivRadio' runat="server" visible="false">
            <table>
                <tr>
                    <td>
                        <div style='padding-bottom: 10px'>
                            <asp:RadioButton ID="RadioTrue" GroupName="TrueFalse" runat="server" Text="ถูก(True)"></asp:RadioButton>
                        </div>
                        <p style="padding-bottom: 2px"><b><u>อธิบายคำตอบ</u></b></p>
                        <br />
                        <asp:CheckBox ID="chkCopyThisAnswerExplainToQuizModeTrue" runat="server" Text="เลือกให้คำอธิบายคำตอบนี้ไปทับที่โหมด Quiz ด้วย" Font-Size="15px" />
                        <telerik:RadEditor ID="RadAnswerExplainTrue" runat="server" Height="220px" Width="680px" OnClientLoad="TelerikDemo.editor_onClientLoad"
                            OnClientPasteHtml="OnClientPasteHtml" AutoResizeHeight="True">

                            <Modules>
                                <telerik:EditorModule Name="RadEditorNodeInspector" />
                            </Modules>

                            <Tools>
                                <telerik:EditorToolGroup Tag="MainToolbar">
                                    <telerik:EditorSplitButton Name="Undo">
                                    </telerik:EditorSplitButton>

                                    <telerik:EditorSplitButton Name="Redo">
                                    </telerik:EditorSplitButton>
                                    <telerik:EditorTool Name="Cut" />
                                    <telerik:EditorTool Name="Copy" />
                                    <telerik:EditorTool Name="Paste" ShortCut="CTRL+V" />
                                </telerik:EditorToolGroup>
                                <telerik:EditorToolGroup Tag="Formatting">
                                    <telerik:EditorTool Name="Bold" />
                                    <telerik:EditorTool Name="Italic" />
                                    <telerik:EditorTool Name="Underline" />
                                    <telerik:EditorTool Name="ImageManager" ShortCut="CTRL+G" />
                                    <telerik:EditorSeparator />
                                    <telerik:EditorSplitButton Name="ForeColor">
                                    </telerik:EditorSplitButton>
                                    <telerik:EditorSeparator />
                                    <%--                                    <telerik:EditorDropDown Name="FontName">
                                    </telerik:EditorDropDown>
                                    <telerik:EditorDropDown Name="FontSize" Width="80px">
                                    </telerik:EditorDropDown>--%>
                                    <telerik:EditorTool Name="Subscript" />
                                    <telerik:EditorTool Name="Superscript" />
                                    <telerik:EditorTool Name="InsertFrontDoubleQuote" />
                                    <telerik:EditorTool Name="InsertBackDoubleQuote" />
                                    <telerik:EditorTool Name="InsertDash"></telerik:EditorTool>
                                </telerik:EditorToolGroup>
                            </Tools>
                            <Content>
                    

                            </Content>

                            <%--<TrackChangesSettings CanAcceptTrackChanges="False"></TrackChangesSettings>--%>
                        </telerik:RadEditor>

                    </td>
                    <td style='padding-left: 10px'>
                        <div style='padding-bottom: 10px'>
                            <asp:RadioButton ID="RadioFalse" GroupName="TrueFalse" runat="server" Text="ผิด(False)"></asp:RadioButton>
                        </div>
                        <p style="padding-bottom: 2px"><b><u>อธิบายคำตอบ</u></b></p>
                        <br />
                        <asp:CheckBox ID="chkCopyThisAnswerExplainToQuizModeFalse" runat="server" Text="เลือกให้คำอธิบายคำตอบนี้ไปทับที่โหมด Quiz ด้วย" Font-Size="15px" />
                        <telerik:RadEditor ID="RadAnswerExplainFalse" runat="server" Height="220px" Width="680px" OnClientLoad="TelerikDemo.editor_onClientLoad" OnClientPasteHtml="OnClientPasteHtml"
                            AutoResizeHeight="True">
                            <Modules>
                                <telerik:EditorModule Name="RadEditorNodeInspector" />
                            </Modules>
                            <Tools>
                                <telerik:EditorToolGroup Tag="MainToolbar">
                                    <telerik:EditorSplitButton Name="Undo">
                                    </telerik:EditorSplitButton>
                                    <telerik:EditorSplitButton Name="Redo">
                                    </telerik:EditorSplitButton>
                                    <telerik:EditorTool Name="Cut" />
                                    <telerik:EditorTool Name="Copy" />
                                    <telerik:EditorTool Name="Paste" ShortCut="CTRL+V" />
                                </telerik:EditorToolGroup>
                                <telerik:EditorToolGroup Tag="Formatting">
                                    <telerik:EditorTool Name="Bold" />
                                    <telerik:EditorTool Name="Italic" />
                                    <telerik:EditorTool Name="Underline" />
                                    <telerik:EditorTool Name="ImageManager" ShortCut="CTRL+G" />
                                    <telerik:EditorSeparator />
                                    <telerik:EditorSplitButton Name="ForeColor">
                                    </telerik:EditorSplitButton>
                                    <telerik:EditorSeparator />
                                    <%--                                    <telerik:EditorDropDown Name="FontName">
                                    </telerik:EditorDropDown>
                                    <telerik:EditorDropDown Name="FontSize" Width="80px">
                                    </telerik:EditorDropDown>--%>
                                    <telerik:EditorTool Name="Subscript" />
                                    <telerik:EditorTool Name="Superscript" />
                                    <telerik:EditorTool Name="InsertFrontDoubleQuote" />
                                    <telerik:EditorTool Name="InsertBackDoubleQuote" />
                                    <telerik:EditorTool Name="InsertDash"></telerik:EditorTool>
                                </telerik:EditorToolGroup>
                            </Tools>
                            <Content>
                    
                            </Content>
                        </telerik:RadEditor>
                    </td>
                </tr>
            </table>
            <br />
        </div>

        <asp:ImageButton ID="btnAddNewAnswer" ImageUrl="~/Images/New.png" ToolTip='เพิ่มคำตอบ' OnClientClick="return confirm('ต้องการเพิ่มคำตอบ ?');" runat="server" />
        <img id='imgPrint' style='float: right; cursor: pointer;' src="../Images/another_page.png" />
        <br />
        <br />
        <asp:Button ID="btnSave" runat="server" OnClientClick="return confirm('ต้องการแก้ไขข้อนี้ใช่หรือไม่ ?');" Text="บันทึก" Style="height: 26px" />
        &nbsp;<input type="button" onclick='ClosePage()' value='ยกเลิก' />
        <br />

    </form>

</body>
</html>
