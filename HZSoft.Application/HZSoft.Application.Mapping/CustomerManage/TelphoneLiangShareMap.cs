using HZSoft.Application.Entity.CustomerManage;
using System.Data.Entity.ModelConfiguration;

namespace HZSoft.Application.Mapping.CustomerManage
{
    /// <summary>
    /// �� ��
    /// 
    /// �� ������������Ա
    /// �� �ڣ�2018-09-06 10:25
    /// �� �������ŷ����¼
    /// </summary>
    public class TelphoneLiangShareMap : EntityTypeConfiguration<TelphoneLiangShareEntity>
    {
        public TelphoneLiangShareMap()
        {
            #region ������
            //��
            this.ToTable("TelphoneLiangShare");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
