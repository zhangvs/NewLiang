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
        for (var e = 0, t = 0; t < n.length; t++) 127 < n.charCodeAt(t) || 94 === n.charCodeAt(t) ? e += 2 : e++;
        return e
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
    checkIsMiniProgram: function (e, t, n, o) {
        n && n(), iUtils.isMobile.WeChat() ? wx.miniProgram.getEnv(function (n) {
            n.miniprogram ? e && e() : t && t(), o && o()
        }) : (t && t(), o && o())
    },
    checkPassword: function (n) {
        return /^[A-Za-z0-9_-]{6,20}$/.test(n)
    },
    checkYzm: function (n, e, t) {
        return e = e || 4, t = t || 6, new RegExp("^\\w{" + e + "," + t + "}$").test(n)
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
    isType: function (n, e) {
        for (var t = [
            ["number", "Number"],
            ["string", "String"],
            ["undefined", "Undefined"],
            ["bool", "Boolean"],
            ["object", "Object"],
            ["array", "Array"],
            ["function", "Function"],
            ["null", "Null"]
        ], o = [], i = 0; i < t.length; i++) t[i][0] === e && (o = t[i]);
        if (!o.length) throw new Error("类型不存在");
        return Object.prototype.toString.call(n) === "[object " + o[1] + "]"
    },
    currency: function (n, e) {
        var t = parseFloat(n);
        if (isNaN(t)) return 0;
        var o = (t = Math.round(100 * t) / 100).toString(),
            i = o.indexOf(".");
        for (i < 0 && (i = o.length, o += "."); o.length <= i + 2;) o += "0";
        return e ? o.split(".") : o
    },
    asyncLoadJS: function (n, e) {
        var t = document.getElementsByTagName("HEAD").item(0),
            o = document.createElement("script");
        o.type = "text/javascript", o.src = n, o.onload = function () {
            e && e()
        }, t.appendChild(o)
    },
    formatDate: function (n, e) {
        n || 0 === n || (n = Date.now()), e = e || "yyyy-MM-dd hh:mm:ss", "string" == typeof n && (n = n.replace(/-/g, "/"));
        var t = new Date(n),
            o = {
                "M+": t.getMonth() + 1,
                "d+": t.getDate(),
                "h+": t.getHours(),
                "m+": t.getMinutes(),
                "s+": t.getSeconds(),
                "q+": Math.floor((t.getMonth() + 3) / 3),
                S: t.getMilliseconds()
            };
        for (var i in /(y+)/.test(e) && (e = e.replace(RegExp.$1, (t.getFullYear() + "").substr(4 - RegExp.$1.length))), o) new RegExp("(" + i + ")").test(e) && (e = e.replace(RegExp.$1, 1 === RegExp.$1.length ? o[i] : ("00" + o[i]).substr(("" + o[i]).length)));
        return e
    },
    getDay: function (n, e) {
        n = n || Date.now();
        return (e = e || "周") + "日一二三四五六"[new Date(n).getDay()]
    },
    getQuery: function (n, e) {
        var t = e || window.location.search,
            o = new RegExp("(^|&)" + n + "=([^&]*)(&|$)"),
            i = t.substr(t.indexOf("?") + 1).match(o);
        return null !== i ? i[2] : ""
    },
    parseJSON: function (n) {
        return "undefined" != typeof JSON ? JSON.parse(n) : new Function("return " + n)()
    },
    getLength: function (n) {
        for (var e = 0, t = n.split(""), o = 0; o < t.length; o++) t[o].charCodeAt(0) < 299 ? e++ : e += 2;
        return e
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
        for (var e = 0, t = n.split(""), o = 0; o < t.length; o++) t[o].charCodeAt(0) < 299 ? e++ : e += 2;
        return e
    },
    checkAddress: function (n, e, t) {
        return !(n.length < e || n.length > t) && !/[~!@#$%^&*\\|"']/.test(n)
    },
    uuid: function () {
        return "xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx".replace(/[xy]/g, function (n) {
            var e = 16 * Math.random() | 0;
            return ("x" === n ? e : 3 & e | 8).toString(16)
        })
    },
    rand: function (n, e) {
        return Math.floor(Math.random() * (e - n + 1) + n)
    },
    createUrl: function (e, n) {
        n = n || location.origin + location.pathname, e = e || {};
        var t, o = location.search.split("?")[1],
            i = location.hash,
            r = [],
            a = [];
        o && o.split("&").forEach(function (n) {
            t = n.split("="), r.push([t[0], t[1]])
        }), r.forEach(function (n) {
            e.hasOwnProperty(n[0]) && (n[1] = e[n[0]]), a.push(n)
        });
        var s = r.map(function (n) {
            return n[0]
        });
        for (var c in e) e.hasOwnProperty(c) && -1 === $.inArray(c, s) && a.push([c, e[c]]);
        return n + "?" + a.reduce(function (n, e) {
            return (n ? n + "&" : "") + e.join("=")
        }, "") + i
    },
    uniqueArray: function (n) {
        (n = n || []).sort();
        for (var e = [], t = 0; t < n.length; t++) n[t] !== e[e.length - 1] && e.push(n[t]);
        return e
    },
    getTime: function (n, e) {
        var t = n / 1e3 % 60;
        return "d" === e ? t = n / 1e3 / 60 / 60 / 24 : "h" === e ? t = n / 1e3 / 60 / 60 % 24 : "m" === e && (t = n / 1e3 / 60 % 60), t = 1 === (t = Math.floor(t).toString()).length ? "0" + t : t
    },
    repeatStr: function (n, e) {
        return new Array(e + 1).join(n)
    },
    createRandom: function (n, e, t) {
        for (var o = [], i = (t = t || []).map(function (n) {
            return n[0]
        }), r = t.map(function (n) {
            return n[1]
        }), a = r.reduce(function (n, e) {
            return n - e
        }, 1), s = n; s <= e; s++) o.push({
            val: s,
            rate: -1 !== i.indexOf(s) ? r[i.indexOf(s)] : a / (e - n - i.length + 1)
        });
        for (var c = 0, u = 0; u < o.length; u++) {
            var l = o[u].rate;
            o[u].range = [c, c += 100 * l]
        }
        o.length && (o[o.length - 1].range[1] = 100);
        for (var d, f = 100 * Math.random(), h = 0; h < o.length; h++) if (f >= (d = o[h].range)[0] && f <= d[1]) return o[h].val;
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
            var e = [];
            if (iUtils.isType(n, "object")) for (var t in n) if (n.hasOwnProperty(t)) {
                var o, i, r = n[t],
                    a = {};
                if (a.id = t, a.name = t, a.childs = [], iUtils.isType(r, "object")) for (var s in r) {
                    r.hasOwnProperty(s) && (o = r[s], (i = {}).id = s, i.name = s, i.childs = [], iUtils.isType(o, "array") && o.forEach(function (n) {
                        i.childs.push({
                            id: n,
                            name: n
                        })
                    }), a.childs.push(i))
                }
                e.push(a)
            }
            return e
        },
        getAreaName: function (n, e, t) {
            if (!n.address) return "";
            var o = "",
                i = n.address[e];
            return t.forEach(function (n) {
                n.id.toString() === i.toString() && (o = n.name)
            }), o
        },
        getAreaArr: function (n, e, t, o) {
            var i = [],
                r = n.address,
                a = !1;
            if (r) {
                if ("provinceid" === o) for (var s in e) e.hasOwnProperty(s) && i.push(e[s]);
                else e.forEach(function (n) {
                    n.id.toString() === t.toString() && iUtils.isType(n.childs, "array") && (i = n.childs)
                });
                i.forEach(function (n) {
                    r[o] && n.id.toString() === r[o].toString() && (a = !0)
                }), !a && i.length && (r[o] = i[0].id.toString())
            }
            return i
        }
    },
    iCookie = {
        set: function (n, e, t) {
            var o = new Date;
            o.setTime(o.getTime() + 1e3 * t), document.cookie = t ? n + "=" + encodeURIComponent(e) + ";expires=" + o.toGMTString() + ";path=/;" : n + "=" + encodeURIComponent(e) + ";path=/;"
        },
        get: function (n) {
            for (var e, t = document.cookie.split("; "), o = null, i = 0; i < t.length; i++) if (n === (e = t[i].split("="))[0]) {
                o = decodeURIComponent(decodeURIComponent(e[1]));
                break
            }
            return o
        },
        delete: function (n) {
            null !== this.get(n) && this.set(n, null, -1e7)
        }
    };
Vue.filter("salePrice", function (n) {
    return n = 0 < n ? n : 0, parseFloat(iUtils.currency(n))
}), Vue.filter("time", function (n, e) {
    return iUtils.formatDate(n, e)
}), Vue.filter("mixMobile", function (n) {
    return (n = n || "").substr(0, 3) + "****" + n.substr(7)
}), Vue.filter("levelName", function (n) {
    return (n = n || 0) ? "一二三四五六七八九十".split("")[n + 1] + "级代理" : "免费代理"
}), function () {
    var t = navigator.appVersion;
    /iphone|ipod|ipad/i.test(t) && document.addEventListener("blur", function (n) {
        var e = n.target.localName;
        "input" !== e && "textarea" !== e || void 0 === window.isNotNeedFixInputError && (/OS\s+12_/i.test(t) ? document.body.scrollIntoView(!0) : document.body.scrollTop = document.body.scrollTop)
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
            var e = this,
                t = {
                    className: "",
                    container: $("body"),
                    aniTime: 500,
                    initFn: null,
                    closeFn: null
                },
                o = $.extend({}, t, n);
            $(".global_cover").length || ($('<div class="global_cover no_touch_action ' + o.className + '"></div>').appendTo(o.container), o.initFn && o.initFn(), o.aniTime && $(".global_cover").css({
                transitionDuration: o.aniTime + "ms"
            }), setTimeout(function () {
                $(".global_cover").addClass("active").click(function () {
                    o.closeFn ? o.closeFn() : e.hide(o.aniTime)
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
    function (i) {
        function r(n) {
            this.options = n, this.uuid = iUtils.uuid()
        }
        var a = [],
            s = {
                timers: [],
                duration: 0,
                node: null,
                countdown: function () {
                    var n = this,
                        e = setTimeout(function () {
                            n.close()
                        }, this.duration);
                    this.timers.push(e)
                },
                close: function () {
                    var n = this;
                    this.node.classList.remove("active");
                    var e = setTimeout(function () {
                        n.node && document.body.removeChild(n.node), n.onClose && n.onClose(), n.node = null
                    }, 500);
                    this.timers.push(e)
                },
                show: function (n, e, t, o) {
                    o = o || {}, e = e || 1500;
                    var i, r = this;
                    this.timers.forEach(function (n) {
                        window.clearTimeout(n)
                    }), this.timers = [], this.duration = e + 700, this.onClose = t, this.node ? this.node.innerHTML = n : ((i = document.createElement("div")).className = "global_toast " + o.className, i.innerHTML = n, document.body.appendChild(i), this.node = i), setTimeout(function () {
                        r.node.classList.add("active"), r.countdown()
                    }, 0)
                }
            };
        r.prototype = {
            show: function () {
                var n = this,
                    e = {
                        wrap: null,
                        className: "",
                        container: i("body"),
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
                    t = i.extend({}, e, n.options),
                    o = '<article class="global_modal no_touch_action"><div class="cover"></div><div class="inner ' + t.className + '">';
                return t.showClose && (o += '<a href="javascript:void(0);" class="close_btn">关闭</a>'), t.hideHeader || (o += "<header><h3>" + t.title + "</h3></header>"), o += '<div class="content">' + t.content + "</div>", o += "<footer>", t.showConfirm && t.showCancel ? o += '<div class="btn_wrap both"><a class="btn cancel_btn" href="javascript:void(0)">' + t.cancelText + '</a><a class="btn confirm_btn" href="javascript:void(0)">' + t.confirmText + "</a></div>" : o += '<div class="btn_wrap single"><a class="btn confirm_btn">' + t.confirmText + "</a></div>", o += "</footer></div></article>", n.wrap = i(o).appendTo(t.container), iUtils.isType(t.content, "object") && n.wrap.find(".content").html(t.content), t.initFn(), n.wrap.css(t.wrapCss).find(".cover").css(t.coverCss).end().find(".inner").css(t.coverCss).end().find("header").css(t.headerCss).end().find("header h3").css(t.h3Css).end().find("footer").css(t.footerCss).end().find("footer a").css(t.linkCss), n.wrap.find(".confirm_btn").click(function () {
                    t.confirmFn()
                }).end().find(".cancel_btn").click(function () {
                    t.cancelFn()
                }).end().find(".close_btn").click(function () {
                    n.hide(), t.closeFn && t.closeFn()
                }), n.wrap.fadeIn(100), a.push({
                    key: n.uuid,
                    o: n
                }), n
            },
            hide: function () {
                var e = this,
                    n = this.wrap;
                n.fadeOut(100, function () {
                    n.remove(), a = a.filter(function (n) {
                        return n.key !== e.uuid
                    })
                })
            }
        }, i.extend({
            alert: function (n, e) {
                var t = new r(i.extend({}, {
                    content: n,
                    className: "alert",
                    confirmFn: function () {
                        t.hide()
                    }
                }, e));
                return t.show()
            },
            confirm: function (n, e, t) {
                var o = new r(i.extend({}, {
                    content: n,
                    className: "confirm",
                    hideHeader: !0,
                    showConfirm: !0,
                    showCancel: !0,
                    confirmFn: e ||
                        function () {
                            o.hide()
                        }
                }, t));
                return o.show()
            },
            removeDialog: function (e) {
                e ? a.forEach(function (n) {
                    n.key === e && n.o.hide()
                }) : a.forEach(function (n) {
                    n.o.hide()
                })
            },
            getDialogArr: function () {
                return a
            },
            toast: function (n, e, t, o) {
                s.show(n, e, t, o)
            }
        })
    }(jQuery);
var globalBackTop = {
    dom: null,
    create: function () {
        this.dom = $('<a href="javascript:void(0)" class="global_back_top hide" id="backTop"></a>').appendTo($("body"))
    },
    bindEvent: function () {
        var e = this.dom,
            t = $(window).height();
        $(window).scroll(function () {
            var n = $(this).scrollTop();
            t / 2 < n ? e.removeClass("hide") : e.addClass("hide")
        }), e.click(function () {
            $("html,body").animate({
                scrollTop: 0
            })
        })
    },
    init: function () {
        this.create(), this.bindEvent()
    }
};

function fnTransition(n, e, t) {
    t = t || 0, n.css({
        "-webkit-transition": "all " + e + "s " + t + "s",
        transition: "all " + e + "s " + t + "s"
    })
}
function fnTranslate(n, e, t) {
    n.css({
        "-webkit-transform": "translate3d(" + e + "px," + t + "px,0)",
        transform: "translate3d(" + e + "px," + t + "px,0)"
    })
}
window.GlobalConfig = {
    APP_KEY: "DS_CUSTOMER",
    baseUrl: window.agentApi || "/webapp/agentApi/"
}, function () {
    var e = GlobalConfig.APP_KEY + "_TOKEN",
        n = {
            isLogin: function () {
                return !!n.getToken()
            },
            setToken: function (n) {
                iCookie.set(e, n, 7776e3)
            },
            getToken: function () {
                return iCookie.get(e)
            },
            removeToken: function () {
                iCookie.delete(e)
            }
        };
    iUtils.getQuery("clearToken") && n.removeToken(), window.iAuth = n
}(), window.iApi = function (n) {
    n = n || {};
    var e = new $.Deferred,
        t = (n = $.extend({
            url: "/",
            data: {},
            method: "POST",
            notNeedLogin: !1
        }, n)).notNeedLogin,
        o = n.data,
        i = GlobalConfig.baseUrl + n.url,
        r = n.method;
    if (!t && !iAuth.isLogin()) return e.reject({
        code: -999,
        msg: "请先登录"
    }), e.promise();
    for (var a in o) {
        !o.hasOwnProperty(a) || null == o[a] && (o[a] = "")
    }
    var s = iAuth.getToken();
    s && (o = $.extend(o, {
        token: s
    }));
    var c = {
        method: r.toLowerCase(),
        url: i,
        data: o
    };
    o instanceof FormData && (c.contentType = !1, c.processData = !1);
    var u = e.resolve,
        l = e.reject;
    return $.ajax(c).done(function (n) {
        var e = n.code,
            t = n.msg || "未知错误",
            o = n.data;
        4e3 === e && (iAuth.removeToken(), l({
            code: e,
            msg: "登录凭证过期，请重新登录"
        })), 200 !== e && 400 !== e || u(o), l({
            code: e,
            msg: t
        })
    }).fail(function (n) {
        console.error(n), l({
            code: -9999,
            msg: "客户端捕获未知错误"
        })
    }), e.promise()
}, function () {
    var t, o = {
        x: "undefined",
        y: "undefined"
    };
    window.scrollFunc = function () {
        void 0 === o.x && (o.x = window.pageXOffset, o.y = window.pageYOffset);
        var n = o.x - window.pageXOffset,
            e = o.y - window.pageYOffset;
        n < 0 ? t = "right" : 0 < n ? t = "left" : e < 0 ? t = "down" : 0 < e && (t = "up"), o.x = window.pageXOffset, o.y = window.pageYOffset, window.scrollDirection = t
    }
}();