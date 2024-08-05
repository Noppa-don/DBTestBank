/* function create button to report problem question*/
$(function () {
    
    // choosequestionset
    var $divListExam = $('#divListExam');
    $divListExam.children('table').children().children('tr').each(function () {
        $(this).hover(function () {
            $(this).append($('<img>', { src: '../Images/reporter.png', 'class': 'btnReportQuestion', onclick: 'ReporterQset("' + $(this).attr('id') + '");' }));
        }, function () {
            $(this).find('img.btnReportQuestion').remove();
        });
    });

    // showtestsetdetailpage
    $('.divExamType1').each(function () {
        $(this).hover(function () {
            var qsetId = $(this).attr('qsetId');
            var questionId = $(this).attr('id');
            $(this).append($('<img>', { src: '../Images/reporter.png', 'class': 'btnReportQuestion', onclick: 'ReporterQuestion("' + qsetId + '","' + questionId + '");' }));
        }, function () {
            $(this).find('img.btnReportQuestion').remove();
        });
    })

    $('.divExamType2').each(function () {
        $(this).hover(function () {
            var qsetId = $(this).attr('qsetId');
            var questionId = $(this).attr('id');
            $(this).append($('<img>', { src: '../Images/reporter.png', 'class': 'btnReportQuestion', onclick: 'ReporterQuestion("' + qsetId + '","' + questionId + '");' }));
        }, function () {
            $(this).find('img.btnReportQuestion').remove();
        });
    })

    $('.divExamType3').each(function () {
        $(this).hover(function () {
            var qsetId = $(this).attr('qsetId');
            var questionId = $(this).attr('id');
            $(this).children('td:last').append($('<img>', { src: '../Images/reporter.png', 'class': 'btnReportQuestion', onclick: 'ReporterQuestion("' + qsetId + '","' + questionId + '");' }));
        }, function () {
            $(this).find('img.btnReportQuestion').remove();
        });
    })

    $('.divExamType6').each(function () {        
        $(this).hover(function () {
            var qsetId = $(this).attr('qsetId');
            var questionId = $(this).attr('id');
            $(this).append($('<img>', { src: '../Images/reporter.png', 'class': 'btnReportQuestion', onclick: 'ReporterQuestion("' + qsetId + '","' + questionId + '");' }));
        }, function () {
            $(this).find('img.btnReportQuestion').remove();
        });
    })

    $('#mainQuestion').each(function () {
        $(this).hover(function () {
            var qsetId = $(this).attr('qsetId');
            var questionId = $(this).attr('questionId');
            $(this).append($('<img>', { src: '../Images/reporter.png', 'class': 'btnReportQuestion', onclick: 'ReporterQuestion("' + qsetId + '","' + questionId + '");' }));
        }, function () {
            $(this).find('img.btnReportQuestion').remove();
        });
    });
});

function ReporterQset(qsetId) {
    $.fancybox({
        fitToView: true,
        'autoScale': true,
        'transitionIn': 'none',
        'transitionOut': 'none',
        'href': '../support/reporterproblemQuestion.aspx?qsetId=' + qsetId,
        'type': 'iframe',
        'width': 600,
        'minHeight': 400//,
        //beforeShow: function () {
        //    //this.width = $('.fancybox-iframe').contents().find('html').width();                    
        //   this.height = ($('.fancybox-iframe').contents().find('html').height()) + 500;
        //}
        //'onClose': refreshPage()
    });
}

function ReporterQuestion(qsetId, questionId) {
    $.fancybox({
        fitToView: true,
        'autoScale': true,
        'transitionIn': 'none',
        'transitionOut': 'none',
        'href': '../support/reporterproblemQuestion.aspx?qsetId=' + qsetId + '&questionId=' + questionId,
        'type': 'iframe',
        'width': 600,
        'minHeight': 400       
    });
}