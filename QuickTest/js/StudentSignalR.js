var GlobalHref;
var stdDeviceId;
if (IsPracticeFromComputer == 'False') {    
    var SignalRCheck = $.connection.hubSignalR;
    //ขารับ
    SignalRCheck.client.send = function (message) {       
        if (message == 'Reload') {
            GetNextAction();
        }
    };
    $.connection.hub.start().done(function () {
        SignalRCheck.server.addToGroup(GroupName);
    });
}

function ResolveUrl(url) {
    if (url.indexOf("~/") == 0) {
        url = baseUrl + url.substring(2);
    }
    return url;
}
function GetNextAction() {
    $.ajax({ type: "POST",
        url: ResolveUrl("~/DroidPad/StudentAction.aspx/SendToGetNextAction"),
        data: "{ DeviceUniqueID: '" + DeviceId + "',IsFirstTime:'NoValue',IsTeacher:false}",
        contentType: "application/json; charset=utf-8", dataType: "json",
        success: function (msg) {
            //ถ้าค่าที่ Return กลับมาไม่เป็นค่าว่างให้ ReLoad หน้าใหม่
            var objJson = jQuery.parseJSON(msg.d);            
            if (objJson.Param.NextURL !== '') {
                var joinQuiz = ResolveUrl('~/Activity/ActivityPage_Pad.aspx?DeviceUniqueID=' + DeviceId);
                stdDeviceId = DeviceId;
                dialogInterrupt(joinQuiz);
            }
        },
        error: function myfunction(request, status) {
            alert('บางอย่างผิดพลาดโปรดลองอีกครั้ง')
        }
    });
}
function dialogInterrupt(href) {
    GlobalHref = href;
    $('#dialog').dialog({
        autoOpen: open,
        buttons: {
            'ตกลง': function () {
                //window.location = ResolveUrl(href);
            }
        },
        draggable: false,
        resizable: false,
        modal: true
    });
    if ($('.ui-button').length != 0) {
        $('.ui-button').each(function () {
            new FastButton(this, OKClickDialog);
        });
    }
    setTimeout(OKClickDialog, 30000);
}

function OKClickDialog(e) {
    //var obj = e.target;
    $.ajax({
        type: "POST",
        url: ResolveUrl("~/DroidPad/StudentAction.aspx/CheckUnActiveQuiz"),
        data: "{ DeviceUniqueID: '" + DeviceId + "'}",
        contentType: "application/json; charset=utf-8", dataType: "json",
        success: function (msg) {
            var ReturnData = jQuery.parseJSON(msg.d);
            var IsActiveQuiz = ReturnData.IsActiveQuiz;
            if (IsActiveQuiz == 'True') {
                window.location = ResolveUrl(GlobalHref)
            } else {
                $('#dialog').dialog('close');
            }
        },
        error: function myfunction(request, status) {
            alert('บางอย่างผิดพลาดโปรดลองอีกครั้ง')
        }
    });
}