var iUtils = {
    isEmpty: function (n) {
        switch (typeof n) {
            case "string":
                return 0 === $.trim(n).length;
            case "number":
                return 0 === n;
            case "object":
                return null === n;
            case "array":
                return 0 === n.length;
            default:
                return !0
        }
    },
    isChinese: function (n) {
        return /^[\u4e00-\u9fa5]+$/.exec(n)
    },
    strLen: function (n) {
        for (var t = 0, e = 0; e < n.length; e++) 127 < n.charCodeAt(e) || 94 === n.charCodeAt(e) ? t += 2 : t++;
        return t
    },
    isDigital: function (n) {
        return /^\d+$/.test(n)
    },
    isTel: function (n) {
        return /^(0[0-9]{2,3}-)?([2-9][0-9]{6,7})+(-[0-9]{1,6})?$/.test(n)
    },
    isUserName: function (n) {
        return /(^[a-zA-Z0-9][a-zA-Z0-9_\-.]*@[a-zA-Z0-9]+$)|(^[A-Za-z0-9_\-.]*@+$)|(^[a-zA-Z0-9][a-zA-Z0-9_\-.]+$)/.test(n)
    },
    isMobile: {
        Android: function () {
            return navigator.userAgent.match(/Android/i)
        },
        IOS: function () {
            return navigator.userAgent.match(/iPhone|iPad|iPod/i)
        },
        Opera: function () {
            return navigator.userAgent.match(/Opera Mini/i)
        },
        Windows: function () {
            return navigator.userAgent.match(/IEMobile/i)
        },
        QQ: function () {
            return navigator.userAgent.match(/QQ/i)
        },
        WeChat: function () {
            return navigator.userAgent.match(/MicroMessenger/i)
        },
        Alipay: function () {
            return navigator.userAgent.match(/AlipayClient/i)
        }
    },
    checkIsMiniProgram: function (t, e, n, i) {
        n && n(), iUtils.isMobile.WeChat() ? wx.miniProgram.getEnv(function (n) {
            n.miniprogram ? t && t() : e && e(), i && i()
        }) : (e && e(), i && i())
    },
    checkPassword: function (n) {
        return /^[A-Za-z0-9_-]{6,20}$/.test(n)
    },
    checkYzm: function (n, t, e) {
        return t = t || 4, e = e || 6, new RegExp("^\\w{" + t + "," + e + "}$").test(n)
    },
    checkEmail: function (n) {
        return /^[\w-]+(\.[\w-]+)*@[A-Za-z0-9]+((.|-|_)[A-Za-z0-9]+)*\.[A-Za-z0-9]+$/.test(n)
    },
    checkMobile: function (n) {
        return /^1\d{10}$/.test(n)
    },
    subTime: function (n) {
        return n < 10 && (n = "0" + n), n + ""
    },
    isType: function (n, t) {
        for (var e = [
            ["number", "Number"],
            ["string", "String"],
            ["undefined", "Undefined"],
            ["bool", "Boolean"],
            ["object", "Object"],
            ["array", "Array"],
            ["function", "Function"],
            ["null", "Null"]
        ], i = [], o = 0; o < e.length; o++) e[o][0] === t && (i = e[o]);
        if (!i.length) throw new Error("类型不存在");
        return Object.prototype.toString.call(n) === "[object " + i[1] + "]"
    },
    currency: function (n, t) {
        var e = parseFloat(n);
        if (isNaN(e)) return 0;
        var i = (e = Math.round(100 * e) / 100).toString(),
            o = i.indexOf(".");
        for (o < 0 && (o = i.length, i += "."); i.length <= o + 2;) i += "0";
        return t ? i.split(".") : i
    },
    asyncLoadJS: function (n, t) {
        var e = document.getElementsByTagName("HEAD").item(0),
            i = document.createElement("script");
        i.type = "text/javascript", i.src = n, i.onload = function () {
            t && t()
        }, e.appendChild(i)
    },
    formatDate: function (n, t) {
        n || 0 === n || (n = Date.now()), t = t || "yyyy-MM-dd hh:mm:ss", "string" == typeof n && (n = n.replace(/-/g, "/"));
        var e = new Date(n),
            i = {
                "M+": e.getMonth() + 1,
                "d+": e.getDate(),
                "h+": e.getHours(),
                "m+": e.getMinutes(),
                "s+": e.getSeconds(),
                "q+": Math.floor((e.getMonth() + 3) / 3),
                S: e.getMilliseconds()
            };
        for (var o in /(y+)/.test(t) && (t = t.replace(RegExp.$1, (e.getFullYear() + "").substr(4 - RegExp.$1.length))), i) new RegExp("(" + o + ")").test(t) && (t = t.replace(RegExp.$1, 1 === RegExp.$1.length ? i[o] : ("00" + i[o]).substr(("" + i[o]).length)));
        return t
    },
    getDay: function (n, t) {
        n = n || Date.now();
        return (t = t || "周") + "日一二三四五六"[new Date(n).getDay()]
    },
    getQuery: function (n, t) {
        var e = t || window.location.search,
            i = new RegExp("(^|&)" + n + "=([^&]*)(&|$)"),
            o = e.substr(e.indexOf("?") + 1).match(i);
        return null !== o ? o[2] : ""
    },
    parseJSON: function (n) {
        return "undefined" != typeof JSON ? JSON.parse(n) : new Function("return " + n)()
    },
    getLength: function (n) {
        for (var t = 0, e = n.split(""), i = 0; i < e.length; i++) e[i].charCodeAt(0) < 299 ? t++ : t += 2;
        return t
    },
    getHref: function () {
        return location.href.split("#")[0]
    },
    getOrigin: function () {
        return location.protocol + "//" + location.hostname
    },
    checkProtocol: function (n) {
        return -1 === n.indexOf("http") ? location.protocol + n : n
    },
    len: function (n) {
        for (var t = 0, e = n.split(""), i = 0; i < e.length; i++) e[i].charCodeAt(0) < 299 ? t++ : t += 2;
        return t
    },
    checkAddress: function (n, t, e) {
        return !(n.length < t || n.length > e) && !/[~!@#$%^&*\\|"']/.test(n)
    },
    uuid: function () {
        return "xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx".replace(/[xy]/g, function (n) {
            var t = 16 * Math.random() | 0;
            return ("x" === n ? t : 3 & t | 8).toString(16)
        })
    },
    rand: function (n, t) {
        return Math.floor(Math.random() * (t - n + 1) + n)
    },
    createUrl: function (t, n) {
        n = n || location.origin + location.pathname, t = t || {};
        var e = location.search.split("?")[1],
            i = location.hash,
            o = [],
            r = [],
            a = [];
        e && e.split("&").forEach(function (n) {
            o = n.split("="), r.push([o[0], o[1]])
        }), r.forEach(function (n) {
            t.hasOwnProperty(n[0]) && (n[1] = t[n[0]]), a.push(n)
        });
        var c = r.map(function (n) {
            return n[0]
        });
        for (var s in t) t.hasOwnProperty(s) && -1 === $.inArray(s, c) && a.push([s, t[s]]);
        return n + "?" + a.reduce(function (n, t) {
            return (n ? n + "&" : "") + t.join("=")
        }, "") + i
    },
    uniqueArray: function (n) {
        (n = n || []).sort();
        for (var t = [], e = 0; e < n.length; e++) n[e] !== t[t.length - 1] && t.push(n[e]);
        return t
    },
    getTime: function (n, t) {
        var e = n / 1e3 % 60;
        return "d" === t ? e = n / 1e3 / 60 / 60 / 24 : "h" === t ? e = n / 1e3 / 60 / 60 % 24 : "m" === t && (e = n / 1e3 / 60 % 60), e = 1 === (e = Math.floor(e).toString()).length ? "0" + e : e
    },
    repeatStr: function (n, t) {
        return new Array(t + 1).join(n)
    },
    createRandom: function (n, t, e) {
        for (var i = [], o = (e = e || []).map(function (n) {
            return n[0]
        }), r = e.map(function (n) {
            return n[1]
        }), a = r.reduce(function (n, t) {
            return n - t
        }, 1), c = n; c <= t; c++) i.push({
            val: c,
            rate: -1 !== o.indexOf(c) ? r[o.indexOf(c)] : a / (t - n - o.length + 1)
        });
        for (var s = 0, u = 0; u < i.length; u++) {
            var l = i[u].rate;
            i[u].range = [s, s += 100 * l]
        }
        i.length && (i[i.length - 1].range[1] = 100);
        for (var d, f = 100 * Math.random(), h = 0; h < i.length; h++) if (f >= (d = i[h].range)[0] && f <= d[1]) return i[h].val;
        return 0
    }
},
    globalWeChat = {
        share: function (n) {
            n = $.extend({
                title: "",
                desc: "",
                link: "",
                imgUrl: "",
                success: function () { },
                cancel: function () { }
            }, n), iUtils.isMobile.WeChat() && wx.ready(function () {
                wx.onMenuShareTimeline(n), wx.onMenuShareAppMessage(n)
            })
        }
    },
    addressHandler = {
        createAddressArr: function (n) {
            var t = [];
            if (iUtils.isType(n, "object")) for (var e in n) if (n.hasOwnProperty(e)) {
                var i, o, r = n[e],
                    a = {};
                if (a.id = e, a.name = e, a.childs = [], iUtils.isType(r, "object")) for (var c in r) {
                    r.hasOwnProperty(c) && (i = r[c], (o = {}).id = c, o.name = c, o.childs = [], iUtils.isType(i, "array") && i.forEach(function (n) {
                        o.childs.push({
                            id: n,
                            name: n
                        })
                    }), a.childs.push(o))
                }
                t.push(a)
            }
            return t
        },
        getAreaName: function (n, t, e) {
            if (!n.address) return "";
            var i = "",
                o = n.address[t];
            return e.forEach(function (n) {
                n.id.toString() === o.toString() && (i = n.name)
            }), i
        },
        getAreaArr: function (n, t, e, i) {
            var o = [],
                r = n.address,
                a = !1;
            if (r) {
                if ("provinceid" === i) for (var c in t) t.hasOwnProperty(c) && o.push(t[c]);
                else t.forEach(function (n) {
                    n.id.toString() === e.toString() && iUtils.isType(n.childs, "array") && (o = n.childs)
                });
                o.forEach(function (n) {
                    r[i] && n.id.toString() === r[i].toString() && (a = !0)
                }), !a && o.length && (r[i] = o[0].id.toString())
            }
            return o
        }
    },
    iCookie = {
        set: function (n, t, e) {
            var i = new Date;
            i.setTime(i.getTime() + 1e3 * e), document.cookie = e ? n + "=" + encodeURIComponent(t) + ";expires=" + i.toGMTString() + ";path=/;" : n + "=" + encodeURIComponent(t) + ";path=/;"
        },
        get: function (n) {
            for (var t, e = document.cookie.split("; "), i = null, o = 0; o < e.length; o++) if (n === (t = e[o].split("="))[0]) {
                i = decodeURIComponent(decodeURIComponent(t[1]));
                break
            }
            return i
        },
        delete: function (n) {
            null !== this.get(n) && this.set(n, null, -1e7)
        }
    };
Vue.filter("salePrice", function (n) {
    return n = 0 < n ? n : 0, parseFloat(iUtils.currency(n))
}), Vue.filter("time", function (n, t) {
    return iUtils.formatDate(n, t)
}), Vue.filter("mixMobile", function (n) {
    return (n = n || "").substr(0, 3) + "****" + n.substr(7)
}), function () {
    var e = navigator.appVersion;
    /iphone|ipod|ipad/i.test(e) && document.addEventListener("blur", function (n) {
        var t = n.target.localName;
        "input" !== t && "textarea" !== t || void 0 === window.isNotNeedFixInputError && (/OS\s+12_/i.test(e) ? document.body.scrollIntoView(!0) : document.body.scrollTop = document.body.scrollTop)
    }, !0)
}();
var globalLoading = {
    show: function () {
        $("#globalLoading").length || $('<div id="globalLoading" class="global_loading no_touch_action"><div class="inner"></div><div class="inner"></div></div>').appendTo("body")
    },
    hide: function () {
        $("#globalLoading").remove()
    }
},
    globalMask = {
        show: function (n) {
            var t = this,
                e = {
                    className: "",
                    container: $("body"),
                    aniTime: 500,
                    initFn: null,
                    closeFn: null
                },
                i = $.extend({}, e, n);
            $(".global_cover").length || ($('<div class="global_cover no_touch_action ' + i.className + '"></div>').appendTo(i.container), i.initFn && i.initFn(), i.aniTime && $(".global_cover").css({
                transitionDuration: i.aniTime + "ms"
            }), setTimeout(function () {
                $(".global_cover").addClass("active").click(function () {
                    i.closeFn ? i.closeFn() : t.hide(i.aniTime)
                })
            }, 17))
        },
        hide: function (n) {
            iUtils.isType(n, "undefined") && (n = 500), $(".global_cover").removeClass("active"), setTimeout(function () {
                $(".global_cover").remove()
            }, n)
        }
    };
!
    function (o) {
        function r(n) {
            this.options = n, this.uuid = iUtils.uuid()
        }
        var a = [],
            c = {
                timers: [],
                duration: 0,
                node: null,
                countdown: function () {
                    var n = this,
                        t = setTimeout(function () {
                            n.close()
                        }, this.duration);
                    this.timers.push(t)
                },
                close: function () {
                    var n = this;
                    this.node.classList.remove("active");
                    var t = setTimeout(function () {
                        n.node && document.body.removeChild(n.node), n.onClose && n.onClose(), n.node = null
                    }, 500);
                    this.timers.push(t)
                },
                show: function (n, t, e, i) {
                    i = i || {}, t = t || 1500;
                    var o, r = this;
                    this.timers.forEach(function (n) {
                        window.clearTimeout(n)
                    }), this.timers = [], this.duration = t + 700, this.onClose = e, this.node ? this.node.innerHTML = n : ((o = document.createElement("div")).className = "global_toast " + i.className, o.innerHTML = n, document.body.appendChild(o), this.node = o), setTimeout(function () {
                        r.node.classList.add("active"), r.countdown()
                    }, 0)
                }
            };
        r.prototype = {
            show: function () {
                var n = this,
                    t = {
                        wrap: null,
                        className: "",
                        container: o("body"),
                        title: "提示",
                        content: "",
                        confirmText: "确定",
                        cancelText: "取消",
                        wrapCss: {},
                        coverCss: {},
                        innerCss: {},
                        closeCss: {},
                        headerCss: {},
                        h3Css: {},
                        footerCss: {},
                        linkCss: {},
                        showClose: !1,
                        initFn: function () { },
                        cancelFn: function () {
                            n.hide()
                        },
                        confirmFn: function () {
                            n.hide()
                        },
                        closeFn: function () {
                            n.hide()
                        }
                    },
                    e = o.extend({}, t, n.options),
                    i = '<article class="global_modal no_touch_action"><div class="cover"></div><div class="inner ' + e.className + '">';
                return e.showClose && (i += '<a href="javascript:void(0);" class="close_btn">关闭</a>'), e.hideHeader || (i += "<header><h3>" + e.title + "</h3></header>"), i += '<div class="content">' + e.content + "</div>", i += "<footer>", e.showConfirm && e.showCancel ? i += '<div class="btn_wrap both"><a class="btn cancel_btn" href="javascript:void(0)">' + e.cancelText + '</a><a class="btn confirm_btn" href="javascript:void(0)">' + e.confirmText + "</a></div>" : i += '<div class="btn_wrap single"><a class="btn confirm_btn">' + e.confirmText + "</a></div>", i += "</footer></div></article>", n.wrap = o(i).appendTo(e.container), iUtils.isType(e.content, "object") && n.wrap.find(".content").html(e.content), e.initFn(), n.wrap.css(e.wrapCss).find(".cover").css(e.coverCss).end().find(".inner").css(e.coverCss).end().find("header").css(e.headerCss).end().find("header h3").css(e.h3Css).end().find("footer").css(e.footerCss).end().find("footer a").css(e.linkCss), n.wrap.find(".confirm_btn").click(function () {
                    e.confirmFn()
                }).end().find(".cancel_btn").click(function () {
                    e.cancelFn()
                }).end().find(".close_btn").click(function () {
                    n.hide(), e.closeFn && e.closeFn()
                }), n.wrap.fadeIn(100), a.push({
                    key: n.uuid,
                    o: n
                }), n
            },
            hide: function () {
                var t = this,
                    n = this.wrap;
                n.fadeOut(100, function () {
                    n.remove(), a = a.filter(function (n) {
                        return n.key !== t.uuid
                    })
                })
            }
        }, o.extend({
            alert: function (n, t) {
                var e = new r(o.extend({}, {
                    content: n,
                    className: "alert",
                    confirmFn: function () {
                        e.hide()
                    }
                }, t));
                return e.show()
            },
            confirm: function (n, t, e) {
                var i = new r(o.extend({}, {
                    content: n,
                    className: "confirm",
                    hideHeader: !0,
                    showConfirm: !0,
                    showCancel: !0,
                    confirmFn: t ||
                        function () {
                            i.hide()
                        }
                }, e));
                return i.show()
            },
            removeDialog: function (t) {
                t ? a.forEach(function (n) {
                    n.key === t && n.o.hide()
                }) : a.forEach(function (n) {
                    n.o.hide()
                })
            },
            getDialogArr: function () {
                return a
            },
            toast: function (n, t, e, i) {
                c.show(n, t, e, i)
            }
        })
    }(jQuery);
var globalBackTop = {
    dom: null,
    create: function () {
        this.dom = $('<a href="javascript:void(0)" class="global_back_top hide" id="backTop"></a>').appendTo($("body"))
    },
    bindEvent: function () {
        var t = this.dom,
            e = $(window).height();
        $(window).scroll(function () {
            var n = $(this).scrollTop();
            e / 2 < n ? t.removeClass("hide") : t.addClass("hide")
        }), t.click(function () {
            $("html,body").animate({
                scrollTop: 0
            })
        })
    },
    init: function () {
        this.create(), this.bindEvent()
    }
};

function fnTransition(n, t, e) {
    e = e || 0, n.css({
        "-webkit-transition": "all " + t + "s " + e + "s",
        transition: "all " + t + "s " + e + "s"
    })
}
function fnTranslate(n, t, e) {
    n.css({
        "-webkit-transform": "translate3d(" + t + "px," + e + "px,0)",
        transform: "translate3d(" + t + "px," + e + "px,0)"
    })
}