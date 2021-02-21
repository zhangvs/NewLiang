using HZSoft.Application.Code;
using HZSoft.Application.Entity.BaseManage;
using HZSoft.Application.Entity.CustomerManage;
using HZSoft.Application.IService.CustomerManage;
using HZSoft.Application.IService.SystemManage;
using HZSoft.Application.Service.SystemManage;
using HZSoft.Data.Repository;
using HZSoft.Util;
using HZSoft.Util.Extension;
using HZSoft.Util.WebControl;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HZSoft.Application.Service.CustomerManage
{
    /// <summary>
    /// 版 本 6.1
    /// 
    /// 创 建：超级管理员
    /// 日 期：2020-04-16 09:07
    /// 描 述：订单表
    /// </summary>
    public class OrdersService : RepositoryFactory<OrdersEntity>, OrdersIService
    {
        private ICodeRuleService coderuleService = new CodeRuleService();

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public IEnumerable<OrdersEntity> GetPageList(Pagination pagination, string queryJson)
        {
            string strSql = $"select * from Orders where (DeleteMark <> 1 or PayStatus = 1) ";
            var expression = LinqExtensions.True<OrdersEntity>();
            var queryParam = queryJson.ToJObject();
            //成立日期
            if (!queryParam["StartTime"].IsEmpty() && !queryParam["EndTime"].IsEmpty())
            {
                DateTime startTime = queryParam["StartTime"].ToDate();
                DateTime endTime = queryParam["EndTime"].ToDate().AddDays(1);
                strSql += " and CreateDate >= '" + startTime + "' and CreateDate < '" + endTime + "'";
            }
            //价格大于
            if (!queryParam["pricef"].IsEmpty())
            {
                string pricef = queryParam["pricef"].ToString();
                strSql += " and Price >= " + pricef;
            }
            //价格小于
            if (!queryParam["pricet"].IsEmpty())
            {
                string pricet = queryParam["pricet"].ToString();
                strSql += " and Price <= " + pricet;
            }

            //单号
            if (!queryParam["OrderSn"].IsEmpty())
            {
                string OrderSn = queryParam["OrderSn"].ToString();
                strSql += " and OrderSn like '%" + OrderSn + "%'";
            }
            //靓号
            if (!queryParam["Tel"].IsEmpty())
            {
                string Tel = queryParam["Tel"].ToString();
                strSql += " and Tel like '%" + Tel + "%'";
            }
            //域名或昵称
            if (!queryParam["Host"].IsEmpty())
            {
                string Host = queryParam["Host"].ToString();
                strSql += " and Host like '%" + Host + "%'";
            }
            //收件人
            if (!queryParam["Receiver"].IsEmpty())
            {
                string Receiver = queryParam["Receiver"].ToString();
                strSql += " and Receiver like '%" + Receiver + "%'";
            }
            //联系电话
            if (!queryParam["ContactTel"].IsEmpty())
            {
                string ContactTel = queryParam["ContactTel"].ToString();
                strSql += " and ContactTel like '%" + ContactTel + "%'";
            }
            //订单状态
            if (!queryParam["Status"].IsEmpty())
            {
                int Status = queryParam["Status"].ToInt();
                strSql += " and Status  = " + Status;
            }
            //支付状态
            if (!queryParam["PayStatus"].IsEmpty())
            {
                int PayStatus = queryParam["PayStatus"].ToInt();
                strSql += " and PayStatus  = " + PayStatus;
            }
            //代理id
            if (!queryParam["AgentId"].IsEmpty())
            {
                int AgentId = queryParam["AgentId"].ToInt();
                strSql += " and AgentId  = " + AgentId;
            }
            //默认只查询当前机构下的代理订单
            if (!OperatorProvider.Provider.Current().IsSystem && OperatorProvider.Provider.Current().UserId != "3303254b-7cd3-4a25-abd3-bb2542a08df9")//龙哥可以查看到所有号码
            {
                string companyId = OperatorProvider.Provider.Current().CompanyId;
                strSql += " and AgentId in (SELECT id FROM Wechat_Agent WHERE OrganizeId='" + companyId + "')";
            }
            return this.BaseRepository().FindList(strSql.ToString(), pagination);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<OrdersEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public OrdersEntity GetEntity(int? keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="OrderSn">主键值</param>
        /// <returns></returns>
        public OrdersEntity GetEntityByOrderSn(string OrderSn)
        {
            return this.BaseRepository().FindEntity(t => t.OrderSn == OrderSn);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="tel">主键值</param>
        /// <returns></returns>
        public OrdersEntity GetEntityByTel(string tel)
        {
            return this.BaseRepository().FindEntity(t => t.Tel == tel);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="tel">购买号码</param>
        /// <param name="contactTel">联系电话</param>
        /// <returns></returns>
        public OrdersEntity GetEntityByContactTel(string tel, string contactTel)
        {
            return this.BaseRepository().FindEntity(t => t.Tel == tel && t.ContactTel == contactTel);
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        public void RemoveForm(int? keyValue)
        {
            this.BaseRepository().Delete(keyValue);
            IRepository db = new RepositoryFactory().BaseRepository().BeginTrans();
            //同步删除佣金日志表
            var list = db.FindList<ComissionLogEntity>(t => t.orderid == keyValue);
            if (list.Count() > 0)
            {
                foreach (var item in list)
                {
                    db.Update<ComissionLogEntity>(item);
                }
                db.Commit();
            }

        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveForm(int? keyValue, OrdersEntity entity)
        {
            if (!string.IsNullOrEmpty(keyValue.ToString()))
            {
                entity.Modify(keyValue);
                this.BaseRepository().Update(entity);
            }
            else
            {
                entity.Create();
                this.BaseRepository().Insert(entity);
            }
        }


        /// <summary>
        /// 保存表单（新增）
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public OrdersEntity SaveForm(OrdersEntity entity)
        {
            IRepository db = new RepositoryFactory().BaseRepository().BeginTrans();
            var list= db.FindList<OrdersEntity>(t => t.Tel == entity.Tel && (t.ContactTel == entity.ContactTel || t.ContactTel == null) && t.Status==0);
            if (list.Count()>0)
            {
                foreach (var item in list)
                {
                    item.DeleteMark = 1;
                    db.Update<OrdersEntity>(item);
                    LogHelper.AddLog("删除未付款订单：" + item.OrderSn);
                }
                //db.Delete<OrdersEntity>(t => t.Tel == entity.Tel && t.ContactTel == entity.ContactTel && t.Status == 0);
                db.Commit();
            }

            entity.Create();
            if (string.IsNullOrEmpty(entity.OrderSn))
            {
                //jsapi会提前生成订单编号，直接用提生成的，保持提交微信的一致
                entity.OrderSn = string.Format("{0}{1}", "LX-", DateTime.Now.ToString("yyyyMMddHHmmss"));//,TenPayV3Util.BuildRandomStr(6)
            }
            this.BaseRepository().Insert(entity);
            return entity;
        }




        /// <summary>
        /// 发货
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveSendForm(int? keyValue, OrdersEntity entity)
        {
            if (!string.IsNullOrEmpty(keyValue.ToString()))
            {
                entity.Modify(keyValue);
                entity.Status = 2;//发货
                entity.DeliveryName = OperatorProvider.Provider.Current().UserName;
                entity.DeliveryDate = DateTime.Now;
                this.BaseRepository().Update(entity);
            }
        }
        /// <summary>
        /// 开卡
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public void UpdateSendState(int? keyValue)
        {
            IRepository db = new RepositoryFactory().BaseRepository().BeginTrans();
            try
            {
                if (!string.IsNullOrEmpty(keyValue.ToString()))
                {
                    OrdersEntity entity = GetEntity(keyValue);
                    entity.Modify(keyValue);
                    entity.Status = 3;//开发，订单已完成
                    this.BaseRepository().Update(entity);

                    //如果是分销订单，进行佣金的入账操作
                    if (entity.OrderSn.Contains("FX-"))
                    {
                        var list = db.FindList<ComissionLogEntity>(t => t.orderno == entity.OrderSn);
                        if (list.Count() > 0)
                        {
                            //需要等到开卡之后再返佣入账
                            foreach (var item in list)
                            {
                                var agent = db.FindEntity<Wechat_AgentEntity>(t => t.Id == item.agent_id);
                                if (agent != null)
                                {
                                    agent.profit += item.profit;//给代理加上佣金
                                    agent.SellCount += entity.Price;//给代理加上销量
                                    item.status = 2;//已入账
                                    db.Update<Wechat_AgentEntity>(agent);//更新代理表
                                    db.Update<ComissionLogEntity>(item);//更新佣金日志表
                                }
                            }
                            db.Commit();
                        }
                    }

                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
