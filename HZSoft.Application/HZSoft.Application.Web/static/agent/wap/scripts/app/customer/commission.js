var app = new Vue({
    el: '#app',
    mixins: [fetchListMixin],
    data: {
        statusList: [{id: 0, title: '未支付'}, {id: 1, title: '已支付'}, {id: 2, title: '已入账'},{id:3,title:'已退款'}],

        agentList: [],           //代理列表
        child_id: '',               //代理id

        commissionPage: true,

        stats: {},           //统计信息

        fetchMethod: getComissionLog
    },
    created: function () {
        this.getAgents();
    },
    methods: {
        //获取代理列表
        getAgents: function () {
            var that = this;
            $.when(getAgents())
                .then(function (data) {
                    that.agentList = data || [];
                })
                .fail(function (e) {
                    console.log(e);
                });
        }
    },
    computed: {
        commissionList: function () {
            return this.list;
        },
        //代理显示
        agentName: function () {
            var child_id = this.child_id,
                name = '下级代理';
            if (child_id) {
                var agent = this.agentList.filter(function (agent) {
                    return agent.Id === child_id;
                })[0];
                name = agent.realname || agent.nickname;
            }
            return name;
        }
    }
});
