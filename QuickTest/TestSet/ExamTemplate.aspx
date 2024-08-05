<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ExamTemplate.aspx.vb"
    Inherits="QuickTest.ExamTemplate" EnableViewState="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <meta http-equiv="content-type" content="text/html; charset=tis-620" />
    <style type="text/css">
        .HText
        {
            text-align:center;
            width: 100%; 
            font-family: "Angsana New";
            font-size:<% =Session("HeaderFontSize") %>pt;
        }
        
       /* .style3
        {
            width: 100%;
            height: 40px;
        }--%>
        .style4
        {
            width: 96%;
        }
        .style5
        {
            width: 87%;
        }*/
        .questiontext
        {
            font-family: "Angsana New";
            text-align: left;
            font-size:  <%=Session("DetailFontSize")%>pt; 
            width: 100%;
        }
        .answerbullet
        {
            font-family: "Angsana New";
            width: 50px;
            font-size: <%=Session("DetailFontSize")%>pt;
            red 10px solid; 
			page-break-inside: avoid;
        }
        .answerbulletTF
        {
            font-family: "Angsana New";
            width: 10px;
            font-size: <%=Session("DetailFontSize")%>pt;
        }
        .questionno
        {
            font-family: "Angsana New";
            font-weight: bold;
            font-size: <%=Session("DetailFontSize")%>pt;
            padding-top: 5px;
        }
        .setname
        {
            font-family: "Angsana New";
            font-size: <%=Session("DetailFontSize")%>pt;
            vertical-align: text-top;
        }
        .MatchAnswer
        {
            font-family: "Angsana New";
            font-size: <%=Session("DetailFontSize")%>pt;
            padding-top: 5px;
            vertical-align: text-top;
            width: 80px;
        }
        .AnswerSheetMath
        {
            border-bottom-style: solid;
            border-bottom-width: 2px;
            font-family: "Angsana New";
            font-size: <%=Session("DetailFontSize")%>pt;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div style="page-break-before:always;">
        <table width="100%">
            <tr>
                <td id="tdSchoolName" runat="server" class="HText">
                    <asp:Literal ID="litSchoolName" runat="server"></asp:Literal>
                </td>
            </tr>
            <tr>
                <td id="tdExamDetail" runat="server"  
                    class="HText">
                    <asp:Literal ID="litExamDetail" runat="server"></asp:Literal>
                </td>
            </tr>
        </table>
        <table width="100%">
            <tr>
                <td id="tdExamAmount" runat="server" class="HText" style="width: 50%; text-align: left;">
                    &nbsp;<asp:Literal ID="litExamAmount" runat="server"></asp:Literal>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                </td>
                <td id="tdToTalTime" runat="server" class="HText" style="width: 50%; text-align: right;">
                    <asp:Literal ID="litTotalTime" runat="server"></asp:Literal>
                </td>
            </tr>
        </table>
        <hr align="center" width="98%" color="#000000">
         
    </div>
    </form>
</body>
</html>
