<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="test1.aspx.vb" Inherits="QuickTest.test1" UICulture="th-TH" Culture="th-TH" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <script type="text/javascript" src="js/jquery-1.7.1.js"></script>
    <script type="text/javascript" src="js/jquery-ui-1.8.18.min.js"></script>
    <link rel="stylesheet" href="css/jquery.button-audio-player.css" />
    <script type="text/javascript" src="js/jquery.slim.min.js"></script>
    <script type="text/javascript" src="js/jquery.button-audio-player.js"></script>

</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <h1>Single Button Audio Player Example</h1>
            <div class="demo">
                <div id="QuestionMulti"></div>
                <p>Default</p>
            </div>
        </div>
    </form>
    <script type="text/javascript">
        (function ($) {
            $('#QuestionMulti').buttonAudioPlayer({
                type: 'default',
                src: 'https://www.jqueryscript.net/dummy/1.mp3'
            });
        })(jQuery);
    </script>
</body>
</html>
