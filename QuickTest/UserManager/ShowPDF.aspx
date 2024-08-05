<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ShowPDF.aspx.vb" Inherits="QuickTest.ShowPDF" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <iframe width="95%"  src="./pdf/<%=request.querystring("file")%>" frameborder="0" 
            height="1000px"></iframe>
    </div>
    </form>
</body>
</html>
