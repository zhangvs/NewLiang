using HZSoft.Application.Entity.CustomerManage;
using System.Data.Entity.ModelConfiguration;

namespace HZSoft.Application.Mapping.CustomerManage
{
    /// <summary>
    /// �� ��
    /// 
    /// �� ������������Ա
    /// �� �ڣ�2018-07-25 11:57
    /// �� ����ԤԼ����
    /// </summary>
    public class TelphoneReserverMap : EntityTypeConfiguration<TelphoneReserverEntity>
    {
        public TelphoneReserverMap()
        {
            #region ������
            //��
            this.ToTable("TelphoneReserver");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
