using HZSoft.Application.Entity.CustomerManage;
using System.Data.Entity.ModelConfiguration;

namespace HZSoft.Application.Mapping.CustomerManage
{
    /// <summary>
    /// �� ��
    /// 
    /// �� ������������Ա
    /// �� �ڣ�2020-10-03 16:08
    /// �� �������˶���
    /// </summary>
    public class OrdersJMMap : EntityTypeConfiguration<OrdersJMEntity>
    {
        public OrdersJMMap()
        {
            #region ������
            //��
            this.ToTable("OrdersJM");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
