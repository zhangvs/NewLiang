using HZSoft.Application.Entity.BaseManage;
using HZSoft.Application.Entity.CustomerManage;
using HZSoft.Application.IService.BaseManage;
using HZSoft.Data.Repository;
using HZSoft.Util;
using HZSoft.Util.Extension;
using HZSoft.Util.WebControl;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace HZSoft.Application.Service.BaseManage
{
    /// <summary>
    /// �� �� 6.1
    /// 
    /// �� ������������Ա
    /// �� �ڣ�2017-10-12 17:30
    /// �� ����TelphoneCertification
    /// </summary>
    public class TelphoneCertificationService : RepositoryFactory<TelphoneCertificationEntity>, TelphoneCertificationIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        public IEnumerable<TelphoneCertificationEntity> GetPageList(Pagination pagination, string queryJson)
        {
            var queryParam = queryJson.ToJObject();
            string strSql = "select * from TelphoneCertification where 1=1 ";
            //���ݱ��
            if (!queryParam["custName"].IsEmpty())
            {
                string custName = queryParam["custName"].ToString();
                strSql += " and custName = '" + custName + "'";
            }
            return this.BaseRepository().FindList(strSql.ToString(), pagination);
        }
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<TelphoneCertificationEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public TelphoneCertificationEntity GetEntity(string keyValue)
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
            //ɾ������Ƭ
            TelphoneCertificationEntity oldEntity = this.BaseRepository().FindEntity(keyValue);
            DirFileHelper.DeleteFile(oldEntity.photo_z);
            DirFileHelper.DeleteFile(oldEntity.photo_b);
            DirFileHelper.DeleteFile(oldEntity.photo_s);
            this.BaseRepository().Delete(keyValue);
        }
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        public string SaveForm(string keyValue, TelphoneCertificationEntity entity)
        {
            IRepository db = new RepositoryFactory().BaseRepository().BeginTrans();
            SmsSingleSenderResult singleResult;
            SmsSingleSender singleSender = new SmsSingleSender(1400040861, "a92c87d0d291698777a9b5f323c0388a");

            TelphonePuEntity puData=db.FindEntity<TelphonePuEntity>(t => t.Telphone == entity.mobileNumber);
            TelphoneLiangEntity liangData = db.FindEntity<TelphoneLiangEntity>(t => t.Telphone == entity.mobileNumber);
            if (puData == null && liangData == null)
            {
                return "Error����֤���ֻ����벻���ڣ�";
            }
            if (!string.IsNullOrEmpty(keyValue))
            {
                //���Ͷ���
                if (entity.sendMessage==1)
                {
                    TelphoneCertificationEntity oldEntity = this.BaseRepository().FindEntity(keyValue);
                    if (oldEntity.loadMark == 0 && entity.loadMark == 2)
                    {
                        List<string> templParams = new List<string>();
                        templParams.Add(entity.mobileNumber);
                        //�ɹ�                
                        singleResult = singleSender.SendWithParam("86", entity.custTelphone, 172611, templParams, "", "", "");
                        //΢������ģ��:���ͨ��
                        WechatHelper.SendToOK(oldEntity.createId);
                    }
                    else if (oldEntity.loadMark == 0 && entity.loadMark == 1)
                    {
                        List<string> templParams = new List<string>();
                        templParams.Add(entity.mobileNumber);
                        templParams.Add(entity.description);
                        //ʧ��
                        singleResult = singleSender.SendWithParam("86", entity.custTelphone, 172613, templParams, "", "", "");
                        //΢������ģ�壺��˲�ͨ��
                        WechatHelper.SendToFail(oldEntity.createId,entity.description);
                    }
                }
                entity.Modify(keyValue);
                this.BaseRepository().Update(entity);
                return "OK�������ɹ�";
            }
            else
            {
                //�Ѿ��ύ���ĺ��룬����
                var CertificationData = db.FindEntity<TelphoneCertificationEntity>(t => t.mobileNumber == entity.mobileNumber);
                if (CertificationData!=null)
                {
                    if (CertificationData.loadMark==2)
                    {
                        return "Error���ú����Ѿ���������ٴ��ύ��";
                    }
                    else if (CertificationData.loadMark == 0)
                    {
                        return "Error��������֤�У��������ĵȴ���֤�������Ҫ�ظ��ύ��";
                    }
                    else
                    {
                        //ʧ�ܵ��ٴ���
                        entity.loadCount= CertificationData.loadCount+1;//�ύ������1
                        entity.loadMark = 0;
                        entity.description = "";
                        entity.Modify(CertificationData.ID);
                        this.BaseRepository().Update(entity);
                        //ɾ������Ƭ
                        DirFileHelper.DeleteFile(CertificationData.photo_z);
                        DirFileHelper.DeleteFile(CertificationData.photo_b);
                        DirFileHelper.DeleteFile(CertificationData.photo_s);
                        return "OK���ٴ��ϴ��ɹ��������ĵȴ���˽������";
                    }
                }
                else
                {
                    entity.Create();
                    this.BaseRepository().Insert(entity);
                    return "OK���ύ�ɹ��������ĵȴ���˽������";
                }

            }
        }
        #endregion
    }
}
