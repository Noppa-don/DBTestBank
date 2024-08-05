function ResolveUrl(baseURL,url) {
    if (url.indexOf("~/") == 0) {
        url = baseURL + url.substring(2);
    }
    return url;
}

function SetStatusSession(StatusType,baseURL) {
    $.ajax({
        type: "POST",
        url: ResolveUrl(baseURL, "~/WebServices/UserPwdService.asmx/SetStatusSession"),
        data: "{ StatusType :  '" + StatusType + "'}",
        contentType: "application/json; charset=utf-8", dataType: "json",
        success: function (msg) {
            console.log("SetStatus : " + StatusType);
        },
        error: function myfunction(request, status) { }
    });
}

function CheckIsNowDevice(DeviceId, Token, baseURL, UserId) {

        $.ajax({
            type: "POST",
            url: ResolveUrl(baseURL, "~/WebServices/UserPwdService.asmx/CheckIsNowDevice"),
            data: "{ DeviceId :  '" + DeviceId + "', UserId :  '" + UserId + "'}",
            contentType: "application/json; charset=utf-8", dataType: "json",
            success: function (msg) {   
                if (msg.d == "False") {
                    console.log("Delete FastButton");
                    DeleteFastButton();
                    CallConfirmDialog('มีเครื่องอื่นใช้งานอยู่ ต้องการสลับมาใช้เครื่องนี้มั้ยคะ', DeviceId, Token, baseURL);
                } else {
                    SetStatusSession("3", baseURL)
                    NewFastButton();
                }
            },
            error: function myfunction(request, status) {
                
            }
        });
}

function CallConfirmDialog(txt, DeviceId, Token, baseURL) {
    var $d = $('.dialogSession');
    var btnArray = [
    {
        text: "ไม่สลับ",
        id: "NotChange",
        click: function () {
            $d.dialog('close');
        }
    },
    {
        text: "สลับเลย",
        id: "OKChange",
        click: function () {
            $.ajax({
                type: "POST",
                url: ResolveUrl(baseURL, "~/WebServices/UserPwdService.asmx/SetNowDeviceToRedis"),
                async: false,
                data: "{ DeviceId :  '" + DeviceId + "'}",
                contentType: "application/json; charset=utf-8", dataType: "json",
                success: function (msg) {
                    window.location = ResolveUrl(baseURL, "~/practicemode_pad/ChooseTestsetMaxOnet.aspx?deviceUniqueId=" + DeviceId + "&token=" + Token);
                },
                error: function myfunction(request, status) {}
            });
        }
    }
    ];
    $d.dialog({ buttons: btnArray, draggable: false, resizable: false, modal: true }).dialog('option', 'title', txt);
}