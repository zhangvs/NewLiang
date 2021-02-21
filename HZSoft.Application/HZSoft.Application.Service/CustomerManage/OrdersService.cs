using HZSoft.Application.Code;
using HZSoft.Application.Entity.BaseManage;
using HZSoft.Application.Entity.CustomerManage;
using HZSoft.Application.IService.CustomerManage;
using HZSoft.Application.IService.SystemManage;
using HZSoft.Application.Service.SystemManage;
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
    /// �� �ڣ�2020-04-16 09:07
    /// �� ����������
    /// </summary>
    public class OrdersService : RepositoryFactory<OrdersEntity>, OrdersIService
    {
        private ICodeRuleService coderuleService = new CodeRuleService();

        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        public IEnumerable<OrdersEntity> GetPageList(Pagination pagination, string queryJson)
        {
            string strSql = $"select * from Orders where (DeleteMark <> 1 or PayStatus = 1) ";
            var expression = LinqExtensions.True<OrdersEntity>();
            var queryParam = queryJson.ToJObject();
            //��������
            if (!queryParam["StartTime"].IsEmpty() && !queryParam["EndTime"].IsEmpty())
            {
                DateTime startTime = queryParam["StartTime"].ToDate();
                DateTime endTime = queryParam["EndTime"].ToDate().AddDays(1);
                strSql += " and CreateDate >= '" + startTime + "' and CreateDate < '" + endTime + "'";
            }
            //�۸����
            if (!queryParam["pricef"].IsEmpty())
            {
                string pricef = queryParam["pricef"].ToString();
                strSql += " and Price >= " + pricef;
            }
            //�۸�С��
            if (!queryParam["pricet"].IsEmpty())
            {
                string pricet = queryParam["pricet"].ToString();
                strSql += " and Price <= " + pricet;
            }

            //����
            if (!queryParam["OrderSn"].IsEmpty())
            {
                string OrderSn = queryParam["OrderSn"].ToString();
                strSql += " and OrderSn like '%" + OrderSn + "%'";
            }
            //����
            if (!queryParam["Tel"].IsEmpty())
            {
                string Tel = queryParam["Tel"].ToString();
                strSql += " and Tel like '%" + Tel + "%'";
            }
            //�������ǳ�
            if (!queryParam["Host"].IsEmpty())
            {
                string Host = queryParam["Host"].ToString();
                strSql += " and Host like '%" + Host + "%'";
            }
            //�ռ���
            if (!queryParam["Receiver"].IsEmpty())
            {
                string Receiver = queryParam["Receiver"].ToString();
                strSql += " and Receiver like '%" + Receiver + "%'";
            }
            //��ϵ�绰
            if (!queryParam["ContactTel"].IsEmpty())
            {
                string ContactTel = queryParam["ContactTel"].ToString();
                strSql += " and ContactTel like '%" + ContactTel + "%'";
            }
            //����״̬
            if (!queryParam["Status"].IsEmpty())
            {
                int Status = queryParam["Status"].ToInt();
                strSql += " and Status  = " + Status;
            }
            //֧��״̬
            if (!queryParam["PayStatus"].IsEmpty())
            {
                int PayStatus = queryParam["PayStatus"].ToInt();
                strSql += " and PayStatus  = " + PayStatus;
            }
            //����id
            if (!queryParam["AgentId"].IsEmpty())
            {
                int AgentId = queryParam["AgentId"].ToInt();
                strSql += " and AgentId  = " + AgentId;
            }
            //Ĭ��ֻ��ѯ��ǰ�����µĴ�����
            if (!OperatorProvider.Provider.Current().IsSystem && OperatorProvider.Provider.Current().UserId != "3303254b-7cd3-4a25-abd3-bb2542a08df9")//������Բ鿴�����к���
            {
                string companyId = OperatorProvider.Provider.Current().CompanyId;
                strSql += " and AgentId in (SELECT id FROM Wechat_Agent WHERE OrganizeId='" + companyId + "')";
            }
            return this.BaseRepository().FindList(strSql.ToString(), pagination);
        }
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<OrdersEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public OrdersEntity GetEntity(int? keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="OrderSn">����ֵ</param>
        /// <returns></returns>
        public OrdersEntity GetEntityByOrderSn(string OrderSn)
        {
            return this.BaseRepository().FindEntity(t => t.OrderSn == OrderSn);
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="tel">����ֵ</param>
        /// <returns></returns>
        public OrdersEntity GetEntityByTel(string tel)
        {
            return this.BaseRepository().FindEntity(t => t.Tel == tel);
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="tel">�������</param>
        /// <param name="contactTel">��ϵ�绰</param>
        /// <returns></returns>
        public OrdersEntity GetEntityByContactTel(string tel, string contactTel)
        {
            return this.BaseRepository().FindEntity(t => t.Tel == tel && t.ContactTel == contactTel);
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
            IRepository db = new RepositoryFactory().BaseRepository().BeginTrans();
            //ͬ��ɾ��Ӷ����־��
            var list = db.FindList<ComissionLogEntity>(t => t.orderid == keyValue);
            if (list.Count() > 0)
            {
                foreach (var item in list)
                {
                    db.Update<ComissionLogEntity>(item);
                }
                db.Commit();
            }

        }
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        public void SaveForm(int? keyValue, OrdersEntity entity)
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


        /// <summary>
        /// �������������
        /// </summary>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        public OrdersEntity SaveForm(OrdersEntity entity)
        {
            IRepository db = new RepositoryFactory().BaseRepository().BeginTrans();
            var list= db.FindList<OrdersEntity>(t => t.Tel == entity.Tel && (t.ContactTel == entity.ContactTel || t.ContactTel == null) && t.Status==0);
            if (list.Count()>0)
            {
                foreach (var item in list)
                {
                    item.DeleteMark = 1;
                    db.Update<OrdersEntity>(item);
                    LogHelper.AddLog("ɾ��δ�������" + item.OrderSn);
                }
                //db.Delete<OrdersEntity>(t => t.Tel == entity.Tel && t.ContactTel == entity.ContactTel && t.Status == 0);
                db.Commit();
            }

            entity.Create();
            if (string.IsNullOrEmpty(entity.OrderSn))
            {
                //jsapi����ǰ���ɶ�����ţ�ֱ���������ɵģ������ύ΢�ŵ�һ��
                entity.OrderSn = string.Format("{0}{1}", "LX-", DateTime.Now.ToString("yyyyMMddHHmmss"));//,TenPayV3Util.BuildRandomStr(6)
            }
            this.BaseRepository().Insert(entity);
            return entity;
        }




        /// <summary>
        /// ����
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        public void SaveSendForm(int? keyValue, OrdersEntity entity)
        {
            if (!string.IsNullOrEmpty(keyValue.ToString()))
            {
                entity.Modify(keyValue);
                entity.Status = 2;//����
                entity.DeliveryName = OperatorProvider.Provider.Current().UserName;
                entity.DeliveryDate = DateTime.Now;
                this.BaseRepository().Update(entity);
            }
        }
        /// <summary>
        /// ����
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public void UpdateSendState(int? keyValue)
        {
            IRepository db = new RepositoryFactory().BaseRepository().BeginTrans();
            try
            {
                if (!string.IsNullOrEmpty(keyValue.ToString()))
                {
                    OrdersEntity entity = GetEntity(keyValue);
                    entity.Modify(keyValue);
                    entity.Status = 3;//���������������
                    this.BaseRepository().Update(entity);

                    //����Ƿ�������������Ӷ������˲���
                    if (entity.OrderSn.Contains("FX-"))
                    {
                        var list = db.FindList<ComissionLogEntity>(t => t.orderno == entity.OrderSn);
                        if (list.Count() > 0)
                        {
                            //��Ҫ�ȵ�����֮���ٷ�Ӷ����
                            foreach (var item in list)
                            {
                                var agent = db.FindEntity<Wechat_AgentEntity>(t => t.Id == item.agent_id);
                                if (agent != null)
                                {
                                    agent.profit += item.profit;//���������Ӷ��
                                    agent.SellCount += entity.Price;//�������������
                                    item.status = 2;//������
                                    db.Update<Wechat_AgentEntity>(agent);//���´����
                                    db.Update<ComissionLogEntity>(item);//����Ӷ����־��
                                }
                            }
                            db.Commit();
                        }
                    }

                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
