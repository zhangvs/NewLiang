using System;
using HZSoft.Application.Code;

namespace HZSoft.Application.Entity.CustomerManage
{
    /// <summary>
    /// �� ��
    /// 
    /// �� ������������Ա
    /// �� �ڣ�2020-03-05 10:43
    /// �� �������������
    /// </summary>
    public class TelphoneVipShareEntity : BaseEntity
    {
        #region ʵ���Ա
        /// <summary>
        /// �����������
        /// </summary>
        /// <returns></returns>
        public string VipShareId { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        public string VipId { get; set; }
        /// <summary>
        /// �����������
        /// </summary>
        /// <returns></returns>
        public string ShareId { get; set; }
        /// <summary>
        /// ������
        /// </summary>
        /// <returns></returns>
        public int? SortCode { get; set; }
        /// <summary>
        /// ����ʱ��
        /// </summary>
        /// <returns></returns>
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// �����û�����
        /// </summary>
        /// <returns></returns>
        public string CreateUserId { get; set; }
        /// <summary>
        /// �����û�
        /// </summary>
        /// <returns></returns>
        public string CreateUserName { get; set; }
        #endregion

        #region ��չ����
        /// <summary>
        /// ��������
        /// </summary>
        public override void Create()
        {
            this.VipShareId = Guid.NewGuid().ToString();
            this.CreateDate = DateTime.Now;
            this.CreateUserId = OperatorProvider.Provider.Current().UserId;
            this.CreateUserName = OperatorProvider.Provider.Current().UserName;
        }
        /// <summary>
        /// �༭����
        /// </summary>
        /// <param name="keyValue"></param>
        public override void Modify(string keyValue)
        {
            this.VipShareId = keyValue;
                                            }
        #endregion
    }
}