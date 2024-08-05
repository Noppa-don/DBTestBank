<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="LoginPageQR.aspx.vb" Inherits="QuickTest.LoginPageQR" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
       
    </style>
    <script src="js/jquery-1.7.1.js" type="text/javascript"></script>
    <script src="js/jquery-ui-1.8.18.min.js" type="text/javascript"></script>


    <script type="text/javascript">

        oFReader = new FileReader();

        oFReader.onload = function (oFREvent) {
            document.getElementById("fotoImg").src = oFREvent.target.result;
            document.getElementById("fotoImg").style.visibility = "visible";
            var screenHeight = screen.availHeight;
            screenHeight = screenHeight - 220;
            document.getElementById("fotoImg").style.height = screenHeight;
            console.log(oFREvent.target.result);
            $.ajax({
                type: "POST",
                url: "<%=ResolveUrl("~")%>test1.aspx/CheckUser",
                data: "{ StrBase64 : '" + oFREvent.target.result + "' }",  //" 
                contentType: "application/json; charset=utf-8", dataType: "json",
                success: function (msg) {
                    var a = msg.d;
                    //alert(a);
                    //$("#spnValid").show();
                    //setTimeout(function () {
                    //    $("#spnValid").hide();
                    //    $("#spnLoginValid").show();
                    //}, 4000);
                    //setTimeout(function () {
                    //    parent.LogInWithQRCode('admin', 'network');
                    //}, 3000);
                    parent.LogInWithQRCode('admin', 'network');
                },
                error: function myfunction(request, status) {
                    alert('error');
                }
            });
        };

        $(function () {
            $("input:file").change(function () {
                var input = document.querySelector('input[type=file]');
                var oFile = input.files[0];
                oFReader.readAsDataURL(oFile);
            });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div style="text-align: center;">
            <h1>เข้าระบบด้วยคิวอาร์โค้ด</h1>
            <div style="margin: auto; width: 150px; height: 150px; background-color: red;"></div>
            <input id="filePic" type="file" name="image" accept="image/*" capture>
            <img id="fotoImg" style="visibility: hidden; width: 100px; height: 100px">
            
            <span id="spnValid" style="display:none;">กำลังเช็คความถูกต้องค่ะ....</span>
            <span id="spnLoginValid" style="display:none;">กำลังล็อคอินด้วยชื่อผู้ใช้ ______ ค่ะ</span>
        </div>
    </form>
</body>
</html>
