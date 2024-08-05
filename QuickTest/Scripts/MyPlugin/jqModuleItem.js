/// <reference path="../jquery-1.4.1-vsdoc.js" />
/// <reference path="jqModuleSelect.js" />

var enMdType = {
    MdModule: "MdModule",
    MdClass: "MdClass",
    MdRoom: "MdRoom",
    MdStudent: "MdStudent",
    MdModuleHomeworkTime: "MdModuleHomeworkTime"
};

(function ($) {
    var settings = {
        onItemSelect: function () {
            // กดเลือก
        },
        onSearch: function () {
            // กดปุ่มค้นหา
        }
    };
    var myjqModuleItem;

    // setting
    $.fn.jqModuleItem = function (options) {
        myjqModuleItem = this;
        settings = $.extend(settings, options);

        return this.each(function () {
            var PanelDetails = [{
                "id": "MdModule",
                "backgroundColor": "rgb(0, 185, 232)",
                "color": "rgb(168, 168, 168)",
                "width": 250
            }, {
                "id": "MdClass",
                "backgroundColor": "rgb(255, 181, 103)",
                "color": "rgb(147, 112, 31)",
                "width": 250
            }, {
                "id": "MdRoom",
                "backgroundColor": "rgb(255, 223, 94)",
                "color": " rgb(106, 75, 59)",
                "width": 250
            }, {
                "id": "MdStudent",
                "backgroundColor": "rgb(255, 157, 233)",
                "color": "rgb(51, 51, 51)",
                "width": 250
            }]

            $(myjqModuleItem).addClass("DivModuleParent");
            $.each(PanelDetails, function () {
                $(myjqModuleItem).append(createPanel(this.id, this.backgroundColor, this.color, this.width));
            });
        })
    }

    // data = {id,text}
    $.fn.jqModuleItemLoadData = function (_enMdType, data, showSearch) {
        return this.each(function () {
            var parent = $("#" + _enMdType + " div");
            parent.find(".DivModuleItem").remove();
            $.each(data, function () {
                var item = createItem(_enMdType, showSearch);
                item.append(this.text);
                item.data("id", this.id);
                item.data("text", this.text);
                item.data("type", _enMdType);
                item.bind("click", settings.onItemSelect);

                parent.append(item);
            });
        })
    }

    // ซ่อน panel (enMdTypeList = enMdType[])
    $.fn.jqModuleItemHidePanel = function (enMdTypeList) {
        return this.each(function () {
            $.each(enMdTypeList, function () {
                var parent = $("#" + this);
                parent.hide();
            });
        })
    }

    function createPanel(id, backgroundColor, color, width) {
        var divPanel = $("<div>").addClass("DivModulePanel").attr("id", id).css("background-color", backgroundColor).css("color", "#880505 !important").width(width);
        var divSubPanel = $("<div>").addClass("DivModuleSubPanel").css("background-color", backgroundColor);
        divPanel.append(divSubPanel);
        return divPanel
    }

    function createItem(typeModule, showSearch) {
        var divItem;
        if (typeModule == enMdType.MdModule) {
            divItem = $("<div>").addClass("DivModuleItem").css("background-color", "#A4E6F7")
                      .css("color", "#007B9A !important").css("border-bottom-color", "#00B9E8").css("border-top-color", "#00B9E8");
            divItem.data("src", enMdTypeImg.MdModule);
        } else if (typeModule == enMdType.MdClass) {
            divItem = $("<div>").addClass("DivModuleItem").css("background-color", "rgb(255, 226, 195)")
                      .css("color", "#880505 !important").css("border-bottom-color", "rgb(255, 226, 195)").css("border-top-color", "rgb(255, 226, 195)");
            divItem.data("src", enMdTypeImg.MdClass);
        } else if (typeModule == enMdType.MdRoom) {
            divItem = $("<div>").addClass("DivModuleItem").css("background-color", "#FEA")
                      .css("color", "#A98700 !important").css("border-bottom-color", "#FEA").css("border-top-color", "#FEA");
            divItem.data("src", enMdTypeImg.MdRoom);
        } else if (typeModule == enMdType.MdStudent) {
            divItem = $("<div>").addClass("DivModuleItem").css("background-color", "rgb(255, 194, 241)")
                      .css("color", "rgb(190, 6, 150) !important").css("border-bottom-color", "rgb(255, 194, 241)").css("border-top-color", "rgb(255, 194, 241)");
            divItem.data("src", enMdTypeImg.MdStudent);
        }
        if (showSearch) {
            var searchImg = $("<img>").addClass("DivModuleItemSubImage").attr("src", "../Images/Homework/OpenDetail.png").css({width : "32px",height : "32px"});
            searchImg.bind("click", settings.onSearch);
            searchImg.click(function (event) {
                event.stopImmediatePropagation();
            });
            divItem.append(searchImg);
        }

        return divItem
    }

})(jQuery);