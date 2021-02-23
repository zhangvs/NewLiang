using System;
using HZSoft.Application.Code;

namespace HZSoft.Application.Entity.CustomerManage
{
    /// <summary>
    /// �� ��
    /// 
    /// �� ������������Ա
    /// �� �ڣ�2020-10-08 14:07
    /// �� ����Ӷ���
    /// </summary>
    public class ComissionLogEntity : BaseEntity
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
        /// ����ʱ��
        /// </summary>
        /// <returns></returns>
        public DateTime? created_at { get; set; }
        /// <summary>
        /// 0ֱ�����ۣ�1������ۣ�2���ۣ�3����
        /// </summary>
        /// <returns></returns>
        public int? indirect { get; set; }
        /// <summary>
        /// ���������Id
        /// </summary>
        /// <returns></returns>
        public int? invited_agent_id { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        public string phonenum { get; set; }
        /// <summary>
        /// Ӷ��
        /// </summary>
        /// <returns></returns>
        public decimal? profit { get; set; }
        /// <summary>
        /// ״̬
        /// </summary>
        /// <returns></returns>
        public int? status { get; set; }
        /// <summary>
        /// �������
        /// </summary>
        /// <returns></returns>
        public string orderno { get; set; }
        /// <summary>
        /// �������id
        /// </summary>
        /// <returns></returns>
        public int? orderid { get; set; }
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
            this.created_at = DateTime.Now;
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