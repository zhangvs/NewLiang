using HZSoft.Application.Entity.BaseManage;
using System.Data.Entity.ModelConfiguration;

namespace HZSoft.Application.Mapping.BaseManage
{
    /// <summary>
    /// 版 本
    /// 
    /// 创 建：超级管理员
    /// 日 期：2020-09-29 17:02
    /// 描 述：加盟代理
    /// </summary>
    public class Wechat_AgentMap : EntityTypeConfiguration<Wechat_AgentEntity>
    {
        public Wechat_AgentMap()
        {
            #region 表、主键
            //表
            this.ToTable("Wechat_Agent");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
