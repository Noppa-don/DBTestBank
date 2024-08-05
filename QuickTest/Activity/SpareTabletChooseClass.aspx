<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="SpareTabletChooseClass.aspx.vb"
    Inherits="QuickTest.SpareTabletChooseClass" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0" name="viewport" />
    <script src="../js/jquery-1.7.1.js" type="text/javascript"></script>
    <script src="../js/jquery-ui-1.8.18.js" type="text/javascript"></script>
    <script src="../js/facescroll.js" type="text/javascript"></script>
    <script type="text/javascript">

        $(function () {

            $('.ForHover').mouseenter(function () {
                $(this).css('background-color', 'gold');
                $(this).css('cursor', 'pointer');
            });

            $('.ForHover').mouseout(function () {
                $(this).css('background-color', 'cornflowerblue');
                $(this).css('cursor', 'none');
            });

            $('#DivClass').alternateScroll();
            $('#DivSoundlab').alternateScroll();
            $('.alt-scroll-vertical-bar').html('<img src="../Images/sprite_On.PNG" /><img src="../Images/sprite_Down.PNG" style="margin-top: 64px;" />')

        });
        

        function ClickChooseLevel(LevelName, SchoolCode,UseSoundlab) {
            //    window.location = "../Activity/SpareTabletChooseSeat.aspx?SpareLevelName=" + LevelName + "&SpareSchoolCode=" + SchoolCode + ""
                $.ajax({ type: "POST",
                url: "<%=ResolveUrl("~")%>Activity/SpareTabletChooseClass.aspx/ClickChooseLevelCB",
                data: "{ SpareLevelName:'" + LevelName + "',SpareSchoolCode:'" + SchoolCode + "',SpareUseTablab:'" + UseSoundlab + "'}",   
	            contentType: "application/json; charset=utf-8", dataType: "json",   
	            success: function (msg) {
                    nextCmd = msg.d;
                    //alert(msg.d);
                    if (msg.d == 'Complete') {//ถ้าได้รับ Complete กลับมาแสดงว่า Code ทำงานได้ถุกต้องให้ Redirect ไปหน้าเลือกที่นั่งเพื่อเปลี่ยนเครื่องสำรองเลย
                            window.location = '../Activity/SpareTabletChooseSeat.aspx'
                    }
                    else
                    {
                    alert('บางอย่างผิดพลาดโปรดลองอีกครั้ง');
                    }
	            },
	            error: function myfunction(request, status)  {
                alert('บางอย่างผิดพลาดโปรดลองอีกครั้ง')
	            }
	        });
       }


       

    </script>


    <style type="text/css">
        .ForText
        {
            font-size: 30px;
        }
        .ForTd
        {
            padding-top: 10px;
            padding-left: 15px;
            width: 485px;
        }
        .ForTdClass
        {
            padding-top: 10px;
            padding-left: 15px;
            width: 485px;
            border-right: 1px solid;
        }
        .ForHover
        {
            background-color:cornflowerblue;
            }
          
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div id='MainDiv' style='width: 970px;' class='ForText'>
    <h3 style='margin-left:90px;'>เลือกห้องที่ต้องการเปลี่ยนไปใช้เครื่องสำรอง</h3>
        <div id='DivClass' style='height: 350px; float: left; border: 1px solid;margin-left:30px;overflow:auto;'>
            <table style='width: 400px;'>
                <tr>
                    <td style='text-align: center; border-bottom: 1px solid;'>
                        ห้องเรียน
                    </td>
                </tr>
                <tr>
                    <td runat="server" id='tagClass'>
                    </td>
                </tr>
            </table>
        </div>
        <div id='DivSoundlab' style='height: 350px; float: left; margin-left:1px; border: 1px solid;overflow:auto;'>
            <table style='width: 400px;'>
                <tr>
                    <td style='text-align: center; border-bottom: 1px solid;'>
                        ห้อง Soundlab
                    </td>
                </tr>
                <tr>
                    <td runat="server" id='tagSoundlab'>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    </form>
</body>
</html>
