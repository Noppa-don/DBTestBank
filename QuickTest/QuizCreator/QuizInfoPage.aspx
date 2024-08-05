<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="QuizInfoPage.aspx.vb" Inherits="QuickTest.QuizInfoPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <style type="text/css">

        #MainDiv {
        background:url('Images/Background.png');
        width:755px;
        height:560px;
        background-size:cover;
        margin-left:auto;
        margin-right:auto;
        text-align:center;
        }
        span {
        color:rgb(61, 53, 53);
        }
        .spnHead {
        font-size:40px;
        font-weight:bold;
        }
        table {
        width:100%;
        padding-top:40px;
        }
        td {
        padding:15px;
        }
        .myButton {
	    -moz-box-shadow:inset 0px 1px 0px 0px #a6827e;
	    -webkit-box-shadow:inset 0px 1px 0px 0px #a6827e;
	    box-shadow:inset 0px 1px 0px 0px #a6827e;
	    background:-webkit-gradient(linear, left top, left bottom, color-stop(0.05, #7d5d3b), color-stop(1, #634b30));
	    background:-moz-linear-gradient(top, #7d5d3b 5%, #634b30 100%);
	    background:-webkit-linear-gradient(top, #7d5d3b 5%, #634b30 100%);
	    background:-o-linear-gradient(top, #7d5d3b 5%, #634b30 100%);
	    background:-ms-linear-gradient(top, #7d5d3b 5%, #634b30 100%);
	    background:linear-gradient(to bottom, #7d5d3b 5%, #634b30 100%);
	    filter:progid:DXImageTransform.Microsoft.gradient(startColorstr='#7d5d3b', endColorstr='#634b30',GradientType=0);
	    background-color:#7d5d3b;
	    -moz-border-radius:3px;
	    -webkit-border-radius:3px;
	    border-radius:3px;
	    border:1px solid #54381e;
	    display:inline-block;
	    cursor:pointer;
	    color:#ffffff;
	    font-family:arial;
	    font-size:13px;
	    padding:6px 24px;
	    text-decoration:none;
	    text-shadow:0px 1px 0px #4d3534;
        width:165px;
        height:65px;
        font-size:30px;
        margin-top:10px;
        }
        .myButton:hover {
	    background:-webkit-gradient(linear, left top, left bottom, color-stop(0.05, #634b30), color-stop(1, #7d5d3b));
	    background:-moz-linear-gradient(top, #634b30 5%, #7d5d3b 100%);
	    background:-webkit-linear-gradient(top, #634b30 5%, #7d5d3b 100%);
	    background:-o-linear-gradient(top, #634b30 5%, #7d5d3b 100%);
	    background:-ms-linear-gradient(top, #634b30 5%, #7d5d3b 100%);
	    background:linear-gradient(to bottom, #634b30 5%, #7d5d3b 100%);
	    filter:progid:DXImageTransform.Microsoft.gradient(startColorstr='#634b30', endColorstr='#7d5d3b',GradientType=0);
	    background-color:#634b30;
        }
        .myButton:active {
	    position:relative;
	    top:1px;
        }

    </style>

</head>
<body>
    <form id="form1" runat="server">
    <div id="MainDiv">
        <table>
            <tr>
                <td colspan="2"><span class="spnHead">เข้าทำข้อสอบ <%=TestsetName %></span></td>
            </tr>
            <tr>
                <td colspan="2">
                    <span style="font-size:35px;">ของ <%=TeacherName %></span>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <span style="font-size:35px;">สร้างเมื่อ <%=CreateDate %></span>
                </td>
            </tr>
              <tr>
                <td style="width:285px;text-align:right;">
                    <span style="font-size:35px;">จำนวน <%=TotalQuestion %> ข้อ</span>
                </td>
                  <td>
                      <span style="font-size:35px;">เต็ม <%=FullScore %> คะแนน</span>
                  </td>
            </tr>
            <tr>
                <td>

                </td>
                <td>
                    <span style="font-size:35px;"><%=PassScore %></span>
                </td>
            </tr>
        </table>
        <input class="myButton" type="button" value="ไม่ทำ" style="margin-right:110px;" />
        <input class="myButton" type="button" value="เริ่มทำเลย" />
    </div>
    </form>
</body>
</html>
