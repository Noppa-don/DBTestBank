<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="QuizLayoutConfirmed.aspx.vb" Inherits="QuickTest.QuizLayoutConfirmed" %>

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
            //$('.edit').click(function () {
            //    var questionid = $(this).attr('qid');
            //    var chkboxstate = $(this).attr('checked');
            //    alert('edit : ' + questionid + ' , checked : ' + chkboxstate);
            //});

            $('.technical').click(function () {
                var questionid = $(this).attr('qid');
                var chkboxstate = $(this).attr('checked');
                var ResultTick;
                if (chkboxstate == 'checked') {
                    ResultTick = true;
                }
                else {
                    ResultTick = false;
                }
                UpdateLayoutConfirmed('QuizTechnicalConfirmed', ResultTick, questionid)
            });
            $('.prepress').click(function () {
                var questionid = $(this).attr('qid');
                var chkboxstate = $(this).attr('checked');
                var ResultTick;
                if (chkboxstate == 'checked') {
                    ResultTick = true;
                }
                else {
                    ResultTick = false;
                }
                UpdateLayoutConfirmed('QuizPrePressConfirmed', ResultTick, questionid)
            });
            $('.auther').click(function () {
                var questionid = $(this).attr('qid');
                var chkboxstate = $(this).attr('checked');
                var ResultTick;
                if (chkboxstate == 'checked') {
                    ResultTick = true;
                }
                else {
                    ResultTick = false;
                }
                UpdateLayoutConfirmed('QuizAutherConfirmed', ResultTick, questionid)
            });
            $('.certify').click(function () {
                var questionid = $(this).attr('qid');
                var chkboxstate = $(this).attr('checked');
                var ResultTick;
                if (chkboxstate == 'checked') {
                    ResultTick = true;
                }
                else {
                    ResultTick = false;
                }
                UpdateLayoutConfirmed('QuizCertifyConfirmed', ResultTick, questionid)
            });
            $('.imgPreview').click(function () {
                var qsetid = $(this).attr('qsetid');
                var qid = $(this).attr('qid');
                var QNo = $(this).attr('QNo');
                var url = ResolveURL + 'Testset/PreviewAndEditExam.aspx?QSetId=' + qsetid + '&qid=' + qid + '&QNo=' + QNo;
                window.open(url, '_blank');
            });
        });
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
            background-color:#FFDDA9;
            }

            td:last-child {
                border-right: none;
            }
        .imgPreview {
        width:20px;
        height:20px;
        cursor:pointer;
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
                        <th style="width: 54%">ตัวอย่างคำถาม</th>
                        <th style="width: 9%">แก้/ดูแล้ว</th>
                        <th style="width: 9%">อนุมัติแล้ว</th>
                        <th style="width: 4%">P.P. อนุมัติ</th>
                        <th style="width: 4%">Auther อนุมัติ</th>
                        <th style="width: 4%">Certify อนุมัติ</th>
                        <th style="width: 4%">Preview</th>
                    </tr>
                    <asp:Repeater ID="rptListQuestion" runat="server">
                        <ItemTemplate>
                            <tr class="allowHover">
                                <td><%# Container.DataItem("Question_No").ToString() %>.</td>

                                <td style="text-align:left;"><%# clslayoutcheck.ReplaceModuleURL(Container.DataItem("Question_Name_Quiz"), Qset_Id)%></td>
                                <td>
                                    <%# clslayoutcheck.CheckThisCheckboxIsTick(Container.DataItem("QuizEditConfirmed"), QuickTest.ClsLayoutCheckConfirmed.ConfirmedType.Edit,Container.DataItem("Question_Id").ToString()) %>
                                </td>
                                <td>
                                    <%# clslayoutcheck.CheckThisCheckboxIsTick(Container.DataItem("QuizTechnicalConfirmed"), QuickTest.ClsLayoutCheckConfirmed.ConfirmedType.Technical, Container.DataItem("Question_Id").ToString())%>
                                </td>
                                <td>
                                    <%# clslayoutcheck.CheckThisCheckboxIsTick(Container.DataItem("QuizPrePressConfirmed"),QuickTest.ClsLayoutCheckConfirmed.ConfirmedType.PrePress,Container.DataItem("Question_Id").ToString()) %>
                                </td>
                                <td>
                                    <%# clslayoutcheck.CheckThisCheckboxIsTick(Container.DataItem("QuizAutherConfirmed"), QuickTest.ClsLayoutCheckConfirmed.ConfirmedType.Auther, Container.DataItem("Question_Id").ToString())%>
                                </td>
                                <td>
                                    <%# clslayoutcheck.CheckThisCheckboxIsTick(Container.DataItem("QuizCertifyConfirmed"), QuickTest.ClsLayoutCheckConfirmed.ConfirmedType.Certify, Container.DataItem("Question_Id").ToString())%>
                                </td>
                                <td>
                                    <%# clslayoutcheck.GenEditAndPreviewImg(Container.DataItem("Question_Id").ToString(), Qset_Id, Container.DataItem("Question_No").ToString(), "imgPreview", "preview") %>
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
