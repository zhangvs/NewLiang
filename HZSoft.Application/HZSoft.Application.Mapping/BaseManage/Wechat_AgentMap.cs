using HZSoft.Application.Entity.BaseManage;
using System.Data.Entity.ModelConfiguration;

namespace HZSoft.Application.Mapping.BaseManage
{
    /// <summary>
    /// �� ��
    /// 
    /// �� ������������Ա
    /// �� �ڣ�2020-09-29 17:02
    /// �� �������˴���
    /// </summary>
    public class Wechat_AgentMap : EntityTypeConfiguration<Wechat_AgentEntity>
    {
        public Wechat_AgentMap()
        {
            #region ������
            //��
            this.ToTable("Wechat_Agent");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
