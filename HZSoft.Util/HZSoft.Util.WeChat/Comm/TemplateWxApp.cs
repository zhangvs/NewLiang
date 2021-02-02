using HZSoft.Util;
using HZSoft.Util.WeChat.Model;
using Newtonsoft.Json;
using System;

namespace HZSoft.Util.WeChat.Comm
{
    public class TemplateWxApp
    {
        public static string appId = Config.GetValue("AppID");
        public static string appSecret = Config.GetValue("AppSecret");

        public static string getToken()
        {
            string token = MPAccessToken.GetToken();
            return token;
        }
    }
}