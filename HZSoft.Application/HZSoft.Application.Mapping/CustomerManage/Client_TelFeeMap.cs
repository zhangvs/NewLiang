using HZSoft.Application.Entity.CustomerManage;
using System.Data.Entity.ModelConfiguration;

namespace HZSoft.Application.Mapping.CustomerManage
{
    /// <summary>
    /// �� ��
    /// 
    /// �� ������������Ա
    /// �� �ڣ�2020-10-04 19:02
    /// �� �������ŷ�Ӷ����
    /// </summary>
    public class Client_TelFeeMap : EntityTypeConfiguration<Client_TelFeeEntity>
    {
        public Client_TelFeeMap()
        {
            #region ������
            //��
            this.ToTable("Client_TelFee");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
