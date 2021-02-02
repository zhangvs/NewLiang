using System;
using HZSoft.Application.Code;

namespace HZSoft.Application.Entity.CustomerManage
{
    /// <summary>
    /// �� ��
    /// 
    /// �� ������������Ա
    /// �� �ڣ�2018-07-28 10:05
    /// �� ����Ԥ����ѯ��¼
    /// </summary>
    public class TelphoneReserveSearchEntity : BaseEntity
    {
        #region ʵ���Ա
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        public string Id { get; set; }
        /// <summary>
        /// ��ѯ����
        /// </summary>
        /// <returns></returns>
        public string SearchNumber { get; set; }
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
        /// ��ѯʱ��
        /// </summary>
        /// <returns></returns>
        public DateTime? SearchDate { get; set; }
        #endregion

        #region ��չ����
        /// <summary>
        /// ��������
        /// </summary>
        public override void Create()
        {
            this.Id = Guid.NewGuid().ToString();
            this.SearchDate = DateTime.Now;
                                            }
        /// <summary>
        /// �༭����
        /// </summary>
        /// <param name="keyValue"></param>
        public override void Modify(string keyValue)
        {
            this.Id = keyValue;
                                            }
        #endregion
    }
}