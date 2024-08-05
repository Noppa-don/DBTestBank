<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ExamMatchTemplate.aspx.vb"
    Inherits="QuickTest.ExamMatchTemplate" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .style2
        {
            width: 771px;
        }
        .style3
        {
            width: 2%;
        }
        .style4
        {
            width: 783px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table>
            <tr>
                <td id="tdExam" runat="server" style="padding-top: 5px;" class="style4">
                    <table>
                        <tr>
                            <td>
                                <asp:Repeater ID="QuestionListing" runat="server">
                                    <HeaderTemplate>
                                        <table>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td>
                                                ___
                                                <%# GetQuestionNo()%>
                                            </td>
                                            <td>
                                                <%# Eval("Question_name")%>
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
                </td>
                <td style="padding-top: 5px; font-family: Angsana New;" class="style2">
                    <table>
                        <tr>
                            <asp:Repeater ID="Repeater1" runat="server">
                                <HeaderTemplate>
                                    <table>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td style="padding-top: 5px;">
                                            <%# Eval("Answer_name")%>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
