function InjectionHover(selector, bouncePixel, isUsePositionRelative) {    
    if (arguments.length == 3) {
        isUsePositionRelative = isUsePositionRelative;
    }
    else {
        isUsePositionRelative = true;
    }
    var topValue = parseInt($(selector).css('top'));
    var currentPosition = $(selector).css('position');
    var offsetTop;
    var currentTop;
    if (isNaN(topValue)) {
        currentTop = 0;
        offsetTop = currentTop - bouncePixel;
    }
    else {
        Flag = false;
        currentTop = topValue;
        if (currentTop > 0) {
            offsetTop = currentTop - bouncePixel;
        }
        else {
            offsetTop = currentTop + (-bouncePixel);
        }
    }    
    $(selector).hover(function () {
        if (isUsePositionRelative == true) {
            $(this).css('position', 'relative');
        }
        $(this).stop().animate({ top: offsetTop }, 200, "easeOutBounce");

    }, function () {
        $(this).stop().animate({ top: currentTop }, 200, "easeOutBounce", function () {
            if (isUsePositionRelative == true) {
                $(this).css('position', 'initial');
            }
        });
    });
}

function InjectHoverShake(selector) {
    $(selector).hover(function () {
        if (!$(this).hasClass('HoverShake')) {
            $(this).addClass('HoverShake');
            $(this).stop().effect('shake', { times: 1, distance: 4 }, 200);
        }
    }, function () {
        $(this).removeClass('HoverShake');
    });
}

function FadePageTransition() {
    //$("html").css("display", "none");
    //$("html").fadeIn(300);
}


function FadePageTransitionOut() {
    //$("html").fadeOut(200);
}

//Sound
function PlaySound(SoundPath) {
    var audio = document.getElementById('audioOpen');
    if (typeof (audio) == 'undefined' || audio == null) {
        $('body').append('<audio id="audioOpen" style="display:none;" ><source src="' + SoundPath + '" /></audio>')
        audio = document.getElementById('audioOpen');
    }
    audio.play();
}


// PlaySound('<%=ResolveUrl("~")%>images/SingleTick.wav',2000);
function PlaySoundRepeat(SoundPath, duration) {
    var audio = document.getElementById('audioOpen');
    if (typeof (audio) == 'undefined' || audio == null) {
        $('body').append('<audio id="audioOpen" style="display:none;" ><source src="' + SoundPath + '" /></audio>')
        audio = document.getElementById('audioOpen');
    }
    var MyTime = setInterval(function () {
        audio.play();
    }, 100);
    setTimeout(function () {
        myStopFunction(MyTime)
    }, duration)
}

function myStopFunction(myVar) {
    clearInterval(myVar);
}