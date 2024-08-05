<%@ Page Title="QuickTest - เมนูทางเลือก" Language="vb"
    AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Alternative_Pad.aspx.vb"
    Inherits="QuickTest.Alternative_Pad" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">

    <style type="text/css">
  .Forbtn { 
        font: 100% 'THSarabunNew'; 
        border: 0; 
        padding: 2px 0 3px 0;
        cursor: pointer; 
        background: #1EC9F4; 
        -moz-border-radius: .5em;  
        -webkit-border-radius: .5em;
        border-radius:.5em;

        /*behavior:url(border-radius.htc);*/
        -webkit-box-shadow: 0 1px 2px rgba(0,0,0,.2);
        -moz-box-shadow: 0 1px 2px rgba(0,0,0,.2);
        box-shadow: 0 1px 2px rgba(0,0,0,.2);  
        color: #FFF;
        border: solid 1px #0D8AA9;
        background: #46C4DD;
        background: -webkit-gradient(linear, left top, left bottom, from(#63CFDF), to(#17B2D9));
        background: -moz-linear-gradient(top,  #63CFDF,  #17B2D9);
        text-shadow: 1px 1px #178497;
        behavior:url('../css/PIE.htc');
        -pie-track-active:false;
        margin:
   }
     </style>

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="HeadStyleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div style='width: 750px; margin-left: auto; margin-right: auto; height: 350px; margin-top: 150px;
        background-color: White; border-radius: 20px;'>
        <div style='background-color:#D3F2F7;width:710px;height:310px;margin-left:20px;top:20px;position:relative;border-radius: 20px;'>
        <table>
        <tr>
        <td style='text-align:center;'>
        <img src="../Images/Activity/Document-Add-icon.png" style='margin-top:15px;' />
        </td>
        <td style='text-align:center;'>
        <img src="../Images/Activity/File-Open-icon.png" />
        </td>
        </tr>
        <tr>
        <td style='text-align:center;'> 
            <input id='btnStep1' class='Forbtn' type="button" value='จัดชุดใหม่' style='width: 150px; height: 100px; background-color: Orange; border-radius: 10px;
                font-size: 30px; color: White;' />
                </td>
                <td style='text-align:center;'>
            <input id='btnPracticeMode' class='Forbtn' type="button" value='ฝึกฝน' style='width: 150px; height: 100px;
                background-color: Orange; border-radius: 10px;  font-size: 40px;
                color: White;' />
                </td>
                </tr>
                </table>
        </div>
    </div>
    <div id="dialog" title="ต้องเข้าควิซแล้วค่ะ">
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ExtraScript" runat="server">    
    <script src="../js/jquery-1.7.1.js" type="text/javascript"></script>
    <script src="../js/jquery-ui-1.8.18.js" type="text/javascript"></script>
    <link href="../css/jquery-ui-1.8.18.custom.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery.signalR-2.0.2.min.js" type="text/javascript"></script>
    <script src='<%=ResolveClientUrl("~/signalr/hubs") %>' type="text/javascript"></script>
    <script type="text/javascript">

        $(function () {

            var groupname = '<%=GroupName %>';

            SignalRCheck = $.connection.hubSignalR;
            // alert(groupname);

            var step1Page = '<%=ResolveUrl("~")%>testset/step1.aspx';
            step1Page = step1Page.toLowerCase();
            var settingPage = '<%=ResolveUrl("~")%>activity/settingactivity.aspx';
            settingPage = settingPage.toLowerCase();
            var activityPage = '<%=ResolveUrl("~")%>activity/activitypage.aspx';
            activityPage = activityPage.toLowerCase();
            var thisPage = '<%=ResolveUrl("~")%>activity/alternativepage.aspx';
            thisPage = thisPage.toLowerCase();

            SignalRCheck.client.send = function (message) {
                //alert(message);
                if (message == step1Page) {                    
                    var joinQuiz = '<%=ResolveUrl("~")%>TestSet/Step1_PadTeacher.aspx';
                    window.location = joinQuiz;
                    //dialogInterrupt(joinQuiz);
                }
                else if (message == settingPage) {
                    window.location = '<%=ResolveUrl("~")%>Activity/SettingActivity_PadTeacher.aspx';
                }
                else if (message == activityPage) {
                    window.location = '<%=ResolveUrl("~")%>Activity/ActivityPage_PadTeacher.aspx';
                }               
            };

            $.connection.hub.start().done(function () {
                SignalRCheck.server.addToGroup(groupname);
                SignalRCheck.server.sendCommand(groupname, thisPage);
            });

            $('input[type=button]').click(function () {
                if ($(this).is('#btnStep1')) {
                    setCurrentPage(step1Page);
                    window.location = '<%=ResolveUrl("~")%>Testset/Step1_PadTeacher.aspx'
                }
                else {
                    window.location = '<%=ResolveUrl("~")%>PracticeMode_Pad/ChooseClass.aspx'
                }
            });

        });
        function dialogInterrupt(href) {
            $('#dialog').dialog({
                autoOpen: open,
                buttons: { 'ตกลง': function () { window.location.href = href; } },
                draggable: false,
                resizable: false,
                modal: true
            });
        }

        function setUnload(unload) {
            $.ajax({ type: "POST",
                  url: "<%=ResolveUrl("~")%>SelectSessionSignalR.aspx/setUnload", 
                  data:"{ unload : '" + unload + "'}",                 
                  async: false,                              
                  contentType: "application/json; charset=utf-8", dataType: "json",
                  success: function (data) {                                              
                  },
                  error: function myfunction(request, status) {   
                        alert(status);                      
                  }
            });
            }
        function  getUnload() {
            $.ajax({ type: "POST",
                  url: "<%=ResolveUrl("~")%>SelectSessionSignalR.aspx/getUnload",
                  async: false,                              
                  contentType: "application/json; charset=utf-8", dataType: "json",
                  success: function (data) {                                 
                        Unload = data.d;                        
                  },
                  error: function myfunction(request, status) {   
                        alert(status);                      
                  }
            });
        }
        function setCurrentPage(thisPage) {
            $.ajax({ type: "POST",
                  url: "<%=ResolveUrl("~")%>SelectSessionSignalR.aspx/setCurrentPage",
                  data:"{ thisPage : '" + thisPage + "'}",
                  async: false,                              
                  contentType: "application/json; charset=utf-8", dataType: "json",
                  success: function (data) {                                              
                  },
                  error: function myfunction(request, status) {   
                        alert(status);                      
                  }
            });
        }
        function  getCurrentPage() {
            $.ajax({ type: "POST",
                  url: "<%=ResolveUrl("~")%>SelectSessionSignalR.aspx/getCurrentPage",
                  async: false,                              
                  contentType: "application/json; charset=utf-8", dataType: "json",
                  success: function (data) {                                 
                        CurrentPage = data.d;                        
                  },
                  error: function myfunction(request, status) {   
                        alert(status);                      
                  }
            });
        }
    </script>
    <script type="text/javascript">
        $(function () {
            
        });
    </script>
</asp:Content>
