using HZSoft.Application.Entity.CustomerManage;
using HZSoft.Util.WebControl;
using System.Collections.Generic;
using System.Data;

namespace HZSoft.Application.IService.CustomerManage
{
    /// <summary>
    /// �� �� 6.1
    /// 
    /// �� ������������Ա
    /// �� �ڣ�2020-06-07 14:11
    /// �� ����ͷ�����ſ�
    /// </summary>
    public interface TelphoneLiangH5IService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        IEnumerable<TelphoneLiangH5Entity> GetPageList(Pagination pagination, string queryJson);
        IEnumerable<TelphoneLiangH5Entity> GetPageListGongHai(Pagination pagination, string queryJson);
        IEnumerable<TelphoneLiangH5Entity> GetPageListH5LX(Pagination pagination, string queryJson);
        IEnumerable<TelphoneLiangH5Entity> GetPageListH5LX_JS(Pagination pagination, string queryJson);
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        IEnumerable<TelphoneLiangH5Entity> GetList(string queryJson);
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        TelphoneLiangH5Entity GetEntity(int? keyValue);
        #endregion

        #region �ύ����
        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="keyValue">����</param>
        void RemoveForm(string keyValues);
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        void SaveForm(int? keyValue, TelphoneLiangH5Entity entity);
        /// <summary>
        /// ������������
        /// </summary>
        /// <param name="dtSource">ʵ�����</param>
        /// <returns></returns>
        string BatchAddEntity(DataTable dtSource);
        /// <summary>
        /// ������ɾ����
        /// </summary>
        /// <param name="dtSource">ʵ�����</param>
        /// <returns></returns>
        string BatchDeleteEntity(DataTable dtSource);
        //�����¼�
        string DownTelphone(string downTelphones);
        //��������
        string PriceTelphone(string priceTelphones);
        /// <summary>
        /// �ϼ�����
        /// </summary>
        /// <param name="keyValue">����</param>
        void UpForm(string keyValues);
        /// <summary>
        /// �ֿ�����
        /// </summary>
        /// <param name="keyValue">����</param>
        void ExistForm(string keyValues);
        /// <summary>
        /// ��ɱ����
        /// </summary>
        /// <param name="keyValue">����</param>
        void MiaoShaForm(string keyValues);
        #endregion
    }
}
