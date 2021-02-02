using System;
using HZSoft.Application.Code;

namespace HZSoft.Application.Entity.CustomerManage
{
    /// <summary>
    /// �� ��
    /// 
    /// �� ������������Ա
    /// �� �ڣ�2018-09-05 17:58
    /// �� �������ż��˴���
    /// </summary>
    public class TelphoneLiangJoinEntity : BaseEntity
    {
        #region ʵ���Ա
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        public string Id { get; set; }
        /// <summary>
        /// ��˾����
        /// </summary>
        /// <returns></returns>
        public string CompanyName { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        public string FullName { get; set; }
        /// <summary>
        /// �绰
        /// </summary>
        /// <returns></returns>
        public string Telphone { get; set; }
        /// <summary>
        /// ʡ
        /// </summary>
        /// <returns></returns>
        public string Pro { get; set; }
        /// <summary>
        /// ��
        /// </summary>
        /// <returns></returns>
        public string City { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        public string Area { get; set; }
        /// <summary>
        /// ��ϸ��ַ
        /// </summary>
        /// <returns></returns>
        public string Address { get; set; }
        /// <summary>
        /// ΢�ű�ʶ
        /// </summary>
        /// <returns></returns>
        public string OpenId { get; set; }
        /// <summary>
        /// ΢���ǳ�
        /// </summary>
        /// <returns></returns>
        public string NickName { get; set; }
        /// <summary>
        /// ΢���Ա�
        /// </summary>
        /// <returns></returns>
        public int? Sex { get; set; }
        /// <summary>
        /// ΢��ͷ��
        /// </summary>
        /// <returns></returns>
        public string HeadimgUrl { get; set; }
        /// <summary>
        /// ΢��ʡ
        /// </summary>
        /// <returns></returns>
        public string WXPro { get; set; }
        /// <summary>
        /// ΢����
        /// </summary>
        /// <returns></returns>
        public string WxCity { get; set; }
        /// <summary>
        /// ΢���˺�
        /// </summary>
        /// <returns></returns>
        public string WxAccount { get; set; }
        /// <summary>
        /// ΢�Ŷ�ά��
        /// </summary>
        /// <returns></returns>
        public string WxQRcode { get; set; }
        /// <summary>
        /// ������Դ
        /// </summary>
        /// <returns></returns>
        public string OrganizeId { get; set; }
        /// <summary>
        /// �����ţ���Ϊ��Ӧ��
        /// </summary>
        /// <returns></returns>
        public int? BoosMark { get; set; }
        /// <summary>
        /// ������������
        /// </summary>
        /// <returns></returns>
        public int? LiangCount { get; set; }
        /// <summary>
        /// û�����ţ���Ϊ����
        /// </summary>
        /// <returns></returns>
        public int? AgentMark { get; set; }
        /// <summary>
        /// 0����ʶ
        /// </summary>
        /// <returns></returns>
        public int? TopMark { get; set; }
        /// <summary>
        /// ��˱�ʶ
        /// </summary>
        /// <returns></returns>
        public int? CheckMark { get; set; }
        /// <summary>
        /// ɾ�����
        /// </summary>
        /// <returns></returns>
        public int? DeleteMark { get; set; }
        /// <summary>
        /// ��ע
        /// </summary>
        /// <returns></returns>
        public string Description { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        public string FollowDes { get; set; }
        /// <summary>
        /// ����ʱ��
        /// </summary>
        /// <returns></returns>
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// �޸�����
        /// </summary>
        /// <returns></returns>
        public DateTime? ModifyTime { get; set; }
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
        /// beiz
        /// </summary>
        /// <returns></returns>
        public string Des1 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string Des2 { get; set; }
        #endregion

        #region ��չ����
        /// <summary>
        /// ��������
        /// </summary>
        public override void Create()
        {
            this.Id = Guid.NewGuid().ToString();
            CreateTime = DateTime.Now;
            CheckMark = 0;
            DeleteMark = 0;
            TopMark = 0;
        }
        /// <summary>
        /// �༭����
        /// </summary>
        /// <param name="keyValue"></param>
        public override void Modify(string keyValue)
        {
            this.Id = keyValue;
            ModifyTime = DateTime.Now;
            this.ModifyUserId = OperatorProvider.Provider.Current().UserId;
            this.ModifyUserName = OperatorProvider.Provider.Current().UserName;
        }
        #endregion
    }
}