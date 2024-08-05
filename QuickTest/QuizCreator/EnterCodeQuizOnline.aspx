<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="EnterCodeQuizOnline.aspx.vb" Inherits="QuickTest.EnterCodeQuizOnline" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="../js/jquery-1.7.1.js"></script>
    <script src="../js/jquery.blockUI.js"></script>
    
    <script type="text/javascript">
        $(function () {

            $('#DivEnterCode').click(function () {
                    $.ajax({
                    type: "POST",
                    url: "<%=ResolveUrl("~")%>QuizCreator/EnterCodeQuizOnline.aspx/CheckCodeIsCorrect",
                    async: false,
                    contentType: "application/json; charset=utf-8", dataType: "json",
                    data: "{QuizCode : '" + $('#txtCode').val() + "'}",
                    success: function (data) {
                        if (data.d == 'False') {
                                $('#SpnValidateCode').stop(true, true).show();
                        }
                        else
                        {
                            $('#SpnValidateCode').stop(true, true).hide();
                            $.ajax({
                                type: "POST",
                                url: "<%=ResolveUrl("~")%>QuizCreator/EnterCodeQuizOnline.aspx/CheckThisCodeIsOnline",
                                    async: false,
                                    contentType: "application/json; charset=utf-8", dataType: "json",
                                    data: "{QuizCode : '" + $('#txtCode').val() + "'}",
                                    success: function (data) {
                                        if (data.d == 'False') {
                                            $('#SpnValidateIsOnline').stop(true, true).show();
                                        }
                                        else {
                                            $('#SpnValidateIsOnline').stop(true, true).hide();
                                            $.ajax({
                                                type: "POST",
                                                url: "<%=ResolveUrl("~")%>QuizCreator/EnterCodeQuizOnline.aspx/CheckThisCodeHavePassword",
                                                async: false,
                                                contentType: "application/json; charset=utf-8", dataType: "json",
                                                data: "{QuizCode : '" + $('#txtCode').val() + "'}",
                                                success: function (data) {
                                                    if (data.d == 'True') {
                                                        OpenBlockUI();
                                                    }
                                                    else {
                                                        alert('เปิดหน้าทำ Quiz')
                                                    }
                                                },
                                                error: function myfunction(request, status) {
                                                    alert(status);
                                                }
                                            })
                                        }
                                    },
                                    error: function myfunction(request, status) {
                                        alert(status);
                                    }
                            })
                        }
                    },
                    error: function myfunction(request, status) {
                        alert(status);
                    }
                })
            });

            $('#DivCancel').click(function () {
                $.unblockUI();
            });

            $('#DivCheckPassword').click(function () {
                    $.ajax({
                    type: "POST",
                    url: "<%=ResolveUrl("~")%>QuizCreator/EnterCodeQuizOnline.aspx/CheckPasswordIsCorrect",
                    async: false,
                    contentType: "application/json; charset=utf-8", dataType: "json",
                    data: "{QuizCode : '" + $('#txtCode').val() + "',QuizPassword:'" + $('#txtPassword').val() + "'}",
                    success: function (data) {
                        if (data.d == 'True') {
                            $('#spnValidatePassword').stop(true,true).hide();
                            alert('เปิดหน้าทำควิซ');
                        }
                        else {
                            $('#spnValidatePassword').stop(true, true).show();
                        }
                    },
                    error: function myfunction(request, status) {
                        alert(status);
                    }
                })
            });

        });


        function OpenBlockUI() {
            $.blockUI({
                message: $('#DivEnterPassword'),
                css: {
                    top: '20%'
                }
            });
        }

    

    </script>

    <style type="text/css">
        body {
        background-color:#2d2b2e;
        }
        #MainDiv {
        width:725px;
        height:500px;
        margin-left:auto;
        margin-right:auto;
        border:1px solid;
        border-color:#fff;
        margin-top:50px;
        border-radius:6px;
        text-align:center;
        }
        span {
        color:#ec4731;
        }
        .spnHead {
        font-size:70px;
        }
        table {
        width:700px;
        margin-left:auto;
        margin-right:auto;
        margin-top:50px;
        }
        .Forbtn {
        width: 300px;
        height: 60px;
        line-height: 60px;
        font-size: 18px;
        background-color: #ff4e37;
        background-image: -webkit-linear-gradient(top, #ff4e37, #f04934);
        background-image: -moz-linear-gradient(top, #ff4e37, #f04934);
        background-image: -o-linear-gradient(top, #ff4e37, #f04934);
        background-image: linear-gradient(to bottom, #ff4e37, #f04934);
        text-decoration: none;
        margin-left:auto;
        margin-right:auto;
        font-size:30px;
        color:#fff;
        border-radius:6px;
        margin-top:100px;
        cursor:pointer;
        }
        #DivEnterPassword {
        width:480px;
        height:300px;
        background-color:#130F14;
        }
        .ForbtnPassword {
        background-color:#ff4e37;
        width:100px;
        height:60px;
        color:#fff;
        position:relative;
        display:inline-block;
        margin-top:65px;
        border-radius:6px;
        line-height:60px;
        font-size:25px;
        cursor:pointer;
        }

    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div id="MainDiv">
        <table>
            <tr>
                <td>
                    <span class="spnHead">ใส่รหัสชุด</span>

                </td>
            </tr>
        <tr>
            <td style="padding-top:20px;">
                <input type="text" id="txtCode" placeholder="ใส่รหัสชุด" style="font-size:30px;padding:10px;border-radius:6px;" />
            </td>
        </tr>
            <tr>
                <td style="padding-top:30px;">
                    <span id="SpnValidateCode" style="color:red;font-size:30px;display:none;">**ไม่พบข้อสอบชุดนี้ค่ะ**</span>
                    <span id="SpnValidateIsOnline" style="color:red;font-size:30px;display:none;">**ข้อสอบชุดนี้ปิดการใช้ชั่วคราว นะคะ**</span>
                </td>
            </tr>
            </table>
        <div id="DivEnterCode" class="Forbtn">
            ตกลง
        </div>

        <div id="DivEnterPassword" style="display:none;">
            <span style="font-size:50px;top:25px;position:relative;">ใส่รหัสลับ</span>
            <br />
            <input type="password"  id="txtPassword" placeholder="ใส่รหัสลับ" style="font-size:25px;padding:8px;border-radius:6px;position:relative;top:35px;" />
            <br />
            <span id="spnValidatePassword" style="font-size:25px;color:red;position:relative;top:50px;display:none;">**รหัสลับผิด**</span>
            <br />
             <div id="DivCancel" class="ForbtnPassword" style="margin-right:90px;">
                ยกเลิก
            </div>
            <div id="DivCheckPassword" class="ForbtnPassword">
                ตกลง
            </div>
        </div>
    </div>
    </form>
</body>
</html>
