<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Info.aspx.vb" Inherits="QuickTest.Info" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>คลังข้อสอบออนไลน์ by วัฒนาพานิช</title>
    <meta name="description" content="website description" />
    <meta name="keywords" content="website keywords, website keywords" />
    <script src="js/jquery-1.7.1.js" type="text/javascript"></script>
    <link rel="stylesheet" href="./css/reset.css" type="text/css" />
    <link rel="stylesheet" type="text/css" href="./css/style.css" />
    <link rel="stylesheet" type="text/css" href="./css/iframestyle.css" />
    <link rel="stylesheet" type="text/css" href="./css/contactstyle.css" />
    <link href="css/menuFixReviewAns.css" rel="stylesheet" type="text/css" />
    <!--[if lt IE 9]>
        <script src="html5.js"></script>
    <![endif]-->
    <script type="text/javascript" src="./js/modernizr-1.5.min.js"></script>
    <link rel="stylesheet" type="text/css" href="./shadowbox/shadowbox.css">
    <link href="css/fixMenuSlide.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="./shadowbox/shadowbox.js"></script>
    <style type="text/css">
        table td {
            padding: 0px !important;
            font-size: 40px;
            text-align: center !important;
            border-radius: 6px;
        }

        #DivInternetStatus {
            width: 50px;
            height: 50px;
            border-radius: 25px;
        }

        .Forbtn {
            position: relative;
            width: 150px;
            height: 55px;
            font-size: 30px;
        }
        table tr th, table tr td {
            text-align: center;
                font-size: 25px;
                border: 1px solid #FFF;
        }
        table tr td
        {
                font-size: 22px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div id="main">
            <header>
                <div id="logo" style="padding-top: 5px">
                    <div id="logo_text">
                        <img class="imgLogo" style="width: 224px; vertical-align: middle; margin: 0 50px 0 350px;" src="./images/logoQ.png" alt="logo" />
                    </div>
                </div>
            </header>
            <!--[if lte IE 7]><br /><br /><br /><![endif]-->
            <div id="site_content" style="padding: 0px; position: relative; top: -40px;">
                <div class="content" style="margin-left: 50px; padding: 0px; margin-bottom: 0px">
                    <div class="form_settings" style='width: 830px;'>
                        <table >
                            <tr>
                                <td colspan="2">
                                    <asp:Label ID="lblFingerPrint" runat="server" Font-Size="25px" Style="float: none; font-weight: bold;" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <span style="font-size: 25px; float: none; display: inline-block; font-weight: bold;">Internet Status : </span>
                                    <div id="DivInternetStatus" style="display: inline-block;" runat="server"></div>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 50%;">
                                    <span style="font-size: 25px; float: none; font-weight: bold;">เบอร์โทร:086-302-6355</span>
                                </td>
                                <td>
                                    <span style="font-size: 25px; float: none; font-weight: bold;">Email:support@iknow.co.th</span>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <span style="font-size: 25px; float: none; font-weight: bold;">คำอธิบาย</span><br />
                                    <span style="width: auto; font-size: large; margin-left: 50px;">เมื่อเครื่องมีปัญหาหรือต้องการให้เจ้าหน้าที่ดูแลให้ ให้ต่อ Internet(Internet Status ขึ้นวงกลมสีเขียว)<br />
                                        แล้วโทรไปที่เบอร์ที่แสดงอยู่ได้เลย</span>


                                </td>
                            </tr>                          
                            <tr>
                                <td>
                                    <%--<asp:Button ID="btnOpenTeamViewer" CssClass="submit" style="position:relative;width:200px;height:55px;font-size:20px;" runat="server" Text="Activate TeamViewer" />--%>
                                    <object id="btnOpenTeamViewer" style="display: none; position: relative; top: 15px;" width="85" height="80" classid="CLSID:E1A0CD79-7850-4259-936F-386F0496A6B1">
                                        <param name="ProgramPath" value="C:\app\quicktest\TeamViewer.exe">
                                    </object>
                                </td>
                                <td>
                                    <asp:Button ID="btnSendEmail" CssClass="submit" Style="position: relative; width: 200px; height: 55px; font-size: 20px;    margin-top: -50px!important;" runat="server" Text="ส่งข้อมูลให้เจ้าหน้าที่" />
                                </td>
                            </tr>
                              <tr>
                                <td colspan="2">
                                    <span style="font-size: 25px; float: none; font-weight: bold;">จำนวนข้อสอบ</span><br />
                                    <asp:Repeater ID="rptQuestionWppAmount" runat="server">
                                        <HeaderTemplate>
                                            <table>
                                                <thead>
                                                    <tr>
                                                        <th>ข้อสอบมาตรฐาน</th>
                                                        <th>ชั้น</th>
                                                        <th>วิชา</th>
                                                        <th>จำนวน</th>
                                                    </tr>
                                                </thead>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td>ข้อสอบมาตรฐาน</td>
                                                <td><%# Container.DataItem("Level_ShortName")%></td>
                                                <td><%# Container.DataItem("GroupSubject_ShortName")%></td>
                                                <td><%# Container.DataItem("QuestionAmountTotal")%></td>
                                            </tr>
                                        </ItemTemplate>
                                        <AlternatingItemTemplate>
                                            <tr>
                                                <td>ข้อสอบมาตรฐาน</td>
                                                <td><%# Container.DataItem("Level_ShortName")%></td>
                                                <td><%# Container.DataItem("GroupSubject_ShortName")%></td>
                                                <td><%# Container.DataItem("QuestionAmountTotal")%></td>
                                            </tr>
                                        </AlternatingItemTemplate>
                                        <FooterTemplate>
                                            </table>
                                        </FooterTemplate>
                                    </asp:Repeater>
                                    <asp:Repeater ID="rptQuestionAmount" runat="server">
                                        <HeaderTemplate>
                                            <table>
                                                <thead>
                                                    <tr>
                                                        <th>ข้อสอบที่เพิ่มเอง</th>
                                                        <th>ชั้น</th>
                                                        <th>วิชา</th>
                                                        <th>จำนวน</th>
                                                    </tr>
                                                </thead>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td>ข้อสอบที่เพิ่มเอง</td>
                                                <td><%# Container.DataItem("Level_ShortName")%></td>
                                                <td><%# Container.DataItem("GroupSubject_ShortName")%></td>
                                                <td><%# Container.DataItem("QuestionAmountTotal")%></td>
                                            </tr>
                                        </ItemTemplate>
                                        <AlternatingItemTemplate>
                                            <tr>
                                                <td>ข้อสอบที่เพิ่มเอง</td>
                                                <td><%# Container.DataItem("Level_ShortName")%></td>
                                                <td><%# Container.DataItem("GroupSubject_ShortName")%></td>
                                                <td><%# Container.DataItem("QuestionAmountTotal")%></td>
                                            </tr>
                                        </AlternatingItemTemplate>
                                        <FooterTemplate>
                                            </table>
                                        </FooterTemplate>
                                    </asp:Repeater>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>
            <footer>
                <a href="http://www.wpp.co.th">สงวนลิขสิทธิ์ &copy; บริษัท สำนักพิมพ์วัฒนาพานิช จำกัด</a>
            </footer>
        </div>
    </form>
    <script type="text/javascript">
        var JVCheckVisibleTVW = '<%= InternetOK%>';
        $(function () {
            if (JVCheckVisibleTVW == 'true') {
                $('#btnOpenTeamViewer').show();
            }
        });
    </script>
</body>
</html>
