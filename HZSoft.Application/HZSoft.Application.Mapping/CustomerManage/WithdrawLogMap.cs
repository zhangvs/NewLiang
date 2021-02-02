using HZSoft.Application.Entity.CustomerManage;
using System.Data.Entity.ModelConfiguration;

namespace HZSoft.Application.Mapping.CustomerManage
{
    /// <summary>
    /// 版 本
    /// 
    /// 创 建：超级管理员
    /// 日 期：2020-10-16 15:40
    /// 描 述：提现记录
    /// </summary>
    public class WithdrawLogMap : EntityTypeConfiguration<WithdrawLogEntity>
    {
        public WithdrawLogMap()
        {
            #region 表、主键
            //表
            this.ToTable("WithdrawLog");
            //主键
            this.HasKey(t => t.id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
