using HZSoft.Application.Code;
using HZSoft.Application.Entity.BaseManage;
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
    /// 日 期：2020-10-16 15:40
    /// 描 述：提现记录
    /// </summary>
    public class WithdrawLogService : RepositoryFactory<WithdrawLogEntity>, WithdrawLogIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public IEnumerable<WithdrawLogEntity> GetPageList(Pagination pagination, string queryJson)
        {
            string strSql = $"select * from WithdrawLog where 1=1 ";//id, agent_id, agent_name, CONVERT(VARCHAR(100),date,111) date, amount, status, img, ModifyDate
            var expression = LinqExtensions.True<OrdersJMEntity>();
            var queryParam = queryJson.ToJObject();
            //单据日期
            if (!queryParam["StartTime"].IsEmpty() && !queryParam["EndTime"].IsEmpty())
            {
                DateTime startTime = queryParam["StartTime"].ToDate();
                DateTime endTime = queryParam["EndTime"].ToDate().AddDays(1);
                strSql += " and date BETWEEN '" + startTime + "' and '" + endTime + "'";
            }
            //代理
            if (!queryParam["agent_name"].IsEmpty())
            {
                string agent_name = queryParam["agent_name"].ToString();
                strSql += " and agent_name = '%" + agent_name+"%'";
            }
            //代理id
            if (!queryParam["agent_id"].IsEmpty())
            {
                string agent_id = queryParam["agent_id"].ToString();
                strSql += " and agent_id = " + agent_id;
            }
            
            //状态
            if (!queryParam["status"].IsEmpty())
            {
                string status = queryParam["status"].ToString();
                strSql += " and status = " + status;
            }
            return this.BaseRepository().FindList(strSql.ToString(), pagination);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<WithdrawLogEntity> GetList(string queryJson)
        {
            string strSql = $"select * from WithdrawLog where 1=1 ";
            var expression = LinqExtensions.True<OrdersJMEntity>();
            var queryParam = queryJson.ToJObject();
            //单据日期
            if (!queryParam["StartTime"].IsEmpty() && !queryParam["EndTime"].IsEmpty())
            {
                DateTime startTime = queryParam["StartTime"].ToDate();
                DateTime endTime = queryParam["EndTime"].ToDate().AddDays(1);
                strSql += " and date BETWEEN '" + startTime + "' and '" + endTime + "'";
            }
            //代理
            if (!queryParam["agent_id"].IsEmpty())
            {
                string agent_id = queryParam["agent_id"].ToString();
                strSql += " and agent_id = " + agent_id;
            }
            //状态
            if (!queryParam["status"].IsEmpty())
            {
                string status = queryParam["status"].ToString();
                strSql += " and status = " + status;
            }
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public WithdrawLogEntity GetEntity(int? keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
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
        public void SaveForm(int? keyValue, WithdrawLogEntity entity)
        {
            if (!string.IsNullOrEmpty(keyValue.ToString()))
            {
                var oldEntity = GetEntity(keyValue);
                entity.Modify(keyValue);
                this.BaseRepository().Update(entity);

                //提现判断[{id: 0, title: '审核中'}, {id: 1, title: '已转账'}, {id: 2, title: '已拒绝'}],
                if (entity.status == 1 && oldEntity.status != 1)
                {
                    IRepository db = new RepositoryFactory().BaseRepository().BeginTrans();
                    var agent = db.FindEntity<Wechat_AgentEntity>(t => t.Id == entity.agent_id);
                    if (agent != null)
                    {
                        agent.Cashout -= entity.amount;//提现中-
                        agent.Cashouted += entity.amount;//提现结束+
                        db.Update<Wechat_AgentEntity>(agent);//更新代理表
                        db.Commit();
                        //后台不需要操作代理缓存，修改缓存12小时为1小时
                        //OperatorAgentProvider.Provider.Current().profit = agent.profit;//修改当前登录者缓存
                        //OperatorAgentProvider.Provider.Current().Cashout = agent.Cashout;//修改当前登录者缓存
                    }
                }
            }
            else
            {
                entity.Create();
                this.BaseRepository().Insert(entity);
            }
        }
        #endregion
    }
}
