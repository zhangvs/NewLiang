using HZSoft.Application.Entity.BaseManage;
using HZSoft.Application.Service.BaseManage;
using HZSoft.Application.Entity.CustomerManage;
using HZSoft.Application.IService.CustomerManage;
using HZSoft.Data.Repository;
using HZSoft.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using HZSoft.Application.Code;
using HZSoft.Util;
using HZSoft.Util.Extension;
using HZSoft.Application.IService.BaseManage;
using System;
using System.Data;

namespace HZSoft.Application.Service.CustomerManage
{
    /// <summary>
    /// �� �� 6.1
    /// 
    /// �� ������������Ա
    /// �� �ڣ�2018-09-05 17:58
    /// �� �������ż��˴���
    /// </summary>
    public class TelphoneLiangJoinService : RepositoryFactory<TelphoneLiangJoinEntity>, TelphoneLiangJoinIService
    {
        private IOrganizeService orgService = new OrganizeService();
        private TelphoneLiangVipIService telphoneLiangVipIService = new TelphoneLiangVipService();

        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        public IEnumerable<TelphoneLiangJoinEntity> GetPageList(Pagination pagination, string queryJson)
        {
            var expression = LinqExtensions.True<TelphoneLiangJoinEntity>();
            var queryParam = queryJson.ToJObject();
            string strSql = "select * from TelphoneLiangJoin where DeleteMark <> 1 ";

            //�ͻ�����
            if (!queryParam["Keyword"].IsEmpty())
            {
                string Keyword = queryParam["Keyword"].ToString();
                strSql += " and NickName like '%" + Keyword + "%'";
            }
            if (!queryParam["Telphone"].IsEmpty())
            {
                string Telphone = queryParam["Telphone"].ToString();
                strSql += " and Telphone = '" + Telphone + "'";
            }
            if (!queryParam["OrganizeId"].IsEmpty())
            {
                string OrganizeId = queryParam["OrganizeId"].ToString();
                strSql += " and OrganizeId = '" + OrganizeId + "'";
            }
            else
            {
                if (!OperatorProvider.Provider.Current().IsSystem)
                {
                    string companyId = OperatorProvider.Provider.Current().CompanyId;
                    if (!string.IsNullOrEmpty(companyId))
                    {
                        strSql += " and OrganizeId ='" + companyId + "' and BoosMark <> 1 ";//ֻ��ʾ��������ʾ��Ӧ��
                    }
                }
            }
            return this.BaseRepository().FindList(strSql.ToString(), pagination);
        }

        /// <summary>
        /// ���˴���鿴��Դ����
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        public IEnumerable<TelphoneLiangJoinEntity> GetPageList1(Pagination pagination, string queryJson)
        {
            var expression = LinqExtensions.True<TelphoneLiangJoinEntity>();
            var queryParam = queryJson.ToJObject();

            string strSql = @"select j.Id,j.CompanyName,j.FullName,j.Telphone,j.Pro,j.City,j.Area,j.Address,j.OpenId,j.NickName,
j.Sex,j.HeadimgUrl,j.WXPro,j.WxCity,j.WxAccount,j.WxQRcode,j.OrganizeId,j.BoosMark,j.LiangCount,j.AgentMark,j.TopMark,j.CheckMark,j.DeleteMark,j.Description,j.FollowDes,
j.CreateTime,j.ModifyTime,j.ModifyUserId,j.ModifyUserName,o.Category Des1,o.TopOrganizeId Des2 from TelphoneLiangJoin j
LEFT JOIN Base_Organize o ON j.OrganizeId = o.OrganizeId
where j.DeleteMark <> 1 and BoosMark <> 1";

            //�ͻ�����
            if (!queryParam["Keyword"].IsEmpty())
            {
                string Keyword = queryParam["Keyword"].ToString();
                strSql += " and NickName like '%" + Keyword + "%'";
            }
            if (!queryParam["Telphone"].IsEmpty())
            {
                string Telphone = queryParam["Telphone"].ToString();
                strSql += " and Telphone = '" + Telphone + "'";
            }
            if (!queryParam["CheckMark"].IsEmpty())
            {
                string CheckMark = queryParam["CheckMark"].ToString();
                strSql += " and j.CheckMark = '" + CheckMark + "'";
            }
            else
            {
                strSql += " and j.CheckMark = 0";//Ĭ����ʾδ���
            }
            if (!queryParam["Category"].IsEmpty())
            {
                string Category = queryParam["Category"].ToString();
                strSql += " and o.Category = '" + Category + "'";
            }

            if (!queryParam["OrganizeId"].IsEmpty())
            {
                string OrganizeId = queryParam["OrganizeId"].ToString();
                strSql += " and o.OrganizeId = '" + OrganizeId + "'";
            }
            else
            {
                if (!OperatorProvider.Provider.Current().IsSystem)
                {
                    string companyId = OperatorProvider.Provider.Current().CompanyId;
                    if (!string.IsNullOrEmpty(companyId))
                    {
                        strSql += " and j.OrganizeId ='" + companyId + "'";//ֻ��ʾ��������ʾ��Ӧ��
                    }
                }
            }
            return this.BaseRepository().FindList(strSql.ToString(), pagination);
        }
        /// <summary>
        /// ��Ӧ�̲鿴
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        public IEnumerable<TelphoneLiangJoinEntity> GetPageList2(Pagination pagination, string queryJson)
        {
            var expression = LinqExtensions.True<TelphoneLiangJoinEntity>();
            var queryParam = queryJson.ToJObject();
            string strSql = @"select j.Id,j.CompanyName,j.FullName,j.Telphone,j.Pro,j.City,j.Area,j.Address,j.OpenId,j.NickName,
j.Sex,j.HeadimgUrl,j.WXPro,j.WxCity,j.WxAccount,j.WxQRcode,j.OrganizeId,j.BoosMark,j.LiangCount,j.AgentMark,j.TopMark,j.CheckMark,j.DeleteMark,j.Description,
j.CreateTime,j.ModifyTime,j.ModifyUserId,j.ModifyUserName,o.Category Des1,o.TopOrganizeId Des2 from TelphoneLiangJoin j
LEFT JOIN Base_Organize o ON j.OrganizeId = o.OrganizeId
where j.DeleteMark <> 1 and BoosMark = 1";

            return this.BaseRepository().FindList(strSql.ToString(), pagination);
        }
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<TelphoneLiangJoinEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public TelphoneLiangJoinEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        /// <summary>
        /// �ֻ��Ų����ظ�
        /// </summary>
        /// <param name="telphone">��˾����</param>
        /// <returns></returns>
        public bool NotExistTelphone(string telphone)
        {
            var expression = LinqExtensions.True<TelphoneLiangJoinEntity>();
            expression = expression.And(t => t.Telphone == telphone);
            return this.BaseRepository().IQueryable(expression).Count() == 0 ? true : false;
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
        public void SaveForm(string keyValue, TelphoneLiangJoinEntity entity)
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
                //��Ӧ�̲����Ͷ�������
                if (entity.BoosMark != 1)
                {
                    //���������ϼ����
                    IRepository db = new RepositoryFactory().BaseRepository().BeginTrans();
                    var parentOrg = db.FindEntity<OrganizeEntity>(t => t.OrganizeId == entity.OrganizeId);
                    //ֻ���������ϣ�0��1��2�����Ͷ���
                    if (parentOrg != null && parentOrg.Category < 3)
                    {
                        SmsSingleSenderResult singleResult;
                        SmsSingleSender singleSender = new SmsSingleSender(1400040861, "a92c87d0d291698777a9b5f323c0388a");
                        List<string> templParams = new List<string>();
                        templParams.Add(parentOrg.FullName);
                        singleResult = singleSender.SendWithParam("86", parentOrg.OuterPhone, 205528, templParams, "", "", "");
                    }
                    //�������+1
                    orgService.UpdateJoinCount(entity.OrganizeId);
                }
                else
                {
                    SmsSingleSenderResult singleResult;
                    SmsSingleSender singleSender = new SmsSingleSender(1400040861, "a92c87d0d291698777a9b5f323c0388a");
                    List<string> templParams = new List<string>();
                    templParams.Add("����Ա");
                    singleResult = singleSender.SendWithParam("86", "18660996839", 205528, templParams, "", "", "");
                }
            }
        }

        /// <summary>
        /// �˵�
        /// </summary>
        /// <param name="entity">����ֵ</param>
        /// <param name="State">״̬��1-������0-����</param>
        public string UpdateTopOrg(TelphoneLiangJoinEntity entity, int State)
        {
            //��ȡ0������id
            IRepository db = new RepositoryFactory().BaseRepository().BeginTrans();

            //���������̳�ƽ̨
            OrganizeEntity orgEntity = new OrganizeEntity()
            {
                ParentId = "0",//0������
                FullName = entity.CompanyName,
                ShortName = entity.NickName,
                OuterPhone = entity.Telphone,
                InnerPhone = entity.WxAccount,//΢���˺�
                Nature = entity.WxQRcode,//΢�Ŷ�ά��
                ManagerId = entity.OpenId,
                Manager = entity.FullName,
                Layer = 1,
                DeleteMark = 0
            };
            OrganizeEntity newEntity = orgService.SaveReturnEntity(orgEntity);

            //��������״̬
            TelphoneLiangJoinEntity reserveEntity = new TelphoneLiangJoinEntity();
            reserveEntity.Modify(entity.Id);
            reserveEntity.TopMark = State;
            this.BaseRepository().Update(reserveEntity);

            //����vip����
            TelphoneLiangVipEntity telphoneLiangVipEntity = new TelphoneLiangVipEntity()
            {
                OrganizeId = newEntity.OrganizeId,
                FullName = newEntity.FullName,
                UploadMax = 1000,
                OtherMax = 0,
                OrgMax = 10,
                Price = 0,
                VipStartDate = DateTime.Now,
                VipEndDate = DateTime.Now.AddDays(7)
            };
            telphoneLiangVipIService.SaveForm(null, telphoneLiangVipEntity);

            //����ͨ������
            if (!string.IsNullOrEmpty(newEntity.Description))
            {
                SmsSingleSenderResult singleResult;
                SmsSingleSender singleSender = new SmsSingleSender(1400040861, "a92c87d0d291698777a9b5f323c0388a");
                List<string> templParams = new List<string>();
                templParams.Add(entity.FullName);
                templParams.Add(newEntity.Description);
                //�ɹ�                
                singleResult = singleSender.SendWithParam("86", entity.Telphone, 205617, templParams, "", "", "");
            }

            return $"��ͨ�����ŵķ�ʽ֪ͨ������¼����������̳�Ϊ��{newEntity.Description}";
        }

        /// <summary>
        /// �˵�
        /// </summary>
        /// <param name="entity">����ֵ</param>
        /// <param name="State">״̬��1-������0-����</param>
        public string UpdateCheckState(TelphoneLiangJoinEntity entity, int State)
        {
            //��ȡ0������id
            IRepository db = new RepositoryFactory().BaseRepository().BeginTrans();

            if (OperatorProvider.Provider.Current().Account != "System")
            {
                var checkEntity = db.FindEntity<OrganizeEntity>(t => t.OuterPhone == entity.Telphone);
                if (checkEntity != null)
                {
                    return "�������˵��ֻ����Ѿ�ͨ����ˣ���ˢ�»����б�鿴��";
                }

                var parentOrg0 = orgService.GetParentIdByOrgId(OperatorProvider.Provider.Current().CompanyId);//��õ�ǰ����������0������
                if (!string.IsNullOrEmpty(parentOrg0.First().OrganizeId))
                {
                    string parentOrg = parentOrg0.First().OrganizeId;
                    //��ȡ0��vip��
                    var vipEntity = db.FindEntity<TelphoneLiangVipEntity>(t => t.OrganizeId == parentOrg && t.VipEndDate > DateTime.Now);
                    if (vipEntity == null)
                    {
                        return "ƽ̨����δ���š�������ˡ����ܣ�";
                    }
                    else
                    {
                        int? orgMax = vipEntity.OrgMax;
                        DataTable dt = orgService.GetOrgCount(parentOrg0.First().OrganizeId);//��ȡ0����������
                        int parentOrgCount = Convert.ToInt32(dt.Rows[0][0].ToString());
                        //���0�����������������õĻ�������
                        if (parentOrgCount > orgMax)
                        {
                            return "ƽ̨�����ѳ������ޣ�����ϵ�ϼ����ӻ���������";
                        }
                    }
                }
            }

            if (State==2)
            {
                entity.OrganizeId = "e21c39be-da56-4f1c-9120-a0926a520947";//�޸���˻���Ϊ��
            }

            //�ж�2,3�������Ǹ�����

            //���������̳�ƽ̨
            OrganizeEntity orgEntity = new OrganizeEntity()
            {
                ParentId = entity.OrganizeId,
                FullName = entity.CompanyName,
                ShortName = entity.NickName,
                OuterPhone = entity.Telphone,
                InnerPhone = entity.WxAccount,//΢���˺�
                Nature = entity.WxQRcode,//΢�Ŷ�ά��
                ManagerId = entity.OpenId,
                Manager = entity.FullName,
                Layer = 1,
                DeleteMark = 0
            };
            OrganizeEntity newEntity = orgService.SaveReturnEntity(orgEntity);

            //��������״̬
            TelphoneLiangJoinEntity reserveEntity = new TelphoneLiangJoinEntity();
            reserveEntity.Modify(entity.Id);
            reserveEntity.CheckMark = State;
            reserveEntity.OrganizeId = entity.OrganizeId;//��˻���Ϊ��
            this.BaseRepository().Update(reserveEntity);


            //����ͨ������
            if (!string.IsNullOrEmpty(newEntity.Description))
            {
                SmsSingleSenderResult singleResult;
                SmsSingleSender singleSender = new SmsSingleSender(1400040861, "a92c87d0d291698777a9b5f323c0388a");
                List<string> templParams = new List<string>();
                templParams.Add(entity.FullName);
                templParams.Add(newEntity.Description);
                //�ɹ�                
                singleResult = singleSender.SendWithParam("86", entity.Telphone, 205617, templParams, "", "", "");
            }

            return $"��ͨ�����ŵķ�ʽ֪ͨ������¼����������̳�Ϊ��{newEntity.Description}";
        }

        /// <summary>
        /// ����
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="State">״̬��1-������0-����</param>
        public void UpdateDeleteState(string keyValue, int State)
        {
            TelphoneLiangJoinEntity reserveEntity = new TelphoneLiangJoinEntity();
            reserveEntity.Modify(keyValue);
            reserveEntity.DeleteMark = State;
            this.BaseRepository().Update(reserveEntity);
            var entity = this.BaseRepository().FindEntity(keyValue);
            if (entity != null)
            {
                SmsSingleSenderResult singleResult;
                SmsSingleSender singleSender = new SmsSingleSender(1400040861, "a92c87d0d291698777a9b5f323c0388a");
                List<string> templParams = new List<string>();
                templParams.Add(entity.FullName);
                //����ʧ������             
                singleResult = singleSender.SendWithParam("86", entity.Telphone, 205531, templParams, "", "", "");
            }
        }
        #endregion


    }
}
