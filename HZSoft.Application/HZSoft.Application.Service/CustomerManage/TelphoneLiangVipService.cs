using HZSoft.Application.Entity.AuthorizeManage;
using HZSoft.Application.Entity.BaseManage;
using HZSoft.Application.Entity.CustomerManage;
using HZSoft.Application.IService.CustomerManage;
using HZSoft.Application.Service.BaseManage;
using HZSoft.Cache.Redis;
using HZSoft.Data.Repository;
using HZSoft.Util.Extension;
using HZSoft.Util.WebControl;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;

namespace HZSoft.Application.Service.CustomerManage
{
    /// <summary>
    /// �� �� 6.1
    /// 
    /// �� ������������Ա
    /// �� �ڣ�2018-10-17 21:56
    /// �� ����VIP�������
    /// </summary>
    public class TelphoneLiangVipService : RepositoryFactory<TelphoneLiangVipEntity>, TelphoneLiangVipIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        public IEnumerable<TelphoneLiangVipEntity> GetPageList(Pagination pagination, string queryJson)
        {
            Expression<Func<TelphoneLiangVipEntity, bool>> expression = LinqExtensions.True<TelphoneLiangVipEntity>();
            expression = expression.And(t => t.DeleteMark != 1);
            return this.BaseRepository().FindList(expression,pagination);
        }
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<TelphoneLiangVipEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// �жϵ�ǰ�����Ƿ���
        /// </summary>
        /// <param name="organizeId"></param>
        /// <returns></returns>
        public bool GetVipByOrganizeId(string organizeId)
        {
            int count = this.BaseRepository().IQueryable(t => t.OrganizeId == organizeId && t.VipEndDate > DateTime.Now).Count();
            if (count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// �жϻ����Ƿ����ڣ����ۻ�����Χ
        /// </summary>
        /// <param name="organizeId"></param>
        /// <param name="pid"></param>
        /// <param name="top"></param>
        /// <returns></returns>
        public List<string> GetVipOrgList(string organizeId, string pid, string top)
        {
            List<string> vipList = new List<string>();
            //�ж�vip�������
            //1.������
            if (GetVipByOrganizeId(organizeId))
            {
                vipList.Add(organizeId);
            }
            else
            {
                organizeId = "";
            }
            //2.����ֱ���ϼ�
            if (!string.IsNullOrEmpty(pid) && organizeId != pid)
            {
                if (GetVipByOrganizeId(pid))
                {
                    vipList.Add(pid);
                }
                else
                {
                    pid = "";
                }
            }
            else
            {
                pid = "";
            }
            // 3.��������
            if (!string.IsNullOrEmpty(top) && top != organizeId && top != pid)
            {
                if (GetVipByOrganizeId(top))
                {
                    vipList.Add(top);
                }
            }

            return vipList;
        }

        /// <summary>
        /// �жϵ�ǰ�����Ƿ���빲��ƽ̨
        /// </summary>
        /// <param name="organizeId"></param>
        /// <returns></returns>
        public bool IsShareMark(string organizeId)
        {
            int count = this.BaseRepository().IQueryable(t => t.OrganizeId == organizeId && t.ShareMark == 1).Count();
            if (count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public TelphoneLiangVipEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }
        /// <summary>
        /// �û��б�
        /// </summary>
        /// <returns></returns>
        public DataTable GetTable()
        {
            return this.BaseRepository().FindTable("SELECT * FROM TelphoneLiangVip");
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
        public void SaveForm(string keyValue, TelphoneLiangVipEntity entity)
        {
            IRepository db = new RepositoryFactory().BaseRepository().BeginTrans();
            if (!string.IsNullOrEmpty(keyValue))
            {
                var oldEntity = db.FindEntity<TelphoneLiangVipEntity>(t => t.Id == keyValue);
                //���빲��ƽ̨
                if (entity.ShareMark == 1 && oldEntity.ShareMark != 1)
                {
                    string shareOrg = RedisCache.Get<string>("ShareOrg");
                    if (!string.IsNullOrEmpty(shareOrg))
                    {
                        if (!shareOrg.Contains(entity.OrganizeId))
                        {
                            RedisCache.Set<string>("ShareOrg", shareOrg + ",'" + entity.OrganizeId + "'");
                        }
                    }
                }

                //ȡ������
                if (entity.ShareMark != 1 && oldEntity.ShareMark == 1)
                {
                    string shareOrg = RedisCache.Get<string>("ShareOrg");
                    if (!string.IsNullOrEmpty(shareOrg))
                    {
                        RedisCache.Set<string>("ShareOrg", shareOrg.Replace(",'" + entity.OrganizeId + "'", ""));
                    }
                }

                entity.Modify(keyValue);
                this.BaseRepository().Update(entity);
            }
            else
            {
                entity.Create();
                this.BaseRepository().Insert(entity);
                //�޸Ļ������е�vip��ʶλ
                var orgEntity = db.FindEntity<OrganizeEntity>(t => t.OrganizeId == entity.OrganizeId);
                orgEntity.VipMark = 1;
                db.Update(orgEntity);
                
                //���ݻ���id��ȡ��ɫid
                var roleEntity=db.FindEntity<RoleEntity>(t => t.OrganizeId == entity.OrganizeId);
                //����ϴ���������>0�����'���ſ�'�˵�,Base_Module�˵�id
                if (entity.UploadMax>0)
                {
                    var authorize = db.FindEntity<AuthorizeEntity>(t => t.ObjectId == roleEntity.RoleId && t.ItemId== "16cea332-0204-4f7d-a510-0426329205ff");
                    if (authorize==null)
                    {
                        AuthorizeEntity authorizeEntity = new AuthorizeEntity();
                        authorizeEntity.Create();
                        authorizeEntity.Category = 2;  //1 - ����2 - ��ɫ3 - ��λ4 - ְλ5 - ������
                        authorizeEntity.ObjectId = roleEntity.RoleId;//��ɫid����ɫ�޶��˻����Ͳ���
                        authorizeEntity.ItemType = 1; //��Ŀ����: 1 - �˵�2 - ��ť3 - ��ͼ4��
                        authorizeEntity.ItemId = "16cea332-0204-4f7d-a510-0426329205ff";//��Ŀ����
                        authorizeEntity.SortCode = 100;
                        db.Insert(authorizeEntity);
                    }
                }

                //������ۺ�������>0�����'�������ſ�'�˵�,Base_Module�˵�id
                if (entity.OtherMax>0)
                {
                    var authorize = db.FindEntity<AuthorizeEntity>(t => t.ObjectId == roleEntity.RoleId && t.ItemId == "effbaf18-e4d1-4250-9aa0-370422634a21");
                    if (authorize == null)
                    {
                        AuthorizeEntity authorizeEntity2 = new AuthorizeEntity();
                        authorizeEntity2.Create();
                        authorizeEntity2.Category = 2;  //1 - ����2 - ��ɫ3 - ��λ4 - ְλ5 - ������
                        authorizeEntity2.ObjectId = roleEntity.RoleId;//��ɫid����ɫ�޶��˻����Ͳ���
                        authorizeEntity2.ItemType = 1; //��Ŀ����: 1 - �˵�2 - ��ť3 - ��ͼ4��
                        authorizeEntity2.ItemId = "effbaf18-e4d1-4250-9aa0-370422634a21";//��Ŀ����
                        authorizeEntity2.SortCode = 100;
                        db.Insert(authorizeEntity2);
                    }
                }
                //����
                var authorize6 = db.FindEntity<AuthorizeEntity>(t => t.ObjectId == roleEntity.RoleId && t.ItemId == "6");
                if (authorize6 == null)
                {
                    AuthorizeEntity authorizeEntity2 = new AuthorizeEntity();
                    authorizeEntity2.Create();
                    authorizeEntity2.Category = 2;  //1 - ����2 - ��ɫ3 - ��λ4 - ְλ5 - ������
                    authorizeEntity2.ObjectId = roleEntity.RoleId;//��ɫid����ɫ�޶��˻����Ͳ���
                    authorizeEntity2.ItemType = 1; //��Ŀ����: 1 - �˵�2 - ��ť3 - ��ͼ4��
                    authorizeEntity2.ItemId = "6";//��Ŀ����
                    authorizeEntity2.SortCode = 12;
                    db.Insert(authorizeEntity2);
                }
                //�������۱���
                var authorize61 = db.FindEntity<AuthorizeEntity>(t => t.ObjectId == roleEntity.RoleId && t.ItemId == "44498bc7-33ac-418f-a0f8-c88241afa118");
                if (authorize61 == null)
                {
                    AuthorizeEntity authorizeEntity2 = new AuthorizeEntity();
                    authorizeEntity2.Create();
                    authorizeEntity2.Category = 2;  //1 - ����2 - ��ɫ3 - ��λ4 - ְλ5 - ������
                    authorizeEntity2.ObjectId = roleEntity.RoleId;//��ɫid����ɫ�޶��˻����Ͳ���
                    authorizeEntity2.ItemType = 1; //��Ŀ����: 1 - �˵�2 - ��ť3 - ��ͼ4��
                    authorizeEntity2.ItemId = "44498bc7-33ac-418f-a0f8-c88241afa118";//��Ŀ����
                    authorizeEntity2.SortCode = 13;
                    db.Insert(authorizeEntity2);
                }

                db.Commit();

                //��ӻ������ƽ̨
                if (entity.ShareMark==1)
                {
                    string shareOrg = RedisCache.Get<string>("ShareOrg");
                    if (!string.IsNullOrEmpty(shareOrg))
                    {
                        RedisCache.Set<string>("ShareOrg", shareOrg + ",'" + entity.OrganizeId + "'");
                    }
                }
            }
        }
        #endregion
    }
}
