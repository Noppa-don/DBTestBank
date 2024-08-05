<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ChoiceControl.ascx.vb"
    Inherits="QuickTest.ChoiceControl" %>

<%--  <%= InsertPageBreakTag()%>--%>

<asp:Literal ID="ltQSetName" runat="server"></asp:Literal>
<asp:Repeater ID="QuestionListing" runat="server">
    <HeaderTemplate>
       
      <table width="100%">
    </HeaderTemplate>
    <ItemTemplate>
      
        <tr> 
       
            <%# GetQuestionNo()%>
            <td>

                <table>
                    <tr>
                        <%# Eval("Question_Name")%>
                    </tr>
                    <%# GetAnswerDetails(Eval("Question_Id").ToString)%>
                </table>
     
            </td>
        </tr> 

            

      
    </ItemTemplate>
    <FooterTemplate>
       
        </table>
    </FooterTemplate>
</asp:Repeater>
