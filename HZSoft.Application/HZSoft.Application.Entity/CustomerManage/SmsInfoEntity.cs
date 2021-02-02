using System;
using HZSoft.Application.Code;

namespace HZSoft.Application.Entity.CustomerManage
{
    /// <summary>
    /// �� ��
    /// 
    /// �� ������������Ա
    /// �� �ڣ�2018-10-30 20:25
    /// �� ����������֤���
    /// </summary>
    public class SmsInfoEntity : BaseEntity
    {
        #region ʵ���Ա
        /// <summary>
        /// Id
        /// </summary>
        /// <returns></returns>
        public int? Id { get; set; }
        /// <summary>
        /// �ֻ�����
        /// </summary>
        /// <returns></returns>
        public string Tel { get; set; }
        /// <summary>
        /// ��֤��
        /// </summary>
        /// <returns></returns>
        public string Captcha { get; set; }
        /// <summary>
        /// ���ʹ���
        /// </summary>
        /// <returns></returns>
        public int? SendCount { get; set; }
        /// <summary>
        /// ״̬
        /// </summary>
        /// <returns></returns>
        public int? Status { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        public int? Type { get; set; }
        /// <summary>
        /// ������
        /// </summary>
        /// <returns></returns>
        public string CreateName { get; set; }
        /// <summary>
        /// ����ʱ��
        /// </summary>
        /// <returns></returns>
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        public string SmsContent { get; set; }
        #endregion

        #region ��չ����
        /// <summary>
        /// ��������
        /// </summary>
        public override void Create()
        {
            this.CreateDate = DateTime.Now;
                                }
        /// <summary>
        /// �༭����
        /// </summary>
        /// <param name="keyValue"></param>
        public override void Modify(int? keyValue)
        {
            this.Id = keyValue;
                                            }
        #endregion
    }
}