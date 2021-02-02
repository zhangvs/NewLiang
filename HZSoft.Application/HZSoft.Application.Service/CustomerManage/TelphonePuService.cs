using HZSoft.Application.Code;
using HZSoft.Application.Entity.BaseManage;
using HZSoft.Application.Entity.CustomerManage;
using HZSoft.Application.Entity.SystemManage;
using HZSoft.Application.IService.BaseManage;
using HZSoft.Application.IService.CustomerManage;
using HZSoft.Application.Service.BaseManage;
using HZSoft.Data.Repository;
using HZSoft.Util;
using HZSoft.Util.Extension;
using HZSoft.Util.WebControl;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;

namespace HZSoft.Application.Service.CustomerManage
{
    /// <summary>
    /// �� �� 6.1
    /// 
    /// �� ������������Ա
    /// �� �ڣ�2017-10-23 14:11
    /// �� �������ſ�
    /// </summary>
    public class TelphonePuService : RepositoryFactory<TelphonePuEntity>, TelphonePuIService
    {
        private IOrganizeService orgService = new OrganizeService();
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        public IEnumerable<TelphonePuEntity> GetPageList(Pagination pagination, string queryJson)
        {
            var queryParam = queryJson.ToJObject();
            string strSql = "select * from TelphonePu where DeleteMark <> 1 and EnabledMark <> 1";
            //���ݱ��
            if (!queryParam["Telphone"].IsEmpty())
            {
                string Telphone = queryParam["Telphone"].ToString();
                strSql += " and Telphone like '%" + Telphone + "%'";
            }
            //����
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
                    //һ���������Բ鿴�ϼ�0���ĺ���⣬��Ϊ�Լ���Ա��
                    strSql += " and OrganizeId IN( select OrganizeId from Base_Organize where OrganizeId='" + companyId
                        + "' or OrganizeId =(SELECT ParentId FROM Base_Organize WHERE OrganizeId='" + companyId + "'))";
                }
            }
            //����
            if (!queryParam["Grade"].IsEmpty())
            {
                string Grade = queryParam["Grade"].ToString();
                strSql += " and Grade like '%" + Grade + "%'";
            }
            //����Ӫ��
            if (!queryParam["City"].IsEmpty())
            {
                string City = queryParam["City"].ToString();
                strSql += " and City = '" + City + "'";
            }
            //�۳���ʶ
            if (!queryParam["SellMark"].IsEmpty())
            {
                string SellMark = queryParam["SellMark"].ToString();
                strSql += " and SellMark = " + SellMark;
            }
            return this.BaseRepository().FindList(strSql.ToString(), pagination);
        }
        /// <summary>
        /// �������ۺ�������
        /// </summary>
        /// <param name="organizeId">����</param>
        /// <returns>���ط�ҳ�б�</returns>
        public DataTable GetPuCountByOrg(string organizeId)
        {
            if (string.IsNullOrEmpty(organizeId))
            {
                return null;
            }
            else
            {
                string strSql = $"select count(1) from TelphonePu where DeleteMark <> 1 and EnabledMark <> 1 and SellMark <> 1 and OrganizeId='{organizeId}'";
                return this.BaseRepository().FindTable(strSql.ToString());
            }

        }
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<TelphonePuEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// ���������Զ���ȫ
        /// </summary>
        /// <param name="telphone"></param>
        /// <param name="organizeId"></param>
        /// <param name="city"></param>
        /// <returns></returns>
        public IEnumerable<TelphonePuEntity> GetList(string telphone, string organizeId, string city)
        {
            if (string.IsNullOrEmpty(organizeId))
            {
                return null;
            }
            var organizeData = orgService.GetEntity(organizeId);
            if (!string.IsNullOrEmpty(organizeData.OrganizeId))
            {
                //��������
                string telSql = "";
                if (!string.IsNullOrEmpty(telphone))
                {
                    telSql = " and Telphone like '%" + telphone + "%' ";
                }
                //��������
                string citySql = "";
                if (!string.IsNullOrEmpty(city) && city != "ȫ��")
                {
                    citySql = " and city like '" + city.Substring(0, 2) + "%'";
                }
                //1.������ 2.����ֱ���ϼ� 3.��������
                string orgSql = "and OrganizeId IN('" + organizeId + "','" + organizeData.ParentId + "','" + organizeData.TopOrganizeId + "')";


                //������룬��ʾǰ20������
                string strSql = @" SELECT TOP 20 * FROM (
 SELECT Telphone,City,Grade,ExistMark,price,EnabledMark FROM TelphoneLiang
  WHERE SellMark<>1 AND DeleteMark<>1 and EnabledMark <> 1 "
                    + orgSql
                    + citySql
                    + telSql
 + @"  UNION ALL 
 SELECT Telphone,City,Grade,ExistMark,price,EnabledMark FROM TelphonePu
  WHERE SellMark<>1 AND DeleteMark<>1 and EnabledMark <> 1 "
                    + citySql
                    + telSql
                    + " )t ORDER BY price desc";

                return this.BaseRepository().FindList(strSql.ToString());
            }
            else
            {
                return null;
            }


        }

        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="organizeId">���Ź�˾</param>
        /// <param name="Grade">��ѯ����</param>
        /// <param name="city"></param>
        /// <returns>�����б�</returns>
        public IEnumerable<TelphonePuEntity> GetGrade(string organizeId, string Grade, string city)
        {
            if (string.IsNullOrEmpty(organizeId))
            {
                return null;
            }
            var organizeData = orgService.GetEntity(organizeId);
            if (!string.IsNullOrEmpty(organizeData.OrganizeId))
            {
                //��������
                string gradeSql = "";
                if (!Grade.IsEmpty())
                {
                    gradeSql += " and Grade like '%," + Grade + "%'";//,105,202,203���ݿ��׺���𶼻�Ӹ�����
                }
                //��������
                string citySql = "";
                if (!string.IsNullOrEmpty(city) && city != "ȫ��")
                {
                    citySql = " and city like '" + city.Substring(0, 2) + "%'";
                }
                //1.������ 2.����ֱ���ϼ� 3.��������
                //string orgSql = "and OrganizeId IN('" + organizeId + "','" + organizeData.ParentId + "','" + organizeData.TopOrganizeId + "')";

                string strSql = @" SELECT * FROM (
 SELECT Telphone,City,Operator,Price,Grade,CASE ExistMark WHEN '1' THEN '�ֿ�' ELSE 'Ԥ��' END Description,Package,EnabledMark FROM TelphonePu 
 WHERE SellMark<>1 AND DeleteMark<>1 and EnabledMark <> 1 "
                    //+ orgSql   �׺Ų�������
                    + gradeSql
                    + citySql
                    + " )t ORDER BY price desc";
                return this.BaseRepository().FindList(strSql.ToString());

            }
            else
            {
                return null;
            }

        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public TelphonePuEntity GetEntity(int? keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }


        /// <summary>
        /// �ֻ��˵�������ʱ��,ֱ��ȫ����������Ϊ��Ȼ���Ե�����룬�ͱ�Ȼ��Ȩ�޷�Χ�ڣ������ظ�У��ú����Ƿ����ڱ�������Χ
        /// </summary>
        /// <param name="organizeId">��ǰ����</param>
        /// <param name="telphone">�ֻ���</param>
        /// <returns></returns>
        public IEnumerable<TelphonePuEntity> GetEntityByOrgTel(string organizeId, string telphone)
        {
            if (string.IsNullOrEmpty(organizeId))
            {
                return null;
            }
            string strSql = "select TelphoneID,Telphone,City,Operator,MinPrice,Price,ChaPrice,Grade,CASE ExistMark WHEN '1' THEN '�ֿ�' ELSE 'Ԥ��' END Description,Package,OrganizeId from TelphoneLiang"
               + " where DeleteMark <> 1 and EnabledMark <> 1 and SellMark <> 1";
            if (!telphone.IsEmpty())
            {
                strSql += " and Telphone='" + telphone + "'";
            }
            strSql += " UNION ALL " +
               "select TelphoneID,Telphone,City,Operator,MinPrice,Price,ChaPrice,Grade,CASE ExistMark WHEN '1' THEN '�ֿ�' ELSE 'Ԥ��' END Description,Package,OrganizeId from TelphonePu"
               + " where DeleteMark <> 1 and EnabledMark <> 1 and SellMark <> 1";
            if (!telphone.IsEmpty())
            {
                strSql += " and Telphone='" + telphone + "'";
            }
            return this.BaseRepository().FindList(strSql);
        }
        #endregion

        #region �ύ����
        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="keyValues">����</param>
        public void RemoveForm(string keyValues)
        {
            //this.BaseRepository().Delete(keyValues);
            IRepository db = new RepositoryFactory().BaseRepository().BeginTrans();
            if (!string.IsNullOrEmpty(keyValues))
            {
                string[] custIds = keyValues.Split(',');
                for (int i = 0; i < custIds.Length; i++)
                {
                    int? id = Convert.ToInt32(custIds[i]);
                    TelphonePuEntity entity = this.BaseRepository().FindEntity(id);
                    entity.Modify(custIds[i]);
                    entity.DeleteMark = 1;//��ɾ��
                    this.BaseRepository().Update(entity);
                }
                db.Commit();
            }
        }
        /// <summary>
        /// �ϼ�����
        /// </summary>
        /// <param name="keyValues">����</param>
        public void UpForm(string keyValues)
        {
            //this.BaseRepository().Delete(keyValues);
            IRepository db = new RepositoryFactory().BaseRepository().BeginTrans();
            if (!string.IsNullOrEmpty(keyValues))
            {
                string[] custIds = keyValues.Split(',');
                for (int i = 0; i < custIds.Length; i++)
                {
                    int? id = Convert.ToInt32(custIds[i]);
                    TelphonePuEntity entity = this.BaseRepository().FindEntity(id);
                    entity.Modify(custIds[i]);
                    entity.SellMark = 0;//����״̬
                    this.BaseRepository().Update(entity);
                    
                }
                db.Commit();
            }
        }
        /// <summary>
        /// �ֿ�����
        /// </summary>
        /// <param name="keyValues">����</param>
        public void ExistForm(string keyValues)
        {
            //this.BaseRepository().Delete(keyValues);
            IRepository db = new RepositoryFactory().BaseRepository().BeginTrans();
            if (!string.IsNullOrEmpty(keyValues))
            {
                string[] custIds = keyValues.Split(',');
                for (int i = 0; i < custIds.Length; i++)
                {
                    int? id = Convert.ToInt32(custIds[i]);
                    TelphonePuEntity entity = this.BaseRepository().FindEntity(id);
                    entity.Modify(custIds[i]);
                    entity.ExistMark = 1;//�ֿ�
                    this.BaseRepository().Update(entity);
                    
                }
                db.Commit();
            }
        }
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        public void SaveForm(int? keyValue, TelphonePuEntity entity)
        {
            if (!string.IsNullOrEmpty(keyValue.ToString()))
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

        /// <summary>
        /// β��ƥ�䣺����Ԥ����¼�У�Ԥ�������ϵģ���û���۳���β��
        /// </summary>
        /// <param name="end4"></param>
        /// <returns></returns>
        public IEnumerable<TelphonePuEntity> GetListEnd4(string end4)
        {
            string strSql = @"SELECT telphone,city,price,operator,'0' Description FROM TelphonePu WHERE SellMark =0 and DeleteMark =0 and substring(telphone,4,8)='0539" + end4 + "'" +
@"UNION ALL
SELECT telphone, city, price,operator,'1' Description FROM TelphoneLiang WHERE SellMark = 0 and DeleteMark =0 and substring(telphone,4, 8)= '0539" + end4 + "' ";
            return this.BaseRepository().FindList(strSql.ToString());
        }

        /// <summary>
        /// ������������
        /// </summary>
        /// <param name="dtSource">ʵ�����</param>
        /// <returns></returns>
        public string BatchAddEntity(DataTable dtSource)
        {
            IRepository db = new RepositoryFactory().BaseRepository().BeginTrans();
            int rowsCount = dtSource.Rows.Count;

            for (int i = 0; i < rowsCount; i++)
            {
                try
                {
                    string telphone = dtSource.Rows[i][0].ToString();
                    if (telphone.Length == 11)
                    {
                        var liang_Data = db.FindEntity<TelphonePuEntity>(t => t.Telphone == telphone && t.DeleteMark != 1);//ɾ�����Ŀ����ٴε���
                        if (liang_Data != null)
                        {
                            return telphone + "�ظ����룡";
                        }

                        //����ǰ7λȷ�����к���Ӫ��
                        string Number7 = telphone.Substring(0, 7);
                        string City = "";
                        string Operator = "";
                        var TelphoneData = db.FindEntity<TelphoneDataEntity>(t => t.Number7 == Number7);
                        if (TelphoneData != null)
                        {
                            City = TelphoneData.City;
                            Operator = TelphoneData.Operate;
                        }
                        else
                        {
                            return "�Ŷβ����ڣ�" + Number7;
                        }

                        //�۸�
                        if (string.IsNullOrEmpty(dtSource.Rows[i][1].ToString()))
                        {
                            return telphone + "�۸�Ϊ��";
                        }
                        decimal Price = Convert.ToDecimal(dtSource.Rows[i][1].ToString());
                        //�ɱ���
                        //if (string.IsNullOrEmpty(dtSource.Rows[i][2].ToString()))
                        //{
                        //    return telphone + "�ɱ���Ϊ��";
                        //}
                        //decimal MinPrice = Convert.ToDecimal(dtSource.Rows[i][2].ToString());
                        decimal MinPrice = Convert.ToDecimal(Price* (decimal)0.55);
                        //����
                        decimal ChaPrice = Price - MinPrice;

                        //���
                        if (string.IsNullOrEmpty(dtSource.Rows[i][3].ToString()))
                        {
                            return telphone + "���Ϊ��";
                        }
                        string itemName = dtSource.Rows[i][3].ToString();
                        string itemNCode = "";
                        var DataItemDetail = db.FindEntity<DataItemDetailEntity>(t => t.ItemName == itemName);
                        if (DataItemDetail != null)
                        {
                            itemNCode = DataItemDetail.ItemValue;
                        }
                        else
                        {
                            return "���Ͳ����ڣ�" + itemName + ",���������ֵ���ά�������͡�";
                        }

                        //�ײ�
                        string Package = dtSource.Rows[i][4].ToString();
                        //״̬
                        if (string.IsNullOrEmpty(dtSource.Rows[i][5].ToString()))
                        {
                            return telphone + "�ֿ�/����״̬Ϊ��";
                        }
                        string existStr = dtSource.Rows[i][5].ToString();
                        int existMark = 0;
                        if (existStr == "�ֿ�")
                        {
                            existMark = 1;
                        }
                        else
                        {
                            existMark = 0;
                        }

                        //�������
                        TelphonePuEntity entity = new TelphonePuEntity()
                        {
                            Telphone = telphone,
                            Price = Price,
                            MinPrice = MinPrice,
                            ChaPrice = ChaPrice,
                            City = City,
                            Operator = Operator,
                            Grade = itemNCode,
                            Package = Package,
                            ExistMark = existMark,
                            SellMark = 0,
                            DeleteMark = 0,
                            OrganizeId = OperatorProvider.Provider.Current().CompanyId,
                        };
                        entity.Create();
                        db.Insert(entity);
                    }

                }
                catch (Exception ex)
                {
                    return ex.Message;
                }

            }
            db.Commit();
            return "����ɹ�";
        }

        /// <summary>
        /// �����¼�
        /// </summary>
        /// <returns></returns>
        public string DownTelphone(string downTelphones)
        {
            string returnMsg = "";
            if (!string.IsNullOrEmpty(downTelphones))
            {
                IRepository db = new RepositoryFactory().BaseRepository().BeginTrans();
                //string[] telphones = downTelphones.Split(new string[] {"%0A"}, StringSplitOptions.RemoveEmptyEntries);
                string[] telphones = downTelphones.Split('\n');
                for (int i = 0; i < telphones.Length; i++)
                {
                    string telphoneName = telphones[i];
                    if (!string.IsNullOrEmpty(telphoneName))
                    {
                        string telphone = telphoneName.Substring(0, 11);
                        string name = telphoneName.Replace(telphone, "").Trim();

                        string companyid = OperatorProvider.Provider.Current().CompanyId;
                        var entity = db.FindEntity<TelphonePuEntity>(t => t.Telphone == telphone && t.SellMark == 0 && t.EnabledMark == 0 && t.DeleteMark == 0 && t.OrganizeId == companyid);
                        if (entity != null)
                        {
                            var userEntity = db.FindEntity<UserEntity>(t => t.RealName == name);
                            if (userEntity != null)
                            {
                                entity.SellerName = userEntity.RealName;
                                entity.SellerId = userEntity.UserId;
                            }
                            else
                            {
                                return name + " �����ڸ��û���";
                            }

                            entity.SellMark = 1;
                            entity.Modify(entity.TelphoneID);
                            db.Update(entity);
                            returnMsg += telphone + " ���¼�</br>";
                        }
                        else
                        {
                            returnMsg += telphone + " �����ڱ���˾�����۳�</br>";
                        }
                    }

                }
                db.Commit();
            }
            else
            {
                returnMsg = "δ���ܵ��κ�����";
            }

            return returnMsg;
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        public string PriceTelphone(string priceTelphones)
        {
            string returnMsg = "";
            if (!string.IsNullOrEmpty(priceTelphones))
            {
                IRepository db = new RepositoryFactory().BaseRepository().BeginTrans();
                string[] telphones = priceTelphones.Split('\n');
                for (int i = 0; i < telphones.Length; i++)
                {
                    string telphoneTxt = telphones[i];
                    if (!string.IsNullOrEmpty(telphoneTxt))
                    {
                        //string[] telphonePrice = telphoneTxt.Split(' ');
                        string[] telphonePrice = Regex.Split(telphoneTxt, "\t|\\s+", RegexOptions.IgnoreCase);//������ʽ
                        string telphone = telphoneTxt.Substring(0, 11);

                        string priceTxt = telphonePrice[1].Trim();
                        decimal price = 0;
                        try
                        {
                            price = Convert.ToDecimal(priceTxt);
                        }
                        catch (Exception)
                        {
                            return telphone + " �ۼ�ת����ʽ����";
                        }

                        string minPriceTxt = telphonePrice[2].Trim();
                        decimal minPrice = 0;
                        try
                        {
                            minPrice = Convert.ToDecimal(minPriceTxt);
                        }
                        catch (Exception)
                        {
                            return telphone + " �ɱ���ת����ʽ����";
                        }

                        decimal chaPrice = price - minPrice;
                        if (chaPrice < 0)
                        {
                            return telphone + " �ɱ��۸����ۼۣ��۸�˳��ߵ���";
                        }

                        string companyid = OperatorProvider.Provider.Current().CompanyId;
                        var entity = db.FindEntity<TelphonePuEntity>(t => t.Telphone == telphone && t.SellMark == 0 && t.EnabledMark == 0 && t.DeleteMark == 0 && t.OrganizeId == companyid);
                        if (entity != null)
                        {
                            entity.MinPrice = minPrice;
                            entity.Price = price;
                            entity.ChaPrice = chaPrice;
                            entity.Modify(entity.TelphoneID);
                            db.Update(entity);
                            returnMsg += telphone + " �ѵ���</br>";
                        }
                        else
                        {
                            returnMsg += telphone + " �����ڱ���˾�����۳�</br>";
                        }
                    }

                }
                db.Commit();
            }
            else
            {
                returnMsg = "δ���ܵ��κ�����";
            }

            return returnMsg;
        }

    }
}
