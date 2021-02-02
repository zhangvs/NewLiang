﻿using Aop.Api.Util;
using HZSoft.Application.Busines.BaseManage;
using HZSoft.Application.Busines.CustomerManage;
using HZSoft.Application.Busines.WeChatManage;
using HZSoft.Application.Code;
using HZSoft.Application.Entity.BaseManage;
using HZSoft.Application.Entity.CustomerManage;
using HZSoft.Application.Entity.WeChatManage;
using HZSoft.Application.Web.Utility;
using HZSoft.Application.Web.Utility.AliPay;
using HZSoft.Util;
using Newtonsoft.Json;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.TenPayLibV3;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HZSoft.Application.Web.Areas.WeChatManage.Controllers
{
    public class WeiXinHomeController : Controller
    {
        private WeChat_UsersBLL userBll = new WeChat_UsersBLL();
        private OrdersBLL ordersbll = new OrdersBLL();
        private TelphoneLiangH5BLL tlbll = new TelphoneLiangH5BLL();
        private ComissionLogBLL comissionLogBll = new ComissionLogBLL();
        private OrdersJMBLL ordersJMbll = new OrdersJMBLL();
        private Wechat_AgentBLL agentbll = new Wechat_AgentBLL();

        //跳转
        public ActionResult Redirect(string code, string state)
        {
            //若用户禁止授权，则重定向后不会带上code参数
            if (string.IsNullOrEmpty(code))
            {
                return Redirect(state);
            }
            else
            {
                WeixinToken token = new WeixinToken();

                //判断是否保存微信token
                if (Session[WebSiteConfig.WXTOKEN_SESSION_NAME] != null)
                {
                    token = Session[WebSiteConfig.WXTOKEN_SESSION_NAME] as WeixinToken;
                }
                else
                {
                    string tokenUrl = string.Format(WeixinConfig.GetTokenUrl, code);
                    token = AnalyzeHelper.Get<WeixinToken>(tokenUrl);
                    if (token.errcode != null)
                    {
                        return Content(token.errcode + ":" + token.errmsg);
                    }
                    Session[WebSiteConfig.WXTOKEN_SESSION_NAME] = token;
                }
                //查询用户是否存在
                var userEntity = userBll.GetEntity(token.openid);
                if (userEntity == null)
                {
                    string userInfoUrl = string.Format(WeixinConfig.GetUserInfoUrl, token.access_token, token.openid);
                    var userInfo = AnalyzeHelper.Get<WeixinUserInfo>(userInfoUrl);
                    if (userInfo.errcode != null)
                    {
                        Response.Write(userInfo.errcode + ":" + userInfo.errmsg);
                        Response.End();
                    }
                    else
                    {
                        userEntity = new WeChat_UsersEntity()
                        {
                            City = userInfo.city,
                            Country = userInfo.country,
                            HeadimgUrl = userInfo.headimgurl,
                            NickName = userInfo.nickname,
                            OpenId = userInfo.openid,
                            Province = userInfo.province,
                            Sex = userInfo.sex,
                            AppName = WeixinConfig.AppName
                        };
                        userBll.SaveForm("", userEntity);
                    }
                }
                Session[WebSiteConfig.WXUSER_SESSION_NAME] = userEntity;
                return Redirect(state);
            }
        }



        //微信支付回调地址
        public ActionResult Notify()
        {
            LogHelper.AddLog("支付回调地址");//记录日志
            ResponseHandler rspHandler = new ResponseHandler(null);
            rspHandler.SetKey(WeixinConfig.Key);
            LogHelper.AddLog(rspHandler.ParseXML());//记录日志


            //SUCCESS/FAIL此字段是通信标识，非交易标识，交易是否成功需要查看result_code来判断 SUCCESS
            string return_code = rspHandler.GetParameter("return_code");
            string return_msg = rspHandler.GetParameter("return_msg");
            if (rspHandler.IsTenpaySign())
            {
                if (return_code == "SUCCESS")
                {
                    //订单号
                    string orderSn = rspHandler.GetParameter("out_trade_no");
                    if (orderSn.Contains("LX-"))
                    {
                        OrdersEntity order = ordersbll.GetEntityByOrderSn(orderSn);
                        if (order != null)
                        {
                            order.PayDate = DateTime.Now;
                            order.PayStatus = (int)PayStatus.已支付;
                            order.Status = (int)OrderStatus.未发货;
                            ordersbll.SaveForm(order.Id, order);
                        }
                        else
                        {
                            LogHelper.AddLog("订单号不存在：" + orderSn);
                        }

                        //同步下架
                        TelphoneLiangH5Entity tel = tlbll.GetEntity(order.TelphoneID);//根据靓号id获取靓号，修改售出状态
                        if (tel != null)
                        {
                            tel.SellMark = 1;
                            tel.SellerName = order.Host;
                        }
                        tlbll.SaveForm(tel.TelphoneID, tel);
                        LogHelper.AddLog("同步下架：" + tel.Telphone);


                        //头条url回调
                        if (!string.IsNullOrEmpty(order.TouUrl))
                        {
                            TouTiaoApi(order.TouUrl);
                        }
                    }
                    else if(orderSn.Contains("JM-"))//加盟订单
                    {
                        var order = ordersJMbll.GetEntityByOrderSn(orderSn);
                        if (order != null)
                        {
                            LogHelper.AddLog("微信支付加盟订单FX-异步调用orderSn：" + orderSn);
                            //支付成功，修改加盟订单表
                            order.PayDate = DateTime.Now;
                            order.PayStatus = (int)PayStatus.已支付;
                            ordersJMbll.SaveForm(order.Id, order);

                            //修改会员表级别
                            var agent = agentbll.GetEntityByOpenId(order.OpenId);//订单里面的OpenId，不是当前微信OpenId
                            if (agent!= null)
                            {
                                agent.LV = order.LV;
                                agentbll.SaveForm(agent.Id, agent);

                                decimal direct = 0;
                                decimal indirect = 0;

                                //上级不等于本身，才进行返佣，如果上级就是自己则为顶级
                                if (agent.Pid!=agent.Id)
                                {
                                    Wechat_AgentEntity agentPid = agentbll.GetEntity(agent.Pid);//上级
                                    if (agentPid != null)
                                    {
                                        Wechat_AgentEntity agentPid2 = agentbll.GetEntity(agentPid.Pid);//上上级
                                        if (agentPid2 != null)
                                        {
                                            //如果父级不等于上上级，则对上上级进行返佣
                                            if (agent.Pid != agentPid2.Id)
                                            {
                                                //获取返佣规则金额
                                                DirectHelper.getJMDirect(order.LV,  agentPid.LV, agentPid2.LV, out direct, out indirect);
                                                //上级返佣
                                                agentPid.profit += direct;
                                                agentbll.SaveForm(agentPid.Id, agentPid);

                                                //直接返佣日志
                                                ComissionLogEntity logEntity = new ComissionLogEntity()
                                                {
                                                    agent_id = agentPid.Id,
                                                    agent_name = agentPid.nickname,
                                                    indirect = 0,
                                                    invited_agent_id = order.AgentId,
                                                    phonenum = order.LV,
                                                    profit = direct,
                                                    status = 2,//已入账
                                                    orderno = order.OrderSn
                                                };
                                                comissionLogBll.SaveForm(null, logEntity);
                                                LogHelper.AddLog("升级订单直接返佣：" + direct);

                                                //上上级返佣
                                                agentPid2.profit += indirect;
                                                agentbll.SaveForm(agentPid2.Id, agentPid2);

                                                //间接返佣日志
                                                ComissionLogEntity logEntity2 = new ComissionLogEntity()
                                                {
                                                    agent_id = agentPid2.Id,
                                                    agent_name = agentPid2.nickname,
                                                    indirect = 1,
                                                    invited_agent_id = order.AgentId,
                                                    phonenum = order.LV,
                                                    profit = indirect,
                                                    status = 2,//已入账
                                                    orderno = order.OrderSn
                                                };
                                                comissionLogBll.SaveForm(null, logEntity2);
                                                LogHelper.AddLog("升级订单间接返佣：" + indirect);
                                            }
                                            else
                                            {
                                                //获取返佣规则金额
                                                DirectHelper.getJMDirect(order.LV, agentPid.LV, null, out direct, out indirect);
                                                //上级返佣
                                                agentPid.profit += direct;
                                                agentbll.SaveForm(agentPid.Id, agentPid);

                                                //直接返佣日志
                                                ComissionLogEntity logEntity = new ComissionLogEntity()
                                                {
                                                    agent_id = agentPid.Id,
                                                    agent_name = agentPid.nickname,
                                                    indirect = 0,
                                                    invited_agent_id = order.AgentId,
                                                    phonenum = order.LV,
                                                    profit = direct,
                                                    status = 2,//已入账
                                                    orderno = order.OrderSn
                                                };
                                                comissionLogBll.SaveForm(null, logEntity);
                                                LogHelper.AddLog("升级订单上级和顶级同一人只直接返佣：" + direct);
                                            }
                                        }
                                        else
                                        {
                                            //获取返佣规则金额
                                            DirectHelper.getJMDirect(order.LV,  agentPid.LV,  null, out direct, out indirect);
                                            //上级返佣
                                            agentPid.profit += direct;
                                            agentbll.SaveForm(agentPid.Id, agentPid);

                                            //直接返佣日志
                                            ComissionLogEntity logEntity = new ComissionLogEntity()
                                            {
                                                agent_id = agentPid.Id,
                                                agent_name = agentPid.nickname,
                                                indirect = 0,
                                                invited_agent_id = order.AgentId,
                                                phonenum = order.LV,
                                                profit = direct,
                                                status = 2,//已入账
                                                orderno = order.OrderSn
                                            };
                                            comissionLogBll.SaveForm(null, logEntity);
                                            LogHelper.AddLog("升级订单上上级为空只返上级直接返佣：" + direct);
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            LogHelper.AddLog("订单号不存在：" + orderSn);
                        }
                    }
                    else if (orderSn.Contains("FX-"))//分销销售
                    {
                        OrdersEntity order = ordersbll.GetEntityByOrderSn(orderSn);
                        if (order != null)
                        {
                            LogHelper.AddLog("微信支付分销订单FX-异步调用orderSn：" + orderSn);
                            //支付成功，修改加盟订单表
                            order.PayDate = DateTime.Now;
                            order.PayStatus = (int)PayStatus.已支付;
                            ordersbll.SaveForm(order.Id, order);

                            //同步下架
                            TelphoneLiangH5Entity tel = tlbll.GetEntity(order.TelphoneID);//根据靓号id获取靓号，修改售出状态
                            if (tel != null)
                            {
                                tel.SellMark = 1;
                                tel.SellerName = order.Host;
                            }
                            tlbll.SaveForm(tel.TelphoneID, tel);
                            LogHelper.AddLog("同步下架：" + tel.Telphone);

                            var logList = comissionLogBll.GetList("{\"orderno\":\"" + orderSn + "\"}");
                            if (logList.Count()>0)
                            {
                                foreach (var item in logList)
                                {
                                    LogHelper.AddLog(item.agent_id+"支付状态修改1已支付，佣金：" + item.profit);
                                    //返佣?不能直接返佣，需要等到开卡之后再返佣
                                    item.status = 1;//已支付，不入账
                                    comissionLogBll.SaveForm(item.id, item);
                                }
                            }
                        }
                        else
                        {
                            LogHelper.AddLog("订单号不存在：" + orderSn);
                        }
                    }
                }
            }

            string xml = string.Format(@"<xml>
<return_code><![CDATA[{0}]]></return_code>
<return_msg><![CDATA[{1}]]></return_msg>
</xml>", return_code, return_msg);

            LogHelper.AddLog("正常返回回调xml信息：" + xml);
            return Content(xml, "text/xml");

        }

        /// <summary>
        /// 服务器异步通知页面
        /// </summary>
        public ActionResult ReturnUrl()
        {
            /* 实际验证过程建议商户添加以下校验。
            1、商户需要验证该通知数据中的out_trade_no是否为商户系统中创建的订单号，
            2、判断total_amount是否确实为该订单的实际金额（即商户订单创建时的金额），
            3、校验通知中的seller_id（或者seller_email) 是否为out_trade_no这笔单据的对应的操作方（有的时候，一个商户可能有多个seller_id/seller_email）
            4、验证app_id是否为该商户本身。
            */
            Dictionary<string, string> sArray = GetRequestGet();
            if (sArray.Count != 0)
            {
                //订单号
                string orderno = sArray["out_trade_no"];
                bool flag = AlipaySignature.RSACheckV1(sArray, WeixinConfig.payKey, WeixinConfig.charset, WeixinConfig.signType, false);//支付宝公钥
                if (flag)
                {
                    ViewBag.Result = "支付成功";
                    ViewBag.icon = "success";
                    //ViewBag.display = "none";
                    LogHelper.AddLog("同步验证通过！ 订单号：" + orderno);
                }
                else
                {
                    ViewBag.Result = "未支付";
                    ViewBag.icon = "warn";
                    //ViewBag.display = "block";
                    LogHelper.AddLog("同步验证失败！ 订单号：" + orderno);
                }
                ViewBag.orderno = orderno;
                //ViewBag.id = OperatorAgentProvider.Provider.Current().Id.ToString();
            }
            return View();
        }

        /// <summary>
        /// 服务器异步通知页面
        /// 调用接口，收到异步通知后，返回小写的success，告知支付宝已收到通知：每当交易状态改变时，服务器异步通知页面就会收到支付宝发来的处理结果通知， 
        /// 程序执行完后必须打印输出success。如果商户反馈给支付宝的字符不是success这7个字符，支付宝服务器会不断重发通知，直到超过24小时22分钟。
        /// 一般情况下，25小时以内完成8次通知（通知的间隔频率一般是：4m,10m,10m,1h,2h,6h,15h）。  参考示例代码： JAVA版本：out.println("success"); PHP版本：echo "success";  
        /// .NET版本：Response.Write("success"); 注：返回success是为了告知支付宝，商户已收到异步，避免重复发送异步通知，与该笔交易的交易状态无关。
        /// </summary>
        public void AliPayNotifyUrl()
        {

            /* 实际验证过程建议商户添加以下校验。
            1、商户需要验证该通知数据中的out_trade_no是否为商户系统中创建的订单号，
            2、判断total_amount是否确实为该订单的实际金额（即商户订单创建时的金额），
            3、校验通知中的seller_id（或者seller_email) 是否为out_trade_no这笔单据的对应的操作方（有的时候，一个商户可能有多个seller_id/seller_email）
            4、验证app_id是否为该商户本身。
            */
            Dictionary<string, string> sArray = GetRequestPost();
            string sArraysStr= JsonConvert.SerializeObject(sArray);
            LogHelper.AddLog("支付宝异步调用sArraysStr：" + sArraysStr);
            if (sArray.Count != 0)
            {
                bool flag = AlipaySignature.RSACheckV1(sArray, WeixinConfig.payKey, WeixinConfig.charset, WeixinConfig.signType, false);//支付宝公钥
                string orderSn = sArray["out_trade_no"];
                if (flag)
                {
                    //注意：
                    //退款日期超过可退款期限后（如三个月可退款），支付宝系统发送该交易状态通知
                    string trade_status = Request.Form["trade_status"];
                    if (trade_status== "TRADE_SUCCESS")
                    {
                        //交易状态
                        //判断该笔订单是否在商户网站中已经做过处理
                        //如果没有做过处理，根据订单号（out_trade_no）在商户网站的订单系统中查到该笔订单的详细，并执行商户的业务程序
                        //请务必判断请求时的total_amount与通知时获取的total_fee为一致的
                        //如果有做过处理，不执行商户的业务程序
                        //订单号
                        if (orderSn.Contains("LX-"))
                        {
                            OrdersEntity order = ordersbll.GetEntityByOrderSn(orderSn);
                            if (order != null)
                            {
                                string total_amount = sArray["total_amount"];
                                if (Convert.ToDecimal(total_amount) == order.Price)
                                {
                                    LogHelper.AddLog("异步调用orderSn：" + orderSn + "total_amount:" + total_amount + "金额一致");
                                    order.PayDate = DateTime.Now;
                                    order.PayStatus = (int)PayStatus.已支付;
                                    order.Status = (int)OrderStatus.未发货;
                                    ordersbll.SaveForm(order.Id, order);

                                    //同步下架
                                    TelphoneLiangH5Entity tel = tlbll.GetEntity(order.TelphoneID);//根据靓号id获取靓号，修改售出状态
                                    if (tel != null)
                                    {
                                        tel.SellMark = 1;
                                        tel.SellerName = order.Host;
                                    }
                                    tlbll.SaveForm(tel.TelphoneID, tel);
                                    LogHelper.AddLog("同步下架：" + tel.Telphone);

                                    //头条url回调
                                    if (!string.IsNullOrEmpty(order.TouUrl))
                                    {
                                        TouTiaoApi(order.TouUrl);
                                    }
                                }
                            }
                        }
                        else if (orderSn.Contains("JM-"))
                        {
                            LogHelper.AddLog("支付宝加盟订单JM-异步调用orderSn：" + orderSn );
                            var order = ordersJMbll.GetEntityByOrderSn(orderSn);
                            if (order != null)
                            {
                                //支付成功，修改加盟订单表
                                order.PayDate = DateTime.Now;
                                order.PayStatus = (int)PayStatus.已支付;
                                ordersJMbll.SaveForm(order.Id, order);

                                //修改会员表级别
                                var agent = agentbll.GetEntityByOpenId(order.OpenId);//订单里面的OpenId，不是当前微信OpenId
                                if (agent != null)
                                {
                                    agent.LV = order.LV;
                                    agentbll.SaveForm(agent.Id, agent);

                                    decimal direct = 0;
                                    decimal indirect = 0;

                                    //上级不等于本身，才进行返佣，如果上级就是自己则为顶级
                                    if (agent.Pid != agent.Id)
                                    {
                                        Wechat_AgentEntity agentPid = agentbll.GetEntity(agent.Pid);//上级
                                        if (agentPid != null)
                                        {
                                            Wechat_AgentEntity agentPid2 = agentbll.GetEntity(agentPid.Pid);//上上级
                                            if (agentPid2 != null)
                                            {
                                                //如果父级不等于上上级，则对上上级进行返佣
                                                if (agent.Pid != agentPid2.Id)
                                                {
                                                    //获取返佣规则金额
                                                    DirectHelper.getJMDirect(order.LV,  agentPid.LV, agentPid2.LV, out direct, out indirect);
                                                    //上级返佣
                                                    agentPid.profit += direct;
                                                    agentbll.SaveForm(agentPid.Id, agentPid);

                                                    //直接返佣日志
                                                    ComissionLogEntity logEntity = new ComissionLogEntity()
                                                    {
                                                        agent_id = agentPid.Id,
                                                        agent_name = agentPid.nickname,
                                                        indirect = 0,
                                                        invited_agent_id = order.AgentId,
                                                        phonenum = order.LV,
                                                        profit = direct,
                                                        status = 2,//已入账
                                                        orderno = order.OrderSn
                                                    };
                                                    comissionLogBll.SaveForm(null, logEntity);
                                                    LogHelper.AddLog("支付宝加盟直接返佣：" + direct);

                                                    //上上级返佣
                                                    agentPid2.profit += indirect;
                                                    agentbll.SaveForm(agentPid2.Id, agentPid2);

                                                    //间接返佣日志
                                                    ComissionLogEntity logEntity2 = new ComissionLogEntity()
                                                    {
                                                        agent_id = agentPid2.Id,
                                                        agent_name = agentPid2.nickname,
                                                        indirect = 1,
                                                        invited_agent_id = order.AgentId,
                                                        phonenum = order.LV,
                                                        profit = indirect,
                                                        status = 2,//已入账
                                                        orderno = order.OrderSn
                                                    };
                                                    comissionLogBll.SaveForm(null, logEntity2);
                                                    LogHelper.AddLog("支付宝加盟直接间接：" + indirect);
                                                }
                                                else
                                                {
                                                    //获取返佣规则金额
                                                    DirectHelper.getJMDirect(order.LV, agentPid.LV, null, out direct, out indirect);
                                                    //上级返佣
                                                    agentPid.profit += direct;
                                                    agentbll.SaveForm(agentPid.Id, agentPid);

                                                    //直接返佣日志
                                                    ComissionLogEntity logEntity = new ComissionLogEntity()
                                                    {
                                                        agent_id = agentPid.Id,
                                                        agent_name = agentPid.nickname,
                                                        indirect = 0,
                                                        invited_agent_id = order.AgentId,
                                                        phonenum = order.LV,
                                                        profit = direct,
                                                        status = 2,//已入账
                                                        orderno = order.OrderSn
                                                    };
                                                    comissionLogBll.SaveForm(null, logEntity);
                                                    LogHelper.AddLog("支付宝加盟上级和顶级同一人只直接返佣：" + direct);
                                                }
                                            }
                                            else
                                            {
                                                //获取返佣规则金额
                                                DirectHelper.getJMDirect(order.LV, agentPid.LV, null, out direct, out indirect);
                                                //上级返佣
                                                agentPid.profit += direct;
                                                agentbll.SaveForm(agentPid.Id, agentPid);

                                                //直接返佣日志
                                                ComissionLogEntity logEntity = new ComissionLogEntity()
                                                {
                                                    agent_id = agentPid.Id,
                                                    agent_name = agentPid.nickname,
                                                    indirect = 0,
                                                    invited_agent_id = order.AgentId,
                                                    phonenum = order.LV,
                                                    profit = direct,
                                                    status = 2,//已入账
                                                    orderno = order.OrderSn
                                                };
                                                comissionLogBll.SaveForm(null, logEntity);
                                                LogHelper.AddLog("支付宝加盟上上级为空只返上级直接返佣：" + direct);
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                LogHelper.AddLog("订单号不存在：" + orderSn);
                            }
                        }
                        else if (orderSn.Contains("FX-"))
                        {
                            OrdersEntity order = ordersbll.GetEntityByOrderSn(orderSn);
                            if (order != null)
                            {
                                string total_amount = sArray["total_amount"];
                                if (Convert.ToDecimal(total_amount) == order.Price)
                                {
                                    LogHelper.AddLog("支付宝分销订单FX-异步调用orderSn：" + orderSn + "total_amount:" + total_amount + "金额一致");
                                    order.PayDate = DateTime.Now;
                                    order.PayStatus = (int)PayStatus.已支付;
                                    order.Status = (int)OrderStatus.未发货;
                                    ordersbll.SaveForm(order.Id, order);

                                    //同步下架
                                    TelphoneLiangH5Entity tel = tlbll.GetEntity(order.TelphoneID);//根据靓号id获取靓号，修改售出状态
                                    if (tel != null)
                                    {
                                        tel.SellMark = 1;
                                        tel.SellerName = order.Host;
                                    }
                                    tlbll.SaveForm(tel.TelphoneID, tel);
                                    LogHelper.AddLog("同步下架：" + tel.Telphone);


                                    var logList = comissionLogBll.GetList("{\"orderno\":\"" + orderSn + "\"}");
                                    if (logList.Count() > 0)
                                    {
                                        foreach (var item in logList)
                                        {
                                            //返佣?不能直接返佣，需要等到开卡之后再返佣
                                            item.status = 1;//已支付，不入账
                                            comissionLogBll.SaveForm(item.id, item);
                                        }
                                    }
                                }
                            }
                        }
                    }

                    LogHelper.AddLog("异步调用success,订单号："+ orderSn);
                    Response.Write("success");
                }
                else
                {
                    LogHelper.AddLog("异步调用fail,订单号：" + orderSn);
                    Response.Write("fail");
                }
            }
        }


        public Dictionary<string, string> GetRequestPost()
        {
            int i = 0;
            Dictionary<string, string> sArray = new Dictionary<string, string>();
            NameValueCollection coll;
            coll = Request.Form;
            String[] requestItem = coll.AllKeys;
            for (i = 0; i < requestItem.Length; i++)
            {
                sArray.Add(requestItem[i], Request.Form[requestItem[i]]);
            }
            return sArray;

        }

        public Dictionary<string, string> GetRequestGet()
        {
            int i = 0;
            Dictionary<string, string> sArray = new Dictionary<string, string>();
            NameValueCollection coll;
            //coll = Request.Form;
            coll = Request.QueryString;
            String[] requestItem = coll.AllKeys;
            for (i = 0; i < requestItem.Length; i++)
            {
                sArray.Add(requestItem[i], Request.QueryString[requestItem[i]]);
            }
            return sArray;

        }


        /// <summary>
        /// 提交到头条API方式
        /// </summary>
        /// <param name="touUrl"></param>
        /// <returns></returns>
        public void TouTiaoApi(string touUrl)
        {
            string result = HttpUtility.UrlEncode(touUrl);
            HttpClientHelper.Get(@"https://ad.oceanengine.com/track/activate/?link=" + result+ "&event_type=2");
        }
    }
}
//{"gmt_create":"2020-05-28 09:48:11","charset":"utf-8","seller_email":"359085234@qq.com","subject":"587259",
//"sign":"L7bDDq7flA97dSuSHn0xky+m0Qg3N2iQ1Yn5+QnjlRED9f88TSshZZcVC67jIRA3BL9vXU7kVsIWEdAiL5Gc33vxeJ5s/VQUJKuCrTTgEJYy5tJXdr09w358rwFO8oV7Q7S/cEFSC7HWyb0fCqKK+y/Ug69dV9AoWsD3bDNsJdJP5f33cFZJKZpP0/OgnR/2a18NHvJaycYafJrW2IoelCyiXL7iXNnda5iMSwEjF2mv+keOXODZfUTR4MG1EP63R2hKgx8phx2iypiqXFXi36pLeLB8AXWsoBRpd91nH9+JcNCsAkDnErgg6KnZyN9pG39cCHQ46adk0CGt3ke5Tg==",
//"body":"支付宝购买靓号","buyer_id":"2088302481469674","invoice_amount":"399.00","notify_id":"2020052800222094812069671446541835",
//"fund_bill_list":"[{\"amount\":\"399.00\",\"fundChannel\":\"ALIPAYACCOUNT\"}]","notify_type":"trade_status_sync","trade_status":
//"TRADE_SUCCESS","receipt_amount":"399.00","buyer_pay_amount":"399.00","app_id":"2021001122647841",
//"sign_type":"RSA2","seller_id":"2088731550772126","gmt_payment":"2020-05-28 09:48:12",
//"notify_time":"2020-05-28 13:15:16","version":"1.0","out_trade_no":"LX-20200528094756",
//"total_amount":"399.00","trade_no":"2020052822001469671458153210","auth_app_id":"2021001122647841","buyer_logon_id":"182***@qq.com","point_amount":"0.00"}