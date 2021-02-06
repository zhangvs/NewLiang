using HZSoft.Application.Code;
using HZSoft.Application.Entity.BaseManage;
using HZSoft.Application.IService.BaseManage;
using HZSoft.Data.Repository;
using HZSoft.Util;
using HZSoft.Util.Extension;
using HZSoft.Util.WebControl;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HZSoft.Application.Service.BaseManage
{
    /// <summary>
    /// �� �� 6.1
    /// 
    /// �� ������������Ա
    /// �� �ڣ�2020-09-29 17:02
    /// �� �������˴���
    /// </summary>
    public class Wechat_AgentService : RepositoryFactory<Wechat_AgentEntity>, Wechat_AgentIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        public IEnumerable<Wechat_AgentEntity> GetPageList(Pagination pagination, string queryJson)
        {
            string strSql = "select * from Wechat_Agent where DeleteMark <> 1 ";
            var expression = LinqExtensions.True<Wechat_AgentEntity>();
            var queryParam = queryJson.ToJObject();
            //��������
            if (!queryParam["StartTime"].IsEmpty() && !queryParam["EndTime"].IsEmpty())
            {
                DateTime startTime = queryParam["StartTime"].ToDate();
                DateTime endTime = queryParam["EndTime"].ToDate().AddDays(1);
                strSql += " and CreateDate >= '" + startTime + "' and CreateDate < '" + endTime + "'";
            }
            //���
            if (!queryParam["Id"].IsEmpty())
            {
                string Id = queryParam["Id"].ToString();
                strSql += " and Id = " + Id ;
            }
            //΢���ǳ�
            if (!queryParam["nickname"].IsEmpty())
            {
                string nickname = queryParam["nickname"].ToString();
                strSql += " and nickname like '%" + nickname + "%'";
            }
            //�ֻ�
            if (!queryParam["contact"].IsEmpty())
            {
                string contact = queryParam["contact"].ToString();
                strSql += " and contact like '%" + contact + "%'";
            }
            //֧����
            if (!queryParam["alipay"].IsEmpty())
            {
                string alipay = queryParam["alipay"].ToString();
                strSql += " and alipay like '%" + alipay + "%'";
            }
            //��ʵ����
            if (!queryParam["realname"].IsEmpty())
            {
                string realname = queryParam["realname"].ToString();
                strSql += " and realname like '%" + realname + "%'";
            }
            //����
            if (!queryParam["LV"].IsEmpty())
            {
                string LV = queryParam["LV"].ToString();
                strSql += " and LV  = '" + LV+"'";
            }
            if (!OperatorProvider.Provider.Current().IsSystem && OperatorProvider.Provider.Current().UserId != "3303254b-7cd3-4a25-abd3-bb2542a08df9")//������Բ鿴�����к���
            {
                string OrganizeId = OperatorProvider.Provider.Current().CompanyId;
                strSql += " and OrganizeId  = '" + OrganizeId + "'";
            }
            return this.BaseRepository().FindList(strSql.ToString(), pagination);
        }
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<Wechat_AgentEntity> GetList(string queryJson)
        {
            string strSql = "select * from Wechat_Agent where DeleteMark <> 1 ";
            var expression = LinqExtensions.True<Wechat_AgentEntity>();
            var queryParam = queryJson.ToJObject();
            //��������
            if (!queryParam["StartTime"].IsEmpty() && !queryParam["EndTime"].IsEmpty())
            {
                DateTime startTime = queryParam["StartTime"].ToDate();
                DateTime endTime = queryParam["EndTime"].ToDate().AddDays(1);
                strSql += " and CreateDate >= '" + startTime + "' and CreateDate < '" + endTime + "'";
            }
            //΢�ű�ʶ
            if (!queryParam["OpenId"].IsEmpty())
            {
                string OpenId = queryParam["OpenId"].ToString();
                strSql += " and OpenId = '" + OpenId + "'";
            }
            //΢���ǳ�
            if (!queryParam["NickName"].IsEmpty())
            {
                string NickName = queryParam["NickName"].ToString();
                strSql += " and NickName like '%" + NickName + "%'";
            }
            //�ֻ�
            if (!queryParam["Tel"].IsEmpty())
            {
                string Tel = queryParam["Tel"].ToString();
                strSql += " and Tel like '%" + Tel + "%'";
            }
            //֧����
            if (!queryParam["Zfb"].IsEmpty())
            {
                string Zfb = queryParam["Zfb"].ToString();
                strSql += " and Zfb like '%" + Zfb + "%'";
            }
            //��ʵ����
            if (!queryParam["RealName"].IsEmpty())
            {
                string RealName = queryParam["RealName"].ToString();
                strSql += " and RealName like '%" + RealName + "%'";
            }
            //����
            if (!queryParam["LV"].IsEmpty())
            {
                int LV = queryParam["LV"].ToInt();
                strSql += " and LV  = " + LV;
            }
            //��
            if (!queryParam["Pid"].IsEmpty())
            {
                int Pid = queryParam["Pid"].ToInt();
                strSql += " and Pid  = " + Pid;
            }
            return this.BaseRepository().FindList(strSql.ToString());
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public Wechat_AgentEntity GetEntity(int? keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="OpenId">����ֵ</param>
        /// <returns></returns>
        public Wechat_AgentEntity GetEntityByOpenId(string OpenId)
        {
            return this.BaseRepository().FindEntity(t => t.OpenId == OpenId);
        }
        /// <summary>
        /// ��ȡ������ܣ�Ӷ���������¼�lv
        /// </summary>
        /// <param name="pid">����ֵ</param>
        /// <returns></returns>
        public IEnumerable<Wechat_AgentEntity> GetSumItem(int? pid)
        {
            return this.BaseRepository().FindList("select sum(profit) childprofit,count(*) childcount,lv from Wechat_Agent where DeleteMark <> 1 AND pid=" + pid + " GROUP BY lv");
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
        public void SaveForm(int? keyValue, Wechat_AgentEntity entity)
        {
            if (!string.IsNullOrEmpty(keyValue.ToString()))
            {
                entity.Modify(keyValue);
                this.BaseRepository().Update(entity);
            }
            else
            {
                //������ͨ�������ϼ����Ӵ򿪵ģ��ϼ���ͨ��������+1
                if (!string.IsNullOrEmpty(entity.Pid.ToString()))
                {
                    var pidEntity = GetEntity(entity.Pid);
                    if (pidEntity!=null)
                    {
                        this.BaseRepository().Update(pidEntity);
                        entity.Category = pidEntity.Category + 1;//������Ϊ�ϼ��ļ���+1
                        entity.OrganizeId = pidEntity.OrganizeId;//���û���idΪ�ϼ�����id
                    }
                    else
                    {
                        entity.Category = 0;//�����𣬶���Ϊ0��
                    }
                }

                entity.Create();
                this.BaseRepository().Insert(entity);
            }
        }
        #endregion
    }
}
