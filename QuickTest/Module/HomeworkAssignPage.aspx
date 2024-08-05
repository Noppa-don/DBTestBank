<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master"
    CodeBehind="HomeworkAssignPage.aspx.vb" Inherits="QuickTest.HomeworkAssignPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadStyleContent" runat="server">

    <link href="../css/styleQuiz.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/jquery-ui-1.10.1.custom.min.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/MySite.css" rel="stylesheet" type="text/css" />
    <link href="../css/jquery.fancybox.css" rel="stylesheet" />

    <style type="text/css">
        .ForBigDiv {
            width: 665px;
            height: 150px;
            border: solid 1px;
            border-radius: 4px;
            margin-bottom: 15px;
        }

        .ForMediumDiv {
            font-size: 33px;
            margin-bottom: 30PX;
        }

        .ForSmallDetailDiv {
            border-radius: 3px;
            background-color: #B2DB48;
            margin: 5px 5px 5px 5px;
            font-size: 20px;
            display: inline-block;
            border: solid 1px;
            padding: 0px 15px 0px 15px;
        }

        h2 {
            margin: 0 0 0 0 !important;
        }

        #divBtnBottom input {
            height: 40px;
            line-height: 40px;
            font-size: 140%;
        }

        .homeworkDialogBG {
            background-image: url('../images/homework/bgHomeworkConfirmDialog.png') !important;
            background-repeat: no-repeat !important;
            background-size: contain !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div id="DivTime" style="display: table-cell; vertical-align: middle;">
        <input type="hidden" />
        <div style="width: 100%">
            <table>
                <tr>
                    <td style="text-align: center">ตั้งแต่วันที่
                    </td>
                    <td style="text-align: center">ถึงวันที่
                    </td>
                </tr>
                <tr>
                    <td>
                        <div id="StartDate" class="time_element" style="font-size: 0.8em;"></div>
                    </td>
                    <td>
                        <div id="EndDate" class="time_element" style="font-size: 0.8em;"></div>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center">
                        <div>
                            <%-- <input id="StartTime" type="text" onkeypress="return event.charCode >= 48 && event.charCode <= 58" maxlength="5" />--%>
                            <input type="text" id="StartTime" name="timepicker1" class="time_element" value="23 : 59" />
                        </div>
                    </td>
                    <td style="text-align: center">
                        <div>
                            <%-- <input id="EndTime" type="text" onkeypress="return event.charCode >= 48 && event.charCode <= 58" maxlength="5" />--%>
                            <input type="text" id="EndTime" name="timepicker1" class="time_element" value="23 : 59" />
                        </div>
                    </td>
                </tr>
            </table>

        </div>
    </div>
    <div id="DivMsgSave" title="จัดการบ้านเรียบร้อบแล้วคะ">
        <%--จัดการบ้านเรียบร้อบแล้วคะ--%>
    </div>
    <div id="DivMsgNotStudent" title="ยังไม่ได้เลือกนักเรียนคะ">
        <%--ยังไม่ได้เลือกนักเรียนคะ--%>
    </div>
    <div id="DivMsgDup" title="สั่งการบ้านซ้ำค่ะ">
    </div>
    <div id="DivMsgUpdate" title="Quick Test">
        <div class="ForBigDiv">
            <div class="ForMediumDiv">
                ข้อมูลที่เพิ่มเข้ามา
            </div>
            <div>
            </div>
        </div>
        <div class="ForBigDiv">
            <div class="ForMediumDiv">
                ข้อมูลที่โดนลบออก
            </div>
            <div>
            </div>
        </div>
    </div>
    <div id='main'>
        <div id='site_content' style="border-radius: 10px;">
            <div id="DivInsitecontent" style="margin-left: auto; margin-right: auto; width: 935px;">
                <div class="content" style="width: 930px;">
                    <center>
						<h2>
							จัดการบ้าน</h2>		
					</center>
                    <div id="ModuleItem" style="overflow-x: hidden; border: 1px solid #FFA032; border-radius: 5px; padding: 10px 0px;">
                    </div>
                    <div id="ModuleSelect">
                    </div>
                </div>
            </div>
            <div style="width: 100%; height: 40px;" id="divBtnBottom">
                <input id="BtnReturn" type="button" value="กลับ" class="submitChangeFontSize" style="float: left; width: 200px; left: -200px; position: relative;" />
                <input id="BtnSave" type="button" value="บันทึก" class="submitChangeFontSize" style="width: 200px; right: 12px; float: right; position: relative;" />
            </div>
        </div>

    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ExtraScript" runat="server">
    <style type="text/css">
        #DivMsgNotStudent {
            height: 0 !important;
        }

        #DivMsgSave {
            height: 40px !important;
        }

        .titlebarTranparent {
            border: 0 !important;
            background-color: transparent !important;
        }
    </style>
    <link href="../css/jquery-ui-1.8.18.custom.css" rel="stylesheet" type="text/css" />
    <%--<link href="../Styles/jquery-ui-1.10.1.custom.min.css" rel="stylesheet" type="text/css" />--%>
    <link href="../Styles/MySite.css" rel="stylesheet" type="text/css" />
    <link href="../css/styleQuiz.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.7.1.js" type="text/javascript"></script>
    <script src="../js/GFB.js" type="text/javascript"></script>
    <script src="../js/jquery.fancybox.js" type="text/javascript"></script>
    <link href="../css/jquery.fancybox.css" rel="stylesheet" />
    <%--<script src="../Scripts/jquery-1.9.1.min.js" type="text/javascript"></script>--%>
    <%--<script src="../js/jquery-ui-1.8.18.js" type="text/javascript"></script>--%>
    <script src="../Scripts/jquery-ui-1.10.1.custom.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-migrate-1.1.1.js" type="text/javascript"></script>
    <script src="../Scripts/jGlobal.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-ui-1.8.10.offset.datepicker.min.js" type="text/javascript"></script>
    <link href="../Styles/timePicker.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery.timePicker.min.js" type="text/javascript"></script>
    <script src="../Scripts/MyPlugin/jqModuleItem.js" type="text/javascript"></script>
    <script src="../Scripts/MyPlugin/jqModuleSelect.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.signalR-2.0.2.min.js" type="text/javascript"></script>
    <script src='<%=ResolveClientUrl("~/signalr/hubs") %>' type="text/javascript"></script>
    <%If Not IE = "1" Then%>
    <script type="text/javascript">var baseUrl = "<%= ResolveUrl("~/") %>";</script>
    <script src="../js/DashboardSignalR.js" type="text/javascript"></script>
    <%End If%>
    
    <%--   <script src="../js/timepicki.js" type="text/javascript"></script>--%>
    <%--    <link rel="stylesheet" type="text/css" href="../css/timepicki.css">--%>

    <script type="text/javascript">
        var moduleIdUI = '<%= ModuleIdUI %>';
        var maId = '<%= MA_Id %>';
        var setStartTime;
        var setEndTime;
        var pageName = '<%= PageName %>';
        var ClassName = '<%= ClassName%>';
        var isNew = ('<%= IsNew %>' == "True" ? true : false);
        //var setAuto;

        var ua = navigator.userAgent.toLowerCase();
        var isAndroid = ua.indexOf("android") > -1;

        $(function () {

            //hover animation
            InjectionHover(':button', 3, false);

            //ปุ่มกลับ
            new FastButton(document.getElementById('BtnReturn'), TriggerReturnClick);

            //ปุ่ม บันทึก
            new FastButton(document.getElementById('BtnSave'), TriggerSaveClick);

            $('#Help').click(function () {
                $.fancybox({
                    'autoScale': true,
                    'blackBG': true,
                    'transitionIn': 'none',
                    'transitionOut': 'none',
                    'href': '../ShowImgHelpPage.aspx?FolderName=Module&PageName=HomeworkAssignPage', 'type': 'iframe', 'width': 750, 'minHeight': 425
                });
            });

            $('#ChangeFontSize').remove();
            // setting
            var options = $.extend(global.datepickerOption, {
                onSelect: function () {
                    var start = $("#StartDate").datepicker("getDate");
                    var end = $("#EndDate").datepicker("getDate");
                    if (start > end) {
                        $("#StartDate").datepicker("setDate", global.dateToString(global.setRealDateUI(end)));
                        start = $("#StartDate").datepicker("getDate");
                        end = $("#EndDate").datepicker("getDate");
                    }
                    //if (start.toDateString() == end.toDateString()) {
                    //    var startTime = $.timePicker("#StartTime").getTime();
                    //    var endTime = $.timePicker("#EndTime").getTime();
                    //    if (startTime >= endTime) {
                    //        startTime.setMinutes(startTime.getMinutes() + 5)

                    //        $.timePicker("#EndTime").setTime(global.timeToString(startTime));
                    //    }
                    //}
                }
            });

            options = $.extend(global.datepickerOption, {

                onSelect: function () {
                    var start = $("#StartDate").datepicker("getDate");
                    var end = $("#EndDate").datepicker("getDate");
                    if (start > end) {
                        $("#EndDate").datepicker("setDate", global.dateToString(global.setRealDateUI(start)));
                        start = $("#StartDate").datepicker("getDate");
                        end = $("#EndDate").datepicker("getDate");
                    }
                },
                minDate: 0
            });
            $("#StartDate").datepicker(options);
            $("#EndDate").datepicker(options);

            $("#StartTime").change(function () {
                var start = $("#StartDate").datepicker("getDate");
                var end = $("#EndDate").datepicker("getDate");
                if (start.toDateString() == end.toDateString()) {
                    var startTime = $.timePicker("#StartTime").getTime();
                    var endTime = $.timePicker("#EndTime").getTime();
                    if (startTime > endTime) {
                        startTime.setMinutes(startTime.getMinutes() + 5)

                        $.timePicker("#EndTime").setTime(global.timeToString(startTime));
                    }
                }
            });
            $("#EndTime").change(function () {
                var start = $("#StartDate").datepicker("getDate");
                var end = $("#EndDate").datepicker("getDate");
                if (start.toDateString() == end.toDateString()) {
                    var startTime = $.timePicker("#StartTime").getTime();
                    var endTime = $.timePicker("#EndTime").getTime();
                    if (startTime > endTime) {
                        endTime.setMinutes(endTime.getMinutes() - 5)

                        $.timePicker("#StartTime").setTime(global.timeToString(endTime));
                    }
                }
            });
            $(".time-picker").css("z-index", "9999");
            $("#DivMsgSave").dialog({
                autoOpen: false,
                show: "blind",
                hide: "blind",
                height: 200,
                width: 500,
                modal: true,
                resizable: false,
                draggable: false,
                position: "center",
                buttons: {
                    ตกลง: function () {
                        if (!isAndroid) {
                            FadePageTransitionOut();
                        }
                        window.location = "../" + pageName;
                        //$("#DivMsgSave").dialog("close");
                    }
                }
            });
            $('#DivMsgSave').parent().addClass('homeworkDialogBG');
            $('#DivMsgSave').prev().addClass('titlebarTranparent');
            $('#DivMsgSave').next().addClass('titlebarTranparent');

            $("#DivMsgNotStudent").dialog({
                autoOpen: false,
                show: "blind",
                hide: "blind",
                height: 200,
                width: 500,
                modal: true,
                resizable: false,
                position: "center",
                draggable: false,
                buttons: {
                    ตกลง: function () {
                        //window.location = "../" + pageName;
                        $("#DivMsgNotStudent").dialog("close");
                    }
                }
            });
            $("#DivMsgDup").dialog({
                autoOpen: false,
                show: "blind",
                hide: "blind",
                height: 380,
                width: 500,
                modal: true,
                position: "center",
                resizable: false,
                draggable: false,
                buttons: {
                    ยกเลิก: function () {
                        $("#DivMsgDup").dialog("close");
                    },
                    ตกลง: function () {
                        $("#DivMsgDup").dialog("close");
                        checkUpdate();
                    }
                }
            });
            $("#DivMsgUpdate").dialog({
                autoOpen: false,
                show: "blind",
                hide: "blind",
                height: 500,
                width: 700,
                modal: true,
                position: "center",
                buttons: {
                    ยกเลิก: function () {
                        $("#DivMsgUpdate").dialog("close");
                    },
                    ตกลง: function () {
                        $("#DivMsgUpdate").dialog("close");
                        saveAssignment();
                    }
                }
            });
            //SetDateDialog
            $("#DivTime").dialog({
                autoOpen: false,
                show: "blind",
                hide: "blind",
                height: 300,
                width: 720,
                modal: true,
                position: "center",
                buttons: {
                    ยกเลิก: function () {
                        $("#DivTime").dialog("close");
                    },
                    ตกลง: function () {
                        $("#DivTime").dialog("close");
                        setStartTime = $("#StartDate").datepicker("getDate");
                        //var sTime = $.timePicker("#StartTime").getTime();
                        //setStartTime.setHours(sTime.getHours());
                        //setStartTime.setMinutes(sTime.getMinutes());

                        setEndTime = $("#EndDate").datepicker("getDate");
                        //var eTime = $.timePicker("#EndTime").getTime();
                        //setEndTime.setHours(eTime.getHours());
                        //setEndTime.setMinutes(eTime.getMinutes());
                        $("#ModuleSelect").jqModuleSelectSetTimeHomework(setStartTime, setEndTime);
                    }

                },
                open: function () {
                    $("#StartDate").datepicker("setDate", global.dateToString(global.setRealDateUI(setStartTime)));
                    $("#EndDate").datepicker("setDate", global.dateToString(global.setRealDateUI(setEndTime)));
                    $.timePicker("#StartTime").setTime(global.timeToString(setStartTime));
                    $.timePicker("#EndTime").setTime(global.timeToString(setEndTime));


                    $("div[aria-describedby=DivTime] button:eq(1)").focus();
                }
            });
            $('#DivTime').parent().addClass('SetDateDialog');
            $("#ModuleItem").jqModuleItem({
                onItemSelect: function () {
                    // เมื่อเลือก panel บนทำการเพิ่ม panel ล่าง
                    $("#ModuleSelect").jqModuleSelectAddItem({ id: $(this).data("id"), text: $(this).data("text"), type: $(this).data("type"), maId: null, mdId: null });
                    if (isAndroid) {
                        $('.DivModuleSelectText').css('font-size', '30px');
                        $('.DivModuleSelectClose').css({ 'width': '45px', 'height': '45px', 'top': '5px', 'left': '230px' });
                    };
                },
                onSearch: function () {
                    // โค๊ดส่วนเพิ่มข้อมูลเข้าไปใน Module ที่กดปุ่ม search (แว่น)
                    var module = $(this).closest(".DivModulePanel").prop("id");
                    if (module == enMdType.MdClass) {
                        // ChangeLayout(2);
                        var input = "{SchoolCode:'<%= SchoolCode %>' , ClassName:'" + $(this).closest(".DivModuleItem").data("id") + "'}";
                        $.ajax({
                            type: "POST",
                            url: "../WebServices/ModuleService.asmx/GetRoomByClassName",
                            data: input,
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (msg) {
                                var result = msg.d;
                                var resultList = [];
                                $.each(result, function () {
                                    resultList.push({ id: this.Room_Id, text: this.Room_Name })
                                });

                                $("#ModuleItem").jqModuleItemLoadData(enMdType.MdRoom, resultList, true);

                                //$('.DivModuleItem').each(function () {
                                //    new FastButton(this, TriggerModuleItemClick);
                                //});

                                ChangeLayout();
                            },
                            error: function myfunction(request, stutus) {
                                alert(request.statusText);
                            }
                        });
                    } else if (module == enMdType.MdRoom) {
                        //   ChangeLayout(3);
                        var input = "{SchoolCode:'<%= SchoolCode %>' , RoomName:'" + $(this).closest(".DivModuleItem").data("text") + "'}";
                            $.ajax({
                                type: "POST",
                                url: "../WebServices/ModuleService.asmx/GetStudentByRoom",
                                data: input,
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                success: function (msg) {
                                    var result = msg.d;
                                    var resultList = [];
                                    $.each(result, function () {
                                        resultList.push({ id: this.Student_Id, text: this.Student_Name })
                                    });
                                    $("#ModuleItem").jqModuleItemLoadData(enMdType.MdStudent, resultList, false);

                                    //$('.DivModuleItem').each(function () {
                                    //    new FastButton(this, TriggerModuleItemClick);
                                    //});

                                    ChangeLayout();
                                },
                                error: function myfunction(request, stutus) {
                                    alert(request.statusText);
                                }
                            });
                        }

                }
            }).jqModuleItemHidePanel([enMdType.MdModule, enMdType.MdRoom, enMdType.MdStudent]);



            $("#ModuleSelect").jqModuleSelect({
                onSetTimeHomework: function () {
                    var divTimeHomework = $("#ModuleSelect").jqModuleSelectGetMy().find(".HomeworkTime").closest(".DivModuleSelect");
                    setStartTime = divTimeHomework.data("start").val;
                    setEndTime = divTimeHomework.data("end").val;
                    //setAuto = divTimeHomework.data("auto").val;

                    $("#DivTime").dialog("open");
                    $(".ui-dialog-titlebar").hide();
                }
            });

            // loaddata
            var result;
            var resultList = []
            resultList.push({ id: '<%= TestSetId %>', text: '<%= TestSetname %>' });
            //$("#ModuleItem").jqModuleItemLoadData(enMdType.MdModule, resultList, false); // เพิ่ม testset ที่รับมาจากหน้า step 3 เข้าไปใน panel module

            result = $.parseJSON('<%= DataClass %>');
            resultList = [];
            $.each(result, function () {
                resultList.push({ id: this.Class_Name, text: this.Class_Name })
            });
            $("#ModuleItem").jqModuleItemLoadData(enMdType.MdClass, resultList, true);
            ChangeLayout(1)


            var dataSetTime = $.parseJSON('<%= DataSetTime %>');
            var dataModule = $.parseJSON('<%= DataModule %>');
            var dataAssign = $.parseJSON('<%= DataAssign %>');
            if (dataModule.length > 0) {
                var mergeSetTime = { id: null, text: null, type: enMdType.MdModuleHomeworkTime, maId: null, mdId: null };
                $.each(dataSetTime, function () {
                    // ถ้าเป็น module เวลา จะเก็บค่าต่างๆใส่ id เลย แต่ไม่ใช่ id นะมันคือ value
                    if (this.text == "start") {
                        mergeSetTime.start = global.convertJsonDateToJsDate(this.id);
                        mergeSetTime.startMaId = this.maId;
                    }
                    if (this.text == "end") {
                        mergeSetTime.end = global.convertJsonDateToJsDate(this.id);
                        mergeSetTime.endMaId = this.maId;
                    }
                    if (this.text == "auto") {
                        mergeSetTime.auto = this.id;
                        mergeSetTime.autoMaId = this.maId;
                    }
                });
                $("#ModuleSelect").jqModuleSelectLoadItem([mergeSetTime], false);
                $("#ModuleSelect").jqModuleSelectLoadItem(dataModule, false);
                $("#ModuleSelect").jqModuleSelectLoadItem(dataAssign, true);
            } else {
                // MdHomeworkTime
                var defaultStart = new Date();
                defaultStart.setHours(defaultStart.getHours(), defaultStart.getMinutes(), 0);
                var defaultEnd = new Date();
                defaultEnd.setHours(23, 59, 0);
                defaultEnd.setDate(defaultEnd.getDate() + 1);
                var autoValidate = true;

                $("#ModuleSelect").jqModuleSelectAddItem({ id: null, text: '', type: enMdType.MdModuleHomeworkTime, maId: null, mdId: null, start: defaultStart, end: defaultEnd, auto: autoValidate }, false);
                $("#ModuleSelect").jqModuleSelectAddItem({ id: '<%= TestSetId %>', text: '<%= TestSetname %>', type: enMdType.MdModule, maId: null, mdId: null }, false);
            }


            //$("#BtnSave").click(function () {
            //	// เช็คว่าเลือกนักเรียนหรือเปล่า
            //	var ItemSelectAll = $("#ModuleSelect").jqModuleSelectGetAll();
            //	var IsSelectStudent = false;
            //	$(ItemSelectAll).each(function () {
            //		if ((this.type == enMdType.MdClass) || (this.type == enMdType.MdRoom) || (this.type == enMdType.MdStudent)) {
            //			IsSelectStudent = true;
            //		}
            //	});


            //	if (IsSelectStudent) {
            //		// เช็คซ้ำ
            //		var input = JSON.stringify({ MaId: maId, Assignment: $("#ModuleSelect").jqModuleSelectGetAll() });
            //		var result;
            //		$.ajax({
            //			type: "POST",
            //			async: false,
            //			url: "../WebServices/ModuleService.asmx/CheckDuplicateModuleHomeworkOnly",
            //			data: input,
            //			contentType: "application/json; charset=utf-8",
            //			dataType: "json",
            //			success: function (msg) {
            //				if (msg.d !== "") {
            //					result = msg.d;
            //				} else {
            //					alert("false2")
            //				}
            //			},
            //			error: function myfunction(request, stutus) {
            //				alert(request.statusText);
            //			}
            //		});

            //		if ((result.IsDup !== undefined) & (result.IsDup)) {
            //			$("#DivMsgDup").html(result.Msg);
            //			$("#DivMsgDup").dialog("open");
            //			$(".ui-dialog-titlebar").hide();
            //		} else {
            //			checkUpdate();
            //		}
            //	} else {
            //		$("#DivMsgNotStudent").dialog("open");
            //	}
            //})

            //$("#BtnReturn").click(function () {
            //                var s = $("#ModuleSelect").jqModuleSelectGetAllAssignText();
            //                alert(s.join(","));

            //window.location = "../" + pageName;
            //}
            //)

            //ดักถ้าเข้าจาก Tablet ของครู
            if (isAndroid) {
                $('#main').css({ 'width': '880px', 'height': '680px' });
                $('#site_content').css('width', '880px');
                $('#DivInsitecontent').css('width', '880px');
                $('.content').css('width', '880px');
                $('.DivModuleSelectTextFull HomeworkTime').css('font-size', '21px');
                $('.DivModuleSelectText').css('font-size', '30px');
                $('#BtnReturn').css({ 'width': '220px', 'height': '80px', 'font-size': '30px' });
                $('#BtnSave').css({ 'width': '220px', 'height': '80px', 'font-size': '30px' });
            }


            //$('.DivModuleItem').each(function () {
            //    new FastButton(this, TriggerModuleItemClick);
            //});
        })

        function TriggerReturnClick(e) {
            if (!isAndroid) {
                FadePageTransitionOut();
            }

            window.location = "../" + pageName + '?ClassName=' + ClassName;
        }

        function TriggerSaveClick() {
            // เช็คว่าเลือกนักเรียนหรือเปล่า
            var ItemSelectAll = $("#ModuleSelect").jqModuleSelectGetAll();
            var IsSelectStudent = false;
            $(ItemSelectAll).each(function () {
                if ((this.type == enMdType.MdClass) || (this.type == enMdType.MdRoom) || (this.type == enMdType.MdStudent)) {
                    IsSelectStudent = true;
                }
            });


            if (IsSelectStudent) {
                // เช็คซ้ำ
                var input = JSON.stringify({ MaId: maId, Assignment: $("#ModuleSelect").jqModuleSelectGetAll() });
                var result;
                $.ajax({
                    type: "POST",
                    async: false,
                    url: "../WebServices/ModuleService.asmx/CheckDuplicateModuleHomeworkOnly",
                    data: input,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        if (msg.d !== "") {
                            result = msg.d;
                        } else {
                            alert("false2")
                        }
                    },
                    error: function myfunction(request, stutus) {
                        alert(request.statusText);
                    }
                });

                if ((result.IsDup !== undefined) & (result.IsDup)) {
                    $("#DivMsgDup").html(result.Msg);
                    $("#DivMsgDup").dialog("open");
                    //$(".ui-dialog-titlebar").hide();
                } else {
                    checkUpdate();
                }
            } else {
                $("#DivMsgNotStudent").dialog("open");
                $("#DivMsgNotStudent").prev(".ui-dialog-titlebar").show();
            }
        }

        //function TriggerModuleItemClick(e) {
        //    var obj = e.target;
        //        $("#ModuleSelect").jqModuleSelectAddItem({ id: $(obj).data("id"), text: $(obj).data("text"), type: $(obj).data("type"), maId: null, mdId: null });
        //        if(isAndroid){
        //           $('.DivModuleSelectText').css('font-size', '30px');
        //           $('.DivModuleSelectClose').css({ 'width': '45px', 'height': '45px', 'top': '5px', 'left': '230px' });
        //        };
        //}

        function ChangeLayout(node) {
            var numDiv = $(".DivModuleParent > div:visible");
            var nodeCount = node
            if (node == undefined) {
                nodeCount = numDiv.length + 1;
            }
            if (nodeCount == 1) {
                //$(numDiv).css("width", "850px");
                $("#MdClass").animate({ width: 850 }, "fast");
            } else if (nodeCount == 2) {
                //$(".DivModuleParent > div:last").css("display", "none");
                //$(numDiv).css("width", "425px");
                if (isAndroid) {
                    $("#MdClass").animate({ width: 410 }, "fast");
                    $("#MdClass").queue(function () {
                        $("#MdRoom").show().animate({ width: 410 }, "fast");
                        $(this).dequeue();
                    });
                }
                else {
                    $("#MdClass").animate({ width: 425 }, "fast");
                    $("#MdClass").queue(function () {
                        $("#MdRoom").show().animate({ width: 425 }, "fast");
                        $(this).dequeue();
                    });
                }
            } else if (nodeCount == 3) {
                if (isAndroid) {
                    $("#MdClass").animate({ width: 265 }, "fast");
                    $("#MdRoom").animate({ width: 265 }, "fast");
                    $("#MdClass").queue(function () {
                        $("#MdStudent").show().animate({ width: 265 }, "fast");
                        $(this).dequeue();
                    });
                }
                else {
                    $("#MdClass").animate({ width: 275 }, "fast");
                    $("#MdRoom").animate({ width: 275 }, "fast");
                    $("#MdClass").queue(function () {
                        $("#MdStudent").show().animate({ width: 275 }, "fast");
                        $(this).dequeue();
                    });
                }
                //$(numDiv).css("width", "275px");

            } else {
                //$(numDiv).css("width", "200px");
                //$(numDiv).animate({ width: 200 }, "fast");
            }
        }

        function saveAssignment() {
            var input;
            if (isNew) {
                input = JSON.stringify({
                    MaId: maId, SchoolCode: '<%= SchoolCode %>',
                    InsertModule: $("#ModuleSelect").jqModuleSelectGetMdItemInsertForClone(),
                    InsertAssignment: $("#ModuleSelect").jqModuleSelectGetMaItemInsertForClone(),
                    DeleteModule: [],
                    DeleteAssignment: [],
                    EditAssignment: [],
                    AssignTo: $("#ModuleSelect").jqModuleSelectGetAllAssignText().join(",")
                })
            } else {
                input = JSON.stringify({
                    MaId: maId, SchoolCode: '<%= SchoolCode %>',
                    InsertModule: $("#ModuleSelect").jqModuleSelectGetMdItemInsert(),
                    InsertAssignment: $("#ModuleSelect").jqModuleSelectGetMaItemInsert(),
                    DeleteModule: $("#ModuleSelect").jqModuleSelectGetMdItemDelete(),
                    DeleteAssignment: $("#ModuleSelect").jqModuleSelectGetMaItemDelete(),
                    EditAssignment: $("#ModuleSelect").jqModuleSelectGetMaItemEdit(),
                    AssignTo: $("#ModuleSelect").jqModuleSelectGetAllAssignText().join(",")
                })
            };

            $.ajax({
                type: "POST",
                url: "../WebServices/ModuleService.asmx/SaveModuleHomeworkOnly",
                data: input,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    if (msg.d !== "") {
                        var result = $.parseJSON(msg.d);
                        maId = result.MdId;
                        isNew = false;
                        $("#ModuleSelect").jqModuleSelectClearItem();
                        var mergeSetTime = { id: null, text: null, type: enMdType.MdModuleHomeworkTime, maId: null, mdId: null };
                        $.each(result.AssignListHomeworkTime, function () {
                            // ถ้าเป็น module เวลา จะเก็บค่าต่างๆใส่ id เลย แต่ไม่ใช่ id นะมันคือ value
                            if (this.text == "start") {
                                mergeSetTime.start = global.convertJsonDateToJsDate(this.id);
                                mergeSetTime.startMaId = this.maId;
                            }
                            if (this.text == "end") {
                                mergeSetTime.end = global.convertJsonDateToJsDate(this.id);
                                mergeSetTime.endMaId = this.maId;
                            }
                        });
                        $("#ModuleSelect").jqModuleSelectLoadItem([mergeSetTime], false);
                        $("#ModuleSelect").jqModuleSelectLoadItem(result.ModuleList, false);
                        $("#ModuleSelect").jqModuleSelectLoadItem(result.AssignList, true);
                        $("#DivMsgSave").dialog("open");
                        // เป็นตัว set ว่าจะให้แสดงหัว title ด้วยหรือเปล่า
                        //$(".ui-dialog-titlebar").hide();
                        $("#DivMsgSave").prev(".ui-dialog-titlebar").show();
                    } else {
                        alert("false3")
                    }
                },
                error: function myfunction(request, stutus) {
                    alert(request.statusText);
                }
            });
        }

        function checkUpdate() {
            // เช็คการเลือกซ้ำคล่อมเวลา
            var input = JSON.stringify({ MaId: maId, Assignment: $("#ModuleSelect").jqModuleSelectGetAll() });
            var result;
            $.ajax({
                type: "POST",
                async: false,
                url: "../WebServices/ModuleService.asmx/CheckeUpdateModuleHomeworkOnly",
                data: input,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    if (msg.d !== "") {
                        result = msg.d;
                        if ((result.MsgIn == undefined) || (result.MsgIn.length == 0 & result.MsgOut.length == 0)) {
                            saveAssignment();
                        } else {
                            createUpdatePanel(result);
                        }
                    } else {
                        alert("false1")
                    }
                },
                error: function myfunction(request, stutus) {
                    alert(request.statusText);
                }
            });
        }

        function createUpdatePanel(result) {
            if (result.MsgIn.length == 0) {
                $("#DivMsgUpdate .ForBigDiv:eq(0)").hide();
            } else {
                createDetailUpdatePanel($("#DivMsgUpdate .ForBigDiv:eq(0)"), result.MsgIn);
            }
            if (result.MsgOut.length == 0) {
                $("#DivMsgUpdate .ForBigDiv:eq(1)").hide();
            } else {
                createDetailUpdatePanel($("#DivMsgUpdate .ForBigDiv:eq(1)"), result.MsgOut);
            }
            if (result.CountStudentDone > 0) {
                var div = $("#DivMsgUpdate .ForBigDiv:eq(1) div:eq(0)");
                var msg = "ข้อมูลที่โดนลบออก (มีนักเรียนที่ทำแล้ว " + result.CountStudentDone + " คน )"
                div.text(msg);
            }
            $("#DivMsgUpdate").dialog("open")
            $(".ui-dialog-titlebar").hide();
        }

        function createDetailUpdatePanel(parent, result) {
            parent.show();
            parent.children().eq(1).html("");
            for (var i = 0; i < result.length; i++) {
                parent.children().eq(1).append($("<div>").addClass("ForSmallDetailDiv").text(result[i]));
            }
        }

    </script>
    <style type="text/css">
        .SetDateDialog {
            width: 726px !Important;
            top: 60px !Important;
        }

        #DivTime {
            width: 726px !Important;
        }

        .ui-draggable .ui-dialog-titlebar {
            display: none;
        }

        .time_pick .timepicker_wrap {
            width: 160px !Important;
            top: -150px !important;
        }

        .time_pick .arrow_top {
            top: initial !important;
            bottom: -10px !important;
            transform: rotate(-180deg);
        }

        time_pick {
            text-align: center !important;
        }

        .HomeworkTime {
            background-color: green;
            opacity: 0.8;
            color: white !important;
        }
    </style>

    <script src="../js/timepicki.js" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" href="../css/timepicki.css">
   <link href="../css/jquery.qtip.css" rel="stylesheet" />
    <script src="../js/jquery.qtip-1.0.0-rc3.js" type="text/javascript"></script>
    <script type="text/javascript">

        $(function () {

            $(".time_element").timepicki({
                start_time: ["23", "59", "AM"],
                show_meridian: false,
                min_hour_value: 0,
                max_hour_value: 23,
                max: ["23", "59", "AM"],
                step_size_minutes: 5,
                overflow_minutes: true,
                increase_direction: 'up',
                disable_keyboard_mobile: true
            });

            // show qtip ชื่อชุด
            $('.DivModuleSelectText').qtip({
                content: $('.DivModuleSelectText').text(),
                show: { event: 'mouseover' },
                style: {
                    width: 500, padding: 15, background: '#F68500', color: 'white', textAlign: 'center',
                    border: { width: 7, radius: 5, color: '#F68500' }, tip: 'bottomMiddle', name: 'dark', 'font-weight': 'bold', 'font-size': '18px', 'line-height': '1.5em'
                },
                position: { corner: { tooltip: 'bottomMiddle', target: 'topMiddle' } },
                hide: { when: { event: 'mouseout' }, fixed: false }
            });
        })
    </script>
</asp:Content>
