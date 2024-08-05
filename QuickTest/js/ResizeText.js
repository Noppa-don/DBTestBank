if (FontSize == null) {
    FontSize = 0;
}
var c, f = FontSize, HaveMenu = true;
//window.onload = function () {
$(document).ready(function () {
    var thisPage = window.location.pathname.split('/').pop().toLowerCase();
    if (thisPage != 'dashboardstudentpage.aspx' && thisPage != 'dashboardquizpage.aspx' && thisPage != 'dashboardhomeworkpage.aspx' && thisPage != 'dashboardpracticepage.aspx' && thisPage != 'dashboardprinttestsetpage.aspx' && thisPage != 'dashboardsetuppage.aspx') {
        //document.getElementById('ChangeFontSize').remove();
        var a = document.getElementById('ChangeFontSize');
        if (a != null) {
            a.style.display = "none";
        }
        HaveMenu = false;
    }
    c = document.getElementById("site_content");
    if (c !== null) {
        resizeText(FontSize);
    }
    
});
//}
function resizeText(multiplier) {
    //    if (c.style.fontSize == "" || c.style.fontSize == null) {
    //        c.style.fontSize = "1.0em";
    //    }
    c.style.fontSize = "1.0em";
    c.style.fontSize = parseFloat(c.style.fontSize) + (multiplier * 0.2) + "em";
    if (HaveMenu) {
        btnFontSizeStatus();
    }
}
function SetFontSize(FontSize, e) {
    if (e.preventDefault) { e.preventDefault(); }
    if (navigator.userAgent.indexOf('Firefox') <= 0) {
        event.returnValue = false;
    }    
    if (f != FontSize) {
        f = FontSize;
        resizeText(FontSize);
        $.ajax({ type: "POST",
            url: ResolveUrl("~/WebServices/TestsetService.asmx/SetFontSize"),
            async:false,
            data: "{ FontSize: '" + FontSize + "'}",
            contentType: "application/json; charset=utf-8", dataType: "json",
            success: function (msg) {
                if (msg.d != 0) {
                }
            },
            error: function myfunction(request, status) {
                //alert('บางอย่างผิดพลาดโปรดลองอีกครั้ง')
            }
        });
    }
}
function ResolveUrl(url) {
    if (url.indexOf("~/") == 0) {
        url = baseUrl + url.substring(2);
    }
    return url;
}
function btnFontSizeStatus() {
    var arrBtn = [0, 1, 2];
    for (var i = 0; i < arrBtn.length; i++) {
        var elem = document.getElementById('btnFontSize' + arrBtn[i]);        
        if (i == f) {
            elem.setAttribute('src', GetImagesStatus(i, true));
        } else {
            elem.setAttribute('src', GetImagesStatus(i, false));
        }
    }
}
function GetImagesStatus(i, IsActive) {
    var img;
    if (i == 0 && IsActive) {
        img = '../images/ResizeText/btnFontSmallOff.png'
    } else if (i == 0 && !IsActive) {
        img = '../Images/ResizeText/btnFontSmallOn.png'
    } else if (i == 1 && IsActive) {
        img = '../images/ResizeText/btnFontMediumOff.png'
    } else if (i == 1 && !IsActive) {
        img = '../Images/ResizeText/btnFontMediumOn.png'
    } else if (i == 2 && IsActive) {
        img = '../images/ResizeText/btnFontLargeOff.png'
    } else if (i == 2 && !IsActive) {
        img = '../Images/ResizeText/btnFontLargeOn.png'
    } 
    return img;
}