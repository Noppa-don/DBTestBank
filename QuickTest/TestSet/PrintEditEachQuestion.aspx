<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="PrintEditEachQuestion.aspx.vb" Inherits="QuickTest.PrintEditEachQuestion" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <script type="text/javascript">

        window.print();

    </script>





    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <br />
    <asp:Label ID="lblInfoQuestion" Font-Bold="true" runat="server" ></asp:Label>
    <br />
     <asp:Label ID="lblInfoQuestion2" Font-Bold="true" runat="server" ></asp:Label>
    <br />
    <br />
    <asp:Label ID="Label1" runat="server" Text="คำถาม" Font-Underline="True"></asp:Label>
    <br />
        <asp:Label ID="lblQuestion" runat="server" ></asp:Label>
        <br />
        <asp:Label ID="Label2" runat="server" Text="อธิบายคำถาม" Font-Underline="True"></asp:Label>
        <br />
        <asp:Label ID="lblQuestionExplain" runat="server" ></asp:Label>
        <br />
        <hr />
        <hr />
        <br />
        <asp:Label ID="lblAnswerHead1" runat="server" Font-Underline="True" Text="คำตอบ1" ></asp:Label>
        <br />
        <asp:Label ID="lblAnswer1" runat="server" ></asp:Label>
        <br />
        <asp:Label ID="Label5" runat="server" Font-Underline="True" Text="อธิบายคำตอบ1" ></asp:Label>
        <br />
        <asp:Label ID="lblAnswerExplain1" runat="server" ></asp:Label>
        <br />
        <hr />

        <asp:Panel ID="Panel5" Visible="false" runat="server">
        <asp:Label ID="lblAnswerHead2" runat="server" Font-Underline="True" Text="คำตอบ2" ></asp:Label>
        <br />
        <asp:Label ID="lblAnswer2" runat="server" ></asp:Label>
        <br />
        <asp:Label ID="Label7" runat="server" Font-Underline="True" Text="อธิบายคำตอบ2" ></asp:Label>
        <br />
        <asp:Label ID="lblAnswerExplain2" runat="server" ></asp:Label>
        <br />
        <hr />
                </asp:Panel>

        <asp:Panel ID="Panel1" Visible="false" runat="server">
            <asp:Label ID="lblAnswerHead3" Font-Underline="True" runat="server" Text="คำตอบ3" ></asp:Label>
        <br />
        <asp:Label ID="lblAnswer3" runat="server" ></asp:Label>
        <br />
        <asp:Label ID="Label9" runat="server" Font-Underline="True" Text="อธิบายคำตอบ3"></asp:Label>
        <br />
        <asp:Label ID="lblAnswerExplain3" runat="server" ></asp:Label>
        <br />
        <hr />
                </asp:Panel>

        <asp:Panel ID="Panel2" Visible ="false" runat="server">
        <asp:Label ID="lblAnswerHead4" Font-Underline="True"  runat="server" Text="คำตอบ4" ></asp:Label>
        <br />
        <asp:Label ID="lblAnswer4"  runat="server" ></asp:Label>
        <br />
        <asp:Label ID="Label13" Font-Underline="True"  runat="server" Text="อธิบายคำตอบ4"></asp:Label>
        <br />
        <asp:Label ID="lblAnswerExplain4"  runat="server" ></asp:Label>
        <br />
        <hr />
                </asp:Panel>

        <asp:Panel ID="Panel3" Visible="false" runat="server">
         <asp:Label ID="lblAnswerHead5" Font-Underline="True"  runat="server" Text="คำตอบ5" ></asp:Label>
        <br />
        <asp:Label ID="lblAnswer5"  runat="server" ></asp:Label>
        <br />
        <asp:Label ID="Label8"  Font-Underline="True" runat="server" Text="อธิบายคำตอบ5"></asp:Label>
        <br />
        <asp:Label ID="lblAnswerExplain5"  runat="server" ></asp:Label>
        <br />
        <hr />
           </asp:Panel>

              <asp:Panel ID="Panel4" Visible="false" runat="server">
         <asp:Label ID="lblAnswerHead6"  Font-Underline="True" runat="server" Text="คำตอบ6" ></asp:Label>
        <br />
        <asp:Label ID="lblAnswer6"  runat="server" ></asp:Label>
        <br />
        <asp:Label ID="Label6" Font-Underline="True"  runat="server" Text="อธิบายคำตอบ6"></asp:Label>
        <br />
        <asp:Label ID="lblAnswerExplain6"  runat="server" ></asp:Label>
        <br />
        <hr />
           </asp:Panel>

    </div>
    </form>
</body>
</html>
