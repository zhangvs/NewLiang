using HZSoft.Application.Entity.CustomerManage;
using HZSoft.Application.IService.CustomerManage;
using HZSoft.Application.Service.CustomerManage;
using HZSoft.Util.WebControl;
using System.Collections.Generic;
using System;

namespace HZSoft.Application.Busines.CustomerManage
{
    /// <summary>
    /// �� �� 6.1
    /// 
    /// �� ������������Ա
    /// �� �ڣ�2018-07-25 11:57
    /// �� ����ԤԼ����
    /// </summary>
    public class TelphoneReserverBLL
    {
        private TelphoneReserverIService service = new TelphoneReserverService();

        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        public IEnumerable<TelphoneReserverEntity> GetPageList(Pagination pagination, string queryJson)
        {
            return service.GetPageList(pagination, queryJson);
        }
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<TelphoneReserverEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        public IEnumerable<TelphoneReserverEntity> GetListTop10()
        {
            return service.GetListTop10();
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public TelphoneReserverEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }
        public TelphoneReserverEntity GetEntityByReserve(string Reserve)
        {
            return service.GetEntityByReserve(Reserve);
        }
        #endregion

        #region �ύ����
        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="keyValue">����</param>
        public void RemoveForm(string keyValue)
        {
            try
            {
                service.RemoveForm(keyValue);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, TelphoneReserverEntity entity)
        {
            try
            {
                service.SaveForm(keyValue, entity);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        /// <summary>
        /// �޸ĺ˵�״̬
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="State">״̬��1-������0-����</param>
        public void UpdateCheckState(string keyValue, int State)
        {
            try
            {
                service.UpdateCheckState(keyValue, State);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// ����
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="State">״̬��1-������0-����</param>
        public void UpdateDeleteState(string keyValue, int State)
        {
            try
            {
                service.UpdateDeleteState(keyValue, State);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
