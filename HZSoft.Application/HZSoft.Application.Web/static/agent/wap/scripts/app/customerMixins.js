var fetchListMixin = {
    data: {
        list: [],
        date: '',
        status: '',
        pagesize: 40,
        page: 0,

        isFetchingMore: false,
        isFetched: false,
        hasMore: true
    },
    created: function () {
        this.getData();
    },
    mounted: function () {
        this.renderDate();
        this.handleScroll();
    },
    methods: {
        //获取数据
        getData: function (isForce, fn) {
            if (!this.hasMore && !isForce) {
                return;
            }
            fn = fn || {};
            var that = this,
                currentPage = that.page + 1;
            if (isForce) {
                currentPage = 1;
            }
            var data = {
                page: currentPage,
                pagesize: that.pagesize
            };
            var dateArr = this.date.split(/\s~\s/);
            if (dateArr.length > 1) {
                data.start = dateArr[0];
                data.end = dateArr[1];
            }
            if (that.status !== '') {
                data.status = that.status;
            }
            if (that.child_id !== '') {
                data.child_id = that.child_id;
            }

            that.isFetchingMore = true;
            globalLoading.show();
            $.when(that.fetchMethod(data))
                .then(function (res) {
                    if (that.commissionPage) {
                        that.stats = res.stats || [];
                        res = res.logs || {};
                    }
                    var list = res.data || [];
                    list.forEach(function (item) {
                        item.statusTitle = that.getStatusTitle(item.status);
                        item.uuid = iUtils.uuid();
                    });
                    list = isForce ? list : that.list.concat(list);
                    that.list = list;
                    that.hasMore = res.total > list.length && res.data.length;
                    that.page = currentPage;
                    if (isForce) {
                        $('html,body').animate({scrollTop: 0});
                    }
                })
                .fail(function (e) {
                    if (fn.fail) {
                        fn.fail();
                    } else {
                        $.toast(e.msg || '未知错误');
                    }
                })
                .always(function () {
                    that.isFetchingMore = false;
                    that.isFetched = true;
                    globalLoading.hide();
                });
        },
        //创建日期插件
        renderDate: function () {
            var that = this;
            laydate.render({
                elem: '#selectDate',
                isInitValue: false,
                range: '~',
                max: 1,
                ready: function () {
                    var dom = $('#layui-laydate1'),
                        _w = $(window).width(),
                        _scale = _w / 546;
                    dom.css({
                        transform: 'scale(' + _scale + ')',
                        top: 15 * _scale / 2 + 'vw',
                        left: (_w - 546) / 2
                    });
                },
                done: function (value) {
                    that.date = value;
                }
            });
        },
        //滚动处理
        handleScroll: function () {
            var that = this;
            $(window).scroll(function () {
                that.isShowFixNav = window.scrollY > 500;
                if (!that.hasMore || that.isFetchingMore) {
                    return;
                }
                scrollFunc();
                if (scrollDirection === 'down' && $(window).scrollTop() + $(window).height() > $(document).height() - 50) {
                    that.getData();
                }
            });
        },
        //获取状态名称
        getStatusTitle: function (statusId) {
            var statusTitle = '';
            this.statusList.forEach(function (status) {
                if (parseInt(status.id) === statusId) {
                    statusTitle = status.title;
                }
            });
            return statusTitle;
        }
    },
    watch: {
        date: function () {
            this.getData(true);
        },
        status: function () {
            this.getData(true);
        },
        child_id: function () {
            this.getData(true);
        }
    }
};
