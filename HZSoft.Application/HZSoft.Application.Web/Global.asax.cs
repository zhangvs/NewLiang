using HZSoft.Cache.Redis;
using HZSoft.Util;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace HZSoft.Application.Web
{
    /// <summary>
    /// 应用程序全局设置
    /// </summary>
    public class MvcApplication : HttpApplication
    {
        /// <summary>
        /// 启动应用程序
        /// </summary>
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleTable.EnableOptimizations = true;
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            WeixinConfig.Register();

            //发送短信
            //SmsSingleSenderResult singleResult;
            //SmsSingleSender singleSender = new SmsSingleSender(1400040861, "a92c87d0d291698777a9b5f323c0388a");
            //List<string> templParams = new List<string>();
            //templParams.Add("132456");
            //singleResult = singleSender.SendWithParam("86", "18660996839", 43313, templParams, "利新商贸", "", "");//sign为签名，不用签名id，

            //头条API方式
            //string result = HttpUtility.UrlEncode("https://www.jnlxsm.net:8064/webapp/jinan2/index?adid=1672885092005949&creativeid=1672885405853736&creativetype=15&clickid=EKjQ9uKvr_wCGKfvvqrzASDzjJOhgQIwDDjBuAJCIjIwMjAwNzI1MjMwNjQ2MDEwMTMxMDU3MDkyMTA1MjQ5NjhIwbgC&city=440100&keyword=8888&page=1&features=&orderType=&price=&except=&yuyi=");
            //string dd= HttpClientHelper.Get(@"https://ad.oceanengine.com/track/activate/?link=" + result + "&event_type=2");
        }

        /// <summary>
        /// 应用程序错误处理
        /// </summary>
        protected void Application_Error(object sender, EventArgs e)
        {
            var lastError = Server.GetLastError();
        }
    }
}