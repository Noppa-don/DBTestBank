<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ScanQRControl.ascx.vb" Inherits="QuickTest.ScanQRControl" %>

    <meta name="description" content="QR Code scanner" />
    <meta name="keywords" content="qrcode,qr code,scanner,barcode,javascript" />
    <meta name="language" content="English" />
    <meta name="copyright" content="Lazar Laszlo (c) 2011" />
    <meta name="Revisit-After" content="1 Days"/>
    <meta name="robots" content="index, follow"/>
    <meta http-equiv="Content-type" content="text/html;charset=UTF-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    
    <script type="text/javascript" src="../js/llqrcode.js"></script>
    <script type="text/javascript" src="../js/plusone.js"></script>
    <script type="text/javascript" src="../js/webqr.js"></script>  
    <script src="../js/jquery-1.7.1.min.js" type="text/javascript"></script>
    <script src="../js/jquery-ui-1.8.18.min.js" type="text/javascript"></script>
    <script src="../js/Animation.js" type="text/javascript"></script>
    <script src="../js/jquery.fancybox.js" type="text/javascript"></script>
    
    <link href="../css/jquery.fancybox.css" rel="stylesheet" />
    <link href="../css/jquery-ui-1.8.18.custom.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript">

        $(function () {
            $('#Alert').text('ไม่สามารถอ่าน QR ได้ ลองอีกครั้งนะคะ');
            $('#Alert').hide();
            load();
        });      
       
        function ResultOK(UserData) {
            $.ajax({
                type: "POST",
                url: "<%=ResolveUrl("~")%>PracticeMode_Pad/MainPractice.aspx/CheckUserFromReadQR",
                data: "{ QRUserData : '" + UserData + "' }",  //" 
                contentType: "application/json; charset=utf-8", dataType: "json",
                success: function (msg) {
                    var a = msg.d;
                    if (a == '0') {
                        $('#Alert').text('QR นี้ไม่สามารถใช้งานได้ ติดต่อเจ้าหน้าที่ศูนย์คอมนะคะ');
                        $('#Alert').show();
                        setTimeout(
                            function ()
                            {
                                $('#QRCode').dialog('close');
                            },5000);
 
                    } else {
                        window.location = '../PracticeMode_Pad/ChooseClass.aspx?UseComputer=0&DashboardMode=6';
                    }

                },
                error: function myfunction(request, status) {
                 
                }
                 });
        }
    </script>

    <script type="text/javascript">

        $(function () {
            $('#btnSelectFile').click(function () {
                $('#filePic').click();
                
            });
        });
        
            oFReader = new FileReader();
            oFReader.onload = function (oFREvent) {
            document.getElementById("fotoImg").src = oFREvent.target.result;
            document.getElementById("fotoImg").style.visibility = "visible";
            var screenHeight = screen.availHeight;
            screenHeight = screenHeight - 220;
            document.getElementById("fotoImg").style.height = screenHeight;

            $.ajax({
                type: "POST",
                url: "<%=ResolveUrl("~")%>PracticeMode_Pad/MainPractice.aspx/CheckUser",
                data: "{ StrBase64 : '" + oFREvent.target.result + "' }",  //" 
                contentType: "application/json; charset=utf-8", dataType: "json",
                success: function (msg) {
                    var ar = msg.d;
                    if (ar == '0') {
                        $('#Alert').text('QR นี้ไม่สามารถใช้งานได้ ติดต่อเจ้าหน้าที่ศูนย์คอมนะคะ');
                        $('#Alert').show();
                        setTimeout(
                           function () {
                               $('#QRCode').dialog('close');
                           }, 5000);

                    } else {
                        window.location = '../PracticeMode_Pad/ChooseClass.aspx?UseComputer=0&DashboardMode=6';
                    }
                    
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

            <div id='QRCode' title='scanner' ">
                <div>
                    <div id="Div1">
                        <tr>
                            <td>
                                <div id="header" style="display:none;">
                                    <div style="position:relative;top:+20px;left:0px;"><g:plusone size="medium"></g:plusone></div>
                                    <canvas id="Canvas1" width="300" height="300" style="margin-left:105px"></canvas>
                                    <div id="result"></div>
                                </div>
                                <div id="mainbody"></div>
                                <canvas id="qr-canvas" width="800" height="600"></canvas>
                                <div id="outdiv"></div>
                            </td>
                        </tr>
                        <tr>
                            <div id="SpanAlert" style="margin-top: 10px;text-align: center; ">
                                <span id="Alert" style="color: red; display:none;">ไม่สามารถอ่าน QR ได้ ลองอีกครั้งนะคะ</span>
                            </div>
                        </tr>
                        <tr>
                            <td>
                                <div>
                                    <input id="btnSelectFile" value="ภาพถ่าย" class="Forbtn" type="button" />
                                    <input id="filePic" type="file" name="image" style="display:none;" accept="image/*"> 	
                                    <img id="fotoImg" style="visibility: hidden;width:100px;height:100px; display:none;">
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td><img class="selector" id="webcamimg" src="vid.png" onclick="setwebcam()" align="left" style="display:none;" /></td>
                            <td><img class="selector" id="qrimg" src="cam.png" onclick="setimg()" align="right" style="display:none;" /></td>
                        </tr>
                    </div>
                </div>
            </div>
