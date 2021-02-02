var indexApp, scrollTimer, scrollTimer1;

$(function () {
    var appData = {},
        defaultData = {
            province: '',				//省份
            city: '',					//城市

            orderby: 'price',		// 排序字段名（仅一个）
            ordername: '',			//排序类型  asc  desc
            price: '0',					// -300  , 300-600 ,50000-

            regex: '',				//regex_id  号码分类的id

            isp: '',             //运营商
            repeatNumber: '',        //连号
            exceptNumber: '',        //排除数字
            moreNumber: '',          //数字较多
            birthdayNumber: ''      //生日
        };
    for (var key in defaultData) {
        if (defaultData.hasOwnProperty(key)) {
            appData[key] = defaultData[key];
            appData['tmp_' + key] = defaultData[key];
        }
    }

    //顶部banner，不需要可直接注释
    new Swiper('.swiper-container', {
        autoHeight: true,
        loop: true,
        speed: 600,
        autoplay: {
            delay: 1500,
            disableOnInteraction: false
        },
        pagination: {
            el: '.swiper-pagination'
        }
    });

    indexApp = new Vue({
        el: '#indexApp',
        data: $.extend(appData,
            {
                show_notice: show_notice,
                show_notice_background_color: show_notice_background_color,
                show_notice_color: show_notice_color,
                show_filter: show_filter,
                text_color: text_color,
                text_highlight_color: text_highlight_color,
                show_price: show_price,
                show_isp: show_isp,
                isNeedFetchProfit: window.is_agent === 1   //is_agent 为1 的时候  ,w.dslianghao.com 首页中   原价部分 不再需要, 显示  :利润估算, 共两个值: 直接销售利润(direct) , 下级代理销售利润(indirect)
            },
            {
                mobileList: [],
                cityList: [],            //城市列表

                page: 0,
                pageCount: 40,

                search: '',				//搜索词语  输入框的数字
                isAccuracy: false,       //是否精确手机号
                isTail: true,            //搜索是否是尾号
                isSearchInputFocused: false,    //手机输入框是否获取聚焦

                scrollTipsStyle: {},    //滚动提示样式
                scrollTipsList: [],      //滚动提示内容

                isShowFixNav: false,         //是否显示顶部导航条

                isShowFilterPrice: false,        //是否显示价格筛选
                isShowFilterSlide: false,        //是否显示侧边栏
                isShowFilterRegex: false,        //是否显示规律选号

                isNormal: 0,                        //推荐数据结束后请求正常数据标识
                isFetchingMore: false,
                isFetched: false,
                isFetchingProfit: false,            //是否正在请求代理利润
                hasMore: true
            }),
        created: function () {
            if (autocity === 1) {
                getCityByIp();
            } else {
                this.getData();
            }
            this.handleScroll();
            this.createAddressList();
        },
        mounted: function () {
            //手机号输入
            (function () {
                $('.accuracy_input').on('input propertychange', function () {
                    var me = $(this);
                    if (me.val() !== '') {
                        me.next('.accuracy_input').focus();
                    } else {
                        me.prev('.accuracy_input').focus();
                    }
                }).on('focus', function () {
                    //聚焦时强制光标位于值后面
                    var me = $(this);
                    if (me.val() && me[0].setSelectionRange) {
                        me[0].setSelectionRange(1, 1);
                    }
                });
                document.onkeydown = function (e) {
                    if (e.code === 'Backspace') {
                        $('.accuracy_input').each(function () {
                            var me = $(this);
                            if (me.is(':focus') && !me.val()) {
                                me.prev('.accuracy_input').focus();
                            }
                        });
                    }
                }
            })();
            this.createScrollTips();
            globalBackTop.init();
        },
        methods: {
            //使code切换焦点
            setPhone: function (flag) {
                if (flag) {
                    $('#searchInput').focus();
                    this.isSearchInputFocused = true;
                } else {
                    $('#searchInput').blur();
                }
            },

            //获取数据
            getData: function (isForce, fn) {
                if (!this.hasMore && !isForce) {
                    return;
                }
                fn = fn || {};
                var that = this,
                    currentPage = that.page + 1,
                    isNeedFetchProfit = that.isNeedFetchProfit;
                if (isForce || that.isNormal) {
                    currentPage = 1;
                }
                var data = {
                    token: token,
                    page: currentPage,
                    pageCount: that.pageCount,
                    search: that.search,
                    search_is_end: that.isTail ? 1 : 0,
                    province: that.provinceName,
                    city: that.cityName,
                    orderby: that.orderby,
                    ordername: that.ordername,
                    price: that.price,
                    regex: that.regex,
                    isp: that.isp,
                    isNormal: that.isNormal,
                    repeatNumber: that.repeatNumber,
                    exceptNumber: that.exceptNumber,
                    moreNumber: that.moreNumber,
                    birthdayNumber: that.birthdayNumber
                };
                if (that.isAccuracy) {
                    var precise_num = '1';
                    $('.accuracy_input').each(function () {
                        var val = $(this).val();
                        precise_num += val || '_';
                    });
                    data.precise_num = precise_num;
                }

                that.isFetchingMore = true;
                if (isNeedFetchProfit) {
                    that.isFetchingProfit = true;
                }

                globalLoading.show();
                $.ajax({
                    url: '/webapp/agentShop/getPhoneNum',
                    method: 'post',
                    data: data
                })
                    .done(function (res) {
                        if (res) {
                            var discount = res.discount,
                                list = res.data || [],
                                mobileList = that.mobileList.map(function (item) {
                                    return item.Telphone;
                                });
                            list.forEach(function (item) {
                                item.area = item.City;
                                item.Price = Math.ceil(item.Price * discount / 100);
                                item.marketPrice = item.Price * 2;
                            });
                            var newList = isForce ? list : list.filter(function (mobile) {
                                return $.inArray(mobile.mobile, mobileList) === -1;
                            });
                            var fetchDataCbData = {
                                res: res,
                                isForce: isForce,
                                currentPage: currentPage,
                                fn: fn
                            };
                            if (isNeedFetchProfit && newList.length) {
                                that.getAgentProfit(newList.map(function (item) {
                                    return item.Telphone
                                }))
                                    .then(function (data) {
                                        var profitObj = data || {};
                                        newList.forEach(function (item) {
                                            var profitVal = profitObj[item.Telphone] || {};
                                            item.direct = profitVal.direct;
                                            item.indirect = profitVal.indirect;
                                        });
                                    })
                                    .always(function () {
                                        that.fetchDataCb(newList, fetchDataCbData);
                                    });
                            } else {
                                that.fetchDataCb(newList, fetchDataCbData);
                            }
                        } else {
                            $.toast(res.msg);
                            fn.fail && fn.fail();
                            that.fetchDataComplete();
                        }
                    })
                    .fail(function (e) {
                        fn.fail && fn.fail();
                        that.fetchDataComplete();
                    });
            },
            //获取手机号成功回调
            fetchDataCb: function (newList, fetchDataCbData) {
                var res = fetchDataCbData.res,
                    isForce = fetchDataCbData.isForce,
                    currentPage = fetchDataCbData.currentPage,
                    fn = fetchDataCbData.fn;
                var list = isForce ? newList : this.mobileList.concat(newList);
                this.mobileList = list;
                this.hasMore = (res.total > list.length || res.force_nextpage === 1) && res.data.length;
                this.page = currentPage;
                this.isNormal = 0;
                if (res.force_nextpage === 1) {
                    this.isNormal = 1;
                }
                if (isForce) {
                    $('html,body').animate({ scrollTop: 0 });
                }
                fn.success && fn.success();
                this.fetchDataComplete();
            },
            //获取数据完成后回调
            fetchDataComplete: function () {
                this.isFetchingMore = false;
                this.isFetched = true;
                this.isFetchingProfit = false;
                this.hideSlide();
                globalLoading.hide();
            },
            //选择过滤条件
            setFilterItem: function (itemKey, val) {
                this[itemKey] = val;
            },
            //隐藏筛选栏
            hideSlide: function () {
                this.isShowFilterSlide = false;
                this.isShowFilterPrice = false;
                this.isShowFilterRegex = false;
            },
            //检查是否启动页面滚动
            checkBodyLock: function (flag) {
                if (flag) {
                    $('body').addClass('no_scroll');
                } else {
                    $('body').removeClass('no_scroll');
                }
            },
            //切换过滤条件
            chooseFilter: function (key) {
                this.hideSlide();
                if (key === 'chooseArea') {
                    if (this.addressList.length) {
                        this.chooseArea();
                    }
                } else {
                    this[key] = true;
                }
            },
            //判断条件是否选择
            checkFilterItemIsSelected: function (itemKey, val) {
                return this[itemKey] === val;
            },
            //重置过滤条件
            reset: function () {
                var that = this;
                this.currentSlidePack.forEach(function (item) {
                    that[item.key.substring(4)] = defaultData[item.key.substring(4)];
                });

                this.getData(true);
            },

            //代理商城获取利润预览
            getAgentProfit: function (mobileList) {
                mobileList = mobileList || [];
                var deferred = new $.Deferred(),
                    resolve = deferred.resolve,
                    reject = deferred.reject;
                $.ajax({
                    method: 'post',
                    url: '/webapp/agentShop/getProfit',
                    data: {
                        mobile: mobileList.join(','),
                        openid: openid
                    }
                })
                    .done(function (res) {
                        var code = res.code,
                            msg = res.msg || '未知错误',
                            data = res.data;
                        if (code === 200) {
                            resolve(data);
                        }
                        reject({
                            code: code,
                            msg: msg
                        });
                    })
                    .fail(function (e) {
                        console.error(e);
                        reject({
                            code: -9999,
                            msg: '客户端捕获未知错误'
                        });
                    });
                return deferred.promise();
            },

            //创建地址
            createAddressList: function () {
                this.addressList = chinaRegion;
            },
            //根据省获取城市
            getCity: function (provinceId, cb) {
                var data = [{ id: '', value: '全部' }];
                if (!provinceId) {
                    cb(data);
                    return;
                }
                var list = this.addressList.filter(function (province) {
                    return province.id === provinceId;
                })[0].childs;
                list.forEach(function (item) {
                    data.push({ id: item.id, value: item.name });
                });
                indexApp.cityList = data;
                cb(data);
            },
            //选择归属地
            chooseArea: function () {
                var that = this;

                new IosSelect(2,               // 第一个参数为级联层级，演示为1
                    [that.provinceList, that.getCity],
                    {
                        container: '.city_picker',
                        showAnimate: true,
                        title: '选择号码归属地',                    // 标题
                        itemHeight: 48,                      // 每个元素的高度
                        itemShowCount: 7,                    // 每一列显示元素个数，超出将隐藏
                        oneLevelId: that.province,
                        twoLevelId: that.city,
                        callback: function (provinceObj, cityObj) {  // 用户确认选择后的回调函数
                            that.province = provinceObj.value;
                            that.city = cityObj.value;
                            that.getData(true);
                        }
                    });
            },
            //滚动处理
            handleScroll: function () {
                var that = this;
                $(window).scroll(function () {
                    that.isShowFixNav = window.scrollY > 500;
                    if (!that.hasMore || that.isFetchingMore || that.isFetchingProfit) {
                        return;
                    }
                    scrollFunc();
                    if (scrollDirection === 'down' && $(window).scrollTop() + $(window).height() > $(document).height() - 50) {
                        that.getData();
                    }
                });
            },
            //获取提示条目
            getScrollTip: function () {
                var prefix = ['165', '170', '167', '171', '162'],
                    end = ['1234', '2233', '2266', '8888', '6666', '2324', '6268', '7878', '9999', '9000', '8000', '8899', '8866', '7799', '5678', '6789', '1234', '9900'],
                    name = ['张', '王', '李', '赵', '袁', '朱', '宋', '金'];

                var mobile = prefix[iUtils.rand(0, prefix.length - 1)] + '****' + end[iUtils.rand(0, end.length - 1)],
                    user = name[iUtils.rand(0, name.length - 1)],
                    step = iUtils.rand(1, 10),
                    lastNum = iCookie.get('lastNum') || 200,
                    newNum = parseInt(lastNum) + step;

                iCookie.set('lastNum', newNum);
                return '已有' + newNum + '人订购&nbsp;&nbsp;|&nbsp;&nbsp;' + user + '**获得' + mobile;
            },
            //滚动提示
            createScrollTips: function () {
                for (var i = 0; i < 3; i++) {
                    this.scrollTipsList.push(this.getScrollTip());
                }
                scrollTimer = setTimeout(this.scrollTips, 1500);
            },
            //滚动
            scrollTips: function () {
                var that = this;
                clearTimeout(scrollTimer);
                that.scrollTipsStyle = {
                    transition: 'transform 500ms ease-in-out 0s',
                    transform: 'translate3d(0px, -.32rem, 0px)'
                };
                scrollTimer1 = setTimeout(function () {
                    clearTimeout(scrollTimer1);
                    that.scrollTipsList.shift();
                    that.scrollTipsList.push(that.getScrollTip());
                    that.scrollTipsStyle = {
                        transition: 'none 0s ease-in-out 0s',
                        transform: 'translate3d(0px, 0, 0px)'
                    };
                }, 500);
                scrollTimer = setTimeout(function () {
                    that.scrollTips();
                }, 2500);
            },
            //购买'//'+curUrl+
            buy: function (mobile) {
                window.location = '/webapp/AgentShop/product/' + mobile;
            },
            //确定选择
            submit: function () {
                //确定筛选条件后交换临时值
                var that = this,
                    key,
                    val,
                    backValList = [];
                this.currentSlidePack.forEach(function (item) {
                    key = item.key.substring(4);
                    val = that[key];
                    that[key] = that[item.key];
                    backValList.push({ key: key, val: val });
                });
                that.getData(true, {
                    fail: function () {
                        backValList.forEach(function (item) {
                            that[item.key] = item.val;
                        });
                    }
                });
            }
        },
        computed: {
            //是否显示蒙层
            isShowCover: function () {
                return this.isShowFilterSlide || this.isShowFilterPrice || this.isShowFilterRegex;
            },
            //规律选号数组
            regexList: function () {
                return [
                    { "val": "0", "title": "超级精品号" },
                    { "val": "1", "title": "6A" },
                    { "val": "2", "title": "5A" },
                    { "val": "3", "title": "4A" },
                    { "val": "11", "title": "3A(0-5)" },
                    { "val": "12", "title": "3A(6-9)" },
                    { "val": "4", "title": "双豹子(0-5)" },
                    { "val": "20", "title": "双豹子(6-9)" },
                    { "val": "9", "title": "ABCDEF" },
                    { "val": "5", "title": "ABCDE" },
                    { "val": "6", "title": "ABCD" },
                    { "val": "7", "title": "ABC" },
                    { "val": "13", "title": "大循环" },
                    { "val": "1202", "title": "三拖一" },
                    { "val": "14", "title": "四拖一" },
                    { "val": "15", "title": "五拖一" },
                    { "val": "18", "title": "六拖一" },
                    { "val": "16", "title": "ABACAD" },
                    { "val": "17", "title": "ABABAB(AB)" },
                    { "val": "1211", "title": "各地区号" },
                    { "val": "1201", "title": "倒顺子" },
                    { "val": "1204", "title": "AABB" },
                    { "val": "1205", "title": "ABAB" },
                    { "val": "1206", "title": "个性号码" },
                    { "val": "1207", "title": "表白号码" },
                    { "val": "1208", "title": "年份(生日）号" },
                    { "val": "1209", "title": "精品AA" },
                    { "val": "1210", "title": "拖二" },

                ];
            },
            //号码类型组
            filterRegexPack: function () {
                return [
                    { title: '规律选号', list: this.regexList, key: 'tmp_regex' }
                ]
            },
            //isp数组
            ispList: function () {
                return [
                    { val: '', title: '不限' },
                    { val: '移动', title: '移动网' },
                    { val: '联通', title: '联通网' },
                    { val: '电信', title: '电信网' }
                ];
            },
            //连号数组
            repeatNumberList: function () {
                var arr = [{ val: '', title: '不限' }];
                for (var i = 9; i >= 0; i--) {
                    arr.push({ val: iUtils.repeatStr(i, 3), title: iUtils.repeatStr(i, 3) });
                }

                return arr;
            },
            //排除数字数组
            exceptNumberList: function () {
                return [4].map(function (item) {
                    return { val: item, title: '排除' + item };
                });
            },
            //数字较多数组
            moreNumberList: function () {
                var arr = [{ val: '', title: '不限' }];
                for (var i = 0; i < 10; i++) {
                    arr.push({ val: i.toString(), title: i + '较多' });
                }

                return arr;
            },
            //生日数组
            birthdayNumberList: function () {
                var labelList = '一 二 三 四 五 六 七 八 九 十 十一 十二'.split(/\s/),
                    arr = [{ val: '', title: '不限' }];
                for (var i = 1; i <= 12; i++) {
                    var _tmp = i.toString();
                    arr.push({
                        val: iUtils.subTime(_tmp),
                        title: labelList[i - 1] + '月'
                    });
                }
                return arr;
            },
            //号码筛选组
            filterSlidePack: function () {
                var list = [
                    { title: '连号', list: this.repeatNumberList, key: 'tmp_repeatNumber' },
                    { title: '排除数字', list: this.exceptNumberList, key: 'tmp_exceptNumber' },
                    { title: '数字较多', list: this.moreNumberList, key: 'tmp_moreNumber' },
                    { title: '生日', list: this.birthdayNumberList, key: 'tmp_birthdayNumber' }
                ];
                if (this.show_isp) {
                    list.unshift({ title: '网络制式', list: this.ispList, key: 'tmp_isp' });
                }
                return list;
            },
            //当前显示数组 号码筛选组/号码类型组
            currentSlidePack: function () {
                var list = [];
                if (this.isShowFilterSlide) {
                    list = this.filterSlidePack;
                }
                if (this.isShowFilterPrice) {
                    list = this.filterSlidePricePack;
                }
                if (this.isShowFilterRegex) {
                    list = this.filterRegexPack;
                }
                return list;
            },
            //价格排序数组
            sortList: function () {
                return [
                    { val: '', title: '默认' },
                    { val: 'desc', title: '价格由高到低' },
                    { val: 'asc', title: '价格由低到高' }
                ];
            },
            //价格范围数组
            priceList: function () {
                var arr = [{ val: '0', title: '全部' }],
                    list = [0, 300, 600, 3000, 5000, 10000, 30000, 50000],
                    i, j;
                list.reduce(function (prev, current, idx, list) {
                    i = idx === 0 ? '' : current + 1;
                    j = idx === list.length - 1 ? '' : list[idx + 1];
                    arr.push({ val: i + '-' + j, title: j ? (i || 0) + ' ~ ' + j : current + '以上' });
                }, 0);
                return arr;
            },
            //价格筛选组
            filterSlidePricePack: function () {
                return [
                    { title: '排序', list: this.sortList, key: 'tmp_ordername' },
                    { title: '号码价格', list: this.priceList, key: 'tmp_price' }
                ]
            },
            //手机号列表过滤显示手机号
            displayMobileList: function () {
                var regex = this.regex,
                    regexList = this.regexList,
                    repeatNumber = this.repeatNumber,
                    text_highlight_color = this.text_highlight_color,
                    mobile,
                    html,
                    _len;
                return this.mobileList.map(function (item) {
                    mobile = item.Telphone.toString();
                    html = '<strong style="color:' + text_highlight_color + '">' + mobile.substring(0, 3) + '</strong>' +
                        '<span>' + mobile.substring(3, 7) + '</span>' +
                        '<strong style="color:' + text_highlight_color + '">' + mobile.substring(7) + '</strong>';
                    if (regex > 0) {
                        _len = regexList.filter(function (i) {
                            return i.val === regex;
                        })[0].title.length;
                        html = '<span>' + mobile.substring(0, 11 - _len) + '</span>' +
                            '<strong style="color:' + text_highlight_color + '">' + mobile.substring(11 - _len) + '</strong>';
                    } else if (repeatNumber.length) {
                        var _i = mobile.indexOf(repeatNumber);
                        _len = repeatNumber.length;
                        html = '<span>' + mobile.substring(0, _i) + '</span>' +
                            '<strong style="color:' + text_highlight_color + '">' + mobile.substring(_i, _i + _len) + '</strong>' +
                            '<span>' + mobile.substring(_i + _len) + '</span>';
                    }
                    item.displayMobile = html + '<i class="flag">靓</i>';
                    return item;
                });
            },
            provinceName: function () {
                return this.province === '全部' ? '' : this.province;
            },
            cityName: function () {
                return this.city === '全部' ? '' : this.city;
            },
            //号码归属地按钮显示文字
            displayAreaName: function () {
                return this.cityName || this.provinceName || '号码归属地';
            },
            //省列表
            provinceList: function () {
                var arr = [{ id: '', value: '全部' }];
                this.addressList.forEach(function (province) {
                    arr.push({ id: province.id, value: province.name });
                });
                return arr;
            }
        },
        watch: {
            //侧边栏出现时禁止滚动，重置筛选项
            isShowCover: function (val) {
                var that = this;
                if (val) {
                    this.currentSlidePack.forEach(function (filterItem) {
                        that[filterItem.key] = that[filterItem.key.substring(4)];
                    });
                }
                this.checkBodyLock(val);
            }
        }
    });
});

//通过ip获取城市
function getCityByIp() {
    $.getScript('https://apis.map.qq.com/ws/location/v1/ip?key=AICBZ-VCXKU-PGXV6-2EQJU-GXN3F-LSF64&output=jsonp&callback=setDefaultCity');
}

//设置默认城市
function setDefaultCity(res) {
    res = res || {};
    var info = res.result && res.result.ad_info;
    if (info) {
        var province = info.province,
            city = info.city,
            _p = '',
            _c = '';
        if (province && city) {
            chinaRegion.forEach(function (p) {
                if (p.name.indexOf(province.substring(0, province.length - 1)) !== -1) {
                    p.childs.forEach(function (c) {
                        if (c.name.indexOf(city.substring(0, city.length - 1)) !== -1) {
                            _p = p.name;
                            _c = c.name;
                        }
                    });
                }
            });
        }
        indexApp.province = _p;
        indexApp.city = _c;
    }
    indexApp.getData(true);
}

(function () {
    var scrollAction = { x: 'undefined', y: 'undefined' }, scrollDirection;

    function scrollFunc() {
        if (typeof scrollAction.x == 'undefined') {
            scrollAction.x = window.pageXOffset;
            scrollAction.y = window.pageYOffset;
        }
        var diffX = scrollAction.x - window.pageXOffset;
        var diffY = scrollAction.y - window.pageYOffset;
        if (diffX < 0) {
            // Scroll right
            scrollDirection = 'right';
        } else if (diffX > 0) {
            // Scroll left
            scrollDirection = 'left';
        } else if (diffY < 0) {
            // Scroll down
            scrollDirection = 'down';
        } else if (diffY > 0) {
            // Scroll up
            scrollDirection = 'up';
        } else {
            // First scroll event
        }
        scrollAction.x = window.pageXOffset;
        scrollAction.y = window.pageYOffset;
        window.scrollDirection = scrollDirection;
    }

    window.scrollFunc = scrollFunc;
})();
