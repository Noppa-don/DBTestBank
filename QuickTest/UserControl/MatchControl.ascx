<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="MatchControl.ascx.vb" Inherits="QuickTest.MatchControl" %>

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

<%= InsertPageBreakTag()%>

<asp:Literal ID="ltQSetName" runat="server"></asp:Literal>

<table  width="100%">
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