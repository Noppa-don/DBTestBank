<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="CreateTabletWithOwner.aspx.vb"
    Inherits="QuickTest.CreateTabletWithOwner" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <style type="text/css">
        .ForMainDiv
        {
            width: 1200px;
            margin-left: auto;
            margin-right: auto;
        }
        .ForCheckBox
        {
            margin: 5px 5px 5px 5px;
        }
        .Fortable
        {
            margin: 20px;
        }
    </style>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
     
    <div id='mainDiv' class='ForMainDiv'>
    <asp:Button ID="BtnBack" runat="server" Text="กลับ"  Font-Size="Larger" Width='120' Height='50' style='margin-bottom:20px;' />
        <div id='DivSchoolInfo'>
            <table>
                <tr>
                    <td>
                        <asp:Label ID="lblSchoolCode" runat="server" Text="รหัส รร."></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtSchoolCode" runat="server" Text="1000001"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblSchoolPass" runat="server" Text="รหัสผ่าน"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtSchoolPass" runat="server" Text="1234"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </div>
        <hr />
        <div id='DivTeacher'>
            <span style='font-size: 30px;'>ส่วนของครู</span>
            <table class='Fortable'>
                <tr>
                    <td>
                        <asp:Label ID="lblTeacherName" runat="server" Text="ชื่อ"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtTeacherName" runat="server" Text=""></asp:TextBox>
                    </td>
                    <td>
                        <asp:Label ID="lblTeacherLastName" runat="server" Text="นามสกุล"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtTeacherLastName" runat="server" Text=""></asp:TextBox>
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td>
                        <asp:Label ID="lblTeacherClass" runat="server" Text="สอนชั้น"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox ID="ChkTC1" CssClass='ForCheckBox' Text='ป.1' runat="server" />
                    </td>
                    <td>
                        <asp:CheckBox ID="ChkTC2" CssClass='ForCheckBox' Text='ป.2' runat="server" />
                    </td>
                    <td>
                        <asp:CheckBox ID="ChkTC3" CssClass='ForCheckBox' Text='ป.3' runat="server" />
                    </td>
                    <td>
                        <asp:CheckBox ID="ChkTC4" CssClass='ForCheckBox' Text='ป.4' runat="server" />
                    </td>
                    <td>
                        <asp:CheckBox ID="ChkTC5" CssClass='ForCheckBox' Text='ป.5' runat="server" />
                    </td>
                    <td>
                        <asp:CheckBox ID="ChkTC6" CssClass='ForCheckBox' Text='ป.6' runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox ID="ChkTC7" CssClass='ForCheckBox' Text='ม.1' runat="server" />
                    </td>
                    <td>
                        <asp:CheckBox ID="ChkTC8" CssClass='ForCheckBox' Text='ม.2' runat="server" />
                    </td>
                    <td>
                        <asp:CheckBox ID="ChkTC9" CssClass='ForCheckBox' Text='ม.3' runat="server" />
                    </td>
                    <td>
                        <asp:CheckBox ID="ChkTC10" CssClass='ForCheckBox' Text='ม.4' runat="server" />
                    </td>
                    <td>
                        <asp:CheckBox ID="ChkTC11" CssClass='ForCheckBox' Text='ม.5' runat="server" />
                    </td>
                    <td>
                        <asp:CheckBox ID="ChkTC12" CssClass='ForCheckBox' Text='ม.6' runat="server" />
                    </td>
                </tr>
            </table>
            <table class='Fortable'>
                <tr>
                    <td>
                        <asp:Label ID="Label1" runat="server" Text="ห้อง(ใส่ตัวเลขตัวเดียว เช่น 1)"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtTeacherRoom" runat="server" Text=""></asp:TextBox>
                    </td>
                </tr>
            </table>
            <table class='Fortable'>
                <tr>
                    <td>
                        <asp:Label ID="lblTeacherSubject" runat="server" Text="วิชา"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox ID="ChkJubject1" CssClass='ForCheckBox' Text='ไทย' runat="server" />
                    </td>
                    <td>
                        <asp:CheckBox ID="ChkJubject2" CssClass='ForCheckBox' Text='สังคมฯ' runat="server" />
                    </td>
                    <td>
                        <asp:CheckBox ID="ChkJubject3" CssClass='ForCheckBox' Text='คณิตฯ' runat="server" />
                    </td>
                    <td>
                        <asp:CheckBox ID="ChkJubject4" CssClass='ForCheckBox' Text='วิทยาศาสตร์' runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox ID="ChkJubject5" CssClass='ForCheckBox' Text='ภาษาอังกฤษ' runat="server" />
                    </td>
                    <td>
                        <asp:CheckBox ID="ChkJubject6" CssClass='ForCheckBox' Text='สุขศึกษาฯ' runat="server" />
                    </td>
                    <td>
                        <asp:CheckBox ID="ChkJubject7" CssClass='ForCheckBox' Text='ศิลปะ' runat="server" />
                    </td>
                    <td>
                        <asp:CheckBox ID="ChkJubject8" CssClass='ForCheckBox' Text='การงานฯ' runat="server" />
                    </td>
                </tr>
            </table>
            <asp:Button ID="btnSaveTeacher" Style='margin-left: 400px;' runat="server" Text="Save ครู" />
        </div>
        <hr />
        <div id='DivStudent'>
            <span style='font-size: 30px;'>ส่วนของนักเรียน</span>
            <table class='Fortable'>
                <tr>
                    <td>
                        <asp:Label ID="lblStudentName" runat="server" Text="ชื่อ"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtStudentName" runat="server" Text=""></asp:TextBox>
                    </td>
                    <td>
                        <asp:Label ID="lblStudentLastName" runat="server" Text="นามสกุล"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtStudentLastName" runat="server" Text=""></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblStudentNoInRoom" runat="server" Text="เลขที่นั่งในห้อง"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtStudentNoInRoom" runat="server" Text=""></asp:TextBox>
                    </td>
                    <td>
                        <asp:Label ID="lblRoom" runat="server" Text="ห้อง(ใส่ตัวเลขตัวเดียว เช่น 1)"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtStudentRoom" runat="server" Text=""></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblClassStudent" runat="server" Text="ชั้น"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:RadioButton ID="rd1" CssClass='ForCheckBox' GroupName='StudentClass' Text='ป.1'
                            runat="server" />
                    </td>
                    <td>
                        <asp:RadioButton ID="rd2" CssClass='ForCheckBox' GroupName='StudentClass' Text='ป.2'
                            runat="server" />
                    </td>
                    <td>
                        <asp:RadioButton ID="rd3" GroupName='StudentClass' Text='ป.3' runat="server" CssClass='ForCheckBox' />
                    </td>
                    <td>
                        <asp:RadioButton ID="rd4" GroupName='StudentClass' Text='ป.4' runat="server" CssClass='ForCheckBox' />
                    </td>
                    <td>
                        <asp:RadioButton ID="rd5" GroupName='StudentClass' Text='ป.5' runat="server" CssClass='ForCheckBox' />
                    </td>
                    <td>
                        <asp:RadioButton ID="rd6" GroupName='StudentClass' Text='ป.6' runat="server" CssClass='ForCheckBox' />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:RadioButton ID="rd7" GroupName='StudentClass' Text='ม.1' runat="server" CssClass='ForCheckBox' />
                    </td>
                    <td>
                        <asp:RadioButton ID="rd8" GroupName='StudentClass' Text='ม.2' runat="server" CssClass='ForCheckBox' />
                    </td>
                    <td>
                        <asp:RadioButton ID="rd9" GroupName='StudentClass' Text='ม.3' runat="server" CssClass='ForCheckBox' />
                    </td>
                    <td>
                        <asp:RadioButton ID="rd10" GroupName='StudentClass' Text='ม.4' runat="server" CssClass='ForCheckBox' />
                    </td>
                    <td>
                        <asp:RadioButton ID="rd11" GroupName='StudentClass' Text='ม.5' runat="server" CssClass='ForCheckBox' />
                    </td>
                    <td>
                        <asp:RadioButton ID="rd12" GroupName='StudentClass' Text='ม.6' runat="server" CssClass='ForCheckBox' />
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td>
                        <asp:Label ID="lblStudentCode" runat="server" Text="รหัสนักเรียน(เป็นตัวกำหนดว่าซ้ำหรือไม่ซ้ำ)"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtStudentCode" runat="server" Text=""></asp:TextBox>
                    </td>
                </tr>
            </table>
             <asp:Button ID="btnSaveStudent" Style='margin-left: 400px;' runat="server" Text="Save นักเรียน" />
        </div>
    </div>
    </form>
</body>
</html>
