using HZSoft.Application.Entity.BaseManage;
using HZSoft.Util.WebControl;
using System.Collections.Generic;

namespace HZSoft.Application.IService.BaseManage
{
    /// <summary>
    /// 版 本 6.1
    /// 
    /// 创 建：超级管理员
    /// 日 期：2020-09-29 17:02
    /// 描 述：加盟代理
    /// </summary>
    public interface Wechat_AgentIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        IEnumerable<Wechat_AgentEntity> GetPageList(Pagination pagination, string queryJson);
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<Wechat_AgentEntity> GetList(string queryJson);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        Wechat_AgentEntity GetEntity(int? keyValue);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        Wechat_AgentEntity GetEntityByOpenId(string OpenId);
        IEnumerable<Wechat_AgentEntity> GetSumItem(int? pid);
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        void RemoveForm(int? keyValue);
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        void SaveForm(int? keyValue, Wechat_AgentEntity entity);
        /// <summary>
        /// 浏览量自增
        /// </summary>
        void SeeCountAdd(int? keyValue);
        /// <summary>
        /// 浮动比例调整
        /// </summary>
        void FuDongUpdate(int? keyValue, int? fuDong);


        #endregion
    }
}
