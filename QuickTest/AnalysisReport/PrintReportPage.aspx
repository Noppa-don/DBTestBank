<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="PrintReportPage.aspx.vb"
    Inherits="QuickTest.PrintReportPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        #MainDiv
        {
            width: 800px;
            margin-left: auto;
            margin-right: auto;
        }
        
        #DivContent
        {
            width: 500px;
            margin-left: auto;
            margin-right: auto;
            text-align: center;
            padding: 20px;
        }
        #DivMonthAndYear
        {
            width: 500px;
            margin-left: auto;
            margin-right: auto;
            text-align: center;
            padding: 20px;
        }
        span
        {
            font-size: 30px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div id='MainDiv'>
        <div id='DivContent'>
            <div id='DivSchoolCode'>
                <span>รหัสโรงเรียน</span>
                <asp:TextBox ID="txtSchoolId" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate='txtSchoolId'
                    ErrorMessage="*"></asp:RequiredFieldValidator>
                <asp:CompareValidator ID="CompareValidator1" Type="Integer" ControlToValidate='txtSchoolId'
                    Operator="DataTypeCheck" runat="server" ErrorMessage="*"></asp:CompareValidator>
            </div>
            <div id='DivMonthAndYear'>
                <span>เดือน</span>
                <asp:DropDownList ID="DDLMonth" runat="server" Font-Size="X-Large">
                </asp:DropDownList>
                <br />
                <br />
                <span>ปี(ค.ศ.)</span>
                <asp:TextBox ID="txtYear" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate='txtYear'
                    runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                <asp:CompareValidator ID="CompareValidator2" Type="Integer" ControlToValidate='txtYear'
                    Operator="DataTypeCheck" runat="server" ErrorMessage="*"></asp:CompareValidator>
                <br />
                <br />
                <asp:Button ID="btnOk" runat="server" Text="ตกลง" />
            </div>
        </div>
    </div>
    </form>
</body>
</html>
