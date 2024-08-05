<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="upgradeClass.aspx.vb"
    Inherits="QuickTest.upgradeClass" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="../js/jquery-1.7.1.js" type="text/javascript"></script>
    <script src="../js/jquery-ui-1.8.18.js" type="text/javascript"></script>
    <link href="../css/menuFixReviewAns.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .educationYear
        {
            position: absolute;
            background-color: #3399CC;
            width: 98%;
            height: 87%;
            left: 1%;
            -webkit-border-radius: 5em;
        }
        .educationYear table
        {
            width: 100%;
            text-align: center;
            color: #002033;
        }
        .classToUpgrade
        {
            position: relative;
            background-color: #B4DCED;
            width: 98%;
            height: 80px;
            left: 1%;
            -webkit-border-radius: 5em;
            margin-top:5px;
        }
        .classToUpgrade table
        {
            width: 100%;
            text-align: center;
            color: #005C91;
        }
       
        .btnUpclass
        {
            position:absolute;
            right:10px;bottom:-35px;margin-top:2px;width:150px;height:50px;
            font-size:larger;font-weight:bolder;
        }
    </style>
    <script type="text/javascript">
        $(function () {
            //create div class to upgrade
            creDivClassUpgrade();
            function creDivClassUpgrade() {
                for (var i = 2; i < 7; i++) {
                    var divClassUp = '<div class="classToUpgrade">';
                    divClassUp += ' <table><tr><td style="width: 40%;"><h1>ม.' + i + '</h1></td>';
                    if (i == 6) {
                        divClassUp += '<td style="width: 20%;"> <img alt="ย้ายห้องเรียน" src="../images/upgradeClass/Actions-arrow-right-icon.png" width="50" height="50"/></td><td><h1>จบการศึกษา</h1></td></tr></table>';
                    }
                    else {
                        divClassUp += '<td style="width: 20%;" > <img alt="ย้ายห้องเรียน" src="../images/upgradeClass/Actions-arrow-right-icon.png" width="50" height="50"/></td><td><h1>ม.' + (i + 1) + '</h1></td></tr></table>';
                    }
                    $('.educationYear').append(divClassUp);
                }
            }

            $('.divBackToStep1').click(function () {
                window.parent.location.href = '/testset/step1.aspx';
            });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div style="width: 60%; height: 95%; left: 20%; text-align: center; position: absolute; color:color: #002033; ">
        <h1>
            เลื่อนชั้นเรียน</h1>
        <div class="educationYear">
            <table>
                <tr>
                    <td style="width: 40%;">
                        <h1>
                            ปีการศึกษา 2555</h1>
                    </td>
                    <td style="width: 20%;">
                        <h1>
                             <img alt="ย้ายห้องเรียน" src="../images/upgradeClass/Actions-arrow-right-double-icon.png" width="60" height="60"/></h1>
                    </td>
                    <td>
                        <h1>
                            ปีการศึกษา 2556</h1>
                    </td>
                </tr>
            </table>
            <div class="classToUpgrade">
                <table>
                    <tr>
                        <td style="width: 40%;">
                            <h1>
                                ม.1</h1>
                        </td>
                        <td style="width: 20%;">
                            <img alt="ย้ายห้องเรียน" src="../images/upgradeClass/Actions-arrow-right-icon.png" width="50" height="50"/>
                        </td>
                        <td>
                            <h1>
                                ม.2</h1>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
       
        <asp:Button runat="server" ID="btnUpgradeClass" Text="เลื่อนชั้น" CssClass="btnUpclass"></asp:Button><div
            id="dialog" title="ต้องการบันทึกการเลื่อนชั้นใช่หรือไม่">
          <div class="divBackToStep1">
            </div>
    </div>
    </form>
</body>
</html>
