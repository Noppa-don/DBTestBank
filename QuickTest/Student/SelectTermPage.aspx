<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="SelectTermPage.aspx.vb"
    Inherits="QuickTest.SelectTermPage" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0" name="viewport" />
    <script src="../js/jquery-1.7.1.js"></script>
    <script type="text/javascript">

        $(function () {

            //เมื่อคลิกจะส่ง CalendarId ไปที่ function ขอวหน้า Parent เพื่อเปลี่ยน Session
            $('.ForDivTerm').click(function () {
                var CalendarId = $(this).attr('TermId');
                var CalendarName = $(this).html();
                parent.SetSesstionCalendarId(CalendarId, CalendarName);
            });
            ////////////////////////////////////////////////////////////////////////////////////////////////
            $('.ForDivTerm').hover(function () {
                $(this).css('background-color', '#F39D10');
            },
            function () {
                $(this).css('background-color', '#FFFFCC');
            }
            )

        });

    </script>
    <style type="text/css">
        .ForMainDivTerm {
            overflow: auto;
            text-align: center;
        }

        .ForDivTerm {
            width: 550px;
            height: 80px;
            border-radius: 5px;
            margin-left: auto;
            margin-right: auto;
            margin-top: 20px;
            font-size: 40px;
            cursor: pointer;
            line-height: 80px;
            color: #fff;
            text-transform: uppercase;
            text-decoration: none;
            border: 1px solid rgb(235, 235, 235);
            background: #faa51a; /* Old browsers */
            /* IE9 SVG, needs conditional override of 'filter' to 'none' */
            background: url(data:image/svg+xml;base64,PD94bWwgdmVyc2lvbj0iMS4wIiA/Pgo8c3ZnIHhtbG5zPSJodHRwOi8vd3d3LnczLm9yZy8yMDAwL3N2ZyIgd2lkdGg9IjEwMCUiIGhlaWdodD0iMTAwJSIgdmlld0JveD0iMCAwIDEgMSIgcHJlc2VydmVBc3BlY3RSYXRpbz0ibm9uZSI+CiAgPGxpbmVhckdyYWRpZW50IGlkPSJncmFkLXVjZ2ctZ2VuZXJhdGVkIiBncmFkaWVudFVuaXRzPSJ1c2VyU3BhY2VPblVzZSIgeDE9IjAlIiB5MT0iMCUiIHgyPSIwJSIgeTI9IjEwMCUiPgogICAgPHN0b3Agb2Zmc2V0PSIwJSIgc3RvcC1jb2xvcj0iI2ZhYTUxYSIgc3RvcC1vcGFjaXR5PSIxIi8+CiAgICA8c3RvcCBvZmZzZXQ9IjEwMCUiIHN0b3AtY29sb3I9IiNmNDdhMjAiIHN0b3Atb3BhY2l0eT0iMSIvPgogIDwvbGluZWFyR3JhZGllbnQ+CiAgPHJlY3QgeD0iMCIgeT0iMCIgd2lkdGg9IjEiIGhlaWdodD0iMSIgZmlsbD0idXJsKCNncmFkLXVjZ2ctZ2VuZXJhdGVkKSIgLz4KPC9zdmc+);
            background: -moz-linear-gradient(top, #faa51a 0%, #f47a20 100%); /* FF3.6+ */
            background: -webkit-gradient(linear, left top, left bottom, color-stop(0%,#faa51a), color-stop(100%,#f47a20)); /* Chrome,Safari4+ */
            background: -webkit-linear-gradient(top, #faa51a 0%,#f47a20 100%); /* Chrome10+,Safari5.1+ */
            background: -o-linear-gradient(top, #faa51a 0%,#f47a20 100%); /* Opera 11.10+ */
            background: -ms-linear-gradient(top, #faa51a 0%,#f47a20 100%); /* IE10+ */
            background: linear-gradient(to bottom, #faa51a 0%,#f47a20 100%); /* W3C */
            filter: progid:DXImageTransform.Microsoft.gradient( startColorstr='#faa51a', endColorstr='#f47a20',GradientType=0 ); /* IE6-8 */
        }
    </style>
    <!--[if gte IE 9]>
        <style type="text/css">
        .gradient{filter:none;}
        </style>
        <![endif]-->
    <title></title>
</head>
<body>
    <form id="form1">
        <div id="MainDiv" style="background-color: #D3F2F7; padding: 20px 0; color: #222;">
            <div id="MainDivTerm" class="ForMainDivTerm" runat="server">
            </div>
        </div>
    </form>
</body>
</html>
