<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="MuteAll.aspx.vb" Inherits="QuickTest.MuteAll" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>

<form method="post" action="../install.aspx" >
NeedMute<input type="text" name='NeedMute' />
<input type="text" name="Method" value="muteall" />
<input type="submit" value="Send" />  
</form>

</body>
</html>
