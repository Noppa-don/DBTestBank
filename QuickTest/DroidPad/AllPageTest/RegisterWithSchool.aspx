<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="RegisterWithSchool.aspx.vb" Inherits="QuickTest.RegisterWithSchool" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>

<form method="post" action="../install.aspx" >
<input type="text" name="DeviceUniqueID" value="ssdfdfdf" />
<input type="text" name="SchoolID" value="1000001" />
<input type="text" name="SchoolPassword" value="1234" />
<input type="text" name="Method" value="registerwithschool" />
<input type="submit" value="Send" />  
<hr />
<br />
<input type="button" value='Go MoveToNewSchool' onclick="location.href='../AllPageTest/movetonewschool.aspx' " />
<input type="button" value='Go RegisterTeacher' onclick="location.href='../AllPageTest/registerteacher.aspx' " />
<input type="button" value='Go RegisterStudent' onclick="location.href='../AllPageTest/registerstudent.aspx' " />

</form>

</body>
</html>
