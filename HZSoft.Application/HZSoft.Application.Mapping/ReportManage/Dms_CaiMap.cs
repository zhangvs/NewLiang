using HZSoft.Application.Entity.ReportManage;
using System.Data.Entity.ModelConfiguration;

namespace HZSoft.Application.Mapping.ReportManage
{
    /// <summary>
    /// �� ��
    /// 
    /// �� ������������Ա
    /// �� �ڣ�2019-09-08 19:35
    /// �� ����Dms_Cai
    /// </summary>
    public class Dms_CaiMap : EntityTypeConfiguration<Dms_CaiEntity>
    {
        public Dms_CaiMap()
        {
            #region ������
            //��
            this.ToTable("Dms_Cai");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
