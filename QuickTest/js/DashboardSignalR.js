
var SignalRCheck;
var WithOutClick = true;
var ChangePage = false;
var SelectedName = GetSelectedSession();
var ThisPage = (window.location.pathname).toLowerCase().substring(1).replace(ResolveUrl("~/").toLowerCase().substring(1), '');
var QueryString = window.location.search;
var changePage = false;


$(window).bind('beforeunload', function () {
    if (WithOutClick == true) {
        SetUnload(true); // unload = true
    }
});

// ถ้ามาที่ dashboard ต้องเคลีย session ทุกตัวที่เกียวกับ testset ThisPage != 'dashboardstudentpage.aspx'
if (ThisPage == 'quiz/dashboardquizpage.aspx' || ThisPage == 'homework/dashboardhomeworkpage.aspx' || ThisPage == 'practice/dashboardpracticepage.aspx' || ThisPage == 'printtestset/dashboardprinttestsetpage.aspx' || ThisPage == 'testset/dashboardsetuppage.aspx') {
    ResetValue();
}

// ถ้าเป็นการ unload ให้ทำการ save หน้าปัจจุบันลงไปที่ application selectsession ด้วย
if (GetUnload() == true) {
    console.log("Unload true")
    SetCurrentPage(ThisPage);
    SetCurrentQuerystring(QueryString);
    SetUnload(false);
    changePage = true;
    if (ThisPage == "testset/step2.aspx") {
        SetObjTestset();
        SetEditId();
    }
    if (ThisPage == "testset/step3.aspx" || ThisPage == "testset/step4.aspx") {
        SetObjTestset();
    }   
    if (ThisPage.indexOf("practicemode_pad/chooseclass.aspx") != -1) {
        console.log("Set Session choosemode");
        SetSessionChooseMode();
    }
    if (ThisPage == "practicemode_pad/choosesubject.aspx") {
        SetPClassId();
    }
    if (ThisPage == "practicemode_pad/choosequestionset.aspx") {
        SetPSubjectName();
    }
}
else {
    // check ว่าหน้าที่เปิดอยู่เป็นหน้าปัจจุบันหรือไม่ ถ้าไม่ใช่ให้ redirect ไปยังหน้าปัจจุบัน    
    var CurrentPage = GetCurrentPage();
    console.log(CurrentPage);
    var CurrentQuerystring = GetCurrentQuerystring();
    if (CurrentPage != ThisPage) {
        WithOutClick = false;
        window.location = ResolveUrl("~/" + CurrentPage + CurrentQuerystring);        
    }
}

SignalRCheck = $.connection.hubSignalR;
SignalRCheck.client.send = function (message) {
    if (message != "EndQuiz" && message != "Reload") {
        // get setting  step3
        if (message == "testset/step3.aspx" || message == "testset/step4.aspx") {
            GetObjTestset();
        }
        if (message == "practicemode_pad/choosesubject.aspx") {
            GetPClassId();
        }
        if (message == "practicemode_pad/choosequestionset.aspx") {
            GetPSubjectName();
        }        
        if (CoverPageName(message) == 'activity/activitypage.aspx') {
            GetSessionQuizId();
            GetSessionChooseMode();
        }
        if (message != ThisPage) {
            WithOutClick = false;
            window.location = ResolveUrl("~/" + message);
        }
    }
};

$.connection.hub.start().done(function () {
    SignalRCheck.server.addToGroup(SelectedName);
    if (changePage) {
        SignalRCheck.server.sendCommand(SelectedName, ThisPage + QueryString);
    }
    if (ThisPage == "activity/alternativepage.aspx") {
        if (Groupname != '') {
            SignalRCheck.server.addToGroup(Groupname);
            SignalRCheck.server.sendCommand(Groupname, "EndQuiz");
        }
    }

});


function ResolveUrl(url) {
    if (url.indexOf("~/") == 0) {
        url = baseUrl + url.substring(2);
    }
    return url;
}
function GetSelectedSession() {
    var selected;
    $.ajax({ type: "POST",
        //url: "../WebServices/DashboardSignalRService.asmx/GetSelectSession",
        url: ResolveUrl('~/WebServices/DashboardSignalRService.asmx/GetSelectSession'),
        async: false,
        contentType: "application/json; charset=utf-8", dataType: "json",
        success: function (data) {            
            selected = data.d;
        },
        error: function myfunction(request, status) {
            //alert('GetSelectedSession');
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
            //alert('SetUnload');
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
            //alert('GetUnload');
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
            //alert('SetCurrentPage');
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
            //alert('GetCurrentPage');
        }
    });
    return CurrentPage;
}
function SetCurrentQuerystring(Querystring) {
    $.ajax({ type: "POST",
        url: ResolveUrl('~/WebServices/DashboardSignalRService.asmx/SetCurrentQuerystring'),
        data: "{ Querystring : '" + Querystring + "'}",
        async: false,
        contentType: "application/json; charset=utf-8", dataType: "json",
        success: function (data) {
        },
        error: function myfunction(request, status) {
            //alert('SetCurrentQuerystring');
        }
    });
}
function GetCurrentQuerystring() {
    var Querystring;
    $.ajax({ type: "POST",
        url: ResolveUrl('~/WebServices/DashboardSignalRService.asmx/GetCurrentQuerystring'),
        async: false,
        contentType: "application/json; charset=utf-8", dataType: "json",
        success: function (data) {
            Querystring = data.d;
        },
        error: function myfunction(request, status) {
            //alert('GetCurrentPage');
        }
    });
    return Querystring;
}
function ResetValue() {
    $.ajax({ type: "POST",
        url: ResolveUrl('~/WebServices/DashboardSignalRService.asmx/SetNothingInSession'),
        async: false,
        contentType: "application/json; charset=utf-8", dataType: "json",
        success: function (data) {
        },
        error: function myfunction(request, status) {
            //alert("ResetValue");
        }
    });
}
function SetObjTestset() {
    $.ajax({ type: "POST",
        url: ResolveUrl('~/WebServices/DashboardSignalRService.asmx/SetObjTestset'),
        async: false,
        contentType: "application/json; charset=utf-8", dataType: "json",
        success: function (data) {
        },
        error: function myfunction(request, status) {
            //alert("SetObjTestset");
        }
    });
}
function GetObjTestset() {
    $.ajax({ type: "POST",
        url: ResolveUrl('~/WebServices/DashboardSignalRService.asmx/GetObjTestset'),
        async: false,
        contentType: "application/json; charset=utf-8", dataType: "json",
        success: function (data) {
        },
        error: function myfunction(request, status) {
            //alert("GetObjTestset");
        }
    });
}
function SetEditId() {
    $.ajax({ type: "POST",
        url: ResolveUrl('~/WebServices/DashboardSignalRService.asmx/SetEditId'),
        async: false,
        contentType: "application/json; charset=utf-8", dataType: "json",
        success: function (data) {
        },
        error: function myfunction(request, status) {
            //alert("SetEditId");
        }
    });
}
function GetEditId() {
    $.ajax({ type: "POST",
        url: ResolveUrl('~/WebServices/DashboardSignalRService.asmx/GetEditId'),
        async: false,
        contentType: "application/json; charset=utf-8", dataType: "json",
        success: function (data) {
        },
        error: function myfunction(request, status) {
            //alert("GetEditId");
        }
    });
}
function SetPClassId() {
    $.ajax({ type: "POST",
        url: ResolveUrl('~/WebServices/DashboardSignalRService.asmx/SetPClassId'),
        async: false,
        contentType: "application/json; charset=utf-8", dataType: "json",
        success: function (data) {
        },
        error: function myfunction(request, status) {
            //alert("SetPClassId");
        }
    });
}
function GetPClassId() {
    $.ajax({ type: "POST",
        url: ResolveUrl('~/WebServices/DashboardSignalRService.asmx/GetPClassId'),
        async: false,
        contentType: "application/json; charset=utf-8", dataType: "json",
        success: function (data) {
        },
        error: function myfunction(request, status) {
            //alert("GetPClassId");
        }
    });
}
function SetPSubjectName() {
    $.ajax({ type: "POST",
        url: ResolveUrl('~/WebServices/DashboardSignalRService.asmx/SetPSubjectName'),
        async: false,
        contentType: "application/json; charset=utf-8", dataType: "json",
        success: function (data) {
        },
        error: function myfunction(request, status) {
            //alert("SetPClassId");
        }
    });
}
function GetPSubjectName() {
    $.ajax({ type: "POST",
        url: ResolveUrl('~/WebServices/DashboardSignalRService.asmx/GetPSubjectName'),
        async: false,
        contentType: "application/json; charset=utf-8", dataType: "json",
        success: function (data) {
        },
        error: function myfunction(request, status) {
            //alert("GetPClassId");
        }
    });
}
function GetSessionQuizId() {
    $.ajax({ type: "POST",
        url: ResolveUrl('~/WebServices/DashboardSignalRService.asmx/GetSessionQuizId'),
        async: false,
        contentType: "application/json; charset=utf-8", dataType: "json",
        success: function (data) {
        },
        error: function myfunction(request, status) {
            //alert("GetSessionQuizId");
        }
    });
}
function SetSessionChooseMode() {
    $.ajax({ type: "POST",
        url: ResolveUrl('~/WebServices/DashboardSignalRService.asmx/SetSessionChooseMode'),
        async: false,
        contentType: "application/json; charset=utf-8", dataType: "json",
        success: function (data) {
        },
        error: function myfunction(request, status) {
            //alert("SetSessionChooseMode");
        }
    });
}
function GetSessionChooseMode() {
    $.ajax({ type: "POST",
        url: ResolveUrl('~/WebServices/DashboardSignalRService.asmx/GetSessionChooseMode'),
        async: false,
        contentType: "application/json; charset=utf-8", dataType: "json",
        success: function (data) {
        },
        error: function myfunction(request, status) {
            //alert("GetSessionChooseMode");
        }
    });
}
function CoverPageName(url) {
    var u = 'activity/activitypage.aspx';
    if (url.indexOf(u) != -1) {
        return u;
    }
}