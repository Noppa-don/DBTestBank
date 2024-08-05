
$(function () {
    $('.BlockBG').live('click',function(){
        $('.Frame').remove();
        $('.Close').remove();
        $(this).fadeOut(400, function () { $(this).remove(); });
    });

    $('.Close').live('click', function () {
        $('.Frame').remove();
        $(this).remove();
        $('.BlockBG').fadeOut(400, function () { $(this).remove(); });
    });
});

function LightboxKN(url) {
    var $MD = $('<div />').appendTo('form');
    $MD.addClass('BlockBG');

    var $IF = $('<iframe />').appendTo($MD);
    $IF.addClass('Frame');
    $IF.attr('src', url);
    var OffsetLeft = ($(window).width() - $IF.width()) / 2;
    var OffsetTop = ($(window).height() - $IF.height()) / 2;
    $IF.css('top', OffsetTop );
    $IF.css('left', OffsetLeft);

    var $Close = $('<div />').appendTo('form');
    $Close.addClass('Close');
    var CloseOffsetLeft = (($(window).width() - $IF.width()) / 2) + $IF.width() - 20;
    var CloseOffsetTop = ($(window).height() - $IF.height()) / 2 -24;
    $Close.css('top', CloseOffsetTop);
    $Close.css('left', CloseOffsetLeft);
}