<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="SelectTermControl.ascx.vb"
    Inherits="QuickTest.SelectTermControl" %>
<style type="text/css">
    .selectedTerm {
        width: 400px;
        margin-left: auto;
        margin-right: auto;
        cursor: pointer;
    }
</style>
<link href="../css/jquery.fancybox.css" rel="stylesheet" type="text/css" />
<script src="../js/jquery.fancybox.js" type="text/javascript"></script>
<script type="text/javascript">
    var ua = navigator.userAgent.toLowerCase();
    var isAndroid = ua.indexOf("android") > -1;
    function SelectedTerm() {
        var h;
        if (typeof t360SchoolId == 'undefined') {
            if (isAndroid) {
                h = 450;
                w = 750;
            }
            else {
                h = 600;
                w = 850;
            }
        } else {
            h = 300
            w = 600;
        }
        $.fancybox({
            'autoScale': true,
            'transitionIn': 'none',
            'transitionOut': 'none',
            'href': '<%=ResolveUrl("~")%>Student/SelectTermPage.aspx',
            'type': 'iframe',
            'width': w,
            'minHeight': h
            //'onClose': refreshPage()
        });

    }
</script>
<div class="selectedTerm" onclick="SelectedTerm();">
    <%--<a class="aTerm" rel="PrettyTerm" href="<%=ResolveUrl("~")%>Student/SelectTermPage.aspx?iframe=true&width=850&height=600">--%>
    <center>
        <h2 style="margin: 0;">
            <label id="lblTerm" runat="server">
            </label>
            <img src="../Images/dashboard/ChooseTerm.png" alt="" style="position: absolute; " />
        </h2>
    </center>
    <%-- </a>--%>
</div>
