﻿/*! jQuery Mobile v1.1.2 9a15f1aaf99faa7913103f5ea19ef6959b73d763 jquerymobile.com | jquery.org/license */
(function (l, s, k) { typeof define === "function" && define.amd ? define(["jquery"], function (C) { k(C, l, s); return C.mobile }) : k(l.jQuery, l, s) })(this, document, function (l, s, k, C) {
    (function (a, c, b, d) {
        function e(a) { for (; a && typeof a.originalEvent !== "undefined";) a = a.originalEvent; return a } function f(b) { for (var e = {}, g, c; b;) { g = a.data(b, t); for (c in g) if (g[c]) e[c] = e.hasVirtualBinding = true; b = b.parentNode } return e } function g() { z && (clearTimeout(z), z = 0); z = setTimeout(function () { G = z = 0; B.length = 0; D = false; H = true }, a.vmouse.resetTimerDuration) }
        function h(b, g, c) {
            var j, h; if (!(h = c && c[b])) { if (c = !c) a: { for (c = g.target; c;) { if ((h = a.data(c, t)) && (!b || h[b])) break a; c = c.parentNode } c = null } h = c } if (h) {
                j = g; var c = j.type, f, D; j = a.Event(j); j.type = b; h = j.originalEvent; f = a.event.props; c.search(/^(mouse|click)/) > -1 && (f = u); if (h) for (D = f.length; D;) b = f[--D], j[b] = h[b]; if (c.search(/mouse(down|up)|click/) > -1 && !j.which) j.which = 1; if (c.search(/^touch/) !== -1 && (b = e(h), c = b.touches, b = b.changedTouches, c = c && c.length ? c[0] : b && b.length ? b[0] : d)) for (h = 0, len = x.length; h < len; h++) b = x[h],
                j[b] = c[b]; a(g.target).trigger(j)
            } return j
        } function j(b) { var e = a.data(b.target, v); if (!D && (!G || G !== e)) if (e = h("v" + b.type, b)) e.isDefaultPrevented() && b.preventDefault(), e.isPropagationStopped() && b.stopPropagation(), e.isImmediatePropagationStopped() && b.stopImmediatePropagation() } function q(b) { var c = e(b).touches, g; if (c && c.length === 1 && (g = b.target, c = f(g), c.hasVirtualBinding)) G = N++, a.data(g, v, G), z && (clearTimeout(z), z = 0), A = H = false, g = e(b).touches[0], y = g.pageX, s = g.pageY, h("vmouseover", b, c), h("vmousedown", b, c) }
        function p(a) { H || (A || h("vmousecancel", a, f(a.target)), A = true, g()) } function m(b) { if (!H) { var c = e(b).touches[0], d = A, j = a.vmouse.moveDistanceThreshold; A = A || Math.abs(c.pageX - y) > j || Math.abs(c.pageY - s) > j; flags = f(b.target); A && !d && h("vmousecancel", b, flags); h("vmousemove", b, flags); g() } } function w(a) { if (!H) { H = true; var b = f(a.target), c; h("vmouseup", a, b); if (!A && (c = h("vclick", a, b)) && c.isDefaultPrevented()) c = e(a).changedTouches[0], B.push({ touchID: G, x: c.clientX, y: c.clientY }), D = true; h("vmouseout", a, b); A = false; g() } } function n(b) {
            var b =
            a.data(b, t), e; if (b) for (e in b) if (b[e]) return true; return false
        } function k() { } function o(b) {
            var e = b.substr(1); return {
                setup: function () { n(this) || a.data(this, t, {}); a.data(this, t)[b] = true; l[b] = (l[b] || 0) + 1; l[b] === 1 && J.bind(e, j); a(this).bind(e, k); if (M) l.touchstart = (l.touchstart || 0) + 1, l.touchstart === 1 && J.bind("touchstart", q).bind("touchend", w).bind("touchmove", m).bind("scroll", p) }, teardown: function () {
                    --l[b]; l[b] || J.unbind(e, j); M && (--l.touchstart, l.touchstart || J.unbind("touchstart", q).unbind("touchmove", m).unbind("touchend",
                    w).unbind("scroll", p)); var c = a(this), g = a.data(this, t); g && (g[b] = false); c.unbind(e, k); n(this) || c.removeData(t)
                }
            }
        } var t = "virtualMouseBindings", v = "virtualTouchID", c = "vmouseover vmousedown vmousemove vmouseup vclick vmouseout vmousecancel".split(" "), x = "clientX clientY pageX pageY screenX screenY".split(" "), u = a.event.props.concat(a.event.mouseHooks ? a.event.mouseHooks.props : []), l = {}, z = 0, y = 0, s = 0, A = false, B = [], D = false, H = false, M = "addEventListener" in b, J = a(b), N = 1, G = 0; a.vmouse = {
            moveDistanceThreshold: 10, clickDistanceThreshold: 10,
            resetTimerDuration: 1500
        }; for (var K = 0; K < c.length; K++) a.event.special[c[K]] = o(c[K]); M && b.addEventListener("click", function (b) { var e = B.length, c = b.target, g, d, j, h, f; if (e) { g = b.clientX; d = b.clientY; threshold = a.vmouse.clickDistanceThreshold; for (j = c; j;) { for (h = 0; h < e; h++) if (f = B[h], j === c && Math.abs(f.x - g) < threshold && Math.abs(f.y - d) < threshold || a.data(j, v) === f.touchID) { b.preventDefault(); b.stopPropagation(); return } j = j.parentNode } } }, true)
    })(l, s, k); (function (a, c, b) {
        function d(a) {
            a = a || location.href; return "#" + a.replace(/^[^#]*#?(.*)$/,
            "$1")
        } var e = "hashchange", f = k, g, h = a.event.special, j = f.documentMode, q = "on" + e in c && (j === b || j > 7); a.fn[e] = function (a) { return a ? this.bind(e, a) : this.trigger(e) }; a.fn[e].delay = 50; h[e] = a.extend(h[e], { setup: function () { if (q) return false; a(g.start) }, teardown: function () { if (q) return false; a(g.stop) } }); g = function () {
            function g() { var b = d(), j = t(n); if (b !== n) o(n = b, j), a(c).trigger(e); else if (j !== n) location.href = location.href.replace(/#.*/, "") + j; h = setTimeout(g, a.fn[e].delay) } var j = {}, h, n = d(), k = function (a) { return a }, o =
            k, t = k; j.start = function () { h || g() }; j.stop = function () { h && clearTimeout(h); h = b }; a.browser.msie && !q && function () {
                var b, c; j.start = function () { if (!b) c = (c = a.fn[e].src) && c + d(), b = a('<iframe tabindex="-1" title="empty"/>').hide().one("load", function () { c || o(d()); g() }).attr("src", c || "javascript:0").insertAfter("body")[0].contentWindow, f.onpropertychange = function () { try { if (event.propertyName === "title") b.document.title = f.title } catch (a) { } } }; j.stop = k; t = function () { return d(b.location.href) }; o = function (c, g) {
                    var d = b.document,
                    j = a.fn[e].domain; if (c !== g) d.title = f.title, d.open(), j && d.write('<script>document.domain="' + j + '"<\/script>'), d.close(), b.location.hash = c
                }
            }(); return j
        }()
    })(l, this); (function (a, c) {
        if (a.cleanData) { var b = a.cleanData; a.cleanData = function (c) { for (var d = 0, g; (g = c[d]) != null; d++) a(g).triggerHandler("remove"); b(c) } } else {
            var d = a.fn.remove; a.fn.remove = function (b, c) {
                return this.each(function () {
                    c || (!b || a.filter(b, [this]).length) && a("*", this).add([this]).each(function () { a(this).triggerHandler("remove") }); return d.call(a(this),
                    b, c)
                })
            }
        } a.widget = function (b, c, g) { var d = b.split(".")[0], j, b = b.split(".")[1]; j = d + "-" + b; if (!g) g = c, c = a.Widget; a.expr[":"][j] = function (c) { return !!a.data(c, b) }; a[d] = a[d] || {}; a[d][b] = function (a, b) { arguments.length && this._createWidget(a, b) }; c = new c; c.options = a.extend(true, {}, c.options); a[d][b].prototype = a.extend(true, c, { namespace: d, widgetName: b, widgetEventPrefix: a[d][b].prototype.widgetEventPrefix || b, widgetBaseClass: j }, g); a.widget.bridge(b, a[d][b]) }; a.widget.bridge = function (b, d) {
            a.fn[b] = function (g) {
                var h =
                typeof g === "string", j = Array.prototype.slice.call(arguments, 1), q = this, g = !h && j.length ? a.extend.apply(null, [true, g].concat(j)) : g; if (h && g.charAt(0) === "_") return q; h ? this.each(function () { var d = a.data(this, b); if (!d) throw "cannot call methods on " + b + " prior to initialization; attempted to call method '" + g + "'"; if (!a.isFunction(d[g])) throw "no such method '" + g + "' for " + b + " widget instance"; var h = d[g].apply(d, j); if (h !== d && h !== c) return q = h, false }) : this.each(function () {
                    var c = a.data(this, b); c ? c.option(g || {})._init() :
                    a.data(this, b, new d(g, this))
                }); return q
            }
        }; a.Widget = function (a, b) { arguments.length && this._createWidget(a, b) }; a.Widget.prototype = {
            widgetName: "widget", widgetEventPrefix: "", options: { disabled: false }, _createWidget: function (b, c) { a.data(c, this.widgetName, this); this.element = a(c); this.options = a.extend(true, {}, this.options, this._getCreateOptions(), b); var g = this; this.element.bind("remove." + this.widgetName, function () { g.destroy() }); this._create(); this._trigger("create"); this._init() }, _getCreateOptions: function () {
                var b =
                {}; a.metadata && (b = a.metadata.get(element)[this.widgetName]); return b
            }, _create: function () { }, _init: function () { }, destroy: function () { this.element.unbind("." + this.widgetName).removeData(this.widgetName); this.widget().unbind("." + this.widgetName).removeAttr("aria-disabled").removeClass(this.widgetBaseClass + "-disabled ui-state-disabled") }, widget: function () { return this.element }, option: function (b, d) {
                var g = b; if (arguments.length === 0) return a.extend({}, this.options); if (typeof b === "string") {
                    if (d === c) return this.options[b];
                    g = {}; g[b] = d
                } this._setOptions(g); return this
            }, _setOptions: function (b) { var c = this; a.each(b, function (a, b) { c._setOption(a, b) }); return this }, _setOption: function (a, b) { this.options[a] = b; a === "disabled" && this.widget()[b ? "addClass" : "removeClass"](this.widgetBaseClass + "-disabled ui-state-disabled").attr("aria-disabled", b); return this }, enable: function () { return this._setOption("disabled", false) }, disable: function () { return this._setOption("disabled", true) }, _trigger: function (b, c, d) {
                var h = this.options[b], c = a.Event(c);
                c.type = (b === this.widgetEventPrefix ? b : this.widgetEventPrefix + b).toLowerCase(); d = d || {}; if (c.originalEvent) for (var b = a.event.props.length, j; b;) j = a.event.props[--b], c[j] = c.originalEvent[j]; this.element.trigger(c, d); return !(a.isFunction(h) && h.call(this.element[0], c, d) === false || c.isDefaultPrevented())
            }
        }
    })(l); (function (a, c) {
        a.widget("mobile.widget", {
            _createWidget: function () { a.Widget.prototype._createWidget.apply(this, arguments); this._trigger("init") }, _getCreateOptions: function () {
                var b = this.element, d = {};
                a.each(this.options, function (a) { var f = b.jqmData(a.replace(/[A-Z]/g, function (a) { return "-" + a.toLowerCase() })); f !== c && (d[a] = f) }); return d
            }, enhanceWithin: function (b, c) { this.enhance(a(this.options.initSelector, a(b)), c) }, enhance: function (b, c) { var e, f = a(b), f = a.mobile.enhanceable(f); c && f.length && (e = (e = a.mobile.closestPageData(f)) && e.keepNativeSelector() || "", f = f.not(e)); f[this.widgetName]() }, raise: function (a) { throw "Widget [" + this.widgetName + "]: " + a; }
        })
    })(l); (function (a, c, b) {
        var d = {}; a.mobile = a.extend({}, {
            version: "1.1.2",
            ns: "", subPageUrlKey: "ui-page", activePageClass: "ui-page-active", activeBtnClass: "ui-btn-active", focusClass: "ui-focus", ajaxEnabled: true, hashListeningEnabled: true, linkBindingEnabled: true, defaultPageTransition: "fade", maxTransitionWidth: false, minScrollBack: 250, touchOverflowEnabled: false, defaultDialogTransition: "pop", loadingMessage: "loading", pageLoadErrorMessage: "Error Loading Page", loadingMessageTextVisible: false, loadingMessageTheme: "a", pageLoadErrorMessageTheme: "e", phonegapNavigationEnabled: false, autoInitializePage: true,
            pushStateEnabled: true, ignoreContentEnabled: false, orientationChangeEnabled: true, buttonMarkup: { hoverDelay: 200 }, keyCode: { ALT: 18, BACKSPACE: 8, CAPS_LOCK: 20, COMMA: 188, COMMAND: 91, COMMAND_LEFT: 91, COMMAND_RIGHT: 93, CONTROL: 17, DELETE: 46, DOWN: 40, END: 35, ENTER: 13, ESCAPE: 27, HOME: 36, INSERT: 45, LEFT: 37, MENU: 93, NUMPAD_ADD: 107, NUMPAD_DECIMAL: 110, NUMPAD_DIVIDE: 111, NUMPAD_ENTER: 108, NUMPAD_MULTIPLY: 106, NUMPAD_SUBTRACT: 109, PAGE_DOWN: 34, PAGE_UP: 33, PERIOD: 190, RIGHT: 39, SHIFT: 16, SPACE: 32, TAB: 9, UP: 38, WINDOWS: 91 }, silentScroll: function (b) {
                if (a.type(b) !==
                "number") b = a.mobile.defaultHomeScroll; a.event.special.scrollstart.enabled = false; setTimeout(function () { c.scrollTo(0, b); a(k).trigger("silentscroll", { x: 0, y: b }) }, 20); setTimeout(function () { a.event.special.scrollstart.enabled = true }, 150)
            }, nsNormalizeDict: d, nsNormalize: function (b) { return !b ? void 0 : d[b] || (d[b] = a.camelCase(a.mobile.ns + b)) }, getInheritedTheme: function (a, b) {
                for (var c = a[0], d = "", e = /ui-(bar|body|overlay)-([a-z])\b/, f, w; c;) { if ((f = c.className || "") && (w = e.exec(f)) && (d = w[2])) break; c = c.parentNode } return d ||
                b || "a"
            }, closestPageData: function (a) { return a.closest(':jqmData(role="page"), :jqmData(role="dialog")').data("page") }, enhanceable: function (a) { return this.haveParents(a, "enhance") }, hijackable: function (a) { return this.haveParents(a, "ajax") }, haveParents: function (b, c) { if (!a.mobile.ignoreContentEnabled) return b; for (var d = b.length, e = a(), f, m, w, n = 0; n < d; n++) { m = b.eq(n); w = false; for (f = b[n]; f;) { if ((f.getAttribute ? f.getAttribute("data-" + a.mobile.ns + c) : "") === "false") { w = true; break } f = f.parentNode } w || (e = e.add(m)) } return e },
            getScreenHeight: function () { return c.innerHeight || a(c).height() }
        }, a.mobile); a.fn.jqmData = function (c, d) { var e; typeof c != "undefined" && (c && (c = a.mobile.nsNormalize(c)), e = arguments.length < 2 || d === b ? this.data(c) : this.data(c, d)); return e }; a.jqmData = function (b, c, d) { var e; typeof c != "undefined" && (e = a.data(b, c ? a.mobile.nsNormalize(c) : c, d)); return e }; a.fn.jqmRemoveData = function (b) { return this.removeData(a.mobile.nsNormalize(b)) }; a.jqmRemoveData = function (b, c) { return a.removeData(b, a.mobile.nsNormalize(c)) }; a.fn.removeWithDependents =
        function () { a.removeWithDependents(this) }; a.removeWithDependents = function (b) { b = a(b); (b.jqmData("dependents") || a()).remove(); b.remove() }; a.fn.addDependents = function (b) { a.addDependents(a(this), b) }; a.addDependents = function (b, c) { var d = a(b).jqmData("dependents") || a(); a(b).jqmData("dependents", a.merge(d, c)) }; a.fn.getEncodedText = function () { return a("<div/>").text(a(this).text()).html() }; a.fn.jqmEnhanceable = function () { return a.mobile.enhanceable(this) }; a.fn.jqmHijackable = function () { return a.mobile.hijackable(this) };
        var e = a.find, f = /:jqmData\(([^)]*)\)/g; a.find = function (b, c, d, q) { b = b.replace(f, "[data-" + (a.mobile.ns || "") + "$1]"); return e.call(this, b, c, d, q) }; a.extend(a.find, e); a.find.matches = function (b, c) { return a.find(b, null, null, c) }; a.find.matchesSelector = function (b, c) { return a.find(c, null, null, [b]).length > 0 }
    })(l, this); (function (a) {
        a(s); var c = a("html"); a.mobile.media = function () {
            var b = {}, d = a("<div id='jquery-mediatest'></div>"), e = a("<body>").append(d); return function (a) {
                if (!(a in b)) {
                    var g = k.createElement("style"),
                    h = "@media " + a + " { #jquery-mediatest { position:absolute; } }"; g.type = "text/css"; g.styleSheet ? g.styleSheet.cssText = h : g.appendChild(k.createTextNode(h)); c.prepend(e).prepend(g); b[a] = d.css("position") === "absolute"; e.add(g).remove()
                } return b[a]
            }
        }()
    })(l); (function (a, c) {
        function b(a) { var b = a.charAt(0).toUpperCase() + a.substr(1), a = (a + " " + g.join(b + " ") + b).split(" "), d; for (d in a) if (f[a[d]] !== c) return true } function d(a, b, c) {
            var d = k.createElement("div"), c = c ? [c] : g, e; for (i = 0; i < c.length; i++) {
                var j = c[i], h = "-" + j.charAt(0).toLowerCase() +
                j.substr(1) + "-" + a + ": " + b + ";", j = j.charAt(0).toUpperCase() + j.substr(1) + (a.charAt(0).toUpperCase() + a.substr(1)); d.setAttribute("style", h); d.style[j] && (e = true)
            } return !!e
        } var e = a("<body>").prependTo("html"), f = e[0].style, g = ["Webkit", "Moz", "O"], h = "palmGetResource" in s, j = s.opera, q = s.operamini && {}.toString.call(s.operamini) === "[object OperaMini]", p = s.blackberry; a.extend(a.mobile, { browser: {} }); a.mobile.browser.ie = function () {
            for (var a = 3, b = k.createElement("div"), c = b.all || []; b.innerHTML = "<\!--[if gt IE " + ++a +
            "]><br><![endif]--\>", c[0];); return a > 4 ? a : !a
        }(); a.extend(a.support, {
            orientation: "orientation" in s && "onorientationchange" in s, touch: "ontouchend" in k, cssTransitions: "WebKitTransitionEvent" in s || d("transition", "height 100ms linear") && !j, pushState: "pushState" in history && "replaceState" in history, mediaquery: a.mobile.media("only all"), cssPseudoElement: !!b("content"), touchOverflow: !!b("overflowScrolling"), cssTransform3d: d("perspective", "10px", "moz") || a.mobile.media("(-" + g.join("-transform-3d),(-") + "-transform-3d),(transform-3d)"),
            boxShadow: !!b("boxShadow") && !p, scrollTop: ("pageXOffset" in s || "scrollTop" in k.documentElement || "scrollTop" in e[0]) && !h && !q, dynamicBaseTag: function () { var b = location.protocol + "//" + location.host + location.pathname + "ui-dir/", c = a("head base"), d = null, j = "", g; c.length ? j = c.attr("href") : c = d = a("<base>", { href: b }).appendTo("head"); g = a("<a href='testurl' />").prependTo(e)[0].href; c[0].href = j || location.pathname; d && d.remove(); return g.indexOf(b) === 0 }(), cssPointerEvents: function () {
                var a = k.createElement("x"), b = k.documentElement,
                c = s.getComputedStyle; if (!("pointerEvents" in a.style)) return false; a.style.pointerEvents = "auto"; a.style.pointerEvents = "x"; b.appendChild(a); c = c && c(a, "").pointerEvents === "auto"; b.removeChild(a); return !!c
            }()
        }); e.remove(); h = function () { var a = s.navigator.userAgent; return a.indexOf("Nokia") > -1 && (a.indexOf("Symbian/3") > -1 || a.indexOf("Series60/5") > -1) && a.indexOf("AppleWebKit") > -1 && a.match(/(BrowserNG|NokiaBrowser)\/7\.[0-3]/) }(); a.mobile.gradeA = function () {
            return a.support.mediaquery || a.mobile.browser.ie && a.mobile.browser.ie >=
            7
        }; a.mobile.ajaxBlacklist = s.blackberry && !s.WebKitPoint || q || h; h && a(function () { a("head link[rel='stylesheet']").attr("rel", "alternate stylesheet").attr("rel", "stylesheet") }); a.support.boxShadow || a("html").addClass("ui-mobile-nosupport-boxshadow")
    })(l); (function (a, c, b) {
        function d(b, c, d) { var e = d.type; d.type = c; a.event.handle.call(b, d); d.type = e } a.each("touchstart touchmove touchend orientationchange throttledresize tap taphold swipe swipeleft swiperight scrollstart scrollstop".split(" "), function (b, c) {
            a.fn[c] =
            function (a) { return a ? this.bind(c, a) : this.trigger(c) }; a.attrFn[c] = true
        }); var e = a.support.touch, f = e ? "touchstart" : "mousedown", g = e ? "touchend" : "mouseup", h = e ? "touchmove" : "mousemove"; a.event.special.scrollstart = { enabled: true, setup: function () { function b(a, j) { e = j; d(c, e ? "scrollstart" : "scrollstop", a) } var c = this, e, g; a(c).bind("touchmove scroll", function (c) { a.event.special.scrollstart.enabled && (e || b(c, true), clearTimeout(g), g = setTimeout(function () { b(c, false) }, 50)) }) } }; a.event.special.tap = {
            setup: function () {
                var b =
                this, c = a(b); c.bind("vmousedown", function (e) { function g() { clearTimeout(o) } function h() { g(); c.unbind("vclick", f).unbind("vmouseup", g); a(k).unbind("vmousecancel", h) } function f(a) { h(); r == a.target && d(b, "tap", a) } if (e.which && e.which !== 1) return false; var r = e.target, o; c.bind("vmouseup", g).bind("vclick", f); a(k).bind("vmousecancel", h); o = setTimeout(function () { d(b, "taphold", a.Event("taphold", { target: r })) }, 750) })
            }
        }; a.event.special.swipe = {
            scrollSupressionThreshold: 10, durationThreshold: 1E3, horizontalDistanceThreshold: 30,
            verticalDistanceThreshold: 75, setup: function () {
                var c = a(this); c.bind(f, function (d) {
                    function e(b) { if (k) { var c = b.originalEvent.touches ? b.originalEvent.touches[0] : b; n = { time: (new Date).getTime(), coords: [c.pageX, c.pageY] }; Math.abs(k.coords[0] - n.coords[0]) > a.event.special.swipe.scrollSupressionThreshold && b.preventDefault() } } var f = d.originalEvent.touches ? d.originalEvent.touches[0] : d, k = { time: (new Date).getTime(), coords: [f.pageX, f.pageY], origin: a(d.target) }, n; c.bind(h, e).one(g, function () {
                        c.unbind(h, e); k && n &&
                        n.time - k.time < a.event.special.swipe.durationThreshold && Math.abs(k.coords[0] - n.coords[0]) > a.event.special.swipe.horizontalDistanceThreshold && Math.abs(k.coords[1] - n.coords[1]) < a.event.special.swipe.verticalDistanceThreshold && k.origin.trigger("swipe").trigger(k.coords[0] > n.coords[0] ? "swipeleft" : "swiperight"); k = n = b
                    })
                })
            }
        }; (function (a, b) {
            function c() { var a = e(); a !== g && (g = a, d.trigger("orientationchange")) } var d = a(b), e, g, h, f, l = { 0: true, 180: true }; if (a.support.orientation && (h = b.innerWidth || a(b).width(), f = b.innerHeight ||
            a(b).height(), h = h > f && h - f > 50, f = l[b.orientation], h && f || !h && !f)) l = { "-90": true, 90: true }; a.event.special.orientationchange = { setup: function () { if (a.support.orientation && a.mobile.orientationChangeEnabled) return false; g = e(); d.bind("throttledresize", c) }, teardown: function () { if (a.support.orientation && a.mobile.orientationChangeEnabled) return false; d.unbind("throttledresize", c) }, add: function (a) { var b = a.handler; a.handler = function (a) { a.orientation = e(); return b.apply(this, arguments) } } }; a.event.special.orientationchange.orientation =
            e = function () { var c = true, c = k.documentElement; return (c = a.support.orientation ? l[b.orientation] : c && c.clientWidth / c.clientHeight < 1.1) ? "portrait" : "landscape" }
        })(l, c); (function () { a.event.special.throttledresize = { setup: function () { a(this).bind("resize", b) }, teardown: function () { a(this).unbind("resize", b) } }; var b = function () { e = (new Date).getTime(); g = e - c; g >= 250 ? (c = e, a(this).trigger("throttledresize")) : (d && clearTimeout(d), d = setTimeout(b, 250 - g)) }, c = 0, d, e, g })(); a.each({
            scrollstop: "scrollstart", taphold: "tap", swipeleft: "swipe",
            swiperight: "swipe"
        }, function (b, c) { a.event.special[b] = { setup: function () { a(this).bind(c, a.noop) } } })
    })(l, this); (function (a) {
        a.widget("mobile.page", a.mobile.widget, {
            options: { theme: "c", domCache: false, keepNativeDefault: ":jqmData(role='none'), :jqmData(role='nojs')" }, _create: function () {
                var a = this; if (a._trigger("beforecreate") === false) return false; a.element.attr("tabindex", "0").addClass("ui-page ui-body-" + a.options.theme).bind("pagebeforehide", function () { a.removeContainerBackground() }).bind("pagebeforeshow",
                function () { a.setContainerBackground() })
            }, removeContainerBackground: function () { a.mobile.pageContainer.removeClass("ui-overlay-" + a.mobile.getInheritedTheme(this.element.parent())) }, setContainerBackground: function (c) { this.options.theme && a.mobile.pageContainer.addClass("ui-overlay-" + (c || this.options.theme)) }, keepNativeSelector: function () { var c = this.options; return c.keepNative && a.trim(c.keepNative) && c.keepNative !== c.keepNativeDefault ? [c.keepNative, c.keepNativeDefault].join(", ") : c.keepNativeDefault }
        })
    })(l);
    (function (a, c, b) {
        var d = function (d) {
            d === b && (d = true); return function (b, e, j, q) {
                var k = new a.Deferred, m = e ? " reverse" : "", l = a.mobile.urlHistory.getActive().lastScroll || a.mobile.defaultHomeScroll, n = a.mobile.getScreenHeight(), r = a.mobile.maxTransitionWidth !== false && a(c).width() > a.mobile.maxTransitionWidth, o = !a.support.cssTransitions || r || !b || b === "none" || Math.max(a(c).scrollTop(), l) > a.mobile.getMaxScrollForTransition(), t = function () {
                    a.mobile.pageContainer.toggleClass("ui-mobile-viewport-transitioning viewport-" +
                    b)
                }, v = function () { a.event.special.scrollstart.enabled = false; c.scrollTo(0, l); setTimeout(function () { a.event.special.scrollstart.enabled = true }, 150) }, x = function () { q.removeClass(a.mobile.activePageClass + " out in reverse " + b).height("") }, r = function () { q && d && x(); j.css("z-index", -10); j.addClass(a.mobile.activePageClass + " ui-page-pre-in"); a.mobile.focusPage(j); j.height(n + l); v(); j.css("z-index", ""); o || j.animationComplete(u); j.removeClass(" ui-page-pre-in").addClass(b + " in" + m); o && u() }, u = function () {
                    d || q && x(); j.removeClass("out in reverse " +
                    b).height(""); t(); a(c).scrollTop() !== l && v(); k.resolve(b, e, j, q, true)
                }; t(); q && !o ? (d ? q.animationComplete(r) : r(), q.height(n + a(c).scrollTop()).addClass(b + " out" + m)) : r(); return k.promise()
            }
        }, e = d(), d = d(false); a.mobile.defaultTransitionHandler = e; a.mobile.transitionHandlers = { "default": a.mobile.defaultTransitionHandler, sequential: e, simultaneous: d }; a.mobile.transitionFallbacks = {}; a.mobile.getMaxScrollForTransition = a.mobile.getMaxScrollForTransition || function () { return a.mobile.getScreenHeight() * 3 }
    })(l, this);
    (function (a, c) {
        function b(b) { l && (!l.closest(".ui-page-active").length || b) && l.removeClass(a.mobile.activeBtnClass); l = null } function d() { o = false; r.length > 0 && a.mobile.changePage.apply(null, r.pop()) } function e(b, c, d, e) {
            c && c.data("page")._trigger("beforehide", null, { nextPage: b }); b.data("page")._trigger("beforeshow", null, { prevPage: c || a("") }); a.mobile.hidePageLoadingMsg(); d && !a.support.cssTransform3d && a.mobile.transitionFallbacks[d] && (d = a.mobile.transitionFallbacks[d]); d = (a.mobile.transitionHandlers[d || "default"] ||
            a.mobile.defaultTransitionHandler)(d, e, b, c); d.done(function () { c && c.data("page")._trigger("hide", null, { nextPage: b }); b.data("page")._trigger("show", null, { prevPage: c || a("") }) }); return d
        } function f() { var b = a("." + a.mobile.activePageClass), c = parseFloat(b.css("padding-top")), d = parseFloat(b.css("padding-bottom")), e = parseFloat(b.css("border-top-width")), g = parseFloat(b.css("border-bottom-width")); b.css("min-height", z() - c - d - e - g) } function g(b, c) { c && b.attr("data-" + a.mobile.ns + "role", c); b.page() } function h(a) {
            for (; a;) {
                if (typeof a.nodeName ===
                "string" && a.nodeName.toLowerCase() == "a") break; a = a.parentNode
            } return a
        } function j(b) { var b = a(b).closest(".ui-page").jqmData("url"), c = u.hrefNoHash; if (!b || !m.isPath(b)) b = c; return m.makeUrlAbsolute(b, c) } var q = a(s); a("html"); var p = a("head"), m = {
            urlParseRE: /^(((([^:\/#\?]+:)?(?:(\/\/)((?:(([^:@\/#\?]+)(?:\:([^:@\/#\?]+))?)@)?(([^:\/#\?\]\[]+|\[[^\/\]@#?]+\])(?:\:([0-9]+))?))?)?)?((\/?(?:[^\/\?#]+\/+)*)([^\?#]*)))?(\?[^#]+)?)(#.*)?/, getLocation: function (a) {
                var b = a ? this.parseUrl(a) : location, a = this.parseUrl(a ||
                location.href).hash; return b.protocol + "//" + b.host + b.pathname + b.search + (a === "#" ? "" : a)
            }, parseLocation: function () { return this.parseUrl(this.getLocation()) }, parseUrl: function (b) {
                if (a.type(b) === "object") return b; b = m.urlParseRE.exec(b || "") || []; return {
                    href: b[0] || "", hrefNoHash: b[1] || "", hrefNoSearch: b[2] || "", domain: b[3] || "", protocol: b[4] || "", doubleSlash: b[5] || "", authority: b[6] || "", username: b[8] || "", password: b[9] || "", host: b[10] || "", hostname: b[11] || "", port: b[12] || "", pathname: b[13] || "", directory: b[14] || "", filename: b[15] ||
                    "", search: b[16] || "", hash: b[17] || ""
                }
            }, makePathAbsolute: function (a, b) { if (a && a.charAt(0) === "/") return a; for (var a = a || "", c = (b = b ? b.replace(/^\/|(\/[^\/]*|[^\/]+)$/g, "") : "") ? b.split("/") : [], d = a.split("/"), e = 0; e < d.length; e++) { var g = d[e]; switch (g) { case ".": break; case "..": c.length && c.pop(); break; default: c.push(g) } } return "/" + c.join("/") }, isSameDomain: function (a, b) { return m.parseUrl(a).domain === m.parseUrl(b).domain }, isRelativeUrl: function (a) { return m.parseUrl(a).protocol === "" }, isAbsoluteUrl: function (a) {
                return m.parseUrl(a).protocol !==
                ""
            }, makeUrlAbsolute: function (a, b) { if (!m.isRelativeUrl(a)) return a; b === c && (b = u); var d = m.parseUrl(a), e = m.parseUrl(b), g = d.protocol || e.protocol, h = d.protocol ? d.doubleSlash : d.doubleSlash || e.doubleSlash, j = d.authority || e.authority, f = d.pathname !== "", n = m.makePathAbsolute(d.pathname || e.filename, e.pathname); return g + h + j + n + (d.search || !f && e.search || "") + d.hash }, addSearchParams: function (b, c) {
                var d = m.parseUrl(b), e = typeof c === "object" ? a.param(c) : c, g = d.search || "?"; return d.hrefNoSearch + g + (g.charAt(g.length - 1) !== "?" ?
                "&" : "") + e + (d.hash || "")
            }, convertUrlToDataUrl: function (a) { var b = m.parseUrl(a); if (m.isEmbeddedPage(b)) return b.hash.split(t)[0].replace(/^#/, ""); else if (m.isSameDomain(b, u)) return b.hrefNoHash.replace(u.domain, "").split(t)[0]; return a }, get: function (a) { if (a === c) a = location.hash; return m.stripHash(a).replace(/[^\/]*\.[^\/*]+$/, "") }, getFilePath: function (b) { var c = "&" + a.mobile.subPageUrlKey; return b && b.split(c)[0].split(t)[0] }, set: function (a) { location.hash = a }, isPath: function (a) { return /\//.test(a) }, clean: function (a) {
                return a.replace(u.domain,
                "")
            }, stripHash: function (a) { return a.replace(/^#/, "") }, cleanHash: function (a) { return m.stripHash(a.replace(/\?.*$/, "").replace(t, "")) }, isHashValid: function (a) { return /^#[^#]+$/.test(a) }, isExternal: function (a) { a = m.parseUrl(a); return a.protocol && a.domain !== x.domain ? true : false }, hasProtocol: function (a) { return /^(:?\w+:)/.test(a) }, isFirstPageUrl: function (b) {
                var b = m.parseUrl(m.makeUrlAbsolute(b, u)), d = a.mobile.firstPage, d = d && d[0] ? d[0].id : c; return (b.hrefNoHash === x.hrefNoHash || F && b.hrefNoHash === u.hrefNoHash) &&
                (!b.hash || b.hash === "#" || d && b.hash.replace(/^#/, "") === d)
            }, isEmbeddedPage: function (a) { a = m.parseUrl(a); return a.protocol !== "" ? a.hash && (a.hrefNoHash === x.hrefNoHash || F && a.hrefNoHash === u.hrefNoHash) : /^#/.test(a.href) }, isPermittedCrossDomainRequest: function (b, c) { return a.mobile.allowCrossDomainPages && b.protocol === "file:" && c.search(/^https?:/) != -1 }
        }, l = null, n = {
            stack: [], activeIndex: 0, getActive: function () { return n.stack[n.activeIndex] }, getPrev: function () { return n.stack[n.activeIndex - 1] }, getNext: function () {
                return n.stack[n.activeIndex +
                1]
            }, addNew: function (a, b, c, d, e) { n.getNext() && n.clearForward(); n.stack.push({ url: a, transition: b, title: c, pageUrl: d, role: e }); n.activeIndex = n.stack.length - 1 }, clearForward: function () { n.stack = n.stack.slice(0, n.activeIndex + 1) }, directHashChange: function (b) { var d, e, g; this.getActive(); a.each(n.stack, function (a, c) { decodeURIComponent(b.currentUrl) === decodeURIComponent(c.url) && (d = a < n.activeIndex, e = !d, g = a) }); this.activeIndex = g !== c ? g : this.activeIndex; d ? (b.either || b.isBack)(true) : e && (b.either || b.isForward)(false) },
            ignoreNextHashChange: false
        }, r = [], o = false, t = "&ui-state=dialog", v = p.children("base"), x = m.parseLocation(), u = v.length ? m.parseUrl(m.makeUrlAbsolute(v.attr("href"), x.href)) : x, F = x.hrefNoHash !== u.hrefNoHash, z = a.mobile.getScreenHeight, y = a.support.dynamicBaseTag ? { element: v.length ? v : a("<base>", { href: u.hrefNoHash }).prependTo(p), set: function (a) { y.element.attr("href", m.makeUrlAbsolute(a, u)) }, reset: function () { y.element.attr("href", u.hrefNoHash) } } : c; a.mobile.back = function () {
            var a = s.navigator; this.phonegapNavigationEnabled &&
            a && a.app && a.app.backHistory ? a.app.backHistory() : s.history.back()
        }; a.mobile.focusPage = function (a) { var b = a.find("[autofocus]"), c = a.find(".ui-title:eq(0)"); b.length ? b.focus() : c.length ? c.focus() : a.focus() }; var E = true, A, B; A = function () { if (E) { var b = a.mobile.urlHistory.getActive(); if (b) { var c = q.scrollTop(); b.lastScroll = c < a.mobile.minScrollBack ? a.mobile.defaultHomeScroll : c } } }; B = function () { setTimeout(A, 100) }; q.bind(a.support.pushState ? "popstate" : "hashchange", function () { E = false }); q.one(a.support.pushState ? "popstate" :
        "hashchange", function () { E = true }); q.one("pagecontainercreate", function () { a.mobile.pageContainer.bind("pagechange", function () { E = true; q.unbind("scrollstop", B); q.bind("scrollstop", B) }) }); q.bind("scrollstop", B); a.fn.animationComplete = function (b) { return a.support.cssTransitions ? a(this).one("webkitAnimationEnd animationend", b) : (setTimeout(b, 0), a(this)) }; a.mobile.path = m; a.mobile.base = y; a.mobile.urlHistory = n; a.mobile.dialogHashKey = t; a.mobile.allowCrossDomainPages = false; a.mobile.getDocumentUrl = function (b) {
            return b ?
            a.extend({}, x) : x.href
        }; a.mobile.getDocumentBase = function (b) { return b ? a.extend({}, u) : u.href }; a.mobile._bindPageRemove = function () { var b = a(this); !b.data("page").options.domCache && b.is(":jqmData(external-page='true')") && b.bind("pagehide.remove", function () { var b = a(this), c = new a.Event("pageremove"); b.trigger(c); c.isDefaultPrevented() || b.removeWithDependents() }) }; a.mobile.loadPage = function (b, d) {
            var e = a.Deferred(), h = a.extend({}, a.mobile.loadPage.defaults, d), f = null, n = null, k = m.makeUrlAbsolute(b, a.mobile.activePage &&
            j(a.mobile.activePage) || u.hrefNoHash); if (h.data && h.type === "get") k = m.addSearchParams(k, h.data), h.data = c; if (h.data && h.type === "post") h.reloadPage = true; var q = m.getFilePath(k), p = m.convertUrlToDataUrl(k); h.pageContainer = h.pageContainer || a.mobile.pageContainer; f = h.pageContainer.children("[data-" + a.mobile.ns + "url='" + p + "']"); f.length === 0 && p && !m.isPath(p) && (f = h.pageContainer.children("#" + p).attr("data-" + a.mobile.ns + "url", p)); if (f.length === 0) if (a.mobile.firstPage && m.isFirstPageUrl(q)) a.mobile.firstPage.parent().length &&
            (f = a(a.mobile.firstPage)); else if (m.isEmbeddedPage(q)) return e.reject(k, d), e.promise(); if (f.length) { if (!h.reloadPage) return g(f, h.role), e.resolve(k, d, f), y && !d.prefetch && y.set(b), e.promise(); n = f } var l = h.pageContainer, v = new a.Event("pagebeforeload"), o = { url: b, absUrl: k, dataUrl: p, deferred: e, options: h }; l.trigger(v, o); if (v.isDefaultPrevented()) return e.promise(); if (h.showLoadMsg) var r = setTimeout(function () { a.mobile.showPageLoadingMsg() }, h.loadMsgDelay); y && typeof d.prefetch === "undefined" && y.reset(); !a.mobile.allowCrossDomainPages &&
            !m.isSameDomain(x, k) ? e.reject(k, d) : a.ajax({
                url: q, type: h.type, data: h.data, dataType: "html", success: function (c, j, x) {
                    var l = a("<div></div>"), v = c.match(/<title[^>]*>([^<]*)/) && RegExp.$1, u = RegExp("\\bdata-" + a.mobile.ns + "url=[\"']?([^\"'>]*)[\"']?"); RegExp("(<[^>]+\\bdata-" + a.mobile.ns + "role=[\"']?page[\"']?[^>]*>)").test(c) && RegExp.$1 && u.test(RegExp.$1) && RegExp.$1 && (b = q = m.getFilePath(RegExp.$1)); y && typeof d.prefetch === "undefined" && y.set(q); l.get(0).innerHTML = c; f = l.find(":jqmData(role='page'), :jqmData(role='dialog')").first();
                    f.length || (f = a("<div data-" + a.mobile.ns + "role='page'>" + c.split(/<\/?body[^>]*>/gmi)[1] + "</div>")); v && !f.jqmData("title") && (~v.indexOf("&") && (v = a("<div>" + v + "</div>").text()), f.jqmData("title", v)); if (!a.support.dynamicBaseTag) {
                        var t = m.get(q); f.find("[src], link[href], a[rel='external'], :jqmData(ajax='false'), a[target]").each(function () {
                            var b = a(this).is("[href]") ? "href" : a(this).is("[src]") ? "src" : "action", c = a(this).attr(b), c = c.replace(location.protocol + "//" + location.host + location.pathname, ""); /^(\w+:|#|\/)/.test(c) ||
                            a(this).attr(b, t + c)
                        })
                    } f.attr("data-" + a.mobile.ns + "url", m.convertUrlToDataUrl(q)).attr("data-" + a.mobile.ns + "external-page", true).appendTo(h.pageContainer); f.one("pagecreate", a.mobile._bindPageRemove); g(f, h.role); k.indexOf("&" + a.mobile.subPageUrlKey) > -1 && (f = h.pageContainer.children("[data-" + a.mobile.ns + "url='" + p + "']")); h.showLoadMsg && (clearTimeout(r), a.mobile.hidePageLoadingMsg()); o.xhr = x; o.textStatus = j; o.page = f; h.pageContainer.trigger("pageload", o); e.resolve(k, d, f, n)
                }, error: function (b, c, g) {
                    y && y.set(m.get());
                    o.xhr = b; o.textStatus = c; o.errorThrown = g; b = new a.Event("pageloadfailed"); h.pageContainer.trigger(b, o); b.isDefaultPrevented() || (h.showLoadMsg && (clearTimeout(r), a.mobile.hidePageLoadingMsg(), a.mobile.showPageLoadingMsg(a.mobile.pageLoadErrorMessageTheme, a.mobile.pageLoadErrorMessage, true), setTimeout(a.mobile.hidePageLoadingMsg, 1500)), e.reject(k, d))
                }
            }); return e.promise()
        }; a.mobile.loadPage.defaults = { type: "get", data: c, reloadPage: false, role: c, showLoadMsg: false, pageContainer: c, loadMsgDelay: 50 }; a.mobile.changePage =
        function (h, j) {
            if (o) r.unshift(arguments); else {
                var f = a.extend({}, a.mobile.changePage.defaults, j); f.pageContainer = f.pageContainer || a.mobile.pageContainer; f.fromPage = f.fromPage || a.mobile.activePage; var q = f.pageContainer, p = new a.Event("pagebeforechange"), l = { toPage: h, options: f }; q.trigger(p, l); if (!p.isDefaultPrevented()) if (h = l.toPage, o = true, typeof h == "string") a.mobile.loadPage(h, f).done(function (b, c, d, e) { o = false; c.duplicateCachedPage = e; a.mobile.changePage(d, c) }).fail(function () {
                    b(true); d(); f.pageContainer.trigger("pagechangefailed",
                    l)
                }); else {
                    if (h[0] === a.mobile.firstPage[0] && !f.dataUrl) f.dataUrl = x.hrefNoHash; var p = f.fromPage, v = f.dataUrl && m.convertUrlToDataUrl(f.dataUrl) || h.jqmData("url"), u = v; m.getFilePath(v); var w = n.getActive(), z = n.activeIndex === 0, s = 0, y = k.title, F = f.role === "dialog" || h.jqmData("role") === "dialog"; if (p && p[0] === h[0] && !f.allowSamePageTransition) o = false, q.trigger("pagechange", l), f.fromHashChange && n.directHashChange({ currentUrl: v, isBack: function () { }, isForward: function () { } }); else {
                        g(h, f.role); f.fromHashChange && n.directHashChange({
                            currentUrl: v,
                            isBack: function () { s = -1 }, isForward: function () { s = 1 }
                        }); try { k.activeElement && k.activeElement.nodeName.toLowerCase() != "body" ? a(k.activeElement).blur() : a("input:focus, textarea:focus, select:focus").blur() } catch (A) { } var E = false; if (F && w) { if (w.url && w.url.indexOf(t) > -1 && !a.mobile.activePage.is(".ui-dialog")) f.changeHash = false, E = true; v = (w.url || "") + t; n.activeIndex === 0 && v === n.initialDst && (v += t) } if (f.changeHash !== false && v) n.ignoreNextHashChange = true, m.set(v); var B = !w ? y : h.jqmData("title") || h.children(":jqmData(role='header')").find(".ui-title").getEncodedText();
                        B && y == k.title && (y = B); h.jqmData("title") || h.jqmData("title", y); f.transition = f.transition || (s && !z ? w.transition : c) || (F ? a.mobile.defaultDialogTransition : a.mobile.defaultPageTransition); !s && !E && n.addNew(v, f.transition, y, u, f.role); k.title = n.getActive().title; a.mobile.activePage = h; f.reverse = f.reverse || s < 0; e(h, p, f.transition, f.reverse).done(function (c, e, g, j, k) { b(); f.duplicateCachedPage && f.duplicateCachedPage.remove(); k || a.mobile.focusPage(h); d(); q.trigger("pagechange", l) })
                    }
                }
            }
        }; a.mobile.changePage.defaults =
        { transition: c, reverse: false, changeHash: true, fromHashChange: false, role: c, duplicateCachedPage: c, pageContainer: c, showLoadMsg: true, dataUrl: c, fromPage: c, allowSamePageTransition: false }; a.mobile.navreadyDeferred = a.Deferred(); a.mobile._registerInternalEvents = function () {
            a(k).delegate("form", "submit", function (b) {
                var c = a(this); if (a.mobile.ajaxEnabled && !c.is(":jqmData(ajax='false')") && c.jqmHijackable().length) {
                    var d = c.attr("method"), e = c.attr("target"), h = c.attr("action"); if (!h && (h = j(c), h === u.hrefNoHash)) h = x.hrefNoSearch;
                    h = m.makeUrlAbsolute(h, j(c)); m.isExternal(h) && !m.isPermittedCrossDomainRequest(x, h) || e || (a.mobile.changePage(h, { type: d && d.length && d.toLowerCase() || "get", data: c.serialize(), transition: c.jqmData("transition"), direction: c.jqmData("direction"), reloadPage: true }), b.preventDefault())
                }
            }); a(k).bind("vclick", function (c) {
                if (!(c.which > 1) && a.mobile.linkBindingEnabled && (c = h(c.target), a(c).jqmHijackable().length && c && m.parseUrl(c.getAttribute("href") || "#").hash !== "#")) b(true), l = a(c).closest(".ui-btn").not(".ui-disabled"),
                l.addClass(a.mobile.activeBtnClass)
            }); a(k).bind("click", function (d) {
                if (a.mobile.linkBindingEnabled) {
                    var e = h(d.target), f = a(e), g; if (e && !(d.which > 1) && f.jqmHijackable().length) {
                        g = function () { s.setTimeout(function () { b(true) }, 200) }; if (f.is(":jqmData(rel='back')")) return a.mobile.back(), false; var k = j(f), e = m.makeUrlAbsolute(f.attr("href") || "#", k); if (!a.mobile.ajaxEnabled && !m.isEmbeddedPage(e)) g(); else {
                            if (e.search("#") != -1) if (e = e.replace(/[^#]*#/, "")) e = m.isPath(e) ? m.makeUrlAbsolute(e, k) : m.makeUrlAbsolute("#" +
                            e, x.hrefNoHash); else { d.preventDefault(); return } f.is("[rel='external']") || f.is(":jqmData(ajax='false')") || f.is("[target]") || m.isExternal(e) && !m.isPermittedCrossDomainRequest(x, e) ? g() : (g = f.jqmData("transition"), k = (k = f.jqmData("direction")) && k === "reverse" || f.jqmData("back"), f = f.attr("data-" + a.mobile.ns + "rel") || c, a.mobile.changePage(e, { transition: g, reverse: k, role: f }), d.preventDefault())
                        }
                    }
                }
            }); a(k).delegate(".ui-page", "pageshow.prefetch", function () {
                var b = []; a(this).find("a:jqmData(prefetch)").each(function () {
                    var c =
                    a(this), d = c.attr("href"); d && a.inArray(d, b) === -1 && (b.push(d), a.mobile.loadPage(d, { role: c.attr("data-" + a.mobile.ns + "rel"), prefetch: true }))
                })
            }); a.mobile._handleHashChange = function (b) {
                var d = m.stripHash(b), e = { transition: a.mobile.urlHistory.stack.length === 0 ? "none" : c, changeHash: false, fromHashChange: true }; if (0 === n.stack.length) n.initialDst = d; if (!a.mobile.hashListeningEnabled || n.ignoreNextHashChange) n.ignoreNextHashChange = false; else {
                    if (n.stack.length > 1 && d.indexOf(t) > -1 && n.initialDst !== d) if (a.mobile.activePage.is(".ui-dialog")) n.directHashChange({
                        currentUrl: d,
                        either: function (b) { var c = a.mobile.urlHistory.getActive(); d = c.pageUrl; a.extend(e, { role: c.role, transition: c.transition, reverse: b }) }
                    }); else { n.directHashChange({ currentUrl: d, isBack: function () { a.mobile.back() }, isForward: function () { s.history.forward() } }); return } d ? (d = typeof d === "string" && !m.isPath(d) ? m.makeUrlAbsolute("#" + d, u) : d, a.mobile.changePage(d, e)) : a.mobile.changePage(a.mobile.firstPage, e)
                }
            }; q.bind("hashchange", function () { a.mobile._handleHashChange(m.parseLocation().hash) }); a(k).bind("pageshow", f);
            a(s).bind("throttledresize", f)
        }; a.mobile.navreadyDeferred.done(function () { a.mobile._registerInternalEvents() })
    })(l); (function (a, c) {
        var b = {}, d = a(c), e = a.mobile.path.parseLocation(), f = a.Deferred(), g = a.Deferred(); a(k).ready(a.proxy(g, "resolve")); a(k).one("mobileinit", a.proxy(f, "resolve")); a.extend(b, {
            initialFilePath: e.pathname + e.search, hashChangeTimeout: 200, hashChangeEnableTimer: C, initialHref: e.hrefNoHash, state: function () {
                return {
                    hash: a.mobile.path.parseLocation().hash || "#" + b.initialFilePath, title: k.title,
                    initialHref: b.initialHref
                }
            }, resetUIKeys: function (b) { var c = "&" + a.mobile.subPageUrlKey, d = b.indexOf(a.mobile.dialogHashKey); d > -1 ? b = b.slice(0, d) + "#" + b.slice(d) : b.indexOf(c) > -1 && (b = b.split(c).join("#" + c)); return b }, nextHashChangePrevented: function (c) { a.mobile.urlHistory.ignoreNextHashChange = c; b.onHashChangeDisabled = c }, onHashChange: function () {
                if (!b.onHashChangeDisabled) {
                    var c, d; c = a.mobile.path.parseLocation().hash; var e = a.mobile.path.isPath(c), f = e ? a.mobile.path.getLocation() : a.mobile.getDocumentUrl(); c =
                    e ? c.replace("#", "") : c; d = b.state(); c = a.mobile.path.makeUrlAbsolute(c, f); e && (c = b.resetUIKeys(c)); history.replaceState(d, k.title, c)
                }
            }, onPopState: function (c) { if (c = c.originalEvent.state) clearTimeout(b.hashChangeEnableTimer), b.nextHashChangePrevented(false), a.mobile._handleHashChange(c.hash), b.nextHashChangePrevented(true), b.hashChangeEnableTimer = setTimeout(function () { b.nextHashChangePrevented(false) }, b.hashChangeTimeout) }, init: function () {
                d.bind("hashchange", b.onHashChange); d.bind("popstate", b.onPopState);
                location.hash === "" && history.replaceState(b.state(), k.title, a.mobile.path.getLocation())
            }
        }); a.when(g, f, a.mobile.navreadyDeferred).done(function () { a.mobile.pushStateEnabled && a.support.pushState && b.init() })
    })(l, this); l.mobile.transitionFallbacks.pop = "fade"; (function (a) { a.mobile.transitionHandlers.slide = a.mobile.transitionHandlers.simultaneous; a.mobile.transitionFallbacks.slide = "fade" })(l, this); l.mobile.transitionFallbacks.slidedown = "fade"; l.mobile.transitionFallbacks.slideup = "fade"; l.mobile.transitionFallbacks.flip =
    "fade"; l.mobile.transitionFallbacks.flow = "fade"; l.mobile.transitionFallbacks.turn = "fade"; (function (a) {
        a.mobile.page.prototype.options.degradeInputs = { color: false, date: false, datetime: false, "datetime-local": false, email: false, month: false, number: false, range: "number", search: "text", tel: false, time: false, url: false, week: false }; a(k).bind("pagecreate create", function (c) {
            var b = a.mobile.closestPageData(a(c.target)), d; if (b) d = b.options, a(c.target).find("input").not(b.keepNativeSelector()).each(function () {
                var b = a(this),
                c = this.getAttribute("type"), g = d.degradeInputs[c] || "text"; if (d.degradeInputs[c]) { var h = a("<div>").html(b.clone()).html(), j = h.indexOf(" type=") > -1; b.replaceWith(h.replace(j ? /\s+type=["']?\w+['"]?/ : /\/?>/, ' type="' + g + '" data-' + a.mobile.ns + 'type="' + c + '"' + (j ? "" : ">"))) }
            })
        })
    })(l); (function (a) {
        a.widget("mobile.dialog", a.mobile.widget, {
            options: { closeBtnText: "Close", overlayTheme: "a", initSelector: ":jqmData(role='dialog')" }, _create: function () {
                var c = this, b = this.element, d = a("<a href='#' data-" + a.mobile.ns + "icon='delete' data-" +
                a.mobile.ns + "iconpos='notext'>" + this.options.closeBtnText + "</a>"), e = a("<div/>", { role: "dialog", "class": "ui-dialog-contain ui-corner-all ui-overlay-shadow" }); b.addClass("ui-dialog ui-overlay-" + this.options.overlayTheme); b.wrapInner(e).children().find(":jqmData(role='header')").prepend(d).end().children(":first-child").addClass("ui-corner-top").end().children(":last-child").addClass("ui-corner-bottom"); d.bind("click", function () { c.close() }); b.bind("vclick submit", function (b) {
                    var b = a(b.target).closest(b.type ===
                    "vclick" ? "a" : "form"), c; b.length && !b.jqmData("transition") && (c = a.mobile.urlHistory.getActive() || {}, b.attr("data-" + a.mobile.ns + "transition", c.transition || a.mobile.defaultDialogTransition).attr("data-" + a.mobile.ns + "direction", "reverse"))
                }).bind("pagehide", function () { c._isClosed = false; a(this).find("." + a.mobile.activeBtnClass).not(".ui-slider-bg").removeClass(a.mobile.activeBtnClass) }).bind("pagebeforeshow", function () {
                    c.options.overlayTheme && c.element.page("removeContainerBackground").page("setContainerBackground",
                    c.options.overlayTheme)
                })
            }, close: function () { var c; if (!this._isClosed) this._isClosed = true, a.mobile.hashListeningEnabled ? a.mobile.back() : (c = a.mobile.urlHistory.getPrev().url, a.mobile.path.isPath(c) || (c = a.mobile.path.makeUrlAbsolute("#" + c)), a.mobile.changePage(c, { changeHash: false, fromHashChange: true })) }
        }); a(k).delegate(a.mobile.dialog.prototype.options.initSelector, "pagecreate", function () { a.mobile.dialog.prototype.enhance(this) })
    })(l, this); (function (a) {
        a.mobile.page.prototype.options.backBtnText = "Back";
        a.mobile.page.prototype.options.addBackBtn = false; a.mobile.page.prototype.options.backBtnTheme = null; a.mobile.page.prototype.options.headerTheme = "a"; a.mobile.page.prototype.options.footerTheme = "a"; a.mobile.page.prototype.options.contentTheme = null; a(k).bind("pagecreate", function (c) {
            var b = a(c.target), d = b.data("page").options, e = b.jqmData("role"), f = d.theme; a(":jqmData(role='header'), :jqmData(role='footer'), :jqmData(role='content')", b).jqmEnhanceable().each(function () {
                var c = a(this), h = c.jqmData("role"),
                j = c.jqmData("theme"), k = j || d.contentTheme || e === "dialog" && f, p; c.addClass("ui-" + h); if (h === "header" || h === "footer") {
                    var m = j || (h === "header" ? d.headerTheme : d.footerTheme) || f; c.addClass("ui-bar-" + m).attr("role", h === "header" ? "banner" : "contentinfo"); h === "header" && (j = c.children("a, button"), p = j.hasClass("ui-btn-left"), k = j.hasClass("ui-btn-right"), p = p || j.eq(0).not(".ui-btn-right").addClass("ui-btn-left").length, k || j.eq(1).addClass("ui-btn-right")); d.addBackBtn && h === "header" && a(".ui-page").length > 1 && b.jqmData("url") !==
                    a.mobile.path.stripHash(location.hash) && !p && a("<a href='javascript:void(0);' class='ui-btn-left' data-" + a.mobile.ns + "rel='back' data-" + a.mobile.ns + "icon='arrow-l'>" + d.backBtnText + "</a>").attr("data-" + a.mobile.ns + "theme", d.backBtnTheme || m).prependTo(c); c.children("h1, h2, h3, h4, h5, h6").addClass("ui-title").attr({ role: "heading", "aria-level": "1" })
                } else h === "content" && (k && c.addClass("ui-body-" + k), c.attr("role", "main"))
            })
        })
    })(l); (function (a) {
        a.fn.fieldcontain = function () {
            return this.addClass("ui-field-contain ui-body ui-br").contents().filter(function () {
                return this.nodeType ===
                3 && !/\S/.test(this.nodeValue)
            }).remove()
        }; a(k).bind("pagecreate create", function (c) { a(":jqmData(role='fieldcontain')", c.target).jqmEnhanceable().fieldcontain() })
    })(l); (function (a) {
        a.fn.grid = function (c) {
            return this.each(function () {
                var b = a(this), d = a.extend({ grid: null }, c), e = b.children(), f = { solo: 1, a: 2, b: 3, c: 4, d: 5 }, d = d.grid; if (!d) if (e.length <= 5) for (var g in f) f[g] === e.length && (d = g); else d = "a", b.addClass("ui-grid-duo"); f = f[d]; b.addClass("ui-grid-" + d); e.filter(":nth-child(" + f + "n+1)").addClass("ui-block-a");
                f > 1 && e.filter(":nth-child(" + f + "n+2)").addClass("ui-block-b"); f > 2 && e.filter(":nth-child(3n+3)").addClass("ui-block-c"); f > 3 && e.filter(":nth-child(4n+4)").addClass("ui-block-d"); f > 4 && e.filter(":nth-child(5n+5)").addClass("ui-block-e")
            })
        }
    })(l); (function (a) { a(k).bind("pagecreate create", function (c) { a(":jqmData(role='nojs')", c.target).addClass("ui-nojs") }) })(l); (function (a, c) {
        function b(a) {
            for (var b; a;) {
                if ((b = typeof a.className === "string" && a.className + " ") && b.indexOf("ui-btn ") > -1 && b.indexOf("ui-disabled ") <
                0) break; a = a.parentNode
            } return a
        } a.fn.buttonMarkup = function (b) {
            for (var b = b && a.type(b) == "object" ? b : {}, f = 0; f < this.length; f++) {
                var g = this.eq(f), h = g[0], j = a.extend({}, a.fn.buttonMarkup.defaults, {
                    icon: b.icon !== c ? b.icon : g.jqmData("icon"), iconpos: b.iconpos !== c ? b.iconpos : g.jqmData("iconpos"), theme: b.theme !== c ? b.theme : g.jqmData("theme") || a.mobile.getInheritedTheme(g, "c"), inline: b.inline !== c ? b.inline : g.jqmData("inline"), shadow: b.shadow !== c ? b.shadow : g.jqmData("shadow"), corners: b.corners !== c ? b.corners : g.jqmData("corners"),
                    iconshadow: b.iconshadow !== c ? b.iconshadow : g.jqmData("iconshadow"), mini: b.mini !== c ? b.mini : g.jqmData("mini")
                }, b), q = "ui-btn-inner", p, m, l, n, r, o; a.each(j, function (b, c) { h.setAttribute("data-" + a.mobile.ns + b, c); g.jqmData(b, c) }); (o = a.data(h.tagName === "INPUT" || h.tagName === "BUTTON" ? h.parentNode : h, "buttonElements")) ? (h = o.outer, g = a(h), l = o.inner, n = o.text, a(o.icon).remove(), o.icon = null) : (l = k.createElement(j.wrapperEls), n = k.createElement(j.wrapperEls)); r = j.icon ? k.createElement("span") : null; d && !o && d(); if (!j.theme) j.theme =
                a.mobile.getInheritedTheme(g, "c"); p = "ui-btn ui-btn-up-" + j.theme; p += j.shadow ? " ui-shadow" : ""; p += j.corners ? " ui-btn-corner-all" : ""; j.mini !== c && (p += j.mini === true ? " ui-mini" : " ui-fullsize"); j.inline !== c && (p += j.inline === true ? " ui-btn-inline" : " ui-btn-block"); if (j.icon) j.icon = "ui-icon-" + j.icon, j.iconpos = j.iconpos || "left", m = "ui-icon " + j.icon, j.iconshadow && (m += " ui-icon-shadow"); j.iconpos && (p += " ui-btn-icon-" + j.iconpos, j.iconpos == "notext" && !g.attr("title") && g.attr("title", g.getEncodedText())); q += j.corners ?
                " ui-btn-corner-all" : ""; j.iconpos && j.iconpos === "notext" && !g.attr("title") && g.attr("title", g.getEncodedText()); o && g.removeClass(o.bcls || ""); g.removeClass("ui-link").addClass(p); l.className = q; n.className = "ui-btn-text"; o || l.appendChild(n); if (r && (r.className = m, !o || !o.icon)) r.innerHTML = "&#160;", l.appendChild(r); for (; h.firstChild && !o;) n.appendChild(h.firstChild); o || h.appendChild(l); o = { bcls: p, outer: h, inner: l, text: n, icon: r }; a.data(h, "buttonElements", o); a.data(l, "buttonElements", o); a.data(n, "buttonElements",
                o); r && a.data(r, "buttonElements", o)
            } return this
        }; a.fn.buttonMarkup.defaults = { corners: true, shadow: true, iconshadow: true, wrapperEls: "span" }; var d = function () {
            var c = a.mobile.buttonMarkup.hoverDelay, f, g; a(k).bind({
                "vmousedown vmousecancel vmouseup vmouseover vmouseout focus blur scrollstart": function (d) {
                    var j, k = a(b(d.target)), d = d.type; if (k.length) if (j = k.attr("data-" + a.mobile.ns + "theme"), d === "vmousedown") a.support.touch ? f = setTimeout(function () { k.removeClass("ui-btn-up-" + j).addClass("ui-btn-down-" + j) }, c) :
                    k.removeClass("ui-btn-up-" + j).addClass("ui-btn-down-" + j); else if (d === "vmousecancel" || d === "vmouseup") k.removeClass("ui-btn-down-" + j).addClass("ui-btn-up-" + j); else if (d === "vmouseover" || d === "focus") a.support.touch ? g = setTimeout(function () { k.removeClass("ui-btn-up-" + j).addClass("ui-btn-hover-" + j) }, c) : k.removeClass("ui-btn-up-" + j).addClass("ui-btn-hover-" + j); else if (d === "vmouseout" || d === "blur" || d === "scrollstart") k.removeClass("ui-btn-hover-" + j + " ui-btn-down-" + j).addClass("ui-btn-up-" + j), f && clearTimeout(f),
                    g && clearTimeout(g)
                }, "focusin focus": function (c) { a(b(c.target)).addClass(a.mobile.focusClass) }, "focusout blur": function (c) { a(b(c.target)).removeClass(a.mobile.focusClass) }
            }); d = null
        }; a(k).bind("pagecreate create", function (b) { a(":jqmData(role='button'), .ui-bar > a, .ui-header > a, .ui-footer > a, .ui-bar > :jqmData(role='controlgroup') > a", b.target).jqmEnhanceable().not("button, input, .ui-btn, :jqmData(role='none'), :jqmData(role='nojs')").buttonMarkup() })
    })(l); (function (a) {
        a.widget("mobile.collapsible",
        a.mobile.widget, {
            options: { expandCueText: " click to expand contents", collapseCueText: " click to collapse contents", collapsed: true, heading: "h1,h2,h3,h4,h5,h6,legend", theme: null, contentTheme: null, iconTheme: "d", mini: false, initSelector: ":jqmData(role='collapsible')" }, _create: function () {
                var c = this.element, b = this.options, d = c.addClass("ui-collapsible"), e = c.children(b.heading).first(), f = d.wrapInner("<div class='ui-collapsible-content'></div>").find(".ui-collapsible-content"), g = c.closest(":jqmData(role='collapsible-set')").addClass("ui-collapsible-set");
                e.is("legend") && (e = a("<div role='heading'>" + e.html() + "</div>").insertBefore(e), e.next().remove()); if (g.length) { if (!b.theme) b.theme = g.jqmData("theme") || a.mobile.getInheritedTheme(g, "c"); if (!b.contentTheme) b.contentTheme = g.jqmData("content-theme"); if (!b.iconpos) b.iconpos = g.jqmData("iconpos"); if (!b.mini) b.mini = g.jqmData("mini") } f.addClass(b.contentTheme ? "ui-body-" + b.contentTheme : ""); e.insertBefore(f).addClass("ui-collapsible-heading").append("<span class='ui-collapsible-heading-status'></span>").wrapInner("<a href='#' class='ui-collapsible-heading-toggle'></a>").find("a").first().buttonMarkup({
                    shadow: false,
                    corners: false, iconpos: c.jqmData("iconpos") || b.iconpos || "left", icon: "plus", mini: b.mini, theme: b.theme
                }).add(".ui-btn-inner", c).addClass("ui-corner-top ui-corner-bottom"); d.bind("expand collapse", function (c) {
                    if (!c.isDefaultPrevented()) {
                        c.preventDefault(); var j = a(this), c = c.type === "collapse", k = b.contentTheme; e.toggleClass("ui-collapsible-heading-collapsed", c).find(".ui-collapsible-heading-status").text(c ? b.expandCueText : b.collapseCueText).end().find(".ui-icon").toggleClass("ui-icon-minus", !c).toggleClass("ui-icon-plus",
                        c).end().find("a").first().removeClass(a.mobile.activeBtnClass); j.toggleClass("ui-collapsible-collapsed", c); f.toggleClass("ui-collapsible-content-collapsed", c).attr("aria-hidden", c); if (k && (!g.length || d.jqmData("collapsible-last"))) e.find("a").first().add(e.find(".ui-btn-inner")).toggleClass("ui-corner-bottom", c), f.toggleClass("ui-corner-bottom", !c); f.trigger("updatelayout")
                    }
                }).trigger(b.collapsed ? "collapse" : "expand"); e.bind("tap", function () { e.find("a").first().addClass(a.mobile.activeBtnClass) }).bind("click",
                function (a) { var b = e.is(".ui-collapsible-heading-collapsed") ? "expand" : "collapse"; d.trigger(b); a.preventDefault(); a.stopPropagation() })
            }
        }); a(k).bind("pagecreate create", function (c) { a.mobile.collapsible.prototype.enhanceWithin(c.target) })
    })(l); (function (a, c) {
        a.widget("mobile.collapsibleset", a.mobile.widget, {
            options: { initSelector: ":jqmData(role='collapsible-set')" }, _create: function () {
                var b = this.element.addClass("ui-collapsible-set"), d = this.options; if (!d.theme) d.theme = a.mobile.getInheritedTheme(b, "c");
                if (!d.contentTheme) d.contentTheme = b.jqmData("content-theme"); if (!d.corners) d.corners = b.jqmData("corners") === c ? true : false; b.jqmData("collapsiblebound") || b.jqmData("collapsiblebound", true).bind("expand collapse", function (b) {
                    var c = b.type === "collapse", b = a(b.target).closest(".ui-collapsible"); b.data("collapsible").options.contentTheme && b.jqmData("collapsible-last") && (b.find(".ui-collapsible-heading").first().find("a").first().toggleClass("ui-corner-bottom", c).find(".ui-btn-inner").toggleClass("ui-corner-bottom",
                    c), b.find(".ui-collapsible-content").toggleClass("ui-corner-bottom", !c))
                }).bind("expand", function (b) { a(b.target).closest(".ui-collapsible").siblings(".ui-collapsible").trigger("collapse") })
            }, _init: function () { this.refresh() }, refresh: function () {
                var b = this.options, c = this.element.children(":jqmData(role='collapsible')"); a.mobile.collapsible.prototype.enhance(c.not(".ui-collapsible")); c.each(function () { a(this).jqmRemoveData("collapsible-last").find(".ui-collapsible-heading").find("a").first().removeClass("ui-corner-top ui-corner-bottom").find(".ui-btn-inner").removeClass("ui-corner-top ui-corner-bottom") });
                c.first().find("a").first().addClass(b.corners ? "ui-corner-top" : "").find(".ui-btn-inner").addClass("ui-corner-top"); c.last().jqmData("collapsible-last", true).find("a").first().addClass(b.corners ? "ui-corner-bottom" : "").find(".ui-btn-inner").addClass("ui-corner-bottom")
            }
        }); a(k).bind("pagecreate create", function (b) { a.mobile.collapsibleset.prototype.enhanceWithin(b.target) })
    })(l); (function (a, c) {
        a.widget("mobile.navbar", a.mobile.widget, {
            options: { iconpos: "top", grid: null, initSelector: ":jqmData(role='navbar')" },
            _create: function () { var b = this.element, d = b.find("a"), e = d.filter(":jqmData(icon)").length ? this.options.iconpos : c; b.addClass("ui-navbar ui-mini").attr("role", "navigation").find("ul").jqmEnhanceable().grid({ grid: this.options.grid }); d.buttonMarkup({ corners: false, shadow: false, inline: true, iconpos: e }); b.delegate("a", "vclick", function (b) { a(b.target).hasClass("ui-disabled") || (d.removeClass(a.mobile.activeBtnClass), a(this).addClass(a.mobile.activeBtnClass)) }); b.closest(".ui-page").bind("pagebeforeshow", function () { d.filter(".ui-state-persist").addClass(a.mobile.activeBtnClass) }) }
        });
        a(k).bind("pagecreate create", function (b) { a.mobile.navbar.prototype.enhanceWithin(b.target) })
    })(l); (function (a) {
        var c = {}; a.widget("mobile.listview", a.mobile.widget, {
            options: { theme: null, countTheme: "c", headerTheme: "b", dividerTheme: "b", splitIcon: "arrow-r", splitTheme: "b", inset: false, initSelector: ":jqmData(role='listview')" }, _create: function () { var a = ""; a += this.options.inset ? " ui-listview-inset ui-corner-all ui-shadow " : ""; this.element.addClass(function (c, e) { return e + " ui-listview " + a }); this.refresh(true) },
            _removeCorners: function (a, c) { a = a.add(a.find(".ui-btn-inner, .ui-li-link-alt, .ui-li-thumb")); c === "top" ? a.removeClass("ui-corner-top ui-corner-tr ui-corner-tl") : c === "bottom" ? a.removeClass("ui-corner-bottom ui-corner-br ui-corner-bl") : a.removeClass("ui-corner-top ui-corner-tr ui-corner-tl ui-corner-bottom ui-corner-br ui-corner-bl") }, _refreshCorners: function (a) {
                var c, e; c = this.element.children("li"); e = a ? c.not(".ui-screen-hidden") : c.filter(":visible"); c.filter(".ui-li-last").removeClass("ui-li-last");
                this.options.inset ? (this._removeCorners(c), c = e.first().addClass("ui-corner-top"), c.add(c.find(".ui-btn-inner").not(".ui-li-link-alt span:first-child")).addClass("ui-corner-top").end().find(".ui-li-link-alt, .ui-li-link-alt span:first-child").addClass("ui-corner-tr").end().find(".ui-li-thumb").not(".ui-li-icon").addClass("ui-corner-tl"), e = e.last().addClass("ui-corner-bottom ui-li-last"), e.add(e.find(".ui-btn-inner")).find(".ui-li-link-alt").addClass("ui-corner-br").end().find(".ui-li-thumb").not(".ui-li-icon").addClass("ui-corner-bl")) :
                e.last().addClass("ui-li-last"); a || this.element.trigger("updatelayout")
            }, _findFirstElementByTagName: function (a, c, e, f) { var g = {}; for (g[e] = g[f] = true; a;) { if (g[a.nodeName]) return a; a = a[c] } return null }, _getChildrenByTagName: function (b, c, e) { var f = [], g = {}; g[c] = g[e] = true; for (b = b.firstChild; b;) g[b.nodeName] && f.push(b), b = b.nextSibling; return a(f) }, _addThumbClasses: function (b) {
                var c, e, f = b.length; for (c = 0; c < f; c++) e = a(this._findFirstElementByTagName(b[c].firstChild, "nextSibling", "img", "IMG")), e.length && (e.addClass("ui-li-thumb"),
                a(this._findFirstElementByTagName(e[0].parentNode, "parentNode", "li", "LI")).addClass(e.is(".ui-li-icon") ? "ui-li-has-icon" : "ui-li-has-thumb"))
            }, refresh: function (b) {
                this.parentPage = this.element.closest(".ui-page"); this._createSubPages(); var c = this.options, e = this.element, f = e.jqmData("dividertheme") || c.dividerTheme, g = e.jqmData("splittheme"), h = e.jqmData("spliticon"), j = this._getChildrenByTagName(e[0], "li", "LI"), l = !!a.nodeName(e[0], "ol"), p = !a.support.cssPseudoElement, m = e.attr("start"), w = {}, n, r, o, t, v, x, u,
                s; l && p && e.find(".ui-li-dec").remove(); l && (m || m === 0 ? p ? x = parseFloat(m) : (n = parseFloat(m) - 1, e.css("counter-reset", "listnumbering " + n)) : p && (x = 1)); if (!c.theme) c.theme = a.mobile.getInheritedTheme(this.element, "c"); for (var z = 0, y = j.length; z < y; z++) {
                    n = j.eq(z); r = "ui-li"; if (b || !n.hasClass("ui-li")) o = n.jqmData("theme") || c.theme, t = this._getChildrenByTagName(n[0], "a", "A"), u = n.jqmData("role") === "list-divider", t.length && !u ? (u = n.jqmData("icon"), n.buttonMarkup({
                        wrapperEls: "div", shadow: false, corners: false, iconpos: "right",
                        icon: t.length > 1 || u === false ? false : u || "arrow-r", theme: o
                    }), u != false && t.length == 1 && n.addClass("ui-li-has-arrow"), t.first().removeClass("ui-link").addClass("ui-link-inherit"), t.length > 1 && (r += " ui-li-has-alt", t = t.last(), v = g || t.jqmData("theme") || c.splitTheme, s = t.jqmData("icon"), t.appendTo(n).attr("title", t.getEncodedText()).addClass("ui-li-link-alt").empty().buttonMarkup({ shadow: false, corners: false, theme: o, icon: false, iconpos: "notext" }).find(".ui-btn-inner").append(a(k.createElement("span")).buttonMarkup({
                        shadow: true,
                        corners: true, theme: v, iconpos: "notext", icon: s || u || h || c.splitIcon
                    })))) : u ? (r += " ui-li-divider ui-bar-" + f, n.attr("role", "heading"), l && (m || m === 0 ? p ? x = parseFloat(m) : (o = parseFloat(m) - 1, n.css("counter-reset", "listnumbering " + o)) : p && (x = 1))) : r += " ui-li-static ui-body-" + o; l && p && r.indexOf("ui-li-divider") < 0 && (o = r.indexOf("ui-li-static") > 0 ? n : n.find(".ui-link-inherit"), o.addClass("ui-li-jsnumbering").prepend("<span class='ui-li-dec'>" + x++ + ". </span>")); w[r] || (w[r] = []); w[r].push(n[0])
                } for (r in w) a(w[r]).addClass(r).children(".ui-btn-inner").addClass(r);
                e.find("h1, h2, h3, h4, h5, h6").addClass("ui-li-heading").end().find("p, dl").addClass("ui-li-desc").end().find(".ui-li-aside").each(function () { var b = a(this); b.prependTo(b.parent()) }).end().find(".ui-li-count").each(function () { a(this).closest("li").addClass("ui-li-has-count") }).addClass("ui-btn-up-" + (e.jqmData("counttheme") || this.options.countTheme) + " ui-btn-corner-all"); this._addThumbClasses(j); this._addThumbClasses(e.find(".ui-link-inherit")); this._refreshCorners(b)
            }, _idStringEscape: function (a) {
                return a.replace(/[^a-zA-Z0-9]/g,
                "-")
            }, _createSubPages: function () {
                var b = this.element, d = b.closest(".ui-page"), e = d.jqmData("url"), f = e || d[0][a.expando], g = b.attr("id"), h = this.options, j = "data-" + a.mobile.ns, k = this, l = d.find(":jqmData(role='footer')").jqmData("id"), m; typeof c[f] === "undefined" && (c[f] = -1); g = g || ++c[f]; a(b.find("li>ul, li>ol").toArray().reverse()).each(function (c) {
                    var d = a(this), f = d.attr("id") || g + "-" + c, c = d.parent(), k = a(d.prevAll().toArray().reverse()), k = k.length ? k : a("<span>" + a.trim(c.contents()[0].nodeValue) + "</span>"), q = k.first().getEncodedText(),
                    f = (e || "") + "&" + a.mobile.subPageUrlKey + "=" + f, v = d.jqmData("theme") || h.theme, x = d.jqmData("counttheme") || b.jqmData("counttheme") || h.countTheme; m = true; d.detach().wrap("<div " + j + "role='page' " + j + "url='" + f + "' " + j + "theme='" + v + "' " + j + "count-theme='" + x + "'><div " + j + "role='content'></div></div>").parent().before("<div " + j + "role='header' " + j + "theme='" + h.headerTheme + "'><div class='ui-title'>" + q + "</div></div>").after(l ? a("<div " + j + "role='footer' " + j + "id='" + l + "'>") : "").parent().appendTo(a.mobile.pageContainer).page();
                    d = c.find("a:first"); d.length || (d = a("<a/>").html(k || q).prependTo(c.empty())); d.attr("href", "#" + f)
                }).listview(); m && d.is(":jqmData(external-page='true')") && d.data("page").options.domCache === false && d.unbind("pagehide.remove").bind("pagehide.remove", function (b, c) { var f = c.nextPage, g = new a.Event("pageremove"); c.nextPage && (f = f.jqmData("url"), f.indexOf(e + "&" + a.mobile.subPageUrlKey) !== 0 && (k.childPages().remove(), d.trigger(g), g.isDefaultPrevented() || d.removeWithDependents())) })
            }, childPages: function () {
                var b =
                this.parentPage.jqmData("url"); return a(":jqmData(url^='" + b + "&" + a.mobile.subPageUrlKey + "')")
            }
        }); a(k).bind("pagecreate create", function (b) { a.mobile.listview.prototype.enhanceWithin(b.target) })
    })(l); (function (a, c) {
        a.widget("mobile.checkboxradio", a.mobile.widget, {
            options: { theme: null, mini: false, initSelector: "input[type='checkbox'],input[type='radio']" }, _create: function () {
                var b = this, d = this.element, e = this.options, f = a(d).closest("label"), g = f.length ? f : a(d).closest("form,fieldset,:jqmData(role='page'),:jqmData(role='dialog')").find("label").filter("[for='" +
                d[0].id + "']"), h = d[0].type, f = d.jqmData("mini") || d.closest("form,fieldset").jqmData("mini") || e.mini, j = h + "-on", l = h + "-off", p = d.parents(":jqmData(type='horizontal')").length ? c : l, m = d.jqmData("iconpos") || d.closest("form,fieldset").jqmData("iconpos"); if (!(h !== "checkbox" && h !== "radio")) {
                    a.extend(this, { label: g, inputtype: h, checkedClass: "ui-" + j + (p ? "" : " " + a.mobile.activeBtnClass), uncheckedClass: "ui-" + l, checkedicon: "ui-icon-" + j, uncheckedicon: "ui-icon-" + l }); if (!e.theme) e.theme = a.mobile.getInheritedTheme(this.element,
                    "c"); g.buttonMarkup({ theme: e.theme, icon: p, shadow: false, mini: f, iconpos: m }); e = k.createElement("div"); e.className = "ui-" + h; d.add(g).wrapAll(e); g.bind({ vmouseover: function (b) { a(this).parent().is(".ui-disabled") && b.stopPropagation() }, vclick: function (a) { if (d.is(":disabled")) a.preventDefault(); else return b._cacheVals(), d.prop("checked", h === "radio" && true || !d.prop("checked")), d.triggerHandler("click"), b._getInputSet().not(d).prop("checked", false), b._updateAll(), false } }); d.bind({
                        vmousedown: function () { b._cacheVals() },
                        vclick: function () { var c = a(this); c.is(":checked") ? (c.prop("checked", true), b._getInputSet().not(c).prop("checked", false)) : c.prop("checked", false); b._updateAll() }, focus: function () { g.addClass(a.mobile.focusClass) }, blur: function () { g.removeClass(a.mobile.focusClass) }
                    }); this.refresh()
                }
            }, _cacheVals: function () { this._getInputSet().each(function () { a(this).jqmData("cacheVal", this.checked) }) }, _getInputSet: function () {
                return this.inputtype === "checkbox" ? this.element : this.element.closest("form,fieldset,:jqmData(role='page')").find("input[name='" +
                this.element[0].name + "'][type='" + this.inputtype + "']")
            }, _updateAll: function () { var b = this; this._getInputSet().each(function () { var c = a(this); (this.checked || b.inputtype === "checkbox") && c.trigger("change") }).checkboxradio("refresh") }, refresh: function () {
                var a = this.element[0], c = this.label, e = c.find(".ui-icon"); a.checked ? (c.addClass(this.checkedClass).removeClass(this.uncheckedClass), e.addClass(this.checkedicon).removeClass(this.uncheckedicon)) : (c.removeClass(this.checkedClass).addClass(this.uncheckedClass),
                e.removeClass(this.checkedicon).addClass(this.uncheckedicon)); a.disabled ? this.disable() : this.enable()
            }, disable: function () { this.element.prop("disabled", true).parent().addClass("ui-disabled") }, enable: function () { this.element.prop("disabled", false).parent().removeClass("ui-disabled") }
        }); a(k).bind("pagecreate create", function (b) { a.mobile.checkboxradio.prototype.enhanceWithin(b.target, true) })
    })(l); (function (a, c) {
        a.widget("mobile.button", a.mobile.widget, {
            options: {
                theme: null, icon: null, iconpos: null, corners: true,
                shadow: true, iconshadow: true, initSelector: "button, [type='button'], [type='submit'], [type='reset'], [type='image']"
            }, _create: function () {
                var b = this.element, d, e = this.options, f; f = e.inline || b.jqmData("inline"); var g = e.mini || b.jqmData("mini"), h = "", j; if (b[0].tagName === "A") !b.hasClass("ui-btn") && b.buttonMarkup(); else {
                    if (!this.options.theme) this.options.theme = a.mobile.getInheritedTheme(this.element, "c"); ~b[0].className.indexOf("ui-btn-left") && (h = "ui-btn-left"); ~b[0].className.indexOf("ui-btn-right") && (h =
                    "ui-btn-right"); if (b.attr("type") === "submit" || b.attr("type") === "reset") h ? h += " ui-submit" : h = "ui-submit"; a("label[for='" + b.attr("id") + "']").addClass("ui-submit"); d = this.button = a("<div></div>").text(b.text() || b.val()).insertBefore(b).buttonMarkup({ theme: e.theme, icon: e.icon, iconpos: e.iconpos, inline: f, corners: e.corners, shadow: e.shadow, iconshadow: e.iconshadow, mini: g }).addClass(h).append(b.addClass("ui-btn-hidden")); e = b.attr("type"); f = b.attr("name"); e !== "button" && e !== "reset" && f && b.bind("vclick", function () {
                        j ===
                        c && (j = a("<input>", { type: "hidden", name: b.attr("name"), value: b.attr("value") }).insertBefore(b), a(k).one("submit", function () { j.remove(); j = c }))
                    }); b.bind({ focus: function () { d.addClass(a.mobile.focusClass) }, blur: function () { d.removeClass(a.mobile.focusClass) } }); this.refresh()
                }
            }, enable: function () { this.element.attr("disabled", false); this.button.removeClass("ui-disabled").attr("aria-disabled", false); return this._setOption("disabled", false) }, disable: function () {
                this.element.attr("disabled", true); this.button.addClass("ui-disabled").attr("aria-disabled",
                true); return this._setOption("disabled", true)
            }, refresh: function () { var b = this.element; b.prop("disabled") ? this.disable() : this.enable(); a(this.button.data("buttonElements").text).text(b.text() || b.val()) }
        }); a(k).bind("pagecreate create", function (b) { a.mobile.button.prototype.enhanceWithin(b.target, true) })
    })(l); (function (a) {
        a.fn.controlgroup = function (c) {
            function b(a, b) { a.removeClass("ui-btn-corner-all ui-corner-top ui-corner-bottom ui-corner-left ui-corner-right ui-controlgroup-last ui-shadow").eq(0).addClass(b[0]).end().last().addClass(b[1]).addClass("ui-controlgroup-last") }
            return this.each(function () {
                var d = a(this), e = a.extend({ direction: d.jqmData("type") || "vertical", shadow: false, excludeInvisible: true, mini: d.jqmData("mini") }, c), f = d.children("legend"), g = d.children(".ui-controlgroup-label"), h = d.children(".ui-controlgroup-controls"), j = e.direction === "horizontal" ? ["ui-corner-left", "ui-corner-right"] : ["ui-corner-top", "ui-corner-bottom"]; d.find("input").first().attr("type"); h.length && h.contents().unwrap(); d.wrapInner("<div class='ui-controlgroup-controls'></div>"); f.length ?
                (a("<div role='heading' class='ui-controlgroup-label'>" + f.html() + "</div>").insertBefore(d.children(0)), f.remove()) : g.length && d.prepend(g); d.addClass("ui-corner-all ui-controlgroup ui-controlgroup-" + e.direction); b(d.find(".ui-btn" + (e.excludeInvisible ? ":visible" : "")).not(".ui-slider-handle"), j); b(d.find(".ui-btn-inner"), j); e.shadow && d.addClass("ui-shadow"); e.mini && d.addClass("ui-mini")
            })
        }
    })(l); (function (a) { a(k).bind("pagecreate create", function (c) { a(c.target).find("a").jqmEnhanceable().not(".ui-btn, .ui-link-inherit, :jqmData(role='none'), :jqmData(role='nojs')").addClass("ui-link") }) })(l);
    (function (a) {
        var c = a("meta[name=viewport]"), b = c.attr("content"), d = b + ",maximum-scale=1, user-scalable=no", e = b + ",maximum-scale=10, user-scalable=yes", f = /(user-scalable[\s]*=[\s]*no)|(maximum-scale[\s]*=[\s]*1)[$,\s]/.test(b); a.mobile.zoom = a.extend({}, {
            enabled: !f, locked: false, disable: function (b) { if (!f && !a.mobile.zoom.locked) c.attr("content", d), a.mobile.zoom.enabled = false, a.mobile.zoom.locked = b || false }, enable: function (b) {
                if (!f && (!a.mobile.zoom.locked || b === true)) c.attr("content", e), a.mobile.zoom.enabled =
                true, a.mobile.zoom.locked = false
            }, restore: function () { if (!f) c.attr("content", b), a.mobile.zoom.enabled = true }
        })
    })(l); (function (a) {
        a.widget("mobile.textinput", a.mobile.widget, {
            options: {
                theme: null, mini: false, preventFocusZoom: /iPhone|iPad|iPod/.test(navigator.platform) && navigator.userAgent.indexOf("AppleWebKit") > -1, initSelector: "input[type='text'], input[type='search'], :jqmData(type='search'), input[type='number'], :jqmData(type='number'), input[type='password'], input[type='email'], input[type='url'], input[type='tel'], textarea, input[type='time'], input[type='date'], input[type='month'], input[type='week'], input[type='datetime'], input[type='datetime-local'], input[type='color'], input:not([type])",
                clearSearchButtonText: "clear text", disabled: false
            }, _create: function () {
                var c = this.element, b = this.options, d = b.theme || a.mobile.getInheritedTheme(this.element, "c"), e = " ui-body-" + d, f = b.mini ? " ui-mini" : "", g, h; a("label[for='" + c.attr("id") + "']").addClass("ui-input-text"); g = c.addClass("ui-input-text ui-body-" + d); typeof c[0].autocorrect !== "undefined" && !a.support.touchOverflow && (c[0].setAttribute("autocorrect", "off"), c[0].setAttribute("autocomplete", "off")); c.is("[type='search'],:jqmData(type='search')") ? (g =
                c.wrap("<div class='ui-input-search ui-shadow-inset ui-btn-corner-all ui-btn-shadow ui-icon-searchfield" + e + f + "'></div>").parent(), h = a("<a href='#' class='ui-input-clear' title='" + b.clearSearchButtonText + "'>" + b.clearSearchButtonText + "</a>").bind("click", function (a) { c.val("").focus().trigger("change"); h.addClass("ui-input-clear-hidden"); a.preventDefault() }).appendTo(g).buttonMarkup({ icon: "delete", iconpos: "notext", corners: true, shadow: true, mini: b.mini }), d = function () {
                    setTimeout(function () {
                        h.toggleClass("ui-input-clear-hidden",
                        !c.val())
                    }, 0)
                }, d(), c.bind("paste cut keyup focus change blur", d)) : c.addClass("ui-corner-all ui-shadow-inset" + e + f); c.focus(function () { g.addClass(a.mobile.focusClass) }).blur(function () { g.removeClass(a.mobile.focusClass) }).bind("focus", function () { b.preventFocusZoom && a.mobile.zoom.disable(true) }).bind("blur", function () { b.preventFocusZoom && a.mobile.zoom.enable(true) }); if (c.is("textarea")) {
                    var j = function () { var a = c[0].scrollHeight; c[0].clientHeight < a && c.height(a + 15) }, l; c.keyup(function () {
                        clearTimeout(l);
                        l = setTimeout(j, 100)
                    }); a(k).one("pagechange", j); a.trim(c.val()) && a(s).load(j)
                } c.attr("disabled") && this.disable()
            }, disable: function () { (this.element.attr("disabled", true).is("[type='search'],:jqmData(type='search')") ? this.element.parent() : this.element).addClass("ui-disabled"); return this._setOption("disabled", true) }, enable: function () {
                (this.element.attr("disabled", false).is("[type='search'],:jqmData(type='search')") ? this.element.parent() : this.element).removeClass("ui-disabled"); return this._setOption("disabled",
                false)
            }
        }); a(k).bind("pagecreate create", function (c) { a.mobile.textinput.prototype.enhanceWithin(c.target, true) })
    })(l); (function (a) {
        a.mobile.listview.prototype.options.filter = false; a.mobile.listview.prototype.options.filterPlaceholder = "Filter items..."; a.mobile.listview.prototype.options.filterTheme = "c"; a.mobile.listview.prototype.options.filterCallback = function (a, b) { return a.toLowerCase().indexOf(b) === -1 }; a(k).delegate("ul, ol", "listviewcreate", function () {
            var c = a(this), b = c.data("listview"); if (b.options.filter) {
                var d =
                a("<form>", { "class": "ui-listview-filter ui-bar-" + b.options.filterTheme, role: "search" }); a("<input>", { placeholder: b.options.filterPlaceholder }).attr("data-" + a.mobile.ns + "type", "search").jqmData("lastval", "").bind("keyup change", function () {
                    var d = a(this), f = this.value.toLowerCase(), g = null, g = d.jqmData("lastval") + "", h = false, j = ""; d.jqmData("lastval", f); g = f.length < g.length || f.indexOf(g) !== 0 ? c.children() : c.children(":not(.ui-screen-hidden)"); if (f) {
                        for (var k = g.length - 1; k >= 0; k--) d = a(g[k]), j = d.jqmData("filtertext") ||
                        d.text(), d.is("li:jqmData(role=list-divider)") ? (d.toggleClass("ui-filter-hidequeue", !h), h = false) : b.options.filterCallback(j, f) ? d.toggleClass("ui-filter-hidequeue", true) : h = true; g.filter(":not(.ui-filter-hidequeue)").toggleClass("ui-screen-hidden", false); g.filter(".ui-filter-hidequeue").toggleClass("ui-screen-hidden", true).toggleClass("ui-filter-hidequeue", false)
                    } else g.toggleClass("ui-screen-hidden", false); b._refreshCorners()
                }).appendTo(d).textinput(); b.options.inset && d.addClass("ui-listview-filter-inset");
                d.bind("submit", function () { return false }).insertBefore(c)
            }
        })
    })(l); (function (a, c) {
        a.widget("mobile.slider", a.mobile.widget, {
            options: { theme: null, trackTheme: null, disabled: false, initSelector: "input[type='range'], :jqmData(type='range'), :jqmData(role='slider')", mini: false }, _create: function () {
                var b = this, d = this.element, e = a.mobile.getInheritedTheme(d, "c"), f = this.options.theme || e, e = this.options.trackTheme || e, g = d[0].nodeName.toLowerCase(), h = g == "select" ? "ui-slider-switch" : "", j = d.attr("id"), l = a("[for='" + j + "']"),
                p = l.attr("id") || j + "-label", l = l.attr("id", p), m = function () { return g == "input" ? parseFloat(d.val()) : d[0].selectedIndex }, w = g == "input" ? parseFloat(d.attr("min")) : 0, n = g == "input" ? parseFloat(d.attr("max")) : d.find("option").length - 1, r = s.parseFloat(d.attr("step") || 1), o = this.options.inline || d.jqmData("inline") == true ? " ui-slider-inline" : "", t = this.options.mini || d.jqmData("mini") ? " ui-slider-mini" : "", v = k.createElement("a"), x = a(v), j = k.createElement("div"), u = a(j), F = d.jqmData("highlight") && g != "select" ? function () {
                    var b =
                    k.createElement("div"); b.className = "ui-slider-bg " + a.mobile.activeBtnClass + " ui-btn-corner-all"; return a(b).prependTo(u)
                }() : false; this._type = g; v.setAttribute("href", "#"); j.setAttribute("role", "application"); j.className = ["ui-slider ", h, " ui-btn-down-", e, " ui-btn-corner-all", o, t].join(""); v.className = "ui-slider-handle"; j.appendChild(v); x.buttonMarkup({ corners: true, theme: f, shadow: true }).attr({ role: "slider", "aria-valuemin": w, "aria-valuemax": n, "aria-valuenow": m(), "aria-valuetext": m(), title: m(), "aria-labelledby": p });
                a.extend(this, { slider: u, handle: x, valuebg: F, dragging: false, beforeStart: null, userModified: false, mouseMoved: false }); if (g == "select") {
                    f = k.createElement("div"); f.className = "ui-slider-inneroffset"; h = 0; for (p = j.childNodes.length; h < p; h++) f.appendChild(j.childNodes[h]); j.appendChild(f); x.addClass("ui-slider-handle-snapping"); f = d.find("option"); j = 0; for (h = f.length; j < h; j++) p = !j ? "b" : "a", o = !j ? " ui-btn-down-" + e : " " + a.mobile.activeBtnClass, k.createElement("div"), t = k.createElement("span"), t.className = ["ui-slider-label ui-slider-label-",
                    p, o, " ui-btn-corner-all"].join(""), t.setAttribute("role", "img"), t.appendChild(k.createTextNode(f[j].innerHTML)), a(t).prependTo(u); b._labels = a(".ui-slider-label", u)
                } l.addClass("ui-slider"); d.addClass(g === "input" ? "ui-slider-input" : "ui-slider-switch").change(function () { b.mouseMoved || b.refresh(m(), true) }).keyup(function () { b.refresh(m(), true, true) }).blur(function () { b.refresh(m(), true) }); a(k).bind("vmousemove", function (a) {
                    if (b.dragging && !b.options.disabled) return b.mouseMoved = true, g === "select" && x.removeClass("ui-slider-handle-snapping"),
                    b.refresh(a), b.userModified = b.beforeStart !== d[0].selectedIndex, false
                }); d.bind("vmouseup", a.proxy(b._checkedRefresh, b)); u.bind("vmousedown", function (a) { if (b.options.disabled) return false; b.dragging = true; b.userModified = false; b.mouseMoved = false; if (g === "select") b.beforeStart = d[0].selectedIndex; b.refresh(a); return false }).bind("vclick", false); u.add(k).bind("vmouseup", function () {
                    if (b.dragging) return b.dragging = false, g === "select" && (x.addClass("ui-slider-handle-snapping"), b.mouseMoved ? b.userModified ? b.refresh(b.beforeStart ==
                    0 ? 1 : 0) : b.refresh(b.beforeStart) : b.refresh(b.beforeStart == 0 ? 1 : 0)), b.mouseMoved = false
                }); u.insertAfter(d); g == "select" && this.handle.bind({ focus: function () { u.addClass(a.mobile.focusClass) }, blur: function () { u.removeClass(a.mobile.focusClass) } }); this.handle.bind({
                    vmousedown: function () { a(this).focus() }, vclick: false, keydown: function (c) {
                        var d = m(); if (!b.options.disabled) {
                            switch (c.keyCode) {
                                case a.mobile.keyCode.HOME: case a.mobile.keyCode.END: case a.mobile.keyCode.PAGE_UP: case a.mobile.keyCode.PAGE_DOWN: case a.mobile.keyCode.UP: case a.mobile.keyCode.RIGHT: case a.mobile.keyCode.DOWN: case a.mobile.keyCode.LEFT: if (c.preventDefault(),
                                !b._keySliding) b._keySliding = true, a(this).addClass("ui-state-active")
                            } switch (c.keyCode) { case a.mobile.keyCode.HOME: b.refresh(w); break; case a.mobile.keyCode.END: b.refresh(n); break; case a.mobile.keyCode.PAGE_UP: case a.mobile.keyCode.UP: case a.mobile.keyCode.RIGHT: b.refresh(d + r); break; case a.mobile.keyCode.PAGE_DOWN: case a.mobile.keyCode.DOWN: case a.mobile.keyCode.LEFT: b.refresh(d - r) }
                        }
                    }, keyup: function () { if (b._keySliding) b._keySliding = false, a(this).removeClass("ui-state-active") }
                }); this.refresh(c, c, true)
            },
            _checkedRefresh: function () { if (this.value != this._value()) this.refresh(this._value()), this.value = this._value() }, _value: function () { return this._type === "input" ? parseFloat(this.element.val()) : this.element[0].selectedIndex }, refresh: function (b, c, e) {
                (this.options.disabled || this.element.attr("disabled")) && this.disable(); var f = this.element, g = f[0].nodeName.toLowerCase(), h = g === "input" ? parseFloat(f.attr("min")) : 0, j = g === "input" ? parseFloat(f.attr("max")) : f.find("option").length - 1, k = g === "input" && parseFloat(f.attr("step")) >
                0 ? parseFloat(f.attr("step")) : 1; if (typeof b === "object") { if (!this.dragging || b.pageX < this.slider.offset().left - 8 || b.pageX > this.slider.offset().left + this.slider.width() + 8) return; b = Math.round((b.pageX - this.slider.offset().left) / this.slider.width() * 100) } else b == null && (b = g === "input" ? parseFloat(f.val() || 0) : f[0].selectedIndex), b = (parseFloat(b) - h) / (j - h) * 100; if (!isNaN(b)) {
                    b < 0 && (b = 0); b > 100 && (b = 100); var l = b / 100 * (j - h) + h, m = (l - h) % k; l -= m; Math.abs(m) * 2 >= k && (l += m > 0 ? k : -k); l = parseFloat(l.toFixed(5)); l < h && (l = h); l > j && (l =
                    j); this.handle.css("left", b + "%"); this.handle.attr({ "aria-valuenow": g === "input" ? l : f.find("option").eq(l).attr("value"), "aria-valuetext": g === "input" ? l : f.find("option").eq(l).getEncodedText(), title: g === "input" ? l : f.find("option").eq(l).getEncodedText() }); this.valuebg && this.valuebg.css("width", b + "%"); if (this._labels) {
                        var h = this.handle.width() / this.slider.width() * 100, s = b && h + (100 - h) * b / 100, n = b === 100 ? 0 : Math.min(h + 100 - s, 100); this._labels.each(function () {
                            var b = a(this).is(".ui-slider-label-a"); a(this).width((b ?
                                s : n) + "%")
                        })
                    } if (!e) e = false, g === "input" ? (e = f.val() !== l, f.val(l)) : (e = f[0].selectedIndex !== l, f[0].selectedIndex = l), !c && e && f.trigger("change")
                }
            }, enable: function () { this.element.attr("disabled", false); this.slider.removeClass("ui-disabled").attr("aria-disabled", false); return this._setOption("disabled", false) }, disable: function () { this.element.attr("disabled", true); this.slider.addClass("ui-disabled").attr("aria-disabled", true); return this._setOption("disabled", true) }
        }); a(k).bind("pagecreate create", function (b) {
            a.mobile.slider.prototype.enhanceWithin(b.target,
            true)
        })
    })(l); (function (a) {
        a.widget("mobile.selectmenu", a.mobile.widget, {
            options: { theme: null, disabled: false, icon: "arrow-d", iconpos: "right", inline: false, corners: true, shadow: true, iconshadow: true, overlayTheme: "a", hidePlaceholderMenuItems: true, closeText: "Close", nativeMenu: true, preventFocusZoom: /iPhone|iPad|iPod/.test(navigator.platform) && navigator.userAgent.indexOf("AppleWebKit") > -1, initSelector: "select:not(:jqmData(role='slider'))", mini: false }, _button: function () { return a("<div/>") }, _setDisabled: function (a) {
                this.element.attr("disabled",
                a); this.button.attr("aria-disabled", a); return this._setOption("disabled", a)
            }, _focusButton: function () { var a = this; setTimeout(function () { a.button.focus() }, 40) }, _selectOptions: function () { return this.select.find("option") }, _preExtension: function () {
                var c = ""; ~this.element[0].className.indexOf("ui-btn-left") && (c = " ui-btn-left"); ~this.element[0].className.indexOf("ui-btn-right") && (c = " ui-btn-right"); this.select = this.element.wrap("<div class='ui-select" + c + "'>"); this.selectID = this.select.attr("id"); this.label =
                a("label[for='" + this.selectID + "']").addClass("ui-select"); this.isMultiple = this.select[0].multiple; if (!this.options.theme) this.options.theme = a.mobile.getInheritedTheme(this.select, "c")
            }, _create: function () {
                this._preExtension(); this._trigger("beforeCreate"); this.button = this._button(); var c = this, b = this.options, d = b.inline || this.select.jqmData("inline"), e = b.mini || this.select.jqmData("mini"), f = b.icon ? b.iconpos || this.select.jqmData("iconpos") : false, d = this.button.insertBefore(this.select).buttonMarkup({
                    theme: b.theme,
                    icon: b.icon, iconpos: f, inline: d, corners: b.corners, shadow: b.shadow, iconshadow: b.iconshadow, mini: e
                }); this.setButtonText(); b.nativeMenu && s.opera && s.opera.version && d.addClass("ui-select-nativeonly"); if (this.isMultiple) this.buttonCount = a("<span>").addClass("ui-li-count ui-btn-up-c ui-btn-corner-all").hide().appendTo(d.addClass("ui-li-has-count")); (b.disabled || this.element.attr("disabled")) && this.disable(); this.select.change(function () { c.refresh() }); this.build()
            }, build: function () {
                var c = this; this.select.appendTo(c.button).bind("vmousedown",
                function () { c.button.addClass(a.mobile.activeBtnClass) }).bind("focus", function () { c.button.addClass(a.mobile.focusClass) }).bind("blur", function () { c.button.removeClass(a.mobile.focusClass) }).bind("focus vmouseover", function () { c.button.trigger("vmouseover") }).bind("vmousemove", function () { c.button.removeClass(a.mobile.activeBtnClass) }).bind("change blur vmouseout", function () { c.button.trigger("vmouseout").removeClass(a.mobile.activeBtnClass) }).bind("change blur", function () {
                    c.button.removeClass("ui-btn-down-" +
                    c.options.theme)
                }); c.button.bind("vmousedown", function () { c.options.preventFocusZoom && a.mobile.zoom.disable(true) }); c.label.bind("click focus", function () { c.options.preventFocusZoom && a.mobile.zoom.disable(true) }); c.select.bind("focus", function () { c.options.preventFocusZoom && a.mobile.zoom.disable(true) }); c.button.bind("mouseup", function () { c.options.preventFocusZoom && setTimeout(function () { a.mobile.zoom.enable(true) }, 0) }); c.select.bind("blur", function () { c.options.preventFocusZoom && a.mobile.zoom.enable(true) })
            },
            selected: function () { return this._selectOptions().filter(":selected") }, selectedIndices: function () { var a = this; return this.selected().map(function () { return a._selectOptions().index(this) }).get() }, setButtonText: function () { var c = this, b = this.selected(), d = this.placeholder, e = a(k.createElement("span")); this.button.find(".ui-btn-text").html(function () { d = b.length ? b.map(function () { return a(this).text() }).get().join(", ") : c.placeholder; return e.text(d).addClass(c.select.attr("class")).addClass(b.attr("class")) }) },
            setButtonCount: function () { var a = this.selected(); this.isMultiple && this.buttonCount[a.length > 1 ? "show" : "hide"]().text(a.length) }, refresh: function () { this.setButtonText(); this.setButtonCount() }, open: a.noop, close: a.noop, disable: function () { this._setDisabled(true); this.button.addClass("ui-disabled") }, enable: function () { this._setDisabled(false); this.button.removeClass("ui-disabled") }
        }); a(k).bind("pagecreate create", function (c) { a.mobile.selectmenu.prototype.enhanceWithin(c.target, true) })
    })(l); (function (a) {
        var c =
        function (b) {
            var c = b.selectID, e = b.label, f = b.select.closest(".ui-page"), g = a("<div>", { "class": "ui-selectmenu-screen ui-screen-hidden" }).appendTo(f), h = b._selectOptions(), j = b.isMultiple = b.select[0].multiple, l = c + "-button", p = c + "-menu", m = a("<div data-" + a.mobile.ns + "role='dialog' data-" + a.mobile.ns + "theme='" + b.options.theme + "' data-" + a.mobile.ns + "overlay-theme='" + b.options.overlayTheme + "'><div data-" + a.mobile.ns + "role='header'><div class='ui-title'>" + e.getEncodedText() + "</div></div><div data-" + a.mobile.ns +
            "role='content'></div></div>"), w = a("<div>", { "class": "ui-selectmenu ui-selectmenu-hidden ui-overlay-shadow ui-corner-all ui-body-" + b.options.overlayTheme + " " + a.mobile.defaultDialogTransition }).insertAfter(g), n = a("<ul>", { "class": "ui-selectmenu-list", id: p, role: "listbox", "aria-labelledby": l }).attr("data-" + a.mobile.ns + "theme", b.options.theme).appendTo(w), r = a("<div>", { "class": "ui-header ui-bar-" + b.options.theme }).prependTo(w), o = a("<h1>", { "class": "ui-title" }).appendTo(r), t; b.isMultiple && (t = a("<a>", {
                text: b.options.closeText,
                href: "#", "class": "ui-btn-left"
            }).attr("data-" + a.mobile.ns + "iconpos", "notext").attr("data-" + a.mobile.ns + "icon", "delete").appendTo(r).buttonMarkup()); a.extend(b, {
                select: b.select, selectID: c, buttonId: l, menuId: p, thisPage: f, menuPage: m, label: e, screen: g, selectOptions: h, isMultiple: j, theme: b.options.theme, listbox: w, list: n, header: r, headerTitle: o, headerClose: t, menuPageContent: void 0, menuPageClose: void 0, placeholder: "", build: function () {
                    var c = this; c.refresh(); c.select.attr("tabindex", "-1").focus(function () {
                        a(this).blur();
                        c.button.focus()
                    }); c.button.bind("vclick keydown", function (b) { if (b.type == "vclick" || b.keyCode && (b.keyCode === a.mobile.keyCode.ENTER || b.keyCode === a.mobile.keyCode.SPACE)) c.open(), b.preventDefault() }); c.list.attr("role", "listbox").bind("focusin", function (b) { a(b.target).attr("tabindex", "0").trigger("vmouseover") }).bind("focusout", function (b) { a(b.target).attr("tabindex", "-1").trigger("vmouseout") }).delegate("li:not(.ui-disabled, .ui-li-divider)", "click", function (d) {
                        var e = c.select[0].selectedIndex, f = c.list.find("li:not(.ui-li-divider)").index(this),
                        g = c._selectOptions().eq(f)[0]; g.selected = c.isMultiple ? !g.selected : true; c.isMultiple && a(this).find(".ui-icon").toggleClass("ui-icon-checkbox-on", g.selected).toggleClass("ui-icon-checkbox-off", !g.selected); (c.isMultiple || e !== f) && c.select.trigger("change"); c.isMultiple ? c.list.find("li:not(.ui-li-divider)").eq(f).addClass("ui-btn-down-" + b.options.theme).find("a").first().focus() : c.close(); d.preventDefault()
                    }).keydown(function (c) {
                        var d = a(c.target), e = d.closest("li"); switch (c.keyCode) {
                            case 38: return c = e.prev().not(".ui-selectmenu-placeholder"),
                            c.is(".ui-li-divider") && (c = c.prev()), c.length && (d.blur().attr("tabindex", "-1"), c.addClass("ui-btn-down-" + b.options.theme).find("a").first().focus()), false; case 40: return c = e.next(), c.is(".ui-li-divider") && (c = c.next()), c.length && (d.blur().attr("tabindex", "-1"), c.addClass("ui-btn-down-" + b.options.theme).find("a").first().focus()), false; case 13: case 32: return d.trigger("click"), false
                        }
                    }); c.menuPage.bind("pagehide", function () { c.list.appendTo(c.listbox); c._focusButton(); a.mobile._bindPageRemove.call(c.thisPage) });
                    c.screen.bind("vclick", function () { c.close() }); c.isMultiple && c.headerClose.click(function () { if (c.menuType == "overlay") return c.close(), false }); c.thisPage.addDependents(this.menuPage)
                }, _isRebuildRequired: function () { var a = this.list.find("li"); return this._selectOptions().text() !== a.text() }, selected: function () { return this._selectOptions().filter(":selected:not(:jqmData(placeholder='true'))") }, refresh: function (b) {
                    var c = this, d; (b || this._isRebuildRequired()) && c._buildList(); d = this.selectedIndices(); c.setButtonText();
                    c.setButtonCount(); c.list.find("li:not(.ui-li-divider)").removeClass(a.mobile.activeBtnClass).attr("aria-selected", false).each(function (b) { a.inArray(b, d) > -1 && (b = a(this), b.attr("aria-selected", true), c.isMultiple ? b.find(".ui-icon").removeClass("ui-icon-checkbox-off").addClass("ui-icon-checkbox-on") : b.is(".ui-selectmenu-placeholder") ? b.next().addClass(a.mobile.activeBtnClass) : b.addClass(a.mobile.activeBtnClass)) })
                }, close: function () {
                    if (!this.options.disabled && this.isOpen) this.menuType == "page" ? a.mobile.back() :
                    (this.screen.addClass("ui-screen-hidden"), this.listbox.addClass("ui-selectmenu-hidden").removeAttr("style").removeClass("in"), this.list.appendTo(this.listbox), this._focusButton()), this.isOpen = false
                }, open: function () {
                    function c() { var e = d.list.find("." + a.mobile.activeBtnClass + " a"); e.length === 0 && (e = d.list.find("li.ui-btn:not(:jqmData(placeholder='true')) a")); e.first().focus().closest("li").addClass("ui-btn-down-" + b.options.theme) } if (!this.options.disabled) {
                        var d = this, e = a(s), f = d.list.parent(), g = f.outerHeight(),
                        f = f.outerWidth(); a(".ui-page-active"); var h = e.scrollTop(), j = d.button.offset().top, l = e.height(), e = e.width(); d.button.addClass(a.mobile.activeBtnClass); setTimeout(function () { d.button.removeClass(a.mobile.activeBtnClass) }, 300); if (g > l - 80 || !a.support.scrollTop) {
                            d.menuPage.appendTo(a.mobile.pageContainer).page(); d.menuPageContent = m.find(".ui-content"); d.menuPageClose = m.find(".ui-header a"); d.thisPage.unbind("pagehide.remove"); if (h == 0 && j > l) d.thisPage.one("pagehide", function () {
                                a(this).jqmData("lastScroll",
                                j)
                            }); d.menuPage.one("pageshow", function () { c(); d.isOpen = true }).one("pagehide", function () { d.isOpen = false }); d.menuType = "page"; d.menuPageContent.append(d.list); d.menuPage.find("div .ui-title").text(d.label.text()); a.mobile.changePage(d.menuPage, { transition: a.mobile.defaultDialogTransition })
                        } else {
                            d.menuType = "overlay"; d.screen.height(a(k).height()).removeClass("ui-screen-hidden"); var n = j - h, o = h + l - j, p = g / 2, q = parseFloat(d.list.parent().css("max-width")), g = n > g / 2 && o > g / 2 ? j + d.button.outerHeight() / 2 - p : n > o ? h + l - g -
                            30 : h + 30; f < q ? h = (e - f) / 2 : (h = d.button.offset().left + d.button.outerWidth() / 2 - f / 2, h < 30 ? h = 30 : h + f > e && (h = e - f - 30)); d.listbox.append(d.list).removeClass("ui-selectmenu-hidden").css({ top: g, left: h }).addClass("in"); c(); d.isOpen = true
                        }
                    }
                }, _buildList: function () {
                    var b = this.options, c = this.placeholder, d = true, e = this.isMultiple ? "checkbox-off" : "false"; this.list.empty().filter(".ui-listview").listview("destroy"); var f = this.select.find("option"), g = f.length, h = this.select[0], j = "data-" + a.mobile.ns, l = j + "option-index", m = j + "icon",
                    n = j + "role"; j += "placeholder"; for (var o = k.createDocumentFragment(), p = false, q, s = 0; s < g; s++, p = false) {
                        var t = f[s], r = a(t), w = t.parentNode, C = r.text(), I = k.createElement("a"), L = []; I.setAttribute("href", "#"); I.appendChild(k.createTextNode(C)); w !== h && w.nodeName.toLowerCase() === "optgroup" && (w = w.getAttribute("label"), w != q && (q = k.createElement("li"), q.setAttribute(n, "list-divider"), q.setAttribute("role", "option"), q.setAttribute("tabindex", "-1"), q.appendChild(k.createTextNode(w)), o.appendChild(q), q = w)); if (d && (!t.getAttribute("value") ||
                        C.length == 0 || r.jqmData("placeholder"))) if (d = false, p = true, t.setAttribute(j, true), b.hidePlaceholderMenuItems && L.push("ui-selectmenu-placeholder"), !c) c = this.placeholder = C; r = k.createElement("li"); t.disabled && (L.push("ui-disabled"), r.setAttribute("aria-disabled", true)); r.setAttribute(l, s); r.setAttribute(m, e); p && r.setAttribute(j, true); r.className = L.join(" "); r.setAttribute("role", "option"); I.setAttribute("tabindex", "-1"); r.appendChild(I); o.appendChild(r)
                    } this.list[0].appendChild(o); !this.isMultiple && !c.length ?
                    this.header.hide() : this.headerTitle.text(this.placeholder); this.list.listview()
                }, _button: function () { return a("<a>", { href: "#", role: "button", id: this.buttonId, "aria-haspopup": "true", "aria-owns": this.menuId }) }
            })
        }; a(k).bind("selectmenubeforecreate", function (b) { b = a(b.target).data("selectmenu"); b.options.nativeMenu || c(b) })
    })(l); (function (a) {
        a.widget("mobile.fixedtoolbar", a.mobile.widget, {
            options: {
                visibleOnPageShow: true, disablePageZoom: true, transition: "slide", fullscreen: false, tapToggle: true, tapToggleBlacklist: "a, button, input, select, textarea, .ui-header-fixed, .ui-footer-fixed",
                hideDuringFocus: "input, textarea, select", updatePagePadding: true, trackPersistentToolbars: true, supportBlacklist: function () {
                    var a = navigator.userAgent, b = navigator.platform, d = a.match(/AppleWebKit\/([0-9]+)/), d = !!d && d[1], e = a.match(/Fennec\/([0-9]+)/), e = !!e && e[1], f = a.match(/Opera Mobi\/([0-9]+)/), g = !!f && f[1]; return (b.indexOf("iPhone") > -1 || b.indexOf("iPad") > -1 || b.indexOf("iPod") > -1) && d && d < 534 || s.operamini && {}.toString.call(s.operamini) === "[object OperaMini]" || f && g < 7458 || a.indexOf("Android") > -1 && d && d < 533 ||
                    e && e < 6 || "palmGetResource" in s && d && d < 534 || a.indexOf("MeeGo") > -1 && a.indexOf("NokiaBrowser/8.5.0") > -1 ? true : false
                }, initSelector: ":jqmData(position='fixed')"
            }, _create: function () {
                var a = this.options, b = this.element, d = b.is(":jqmData(role='header')") ? "header" : "footer", e = b.closest(".ui-page"); a.supportBlacklist() ? this.destroy() : (b.addClass("ui-" + d + "-fixed"), a.fullscreen ? (b.addClass("ui-" + d + "-fullscreen"), e.addClass("ui-page-" + d + "-fullscreen")) : e.addClass("ui-page-" + d + "-fixed"), this._addTransitionClass(), this._bindPageEvents(),
                this._bindToggleHandlers())
            }, _addTransitionClass: function () { var a = this.options.transition; a && a !== "none" && (a === "slide" && (a = this.element.is(".ui-header") ? "slidedown" : "slideup"), this.element.addClass(a)) }, _bindPageEvents: function () {
                var c = this, b = c.options; c.element.closest(".ui-page").bind("pagebeforeshow", function () { b.disablePageZoom && a.mobile.zoom.disable(true); b.visibleOnPageShow || c.hide(true) }).bind("webkitAnimationStart animationstart updatelayout", function () { b.updatePagePadding && c.updatePagePadding(this) }).bind("pageshow",
                function () { var d = this; c.updatePagePadding(d); b.updatePagePadding && a(s).bind("throttledresize." + c.widgetName, function () { c.updatePagePadding(d) }) }).bind("pagebeforehide", function (d, e) {
                    b.disablePageZoom && a.mobile.zoom.enable(true); b.updatePagePadding && a(s).unbind("throttledresize." + c.widgetName); if (b.trackPersistentToolbars) {
                        var f = a(".ui-footer-fixed:jqmData(id)", this), g = a(".ui-header-fixed:jqmData(id)", this), h = f.length && e.nextPage && a(".ui-footer-fixed:jqmData(id='" + f.jqmData("id") + "')", e.nextPage),
                        j = g.length && e.nextPage && a(".ui-header-fixed:jqmData(id='" + g.jqmData("id") + "')", e.nextPage), h = h || a(); if (h.length || j.length) h.add(j).appendTo(a.mobile.pageContainer), e.nextPage.one("pageshow", function () { h.add(j).appendTo(this) })
                    }
                })
            }, _visible: true, updatePagePadding: function (c) { var b = this.element, d = b.is(".ui-header"); this.options.fullscreen || (c = c || b.closest(".ui-page"), a(c).css("padding-" + (d ? "top" : "bottom"), b.outerHeight())) }, _useTransition: function (c) {
                var b = this.element, d = a(s).scrollTop(), e = b.height(),
                f = b.closest(".ui-page").height(), g = a.mobile.getScreenHeight(), b = b.is(":jqmData(role='header')") ? "header" : "footer"; return !c && (this.options.transition && this.options.transition !== "none" && (b === "header" && !this.options.fullscreen && d > e || b === "footer" && !this.options.fullscreen && d + g < f - e) || this.options.fullscreen)
            }, show: function (a) { var b = this.element; this._useTransition(a) ? b.removeClass("out ui-fixed-hidden").addClass("in") : b.removeClass("ui-fixed-hidden"); this._visible = true }, hide: function (a) {
                var b = this.element,
                d = "out" + (this.options.transition === "slide" ? " reverse" : ""); this._useTransition(a) ? b.addClass(d).removeClass("in").animationComplete(function () { b.addClass("ui-fixed-hidden").removeClass(d) }) : b.addClass("ui-fixed-hidden").removeClass(d); this._visible = false
            }, toggle: function () { this[this._visible ? "hide" : "show"]() }, _bindToggleHandlers: function () {
                var c = this, b = c.options; c.element.closest(".ui-page").bind("vclick", function (d) { b.tapToggle && !a(d.target).closest(b.tapToggleBlacklist).length && c.toggle() }).bind("focusin focusout",
                function (d) { if (screen.width < 500 && a(d.target).is(b.hideDuringFocus) && !a(d.target).closest(".ui-header-fixed, .ui-footer-fixed").length) c[d.type === "focusin" && c._visible ? "hide" : "show"]() })
            }, destroy: function () { var a = this.element, b = a.is(".ui-header"); a.closest(".ui-page").css("padding-" + (b ? "top" : "bottom"), ""); a.removeClass("ui-header-fixed ui-footer-fixed ui-header-fullscreen ui-footer-fullscreen in out fade slidedown slideup ui-fixed-hidden"); a.closest(".ui-page").removeClass("ui-page-header-fixed ui-page-footer-fixed ui-page-header-fullscreen ui-page-footer-fullscreen") }
        });
        a(k).bind("pagecreate create", function (c) { a(c.target).jqmData("fullscreen") && a(a.mobile.fixedtoolbar.prototype.options.initSelector, c.target).not(":jqmData(fullscreen)").jqmData("fullscreen", true); a.mobile.fixedtoolbar.prototype.enhanceWithin(c.target) })
    })(l); (function (a, c) {
        if (/iPhone|iPad|iPod/.test(navigator.platform) && navigator.userAgent.indexOf("AppleWebKit") > -1) {
            var b = a.mobile.zoom, d, e, f, g, h; a(c).bind("orientationchange.iosorientationfix", b.enable).bind("devicemotion.iosorientationfix", function (a) {
                d =
                a.originalEvent; h = d.accelerationIncludingGravity; e = Math.abs(h.x); f = Math.abs(h.y); g = Math.abs(h.z); !c.orientation && (e > 7 || (g > 6 && f < 8 || g < 8 && f > 6) && e > 5) ? b.enabled && b.disable() : b.enabled || b.enable()
            })
        }
    })(l, this); (function (a, c) {
        function b() { var b = a("." + a.mobile.activeBtnClass).first(); h.css({ top: a.support.scrollTop && g.scrollTop() + g.height() / 2 || b.length && b.offset().top || 100 }) } function d() {
            var c = h.offset(), e = g.scrollTop(), f = a.mobile.getScreenHeight(); if (c.top < e || c.top - e > f) h.addClass("ui-loader-fakefix"), b(),
            g.unbind("scroll", d).bind("scroll", b)
        } function e() { f.removeClass("ui-mobile-rendering") } var f = a("html"); a("head"); var g = a(c); a(c.document).trigger("mobileinit"); if (a.mobile.gradeA()) {
            if (a.mobile.ajaxBlacklist) a.mobile.ajaxEnabled = false; f.addClass("ui-mobile ui-mobile-rendering"); setTimeout(e, 5E3); var h = a("<div class='ui-loader'><span class='ui-icon ui-icon-loading'></span><h1></h1></div>"); a.extend(a.mobile, {
                showPageLoadingMsg: function (b, c, e) {
                    f.addClass("ui-loading"); if (a.mobile.loadingMessage) {
                        var k =
                        e || a.mobile.loadingMessageTextVisible; b = b || a.mobile.loadingMessageTheme; h.attr("class", "ui-loader ui-corner-all ui-body-" + (b || "a") + " ui-loader-" + (k ? "verbose" : "default") + (e ? " ui-loader-textonly" : "")).find("h1").text(c || a.mobile.loadingMessage).end().appendTo(a.mobile.pageContainer); d(); g.bind("scroll", d)
                    }
                }, hidePageLoadingMsg: function () { f.removeClass("ui-loading"); a.mobile.loadingMessage && h.removeClass("ui-loader-fakefix"); a(c).unbind("scroll", b); a(c).unbind("scroll", d) }, initializePage: function () {
                    var b =
                    a(":jqmData(role='page'), :jqmData(role='dialog')"); b.length || (b = a("body").wrapInner("<div data-" + a.mobile.ns + "role='page'></div>").children(0)); b.each(function () { var b = a(this); b.jqmData("url") || b.attr("data-" + a.mobile.ns + "url", b.attr("id") || location.pathname + location.search) }); a.mobile.firstPage = b.first(); a.mobile.pageContainer = b.first().parent().addClass("ui-mobile-viewport"); g.trigger("pagecontainercreate"); a.mobile.showPageLoadingMsg(); e(); !a.mobile.hashListeningEnabled || !a.mobile.path.isHashValid(location.hash) ||
                    !a(location.hash + ':jqmData(role="page")').length && !a.mobile.path.isPath(location.hash) ? a.mobile.changePage(a.mobile.firstPage, { transition: "none", reverse: true, changeHash: false, fromHashChange: true }) : g.trigger("hashchange", [true])
                }
            }); a.mobile.navreadyDeferred.resolve(); a(function () {
                c.scrollTo(0, 1); a.mobile.defaultHomeScroll = !a.support.scrollTop || a(c).scrollTop() === 1 ? 0 : 1; a.fn.controlgroup && a(k).bind("pagecreate create", function (b) { a(":jqmData(role='controlgroup')", b.target).jqmEnhanceable().controlgroup({ excludeInvisible: false }) });
                a.mobile.autoInitializePage && a.mobile.initializePage(); g.load(a.mobile.silentScroll); a.support.cssPointerEvents || a(k).delegate(".ui-disabled", "vclick", function (a) { a.preventDefault(); a.stopImmediatePropagation() })
            })
        }
    })(l, this)
});