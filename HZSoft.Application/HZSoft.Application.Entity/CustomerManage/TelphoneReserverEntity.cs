using System;
using HZSoft.Application.Code;

namespace HZSoft.Application.Entity.CustomerManage
{
    /// <summary>
    /// �� ��
    /// 
    /// �� ������������Ա
    /// �� �ڣ�2018-07-25 11:57
    /// �� ����ԤԼ����
    /// </summary>
    public class TelphoneReserverEntity : BaseEntity
    {
        #region ʵ���Ա
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        public string Id { get; set; }
        /// <summary>
        /// ԤԼ����
        /// </summary>
        /// <returns></returns>
        public string Reserve { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        public string Name { get; set; }
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
        /// �˵���ʶ
        /// </summary>
        /// <returns></returns>
        public int? CheckMark { get; set; }
        /// <summary>
        /// �۳���ʶ
        /// </summary>
        /// <returns></returns>
        public int? SellMark { get; set; }
        /// <summary>
        /// ���ϱ�ʶ
        /// </summary>
        /// <returns></returns>
        public int? DeleteMark { get; set; }        
        /// <summary>
        /// ��ע
        /// </summary>
        /// <returns></returns>
        public string Description { get; set; }
        /// <summary>
        /// ��Դ
        /// </summary>
        /// <returns></returns>
        public string Source { get; set; }
        /// <summary>
        /// ����ʱ��
        /// </summary>
        /// <returns></returns>
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// �޸�ʱ��
        /// </summary>
        /// <returns></returns>
        public DateTime? ModifyTime { get; set; }
        /// <summary>
        /// �����û�ID
        /// </summary>
        /// <returns></returns>
        public string ModifyUserId { get; set; }
        /// <summary>
        /// �����û���
        /// </summary>
        /// <returns></returns>
        public string ModifyUserName { get; set; }

        #endregion

        #region ��չ����
        /// <summary>
        /// ��������
        /// </summary>
        public override void Create()
        {
            this.Id = Guid.NewGuid().ToString();
            CheckMark = 0;
            SellMark = 0;
            DeleteMark = 0;
            this.CreateTime = DateTime.Now;
                                            }
        /// <summary>
        /// �༭����
        /// </summary>
        /// <param name="keyValue"></param>
        public override void Modify(string keyValue)
        {
            this.Id = keyValue;
            this.ModifyTime = DateTime.Now;
            this.ModifyUserId = OperatorProvider.Provider.Current().UserId;
            this.ModifyUserName = OperatorProvider.Provider.Current().UserName;
        }
        #endregion
    }
}