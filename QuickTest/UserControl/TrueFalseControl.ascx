<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="TrueFalseControl.ascx.vb"
    Inherits="QuickTest.TrueFalseControl" %>
 
<%= InsertPageBreakTag()%>    

<asp:Literal ID="ltQSetName" runat="server"></asp:Literal>


<asp:Repeater ID="QuestionListing" runat="server">
    <HeaderTemplate>
        <table width="100%">
    </HeaderTemplate>
    <ItemTemplate>
        <tr>
        <td style="width: 20px;">
        
        </td>
            <td>
                 <%# GetAnswer(Eval("Question_Id").ToString)%>   
            </td>
         <%--   <td style="width: 50px;">    
            </td>--%>
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
