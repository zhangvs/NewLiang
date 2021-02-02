using System;
using HZSoft.Application.Code;

namespace HZSoft.Application.Entity.CustomerManage
{
    /// <summary>
    /// �� ��
    /// 
    /// �� ������������Ա
    /// �� �ڣ�2018-10-17 21:56
    /// �� ����VIP�������
    /// </summary>
    public class TelphoneLiangVipEntity : BaseEntity
    {
        #region ʵ���Ա
        /// <summary>
        /// Id
        /// </summary>
        /// <returns></returns>
        public string Id { get; set; }
        /// <summary>
        /// ������˾
        /// </summary>
        /// <returns></returns>
        public string OrganizeId { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        public string FullName { get; set; }
        /// <summary>
        /// �ϴ���������
        /// </summary>
        /// <returns></returns>
        public int? UploadMax { get; set; }
        /// <summary>
        /// ���ۺ�������
        /// </summary>
        /// <returns></returns>
        public int? OtherMax { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        public int? OrgMax { get; set; }
        /// <summary>
        /// �۸�
        /// </summary>
        /// <returns></returns>
        public decimal? Price { get; set; }
        /// <summary>
        /// ����ʼʱ��
        /// </summary>
        /// <returns></returns>
        public DateTime? VipStartDate { get; set; }
        /// <summary>
        /// �������ʱ��
        /// </summary>
        /// <returns></returns>
        public DateTime? VipEndDate { get; set; }
        /// <summary>
        /// ɾ�����
        /// </summary>
        /// <returns></returns>
        public int? DeleteMark { get; set; }
        /// <summary>
        /// ��Ч��־
        /// </summary>
        /// <returns></returns>
        public int? EnabledMark { get; set; }
        /// <summary>
        /// ��ע
        /// </summary>
        /// <returns></returns>
        public string Description { get; set; }
        /// <summary>
        /// ��������
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
        /// <summary>
        /// �޸�����
        /// </summary>
        /// <returns></returns>
        public DateTime? ModifyDate { get; set; }
        /// <summary>
        /// �޸��û�����
        /// </summary>
        /// <returns></returns>
        public string ModifyUserId { get; set; }
        /// <summary>
        /// �޸��û�
        /// </summary>
        /// <returns></returns>
        public string ModifyUserName { get; set; }
        /// <summary>
        /// �����־
        /// </summary>
        /// <returns></returns>
        public int? ShareMark { get; set; }
        #endregion

        #region ��չ����
        /// <summary>
        /// ��������
        /// </summary>
        public override void Create()
        {
            this.Id = Guid.NewGuid().ToString();
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
            this.Id = keyValue;
            this.ModifyDate = DateTime.Now;
            this.ModifyUserId = OperatorProvider.Provider.Current().UserId;
            this.ModifyUserName = OperatorProvider.Provider.Current().UserName;
        }
        #endregion
    }
}