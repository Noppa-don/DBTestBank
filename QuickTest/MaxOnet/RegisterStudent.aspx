<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="RegisterStudent.aspx.vb" Inherits="QuickTest.RegisterStudent1" %>

<!DOCTYPE html>

<%--<html xmlns="http://www.w3.org/1999/xhtml" style="background-color: #00bff3;">--%>
<html xmlns="http://www.w3.org/1999/xhtml" style="background-color: #a0c56a;">
   
<head runat="server">
    <title></title>
    <style type="text/css">
        #DivMain {

            /*border-radius: 1em; background-color: #f26522; position: absolute; padding: 15px; margin-left: 50%; left: -340px; margin-top: 5%;    border: solid 9px white;*/
            border-radius: 1em; position: absolute; padding: 15px; margin-left: 50%; left: -340px; margin-top: 5%;    border: solid 9px white;
            
        }

         .txtInput {
            border-radius: 10px; height: 22px; width: 250px;padding-left: 10px;margin-bottom: 20px;
        }
        #btnRegister{
            /*width: 50%; height: 45px; border-radius: 0.5em; font-size: 20p  x; background-color: #e81062; margin-top: 10px; color:white;*/
            width: 50%; height: 45px; border-radius: 0.5em; font-size: 20px; margin-top: 10px; color:white;
           
        }
        .trHeader {
            /*height: 70px; text-align: center; font-size: 25px;    color: #4a0101; font-weight: bold; padding-bottom: 15px;*/
            height: 70px; text-align: center; font-size: 25px;    color: white; font-weight: bold; padding-bottom: 15px;
        }
        .cntr {
            margin: auto;
        }
        .btn-radio { cursor: pointer; display: inline-block; float: left; -webkit-user-select: none; user-select: none;
        }
        .btn-radio:not(:first-child) { margin-left: 20px;
        }

        @media screen and (max-width: 480px) {
            .btn-radio {
                display: block; float: none;
            }
            .btn-radio:not(:first-child) {
                margin-left: 0; margin-top: 15px;
            }
        }

        .btn-radio svg {
            fill: none; vertical-align: middle;
        }
        .btn-radio svg circle {
            stroke-width: 2; stroke: #4a0101;
        }
        .btn-radio svg path {
            stroke: #4a0101;
        }
        .btn-radio svg path.inner {
            stroke-width: 6; stroke-dasharray: 19; stroke-dashoffset: 19;
        }
        .btn-radio svg path.outer {
            stroke-width: 2; stroke-dasharray: 57; stroke-dashoffset: 57;
        }
        .btn-radio input {
            display: none;
        }
        .btn-radio input:checked + svg path {
            transition: all 0.4s ease;
        }
        .btn-radio input:checked + svg path.inner {
            stroke-dashoffset: 38; transition-delay: 0.3s;
        }
        .btn-radio input:checked + svg path.outer {
            stroke-dashoffset: 0;
        }
        .btn-radio span {
            display: inline-block; vertical-align: middle;
        }
        span {
            margin-left: 10px;
            font-size: 21px;
        }
        .ddlRoom {
            border-radius: 10px;
            height: 29px;
            width: 80px;
            padding-left: 10px;
            margin-top: 15px;
        }
    </style>

        <script src="../js/jquery-1.7.1.js" type="text/javascript"></script>

    <script type="text/javascript">

        $(document).ready(function () {

            var cssName = '<%= HttpContext.Current.Session("cssName")%>';
            cssName = cssName.replace("css","../css");
            $('head').append(cssName);

            $('#rdbP1').click(function () {

            });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div id="DivMain">
            <table style="color: white; font-size: 19px;">
                <tr class="trHeader">
                    <td colspan="5">ลงทะเบียนเครื่องนี้กับนักเรียน</td>
                </tr>
                <tr id="trName">
                    <td style="padding-bottom: 20px;">ชื่อ</td>
                    <td><input type="text" id="txtFirstName" runat="server" class="txtInput" /></td>
                    <td style="padding-left: 20px; padding-bottom: 20px;">นามสกุล</td>
                    <td><input type="text" id="txtLastName" runat="server" class="txtInput" /></td>
                </tr>
                <tr id="trGender">
                    <td style="padding-bottom: 20px;">เพศ</td>
                    <td style="padding-left: 15px;padding-bottom: 20px;">
                        <label for="rdbBoy" class="btn-radio">
                            <input type="radio" runat="server" id="rdbBoy" name="rdbGender" />
                            <svg width="18px" height="18px" viewBox="0 0 20 20">
                                <circle cx="10" cy="10" r="9"></circle>
                                <path d="M10,7 C8.34314575,7 7,8.34314575 7,10 C7,11.6568542 8.34314575,13 10,13 C11.6568542,13 13,11.6568542 13,10 C13,8.34314575 11.6568542,7 10,7 Z" class="inner"></path>
                                <path d="M10,1 L10,1 L10,1 C14.9705627,1 19,5.02943725 19,10 L19,10 L19,10 C19,14.9705627 14.9705627,19 10,19 L10,19 L10,19 C5.02943725,19 1,14.9705627 1,10 L1,10 L1,10 C1,5.02943725 5.02943725,1 10,1 L10,1 Z" class="outer"></path>
                            </svg>
                            <span>ชาย</span>
                        </label>
                        <label for="rdbGirl" class="btn-radio">
                            <input type="radio" runat="server" id="rdbGirl" name="rdbGender" />
                            <svg width="18px" height="18px" viewBox="0 0 20 20">
                                <circle cx="10" cy="10" r="9"></circle>
                                <path d="M10,7 C8.34314575,7 7,8.34314575 7,10 C7,11.6568542 8.34314575,13 10,13 C11.6568542,13 13,11.6568542 13,10 C13,8.34314575 11.6568542,7 10,7 Z" class="inner"></path>
                                <path d="M10,1 L10,1 L10,1 C14.9705627,1 19,5.02943725 19,10 L19,10 L19,10 C19,14.9705627 14.9705627,19 10,19 L10,19 L10,19 C5.02943725,19 1,14.9705627 1,10 L1,10 L1,10 C1,5.02943725 5.02943725,1 10,1 L10,1 Z" class="outer"></path>
                            </svg>
                            <span>หญิง</span>
                        </label>
                    </td>
                    <td style="padding-left: 20px; padding-bottom: 20px;">เบอร์โทรศัพท์</td>
                    <td><input type="text" id="txtTel" runat="server" class="txtInput" /></td>
                </tr>
                <tr id="trUserPWD">
                    <td style="padding-bottom: 20px;">ชื่อผู้ใช้</td>
                    <td><input type="text" id="txtUser" runat="server" class="txtInput" /></td>
                    <td style="padding-left: 20px; padding-bottom: 20px;">รหัสผ่าน</td>
                    <td><input type="password" id="txtPassword" runat="server" class="txtInput" /></td>
                </tr>
            </table>
            
     <%--       <table style="background-color:#b6cc8f; width: 99%; border-radius: 7px; padding: 15px;color:#4a0101; display:none;">--%>
            <table style="background-color:#fbaf5d; width: 99%; border-radius: 7px; padding: 15px;color:#4a0101;">
                <tr>
                    <td>ชั้น</td>
                     <td  style="padding-left: 15px;padding-bottom: 10px;">
                        <label for="rdbP1" class="btn-radio">
                            <input type="radio" runat="server" id="rdbP1" name="rdbClass" />
                            <svg width="18px" height="18px" viewBox="0 0 20 20">
                                <circle cx="10" cy="10" r="9"></circle>
                                <path d="M10,7 C8.34314575,7 7,8.34314575 7,10 C7,11.6568542 8.34314575,13 10,13 C11.6568542,13 13,11.6568542 13,10 C13,8.34314575 11.6568542,7 10,7 Z" class="inner"></path>
                                <path d="M10,1 L10,1 L10,1 C14.9705627,1 19,5.02943725 19,10 L19,10 L19,10 C19,14.9705627 14.9705627,19 10,19 L10,19 L10,19 C5.02943725,19 1,14.9705627 1,10 L1,10 L1,10 C1,5.02943725 5.02943725,1 10,1 L10,1 Z" class="outer"></path>
                            </svg>
                            <span>ป.1</span>
                        </label>
                     </td>  
                    <td>
                        <label for="rdbP4" class="btn-radio">
                            <input type="radio" runat="server" id="rdbP4" name="rdbClass" />
                            <svg width="18px" height="18px" viewBox="0 0 20 20">
                                <circle cx="10" cy="10" r="9"></circle>
                                <path d="M10,7 C8.34314575,7 7,8.34314575 7,10 C7,11.6568542 8.34314575,13 10,13 C11.6568542,13 13,11.6568542 13,10 C13,8.34314575 11.6568542,7 10,7 Z" class="inner"></path>
                                <path d="M10,1 L10,1 L10,1 C14.9705627,1 19,5.02943725 19,10 L19,10 L19,10 C19,14.9705627 14.9705627,19 10,19 L10,19 L10,19 C5.02943725,19 1,14.9705627 1,10 L1,10 L1,10 C1,5.02943725 5.02943725,1 10,1 L10,1 Z" class="outer"></path>
                            </svg>
                            <span>ป.4</span>
                        </label>
                    </td> 
                    <td>
                        <label for="rdbM1" class="btn-radio">
                            <input type="radio" runat="server" id="rdbM1" name="rdbClass" />
                            <svg width="18px" height="18px" viewBox="0 0 20 20">
                                <circle cx="10" cy="10" r="9"></circle>
                                <path d="M10,7 C8.34314575,7 7,8.34314575 7,10 C7,11.6568542 8.34314575,13 10,13 C11.6568542,13 13,11.6568542 13,10 C13,8.34314575 11.6568542,7 10,7 Z" class="inner"></path>
                                <path d="M10,1 L10,1 L10,1 C14.9705627,1 19,5.02943725 19,10 L19,10 L19,10 C19,14.9705627 14.9705627,19 10,19 L10,19 L10,19 C5.02943725,19 1,14.9705627 1,10 L1,10 L1,10 C1,5.02943725 5.02943725,1 10,1 L10,1 Z" class="outer"></path>
                            </svg>
                            <span>ม.1</span>
                        </label>
                    </td>
                    <td>
                        <label for="rdbM4" class="btn-radio">
                            <input type="radio" runat="server" id="rdbM4" name="rdbClass" />
                            <svg width="18px" height="18px" viewBox="0 0 20 20">
                                <circle cx="10" cy="10" r="9"></circle>
                                <path d="M10,7 C8.34314575,7 7,8.34314575 7,10 C7,11.6568542 8.34314575,13 10,13 C11.6568542,13 13,11.6568542 13,10 C13,8.34314575 11.6568542,7 10,7 Z" class="inner"></path>
                                <path d="M10,1 L10,1 L10,1 C14.9705627,1 19,5.02943725 19,10 L19,10 L19,10 C19,14.9705627 14.9705627,19 10,19 L10,19 L10,19 C5.02943725,19 1,14.9705627 1,10 L1,10 L1,10 C1,5.02943725 5.02943725,1 10,1 L10,1 Z" class="outer"></path>
                            </svg>
                            <span>ม.4</span>
                        </label>
                    </td>
                </tr>
                <tr>
                    <td></td>
                     <td  style="padding-left: 15px;padding-bottom: 10px;">
                        <label for="rdbP2" class="btn-radio">
                            <input type="radio" runat="server" id="rdbP2" name="rdbClass" />
                            <svg width="18px" height="18px" viewBox="0 0 20 20">
                                <circle cx="10" cy="10" r="9"></circle>
                                <path d="M10,7 C8.34314575,7 7,8.34314575 7,10 C7,11.6568542 8.34314575,13 10,13 C11.6568542,13 13,11.6568542 13,10 C13,8.34314575 11.6568542,7 10,7 Z" class="inner"></path>
                                <path d="M10,1 L10,1 L10,1 C14.9705627,1 19,5.02943725 19,10 L19,10 L19,10 C19,14.9705627 14.9705627,19 10,19 L10,19 L10,19 C5.02943725,19 1,14.9705627 1,10 L1,10 L1,10 C1,5.02943725 5.02943725,1 10,1 L10,1 Z" class="outer"></path>
                            </svg>
                            <span>ป.2</span>
                        </label>
                     </td>  
                    <td>
                        <label for="rdbP5" class="btn-radio">
                            <input type="radio" runat="server" id="rdbP5" name="rdbClass" />
                            <svg width="18px" height="18px" viewBox="0 0 20 20">
                                <circle cx="10" cy="10" r="9"></circle>
                                <path d="M10,7 C8.34314575,7 7,8.34314575 7,10 C7,11.6568542 8.34314575,13 10,13 C11.6568542,13 13,11.6568542 13,10 C13,8.34314575 11.6568542,7 10,7 Z" class="inner"></path>
                                <path d="M10,1 L10,1 L10,1 C14.9705627,1 19,5.02943725 19,10 L19,10 L19,10 C19,14.9705627 14.9705627,19 10,19 L10,19 L10,19 C5.02943725,19 1,14.9705627 1,10 L1,10 L1,10 C1,5.02943725 5.02943725,1 10,1 L10,1 Z" class="outer"></path>
                            </svg>
                            <span>ป.5</span>
                        </label>
                    </td> 
                    <td>
                        <label for="rdbM2" class="btn-radio">
                            <input type="radio" runat="server" id="rdbM2" name="rdbClass" />
                            <svg width="18px" height="18px" viewBox="0 0 20 20">
                                <circle cx="10" cy="10" r="9"></circle>
                                <path d="M10,7 C8.34314575,7 7,8.34314575 7,10 C7,11.6568542 8.34314575,13 10,13 C11.6568542,13 13,11.6568542 13,10 C13,8.34314575 11.6568542,7 10,7 Z" class="inner"></path>
                                <path d="M10,1 L10,1 L10,1 C14.9705627,1 19,5.02943725 19,10 L19,10 L19,10 C19,14.9705627 14.9705627,19 10,19 L10,19 L10,19 C5.02943725,19 1,14.9705627 1,10 L1,10 L1,10 C1,5.02943725 5.02943725,1 10,1 L10,1 Z" class="outer"></path>
                            </svg>
                            <span>ม.2</span>
                        </label>
                    </td>
                    <td>
                        <label for="rdbM5" class="btn-radio">
                            <input type="radio" runat="server" id="rdbM5" name="rdbClass" />
                            <svg width="18px" height="18px" viewBox="0 0 20 20">
                                <circle cx="10" cy="10" r="9"></circle>
                                <path d="M10,7 C8.34314575,7 7,8.34314575 7,10 C7,11.6568542 8.34314575,13 10,13 C11.6568542,13 13,11.6568542 13,10 C13,8.34314575 11.6568542,7 10,7 Z" class="inner"></path>
                                <path d="M10,1 L10,1 L10,1 C14.9705627,1 19,5.02943725 19,10 L19,10 L19,10 C19,14.9705627 14.9705627,19 10,19 L10,19 L10,19 C5.02943725,19 1,14.9705627 1,10 L1,10 L1,10 C1,5.02943725 5.02943725,1 10,1 L10,1 Z" class="outer"></path>
                            </svg>
                            <span>ม.5</span>
                        </label>
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td  style="padding-left: 15px;">
                        <label for="rdbP3" class="btn-radio">
                            <input type="radio" runat="server" id="rdbP3" name="rdbClass" />
                            <svg width="18px" height="18px" viewBox="0 0 20 20">
                                <circle cx="10" cy="10" r="9"></circle>
                                <path d="M10,7 C8.34314575,7 7,8.34314575 7,10 C7,11.6568542 8.34314575,13 10,13 C11.6568542,13 13,11.6568542 13,10 C13,8.34314575 11.6568542,7 10,7 Z" class="inner"></path>
                                <path d="M10,1 L10,1 L10,1 C14.9705627,1 19,5.02943725 19,10 L19,10 L19,10 C19,14.9705627 14.9705627,19 10,19 L10,19 L10,19 C5.02943725,19 1,14.9705627 1,10 L1,10 L1,10 C1,5.02943725 5.02943725,1 10,1 L10,1 Z" class="outer"></path>
                            </svg>
                            <span>ป.3</span>
                        </label>
                     </td>  
                    <td>
                        <label for="rdbP6" class="btn-radio">
                            <input type="radio" runat="server" id="rdbP6" name="rdbClass" />
                            <svg width="18px" height="18px" viewBox="0 0 20 20">
                                <circle cx="10" cy="10" r="9"></circle>
                                <path d="M10,7 C8.34314575,7 7,8.34314575 7,10 C7,11.6568542 8.34314575,13 10,13 C11.6568542,13 13,11.6568542 13,10 C13,8.34314575 11.6568542,7 10,7 Z" class="inner"></path>
                                <path d="M10,1 L10,1 L10,1 C14.9705627,1 19,5.02943725 19,10 L19,10 L19,10 C19,14.9705627 14.9705627,19 10,19 L10,19 L10,19 C5.02943725,19 1,14.9705627 1,10 L1,10 L1,10 C1,5.02943725 5.02943725,1 10,1 L10,1 Z" class="outer"></path>
                            </svg>
                            <span>ป.6</span>
                        </label>
                    </td> 
                    <td>
                        <label for="rdbM3" class="btn-radio">
                            <input type="radio" runat="server" id="rdbM3" name="rdbClass" />
                            <svg width="18px" height="18px" viewBox="0 0 20 20">
                                <circle cx="10" cy="10" r="9"></circle>
                                <path d="M10,7 C8.34314575,7 7,8.34314575 7,10 C7,11.6568542 8.34314575,13 10,13 C11.6568542,13 13,11.6568542 13,10 C13,8.34314575 11.6568542,7 10,7 Z" class="inner"></path>
                                <path d="M10,1 L10,1 L10,1 C14.9705627,1 19,5.02943725 19,10 L19,10 L19,10 C19,14.9705627 14.9705627,19 10,19 L10,19 L10,19 C5.02943725,19 1,14.9705627 1,10 L1,10 L1,10 C1,5.02943725 5.02943725,1 10,1 L10,1 Z" class="outer"></path>
                            </svg>
                            <span>ม.3</span>
                        </label>
                    </td>
                    <td>
                        <label for="rdbM6" class="btn-radio">
                            <input type="radio" runat="server" id="rdbM6" name="rdbClass" />
                            <svg width="18px" height="18px" viewBox="0 0 20 20">
                                <circle cx="10" cy="10" r="9"></circle>
                                <path d="M10,7 C8.34314575,7 7,8.34314575 7,10 C7,11.6568542 8.34314575,13 10,13 C11.6568542,13 13,11.6568542 13,10 C13,8.34314575 11.6568542,7 10,7 Z" class="inner"></path>
                                <path d="M10,1 L10,1 L10,1 C14.9705627,1 19,5.02943725 19,10 L19,10 L19,10 C19,14.9705627 14.9705627,19 10,19 L10,19 L10,19 C5.02943725,19 1,14.9705627 1,10 L1,10 L1,10 C1,5.02943725 5.02943725,1 10,1 L10,1 Z" class="outer"></path>
                            </svg>
                            <span>ม.6</span>
                        </label>
                    </td>
                </tr>
                <tr>
                    <td>ห้อง</td>
                    <td colspan="4"><input type="text" id="txtRoom" runat="server" style="border-radius: 10px;height: 22px; width: 80px; padding-left: 10px; margin-top: 10px;" /><span>*** ไม่ต้องกรอก " / " ให้กรอกเฉพาะเลขห้องค่ะ</span></td>
                </tr>
            </table>
            <table style="width: 100%;text-align: center;">
                <tr>
                    <td>
                        <input id="btnRegister" runat="server" class="RegisterPageButton" type="button" value="ลงทะเบียน" />
                    </td>
                </tr>
            </table>

        </div>
    </form>
</body>
</html>
