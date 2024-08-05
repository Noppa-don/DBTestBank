<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="UserMainPage.aspx.vb" Inherits="QuickTestGenUserData.UserMainPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    
    <link href="../css/bootstrap.min.css" rel="stylesheet" />
    <link href="../css/bootstrap-theme.min.css" rel="stylesheet" />
    <link href="../css/ManualControl.css" rel="stylesheet" />
    
    <script src="../Script/jquery-1.10.2.min.js"></script>
    <script src="../Script/bootstrap.min.js"></script>
     
    <style type="text/css">    
        #divSearch,#divUserInSchool{
            margin-bottom: 30px;
            color: inherit;
            background-color: #eee;
            width: 100%;
            border-radius: 10px;
            border: solid;
            border-color: #eee;
            margin: 0 auto;
            margin-top: 30px;
        }
        #divUserInSchool {
            padding: 10px;
        }

        h2 {
            text-align: center;
            margin-bottom: 40px;
        }

         #btnSearch {
            width: 230px;
         }

        #txtSchoolName {
            width: 356px;
            margin-left: -25px;
        }

        #btnAddUser {
            width: 100px;
            float: right;
            margin-bottom: 10px;
        }

        #lblSchoolSelected {
            font-size: 18px;
        }

        th {
            text-align:center;
            background-color:#1d4890;
            color:white;
        }
        .table-bordered > tbody > tr > td {
            border: 1px solid #286090;
        }
     </style>

    <script type="text/javascript">
        //$(function () {



        //    var opts = $('#ddlSchool option').map(function () {
        //        return [[this.value, $(this).text()]];
        //    });

        //    $('#txtSchoolName').keyup(function () {
        //        var rxp = new RegExp($('#txtSchoolName').val(), 'i');
        //        var optlist = $('#ddlSchool').empty();
        //        opts.each(function () {
        //            if (rxp.test(this[1])) {
        //                optlist.append($('<option/>').attr('value', this[0]).text(this[1]));
        //            }
        //        });
        //    });
        //});
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div id="MainDiv" class="container">
        <div id="divSearch">
            <h2 id="hederKeyTxt" runat="server">ค้นหาผู้ใช้ระบบ</h2>
            <table style="margin: 0 auto;">
                <tr>
                    <td>
                        <label for="ddlProvince">จังหวัด</label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlProvince" runat="server" AutoPostBack="true" CssClass="ControlTextStyle" ClientIDMode="Static"></asp:DropDownList>
                    </td>
                     <td>
                       <label for="ddlAmphur">เขต/อำเภอ</label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlAmphur" runat="server" AutoPostBack="true" CssClass="ControlTextStyle" ClientIDMode="Static"></asp:DropDownList>
                    </td>
                     <td>
                          <label for="ddlTambol">แขวง/ตำบล</label>                        
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlTambol" runat="server" AutoPostBack="true" CssClass="ControlTextStyle" ClientIDMode="Static"></asp:DropDownList>
                    </td>
                </tr>
                <tr>

                     <td>
                        <label for="ddlSchool">ชื่อโรงเรียน</label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlSchool" runat="server" AutoPostBack="true" Visible="true" CssClass="ControlTextStyle" ClientIDMode="Static"></asp:DropDownList>
                    </td>
                   <%--  <td>
                        <label for="txtUserName">ชื่อครูที่ติดต่อ</label>
                    </td>--%>
                    <td colspan="2">
                        <asp:TextBox ID="txtSchoolName"  placeholder="ชื่อโรงเรียน" runat="server" CssClass="ControlTextStyle"></asp:TextBox>
                    </td>
                    <td colspan="2" style="text-align:center;">
                        <asp:Button ID="btnSearch" runat="server" Text="ค้นหา" CssClass="ControlButton" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div id="DetailDiv" runat="server" class="container" style="display:none;">
        <div id="divUserInSchool">
            <label id="lblSchoolSelected" runat="server">ผู้ใช้ระบบของโรงเรียน : โพธิสารพิทยากร</label>
            <asp:Button ID="btnAddUser" runat="server" Text="เพิ่มผู้ใช้" CssClass="ControlButton" />
              <asp:GridView ID="gvUser" runat="server" AllowPaging="true" PageSize="20" AutoGenerateColumns="false" CssClass="table table-bordered table-hover" ClientIDMode="Static">
            <Columns>
                <asp:BoundField Visible="false" DataField="UserId" HeaderText="รหัส" />
                <asp:BoundField DataField="FullName" HeaderText="ชื่อ - นามสกุล" />
                <asp:BoundField DataField="UserName" HeaderText="ชื่อผู้ใช้" />
            </Columns>
            <PagerStyle HorizontalAlign="Right" CssClass="pagination-ys" />
        </asp:GridView>
        </div>
    </div>
    </form>
</body>
</html>




 