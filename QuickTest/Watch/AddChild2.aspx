<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="AddChild2.aspx.vb" Inherits="QuickTest.AddChild2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../css/jquery-ui-1.8.18.custom.min.css" rel="stylesheet" />
    <style type="text/css">
        @import url(../fonts/thsarabunnew.css);

        body {
            font: normal 0.95em 'THSarabunNew';
            color: #444;
        }

        div.main {
            width: 90%;
            margin-left: auto;
            margin-right: auto;
            margin-top:90px;
        }

            div.main > table {
                width: 90%;
            }

                div.main > table td:first-child {
                    width: 50%;
                    text-align: right;
                    padding-right: 10px;
                    padding-bottom: 7.5px;
                    padding-top:7.5px;
                }

                div.main > table span {
                    font-size: 20px;
                }

        .btn {
            font: 100% 'THSarabunNew';
            border: 0;
            padding: 2px 0 3px 0;
            cursor: pointer;
            border-radius: .5em;
            box-shadow: 0 1px 2px rgba(0,0,0,.2);
            color: #FFF;
            border: solid 1px #0D8AA9;
            background: -webkit-gradient(linear, left top, left bottom, from(#63CFDF), to(#17B2D9));
            text-shadow: 1px 1px #178497;
            height: 60px;
            font-size: 25px;
            margin-top: 20px;
        }

        .txtBox {
            padding: 1px;            
            font: 20px 'THSarabunNew';
            border: 1px solid #C6E7F0;
            background: #EFF8FB;
            width:250px;
        }
    </style>
    <script src="../js/jquery-1.7.1.min.js"></script>
    <script src="../js/jquery-ui-1.8.18.min.js"></script>
    <script type="text/javascript">
        var DeviceId = '<%=DeviceId%>';
        var SendPage = '<%=Sendpage%>';
        $(function () {
            $('#dialogInfo').dialog({
                autoOpen: false, draggable: false, resizable: false, modal: true, width: 'auto', buttons: {
                    'ตกลง': function () {
                        $(this).dialog('close');
                    }
                }
            });

            $("#btnRegister").click(function (e) {
              
                if ($('#txtSchoolCode').val() == "") {
                    e.preventDefault();
                    callDialog("ใส่รหัสโรงเรียนด้วยค่ะ");
                    return false;
                }
                if ($('#txtParentCode').val() == "") {
                    e.preventDefault();
                    callDialog("ใส่รหัสลงทะเบียนด้วยค่ะ");
                    return false;
                }
                if ($('#txtParentName').val() == "") {
                    e.preventDefault();
                    callDialog("ใส่ชื่อด้วยค่ะ");
                    return false;
                }
                if ($('#txtParentLastName').val() == "") {
                    e.preventDefault();
                    callDialog("ใส่นามสกุลด้วยค่ะ");
                    return false;
                }
                if ($('#txtParentPhone').val() == "") {
                    e.preventDefault();
                    callDialog("ใส่เบอร์โทรศัพท์ด้วยค่ะ");
                    return false;
                }
                
                var registered = CheckPwdRegister($('#txtSchoolCode').val(), $('#txtParentCode').val());
                if (registered == "Registered") {
                    e.preventDefault();
                    callDialog("เพิ่มนักเรียนในเครื่องนี้ไปแล้วค่ะ");
                    return false;
                } else if (registered == "False") {
                    e.preventDefault();
                    callDialog("ใส่รหัสลงทะเบียนไม่ถูกต้องค่ะ");
                    return false;
                } else {
                    return true;
                }
                         
            });

            $('html').click(function () {
                //var p = window.location.href;
                var isRefresh = GetQueryString("addChild"); 
                if (isRefresh == null || isRefresh == "") {
                    if (CheckDeviceRegistered()) {
                        window.location = "../" + SendPage + "?DeviceId=" + DeviceId;
                    }
                }
            });
        });

        function GetQueryString(name) {            
            var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"), results = regex.exec(document.URL);
            return results == null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
        }

        function callDialog(txt) {
            $('#dialogInfo').dialog('option', 'title', txt).dialog('open');

        }

        function CheckPwdRegister(SchoolCode, ParentCode) {            
            var result;
            $.ajax({
                type: "POST",
                url: "<%=ResolveUrl("~")%>WebServices/UserPwdService.asmx/CheckPwdRegister",
                data: "{  SchoolCode: '" + SchoolCode + "',ParentCode: '" + ParentCode + "',DeviceId:'" + DeviceId + "' }",
                async: false,
                contentType: "application/json; charset=utf-8", dataType: "json",
                success: function (data) {                    
                    result = data.d;
                },
                error: function myfunction(request, status) { }
            });
            return result;
        }

        function CheckDeviceRegistered() {
            var result;
            $.ajax({
                type: "POST",
                url: "<%=ResolveUrl("~")%>WebServices/UserPwdService.asmx/CheckDeviceRegistered",
                data: "{  DeviceId:'" + DeviceId + "' }",
                async: false,
                contentType: "application/json; charset=utf-8", dataType: "json",
                success: function (data) {
                    result = data.d;
                },
                error: function myfunction(request, status) { }
            });
            return result;
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="main">
            <table>
                <tr>
                    <td><span>รหัสโรงเรียน</span></td>
                    <td>
                        <asp:TextBox ID="txtSchoolCode" runat="server" ClientIDMode="Static" CssClass="txtBox"></asp:TextBox></td>
                </tr>
                <tr>
                    <td><span>รหัสลงทะเบียน</span></td>
                    <td>
                        <asp:TextBox ID="txtParentCode" runat="server" ClientIDMode="Static" CssClass="txtBox"></asp:TextBox></td>
                </tr>

                <% If Request.QueryString("addChild") Is Nothing Then %>
                <tr>
                    <td><span>ชื่อ</span></td>
                    <td>
                        <asp:TextBox ID="txtParentName" runat="server" ClientIDMode="Static" CssClass="txtBox"></asp:TextBox></td>
                </tr>
                <tr>
                    <td><span>นามสกุล</span></td>
                    <td>
                        <asp:TextBox ID="txtParentLastName" runat="server" ClientIDMode="Static" CssClass="txtBox"></asp:TextBox></td>
                </tr>
                <tr>
                    <td><span>เบอร์โทร</span></td>
                    <td>
                        <asp:TextBox ID="txtParentPhone" runat="server" ClientIDMode="Static" CssClass="txtBox" MaxLength="10"  onkeypress="return event.charCode >= 48 && event.charCode <= 57"></asp:TextBox></td>
                </tr>
                <%End If%>

                <tr>
                    <td></td>
                    <td>
                        <asp:Button ID="btnRegister" runat="server" Text="ลงทะเบียน" ClientIDMode="Static" CssClass="btn" Width="200" /></td>
                </tr>
            </table>

        </div>
        <div id="dialogInfo"></div>
    </form>
</body>
</html>
