<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="sound.aspx.vb" Inherits="QuickTest.sound" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="js/jquery-1.7.1.min.js"></script>
    <script src="js/howler.js"></script>
    <script type="text/javascript">
        var soundbg = new Howl({ urls: ['<%=ResolveUrl("~")%>images/bgmusic1.mp3'], autoplay: true, loop: true });
        //soundbg.play();
        var soundnext = new Howl({ urls: ['<%=ResolveUrl("~")%>images/next.mp3'] });

        $(function () {
            $('#btnSound').on('click', function () {
               
                soundnext.stop().play();
            });
        });
    </script>
</head>
<body onload="startSound()">
    <form id="form1" runat="server">
    <div>
     <%--<audio id="audiobg" autobuffer controls autoplay loop>
                <source src="<%=ResolveUrl("~")%>images/bgmusic1.mp3" />
            </audio>

            <audio id="audioNext" style="display: none;">
                <source src="<%=ResolveUrl("~")%>images/next.mp3" />
            </audio>

            <audio id="audioBack" style="display: none;">
                <source src="<%=ResolveUrl("~")%>images/back.mp3" />
            </audio>--%>
        <input type="button" id="btnbg"  style="width: 200px;height: 40px;" /><br />
        <input type="button" id="btnSound"  style="width: 200px;height: 40px;" />
    </div>
        
    </form>
  <%--  <script type="text/javascript">
        function init() {
            var myAudio = document.getElementById("audiobg");
            myAudio.addEventListener("ended", loopAudio, false);
        }
        function loopAudio(){
            var myAudio = document.getElementById("");
            myAudio.play();
        }
    </script>--%>
 <%--   <script type="text/javascript">
       
        $(document).ready(function() {
            $('#btnSound').trigger("click");
            
            setTimeout(function () {
                mySound.play();
            }, 2000);
            
            setInterval(function () {
                mySound.play();
            }, 3300);
        });

        

        var mySound;
        mySound = new sound("<%=ResolveUrl("~")%>images/bgmusic1.mp3");

        mySound.addEventListener("ended", loopAudio, false);
        function loopAudio() {
            mySound.play();
        }


        function startSound() {
         
        }

        function sound(src) {
            this.sound = document.createElement("audio");
            this.sound.src = src;
            this.sound.setAttribute("preload", "auto");
            this.sound.setAttribute("controls", "none");
            
            document.body.appendChild(this.sound);
            this.play = function () {
                this.sound.play();
            }
        }

        document.addEventListener('touchstart', function (ev) {
            mySound.play();
        });

        function playMySound() {
            setTimeout(function () {
                mySound.play(); alert(1);
            }, 3000);
           
        }
    </script>--%>
<%--   <script type="text/javascript">
       function fakeClick(fn) {
           var $a = $('<a href="#" id="fakeClick"></a>');
           $a.bind("click", function (e) {
               e.preventDefault();
               fn();
           });
           $('body').append($a);

           var evt, el = $('#fakeClick').get(0);

           if (document.createEvent) {
               evt = document.createEvent("MouseEvents");
               if (evt.initMouseEvent) {
                   evt.initMouseEvent("click", true, true, window, 0, 0, 0, 0, 0, false, false, false, false, 0, null);
                   el.dispatchEvent(evt);
               }
           }

           $(el).remove();
       }

       $(function () {
           var audio = $("#audiobg").get(0);

           fakeClick(function () {
               audio.play();
           });
       });

      
   </script>
   --%>
  
   </body>
</html>
