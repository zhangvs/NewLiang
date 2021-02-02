using System;
using HZSoft.Application.Code;

namespace HZSoft.Application.Entity.CustomerManage
{
    /// <summary>
    /// �� ��
    /// 
    /// �� ������������Ա
    /// �� �ڣ�2020-10-04 10:18
    /// �� ������Ա����
    /// </summary>
    public class Client_LVEntity : BaseEntity
    {
        #region ʵ���Ա
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        public int? Id { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        public string LV { get; set; }
        /// <summary>
        /// ���������������
        /// </summary>
        /// <returns></returns>
        public int? Count { get; set; }
        /// <summary>
        /// �۸�
        /// </summary>
        /// <returns></returns>
        public decimal? Price { get; set; }
        /// <summary>
        /// ֱ������ƽ����
        /// </summary>
        /// <returns></returns>
        public decimal? Z1 { get; set; }
        /// <summary>
        /// ֱ��������ʯ����
        /// </summary>
        /// <returns></returns>
        public decimal? Z2 { get; set; }
        /// <summary>
        /// �������ƽ����
        /// </summary>
        /// <returns></returns>
        public decimal? J1 { get; set; }
        /// <summary>
        /// ���������ʯ����
        /// </summary>
        /// <returns></returns>
        public decimal? J2 { get; set; }
        #endregion

        #region ��չ����
        /// <summary>
        /// ��������
        /// </summary>
        public override void Create()
        {
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