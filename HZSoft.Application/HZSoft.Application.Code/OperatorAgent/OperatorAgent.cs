using System;
using System.Collections.Generic;

namespace HZSoft.Application.Code
{
    /// <summary>
    /// 版 本 6.1
    /// 
    /// 创建人：佘赐雄
    /// 日 期：2015.10.10
    /// 描 述：当前操作者信息类
    /// </summary>
    public class OperatorAgent
    {
        /// <summary>
        /// 主键
        /// </summary>
        /// <returns></returns>
        public int? Id { get; set; }
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
        /// 登录时间
        /// </summary>
        public DateTime LogTime { get; set; }
        /// <summary>
        /// 登录Token
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// 价格浮动比例上下10%
        /// </summary>
        /// <returns></returns>
        public decimal? FuDong { get; set; }
        /// <summary>
        /// 登录代理所属机构id
        /// </summary>
        public string OrganizeId { get; set; }
        /// <summary>
        /// 登录代理所属机构级别
        /// </summary>
        public int? Category { get; set; }

    }
}
