using HZSoft.Application.Entity.CustomerManage;
using System.Data.Entity.ModelConfiguration;

namespace HZSoft.Application.Mapping.CustomerManage
{
    /// <summary>
    /// �� ��
    /// 
    /// �� ������������Ա
    /// �� �ڣ�2018-10-30 20:25
    /// �� ����������֤���
    /// </summary>
    public class SmsInfoMap : EntityTypeConfiguration<SmsInfoEntity>
    {
        public SmsInfoMap()
        {
            #region ������
            //��
            this.ToTable("SmsInfo");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
