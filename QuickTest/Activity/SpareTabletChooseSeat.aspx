<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="SpareTabletChooseSeat.aspx.vb"
    Inherits="QuickTest.SpareTabletChooseSeat" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0" name="viewport" />
    <script src="../js/jquery-1.7.1.js" type="text/javascript"></script>
    <script src="../js/jquery-ui-1.8.18.js" type="text/javascript"></script>
    <link href="../css/jquery-ui-1.8.18.custom.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        $(document).ready(function () {

            $('.divSeat,.divSeatActive').live().click(function () {
            
                $('.divSeat,.divSeatActive').live().each(function () {
                    $(this).removeClass('SeatSelected');
                });

                $(this).addClass('SeatSelected');

                var numberStudent = $(this).html();
                var playerId = $(this).attr('id');
                var valKeep = numberStudent + '|' + playerId;
                $('#hdKeepPlayerId').val(valKeep);
            });

            $('#dialog').dialog({
                autoOpen: false,
                buttons: { 'ใช่': function () {updateChangeTablet(); }, 'ไม่': function () {                    
                    $(this).dialog('close');
                }
                },
                draggable: false,
                resizable: false,
                modal: true
            });

            $('#btnConfirm').click(function (e) {
                e.preventDefault();
                var player = getDialog();

                var title = 'ต้องการเปลี่ยนเครื่องของเลขที่  ' + player[0] + '  มาเป็นเครื่องสำรอง ?';
                $('#dialog').dialog('option', 'title', title).dialog('open'); 
            });

//            var refreshStatus = setInterval(function(){
//                refreshStatusTablet();
//            },10000);
        });
        function getDialog() {
            var valKeep = $('#hdKeepPlayerId').val();
            valKeep = valKeep.split('|');
            return valKeep;
        }
        function updateChangeTablet() {
            var player = getDialog();
            $.ajax({ type: "POST",
                url: "<%=ResolveUrl("~")%>Activity/SpareTabletChooseSeat.aspx/updateChangeTabletInQuiz",
                data:"{playerId : '" + player[1] + "' }",
                async: false,                              
                contentType: "application/json; charset=utf-8", dataType: "json",
                success: function (data) {
                    if (data.d == 0) {
                        alert('session userid is nothing');
                    }
                    //alert(data.d);                                       
                },
                error: function myfunction(request, status) {   
                    alert(status);                      
                }
            });
        }
        function refreshStatusTablet() {            
            $.ajax({ type: "POST",
                url: "<%=ResolveUrl("~")%>Activity/SpareTabletChooseSeat.aspx/refreshStatusConnectionTablet",                
                async: false,                              
                contentType: "application/json; charset=utf-8", dataType: "json",
                success: function (data) {//session(UserId) หายจะ return 0 - Sefety function
                    if (data.d != 0) {
                        $('#divSeatForChoose').html(data.d);
                    }
                },
                error: function myfunction(request, status) {   
                    alert(status);                      
                }
            });
        }
    </script>
    <style type="text/css">
        .ForText
        {
            font-size: 30px;
            margin-left: auto;
            margin-right: auto;
            padding-top: 20px;
        }
        .ForTd
        {
            padding-top: 10px;
            padding-left: 15px;
            width: 485px;
        }
        .ForTdClass
        {
            padding-top: 10px;
            padding-left: 15px;
            width: 485px;
            border-right: 1px solid;
        }
        .ForHover
        {
            background-color: Red;
        }
        .divSeat, .divSeatActive
        {
            width: 50px;
            height: 50px;
            margin: 10px;
            float: left;
            vertical-align: middle;
            text-align: center;
            display: table-cell;
            border: 1px solid;
            line-height: 50px;
            cursor: pointer;
        }
        .divSeat
        {
            background-color: Gray;
        }
        .divSeatActive
        {
            background-color: Green;
        }
        .divSeat:hover, .divSeatActive:hover
        {
            background-color: Red;
        }
        #dialog
        {
            width: 315px;
        }
        .SeatSelected
        {
            background-color: Yellow;
        }
        #btnBack
        {
            position: relative;
            right: 200px;
            width:100px;height:50px;
            background-size: 100px 50px;
        }
        #btnConfirm
        {
            position: relative;
            left: 200px;
            width:100px;height:50px;
            bottom: -15px;
        }
        .active
        {
            background-color: Green;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <asp:HiddenField ID="hdKeepPlayerId" Value="" runat="server" />
    <div id='MainDiv' style='width: 800px;' class='ForText'>
        <center>
            <asp:Label ID="lblTestName" runat="server" Text=""></asp:Label></center>
        <center>
            <div style="border: 1px solid; padding-top: 20px; padding-bottom: 20px;">
                <asp:Label ID="lblClassRoom" runat="server" Text=""></asp:Label>
                <div id='divSeatForChoose' style='height: auto;' runat="server">
                </div>
                <input type="button" id="btnBack" value="" style="background-image:url('../images/upgradeClass/back.png');background-repeat:no-repeat;"/>
                <asp:Button ID="btnConfirm" runat="server" Text="ตกลง" /></div>
        </center>
    </div>
    <div id="dialog" title="">
    </div>
    </form>
</body>
</html>
