///<reference path="jquery-1.7.1-vsdoc.js" />

///คลาสช่วยจัดการตำแหน่ง
function Position() {
}
Position.documentHeight = function () {
    return parseInt($(document).height());
}
Position.documentWidth = function () {
    return parseInt($(document).width());
}
Position.screenHeight = function () {
    return parseInt($(window).height());
}
Position.screenWidth = function () {
    return parseInt($(window).width());
}
Position.elementHeight = function (el) {
    return parseInt($(el).height());
}
Position.elementWidth = function (el) {
    return parseInt($(el).width());
}

///คลาสช่วยตั้งค่า Controls ต่างๆ
function Controls() {
}
Controls.setOverFlowAutoHidden = function (control) {
    $(control).css("overflow-y", "hidden");
    $(control).mouseenter(function () {
        $(this).css("overflow-y", "auto");
        $(this).css("overflow-x", "auto");
    }).mouseleave(function () {
        $(this).css("overflow-y", "hidden");
        $(this).css("overflow-x", "hidden");
    })
}
