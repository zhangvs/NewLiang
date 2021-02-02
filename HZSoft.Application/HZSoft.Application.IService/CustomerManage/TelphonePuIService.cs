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
    /// �� �ڣ�2017-10-23 14:11
    /// �� �������ſ�
    /// </summary>
    public interface TelphonePuIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        IEnumerable<TelphonePuEntity> GetPageList(Pagination pagination, string queryJson);
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        IEnumerable<TelphonePuEntity> GetList(string queryJson);
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        IEnumerable<TelphonePuEntity> GetList(string telphone, string organizeId, string city);
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        IEnumerable<TelphonePuEntity> GetGrade(string organizeId, string Grade, string city);
        IEnumerable<TelphonePuEntity> GetListEnd4(string end4);
        /// <summary>
        /// �Ƿ���Ȩ���ڵĺ���
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        IEnumerable<TelphonePuEntity> GetEntityByOrgTel(string organizeId, string telphone);
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        TelphonePuEntity GetEntity(int? keyValue);
        #endregion

        #region �ύ����
        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="keyValue">����</param>
        void RemoveForm(string keyValues);
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
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        void SaveForm(int? keyValue, TelphonePuEntity entity);
        /// <summary>
        /// ������������
        /// </summary>
        /// <param name="dtSource">ʵ�����</param>
        /// <returns></returns>
        string BatchAddEntity(DataTable dtSource);
        //�����¼�
        string DownTelphone(string downTelphones);
        //��������
        string PriceTelphone(string priceTelphones);
        #endregion
    }
}
