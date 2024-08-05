<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Recommend.aspx.vb" Inherits="QuickTest.Recommend" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <style type="text/css">
        .imgPlayQuiz
        {
            position: absolute;
            right: 50%;
            margin-right: -25px;
            width: 50px;
            height: 50px;
            display: none;
        }
    </style>
    <script src="../js/jquery-1.7.1.js" type="text/javascript"></script>
    <script src="../js/jquery-ui-1.8.18.js" type="text/javascript"></script>
    <script src="../js/slimScroll.js" type="text/javascript"></script>
    <script src="../js/jquery.ui.touch-punch.min.js" type="text/javascript"></script>
    <script src="../js/facescroll.js" type="text/javascript"></script>
    <script src="../js/jquery-1.7.1.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.signalR-2.0.2.min.js" type="text/javascript"></script>
    <script src='<%=ResolveClientUrl("~/signalr/hubs") %>' type="text/javascript"></script>
    <script type="text/javascript">

        $("#divListing").alternateScroll();
        $('.alt-scroll-vertical-bar').html('<img src="../Images/sprite_On.PNG" /><img src="../Images/sprite_Down.PNG" style="margin-top: 52px;" />')
        ToggleBox();


        function ToggleBox() {

            $("#divListing").slideToggle("slow");
            if ($.browser.msie) {
                if ($.browser.version <= 7) {
                    $('#divListing').css('overflow', 'auto');
                }
            }

        }

        function ChengeColorTr() {
            var i = 1;

            $('.bordered').find('tr').each(function () {

                if (i > 1) {
                    var td = $(this).find("td");
                    if (i % 2 == 0) {
                        $(td).css('background', '#FFFFCC');
                    } else {
                        $(td).css('background', '#FFFFFF');
                    }
                }
                i = i + 1;
            });
            $('.imgPlayQuiz').hide();
        }  
    </script>
    <link rel="stylesheet" href="<%=ResolveUrl("~")%>css/prettyPhoto.css" type="text/css" />
    <link rel="stylesheet" href="<%=ResolveUrl("~")%>css/prettyLoader.css" type="text/css" />
    <script src="<%=ResolveUrl("~")%>js/jquery.prettyLoader.js" type="text/javascript"
        charset="utf-8"></script>
    <script src="<%=ResolveUrl("~")%>js/jquery.prettyPhoto.js" type="text/javascript"
        charset="utf-8"></script>
    <script type="text/javascript" charset="utf-8">
        $(document).ready(function () {
            $("a[rel^='prettyPhoto']").prettyPhoto({
                default_width: 800,
                default_height: 600,
                modal: true, social_tools: false
            });
            $.prettyLoader();
        });
        
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('tr').click(function () {
                var id = $(this).attr('id');
                ChengeColorTr();

                //$(this).children('td').addClass('forPlay');
                $(this).children('td').css('background', '#17FFBE');
                $('#play_' + id).show();
                
            });

            $('.imgPlayQuiz').click(function(){
                var id = $(this).attr('id');
                id=id.replace('play_','');
                 SaveTestsetAndQuiz(id);
                });
        });

          function SaveTestsetAndQuiz(QsetId) {
              var valReturnFromCodeBehide;
              $.ajax({ type: "POST",
                  url: "<%=ResolveUrl("~")%>PracticeMode_Pad/Recommend.aspx/SaveTestsetAndQuiz",
                  async: false,
                  data: "{ QsetId :  '" + QsetId + "'}",
                  contentType: "application/json; charset=utf-8", dataType: "json",   
                  success: function (msg) {
                      if (msg.d != 0) {//session(UserId) หายจะ return 0 - Sefety function
                          valReturnFromCodeBehide = msg.d;
                          window.location.href = msg.d;
                      }
                              },
                  error: function myfunction(request, status) {

                  }
                 });
                 }
        
    </script>
</head>
<body>
    <script type="text/javascript">
        document.createElement('article');
        document.createElement('aside');
        document.createElement('figure');
        document.createElement('footer');
        document.createElement('header');
        document.createElement('hgroup');
        document.createElement('nav');
        document.createElement('section');
    </script>
    <div style="width: 100%; height: 60px; background-color: Aqua;">
        <table style="width: 100%;">
            <tr>
                <td>
                    ทักษะที่ต้องปรับปรุง
                </td>
            </tr>
        </table>
    </div>
    <div id="divListing" class="ListingContent" style='text-align: center; position: relative;
        overflow: scroll; visibility: inherit'>
        <asp:Repeater ID="Listing" runat="server">
            <HeaderTemplate>
                <table class="bordered" style="width: 91%; border-spacing: 4; margin-top: 0px; margin-left: 41px;">
                    <thead>
                        <tr>
                            <th>
                                ชื่อ
                            </th>
                        </tr>
                    </thead>
            </HeaderTemplate>
            <ItemTemplate>
                <tr id="<%# Container.DataItem("Qset_Id")%>">
                    <td style="background: #FFFFCC;">
                        <img id="play_<%# Container.DataItem("Qset_Id")%>" src="../Images/upgradeClass/Actions-arrow-right-icon.png"
                            class="imgPlayQuiz" />
                        <%# Container.DataItem("Qset_Name")%>
                        <img src="../Images/Delete-icon.png" style='float: right; cursor: pointer; visibility: <%# Container.DataItem("IsPracticed")%>' />
                    </td>
                </tr>
            </ItemTemplate>
            <AlternatingItemTemplate>
                <tr id="<%# Container.DataItem("Qset_Id")%>">
                    <td style="background: #FFFFFF;">
                        <img id="play_<%# Container.DataItem("Qset_Id")%>" src="../Images/upgradeClass/Actions-arrow-right-icon.png"
                            class="imgPlayQuiz" />
                        <%# Container.DataItem("Qset_Name")%>
                        <img src="../Images/Delete-icon.png" style='float: right; cursor: pointer; visibility: <%# Container.DataItem("IsPracticed")%>' />
                    </td>
                </tr>
            </AlternatingItemTemplate>
            <FooterTemplate>
                </table>
            </FooterTemplate>
        </asp:Repeater>
    </div>
    <div>
        <table style="width: 100%;">
            <tr>
                <td style="width: 50%; float: left; height: 65px; text-align: center; background-color: pink;
                    line-height: 65px;">
                    ข้อสอบที่เกี่ยวข้อง
                </td>
                <td style="width: 49%; text-align: right; height: 65px; float: right; text-align: center;
                    background-color: pink; line-height: 65px;">
                    ข้อสอบที่แนะนำ
                </td>
            </tr>
        </table>
    </div>
</body>
<script type="text/javascript">
        $ (document).ready(function() {
            //getnextaction ตอนท้ายเพจ   
            getNextAction();
        });

       function getNextAction() {       
         var DeviceId = '<%=DVID %>';
            $.ajax({ type: "POST",
	            url: "<%=ResolveUrl("~")%>DroidPad/StudentAction.aspx/SendToGetNextAction",
	            data: "{ DeviceUniqueID: '" + DeviceId + "',IsFirstTime:'NoValue',IsTeacher:false}",
	            contentType: "application/json; charset=utf-8", dataType: "json",   
	            success: function (msg) {
                //ถ้าค่าที่ Return กลับมาไม่เป็นค่าว่างให้ ReLoad หน้าใหม่
                var objJson = jQuery.parseJSON(msg.d);
                if (objJson.Param.NextURL !== '') {
                                    //window.location = '../Activity/ActivityPage_Pad.aspx?DeviceUniqueID=' + DeviceId;
//                    if (objJson.Param.NextURL == '/Activity/ActivityPage_Pad.aspx?i=t&DeviceUniqueID=' + DeviceId) { 
//                        var joinQuiz = '../Activity/ActivityPage_Pad.aspx?DeviceUniqueID=' + DeviceId;
//                        dialogInterrupt(joinQuiz);
//                    }
                     if (objJson.Param.NextURL == '/Activity/ActivityPage_Pad.aspx?i=t') { 
                        var joinQuiz = '../Activity/ActivityPage_Pad.aspx?DeviceUniqueID=' + DeviceId;
                        dialogInterrupt(joinQuiz);
                    }
                }
	            },
	            error: function myfunction(request, status)  {
                alert('บางอย่างผิดพลาดโปรดลองอีกครั้ง')
	            }
	        });
        }
</script>
</html>
