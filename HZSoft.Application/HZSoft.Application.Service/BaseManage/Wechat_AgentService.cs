using HZSoft.Application.Code;
using HZSoft.Application.Entity.BaseManage;
using HZSoft.Application.IService.BaseManage;
using HZSoft.Data.Repository;
using HZSoft.Util;
using HZSoft.Util.Extension;
using HZSoft.Util.WebControl;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HZSoft.Application.Service.BaseManage
{
    /// <summary>
    /// 版 本 6.1
    /// 
    /// 创 建：超级管理员
    /// 日 期：2020-09-29 17:02
    /// 描 述：加盟代理
    /// </summary>
    public class Wechat_AgentService : RepositoryFactory<Wechat_AgentEntity>, Wechat_AgentIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public IEnumerable<Wechat_AgentEntity> GetPageList(Pagination pagination, string queryJson)
        {
            string strSql = "select * from Wechat_Agent where DeleteMark <> 1 ";
            var expression = LinqExtensions.True<Wechat_AgentEntity>();
            var queryParam = queryJson.ToJObject();
            //成立日期
            if (!queryParam["StartTime"].IsEmpty() && !queryParam["EndTime"].IsEmpty())
            {
                DateTime startTime = queryParam["StartTime"].ToDate();
                DateTime endTime = queryParam["EndTime"].ToDate().AddDays(1);
                strSql += " and CreateDate >= '" + startTime + "' and CreateDate < '" + endTime + "'";
            }
            //编号
            if (!queryParam["Id"].IsEmpty())
            {
                string Id = queryParam["Id"].ToString();
                strSql += " and Id = " + Id ;
            }
            //微信昵称
            if (!queryParam["nickname"].IsEmpty())
            {
                string nickname = queryParam["nickname"].ToString();
                strSql += " and nickname like '%" + nickname + "%'";
            }
            //手机
            if (!queryParam["contact"].IsEmpty())
            {
                string contact = queryParam["contact"].ToString();
                strSql += " and contact like '%" + contact + "%'";
            }
            //支付宝
            if (!queryParam["alipay"].IsEmpty())
            {
                string alipay = queryParam["alipay"].ToString();
                strSql += " and alipay like '%" + alipay + "%'";
            }
            //真实姓名
            if (!queryParam["realname"].IsEmpty())
            {
                string realname = queryParam["realname"].ToString();
                strSql += " and realname like '%" + realname + "%'";
            }
            //级别
            if (!queryParam["LV"].IsEmpty())
            {
                string LV = queryParam["LV"].ToString();
                strSql += " and LV  = '" + LV+"'";
            }
            if (!OperatorProvider.Provider.Current().IsSystem && OperatorProvider.Provider.Current().UserId != "3303254b-7cd3-4a25-abd3-bb2542a08df9")//龙哥可以查看到所有号码
            {
                string OrganizeId = OperatorProvider.Provider.Current().CompanyId;
                strSql += " and OrganizeId  = '" + OrganizeId + "'";
            }
            return this.BaseRepository().FindList(strSql.ToString(), pagination);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<Wechat_AgentEntity> GetList(string queryJson)
        {
            string strSql = "select * from Wechat_Agent where DeleteMark <> 1 ";
            var expression = LinqExtensions.True<Wechat_AgentEntity>();
            var queryParam = queryJson.ToJObject();
            //成立日期
            if (!queryParam["StartTime"].IsEmpty() && !queryParam["EndTime"].IsEmpty())
            {
                DateTime startTime = queryParam["StartTime"].ToDate();
                DateTime endTime = queryParam["EndTime"].ToDate().AddDays(1);
                strSql += " and CreateDate >= '" + startTime + "' and CreateDate < '" + endTime + "'";
            }
            //微信标识
            if (!queryParam["OpenId"].IsEmpty())
            {
                string OpenId = queryParam["OpenId"].ToString();
                strSql += " and OpenId = '" + OpenId + "'";
            }
            //微信昵称
            if (!queryParam["NickName"].IsEmpty())
            {
                string NickName = queryParam["NickName"].ToString();
                strSql += " and NickName like '%" + NickName + "%'";
            }
            //手机
            if (!queryParam["Tel"].IsEmpty())
            {
                string Tel = queryParam["Tel"].ToString();
                strSql += " and Tel like '%" + Tel + "%'";
            }
            //支付宝
            if (!queryParam["Zfb"].IsEmpty())
            {
                string Zfb = queryParam["Zfb"].ToString();
                strSql += " and Zfb like '%" + Zfb + "%'";
            }
            //真实姓名
            if (!queryParam["RealName"].IsEmpty())
            {
                string RealName = queryParam["RealName"].ToString();
                strSql += " and RealName like '%" + RealName + "%'";
            }
            //级别
            if (!queryParam["LV"].IsEmpty())
            {
                int LV = queryParam["LV"].ToInt();
                strSql += " and LV  = " + LV;
            }
            //父
            if (!queryParam["Pid"].IsEmpty())
            {
                int Pid = queryParam["Pid"].ToInt();
                strSql += " and Pid  = " + Pid;
            }
            return this.BaseRepository().FindList(strSql.ToString());
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public Wechat_AgentEntity GetEntity(int? keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="OpenId">主键值</param>
        /// <returns></returns>
        public Wechat_AgentEntity GetEntityByOpenId(string OpenId)
        {
            return this.BaseRepository().FindEntity(t => t.OpenId == OpenId);
        }
        /// <summary>
        /// 获取代理汇总，佣金，数量，下级lv
        /// </summary>
        /// <param name="pid">主键值</param>
        /// <returns></returns>
        public IEnumerable<Wechat_AgentEntity> GetSumItem(int? pid)
        {
            return this.BaseRepository().FindList("select sum(profit) childprofit,count(*) childcount,lv from Wechat_Agent where DeleteMark <> 1 AND pid=" + pid + " GROUP BY lv");
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
        public void SaveForm(int? keyValue, Wechat_AgentEntity entity)
        {
            if (!string.IsNullOrEmpty(keyValue.ToString()))
            {
                entity.Modify(keyValue);
                this.BaseRepository().Update(entity);
            }
            else
            {
                //创建普通代理，由上级连接打开的，上级普通代理数量+1
                if (!string.IsNullOrEmpty(entity.Pid.ToString()))
                {
                    var pidEntity = GetEntity(entity.Pid);
                    if (pidEntity!=null)
                    {
                        this.BaseRepository().Update(pidEntity);
                        entity.Category = pidEntity.Category + 1;//代理级别为上级的级别+1
                        entity.OrganizeId = pidEntity.OrganizeId;//设置机构id为上级机构id
                    }
                    else
                    {
                        entity.Category = 0;//代理级别，顶级为0级
                    }
                }

                entity.Create();
                this.BaseRepository().Insert(entity);
            }
        }
        #endregion
    }
}
