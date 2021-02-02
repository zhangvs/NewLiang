using HZSoft.Application.Entity.WeChatManage;
using HZSoft.Application.IService.WeChatManage;
using HZSoft.Data.Repository;
using HZSoft.Util;
using HZSoft.Util.Extension;
using HZSoft.Util.WebControl;
using System.Collections.Generic;
using System.Linq;

namespace HZSoft.Application.Service.WeChatManage
{
    /// <summary>
    /// �� �� 6.1
    /// 
    /// �� ������������Ա
    /// �� �ڣ�2017-11-21 14:46
    /// �� �����û���
    /// </summary>
    public class WeChat_UsersService : RepositoryFactory<WeChat_UsersEntity>, WeChat_UsersIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        public IEnumerable<WeChat_UsersEntity> GetPageList(Pagination pagination, string queryJson)
        {
            var queryParam = queryJson.ToJObject();
            string strSql = "select * from WeChat_Users where 1=1";
            //���ݱ��
            if (!queryParam["Keyword"].IsEmpty())
            {
                string NickName = queryParam["Keyword"].ToString();
                strSql += " and NickName like '%" + NickName + "%'";
            }
            return this.BaseRepository().FindList(strSql.ToString(), pagination);
        }
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="Keyword">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<WeChat_UsersEntity> GetList(string Keyword)
        {
            string strSql = "select * from WeChat_Users where 1=1";

            //���ݱ��
            if (!Keyword.IsEmpty())
            {
                strSql += " and NickName like '%" + Keyword + "%' and AppName='"+ Config.GetValue("AppName")+"'";
            }
            strSql += "  ORDER BY CreateDate desc";
            return this.BaseRepository().FindList(strSql.ToString());
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public WeChat_UsersEntity GetEntity(string keyValue)
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
        public void SaveForm(string keyValue, WeChat_UsersEntity entity)
        {
            if (!string.IsNullOrEmpty(keyValue))
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
