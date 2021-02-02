using System;
using HZSoft.Application.Code;

namespace HZSoft.Application.Entity.CustomerManage
{
    /// <summary>
    /// 版 本
    /// 
    /// 创 建：超级管理员
    /// 日 期：2020-10-16 15:40
    /// 描 述：提现记录
    /// </summary>
    public class WithdrawLogEntity : BaseEntity
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
        /// 提现日期
        /// </summary>
        /// <returns></returns>
        public DateTime? date { get; set; }
        /// <summary>
        /// 提现金额
        /// </summary>
        /// <returns></returns>
        public decimal? amount { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        /// <returns></returns>
        public int? status { get; set; }
        /// <summary>
        /// 凭证
        /// </summary>
        /// <returns></returns>
        public string img { get; set; }
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
            this.date = DateTime.Now;
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