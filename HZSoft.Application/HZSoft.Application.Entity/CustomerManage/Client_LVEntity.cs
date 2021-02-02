using System;
using HZSoft.Application.Code;

namespace HZSoft.Application.Entity.CustomerManage
{
    /// <summary>
    /// 版 本
    /// 
    /// 创 建：超级管理员
    /// 日 期：2020-10-04 10:18
    /// 描 述：会员规则
    /// </summary>
    public class Client_LVEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 主键
        /// </summary>
        /// <returns></returns>
        public int? Id { get; set; }
        /// <summary>
        /// 级别
        /// </summary>
        /// <returns></returns>
        public string LV { get; set; }
        /// <summary>
        /// 免费升级数量限制
        /// </summary>
        /// <returns></returns>
        public int? Count { get; set; }
        /// <summary>
        /// 价格
        /// </summary>
        /// <returns></returns>
        public decimal? Price { get; set; }
        /// <summary>
        /// 直接邀请黄金代理
        /// </summary>
        /// <returns></returns>
        public decimal? Z1 { get; set; }
        /// <summary>
        /// 直接邀请钻石代理
        /// </summary>
        /// <returns></returns>
        public decimal? Z2 { get; set; }
        /// <summary>
        /// 间接邀请黄金代理
        /// </summary>
        /// <returns></returns>
        public decimal? J1 { get; set; }
        /// <summary>
        /// 间接邀请钻石代理
        /// </summary>
        /// <returns></returns>
        public decimal? J2 { get; set; }
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