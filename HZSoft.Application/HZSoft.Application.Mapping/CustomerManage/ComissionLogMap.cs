using HZSoft.Application.Entity.CustomerManage;
using System.Data.Entity.ModelConfiguration;

namespace HZSoft.Application.Mapping.CustomerManage
{
    /// <summary>
    /// �� ��
    /// 
    /// �� ������������Ա
    /// �� �ڣ�2020-10-08 14:07
    /// �� ����Ӷ���
    /// </summary>
    public class ComissionLogMap : EntityTypeConfiguration<ComissionLogEntity>
    {
        public ComissionLogMap()
        {
            #region ������
            //��
            this.ToTable("ComissionLog");
            //����
            this.HasKey(t => t.id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
