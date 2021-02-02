using HZSoft.Application.Code;
using HZSoft.Application.Entity.BaseManage;
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
    /// �� �ڣ�2020-10-16 15:40
    /// �� �������ּ�¼
    /// </summary>
    public class WithdrawLogService : RepositoryFactory<WithdrawLogEntity>, WithdrawLogIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        public IEnumerable<WithdrawLogEntity> GetPageList(Pagination pagination, string queryJson)
        {
            string strSql = $"select * from WithdrawLog where 1=1 ";//id, agent_id, agent_name, CONVERT(VARCHAR(100),date,111) date, amount, status, img, ModifyDate
            var expression = LinqExtensions.True<OrdersJMEntity>();
            var queryParam = queryJson.ToJObject();
            //��������
            if (!queryParam["StartTime"].IsEmpty() && !queryParam["EndTime"].IsEmpty())
            {
                DateTime startTime = queryParam["StartTime"].ToDate();
                DateTime endTime = queryParam["EndTime"].ToDate().AddDays(1);
                strSql += " and date BETWEEN '" + startTime + "' and '" + endTime + "'";
            }
            //����
            if (!queryParam["agent_name"].IsEmpty())
            {
                string agent_name = queryParam["agent_name"].ToString();
                strSql += " and agent_name = '%" + agent_name+"%'";
            }
            //����id
            if (!queryParam["agent_id"].IsEmpty())
            {
                string agent_id = queryParam["agent_id"].ToString();
                strSql += " and agent_id = " + agent_id;
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
        public IEnumerable<WithdrawLogEntity> GetList(string queryJson)
        {
            string strSql = $"select * from WithdrawLog where 1=1 ";
            var expression = LinqExtensions.True<OrdersJMEntity>();
            var queryParam = queryJson.ToJObject();
            //��������
            if (!queryParam["StartTime"].IsEmpty() && !queryParam["EndTime"].IsEmpty())
            {
                DateTime startTime = queryParam["StartTime"].ToDate();
                DateTime endTime = queryParam["EndTime"].ToDate().AddDays(1);
                strSql += " and date BETWEEN '" + startTime + "' and '" + endTime + "'";
            }
            //����
            if (!queryParam["agent_id"].IsEmpty())
            {
                string agent_id = queryParam["agent_id"].ToString();
                strSql += " and agent_id = " + agent_id;
            }
            //״̬
            if (!queryParam["status"].IsEmpty())
            {
                string status = queryParam["status"].ToString();
                strSql += " and status = " + status;
            }
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public WithdrawLogEntity GetEntity(int? keyValue)
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
        public void SaveForm(int? keyValue, WithdrawLogEntity entity)
        {
            if (!string.IsNullOrEmpty(keyValue.ToString()))
            {
                var oldEntity = GetEntity(keyValue);
                entity.Modify(keyValue);
                this.BaseRepository().Update(entity);

                //�����ж�[{id: 0, title: '�����'}, {id: 1, title: '��ת��'}, {id: 2, title: '�Ѿܾ�'}],
                if (entity.status == 1 && oldEntity.status != 1)
                {
                    IRepository db = new RepositoryFactory().BaseRepository().BeginTrans();
                    var agent = db.FindEntity<Wechat_AgentEntity>(t => t.Id == entity.agent_id);
                    if (agent != null)
                    {
                        agent.Cashout -= entity.amount;//������-
                        agent.Cashouted += entity.amount;//���ֽ���+
                        db.Update<Wechat_AgentEntity>(agent);//���´����
                        db.Commit();
                        //��̨����Ҫ���������棬�޸Ļ���12СʱΪ1Сʱ
                        //OperatorAgentProvider.Provider.Current().profit = agent.profit;//�޸ĵ�ǰ��¼�߻���
                        //OperatorAgentProvider.Provider.Current().Cashout = agent.Cashout;//�޸ĵ�ǰ��¼�߻���
                    }
                }
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
