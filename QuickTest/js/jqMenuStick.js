/// <reference path="/JqueryDoc.js" />

(function ($) {

    $.fn.jqMenuStick = function () {
        return this.each(function () {
            var mainMenu = this;
            //            var divNode = '<div class="msNode"><img src="../Images/expand_right.png" /></div>';
            //            $(this).append(divNode);
            closeMainMenu(this);

            //event
            $(this).mouseenter(function () {
                openMainMenu(this);
            })
            .mouseleave(function () {
                    closeMainMenu(this);
            });

            $(".menuAcdItem").mouseenter(function () {
                var name = $(this).data("id");
                $(".msSubMenu > div").hide();
                var sub = $(".msSubMenu");
                var divFound = $("div[data-id=" + name + "]");
                divFound.show();
                if (divFound.length > 0) {
                    openSubMenu(sub);
                }
            })
            .mouseleave(function () {
                var sub = $(".msSubMenu");
                closeSubMenu(sub);
            })

            $(".msSubMenu").mouseenter(function () {
                var sub = $(".msSubMenu");
                openSubMenu(sub);
                $(mainMenu).trigger("mouseenter")
            })
            .mouseleave(function () {
                closeMainMenu(mainMenu);
                var sub = $(".msSubMenu");
                closeSubMenu(sub);
            })


            $(".msMainDesc,.msSubDesc").click(function (event) {
                event.stopImmediatePropagation();
            });


            $(".msNode").click(function () {
                closeMainMenu(mainMenu);
            });

            //            $(this).click(function () {
            //                closeMainMenu(this);
            //            });
            //            $(".msMainDesc,.msSubDesc").mouseenter(function() {
            //                var name = $(this).attr("name");
            //                $(".msWrapSub[name=" + name + //"]").stop().slideDown("medium");
            //            }).mouseleave(function() {
            //               var name = $(this).attr("name");
            //                $(".msWrapSub[name=" + name + //"]").stop().slideUp("medium");
            //            });
        });
    }

    $.fn.jqMenuStickSetPage = function (exceptPage) {
        return this.each(function () {
            var myControl = $(this);
            $.each(exceptPage, function (index, value) {
                var item = $(myControl).find("div[onclick*='" + value + "']")
                if (item.length > 0) {
                    $(item).hide();
                }
            })
        });
    }

    function openMainMenu(main) {
        $(main).stop().animate({
            "left": "0"
        }, 0);
        $(".msMainDesc").fadeIn();
        $(".msWrapSub").fadeIn();
        $(".msNode").fadeIn();
    }

    function closeMainMenu(main) {
        $(main).stop().animate({
            "left": "-150"
        }, 0);
        $(".msMainDesc").fadeOut();
        $(".msWrapSub").fadeOut();
    }

    function openSubMenu(sub) {
        $(sub).show();
        $(sub).stop().animate({
            "left": "170"
        }, 0);
    }

    function closeSubMenu(sub) {
        $(sub).fadeOut(50);
    }

})(jQuery);