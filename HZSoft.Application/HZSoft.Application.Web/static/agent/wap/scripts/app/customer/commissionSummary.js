var app = new Vue({
    el: '#app',
    mixins: [fetchListMixin],
    data: {},
    created: function () {
        this.getData();
    },
    methods: {
        getData: function () {
            var that = this,
                data = {};
            var dateArr = this.date.split(/\s~\s/);
            if (dateArr.length > 1) {
                data.start = dateArr[0];
                data.end = dateArr[1];
            }
            globalLoading.show();
            $.when(getCommissionSummary(data))
                .then(function (res) {
                    that.list = res || [];
                })
                .fail(function (e) {
                    $.toast(e.msg || '未知错误');
                })
                .always(function () {
                    that.isFetched = true;
                    globalLoading.hide();
                });
        },
        handleScroll: function () {
            //不处理
        }
    },
    computed: {}
});
