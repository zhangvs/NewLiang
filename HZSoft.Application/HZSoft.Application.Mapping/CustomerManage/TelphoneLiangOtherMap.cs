using HZSoft.Application.Entity.CustomerManage;
using System.Data.Entity.ModelConfiguration;

namespace HZSoft.Application.Mapping.CustomerManage
{
    /// <summary>
    /// �� ��
    /// 
    /// �� ������������Ա
    /// �� �ڣ�2018-10-09 20:14
    /// �� �����������ſ�
    /// </summary>
    public class TelphoneLiangOtherMap : EntityTypeConfiguration<TelphoneLiangOtherEntity>
    {
        public TelphoneLiangOtherMap()
        {
            #region ������
            //��
            this.ToTable("TelphoneLiangOther");
            //����
            this.HasKey(t => t.TelphoneID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
