<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="selectedSearch.aspx.vb"
    Inherits="QuickTest.selectedSearch" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <%--<script src="js/jquery-1.7.1.js" type="text/javascript"></script>
    <script src="js/jquery.qtip-1.0.0-rc3.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('form').mouseup(function () {
                var selection = getSelected();
                //alert(selection);

                $.ajax({ type: "POST",
                  url: "<%=ResolveUrl("~")%>selectedSearch.aspx/translateFromSelected",
                  data:"{ selectedText : '" + selection + "'}",
                  async: false,                              
                  contentType: "application/json; charset=utf-8", dataType: "json",
                  success: function (data) {                                 
                       alert(data.d);
                       $('#testSpan').html(data.d);
                       $('#divtest').qtip({
                         content: data.d,
                         show: { ready: true },
                         style: {
                            width: 300, padding: 5, background: '#F68500', color: 'white', 
                            textAlign: 'left', border: { width: 7, radius: 5, color: '#F68500' }, 
                            tip: 'topLeft', name: 'dark', 'font-weight': 'bold'
                            }//,
                        ///position: { target: $('#txtTimeShowAnswer') }//,
                        //hide: { when: { event: 'mouseout' }, fixed: false }
                      });      
                  },
                  error: function myfunction(request, status) {   
                        alert(status);                      
                  }
                });
            });
        });
        function getSelected() {
            if (window.getSelection) { return window.getSelection(); }
            else if (document.getSelection) { return document.getSelection(); }
            else {
                var selection = document.selection && document.selection.createRange();
                if (selection.text) { return selection.text; }
                return false;
            }
            return false;
        }
    </script>--%>
</head>
<body>
    <%--<form id="form1" runat="server">
    <div id="divtest" style="width:100px;">
        a day div diction
    </div>
    <span id="testSpan"></span>
    </form>--%>
</body>
</html>
