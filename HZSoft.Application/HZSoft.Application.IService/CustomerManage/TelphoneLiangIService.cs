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
    public interface TelphoneLiangIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        IEnumerable<TelphoneLiangEntity> GetPageList(Pagination pagination, string queryJson);
        IEnumerable<TelphoneLiangEntity> GetPageListJoin(Pagination pagination, string queryJson);
        IEnumerable<TelphoneLiangEntity> GetPageListH5(Pagination pagination, string queryJson);
        IEnumerable<TelphoneLiangEntity> GetPageListH5LX(Pagination pagination, string queryJson);
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        IEnumerable<TelphoneLiangEntity> GetList(string queryJson);
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        IEnumerable<TelphoneLiangEntity> GetList(string telphone, string organizeId, string city);
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        IEnumerable<TelphoneLiangEntity> GetGrade(string organizeId, string Grade, string city, int? ExistMark);
        /// <summary>
        /// ��������б������жϻ���
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        IEnumerable<TelphoneLiangEntity> GetEntityByTel(string organizeId, string telphone);
        /// <summary>
        /// �����ѯ��ť����������
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        IEnumerable<TelphoneLiangEntity> GetEntityByOrgTel(string organizeId, string telphone);
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        TelphoneLiangEntity GetEntity(int? keyValue);
        TelphoneLiangEntity GetEntityByOrgTel(string telphone);
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
        void SaveForm(int? keyValue, TelphoneLiangEntity entity);
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
