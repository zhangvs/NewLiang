﻿using System.Web.Mvc;
using HZSoft.Application.Code;
using HZSoft.Util;
using System.Web;
using HZSoft.Application.Web.Utility;

namespace HZSoft.Application.Web
{
    /// <summary>
    /// 版 本 6.1
    /// 
    /// 创建人：佘赐雄
    /// 日 期：2015.11.9 10:45
    /// 描 述：登录认证（会话验证组件）
    /// </summary>
    public class HandlerAgentAttribute : AuthorizeAttribute
    {
        private LoginMode _customMode;
        /// <summary>默认构造</summary>
        /// <param name="Mode">认证模式</param>
        public HandlerAgentAttribute(LoginMode Mode)
        {
            _customMode = Mode;
        }
        /// <summary>
        /// 响应前执行登录验证,查看当前用户是否有效 
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            //登录拦截是否忽略
            if (_customMode == LoginMode.Ignore)
            {
                return;
            }

            HttpRequest request = HttpContext.Current.Request;
            string RequestUri =!string.IsNullOrEmpty(request.Params["id"])? request.Params["id"]:request.FilePath;
            //登录是否过期
            if (OperatorAgentProvider.Provider.IsOverdue())
            {
                WebHelper.WriteCookie("hzsoft_login_error", "Overdue");//登录已超时,请重新登录
                filterContext.Result = new RedirectResult("~/webapp/AgentLogin/Index?id=" + RequestUri);
                return;
            }
            //是否已登录
            var OnLine = OperatorAgentProvider.Provider.IsOnLine();
            if (OnLine == 0)
            {
                WebHelper.WriteCookie("hzsoft_login_error", "OnLine");//您的帐号已在其它地方登录,请重新登录
                filterContext.Result = new RedirectResult("~/webapp/AgentLogin/Index?id=" + RequestUri);
                return;
            }
            else if (OnLine == -1)
            {
                WebHelper.WriteCookie("hzsoft_login_error", "-1");//缓存已超时,请重新登录
                filterContext.Result = new RedirectResult("~/webapp/AgentLogin/Index?id=" + RequestUri);
                return;
            }
            //去获取坐标
            //if (request.Cookies["currlong"] == null || request.Cookies["currlat"] == null)
            //{
            //    filterContext.Result = new RedirectResult("~/WeChatManage/WeiXinHome/Index?id=" + RequestUri);
            //    return;
            //}
        }
    }
}