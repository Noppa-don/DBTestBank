
$(function () {
    InitialIconFavorite();
});

function InitialIconFavorite() {
    //ปุ่มดาว Favorite
    //$('.imgStar').each(function () {
    //    new FastButton(this, TriggerImgStarClick);
    //});

    // qtip สำหรับเลือก Icon Favorite
    $('.favoriteStudent').each(function () {
        var studentId = $(this).attr('studentid');
        var contentFavorite = getContentFavorite(studentId);
        // กำหนด qtip 
        $(this).qtip({
            content: contentFavorite,
            //show: { ready: true },
            show: { event: 'mouseover,taphold' },
            style: {
                width: 225, padding: 0, background: '#dd5116', color: 'white', textAlign: 'center', border: { width: 4, radius: 5, color: '#b92e7f' }, tip: 'topMiddle', name: 'dark', 'font-weight': 'bold', 'font-size': '17px', 'line-height': '2em'
            }, hide: false,
            position: { corner: { tooltip: 'topMiddle', target: 'bottomMiddle' } },
            hide: { when: { event: 'mouseout' }, fixed: true }
        });
    });
    $('.divSocial > div').live('click', function () {
        $(this).parent().children().removeClass('active');
        $(this).addClass('active');
        var studentId = $(this).parent('.divSocial').attr('stuid');
        var favoriteCode = parseInt($(this).attr('id').replace("favorite", ""));
        var favoriteScore = $(this).attr('favoriteScore');
        var content = getFavoriteCodeStudent(favoriteCode, favoriteScore);
        $(this).parent().next().html(content);
    });

    $('.statusFavorite').live('click', function () {
        $(this).parent().children().removeClass('active');
        $(this).addClass('active');

        var favoriteScore = $(this).attr('favoriteScore');

        var parent = $(this).parent().parent().parent().prev().find('div.active');
        var favoriteCode = parseInt($(parent).attr('id').replace("favorite", ""));
        $(parent).attr('favoriteScore', favoriteScore);

        var studentId = $(this).parent().parent().parent().prev().attr('stuid');

        var imgStudentFavorite = setStudentFavorite(studentId, favoriteCode, favoriteScore);

        var imgPath = getIconFavorite(favoriteCode, favoriteScore);
        $(parent).css('background-image', "url(" + imgPath + ")");

        var divfavoritetab = $('div[studentid="' + studentId + '"]');
        $(divfavoritetab).html(imgStudentFavorite);

        if ($(divfavoritetab).children().length == 1) {
            if ($(divfavoritetab).children().hasClass("notfavorite")) {
                if ($('#MainDiv').length > 0) {
                    $(divfavoritetab).parent().parent().remove();
                    //alert($('#MainDiv').children().length);
                    if ($('#MainDiv').children().children().length == 0) {
                        $('#DivHaveData').remove();
                        var txt = $("<div id='DivNoData' class='ForMainDivNoData' ><div id='DivShowInfo' class='ForDivShowInFo' ><img src='../Images/star.gif' style='width:120px;position:absolute;top:25px;left:30px;' /><span style='font-size: 40px; font-weight: bold; position: relative; top: 25px;'>แสดงนักเรียนที่ติดดาวไว้</span><br /><span style='font-size: 30px; position: relative; top: 30px;'>(ใช้ติดตามเด็กที่ต้องการดูแลเป็นพิเศษ)</span><br /><span class='hint' style='top:30px;position: relative;'>ติดดาวเด็กที่ต้องการดูแลเป็นพิเศษได้ที่ <a href='../Student/StudentListPage.aspx' class='hint'>หน้ารายชื่อนักเรียน</a> ค่ะ</span></div></div>");
                        $('#MainDiv').append(txt).addClass('nodata');
                    }
                }
            }
        }

    });
}

function ResolveUrl(url) {
    if (url.indexOf("~/") == 0) {
        url = baseUrl + url.substring(2);
    }
    return url;
}

function getFavoriteCodeStudent(favoriteCode, favoriteScore) {
    var favoriteScoreArr = (favoriteCode == 0) ? [-1, 0, 1] : [-2, -1, 0, 1, 2];
    var divFavoriteScore = '';
    for (var i = 0; i < favoriteScoreArr.length; i++) {
        var thisFavoriteScore = favoriteScoreArr[i] + 3;
        var active = (favoriteScore == thisFavoriteScore) ? 'active' : '';
        divFavoriteScore += '<div class="statusFavorite ' + active + '" favoriteScore="' + (favoriteScoreArr[i] + 3) + '"  style="background-image:url(' + getIconFavoriteScore(favoriteCode, thisFavoriteScore) + ');"></div>';
    }

    var txts = getFavoriteCodeText(favoriteCode);
    var content = '<hr class="underLine"></hr><div><span>' + txts[0] + '</span><div>' + divFavoriteScore + ' </div><span>' + txts[1] + '</span></div>';
    return content;
}

function getFavoriteCodeText(favoriteCode) {
    favoriteCode = parseInt(favoriteCode);
    var txt;
    switch (favoriteCode) {
        case 0: txt = ["Favorite", "ติดตามนักเรียน"]; break;
        case 1: txt = ["IQ", "สติปัญญา"]; break;
        case 2: txt = ["EQ", "อารมณ์"]; break;
        case 3: txt = ["AQ", "การแก้ไขปัญหา"]; break;
        case 4: txt = ["MQ", "คุณธรรม จริยธรรม"]; break;
        case 5: txt = ["HQ", "พลานามัย"]; break;
        case 6: txt = ["PQ", "การเล่น"]; break;
        case 7: txt = ["SQ", "สังคม"]; break;
        case 8: txt = ["OQ", "การมองโลกในแง่ดี"]; break;
        case 9: txt = ["UQ", "ริเริ่มสร้างสรรค์"]; break;
        default: txt = ["ERROR", "หาไม่เจอ"];
    }
    return txt;
}

function changeImgParent() {

}

function setFavoriteCodeStudent(studentId, favariteCode) {
    $.ajax({
        type: "POST",
        url: "../WebServices/StudentService.asmx/SetFavoriteCodeStudent",
        data: "{ studentId : '" + studentId + "', favariteCode: '" + favariteCode + "'}",
        async: false,
        contentType: "application/json; charset=utf-8", dataType: "json",
        success: function (data) {
            if (data.d == true) {
                var iconFav = getIconFavorite(favariteCode);
                $('img[stid="' + studentId + '"]').attr('src', iconFav);
                if (favariteCode == 0) {
                    $('img[stid="' + studentId + '"]').removeClass('ForYellowStar');
                    $('img[stid="' + studentId + '"]').addClass('ForGrayStar');
                } else {
                    $('img[stid="' + studentId + '"]').removeClass('ForGrayStar');
                    $('img[stid="' + studentId + '"]').addClass('ForYellowStar');
                }
            }
        },
        error: function (xhr, status, text) {
            //var response = $.parseJSON(xhr.responseText);
            alert(status);
        }
    });
}
function TriggerImgStarClick(e) {
    if ($(e.target).is('img')) {
        var obj = e.target;
        var studentId = $(obj).attr('StId');
        if ($(obj).hasClass('ForGrayStar')) {
            setFavoriteCodeStudent(studentId, "1");
        }
        else {
            setFavoriteCodeStudent(studentId, 0);
        }
    }
}
function getContentFavorite(studentId) {
    var c = '<div class="mainDivSocial"><div class="divSocial" stuid="' + studentId + '" style="height:85px;">';
    var data = getStudentFavorite(studentId); //console.log(data);
    if (data != "") {
        c += data;
    } else if (data == "") {
        //alert(data);
        for (var i = 0; i < 10; i++) {
            c += createDivIcon(i);
        }
    }
    c += '</div><div></div></div>';
    return c;
}

function getStudentFavorite(studentId) {
    var value;
    $.ajax({
        type: "POST",
        url: "../WebServices/StudentService.asmx/getStudentFavorite",
        data: "{ studentId : '" + studentId + "'}",
        async: false,
        contentType: "application/json; charset=utf-8", dataType: "json",
        success: function (data) {
            //alert(data.d);
            value = data.d;

        },
        error: function (xhr, status, text) {
            alert(status);
        }
    });
    return value;
}

function setStudentFavorite(studentId, favoriteCode, favoriteScore) {
    var value;
    $.ajax({
        type: "POST",
        url: "../WebServices/StudentService.asmx/setStudentFavorite",
        data: "{ studentId : '" + studentId + "',favoriteCode : '" + favoriteCode + "',favoriteScore : '" + favoriteScore + "'}",
        async: false,
        contentType: "application/json; charset=utf-8", dataType: "json",
        success: function (data) {
            value = data.d;
        },
        error: function (xhr, status, text) {
            alert(status);
        }
    });
    return value;
}

function createDivIcon(iconType) {
    var imgPath = getIconFavorite(iconType, 3);
    return '<div id="favorite' + iconType + '" style="background-image:url(' + imgPath + ');" favoriteScore="3"></div>';
}

function getIconFavorite(favoriteCode, favoriteScore) {
    var imgPath;
    switch (favoriteCode) {
        case 0: imgPath = "../images/dashboard/student/f" + favoriteScore + ".png"; break;
        case 1: imgPath = "../images/dashboard/student/favorite/iq/iq" + favoriteScore + ".png"; break;
        case 2: imgPath = "../images/dashboard/student/favorite/eq/eq" + favoriteScore + ".png"; break;
        case 3: imgPath = "../images/dashboard/student/favorite/aq/aq" + favoriteScore + ".png"; break;
        case 4: imgPath = "../images/dashboard/student/favorite/mq/mq" + favoriteScore + ".png"; break;
        case 5: imgPath = "../images/dashboard/student/favorite/hq/hq" + favoriteScore + ".png"; break;
        case 6: imgPath = "../images/dashboard/student/favorite/pq/pq" + favoriteScore + ".png"; break;
        case 7: imgPath = "../images/dashboard/student/favorite/sq/sq" + favoriteScore + ".png"; break;
        case 8: imgPath = "../images/dashboard/student/favorite/oq/oq" + favoriteScore + ".png"; break;
        case 9: imgPath = "../images/dashboard/student/favorite/uq/uq" + favoriteScore + ".png"; break;
        default: imgPath = "";
    }
    return imgPath;
}

function getIconFavoriteScore(favoriteCode, favoriteScore) {
    var imgPath;
    switch (favoriteCode) {
        case 0: imgPath = "../images/dashboard/student/favorite" + favoriteScore + ".png"; break;
        case 1: imgPath = "../images/dashboard/student/favorite/iq/iqs" + favoriteScore + ".png"; break;
        case 2: imgPath = "../images/dashboard/student/favorite/eq/eqs" + favoriteScore + ".png"; break;
        case 3: imgPath = "../images/dashboard/student/favorite/aq/aqs" + favoriteScore + ".png"; break;
        case 4: imgPath = "../images/dashboard/student/favorite/mq/mqs" + favoriteScore + ".png"; break;
        case 5: imgPath = "../images/dashboard/student/favorite/hq/hqs" + favoriteScore + ".png"; break;
        case 6: imgPath = "../images/dashboard/student/favorite/pq/pqs" + favoriteScore + ".png"; break;
        case 7: imgPath = "../images/dashboard/student/favorite/sq/sqs" + favoriteScore + ".png"; break;
        case 8: imgPath = "../images/dashboard/student/favorite/oq/oqs" + favoriteScore + ".png"; break;
        case 9: imgPath = "../images/dashboard/student/favorite/uq/uqs" + favoriteScore + ".png"; break;
        default: imgPath = "";
    }
    return imgPath;
}