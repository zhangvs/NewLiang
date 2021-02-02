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
    /// �� �ڣ�2020-06-07 14:11
    /// �� ����ͷ�����ſ�
    /// </summary>
    public class TelphoneLiangH5Service : RepositoryFactory<TelphoneLiangH5Entity>, TelphoneLiangH5IService
    {
        private IOrganizeService orgService = new OrganizeService();
        private TelphoneLiangVipIService vipService = new TelphoneLiangVipService();
        private TelphoneVipShareIService vipShareService = new TelphoneVipShareService();
        #region ��ȡ����

        /// <summary>
        /// ����jsonƴ��sql�������������������
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        private string GetSql(string queryJson)
        {
            string strSql = "";
            var queryParam = queryJson.ToJObject();

            //��������
            if (!queryParam["search"].IsEmpty())
            {
                string search = queryParam["search"].ToString();
                strSql += " and Telphone like '%" + search + "%'";
            }

            //����
            if (!queryParam["Telphone"].IsEmpty())
            {
                string Telphone = queryParam["Telphone"].ToString();
                if (Telphone.Contains("|"))
                {
                    strSql += " and Telphone like '%" + Telphone.Replace("|", "") + "'";
                }
                else
                {
                    strSql += " and Telphone like '%" + Telphone + "%'";
                }
            }
            //����s
            if (!queryParam["Telphones"].IsEmpty())
            {
                string Telphones = queryParam["Telphones"].ToString();
                strSql += " and Telphone in (" + Telphones + ")";
            }
            //����
            if (!queryParam["Grade"].IsEmpty())
            {
                string Grade = queryParam["Grade"].ToString();
                if (Grade.Contains(","))
                {
                    strSql += " and Grade in (" + Grade + ")";
                }
                else
                {
                    strSql += " and Grade = '" + Grade + "'";
                }
            }
            //����
            if (!queryParam["city"].IsEmpty())
            {
                string city = queryParam["city"].ToString();
                if (city.Contains("0000"))
                {
                    strSql += " and cityid like '" + city.Substring(0, 2) + "%'";
                }
                else if (city != "0")
                {
                    strSql += " and cityid = '" + city + "'";
                }
            }
            //ʡ������
            if (!queryParam["province"].IsEmpty())
            {
                string Province = queryParam["province"].ToString();
                strSql += " and Province like '%" + Province.Substring(0,2) + "%'";
            }
            //��������
            if (!queryParam["City"].IsEmpty())
            {
                string City = queryParam["City"].ToString();
                strSql += " and City like '%" + City.Replace("��","") + "%'";
            }
            //�۸�����
            if (!queryParam["price"].IsEmpty())
            {
                string price = queryParam["price"].ToString();
                string[] jgqj = price.Split('-');
                if (!string.IsNullOrEmpty(jgqj[0]))
                {
                    strSql += " and price >= '" + jgqj[0] + "'";
                }
                if (jgqj.Length>1)
                {
                    if (!string.IsNullOrEmpty(jgqj[1]) && jgqj[1] != "0")//���۸�Ϊ0
                    {
                        strSql += " and price <= '" + jgqj[1] + "'";
                    }
                }
            }
            //�۸�����
            if (!queryParam["MaxPrice"].IsEmpty())
            {
                string MaxPrice = queryParam["MaxPrice"].ToString();
                string[] jgqj = MaxPrice.Split('-');
                if (!string.IsNullOrEmpty(jgqj[0]))
                {
                    strSql += " and MaxPrice >= '" + jgqj[0] + "'";
                }
                if (jgqj.Length>1)
                {
                    if (!string.IsNullOrEmpty(jgqj[1]) && jgqj[1] != "0")
                    {
                        strSql += " and MaxPrice <= '" + jgqj[1] + "'";
                    }
                }
            }
            //�ų�
            if (!queryParam["except"].IsEmpty())
            {
                string except = queryParam["except"].ToString();
                strSql += " and Telphone not like '%" + except.Replace("e", "") + "%'";
            }
            //Ԣ��
            if (!queryParam["yuyi"].IsEmpty())
            {
                string yuyi = queryParam["yuyi"].ToString();
                if (yuyi == "1")
                {//1349��ˮ����
                    strSql += " and Telphone like '%1349%'";
                }
                else if (yuyi == "2")
                {//
                    strSql += " and (Telphone like '%520%' or Telphone like '%521%' or Telphone like '%1314%')";
                }
            }
            //�۳���ʶ
            if (!queryParam["SellMark"].IsEmpty())
            {
                string SellMark = queryParam["SellMark"].ToString();
                strSql += " and SellMark = " + SellMark;
            }

            //״̬����
            if (!queryParam["ExistMark"].IsEmpty())
            {
                string ExistMark = queryParam["ExistMark"].ToString();
                strSql += " and ExistMark = " + ExistMark;
            }

            //����
            if (!queryParam["Operator"].IsEmpty())
            {
                string Operator = queryParam["Operator"].ToString();
                strSql += " and Operator like '%" + Operator + "%'";
            }
            //3����
            if (!queryParam["repeatNumber"].IsEmpty())
            {
                string repeatNumber = queryParam["repeatNumber"].ToString();
                strSql += " and substring(telphone,9,3) like '%" + repeatNumber + "%'";
            }
            //������
            if (!queryParam["moreNumber"].IsEmpty())
            {
                string moreNumber = queryParam["moreNumber"].ToString();
                strSql += " and substring(telphone,8,4) like '%" + moreNumber + moreNumber + moreNumber + "%'";
            }
            //����
            if (!queryParam["birthdayNumber"].IsEmpty())
            {
                string birthdayNumber = queryParam["birthdayNumber"].ToString();
                switch (birthdayNumber)
                {
                    case "01":
                        strSql += " and substring(telphone,8,4) BETWEEN '0101' AND '0131'";
                        break;
                    case "02":
                        strSql += " and substring(telphone,8,4) BETWEEN '0201' AND '0228'";
                        break;
                    case "03":
                        strSql += " and substring(telphone,8,4) BETWEEN '0301' AND '0331'";
                        break;
                    case "04":
                        strSql += " and substring(telphone,8,4) BETWEEN '0401' AND '0430'";
                        break;
                    case "05":
                        strSql += " and substring(telphone,8,4) BETWEEN '0501' AND '0531'";
                        break;
                    case "06":
                        strSql += " and substring(telphone,8,4) BETWEEN '0601' AND '0630'";
                        break;
                    case "07":
                        strSql += " and substring(telphone,8,4) BETWEEN '0701' AND '0731'";
                        break;
                    case "08":
                        strSql += " and substring(telphone,8,4) BETWEEN '0801' AND '0831'";
                        break;
                    case "09":
                        strSql += " and substring(telphone,8,4) BETWEEN '0901' AND '0930'";
                        break;
                    case "10":
                        strSql += " and substring(telphone,8,4) BETWEEN '1001' AND '1031'";
                        break;
                    case "11":
                        strSql += " and substring(telphone,8,4) BETWEEN '1101' AND '1130'";
                        break;
                    case "12":
                        strSql += " and substring(telphone,8,4) BETWEEN '1201' AND '1231'";
                        break;
                    default:
                        break;
                }
            }
            return strSql;
        }


        /// <summary>
        /// �ӹ��˹���ƽ̨�е�vip����
        /// </summary>
        /// <param name="vipList">vip����</param>
        /// <returns></returns>
        private string GetOtherOrg(List<string> vipList)
        {
            string shareOrg = "";
            //1.�Զ��干�����
            foreach (var item in vipList)
            {
                var sharelist = vipShareService.GetVipList(item);
                if (sharelist != null)
                {
                    foreach (var item2 in sharelist)
                    {
                        shareOrg += "'" + item2.ShareId + "',";//�����Զ���ѡ��Ĺ������
                    }
                }
            }
            return shareOrg.Trim(',');
        }
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        public IEnumerable<TelphoneLiangH5Entity> GetPageList(Pagination pagination, string queryJson)
        {
            var queryParam = queryJson.ToJObject();
            string strSql = "select * from TelphoneLiangH5 where DeleteMark <> 1 and EnabledMark <> 1";
            strSql += GetSql(queryJson);
            return this.BaseRepository().FindList(strSql.ToString(), pagination);
        }


        /// <summary>
        /// ͷ���ƹ�⣬ֻ�����¿����ϴ���ô���������������Բ��ò�ѯ����id
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public IEnumerable<TelphoneLiangH5Entity> GetPageListH5LX(Pagination pagination, string queryJson)
        {
            string ownWhere = GetSql(queryJson);
            //��������0��
            string strSql = @" SELECT * FROM (
 SELECT TelphoneID,Telphone,Price,City FROM TelphoneLiangH5 
 WHERE SellMark<>1 AND DeleteMark<>1 and EnabledMark <> 1 " + ownWhere
                + " )t ";
            return this.BaseRepository().FindList(strSql.ToString(), pagination);
        }



        /// <summary>
        /// CheckPrice�ͼ�
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public IEnumerable<TelphoneLiangH5Entity> GetPageListH5LX_JS(Pagination pagination, string queryJson)
        {
            string ownWhere = GetSql(queryJson);
            //��������0��
            string strSql = @" SELECT * FROM (
 SELECT TelphoneID,Telphone,CheckPrice Price,City FROM TelphoneLiangH5 
 WHERE SellMark<>1 AND DeleteMark<>1 and EnabledMark <> 1 AND CheckPrice>0 " + ownWhere + " )t ";
            return this.BaseRepository().FindList(strSql.ToString(), pagination);
        }
        
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<TelphoneLiangH5Entity> GetList(string queryJson)
        {
            string ownWhere = GetSql(queryJson);
            //��������0��
            string strSql = @" SELECT * FROM (
 SELECT TelphoneID,Telphone,CheckPrice Price,City FROM TelphoneLiangH5 
 WHERE SellMark<>1 AND DeleteMark<>1 and EnabledMark <> 1 AND CheckPrice>0 " + ownWhere + " )t ";
            return this.BaseRepository().FindList(strSql.ToString());
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public TelphoneLiangH5Entity GetEntity(int? keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
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
                    this.BaseRepository().Delete(id);
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
            IRepository db = new RepositoryFactory().BaseRepository().BeginTrans();
            if (!string.IsNullOrEmpty(keyValues))
            {
                string[] custIds = keyValues.Split(',');
                for (int i = 0; i < custIds.Length; i++)
                {
                    int? id = Convert.ToInt32(custIds[i]);
                    TelphoneLiangH5Entity entity = this.BaseRepository().FindEntity(id);
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
                    TelphoneLiangH5Entity entity = this.BaseRepository().FindEntity(id);
                    entity.Modify(custIds[i]);
                    entity.ExistMark = 1;//�ֿ�
                    this.BaseRepository().Update(entity);
                }
                db.Commit();
            }
        }

        /// <summary>
        /// ��ɱ����
        /// </summary>
        /// <param name="keyValues">����</param>
        public void MiaoShaForm(string keyValues)
        {
            //this.BaseRepository().Delete(keyValues);
            IRepository db = new RepositoryFactory().BaseRepository().BeginTrans();
            if (!string.IsNullOrEmpty(keyValues))
            {
                string[] custIds = keyValues.Split(',');
                for (int i = 0; i < custIds.Length; i++)
                {
                    int? id = Convert.ToInt32(custIds[i]);
                    TelphoneLiangH5Entity entity = this.BaseRepository().FindEntity(id);
                    entity.Modify(custIds[i]);
                    entity.ExistMark = 2;//��ɱ
                    this.BaseRepository().Update(entity);
                }
                db.Commit();
            }
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
                string[] telphones = downTelphones.Split('\n');
                for (int i = 0; i < telphones.Length; i++)
                {
                    string telphoneName = telphones[i];
                    if (!string.IsNullOrEmpty(telphoneName))
                    {
                        IRepository db = new RepositoryFactory().BaseRepository().BeginTrans();
                        string telphone = telphoneName.Substring(0, 11);
                        string name = telphoneName.Replace(telphone, "").Trim();
                        
                        var entity = db.FindEntity<TelphoneLiangH5Entity>(t => t.Telphone == telphone && t.EnabledMark == 0 && t.DeleteMark == 0);// && t.SellMark == 0 && t.OrganizeId == companyid
                        if (entity != null)
                        {
                            entity.SellerName = name;

                            entity.SellMark = 1;
                            entity.Modify(entity.TelphoneID);
                            db.Update(entity);
                            returnMsg += telphone + " ���¼�</br>";
                        }
                        else
                        {
                            returnMsg += telphone + " �����ڱ���˾�����۳�</br>";
                        }
                        db.Commit();
                    }
                }
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
                        string telphone = telphoneTxt.Substring(0, 11);
                        string[] telphonePrice = Regex.Split(telphoneTxt, "\t|\\s+", RegexOptions.IgnoreCase);//������ʽ
                        //�ۼ�
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
                        //�ɱ���
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
                        //����
                        decimal chaPrice = price - minPrice;
                        if (chaPrice < 0)
                        {
                            return telphone + " �ɱ��۸����ۼۣ��۸�˳��ߵ���";
                        }

                        //�����
                        decimal checkPrice = 0;
                        if (telphonePrice.Length == 4)
                        {
                            string checkPriceTxt = telphonePrice[3].Trim();
                            try
                            {
                                checkPrice = Convert.ToDecimal(checkPriceTxt);
                            }
                            catch (Exception)
                            {
                                return telphone + " ����ۼ�ת����ʽ����";
                            }

                            decimal checkChaPrice = price - checkPrice;
                            if (checkChaPrice < 0)
                            {
                                return telphone + " ����۸����ۼۣ��۸�˳��ߵ���";
                            }
                        }

                        
                        var entity = db.FindEntity<TelphoneLiangH5Entity>(t => t.Telphone == telphone && t.SellMark == 0 && t.EnabledMark == 0 && t.DeleteMark == 0);//&& t.OrganizeId == companyid//һ���������Բ���
                        if (entity != null)
                        {
                            entity.MinPrice = minPrice;//�ɱ���
                            entity.Price = price;//�ۼ�
                            entity.ChaPrice = chaPrice;//����
                            entity.CheckPrice = checkPrice;//�����
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

        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        public void SaveForm(int? keyValue, TelphoneLiangH5Entity entity)
        {
            if (!string.IsNullOrEmpty(keyValue.ToString()))
            {
                entity.Modify(keyValue);
                this.BaseRepository().Update(entity);
            }
            else
            {
                //����ǰ7λȷ����������
                IRepository db = new RepositoryFactory().BaseRepository().BeginTrans();
                string Number7 = entity.Telphone.Substring(0, 7);

                var TelphoneData = db.FindEntity<TelphoneDataEntity>(t => t.Number7 == Number7);
                if (TelphoneData != null)
                {
                    if (string.IsNullOrEmpty(TelphoneData.City))
                    {
                        throw new Exception("�Ŷγ���Ϊ�գ�" + Number7);
                    }
                    else
                    {
                        entity.City = TelphoneData.City.Replace("��", "");
                        entity.CityId = TelphoneData.CityId;
                        entity.Operator = TelphoneData.Operate;
                        entity.Brand = TelphoneData.Brand;
                    }
                }
                else
                {
                    throw new Exception("�Ŷβ����ڣ�" + Number7);
                }
                entity.Create();
                this.BaseRepository().Insert(entity);
            }
        }

        /// <summary>
        /// ������������
        /// </summary>
        /// <param name="dtSource">ʵ�����</param>
        /// <returns></returns>
        public string BatchAddEntity(DataTable dtSource)
        {
            int rowsCount = dtSource.Rows.Count;
            List<string> ts = new List<string>();
            List<string> Number7s = new List<string>();

            IRepository db = new RepositoryFactory().BaseRepository().BeginTrans();
            for (int i = 0; i < rowsCount; i++)
            {
                //�ȼ��鵱ǰ�ļ��Ƿ�����ظ�����
                string telphone = dtSource.Rows[i][0].ToString();
                if (ts.Contains(telphone))
                {
                    return "�ϴ��ļ������ظ����룬����ʧ�ܣ�����ȥ���ٵ��룡"+ telphone;
                }
                ts.Add(telphone);
                
                //�ȼ��鵱ǰ�ļ��Ŷβ�����
                string Number7 = telphone.Substring(0, 7);
                if (!Number7s.Contains(Number7))
                {
                    Number7s.Add(Number7);
                    var TelphoneData = db.FindEntity<TelphoneDataEntity>(t => t.Number7 == Number7);
                    if (TelphoneData == null)
                    {
                        return "�Ŷβ����ڣ�" + Number7;
                    }
                }
            }

            try
            {
                int commitInt = 0;
                int columns = dtSource.Columns.Count;
                for (int i = 0; i < rowsCount; i++)
                {
                    db = new RepositoryFactory().BaseRepository().BeginTrans();
                    string telphone = dtSource.Rows[i][0].ToString();
                    if (telphone.Length == 11)
                    {
                        var liang_Data = db.FindEntity<TelphoneLiangH5Entity>(t => t.Telphone == telphone && t.DeleteMark != 1);//ɾ�����Ŀ����ٴε���
                        if (liang_Data != null)
                        {
                            db.Delete<TelphoneLiangH5Entity>(liang_Data.TelphoneID);//ɾ���ϵ�����
                        }

                        //����ǰ7λȷ�����к���Ӫ��
                        string Number7 = telphone.Substring(0, 7);
                        string Province ="", City = "", CityId = "", Operator = "", Brand = "";
                        var TelphoneData = db.FindEntity<TelphoneDataEntity>(t => t.Number7 == Number7);
                        if (TelphoneData != null)
                        {
                            if (string.IsNullOrEmpty(TelphoneData.City))
                            {
                                return "�Ŷγ���Ϊ�գ�" + Number7;
                            }
                            else
                            {
                                City = TelphoneData.City.Replace("��", "");
                                CityId = TelphoneData.CityId;
                                Operator = TelphoneData.Operate;
                                Brand = TelphoneData.Brand;
                                Province = TelphoneData.Province;
                            }
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
                        if (string.IsNullOrEmpty(dtSource.Rows[i][2].ToString()))
                        {
                            return telphone + "�ɱ���Ϊ��";
                        }
                        decimal MinPrice = Convert.ToDecimal(dtSource.Rows[i][2].ToString());
                        //����
                        decimal ChaPrice = Price - MinPrice;

                        //�����
                        decimal CheckPrice = Convert.ToDecimal(dtSource.Rows[i][3].ToString());

                        //���
                        string itemName = dtSource.Rows[i][4].ToString();
                        if (string.IsNullOrEmpty(itemName))
                        {
                            return telphone + "���Ϊ��";
                        }
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
                        string Package = dtSource.Rows[i][5].ToString();
                        //״̬
                        string existStr = dtSource.Rows[i][6].ToString();

                        if (string.IsNullOrEmpty(existStr))
                        {
                            return telphone + "�ֿ�/����/��ɱ״̬Ϊ��";
                        }
                        if (existStr != "��ɱ" && existStr != "�ֿ�" && existStr != "Ԥ��")
                        {
                            return telphone + "�ֿ�/����/��ɱ״̬��д����";
                        }
                        int existMark = 0;
                        if (existStr == "��ɱ")
                        {
                            existMark = 2;
                        }
                        else if (existStr == "�ֿ�")
                        {
                            existMark = 1;
                        }
                        else
                        {
                            existMark = 0;
                        }

                        //��ֹ�����ƹ�۴��� ������ʾ���޷��ҵ��� 7
                        decimal? MaxPrice = null;
                        if (columns == 8)
                        {
                            //�ƹ��
                            string maxPriceStr = dtSource.Rows[i][7].ToString();
                            //�����ǰ�еĵ�Ԫ�񱨴�Ҳ��ת���ʹ���
                            if (!string.IsNullOrEmpty(maxPriceStr))
                            {
                                MaxPrice = Convert.ToDecimal(maxPriceStr);
                            }
                        }

                        //�������
                        TelphoneLiangH5Entity entity = new TelphoneLiangH5Entity()
                        {
                            Telphone = telphone,
                            Price = Price,
                            MaxPrice = MaxPrice,
                            MinPrice = MinPrice,
                            ChaPrice = ChaPrice,
                            CheckPrice = CheckPrice,
                            Province= Province,
                            City = City,
                            CityId = CityId,
                            Operator = Operator,
                            Brand = Brand,
                            Grade = itemNCode,
                            Package = Package,
                            ExistMark = existMark,
                            SellMark = 0,
                            DeleteMark = 0,
                            OrganizeId = OperatorProvider.Provider.Current().CompanyId,
                        };
                        entity.Create();
                        db.Insert(entity);
                        db.Commit();
                        commitInt++;
                    }
                }
                return "����ɹ���"+ commitInt;
            }
            catch (Exception ex)
            {
                db.Rollback();
                LogHelper.AddLog(ex.Message);
                return ex.Message;
            }

        }
        #endregion



        /// <summary>
        /// ������ɾ����
        /// </summary>
        /// <param name="dtSource">ʵ�����</param>
        /// <returns></returns>
        public string BatchDeleteEntity(DataTable dtSource)
        {
            int rowsCount = dtSource.Rows.Count;
            int columns = dtSource.Columns.Count;
            int ok = 0;
            for (int i = 0; i < rowsCount; i++)
            {
                try
                {
                    string telphone = dtSource.Rows[i][0].ToString().Trim();
                    if (telphone.Length == 11)
                    {
                        IRepository db = new RepositoryFactory().BaseRepository().BeginTrans();
                        var liang_Data = db.FindList<TelphoneLiangH5Entity>(t => t.Telphone == telphone && t.DeleteMark != 1);//ɾ�����Ŀ����ٴε���
                        foreach (var item in liang_Data)
                        {
                            db.Delete(item);
                            ok++;
                        }
                        db.Commit();
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.AddLog(ex.Message);
                    return ex.Message;
                }
            }
            return "����ɾ����ɣ�"+ok;
        }
    }
}
