$(document).ready(function () {
    var ww = ($(window).width() < window.screen.width) ? $(window).width() : window.screen.width; //get proper width
    if (ww == 0) {
        setTimeout(function () {
            ww = ($(window).width() < window.screen.width) ? $(window).width() : window.screen.width; //get proper width
        }, 1200);
        if (ww == 0) { ww = 640 };
    }

    var iOS = /iPad|iPhone|iPod/.test(navigator.userAgent) && !window.MSStream;

    var pageLocation = window.location.href;
    var scale;
    if (pageLocation.toLowerCase().indexOf('activitypage_pad') != -1) {
        scale = ww / 942;
        if (iOS) {
            if (ww < 768) {
                scale = ww / 580;
            } else {
                scale = ww / 742;
            }
        } else {
            scale = ww / 942;
        }
    } else {
        if (iOS) {
            if (ww < 768) { // iphone 5,6
                scale = ww / 700;
            } else { //ipad mini
                scale = ww / 900;
            }
        } else {
            scale = ww / 1100;
        }
    }
    $('meta[name=viewport]').attr('content', 'width=device-width,user-scalable=yes,initial-scale=' + scale + ', maximum-scale=' + scale + ' minimum-scale=' + scale);
});
