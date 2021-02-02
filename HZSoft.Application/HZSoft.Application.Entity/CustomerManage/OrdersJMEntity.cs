using System;
using HZSoft.Application.Code;

namespace HZSoft.Application.Entity.CustomerManage
{
    /// <summary>
    /// 版 本
    /// 
    /// 创 建：超级管理员
    /// 日 期：2020-10-03 16:08
    /// 描 述：加盟订单
    /// </summary>
    public class OrdersJMEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// Id
        /// </summary>
        /// <returns></returns>
        public int? Id { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        /// <returns></returns>
        public string OrderSn { get; set; }
        /// <summary>
        /// 级别
        /// </summary>
        /// <returns></returns>
        public string LV { get; set; }
        /// <summary>
        /// 价格
        /// </summary>
        /// <returns></returns>
        public decimal? Price { get; set; }
        /// <summary>
        /// 发起时间
        /// </summary>
        /// <returns></returns>
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// 支付状态
        /// </summary>
        /// <returns></returns>
        public int? PayStatus { get; set; }
        /// <summary>
        /// 支付时间
        /// </summary>
        /// <returns></returns>
        public DateTime? PayDate { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        /// <returns></returns>
        public string Remark { get; set; }
        /// <summary>
        /// 修改日期
        /// </summary>
        /// <returns></returns>
        public DateTime? ModifyDate { get; set; }
        /// <summary>
        /// 删除标记
        /// </summary>
        /// <returns></returns>
        public int? DeleteMark { get; set; }
        /// <summary>
        /// 代理id
        /// </summary>
        /// <returns></returns>
        public int? AgentId { get; set; }
        /// <summary>
        /// OpenId
        /// </summary>
        /// <returns></returns>
        public string OpenId { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        /// <returns></returns>
        public string NickName { get; set; }
        /// <summary>
        /// 父级
        /// </summary>
        /// <returns></returns>
        public int? Pid { get; set; }
        /// <summary>
        /// 顶级
        /// </summary>
        /// <returns></returns>
        public int? Tid { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.CreateDate = DateTime.Now;
            this.PayStatus = 0;
                                }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="keyValue"></param>
        public override void Modify(int? keyValue)
        {
            this.Id = keyValue;
            this.ModifyDate = DateTime.Now;
                                }
        #endregion
    }
}