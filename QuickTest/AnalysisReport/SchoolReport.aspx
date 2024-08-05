<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="SchoolReport.aspx.vb" Inherits="QuickTest.SchoolReport" %>

<%@ Register assembly="Telerik.ReportViewer.WebForms, Version=7.1.13.612, Culture=neutral, PublicKeyToken=a9d7983dfcc261be" namespace="Telerik.ReportViewer.WebForms" tagprefix="telerik" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    
    <style type="text/css">

        #DivReport {
        width:1200px;
        margin-left:auto;
        margin-right:auto;
        }

    </style>

</head>
<body>
    <form id="form1" runat="server">
    <div id="DivReport" >
        <telerik:ReportViewer ID="ReportViewer1" runat="server" Width="1000px" Height="800px"></telerik:ReportViewer>
    </div>
    </form>
</body>
</html>
