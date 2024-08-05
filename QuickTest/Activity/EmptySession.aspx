<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="EmptySession.aspx.vb"
    Inherits="QuickTest.EmptySession" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="../js/jquery-1.7.1.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.signalR-2.0.2.min.js" type="text/javascript"></script>
    <script src='<%=ResolveClientUrl("~/signalr/hubs") %>' type="text/javascript"></script>
    <script type="text/javascript">

        $(function () {

            var groupname = '<%=GroupName %>';
            var JsDeviceId = '<%=DVID %>';
            SignalRCheck = $.connection.hubSignalR;
            // alert(groupname);

            SignalRCheck.client.send = function (message) {
                if (message == 'Reload') {
                    $.ajax({ type: "POST",
	                url: "<%=ResolveUrl("~")%>DroidPad/StudentAction.aspx/SendToGetNextAction",
	                data: "{ DeviceUniqueID: '" + CurrentDeviceId + "',IsFirstTime:'NoValue',IsTeacher:false}",
	                contentType: "application/json; charset=utf-8", dataType: "json",   
	                success: function (msg) {
                    //ถ้าค่าที่ Return กลับมาไม่เป็นค่าว่างให้เปลี่ยนไปหน้าเด็ก
                    var objJson = jQuery.parseJSON(msg.d);
                    if (objJson.Param.NextURL !== '') {
                    window.location = '<%=ResolveUrl("~")%>Activity/ActivityPage_Pad.aspx?DeviceUniqueID=' + JsDeviceId; 
                    }
	                },
	                error: function myfunction(request, status)  {
                    alert('บางอย่างผิดพลาดโปรดลองอีกครั้ง')
	                }
	                });
                }
            };

            $.connection.hub.start().done(function () {
                SignalRCheck.server.addToGroup(groupname);
                //SignalRCheck.server.sendCommand(groupname, '../TestSet/Step1.aspx');
            });

        });


    </script>
    <%--<script type="text/javascript">
        $(document).ready(function () {
            var BrowserDetect = {
                init: function () {
                    this.browser = this.searchString(this.dataBrowser) || "An unknown browser";
                    this.version = this.searchVersion(navigator.userAgent)
                                   || this.searchVersion(navigator.appVersion)
                                   || "an unknown version";
                    this.OS = this.searchString(this.dataOS) || "an unknown OS";
                },
                searchString: function (data) {
                    for (var i = 0; i < data.length; i++) {
                        var dataString = data[i].string;
                        var dataProp = data[i].prop;
                        this.versionSearchString = data[i].versionSearch || data[i].identity;
                        if (dataString) {
                            if (dataString.indexOf(data[i].subString) != -1)
                                return data[i].identity;
                        }
                        else if (dataProp) return data[i].identity;
                    }
                },
                searchVersion: function (dataString) {
                    var index = dataString.indexOf(this.versionSearchString);
                    if (index == -1) return;
                    return parseFloat(dataString.substring(index + this.versionSearchString.length + 1));
                },
                dataBrowser: [
                            { string: navigator.userAgent, subString: "Chrome", identity: "Chrome" },
                            { string: navigator.userAgent, subString: "OmniWeb", versionSearch: "OmniWeb/", identity: "OmniWeb" },
                            { string: navigator.vendor, subString: "Apple", identity: "Safari", versionSearch: "Version" },
                            { prop: window.opera, identity: "Opera", versionSearch: "Version" },
                            { string: navigator.vendor, subString: "iCab", identity: "iCab" },
                            { string: navigator.vendor, subString: "KDE", identity: "Konqueror" },
                            { string: navigator.userAgent, subString: "Firefox", identity: "Firefox" },
                            { string: navigator.vendor, subString: "Camino", identity: "Camino" },
                            { //  for newer Netscapes (6+) 
                                string: navigator.userAgent, subString: "Netscape", identity: "Netscape"
                            },
                            { string: navigator.userAgent, subString: "MSIE", identity: "Explorer", versionSearch: "MSIE" },
                            { string: navigator.userAgent, subString: "Gecko", identity: "Mozilla", versionSearch: "rv" },
                            {
                                // for older Netscapes (4-) 
                                string: navigator.userAgent, subString: "Mozilla", identity: "Netscape", versionSearch: "Mozilla"
                            }],
                dataOS: [{ string: navigator.platform, subString: "Win", identity: "Windows" },
                            { string: navigator.platform, subString: "Mac", identity: "Mac" },
                            { string: navigator.userAgent, subString: "iPhone", identity: "iPhone/iPod" },
                            { string: navigator.platform, subString: "Linux", identity: "Linux"}]
            };
            BrowserDetect.init();
            alert(BrowserDetect.browser);
            if (BrowserDetect.browser == "Firefox") {
                window.location.href = '../ClassUpgrade/upgradeClass.aspx';
            }
            else if (BrowserDetect.browser == "Chrome") {
                window.location.href = '../ClassUpgrade/upgradeClass.aspx';
            }
            else if (BrowserDetect.browser == "Explorer") {
                window.location.href = '../ClassUpgrade/upgradeClass.aspx';
            }
            else if (BrowserDetect.browser == "Safari") {
                window.location.href = '../ClassUpgrade/upgradeClass.aspx';
            }
        });
    </script>--%>
</head>
<body style='background-color: Orange;'>
    <form id="form1" runat="server">
    <div style='margin-left: auto; margin-right: auto; width: 800px; height: 300px;'>
        <span style='font-size: 69px; color: White; position: relative; top: 258px;'>รอก่อนนะจ๊ะ
            ข้อสอบยังไม่มา </span>
        <img src="../Images/Activity/run.gif" style='position: relative; top: 264px;' />
    </div>
    </form>
</body>
</html>
