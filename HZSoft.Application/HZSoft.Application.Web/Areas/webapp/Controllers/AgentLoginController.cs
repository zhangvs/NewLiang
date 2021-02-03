using HZSoft.Application.Busines.BaseManage;
using HZSoft.Application.Busines.WeChatManage;
using HZSoft.Application.Entity.BaseManage;
using HZSoft.Application.Entity.WeChatManage;
using HZSoft.Application.Web.Utility;
using HZSoft.Util;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.AdvancedAPIs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Net;
using HZSoft.Application.Entity.SystemManage;
using HZSoft.Application.Code;
using HZSoft.Application.Busines.AuthorizeManage;
using HZSoft.Application.Busines.SystemManage;
using HZSoft.Util.Attributes;
using Newtonsoft.Json;
using HZSoft.Application.Entity.CustomerManage;
using HZSoft.Application.Busines.CustomerManage;
using HZSoft.Application.Web.Areas.WeChatManage.Controllers;

namespace HZSoft.Application.Web.Areas.webapp.Controllers
{
    /// <summary>
    /// 微信认证和登录
    /// </summary>
    public class AgentLoginController : BaseWxUserController
    {
        Wechat_AgentBLL agentBll = new Wechat_AgentBLL();

        /// <summary>
        /// 微信登录：1.登录过得直接跳转到地图界面
        /// 2.没登录的新用户进入登录界面，使用账号密码登录，登录后会修改微信用户的userid，username字段，下次可直接进入主界面
        /// </summary>
        /// <returns></returns>
        [HandlerWXAuthorizeAttribute(LoginMode.Enforce)]
        public ActionResult Index(int? id)
        {
            //1.2根据注册的微信id去用户表中匹配是否有此员工
            Wechat_AgentEntity agentEntity = agentBll.GetEntityByOpenId(CurrentWxUser.OpenId);//"oQU_IwcWO42_aBNVXcVzungZA0uw"
            if (agentEntity == null)
            {
                int? tid=null;
                var pidEntity = agentBll.GetEntity(id);//上级
                if (pidEntity!=null)
                {
                    if (!string.IsNullOrEmpty(pidEntity.Tid.ToString()))
                    {
                        tid = pidEntity.Tid;
                    }
                    else
                    {
                        tid = id;
                    }
                }
                else
                {
                    tid = id;
                }
                Wechat_AgentEntity agentEntityNew = new Wechat_AgentEntity()
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
                    LV ="普通代理"
                };
                agentBll.SaveForm(null, agentEntityNew);

                //如果父级和顶级为null，坚决赋值，更新null为当前id，或者当前机构就是顶级员工
                Wechat_AgentEntity agentEntity2 = agentBll.GetEntityByOpenId(CurrentWxUser.OpenId);
                if (agentEntity2!=null)
                {
                    if (string.IsNullOrEmpty(agentEntity2.Pid.ToString()))
                    {
                        agentEntity2.Pid = agentEntity2.Id;
                        agentEntity2.Tid = agentEntity2.Id;
                    }
                    else if (string.IsNullOrEmpty(agentEntity2.Tid.ToString()))
                    {
                        agentEntity2.Tid = agentEntity2.Pid;
                    }
                }
                agentBll.SaveForm(agentEntity2.Id, agentEntity2);
            }

            OperatorAgent operators = new OperatorAgent();
            operators.Id = agentEntity.Id;
            operators.Pid = agentEntity.Pid;
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
            OperatorAgentProvider.Provider.AddCurrent(operators);

            return RedirectToAction("Index", "Agent", new { id = id });

        }

    }
}
