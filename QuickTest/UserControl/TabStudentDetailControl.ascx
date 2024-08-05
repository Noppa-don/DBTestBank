<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="TabStudentDetailControl.ascx.vb" Inherits="QuickTest.TabStudentDetailControl" %>

<!DOCTYPE html>
<style type="text/css">

     .ForDivBtn, .ForDivBtnBottom {
            width: 800px;
            margin: auto;
            border: solid 1px #DA7C0C;
            border-radius: .5em;
            text-align: center;
            display: table;
            margin-top: 20px;
            background: #faa51a; /* Old browsers */
            background: url(data:image/svg+xml;base64,PD94bWwgdmVyc2lvbj0iMS4wIiA/Pgo8c3ZnIHhtbG5zPSJodHRwOi8vd3d3LnczLm9yZy8yMDAwL3N2ZyIgd2lkdGg9IjEwMCUiIGhlaWdodD0iMTAwJSIgdmlld0JveD0iMCAwIDEgMSIgcHJlc2VydmVBc3BlY3RSYXRpbz0ibm9uZSI+CiAgPGxpbmVhckdyYWRpZW50IGlkPSJncmFkLXVjZ2ctZ2VuZXJhdGVkIiBncmFkaWVudFVuaXRzPSJ1c2VyU3BhY2VPblVzZSIgeDE9IjAlIiB5MT0iMCUiIHgyPSIwJSIgeTI9IjEwMCUiPgogICAgPHN0b3Agb2Zmc2V0PSIwJSIgc3RvcC1jb2xvcj0iI2ZhYTUxYSIgc3RvcC1vcGFjaXR5PSIxIi8+CiAgICA8c3RvcCBvZmZzZXQ9IjEwMCUiIHN0b3AtY29sb3I9IiNmNDdhMjAiIHN0b3Atb3BhY2l0eT0iMSIvPgogIDwvbGluZWFyR3JhZGllbnQ+CiAgPHJlY3QgeD0iMCIgeT0iMCIgd2lkdGg9IjEiIGhlaWdodD0iMSIgZmlsbD0idXJsKCNncmFkLXVjZ2ctZ2VuZXJhdGVkKSIgLz4KPC9zdmc+);
            background: -moz-linear-gradient(top, #faa51a 0%, #f47a20 100%); /* FF3.6+ */
            background: -webkit-gradient(linear, left top, left bottom, color-stop(0%,#faa51a), color-stop(100%,#f47a20)); /* Chrome,Safari4+ */
            background: -webkit-linear-gradient(top, #faa51a 0%,#f47a20 100%); /* Chrome10+,Safari5.1+ */
            background: -o-linear-gradient(top, #faa51a 0%,#f47a20 100%); /* Opera 11.10+ */
            background: -ms-linear-gradient(top, #faa51a 0%,#f47a20 100%); /* IE10+ */
            background: linear-gradient(to bottom, #faa51a 0%,#f47a20 100%); /* W3C */
            filter: progid:DXImageTransform.Microsoft.gradient( startColorstr='#faa51a', endColorstr='#f47a20',GradientType=0 ); /* IE6-8 */
        }
</style>

   <div id="tab-container" class='tab-container'>
        <ul class='etabs'>
            <li class='tab'><a href="#tabs1-html">การบ้าน</a></li>
            <li class='tab'><a href="#tabs1-html">ประวัติควิซ</a></li>
            <li class='tab'><a href="#tabs1-html">ประวัติฝึกฝน</a></li>
            <li class='tab'><a href="#tabs1-html">กิจกรรม</a></li>
        </ul>
 
        <div class='panel-container'>
            <div id="tabs1-html">
                <div id='DivBottomInfo' class='ForDivBtnBottom' runat="server"></div>
            </div>
        </div>
   </div>
   
                 
                    
            

                            
