var app = new Vue({
    el: '#app',
    mixins: [fetchListMixin],
    data: {
        statusList: [{id: 0, title: '审核中'}, {id: 1, title: '已转账'}, {id: 2, title: '已拒绝'}],

        isShowScreenshot: false,                //是否显示转账截图弹窗
        isShowReason: false,                     //是否显示失败原因弹窗

        showImg: '',                     //已入账或已拒绝截图

        fetchMethod: getWithdrawLogs
    },
    methods: {
        showPop: function (img) {
            var that = this;
            var image = new Image();
            image.onload = function () {
                that.showImg =  img;
            };
            image.onerror = function () {
                $.toast('图片加载失败，请重试');
            };
            image.src = img;
        },
        //关闭弹窗
        closePop: function () {
            this.showImg = '';
        },
        //切换body高度
        toggleBodyHeight: function (val) {
            if (val) {
                $('body').addClass('no_scroll');
            } else {
                $('body').removeClass('no_scroll');
            }
        }
    },
    watch: {
        isShowScreenshot: function (val) {
            this.toggleBodyHeight(val);
        },
        isShowReason: function (val) {
            this.toggleBodyHeight(val);
        }
    },
    computed: {
        withdrawalList: function () {
            return this.list;
        }
    }
});
