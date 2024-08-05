<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="AddChild.aspx.vb" Inherits="QuickTest.Childs" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        @import url(../fonts/thsarabunnew.css);

        body {
            font: normal 0.95em 'THSarabunNew';
            color: #444;
        }

        div.main {
            width: 90%;
            margin-left: auto;
            margin-right: auto;
            padding: 20px 10px;
            text-align: center;
        }

        div.mainChildren {
            padding: 10px;
        }

        div.children, div.addChild {
            width: 250px;
            height: 300px;
            border: 2px solid #C6E7F0;
            border-radius: 10px;
            padding: 5px;
            margin:10px;
            display:inline-block;
        }

            div.children span {
                font-size: 20px;
            }

        div.imgChild {
            width: 100%;
            height: 230px;
            background-repeat: no-repeat;
            background-size: cover;
        }

        span.head {
            font-size: 30px;
            font-weight: bold;
        }

        div.addChild {
            height:250px;
            background:#00C7FD;
        }

            div.addChild > div {
                background-image: url('../Images/PlusSign.png');
                width: 100%;
                height: 100%;
                background-repeat: no-repeat;
                background-size: 100%;                
            }
    </style>
    <script src="../js/jquery-1.7.1.min.js"></script>
    <script type="text/javascript">
        $(function () {
            $('.addChild').on('click', function () {
                window.location = '../watch/addchild2.aspx?DeviceId=' + '<%=DeviceId%>' + '&Sendpage=watch/addchild.aspx&addChild=true';
            });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="main">
            <span class="head">นักเรียนในความดูแลของฉัน</span>
            <div id="divChilds" runat="server">
                <div class="children">
                    <div class="imgChild">
                    </div>
                    <div><span>ด.ช.มาโนช  พุฒตาล</span></div>
                    <div><span>เลขที่ 5  ป.1/1</span></div>
                </div>
                <div class="addChild">
                </div>
            </div>
        </div>
    </form>
</body>
</html>
