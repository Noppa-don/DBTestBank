<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="UserPage.aspx.vb" Inherits="QuickTestGenUserData.UserPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    
    <link href="../css/bootstrap.min.css" rel="stylesheet" />
    <link href="../css/bootstrap-theme.min.css" rel="stylesheet" />
    <link href="../css/jquery-ui-1.8.18.custom.css" rel="stylesheet" />
    <link href="../css/ManualControl.css" rel="stylesheet" />
    <link href="../css/checkboxStyle.css" rel="stylesheet" />

   
<%--    <script src="../Scripts/bootstrap.min.js"></script>--%>

     <style type="text/css">    
        #divUserDetail{
            padding: 20px;
            color: inherit;
            background-color: #eee;
            width: 100%;
            border-radius: 10px;
            border: solid;
            border-color: #eee;
            margin-top: 30px;
        }

        #DetailDiv{
            margin-bottom: 30px;
            color: inherit;
            background-color: #eee;
            width: 72%;
            border-radius: 10px;
            border: solid;
            border-color: #eee;
            margin-top: 30px;
        }

         h2 {
            margin-left: 20px;
            margin-bottom: 25px;
         }

         table {
            margin-left: 60px;
         }

         #txtPreficSchool {
            margin-left: 42px;
            width: 55px;
         }

         #txtPassword{
            margin-left: -28px;
         }
         
         #txtLastName {
             margin-left: -84px;
         }
         .divRDB {
            padding-left: 18px;
            line-height: 7px;
            padding-right: 25px;
         }

         #rdbAllSubject,#rdbSelectSubject {
                 margin-top: -4px;
         }

         .radio label {
            padding-left: 10px;
            line-height: 17px;
         }
         .controlTextStyle {
             margin-left: 25px;
         }

         .radioFilter {
             padding-right: 25px;
         }

         #MainDiv td {
            line-height: 40px;
         }

         #lblSelectSubjectClassHeader {
             margin-bottom: 25px;
         }

         #tdSelectSubjectClass {
             width: 76.5%;
         }

         #btnOK, #btnOK2 {
            float: right;
            width: 120px;
            line-height: 0px;
         }

         #lblChkIsContact {
             line-height: 10px;
         }

         #txtUserName {
             width: 306px;
         }

     </style>

     <script src="/Scripts/jquery-1.10.2.min.js" type="text/javascript"></script>
<%--     <script src="../Scripts/jquery-1.7.1.min.js" type="text/javascript"></script>
     <script src="../Scripts/jquery-ui-1.8.18.js" type="text/javascript"></script>
    <script type="text/javascript" src="/javascript/main.js"></script>--%>

    <script type="text/javascript">
        $(function () {

            $("#btnOK2").click(function () {
                alert('1234');
                var name = "my name";
                var dataValue = { "InputKey": name };                                                                                                 
                $.ajax({
                    type: "POST",
                    url: "UserPage.aspx/GetCurrentTime",
                    data: dataValue,
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
                    },
                    success: function (result) {
                        alert("We returned: " + result);
                    }
                });
            });
        });
    </script>
          
</head>
<body>
    <form id="form1" runat="server">
        <div id="MainDiv" class="container">
            <div id="divUserDetail">
                <h2 id="HeaderDetailText" runat="server">เพิ่มข้อมูลผู้ใช้ สังกัดโรงเรียน : โพธิสารพิทยากร</h2>
                    <table>
                        <tr>
                            <td>
                                <asp:HiddenField id="IsAllowUpdateSchoolShortName" runat="server" value="1"/>
                                <label for="txtFirstName">ชื่อ - นามสกุล</label>
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txtFirstName"  placeholder="ชื่อจริง" runat="server" CssClass="ControlTextStyle"></asp:TextBox>
                            </td>
                             <td colspan="2">
                                <asp:TextBox ID="txtLastName"  placeholder="นามสกุล" runat="server" CssClass="ControlTextStyle"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label for="txtUserName">ชื่อผู้ใช้</label>
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txtUserName"  placeholder="ชื่อผู้ใช้" runat="server" CssClass="ControlTextStyle"></asp:TextBox>
                            </td>
                            <td>
                                <asp:CheckBox ID="chkIsContact" runat="server" Text=" " CssClass="checkbox checkbox-warning" />
                            </td>
                            <td>
                                <label id="lblChkIsContact" for="chkIsContact">กำหนดให้เป็นผู้ติดต่อปัจจุบัน</label>
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td colspan="4">
                                <label id="lblPasswordWarning" style="color:red;display:none;">** ถ้าไม่ต้องการแก้ไขรหัสผ่าน&nbsp;<b><i>ไม่ต้องกรอก</i></b>&nbsp;ช่องรหัสผ่านนะคะ</label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label for="txtPreficSchool">รหัสผ่าน</label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPreficSchool"  placeholder="ตัวย่อ" runat="server" CssClass="ControlTextStyle"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPassword"  placeholder="รหัสผ่าน" runat="server" CssClass="ControlTextStyle"></asp:TextBox>
                            </td>
                            <td> </td> <td> </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td>
                                <label for="rdbAllSubject">สิทธิ์การใช้งาน</label>
                            </td>
                            <td colspan="2" Class="divRDB">
                                <asp:RadioButton ID="rdbAllSubject" GroupName="rdSelectSubject" runat="server" Text="ทุกชั้น - วิชา" AutoPostBack="true"  CssClass="radio radio-warning radio-inline" Checked="true" />
                            </td>
                            <td id="tdSelectSubjectClass" colspan="2" Class="divRDB">
                                <asp:RadioButton ID="rdbSelectSubject" GroupName="rdSelectSubject" runat="server" Text="กำหนดเอง" AutoPostBack="true"  CssClass="radio radio-warning radio-inline" Checked="false" />
                                <asp:Button ID="btnOK" runat="server" Text="บันทึก" CssClass="ControlButton" />
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <div id="DetailDiv" runat="server" class="container">
                
              <%--  <div runat="server" id="AlertBox" class="alertBox" Visible="false">
                
                    <div runat="server" id="AlertBoxMessage"></div>
                    <button onclick="closeAlert.call(this, event)">Ok</button>
                </div>--%>
                
                <div class="jumbotron">
                    <div class="form-group subjectsClass" data-toggle="buttons">
                        <label id="lblSelectSubjectClassHeader">เลือกชั้นและวิชาที่ต้องการใช้</label>
                        <br />
                        <div class="btn-group">
                            <asp:RadioButton ID="radioP" Text="ประถมอย่างเดียว" GroupName="radioClass" runat="server" AutoPostBack="true" CssClass="radio radio-warning radio-inline" />
                            <asp:RadioButton ID="radioM" Text="มัธยมอย่างเดียว" GroupName="radioClass" runat="server" AutoPostBack="true" CssClass="radio radio-warning radio-inline" />
                            <asp:RadioButton ID="radioPM" Text="ประถม + มัธยม" GroupName="radioClass" runat="server" AutoPostBack="true" Checked="true"  CssClass="radio radio-warning radio-inline" />
                        </div>
                        <br />
                        <br />
                        <asp:Panel ID="PanelUserSubjectClass" runat="server">
                            <table class="table table-bordered">
                                <tr>
                                    <td>ชั้น</td>
                                    <td colspan="2">
                                        <asp:CheckBox ID="ChkAll" AutoPostBack="true" Text="เลือกทั้งหมด" CssClass="checkbox checkbox-warning" runat="server" />
                                    </td>
                                    <td colspan="6" style="text-align: center;">วิชา</td>
                                </tr>

                                <asp:Panel ID="PrimaryPanel" runat="server">
                                    <tr>
                                        <td>ป.1</td>
                                        <td>
                                            <asp:CheckBox ID="C4_1" ToolTip="thai-K4" Text="ไทย" runat="server" CssClass="checkbox checkbox-warning" /></td>
                                        <td>
                                            <asp:CheckBox ID="C4_2" ToolTip="social-K4" Text="สังคม" runat="server" CssClass="checkbox checkbox-warning" /></td>
                                        <td>
                                            <asp:CheckBox ID="C4_3" ToolTip="math-K4" Text="เลข" runat="server" CssClass="checkbox checkbox-warning" /></td>
                                        <td>
                                            <asp:CheckBox ID="C4_4" ToolTip="science-K4" Text="วิทย์" runat="server" CssClass="checkbox checkbox-warning" /></td>
                                        <td>
                                            <asp:CheckBox ID="C4_5" ToolTip="eng-K4" Text="อังกฤษ" runat="server" CssClass="checkbox checkbox-warning" /></td>
                                        <td>
                                            <asp:CheckBox ID="C4_6" ToolTip="health-K4" Text="สุขศึกษา" runat="server" CssClass="checkbox checkbox-warning" /></td>
                                        <td>
                                            <asp:CheckBox ID="C4_7" ToolTip="art-K4" runat="server" Text="ศิลปะ" CssClass="checkbox checkbox-warning" /></td>
                                        <td>
                                            <asp:CheckBox ID="C4_8" ToolTip="career-K4" Text="การงานฯ" runat="server" CssClass="checkbox checkbox-warning" /></td>
                                    </tr>
                                    <tr>
                                        <td>ป.2</td>
                                        <td>
                                            <asp:CheckBox ID="C5_1" ToolTip="thai-K5" Text="ไทย" runat="server" CssClass="checkbox checkbox-warning" /></td>
                                        <td>
                                            <asp:CheckBox ID="C5_2" ToolTip="social-K5" Text="สังคม" runat="server" CssClass="checkbox checkbox-warning" /></td>
                                        <td>
                                            <asp:CheckBox ID="C5_3" ToolTip="math-K5" Text="เลข" runat="server" CssClass="checkbox checkbox-warning" /></td>
                                        <td>
                                            <asp:CheckBox ID="C5_4" ToolTip="science-K5" Text="วิทย์" runat="server" CssClass="checkbox checkbox-warning" /></td>
                                        <td>
                                            <asp:CheckBox ID="C5_5" ToolTip="eng-K5" Text="อังกฤษ" runat="server" CssClass="checkbox checkbox-warning" /></td>
                                        <td>
                                            <asp:CheckBox ID="C5_6" ToolTip="health-K5" Text="สุขศึกษา" runat="server" CssClass="checkbox checkbox-warning" /></td>
                                        <td>
                                            <asp:CheckBox ID="C5_7" ToolTip="art-K5" runat="server" Text="ศิลปะ" CssClass="checkbox checkbox-warning" /></td>
                                        <td>
                                            <asp:CheckBox ID="C5_8" ToolTip="career-K5" Text="การงานฯ" runat="server" CssClass="checkbox checkbox-warning" /></td>
                                    </tr>
                                    <tr>
                                        <td>ป.3</td>
                                        <td>
                                            <asp:CheckBox ID="C6_1" ToolTip="thai-K6" Text="ไทย" runat="server" CssClass="checkbox checkbox-warning" /></td>
                                        <td>
                                            <asp:CheckBox ID="C6_2" ToolTip="social-K6" Text="สังคม" runat="server" CssClass="checkbox checkbox-warning" /></td>
                                        <td>
                                            <asp:CheckBox ID="C6_3" ToolTip="math-K6" Text="เลข" runat="server" CssClass="checkbox checkbox-warning" /></td>
                                        <td>
                                            <asp:CheckBox ID="C6_4" ToolTip="science-K6" Text="วิทย์" runat="server" CssClass="checkbox checkbox-warning" /></td>
                                        <td>
                                            <asp:CheckBox ID="C6_5" ToolTip="eng-K6" Text="อังกฤษ" runat="server" CssClass="checkbox checkbox-warning" /></td>
                                        <td>
                                            <asp:CheckBox ID="C6_6" ToolTip="health-K6" Text="สุขศึกษา" runat="server" CssClass="checkbox checkbox-warning" /></td>
                                        <td>
                                            <asp:CheckBox ID="C6_7" ToolTip="art-K6" runat="server" Text="ศิลปะ" CssClass="checkbox checkbox-warning" /></td>
                                        <td>
                                            <asp:CheckBox ID="C6_8" ToolTip="career-K6" Text="การงานฯ" runat="server" CssClass="checkbox checkbox-warning" /></td>
                                    </tr>
                                    <tr>
                                        <td>ป.4</td>
                                        <td>
                                            <asp:CheckBox ID="C7_1" ToolTip="thai-K7" Text="ไทย" runat="server" CssClass="checkbox checkbox-warning" /></td>
                                        <td>
                                            <asp:CheckBox ID="C7_2" ToolTip="social-K7" Text="สังคม" runat="server" CssClass="checkbox checkbox-warning" /></td>
                                        <td>
                                            <asp:CheckBox ID="C7_3" ToolTip="math-K7" Text="เลข" runat="server" CssClass="checkbox checkbox-warning" /></td>
                                        <td>
                                            <asp:CheckBox ID="C7_4" ToolTip="science-K7" Text="วิทย์" runat="server" CssClass="checkbox checkbox-warning" /></td>
                                        <td>
                                            <asp:CheckBox ID="C7_5" ToolTip="eng-K7" Text="อังกฤษ" runat="server" CssClass="checkbox checkbox-warning" /></td>
                                        <td>
                                            <asp:CheckBox ID="C7_6" ToolTip="health-K7" Text="สุขศึกษา" runat="server" CssClass="checkbox checkbox-warning" /></td>
                                        <td>
                                            <asp:CheckBox ID="C7_7" ToolTip="art-K7" runat="server" Text="ศิลปะ" CssClass="checkbox checkbox-warning" /></td>
                                        <td>
                                            <asp:CheckBox ID="C7_8" ToolTip="career-K7" Text="การงานฯ" runat="server" CssClass="checkbox checkbox-warning" /></td>
                                    </tr>
                                    <tr>
                                        <td>ป.5</td>
                                        <td>
                                            <asp:CheckBox ID="C8_1" ToolTip="thai-K8" Text="ไทย" runat="server" CssClass="checkbox checkbox-warning" /></td>
                                        <td>
                                            <asp:CheckBox ID="C8_2" ToolTip="social-K8" Text="สังคม" runat="server" CssClass="checkbox checkbox-warning" /></td>
                                        <td>
                                            <asp:CheckBox ID="C8_3" ToolTip="math-K8" Text="เลข" runat="server" CssClass="checkbox checkbox-warning" /></td>
                                        <td>
                                            <asp:CheckBox ID="C8_4" ToolTip="science-K8" Text="วิทย์" runat="server" CssClass="checkbox checkbox-warning" /></td>
                                        <td>
                                            <asp:CheckBox ID="C8_5" ToolTip="eng-K8" Text="อังกฤษ" runat="server" CssClass="checkbox checkbox-warning" /></td>
                                        <td>
                                            <asp:CheckBox ID="C8_6" ToolTip="health-K8" Text="สุขศึกษา" runat="server" CssClass="checkbox checkbox-warning" /></td>
                                        <td>
                                            <asp:CheckBox ID="C8_7" ToolTip="art-K8" runat="server" Text="ศิลปะ" CssClass="checkbox checkbox-warning" /></td>
                                        <td>
                                            <asp:CheckBox ID="C8_8" ToolTip="career-K8" Text="การงานฯ" runat="server" CssClass="checkbox checkbox-warning" /></td>
                                    </tr>
                                    <tr>
                                        <td>ป.6</td>
                                        <td>
                                            <asp:CheckBox ID="C9_1" ToolTip="thai-K9" Text="ไทย" runat="server" CssClass="checkbox checkbox-warning" /></td>
                                        <td>
                                            <asp:CheckBox ID="C9_2" ToolTip="social-K9" Text="สังคม" runat="server" CssClass="checkbox checkbox-warning" /></td>
                                        <td>
                                            <asp:CheckBox ID="C9_3" ToolTip="math-K9" Text="เลข" runat="server" CssClass="checkbox checkbox-warning" /></td>
                                        <td>
                                            <asp:CheckBox ID="C9_4" ToolTip="science-K9" Text="วิทย์" runat="server" CssClass="checkbox checkbox-warning" /></td>
                                        <td>
                                            <asp:CheckBox ID="C9_5" ToolTip="eng-K9" Text="อังกฤษ" runat="server" CssClass="checkbox checkbox-warning" /></td>
                                        <td>
                                            <asp:CheckBox ID="C9_6" ToolTip="health-K9" Text="สุขศึกษา" runat="server" CssClass="checkbox checkbox-warning" /></td>
                                        <td>
                                            <asp:CheckBox ID="C9_7" ToolTip="art-K9" runat="server" Text="ศิลปะ" CssClass="checkbox checkbox-warning" /></td>
                                        <td>
                                            <asp:CheckBox ID="C9_8" ToolTip="career-K9" Text="การงานฯ" runat="server" CssClass="checkbox checkbox-warning" /></td>
                                    </tr>
                                </asp:Panel>
                                
                                <asp:Panel ID="MiddlePanel" runat="server">
                                    <tr>
                                        <td>ม.1</td>
                                        <td>
                                            <asp:CheckBox ID="C10_1" ToolTip="thai-K10" Text="ไทย" runat="server" CssClass="checkbox checkbox-warning" /></td>
                                        <td>
                                            <asp:CheckBox ID="C10_2" ToolTip="social-K10" Text="สังคม" runat="server" CssClass="checkbox checkbox-warning" /></td>
                                        <td>
                                            <asp:CheckBox ID="C10_3" ToolTip="math-K10" Text="เลข" runat="server" CssClass="checkbox checkbox-warning" /></td>
                                        <td>
                                            <asp:CheckBox ID="C10_4" ToolTip="science-K10" Text="วิทย์" runat="server" CssClass="checkbox checkbox-warning" /></td>
                                        <td>
                                            <asp:CheckBox ID="C10_5" ToolTip="eng-K10" Text="อังกฤษ" runat="server" CssClass="checkbox checkbox-warning" /></td>
                                        <td>
                                            <asp:CheckBox ID="C10_6" ToolTip="health-K10" Text="สุขศึกษา" runat="server" CssClass="checkbox checkbox-warning" /></td>
                                        <td>
                                            <asp:CheckBox ID="C10_7" ToolTip="art-K10" runat="server" Text="ศิลปะ" CssClass="checkbox checkbox-warning" /></td>
                                        <td>
                                            <asp:CheckBox ID="C10_8" ToolTip="career-K10" Text="การงานฯ" runat="server" CssClass="checkbox checkbox-warning" /></td>
                                    </tr>
                                    <tr>
                                        <td>ม.2</td>
                                        <td>
                                            <asp:CheckBox ID="C11_1" ToolTip="thai-K11" Text="ไทย" runat="server" CssClass="checkbox checkbox-warning" /></td>
                                        <td>
                                            <asp:CheckBox ID="C11_2" ToolTip="social-K11" Text="สังคม" runat="server" CssClass="checkbox checkbox-warning" /></td>
                                        <td>
                                            <asp:CheckBox ID="C11_3" ToolTip="math-K11" Text="เลข" runat="server" CssClass="checkbox checkbox-warning" /></td>
                                        <td>
                                            <asp:CheckBox ID="C11_4" ToolTip="science-K11" Text="วิทย์" runat="server" CssClass="checkbox checkbox-warning" /></td>
                                        <td>
                                            <asp:CheckBox ID="C11_5" ToolTip="eng-K11" Text="อังกฤษ" runat="server" CssClass="checkbox checkbox-warning" /></td>
                                        <td>
                                            <asp:CheckBox ID="C11_6" ToolTip="health-K11" Text="สุขศึกษา" runat="server" CssClass="checkbox checkbox-warning" /></td>
                                        <td>
                                            <asp:CheckBox ID="C11_7" ToolTip="art-K11" runat="server" Text="ศิลปะ" CssClass="checkbox checkbox-warning" /></td>
                                        <td>
                                            <asp:CheckBox ID="C11_8" ToolTip="career-K11" Text="การงานฯ" runat="server" CssClass="checkbox checkbox-warning" /></td>
                                    </tr>
                                    <tr>
                                        <td>ม.3</td>
                                        <td>
                                            <asp:CheckBox ID="C12_1" ToolTip="thai-K12" Text="ไทย" runat="server" CssClass="checkbox checkbox-warning" /></td>
                                        <td>
                                            <asp:CheckBox ID="C12_2" ToolTip="social-K12" Text="สังคม" runat="server" CssClass="checkbox checkbox-warning" /></td>
                                        <td>
                                            <asp:CheckBox ID="C12_3" ToolTip="math-K12" Text="เลข" runat="server" CssClass="checkbox checkbox-warning" /></td>
                                        <td>
                                            <asp:CheckBox ID="C12_4" ToolTip="science-K12" Text="วิทย์" runat="server" CssClass="checkbox checkbox-warning" /></td>
                                        <td>
                                            <asp:CheckBox ID="C12_5" ToolTip="eng-K12" Text="อังกฤษ" runat="server" CssClass="checkbox checkbox-warning" /></td>
                                        <td>
                                            <asp:CheckBox ID="C12_6" ToolTip="health-K12" Text="สุขศึกษา" runat="server" CssClass="checkbox checkbox-warning" /></td>
                                        <td>
                                            <asp:CheckBox ID="C12_7" ToolTip="art-K12" runat="server" Text="ศิลปะ" CssClass="checkbox checkbox-warning" /></td>
                                        <td>
                                            <asp:CheckBox ID="C12_8" ToolTip="career-K12" Text="การงานฯ" runat="server" CssClass="checkbox checkbox-warning" /></td>
                                    </tr>
                                    <tr>
                                        <td>ม.4</td>
                                        <td>
                                            <asp:CheckBox ID="C13_1" ToolTip="thai-K13" Text="ไทย" runat="server" CssClass="checkbox checkbox-warning" /></td>
                                        <td>
                                            <asp:CheckBox ID="C13_2" ToolTip="social-K13" Text="สังคม" runat="server" CssClass="checkbox checkbox-warning" /></td>
                                        <td>
                                            <asp:CheckBox ID="C13_3" ToolTip="math-K13" Text="เลข" runat="server" CssClass="checkbox checkbox-warning" /></td>
                                        <td>
                                            <asp:CheckBox ID="C13_4" ToolTip="science-K13" Text="วิทย์" runat="server" CssClass="checkbox checkbox-warning" /></td>
                                        <td>
                                            <asp:CheckBox ID="C13_5" ToolTip="eng-K13" Text="อังกฤษ" runat="server" CssClass="checkbox checkbox-warning" /></td>
                                        <td>
                                            <asp:CheckBox ID="C13_6" ToolTip="health-K13" Text="สุขศึกษา" runat="server" CssClass="checkbox checkbox-warning" /></td>
                                        <td>
                                            <asp:CheckBox ID="C13_7" ToolTip="art-K13" runat="server" Text="ศิลปะ" CssClass="checkbox checkbox-warning" /></td>
                                        <td>
                                            <asp:CheckBox ID="C13_8" ToolTip="career-K13" Text="การงานฯ" runat="server" CssClass="checkbox checkbox-warning" /></td>
                                    </tr>
                                    <tr>
                                        <td>ม.5</td>
                                        <td>
                                            <asp:CheckBox ID="C14_1" ToolTip="thai-K14" Text="ไทย" runat="server" CssClass="checkbox checkbox-warning" /></td>
                                        <td>
                                            <asp:CheckBox ID="C14_2" ToolTip="social-K14" Text="สังคม" runat="server" CssClass="checkbox checkbox-warning" /></td>
                                        <td>
                                            <asp:CheckBox ID="C14_3" ToolTip="math-K14" Text="เลข" runat="server" CssClass="checkbox checkbox-warning" /></td>
                                        <td>
                                            <asp:CheckBox ID="C14_4" ToolTip="science-K14" Text="วิทย์" runat="server" CssClass="checkbox checkbox-warning" /></td>
                                        <td>
                                            <asp:CheckBox ID="C14_5" ToolTip="eng-K14" Text="อังกฤษ" runat="server" CssClass="checkbox checkbox-warning" /></td>
                                        <td>
                                            <asp:CheckBox ID="C14_6" ToolTip="health-K14" Text="สุขศึกษา" runat="server" CssClass="checkbox checkbox-warning" /></td>
                                        <td>
                                            <asp:CheckBox ID="C14_7" ToolTip="art-K14" runat="server" Text="ศิลปะ" CssClass="checkbox checkbox-warning" /></td>
                                        <td>
                                            <asp:CheckBox ID="C14_8" ToolTip="career-K14" Text="การงานฯ" runat="server" CssClass="checkbox checkbox-warning" /></td>
                                    </tr>
                                    <tr>
                                        <td>ม.6</td>
                                        <td>
                                            <asp:CheckBox ID="C15_1" ToolTip="thai-K15" Text="ไทย" runat="server" CssClass="checkbox checkbox-warning" /></td>
                                        <td>
                                            <asp:CheckBox ID="C15_2" ToolTip="social-K15" Text="สังคม" runat="server" CssClass="checkbox checkbox-warning" /></td>
                                        <td>
                                            <asp:CheckBox ID="C15_3" ToolTip="math-K15" Text="เลข" runat="server" CssClass="checkbox checkbox-warning" /></td>
                                        <td>
                                            <asp:CheckBox ID="C15_4" ToolTip="science-K15" Text="วิทย์" runat="server" CssClass="checkbox checkbox-warning" /></td>
                                        <td>
                                            <asp:CheckBox ID="C15_5" ToolTip="eng-K15" Text="อังกฤษ" runat="server" CssClass="checkbox checkbox-warning" /></td>
                                        <td>
                                            <asp:CheckBox ID="C15_6" ToolTip="health-K15" Text="สุขศึกษา" runat="server" CssClass="checkbox checkbox-warning" /></td>
                                        <td>
                                            <asp:CheckBox ID="C15_7" ToolTip="art-K15" runat="server" Text="ศิลปะ" CssClass="checkbox checkbox-warning" /></td>
                                        <td>
                                            <asp:CheckBox ID="C15_8" ToolTip="career-K15" Text="การงานฯ" runat="server" CssClass="checkbox checkbox-warning" /></td>
                                    </tr>
                                </asp:Panel>
                            </table>
                        </asp:Panel>
                        <div>
                            <input type="button" id="btnOK2" value="บันทึก" class="ControlButton" />
                            <%-- <asp:Button ID="btnOK2" runat="server" Text="บันทึก" CssClass="ControlButton" />--%>
                        </div>
                    </div>
                </div>
            </div>
            <div id="dialogWarning" title="แจ้งเตือน">
                <div style="padding: 10px;">
                    <span>ใส่รหัสไม่ครบค่ะ!!</span>
                </div>
            </div>
        </form>
    </body>
</html>
