using HZSoft.Application.Entity.CustomerManage;
using System.Data.Entity.ModelConfiguration;

namespace HZSoft.Application.Mapping.CustomerManage
{
    /// <summary>
    /// �� ��
    /// 
    /// �� ������������Ա
    /// �� �ڣ�2018-11-06 20:39
    /// �� �����պſ�
    /// </summary>
    public class TelphonePuMap : EntityTypeConfiguration<TelphonePuEntity>
    {
        public TelphonePuMap()
        {
            #region ������
            //��
            this.ToTable("TelphonePu");
            //����
            this.HasKey(t => t.TelphoneID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
