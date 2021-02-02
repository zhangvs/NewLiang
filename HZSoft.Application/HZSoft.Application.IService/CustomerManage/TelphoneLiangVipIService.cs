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
    /// �� �ڣ�2018-10-17 21:56
    /// �� ����VIP�������
    /// </summary>
    public interface TelphoneLiangVipIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        IEnumerable<TelphoneLiangVipEntity> GetPageList(Pagination pagination, string queryJson);
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        IEnumerable<TelphoneLiangVipEntity> GetList(string queryJson);
        /// <summary>
        /// �жϵ�ǰ�����Ƿ���
        /// </summary>
        /// <param name="organizeId"></param>
        /// <returns></returns>
        bool GetVipByOrganizeId(string organizeId);
        List<string> GetVipOrgList(string organizeId, string pid, string top);
        /// <summary>
        /// �жϵ�ǰ�����Ƿ���
        /// </summary>
        /// <param name="organizeId"></param>
        /// <returns></returns>
        bool IsShareMark(string organizeId);
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        TelphoneLiangVipEntity GetEntity(string keyValue);
        /// <summary>
        /// vip����
        /// </summary>
        /// <returns></returns>
        DataTable GetTable();
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
        void SaveForm(string keyValue, TelphoneLiangVipEntity entity);
        #endregion
    }
}
