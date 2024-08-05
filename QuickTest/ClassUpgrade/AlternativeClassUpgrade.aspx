<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="AlternativeClassUpgrade.aspx.vb"
    Inherits="QuickTest.AlternativeClassUpgrade" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="../js/jquery-1.7.1.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('div').click(function () {

                if ($(this).is('#upgradeClass')) {
                    window.parent.location.href = '../ClassUpgrade/upgradeClass.aspx';
                }
                else if ($(this).is('#changeClass')) {
                    window.parent.location.href = '../ClassUpgrade/changeClass.aspx';
                }
                else if ($(this).is('#purgeClass')) {
                    window.parent.location.href = '../ClassUpgrade/purgeClass.aspx';
                }
            });
        });
    </script>
    <style type="text/css">
        @import url(../fonts/thsarabunnew.css);
        
        .submit
        {
            font: 100% 'THSarabunNew';
            border: 0;
            width: 99px;
            margin: 0 0 0 212px;
            height: 33px;
            padding: 2px 0 3px 0;
            cursor: pointer;
            background: #1EC9F4;
            -moz-border-radius: .5em;
            -webkit-border-radius: .5em;
            border-radius: .5em; /*behavior:url(border-radius.htc);*/
            -webkit-box-shadow: 0 1px 2px rgba(0,0,0,.2);
            -moz-box-shadow: 0 1px 2px rgba(0,0,0,.2);
            box-shadow: 0 1px 2px rgba(0,0,0,.2);
            color: #FFF;
            border: solid 1px #0D8AA9;
            background: #46C4DD;
            background: -webkit-gradient(linear, left top, left bottom, from(#63CFDF), to(#17B2D9));
            background: -moz-linear-gradient(top,  #63CFDF,  #17B2D9);
            background: linear-gradient(#63CFDF,  #17B2D9);
            -pie-background: linear-gradient(#63CFDF,  #17B2D9);
            text-shadow: 1px 1px #178497;
            position: absolute;
            background-color: #B6B6B6;
            behavior: url('../css/PIE.htc');
            -pie-track-active: false;
        }

    </style>
</head>
<body>
    <form id="form1" runat="server">
    <center>
        <div>
            <div id="changeClass" style="position: absolute; width: 200px; height: 170px; top: 18%;
                left: 18%; text-align: center; cursor: pointer; -webkit-border-radius: 2em;">
                <img alt="ย้ายห้องเรียน" src="../images/upgradeClass/btnMoveClass.png" height="120"
                    width="120" />
                <br />
                <asp:Button Style="margin: 0 0 0 0px; width: 200px; position: relative;" ID="btnReport"
                    runat="server" Text="ย้ายห้องเรียน" class="submit" />
            </div>
            <div id="purgeClass" style="position: absolute; width: 200px; height: 170px; top: 18%;
                left: 59%; text-align: center; cursor: pointer; -webkit-border-radius: 2em;">
                <img alt="ล้างข้อมูล" src="../images/upgradeClass/btnPurgeData.png" height="120"
                    width="120" />
                <br />
                <asp:Button Style="margin: 0 0 0 0px; width: 200px; position: relative;" ID="Button1"
                    runat="server" Text="ล้างข้อมูล" class="submit" />
            </div>
        </div>
    </center>
    </form>
</body>
</html>
