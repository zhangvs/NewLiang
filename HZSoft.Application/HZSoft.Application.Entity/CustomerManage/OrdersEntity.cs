using System;
using HZSoft.Application.Code;

namespace HZSoft.Application.Entity.CustomerManage
{
    /// <summary>
    /// �� ��
    /// 
    /// �� ������������Ա
    /// �� �ڣ�2020-04-16 09:07
    /// �� ����������
    /// </summary>
    public class OrdersEntity : BaseEntity
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
        /// �û�ID
        /// </summary>
        /// <returns></returns>
        public string Host { get; set; }
        /// <summary>
        /// ����id
        /// </summary>
        /// <returns></returns>
        public int? AgentId { get; set; }
        /// <summary>
        /// �û�ID
        /// </summary>
        /// <returns></returns>
        public int? Pid { get; set; }
        /// <summary>
        /// �û�ID
        /// </summary>
        /// <returns></returns>
        public int? Tid { get; set; }
        /// <summary>
        /// TelphoneID
        /// </summary>
        /// <returns></returns>
        public int? TelphoneID { get; set; }
        /// <summary>
        /// �ֻ�����
        /// </summary>
        /// <returns></returns>
        public string Tel { get; set; }
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
        /// 0 ������ 1 ������ 2 ������ 3 ����� 
        /// </summary>
        /// <returns></returns>
        public int? Status { get; set; }
        /// <summary>
        /// ֧����ʽ
        /// </summary>
        /// <returns></returns>
        public string PayType { get; set; }
        /// <summary>
        /// 0 δ֧�� 1��֧��
        /// </summary>
        /// <returns></returns>
        public int? PayStatus { get; set; }
        /// <summary>
        /// ֧��ʱ��
        /// </summary>
        /// <returns></returns>
        public DateTime? PayDate { get; set; }
        /// <summary>
        /// �ռ���
        /// </summary>
        /// <returns></returns>
        public string Receiver { get; set; }
        /// <summary>
        /// ��ϵ�绰
        /// </summary>
        /// <returns></returns>
        public string ContactTel { get; set; }
        /// <summary>
        /// ʡ
        /// </summary>
        /// <returns></returns>
        public string Province { get; set; }
        /// <summary>
        /// ��
        /// </summary>
        /// <returns></returns>
        public string City { get; set; }
        /// <summary>
        /// ��
        /// </summary>
        /// <returns></returns>
        public string Country { get; set; }
        /// <summary>
        /// ��ϸ��ַ
        /// </summary>
        /// <returns></returns>
        public string Address { get; set; }
        /// <summary>
        /// ��ϸ��ַ
        /// </summary>
        /// <returns></returns>
        public string Remark { get; set; }
        /// <summary>
        /// �����ߵ�OpenId
        /// </summary>
        /// <returns></returns>
        public string OpenId { get; set; }
        /// <summary>
        /// ������
        /// </summary>
        /// <returns></returns>
        public string DeliveryName { get; set; }
        /// <summary>
        /// ����ʱ��
        /// </summary>
        /// <returns></returns>
        public DateTime? DeliveryDate { get; set; }
        /// <summary>
        /// ��ݹ�˾
        /// </summary>
        /// <returns></returns>
        public string ExpressCompany { get; set; }
        /// <summary>
        /// ��ݵ���
        /// </summary>
        /// <returns></returns>
        public string ExpressSn { get; set; }
        /// <summary>
        /// ��ݹ�˾���
        /// </summary>
        /// <returns></returns>
        public string ExpressCode { get; set; }
        /// <summary>
        /// ֧������
        /// </summary>
        /// <returns></returns>
        public string PaySn { get; set; }
        /// <summary>
        /// �˿�ʱ��
        /// </summary>
        /// <returns></returns>
        public DateTime? ReturnDate { get; set; }
        /// <summary>
        /// �˿�ԭ��
        /// </summary>
        /// <returns></returns>
        public string ReturnRemark { get; set; }
        /// <summary>
        /// �˻����
        /// </summary>
        /// <returns></returns>
        public string ReturnSn { get; set; }
        /// <summary>
        /// 0 �ֻ���H5֧����  1 ���ԣ�ɨ��Native֧������2΢���������JSAPI��
        /// </summary>
        /// <returns></returns>
        public int? PC { get; set; }
        /// <summary>
        /// �޸�ʱ��
        /// </summary>
        /// <returns></returns>
        public DateTime? ModifyDate { get; set; }
        /// <summary>
        /// ɾ�����
        /// </summary>
        /// <returns></returns>
        public int? DeleteMark { get; set; }
        /// <summary>
        /// ͷ�����ҳ
        /// </summary>
        /// <returns></returns>
        public string TouUrl { get; set; }
        #endregion

        #region ��չ����
        /// <summary>
        /// ��������
        /// </summary>
        public override void Create()
        {
            this.CreateDate = DateTime.Now;
            this.Status = 0;
            this.DeleteMark = 0;
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