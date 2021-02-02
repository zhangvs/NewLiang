using HZSoft.Application.Entity.CustomerManage;
using HZSoft.Application.IService.CustomerManage;
using HZSoft.Data.Repository;
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
    /// �� �ڣ�2020-03-05 10:43
    /// �� �������������
    /// </summary>
    public class TelphoneVipShareService : RepositoryFactory<TelphoneVipShareEntity>, TelphoneVipShareIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        public IEnumerable<TelphoneVipShareEntity> GetPageList(Pagination pagination, string queryJson)
        {
            return this.BaseRepository().FindList(pagination);
        }
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<TelphoneVipShareEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public TelphoneVipShareEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        /// <summary>
        /// ��ȡ��Ա�б�
        /// </summary>
        /// <param name="vipId">����Id</param>
        /// <returns></returns>
        public IEnumerable<TelphoneVipShareEntity> GetVipList(string vipId)
        {
            return this.BaseRepository().IQueryable(t => t.VipId == vipId).OrderByDescending(t => t.CreateDate).ToList();
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
        public void SaveForm(string keyValue, TelphoneVipShareEntity entity)
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
        /// <summary>
        /// ��ӳ�Ա
        /// </summary>
        /// <param name="vipId">����Id</param>
        /// <param name="shareIds">��ԱId</param>
        public void SaveMember(string vipId, string[] shareIds)
        {
            IRepository db = new RepositoryFactory().BaseRepository().BeginTrans();
            try
            {
                db.Delete<TelphoneVipShareEntity>(t => t.VipId == vipId);
                int SortCode = 1;
                foreach (string item in shareIds)
                {
                    TelphoneVipShareEntity vipShareEntity = new TelphoneVipShareEntity();
                    vipShareEntity.Create();
                    vipShareEntity.VipId = vipId;
                    vipShareEntity.ShareId = item;
                    vipShareEntity.SortCode = SortCode++;
                    db.Insert(vipShareEntity);
                }
                db.Commit();
            }
            catch (Exception)
            {
                db.Rollback();
                throw;
            }
        }
        #endregion
    }
}
