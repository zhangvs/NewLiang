using HZSoft.Application.Entity.CustomerManage;
using System.Data.Entity.ModelConfiguration;

namespace HZSoft.Application.Mapping.CustomerManage
{
    /// <summary>
    /// �� ��
    /// 
    /// �� ������������Ա
    /// �� �ڣ�2018-08-23 16:22
    /// �� ����������ҳ���
    /// </summary>
    public class TelphoneLiangSeeMap : EntityTypeConfiguration<TelphoneLiangSeeEntity>
    {
        public TelphoneLiangSeeMap()
        {
            #region ������
            //��
            this.ToTable("TelphoneLiangSee");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
