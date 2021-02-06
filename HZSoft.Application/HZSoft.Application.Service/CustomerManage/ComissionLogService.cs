using HZSoft.Application.Code;
using HZSoft.Application.Entity.CustomerManage;
using HZSoft.Application.IService.CustomerManage;
using HZSoft.Data.Repository;
using HZSoft.Util;
using HZSoft.Util.Extension;
using HZSoft.Util.WebControl;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HZSoft.Application.Service.CustomerManage
{
    /// <summary>
    /// �� �� 6.1
    /// 
    /// �� ������������Ա
    /// �� �ڣ�2020-10-08 14:07
    /// �� ����Ӷ���
    /// </summary>
    public class ComissionLogService : RepositoryFactory<ComissionLogEntity>, ComissionLogIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        public IEnumerable<ComissionLogEntity> GetPageList(Pagination pagination, string queryJson)
        {
            string strSql = $"select * from ComissionLog where profit IS NOT NULL ";
            var expression = LinqExtensions.True<ComissionLogEntity>();
            var queryParam = queryJson.ToJObject();
            //��������
            if (!queryParam["StartTime"].IsEmpty() && !queryParam["EndTime"].IsEmpty())
            {
                DateTime startTime = queryParam["StartTime"].ToDate();
                DateTime endTime = queryParam["EndTime"].ToDate().AddDays(1);
                strSql += " and created_at BETWEEN '" + startTime + "' and '" + endTime + "'";
            }
            //����
            if (!queryParam["agent_id"].IsEmpty())
            {
                string agent_id = queryParam["agent_id"].ToString();
                strSql += " and agent_id = " + agent_id;
            }
            else
            {
                if (!OperatorProvider.Provider.Current().IsSystem && OperatorProvider.Provider.Current().UserId != "3303254b-7cd3-4a25-abd3-bb2542a08df9")//������Բ鿴�����к���
                {
                    string companyId = OperatorProvider.Provider.Current().CompanyId;
                    strSql += " and agent_id in (SELECT id FROM Wechat_Agent WHERE OrganizeId='" + companyId + "')";
                }
            }
            //����
            if (!queryParam["agent_name"].IsEmpty())
            {
                string agent_name = queryParam["agent_name"].ToString();
                strSql += " and agent_name = '%" + agent_name + "%'";
            }
            //����id
            if (!queryParam["ordernoid"].IsEmpty())
            {
                string ordernoid = queryParam["ordernoid"].ToString();
                strSql += " and ordernoid = " + ordernoid;
            }
            //����
            if (!queryParam["orderno"].IsEmpty())
            {
                string orderno = queryParam["orderno"].ToString();
                strSql += " and orderno = '" + orderno + "'";
            }
            //��������
            if (!queryParam["invited_agent_id"].IsEmpty())
            {
                string invited_agent_id = queryParam["invited_agent_id"].ToString();
                strSql += " and invited_agent_id = " + invited_agent_id;
            }
            //״̬
            if (!queryParam["status"].IsEmpty())
            {
                string status = queryParam["status"].ToString();
                strSql += " and status = " + status;
            }
            return this.BaseRepository().FindList(strSql.ToString(), pagination);
        }
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<ComissionLogEntity> GetList(string queryJson)
        {
            string strSql = $"select * from ComissionLog where  profit IS NOT NULL ";
            var expression = LinqExtensions.True<ComissionLogEntity>();
            var queryParam = queryJson.ToJObject();
            //����
            if (!queryParam["orderno"].IsEmpty())
            {
                string orderno = queryParam["orderno"].ToString();
                strSql += " and orderno = '" + orderno + "'";
            }
            //״̬
            if (!queryParam["status"].IsEmpty())
            {
                string status = queryParam["status"].ToString();
                strSql += " and status = " + status;
            }
            return this.BaseRepository().FindList(strSql.ToString());
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public ComissionLogEntity GetEntity(int? keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }
        #endregion

        #region �ύ����
        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="keyValue">����</param>
        public void RemoveForm(int? keyValue)
        {
            this.BaseRepository().Delete(keyValue);
        }
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        public void SaveForm(int? keyValue, ComissionLogEntity entity)
        {
            if (!string.IsNullOrEmpty(keyValue.ToString()))
            {
                entity.Modify(keyValue);
                this.BaseRepository().Update(entity);
            }
            else
            {
                entity.Create();
                this.BaseRepository().Insert(entity);
            }
        }
        #endregion
    }
}
