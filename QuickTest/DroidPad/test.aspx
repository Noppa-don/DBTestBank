<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="test.aspx.vb" Inherits="QuickTest.test" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="../js/jquery-1.7.1.js" type="text/javascript"></script>
    <script src="../js/jquery-ui-1.8.18.js" type="text/javascript"></script>
    <script src="../js/jquery.prettyPhoto.js" type="text/javascript"></script>
    <link href="../css/jquery-ui-1.8.18.custom.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
    // RANDOM ON KEYUP OF Percent
        $(function () {
            var delayID = null;
            $('#txtPercent').click(function (e) {
                 getStudentDontReply();
            });
     
        });
         
            function getStudentDontReply() {
                alert('aaa');
                $.ajax({ type: "POST",
                  url: "<%=ResolveUrl("~")%>DroidPad/install.aspx/RegisterWithSchool",
                  async: false,         
                  data: "{DeviceUniqueID : 'A999' , SchoolID : '555', SchoolPassword : '666' }",  //"                      
                  contentType: "application/json; charset=utf-8", dataType: "json",
                  success: function (data) {                 
                    alert(data);                   
                        var studentId = jQuery.parseJSON(data.d);                       
                        alert(studentId);
                        //$('#ForActivityPage').append('<div class="divReplyAns"><br/>' + studentId.stuId + '</div>');

//////                        for(i = 0;i< studentId.length;i++){
//////                            $('#ForActivityPage').append('<div class="divReplyAns"><br/>' + studentId[i].stuId + '</div>');
//////                        }
//////                        //sum no. user don't reply
//////                        var sumDontReply = $('.divReplyAns').length;
//////                        $('#sumDontReply').text(sumDontReply);
                                          
                  },
                  error: function myfunction(request, status) {   
                        alert(status);                      
                  }
                });
            }
   
        
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <asp:Button ID="btnAA" runat="server" Text="Button" OnClientClick="getStudentDontReply();" />
    
        <asp:Button ID="Button1" runat="server" Text="GenSI" />
    
    </div>
    </form>
</body>
</html>
