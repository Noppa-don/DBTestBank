var pageLocation = window.location.href.split('?')[0].toLowerCase();
var bgName = "";
if (pageLocation.indexOf("loginpage") > -1) {
    bgName = "login";
} else if (pageLocation.indexOf("mainpractice") > -1) {
    bgName = "mainpractice";
} else if (pageLocation.indexOf("chooseclass") > -1) {
    bgName = "chooseclass";
} else if (pageLocation.indexOf("choosesubject") > -1) {
    bgName = "choosesubject";
} else if (pageLocation.indexOf("choosequestionset") > -1) {
    bgName = "choosequestionset";
}else if (pageLocation.indexOf("dashboardstudentpage") > -1) {
    bgName = 1;
} else if (pageLocation.indexOf("dashboardquizpage") > -1) {
    bgName = 2;
} else if (pageLocation.indexOf("dashboardhomeworkpage") > -1) {
    bgName = 3;
} else if (pageLocation.indexOf("dashboardpracticepage") > -1) {
    bgName = 4;
} else if (pageLocation.indexOf("dashboardprinttestsetpage") > -1) {
    bgName = 1;
} else if (pageLocation.indexOf("dashboardsetuppage") > -1) {
    bgName = 3;
} else {
    bgName = "login";
}
var w = window.screen.availWidth;
var h = window.screen.availHeight;
var bgresolution;
if (w <= 800) {
    bgresolution = "res800600";
} else if (w > 800 && w <= 1024) {
    bgresolution = "res1024768";
} else if (w > 1024 && w <= 1280) {
    bgresolution = "res1280800";
} else if (w > 1280 && w <= 1366) {
    bgresolution = "res1366768";
} else if (w > 1366 && w <= 1600) {
    bgresolution = "res1600900";
} else if (w > 1600 && w <= 1920) {
    bgresolution = "res19201080";
} else if (w > 1920 && w <= 2048) {
    bgresolution = "res20481536";
} else {
    bgresolution = "res40003000";
}
document.body.style.background = "url('../images/bg/" + bgresolution + "/" + bgName + ".png')";
//$('body').css('background', "url('../images/bg/" + bgresolution + "/" + bgName + ".png')");