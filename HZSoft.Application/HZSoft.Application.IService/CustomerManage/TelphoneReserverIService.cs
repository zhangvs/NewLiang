using HZSoft.Application.Entity.CustomerManage;
using HZSoft.Util.WebControl;
using System.Collections.Generic;

namespace HZSoft.Application.IService.CustomerManage
{
    /// <summary>
    /// �� �� 6.1
    /// 
    /// �� ������������Ա
    /// �� �ڣ�2018-07-25 11:57
    /// �� ����ԤԼ����
    /// </summary>
    public interface TelphoneReserverIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        IEnumerable<TelphoneReserverEntity> GetPageList(Pagination pagination, string queryJson);
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        IEnumerable<TelphoneReserverEntity> GetList(string queryJson);
        IEnumerable<TelphoneReserverEntity> GetListTop10();
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        TelphoneReserverEntity GetEntity(string keyValue);
        TelphoneReserverEntity GetEntityByReserve(string Reserve);
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
        void SaveForm(string keyValue, TelphoneReserverEntity entity);
        #endregion


        /// <summary>
        /// �˵�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="State">״̬��1-������0-����</param>
        void UpdateCheckState(string keyValue, int State);

        /// <summary>
        /// ����
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="State">״̬��1-������0-����</param>
        void UpdateDeleteState(string keyValue, int State);
    }
}
