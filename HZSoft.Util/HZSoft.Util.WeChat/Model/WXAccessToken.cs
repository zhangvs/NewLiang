using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HZSoft.Util;
using HZSoft.Util.WeChat.Helper;

namespace HZSoft.Util.WeChat.Model
{
    public class MPAccessToken
    {
        public static MPAccessToken _Token;

        public string access_token { get; set; }

        public int expires_in { get; set; }

        private DateTime createTokenTime = DateTime.Now;

        /// <summary>
        /// 到期时间(防止时间差，提前1分钟到期)
        /// </summary>
        /// <returns></returns>
        public DateTime TookenOverdueTime
        {
            get { return createTokenTime.AddSeconds(expires_in - 60); }
        }

        /// <summary>
        /// 刷新Token
        /// </summary>
        public static void Renovate()
        {
            if (_Token == null)
            {
                GetNewToken();
            }

            MPAccessToken._Token.createTokenTime = DateTime.Now;
        }

        public static bool IsTimeOut()
        {
            if (_Token == null)
            {
                GetNewToken();
            }

            return DateTime.Now >= MPAccessToken._Token.TookenOverdueTime;
        }

        public static MPAccessToken GetNewToken()
        {
            string strulr = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}";

            string appId = Config.GetValue("AppID"); //appid
            string appSecret = Config.GetValue("AppSecret");//app密钥

            HttpHelper http = new HttpHelper();

            string respone = http.Get(string.Format(strulr, appId, appSecret), Encoding.UTF8);
            LogHelper.AddLog(respone);//获取新的access_token
            var token = respone.ToObject<MPAccessToken>();

            MPAccessToken._Token = token;

            return token;
        }

        public static string GetToken()
        {
            if (IsTimeOut())
            {
                GetNewToken();
            }
            return _Token.access_token;
        }
    }
}
