<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ChatTeacherSelectParent.aspx.vb" Inherits="QuickTest.ChatTeacherSelectParent" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0" name="viewport" />
    <title></title>
    <script src="../js/jquery-1.7.1.js"></script>
    <link href="../css/StyleMobile.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        var ua = navigator.userAgent.toLowerCase();
        var isAndroid = ua.indexOf("android") > -1;

        $(function () {
            //ดักถ้าเข้าจาก Tablet ของครู
            if (isAndroid) {
                //$('#imgBack').css('left', '10px');
            }
            
            $('#btnBack').click(function () {
                window.location = '<%=ResolveUrl("~")%>Student/DashboardStudentPage.aspx'
            });

            $('#btnSearch').click(function () {
                window.location = '<%=ResolveUrl("~")%>Watch/ChatTeacherSearchStudentParent.aspx'
            });

        });

        function ParentClick(InputChatRoomId) {
            window.location = "../Watch/Chat.aspx?ChatRoom_Id=" + InputChatRoomId + '&FromTeacher=True';
        }

    </script>

     <style type="text/css">
        .ForDivShowInFo
        {
            text-align: center;
            position: relative;
            top: 15px;
        }
        .spnHead
        {
            font-size: 50px;
            font-weight: bold;
        }
        .ForSpnNotification {
            background-color: red;
            position: relative;
            left: 265px;
            top: -20px;
            width:45px;
            height:30px;
            border-radius: 25px;
            padding: 10px;
            text-align: center;
            color: white;
            font-size: 30px;
        }
         #imgBack {
         position:absolute;
         left:100px;
         top:10px;
         width:80px;
         height:80px;
         cursor:pointer;
         }
         #MainDiv {
         width:870px;
         margin-left:auto;
         margin-right:auto;
         }
           .Forbtn {
        position:absolute;
        left:40px;
        width:150px;
        }
        .Forbtn:hover {
        background-color:rgb(35, 233, 75) !important;
       }
        
    </style>
    
</head>
<body>
    <form id="form1" runat="server">
        <div id="MainDiv" style="text-align: center;padding-top:20px; ">
            <%--<img id="imgBack" src="../Images/Arrow back.png" />--%>
        <%--<img id='ImgBack' src="../Images/Arrow back.png" onclick='BackSelectStudent();' />--%>
        <%--<span class="spnHead">เลือกครู </span>--%>
        <div id="DivSelectParent" runat="server">
          <%--  <div id='DivCover1' class='DivWidth DivCover'>
                <div id='Divpicture1' class='DivWidth DivPicture' >
                     
                    <div class="ForSpnNotification">888</div>
                    </div>
                <div id='DivName" & CountLoop & "' class='DivWidth DivName' >
                    zxcrmvb
                    </div>
            </div>--%>
        </div>
    </div>
    <div class="footer">
        <input type="button" id="btnBack" value="กลับ" class="Forbtn" />
        เลือกผู้ปกครอง
        <input type="button" id="btnSearch" style="left:initial;right:40px;" value="ค้นหา" class="Forbtn" />
    </div>
    </form>
</body>
</html>
