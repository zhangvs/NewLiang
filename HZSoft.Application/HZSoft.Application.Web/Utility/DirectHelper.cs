using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Text;
using Newtonsoft.Json.Linq;
using HZSoft.Util;
using HZSoft.Application.Code;
using HZSoft.Application.Busines.BaseManage;

namespace HZSoft.Application.Web.Utility
{
    public class DirectHelper
    {


        public class Comission
        {
            /// <summary>
            /// 
            /// </summary>
            public decimal? direct { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public decimal? indirect { get; set; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uplv">目标级别</param>
        /// <param name="pidlv">上级级别</param>
        /// <param name="pid2lv">上上级级别</param>
        /// <param name="direct">直接返佣</param>
        /// <param name="indirect">间接返佣</param>
        public static void getJMDirect(string uplv,string pidlv,string pid2lv, out decimal direct, out decimal indirect)
        {
            direct = 0;
            indirect = 0;
            //升级一个黄金代理
            if (uplv == "黄金代理")
            {
                //直接返佣给上级
                if (!string.IsNullOrEmpty(pidlv))
                {
                    switch (pidlv)
                    {
                        case "普通代理":
                            direct = 100;
                            break;
                        case "黄金代理":
                            direct = 160;
                            break;
                        case "钻石代理":
                            direct = 219.45M;
                            break;
                        default:
                            break;
                    }
                }
                //间接返佣给上上级
                if (!string.IsNullOrEmpty(pid2lv))
                {
                    switch (pid2lv)
                    {
                        case "普通代理":
                            indirect = 59;
                            break;
                        case "黄金代理":
                            indirect = 79.8M;
                            break;
                        case "钻石代理":
                            indirect = 79.8M;
                            break;
                        default:
                            break;
                    }
                }
            }
            else if (uplv == "钻石代理")
            {
                //直接返佣给上级
                if (!string.IsNullOrEmpty(pidlv))
                {
                    switch (pidlv)
                    {
                        case "普通代理":
                            direct = 500;
                            break;
                        case "黄金代理":
                            direct = 799.6M;
                            break;
                        case "钻石代理":
                            direct = 1099.45M;
                            break;
                        default:
                            break;
                    }
                }
                //间接返佣给上上级
                if (!string.IsNullOrEmpty(pid2lv))
                {
                    switch (pid2lv)
                    {
                        case "普通代理":
                            indirect = 299;
                            break;
                        case "黄金代理":
                            indirect = 399.8M;
                            break;
                        case "钻石代理":
                            indirect = 799.6M;
                            break;
                        default:
                            break;
                    }
                }
            }

        }

        private static OrganizeBLL organizeBLL = new OrganizeBLL();

        public static void getDirectDH(string organizeId, int? lv, out decimal? direct, out decimal? indirect)
        {
            //代售 产生的订单佣金(售价30%)返给对应1级(由1级自由分配)
            if (organizeId != OperatorAgentProvider.Provider.Current().OrganizeId)
            {
                direct = null;
                indirect = null;
            }
            else
            {
                //获取当前机构的二级三级返佣比例,不是当前代理的机构ID,而是当前号码的机构ID的返佣比例
                var org = organizeBLL.GetEntity(organizeId);
                if (org != null)
                {
                    decimal? YongRatio2 = org.YongRatio2;
                    decimal? YongRatio3 = org.YongRatio3;
                    direct = 0;
                    indirect = 0;
                    if (lv == 4 || lv == 3)
                    {
                        direct = YongRatio2;
                        indirect = YongRatio3;
                    }
                    else if (lv == 2)
                    {
                        direct = 0.3M;
                        indirect = 0;
                    }
                    else
                    {
                        direct = 0;
                        indirect = 0;
                    }
                }
                else
                {
                    direct = 0;
                    indirect = 0;
                }
            }
        }
        public static void getDirectLX(decimal? price, string lv, out decimal direct, out decimal indirect)
        {
            direct = 0;
            indirect = 0;
            if (price >= 10 && price <= 479)
            {
                if (lv == "普通代理")
                {
                    direct = 0.8M;
                    indirect = 24;
                }
                else if (lv == "黄金代理")
                {
                    direct = 0.6M;
                    indirect = 29;
                }
                else if (lv == "钻石代理")
                {
                    direct = 0.5M;
                    indirect = 45;
                }
            }
            else if (price >= 480 && price <= 799)
            {
                if (lv == "普通代理")
                {
                    direct = 0.8M;
                    indirect = 29;
                }
                else if (lv == "黄金代理")
                {
                    direct = 0.7M;
                    indirect = 34;
                }
                else if (lv == "钻石代理")
                {
                    direct = 0.6M;
                    indirect = 50;
                }
            }
            else if (price >= 800 && price <= 1099)
            {
                if (lv == "普通代理")
                {
                    direct = 0.9M;
                    indirect = 39;
                }
                else if (lv == "黄金代理")
                {
                    direct = 0.8M;
                    indirect = 44;
                }
                else if (lv == "钻石代理")
                {
                    direct = 0.7M;
                    indirect = 60;
                }
            }
            else if (price >= 1100 && price <= 2499)
            {
                if (lv == "普通代理")
                {
                    direct = 0.9M;
                    indirect = 60;
                }
                else if (lv == "黄金代理")
                {
                    direct = 0.8M;
                    indirect = 80;
                }
                else if (lv == "钻石代理")
                {
                    direct = 0.7M;
                    indirect = 100;
                }
            }
            else if (price >= 2500 && price <= 9749)
            {
                if (lv == "普通代理")
                {
                    direct = 0.9M;
                    indirect = 100;
                }
                else if (lv == "黄金代理")
                {
                    direct = 0.85M;
                    indirect = 150;
                }
                else if (lv == "钻石代理")
                {
                    direct = 0.8M;
                    indirect = 200;
                }
            }
            else if (price >= 9750 && price <= 19999)
            {
                if (lv == "普通代理")
                {
                    direct = 0.95M;
                    indirect = 120;
                }
                else if (lv == "黄金代理")
                {
                    direct = 0.9M;
                    indirect = 200;
                }
                else if (lv == "钻石代理")
                {
                    direct = 0.85M;
                    indirect = 300;
                }
            }
            else if (price >= 20000 && price <= 34999)
            {
                if (lv == "普通代理")
                {
                    direct = 0.97M;
                    indirect = 200;
                }
                else if (lv == "黄金代理")
                {
                    direct = 0.92M;
                    indirect = 300;
                }
                else if (lv == "钻石代理")
                {
                    direct = 0.88M;
                    indirect = 500;
                }
            }
            else if (price >= 35000)
            {
                if (lv == "普通代理")
                {
                    direct = 0.97M;
                    indirect = 300;
                }
                else if (lv == "黄金代理")
                {
                    direct = 0.92M;
                    indirect = 400;
                }
                else if (lv == "钻石代理")
                {
                    direct = 0.88M;
                    indirect = 600;
                }
            }
        }
    }
}