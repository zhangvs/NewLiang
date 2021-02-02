using HZSoft.Application.Entity.CustomerManage;
using System.Data.Entity.ModelConfiguration;

namespace HZSoft.Application.Mapping.CustomerManage
{
    /// <summary>
    /// 版 本
    /// 
    /// 创 建：超级管理员
    /// 日 期：2020-10-04 19:02
    /// 描 述：卖号返佣规则
    /// </summary>
    public class Client_TelFeeMap : EntityTypeConfiguration<Client_TelFeeEntity>
    {
        public Client_TelFeeMap()
        {
            #region 表、主键
            //表
            this.ToTable("Client_TelFee");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
