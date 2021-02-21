using System;
using HZSoft.Application.Code;

namespace HZSoft.Application.Entity.BaseManage
{
    /// <summary>
    /// �� ��
    /// 
    /// �� ������������Ա
    /// �� �ڣ�2020-09-29 17:02
    /// �� �������˴���
    /// </summary>
    public class Wechat_AgentEntity : BaseEntity
    {
        #region ʵ���Ա
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        public int? Id { get; set; }
        /// <summary>
        /// �ȼ�
        /// </summary>
        /// <returns></returns>
        public int? Category { get; set; }
        /// <summary>
        /// �ϼ�
        /// </summary>
        /// <returns></returns>
        public int? Pid { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        public int? Tid { get; set; }
        /// <summary>
        /// �۸񸡶���������10%
        /// </summary>
        /// <returns></returns>
        public int? FuDong { get; set; }
        /// <summary>
        /// ΢���û���ʶ
        /// </summary>
        /// <returns></returns>
        public string OpenId { get; set; }
        /// <summary>
        /// ΢���ǳ�
        /// </summary>
        /// <returns></returns>
        public string nickname { get; set; }
        /// <summary>
        /// �û����Ա�
        /// </summary>
        /// <returns></returns>
        public int? Sex { get; set; }
        /// <summary>
        /// ΢��ͷ��
        /// </summary>
        /// <returns></returns>
        public string HeadimgUrl { get; set; }
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
        /// ����Ip
        /// </summary>
        /// <returns></returns>
        public string IPAddress { get; set; }
        /// <summary>
        /// ����Ip����
        /// </summary>
        /// <returns></returns>
        public string IPAddressName { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        public string LV { get; set; }
        /// <summary>
        /// Ӷ��
        /// </summary>
        /// <returns></returns>
        public decimal? profit { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        public decimal? Cashout { get; set; }
        /// <summary>
        /// �������
        /// </summary>
        /// <returns></returns>
        public decimal? Cashouted { get; set; }
        /// <summary>
        /// ������
        /// </summary>
        /// <returns></returns>
        public DateTime? EndDate { get; set; }
        /// <summary>
        /// ����ͼ
        /// </summary>
        /// <returns></returns>
        public string banner { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        public string realname { get; set; }
        /// <summary>
        /// ��ϵ�绰
        /// </summary>
        /// <returns></returns>
        public string contact { get; set; }
        /// <summary>
        /// ֧����
        /// </summary>
        /// <returns></returns>
        public string alipay { get; set; }
        /// <summary>
        /// ɾ����ʶ
        /// </summary>
        /// <returns></returns>
        public int? DeleteMark { get; set; }
        /// <summary>
        /// ���ñ�ʶ
        /// </summary>
        /// <returns></returns>
        public int? EnabledMark { get; set; }
        /// <summary>
        /// ����ʱ��
        /// </summary>
        /// <returns></returns>
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// �¼������ϼ�
        /// </summary>
        /// <returns></returns>
        public int? childcount { get; set; }
        /// <summary>
        /// �¼�Ӷ��ϼ�
        /// </summary>
        /// <returns></returns>
        public decimal? childprofit { get; set; }
        /// <summary>
        /// ����id
        /// </summary>
        /// <returns></returns>
        public string OrganizeId { get; set; }
        /// <summary>
        /// �����
        /// </summary>
        /// <returns></returns>
        public int? SeeCount { get; set; }
        /// <summary>
        /// ���۶�
        /// </summary>
        /// <returns></returns>
        public decimal? SellCount { get; set; }
        #endregion

        #region ��չ����
        /// <summary>
        /// ��������
        /// </summary>
        public override void Create()
        {
            this.CreateDate = DateTime.Now;
            this.profit = 0;
            this.Cashout = 0;
            this.Cashouted = 0;
            this.DeleteMark = 0;
            this.EnabledMark = 1;
            this.SeeCount = 0;
            this.SellCount = 0;
            this.childcount = 0;
            this.childprofit = 0;
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