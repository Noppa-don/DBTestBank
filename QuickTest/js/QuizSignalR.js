
var SignalRCheck;
var Groupname;

var Unload;var CurrentPage;var SignalRCheck;
var withOutClick = true;
var changePage = false;
var firstClick = true;
       
Groupname = '<%=GroupName %>'; // เก็บ GroupName
var SelectedName = GetSelectedSession();

var ThisPage = (window.location.pathname).toLowerCase().substring(1);        

SignalRCheck = $.connection.HubSignalR;

SignalRCheck.client.send = function (message) {
    if (CoverPageName(message) == ThisPage || message == 'Reload') {
    }    
    else {
        window.location = message;
    }
};

// รับค่าจากที่ส่งมา
SignalRCheck.client.cmdControl = function (cmd) {           
    if (cmd == 'Next') {
        firstClick = false;
        $('#<%=btnNextTop.ClientID %>').trigger('click');
    }
    else if (cmd == 'Prev') {
        firstClick = false;
        $('#<%=btnPrvTop.ClientID %>').trigger('click');
    }
};

// รับค่าของตัวเองหลังจากที่ส่งไป
SignalRCheck.client.raiseEvent = function (cmd) {
        
    if (cmd == 'Next') {
        firstClick = false;
        $('#<%=btnNextTop.ClientID %>').trigger('click');
    }
    else if (cmd == 'Prev') {
        firstClick = false;
        $('#<%=btnPrvTop.ClientID %>').trigger('click');
    }
};

$.connection.hub.start().done(function () {

    SignalRCheck.server.addToGroup(Groupname);
    if ($('#HDCheckChangeQuestion').val() == 'Reload') {
        SignalRCheck.server.sendCommand(Groupname, 'Reload');
        $('#HDCheckChangeQuestion').val('Null');
    }

    SignalRCheck.server.addToGroup(SelectedName);
    SignalRCheck.server.sendCommand(SelectedName, ThisPage + window.location.search);

    var PracticeFromComputer = '<%= PracticeFromComputer %>';     
    var QuizId = '<%= shareQuizId %>';         
                
    $('#dialogIsLastQuestion').dialog({
        autoOpen: false,
        buttons: { 
        'จบควิซเลย': function () {
            if (PracticeFromComputer == "True") {
                window.location.href = '<%=ResolveUrl("~")%>Activity/ShowScore.aspx?QuizId=' + QuizId;
            }
            else {
                window.location = ResolveUrl("~/activity/alternativepage.aspx");                    
            }                    
        }, 
        'ยังก่อน': function () { 
            $(this).dialog('close'); }
        },
        draggable: false,
        resizable: false,
        modal: true
    });
                        
    // btnNext
    $('#<%=btnNextTop.ClientID %>').click(function (e) {            
        if (firstClick) {
            e.preventDefault();
            IsLastQuestion('Next');                
        }
    });
    $('#<%=btnNextSide.ClientID %>').click(function (e) {
        if (firstClick) {
            e.preventDefault();
            //SignalRCheck.server.cmdControlBtnPrevNext(GroupOld, 'Next');
            IsLastQuestion('Next');
        }
    });
    $('#<%=btnNext.ClientID %>').click(function (e) {
        if (firstClick) {
            e.preventDefault();
            //SignalRCheck.server.cmdControlBtnPrevNext(GroupOld, 'Next');
            IsLastQuestion('Next');
        }
    });

    // btn Prev
    $('#<%=btnPrvTop.ClientID %>').click(function (e) {
        if (firstClick) {
            e.preventDefault();
            SignalRCheck.server.cmdControlBtnPrevNext(GroupOld, 'Prev');                    
        }
    })  
    $('#<%=btnPrvSide.ClientID %>').click(function (e) {
        if (firstClick) {
            e.preventDefault();
            SignalRCheck.server.cmdControlBtnPrevNext(GroupOld, 'Prev');                    
        }
    }) 
    $('#<%=btnPrevious.ClientID %>').click(function (e) {
        if (firstClick) {
            e.preventDefault();
            SignalRCheck.server.cmdControlBtnPrevNext(GroupOld, 'Prev');                    
        }
    })   

}).fail(function(){   });
        
// function เช็ค ข้อสุดท้าย
var ShowDialog = '<%= Dialog %>';
var titleDialog = "<%=DialogTitle %>"; 
function IsLastQuestion(cmd) {
    if (ShowDialog == "True") {
        $('#dialogIsLastQuestion').dialog('open').dialog('option', 'title', titleDialog);
    }
    else {
        SignalRCheck.server.cmdControlBtnPrevNext(GroupOld, cmd);
    }   
} 

function sendNext() {
    if (firstClick) {
        //SignalRCheck.server.cmdControlBtnPrevNext(GroupOld, 'Next');
        return false;
    }
}
function sendPrev() {
    if (firstClick) {
        //SignalRCheck.server.cmdControlBtnPrevNext(GroupOld, 'Prev');
        return false;
    }
}
function ResolveUrl(url) {
    if (url.indexOf("~/") == 0) {
        url = baseUrl + url.substring(2);
    }
    return url;
}
function GetSelectedSession() {
    var selected;
    $.ajax({ type: "POST",        
        url: ResolveUrl('~/WebServices/DashboardSignalRService.asmx/GetSelectSession'),
        async: false,
        contentType: "application/json; charset=utf-8", dataType: "json",
        success: function (data) {
            selected = data.d;
        },
        error: function myfunction(request, status) {
            alert('GetSelectedSession');
        }
    });
    return selected;
}
function SetUnload(unload) {
    $.ajax({ type: "POST",
        url: ResolveUrl('~/WebServices/DashboardSignalRService.asmx/SetUnload'),
        data: "{ unload : '" + unload + "'}",
        async: false,
        contentType: "application/json; charset=utf-8", dataType: "json",
        success: function (data) {
        },
        error: function myfunction(request, status) {
            alert('SetUnload');
        }
    });
}
function GetUnload() {
    var Unload;
    $.ajax({ type: "POST",
        url: ResolveUrl('~/WebServices/DashboardSignalRService.asmx/GetUnload'),
        async: false,
        contentType: "application/json; charset=utf-8", dataType: "json",
        success: function (data) {
            Unload = data.d;
        },
        error: function myfunction(request, status) {
            alert('GetUnload');
        }
    });
    return Unload;
}
function SetCurrentPage(ThisPage) {
    $.ajax({ type: "POST",
        url: ResolveUrl('~/WebServices/DashboardSignalRService.asmx/SetCurrentPage'),
        data: "{ page : '" + ThisPage + "'}",
        async: false,
        contentType: "application/json; charset=utf-8", dataType: "json",
        success: function (data) {
        },
        error: function myfunction(request, status) {
            alert('SetCurrentPage');
        }
    });
}
function GetCurrentPage() {
    var CurrentPage;
    $.ajax({ type: "POST",
        url: ResolveUrl('~/WebServices/DashboardSignalRService.asmx/GetCurrentPage'),
        async: false,
        contentType: "application/json; charset=utf-8", dataType: "json",
        success: function (data) {
            CurrentPage = data.d;
        },
        error: function myfunction(request, status) {
            alert('GetCurrentPage');
        }
    });
    return CurrentPage;
}
function CoverPageName(url) {
    var u = 'activity/activitypage.aspx';
    if (url.indexOf(u) != -1) {
        return u;
    }
}