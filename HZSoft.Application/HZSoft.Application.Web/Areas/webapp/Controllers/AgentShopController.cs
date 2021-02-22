using Aop.Api;
using Aop.Api.Domain;
using Aop.Api.Request;
using Aop.Api.Response;
using HZSoft.Application.Busines.BaseManage;
using HZSoft.Application.Busines.CustomerManage;
using HZSoft.Application.Code;
using HZSoft.Application.Entity.BaseManage;
using HZSoft.Application.Entity.CustomerManage;
using HZSoft.Application.Entity.WeChatManage;
using HZSoft.Application.Web.Utility;
using HZSoft.Util;
using HZSoft.Util.WebControl;
using Newtonsoft.Json;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.TenPayLibV3;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using static HZSoft.Application.Web.Utility.DirectHelper;

namespace HZSoft.Application.Web.Areas.webapp.Controllers
{
    public class AgentShopController : Controller
    {
        private OrganizeBLL organizeBLL = new OrganizeBLL();
        private TelphoneLiangH5BLL tlbll = new TelphoneLiangH5BLL();
        private Wechat_AgentBLL agentBll = new Wechat_AgentBLL();
        private ComissionLogBLL comissionLogBll = new ComissionLogBLL();
        private Client_TelFeeBLL telFeeBll = new Client_TelFeeBLL();
        private OrdersBLL ordersbll = new OrdersBLL();
        private static TenPayV3Info tenPayV3Info = new TenPayV3Info(WeixinConfig.AppID, WeixinConfig.AppSecret, WeixinConfig.MchId
            , WeixinConfig.Key, WeixinConfig.TenPayV3Notify);


        public ActionResult Index(int? id)
        {
            ViewBag.banner = "/static/default/images/index/banner.png"; 
            ViewBag.OpenId = 0;
            ViewBag.is_agent = 0;
            ViewBag.Style = "none";
            ViewBag.StyleMinPrice = "none";
            //根据当前id，查询是哪个代理，把写入当前用户信息
            Wechat_AgentEntity agentEntity = agentBll.GetEntity(id);//"oQU_IwcWO42_aBNVXcVzungZA0uw"
            if (agentEntity != null)
            {
                if (OperatorAgentProvider.Provider.IsOverdue())
                {
                    OperatorAgent(agentEntity);
                    LogHelper.AddLog("缓存过期，重新缓存："+ OperatorAgentProvider.Provider.Current().Id);
                }
                else
                {
                    //如果连接参数id不等于缓存登陆者id，则清空缓存，重新缓存
                    if (id != OperatorAgentProvider.Provider.Current().Id)
                    {
                        LogHelper.AddLog("缓存不一致，重新更新缓存！从"+ OperatorAgentProvider.Provider.Current().Id+"到"+id);
                        OperatorAgentProvider.Provider.EmptyCurrent();

                        OperatorAgent(agentEntity);
                        LogHelper.AddLog("最新缓存："+ OperatorAgentProvider.Provider.Current().Id);
                    }
                }

                if (!string.IsNullOrEmpty(agentEntity.banner))
                {
                    ViewBag.banner = agentEntity.banner;
                }

                //如果是微信浏览器
                if (Request.UserAgent.ToLower().Contains("micromessenger"))
                {
                    //请求地址;
                    string RequestUri = Request.RawUrl;//AbsoluteUri//FilePath//!string.IsNullOrEmpty(request.Params["urlstr"]) ? request.Params["urlstr"] : 
                    if (RequestUri.IndexOf("&amp;") > 0)
                    {
                        //WeChatManage/Liang/Index?organizeId=4da3b884-ac5b-46fb-af10-150beb9e6c69&amp;amp;amp;amp;amp;from=timeline&amp;amp;amp;amp;from=timeline&amp;amp;amp;from=timeline&amp;amp;from=timeline&amp;from=timeline
                        //防止提示state参数过长问题,连接转发多次之后会带着一些转发到哪里的冗余信息
                        RequestUri = RequestUri.Substring(0, RequestUri.IndexOf("&amp;"));//WeChatManage/Liang/Index?organizeId=4da3b884-ac5b-46fb-af10-150beb9e6c69
                    }
                    //判断是否微信通过认证
                    if (CurrentWxUser.Users == null)
                    {
                        string url = string.Format(WeixinConfig.GetCodeUrl, HttpUtility.UrlEncode(RequestUri));
                        return Redirect(url);
                    }

                    Wechat_AgentEntity agentEntityWX = agentBll.GetEntityByOpenId(CurrentWxUser.OpenId);//"oQU_IwcWO42_aBNVXcVzungZA0uw"
                    //只有当前登录者是代理的时候，才显示低价
                    if (agentEntityWX != null)
                    {
                        //只有二级三级才返佣金
                        if (agentEntityWX.Category <= 3)
                        {
                            ViewBag.OpenId = agentEntity.OpenId;
                            ViewBag.is_agent = 1;
                            ViewBag.Style = "display";
                        }
                        //只有二级三级才返佣金
                        if (agentEntityWX.Category <= 2)
                        {
                            ViewBag.OpenId = agentEntity.OpenId;
                            ViewBag.is_agent = 1;
                            ViewBag.Style = "display";
                            ViewBag.StyleMinPrice = "display";
                        }
                    }
                }
                //浏览量自增·1
                agentBll.SeeCountAdd(id);

            }

            return View();
        }
        public void OperatorAgent(Wechat_AgentEntity agentEntity)
        {
            OperatorAgent operators = new OperatorAgent();
            operators.Id = agentEntity.Id;
            operators.Pid = agentEntity.Pid;
            operators.Tid = agentEntity.Tid;
            operators.OpenId = agentEntity.OpenId;
            operators.nickname = agentEntity.nickname;
            operators.Sex = agentEntity.Sex;
            operators.HeadimgUrl = agentEntity.HeadimgUrl;
            operators.Province = agentEntity.Province;
            operators.City = agentEntity.City;
            operators.Country = agentEntity.Country;
            operators.LV = agentEntity.LV;
            operators.profit = agentEntity.profit;
            operators.Cashout = agentEntity.Cashout;
            operators.EndDate = agentEntity.EndDate;
            operators.banner = agentEntity.banner;
            operators.realname = agentEntity.realname;
            operators.contact = agentEntity.contact;
            operators.alipay = agentEntity.alipay;
            operators.CreateDate = agentEntity.CreateDate;
            operators.IPAddress = Net.Ip;
            operators.IPAddressName = IPLocation.GetLocation(Net.Ip);
            operators.LogTime = DateTime.Now;
            operators.Token = DESEncrypt.Encrypt(Guid.NewGuid().ToString());
            operators.FuDong = agentEntity.FuDong;
            operators.OrganizeId = agentEntity.OrganizeId;
            operators.Category = agentEntity.Category;
            OperatorAgentProvider.Provider.AddCurrent(operators);
        }



        /// <summary>
        /// 模糊搜索等 + '&price=' + price + '&except=' + except + '&yy=' + yuyi;
        /// </summary>
        /// <returns></returns>
        public ActionResult getPhoneNum(AgentWhereEntity pageInfo)
        {
            int ipage = pageInfo.page == null ? 1 : int.Parse(pageInfo.page.ToString());

            if (pageInfo.orderby == "sale_price")
            {
                pageInfo.orderby = "price";
            }
            if (string.IsNullOrEmpty(pageInfo.ordername))
            {
                pageInfo.orderby = "TelphoneID";//按照最后一位排序
                pageInfo.ordername = "desc";
            }
            string search = "";
            if (!string.IsNullOrEmpty(pageInfo.search))
            {
                search = pageInfo.search;
            }
            else if (!string.IsNullOrEmpty(pageInfo.precise_num))
            {
                search = pageInfo.precise_num;
            }
            WhereEntity whereEntity = new WhereEntity()
            {
                page = pageInfo.page,
                price = pageInfo.price,
                Grade = pageInfo.regex,//规律
                search = search,
                search_is_end = pageInfo.search_is_end,
                province = pageInfo.province,
                City = pageInfo.city,//小写为城市编号，这里其实是中文带市
                orderby = pageInfo.orderby,
                ordername = pageInfo.ordername,
                nomore = pageInfo.isNormal,//
                Operator = pageInfo.isp,//运营商
                repeatNumber = pageInfo.repeatNumber,
                except = pageInfo.exceptNumber,
                moreNumber = pageInfo.moreNumber,
                birthdayNumber = pageInfo.birthdayNumber
            };
            Pagination pagination = new Pagination()
            {
                rows = 40,
                page = ipage,
                sidx = pageInfo.orderby,
                sord = pageInfo.ordername
            };
            var data = tlbll.GetPageListH5(pagination, whereEntity.ToJson());

            var jsonData = new
            {
                current_page = pagination.page,
                data = data,
                last_page = pagination.total,
                total = pagination.records,
                discount = OperatorAgentProvider.Provider.Current().FuDong//折扣100.相当于不打折
            };
            return Json(jsonData);
        }

        /// <summary>
        /// 佣金数据
        /// </summary>
        /// <returns></returns>
        public ActionResult getProfit(string mobile, string openid)
        {
            var agentEntity = agentBll.GetEntityByOpenId(openid);
            if (agentEntity != null)
            {
                decimal? direct = 0;
                decimal? indirect = 0;
                Dictionary<string, Comission> dataCom = new Dictionary<string, Comission>();

                var entityList = tlbll.GetList("{\"Telphones\":\"" + mobile + "\"}");
                if (entityList.Count() > 0)
                {
                    foreach (var telEntity in entityList)
                    {
                        if (telEntity.Price != null)
                        {
                            telEntity.Price = telEntity.Price * OperatorAgentProvider.Provider.Current().FuDong * 0.01M;//浮动
                            //获取返佣金额
                            getDirectDH(telEntity.OrganizeId, agentEntity.Category, out direct, out indirect);
                            Comission comission = new Comission()
                            {
                                direct = telEntity.Price * direct,
                                indirect = telEntity.Price * indirect
                            };
                            dataCom.Add(telEntity.Telphone, comission);

                        }
                    }
                }

                dynamic obj = new ExpandoObject();
                obj.code = 200;
                obj.msg = "success";
                obj.data = dataCom;

                string json = JsonConvert.SerializeObject(obj);
                return Content(json, "text/json");
            }
            else
            {
                return null;
            }
        }

        public ActionResult product(int? id)
        {
            TelphoneLiangH5Entity entity = tlbll.GetEntity(id);
            if (entity != null)
            {
                entity.Price = entity.Price * OperatorAgentProvider.Provider.Current().FuDong * 0.01M;//浮动
                return View(entity);
            }
            else
            {
                return Content("号码不存在！");
            }
        }

        [HttpPost]
        public ActionResult CreateOrder(OrdersEntity ordersEntity)
        {
            try
            {
                string city = ordersEntity.City;
                if (city.Contains("-"))
                {
                    string[] area = ordersEntity.City.Split('-');
                    if (area.Length > 0)
                    {
                        ordersEntity.Province = area[0];//省
                        ordersEntity.City = area[1];//市
                        ordersEntity.Country = area[2];//市
                    }
                }
                else
                {
                    string[] area = ordersEntity.City.Split(' ');
                    if (area.Length > 0)
                    {
                        ordersEntity.Province = area[0];//省
                        ordersEntity.City = area[1];//市
                    }
                }

                //创建订单表
                string payType = ordersEntity.PayType;
                if (payType == "1")
                {
                    payType = "支付宝";
                }
                else
                {
                    if (ordersEntity.PC == 1)
                    {
                        payType = "微信扫码";
                    }
                    else if (Request.UserAgent.ToLower().Contains("micromessenger"))
                    {
                        payType = "微信JSAPI";
                    }
                    else
                    {
                        payType = "微信H5";
                    }
                }
                ordersEntity.PayType = payType;
                ordersEntity.OrderSn = string.Format("{0}{1}", "FX-", DateTime.Now.ToString("yyyyMMddHHmmss"));//分销订单区分LX
                if (!OperatorAgentProvider.Provider.IsOverdue())
                {
                    //连接的代理id
                    ordersEntity.Host = OperatorAgentProvider.Provider.Current().nickname;//昵称
                    ordersEntity.AgentId = OperatorAgentProvider.Provider.Current().Id;
                    ordersEntity.Pid = OperatorAgentProvider.Provider.Current().Pid;
                    ordersEntity.Tid = OperatorAgentProvider.Provider.Current().Tid;
                    ordersEntity.OpenId = CurrentWxUser.OpenId;//购买者OpenId，用于首页展示再次支付提示，方便再次支付
                    ordersEntity = ordersbll.SaveForm(ordersEntity);


                    if (!string.IsNullOrEmpty(OperatorAgentProvider.Provider.Current().Id.ToString()))
                    {
                        decimal? direct = 0;
                        decimal? indirect = 0;

                        getDirectDH(ordersEntity.OrganizeId, OperatorAgentProvider.Provider.Current().Category, out direct, out indirect);
                        if (direct != 0)
                        {
                            //直接号码返佣
                            ComissionLogEntity logEntity = new ComissionLogEntity()
                            {
                                agent_id = OperatorAgentProvider.Provider.Current().Id,
                                agent_name = OperatorAgentProvider.Provider.Current().nickname,
                                indirect = 0,
                                invited_agent_id = 0,
                                phonenum = ordersEntity.Tel,
                                profit = ordersEntity.Price * direct,//佣金=原价格-原价格*折扣
                                status = 0,
                                orderno = ordersEntity.OrderSn,
                                orderid = ordersEntity.Id
                            };
                            comissionLogBll.SaveForm(null, logEntity);
                        }

                        if (indirect!=0)
                        {
                            //间接号码返佣
                            ComissionLogEntity logEntity2 = new ComissionLogEntity()
                            {
                                agent_id = OperatorAgentProvider.Provider.Current().Pid,
                                agent_name = OperatorAgentProvider.Provider.Current().nickname,//显示返现的昵称log还是下级，因为下级才返现的
                                indirect = 1,//间接
                                invited_agent_id = 0,//父机构，间接返现
                                phonenum = ordersEntity.Tel,
                                profit = indirect,
                                status = 0,
                                orderno = ordersEntity.OrderSn,
                                orderid = ordersEntity.Id
                            };
                            comissionLogBll.SaveForm(null, logEntity2);
                        }

                        //代售 产生的订单佣金(售价30%)返给对应1级(由1级自由分配)
                        if (ordersEntity.OrganizeId != OperatorAgentProvider.Provider.Current().OrganizeId)
                        {
                            //根据当前代理找到1级
                            var entity = agentBll.GetEntity(OperatorAgentProvider.Provider.Current().Id);
                            for (int i = 0; i < 3; i++)
                            {
                                if (entity.Category!=1)
                                {
                                    entity=agentBll.GetEntity(entity.Pid);
                                }
                                else
                                {                            
                                    //给1级返佣30%
                                    ComissionLogEntity logEntity = new ComissionLogEntity()
                                    {
                                        agent_id = entity.Id,
                                        agent_name = entity.nickname,
                                        indirect = 2,//代售
                                        invited_agent_id = 0,
                                        phonenum = ordersEntity.Tel,
                                        profit = ordersEntity.Price * 0.3M,//佣金=原价格-原价格*折扣
                                        status = 0,
                                        orderno = ordersEntity.OrderSn,
                                        orderid = ordersEntity.Id
                                    };
                                    comissionLogBll.SaveForm(null, logEntity);
                                    break;
                                }
                            }
                        }

                        //给0级返本金
                        var orgEntity = organizeBLL.GetEntity(ordersEntity.OrganizeId);
                        if (orgEntity!=null)
                        {
                            var tidEntity = agentBll.GetEntity(orgEntity.AgentId);
                            if (tidEntity != null)
                            {
                                ComissionLogEntity logEntity = new ComissionLogEntity()
                                {
                                    agent_id = tidEntity.Id,
                                    agent_name = tidEntity.nickname,
                                    indirect = 3,//本金
                                    invited_agent_id = 0,
                                    phonenum = ordersEntity.Tel,
                                    profit = ordersEntity.Price - ordersEntity.Price*direct - ordersEntity.Price*indirect - ordersEntity.Price * 0.01M,//本金=售价-佣金合计-售价0.01
                                    status = 0,
                                    orderno = ordersEntity.OrderSn,
                                    orderid = ordersEntity.Id
                                };
                                comissionLogBll.SaveForm(null, logEntity);
                            }
                        }
                    }

                    return Content("{\"code\":200,\"msg\":\"success\",\"data\":{\"orderno\":\"" + ordersEntity.OrderSn + "\"}}", "text/json");
                }
                else
                {
                    LogHelper.AddLog("缓存过期，请重新打开首页链接！");
                    return Content("{\"code\":400,\"msg\":\"缓存过期，请重新打开首页链接！\"}", "text/json");
                }
            }
            catch (Exception ex)
            {
                LogHelper.AddLog(ex.Message);//记录日志
                throw;
            }
        }

        /// <summary>
        /// 获取最近的一个订单
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult getLastOrder()
        {
            ReturnJson root = null;
            if (!string.IsNullOrEmpty(CurrentWxUser.OpenId))
            {
                var agentList = ordersbll.GetList("{\"OpenId\":\"" + CurrentWxUser.OpenId + "\"}");
                if (agentList != null)
                {
                    root = new ReturnJson { code = 200, msg = "success", data = agentList.FirstOrDefault() };
                }
            }
            root = new ReturnJson { code = 400, msg = "fail" };
            return Json(root);
        }

        [HttpGet]
        public ActionResult prePayOrder(string orderno, string gateway)
        {
            ViewBag.orderno = orderno;
            ViewBag.gateway = gateway;
            return View();
        }

        [HttpPost]
        public ActionResult ajaxorder(string orderno)
        {
            try
            {
                ReturnJson root = null;
                var ordersEntity = ordersbll.GetEntityByOrderSn(orderno);
                if (ordersEntity != null)
                {
                    var sp_billno = ordersEntity.OrderSn;
                    var nonceStr = TenPayV3Util.GetNoncestr();

                    //商品Id，用户自行定义
                    string productId = ordersEntity.TelphoneID.ToString();
                    decimal? Amount = ordersEntity.Price;//0.01M 测试
                    if (Amount < 0)
                    {
                        root = new ReturnJson { code = 200, msg = "付款金额小于0" };
                    }
                    else
                    {
                        if (ordersEntity.PayType == "支付宝")
                        {
                            try
                            {
                                DefaultAopClient client = new DefaultAopClient(WeixinConfig.serviceUrl, WeixinConfig.aliAppId, WeixinConfig.privateKey, "json", "1.0",
                                    WeixinConfig.signType, WeixinConfig.payKey, WeixinConfig.charset, false);

                                // 组装业务参数model
                                AlipayTradeWapPayModel model = new AlipayTradeWapPayModel();
                                model.Body = "支付宝购买靓号";// 商品描述
                                model.Subject = productId;// 订单名称
                                model.TotalAmount = Amount.ToString();// 付款金额"0.01"ordersEntity.Price.ToString()
                                model.OutTradeNo = sp_billno;// 外部订单号，商户网站订单系统中唯一的订单号
                                model.ProductCode = "QUICK_WAP_WAY";
                                model.QuitUrl = "https://www.1650539.com/webapp/agentshop/index";// 支付中途退出返回商户网站地址
                                model.TimeoutExpress = "90m";
                                AlipayTradeWapPayRequest request = new AlipayTradeWapPayRequest();
                                //设置支付完成同步回调地址
                                request.SetReturnUrl(WeixinConfig.return_url);
                                //设置支付完成异步通知接收地址
                                request.SetNotifyUrl(WeixinConfig.notify_url);
                                // 将业务model载入到request
                                request.SetBizModel(model);

                                AlipayTradeWapPayResponse response = null;
                                try
                                {
                                    response = client.pageExecute(request, null, "post");
                                    //Response.Write(response.Body);
                                    H5PayData h5PayData = new H5PayData();
                                    h5PayData.form = response.Body;
                                    root = new ReturnJson { code = 200, msg = "\u652f\u4ed8\u5b9d\u63d0\u4ea4\u6210\u529f\uff01", data = h5PayData };
                                }
                                catch (Exception exp)
                                {
                                    throw exp;
                                }
                            }
                            catch (Exception ex)
                            {
                                //return Json(new { Result = false, msg = "缺少参数" });
                            }
                        }
                        else
                        {
                            //0 手机（H5支付）  1 电脑（扫码Native支付），2微信浏览器（JSAPI）
                            //pc端返回二维码，否则H5
                            if (ordersEntity.PayType == "微信扫码")
                            {
                                //创建请求统一订单接口参数
                                var xmlDataInfo = new TenPayV3UnifiedorderRequestData(WeixinConfig.AppID,
                                tenPayV3Info.MchId,
                                "扫码支付靓号",
                                sp_billno,
                                Convert.ToInt32(Amount * 100),
                                //1,
                                Request.UserHostAddress,
                                tenPayV3Info.TenPayV3Notify,
                               TenPayV3Type.NATIVE,
                                null,
                                tenPayV3Info.Key,
                                nonceStr,
                                productId: productId);
                                //调用统一订单接口
                                var result = TenPayV3.Unifiedorder(xmlDataInfo);
                                if (result.return_code == "SUCCESS")
                                {
                                    H5PayData h5PayData = new H5PayData()
                                    {
                                        appid = WeixinConfig.AppID,
                                        code_url = result.code_url,//weixin://wxpay/bizpayurl?pr=lixpXgt-----------扫码支付
                                        mch_id = WeixinConfig.MchId,
                                        nonce_str = result.nonce_str,
                                        prepay_id = result.prepay_id,
                                        result_code = result.result_code,
                                        return_code = result.return_code,
                                        return_msg = result.return_msg,
                                        sign = result.sign,
                                        trade_type = "NATIVE",
                                        trade_no = sp_billno,
                                        payid = ordersEntity.Id.ToString(),
                                        wx_query_href = "https://www.1650539.com/webapp/agentshop/queryWx/" + ordersEntity.Id,
                                        wx_query_over = "https://www.1650539.com/webapp/agentshop/paymentFinish/" + ordersEntity.Id
                                    };

                                    root = new ReturnJson { code = 200, msg = "\u5fae\u4fe1\u626b\u7801\u63d0\u4ea4\u6210\u529f\uff01", data = h5PayData };
                                }
                                else
                                {
                                    root = new ReturnJson { code = 400, msg = result.return_msg };
                                }
                            }
                            else
                            {
                                var xmlDataInfoH5 = new TenPayV3UnifiedorderRequestData(WeixinConfig.AppID, tenPayV3Info.MchId, "H5购买靓号", sp_billno,
                                // 1,
                                Convert.ToInt32(Amount * 100),
                                Request.UserHostAddress, tenPayV3Info.TenPayV3Notify, TenPayV3Type.MWEB/*此处无论传什么，方法内部都会强制变为MWEB*/, null, tenPayV3Info.Key, nonceStr);

                                var resultH5 = TenPayV3.Html5Order(xmlDataInfoH5);//调用统一订单接口
                                LogHelper.AddLog(resultH5.ResultXml);//记录日志
                                if (resultH5.return_code == "SUCCESS")
                                {
                                    H5PayData h5PayData = new H5PayData()
                                    {
                                        appid = WeixinConfig.AppID,
                                        mweb_url = resultH5.mweb_url,//H5访问链接
                                        mch_id = WeixinConfig.MchId,
                                        nonce_str = resultH5.nonce_str,
                                        prepay_id = resultH5.prepay_id,
                                        result_code = resultH5.result_code,
                                        return_code = resultH5.return_code,
                                        return_msg = resultH5.return_msg,
                                        sign = resultH5.sign,
                                        trade_type = "H5",
                                        trade_no = sp_billno,
                                        payid = ordersEntity.Id.ToString(),
                                        wx_query_href = "https://www.1650539.com/webapp/agentshop/queryWx/" + ordersEntity.Id,
                                        wx_query_over = "https://www.1650539.com/webapp/agentshop/paymentFinish/" + ordersEntity.Id
                                    };

                                    root = new ReturnJson { code = 200, msg = "\u5fae\u4fe1\u0048\u0035\u63d0\u4ea4\u6210\u529f\uff01", data = h5PayData };
                                }
                                else
                                {
                                    root = new ReturnJson { code = 400, msg = resultH5.return_msg };
                                }
                            }
                        }
                    }

                }
                else
                {
                    root = new ReturnJson { code = 400, msg = "订单号不存在！" };
                }

                LogHelper.AddLog(JsonConvert.SerializeObject(root));//记录日志
                return Json(root);
            }
            catch (Exception ex)
            {
                LogHelper.AddLog(ex.Message);//记录日志
                throw;
            }
        }

        [HandlerWXAuthorizeAttribute(LoginMode.Enforce)]
        public ActionResult pay(string orderno)
        {
            var ordersEntity = ordersbll.GetEntityByOrderSn(orderno);
            if (ordersEntity != null)
            {
                var nonceStr = TenPayV3Util.GetNoncestr();
                var timeStamp = TenPayV3Util.GetTimestamp();

                //商品Id，用户自行定义
                var xmlDataInfoH5 = new TenPayV3UnifiedorderRequestData(WeixinConfig.AppID, tenPayV3Info.MchId, ordersEntity.Tel, ordersEntity.OrderSn,
    Convert.ToInt32(Convert.ToDecimal(ordersEntity.Price) * 100),//1
    Request.UserHostAddress, WeixinConfig.TenPayV3Notify, TenPayV3Type.JSAPI, CurrentWxUser.OpenId, tenPayV3Info.Key, nonceStr);
                var result = TenPayV3.Unifiedorder(xmlDataInfoH5);//调用统一订单接口
                LogHelper.AddLog(result.ResultXml);//记录日志
                var package = string.Format("prepay_id={0}", result.prepay_id);
                if (result.return_code == "SUCCESS")
                {
                    WFTWxModel jsApiPayData = new WFTWxModel()
                    {
                        appId = WeixinConfig.AppID,
                        timeStamp = timeStamp,
                        nonceStr = nonceStr,
                        package = package,
                        paySign = TenPayV3.GetJsPaySign(WeixinConfig.AppID, timeStamp, nonceStr, package, WeixinConfig.Key)
                    };
                    ViewBag.id = ordersEntity.Id;
                    ViewBag.WxModel = jsApiPayData;
                    LogHelper.AddLog(JsonConvert.SerializeObject(jsApiPayData));//记录日志
                }
                return View();
            }
            else
            {
                ReturnJson root = new ReturnJson { code = 400, msg = "订单号不存在！" };
                return Json(root);
            }
        }

        public ActionResult express(string mobile)
        {
            string display = "none";
            if (!string.IsNullOrEmpty(mobile))
            {
                var ordersEntity = ordersbll.GetEntityByTel(mobile);
                if (ordersEntity != null)
                {
                    string msg = "";
                    //0 待付款 1 待发货 2 待开卡 3 已完成
                    switch (ordersEntity.Status)
                    {
                        case 0:
                            msg = "待付款";
                            break;
                        case 1:
                            msg = "待发货";
                            break;
                        case 2:
                            msg = "已发货待开卡，" + ordersEntity.ExpressCompany + "：" + ordersEntity.ExpressSn;
                            break;
                        case 3:
                            msg = "已完成";
                            break;
                        default:
                            break;
                    }
                    ViewBag.msg = msg;
                }
                else
                {
                    ViewBag.msg = "暂无信息";
                }
                display = "block";

            }
            ViewBag.display = display;
            return View();
        }

        public ActionResult queryWx(int? id)
        {
            OrdersEntity ordersEntity = ordersbll.GetEntity(id);
            if (ordersEntity.PayStatus == 1)
            {
                return Json(new { status = true });
            }
            return Json(new { status = false });
        }

        /// <summary>
        /// 支付结果页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult paymentFinish(int? id)
        {
            ViewBag.Id = id;
            OrdersEntity ordersEntity = ordersbll.GetEntity(id);//第一次打开，微信回调还没完成，一般是未支付状态
            for (int i = 0; i < 3; i++)
            {
                ordersEntity = ordersbll.GetEntity(id);//第二次才会成功获取到支付成功状态
                if (ordersEntity.PayStatus == 1)//如果支付成功直接返回
                {
                    ViewBag.Result = "支付成功";
                    ViewBag.icon = "success";
                    ViewBag.display = "none";
                    ViewBag.Tel = ordersEntity.Tel;
                    LogHelper.AddLog(id + "支付成功");
                    return View();
                }
                else
                {
                    Thread.Sleep(3000);//当前线程休眠3秒，等待微信回调执行完成
                    LogHelper.AddLog(id + "支付结果获取：" + i);
                }
            }
            //如果超过15秒还未支付成功，返回未支付，防止直接跳转结果页面，显示失败，微信回调还没有完成
            ViewBag.Result = "未支付";
            ViewBag.icon = "warn";
            ViewBag.display = "block";
            ViewBag.id = OperatorAgentProvider.Provider.Current().Id.ToString();
            ViewBag.orderno = ordersEntity.OrderSn;
            return View();
        }




    }
}
