<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="StudentListControl.ascx.vb"
    Inherits="QuickTest.StudentListControl" %>
<%--<script src="../js/jquery-1.7.1.js" type="text/javascript"></script>--%>

<script src="../js/GFB.js" type="text/javascript"></script>
<script src="../js/jquery.qtip-1.0.0-rc3.js" type="text/javascript"></script>
<script type="text/javascript">

    $(function () {

        //DivPicture
        $('.DivPicture').each(function () {
            new FastButton(this, TriggerDivPictureClick);
        });

        $('.HTD').qtip({
            content: 'จำนวน "การบ้าน" ที่ทำในวันนี้ (ถึงกำหนดภายใน 24ชม.)',
            position: {
                corner: {
                    tooltip: 'bottomLeft',
                    target: 'topRight'
                }
            },
            style: {
                width: 250, padding: 5, background: '#F68500', color: 'white', textAlign: 'center', border: { width: 7, radius: 5, color: '#F68500' }, tip: 'bottomLeft', name: 'dark', 'font-weight': 'bold'
            }
        });

        $('.QTD').qtip({
            content: 'จำนวน "ควิซ" ที่ทำในวันนี้ (ถึงกำหนดภายใน 24ชม.)',
            position: {
                corner: {
                    tooltip: 'bottomRight',
                    target: 'topLeft'
                }
            },
            style: {
                width: 250, padding: 5, background: '#F68500', color: 'white', textAlign: 'center', border: { width: 7, radius: 5, color: '#F68500' }, tip: 'bottomRight', name: 'dark', 'font-weight': 'bold'
            }
        });

        $('.PTD').qtip({
            content: 'จำนวนการ "ฝึกฝน" ในวันนี้ (ถึงกำหนดภายใน 24ชม.)',
            position: {
                corner: {
                    tooltip: 'bottomLeft',
                    target: 'topRight'
                }
            },
            style: {
                width: 250, padding: 5, background: '#F68500', color: 'white', textAlign: 'center', border: { width: 7, radius: 5, color: '#F68500' }, tip: 'bottomLeft', name: 'dark', 'font-weight': 'bold'
            }
        });

        $('.HWTD').qtip({
            content: 'จำนวนการบ้านที่ยังไม่ส่งในวันนี้ (ถึงกำหนดภายใน 24ชม.)',
            position: {
                corner: {
                    tooltip: 'bottomLeft',
                    target: 'topRight'
                }
            },
            style: {
                width: 250, padding: 5, background: '#F68500', color: 'white', textAlign: 'center', border: { width: 7, radius: 5, color: '#F68500' }, tip: 'bottomLeft', name: 'dark', 'font-weight': 'bold'
            }
        });

        $('.HWALL').qtip({
            content: 'จำนวนการบ้านที่ยังไม่ส่ง',
            position: {
                corner: {
                    tooltip: 'bottomRight',
                    target: 'topLeft'
                }
            },
            style: {
                width: 200, padding: 5, background: '#F68500', color: 'white', textAlign: 'center', border: { width: 7, radius: 5, color: '#F68500' }, tip: 'bottomRight', name: 'dark', 'font-weight': 'bold'
            }
        });

        



        //function กดติดดาวเพิ่มนักเรียนเข้าไปใน favorite
        //                $('.imgStar').click(function () {
        //                    alert('โดนดาวจ้า');
        //            if ($(this).hasClass('ForGrayStar')) {
        //                var ImgId = $(this);
        //                var StudentId = $(this).attr('StId');
        //                $.ajax({ type: "POST",
        //	            url: "<%=ResolveUrl("~")%>WebServices/StudentService.asmx/AddStudentFavorite",
        //	            data: "{ StudentId: '" + StudentId + "'}",
        //	            contentType: "application/json; charset=utf-8", dataType: "json",   
        //	            success: function (msg) {
        //                    if (msg.d !== '') {
        //                        //alert(msg.d);
        //                        $(ImgId).attr('src', '/Images/star.gif');
        //                        $(ImgId).removeClass('ForGrayStar');
        //                        $(ImgId).addClass('ForYellowStar');
        //                        }
        //	                },
        //	                error: function myfunction(request, status)  {
        //                    //alert('บางอย่างผิดพลาดโปรดลองอีกครั้ง')
        //	                }
        //	            });
        //            }
        //            else {
        //             var ImgId = $(this);
        //              var StudentId = $(this).attr('StId');
        //              $.ajax({ type: "POST",
        //	            url: "<%=ResolveUrl("~")%>WebServices/StudentService.asmx/DeleteFavoriteStudent",
        //	            data: "{ StudentId: '" + StudentId + "'}",
        //	            contentType: "application/json; charset=utf-8", dataType: "json",   
        //	            success: function (msg) {
        //                    if (msg.d !== '') {
        //                           //alert(msg.d);
        //                           $(ImgId).attr('src', '/Images/Grey_Star.gif');
        //                           $(ImgId).removeClass('ForYellowStar');
        //                           $(ImgId).addClass('ForGrayStar');
        //                        }
        //	                },
        //	                error: function myfunction(request, status)  {
        //                    //alert('บางอย่างผิดพลาดโปรดลองอีกครั้ง')
        //	                }
        //	            });
        //            }
        //        });
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //Function ลบนักเรียนออกจาก favorite 
        //        $('.ForDeleteFavorite').click(function () {
        //        alert('โดนดาวจ้า');
        //           var Objimg = $(this);
        //           var StudentId = $(this).attr('StId');
        //              $.ajax({ type: "POST",
        //	            url: "<%=ResolveUrl("~")%>WebServices/StudentService.asmx/DeleteFavoriteStudent",
        //	            data: "{ StudentId: '" + StudentId + "'}",
        //	            contentType: "application/json; charset=utf-8", dataType: "json",   
        //	            success: function (msg) {
        //                    if (msg.d !== '') {
        //                        //alert(msg.d);
        //                        $(Objimg).parent().parent().hide();
        //                        var CheckDivResult = $('.DivCover:visible').length;
        //                        if (CheckDivResult == 0) {
        //                        $('#DivHaveData').remove();
        //                        var ap = $("<div id='DivNoData' class='ForMainDivNoData' ><div id='DivShowInfo' class='ForDivShowInFo'><img src='Images/blue-arrow-pointing-left-hi.png' style='width: 130px; position: absolute; top: 40px;left:20px;' /><span style='font-size: 60px; font-weight: bold; position: relative; top: 60px;'>เลือกห้องก่อนค่ะ</span></div></div>");
        //                         $('#MainDiv').append(ap);
        //                        }
        //                        }
        //	                },
        //	                error: function myfunction(request, status)  {
        //                    //alert('บางอย่างผิดพลาดโปรดลองอีกครั้ง')
        //	                }
        //	            });
        //        });
    });

    function TriggerDivPictureClick(e) {
        var obj = e.target;
        var stuId = $(obj).attr('stuid');

        if ($(obj).is('.DivPicture')) {
            $(obj).css('background-image', "url('MonsterID.axd?seed=" + stuId + "')");
        } else if ($(obj).parent().is('.DivPicture')) {
            stuId = $(obj).parent('.DivPicture').attr('stuid');
            $(obj).parent('.DivPicture').css('background-image', "url('MonsterID.axd?seed=" + stuId + "')");
        } else {
            stuId = $(obj).parent().parent('.DivPicture').attr('stuid');
            $(obj).parent().parent('.DivPicture').css('background-image', "url('MonsterID.axd?seed=" + stuId + "')");
        }

        GotoTeacherStudentDetailPage(stuId, e, obj);
    }

    /////////////////////Function กดที่รูปเด็กเพื่อไปหน้า TeacherStudentDetailPage.aspx
    function GotoTeacherStudentDetailPage(StudentId, e, t) {
        var a = '';
        if ($.browser.msie && $.browser.version <= 9.0) {
            a = e.srcElement.getAttribute('class');
            a = a != null ? a : 'DivPicture';
        }
        //alert($(e.target).is('.ForDeleteFavorite'));
        if ($(e.target).is('.ForDeleteFavorite') || a == 'ForDeleteFavorite') {
            //alert($(e.target).attr('stid'));  
            if ($.browser.msie && $.browser.version < 9.0) {
               // DeleteFavorite(e, t);
            } else {
             //   DeleteFavorite($(e.target), t);
            }
        }
        else if ($(e.target).is('.ForGrayStar') || a == 'ForGrayStar') {
           // AddOrRemoveStudentFavorite($(e.target));
        }
        else if ($(e.target).is('.ForYellowStar' || a == 'ForYellowStar')) {
         //   AddOrRemoveStudentFavorite($(e.target));
        }
        else if ($(e.target).is('.DivPicture') || a == 'DivPicture' || $(e.target).is('.ForLeftPanel')) {
            window.location = '<%=ResolveUrl("~")%>Teacher/TeacherStudentDetailPage.aspx?StudentId=' + StudentId;
        }
}


//Function ลบนักเรียนออกจาก favorite 
function DeleteFavorite(InputObj, t) {
    var StudentId; var Objimg;
    if ($.browser.msie && $.browser.version < 9.0) {
        StudentId = InputObj.srcElement.getAttribute('StId');
    } else {
        Objimg = $(InputObj);
        StudentId = $(Objimg).attr('StId');
    }
    //alert(StudentId);
    $.ajax({
        type: "POST",
        url: "<%=ResolveUrl("~")%>WebServices/StudentService.asmx/DeleteFavoriteStudent",
        data: "{ StudentId: '" + StudentId + "'}",
        contentType: "application/json; charset=utf-8", dataType: "json",
        success: function (msg) {
            if (msg.d !== '') {
                //alert(msg.d);
                if ($(t).is('img')) {
                    $(t).parent().parent().remove();
                }
                else {
                    $(t).parent().remove();
                }
                //$(Objimg).parent().parent().hide();
                var CheckDivResult = $('.DivCover:visible').length;
                if (CheckDivResult == 0) {
                    $('#DivHaveData').remove();
                    var ap;
                    if ($('#imgChooseClass').length > 0) {
                        ap = $("<div id='DivNoData' class='ForMainDivNoData' ><div id='DivShowInfo' class='ForDivShowInFo' ><img src='../Images/star.gif' style='width:120px;position:absolute;top:25px;left:30px;' /><span style='font-size: 40px; font-weight: bold; position: relative; top: 25px;'>แสดงนักเรียนที่ติดดาวไว้</span><br /><span style='font-size: 30px; position: relative; top: 30px;'>(ใช้ติดตามเด็กที่ต้องการดูแลเป็นพิเศษ)</span><br /><span class='hint' style='top:30px;position: relative;'>ติดดาวเด็กที่ต้องการดูแลเป็นพิเศษได้ที่ <a href='../Student/StudentListPage.aspx' class='hint'>หน้ารายชื่อนักเรียน</a> ค่ะ</span></div></div>");
                    } else {
                        ap = $("<div id='DivNoData' class='ForMainDivNoData' ><div id='DivShowInfo' class='ForDivShowInFo'><img src='Images/blue-arrow-pointing-left-hi.png' style='width: 130px; position: absolute; top: 40px;left:20px;' /><span style='font-size: 60px; font-weight: bold; position: relative; top: 60px;'>เลือกห้องก่อนค่ะ</span></div></div>");
                    }
                    $('#MainDiv').append(ap).addClass('nodata');
                }
            }
        },
        error: function myfunction(request, status) {
            //alert('บางอย่างผิดพลาดโปรดลองอีกครั้ง')
        }
    });
}


function AddOrRemoveStudentFavorite(InputObj) {
    var ObjImg = $(InputObj);
    if ($(ObjImg).hasClass('ForGrayStar')) {
        var ImgId = $(ObjImg);
        var StudentId = $(ObjImg).attr('StId');
        $.ajax({
            type: "POST",
            url: "<%=ResolveUrl("~")%>WebServices/StudentService.asmx/AddStudentFavorite",
                data: "{ StudentId: '" + StudentId + "'}",
                contentType: "application/json; charset=utf-8", dataType: "json",
                success: function (msg) {
                    if (msg.d !== '') {
                        //alert(msg.d);
                        $(ImgId).attr('src', '../Images/dashboard/student/Unfavorite.png');
                        $(ImgId).removeClass('ForGrayStar');
                        $(ImgId).addClass('ForYellowStar');
                    }
                },
                error: function myfunction(request, status) {
                    //alert('บางอย่างผิดพลาดโปรดลองอีกครั้ง')
                }
            });
        }
        else {
            var ImgId = $(ObjImg);
            var StudentId = $(ObjImg).attr('StId');
            $.ajax({
                type: "POST",
                url: "<%=ResolveUrl("~")%>WebServices/StudentService.asmx/DeleteFavoriteStudent",
            data: "{ StudentId: '" + StudentId + "'}",
            contentType: "application/json; charset=utf-8", dataType: "json",
            success: function (msg) {
                if (msg.d !== '') {
                    //alert(msg.d);
                    $(ImgId).attr('src', '../Images/dashboard/student/Favorite.png');
                    $(ImgId).removeClass('ForYellowStar');
                    $(ImgId).addClass('ForGrayStar');
                }
            },
            error: function myfunction(request, status) {
                //alert('บางอย่างผิดพลาดโปรดลองอีกครั้ง')
            }
        });
    }
}

</script>
<script src="../js/iconFavorite.js" type="text/javascript"></script>
<style type="text/css">
    .DivWidth {
        width: 179px;
    }

    .DivCover {
        border: 1px solid rgb(235, 235, 235);
        border-radius: 5px;
        height: 190px;
        display: inline-block;
        margin: 0 10px 20px 10px;
        position: relative;
        z-index: 100;
    }

    .DivPicture {
        height: 80%; /*background-image: url('../Images/default-profile-image.png');*/
        background-size: cover;
        border-radius: 5px 5px 0 0;
        -ms-behavior: url('../css/backgroundsize.min.htc');
        position:relative;
    }

    .backgroundsize .DivPicture {
        background-size: cover;
    }

    .DivName {
        height: 19.5%;
        line-height: 38px;
        text-align: center; /*overflow-x:auto;white-space:nowrap;*/
        background-color: #FFA032;
        border-top: 1px solid rgb(235, 235, 235);
        border-radius: 0 0 5px 5px;
        overflow: hidden;
        position: absolute;
    }

    .ForDivShowInFo {
        text-align: center;
        position: relative;
        /*top: 15px;*/
    }

    .ForYellowStar {
        /*width: 40px;
        height: 35px;*/
        left: 5px;
        position: relative; /*bottom: -45px;*/
        left: 1px;
        top: 62px;
        cursor: pointer;
        z-index: 90;
    }

    .ForGrayStar {
        /*float: left;*/
        /*width: 40px;
        height: 35px;*/
        left: 5px;
        position: relative; /*bottom: -35px;*/
        left: 1px;
        top: 62px;
        cursor: pointer;
        z-index: 90;
    }

    .ForDeleteFavorite, .ForGrayStar, .ForYellowStar {
        float: left;
        /*width: 40px;
        height: 40px;*/
        left: 5px;
        position: relative;
        bottom: -110px;
        cursor: pointer;
        z-index: 90;
    }

    .ForMainDivHaveDataRoom {
        width: 100%;
        height: 360px;
        overflow-y: auto;
        border-radius: 3px;
    }

    .ForMainDivHaveDataTeacher {
        width: 100%;
        height: auto; /*overflow-y: auto;*/
    }

    .ForMainDivNoData {
        width: 100%;
        height: 244px;
        overflow-y: auto;
        /*border: 2px dashed;*/
    }

    .ForSmallDivRight {
        border-left: 1px solid rgb(235, 235, 235);
    }

    .StandardForSmallDiv {
        width: 45px;
        height: 38px;
        border-bottom: 1px solid rgb(235, 235, 235);
        text-align: center;
        line-height: 40px;
        background-color: orange;
        color: black;
    }

    div.StandardForSmallDiv:last-child {
        border-radius: 5px 0 0 0;
    }

    .ForSmallDivLeft {
        border-right: 1px solid rgb(235, 235, 235);
    }

    .ForLeftPanel {
        position: absolute; /*top:-35px;*/
        width: 45px;
        height: 95px;
        top: 0;
    }

    .ForRightPanel {
        padding-left: 35px;
        font-weight: bold;
        font-size: 17px;
        background-color: rgba(255, 215, 168, 0.74);
        color: black;
        height: 38px;
        line-height: 40px;
        border-radius: 5px 5px 0 0;
    }

    #MainDiv {
        width: 821px;
        border: 1px solid #FFA032;
        border-radius: 5px;
        margin: auto;
        padding: 20px 0 0 0;
        min-height: 172px;
        max-height: 364px;
        overflow-y: auto;
        text-align: center;
    }

        #MainDiv.nodata {
            border: 1px dashed #FFA032;
        }
</style>
<script type="text/javascript">
    // check ว่า browser ที่ใช้ support หรือเปล่า?
    //if (Modernizr.flexbox) {
    //    alert('support ');
    //} else {
    //    alert('no support ');
    //}  
</script>

<div id='MainDiv' runat="server" clientidmode="Static">
</div>
