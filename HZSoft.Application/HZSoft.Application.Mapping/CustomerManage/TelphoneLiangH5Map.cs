using HZSoft.Application.Entity.CustomerManage;
using System.Data.Entity.ModelConfiguration;

namespace HZSoft.Application.Mapping.CustomerManage
{
    /// <summary>
    /// �� ��
    /// 
    /// �� ������������Ա
    /// �� �ڣ�2020-06-07 14:11
    /// �� ����ͷ�����ſ�
    /// </summary>
    public class TelphoneLiangH5Map : EntityTypeConfiguration<TelphoneLiangH5Entity>
    {
        public TelphoneLiangH5Map()
        {
            #region ������
            //��
            this.ToTable("TelphoneLiangH5");
            //����
            this.HasKey(t => t.TelphoneID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
