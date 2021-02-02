using HZSoft.Application.Entity.CustomerManage;
using System.Data.Entity.ModelConfiguration;

namespace HZSoft.Application.Mapping.CustomerManage
{
    /// <summary>
    /// 版 本
    /// 
    /// 创 建：超级管理员
    /// 日 期：2020-10-04 10:18
    /// 描 述：会员规则
    /// </summary>
    public class Client_LVMap : EntityTypeConfiguration<Client_LVEntity>
    {
        public Client_LVMap()
        {
            #region 表、主键
            //表
            this.ToTable("Client_LV");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
