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
    /// �� �ڣ�2020-10-03 16:08
    /// �� �������˶���
    /// </summary>
    public class OrdersJMService : RepositoryFactory<OrdersJMEntity>, OrdersJMIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        public IEnumerable<OrdersJMEntity> GetPageList(Pagination pagination, string queryJson)
        {
            string strSql = $"select * from OrdersJM where 1=1 ";
            var expression = LinqExtensions.True<OrdersJMEntity>();
            var queryParam = queryJson.ToJObject();
            //��������
            if (!queryParam["StartTime"].IsEmpty() && !queryParam["EndTime"].IsEmpty())
            {
                DateTime startTime = queryParam["StartTime"].ToDate();
                DateTime endTime = queryParam["EndTime"].ToDate().AddDays(1);
                strSql += " and created_at BETWEEN '" + startTime + "' and '" + endTime + "'";
            }
            //����
            if (!queryParam["agent_name"].IsEmpty())
            {
                string agent_name = queryParam["agent_name"].ToString();
                strSql += " and nickname = '%" + agent_name + "%'";
            }
            //����id
            if (!queryParam["agent_id"].IsEmpty())
            {
                string agent_id = queryParam["agent_id"].ToString();
                strSql += " and agent_id = " + agent_id;
            }
            //����
            if (!queryParam["orderno"].IsEmpty())
            {
                string orderno = queryParam["orderno"].ToString();
                strSql += " and OrderSn = '" + orderno + "'";
            }
            //����
            if (!queryParam["OrderSn"].IsEmpty())
            {
                string orderno = queryParam["OrderSn"].ToString();
                strSql += " and OrderSn = '" + orderno + "'";
            }
            return this.BaseRepository().FindList(strSql.ToString(), pagination);
        }
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<OrdersJMEntity> GetList(string queryJson)
        {
            string strSql = $"select * from OrdersJM where 1=1 ";
            var expression = LinqExtensions.True<OrdersJMEntity>();
            var queryParam = queryJson.ToJObject();
            //��������
            if (!queryParam["StartTime"].IsEmpty() && !queryParam["EndTime"].IsEmpty())
            {
                DateTime startTime = queryParam["StartTime"].ToDate();
                DateTime endTime = queryParam["EndTime"].ToDate().AddDays(1);
                strSql += " and created_at BETWEEN '" + startTime + "' and '" + endTime + "'";
            }
            //����
            if (!queryParam["agent_name"].IsEmpty())
            {
                string agent_name = queryParam["agent_name"].ToString();
                strSql += " and nickname = '%" + agent_name + "%'";
            }
            //����id
            if (!queryParam["agent_id"].IsEmpty())
            {
                string agent_id = queryParam["agent_id"].ToString();
                strSql += " and agent_id = " + agent_id;
            }
            //����
            if (!queryParam["orderno"].IsEmpty())
            {
                string orderno = queryParam["orderno"].ToString();
                strSql += " and OrderSn = '" + orderno + "'";
            }
            //����
            if (!queryParam["OrderSn"].IsEmpty())
            {
                string orderno = queryParam["OrderSn"].ToString();
                strSql += " and OrderSn = '" + orderno + "'";
            }
            return this.BaseRepository().FindList(strSql.ToString());
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public OrdersJMEntity GetEntity(int? keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="OrderSn">����ֵ</param>
        /// <returns></returns>
        public OrdersJMEntity GetEntityByOrderSn(string OrderSn)
        {
            return this.BaseRepository().FindEntity(t => t.OrderSn == OrderSn);
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
        public OrdersJMEntity SaveForm(int? keyValue, OrdersJMEntity entity)
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
            return entity;
        }
        #endregion
    }
}
