<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="TestTeacherAction.aspx.vb"
    Inherits="QuickTest.TestTeacherAction" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form method="post" action="../teacheraction.aspx">
    DeviceUniqueID<input type="text" name="DeviceUniqueID" />
    <input type="text" name="Method" value="nextaction" />
    <input type="submit" value="Send" />
    </form>
</body>
</html>
