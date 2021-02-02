using HZSoft.Application.Code;
using HZSoft.Application.Entity.BaseManage;
using HZSoft.Application.Entity.CustomerManage;
using HZSoft.Application.IService.CustomerManage;
using HZSoft.Data.Repository;
using HZSoft.Util;
using HZSoft.Util.Extension;
using HZSoft.Util.WebControl;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace HZSoft.Application.Service.CustomerManage
{
    /// <summary>
    /// �� �� 6.1
    /// 
    /// �� ������������Ա
    /// �� �ڣ�2018-10-09 20:14
    /// �� �����������ſ�
    /// </summary>
    public class TelphoneLiangOtherService : RepositoryFactory<TelphoneLiangOtherEntity>, TelphoneLiangOtherIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        public IEnumerable<TelphoneLiangOtherEntity> GetPageList(Pagination pagination, string queryJson)
        {
            var queryParam = queryJson.ToJObject();

            string companyId = OperatorProvider.Provider.Current().CompanyId;
            IRepository db = new RepositoryFactory().BaseRepository().BeginTrans();
            var organizeData = db.FindEntity<OrganizeEntity>(t => t.OrganizeId == companyId);
            if (!OperatorProvider.Provider.Current().IsSystem)
            {
                if (!string.IsNullOrEmpty(organizeData.OtherOrganizeId))
                {
                    //Ĭ��EnabledMark =1Ϊ�ϼ�SELECT top 100 percent * FROM (
                    string strSql = "SELECT TelphoneID,Telphone,EnabledMark,ExistMark,Grade,Operator,City,Price,MinPrice,ChaPrice,ModifyUserName,ModifyDate,Package,CreateOrganizeId FROM TelphoneLiangOther" +
                                        " WHERE CreateOrganizeId='" + companyId + "'" +
                                        //�������ۻ��������к���
                                        " UNION ALL " +
                                        " select TelphoneID,Telphone,EnabledMark,ExistMark,Grade,Operator,City,Price,MinPrice,ChaPrice,ModifyUserName,ModifyDate,Package,'' CreateOrganizeId " +
                                        " from TelphoneLiang where DeleteMark <> 1 and EnabledMark <> 1 and SellMark <> 1  and OrganizeId='" + organizeData.OtherOrganizeId + "' " +
                                        " and telphone not in (select Telphone from TelphoneLiangOther where CreateOrganizeId in ('" + organizeData.ParentId + "','" + organizeData.TopOrganizeId + "') ) ";
                    //���ݱ��
                    if (!queryParam["Telphone"].IsEmpty())
                    {
                        string Telphone = queryParam["Telphone"].ToString();
                        strSql += " and Telphone like '%" + Telphone + "%'";
                    }
                    else
                    {
                        //����TelphoneLiangOther����
                        strSql += " AND telphone NOT IN (SELECT Telphone FROM TelphoneLiangOther WHERE CreateOrganizeId='" + companyId + "')";
                    }
                    //����
                    if (!queryParam["Grade"].IsEmpty())
                    {
                        string Grade = queryParam["Grade"].ToString();
                        strSql += " and Grade = '" + Grade + "'";
                    }
                    //����Ӫ��
                    if (!queryParam["City"].IsEmpty())
                    {
                        string City = queryParam["City"].ToString();
                        strSql += " and City = '" + City + "'";
                    }
                    //�ϼܱ�ʶ
                    if (!queryParam["EnabledMark"].IsEmpty())
                    {
                        string EnabledMark = queryParam["EnabledMark"].ToString();
                        strSql += " and EnabledMark = " + EnabledMark;
                    }
                    return this.BaseRepository().FindList(strSql.ToString(), pagination);
                }
                else
                {
                    return null;
                }
            }
            else
            {
                string strSql = "SELECT TelphoneID,Telphone,EnabledMark,ExistMark,Grade,Operator,City,Price,MinPrice,ChaPrice,ModifyUserName,ModifyDate,Package,CreateOrganizeId FROM TelphoneLiangOther";
                return this.BaseRepository().FindList(strSql.ToString(), pagination);
            }
        }

        /// <summary>
        /// �����ִ��ۺ�������
        /// </summary>
        /// <param name="organizeId">����</param>
        /// <returns>���ط�ҳ�б�</returns>
        public DataTable GetLiangOtherCountByOrg(string organizeId)
        {
            if (string.IsNullOrEmpty(organizeId))
            {
                return null;
            }
            else
            {
                string strSql = $"select count(1) from TelphoneLiangOther where CreateOrganizeId='{organizeId}'";
                return this.BaseRepository().FindTable(strSql.ToString());
            }

        }

        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<TelphoneLiangOtherEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public TelphoneLiangOtherEntity GetEntity(string keyValue)
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
        public void SaveForm(string keyValue, TelphoneLiangOtherEntity entity)
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

        #region �жϺ�������
        /// <summary>
        /// ����������
        /// </summary>
        /// <param name="rowsCount"></param>
        /// <returns></returns>
        public string IsGreater(int rowsCount)
        {
            IRepository db = new RepositoryFactory().BaseRepository().BeginTrans();
            string companyid = OperatorProvider.Provider.Current().CompanyId;
            var vipEntity = db.FindEntity<TelphoneLiangVipEntity>(t => t.OrganizeId == companyid && t.VipEndDate > DateTime.Now && t.DeleteMark != 1);
            if (vipEntity==null)
            {
                return "�����������������롿Ȩ��δ��ͨ���ѹ���,����ϵ����Ա��ͨ��";
            }
            else
            {
                int otherMax = Convert.ToInt32(vipEntity.OtherMax);//�������ϴ�����
                DataTable dt = GetLiangOtherCountByOrg(companyid);
                int nowLiangCount = Convert.ToInt32(dt.Rows[0][0].ToString());
                if (nowLiangCount + rowsCount > otherMax)
                {
                    return $"���ۺ����ѳ�������{otherMax}������ϵ����Ա׷�Ӻ������ޣ�";
                }
                else
                {
                    return "";
                }
            }
        }
        #endregion

        /// <summary>
        /// �����ϼ�
        /// </summary>
        /// <param name="upTelphones">����</param>
        public string UpsForm(string upTelphones)
        {
            if (!string.IsNullOrEmpty(upTelphones))
            {

                IRepository db = new RepositoryFactory().BaseRepository().BeginTrans();
                string[] custIds = upTelphones.Split(',');
                int rowsCount = custIds.Length;
                //�жϺ�������
                string greaterMsg = IsGreater(rowsCount);
                if (!string.IsNullOrEmpty(greaterMsg))
                {
                    //throw new Exception(greaterMsg);
                    return greaterMsg;
                }
                
                for (int i = 0; i < rowsCount; i++)
                {
                    int? id = Convert.ToInt32(custIds[i]);
                    var liangEntity = db.FindEntity<TelphoneLiangEntity>(t => t.TelphoneID == id);
                    if (liangEntity != null)
                    {
                        TelphoneLiangOtherEntity entity = new TelphoneLiangOtherEntity()
                        {
                            Telphone = liangEntity.Telphone,
                            EnabledMark = liangEntity.EnabledMark,
                            ExistMark = liangEntity.ExistMark,
                            Grade = liangEntity.Grade,
                            City = liangEntity.City,
                            Operator = liangEntity.Operator,
                            Price = liangEntity.Price,
                            MinPrice = liangEntity.MinPrice,
                            ChaPrice = liangEntity.ChaPrice,
                            ModifyUserName = liangEntity.ModifyUserName,
                            ModifyDate = liangEntity.ModifyDate,
                            Package = liangEntity.Package,
                            OrganizeId = liangEntity.OrganizeId
                        };
                        entity.Create();
                        this.BaseRepository().Insert(entity);
                    }

                }
                return "�����ɹ�";
            }
            else
            {
                return "δѡ���ϼܺ��룡";
            }
        }


        /// <summary>
        /// �����¼�
        /// </summary>
        /// <param name="downTelphones">����</param>
        public void DownsForm(string downTelphones)
        {
            if (!string.IsNullOrEmpty(downTelphones))
            {
                string[] custIds = downTelphones.Split(',');
                for (int i = 0; i < custIds.Length; i++)
                {
                    int? id = Convert.ToInt32(custIds[i]);
                    TelphoneLiangOtherEntity entity = this.BaseRepository().FindEntity(id);
                    if (entity!=null)
                    {
                        this.BaseRepository().Delete(entity);
                    }
                    
                }
            }
        }
        #endregion
    }
}
