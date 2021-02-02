using HZSoft.Application.Entity.CustomerManage;
using HZSoft.Application.IService.CustomerManage;
using HZSoft.Application.Service.CustomerManage;
using HZSoft.Util.WebControl;
using System.Collections.Generic;
using System;
using HZSoft.Cache.Factory;

namespace HZSoft.Application.Busines.CustomerManage
{
    /// <summary>
    /// �� �� 6.1
    /// 
    /// �� ������������Ա
    /// �� �ڣ�2018-09-05 17:58
    /// �� �������ż��˴���
    /// </summary>
    public class TelphoneLiangJoinBLL
    {
        private TelphoneLiangJoinIService service = new TelphoneLiangJoinService();

        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        public IEnumerable<TelphoneLiangJoinEntity> GetPageList(Pagination pagination, string queryJson)
        {
            return service.GetPageList(pagination, queryJson);
        }
        public IEnumerable<TelphoneLiangJoinEntity> GetPageList1(Pagination pagination, string queryJson)
        {
            return service.GetPageList1(pagination, queryJson);
        }
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        public IEnumerable<TelphoneLiangJoinEntity> GetPageList2(Pagination pagination, string queryJson)
        {
            return service.GetPageList2(pagination, queryJson);
        }
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<TelphoneLiangJoinEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public TelphoneLiangJoinEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }
        public bool NotExistTelphone(string telphone)
        {
            return service.NotExistTelphone(telphone);
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
        public void SaveForm(string keyValue, TelphoneLiangJoinEntity entity)
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
        public string UpdateCheckState(TelphoneLiangJoinEntity entity, int State)
        {
            try
            {
                string msg= service.UpdateCheckState(entity, State);
                CacheFactory.Cache().RemoveCache("OrganizeCache");
                CacheFactory.Cache().RemoveCache("DepartmentCache");
                CacheFactory.Cache().RemoveCache("RoleCache");
                CacheFactory.Cache().RemoveCache("userCache");
                return msg;
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// �޸�0������
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="State">״̬��1-������0-����</param>
        public string UpdateTopOrg(TelphoneLiangJoinEntity entity, int State)
        {
            try
            {
                string msg = service.UpdateTopOrg(entity, State);
                CacheFactory.Cache().RemoveCache("OrganizeCache");
                CacheFactory.Cache().RemoveCache("DepartmentCache");
                CacheFactory.Cache().RemoveCache("RoleCache");
                CacheFactory.Cache().RemoveCache("userCache");
                return msg;
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
