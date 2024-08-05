

/// <reference path="jquery-1.7.1.js" />
/// <reference path="calculator.js" />
var btnStuAns = [];

$(function () {
    $('.ForToolsTop').click(getToolsForUse);
    $('.ForToolsLeft').click(getToolsForUse);    
    // ปุ่มปิด Tools
    $('.Aclosed').click(function () {
        $('span').removeClass('spanHilight');
        $(this).parent().hide();
    });
});

function getToolsForUse() {
    var exit; var viewreport; var forTools;
    if ($(this).is('.ForToolsTop')) {
        exit = $('#DivExit1'); viewreport = $('#DivViewReport1'); forTools = $('.ForToolsTop');
    }
    else {
        exit = $('#DivExit2'); viewreport = $('#DivViewReport2'); forTools = $('.ForToolsLeft');
    }

    if (($(this).hasClass('btnNote'))) {
        $('#tools_note').draggable({ cancel: '.content' }).show();
        var UserId = $('#hdUserId').val();
        getPanelNote(UserId);
        hideMenuSettingTools(exit, viewreport, forTools);
    }
    else if (($(this).hasClass('btnWordBook'))) {
        $('#tools_wordbook').draggable({ cancel: '.notDraggable' }).show();
        var QuestionId = $('#hdQuestionId').val();
        getPanelAlphabet(QuestionId);
        hideMenuSettingTools(exit, viewreport, forTools);
    }
    else if (($(this).hasClass('btnCalculator'))) {
        $('#tools_calculator').draggable({ cancel: '.btnCalculatorDiv' }).show();
        hideMenuSettingTools(exit, viewreport, forTools);
    }
    else if (($(this).hasClass('btnDictionary'))) {        
        if ($(this).is('.ForToolsTop')) {
            var tooltip = 'right center'; var tooltipAt = 'left center';
        } else {
            var tooltip = 'left center'; var tooltipAt = 'right center';
        }
        var elm = $(this).is('.ForToolsTop') ? $('#btnWordBookTop') : $('#btnWordBookSide');
        var y = $(this).is('.ForToolsTop') ? -20 : 20;
        // เปิด/ปิด แปลศัพท์
        if ($(this).hasClass('DictOn')) {            
            $('.btnDictionary').removeClass('DictOn').addClass('DictOff');
            turnOffDict();
            $(elm).qtip('destroy');
        } else {            
            $('.btnDictionary').removeClass('DictOff').addClass('DictOn');
            turnOnDict();

            //$(elm).qtip('destroy');
            $(elm).qtip({
                content: 'กดที่คำศัพท์<br/> แล้วจะเห็นคำแปลค่ะ',
                show: { ready: true },
                style: {
                    width: 185, padding: 0, background: '#F68500', color: 'white', textAlign: 'center', border: { width: 7, radius: 5, color: '#F68500' }, tip: 'leftMiddle', name: 'dark', 'font-weight': 'bold', 'font-size': '17px', 'line-height': '2em'
                }, hide: false,
                position: { corner: { tooltip: 'leftMiddle', target: 'middleRight' }, adjust: { x: -10, y: y } },
                fixed: false
            });

            setTimeout(function () { $(elm).qtip('destroy'); hideMenuSettingTools(exit, viewreport, forTools); }, 3000);
        }
    }
    else if (($(this).hasClass('btnProtractor'))) {
        $('#tools_protractor').draggable({ cancel: '.btnRotate' }).show();
        hideMenuSettingTools(exit, viewreport, forTools);
    }
}
function hideMenuSettingTools(exit, viewreport, forTools) {
    $(exit).hide(300);
    $(viewreport).hide(300);
    $(forTools).hide(300);
}

// WORDBOOK 
// js for control on wordbook
$(function () {
    $('.Alphabet').live('click', function () {
        var alphabet = $(this).html();
        var QuestionId = $('#hdQuestionId').val();
        getPanelWordBook(QuestionId, alphabet, 0);
    });
    $('.backAlphabet').live('click touchstart', function () {
        var QuestionId = $('#hdQuestionId').val();
        getPanelAlphabet(QuestionId);
    });
    $('.nextAlphabet').live('click', function () {
        var alphabet = $(this).text();
        var QuestionId = $('#hdQuestionId').val();
        getPanelWordBook(QuestionId, alphabet, 1);
    });
});
function getPanelWordBook(QuestionId, Alphabet, ChangeAlphabet) {
    console.log('url' + ResolveUrl("ToolsActivity/WordBookAndNote.aspx/createWordBook"));
    $.ajax({ type: "POST",
        //url: "<%=ResolveUrl("~")%>WebForm5.aspx/createWordBook", 
        url: ResolveUrl("ToolsActivity/WordBookAndNote.aspx/createWordBook"),
        data: "{QuestionId : '" + QuestionId + "', Alphabet : '" + Alphabet + "', ChangeAlphabet : '" + ChangeAlphabet + "' }",
        async: false,
        contentType: "application/json; charset=utf-8", dataType: "json",
        success: function (data) {
            $('#wordbookMain').html(data.d);
        },
        error: function myfunction(request, status) {
            alert(status);
        }
    });
}
function getPanelAlphabet(QuestionId) {
    $.ajax({ type: "POST",
        //url: "<%=ResolveUrl("~")%>WebForm5.aspx/createWordBook", 
        url: ResolveUrl("ToolsActivity/WordBookAndNote.aspx/createAlphabet"),
        data: "{ QuestionId : '" + QuestionId + "'}",
        async: false,
        contentType: "application/json; charset=utf-8", dataType: "json",
        success: function (data) {
            $('#wordbookMain').html(data.d);
        },
        error: function myfunction(request, status) {
            alert(status);
        }
    });
}

function ResolveUrl(url) {
    if (url.indexOf("~/") == 0) {
        url = baseUrl + url.substring(2);
    }
    return url;
}

// Note
// js for control note
$(function () {
    $('#myNote').find('textarea').live('focusout', function () {
        var html = $(this).val();        
        var UserId = $('#hdUserId').val();
        saveNote(html, UserId);
    });

});
function getPanelNote(UserId) {
    $.ajax({ type: "POST",
        url: "ToolsActivity/WordBookAndNote.aspx/createNote",
        data: "{ UserId : '" + UserId + "'}",
        async: false,
        contentType: "application/json; charset=utf-8", dataType: "json",
        success: function (data) {
            //$('.noteTabs').html(data.d);
            $('#myNote').html(data.d);
        },
        error: function myfunction(request, status) {
        }
    });
}
function saveNote(myNote, UserId) {
    $.ajax({ type: "POST",
        url: "ToolsActivity/WordBookAndNote.aspx/saveMyNote",
        data: "{ myNote : '" + myNote + "',UserId : '" + UserId + "'}",
        async: false,
        contentType: "application/json; charset=utf-8", dataType: "json",
        success: function (data) {
        },
        error: function myfunction(request, status) {
            alert('save Note ไม่ลง');
        }
    });
}
// Protractor
// js for control Protractor
$(function () {
    var keepVal_IE = '';
    //var isDragging = false;
    $('.btnRotate').mousedown(function () {        
        var t = 0; var divImg = document.getElementById('divImg');
        var curAngle = parseInt($('#divImg').getRotateAngle()); var operator;
        if (isNaN(curAngle)) {
            curAngle = 0;
        }
        if ($(this).is('#btnRotateL')) {
            operator = '-';
        } else {
            operator = '+';
        }
        times = setInterval(function () {
            if (t >= 0 && t <= 5) {
                t = t + 1;
            }
            else if (t >= 6 && t <= 10) {
                t = t + 5;
            }
            else {
                t = t + 10;
            }
            val = curAngle + operator + t;
            if ($.browser.msie) {
                if (keepVal_IE == '') {
                    keepVal_IE = eval(val);
                } else {
                    keepVal_IE = keepVal_IE + eval(val);
                }
                var mrotate = "rotate(" + eval(keepVal_IE) + "deg)";
                cssSandpaper.setTransform(divImg, mrotate);
            } else {
                $('#divImg').rotate({ angle: eval(val), easing: $.easing.easeInOutElastic });
            }
        }, 80);
        $(window).mousemove(function () {
            clearInterval(times);
        });
    }).mouseup(function () {      
        clearInterval(times);
    });
   
    
});
// Tools On Tablet
$(function () {

    //$('#PanelTools').click(function () {
    //    if ($(this).hasClass('OpenTools')) {
    //        $(this).removeClass('OpenTools');
    //        $('#ToolsOnTablet').hide();
    //    } else {
    //        $(this).addClass('OpenTools');
    //        $('#ToolsOnTablet').show();
    //    }
    //});

    //PanelTool เมือ่กเปิดเครืองมือ
    if ($('#PanelTools').length != 0) {
        new FastButton(document.getElementById('PanelTools'), TriggerToolClick);
    }
    
    //เมื่อกด
    if ($('.ForToolsTablet').length != 0) {
        $('.ForToolsTablet').each(function () {
            new FastButton(this, getToolsForUseFromTablet);
        });
    }
    
    //$('.ForToolsTablet').live('click', getToolsForUseFromTablet);

    var sta = $('#hdStatusDict').val();
    if (sta == "On") {       
        //turnOnDict();
        $('.btnDictionary').removeClass('DictOff').addClass('DictOn');
    }

    

});




function TriggerToolClick(e) {
    chooseSound.play();
    var obj = e.srcElement;
    if ($(obj).hasClass('OpenTools')) {
        $(obj).removeClass('OpenTools');
        $('#ToolsOnTablet').hide();
    } else {
        $(obj).addClass('OpenTools');
        $('#ToolsOnTablet').show();
    }
}

function getToolsForUseFromTablet(e) {
    chooseSound.play();
    var obj = e.srcElement;
    if (($(obj).hasClass('btnNote'))) {
        //$('#tools_note').draggable({ cancel: '.content' }).show();
        $('#tools_note').show();
        var UserId = $('#hdUserId').val();
        getPanelNote(UserId);
        hideMenuToolsTablet();
    }
    else if (($(obj).hasClass('btnWordBook'))) {
        //$('#tools_wordbook').draggable({ cancel: '.notDraggable' }).show();
        $('#tools_wordbook').show();
        var QuestionId = $('#hdQuestionId').val();
        getPanelAlphabet(QuestionId);
        hideMenuToolsTablet();
    }
    else if (($(obj).hasClass('btnCalculator'))) {
        $('#tools_calculator').draggable({ handle: '#txtCalculator', cancel: '.btnCalculatorDiv' }).show();
        hideMenuToolsTablet();
    }
    else if (($(obj).hasClass('btnDictionary'))) {       
        if ($(obj).hasClass('DictOn')) {
            $(obj).removeClass('DictOn').addClass('DictOff');
            turnOffDict();
            //hideMenuToolsTablet();
        } else {
            $(obj).removeClass('DictOff').addClass('DictOn');
            turnOnDict();

            var tooltip = 'right center'; var tooltipAt = 'left center'; 
            $('#btnWordBookTop').qtip({
                content: 'กดที่คำศัพท์<br/> แล้วจะเห็นคำแปลค่ะ',
                show: { ready: true },
                style: {
                    width: 185, padding: 0, background: '#F68500', color: 'white', textAlign: 'center', border: { width: 7, radius: 5, color: '#F68500' }, tip: 'rightMiddle', name: 'dark', 'font-weight': 'bold', 'font-size': '17px', 'line-height': '2em'
                }, hide: false,
                position: { corner: { tooltip: 'rightMiddle', target: 'middleRight' }, adjust: { x: -10, y: -20 } },
                fixed: false
            });

            setTimeout(function () { $('#btnWordBookTop').qtip('destroy'); }, 3000);            
        }
    }
    else if (($(obj).hasClass('btnProtractor'))) {
        $('#tools_protractor').draggable({ cancel: '.btnRotate' }).show();
        hideMenuToolsTablet();
    }
}


//function getToolsForUseFromTablet() {   

    //if (($(this).hasClass('btnNote'))) {
    //    //$('#tools_note').draggable({ cancel: '.content' }).show();
    //    $('#tools_note').show();
    //    var UserId = $('#hdUserId').val();
    //    getPanelNote(UserId);
    //    hideMenuToolsTablet();
    //}
    //else if (($(this).hasClass('btnWordBook'))) {
    //    //$('#tools_wordbook').draggable({ cancel: '.notDraggable' }).show();
    //    $('#tools_wordbook').show();
    //    var QuestionId = $('#hdQuestionId').val();
    //    getPanelAlphabet(QuestionId);
    //    hideMenuToolsTablet();
    //}
    //else if (($(this).hasClass('btnCalculator'))) {
    //    $('#tools_calculator').draggable({ cancel: '.btnCalculatorDiv' }).show();
    //    hideMenuToolsTablet();
    //}
    //else if (($(this).hasClass('btnDictionary'))) {
    //    if ($(this).hasClass('DictOn')) {
    //        $(this).removeClass('DictOn').addClass('DictOff');
    //        turnOffDict();
    //        hideMenuToolsTablet();
    //    } else {
    //        $(this).removeClass('DictOff').addClass('DictOn');
    //        turnOnDict();

    //        var tooltip = 'right center'; var tooltipAt = 'left center';
    //        $(this).qtip({
    //            content: { text: 'กดที่คำศัพท์<br/> แล้วจะเห็นคำแปลค่ะ' },
    //            show: { ready: true, event: false },
    //            style: { classes: 'ui-tooltip-cloneSetting', width: 185, height: 65 },
    //            position: { my: tooltip.toString(), at: tooltipAt.toString(), target: $(this) },
    //            hide: false
    //        });
    //        setTimeout(function () { $('.ui-tooltip-cloneSetting').remove(); hideMenuToolsTablet(); }, 3000);
    //    }      
    //}
    //else if (($(this).hasClass('btnProtractor'))) {
    //    $('#tools_protractor').draggable({ cancel: '.btnRotate' }).show();
    //    hideMenuToolsTablet();
    //}
//}
function hideMenuToolsTablet() {
    $('#ToolsOnTablet').hide();
    $('#PanelTools').removeClass('OpenTools');
}
function turnOnDict() {
    //var IsEng = $('#hdIsGroupEng').val();
    //if (IsEng == "True") {
    //    var mainQuestion = $('#mainQuestion');
    //    //mainQuestion.html(function (index, oldHtml) {
    //    //    return oldHtml.replace(/\b(\w+?)\b(?!([^<]+)?>)\b(?!([^&]+)?;)/g, '<span class="wordsDict">$1</span>')
    //    //});
    //    //console.log($(mainQuestion).html());
    //    //console.log('text = ' + $(mainQuestion).text());
    //    var a = $(mainQuestion).text().replace(/\b(\w+?)\b(?!([^<]+)?>)\b(?!([^&]+)?;)/g, '<span class="wordsDict">$1</span>');
    //    console.log(a);
    //    var w = '';
    //    var d = document.createElement('div');
    //    d.innerHTML = a;
    //    $(d).find('span.wordsDict').each(function () {            
    //        w += $(this).text() + ',';
    //    });       
    //    var words = w.split(",");
    //    var uniqueWords = [];

    //    $.each(words, function (i, el) {
    //        if ($.inArray(el, uniqueWords) === -1) uniqueWords.push(el);
    //    });

                
    //   // console.log(h);
    //    for (var i = 0; i < uniqueWords.length - 1; i++) {
    //        h = h.replace(uniqueWords[i], '<span class="wordsDict">' + uniqueWords[i] + '</span>');
    //    }        
    //    $(mainQuestion).html(h);
    //   // console.log($(mainQuestion).html());
        
    //    var table1 = $('#Table1');
    //    table1.children('tbody').children('tr').children('td').each(function () {
    //        $(this).each(function () {               
    //            $(this).html(function (index, oldHtml) { return oldHtml.replace(/\b(\w+?)\b(?!([^<]+)?>)\b(?!([^&]+)?;)/g, '<span class="wordsDict">$1</span>') });
    //        });
    //    });
    //}
    //var IsEng = $('#hdIsGroupEng').val();
    //if (IsEng == "True") {
    //    //delete FastButton.prototype.handleEvent;
    //   // FastButton.prototype.handleEvent = null;
    //    //delete FastButton.prototype.onTouchStart;
    //    //delete FastButton.prototype.onTouchMove;
    //    //delete FastButton.prototype.onClick;
    //}    
    $('#hdStatusDict').val('On');
}
function turnOffDict() {
    //var mainQuestion = $('#mainQuestion');
    //mainQuestion.find('span.wordsDict').contents().unwrap();
    //var table1 = $('#Table1');
    //table1.children('tbody').children('tr').children('td').each(function () {
    //    $(this).each(function () {
    //        $(this).find('span.wordsDict').contents().unwrap();
    //    });
    //});
    //var IsEng = $('#hdIsGroupEng').val();
    //if (IsEng == "True") {       
    //}
    $('#hdStatusDict').val('Off');
}


///////////////////////////////////////////Calculator///////////////////////////////////////////

//Calculator
var keepNum = '';
function calculator(number) {
    var txtNum = document.getElementById('SumNumber').innerHTML;
    if (IsNumber(number)) {
        var s = keepNum.indexOf('.');
        if (txtNum == '0') {
            if (number == '.') {
                if (s == -1) {
                    document.getElementById('SumNumber').innerHTML += number;
                }
            } else {
                NoOfNumber++;
                document.getElementById('SumNumber').innerHTML = number;
            }
        } else {
            if (number == '.') {
                if (s == -1) {
                    document.getElementById('SumNumber').innerHTML += number;
                }
            } else {
                if (IsNotOverBaseCharacter(txtNum)) {
                 document.getElementById('SumNumber').innerHTML += number;                    
                }
            }
        }
        keepNum += number;
    } else {
        keepNum = '';
        txtNum = StrBeforeEval(txtNum);        
        if (number == '=') {
            document.getElementById('SumNumber').innerHTML = eval(txtNum);
        } else {
            document.getElementById('SumNumber').innerHTML = txtNum + number;
        }
    }

}
function IsNumber(number) {
    if (number == '/' || number == '*' || number == '-' || number == '+' || number == '=') {
        NoOfNumber = 0;
        return false;
    } else {
        return true;
    }
}
function StrBeforeEval(str) {
    var s = str.substr(str.length - 1, 1);
    if (isNaN(s)) {
        str = str.substring(0, str.length - 1);
    }
    return str;
}
//กรอกตัวเลขแต่ละชุดได้ไม่เกิน 6 หลัก
var NoOfNumber = 0;
function IsNotOverBaseCharacter(Number) {
    //var ArrOperator = ['/', '*', '-', '+']; var IsHaveOperater;
    //for (var i = 0; i < ArrOperator.length - 1; i++) {
    //    if (Number.indexOf(ArrOperator[i].toString) != -1) {
    //        IsHaveOperater = true;
    //        break;
    //    }
    //    else {
    //        IsHaveOperater = false;
    //    }        
    //}
    //var Operators = '/*-+';
    //var IsHaveOperater = (Number.indexOf(Operators) != -1) ? true : false;
    //console.log(Number.indexOf(Operators));
    //var ArrNumber = Number.split(ArrOperator[0].toString);//.split(ArrOperator[1].toString).split(ArrOperator[2].toString).split(ArrOperator[3].toString);
    //alert(ArrNumber[ArrNumber.length - 1]);

    //console.log(NoOfNumber);
    //if (IsHaveOperater) {
    //        var ArrNumber = Number.split(ArrOperator[0].toString).split(ArrOperator[1].toString).split(ArrOperator[2].toString).split(ArrOperator[3].toString);
    //        alert(ArrNumber[ArrNumber.length - 1]);
    //    NoOfNumber = 1;
    //}
    //else {        

    //}
    //console.log(NoOfNumber);
    if (NoOfNumber >= 6) {
        return false;
    }
    else {       
        NoOfNumber++;
        return true;
    }
}

function ClearValue() {
    NoOfNumber = 0;
    document.getElementById("SumNumber").innerHTML = "0";
}