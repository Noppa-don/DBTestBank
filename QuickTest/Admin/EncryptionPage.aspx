<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="EncryptionPage.aspx.vb" Inherits="QuickTest.EncryptionPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
     <title></title>
</head>
<body>

    <form id="form1" runat="server">   
        <br />
        <span id="Path" runat="server"> </span>
        <br />
        <br />
    <div style="text-align:center;background-color:lightpink">
        <table>
            <tr>
                <td style="width: 99%; padding-left: 15px; padding-bottom: 20px; padding-top:20px;">
                    <asp:TextBox ID="txtAppSetting" Width="98%" Height="120px" runat="server" TextMode="MultiLine"></asp:TextBox>
                    <br />
                    <br />
                    <asp:TextBox ID="txtEncryption" Width="98%" Height="120px" runat="server" TextMode="MultiLine"></asp:TextBox>
                </td>
                <td style="padding-right: 19px;">
                    <asp:Button ID="btnEncryption" height="80px" width= "100px" font-size="24px" runat="server" Text="เข้ารหัส" />
                </td>
            </tr>
        </table>
    </div>
        <br />

            <div style="text-align:center;background-color:lightskyblue">
        <table>
            <tr>
                <td style="width: 99%; padding-left: 15px; padding-bottom: 20px; padding-top:20px;">
                    <asp:TextBox ID="TextBox1" Width="98%" Height="120px" runat="server" TextMode="MultiLine"></asp:TextBox>
                    <br />
                    <br />
                    <asp:TextBox ID="TextBox2" Width="98%" Height="120px" runat="server" TextMode="MultiLine"></asp:TextBox>
                </td>
                <td style="padding-right: 19px;">
                    <asp:Button ID="Button1" height="80px" width= "100px" font-size="24px" runat="server" Text="ถอดรหัส" />
                </td>
            </tr>
        </table>
    </div>
    <div>            
        <asp:GridView ID="gvExcode" runat="server" AutoGenerateColumns="false" Visible="false" ClientIDMode="Static">
            <Columns>    
                <asp:BoundField DataField="RowNum" HeaderText="ลำดับที่" />                   
                <asp:BoundField DataField="KeyName" HeaderText="ชื่อ Setting" />
                <asp:BoundField DataField="KeyValue" HeaderText="ค่า Setting" />
            </Columns>
        </asp:GridView>
    </div>

      <%--  <div>
        ถอดรหัส    
            <asp:TextBox ID="TextBox1" Width="90%" Height="120px" runat="server" TextMode="MultiLine"></asp:TextBox>
            <br />
            <br />
            <asp:TextBox ID="TextBox2" Width="90%" Height="120px" runat="server" TextMode="MultiLine"></asp:TextBox>
            <asp:Button ID="Button1" runat="server" Text="OK" />
        </div>--%>
    </form>
</body>
</html>
