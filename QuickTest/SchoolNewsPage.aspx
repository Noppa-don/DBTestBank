<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="SchoolNewsPage.aspx.vb" Inherits="QuickTest.SchoolNewsPage" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0" name="viewport" />
    <script src="js/jquery-1.7.1.min.js"></script>
    <script src="js/jquery-ui-1.8.18.js"></script>
    <script src="js/Animation.js"></script>
    <link href="css/jquery-ui-1.10.1.custom.min.css" rel="stylesheet" />
    <script type="text/javascript">
        var ua = navigator.userAgent.toLowerCase();
        var isAndroid = ua.indexOf("android") > -1;
        $(function () {
            if (isAndroid) {
                if (ua.indexOf("android 4.4.2") > -1) {
                    $('meta[name=viewport]').attr('content', 'width=1200,user-scalable=yes,initial-scale=0.1, maximum-scale=0.1, minimum-scale=0.1');
                } else if (ua.indexOf("android 4.2.1") > -1) {
                    $('meta[name=viewport]').attr('content', 'width=1200,user-scalable=yes,initial-scale=0.1, maximum-scale=0.1, minimum-scale=0.1');
                }
            }
        });
    </script>
    <script type="text/javascript">
        $(function () {
            //hover animation
            InjectionHover('.ForbtnMenu', 3);
            InjectionHover('#btnCraeteNews', 3);


            //ถ้าเป็นนักเรียนจะต้องโชว์ แค่เมนู ข่าวโรงเรียน , ข่าวเก่า
            var CheckIsStudent = '<%=CheckIsStudent%>';
            if (CheckIsStudent == 'true') {
                $('#DivBtnTeacherNewsMenu').hide();
                $('#DivMenu').css('text-align', 'center');
                $('#DivBtnChooseMode').css('width', '670px');
                $('#CurrentNews').css('width', '220px');
                $('#HistoryNews').css({ 'width': '220px', 'border-right': '1px solid' });
                $('#BtnTeacherNews').css('width', '220px');
            }

            var d = new Date();
            var toDay = d.getDate() + '/' + (d.getMonth() + 1) + '/' + (d.getFullYear() + 543);           

            $("#StartDate").datepicker({
                dateFormat: 'dd/mm/yy', isBuddhist: true, defaultDate: toDay, dayNames: ['อาทิตย์', 'จันทร์', 'อังคาร', 'พุธ', 'พฤหัสบดี', 'ศุกร์', 'เสาร์'],
                dayNamesMin: ['อา.', 'จ.', 'อ.', 'พ.', 'พฤ.', 'ศ.', 'ส.'],
                monthNames: ['มกราคม', 'กุมภาพันธ์', 'มีนาคม', 'เมษายน', 'พฤษภาคม', 'มิถุนายน', 'กรกฎาคม', 'สิงหาคม', 'กันยายน', 'ตุลาคม', 'พฤศจิกายน', 'ธันวาคม'],
                monthNamesShort: ['ม.ค.', 'ก.พ.', 'มี.ค.', 'เม.ย.', 'พ.ค.', 'มิ.ย.', 'ก.ค.', 'ส.ค.', 'ก.ย.', 'ต.ค.', 'พ.ย.', 'ธ.ค.']
            });
            $("#EndDate").datepicker({
                dateFormat: 'dd/mm/yy', isBuddhist: true, defaultDate: toDay, dayNames: ['อาทิตย์', 'จันทร์', 'อังคาร', 'พุธ', 'พฤหัสบดี', 'ศุกร์', 'เสาร์'],
                dayNamesMin: ['อา.', 'จ.', 'อ.', 'พ.', 'พฤ.', 'ศ.', 'ส.'],
                monthNames: ['มกราคม', 'กุมภาพันธ์', 'มีนาคม', 'เมษายน', 'พฤษภาคม', 'มิถุนายน', 'กรกฎาคม', 'สิงหาคม', 'กันยายน', 'ตุลาคม', 'พฤศจิกายน', 'ธันวาคม'],
                monthNamesShort: ['ม.ค.', 'ก.พ.', 'มี.ค.', 'เม.ย.', 'พ.ค.', 'มิ.ย.', 'ก.ค.', 'ส.ค.', 'ก.ย.', 'ต.ค.', 'พ.ย.', 'ธ.ค.']
            });


            //$('#StartDate').datepicker();
            //$('#EndDate').datepicker();

            $('.FortdClass').live('click', (function () {
                $('.FortdClass').css('background-color', 'rgb(255, 226, 195)');
                $('.FortdClass').css('color', 'rgb(136, 5, 5)');
                $(this).css('background-color', 'rgb(255, 132, 0)');
                $(this).css('color', 'white');
                GenHtmlRoom($(this).attr('classname'));
                $('#DivStudent').html('');
            }));

            $('.FortdRoom').live('click', (function () {
                $('.FortdRoom').css('background-color', 'rgb(255, 238, 170)');
                $('.FortdRoom').css('color', 'rgb(169, 135, 0)');
                $(this).css('background-color', 'rgb(219, 175, 0)');
                $(this).css('color', 'white');
                $('#HDClassRommName').val($(this).attr('roomname'));
                $('#HDStudentId').val('');
                var StrRoomName = $(this).attr('roomname');
                var spliteStr = StrRoomName.split('/');
                var CurrentClassName = spliteStr[0];
                var CurrentRoomName = '/' + spliteStr[1];
                GenHtmlStudent(CurrentClassName, CurrentRoomName)
            }));

            $('.FortdStudent').live('click', (function () {
                $('.FortdStudent').css('background-color', 'rgb(255, 194, 241)');
                $('.FortdStudent').css('color', 'rgb(190, 6, 150)');
                $(this).css('background-color', 'rgb(255, 27, 203)');
                $(this).css('color', 'white');
                $('#HDClassRoomName').val('');
                $('#HDStudentId').val($(this).attr('stuId'));
            }));

            $('#btnCraeteNews').click(function () {
                var StartDate = $('#StartDate').val();
                var EndDate = $('#EndDate').val();
                if (ValidateBeforeInsertOrUpdate(StartDate, EndDate) != false) {
                    var txtNews = $('#CreateNewstxt').val();
                    txtNews = txtNews.replace(/'/g, "");
                    if ($('#HDStudentId').val() != '') {
                        InsertTeacherNewsStudent(StartDate, EndDate, $('#HDStudentId').val(), txtNews);
                    }
                    else {
                        var CurrentClassRoomName = $('#HDClassRommName').val();
                        var SpliteStr = CurrentClassRoomName.split('/');
                        var CurrentClassName = SpliteStr[0];
                        var CurrentRoomName = '/' + SpliteStr[1];
                        InsertTeacherNewsRoom(StartDate, EndDate, CurrentClassName, CurrentRoomName, txtNews, StartDate, EndDate);
                    }
                }
            });
        });

        function ValidateBeforeInsertOrUpdate(InputStartDate, InputEndDate) {
            //ตรวจวันที่ก่อน StartDate ต้อง > EndDate
            if (ValidateDate(InputStartDate, InputEndDate) != false) {
                //ใน txtbox ต้องมีตัวอักษรห้ามเป็นค่าว่าง
                if ($('#CreateNewstxt').val() != '') {
                    //ต้องบังคับให้เลือกห้องเป็นอย่างน้อย
                    if ($('#HDClassRommName').val() != '') {
                        return true
                    }
                    else {
                        alert('ต้องเลือกห้องหรือนักเรียนก่อนค่ะ');
                        return false;
                    }
                }
                else {
                    alert('ข่าวห้ามเป็นค่าว่างค่ะ');
                    return false;
                }
            }
            else {
                alert('เลือกรูปแบบวันที่ไม่ถูกต้องค่ะ');
                return false;
            }
        }

        function ValidateDate(StartDate, EndDate) {
            var SD = StartDate.split('/');
            var ED = EndDate.split('/');
            var SplitSD = new Date(SD[2], SD[0] - 1, SD[1]);
            var SplitED = new Date(ED[2], ED[0] - 1, ED[1]);
            if (SplitSD > SplitED) {
                return false;
            }
        }

        //Gen ห้อง
        function GenHtmlRoom(className) {
            $.ajax({
                type: "POST",
                url: "<%=ResolveUrl("~")%>SchoolNewsPage.aspx/GenHtmlRoomPanel",
                contentType: "application/json; charset=utf-8", dataType: "json",
                data: "{ ClassName: '" + className + "'}",
                success: function (msg) {
                    if (msg.d != '') {
                        $('#DivRoom').text('');
                        $('#DivRoom').append(msg.d);
                    }
                },
                error: function myfunction(request, status) {
                    alert('บางอย่างผิดพลาดโปรดลองอีกครั้ง')
                }
            });
        }

        //Gen นักเรียน
        function GenHtmlStudent(className, RoomName) {
            $.ajax({
                type: "POST",
                url: "<%=ResolveUrl("~")%>SchoolNewsPage.aspx/GenHtmlStudentPanel",
                contentType: "application/json; charset=utf-8", dataType: "json",
                data: "{ ClassName: '" + className + "',RoomName:'" + RoomName + "'}",
                success: function (msg) {
                    if (msg.d != '') {
                        $('#DivStudent').text('');
                        $('#DivStudent').append(msg.d);
                    }
                },
                error: function myfunction(request, status) {
                    alert('บางอย่างผิดพลาดโปรดลองอีกครั้ง')
                }
            });
        }

        //Save ข่าว
        function InsertTeacherNewsRoom(StartDate, EndDate, ClassName, RoomName, txtNews) {
            $.ajax({
                type: "POST",
                url: "<%=ResolveUrl("~")%>SchoolNewsPage.aspx/SaveNewsRoom",
                contentType: "application/json; charset=utf-8", dataType: "json",
                data: "{ ClassName: '" + ClassName + "',RoomName:'" + RoomName + "',txtNews:'" + txtNews + "',StartDate:'" + StartDate + "',EndDate:'" + EndDate + "'}",
                success: function (msg) {
                    if (msg.d == 'Complete') {
                        window.location = '<%=ResolveUrl("~")%>SchoolNewsPage.aspx?AfterInsert=1';
                    }
                },
                error: function myfunction(request, status) {
                    alert('บางอย่างผิดพลาดโปรดลองอีกครั้ง')
                }
            });
        }

        function InsertTeacherNewsStudent(StartDate, EndDate, StudentId, txtNews) {
            $.ajax({
                type: "POST",
                url: "<%=ResolveUrl("~")%>SchoolNewsPage.aspx/SaveNewsStudent",
                contentType: "application/json; charset=utf-8", dataType: "json",
                data: "{ StudentId: '" + StudentId + "',txtNews:'" + txtNews + "',StartDate:'" + StartDate + "',EndDate:'" + EndDate + "'}",
                success: function (msg) {
                    if (msg.d == 'Complete') {
                        window.location = '<%=ResolveUrl("~")%>SchoolNewsPage.aspx?AfterInsert=1';
                    }
                },
                error: function myfunction(request, status) {
                    alert('บางอย่างผิดพลาดโปรดลองอีกครั้ง')
                }
            });
        }

    </script>

    <style type="text/css">
        body {
            background-color: rgb(252, 236, 209);
        }

        .ForDivBtn, .ForDivBtnBottom {
            width: 720px;
            margin: auto;
            border: solid 1px #DA7C0C;
            border-radius: .5em;
            text-align: center;
            display: table;
            background: #F78D1D;
            background: -webkit-gradient(linear, left top, left bottom, from(#FAA51A), to(#F47A20));
            background: -moz-linear-gradient(top, #FAA51A, #F47A20);
        }

        .ForDivMenutop {
            display: inline-block;
            width: 320px;
            border: solid 1px #DA7C0C;
            border-radius: .5em;
            text-align: center;
            background: #F78D1D;
            background: -webkit-gradient(linear, left top, left bottom, from(#FAA51A), to(#F47A20));
            background: -moz-linear-gradient(top, #FAA51A, #F47A20);
        }

        .ForDivBtnBottom {
            border: initial;
            background: initial;
        }

            .ForDivBtnBottom #divBtn {
                border: 1px solid #FFA032;
                border-radius: .5em;
                margin-top: initial !important;
                text-align: center !important;
            }

        #MainDiv {
            width: 900px;
            margin-left: auto;
            margin-right: auto;
            background-color: rgb(252, 236, 209);
            height: 370px;
            padding: 20px;
            border-radius: 6px;
        }

        .ForbtnMenu {
            font: 100% 'THSarabunNew';
            font-size: 20px;
            width: 155px;
            height: 45px;
            line-height: 45px;
            display: inline-block;
            text-align: center;
            cursor: pointer;
            color: #fff;
            border: 0;
            background: transparent;
            text-transform: uppercase;
            text-decoration: none;
            border-right: 1px solid;
        }

        table {
            width: 100%;
            margin-left: auto;
            margin-right: auto;
            font-size: 18px;
            /*border: 1px solid;*/
            border-radius: 6px;
            font: normal 100% THSarabunNew;
        }

        .ForDivContent {
            margin-top: 25px;
            height: 330px;
            overflow: auto;
        }

        th {
            border-bottom: 1px solid;
            background-color: #FF9F0F;
            color: white;
            font-size: 22px;
        }

        .ForborderLeft {
            border-left: 1px solid;
            border-color: white;
        }

        td {
            padding: 15px 20px 15px 20px;
            background-color: white;
        }

        .ForImgEdit {
            margin-left: 5px;
            margin-right: 5px;
        }

        #MainCreateNewsDetail {
            margin-top: 15px;
        }

        .FortdDate {
            background-color: rgb(255, 126, 126);
            padding: 8px;
            font-size: 16px;
            color: white;
        }

        .FortdClass {
            background-color: rgb(255, 226, 195);
            padding: 10px;
            color: rgb(136, 5, 5);
            font-size: 30px;
        }

        .FortdRoom {
            background-color: rgb(255, 238, 170);
            padding: 10px;
            color: rgb(169, 135, 0);
            font-size: 30px;
        }

        .FortdStudent {
            background-color: rgb(255, 194, 241);
            padding: 10px;
            color: rgb(190, 6, 150);
            font-size: 20px;
        }

        .ForEachTopDivCreateNews {
            width: 153px;
            height: 175px;
            display: inline-block;
            padding: 5px 5px 0px 5px;
            border-radius: 6px;
        }

        #DivCreateNewsContent {
            margin-top: 17px;
            text-align: center;
        }

        .NewIcon {
            width: 35px;
            height: 25px;
            margin-left: -15px;
            padding-right: 5px;
        }
    </style>
    <%If Not IsAllowNewsPost Then%>
    <style type="text/css">
        input[name='btnOurNews']{
            border-right:0!important;
        }
    </style>
    <%End If %>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
        </telerik:RadScriptManager>
        <div id="MainDiv">
            <div id="DivMenu" style="text-align: center">
                <div id='DivBtnChooseMode' class='ForDivMenutop'>
                    <asp:Button ID="CurrentNews" runat="server" Text="ข่าวโรงเรียน" CssClass='ForbtnMenu' />
                    <asp:Button ID="HistoryNews" runat="server" Text="ข่าวเก่า" CssClass='ForbtnMenu' Style="border-right: 0;" />
                    <%If CheckIsStudent = "true" Then%>
                    <asp:Button ID="BtnTeacherNews" runat="server" Text="ข่าวจากครู" CssClass='ForbtnMenu' Style="border-right: 0;" />
                    <%End If%>
                </div>

                <div id='DivBtnTeacherNewsMenu' style="margin-left: 20px;" class='ForDivMenutop'>
                    <asp:Button ID="btnOurNews" runat="server" Text="ข่าวที่ประกาศเอง" CssClass='ForbtnMenu' />
                    <%If IsAllowNewsPost Then %>
                    <asp:Button ID="btnCreateNews" runat="server" Text="ประกาศข่าว" CssClass='ForbtnMenu' Style="border-right: 0;" />
                    <%End If %>
                </div>
            </div>

            <div id="DivNewsDetail" runat="server" class="ForDivContent" style="width: 775px; margin-left:auto;margin-right:auto;">
                <table>
                    <tr>

                        <th style="border-top-left-radius: 6px; width: 15%; height: 50px;" class="ForborderLeft">ผู้ประกาศ</th>
                        <th style="height: 50px; width: 18%;" class="ForborderLeft">ประกาศช่วง</th>
                        <th style="border-top-right-radius: 6px; width: 39%; height: 50px;">ข่าว</th>
                    </tr>
                    <asp:Repeater ID="Repeater1" runat="server">
                        <ItemTemplate>
                            <tr>

                                <td class="ForborderLeft">
                                    <%--อทิวรา พวงมาลัย--%>
                                    <%# Container.DataItem("News_Announcer")%></span>
                                </td>
                                <td class="ForborderLeft">
                                    <%--15/11--%>
                                    <%# Container.DataItem("DateNews")%></span>
                                </td>
                                <td>
                                    <%--เตรียมตัววันไหว้ครู ขอให้นักเรียนทุกคนนำดอกไม้มาไวห้ครูด้วยค่ะ--%>
                                    <%# Container.DataItem("News_Information")%></span>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </table>
            </div>

            <div id="DivOurNewsDetail" runat="server" style="display: none;" class="ForDivContent">
                <table>
                    <tr>
                        <th style="border-top-left-radius: 6px; width: 80%; height: 50px;">ข่าว</th>
                        <th style="border-top-right-radius: 6px; height: 50px; width: 20%;" class="ForborderLeft">
                            <img src="Images/ApproveButton.png" /></th>
                    </tr>
                    <asp:Repeater ID="Repeater2" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <div style="font-size: 20px;">
                                        <%# Container.DataItem("Description") %>
                                    </div>
                                    <div style="font-size: 17px; margin-top: 15px;">
                                        <span><%# Container.DataItem("DurationDateTime").ToString().Replace("00:00:00","") %></span>
                                        <span><%# Container.DataItem("NoticeTo") %></span>
                                    </div>
                                </td>

                                <td>
                                    <%If IsAllowNewsPost Then %>
                                    <asp:ImageButton ID="ImgEdit" ImageUrl="~/Images/freehand.png" CommandName="ImgEditClick" CommandArgument='<%#Container.DataItem("TN_Id")%>' CssClass="ForImgEdit" runat="server" />
                                    <asp:ImageButton ID="ImgDelete" ImageUrl="~/Images/Delete-icon.png" CommandName="DeleteOurNews" CommandArgument='<%#Container.DataItem("TN_Id")%>' OnClientClick="return confirm('ต้องการลบข่าวนี้หรอค่ะ ?');" CssClass="ForImgEdit" runat="server" />
                                    <%End If %>
                                </td>

                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </table>
            </div>

            <div id="DivCreateNews" runat="server" style="display: none; text-align: center;">
                <div id="MainCreateNewsDetail">

                    <div id="DivDate" class="ForEachTopDivCreateNews" style="background-color: rgb(187, 28, 28);">
                        <div style="height: 170px; overflow: auto;">
                            <table style="text-align: center; height: 170px; overflow: auto;">
                                <tr>
                                    <td class="FortdDate">ประกาศตั้งแต่</td>
                                </tr>
                                <tr>
                                    <td class="FortdDate">
                                        <%--<telerik:RadDatePicker Width="120" runat="server" ID="StartDate">
                                    </telerik:RadDatePicker>--%>
                                        <input type="text" runat="server" id="StartDate" style="width: 90px;" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="FortdDate">ถึงวันที่</td>
                                </tr>
                                <tr>
                                    <td class="FortdDate">
                                        <%--  <telerik:RadDatePicker Width="120" runat="server" ID="EndDate">
                                    </telerik:RadDatePicker>--%>
                                        <input type="text" runat="server" id="EndDate" style="width: 90px;" />
                                    </td>
                                </tr>
                            </table>
                        </div>

                    </div>

                    <div id="DivClass" runat="server" style="background-color: rgb(255, 181, 103);" class="ForEachTopDivCreateNews">
                        <%--<div style="height:170px;overflow:auto;">
                    <table id="tableClass" style="text-align:center;">
                        <tr>
                            <td classname="ป.1" class="FortdClass">
                                ป.1
                            </td>
                        </tr>
                        <tr>
                            <td classname="ป.1" class="FortdClass">
                                ป.2
                            </td>
                        </tr>
                        <tr>
                            <td classname="ป.1" class="FortdClass" style="background-color:rgb(255, 132, 0);color:white;">
                                ป.3
                            </td>
                        </tr>
                        <tr>
                            <td classname="ป.1" class="FortdClass">
                                ป.4
                            </td>
                        </tr>
                         <tr>
                            <td class="FortdClass">
                                ป.6
                            </td>
                        </tr>
                         <tr>
                            <td class="FortdClass">
                                ม.1
                            </td>
                        </tr>
                    </table>
                        </div>--%>
                    </div>

                    <div id="DivRoom" runat="server" class="ForEachTopDivCreateNews" style="background-color: rgb(255, 223, 94);">
                        <%-- <div style="height:170px;overflow:auto;">
                        <table id="tableRoom" style="text-align:center;">
                            <tr>
                                <td roomname="ป.3/1" class="FortdRoom">
                                    ป.3/1
                                </td>
                            </tr>
                            <tr>
                                <td roomname="ป.3/1" class="FortdRoom">
                                    ป.3/2
                                </td>
                            </tr>
                            <tr>
                                <td roomname="ป.3/1" class="FortdRoom">
                                    ป.3/3
                                </td>
                            </tr>
                            <tr>
                                <td roomname="ป.3/1" class="FortdRoom">
                                    ป.3/4
                                </td>
                            </tr>
                        </table>
                    </div>--%>
                    </div>

                    <div id="DivStudent" runat="server" class="ForEachTopDivCreateNews" style="background-color: rgb(255, 157, 233);">
                        <%-- <div style="height:170px;overflow:auto;">
                        <table id="tableStudent" style="text-align:center;">
                            <tr>
                                <td stuId="123" class="FortdStudent">
                                    สมรัก พรรคเพื่อเก้ง
                                </td>
                            </tr>
                            <tr>
                                <td stuId="123" class="FortdStudent">
                                    กังนัม สไตล์
                                </td>
                            </tr>
                            <tr>
                                <td stuId="123" class="FortdStudent">
                                    โดดเดี่ยว ผู้น่ารัก
                                </td>
                            </tr>
                            <tr>
                                <td stuId="123" class="FortdStudent">
                                    บี้ เดอะสกา
                                </td>
                            </tr>
                        </table>
                    </div>--%>
                    </div>

                </div>

                <div id="DivCreateNewsContent">
                    <div>
                        <textarea id="CreateNewstxt" runat="server" style="width: 660px; height: 120px; font-size: 20px;"></textarea>
                    </div>
                    <div>
                        <input type="button" id="btnCraeteNews" class="ForDivMenutop" style="width: 110px; margin-top: 10px; padding: 5px; color: white; font-size: 25px;" value="ตกลง" />
                    </div>
                </div>

            </div>

        </div>
        <input type="hidden" id="HDClassRommName" value="" runat="server" />
        <input type="hidden" id="HDStudentId" value="" runat="server" />
    </form>

</body>
</html>
