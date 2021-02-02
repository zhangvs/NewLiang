using HZSoft.Application.Code;
using HZSoft.Application.Entity.BaseManage;
using HZSoft.Application.Entity.CustomerManage;
using HZSoft.Application.IService.BaseManage;
using HZSoft.Application.IService.CustomerManage;
using HZSoft.Application.Service.BaseManage;
using HZSoft.Data.Repository;
using HZSoft.Util;
using HZSoft.Util.Extension;
using HZSoft.Util.WebControl;
using System.Collections.Generic;
using System.Linq;

namespace HZSoft.Application.Service.CustomerManage
{
    /// <summary>
    /// �� �� 6.1
    /// 
    /// �� ������������Ա
    /// �� �ڣ�2018-08-23 16:22
    /// �� ����������ҳ���
    /// </summary>
    public class TelphoneLiangSeeService : RepositoryFactory<TelphoneLiangSeeEntity>, TelphoneLiangSeeIService
    {
        private IOrganizeService orgService = new OrganizeService();
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        public IEnumerable<TelphoneLiangSeeEntity> GetPageList(Pagination pagination, string queryJson)
        {
            var expression = LinqExtensions.True<TelphoneLiangSeeEntity>();
            var queryParam = queryJson.ToJObject();
            string strSql = "select * from TelphoneLiangSee where 1=1";
            //�ͻ�����
            if (!queryParam["Keyword"].IsEmpty())
            {
                string Keyword = queryParam["Keyword"].ToString();
                strSql += " and NickName like '%" + Keyword + "%'";
            }
            //if (!queryParam["OrganizeId"].IsEmpty())
            //{
            //    string OrganizeId = queryParam["OrganizeId"].ToString();
            //    strSql += " and OrganizeId = '" + OrganizeId + "'";
            //}
            //else
            //{

            //}            
            if (!OperatorProvider.Provider.Current().IsSystem)
            {
                string companyId = OperatorProvider.Provider.Current().CompanyId;
                if (!string.IsNullOrEmpty(companyId))
                {
                    //strSql += " and OrganizeId IN( select OrganizeId from Base_Organize where ParentId='" + companyId +
                    //    "' OR OrganizeId='" + companyId + "')";
                    strSql += " and OrganizeId ='" + companyId + "'";
                }
            }
            return this.BaseRepository().FindList(strSql.ToString(), pagination);
        }
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<TelphoneLiangSeeEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public TelphoneLiangSeeEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }
        #endregion

        #region �ύ����
        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="keyValue">����</param>
        public void RemoveForm(string keyValue)
        {
            this.BaseRepository().Delete(keyValue);
        }
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, TelphoneLiangSeeEntity entity)
        {
            IRepository db = new RepositoryFactory().BaseRepository().BeginTrans();
            if (!string.IsNullOrEmpty(keyValue))
            {
                entity.Modify(keyValue);
                db.Update(entity);
            }
            else
            {
                entity.Create();
                db.Insert(entity);
                //���+1
                orgService.UpdateSeeCount(entity.OrganizeId);
            }
            db.Commit();
        }
        #endregion
    }
}
