using System;
using HZSoft.Application.Code;

namespace HZSoft.Application.Entity.CustomerManage
{
    /// <summary>
    /// �� ��
    /// 
    /// �� ������������Ա
    /// �� �ڣ�2020-10-16 15:40
    /// �� �������ּ�¼
    /// </summary>
    public class WithdrawLogEntity : BaseEntity
    {
        #region ʵ���Ա
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        public int? id { get; set; }
        /// <summary>
        /// ����id
        /// </summary>
        /// <returns></returns>
        public int? agent_id { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        public string agent_name { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        public DateTime? date { get; set; }
        /// <summary>
        /// ���ֽ��
        /// </summary>
        /// <returns></returns>
        public decimal? amount { get; set; }
        /// <summary>
        /// ״̬
        /// </summary>
        /// <returns></returns>
        public int? status { get; set; }
        /// <summary>
        /// ƾ֤
        /// </summary>
        /// <returns></returns>
        public string img { get; set; }
        /// <summary>
        /// ����ʱ��
        /// </summary>
        /// <returns></returns>
        public DateTime? ModifyDate { get; set; }
        #endregion

        #region ��չ����
        /// <summary>
        /// ��������
        /// </summary>
        public override void Create()
        {
            this.date = DateTime.Now;
                                            }
        /// <summary>
        /// �༭����
        /// </summary>
        /// <param name="keyValue"></param>
        public override void Modify(int? keyValue)
        {
            this.id = keyValue;
            this.ModifyDate = DateTime.Now;
        }
        #endregion
    }
}