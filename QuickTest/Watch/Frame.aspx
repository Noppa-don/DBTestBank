<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Frame.aspx.vb" Inherits="QuickTest.Frame1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0" name="viewport" />
    <style type="text/css">
        iframe
        {
            background-color: #FFF;
            border: 0!important;
        }
        body
        {
            margin: 0!important;
            padding: 0!important;
            border: 0!important;
        }
    </style>
    <script src="../js/jquery-1.7.1.js" type="text/javascript"></script>
    <script type="text/javascript">

        var w, h;
        $(function () {
            h = $(window).height();
            // + ชดเชย iframe ไม่เต็มจอ
            //h += 17;
            //alert(h);
            //$("#FrameMain").height($(window).height());          
            //$("#FrameMain").height(h);
            //$("#FrameMain").css("border", "0px");

            //            // - height head
            //            h -= 65;
            //            // - height footer
            //            h -= 41;
            //            // - padding img graph
            //            h -= 10;

            //            w = $(window).width();
            //            w -= 25;

           // callPage(getParameterByName());
            callPage(getPageByName(), getDeviceId());
        });

        //function callPage(page, w, h) {
        function callPage(page, devid) {
            document.getElementById("FrameMain").src = "../Watch/" + page + ".aspx" + "?DeviceId=" + devid;
        }

        function getPageByName() {
            var name = 'page';
            name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
            var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"), results = regex.exec(document.URL);
            return results == null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
        }
        function getDeviceId() {
            var name = 'deviceid';
            //name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");            
            var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"), results = regex.exec(document.URL);
            return results == null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <iframe id="FrameMain" width="100%" height="100%"></iframe>
    </form>
</body>
</html>
