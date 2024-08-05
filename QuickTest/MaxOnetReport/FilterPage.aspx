<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="FilterPage.aspx.vb" Inherits="QuickTest.FilterPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div id="ProfileImageDiv">
            <table>
                <tr>
                    <td>
                        <asp:Image ID="Image1" runat="server" />
                    </td>
                </tr>    
            </table>
        </div>
        <div id="UserDetailDiv">
            <table>
                <tr>
                    <td>
                        <span></span>
                    </td>
                </tr> 
                 <tr>
                    <td>
                        <span></span> 
                    </td>
                </tr>    
                 <tr>
                    <td>
                         <span></span>
                    </td>
                </tr>       
            </table>
        </div>
    </form>
</body>
</html>
