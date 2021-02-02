using HZSoft.Application.Entity.CustomerManage;
using HZSoft.Util.WebControl;
using System.Collections.Generic;

namespace HZSoft.Application.IService.CustomerManage
{
    /// <summary>
    /// �� �� 6.1
    /// 
    /// �� ������������Ա
    /// �� �ڣ�2018-09-05 17:58
    /// �� �������ż��˴���
    /// </summary>
    public interface TelphoneLiangJoinIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        IEnumerable<TelphoneLiangJoinEntity> GetPageList(Pagination pagination, string queryJson);
        IEnumerable<TelphoneLiangJoinEntity> GetPageList1(Pagination pagination, string queryJson);
        IEnumerable<TelphoneLiangJoinEntity> GetPageList2(Pagination pagination, string queryJson);
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        IEnumerable<TelphoneLiangJoinEntity> GetList(string queryJson);
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        TelphoneLiangJoinEntity GetEntity(string keyValue);
        bool NotExistTelphone(string telphone);
        #endregion

        #region �ύ����
        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="keyValue">����</param>
        void RemoveForm(string keyValue);
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        void SaveForm(string keyValue, TelphoneLiangJoinEntity entity);
        #endregion

        
        /// <summary>
        /// �˵�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="State">״̬��1-������0-����</param>
        string UpdateCheckState(TelphoneLiangJoinEntity entity, int State);

        /// <summary>
        /// ����0����Ӧ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="State">״̬��1-������0-����</param>
        string UpdateTopOrg(TelphoneLiangJoinEntity entity, int State);

        /// <summary>
        /// ����
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="State">״̬��1-������0-����</param>
        void UpdateDeleteState(string keyValue, int State);
    }
}
