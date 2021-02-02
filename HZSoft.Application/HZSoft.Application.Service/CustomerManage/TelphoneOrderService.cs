using HZSoft.Application.Code;
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
    /// �� �ڣ�2017-09-22 15:43
    /// �� �������붩��
    /// </summary>
    public class TelphoneOrderService : RepositoryFactory<TelphoneOrderEntity>, TelphoneOrderIService
    {
        private ICodeRuleService coderuleService = new CodeRuleService();
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        public IEnumerable<TelphoneOrderEntity> GetPageList(Pagination pagination, string queryJson)
        {
            var expression = LinqExtensions.True<TelphoneOrderEntity>();
            var queryParam = queryJson.ToJObject();
            string strSql = "select * from TelphoneOrder where 1=1 ";
            //��������
            if (!queryParam["StartTime"].IsEmpty() && !queryParam["EndTime"].IsEmpty())
            {
                DateTime startTime = queryParam["StartTime"].ToDate();
                DateTime endTime = queryParam["EndTime"].ToDate().AddDays(1);
                strSql += " and CreateDate BETWEEN '" + startTime + "' and '" + endTime + "'";
            }
            //���ݱ��
            if (!queryParam["Telphone"].IsEmpty())
            {
                string Telphone = queryParam["Telphone"].ToString();
                strSql += " and Telphone = '" + Telphone + "'";
            }
            //������
            //if (!queryParam["SellerId"].IsEmpty())
            //{
            //    string SellerId = queryParam["SellerId"].ToString();
            //    strSql += " and SellerId = '" + SellerId + "'";
            //}
            //else
            //{
            //    if (!OperatorProvider.Provider.Current().IsSystem)
            //    {
            //        string dataAutor = string.Format(OperatorProvider.Provider.Current().DataAuthorize.ReadAutorize, OperatorProvider.Provider.Current().UserId);
            //        strSql += " and SellerId in (" + dataAutor + ")";
            //    }
            //}

            //��ϵ��
            if (!queryParam["Consignee"].IsEmpty())
            {
                string Consignee = queryParam["Consignee"].ToString();
                strSql += " and Consignee = '" + Consignee + "'";
            }

            //���
            if (!queryParam["CheckMark"].IsEmpty())
            {
                int CheckMark = queryParam["CheckMark"].ToInt();
                strSql += " and CheckMark = " + CheckMark;
            }

            //����
            if (!queryParam["DeleteMark"].IsEmpty())
            {
                int DeleteMark = queryParam["DeleteMark"].ToInt();
                strSql += " and DeleteMark = " + DeleteMark;
            }

            //����״̬
            if (!queryParam["SendMark"].IsEmpty())
            {
                int SendMark = queryParam["SendMark"].ToInt();
                strSql += " and SendMark = " + SendMark;
            }

            return this.BaseRepository().FindList(strSql.ToString(), pagination);
        }
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<TelphoneOrderEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// ���µ�10����¼
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TelphoneOrderEntity> GetListTop10()
        {
            return this.BaseRepository().IQueryable().OrderByDescending(t => t.CreateDate).Take(10).ToList();
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public TelphoneOrderEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }
        public TelphoneOrderEntity GetEntityByNu(string Nu)
        {
            return this.BaseRepository().FindEntity(t => t.Numbers == Nu);
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="Telphone">����ֵ</param>
        /// <returns></returns>
        public TelphoneOrderEntity GetEntityByTelphone(string Telphone)
        {
            return this.BaseRepository().FindEntity(t => t.Telphone == Telphone && t.DeleteMark == 0);
        }
        #endregion

        #region �ύ����
        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="keyValue">����</param>
        public void RemoveForm(string keyValue)
        {
            IRepository db = new RepositoryFactory().BaseRepository().BeginTrans();
            try
            {
                TelphoneOrderEntity entity = this.BaseRepository().FindEntity(keyValue);
                var telphone_Data = db.FindEntity<TelphonePuEntity>(t => t.Telphone == entity.Telphone);
                if (telphone_Data != null)
                {
                    telphone_Data.SellMark = 0;
                    telphone_Data.SellerId = "";
                    telphone_Data.SellerName = "";
                    telphone_Data.Description = "";
                    telphone_Data.Modify(telphone_Data.TelphoneID);
                    db.Update(telphone_Data);
                }
                db.Commit();
                this.BaseRepository().Delete(keyValue);

            }
            catch (Exception)
            {
                db.Rollback();
                throw;
            }
        }
        /// <summary>
        /// �޸�����״̬
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        public void SaveStateForm(string keyValue, TelphoneOrderEntity entity)
        {
            IRepository db = new RepositoryFactory().BaseRepository().BeginTrans();
            if (!string.IsNullOrEmpty(keyValue))
            {
                this.BaseRepository().Update(entity);
            }
        }

        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, TelphoneOrderEntity entity)
        {
            IRepository db = new RepositoryFactory().BaseRepository().BeginTrans();

            if (!string.IsNullOrEmpty(keyValue))
            {
                entity.Modify(keyValue);
                this.BaseRepository().Update(entity);
            }
            else
            {
                try
                {
                    entity.Create();
                    this.BaseRepository().Insert(entity);
                    //�޸ĺ�����к�����۳�״̬
                    var telphone_Data = db.FindEntity<TelphonePuEntity>(t => t.Telphone == entity.Telphone);
                    if (telphone_Data != null)
                    {
                        telphone_Data.SellMark = 1;
                        telphone_Data.SellerId = entity.SellerId;
                        telphone_Data.SellerName = entity.SellerName;
                        telphone_Data.Description += "|" + entity.SellerName + "��Ԥ��";
                        telphone_Data.Modify(telphone_Data.TelphoneID);
                        db.Update(telphone_Data);
                    }
                    //ռ�õ��ݺ�
                    coderuleService.UseRuleSeed("c576c3f7-631d-4108-baaf-1495bdc0d6bb", db);
                    //coderuleService.UseRuleSeed(entity.CreateUserId, "", ((int)CodeRuleEnum.Telphone_OrderCode).ToString(), db);//ռ�õ��ݺ�
                    db.Commit();
                }
                catch (Exception)
                {
                    db.Rollback();
                    throw;
                }
            }
        }


        /// <summary>
        /// �˵�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="State">״̬��1-������0-����</param>
        public void UpdateCheckState(string keyValue, int State)
        {
            TelphoneOrderEntity reserveEntity = new TelphoneOrderEntity();
            reserveEntity.Modify(keyValue);
            reserveEntity.CheckMark = State;
            this.BaseRepository().Update(reserveEntity);
        }

        /// <summary>
        /// ����:��ԭδԤ��״̬
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="State">״̬��1-������0-����</param>
        public void UpdateDeleteState(string keyValue, int State)
        {
            IRepository db = new RepositoryFactory().BaseRepository().BeginTrans();

            TelphoneOrderEntity reserveEntity = new TelphoneOrderEntity();
            reserveEntity.Modify(keyValue);
            reserveEntity.DeleteMark = State;
            db.Update(reserveEntity);

            //�޸ĺ�����к�����۳�״̬
            TelphoneOrderEntity entity = this.BaseRepository().FindEntity(keyValue);
            var telphone_Data = db.FindEntity<TelphonePuEntity>(t => t.Telphone == entity.Telphone);
            if (telphone_Data != null)
            {
                telphone_Data.SellMark = 0;
                telphone_Data.SellerId = "";
                telphone_Data.SellerName = "";
                telphone_Data.Description = "";
                telphone_Data.Modify(telphone_Data.TelphoneID);
                db.Update(telphone_Data);
            }
            db.Commit();
        }
        #endregion
    }
}
