using System;
using HZSoft.Application.Code;

namespace HZSoft.Application.Entity.CustomerManage
{
    /// <summary>
    /// 版 本
    /// 
    /// 创 建：超级管理员
    /// 日 期：2020-10-04 19:02
    /// 描 述：卖号返佣规则
    /// </summary>
    public class Client_TelFeeEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 主键
        /// </summary>
        /// <returns></returns>
        public int? Id { get; set; }
        /// <summary>
        /// 大于
        /// </summary>
        /// <returns></returns>
        public decimal? Min { get; set; }
        /// <summary>
        /// 小于
        /// </summary>
        /// <returns></returns>
        public decimal? Max { get; set; }
        /// <summary>
        /// 普通直接返佣
        /// </summary>
        /// <returns></returns>
        public decimal? PuZ { get; set; }
        /// <summary>
        /// 黄金直接返佣
        /// </summary>
        /// <returns></returns>
        public decimal? HuangZ { get; set; }
        /// <summary>
        /// 钻石直接返佣
        /// </summary>
        /// <returns></returns>
        public decimal? ZuanZ { get; set; }
        /// <summary>
        /// 普通间接返佣
        /// </summary>
        /// <returns></returns>
        public decimal? PuJ { get; set; }
        /// <summary>
        /// 黄金间接返佣
        /// </summary>
        /// <returns></returns>
        public decimal? HuangJ { get; set; }
        /// <summary>
        /// 钻石间接返佣
        /// </summary>
        /// <returns></returns>
        public decimal? ZuanJ { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {

        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="keyValue"></param>
        public override void Modify(int? keyValue)
        {
            this.Id = keyValue;
                                            }
        #endregion
    }
}