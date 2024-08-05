<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="SchoolNews.aspx.vb" Inherits="QuickTest.SchoolNews" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0" name="viewport" />
    <script src="../js/jquery-1.7.1.js"></script>
    <title></title>
    <style type="text/css">
        #MainDiv
        {
            /*background-color: rgb(255, 216, 246);
            border-radius: 5px;*/
            text-align: center;
            padding: 10px 0 70px 0;
            width: 100%;
        }
        #DivSchoolNews
        {
            width: 90%;
            margin: 10px auto;
            background-color: #5ECBFF;
            border-radius: 5px;
            height: auto;
        }
        th
        {
            font-size: 30px;
            padding: 10px;
            background: rgb(0, 153, 255);
        }
        td
        {
            padding: 5px;
        }
        .tdRight
        {
            text-align: left;
        }
        th.ForLeft
        {
            border-radius: 5px 0 0 0;
        }
        th.ForRight
        {
            border-radius: 0 5px 0 0;
        }
        .ForLeft
        {
            border-bottom: 1px solid;
            border-color: white;
        }
        .ForRight
        {
            border-left: 1px solid;
            border-bottom: 1px solid;
            border-color: white;
        }
        td.ForRight
        {
            padding-left: 10px;
        }
        /*Span*/
        .spnHeadNews
        {
            font-size: 30px;
            font-weight: bold;
            position: relative;
            top: 20px;
        }
        .spnDate
        {
            font-size: 20px;
        }
        .spnTeacherAn
        {
            font-size: 16px;
        }
        .spnNewsDetail
        {
            font-size: 25px;
        }
        #SchoolNewsTable
        {
            border: 1px solid #cdcdcd;
            border-radius: 5px;
        }
    </style>
    <link href="../css/StyleMobile.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div id="MainDiv">
        <%-- <span class="spnHeadNews">ข่าวสารจากโรงเรียน</span>--%>
        <div id="DivSchoolNews">
            <table id="SchoolNewsTable" style="width: 100%;" cellspacing="0">
                <tr>
                    <th class="ForLeft" style="width: 25%">
                        วันที่
                    </th>
                    <th class="ForRight" style="width: 75%">
                        เนื้อหา
                    </th>
                </tr>
                <asp:Repeater ID="Repeater1" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td class="ForLeft">
                                <div>
                                    <span class="spnDate">
                                        <%# Container.DataItem("TimeAgo")%></span>
                                    <br />
                                    <span class="spnTeacherAn">
                                        <%#Container.DataItem("News_Announcer")%></span>
                                </div>
                            </td>
                            <td class="ForRight tdRight">
                                <span class="spnNewsDetail">
                                    <%#Container.DataItem("News_Information")%></span>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                        <tr>
                            <td class="ForLeft" style="background: rgb(161, 232, 242);">
                                <div>
                                    <span class="spnDate">
                                        <%# Container.DataItem("TimeAgo")%></span>
                                    <br />
                                    <span class="spnTeacherAn">
                                        <%#Container.DataItem("News_Announcer")%></span>
                                </div>
                            </td>
                            <td class="ForRight tdRight" style="background: rgb(161, 232, 242);">
                                <span class="spnNewsDetail">
                                    <%#Container.DataItem("News_Information")%></span>
                            </td>
                        </tr>
                    </AlternatingItemTemplate>
                </asp:Repeater>
            </table>
        </div>
    </div>
    <div class="footer">
        ข่าวสารจากโรงเรียน
    </div>
    </form>
</body>
</html>
<script type="text/javascript">
    var ua = navigator.userAgent.toLowerCase();
    var isAndroid = ua.indexOf("android") > -1;
    if (isAndroid) {
        $('.footer').css('min-height', '80px');
        $('#MainContent').css({ 'padding-bottom': '10px;', 'padding-top': '75px;' });
    }
    var w = window.innerWidth;
    var h = window.innerHeight;
    var mainDivHeight = document.getElementById("DivSchoolNews").offsetHeight;
    if (mainDivHeight > (h - 80)) {
        $('#MainDiv').css('margin-bottom', '40px');
    }

    $(function () {
        $('html').click(function () {
            window.location = window.location.href;
        });
    });
</script>