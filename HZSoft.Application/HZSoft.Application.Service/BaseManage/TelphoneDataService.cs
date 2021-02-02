using HZSoft.Application.Entity.BaseManage;
using HZSoft.Application.Entity.CustomerManage;
using HZSoft.Application.IService.BaseManage;
using HZSoft.Application.IService.CustomerManage;
using HZSoft.Application.Service.CustomerManage;
using HZSoft.Data.Repository;
using HZSoft.Util;
using HZSoft.Util.Extension;
using HZSoft.Util.WebControl;
using System.Collections.Generic;
using System.Linq;

namespace HZSoft.Application.Service.BaseManage
{
    /// <summary>
    /// �� �� 6.1
    /// 
    /// �� ������������Ա
    /// �� �ڣ�2017-09-19 17:45
    /// �� ���������
    /// </summary>
    public class TelphoneDataService : RepositoryFactory<TelphoneDataEntity>, TelphoneDataIService
    {
        private TelphoneLiangH5IService telphoneLiangH5IService = new TelphoneLiangH5Service();
        private TelphoneLiangIService telphoneLiangIService = new TelphoneLiangService();
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        public IEnumerable<TelphoneDataEntity> GetPageList(Pagination pagination, string queryJson)
        {
            var queryParam = queryJson.ToJObject();
            string strSql = "select * from TelphoneData where 1=1 ";
            //���ݱ��
            if (!queryParam["Number7"].IsEmpty())
            {
                string Number7 = queryParam["Number7"].ToString();
                strSql += " and Number7 = '" + Number7 + "'";
            }
            return this.BaseRepository().FindList(strSql.ToString(), pagination);
        }
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<TelphoneDataEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public TelphoneDataEntity GetEntity(int? keyValue)
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
        public void SaveForm(int? keyValue, TelphoneDataEntity entity)
        {
            if (keyValue != null)
            {
                IRepository db = new RepositoryFactory().BaseRepository().BeginTrans();
                TelphoneDataEntity oldEntity = GetEntity(keyValue);
                //���Ʒ����Ϣ
                if (oldEntity.Brand==null && entity.Brand!=null)
                {
                    //�޸�ͷ�����ſ�Ʒ��
                    var listH5 = db.FindList<TelphoneLiangH5Entity>(t => t.Telphone.Contains(entity.Number7) && t.DeleteMark!=1);
                    foreach (var item in listH5)
                    {
                        item.Brand = entity.Brand;
                        db.Update<TelphoneLiangH5Entity>(item);
                    }
                    //�޸����ſ�Ʒ��
                    var list = db.FindList<TelphoneLiangEntity>(t => t.Telphone.Contains(entity.Number7) && t.DeleteMark != 1);
                    foreach (var item in list)
                    {
                        item.Brand = entity.Brand;
                        db.Update<TelphoneLiangEntity>(item);
                    }
                }
                entity.Modify(keyValue);
                db.Update<TelphoneDataEntity>(entity);
                db.Commit();
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
