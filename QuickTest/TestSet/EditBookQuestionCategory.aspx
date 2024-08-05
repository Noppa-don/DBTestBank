<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="EditBookQuestionCategory.aspx.vb" Inherits="QuickTest.EditBookQuestionCategory" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">

    <div style='width:750px;margin-left:auto;margin-right:auto;text-align:center;'>
    <br />
    <br />
    <asp:Label ID="Label1" Font-Bold="true" Font-Underline="true"  runat="server" Text="ชื่อหน่วยการเรียนรู้"></asp:Label>
    <br />
        <asp:Label ID="lblQuestionCategoryName" Font-Bold="true"  runat="server" Text=""></asp:Label>
    <br />
    <br />
    <span>เลือกหนังสือ</span>
    <br />
    <asp:DropDownList ID="ddlBook" runat="server">
    </asp:DropDownList>
    <br />
    <br />
    <asp:Button ID="btnSave"   style='float:right;'  runat="server" Text="ตกลง" Width="90" Height="60" />
    </div>

    </form>
</body>
</html>
