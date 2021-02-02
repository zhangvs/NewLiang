using HZSoft.Application.Entity.CustomerManage;
using System.Data.Entity.ModelConfiguration;

namespace HZSoft.Application.Mapping.CustomerManage
{
    /// <summary>
    /// 版 本
    /// 
    /// 创 建：超级管理员
    /// 日 期：2020-10-08 14:07
    /// 描 述：佣金表
    /// </summary>
    public class ComissionLogMap : EntityTypeConfiguration<ComissionLogEntity>
    {
        public ComissionLogMap()
        {
            #region 表、主键
            //表
            this.ToTable("ComissionLog");
            //主键
            this.HasKey(t => t.id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
