<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="KeyCodeExpiredPage.aspx.vb" Inherits="QuickTest.KeyCodeExpiredPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    
    <link href="../css/jquery-ui-1.8.18.custom.min.css" rel="stylesheet" />
    
    <style type="text/css">
        div {
            width: 100%;
            height: 100%;
            text-align: center;
            /*padding-top: 50px;*/
        }

        span {
            font-size: 24px;
        }

        .divWarningMaxOnet {
            padding-top: 0;
            color: red;
            text-align: left;
            width: 710px;
            margin-left: auto;
            margin-right: auto;
        }

        #btnAddCredit {
           font-size: larger;
           margin: 0px 20px;
           line-height: 40px;
           border-radius: 10px;
           color: white;
           position: initial;
           top: 0px;
           text-align: center;
           background: linear-gradient(to bottom, #63cfdf 0%,#17b2d9 100%);

        }

        #txtUserName,#txtPassword {
            font: 17px 'THSarabunNew';
            width: 250px;
            height: 25px;
            border-radius: 8px;
            padding-left: 10px;
        }

        .ui-dialog-titlebar {
            text-align: left;
        }
    </style>

    <script src="../js/jquery-1.7.1.min.js" type="text/javascript"></script>
    <script src="../js/jquery-ui-1.8.18.min.js" type="text/javascript"></script>
    <script src="../js/jquery.blockUI.js" type="text/javascript"></script>

     <script type="text/javascript">

        var tokenId = "<%=TokenId%>";
        var deviceId = "<%=DeviceId%>";
        var StudentId = "<%=SubjectsIdStr%>";

         $(function () {
            $('.addSubject').click(function () {
                $('.addCredit').show();
            });
         });

         function callAlertDialog(titleName) {

             var $d = $('#dialog');

             var myBtn = {};
             myBtn["ตกลง"] = function () {
                 $d.dialog('close');
             };
             $d.html('');
             $d.dialog({ buttons: myBtn, draggable: false, resizable: false, modal: true }).dialog('option', 'title', titleName);
         }

         function callConfirmDialog(titleName) {

             var $d = $('#dialog');

             var myBtn = {};
             myBtn["ตกลง"] = function () {
                 $d.dialog('close');
                 //location.reload();
                 window.location = "../practicemode_pad/DefaultMaxOnet.aspx?deviceuniqueid=" + deviceId + "&token=" + tokenId + "&addSubject=True";
             };
             $d.html('');
             $d.dialog({ buttons: myBtn, draggable: false, resizable: false, modal: true }).dialog('option', 'title', titleName);
         }

         function onBtnAddCredit() {
             // check ว่ากรอกครบไหม
             var userName = $('#txtUserName').val();
             var pwd = $('#txtPassword').val();

             if (userName == "") {
                 callAlertDialog("กรอกชื่อผู้ใช้ก่อนค่ะ");
                 return 0;
             }
             if (pwd == "") {
                 callAlertDialog("กรอกรหัสผ่านก่อนค่ะ");
                 return 0;
             }

             var AddCreditResult = addCredit(userName, pwd);

             if (AddCreditResult == 1) {
                 callConfirmDialog("เพิ่ม credit เรียบร้อยแล้วค่ะ");
                 
             } else if (AddCreditResult == -1) {
                 callAlertDialog("ชื่อผู้ใช้และรหัสผ่านไม่ถูกต้องค่ะ");
                 return 0;
             } else if (AddCreditResult == -2) {
                 callAlertDialog("รหัสนี้ใช้งานไปแล้วค่ะ");
                 return 0;
             } else if (AddCreditResult == -3) {
                 callAlertDialog("รหัสนี้หมดอายุแล้วค่ะ");
                 return 0;
             }
             else if (AddCreditResult == -4) {
                 callAlertDialog("มีข้อผิดพลาดในการลงทะเบียนค่ะ");
                 return 0;
             }
         }

         function addCredit(userName, password) {
            var returnValue;
            $.ajax({
                type: "POST",
                url: "<%=ResolveUrl("~")%>WebServices/MaxOnetService.asmx/AddCreditMaxonet",
                async: false,
                data: "{studentId : '" + StudentId + "',tokenId : '" + tokenId + "',deviceId : '" + deviceId + "', userName :  '" + userName + "',password :  '" + password + "'}",
                contentType: "application/json; charset=utf-8", dataType: "json",
                success: function (msg) {
                    returnValue = msg.d;
                },
                error: function myfunction(request, status) {

                }
            });
            return returnValue;
        }
     </script>
</head>
<body>
    <form id="form1" runat="server">
        <div id="dialog"></div>
        <div style="color:red;">
            <img src="../Images/NotApproveButton.png" width="80" />
            <br />
            <h1 style="color: red;">หมดอายุการใช้งาน กรุณาเติมเครดิตค่ะ</h1>
            <br />
            <span style="color: red;" class="header">เติมเครดิต เพื่อเลือกวิชาเพิ่ม</span>
            <br />
            <br />
            <br />
            <div class="addSubject">
                <span>ชื่อผู้ใช้         :          </span><input type="text" id="txtUserName" />
                <br />
                <br />
                <span>รหัสลับ         :          </span><input type="password" id="txtPassword"  />
                <br />
                <br />
                <br />
                <input type="button" id="btnAddCredit" value="เพิ่มเครดิต" style="width: 150px;" onclick="onBtnAddCredit();" />
            </div>
            <br />
            <br />
            <br />
            <span>สอบถามติดต่อ email : maxonet@iknow.co.th</span>
        </div>
    </form>
</body>
</html>
