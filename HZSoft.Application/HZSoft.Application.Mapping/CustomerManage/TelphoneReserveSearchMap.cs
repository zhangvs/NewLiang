using HZSoft.Application.Entity.CustomerManage;
using System.Data.Entity.ModelConfiguration;

namespace HZSoft.Application.Mapping.CustomerManage
{
    /// <summary>
    /// �� ��
    /// 
    /// �� ������������Ա
    /// �� �ڣ�2018-07-28 10:05
    /// �� ����Ԥ����ѯ��¼
    /// </summary>
    public class TelphoneReserveSearchMap : EntityTypeConfiguration<TelphoneReserveSearchEntity>
    {
        public TelphoneReserveSearchMap()
        {
            #region ������
            //��
            this.ToTable("TelphoneReserveSearch");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
