<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="StudentDetailModeControl.ascx.vb" Inherits="QuickTest.StudentDetailModeControl" %>
  
    <script src="../js/jquery-1.7.1.min.js" type="text/javascript"></script> 
    <script src="../js/jquery.hashchange.min.js" type="text/javascript"></script>
    <script src="../js/jquery.easytabs.min.js" type="text/javascript"></script>

<style type="text/css">
      
    .etabs { margin: 0; padding: 0; }
    
    .tab { display: inline-block; zoom:1; *display:inline; background: #eee; border: solid 1px #999; border-bottom: none; -moz-border-radius: 4px 4px 0 0; 
            -webkit-border-radius: 4px 4px 0 0; }
    .tab a { font-size: 14px; line-height: 2em; display: block; padding: 0 10px; outline: none; }
    
    .tab a:hover { text-decoration: underline; }
    
    .tab.active { background: #fff; padding-top: 6px; position: relative; top: 1px; border-color: #666; }
   
    .tab a.active { font-weight: bold; }
    
    .tab-container .panel-container { background: #fff; border: solid #666 1px; padding: 10px; -moz-border-radius: 0 4px 4px 4px; 
                                      -webkit-border-radius: 0 4px 4px 4px; }
    
    .panel-container { margin-bottom: 10px; }
  
</style>

 
<script type="text/javascript">

    $(document).ready(function () {


        $('#subtabs').easytabs();

    });

</script>
    <div id="subtabs" class='tab-container'>
        <ul class='etabs'>
            <li class='tab'><a href="#Homework">การบ้าน</a></li>
            <li class='tab'><a href="#Quiz">ประวัติควิซ</a></li>
            <li class='tab'><a href="#Practice">ประวัติฝึกฝน</a></li>
            <li class='tab'><a href="#Log">กิจกรรม</a></li>
        </ul>
        <div class='panel-container'>
            <div id="Homework">
                <div id='DivHomeWork' class='ForDivBtnBottom' runat="server">
                      HW
                </div>
            </div>
            <div id="Quiz">
                <div id='DivQuiz' class='ForDivBtnBottom' runat="server">
                    Q
                </div>
            </div>
            <div id="Practice">
                <div id='DivPractice' class='ForDivBtnBottom' runat="server">
                    P
                </div>
            </div>
            <div id="Log">
                <div id='DivLog' class='ForDivBtnBottom' runat="server">
                    L
                </div>
            </div>
        </div>
</div>