using HZSoft.Application.Entity.WeChatManage;
using System.Data.Entity.ModelConfiguration;

namespace HZSoft.Application.Mapping.WeChatManage
{
    /// <summary>
    /// �� ��
    /// 
    /// �� ������������Ա
    /// �� �ڣ�2017-11-21 14:46
    /// �� �����û���
    /// </summary>
    public class WeChat_UsersMap : EntityTypeConfiguration<WeChat_UsersEntity>
    {
        public WeChat_UsersMap()
        {
            #region ������
            //��
            this.ToTable("WeChat_Users");
            //����
            this.HasKey(t => t.OpenId);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
