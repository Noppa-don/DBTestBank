/// <reference path="../jquery-1.4.1-vsdoc.js" />
/// <reference path="jqModuleItem.js" />

// id เก็บ id จริงของแต่ละเรื่องของ Module ที่เลือกจากด้านบน
// item ที่เลือกแบ่งเป็นสองกลุ่ม กลุ่ม Module(มากจาก ModuleDetail) mdId จะมีค่า  และ กุลุ่ม Assignment(มากจาก ModuleAssignment) maId จะมีค่า
// ถ้า Item ตัวนั้น เป็น maId=null , mdId=null ใหม่
var enMdTypeImg = {
    MdModule: "../Images/Homework/HomeworkIcon.png",
    MdClass: "../Images/Homework/ClassIcon.png",
    MdRoom: "../Images/Homework/RoomIcon.png",
    MdStudent: "../Images/Homework/StudentIcon.png",
    MdModuleHomeworkTime: "",
    getImage: function (val) {
        switch (val) {
            case enMdType.MdModule: return this.MdModule;
            case enMdType.MdClass: return this.MdClass;
            case enMdType.MdRoom: return this.MdRoom;
            case enMdType.MdStudent: return this.MdStudent;
            case enMdType.MdModuleHomeworkTime: return this.MdModuleHomeworkTime;
        }
    }
};

(function ($) {
    var settings = {
        onSetTimeHomework: function () {
            // ทำงานตอนกด panel ตั้งเวลาการบ้าน
        }
    };
    var myjqModuleSelect;
    var deleteMdList = [];
    var deleteMaList = [];

    // setting
    $.fn.jqModuleSelect = function (options) {
        myjqModuleSelect = this;
        settings = $.extend(settings, options);

        return this.each(function () {
            $(myjqModuleSelect).addClass("DivModuleSelectParent");
        })
    }

    //  data = {id,text,type,maId,mdId}
    $.fn.jqModuleSelectAddItem = function (data, showDelete) {
        return this.each(function () {
            var isFound = false;
            $.each($(".DivModuleSelect"), function () {
                if ($(this).data("id") == data.id) {
                    isFound = true;
                }
            });
            if (!isFound) {
                if (showDelete == undefined) {
                    showDelete = true
                }
                if (data.type !== enMdType.MdModuleHomeworkTime) {
                    $(myjqModuleSelect).append(createItem(data.id, data.text, enMdTypeImg.getImage(data.type), data.type, data.maId, data.mdId, showDelete));
                } else if (data.type == enMdType.MdModuleHomeworkTime) {
                    // ถ้าเป็นการตั้งเวลาการบ้าน ใหส่งเป็น property เพิ่มเข้ามามี start, end, auto เอาค่าใส่ property หล่าวนี้เข้ามาเลย
                    var textTime = homeworkTimeToText(data.start, data.end);
                    //var textAuto = homeworkAutoToText(data.auto);
                    var optional = {
                        start: { maId: null, val: data.start },
                        end: { maId: null, val: data.end }
                        //auto: { maId: null, val: data.auto }
                    }
                    $(myjqModuleSelect).append(createItem(data.id, textTime  , "", data.type, data.maId, data.mdId, showDelete, optional));
                }
            }

        })
    }
    //  data = [{id,text,type,maId,mdId}] maId=id ที่ตาราง Module Assignmentม mdId= id ที่ตาราง Module
    $.fn.jqModuleSelectLoadItem = function (data, showDelete) {
        return this.each(function () {
            $.each(data, function () {
                if (showDelete == undefined) {
                    showDelete = true
                }
                if (this.type !== enMdType.MdModuleHomeworkTime) {
                    $(myjqModuleSelect).append(createItem(this.id, this.text, enMdTypeImg.getImage(this.type), this.type, this.maId, this.mdId, showDelete));
                } else if (this.type == enMdType.MdModuleHomeworkTime) {
                    // ถ้าเป็นการตั้งเวลาการบ้าน ใหส่งเป็น property เพิ่มเข้ามามี start, end, auto เอาค่าใส่ property หล่าวนี้เข้ามาเลย
                    // และนำ id ค่่าหล่าวนี้เข้าไปเก็บตามชื่อ startMaId, endMaId, autoMaId
                    var textTime = homeworkTimeToText(this.start, this.end);
                    //var textAuto = homeworkAutoToText(this.auto);
                    var optional = {
                        start: { maId: this.startMaId, val: this.start },
                        end: { maId: this.endMaId, val: this.end }
                        //auto: { maId: this.autoMaId, val: this.auto }
                    }
                    $(myjqModuleSelect).append(createItem(this.id, textTime , "", this.type, this.maId, this.mdId, showDelete, optional));
                }
            });
        })
    }
    //  ล้าง item ทิ้งให้หมด
    $.fn.jqModuleSelectClearItem = function () {
        return this.each(function () {
            $(".DivModuleSelect").remove();
            deleteMdList = [];
            deleteMaList = [];
        })
    }
    //  เอาทุกอย่างที่ตั่งค่าไว้จะไปเช็คเรื่องซ้ำ
    $.fn.jqModuleSelectGetAll = function () {
        var listId = [];
        $.each($(".DivModuleSelect"), function () {
            if ( $(this).data("type") == enMdType.MdModule) {
                listId.push({ id: $(this).data("id"), text: $(this).data("text"), type: $(this).data("type"), mdId: $(this).data("mdId") });
            }
            if (($(this).data("type") !== enMdType.MdModule & $(this).data("type") !== enMdType.MdModuleHomeworkTime)) {
                listId.push({ id: $(this).data("id"), text: $(this).data("text"), type: $(this).data("type"), maId: $(this).data("maId") });
            }
            if ($(this).data("type") == enMdType.MdModuleHomeworkTime ) {
                listId.push({ id: $(this).data("start").val, text: "start", type: $(this).data("type"), maId: $(this).data("maId") });
                listId.push({ id: $(this).data("end").val, text: "end", type: $(this).data("type"), maId: $(this).data("maId") });
            }
        });
        return listId;
    }
    //  เอาทุกอย่างที่ตั่งค่าไว้เฉพาะ text
    $.fn.jqModuleSelectGetAllAssignText = function () {
        var listId = [];
        $.each($(".DivModuleSelect"), function () {
            if (($(this).data("type") !== enMdType.MdModule) & ($(this).data("type") !== enMdType.MdModuleHomeworkTime))   {
                listId.push($(this).data("text"));
            }
        });
        return listId;
    }
    //  Get ค่า id ที่โดนเพิ่ม Module
    $.fn.jqModuleSelectGetMdItemInsert = function () {
        var listId = [];
        $.each($(".DivModuleSelect"), function () {
            if ($(this).data("mdId") == null & $(this).data("type") == enMdType.MdModule) {
                listId.push({ id: $(this).data("id"), text: $(this).data("text"), type: $(this).data("type"), mdId: $(this).data("mdId") });
            }
        });
        return listId;
    }
    // Get ค่า id ที่โดนลบกลุ่ม Module
    $.fn.jqModuleSelectGetMdItemDelete = function () {
        return deleteMdList;
    }
    //  Get ค่า id ที่โดนเพิ่ม Assignment
    $.fn.jqModuleSelectGetMaItemInsert = function () {
        var listId = [];
        $.each($(".DivModuleSelect"), function () {
            if ($(this).data("maId") == null & ($(this).data("type") !== enMdType.MdModule & $(this).data("type") !== enMdType.MdModuleHomeworkTime)) {
                listId.push({ id: $(this).data("id"), text: $(this).data("text"), type: $(this).data("type"), maId: $(this).data("maId") });
            }
            if ($(this).data("type") == enMdType.MdModuleHomeworkTime ) {
                if ($(this).data("start").maId == null) {
                    listId.push({ id: $(this).data("start").val, text: "start", type: $(this).data("type"), maId: $(this).data("maId") });
                    listId.push({ id: $(this).data("end").val, text: "end", type: $(this).data("type"), maId: $(this).data("maId") });
                    //listId.push({ id: $(this).data("auto").val, text: "auto", type: $(this).data("type"), maId: $(this).data("maId") });
                }
            }
        });
        return listId;
    }
    // Get ค่า id ที่โดนลบกลุ่ม Assignment
    $.fn.jqModuleSelectGetMaItemDelete = function () {
        return deleteMaList;
    }
     //  Get ค่า id ที่โดนแก้ไขเรื่องตั้งค่าเวลาการบ้าน
    $.fn.jqModuleSelectGetMaItemEdit = function () {
        var listId = [];
        $.each($(".DivModuleSelect"), function () {
            if ($(this).data("edit")==true & $(this).data("type") == enMdType.MdModuleHomeworkTime) {
                listId.push({ id: $(this).data("start").val, text: "start", type: $(this).data("type"), maId: $(this).data("start").maId });
                listId.push({ id: $(this).data("end").val, text: "end", type: $(this).data("type"), maId:  $(this).data("end").maId });
                //listId.push({ id: $(this).data("auto").val, text: "auto", type: $(this).data("type"), maId:  $(this).data("auto").maId });
            }
        });
        return listId;
    }

    //  Get ค่า id ที่โดนเพิ่ม Module
    $.fn.jqModuleSelectGetMdItemInsertForClone = function () {
        var listId = [];
        $.each($(".DivModuleSelect"), function () {
            if ($(this).data("type") == enMdType.MdModule) {
                listId.push({ id: $(this).data("id"), text: $(this).data("text"), type: $(this).data("type"), mdId: $(this).data("mdId") });
            }
        });
        return listId;
    }
    //  Get ค่า id ที่โดนเพิ่ม Assignment
    $.fn.jqModuleSelectGetMaItemInsertForClone = function () {
        var listId = [];
        $.each($(".DivModuleSelect"), function () {
            if ($(this).data("type") !== enMdType.MdModule & $(this).data("type") !== enMdType.MdModuleHomeworkTime) {
                listId.push({ id: $(this).data("id"), text: $(this).data("text"), type: $(this).data("type"), maId: $(this).data("maId") });
            }
            if ($(this).data("type") == enMdType.MdModuleHomeworkTime ) {
                    listId.push({ id: $(this).data("start").val, text: "start", type: $(this).data("type"), maId: $(this).data("maId") });
                    listId.push({ id: $(this).data("end").val, text: "end", type: $(this).data("type"), maId: $(this).data("maId") });
                    //listId.push({ id: $(this).data("auto").val, text: "auto", type: $(this).data("type"), maId: $(this).data("maId") });
            }
        });
        return listId;
    }


    // คืนตัวเอง
    $.fn.jqModuleSelectGetMy = function () {
        return myjqModuleSelect;
    }
    // ใส่ค่าให้ ตั้งค่าเวลาการบ้าน
    $.fn.jqModuleSelectSetTimeHomework = function (start,end) {
        var divTimeHomework = $.fn.jqModuleSelectGetMy().find(".HomeworkTime").closest(".DivModuleSelect");
        divTimeHomework.data("edit",true);
        divTimeHomework.data("start").val  = start;
        divTimeHomework.data("end").val  = end;
        //divTimeHomework.data("auto").val  = auto;

        var textTime = homeworkTimeToText(start, end);
        //var textAuto = homeworkAutoToText(auto);
        $.fn.jqModuleSelectGetMy().find(".HomeworkTime").html(textTime );
    }

    function createItem(id, text, src, type, maId, mdId, showDelete, optional) {
        var div = $("<div>").addClass("DivModuleSelect").addClass(type);
        var divImg = $("<img>").addClass("DivModuleSelectImg").prop("src", src).css({ width: "90px", height: "50px" });
        var divClose = $("<img>").addClass("DivModuleSelectClose").prop("src", "../Images/Close.png").css({ width: "25px", height: "25px" });
        var divIext;
        // add class ให้ div แสดงผล text
        if (type !== enMdType.MdModuleHomeworkTime) {
            divIext = $("<div>").addClass("DivModuleSelectText");
        } else {
            divIext = $("<div>").addClass("DivModuleSelectTextFull HomeworkTime");
            divIext.click(function () {
                settings.onSetTimeHomework();
            });
        }
        divClose.click(function () {
            if ($(this).closest(".DivModuleSelect").data("type") == enMdType.MdModule & $(this).closest(".DivModuleSelect").data("mdId") !== null) {
                deleteMdList.push($(this).closest(".DivModuleSelect").data("mdId"));
            }
            if ($(this).closest(".DivModuleSelect").data("type") !== enMdType.MdModule & $(this).closest(".DivModuleSelect").data("maId") !== null) {
                deleteMaList.push($(this).closest(".DivModuleSelect").data("maId"));
            }
            $(this).closest(".DivModuleSelect").remove();
        });
        div.data("id", id);
        div.data("text", text);
        div.data("type", type);
        div.data("maId", maId);
        div.data("mdId", mdId);
        div.data("edit", false)
        if (type == enMdType.MdModuleHomeworkTime) {
             // ถ้าเป็นตั้งเวลาการบ้านรูปแบบการเก็บดาต้าจะต่างกับคนอื่น {val,maId} val คือค่า maId ถ้าเป็นเคสแก้ไขจะมีค่า maId มาด้วย
            div.data("start", optional.start);
            div.data("end", optional.end);
            //div.data("auto", optional.auto);
        }
        divIext.append(text);
        if (type !== enMdType.MdModuleHomeworkTime) {
            // ถ้าเป็นตั้งเวลาการบ้านไม่ต้องใส่ รูปปิด
            div.append(divImg);
        }
        div.append(divIext)
        if (showDelete) {
            div.append(divClose);
        }

        return div;
    }

    function homeworkTimeToText(start, end) {
        if (start.getMonth() == end.getMonth()) {
            return "ทำในช่วง " + start.getDate() + " - " + end.getDate() + "/" + (end.getMonth()+1) + "/" + (end.getFullYear() + 543)
        } else {
            return "ทำในช่วง " + start.getDate() + "/" + (start.getMonth()+1) + " - " + end.getDate() + "/" + (end.getMonth()+1) + "/"+ (end.getFullYear() + 543)
        }
    }

    function homeworkAutoToText(auto) {
        if (auto) {
            return "ตรวจการบ้านอัตโนมัติ"
        } else {
            return "ไม่ตรวจการบ้านอัตโนมัติ"
        }
    }

})(jQuery);