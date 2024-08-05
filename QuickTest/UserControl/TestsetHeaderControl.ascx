<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="TestsetHeaderControl.ascx.vb" Inherits="QuickTest.TestsetHeaderControl" %>
<style type="text/css">
    @import url(../fonts/thsarabunnew.css);

    .ForSmallDetailDiv {
        border-radius: 3px;
        margin: 5px 5px 5px 5px;
        font-size: 20px;
        display: inline-block;
        /*border: solid 1px #DA7C0C;*/
        padding: 5px 10px 5px 10px;
        font: 100% 'THSarabunNew';
        color: white;
        background: #faa51a; /* Old browsers */
    }

    /* IE9 SVG, needs conditional override of 'filter' to 'none' 
        background: url(data:image/svg+xml;base64,PD94bWwgdmVyc2lvbj0iMS4wIiA/Pgo8c3ZnIHhtbG5zPSJodHRwOi8vd3d3LnczLm9yZy8yMDAwL3N2ZyIgd2lkdGg9IjEwMCUiIGhlaWdodD0iMTAwJSIgdmlld0JveD0iMCAwIDEgMSIgcHJlc2VydmVBc3BlY3RSYXRpbz0ibm9uZSI+CiAgPGxpbmVhckdyYWRpZW50IGlkPSJncmFkLXVjZ2ctZ2VuZXJhdGVkIiBncmFkaWVudFVuaXRzPSJ1c2VyU3BhY2VPblVzZSIgeDE9IjAlIiB5MT0iMCUiIHgyPSIwJSIgeTI9IjEwMCUiPgogICAgPHN0b3Agb2Zmc2V0PSIwJSIgc3RvcC1jb2xvcj0iI2ZhYTUxYSIgc3RvcC1vcGFjaXR5PSIxIi8+CiAgICA8c3RvcCBvZmZzZXQ9IjEwMCUiIHN0b3AtY29sb3I9IiNmNDdhMjAiIHN0b3Atb3BhY2l0eT0iMSIvPgogIDwvbGluZWFyR3JhZGllbnQ+CiAgPHJlY3QgeD0iMCIgeT0iMCIgd2lkdGg9IjEiIGhlaWdodD0iMSIgZmlsbD0idXJsKCNncmFkLXVjZ2ctZ2VuZXJhdGVkKSIgLz4KPC9zdmc+);
        background: -moz-linear-gradient(top, #faa51a 0%, #f47a20 100%); /* FF3.6+ */
    /*background: -webkit-gradient(linear, left top, left bottom, color-stop(0%,#faa51a), color-stop(100%,#f47a20)); /* Chrome,Safari4+ */
    /*background: -webkit-linear-gradient(top, #faa51a 0%,#f47a20 100%);*/ /* Chrome10+,Safari5.1+ */
    /*background: -o-linear-gradient(top, #faa51a 0%,#f47a20 100%);*/ /* Opera 11.10+ */
    /*background: -ms-linear-gradient(top, #faa51a 0%,#f47a20 100%);*/ /* IE10+ */
    /*background: linear-gradient(to bottom, #faa51a 0%,#f47a20 100%);*/ /* W3C */
    /*filter: progid:DXImageTransform.Microsoft.gradient( startColorstr='#faa51a', endColorstr='#f47a20',GradientType=0 );*/ /* IE6-8 */
    #MainTestsetDiv {
        padding-top: 25px;
    }

    #MainTestsetDiv {
        margin-top: 10px;
        padding: 10px;
        width: 95.5%;
        /*margin-left: 1%;*/
        border: 2px solid #FFA032;
        background-color: wheat;
        border-radius: 5px;
        margin-left:auto;
        margin-right:auto;
    }
</style>
<!--[if gte IE 9]>
        <style type="text/css">
        .gradient{filter:none;}
        </style>
        <![endif]-->
<div id="MainTestsetDiv">
    <div id='MainDiv' style='width: 98%; /*border: solid 1px; */ border-radius: 4px; margin-left: 1%; border-bottom-width: thin;'>
        <div id='DivRow1' style='font-size: 30px; margin-left: 10px; margin-top: 5px; font: THSarabunNew;' runat="server">
        </div>
        <div id='DivRow2' style='font-size: 18px; margin-left: 10px; margin-top: 10px;' runat="server">
        </div>
        <div id='DivRow3' style='width: 100%; overflow-x: auto; height: 60px; overflow-y: hidden; white-space: nowrap; margin-top: 10px;' runat="server">
        </div>
    </div>
</div>

