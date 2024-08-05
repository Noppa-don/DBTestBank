<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="UserToChangePassword.aspx.vb"
    Inherits="QuickTest.UserToChangePassword" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="<%=ResolveUrl("~")%>css/fixMenuSlide.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" type="text/css" href="<%=ResolveUrl("~")%>css/styleQuiz.css" />
    <!-- modernizr enables HTML5 elements and feature detects -->
    <meta name="description" content="คลังข้อสอบออนไลน์ให้ใช้งานฟรีๆ สำหรับโรงเรียนที่ใช้หนังสือเรียนแบบเรียนอันถูกหลักวิชาการของสำนักพิมพ์วัฒนาพานิช เท่านั้น!" />
    <meta name="keywords" content="คลัง ข้อสอบ ง่าย จัดชุด ยำข้อสอบ ผสม ทดสอบ " />
    <meta http-equiv="content-type" content="text/html; charset=UTF-8" />
    <script src="<%=ResolveUrl("~")%>js/jquery-1.7.1.js" type="text/javascript"></script>
    <script src="<%=ResolveUrl("~")%>js/Animation.js" type="text/javascript"></script>
    <script src="<%=ResolveUrl("~")%>js/jquery-ui-1.8.18.min.js" type="text/javascript"></script>
    <script src="<%=ResolveUrl("~")%>js/json2.js" type="text/javascript"></script>
    <link rel="stylesheet" href="<%=ResolveUrl("~")%>css/prettyLoader.css" type="text/css"
        media="screen" charset="utf-8" />
    <script src="<%=ResolveUrl("~")%>js/jquery.prettyLoader.js" type="text/javascript"
        charset="utf-8"></script>
    <link rel="stylesheet" href="<%=ResolveUrl("~")%>css/prettyPhoto.css" type="text/css" />
    <script type="text/javascript" src="<%=ResolveUrl("~")%>js/jquery.prettyPhoto.js"
        charset="utf-8"> </script>
    <style type="text/css">
        .style3 {
            font-size: medium;
            background-color: #FFFFFF;
            text-align: right;
            padding-right: 20px;
            width: 40%;
        }

        .style5 {
            background-color: #FFFFFF;
            float: left;
            padding-left: 10px;
            padding-right: 10px;
        }

        .trshoweror {
            padding-left: 10px;
            padding-right: 10px;
            width: auto;
            background-color: pink;
            -webkit-border-radius: .5em;
            -moz-border-radius: .5em;
            border-radius: .5em;
            behavior: url('../css/PIE.htc');
            -pie-track-active: false;
        }

        .trnewpwd {
            padding-left: 10px;
            padding-right: 10px;
            width: 238px;
            background-color: pink;
            -webkit-border-top-left-radius: .5em;
            -webkit-border-top-right-radius: .5em;
            -moz-border-radius: .5em .5em 0px 0px;
            border-radius: .5em .5em 0px 0px;
            behavior: url('../css/PIE.htc');
            -pie-track-active: false;
        }

        .trrenewpwd {
            padding-left: 10px;
            padding-right: 10px;
            width: auto;
            background-color: pink;
            -webkit-border-bottom-left-radius: .5em;
            -webkit-border-bottom-right-radius: .5em;
            -moz-border-radius: 0px 0px .5em .5em;
            border-radius: 0px 0px .5em .5em;
            behavior: url('../css/PIE.htc');
            -pie-track-active: false;
        }
        /* IE CSS HACK!
        *+html .style3{width:300px;} */
        body aside.stamp {
            display: block;
            width: 150px;
            height: 150px;
            background: url(./Images/changePwd/password_icon.png) 0 0 no-repeat;
            background-size: 150px 150px;
            position: absolute; /*margin-top: -100px;*/
            margin-left: 450px;
            margin-top: -50px;
        }

        input[type="button"]:disabled {
            color: #686868;
            text-shadow: 1px 1px #E1F5F8;
            border: solid 1px #BDBDBD;      
        }
        .form_settings .submit {
            filter:none;
        }
        
    </style>
    <script type="text/javascript">
        var ua = navigator.userAgent.toLowerCase();
        var isAndroid = ua.indexOf("android") > -1;
        document.createElement('aside');
        var srcRigth = '<%=ResolveUrl("~")%>Images/changePwd/RightPwd.png';
        var srcWrong = '<%=ResolveUrl("~")%>Images/changePwd/icon_false.gif';

        $(document).ready(function () {
            //hover animation
            InjectionHover('#BtnSubmit', 3);

            if (isAndroid) {
                $('#tdOldPWd').css('font-size', '23px');
                $('#txtOldPwd').css('font-size', '23px');
                $('#tdNewPwd').css('font-size', '23px');
                $('#txtNewPwd').css('font-size', '23px');
                $('#tdRenewPwd').css('font-size', '23px');
                $('#txtReNewPwd').css('font-size', '23px');
                $('h2').css('padding', '0px');
                $('h2').css('margin', '0px');
                $('table').css('margin', '0px 0 10px 0');
                $('#BtnSubmit').css('font-size', '20px');
                $('#BtnSubmit').css('width', '110px');
                $('#BtnSubmit').css('height', '45px');
            }

            $("a[rel^='prettyPhoto']").prettyPhoto({
                default_width: 800,
                default_height: 600,
                modal: true,
                social_tools: false
            });
            $.prettyLoader();
            if (!$.browser.msie && $.browser.version != 8.0) {
                $('#txtOldPwd').focus();
            }            
            $('#txtNewPwd').attr('disabled', 'disabled');
            $('#txtReNewPwd').attr('disabled', 'disabled');
            $('#BtnSubmit').attr('disabled', 'disabled').css('background', '#eee');
            
            if ($.browser.msie) {
                if ($.browser.version == 7.0) {
                    $('.style3').css('width', '40%');
                }
                //$('#BtnSubmit').css('background-color', 'red');
                $('aside').css('width', '200px').css('margin-left', '400px');
            }

            $(function () {
                var delayID = null;
                $('input[class="contact"]').keyup(function (e) {
                    var id = $(this).attr('id');
                    if (delayID == null) {
                        delayID = setTimeout(function () {
                            chkPwdWithTime(id);
                        }, 500);
                    }
                    else if (delayID != null) {
                        clearTimeout(delayID);
                        delayID = setTimeout(function () {
                            chkPwdWithTime(id);
                        }, 500);
                    }
                });
            });
            function chkPwdWithTime(txtPwd) {
                if (txtPwd == 'txtOldPwd') {
                    if ($('#' + txtPwd).val() != "") {
                        var oldPwd = $('#' + txtPwd).val();
                        checkPwd(oldPwd);
                        $('#' + txtPwd).blur();
                    } else {
                        $('#trOldPwd').removeClass('trshoweror');
                        $('#spnErrorOldPwd').hide();
                        $('#imgOldPwd').css('display', 'none');
                        $('#trNewPwd').removeClass('trnewpwd');
                        $('#trReNewPwd').removeClass('trrenewpwd');
                        $('#spnErrorNewPwd').hide();
                        $('#txtNewPwd').val('');
                        $('#txtReNewPwd').val('');
                        $('#imgNewPwd').css('display', 'none');
                        $('#imgReNewPwd').css('display', 'none');
                    }
                }
                else {
                    console.log(1);
                    var newPwd = $('#txtNewPwd').val();
                    var reNewPwd = $('#txtReNewPwd').val();
                    //if ($('#' + txtPwd).is('#txtNewPwd')) {
                    //    $('#imgNewPwd').attr('src', srcRigth).show();
                    //    $('#' + txtPwd).blur();
                    //    $('#txtReNewPwd').attr('disabled', false);
                    //    $('#txtReNewPwd').focus();
                    //}
                    if ($('#txtNewPwd').val() != "" && $('#txtReNewPwd').val() == "") {
                        $('#spnErrorNewPwd').hide();
                        $('#trNewPwd').removeClass('trnewpwd');
                        $('#trReNewPwd').removeClass('trrenewpwd');
                            $('#imgNewPwd').attr('src', srcRigth).show();
                            $('#' + txtPwd).blur();
                            $('#txtReNewPwd').attr('disabled', false);
                            $('#txtReNewPwd').focus();
                            $('#imgReNewPwd').css('display', 'none');
                            $('#BtnSubmit').attr('disabled', 'disabled').css('background', '#eee');
                    } else if ($('#txtNewPwd').val() != "" && $('#txtReNewPwd').val() != "") {
                        if (newPwd != reNewPwd) {
                            $('#trNewPwd').addClass('trnewpwd');
                            $('#trReNewPwd').addClass('trrenewpwd');
                            $('#spnErrorNewPwd').show();
                            $('#imgNewPwd').attr('src', srcWrong).show();
                            $('#imgReNewPwd').attr('src', srcWrong).show();
                            $('#BtnSubmit').attr('disabled', 'disabled').css('background', '#eee');
                        }
                        else {
                            $('#trNewPwd').removeClass('trnewpwd');
                            $('#trReNewPwd').removeClass('trrenewpwd');
                            $('#spnErrorNewPwd').hide();
                            $('#imgNewPwd').attr('src', srcRigth).show();
                            $('#imgReNewPwd').attr('src', srcRigth).show();
                            $('#txtReNewPwd').blur();
                            $('#BtnSubmit').attr('disabled', false).css('background-color', '#46C4DD');
                        }
                    }
                    else if ($('#txtNewPwd').val() == "" && $('#txtReNewPwd').val() != "") {
                        $('#trNewPwd').addClass('trnewpwd');
                        $('#trReNewPwd').addClass('trrenewpwd');
                        $('#spnErrorNewPwd').show();
                        $('#imgNewPwd').attr('src', srcWrong).show();
                        $('#imgReNewPwd').attr('src', srcWrong).show();
                        $('#BtnSubmit').attr('disabled', 'disabled').css('background', '#eee');
                    }
                    else if ($('#txtNewPwd').val() == "" && $('#txtReNewPwd').val() == "")     {                                                         
                        $('#spnErrorNewPwd').hide();
                        $('#trNewPwd').removeClass('trnewpwd');
                        $('#trReNewPwd').removeClass('trrenewpwd');
                        $('#txtNewPwd').focus();
                        $('#txtReNewPwd').attr('disabled', 'disabled');
                        $('#imgNewPwd').css('display', 'none');
                        $('#imgReNewPwd').css('display', 'none');
                        $('#BtnSubmit').attr('disabled', 'disabled').css('background', '#eee');
                    }                    
                }
            }

            //            $('input[class="contact"]').blur(function(){
            //                if($(this).attr('id') == 'txtOldPwd'){
            //                    if($(this).val() != ""){
            //                        var oldPwd = $('#txtOldPwd').val();
            //                    checkPwd(oldPwd);
            //                    }                    
            //                }
            //                else{
            //                    var newPwd = $('#txtNewPwd').val();
            //                    var reNewPwd = $('#txtReNewPwd').val();
            //                    if($(this).is('#txtNewPwd')){
            //                        $('#imgNewPwd').attr('src',srcRigth);
            //                    }
            //                    if( $('#txtNewPwd').val() != "" && $('#txtReNewPwd').val() != ""){
            //                    if(newPwd != reNewPwd){
            //                        $('#trNewPwd').addClass('trnewpwd');
            //                        $('#trReNewPwd').addClass('trrenewpwd');
            //                        $('#spnErrorNewPwd').show();
            //                        $('#imgNewPwd').attr('src',srcWrong);
            //                        $('#imgReNewPwd').attr('src',srcWrong);
            //                        $('#BtnSubmit').attr('disabled','disabled').css('background','none');
            //                        if ($.browser.msie) {
            //                            $('#BtnSubmit').css('background-color','gray');
            //                        }
            //                    }
            //                    else{ 
            //                        $('#trNewPwd').removeClass('trnewpwd');
            //                        $('#trReNewPwd').removeClass('trrenewpwd');
            //                        $('#spnErrorNewPwd').hide();
            //                        $('#imgNewPwd').attr('src',srcRigth);
            //                        $('#imgReNewPwd').attr('src',srcRigth);
            //                        $('#BtnSubmit').attr('disabled',false).css('background-color','#46C4DD');
            //                        }
            //                        }
            //                    }
            //            });

            $('#BtnSubmit').click(function () {
                var newPwd = $('#txtNewPwd').val();
                changePwdCodeBehind(newPwd);
                $(this).replaceWith('<span style="color:red;" id="spanShowResult" >เปลี่ยนรหัสผ่านเรียบร้อยแล้วค่ะ</span>');
                $('input[class="contact"]').attr('disabled', 'disabled');
            });
        });


        function checkPwd(oldPwd) {
            var valReturnFromCodeBehide;
            $.ajax({
                type: "POST",
                url: "<%=ResolveUrl("~")%>UserToChangePassword.aspx/checkOldPwdCodeBehind",
                  data: "{ oldPwd : '" + oldPwd + "'}",  //" 
                  contentType: "application/json; charset=utf-8", dataType: "json",
                  success: function (msg) {
                      if (msg.d != 0) {//session(UserId) หายจะ return 0 - Sefety function
                          valReturnFromCodeBehide = msg.d;
                          //alert('success'+valReturnFromCodeBehide);
                          if (msg.d == "error") {
                              showError();
                          }
                          else {
                              showPass();
                          }
                      } else {
                          alert("session หลุดแล้ว");
                      }
                  },
                  error: function myfunction(request, status) {
                      //alert('shin' + request.statusText + status);    
                  }
              });
              }
              function changePwdCodeBehind(newPwd) {
                  var valReturnFromCodeBehide;
                  $.ajax({
                      type: "POST",
                      url: "<%=ResolveUrl("~")%>UserToChangePassword.aspx/changePwdCodeBehind",
                  data: "{ newPwd : '" + newPwd + "'}",  //" 
                  contentType: "application/json; charset=utf-8", dataType: "json",
                  success: function (msg) {
                      if (msg.d != 0) {//session(UserId) หายจะ return 0 - Sefety function
                          valReturnFromCodeBehide = msg.d;
                          //alert('success'+valReturnFromCodeBehide);
                      }
                  },
                  error: function myfunction(request, status) {
                      //alert('shin' + request.statusText + status);    
                  }
              });
              }

              function showError() {
                  $('#trOldPwd').addClass('trshoweror');
                  $('#txtOldPwd').focus();
                  $('#spnErrorOldPwd').show();
                  $('#txtNewPwd').attr('disabled', 'disabled').val("");
                  $('#txtReNewPwd').attr('disabled', 'disabled').val("");
                  $('#imgOldPwd').attr('src', srcWrong).show();
                  $('#imgNewPwd').css('display', 'none');
                  $('#imgReNewPwd').css('display', 'none');
                  $('#trNewPwd').removeClass('trnewpwd');
                  $('#trReNewPwd').removeClass('trrenewpwd');
                  $('#spnErrorNewPwd').hide();
                  $('#BtnSubmit').attr('disabled', 'disabled').css('background', '#eee');
              }
              function showPass() {
                  $('#trOldPwd').removeClass('trshoweror');
                  $('#spnErrorOldPwd').hide();
                  $('#txtNewPwd').attr('disabled', false);
                  $('#txtNewPwd').focus();
                  $('#imgOldPwd').attr('src', srcRigth).show();
              }

              function ShowHelp() {
                  alert('q');
                  $('#FrameShowHowTo').attr('src', '../HowTo/HowToChangePassword/HowToChangePassword.htm');

                  $('#HowToDialog').show();

              }
              function CloseHelp() {
                  alert('l');

                  $('#HowToDialog').hide();

              }

              $(function () {
                  $('#Help a').stop().animate({ 'marginLeft': '-52px' }, 1000);
                  $('#Help > li').hover(
                 function () {
                     $('a', $(this)).stop().animate({ 'marginLeft': '-2px' }, 200);
                 },
                     function () {
                         $('a', $(this)).stop().animate({ 'marginLeft': '-52px' }, 200);
                     }
                     );
              });

    </script>
   <%-- <% If HttpContext.Current.Application("NeedQuizMode") = True Then%>
    <ul id="Help">
        <li class="about2" style="z-index: 999;"><a title="สงสัยในการใช้งาน ทำตามขั้นตอนตัวอย่างนี้นะคะ"
            id="HelpLogin" onclick="ShowHelp();">ช่วย<br />
            เหลือ<br />
        </a></li>
    </ul>
    <%End If%>--%>
    <div id='Div1' style="display: none; width: 100%; height: 100%; z-index: 100; position: fixed; top: 0px; left: 0px; background-color: Black">
        <iframe id="Iframe1" scrolling="no" style="overflow: hidden; white-space: nowrap; width: 100%; height: 100%; position: relative; margin-left: auto; margin-right: auto;"
            frameborder="0"></iframe>
        <ul id="CHelp">
            <li class="about1" style="z-index: 999;"><a title="จบการฝึกฝน" id="CloseHelp" onclick="CloseHelp();">จบ<br />
            </a></li>
        </ul>
    </div>
    <div id='HowToDialog' style="display: none; width: 100%; height: 100%; z-index: 100; position: fixed; top: 0px; left: 0px; background-color: Black">
        <iframe id="FrameShowHowTo" scrolling="no" style="overflow: hidden; white-space: nowrap; width: 100%; height: 100%; position: relative; margin-left: auto; margin-right: auto;"
            frameborder="0"></iframe>
    </div>
</head>
<body style='background-image: none; background-color: white; margin: 0; padding: 0; overflow: hidden;'>
    <center>
        <form id="contact" runat="server">
            <div id="site_content" style="width: 100%">
                <div class="form_settings">
                    <h2>เปลี่ยนรหัสผ่าน</h2>
                    <aside class="stamp"></aside>
                    <table style="width: 100%">
                        <tr>
                            <td class="style3">รหัสโรงเรียน
                            </td>
                            <td class="style5">
                                <asp:Label ID="lblSchoolId" runat="server" class="contact" Text="" Font-Bold="true"
                                    Width="200px" Font-Size="Larger"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="style3">ชื่อผู้ใช้
                            </td>
                            <td class="style5">
                                <asp:Label ID="lblUserName" runat="server" class="contact" Text="" Font-Bold="true"
                                    Width="200px" Font-Size="Larger"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td id="tdOldPWd" class="style3">รหัสผ่านเก่า
                            </td>
                            <td class="style5" id="trOldPwd">
                                <asp:TextBox ID="txtOldPwd" runat="server" class="contact" TextMode="Password" Width="200px"></asp:TextBox>
                                <img id="imgOldPwd" style="width: 30px; height: 30px; position: absolute; display: none;" />
                                <br />
                                <span style="display: none; padding-top: 5px" id='spnErrorOldPwd'>รหัสผ่านเก่าไม่ถูกค่ะ</span>
                            </td>
                        </tr>
                        <tr>
                            <td id="tdNewPwd" class="style3">รหัสผ่านใหม่
                            </td>
                            <td class="style5" id="trNewPwd">
                                <asp:TextBox ID="txtNewPwd" runat="server" class="contact" TextMode="Password" Width="200px"></asp:TextBox>
                                <img id="imgNewPwd" style="width: 30px; height: 30px; position: absolute; display: none;" />
                                <br />
                                <%--<asp:Label ID="lblErrorNewPwd" runat="server"></asp:Label>--%>
                            </td>
                        </tr>
                        <tr>
                            <td id="tdRenewPwd" class="style3">รหัสผ่านใหม่อีกครั้ง
                            </td>
                            <td class="style5" id="trReNewPwd">
                                <asp:TextBox ID="txtReNewPwd" runat="server" class="contact" TextMode="Password"
                                    Width="200px"></asp:TextBox>
                                <img id="imgReNewPwd" style="width: 30px; height: 30px; position: absolute; display: none;" />
                                <br />
                                <span style="display: none; padding-top: 5px; width: auto" id='spnErrorNewPwd'>รหัสผ่านใหม่ทั้งสองช่องไม่เหมือนกันค่ะ</span>
                            </td>
                        </tr>
                        <tr>
                            <td class="style3"></td>
                            <td class="style5" style="background-color: #FFFFFF;">
                                <input type="button" id="BtnSubmit" value="ตกลง" class="submit" style="margin: 0 0 0 0;" />
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </form>
    </center>
</body>
</html>
