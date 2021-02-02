using HZSoft.Application.Entity.CustomerManage;
using HZSoft.Application.IService.CustomerManage;
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
    /// 日 期：2020-10-03 16:08
    /// 描 述：加盟订单
    /// </summary>
    public class OrdersJMService : RepositoryFactory<OrdersJMEntity>, OrdersJMIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public IEnumerable<OrdersJMEntity> GetPageList(Pagination pagination, string queryJson)
        {
            string strSql = $"select * from OrdersJM where 1=1 ";
            var expression = LinqExtensions.True<OrdersJMEntity>();
            var queryParam = queryJson.ToJObject();
            //单据日期
            if (!queryParam["StartTime"].IsEmpty() && !queryParam["EndTime"].IsEmpty())
            {
                DateTime startTime = queryParam["StartTime"].ToDate();
                DateTime endTime = queryParam["EndTime"].ToDate().AddDays(1);
                strSql += " and created_at BETWEEN '" + startTime + "' and '" + endTime + "'";
            }
            //代理
            if (!queryParam["agent_name"].IsEmpty())
            {
                string agent_name = queryParam["agent_name"].ToString();
                strSql += " and nickname = '%" + agent_name + "%'";
            }
            //代理id
            if (!queryParam["agent_id"].IsEmpty())
            {
                string agent_id = queryParam["agent_id"].ToString();
                strSql += " and agent_id = " + agent_id;
            }
            //单号
            if (!queryParam["orderno"].IsEmpty())
            {
                string orderno = queryParam["orderno"].ToString();
                strSql += " and OrderSn = '" + orderno + "'";
            }
            //单号
            if (!queryParam["OrderSn"].IsEmpty())
            {
                string orderno = queryParam["OrderSn"].ToString();
                strSql += " and OrderSn = '" + orderno + "'";
            }
            return this.BaseRepository().FindList(strSql.ToString(), pagination);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<OrdersJMEntity> GetList(string queryJson)
        {
            string strSql = $"select * from OrdersJM where 1=1 ";
            var expression = LinqExtensions.True<OrdersJMEntity>();
            var queryParam = queryJson.ToJObject();
            //单据日期
            if (!queryParam["StartTime"].IsEmpty() && !queryParam["EndTime"].IsEmpty())
            {
                DateTime startTime = queryParam["StartTime"].ToDate();
                DateTime endTime = queryParam["EndTime"].ToDate().AddDays(1);
                strSql += " and created_at BETWEEN '" + startTime + "' and '" + endTime + "'";
            }
            //代理
            if (!queryParam["agent_name"].IsEmpty())
            {
                string agent_name = queryParam["agent_name"].ToString();
                strSql += " and nickname = '%" + agent_name + "%'";
            }
            //代理id
            if (!queryParam["agent_id"].IsEmpty())
            {
                string agent_id = queryParam["agent_id"].ToString();
                strSql += " and agent_id = " + agent_id;
            }
            //单号
            if (!queryParam["orderno"].IsEmpty())
            {
                string orderno = queryParam["orderno"].ToString();
                strSql += " and OrderSn = '" + orderno + "'";
            }
            //单号
            if (!queryParam["OrderSn"].IsEmpty())
            {
                string orderno = queryParam["OrderSn"].ToString();
                strSql += " and OrderSn = '" + orderno + "'";
            }
            return this.BaseRepository().FindList(strSql.ToString());
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public OrdersJMEntity GetEntity(int? keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="OrderSn">主键值</param>
        /// <returns></returns>
        public OrdersJMEntity GetEntityByOrderSn(string OrderSn)
        {
            return this.BaseRepository().FindEntity(t => t.OrderSn == OrderSn);
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
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public OrdersJMEntity SaveForm(int? keyValue, OrdersJMEntity entity)
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
            return entity;
        }
        #endregion
    }
}
