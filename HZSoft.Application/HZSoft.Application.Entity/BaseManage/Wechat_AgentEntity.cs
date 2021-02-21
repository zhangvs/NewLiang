using System;
using HZSoft.Application.Code;

namespace HZSoft.Application.Entity.BaseManage
{
    /// <summary>
    /// 版 本
    /// 
    /// 创 建：超级管理员
    /// 日 期：2020-09-29 17:02
    /// 描 述：加盟代理
    /// </summary>
    public class Wechat_AgentEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 主键
        /// </summary>
        /// <returns></returns>
        public int? Id { get; set; }
        /// <summary>
        /// 等级
        /// </summary>
        /// <returns></returns>
        public int? Category { get; set; }
        /// <summary>
        /// 上级
        /// </summary>
        /// <returns></returns>
        public int? Pid { get; set; }
        /// <summary>
        /// 顶级
        /// </summary>
        /// <returns></returns>
        public int? Tid { get; set; }
        /// <summary>
        /// 价格浮动比例上下10%
        /// </summary>
        /// <returns></returns>
        public int? FuDong { get; set; }
        /// <summary>
        /// 微信用户标识
        /// </summary>
        /// <returns></returns>
        public string OpenId { get; set; }
        /// <summary>
        /// 微信昵称
        /// </summary>
        /// <returns></returns>
        public string nickname { get; set; }
        /// <summary>
        /// 用户的性别
        /// </summary>
        /// <returns></returns>
        public int? Sex { get; set; }
        /// <summary>
        /// 微信头像
        /// </summary>
        /// <returns></returns>
        public string HeadimgUrl { get; set; }
        /// <summary>
        /// 省
        /// </summary>
        /// <returns></returns>
        public string Province { get; set; }
        /// <summary>
        /// 市
        /// </summary>
        /// <returns></returns>
        public string City { get; set; }
        /// <summary>
        /// 区
        /// </summary>
        /// <returns></returns>
        public string Country { get; set; }
        /// <summary>
        /// 访问Ip
        /// </summary>
        /// <returns></returns>
        public string IPAddress { get; set; }
        /// <summary>
        /// 访问Ip地区
        /// </summary>
        /// <returns></returns>
        public string IPAddressName { get; set; }
        /// <summary>
        /// 级别
        /// </summary>
        /// <returns></returns>
        public string LV { get; set; }
        /// <summary>
        /// 佣金
        /// </summary>
        /// <returns></returns>
        public decimal? profit { get; set; }
        /// <summary>
        /// 提现
        /// </summary>
        /// <returns></returns>
        public decimal? Cashout { get; set; }
        /// <summary>
        /// 提现完成
        /// </summary>
        /// <returns></returns>
        public decimal? Cashouted { get; set; }
        /// <summary>
        /// 到期日
        /// </summary>
        /// <returns></returns>
        public DateTime? EndDate { get; set; }
        /// <summary>
        /// 宣传图
        /// </summary>
        /// <returns></returns>
        public string banner { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        /// <returns></returns>
        public string realname { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        /// <returns></returns>
        public string contact { get; set; }
        /// <summary>
        /// 支付宝
        /// </summary>
        /// <returns></returns>
        public string alipay { get; set; }
        /// <summary>
        /// 删除标识
        /// </summary>
        /// <returns></returns>
        public int? DeleteMark { get; set; }
        /// <summary>
        /// 可用标识
        /// </summary>
        /// <returns></returns>
        public int? EnabledMark { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        /// <returns></returns>
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// 下级数量合计
        /// </summary>
        /// <returns></returns>
        public int? childcount { get; set; }
        /// <summary>
        /// 下级佣金合计
        /// </summary>
        /// <returns></returns>
        public decimal? childprofit { get; set; }
        /// <summary>
        /// 机构id
        /// </summary>
        /// <returns></returns>
        public string OrganizeId { get; set; }
        /// <summary>
        /// 浏览量
        /// </summary>
        /// <returns></returns>
        public int? SeeCount { get; set; }
        /// <summary>
        /// 销售额
        /// </summary>
        /// <returns></returns>
        public decimal? SellCount { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.CreateDate = DateTime.Now;
            this.profit = 0;
            this.Cashout = 0;
            this.Cashouted = 0;
            this.DeleteMark = 0;
            this.EnabledMark = 1;
            this.SeeCount = 0;
            this.SellCount = 0;
            this.childcount = 0;
            this.childprofit = 0;
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