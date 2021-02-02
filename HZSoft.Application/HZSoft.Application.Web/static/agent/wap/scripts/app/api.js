//获取会员基础信息
function getAgentInfo(data) {
    return iApi({
        url: 'agentInfo',
        notNeedLogin: true,
        data: data || {}
    });
}

//获取团队概要
function getTeamSummary(data) {
    return iApi({
        url: 'teamSummary',
        notNeedLogin: true,
        data: data || {}
    });
}

//获取代理等级信息
function getLevel(data) {
    return iApi({
        url: 'getLevel',
        notNeedLogin: true,
        data: data || {}
    });
}

//获取佣金摘要
function getBalance(data) {
    return iApi({
        url: 'getBalance',
        notNeedLogin: true,
        data: data || {}
    });
}

//获取佣金记录
function getComissionLog(data) {
    return iApi({
        url: 'comissionLog',
        notNeedLogin: true,
        data: data || {}
    });
}

//获取提现记录
function getWithdrawLogs(data) {
    return iApi({
        url: 'withdrawLogs',
        notNeedLogin: true,
        data: data || {}
    });
}

//获取个人资料
function getUserInfo(data) {
    return iApi({
        url: 'userInfo',
        notNeedLogin: true,
        data: data || {}
    });
}

//保存个人资料
function saveUserInfo(data) {
    return iApi({
        url: 'saveUserInfo',
        notNeedLogin: true,
        data: data || {}
    });
}

//获取代理佣金明细
function getCommissionSummary(data) {
    return iApi({
        url: 'comissionSummary',
        notNeedLogin: true,
        data: data || {}
    });
}

//获取下级代理
function getAgents(data) {
    return iApi({
        url: 'getAgents',
        notNeedLogin: true,
        data: data || {}
    });
}
