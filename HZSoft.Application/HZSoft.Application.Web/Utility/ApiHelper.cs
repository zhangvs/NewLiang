using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Text;
using Newtonsoft.Json.Linq;
using HZSoft.Util;

namespace HZSoft.Application.Web.Utility
{
    public class ApiHelper
    {
        /// <summary>
        /// 百度api
        /// </summary>
        /// <returns></returns>
        public static string GetBaiduIp(string ip)
        {
            try
            {
                string url = "http://api.map.baidu.com/location/ip?ak=xaVAHHxPUd8tkrzvHofMTKW9qkwtiK1Z&ip=" + ip;
                WebClient client = new WebClient();
                var buffer = client.DownloadData(url);
                string jsonText = Encoding.UTF8.GetString(buffer);
                JObject jo = JObject.Parse(jsonText);
                var province = jo["content"]["address_detail"]["province"].ToString();
                var city = jo["content"]["address_detail"]["city"].ToString();
                if (string.IsNullOrEmpty(province) || string.IsNullOrEmpty(city))
                {
                    return "";

                }
                return city;
            }
            catch
            {
                return "";
            }
        }
    }
}