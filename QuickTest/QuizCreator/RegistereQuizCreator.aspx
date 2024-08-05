<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="RegistereQuizCreator.aspx.vb" Inherits="QuickTest.RegistereQuizCreator" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="../js/jquery-1.7.1.js"></script>
    <script src="../js/jquery-ui-1.8.18.js"></script>
    <link href="../css/jquery-ui-1.10.1.custom.min.css" rel="stylesheet" />

    <script type="text/javascript">

        $(function () {
 
            //Bind Autocomplete ลง textbox
            BindAutoComplete();

            //เมื่อคลิกที่ Div "สมัครใหม่"
            $('#DivNewRegister').toggle(
                function () {
                    $('#DivChildNewRegister').stop(true, true).fadeIn(500);
                },
                function () {
                    //เช็คก่อนว่าไปถึง Step 2 หรือยัง
                    if ($('#DivCheckNewRegisterStep2').css('display') == 'block') {
                        $('#DivChildNewRegister').stop(true, true).hide();
                        $('#DivCheckNewRegisterStep2').stop(true, true).fadeOut(500);
                    }
                    else {
                        $('#DivChildNewRegister').stop(true, true).fadeOut(500);
                    }
                }
                );

            //เมื่อคลิกที่ Div "เคยสมัครแล้ว"
            $('#AlreadyRegister').toggle(
                function () {
                    $('#ChildAlreadyRegister').stop(true,true).fadeIn(500);
                },
                function () {
                    $('#ChildAlreadyRegister').stop(true, true).fadeOut(500);
                }
                );

            //เมื่อกด "ลิมรหัสผ่าน"
            $('#ForgotPassword').toggle(
                function () {
                    $('#DivForgotPassword').stop(true,true).fadeIn(500);
                },
                function () {
                    $('#DivForgotPassword').stop(true, true).fadeOut(500);
                }
                );

            //เช็คว่า Pattern Email ที่ใส่เข้ามาถูกต้องหรือเปล่า
            $('#txtEmalNewRegister').focusout(function () {
                if ($(this).val() !== '') {
                    var CheckEmail = validateEmail($(this).val());
                    if (CheckEmail == true) {
                        $('#SpnValidateEmail').stop(true, true).hide();
                    }
                    else {
                        $('#SpnValidateEmail').stop(true, true).show();
                    }
                }
            });

            //เช็คว่า รหัสผ่านที่กรอกเข้ามาน้อยกว่า 4 ตัวอักษารหรือเปล่า
            $('#txtPasswordNewRegister').focusout(function () {
                if ($(this).val().length < 4) {
                    $('#SpnValidatePassword').stop(true,true).show();
                } else {
                    $('#SpnValidatePassword').stop(true, true).hide();
                }
            });

            //เมื่อกดปุ่ม 'ไปต่อ' ต้องดูว่ามี txt อันไหนที่เป็นค่าว่างหรือเปล่าต้องขึ้นเตือน
            $('#btnNextNewRegister').click(function () {
                var checkValidateStep1 = validateStep1Register();
                if (checkValidateStep1 == true) {
                    $('#DivChildNewRegister').stop(true, true).hide();
                    $('#DivCheckNewRegisterStep2').stop(true, true).fadeIn(500);
                }
            });

            //ปุ่ม "ย้อนกลับ" จาก RegisterStep2
            $('#btnBackToStep1').click(function () {
                $('#DivCheckNewRegisterStep2').stop(true, true).hide();
                $('#DivChildNewRegister').stop(true, true).fadeIn(500);
            });

            //หา Autocomplete เมื่อพิมพ์ชื่ออำเภอ
            $('#txtAmphur').mousedown(function () {
                var txtProvinceName = $('#txtProvince').val();
                $.ajax({
                    type: "POST",
                    url: "<%=ResolveUrl("~")%>QuizCreator/RegistereQuizCreator.aspx/GetDataAmphur",
                    async: false,
                    data: "{ProvinceName : '" + txtProvinceName + "' }",
                    contentType: "application/json; charset=utf-8", dataType: "json",
                    success: function (data) {
                    if (data.d !== '') {
                        var AmphurArr = new Array();
                        var objAmphur = data.d;
                        var AmphurSplit = objAmphur.split(',')
                        for (var i = 0; i < AmphurSplit.length - 1; i++) {
                            AmphurArr.push(AmphurSplit[i]);
                        }
                        $('#txtAmphur').autocomplete({ source: AmphurArr });
                    }
                    },
                    error: function myfunction(request, status) {
                        alert(status);
                    }
                });
            })

            //หา Autocomplete โรงเรียน
            $('#txtSchool').mousedown(function () {
                var txrAmphurName = $('#txtAmphur').val();
                $.ajax({
                    type: "POST",
                    url: "<%=ResolveUrl("~")%>QuizCreator/RegistereQuizCreator.aspx/GetDataSchool",
                    async: false,
                    data: "{AmphurName : '" + txrAmphurName + "' }",
                    contentType: "application/json; charset=utf-8", dataType: "json",
                    success: function (data) {
                        if (data.d !== '') {
                            var SchoolArr = new Array();
                            var objSchool = data.d;
                            var SchoolSplit = objSchool.split(',')
                            for (var i = 0; i < SchoolSplit.length - 1; i++) {
                                SchoolArr.push(SchoolSplit[i]);
                            }
                            $('#txtSchool').autocomplete({ source: SchoolArr });
                        }
                    },
                    error: function myfunction(request, status) {
                        alert(status);
                    }
                });
            })

            //เมื่อเลือก radio ครู/อาจารย์
            $('#rdoTeacher').click(function () {
                $('#txtOther').hide();
                $('#ChildRdoTeacher').stop(true, true).fadeIn(300);
            });

            //เมื่อเลือก radio อื่นๆ
            $('#rdoOther').click(function () {
                $('#ChildRdoTeacher').hide();
                $('#txtOther').stop(true, true).fadeIn(300);
            });

            //เมื่อคลิกที่ปุ่ม "สมัคร"
            $('#btnRegisterFinalStep').click(function () {
                if ($('#rdoTeacher').is(':checked')) {
                    var CheckvalidateFinalStep = validateFinalStep();
                    if (CheckvalidateFinalStep == true) {
                        TeacherRegister();
                    }
                }
                if ($('#rdoOther').is(':checked')) {
                    var CheckValidateFinalOtherStep = validateFinalStepOher();
                    if (CheckValidateFinalOtherStep == true) {
                        OherRegister();
                    }
                }
            });

        });

        function validateEmail(email) { 
            var re = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
            return re.test(email);
        } 

        //Function ที่จะ Validate ทุกอย่างใน Step1
        function validateStep1Register() {
            //เช็คว่า Email ผิด Pattern หรือเปล่า
            var CheckEmail = validateEmail($('#txtEmalNewRegister').val());
            if (CheckEmail == true) {
                $('#SpnValidateEmail').stop(true, true).hide();
            }
            else {
                $('#SpnValidateEmail').stop(true, true).show();
                return false
            }
            
            //เช็คก่อนว่า Email ซ้ำหรือเปล่า ?
            var CheckEmailInAjax;
            $.ajax({
                type: "POST",
                url: "<%=ResolveUrl("~")%>QuizCreator/RegistereQuizCreator.aspx/CheckEmailInDB",
                async: false,
                data: "{Email : '" + $('#txtEmalNewRegister').val() + "' }",
                contentType: "application/json; charset=utf-8", dataType: "json",
                success: function (data) {
                    if (data.d == 'AlreadyUse') {
                        $('#SpnReqEmail').stop(true, true).show();
                        CheckEmailInAjax = false
                    }
                    else {
                        $('#SpnReqEmail').stop(true, true).hide();
                    }
                },
                error: function myfunction(request, status) {
                    alert(status);
                }
            });

            if (CheckEmailInAjax == false) {
                return false;
            }

            //เช็คช่องใส่ ชื่อ ต้องไม่เป็นค่าว่าง
            if ($('#txtFirstnameNewRegister').val() == '') {
                $('#SpnReqFirstName').stop(true, true).show();
                return false
            }
            else {
                $('#SpnReqFirstName').stop(true, true).hide();
            }
            
            //เช็คช่องใส่ นามสกุล ต้องไม่เป็นค่าว่าง
            if ($('#txtLastNameNewRegister').val() == '') {
                $('#SpnReqLastName').stop(true, true).show();
                return false
            }
            else {
                $('#SpnReqLastName').stop(true, true).hide();
            }
            
            //เช็คช่องใส่รหัสผ่าน ต้องไม่เป็นค่าว่าง 
            if ($('#txtPasswordNewRegister').val() == '') {
                $('#SpnReqPassword').stop(true, true).show();
                return false
            }
            else {
                $('#SpnReqPassword').stop(true, true).hide();
            }

            //เช็คช่องใส่รหัสผ่าน ไม่น้อยกว่า 4 ตัวอักษร
            if ($('#txtPasswordNewRegister').val().length < 4) {
                $('#SpnValidatePassword').stop(true, true).show();
                return false
            } else {
                $('#SpnValidatePassword').stop(true, true).hide();
            }

            return true
        };


        //Function เช็คว่า Email ที่ส่งเข้ามาถูกใช้งานไปแล้วหรือยัง ***************************************
        function CheckIsEmailIsAlreadyUse(InputEmail) {
            $.ajax({
                type: "POST",
                url: "<%=ResolveUrl("~")%>QuizCreator/RegistereQuizCreator.aspx/CheckEmailInDB",
                async: false,
                data: "{Email : '" + InputEmail + "' }",
                contentType: "application/json; charset=utf-8", dataType: "json",
                success: function (data) {
                    if (data.d == 'AlreadyUse') {
                        return true
                    }
                    else {
                        return false
                    }
                },
                error: function myfunction(request, status) {
                    alert(status);
                }
            });
        };

        //Bind ข้อมูล จังหวัด,อำเภอ,โรงเรียน ลงใน textbox
        function BindAutoComplete() {
            //จังหวัด
            $.ajax({
                type: "POST",
                url: "<%=ResolveUrl("~")%>QuizCreator/RegistereQuizCreator.aspx/GetDataProvince",
                async: false,
                contentType: "application/json; charset=utf-8", dataType: "json",
                success: function (data) {
                    if (data.d !== '') {
                        var ProvinceArr = new Array();
                        var objProvince = data.d;
                        var ProvinceSplit = objProvince.split(',')
                        for (var i = 0; i < ProvinceSplit.length - 1; i++) {
                            ProvinceArr.push(ProvinceSplit[i]);
                        }
                        $('#txtProvince').autocomplete({ source: ProvinceArr });
                    }
                },
                error: function myfunction(request, status) {
                    alert(status);
                }
            });
        }

        //Function Validate RegisterFinalStep
        function validateFinalStep() {
            //จังหวัดต้องไม่เป็นค่าว่าง
            if ($('#txtProvince').val() == '') {
                $('#spnReqProvince').stop(true, true).show();
                return false
            }
            else {
                $('#spnReqProvince').stop(true, true).hide();
            }
            //จังหวัดต้องตรงกับข้อมูลที่มีอยู่ใน Database
            var AjaxvalidateProvince;
            $.ajax({
                type: "POST",
                url: "<%=ResolveUrl("~")%>QuizCreator/RegistereQuizCreator.aspx/CheckProvinceIsExistInDB",
                async: false,
                contentType: "application/json; charset=utf-8", dataType: "json",
                data: "{ProvinceName : '" + $('#txtProvince').val() + "' }",
                success: function (data) {
                    if (data.d == 'True') {
                        $('#spnValidateProvince').stop(true,true).hide();
                    }
                    else {
                        $('#spnValidateProvince').stop(true, true).show();
                        AjaxvalidateProvince = false
                    }
                },
                error: function myfunction(request, status) {
                    alert(status);
                }
            })
            if (AjaxvalidateProvince == false) {
                return false;
            }

            //อำเภอต้องไม่เป็นค่าว่าง
            if ($('#txtAmphur').val() == '') {
                $('#spnReqAmphur').stop(true,true).show();
                return false
            }
            else {
                $('#spnReqAmphur').stop(true, true).hide();
            }
            //ชื่ออำเภอต้องตรงกับข้อมูลใน Database
            var AjaxvalidateAmphur;
            $.ajax({
                type: "POST",
                url: "<%=ResolveUrl("~")%>QuizCreator/RegistereQuizCreator.aspx/CheckAmphurIsExistInDB",
                async: false,
                contentType: "application/json; charset=utf-8", dataType: "json",
                data: "{AmphurName : '" + $('#txtAmphur').val() + "' }",
                success: function (data) {
                    if (data.d == 'True') {
                        $('#spnValidateAmphur').stop(true, true).hide();
                    }
                    else {
                        $('#spnValidateAmphur').stop(true, true).show();
                        AjaxvalidateAmphur = false
                    }
                },
                error: function myfunction(request, status) {
                    alert(status);
                }
            })
            if (AjaxvalidateAmphur == false) {
                return false
            }

            //โรงเรียนต้องไม่เป็นค่าว่าง
            if ($('#txtSchool').val() == '') {
                $('#spnReqSchool').stop(true, true).show();
                return false
            }
            else {
                $('#spnReqSchool').stop(true, true).hide();
            }
            
            //ชั้นต้องไม่เป็นค่าว่าง
            if ($('#txtClassName').val() == '') {
                $('#spnReqClassName').stop(true, true).show();
                return false
            }
            else {
                $('#spnReqClassName').stop(true, true).hide();
            }

            //หมวดวิชาต้องไม่เป็นค่าว่าง
            if ($('#txtGroupsubjectName').val() == '') {
                $('#spnReqGroupsubjectName').stop(true, true).show();
                return false
            }
            else {
                $('#spnReqGroupsubjectName').stop(true, true).hide();
            }
            
            //เบอร์ติดต่อต้องไม่เป็นค่าว่าง
            if ($('#txtPhone').val() == '') {
                $('#spnReqPhone').stop(true, true).show();
                return false
            }
            else {
                $('#spnReqPhone').stop(true, true).hide();
            }
            //เบอร์ติดต่อต้องไม่น้อยกว่า 9 ตัวอักษร  
            if ($('#txtPhone').val().length < 10) {
                $('#spnValidatePhone').stop(true,true).show();
                return false
            }
            else {
                $('#spnValidatePhone').stop(true,true).hide();
            }
            return true;
        };

        //validate ตอนกดเลือก rdo อื่นๆ
        function validateFinalStepOher() {
            //textbox อื่นๆ ต้องไม่เป็นค่าว่าง
            if ($('#txtOther').val() == '') {
                $('#spnReqOther').stop(true, true).show();
                return false
            }
            else {
                $('#spnReqOther').stop(true, true).hide();
            }

            //ต้องยาวมากกว่า 5 ตัวอักษร
            if ($('#txtOther').val().length < 5) {
                $('#spnValidateOher').stop(true, true).show();
                return false
            }
            else {
                $('#spnValidateOher').stop(true, true).hide();
            }
            return true
        };

        //Fucntion สมัครแบบ เลือกเป็น ครู/อาจารย์
        function TeacherRegister() {
            $.ajax({
                type: "POST",
                url: "<%=ResolveUrl("~")%>QuizCreator/RegistereQuizCreator.aspx/InsertQcUser",
                async: false,
                contentType: "application/json; charset=utf-8", dataType: "json",
                data: "{Email : '" + $('#txtEmalNewRegister').val() + "',QCPassword: '" + $('#txtPasswordNewRegister').val() + "',FirstName:'" + $('#txtFirstnameNewRegister').val() + "',LastName:'" + $('#txtLastNameNewRegister').val() + "',ProvinceName:'" + $('#txtProvince').val() + "',AmphurName:'" + $('#txtAmphur').val() + "',SchoolName:'" + $('#txtSchool').val() + "',ClassName:'" + $('#txtClassName').val() + "',GroupSubjectName:'" + $('#txtGroupsubjectName').val() + "',Phone:'" + $('#txtPhone').val() + "' }",
                success: function (data) {
                    if (data.d == 'EmailAlreadyUse') {
                        alert('อีเมล์นี้ถูกใช้งานแล้ว');
                    }
                    else if (data.d == 'Complete') {
                        alert('สมัครเรียบร้อยแล้ว');
                    }
                },
                error: function myfunction(request, status) {
                    alert(status);
                }
              })
        }

        //Function สมัครแบบ อื่นๆ
        function OherRegister() {
            $.ajax({
                type: "POST",
                url: "<%=ResolveUrl("~")%>QuizCreator/RegistereQuizCreator.aspx/InsertQCUserOther",
                async: false,
                contentType: "application/json; charset=utf-8", dataType: "json",
                data: "{Email : '" + $('#txtEmalNewRegister').val() + "',QCPassword: '" + $('#txtPasswordNewRegister').val() + "',FirstName:'" + $('#txtFirstnameNewRegister').val() + "',LastName:'" + $('#txtLastNameNewRegister').val() + "',TypeName:'" + $('#txtOther').val() + "' }",
                success: function (data) {
                    if (data.d == 'Complete') {
                        alert('สมัครเรียบร้อยแล้ว');
                    }
                },
                error: function myfunction(request, status) {
                    alert(status);
                }
            })
        }

    </script>

    <style type="text/css">
        body {
        background-color:#2d2b2e;
        }
        #MainDiv {
            width:960px;
            margin-left:auto;
            margin-right:auto;
            /*background-color:#2d2b2e;*/
            text-align:center;
            padding:10px;
        }
        span {
        color:#fff;
        }
        .SpnLargeFont {
        font-size:40px;
        }
        .SpnMediumFont {
        font-size:25px
        }
        .Forbtn {
            background-color: #ff4e37;
            background-image: -webkit-linear-gradient(top, #ff4e37, #f04934);
            background-image: -moz-linear-gradient(top, #ff4e37, #f04934);
            background-image: -o-linear-gradient(top, #ff4e37, #f04934);
            background-image: linear-gradient(to bottom, #ff4e37, #f04934);
            text-decoration: none;
            width:350px;
            height:70px;
            line-height:70px;
            margin-left:auto;
            margin-right:auto;
            margin-top:10px;
            margin-bottom:10px;
            cursor:pointer;
            border-radius:6px;
        }

        #ChildAlreadyRegister,#DivChildNewRegister,#DivCheckNewRegisterStep2,#ChildRdoTeacher {
            border: 1px solid;
            width: 500px;
            margin-left: auto;
            margin-right: auto;
            border-color: #fff;
            padding: 15px;
            border-radius: 6px;
            display:none;
        }
        input[type=text],input[type=password] {
        font-size:16px;
        }

        table {
        width:500px;
        margin-left:auto;
        margin-right:auto;
        margin-bottom:10px;
        }
        .SpnValidate {
        color:red;
        display:none;
        }

    </style>

</head>
<body>
    <form id="form1" runat="server">
         
    <div id="MainDiv">

        <div id="DivNewRegister" class="Forbtn">
            <span class="SpnLargeFont">สมัครใหม่</span>
        </div>
            <div id="DivChildNewRegister">
                <table>
                    <tr>
                        <td><span class="SpnMediumFont">อีเมล์</span></td>
                        <td style="text-align:left">
                            <input type="text" id="txtEmalNewRegister" />
                            <span id="SpnValidateEmail"  class="SpnValidate">อีเมล์ไม่ถูกต้อง</span>
                            <span id="SpnReqEmail"  class="SpnValidate">อีเมล์ถูกใช้งานไปแล้ว</span>
                        </td>
                    </tr>
                    <tr>
                        <td><span class="SpnMediumFont">ชื่อ</span></td>
                        <td style="text-align:left">
                            <input type="text" id="txtFirstnameNewRegister" />
                            <span class="SpnValidate" id="SpnReqFirstName">*</span>
                        </td>
                    </tr>
                    <tr>
                        <td><span class="SpnMediumFont">นามสกุล</span></td>
                        <td style="text-align:left">
                            <input type="text" id="txtLastNameNewRegister" />
                            <span class="SpnValidate" id="SpnReqLastName">*</span>
                        </td>
                    </tr>
                    <tr>
                        <td><span class="SpnMediumFont">ตั้งรหัสผ่าน</span></td>
                        <td style="text-align:left">
                            <input type="text" id="txtPasswordNewRegister" />
                            <span class="SpnValidate" id="SpnValidatePassword">ต้องมากกว่า 4 ตัวอักษร</span>
                            <span class="SpnValidate" id="SpnReqPassword">*</span>
                        </td>
                    </tr>
                </table>
                <input type="button" class="Forbtn" style="width: 90px;height: 35px;line-height: 0px;font-size: 25px;color: #fff;margin-bottom:0px;" value="ไปต่อ"  id="btnNextNewRegister"/>
            </div>

        <div id="DivCheckNewRegisterStep2">
            <table style="width:230px;text-align:left;">
                <tr>
                    <td>
                        <input id="rdoTeacher" name="Type" type="radio" /><label style="font-size:25px;color:#fff;" for="rdoTeacher">ครู/อาจารย์</label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <input id="rdoOther" name="Type" type="radio" value="อื่นๆ" /><label style="font-size:25px;color:#fff;" for="rdoOther">อื่นๆ</label>
                        <input type="text" id="txtOther" style="display:none;width:140px;" />
                        <span class="SpnValidate" id="spnReqOther">*</span>
                        <span class="SpnValidate" id="spnValidateOher">ต้องยาวมากกว่า 5 ตัวอักษร</span>
                    </td>
                </tr>
            </table>
                <div id="ChildRdoTeacher" style="width:450px;">
                    <table>
                        <tr>
                            <td><span class="SpnMediumFont">สอนที่จังหวัด</span></td>
                            <td style="text-align:left;">
                                <input type="text" id="txtProvince" />
                                <span class="SpnValidate" id="spnReqProvince">*</span>
                                <span class="SpnValidate" id="spnValidateProvince">ชื่อจังหวัดไม่ถูกต้อง</span>
                            </td>
                        </tr>
                        <tr>
                            <td><span class="SpnMediumFont">สอนที่อำเภอ</span></td>
                            <td style="text-align:left;">
                                <input type="text" id="txtAmphur" />
                                <span class="SpnValidate" id="spnReqAmphur">*</span>
                                <span class="SpnValidate" id="spnValidateAmphur">ชื่ออำเภอไม่ถูกต้อง</span>
                            </td>
                        </tr>
                        <tr>
                            <td><span class="SpnMediumFont">โรงเรียน</span></td>
                            <td style="text-align:left;">
                                <input type="text" id="txtSchool" />
                                <span class="SpnValidate" id="spnReqSchool">*</span>
                            </td>
                        </tr>
                        <tr>
                            <td><span class="SpnMediumFont">ชั้น</span></td>
                            <td style="text-align:left;"> 
                                <input type="text" id="txtClassName" />
                                <span class="SpnValidate" id="spnReqClassName">*</span>
                            </td>
                        </tr>
                        <tr>
                            <td><span class="SpnMediumFont">หมวดวิชา</span></td>
                            <td style="text-align:left;">
                                <input type="text" id="txtGroupsubjectName" />
                                <span class="SpnValidate" id="spnReqGroupsubjectName">*</span>
                            </td>
                        </tr>
                        <tr>
                            <td><span class="SpnMediumFont">เบอร์ติดต่อ</span></td>
                            <td style="text-align:left;">
                                <input type="text" id="txtPhone" />
                                <span class="SpnValidate" id="spnReqPhone">*</span>
                                <span class="SpnValidate" id="spnValidatePhone">ต้องมากกว่า 9 หลัก</span>
                            </td>
                        </tr>
                    </table>
                </div>
            <input type="button" id="btnBackToStep1" class="Forbtn" value="ย้อนกลับ" style="width: 90px;height: 40px;height: 50px;line-height: 0px;font-size: 20px;color: #fff;margin-bottom:0px;margin-right:135px;" />
            <input type="button" id="btnRegisterFinalStep" class="Forbtn" value="สมัคร" style="width: 90px;height: 40px;height: 50px;line-height: 0px;font-size: 20px;color: #fff;margin-bottom:0px;" />
      </div>

        <div id="AlreadyRegister" style="margin-top:25px;" class="Forbtn">
            <span class="SpnLargeFont">เคยสมัครแล้ว</span>
        </div>
           <div id="ChildAlreadyRegister">
                    <table>
                        <tr>
                            <td><span class="SpnMediumFont">อีเมล์</span></td>
                            <td style="text-align:left;">
                                <input type="text" id="txtEmail" />
                            </td>
                        </tr>
                        <tr>
                            <td><span class="SpnMediumFont">รหัสผ่าน</span></td>
                            <td style="text-align:left;">
                                <input type="password" id="txtPassword" />
                            </td>
                        </tr>
                    </table>
               <input id="chkRemeberAccount" type="checkbox" /><label style="font-size:25px;color:#fff;" for="chkRemeberAccount">จำค่าไว้</label>
               <br />
               <input type="button" value="เข้าใช้งาน" class="Forbtn" style="width: 150px;height: 40px;height: 50px;line-height: 0px;font-size: 25px;color: #fff;margin-bottom:25px;" />
               <br />
                    <span id="ForgotPassword" style="cursor:pointer;" class="SpnMediumFont">ลืมรหัสผ่าน ?</span>
                    <div id="DivForgotPassword" style="display:none;border: 1px solid;width: 350px;padding:10px;border-color: #fff;margin-left: auto;margin-right: auto;margin-top:10px;">
                        <span>ส่งไปทางอีเมล์</span>
                        <input type="text" id="txtEmailForgotPassword" />
                        <br />
                        <input type="button" class="Forbtn" style="width: 90px;height: 35px;line-height: 0px;font-size: 25px;color: #fff;margin-bottom:0px;" id="btnSendPasswordToEmail" value="ส่ง" />
                   </div>
               </div>

    </div>

        <asp:HiddenField ID="HiddenField1" runat="server" />

    </form>
</body>
</html>
