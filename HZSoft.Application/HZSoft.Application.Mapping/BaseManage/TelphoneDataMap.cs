using HZSoft.Application.Entity.BaseManage;
using System.Data.Entity.ModelConfiguration;

namespace HZSoft.Application.Mapping.BaseManage
{
    /// <summary>
    /// 版 本
    /// 
    /// 创 建：超级管理员
    /// 日 期：2017-09-19 17:45
    /// 描 述：号码库
    /// </summary>
    public class TelphoneDataMap : EntityTypeConfiguration<TelphoneDataEntity>
    {
        public TelphoneDataMap()
        {
            #region 表、主键
            //表
            this.ToTable("TelphoneData");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
