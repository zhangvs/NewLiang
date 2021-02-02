using System;
using HZSoft.Application.Code;

namespace HZSoft.Application.Entity.CustomerManage
{
    /// <summary>
    /// �� ��
    /// 
    /// �� ������������Ա
    /// �� �ڣ�2020-10-03 16:08
    /// �� �������˶���
    /// </summary>
    public class OrdersJMEntity : BaseEntity
    {
        #region ʵ���Ա
        /// <summary>
        /// Id
        /// </summary>
        /// <returns></returns>
        public int? Id { get; set; }
        /// <summary>
        /// ������
        /// </summary>
        /// <returns></returns>
        public string OrderSn { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        public string LV { get; set; }
        /// <summary>
        /// �۸�
        /// </summary>
        /// <returns></returns>
        public decimal? Price { get; set; }
        /// <summary>
        /// ����ʱ��
        /// </summary>
        /// <returns></returns>
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// ֧��״̬
        /// </summary>
        /// <returns></returns>
        public int? PayStatus { get; set; }
        /// <summary>
        /// ֧��ʱ��
        /// </summary>
        /// <returns></returns>
        public DateTime? PayDate { get; set; }
        /// <summary>
        /// ��ע
        /// </summary>
        /// <returns></returns>
        public string Remark { get; set; }
        /// <summary>
        /// �޸�����
        /// </summary>
        /// <returns></returns>
        public DateTime? ModifyDate { get; set; }
        /// <summary>
        /// ɾ�����
        /// </summary>
        /// <returns></returns>
        public int? DeleteMark { get; set; }
        /// <summary>
        /// ����id
        /// </summary>
        /// <returns></returns>
        public int? AgentId { get; set; }
        /// <summary>
        /// OpenId
        /// </summary>
        /// <returns></returns>
        public string OpenId { get; set; }
        /// <summary>
        /// �ǳ�
        /// </summary>
        /// <returns></returns>
        public string NickName { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        public int? Pid { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        public int? Tid { get; set; }
        #endregion

        #region ��չ����
        /// <summary>
        /// ��������
        /// </summary>
        public override void Create()
        {
            this.CreateDate = DateTime.Now;
            this.PayStatus = 0;
                                }
        /// <summary>
        /// �༭����
        /// </summary>
        /// <param name="keyValue"></param>
        public override void Modify(int? keyValue)
        {
            this.Id = keyValue;
            this.ModifyDate = DateTime.Now;
                                }
        #endregion
    }
}