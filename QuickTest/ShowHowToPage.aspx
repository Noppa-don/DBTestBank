<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ShowHowToPage.aspx.vb"
    Inherits="QuickTest.ShowHowToPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="js/jquery-1.7.1.js" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" href="./css/style.css" />
    <style type="text/css">
        @import url(../fonts/thsarabunnew.css);
        .MenuList
        {
            font: 18px 'THSarabunNew';
            color: #362C20;
            line-height: 7.5em;
            top: 0;
        }
        .top
        {
            float: left;
            width: 100%;
            height: 100%;
            position: relative;
            top: 0;
            background-color: #FFC76F;
            box-shadow: 0px 0px 10px rgba(0, 0, 0, 0.7);
        }
        .menuItem
        {
            position: relative;
            float: left;
            padding: 5px 15px 5px 15px;
            height: 60px;
            font-size:18px;
        }
        .menuItem:hover
        {
            background-color: #FDA212;
            -webkit-border-radius: 5px;
            padding: 10px 15px 5px 15px;
            height: 80px;
            font-weight:bold;
            cursor:pointer;
        }
        .selected
        {
            background-color: #FDA212;
            -webkit-border-radius: 5px;
              height: 80px;
            color: #F4F7FF;
        }
        .clearAll
        {
            clear: both;
            line-height: 0;
            height: 0;
        }
        #HowTo
        {
            width: 100%;
            height: 100%;
            position: relative;
            background-color: #FFFFFF;;
            float: left;
        }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            // page load ให้ ตั้งค่า default ไว้ก่อน
            //            $('#HowTo').html('<iframe src="../Training/Introduction/Introduction.htm" width="1000px" height="1000px" ></iframe>');
            $('#HowTo').html('<iframe src="../Training/Report/Report.htm" width="800px" height="650px" ></iframe>');
            // เมื่อคลิกที่ปุ่มเมนู
            $('.menuItem').click(function () {
                var tutorial = '';
                if ($(this).hasClass('selected')) {
                    // ถ้าเค้าเลือกตัวเก่าไม่ต้องให้โหลดเพจ tutorial ใหม่
                    //$(this).removeClass('selected');
                }
                else {
                    $('.menuItem').removeClass('selected');
                    $(this).addClass('selected');
                }
                // ใช้ iframe ตัวไหน ตามเมนูที่เราเลือก
                if ($(this).is('#introduction')) {
                    $('#HowTo').html('<iframe src="../Training/Introduction/Introduction.htm" width="1000px" height="1000px" ></iframe>');

                }
                else if ($(this).is('#manageQuiz')) {
                    $('#HowTo').html('<iframe src="../Training/CreateTestset/CreateTestset.htm" width="1000px" height="1000px" ></iframe>');
                }
                else if ($(this).is('#modifyQuiz')) {
                    $('#HowTo').html('<iframe src="../Training/EditTestset/EditTestset.htm" width="1000px" height="1000px" ></iframe>');
                }
                else if ($(this).is('#Report')) {
                    $('#HowTo').html('<iframe src="../Training/Report/Report.htm" width="850px" height="700px" ></iframe>');
                }


            });
        });
        //        function getTutorial(tutorial) {
        //        $.ajax({ type: "POST",
        //	            url: "<%=ResolveUrl("~")%>ShowHowToPage.aspx/getIframeTutorial",
        //	            data: "{ tutorial: '" + tutorial + "'}",  //" 
        //	            contentType: "application/json; charset=utf-8", dataType: "json",   
        //	            success: function (data) {
        //                    //$('#HowTo').html('');
        //                    $('#HowTo').html(<'iframe src=""../HowTo/HowToSelectClassSubject/HowToSelectClassSubject.htm"" width='100%' height='500'></iframe>');
        //	            },
        //	            error: function myfunction(request, status)  {       
        //	            }
        //	        });
        //        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div style='width: 100%; height: 100%; margin-left: auto; margin-right: auto; position: relative;'>
        <%--<table>
            <tr>
                <td>
                    <table>
                        <tr>
                            <td>
                                <div class="MenuList" 
                                    style=" left: 10px; position: relative; height:150px; width:220px; ">
                                    วิธีการใช้งานเบื้องต้น
                                </div>
                            
                                <div class="MenuList" 
                                style="left: 10px; position: relative;height:150px;width:220px;">
                                    การจัดชุดข้อสอบ
                                </div>
                              
                                <div class="MenuList" 
                                    style=" left: 10px; text-align:center; position: relative;height:150px;width:220px; background-color:#f5f4f1;">
                                    การแก้ไขชุดข้อสอบ
                                </div>
                            </td>
                        </tr>
                    </table>
                </td>
                <td>
                    <div id='HowTo' runat='server' style='width: 600px; height: 500px; position: relative;
                        top: 0px; background-color: Black;'>--%>
        <%-- <iframe src="../HowTo/HowToSelectClassSubject/HowToSelectClassSubject.htm" width="98%"  height="600px"></iframe>--%>
        <%--<iframe src="../Training/3.แก้ไขชุดข้อสอบเดิมและแสดงไฟล์ชุดข้อสอบ/3.แก้ไขชุดข้อสอบเดิมและแสดงไฟล์ชุดข้อสอบ.htm"
                            width="600px" height="500px"></iframe>
                    </div>
                </td>
            </tr>
        </table>--%>
        <div class='top'>
            <table>
                <tr>
                    <td>
                        <div class='menuItem' id='Report'>
                            ดูรายงาน</div>
                        <div class='menuItem' id='manageQuiz'>
                            การจัดชุดข้อสอบ</div>
                        <div class='menuItem' id='modifyQuiz'>
                            การแก้ไขชุดข้อสอบ</div>
                        <div class='menuItem' id='introduction'>
                            วิธีการใช้งานเบื้องต้น</div>
                    </td>
                    <td>
                        <div id='HowTo' style="text-align: right; width: 700; height: 550;" runat='server'>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
        </div>
    </form>
    <%--    <div class='clearAll'>
                        </div>--%>
</body>
</html>
