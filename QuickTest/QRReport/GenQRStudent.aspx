<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="GenQRStudent.aspx.vb" Inherits="QuickTest.GenQRStudent" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <script src="../js/jquery-1.7.1.min.js"></script>
    <script type="text/javascript">
        var JVStudentId = '<%=StudentId %>';
        $(function () {
            $('.DivCover').click(function () {
                $('#hdSlotSelect').val($(this).attr('slot'));
                $('.DivCover').removeClass('Onselect');
                $('.Forimg').hide();
                $(this).addClass('Onselect');
                $(this).children().show();
            });

            $('#btnPrint').click(function () {
                var slotNumber = $('#hdSlotSelect').val()
                if (slotNumber != '') {
                    //alert(slotNumber);
                    window.location = '<%=ResolveUrl("~")%>QRReport/ProcessQRReport.aspx?StudentId=' + JVStudentId + '&Position=' + slotNumber;
                }
                else {
                    alert('ต้องเลือกสักช่องก่อนค่ะ');
                }
            });
        });
    </script>


    <title></title>

    <style type="text/css">
        #MainDiv {
        width:900px;
        margin-left:auto;
        margin-right:auto;
        text-align:center;
        }
        table {
        width:100%;
        }
        td div {
        border:3px dotted;
        border-radius:6px;
        height:70px;
        border-color:rgb(37, 132, 235);
        font-size:30px;
        line-height:65px;
        }
        td {
        padding:5px;
        width:50%;
        text-align:center;
        }
        .Forimg {
        display:none;
        position:absolute;
        margin-left:20px;
        }
        .onSelect {
        background:rgb(9, 172, 42);
        color:white;
        }
        .DivCover {
        background:none;
        color:black;
        }
        .Onselect {
        background:rgb(46, 129, 253);
        color: white;
        border-color: white;
        }
        #btnPrint,#btnBack {
        font-size:40px;
        margin-top:20px;
        }
    </style>

</head>
<body>
    <form id="form1" runat="server">
    <div id="MainDiv">
        <table>
            <tr>
                <td>
                    <div id="slot1" slot="1" class="DivCover">
                        ดวงที่ 1
                        <img src="../Images/ApproveButton.png" class="Forimg" />
                    </div>
                </td>
                <td>
                    <div id="slot8" slot="8" class="DivCover">
                        ดวงที่ 8
                        <img src="../Images/ApproveButton.png" class="Forimg" />
                    </div>
                </td>
            </tr>

            <tr>
                <td>
                    <div id="slot2" slot="2" class="DivCover">
                        ดวงที่ 2
                        <img src="../Images/ApproveButton.png" class="Forimg" />
                    </div>
                </td>
                <td>
                    <div id="slot9" slot="9" class="DivCover">
                        ดวงที่ 9
                        <img src="../Images/ApproveButton.png" class="Forimg" />
                    </div>
                </td>
            </tr>

            <tr>
                <td>
                    <div id="slot3" slot="3" class="DivCover">
                        ดวงที่ 3
                        <img src="../Images/ApproveButton.png" class="Forimg" />
                    </div>
                </td>
                <td>
                    <div id="slot10" slot="10" class="DivCover">
                        ดวงที่ 10
                        <img src="../Images/ApproveButton.png" class="Forimg" />
                    </div>
                </td>
            </tr>

            <tr>
                <td>
                    <div id="slot4" slot="4" class="DivCover">
                        ดวงที่ 4
                        <img src="../Images/ApproveButton.png" class="Forimg" />
                    </div>
                </td>
                <td>
                    <div id="slot11" slot="11" class="DivCover">
                        ดวงที่ 11
                        <img src="../Images/ApproveButton.png" class="Forimg" />
                    </div>
                </td>
            </tr>

            <tr>
                <td>
                    <div id="slot5" slot="5" class="DivCover">
                        ดวงที่ 5
                        <img src="../Images/ApproveButton.png" class="Forimg" />
                    </div>
                </td>
                <td>
                    <div id="slot12" slot="12" class="DivCover">
                        ดวงที่ 12
                        <img src="../Images/ApproveButton.png" class="Forimg" />
                    </div>
                </td>
            </tr>

            <tr>
                <td>
                    <div id="slot6" slot="6" class="DivCover">
                        ดวงที่ 6
                        <img src="../Images/ApproveButton.png" class="Forimg" />
                    </div>
                </td>
                <td>
                    <div id="slot13" slot="13" class="DivCover">
                        ดวงที่ 13
                        <img src="../Images/ApproveButton.png" class="Forimg" />
                    </div>
                </td>
            </tr>

            <tr>
                <td>
                    <div id="slot7" slot="7" class="DivCover">
                        ดวงที่ 7
                        <img src="../Images/ApproveButton.png" class="Forimg" />
                    </div>
                </td>
                <td>
                    <div id="slot14" slot="14" class="DivCover">
                        ดวงที่ 14
                        <img src="../Images/ApproveButton.png" class="Forimg" />
                    </div>
                </td>
            </tr>
        </table>
        <input type="button" value="กลับ" id="btnBack" />
        <input type="button" value="ตกลง" id="btnPrint" />
        <input type="hidden"  id="hdSlotSelect" value="" />
    </div>
    </form>
</body>
</html>
