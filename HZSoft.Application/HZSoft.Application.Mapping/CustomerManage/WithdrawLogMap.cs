using HZSoft.Application.Entity.CustomerManage;
using System.Data.Entity.ModelConfiguration;

namespace HZSoft.Application.Mapping.CustomerManage
{
    /// <summary>
    /// �� ��
    /// 
    /// �� ������������Ա
    /// �� �ڣ�2020-10-16 15:40
    /// �� �������ּ�¼
    /// </summary>
    public class WithdrawLogMap : EntityTypeConfiguration<WithdrawLogEntity>
    {
        public WithdrawLogMap()
        {
            #region ������
            //��
            this.ToTable("WithdrawLog");
            //����
            this.HasKey(t => t.id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
