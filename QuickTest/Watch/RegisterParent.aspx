<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="RegisterParent.aspx.vb" Inherits="QuickTest.RegisterParent" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <style type="text/css">

        #MainDiv {
        width:100%;
        text-align:center;
        }

        .ForDivInFo {
        width:70%;
        margin-left:auto;
        margin-right:auto;
        text-align:center;
        margin-top:35px;
        border:1px solid;
        border-radius:5px;
        }
        #spnParentInfo {
        font-size:30px;
        font-weight:bold;
        position:relative;
        top:10px;
        }
        #tableParentInfo {
        width:100%;
        text-align:left;
        margin-top:40px;
        padding:10px;
        
        }
        .ForDivDropdown {
        width:90%;
        margin-left:auto;
        margin-right:auto;
        padding:10px;
        }
        #spnUploadPic {
        font-size:25px;
        position:relative;
        top:10px;
        }
        .forInputUploadPic {
        position:relative;
        margin-top:30px;
        margin-bottom:20px;
        }

        .Forbtn
        {
            font: 100% 'THSarabunNew';
            border: 0;
            padding: 2px 0 3px 0;
            cursor: pointer;
            background: #1EC9F4;
            -moz-border-radius: .5em;
            -webkit-border-radius: .5em;
            border-radius: .5em; /*behavior:url(border-radius.htc);*/
            -webkit-box-shadow: 0 1px 2px rgba(0,0,0,.2);
            -moz-box-shadow: 0 1px 2px rgba(0,0,0,.2);
            box-shadow: 0 1px 2px rgba(0,0,0,.2);
            color: #FFF;
            border: solid 1px #0D8AA9;
            background: #46C4DD;
            background: -webkit-gradient(linear, left top, left bottom, from(#63CFDF), to(#17B2D9));
            background: -moz-linear-gradient(top,  #63CFDF,  #17B2D9);
            text-shadow: 1px 1px #178497;
            behavior: url('../css/PIE.htc');
            -pie-track-active: false;
            width:128px;
            margin-top:20px;
            height:60px;
            width:200px;
            font-size:30px;
        }
    </style>

</head>
<body>
    <form id="form1" runat="server">


        <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
            <Scripts>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js">
                </asp:ScriptReference>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js">
                </asp:ScriptReference>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js">
                </asp:ScriptReference>
            </Scripts>
        </telerik:RadScriptManager>


    <div id="MainDiv">
    
        <div id="DivParentInFo" class="ForDivInFo">
            <span id="spnParentInfo">โปรดกรอกรายละเอียดของผู้ปกครอง</span>
            <table id="tableParentInfo">
                <tr>
                    <td>
                        <asp:Label ID="Label1" runat="server" Text="ชื่อผู้ปกครอง"></asp:Label>
                    </td>
                    <td>
                          <asp:TextBox ID="txtPRFirstName" runat="server"></asp:TextBox>
                      <asp:Label ID="lblValidateFirstName" runat="server" ForeColor="Red" Visible="false" Text="กรอกชื่อผู้ปกครอง"></asp:Label>
                    </td>

                    <td>
                        <asp:Label ID="Label2" runat="server" Text="นามสกุลผู้ปกครอง"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtPRLastName" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label3" runat="server" Text="เบอร์โทรผู้ปกครอง"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtPRPhone" runat="server"></asp:TextBox>
                         <asp:Label ID="lblValidatePhone" runat="server" ForeColor="Red" Visible="false" Text="กรอกเบอร์โทรศัพท์ผู้ปกครอง"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>

        <div id="DivStudentInFo" class="ForDivInFo">
            
            <div class="ForDivDropdown">
                <telerik:RadComboBox ID="RadComboBoxProvince" Label="จังหวัด" AutoPostBack="true" Width="300px"  Font-Size="Medium"  EmptyMessage="เลือกจังหวัด" runat="server"></telerik:RadComboBox>
                <asp:Label ID="lblValidateProvince" runat="server" ForeColor="Red"  Visible="false" Text="ต้องเลือกจังหวัดก่อน"></asp:Label>
            </div> 

            <div class="ForDivDropdown">
                <telerik:RadComboBox ID="RadComboBoxAmphur"  Enabled="false" Label="อำเภอ" Width="300px" Font-Size="Medium" AutoPostBack="true" EmptyMessage="เลือกอำเภอ" runat="server"></telerik:RadComboBox>
                <asp:Label ID="lblValidateAmphur" runat="server" ForeColor="Red"  Visible="false" Text="ต้องเลือกจังอำเภอก่อน"></asp:Label>
            </div>

            <div class="ForDivDropdown">
                <telerik:RadComboBox ID="RadComboBoxSchool" Enabled="false"  Label="โรงเรียน" Width="300px" Font-Size="Medium" AutoPostBack="true" EmptyMessage="เลือกโรงเรียน" runat="server"></telerik:RadComboBox>
                <asp:Label ID="lblValidateSchool" runat="server" ForeColor="Red"  Visible="false" Text="ต้องเลือกโรงเรียนก่อน"></asp:Label>
            </div>

            <div class="ForDivDropdown">
                <telerik:RadComboBox ID="RadComboBoxClass" Enabled="false" Label="ชั้น" Width="300px" Font-Size="Medium" AutoPostBack="true" EmptyMessage="เลือกชั้น" runat="server"></telerik:RadComboBox>
                <asp:Label ID="lblValidateClass" runat="server" ForeColor="Red"  Visible="false" Text="ต้องเลือกชั้นก่อน"></asp:Label>
            </div>

            <div class="ForDivDropdown">
                <telerik:RadComboBox ID="RadComboBoxRoom" Enabled="false" Label="ห้อง" Width="300px" Font-Size="Medium" AutoPostBack="true" EmptyMessage="เลือกห้อง" runat="server"></telerik:RadComboBox>
                <asp:Label ID="lblValidateRoom" runat="server" ForeColor="Red"  Visible="false" Text="ต้องเลือกห้องก่อน"></asp:Label>
            </div>

            <div class="ForDivDropdown">
                <telerik:RadComboBox ID="RadComboBoxStudent" Enabled="false" Label="ชื่อนักเรียน" Width="300px" Font-Size="Medium"  EmptyMessage="เลือกนักเรียน" runat="server"></telerik:RadComboBox>
                <asp:Label ID="lblValidateStudent" runat="server" ForeColor="Red"  Visible="false" Text="ต้องเลือกนักเรียนก่อน"></asp:Label>
            </div>
            <hr />
            <div id="DivUploadPic">
                <span id="spnUploadPic">คลิกปุ่มด้านล่างเพื่อนถ่ายรูปกับลูกของคุณ(หรือเลือกรุปที่ถ่ายกับลูก)</span>
                <br />
                <asp:FileUpload ID="FileUpload1"  CssClass="forInputUploadPic" accept="image/*"  runat="server" />
                <asp:Label ID="lblValidateUploadPic" runat="server" ForeColor="Red"  Visible="false" Text="ต้องถ่ายรูปหรือเลือกรูปที่ถ่ายกับนักเรียนก่อน"></asp:Label>
            </div>

        </div>

        <asp:Button ID="btnSave" CssClass="Forbtn" runat="server" Text="ตกลง" />

    </div>


        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
            <AjaxSettings>

                <telerik:AjaxSetting AjaxControlID="RadComboBoxProvince">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="RadComboBoxAmphur" />
                        <telerik:AjaxUpdatedControl ControlID="RadComboBoxSchool" />
                        <telerik:AjaxUpdatedControl ControlID="RadComboBoxClass" />
                        <telerik:AjaxUpdatedControl ControlID="RadComboBoxRoom" />
                        <telerik:AjaxUpdatedControl ControlID="RadComboRadComboBoxStudentBoxSchool" />
                    </UpdatedControls>
                </telerik:AjaxSetting>

                <telerik:AjaxSetting AjaxControlID="RadComboBoxAmphur">
                    <UpdatedControls>
                       <telerik:AjaxUpdatedControl ControlID="RadComboBoxSchool" />
                        <telerik:AjaxUpdatedControl ControlID="RadComboBoxClass" />
                        <telerik:AjaxUpdatedControl ControlID="RadComboBoxRoom" />
                        <telerik:AjaxUpdatedControl ControlID="RadComboRadComboBoxStudentBoxSchool" />
                        </UpdatedControls>
                </telerik:AjaxSetting>

                <telerik:AjaxSetting AjaxControlID="RadComboBoxSchool">
                    <UpdatedControls>
                         <telerik:AjaxUpdatedControl ControlID="RadComboBoxClass" />
                        <telerik:AjaxUpdatedControl ControlID="RadComboBoxRoom" />
                        <telerik:AjaxUpdatedControl ControlID="RadComboRadComboBoxStudentBoxSchool" />
                    </UpdatedControls>
                </telerik:AjaxSetting>

                <telerik:AjaxSetting AjaxControlID="RadComboBoxClass">
                    <UpdatedControls>
                           <telerik:AjaxUpdatedControl ControlID="RadComboBoxRoom" />
                        <telerik:AjaxUpdatedControl ControlID="RadComboRadComboBoxStudentBoxSchool" />
                    </UpdatedControls>
                </telerik:AjaxSetting>

                <telerik:AjaxSetting AjaxControlID="RadComboBoxRoom">
                    <UpdatedControls>
                         <telerik:AjaxUpdatedControl ControlID="RadComboBoxStudent" />
                    </UpdatedControls>
                </telerik:AjaxSetting>

            </AjaxSettings>
        </telerik:RadAjaxManager>

        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" />


    </form>
</body>
</html>
