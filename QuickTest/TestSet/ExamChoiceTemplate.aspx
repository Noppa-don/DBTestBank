<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ExamChoiceTemplate.aspx.vb"
    Inherits="QuickTest.ExamChoiceTemplate" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <style type="text/css">
        .style2
        {
            width: 1459px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table width="100%">
            <tr>
                <td id="tdSchoolName" runat="server" style="width: 100%; font-family: Angsana New;
                    font-size: 56pt;" align="center">
                    <asp:Literal ID="litSchoolName" runat="server"></asp:Literal>
                </td>
            </tr>
            <tr>
                <td>
                </td>
            </tr>
            <tr>
                <td id="tdExamDetail" runat="server" style="width: 100%; font-family: Angsana New;
                    font-size: 52pt;" align="center">
                    <asp:Literal ID="litExamDetail" runat="server"></asp:Literal>
                </td>
            </tr>
            <tr>
                <td>
                </td>
            </tr>
        </table>
        <table>
            <tr>
                <td id="tdExamAmount" runat="server" class="style2" style="font-family: Angsana New;
                    font-size: 52pt;">
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Literal ID="litExamAmount" runat="server"></asp:Literal>
                </td>
                <td id="tdToTalTime" runat="server" style="font-family: Angsana New; font-size: 44pt;">
                    <asp:Literal ID="litTotalTime" runat="server"></asp:Literal>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    _______________________________________________________________________________________________________________________________________________________________________________________________
                </td>
    </div>
            </tr>
        </table>
    <div>
        <table id="tableExam" runat="server">
            <tr>
                <td>
                </td>
            </tr>
            <tr>
                <td style="width: 10%">
                </td>
                <td >
                    <asp:Repeater ID="QuestionListing" runat="server">
                        <HeaderTemplate>
                            <table>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td valign="top" style="padding-top: 5px; font-family: Angsana New;" >
                                <%--    <%# GetQuestionNo()%>--%>
                                </td>
                                <td>
                                    <table style="font-family: Angsana New;"> 
                                        <tr>
                                            <td colspan="2" style="font-family: Angsana New;">
                                                <%# Eval("Question_Name")%>
                                            </td>
                                        </tr>
                                 <%--       <%# GetAnswerDetails(Eval("Question_Id").ToString)%>--%>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            </table>
                        </FooterTemplate>
                    </asp:Repeater>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
