<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="MenuPage.aspx.vb" Inherits="QuickTest.MenuPage" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager runat="server">
    </asp:ScriptManager>
    <div>
        <table>
            <tr>
                <td>
                    <telerik:RadButton ID="btnUser" Text="เมนูจัดการข้อมูลผู้ใช้(โรงเรียน)" runat="server">
                    </telerik:RadButton>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadButton ID="btnUserAdmin" Text="เมนูจัดการข้อมูลผู้ใช้(Admin)" runat="server">
                    </telerik:RadButton>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadButton ID="btnLogPage" Text="เมนูดูข้อมูลการใช้งาน" runat="server">
                    </telerik:RadButton>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadButton ID="btnQuestion" Text="เมนูคำถามเพิ่มเติม" runat="server">
                    </telerik:RadButton>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadButton ID="btnChangePassword" runat="server" Text="เปลี่ยนรหัสผ่าน">
                    </telerik:RadButton>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadButton ID="btnSetEmail" runat="server" Text="ตั้งค่าอีเมล์">
                    </telerik:RadButton>
                    <br />
                </td>
            </tr>
                       <tr>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadButton ID="btnCreateEvalIndex" runat="server" Text="กำหนดค่า ตัวชี้วัด, ดัชนีชี้วัด">
                    </telerik:RadButton>
                </td>
            </tr>
        </table>
        <table border="1">
            <tr>
                <td width="50px">
                    <b>เวอร์ชั่น</b>
                </td>
                <td width="100px">
                    <b>วันที่</b>
                </td>
                <td>
                    <b>รายละเอียด</b>
                </td>
                <td width="100px">
                    <b>คนเขียน</b>
                </td>
            </tr>
            <tr>
                <td>
                    3</td>
                <td>
                    2 ก.พ. 56</td>
                <td>
                    - รองรับการสร้าง username ชื่อ ApproveXX ได้ถึง 99คน (แยกวิชากันได้)
                </td>
                <td>
                    ต้น</td>
            </tr>
            <tr>
                <td>
                    2
                </td>
                <td>
                    5 ตค. 55
                </td>
                <td>
                    - เพิ่มข้อความให้ฝ่ายวิชาการเห็นได้ง่าย ว่าข้อไหน ยังไม่ได้ทำ ดัชนีชี้วัด , หรือ
                    ข้อไหน ยังไม่ได้ใส่คำอธิบายโจทย์<br />
                    -เปลี่ยนวิธีการเข้าหน้าจอตั้งค่าตัวชี้วัด ให้เข้าผ่าน loginAdminPage.aspx
                </td>
                <td>
                    ต้น
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                </td>
                <td>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                </td>
                <td>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                </td>
                <td>
                </td>
                <td>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
