using HZSoft.Application.Entity.CustomerManage;
using System.Data.Entity.ModelConfiguration;

namespace HZSoft.Application.Mapping.CustomerManage
{
    /// <summary>
    /// �� ��
    /// 
    /// �� ������������Ա
    /// �� �ڣ�2018-09-05 17:58
    /// �� �������ż��˴���
    /// </summary>
    public class TelphoneLiangJoinMap : EntityTypeConfiguration<TelphoneLiangJoinEntity>
    {
        public TelphoneLiangJoinMap()
        {
            #region ������
            //��
            this.ToTable("TelphoneLiangJoin");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
