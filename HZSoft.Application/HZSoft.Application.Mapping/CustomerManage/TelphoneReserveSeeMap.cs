using HZSoft.Application.Entity.CustomerManage;
using System.Data.Entity.ModelConfiguration;

namespace HZSoft.Application.Mapping.CustomerManage
{
    /// <summary>
    /// �� ��
    /// 
    /// �� ������������Ա
    /// �� �ڣ�2018-07-27 17:06
    /// �� ��������Ԥ�����ʴ���
    /// </summary>
    public class TelphoneReserveSeeMap : EntityTypeConfiguration<TelphoneReserveSeeEntity>
    {
        public TelphoneReserveSeeMap()
        {
            #region ������
            //��
            this.ToTable("TelphoneReserveSee");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
