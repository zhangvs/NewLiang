using HZSoft.Application.Entity.CustomerManage;
using System.Data.Entity.ModelConfiguration;

namespace HZSoft.Application.Mapping.CustomerManage
{
    /// <summary>
    /// �� ��
    /// 
    /// �� ������������Ա
    /// �� �ڣ�2020-03-05 10:43
    /// �� �������������
    /// </summary>
    public class TelphoneVipShareMap : EntityTypeConfiguration<TelphoneVipShareEntity>
    {
        public TelphoneVipShareMap()
        {
            #region ������
            //��
            this.ToTable("TelphoneVipShare");
            //����
            this.HasKey(t => t.VipShareId);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
