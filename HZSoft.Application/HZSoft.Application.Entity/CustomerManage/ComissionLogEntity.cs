using System;
using HZSoft.Application.Code;

namespace HZSoft.Application.Entity.CustomerManage
{
    /// <summary>
    /// 版 本
    /// 
    /// 创 建：超级管理员
    /// 日 期：2020-10-08 14:07
    /// 描 述：佣金表
    /// </summary>
    public class ComissionLogEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 主键
        /// </summary>
        /// <returns></returns>
        public int? id { get; set; }
        /// <summary>
        /// 代理id
        /// </summary>
        /// <returns></returns>
        public int? agent_id { get; set; }
        /// <summary>
        /// 代理名称
        /// </summary>
        /// <returns></returns>
        public string agent_name { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        /// <returns></returns>
        public DateTime? created_at { get; set; }
        /// <summary>
        /// 0直接销售，1间接销售，2代售，3本金
        /// </summary>
        /// <returns></returns>
        public int? indirect { get; set; }
        /// <summary>
        /// 被邀请代理Id
        /// </summary>
        /// <returns></returns>
        public int? invited_agent_id { get; set; }
        /// <summary>
        /// 号码
        /// </summary>
        /// <returns></returns>
        public string phonenum { get; set; }
        /// <summary>
        /// 佣金
        /// </summary>
        /// <returns></returns>
        public decimal? profit { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        /// <returns></returns>
        public int? status { get; set; }
        /// <summary>
        /// 订单编号
        /// </summary>
        /// <returns></returns>
        public string orderno { get; set; }
        /// <summary>
        /// 订单编号id
        /// </summary>
        /// <returns></returns>
        public int? orderid { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        /// <returns></returns>
        public DateTime? ModifyDate { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.created_at = DateTime.Now;
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="keyValue"></param>
        public override void Modify(int? keyValue)
        {
            this.id = keyValue;
            this.ModifyDate = DateTime.Now;
        }
        #endregion
    }
}