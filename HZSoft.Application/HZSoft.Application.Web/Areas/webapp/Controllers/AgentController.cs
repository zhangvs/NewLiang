using HZSoft.Application.Busines.BaseManage;
using HZSoft.Application.Busines.CustomerManage;
using HZSoft.Application.Code;
using HZSoft.Application.Entity.BaseManage;
using HZSoft.Application.Entity.CustomerManage;
using HZSoft.Application.Entity.WeChatManage;
using HZSoft.Application.Web.Utility;
using HZSoft.Util;
using HZSoft.Util.WeChat.Comm;
using Newtonsoft.Json;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.TenPayLibV3;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace HZSoft.Application.Web.Areas.webapp.Controllers
{
    [HandlerWXAuthorizeAttribute(LoginMode.Enforce)]
    public class AgentController : Controller
    {
        private Wechat_AgentBLL agentBll = new Wechat_AgentBLL();
        private OrdersJMBLL ordersJMBll = new OrdersJMBLL();
        private Client_LVBLL lvBLL = new Client_LVBLL();
        private WithdrawLogBLL withBLL = new WithdrawLogBLL();
        private UserBLL UserBLL = new UserBLL();
        

        private static TenPayV3Info tenPayV3Info = new TenPayV3Info(WeixinConfig.AppID, WeixinConfig.AppSecret, WeixinConfig.MchId
            , WeixinConfig.Key, WeixinConfig.TenPayV3Notify);


        //
        // GET: /webapp/Agent/
        public ActionResult Index(int? id)
        {
            //1.2根据注册的微信id去用户表中匹配是否有此员工
            Wechat_AgentEntity agentEntity = agentBll.GetEntityByOpenId(CurrentWxUser.OpenId);//o7HEd1LjnupfP0BBBMz5f69MFYVE
            if (agentEntity == null)
            {
                int? tid = null;
                if (!string.IsNullOrEmpty(id.ToString()))
                {
                    var pidEntity = agentBll.GetEntity(id);//上级
                    if (pidEntity != null)
                    {
                        if (!string.IsNullOrEmpty(pidEntity.Tid.ToString()))
                        {
                            tid = pidEntity.Tid;
                        }
                    }
                }

                agentEntity = new Wechat_AgentEntity()
                {
                    OpenId = CurrentWxUser.OpenId,
                    nickname = CurrentWxUser.NickName,
                    Sex = CurrentWxUser.Users.Sex,
                    HeadimgUrl = CurrentWxUser.Users.HeadimgUrl,
                    Province = CurrentWxUser.Users.Province,
                    City = CurrentWxUser.Users.City,//微信城市
                    Country = CurrentWxUser.Users.Country,
                    Pid = id,//设置上级id为访问id
                    Tid = tid,//获取上级中的顶级id
                    LV = "普通代理"
                };
                agentBll.SaveForm(null, agentEntity);
            }
            return View(agentEntity);
        }

        /// <summary>
        /// 升级
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult upgrade(int? id)
        {
            return View();
        }


        /// <summary>
        /// 创建顶级
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public ActionResult Create(string keyValue)
        {
            var user= UserBLL.GetEntity(keyValue);
            if (user != null)
            {
                var agentEntity = agentBll.GetEntityByOpenId(CurrentWxUser.OpenId);
                if (agentEntity != null)
                {
                    if (agentEntity.Pid!= agentEntity.Id && agentEntity.Pid != agentEntity.Id)
                    {
                        agentEntity.Pid = agentEntity.Id;//设置上级id为访问id
                        agentEntity.Tid = agentEntity.Id;//获取上级中的顶级id
                        agentBll.SaveForm(agentEntity.Id, agentEntity);

                        user.WeOpenId = CurrentWxUser.OpenId;
                        user.WeChat = CurrentWxUser.NickName;
                        user.TopAgentMark = 1;//顶级
                        UserBLL.SaveForm(user.UserId, user);
                    }
                    else
                    {
                        return Content("已经成为顶级，无需重复设置");
                    }
                }
                else
                {
                    agentEntity = new Wechat_AgentEntity()
                    {
                        OpenId = CurrentWxUser.OpenId,
                        nickname = CurrentWxUser.NickName,
                        Sex = CurrentWxUser.Users.Sex,
                        HeadimgUrl = CurrentWxUser.Users.HeadimgUrl,
                        Province = CurrentWxUser.Users.Province,
                        City = CurrentWxUser.Users.City,//微信城市
                        Country = CurrentWxUser.Users.Country,
                        LV = "普通代理"
                    };
                    agentBll.SaveForm(null, agentEntity);

                    user.WeOpenId = CurrentWxUser.OpenId;
                    user.WeChat = CurrentWxUser.NickName;
                    user.TopAgentMark = 1;//顶级
                    UserBLL.SaveForm(user.UserId, user);
                }
            }

            return View();
        }


        public ActionResult upgradeLevel(int? tid)
        {
            var agentEntity = agentBll.GetEntityByOpenId(CurrentWxUser.OpenId);
            if (agentEntity != null)
            {
                //var ordersEntityOld = ordersJMBll.GetList("{\"AgentId\":\"" + agentEntity.Id + "\",\"PayStatus\":\"" + 0 + "\"}");
                //if (ordersEntityOld.Count()>0)
                //{
                //    //存在未付款的升级订单

                //}

                LogHelper.AddLog("upgradeLevel tid=" + tid);//记录日志
                decimal price = 0;
                string LV = "";
                if (tid == 2)
                {
                    price = 399;
                    LV = "黄金代理";
                }
                else if (tid == 3)
                {
                    price = 1999;
                    LV = "钻石代理";
                }
                var sp_billno = string.Format("{0}{1}", "JM-", DateTime.Now.ToString("yyyyMMddHHmmss"));

                OrdersJMEntity ordersEntity = new OrdersJMEntity()
                {
                    Price = price,
                    LV = LV,
                    OrderSn = sp_billno,
                    OpenId = CurrentWxUser.OpenId,
                    NickName = CurrentWxUser.NickName,
                    AgentId = agentEntity.Id,
                    Pid = agentEntity.Pid,
                    Tid = agentEntity.Tid
                };

                ordersEntity = ordersJMBll.SaveForm(null, ordersEntity);//创建JM升级订单表

                var nonceStr = TenPayV3Util.GetNoncestr();
                var timeStamp = TenPayV3Util.GetTimestamp();

                //商品Id，用户自行定义
                var xmlDataInfoH5 = new TenPayV3UnifiedorderRequestData(WeixinConfig.AppID, tenPayV3Info.MchId, LV, sp_billno,
    Convert.ToInt32(Convert.ToDecimal(price) * 100), //1
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
                    ViewBag.WxModel = jsApiPayData;
                    LogHelper.AddLog(JsonConvert.SerializeObject(jsApiPayData));//记录日志
                }
            }
            return View();
        }

        /// <summary>
        /// 下级代理明细
        /// </summary>
        /// <returns></returns>
        public ActionResult comissionSummary(int? id)
        {
            return View();
        }



        /// <summary>
        /// 邀请代理
        /// </summary>
        /// <returns></returns>
        public ActionResult invitation(int? id)
        {
            var entity = agentBll.GetEntity(id);
            if (entity != null)
            {
                if (entity.LV == "黄金代理")
                {
                    ViewBag.LV2 = "钻石代理";
                    ViewBag.Count = 30;
                }
                else
                {
                    ViewBag.LV2 = "黄金代理";
                    ViewBag.Count = 80;
                }
                return View(entity);
            }
            return View();
        }



        /// <summary>
        /// 佣金明细
        /// </summary>
        /// <returns></returns>
        public ActionResult commission(int? id)
        {
            return View();
        }
        
        /// <summary>
        /// 提现记录
        /// </summary>
        /// <returns></returns>
        public ActionResult withdrawLogs(int? id)
        {
            return View();
        }
        /// <summary>
        /// 提现申请页面
        /// </summary>
        /// <returns></returns>
        public ActionResult withdrawal(int? id)
        {
            var agentEntity = agentBll.GetEntityByOpenId(CurrentWxUser.OpenId);
            if (agentEntity!=null)
            {
                ViewBag.profit = agentEntity.profit;
            }
            
            return View();
        }

        /// <summary>
        /// 提现申请提交
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult withdraw(decimal? cashNum)
        {
            ReturnJson root = null;

            var agentEntity = agentBll.GetEntityByOpenId(CurrentWxUser.OpenId);
            if (string.IsNullOrEmpty(agentEntity.alipay))
            {
                //401,跳转到修改个人信息
                root = new ReturnJson { code = 401, msg = "支付宝账号为空" };
            }
            else
            {
                if (cashNum>=100 && cashNum< agentEntity.profit)
                {
                    var withdrawLogList = withBLL.GetList("{\"agent_id\":\"" + agentEntity.Id + "\",\"status\":\"" + 0 + "\"}");
                    if (withdrawLogList.Count()>0)
                    {
                        //401,跳转到修改个人信息
                        root = new ReturnJson { code = 400, msg = "已经存在未审核的提现申请记录" };
                    }
                    //创建提现日志表
                    WithdrawLogEntity entity = new WithdrawLogEntity()
                    {
                        agent_id = agentEntity.Id,
                        agent_name = agentEntity.nickname,
                        amount = cashNum,
                        status = 0
                    };
                    withBLL.SaveForm(null, entity);

                    Wechat_AgentEntity wechat_AgentEntity = new Wechat_AgentEntity()
                    {
                        profit = agentEntity.profit - cashNum,//佣金-
                        Cashout = cashNum,//提现中+
                    };
                    agentBll.SaveForm(agentEntity.Id, wechat_AgentEntity);

                    root = new ReturnJson { code = 200, msg = "success" };
                }
                else
                {
                    root = new ReturnJson { code = 400, msg = "提现金额不符" };
                }
            }
            
            return Json(root);
        }
        /// <summary>
        /// 个人资料
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult profile(int? id)
        {
            return View();
        }

        
        
        public ActionResult GetWxPic(string media_id, string callback, string folder)
        {
            WeixinTokenBase tokenBase = new WeixinTokenBase();

            //判断是否保存微信token
            if (Session[WebSiteConfig.WXTOKEN_SESSION_NAME_BASE] != null)
            {
                tokenBase = Session[WebSiteConfig.WXTOKEN_SESSION_NAME_BASE] as WeixinTokenBase;
            }
            else
            {
                string returnUrl = Url.Action(callback);
                return RedirectToAction("Index", "Agent", new { urlstr = returnUrl });
            }
            string dir = "/Resource/DocumentFile/Agent/";
            if (Directory.Exists(Server.MapPath(dir)) == false)//如果不存在就创建file文件夹
            {
                Directory.CreateDirectory(Server.MapPath(dir));
            }

            string newfileName = Guid.NewGuid().ToString();
            //原图
            string fullDir1 = dir + newfileName + ".jpg";
            string imgFilePath = Request.MapPath(fullDir1);


            HttpWebResponse myResponse = null;
            try
            {
                var url = string.Format("https://api.weixin.qq.com/cgi-bin/media/get?access_token={0}&media_id={1}", tokenBase.access_token, media_id);
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);

                req.Method = "GET";

                myResponse = (HttpWebResponse)req.GetResponse();
                StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);
                Stream stream = myResponse.GetResponseStream();

                #region 保存下载图片  
                FileStream fileStream = new FileStream(imgFilePath, FileMode.Create, FileAccess.Write);
                byte[] bytes = new byte[4096];
                int readSize = 0;
                while ((readSize = stream.Read(bytes, 0, 4096)) > 0)
                {
                    fileStream.Write(bytes, 0, readSize);
                    fileStream.Flush();
                }
                #endregion

                myResponse.Close();
                stream.Close();
                fileStream.Close();
            }
            //异常请求  
            catch (WebException ex)
            {
                fullDir1 = "";
            }
            finally
            {

            }
            return Content(fullDir1);
        }



        /// <summary>
        /// 售价浮动
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult fudong(int? id)
        {
            var agentEntity = agentBll.GetEntityByOpenId(CurrentWxUser.OpenId);
            if (agentEntity != null)
            {
                ViewBag.fuDong = agentEntity.FuDong;
            }
            return View();
        }
        /// <summary>
        /// 售价浮动
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult fudong(decimal? fuDong)
        {
            ReturnJson returnJson = new ReturnJson();
            try
            {
                var agentEntity = agentBll.GetEntityByOpenId(CurrentWxUser.OpenId);
                if (agentEntity != null)
                {
                    Wechat_AgentEntity entity = new Wechat_AgentEntity()
                    {
                        FuDong = fuDong,
                    };
                    agentBll.FuDongUpdate(agentEntity.Id, fuDong);

                    returnJson.msg = "success";
                    returnJson.code = 200;
                    return Json(returnJson);
                }
                else
                {
                    var root = new ReturnJson { code = 401, msg = "fail" };
                    return Json(root);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
