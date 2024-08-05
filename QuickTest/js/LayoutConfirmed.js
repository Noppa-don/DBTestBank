function UpdateLayoutConfirmed(updateField, tick, questionId) {
    $.ajax({
        type: "POST",
        async: false,
        url: ResolveURL + 'WebServices/TestsetService.asmx/UpdateLayoutConfirmed',
        data: "{ UpdateField : '" + updateField + "', Tick : '" + tick + "', QuestionId : '" + questionId + "'}",  //" 
    contentType: "application/json; charset=utf-8", dataType: "json",
    success: function (msg) {
    }
});
}

function SaveLog(LogText) {
    alert(1);
    $.ajax({
        type: "POST",
        async: false,
        url: ResolveURL + 'WebServices/TestsetService.asmx/SaveLog',
        data: "{ LogText : '" + LogText + "'}",  //" 
        contentType: "application/json; charset=utf-8", dataType: "json",
        success: function (msg) {
        }
    });
}

