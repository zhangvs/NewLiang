using HZSoft.Application.Entity.BaseManage;
using HZSoft.Application.IService.BaseManage;
using HZSoft.Application.Service.BaseManage;
using HZSoft.Util.WebControl;
using System.Collections.Generic;
using System;

namespace HZSoft.Application.Busines.BaseManage
{
    /// <summary>
    /// �� �� 6.1
    /// 
    /// �� ������������Ա
    /// �� �ڣ�2020-09-29 17:02
    /// �� �������˴���
    /// </summary>
    public class Wechat_AgentBLL
    {
        private Wechat_AgentIService service = new Wechat_AgentService();

        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        public IEnumerable<Wechat_AgentEntity> GetPageList(Pagination pagination, string queryJson)
        {
            return service.GetPageList(pagination, queryJson);
        }
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<Wechat_AgentEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public Wechat_AgentEntity GetEntity(int? keyValue)
        {
            return service.GetEntity(keyValue);
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public Wechat_AgentEntity GetEntityByOpenId(string OpenId)
        {
            return service.GetEntityByOpenId(OpenId);
        }
        public IEnumerable<Wechat_AgentEntity> GetSumItem(int? pid)
        {
            return service.GetSumItem(pid);

        }
        #endregion

        #region �ύ����
        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="keyValue">����</param>
        public void RemoveForm(int? keyValue)
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
        public void SaveForm(int? keyValue, Wechat_AgentEntity entity)
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
        /// <summary>
        /// ���������
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public void SeeCountAdd(int? keyValue)
        {
            try
            {
                service.SeeCountAdd(keyValue);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// ������������
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public void FuDongUpdate(int? keyValue, int? fuDong)
        {
            try
            {
                service.FuDongUpdate(keyValue, fuDong);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
