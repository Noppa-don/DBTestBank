<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ReportStudent.aspx.vb" Inherits="QuickTest.ReportStudent" %>

<%@ Register assembly="Telerik.ReportViewer.WebForms, Version=7.1.13.612, Culture=neutral, PublicKeyToken=a9d7983dfcc261be" namespace="Telerik.ReportViewer.WebForms" tagprefix="telerik" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
  <style type="text/css">

      #MainDiv {
      width:1100px;
      margin-left:auto;
      margin-right:auto;
      }

  </style>
</head>
<body>
    <form id="form1" runat="server">
    <div id="MainDiv">

        <telerik:ReportViewer ID="ReportViewer1" Width="1000" Height="1000" runat="server"></telerik:ReportViewer>

    </div>
    </form>
</body>
</html>
