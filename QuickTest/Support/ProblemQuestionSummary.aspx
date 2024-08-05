<%@ Page Language="vb" AutoEventWireup="false" EnableEventValidation="False" CodeBehind="ProblemQuestionSummary.aspx.vb" Inherits="QuickTest.ProblemQuestionSummary" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="../shadowbox/shadowbox.css" />
    <link rel="stylesheet" type="text/css" href="../css/jquery-ui-1.8.18.custom.min.css" />
    <style type="text/css">
        h1 {
            text-align: center;
            color: #FFFFFF;
        }

        .btnActive {
            height: 40px;
            font-size: 20px;
            background-color: burlywood;
            cursor: pointer;
        }

        .btnUnActive {
            height: 40px;
            font-size: 20px;
            background-color: white;
            cursor: pointer;
        }

        .SumText {
            text-align: right;
            padding-right: 30px;
            font-size: 20px;
            padding-bottom: 15px;
        }

        .ui-widget-header {
            border: 1px solid #0c4649;
            background: #53b6bb;
            color: #FFFFFF;
            font-weight: bold;
        }

        .btnEdited, .btnReturn, .btnApprove {
            height: 30px;
            border-radius: 10px;
        }

        #dialogDetail {
            display: none;
            position: absolute;
            z-index: 50;
            outline: 0px;
            height: auto;
            width: 500px;
            max-height: 700px;
            top: 10%;
            left: 35%;
            background-color: whitesmoke;
            border-radius: 10px;
            padding: 22px;
        }

        .tdHeader {
            border: 1px solid #0c4649;
            background: #53b6bb;
            color: #FFFFFF;
            font-weight: bold;
            border-radius: 10px;
            padding: 15px;
            font: normal 18px 'THSarabunNew';
            width: 115%;
        }

        .dll {
            height: 30px;
            width: 100px;
            border-radius: 8px;
            font-size: 16px;
            padding-left: 5px;
            margin-right: 10px;
            margin-left: 5px;
            padding-right: 5px;
        }
    </style>

    <script type="text/javascript" src="../js/jquery-1.7.1.js"></script>
    <script type="text/javascript" src="../js/jquery.blockUI.js"></script>
    <script type="text/javascript" src="../js/jquery-ui-1.8.18.min.js"></script>
    <script type="text/javascript" src="../js/GFB.js"></script>
    <script type="text/javascript" src="../js/jquery.blockUI.js"></script>
    <script type="text/javascript">

        $(function () {
            var selector;
            var problemType;
            var remarkDetail;

            $('.btnDetail').click(function () {
                selector = $(this).attr('QPId');
                $.ajax({
                    type: "POST",
                    url: "../Support/ProblemQuestionSummary.aspx/GetHistory",
                    data: "{QPId : '" + selector + "' }",
                    async: false,
                    contentType: "application/json; charset=utf-8", dataType: "json",
                    success: function (data) {
                        $("#divDetail").html(data.d);
                    },
                    error: function myfunction(request, status) {
                        alert(status);
                    }
                });

                //$('#dialogDetail').dialog('option', 'title', 'ประวัติการแก้ไข').dialog("open");
                $('#MyShadow').show();
                $('#dialogDetail').show();

            });

            $('.btnEdited').click(function () {
                selector = $(this).attr('QPId');
                problemType = 2;
                $('#txtRemark').val("แก้ไขปัญหาเรียบร้อย");
                $('#dialogRemark').dialog('option', 'title', 'หมายเหตุ').dialog("open");
            });

            $('.btnReturn').click(function () {
                selector = $(this).attr('QPId');
                problemType = 3
                $('#dialogRemark').dialog('option', 'title', 'กรุณาระบุสาเหตุ').dialog("open");
            });

            $('.btnClose').click(function () {
                $('#MyShadow').hide();
                $('#dialogDetail').hide();
            });


            $('.btnApprove').click(function () {
                selector = $(this).attr('QPId');
                problemType = 4
                remarkDetail = '';
                var QStr = window.location.search.substring(1);
                var EachQStr = QStr.split("=");
                var UsId = EachQStr[2];
                $.ajax({
                    type: "POST",
                    url: "<%=ResolveUrl("~")%>Support/ProblemQuestionSummary.aspx/UpdateProblem",
                    data: "{QPId : '" + selector + "',RemarkDetail : '" + remarkDetail + "',ProblemType : '" + problemType + "',UsId : '" + UsId + "' }",
                    async: false,
                    contentType: "application/json; charset=utf-8", dataType: "json",
                    success: function (data) {
                        if (data.d == true) {
                            $('#dialogAlert').dialog('option', 'title', 'บันทึกข้อมูลเรียบร้อยค่ะ').dialog("open");
                        }

                    },
                    error: function myfunction(request, status) {
                        alert(status);
                    }
                });
            });

            $('#dialogRemark').dialog({
                autoOpen: false,
                buttons: {
                    'ยกเลิก': function () {
                        $(this).dialog('close');
                        $('#txtRemark').val('');
                    }, "บันทึก": function () {
                        var QStr = window.location.search.substring(1);
                        var EachQStr = QStr.split("=");
                        var UsId = EachQStr[2];
                        remarkDetail = $('#txtRemark').val();
                        $.ajax({
                            type: "POST",
                            url: "<%=ResolveUrl("~")%>Support/ProblemQuestionSummary.aspx/UpdateProblem",
                            data: "{QPId : '" + selector + "',RemarkDetail : '" + remarkDetail + "',ProblemType : '" + problemType + "',UsId : '" + UsId + "' }",
                            async: false,
                            contentType: "application/json; charset=utf-8", dataType: "json",
                            success: function (data) {
                                if (data.d == true) {
                                    $('#dialogRemark').dialog("close");
                                    $('#dialogAlert').dialog('option', 'title', 'บันทึกข้อมูลเรียบร้อยค่ะ').dialog("open");
                                }

                            },
                            error: function myfunction(request, status) {
                                alert(status);
                            }
                        });
                    }
                },
                draggable: false,
                resizable: false,
                modal: true
            });

            $('#dialogAlert').dialog({
                autoOpen: false,
                buttons: {
                    'ตกลง': function () {
                        $(this).dialog('close');
                        if (problemType == 2) {
                            $('#btnProblem').trigger("click");
                        } else if (problemType == 3) {
                            $('#btnEdited').trigger("click");
                        } else if (problemType == 4) {
                            $('#btnComplete').trigger("click");
                        }
                    }
                },
                draggable: false,
                resizable: false,
                modal: true
            });

            //$('#dialogDetail').dialog({
            //    autoOpen: false,
            //    buttons: {
            //        'ตกลง': function () {
            //            $(this).dialog('close');
            //        }
            //    },
            //    draggable: false,
            //    resizable: false,
            //    modal: true
            //});
        });

    </script>
</head>

<body style="background-color: #025458;">
    <form id="form1" runat="server">
        <div style="position: absolute; width: 98%; height: 98%;">
            <div>
                <asp:Button ID="btnProblem" runat="server" CssClass="btnActive" Text="ข้อสอบที่มีปัญหา" />
                <asp:Button ID="btnEdited" runat="server" CssClass="btnUnActive" Text="ข้อสอบที่มีการแก้ไขแล้ว" />
                <asp:Button ID="btnComplete" runat="server" CssClass="btnUnActive" Text="ข้อสอบที่ตรวจสอบแล้ว" />
            </div>

            <asp:Panel ID="pnProblem" runat="server">
                <div>
                    <table style="width: 100%;">
                        <tr>
                            <td colspan="2">
                                <h1>ข้อสอบที่มีปัญหา</h1>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table>
                                    <tr>
                                        <td style="color: White;"><span>ชั้น</span></td>
                                        <td><span>
                                            <asp:DropDownList ID="ddlLevelProblem" AutoPostBack="true" runat="server" CssClass="dll"></asp:DropDownList></span></td>
                                        <td style="color: White;"><span>วิชา</span></td>
                                        <td><span>
                                            <asp:DropDownList ID="ddlGroupSubjectProblem" AutoPostBack="true" runat="server" CssClass="dll"></asp:DropDownList></span></td>
                                    </tr>
                                </table>
                            </td>
                            <td class="SumText">
                                <asp:Label ID="lblProblem" runat="server" Text="รวม" ForeColor="White"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Repeater ID="rptProblem" runat="server">
                                    <HeaderTemplate>
                                        <table style="width: 100%;">
                                            <thead>
                                                <tr style="background-color: #FFFFFF; height: 50px;">
                                                    <th style="width: 3%;">ลำดับ </th>
                                                    <th style="width: 5%;">วิชา </th>
                                                    <th style="width: 3%;">ชั้น </th>
                                                    <th style="width: 15%;">หน่วย </th>
                                                    <th style="width: 20%;">บท </th>
                                                    <th style="width: 3%;">ข้อ </th>
                                                    <th style="width: 30%;">รายละเอียด </th>
                                                    <th style="width: 10%;">วันที่แจ้ง </th>
                                                    <th style="width: 10%;">ผู้แจ้ง </th>
                                                    <% If Request.QueryString("pmt") = 2 Then %>
                                                    <th style="width: 50%;">หมายเหตุ </th>
                                                    <th style="width: 30%;"></th>
                                                    <% End If %>
                                                </tr>
                                            </thead>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td style="background: #e4a5c5; padding-top: 15px; padding-bottom: 15px; text-align: center;">
                                                <asp:Label Text='<%# Eval("AnnoNo") %>' ToolTip='<%# Eval("QuestionId") %>' runat="server" />
                                            </td>
                                            <td style="background: #e4a5c5; text-align: center">
                                                <asp:Label Text='<%# Eval("SubjectName") %>' ToolTip='<%# Eval("QuestionId") %>' runat="server" />
                                            </td>
                                            <td style="background: #e4a5c5; text-align: center">
                                                <asp:Label Text='<%# Eval("LevelName") %>' ToolTip='<%# Eval("QuestionId") %>' runat="server" />
                                            </td>
                                            <td style="background: #e4a5c5; padding-left: 5px;">
                                                <asp:Label Text='<%# Eval("QCName") %>' ToolTip='<%# Eval("QuestionId") %>' runat="server" />
                                            </td>
                                            <td style="background: #e4a5c5; padding-left: 5px;">
                                                <asp:Label Text='<%# Eval("QSName") %>' ToolTip='<%# Eval("QuestionId") %>' runat="server" />
                                            </td>
                                            <td style="background: #e4a5c5; text-align: center">
                                                <asp:Label Text='<%# Eval("QNo") %>' ToolTip='<%# Eval("QuestionId") %>' runat="server" />
                                            </td>
                                            <td style="background: #e4a5c5; padding-left: 5px;">
                                                <asp:Label Text='<%# Eval("Annotation") %>' ToolTip='<%# Eval("QuestionId") %>' runat="server" />
                                            </td>
                                            <td style="background: #e4a5c5; padding-left: 5px;">
                                                <asp:Label Text='<%# Eval("ReportTime") %>' ToolTip='<%# Eval("QuestionId") %>' runat="server" />
                                            </td>
                                            <td style="background: #e4a5c5; padding-left: 5px;">
                                                <asp:Label Text='<%# Eval("UserName") %>' ToolTip='<%# Eval("QuestionId") %>' runat="server" />
                                            </td>
                                            <% If Request.QueryString("pmt") = 2 Then %>
                                            <td style="background: #e4a5c5; padding-left: 5px;">
                                                <asp:Label Text='<%# Eval("ProblemRemark") %>' ToolTip='<%# Eval("QuestionId") %>' runat="server" />
                                            </td>

                                            <td style="background: #e4a5c5; padding-left: 5px; padding-right: 5px;">
                                                <input type="button" class="btnEdited" qpid="<%# Container.DataItem("QPId")%>" value="บันทึก" />
                                            </td>
                                            <% End If %>
                                        </tr>
                                    </ItemTemplate>
                                    <AlternatingItemTemplate>
                                        <tr>
                                            <td style="background: #FFFFFF; padding-top: 15px; padding-bottom: 15px; text-align: center;">
                                                <asp:Label Text='<%# Eval("AnnoNo") %>' ToolTip='<%# Eval("QuestionId") %>' runat="server" />
                                            </td>
                                            <td style="background: #FFFFFF; text-align: center">
                                                <asp:Label Text='<%# Eval("SubjectName") %>' ToolTip='<%# Eval("QuestionId") %>' runat="server" />
                                            </td>
                                            <td style="background: #FFFFFF; text-align: center">
                                                <asp:Label Text='<%# Eval("LevelName") %>' ToolTip='<%# Eval("QuestionId") %>' runat="server" />
                                            </td>
                                            <td style="background: #FFFFFF; padding-left: 5px;">
                                                <asp:Label Text='<%# Eval("QCName") %>' ToolTip='<%# Eval("QuestionId") %>' runat="server" />
                                            </td>
                                            <td style="background: #FFFFFF; padding-left: 5px;">
                                                <asp:Label Text='<%# Eval("QSName") %>' ToolTip='<%# Eval("QuestionId") %>' runat="server" />
                                            </td>
                                            <td style="background: #FFFFFF; text-align: center">
                                                <asp:Label Text='<%# Eval("QNo") %>' ToolTip='<%# Eval("QuestionId") %>' runat="server" />
                                            </td>
                                            <td style="background: #FFFFFF; padding-left: 5px;">
                                                <asp:Label Text='<%# Eval("Annotation") %>' ToolTip='<%# Eval("QuestionId") %>' runat="server" />
                                            </td>
                                            <td style="background: #FFFFFF; padding-left: 5px;">
                                                <asp:Label Text='<%# Eval("ReportTime") %>' ToolTip='<%# Eval("QuestionId") %>' runat="server" />
                                            </td>
                                            <td style="background: #FFFFFF; padding-left: 5px;">
                                                <asp:Label Text='<%# Eval("UserName") %>' ToolTip='<%# Eval("QuestionId") %>' runat="server" />
                                            </td>
                                            <% If Request.QueryString("pmt") = 2 Then %>
                                            <td style="background: #FFFFFF; padding-left: 5px;">
                                                <asp:Label Text='<%# Eval("ProblemRemark") %>' ToolTip='<%# Eval("QuestionId") %>' runat="server" />
                                            </td>
                                            <td style="background: #FFFFFF; padding-left: 5px; padding-right: 5px;">
                                                <input type="button" class="btnEdited" qpid="<%# Container.DataItem("QPId")%>" value="บันทึก" />
                                            </td>

                                            <% End If %>
                                        </tr>
                                    </AlternatingItemTemplate>
                                    <FooterTemplate>
                                        </table>
                                    </FooterTemplate>
                                </asp:Repeater>
                            </td>
                        </tr>
                    </table>
                </div>
            </asp:Panel>

            <asp:Panel ID="pnEdited" runat="server">
                <div>
                    <table style="width: 100%;">
                        <tr>
                            <td colspan="2">
                                <h1>ข้อสอบที่มีการแก้ไขแล้ว</h1>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table>
                                    <tr>
                                        <td style="color: White;"><span>ชั้น</span></td>
                                        <td><span>
                                            <asp:DropDownList ID="ddlLevelEdited" AutoPostBack="true" runat="server" CssClass="dll"></asp:DropDownList></span></td>
                                        <td style="color: White;"><span>วิชา</span></td>
                                        <td><span>
                                            <asp:DropDownList ID="ddlGroupSubjectEdited" AutoPostBack="true" runat="server" CssClass="dll"></asp:DropDownList></span></td>
                                    </tr>
                                </table>
                            </td>
                            <td class="SumText">
                                <asp:Label ID="lblEdited" runat="server" Text="รวม" ForeColor="White"></asp:Label></td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Repeater ID="rptEdited" runat="server">
                                    <HeaderTemplate>
                                        <table style="width: 100%;">
                                            <thead>
                                                <tr style="background-color: #FFFFFF; height: 50px;">
                                                    <th style="width: 3%;">ลำดับ </th>
                                                    <th style="width: 5%;">วิชา </th>
                                                    <th style="width: 3%;">ชั้น </th>
                                                    <th style="width: 15%;">หน่วย </th>
                                                    <th style="width: 18%;">บท </th>
                                                    <th style="width: 3%;">ข้อ </th>
                                                    <th style="width: 25%;">รายละเอียด </th>
                                                    <th style="width: 8%;">วันที่แจ้ง </th>
                                                    <th style="width: 5%;">ผู้แจ้ง </th>
                                                    <th style="width: 30%;">หมายเหตุ </th>
                                                    <% If Request.QueryString("pmt") = 1 Then %>
                                                    <th style="width: 10%;"></th>
                                                    <th style="width: 10%;"></th>
                                                    <% End If %>
                                                </tr>
                                            </thead>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td style="background: #e4a5c5; padding-top: 15px; padding-bottom: 15px; text-align: center;">
                                                <asp:Label Text='<%# Eval("AnnoNo") %>' ToolTip='<%# Eval("QuestionId") %>' runat="server" />
                                            </td>
                                            <td style="background: #e4a5c5; text-align: center">
                                                <asp:Label Text='<%# Eval("SubjectName") %>' ToolTip='<%# Eval("QuestionId") %>' runat="server" />
                                            </td>
                                            <td style="background: #e4a5c5; text-align: center">
                                                <asp:Label Text='<%# Eval("LevelName") %>' ToolTip='<%# Eval("QuestionId") %>' runat="server" />
                                            </td>
                                            <td style="background: #e4a5c5; padding-left: 5px;">
                                                <asp:Label Text='<%# Eval("QCName") %>' ToolTip='<%# Eval("QuestionId") %>' runat="server" />
                                            </td>
                                            <td style="background: #e4a5c5; padding-left: 5px;">
                                                <asp:Label Text='<%# Eval("QSName") %>' ToolTip='<%# Eval("QuestionId") %>' runat="server" />
                                            </td>
                                            <td style="background: #e4a5c5; text-align: center">
                                                <asp:Label Text='<%# Eval("QNo") %>' ToolTip='<%# Eval("QuestionId") %>' runat="server" />
                                            </td>
                                            <td style="background: #e4a5c5; padding-left: 5px;">
                                                <asp:Label Text='<%# Eval("Annotation") %>' ToolTip='<%# Eval("QuestionId") %>' runat="server" />
                                            </td>
                                            <td style="background: #e4a5c5; padding-left: 5px;">
                                                <asp:Label Text='<%# Eval("ReportTime") %>' ToolTip='<%# Eval("QuestionId") %>' runat="server" />
                                            </td>
                                            <td style="background: #e4a5c5; padding-left: 5px;">
                                                <asp:Label Text='<%# Eval("UserName") %>' ToolTip='<%# Eval("QuestionId") %>' runat="server" />
                                            </td>
                                            <td style="background: #e4a5c5; padding-left: 5px;">
                                                <asp:Label Text='<%# Eval("ProblemRemark") %>' ToolTip='<%# Eval("QuestionId") %>' runat="server" />
                                            </td>

                                            <% If Request.QueryString("pmt") = 1 Then %>
                                            <td style="background: #e4a5c5; padding-left: 5px; padding-right: 5px;">
                                                <input type="button" class="btnReturn" qpid="<%# Container.DataItem("QPId")%>" value="ตีกลับ" />
                                            </td>
                                            <td style="background: #e4a5c5; padding-left: 5px; padding-right: 5px;">
                                                <input type="button" class="btnApprove" qpid="<%# Container.DataItem("QPId")%>" value="อนุมัติ" />
                                            </td>
                                            <% End If %>
                                        </tr>
                                    </ItemTemplate>
                                    <AlternatingItemTemplate>
                                        <tr>
                                            <td style="background: #FFFFFF; padding-top: 15px; padding-bottom: 15px; text-align: center;">
                                                <asp:Label Text='<%# Eval("AnnoNo") %>' ToolTip='<%# Eval("QuestionId") %>' runat="server" />
                                            </td>
                                            <td style="background: #FFFFFF; text-align: center">
                                                <asp:Label Text='<%# Eval("SubjectName") %>' ToolTip='<%# Eval("QuestionId") %>' runat="server" />
                                            </td>
                                            <td style="background: #FFFFFF; text-align: center">
                                                <asp:Label Text='<%# Eval("LevelName") %>' ToolTip='<%# Eval("QuestionId") %>' runat="server" />
                                            </td>
                                            <td style="background: #FFFFFF; padding-left: 5px;">
                                                <asp:Label Text='<%# Eval("QCName") %>' ToolTip='<%# Eval("QuestionId") %>' runat="server" />
                                            </td>
                                            <td style="background: #FFFFFF; padding-left: 5px;">
                                                <asp:Label Text='<%# Eval("QSName") %>' ToolTip='<%# Eval("QuestionId") %>' runat="server" />
                                            </td>
                                            <td style="background: #FFFFFF; text-align: center">
                                                <asp:Label Text='<%# Eval("QNo") %>' ToolTip='<%# Eval("QuestionId") %>' runat="server" />
                                            </td>
                                            <td style="background: #FFFFFF; padding-left: 5px;">
                                                <asp:Label Text='<%# Eval("Annotation") %>' ToolTip='<%# Eval("QuestionId") %>' runat="server" />
                                            </td>
                                            <td style="background: #FFFFFF; padding-left: 5px;">
                                                <asp:Label Text='<%# Eval("ReportTime") %>' ToolTip='<%# Eval("QuestionId") %>' runat="server" />
                                            </td>
                                            <td style="background: #FFFFFF; padding-left: 5px;">
                                                <asp:Label Text='<%# Eval("UserName") %>' ToolTip='<%# Eval("QuestionId") %>' runat="server" />
                                            </td>
                                            <td style="background: #FFFFFF; padding-left: 5px;">
                                                <asp:Label Text='<%# Eval("ProblemRemark") %>' ToolTip='<%# Eval("QuestionId") %>' runat="server" />
                                            </td>
                                            <% If Request.QueryString("pmt") = 1 Then %>
                                            <td style="background: #FFFFFF; padding-left: 5px; padding-right: 5px;">
                                                <input type="button" class="btnReturn" qpid="<%# Container.DataItem("QPId")%>" value="ตีกลับ" />
                                            </td>
                                            <td style="background: #FFFFFF; padding-left: 5px; padding-right: 5px;">
                                                <input type="button" class="btnApprove" qpid="<%# Container.DataItem("QPId")%>" value="อนุมัติ" />
                                            </td>
                                            <% End If %>
                                        </tr>
                                    </AlternatingItemTemplate>
                                    <FooterTemplate>
                                        </table>
                                    </FooterTemplate>
                                </asp:Repeater>
                            </td>
                        </tr>
                    </table>


                </div>
            </asp:Panel>

            <asp:Panel ID="pnComplete" runat="server">
                <div>
                    <table style="width: 100%;">
                        <tr>
                            <td colspan="2">
                                <h1>ข้อสอบที่ตรวจสอบแล้ว</h1>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table>
                                    <tr>
                                        <td style="color: White;"><span>ชั้น</span></td>
                                        <td><span>
                                            <asp:DropDownList ID="ddlLevelComplete" AutoPostBack="true" runat="server" CssClass="dll"></asp:DropDownList></span></td>
                                        <td style="color: White;"><span>วิชา</span></td>
                                        <td><span>
                                            <asp:DropDownList ID="ddlGroupSubjectComplete" AutoPostBack="true" runat="server" CssClass="dll"></asp:DropDownList></span></td>
                                    </tr>
                                </table>
                            </td>
                            <td class="SumText">
                                <asp:Label ID="lblComplete" runat="server" Text="รวม" ForeColor="White"></asp:Label></td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Repeater ID="rptComplete" runat="server">
                                    <HeaderTemplate>
                                        <table style="width: 100%;">
                                            <thead>
                                                <tr style="background-color: #FFFFFF; height: 50px;">
                                                    <th style="width: 3%;">ลำดับ </th>
                                                    <th style="width: 5%;">วิชา </th>
                                                    <th style="width: 3%;">ชั้น </th>
                                                    <th style="width: 15%;">หน่วย </th>
                                                    <th style="width: 20%;">บท </th>
                                                    <th style="width: 3%;">ข้อ </th>
                                                    <th style="width: 30%;">รายละเอียด </th>
                                                    <th style="width: 10%;">วันที่แจ้ง </th>
                                                    <th style="width: 10%;">ผู้แจ้ง </th>
                                                    <th style="width: 30%;"></th>
                                                </tr>
                                            </thead>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td style="background: #e4a5c5; padding-top: 15px; padding-bottom: 15px; text-align: center;">
                                                <asp:Label Text='<%# Eval("AnnoNo") %>' ToolTip='<%# Eval("QuestionId") %>' runat="server" />
                                            </td>
                                            <td style="background: #e4a5c5; text-align: center">
                                                <asp:Label Text='<%# Eval("SubjectName") %>' ToolTip='<%# Eval("QuestionId") %>' runat="server" />
                                            </td>
                                            <td style="background: #e4a5c5; text-align: center">
                                                <asp:Label Text='<%# Eval("LevelName") %>' ToolTip='<%# Eval("QuestionId") %>' runat="server" />
                                            </td>
                                            <td style="background: #e4a5c5; padding-left: 5px;">
                                                <asp:Label Text='<%# Eval("QCName") %>' ToolTip='<%# Eval("QuestionId") %>' runat="server" />
                                            </td>
                                            <td style="background: #e4a5c5; padding-left: 5px;">
                                                <asp:Label Text='<%# Eval("QSName") %>' ToolTip='<%# Eval("QuestionId") %>' runat="server" />
                                            </td>
                                            <td style="background: #e4a5c5; text-align: center">
                                                <asp:Label Text='<%# Eval("QNo") %>' ToolTip='<%# Eval("QuestionId") %>' runat="server" />
                                            </td>
                                            <td style="background: #e4a5c5; padding-left: 5px;">
                                                <asp:Label Text='<%# Eval("Annotation") %>' ToolTip='<%# Eval("QuestionId") %>' runat="server" />
                                            </td>
                                            <td style="background: #e4a5c5; padding-left: 5px;">
                                                <asp:Label Text='<%# Eval("ReportTime") %>' ToolTip='<%# Eval("QuestionId") %>' runat="server" />
                                            </td>
                                            <td style="background: #e4a5c5; padding-left: 5px;">
                                                <asp:Label Text='<%# Eval("UserName") %>' ToolTip='<%# Eval("QuestionId") %>' runat="server" />
                                            </td>
                                            <td style="background: #e4a5c5; padding-left: 5px; padding-right: 5px;">
                                                <input type="button" class="btnDetail" qpid="<%# Container.DataItem("QPId")%>" value="ดูประวัติ" />
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                    <AlternatingItemTemplate>
                                        <tr>
                                            <td style="background: #FFFFFF; padding-top: 15px; padding-bottom: 15px; text-align: center;">
                                                <asp:Label Text='<%# Eval("AnnoNo") %>' ToolTip='<%# Eval("QuestionId") %>' runat="server" />
                                            </td>
                                            <td style="background: #FFFFFF; text-align: center">
                                                <asp:Label Text='<%# Eval("SubjectName") %>' ToolTip='<%# Eval("QuestionId") %>' runat="server" />
                                            </td>
                                            <td style="background: #FFFFFF; text-align: center">
                                                <asp:Label Text='<%# Eval("LevelName") %>' ToolTip='<%# Eval("QuestionId") %>' runat="server" />
                                            </td>
                                            <td style="background: #FFFFFF; padding-left: 5px;">
                                                <asp:Label Text='<%# Eval("QCName") %>' ToolTip='<%# Eval("QuestionId") %>' runat="server" />
                                            </td>
                                            <td style="background: #FFFFFF; padding-left: 5px;">
                                                <asp:Label Text='<%# Eval("QSName") %>' ToolTip='<%# Eval("QuestionId") %>' runat="server" />
                                            </td>
                                            <td style="background: #FFFFFF; text-align: center">
                                                <asp:Label Text='<%# Eval("QNo") %>' ToolTip='<%# Eval("QuestionId") %>' runat="server" />
                                            </td>
                                            <td style="background: #FFFFFF; padding-left: 5px;">
                                                <asp:Label Text='<%# Eval("Annotation") %>' ToolTip='<%# Eval("QuestionId") %>' runat="server" />
                                            </td>
                                            <td style="background: #FFFFFF; padding-left: 5px;">
                                                <asp:Label Text='<%# Eval("ReportTime") %>' ToolTip='<%# Eval("QuestionId") %>' runat="server" />
                                            </td>
                                            <td style="background: #FFFFFF; padding-left: 5px;">
                                                <asp:Label Text='<%# Eval("UserName") %>' ToolTip='<%# Eval("QuestionId") %>' runat="server" />
                                            </td>
                                            <td style="background: #FFFFFF; padding-left: 5px; padding-right: 5px;">
                                                <input type="button" class="btnDetail" qpid="<%# Container.DataItem("QPId")%>" value="ดูประวัติ" />
                                            </td>
                                        </tr>
                                    </AlternatingItemTemplate>
                                    <FooterTemplate>
                                        </table>
                                    </FooterTemplate>
                                </asp:Repeater>
                            </td>
                        </tr>
                    </table>
                </div>
            </asp:Panel>

        </div>

        <div id="dialogRemark">
            <div id="divRemark" style="margin-top: 15px;">
                <asp:TextBox runat="server" ID="txtRemark" TextMode="MultiLine" Rows="5" Style="width: 98%;" />
            </div>
        </div>

        <div id="dialogAlert"></div>
        <div id="MyShadow" style="background-color: gray; opacity: 0.7; width: 99%; height: 98%; position: absolute; z-index: 20; display: none;"></div>
        <div id="dialogDetail">
            <div id="divDetail" style="overflow-y: scroll; max-height: 500px;">
            </div>
            <div style="text-align: center;">
                <input type="button" class="btnClose" value="ปิด" style="cursor: pointer; font: normal 20px 'THSarabunNew'; font-weight: bold; width: 45%; border: 1px solid #ccc; background: #F6F6F6; color: #1C94C4; margin-top: 20px;" />
            </div>
        </div>

    </form>
</body>
</html>
