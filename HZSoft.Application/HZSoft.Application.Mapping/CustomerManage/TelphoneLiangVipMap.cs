using HZSoft.Application.Entity.CustomerManage;
using System.Data.Entity.ModelConfiguration;

namespace HZSoft.Application.Mapping.CustomerManage
{
    /// <summary>
    /// �� ��
    /// 
    /// �� ������������Ա
    /// �� �ڣ�2018-10-17 21:56
    /// �� ����VIP�������
    /// </summary>
    public class TelphoneLiangVipMap : EntityTypeConfiguration<TelphoneLiangVipEntity>
    {
        public TelphoneLiangVipMap()
        {
            #region ������
            //��
            this.ToTable("TelphoneLiangVip");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
