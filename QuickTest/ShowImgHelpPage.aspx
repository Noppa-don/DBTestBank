<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ShowImgHelpPage.aspx.vb" Inherits="QuickTest.ShowImgHelpPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <script src="<%=ResolveUrl("~")%>js/jquery-1.7.1.js" type="text/javascript"></script>

    <script type="text/javascript">
        var JVRunMode = '<%= RunMode %>';

        $(function () {
            var JvFolderName = "<%= FolderName%>";
            var JvPageName = "<%= PageName%>";
            var JvCheckIslastImg = '<%=IsLastImage %>';
            if ($('#ImgHdNo').val() == '0') {
                var imgPath = '<%=ResolveUrl("~")%>HowTo/HelpImg/' + JVRunMode + '/' + JvFolderName + '_' + JvPageName + '00.png';
                $('#DivimgHelp').css("background-image", "url(" + imgPath + ")");
                if (JvCheckIslastImg == 'True') {
                    $('#btnNext').val('OK เข้าใจครบล่ะ!');
                    $('#btnNext').removeClass('UnderStand');
                    $('#btnNext').addClass('CompleteHelp');
                    $('#btnNext').css('font-size', '20px');
                }
            }
            else {
                alert($('#ImgHdNo').val());
                alert('else');
            }

            $('#btnNext').click(function () {
                if ($(this).hasClass('CompleteHelp') == false) {
                    ShowNextHelp(JvFolderName, JvPageName, $('#ImgHdNo').val());
                }
            });

            $('#btnBack').click(function () {
                ShowBackHelp(JvFolderName, JvPageName, $('#ImgHdNo').val())
            });
            
            $(".CompleteHelp").live("click", function () {
                parent.CloseHelpPanel();
            });

        });

        function ShowNextHelp(inputFolderName, InputPageName, InputImgNo) {
            $('#btnBack').show();
            $.ajax({
                type: "POST",
                url: "<%=ResolveUrl("~")%>ShowImgHelpPage.aspx/GetNextImgHelpSrc",
                data: "{ FolderName: '" + inputFolderName + "',PageName:'" + InputPageName + "',ImgNo:'" + InputImgNo + "'}",
                contentType: "application/json; charset=utf-8", dataType: "json",
                success: function (data) {
                    var JsonString = jQuery.parseJSON(data.d);
                    var JvImgSrc = JsonString.SrcImg;
                    var JvIsLastImg = JsonString.IsLastImg;
                    var imgPath = '<%=ResolveUrl("~")%>HowTo/HelpImg/' + JVRunMode + '/' + inputFolderName + '_' + InputPageName + JvImgSrc + '.png';
                    if (JvImgSrc !== '') {
                        $('#DivimgHelp').css("background-image", "url(" + imgPath + ")");
                        if (JvIsLastImg == 'True') {
                            $('#btnNext').val('OK เข้าใจครบล่ะ!');
                            $('#btnNext').css('font-size','20px');
                            $('#btnNext').removeClass('UnderStand');
                            $('#btnNext').addClass('CompleteHelp');
                        }
                        var intImgNo = parseInt($('#ImgHdNo').val());
                        intImgNo += 1;
                        $('#ImgHdNo').val(intImgNo);
                    }
                },
                error: function myfunction(request, status) {
                    //alert('บางอย่างผิดพลาดโปรดลองอีกครั้ง')
                }
                 });
        }

        function ShowBackHelp(inputFolderName, InputPageName, InputImgNo) {
            $('#btnNext').val('เข้าใจ');
            $('#btnNext').css('font-size', '30px');
            $('#btnNext').removeClass('CompleteHelp');
            $('#btnNext').addClass('UnderStand');
            $.ajax({
                type: "POST",
                url: "<%=ResolveUrl("~")%>ShowImgHelpPage.aspx/GetBackImgHelpSrc",
                data: "{ FolderName: '" + inputFolderName + "',PageName:'" + InputPageName + "',ImgNo:'" + InputImgNo + "'}",
                contentType: "application/json; charset=utf-8", dataType: "json",
                success: function (data) {
                    var JsonString = jQuery.parseJSON(data.d);
                    var JvImgSrc = JsonString.SrcImg;
                    var JvIsLastImg = JsonString.IsLastImg;
                    var imgPath = '<%=ResolveUrl("~")%>HowTo/HelpImg/' + JVRunMode + '/' + inputFolderName + '_' + InputPageName + JvImgSrc + '.png';
                    if (JvImgSrc !== '') {
                        $('#DivimgHelp').css("background-image", "url(" + imgPath + ")");
                        if (JvIsLastImg == 'True') {
                            $('#btnBack').hide();
                        }
                        var intImgNo = parseInt($('#ImgHdNo').val());
                        intImgNo -= 1;
                        $('#ImgHdNo').val(intImgNo);
                    }
                    else {

                    }
                },
                error: function myfunction(request, status) {
                    //alert('บางอย่างผิดพลาดโปรดลองอีกครั้ง')
                }
            });
        }

    </script>

    <style type="text/css">
        #DivimgHelp {
        width:730px;
        height:355px;
        border-radius:10px;
        background-size:cover;
        margin-left:auto;
        margin-right:auto;
        }
        #DivButton {
        padding:10px;
        }
        #btnBack {
        float:left;
        /*margin-top:10px;
        font: 100% 'THSarabunNew';
        border: 0;
        padding: 2px 0 3px 0;
        cursor: pointer;
        background: #1EC9F4;
        -moz-border-radius: .5em;
        -webkit-border-radius: .5em;
        border-radius: .5em; 
        behavior:url(border-radius.htc);
        -webkit-box-shadow: 0 1px 2px rgba(0,0,0,.2);
        -moz-box-shadow: 0 1px 2px rgba(0,0,0,.2);
        box-shadow: 0 1px 2px rgba(0,0,0,.2);
        color: #FFF;
        border: solid 1px #0D8AA9;
        background: #46C4DD;
        background: -webkit-gradient(linear, left top, left bottom, from(#63CFDF), to(#17B2D9));
        background: -moz-linear-gradient(top,  #63CFDF,  #17B2D9);
        text-shadow: 1px 1px #178497;
        behavior: url('../css/PIE.htc');
        -pie-track-active: false;
        height:50px;
        width:150px;
        font-size:30px;*/
        }
        #btnNext {
        float:right;
        /*margin-top:10px;
        font: 100% 'THSarabunNew';
        border: 0;
        padding: 2px 0 3px 0;
        cursor: pointer;
        background: #1EC9F4;
        -moz-border-radius: .5em;
        -webkit-border-radius: .5em;
        border-radius: .5em; behavior:url(border-radius.htc);
        -webkit-box-shadow: 0 1px 2px rgba(0,0,0,.2);
        -moz-box-shadow: 0 1px 2px rgba(0,0,0,.2);
        box-shadow: 0 1px 2px rgba(0,0,0,.2);
        color: #FFF;
        border: solid 1px #0D8AA9;
        background: #46C4DD;
        background: -webkit-gradient(linear, left top, left bottom, from(#63CFDF), to(#17B2D9));
        background: -moz-linear-gradient(top,  #63CFDF,  #17B2D9);
        text-shadow: 1px 1px #178497;
        behavior: url('../css/PIE.htc');
        -pie-track-active: false;
        height:50px;
        width:150px;
        font-size:30px;*/
        }
        .ForBtn {
        position: relative;
        display: inline-block;
        margin: 0 0.2%;
        background-color: #F68500;
        list-style: none;
        padding: 0;
        text-shadow: 1px 1px #7E4D0E;
        -webkit-box-shadow: 0 1px 2px rgba(0,0,0,.2);
        -moz-box-shadow: 0 1px 2px rgba(0,0,0,.2);
        box-shadow: 0 1px 2px rgba(0,0,0,.2);
        color: #FFF;
        border: solid 1px #DA7C0C;
        background: #F78D1D;
        background: -webkit-gradient(linear, left top, left bottom, from(#FAA51A), to(#F47A20));
        background: -moz-linear-gradient(top, #FAA51A, #F47A20);
        -webkit-border-radius: 0.5em;
        -moz-border-radius: 0.5em;
        border-radius: 0.5em;
        height:50px;
        width:150px;
        font-size:30px;
        margin-top:10px;
        cursor:pointer;
        }

    </style>

    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div id="MainDiv">
        <div id="DivimgHelp">
        </div>
         
        <input type="button" value="เข้าใจ" class="UnderStand ForBtn" id="btnNext" />
        <input type="button" value="ย้อนกลับ" style="display:none;" class="UnderStand ForBtn" id="btnBack" />
        <input  type="hidden" value="0" id="ImgHdNo" />
        <input type="hidden" id='HdCheckPostBack' value="<%=Page.IsPostBack.Tostring() %>" />
             
    </div>
    </form>
</body>
</html>
