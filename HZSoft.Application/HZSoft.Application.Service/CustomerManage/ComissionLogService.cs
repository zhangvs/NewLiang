using HZSoft.Application.Code;
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
    /// 日 期：2020-10-08 14:07
    /// 描 述：佣金表
    /// </summary>
    public class ComissionLogService : RepositoryFactory<ComissionLogEntity>, ComissionLogIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public IEnumerable<ComissionLogEntity> GetPageList(Pagination pagination, string queryJson)
        {
            string strSql = $"select * from ComissionLog where profit IS NOT NULL ";
            var expression = LinqExtensions.True<ComissionLogEntity>();
            var queryParam = queryJson.ToJObject();
            //单据日期
            if (!queryParam["StartTime"].IsEmpty() && !queryParam["EndTime"].IsEmpty())
            {
                DateTime startTime = queryParam["StartTime"].ToDate();
                DateTime endTime = queryParam["EndTime"].ToDate().AddDays(1);
                strSql += " and created_at BETWEEN '" + startTime + "' and '" + endTime + "'";
            }
            //代理
            if (!queryParam["agent_id"].IsEmpty())
            {
                string agent_id = queryParam["agent_id"].ToString();
                strSql += " and agent_id = " + agent_id;
            }
            else
            {
                if (!OperatorProvider.Provider.Current().IsSystem && OperatorProvider.Provider.Current().UserId != "3303254b-7cd3-4a25-abd3-bb2542a08df9")//龙哥可以查看到所有号码
                {
                    string companyId = OperatorProvider.Provider.Current().CompanyId;
                    strSql += " and agent_id in (SELECT id FROM Wechat_Agent WHERE OrganizeId='" + companyId + "')";
                }
            }
            //代理
            if (!queryParam["agent_name"].IsEmpty())
            {
                string agent_name = queryParam["agent_name"].ToString();
                strSql += " and agent_name = '%" + agent_name + "%'";
            }
            //单号id
            if (!queryParam["ordernoid"].IsEmpty())
            {
                string ordernoid = queryParam["ordernoid"].ToString();
                strSql += " and ordernoid = " + ordernoid;
            }
            //单号
            if (!queryParam["orderno"].IsEmpty())
            {
                string orderno = queryParam["orderno"].ToString();
                strSql += " and orderno = '" + orderno + "'";
            }
            //加盟升级
            if (!queryParam["invited_agent_id"].IsEmpty())
            {
                string invited_agent_id = queryParam["invited_agent_id"].ToString();
                strSql += " and invited_agent_id = " + invited_agent_id;
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
        public IEnumerable<ComissionLogEntity> GetList(string queryJson)
        {
            string strSql = $"select * from ComissionLog where  profit IS NOT NULL ";
            var expression = LinqExtensions.True<ComissionLogEntity>();
            var queryParam = queryJson.ToJObject();
            //单号
            if (!queryParam["orderno"].IsEmpty())
            {
                string orderno = queryParam["orderno"].ToString();
                strSql += " and orderno = '" + orderno + "'";
            }
            //状态
            if (!queryParam["status"].IsEmpty())
            {
                string status = queryParam["status"].ToString();
                strSql += " and status = " + status;
            }
            return this.BaseRepository().FindList(strSql.ToString());
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public ComissionLogEntity GetEntity(int? keyValue)
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
        public void SaveForm(int? keyValue, ComissionLogEntity entity)
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
        #endregion
    }
}
