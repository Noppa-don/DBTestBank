<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="TeacherNewsDetail.aspx.vb"
    Inherits="QuickTest.TeacherNewsDetail" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0" name="viewport" />
    <title></title>
    <style type="text/css">
        .DivTeacherDetail {
            width: 85%;
            height: 153px;
            margin: auto;
            border-radius: 5px;
            font-size: 20px;
            padding: 15px;
            margin-top: 30px;
            background-color: rgb(76, 170, 179);
            color: white;
            text-align: left;
        }

        .DivForImg {
            width: 150px;
            height: 150px;
            float: left;
            border: 1px solid #fff;
            border-radius: 5px;
        }

        .ForDivSubject {
            border: 1px solid;
            border-radius: 5px;
            font-size: 70px;
            text-align: center; /*padding:10px;*/
        }

        .spnStatus {
            font-size: 70px;
        }

        .ForDivInfo {
            border-bottom: 1px solid;
        }

        .imgBack {
            width: 65px;
            position: fixed;
            top: 5px;
            left: 5px;
            cursor: pointer;
        }

        .FortdStudentHomework {
            width: 50%;
            text-align: center;
        }

        .ForDivInfo
        {
            border-bottom: 1px solid;
            margin-top: 10px;
            width: 95%;
            margin-left: auto;
            margin-right: auto;
        }
        .ForLeft {
            border-bottom: 1px solid #fff;
            text-align: center;
        }

        .ForRight {
            border-left: 1px solid #fff;
            border-bottom: 1px solid #fff;
        }

        td.ForRight {
            padding-left: 10px;
        }
        /*Span*/
        .spnHeadNews {
            font-size: 50px;
            font-weight: bold;
            position: relative;
            top: 20px;
        }

        .spnDate {
            font-size: 20px;
        }

        .spnNewsDetail {
            font-size: 25px;
        }

        #DivTeacherNews {
            margin: auto;
            margin-top: 30px;
            width: 95%;
            background-color: #5ECBFF;
            border-radius: 5px;            
        }

        th {
            font-size: 30px;
            padding: 10px;
            background: rgb(0, 153, 255);
        }

            th.ForLeft {
                border-radius: 5px 0 0 0;
            }

            th.ForRight {
                border-radius: 0 5px 0 0;
            }

        .tdRight {
            text-align: left;
        }

        td {
            padding: 5px;
        }

        .spnhead {
            font-size: 50px;
            font-weight: bold;
        }

        #TeacherNewsTable {
            border: 1px solid #cdcdcd;
            border-radius: 5px;
        }
        .NewIcon
        {
            width:35px;
            height:25px;
            margin-left:-15px;
            padding-right: 5px;
        }
    </style>
    <link href="../css/StyleMobile.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.7.1.js" type="text/javascript"></script>
    <script src="../js/jquery.blockUI.js" type="text/javascript"></script>
    <script type="text/javascript">
        var DeviceId = '<%=DeviceId%>';
        $(function () {
            var ChecckHaveBackBtn = '<%=CheckIsHaveBackbtn %>';
            if (ChecckHaveBackBtn == 'False') {
                $('#BtnBack').hide();
                $('div.header').hide();
            }
            $('#BtnBack').click(function (e) {
                e.preventDefault();
                callBlockUI();
                BackClick();
            });
        });
        function BackClick() {
            window.location = '../Watch/TeacherNewsSelectTeacher.aspx?DeviceId=' + DeviceId;
        }
        function callBlockUI() {
            $('body').append('<div id="imgBlockUi" style="display: none;"><img src="../Images/metroloader_white.gif" /></div>');
            $.blockUI({
                css: {
                    border: 'none',
                    padding: '15px',
                    backgroundColor: '#000',
                    '-webkit-border-radius': '10px',
                    '-moz-border-radius': '10px',
                    opacity: .5,
                    color: '#fff',
                    'font-size': '50px',
                    left: ($(window).width() - 300) / 2 + 'px',
                    width: '300px'

                },
                message: $('#imgBlockUi')
            });
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="header">
            <asp:Button ID="BtnBack" CssClass="Forbtn" runat="server" Text="กลับ" Width="20%" />
        </div>
        <div id="MainDiv" style="text-align: center;padding-top:75px;">
            <%--<img id='BtnBack' src="../Images/Arrow back.png" onclick="BackClick();" class="imgBack" />--%>
            <%--<span class="spnhead">ประกาศจากครู</span>--%>
            <div class="DivTeacherDetail" id="DivTeacherDetail" runat="server">
                <div class='DivForImg' id="TeacherImg" runat="server">
                    <%--<img src="../Images/Student.png" style='width: 150px; margin-top: 5px;' />--%>
                </div>
                <div id='DivTeacherInfo' style='float: left; margin-left: 10px;'>
                    <table>
                        <tr>
                            <td style="padding-top: 30px;">
                                <%=TeacherName %>
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-top: 30px;">
                                <%=TeacherCurrentClass%>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <div id="DivTeacherNews">
                <table id="TeacherNewsTable" style="width: 100%" cellspacing="0">
                    <tr>
                        <th class="ForLeft" style="width: 16%">วันที่แจ้ง
                        </th>
                         <% If (MoreThanStudent = True) Then%>
                                <th style="border-left: 1px solid #fff; border-right: 1px solid #fff; width:15%;">ประกาศถึง
                                        <% End If%>
                        </th>
                            
                        <th class="ForRight" style="width: 75%">เนื้อหา
                        </th>
                    </tr>
                    <asp:Repeater ID="Repeater1" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td class="ForLeft">
                                    <span class="spnDate">
                                        <%# Container.DataItem("TimeAgo")%></span>
                                </td>
                              <% If (MoreThanStudent = True) Then%>
                                <td style="border: 1px solid #fff;">
                                    <span class="spnNewsDetail">
                                        <%#Container.DataItem("Student_FirstName")%></span>
                                </td>
                               <% End If%>
                                <td class="ForRight tdRight">
                                    <span class="spnNewsDetail">
                                        <%#Container.DataItem("Description")%></span>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <AlternatingItemTemplate>
                            <tr>
                                <td class="ForLeft" style="background: rgb(161, 232, 242);">
                                    <span class="spnDate">
                                        <%# Container.DataItem("TimeAgo")%></span>
                                </td>
                                
                                <% If (MoreThanStudent = True) Then%>
                                <td style="background: rgb(161, 232, 242);border-left: 1px solid #fff;border-right: 1px solid #fff;">
                                    <span class="spnNewsDetail">
                                        <%#Container.DataItem("Student_FirstName")%></span>
                                </td>
                               <% End If%>
                           
                                <td class="ForRight tdRight" style="background: rgb(161, 232, 242);">
                                    <span class="spnNewsDetail">
                                        <%#Container.DataItem("Description")%></span>
                                </td>
                            </tr>
                        </AlternatingItemTemplate>
                    </asp:Repeater>
                </table>
            </div>
        </div>
        <div class="footer">
            ประกาศจากครู
        </div>
    </form>
</body>
</html>
<script type="text/javascript">
    var ua = navigator.userAgent.toLowerCase();
    var isAndroid = ua.indexOf("android") > -1;
    if (isAndroid) {
        $('.footer').css('min-height', '80px');
    }
    var w = window.innerWidth;
    var h = window.innerHeight;
    var mainDivHeight = document.getElementById("MainDiv").offsetHeight;
    if (mainDivHeight > (h - 80)) {
        $('#MainDiv').css('margin-bottom', '110px');
    }
</script>
