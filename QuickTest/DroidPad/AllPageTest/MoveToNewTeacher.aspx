<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="MoveToNewTeacher.aspx.vb" Inherits="QuickTest.MoveToNewTeacher" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
<form method="post" action="../install.aspx" >
DeviceUniqueID<input type="text" name="DeviceUniqueID" />
FirstName<input type="text" name="FirstName" />
LastName<input type="text" name="LastName" />
TeacherClass<input type="text" name='Class' />
Room<input type="text" name='Room' />
Subject<input type="text" name='Subject' />
<input type="text" name="Method" value="movetonewteacher" />
<input type="submit" value="Send" />  
</form>
</body>
</html>
