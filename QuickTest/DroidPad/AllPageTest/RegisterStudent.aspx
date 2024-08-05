<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="RegisterStudent.aspx.vb" Inherits="QuickTest.RegisterStudent" %>

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
StudentClass<input type="text" name="Class" />
Room<input type="text" name="Room" />
StudentCode<input type="text" name="StudentCode" />
NumberInRoom<input type="text" name="NumberInRoom" />
Gender(M or F Only)<input type="text" name="Gender" />
<input type="text" name="Method" value="registerstudent" />
<input type="submit" value="Send" />  
<hr />
<br />
<input type="button" value='Go MoveToNewStudent' onclick="location.href='../AllPageTest/movetonewstudent.aspx' " />
</form>
</body>
</html>
