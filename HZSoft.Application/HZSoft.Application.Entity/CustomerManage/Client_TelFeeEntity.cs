using System;
using HZSoft.Application.Code;

namespace HZSoft.Application.Entity.CustomerManage
{
    /// <summary>
    /// �� ��
    /// 
    /// �� ������������Ա
    /// �� �ڣ�2020-10-04 19:02
    /// �� �������ŷ�Ӷ����
    /// </summary>
    public class Client_TelFeeEntity : BaseEntity
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
        public decimal? Min { get; set; }
        /// <summary>
        /// С��
        /// </summary>
        /// <returns></returns>
        public decimal? Max { get; set; }
        /// <summary>
        /// ��ֱͨ�ӷ�Ӷ
        /// </summary>
        /// <returns></returns>
        public decimal? PuZ { get; set; }
        /// <summary>
        /// �ƽ�ֱ�ӷ�Ӷ
        /// </summary>
        /// <returns></returns>
        public decimal? HuangZ { get; set; }
        /// <summary>
        /// ��ʯֱ�ӷ�Ӷ
        /// </summary>
        /// <returns></returns>
        public decimal? ZuanZ { get; set; }
        /// <summary>
        /// ��ͨ��ӷ�Ӷ
        /// </summary>
        /// <returns></returns>
        public decimal? PuJ { get; set; }
        /// <summary>
        /// �ƽ��ӷ�Ӷ
        /// </summary>
        /// <returns></returns>
        public decimal? HuangJ { get; set; }
        /// <summary>
        /// ��ʯ��ӷ�Ӷ
        /// </summary>
        /// <returns></returns>
        public decimal? ZuanJ { get; set; }
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