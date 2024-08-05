<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Contact.aspx.vb" Inherits="QuickTest.Contact" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
    <head runat="server">
    <title>ติดต่อเจ้าหน้าที่</title>
    <script src="js/jquery-1.7.1.js" type="text/javascript"></script>
    <script src="js/Animation.js" type="text/javascript"></script>
    <script src="js/jquery-ui-1.8.18.min.js" type="text/javascript"></script>
    <meta http-equiv="content-type" content="text/html; charset=UTF-8" />
    <link rel="stylesheet" type="text/css" href="./css/style.css" />
    <link href="css/fixMenuSlide.css" rel="stylesheet" type="text/css" />
    <script src="<%=ResolveUrl("~")%>js/jquery-ui-1.8.18.min.js" type="text/javascript"></script>
    <link href="../css/jquery-ui-1.8.18.custom.min.css" rel="stylesheet" />
    <script type="text/javascript" src="js/modernizr-1.5.min.js"></script>

        <style type="text/css">

        .Forbtn {
            margin-left: 240px;
            width: 130px;
            font: 100% 'THSarabunNew'!important;
            border: 0;
            padding: 2px 0 3px 0;
            cursor: pointer;
            background: #1EC9F4!important;  
            -moz-border-radius: .5em;
            -webkit-border-radius: .5em;
            border-radius: .5em; /*behavior:url(border-radius.htc);*/
            -webkit-box-shadow: 0 1px 2px rgba(0,0,0,.2);
            -moz-box-shadow: 0 1px 2px rgba(0,0,0,.2);
            box-shadow: 0 1px 2px rgba(0,0,0,.2);
            color: #FFF;
            border: solid 1px #0D8AA9!important;
            text-shadow: 1px 1px #178497;
            behavior: url('../css/PIE.htc');
            background: url(data:image/svg+xml;base64,PD94bWwgdmVyc2lvbj0iMS4wIiA/Pgo8c3ZnIHhtbG5zPSJodHRwOi8vd3d3LnczLm9yZy8yMDAwL3N2ZyIgd2lkdGg9IjEwMCUiIGhlaWdodD0iMTAwJSIgdmlld0JveD0iMCAwIDEgMSIgcHJlc2VydmVBc3BlY3RSYXRpbz0ibm9uZSI+CiAgPGxpbmVhckdyYWRpZW50IGlkPSJncmFkLXVjZ2ctZ2VuZXJhdGVkIiBncmFkaWVudFVuaXRzPSJ1c2VyU3BhY2VPblVzZSIgeDE9IjAlIiB5MT0iMCUiIHgyPSIwJSIgeTI9IjEwMCUiPgogICAgPHN0b3Agb2Zmc2V0PSIwJSIgc3RvcC1jb2xvcj0iIzYzY2ZkZiIgc3RvcC1vcGFjaXR5PSIxIi8+CiAgICA8c3RvcCBvZmZzZXQ9IjEwMCUiIHN0b3AtY29sb3I9IiMxN2IyZDkiIHN0b3Atb3BhY2l0eT0iMSIvPgogIDwvbGluZWFyR3JhZGllbnQ+CiAgPHJlY3QgeD0iMCIgeT0iMCIgd2lkdGg9IjEiIGhlaWdodD0iMSIgZmlsbD0idXJsKCNncmFkLXVjZ2ctZ2VuZXJhdGVkKSIgLz4KPC9zdmc+);
            background: -moz-linear-gradient(top, #63cfdf 0%, #17b2d9 100%); /* FF3.6+ */
            background: -webkit-gradient(linear, left top, left bottom, color-stop(0%,#63cfdf), color-stop(100%,#17b2d9)); /* Chrome,Safari4+ */
            background: -webkit-linear-gradient(top, #63cfdf 0%,#17b2d9 100%); /* Chrome10+,Safari5.1+ */
            background: -o-linear-gradient(top, #63cfdf 0%,#17b2d9 100%); /* Opera 11.10+ */
            background: -ms-linear-gradient(top, #63cfdf 0%,#17b2d9 100%); /* IE10+ */
            background: linear-gradient(to bottom, #63cfdf 0%,#17b2d9 100%); /* W3C */
            filter: progid:DXImageTransform.Microsoft.gradient( startColorstr='#63cfdf', endColorstr='#17b2d9',GradientType=0 ); /* IE6-8 */
        }
        
        input[type="radio"] {
            width : auto!important;    
        }

            tr {
                vertical-align: top;
            }
    </style>

</head>
<body style='background-image: none; background-color: white; margin: 0; padding: 0; overflow: hidden;'>
    <div id="main" style="margin: -20px auto !important; width: 650px;">
        <div id="site_content" style="width: 580px;">

            <div class="content" style="width: 550px;">
                <div style="text-align:center;">
                    <asp:Label ID="lblError" runat="server" Visible="false" ForeColor="Red" Text="ไม่สามารถส่ง Email ได้ค่ะ ลองใหม่อีกครั้งนะค่ะ"></asp:Label>
                </div>
                <form action="#" method="post" runat="server">
                   <%-- <div class="form_settings">--%>
                        <table class="form_settings">
                            <tr>
                                <td><span>เรื่อง</span></td>
                                <td colspan="4">
                                    <select id="id" name="dropdown" style="width:300px;">
                                        <option value="0" selected="selected">เลือกหัวข้อค่ะ ...</option>
                                        <option value="1">เข้าไม่ได้</option>
                                        <option value="2">เลือกข้อสอบไม่ได้</option>
                                        <option value="3">พิมพ์ออกไม่ได้ ตกขอบกระดาษ</option>
                                        <option value="4">สับสน ไม่เข้าใจว่าใช้งานยังไง</option>
                                        <option value="5">บางจุดกดแล้วไม่ทำงาน หรือ น่าจะเสีย</option>
                                        <option value="6">แนะนำเพิ่มเติม เสนอแนะไอเดีย</option>
                                        <option value="7">อยากใช้บ้างต้องทำอย่างไร</option>
                                        <option value="9">เรื่องอื่นๆ</option>
                                    </select>
                                </td>
                            </tr>
                            <tr>
                                <td><span>รายละเอียด</span></td>
                                <td colspan="4">
                                    <textarea rows="4" cols="1" id="txtDescript" name="descript" style="width:300px;"></textarea>
                                </td>
                                <td style="color:red;">**</td>
                            </tr>
                            <tr>
                                <td><span>ชื่อ-นามสกุล</span></td>
                                <td colspan="4"><input type="text" name="descriptName" value="" style="width:300px;" /></td>
                                <td style="color:red;">**</td>
                            </tr>
                            <tr>
                                <td><span>อีเมล์</span></td>
                                <td colspan="4"><input type="text" name="Email" value="" style="width:300px;"/></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td><span>เบอร์ติดต่อ</span></td>
                                <td colspan="4"><input type="text" name="Tel" value="" style="width:300px;" /></td>
                                  <td style="color:red;">**</td>
                                <td></td>
                            </tr>
                            <tr>
                                <td><span>ติดต่อกลับทาง</span></td>
                                <td> 
                                    <asp:RadioButton ID="radioMail" GroupName="ContactBy" runat="server" /> 
                              <%--      <input name="radioMail" type="radio" value="radioMail" onselect="true" style="width:auto;" />--%>
                                </td>
                                <td> 
                                    <label for="radioMail">อีเมล์</label>    
                                </td>
                                <td>
                                    <asp:RadioButton ID="radioTel" GroupName="ContactBy" runat="server" />  
                      <%--              <input name="radioTel" type="radio" value="radioTel" style="width:auto;" />--%>
                                </td>
                                <td> 
                                    <label for="radioTel">โทรศัพท์</label>
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td colspan="6" style="text-align: center;padding-top: 25px;">
                                    <asp:Button ID="btnSubmit" runat="server" text="ส่ง" 
                                                class="Forbtn" style="width: 95px; height: 40px; margin: 0px 20px; line-height: 40px; border-radius: 10px; color: white; 
                                                position: initial; top: 0px; text-align: center;" />
                                </td>
                            </tr>
                            
                        </table>
                        
          <%--              <p style="padding-top: 15px">
                            <span><asp:Button ID="btnSubmit" runat="server" Text="ส่งคำถาม" class="submit" /></span>

                        </p>--%>

                        <aside class="stamp"></aside>
                 <%--   </div>--%>
                </form>
            </div>
            <div id="dialog"></div>
        </div>
        </div>
        <!-- javascript at the bottom for fast page loading -->
        <script type="text/javascript" src="js/jquery.js"></script>
        <script type="text/javascript" src="js/jquery.easing-sooper.js"></script>
        <script type="text/javascript" src="js/jquery.sooperfish.js"></script>
        <style type="text/css">

            table tr td {
                background-color : white;
                font-weight: bold;
            }

        </style>

        <script type="text/javascript">
            $(document).ready(function () {
                $('ul.sf-menu').sooperfish();
            });
        </script>
        <%--<script type="text/javascript" src="../js/jquery-1.7.1.js"></script>--%>
        <script type="text/javascript">
            $(document).ready(function () {
                //hover animation
                InjectionHover('#btnSubmit', 3);
                
                //ie to use checkbox:checked
                if ($.browser.msie) {
                    if ($.browser.version > 6.0) {
                        //alert("IE");                 
                        $('input:checkbox').click(function () {
                            if ($(this).attr('checked') == 'checked') {
                                //alert("true");
                                var chkT = { 'background-image': 'url(../images/bullet_checked.gif)' };
                                $(this).next().css(chkT);
                            }
                            else if ($(this).attr('checked') != 'checked') {
                                //alert("false");
                                var chkF = { 'background': 'url(../images/bullet.gif) center left no-repeat', 'height': '21px', 'padding-left': '21px' };
                                $(this).next().css(chkF);
                            }
                        });
                    }
                    else if ($.browser.version <= 6.0) {
                    }
                }
                else if ($.browser.mozilla) {
                    
                }

                //$('#btnSubmit').click(function () {
                //    //setTimeout(function () {
                //    //    SendEmailContact()
                //    //}, 1000);
                //    SendEmailContact();
                //});

            });
            function ShowHelp() {
                alert('q');
                $('#FrameShowHowTo').attr('src', '../HowTo/HowToContact/HowToContact.htm');

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

            function callDialog(txt) {
                var $d = $('#dialog');
                var myBtn = {};
                myBtn["ตกลง"] = function () {
                    $d.dialog('close');
                };
                $d.dialog({ buttons: myBtn, draggable: false, resizable: false, modal: true }).dialog('option', 'title', txt);
            }

            function SendEmailContact() {
                var fromEmail = 'test@test.com';
                var strBody = $('#txtDescript').val();
                $.ajax({
                    type: "POST",
                    url: "<%=ResolveUrl("~")%>Contact.aspx/sendEmailToAdmin",
                    async:true,
              data: "{fromEmail : '" + fromEmail + "', strBody : '" + strBody + "' }",  //" 
              contentType: "application/json; charset=utf-8", dataType: "json",
              success: function (msg) {

              },
              error: function myfunction(request, status) {

              }
          });
      }

            
        </script>
        
</body>
</html>
