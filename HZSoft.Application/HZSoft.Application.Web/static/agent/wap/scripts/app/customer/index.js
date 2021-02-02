var app = new Vue({
    el: '#app',
    data: {
        customerInfo: {},
        agentList: [],
        linkList: []
    },
    created: function () {

    },
    mounted: function () {
        this.createAgentList();
        this.createLinkList();
    },
    methods: {
        //团队情况
        createAgentList: function () {
            this.agentList = [
                {
                    level: 1,
                    people: iUtils.rand(1, 10000),
                    money: iUtils.rand(10, 1000000)
                },
                {
                    level: 2,
                    people: iUtils.rand(1, 10000),
                    money: iUtils.rand(10, 1000000)
                },
                {
                    level: 3,
                    people: iUtils.rand(1, 10000),
                    money: iUtils.rand(10, 1000000)
                }
            ];
        },
        //链接列表
        createLinkList: function () {
            this.linkList = [
                {
                    key: 'invitation',
                    url: '/webapp/agent/invitation',
                    title: '邀请代理'
                },
                {
                    key: 'commission',
                    url: '/webapp/agent/commission',
                    title: '佣金明细'
                },

                {
                    key: 'withdrawal',
                    url: '/webapp/agent/withdrawLogs',
                    title: '提现记录'
                },
                {
                    key: 'agent',
                    url: '/webapp/agent/profile',
                    title: '个人资料'
                },
                {
                    key: 'agent',
                    url: 'https://w.dslianghao.com/?aid='+aid,
                    title: '我的商城'
                }
            ];
        }
    },
    computed: {},
    watch: {}
});
