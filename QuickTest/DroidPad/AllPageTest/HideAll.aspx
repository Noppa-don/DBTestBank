﻿<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="HideAll.aspx.vb" Inherits="QuickTest.HideAll" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
<form method="post" action="../install.aspx" >
NeedHide<input type="text" name='NeedHide' />
<input type="text" name="Method" value="hideall" />
<input type="submit" value="Send" />  
</form>
</body>
</html>
