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
    /// 版 本 6.1
    /// 
    /// 创 建：超级管理员
    /// 日 期：2020-06-07 14:11
    /// 描 述：头条靓号库
    /// </summary>
    public class TelphoneLiangH5Service : RepositoryFactory<TelphoneLiangH5Entity>, TelphoneLiangH5IService
    {
        private IOrganizeService orgService = new OrganizeService();
        private TelphoneLiangVipIService vipService = new TelphoneLiangVipService();
        private TelphoneVipShareIService vipShareService = new TelphoneVipShareService();
        #region 获取数据

        /// <summary>
        /// 根据json拼接sql条件，除机构外的条件
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        private string GetSql(string queryJson)
        {
            string strSql = "";
            var queryParam = queryJson.ToJObject();

            //搜索内容
            if (!queryParam["search"].IsEmpty())
            {
                string search = queryParam["search"].ToString();
                strSql += " and Telphone like '%" + search + "%'";
            }

            //号码
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
            //号码s
            if (!queryParam["Telphones"].IsEmpty())
            {
                string Telphones = queryParam["Telphones"].ToString();
                strSql += " and Telphone in (" + Telphones + ")";
            }
            //分类
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
            //城市
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
            //省份名称
            if (!queryParam["province"].IsEmpty())
            {
                string Province = queryParam["province"].ToString();
                strSql += " and Province like '%" + Province.Substring(0,2) + "%'";
            }
            //城市名称
            if (!queryParam["City"].IsEmpty())
            {
                string City = queryParam["City"].ToString();
                strSql += " and City like '%" + City.Replace("市","") + "%'";
            }
            //价格区间
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
                    if (!string.IsNullOrEmpty(jgqj[1]) && jgqj[1] != "0")//最大价格不为0
                    {
                        strSql += " and price <= '" + jgqj[1] + "'";
                    }
                }
            }
            //价格区间
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
            //排除
            if (!queryParam["except"].IsEmpty())
            {
                string except = queryParam["except"].ToString();
                strSql += " and Telphone not like '%" + except.Replace("e", "") + "%'";
            }
            //寓意
            if (!queryParam["yuyi"].IsEmpty())
            {
                string yuyi = queryParam["yuyi"].ToString();
                if (yuyi == "1")
                {//1349风水能量
                    strSql += " and Telphone like '%1349%'";
                }
                else if (yuyi == "2")
                {//
                    strSql += " and (Telphone like '%520%' or Telphone like '%521%' or Telphone like '%1314%')";
                }
            }
            //售出标识
            if (!queryParam["SellMark"].IsEmpty())
            {
                string SellMark = queryParam["SellMark"].ToString();
                strSql += " and SellMark = " + SellMark;
            }

            //状态条件
            if (!queryParam["ExistMark"].IsEmpty())
            {
                string ExistMark = queryParam["ExistMark"].ToString();
                strSql += " and ExistMark = " + ExistMark;
            }

            //网络
            if (!queryParam["Operator"].IsEmpty())
            {
                string Operator = queryParam["Operator"].ToString();
                strSql += " and Operator like '%" + Operator + "%'";
            }
            //3连号
            if (!queryParam["repeatNumber"].IsEmpty())
            {
                string repeatNumber = queryParam["repeatNumber"].ToString();
                strSql += " and substring(telphone,9,3) like '%" + repeatNumber + "%'";
            }
            //几更多
            if (!queryParam["moreNumber"].IsEmpty())
            {
                string moreNumber = queryParam["moreNumber"].ToString();
                strSql += " and substring(telphone,8,4) like '%" + moreNumber + moreNumber + moreNumber + "%'";
            }
            //生日
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
        /// 从过滤共享平台中的vip机构
        /// </summary>
        /// <param name="vipList">vip机构</param>
        /// <returns></returns>
        private string GetOtherOrg(List<string> vipList)
        {
            string shareOrg = "";
            //1.自定义共享机构
            foreach (var item in vipList)
            {
                var sharelist = vipShareService.GetVipList(item);
                if (sharelist != null)
                {
                    foreach (var item2 in sharelist)
                    {
                        shareOrg += "'" + item2.ShareId + "',";//所有自定义选择的共享机构
                    }
                }
            }
            return shareOrg.Trim(',');
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public IEnumerable<TelphoneLiangH5Entity> GetPageList(Pagination pagination, string queryJson)
        {
            var queryParam = queryJson.ToJObject();
            string strSql = "select * from TelphoneLiangH5 where DeleteMark <> 1 and EnabledMark <> 1";
            strSql += GetSql(queryJson);
            return this.BaseRepository().FindList(strSql.ToString(), pagination);
        }


        /// <summary>
        /// 头条推广库，只有利新可以上传，么有其它机构，可以不用查询机构id
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public IEnumerable<TelphoneLiangH5Entity> GetPageListH5LX(Pagination pagination, string queryJson)
        {
            string ownWhere = GetSql(queryJson);
            //自身，父，0级
            string strSql = @" SELECT * FROM (
 SELECT TelphoneID,Telphone,Price,City FROM TelphoneLiangH5 
 WHERE SellMark<>1 AND DeleteMark<>1 and EnabledMark <> 1 " + ownWhere
                + " )t ";
            return this.BaseRepository().FindList(strSql.ToString(), pagination);
        }



        /// <summary>
        /// CheckPrice低价
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public IEnumerable<TelphoneLiangH5Entity> GetPageListH5LX_JS(Pagination pagination, string queryJson)
        {
            string ownWhere = GetSql(queryJson);
            //自身，父，0级
            string strSql = @" SELECT * FROM (
 SELECT TelphoneID,Telphone,CheckPrice Price,City FROM TelphoneLiangH5 
 WHERE SellMark<>1 AND DeleteMark<>1 and EnabledMark <> 1 AND CheckPrice>0 " + ownWhere + " )t ";
            return this.BaseRepository().FindList(strSql.ToString(), pagination);
        }
        
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<TelphoneLiangH5Entity> GetList(string queryJson)
        {
            string ownWhere = GetSql(queryJson);
            //自身，父，0级
            string strSql = @" SELECT * FROM (
 SELECT TelphoneID,Telphone,CheckPrice Price,City FROM TelphoneLiangH5 
 WHERE SellMark<>1 AND DeleteMark<>1 and EnabledMark <> 1 AND CheckPrice>0 " + ownWhere + " )t ";
            return this.BaseRepository().FindList(strSql.ToString());
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public TelphoneLiangH5Entity GetEntity(int? keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValues">主键</param>
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
        /// 上架数据
        /// </summary>
        /// <param name="keyValues">主键</param>
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
                    entity.SellMark = 0;//销售状态
                    this.BaseRepository().Update(entity);
                }
                db.Commit();
            }
        }
        /// <summary>
        /// 现卡数据
        /// </summary>
        /// <param name="keyValues">主键</param>
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
                    entity.ExistMark = 1;//现卡
                    this.BaseRepository().Update(entity);
                }
                db.Commit();
            }
        }

        /// <summary>
        /// 秒杀数据
        /// </summary>
        /// <param name="keyValues">主键</param>
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
                    entity.ExistMark = 2;//秒杀
                    this.BaseRepository().Update(entity);
                }
                db.Commit();
            }
        }

        /// <summary>
        /// 批量下架
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
                            returnMsg += telphone + " 已下架</br>";
                        }
                        else
                        {
                            returnMsg += telphone + " 不属于本公司或已售出</br>";
                        }
                        db.Commit();
                    }
                }
            }
            else
            {
                returnMsg = "未接受到任何数据";
            }

            return returnMsg;
        }

        /// <summary>
        /// 批量调价
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
                        string[] telphonePrice = Regex.Split(telphoneTxt, "\t|\\s+", RegexOptions.IgnoreCase);//正则表达式
                        //售价
                        string priceTxt = telphonePrice[1].Trim();
                        decimal price = 0;
                        try
                        {
                            price = Convert.ToDecimal(priceTxt);
                        }
                        catch (Exception)
                        {
                            return telphone + " 售价转换格式错误！";
                        }
                        //成本价
                        string minPriceTxt = telphonePrice[2].Trim();
                        decimal minPrice = 0;
                        try
                        {
                            minPrice = Convert.ToDecimal(minPriceTxt);
                        }
                        catch (Exception)
                        {
                            return telphone + " 成本价转换格式错误！";
                        }
                        //利润
                        decimal chaPrice = price - minPrice;
                        if (chaPrice < 0)
                        {
                            return telphone + " 成本价高于售价，价格顺序颠倒！";
                        }

                        //核算价
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
                                return telphone + " 核算价价转换格式错误！";
                            }

                            decimal checkChaPrice = price - checkPrice;
                            if (checkChaPrice < 0)
                            {
                                return telphone + " 核算价高于售价，价格顺序颠倒！";
                            }
                        }

                        
                        var entity = db.FindEntity<TelphoneLiangH5Entity>(t => t.Telphone == telphone && t.SellMark == 0 && t.EnabledMark == 0 && t.DeleteMark == 0);//&& t.OrganizeId == companyid//一级机构可以操作
                        if (entity != null)
                        {
                            entity.MinPrice = minPrice;//成本价
                            entity.Price = price;//售价
                            entity.ChaPrice = chaPrice;//利润
                            entity.CheckPrice = checkPrice;//核算价
                            entity.Modify(entity.TelphoneID);
                            db.Update(entity);
                            returnMsg += telphone + " 已调价</br>";
                        }
                        else
                        {
                            returnMsg += telphone + " 不属于本公司或已售出</br>";
                        }
                    }

                }
                db.Commit();
            }
            else
            {
                returnMsg = "未接受到任何数据";
            }

            return returnMsg;
        }

        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
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
                //根据前7位确定城市网络
                IRepository db = new RepositoryFactory().BaseRepository().BeginTrans();
                string Number7 = entity.Telphone.Substring(0, 7);

                var TelphoneData = db.FindEntity<TelphoneDataEntity>(t => t.Number7 == Number7);
                if (TelphoneData != null)
                {
                    if (string.IsNullOrEmpty(TelphoneData.City))
                    {
                        throw new Exception("号段城市为空：" + Number7);
                    }
                    else
                    {
                        entity.City = TelphoneData.City.Replace("市", "");
                        entity.CityId = TelphoneData.CityId;
                        entity.Operator = TelphoneData.Operate;
                        entity.Brand = TelphoneData.Brand;
                    }
                }
                else
                {
                    throw new Exception("号段不存在：" + Number7);
                }
                entity.Create();
                this.BaseRepository().Insert(entity);
            }
        }

        /// <summary>
        /// 批量（新增）
        /// </summary>
        /// <param name="dtSource">实体对象</param>
        /// <returns></returns>
        public string BatchAddEntity(DataTable dtSource)
        {
            int rowsCount = dtSource.Rows.Count;
            List<string> ts = new List<string>();
            List<string> Number7s = new List<string>();

            IRepository db = new RepositoryFactory().BaseRepository().BeginTrans();
            for (int i = 0; i < rowsCount; i++)
            {
                //先检验当前文件是否存在重复号码
                string telphone = dtSource.Rows[i][0].ToString();
                if (ts.Contains(telphone))
                {
                    return "上传文件存在重复号码，导入失败，请先去重再导入！"+ telphone;
                }
                ts.Add(telphone);
                
                //先检验当前文件号段不存在
                string Number7 = telphone.Substring(0, 7);
                if (!Number7s.Contains(Number7))
                {
                    Number7s.Add(Number7);
                    var TelphoneData = db.FindEntity<TelphoneDataEntity>(t => t.Number7 == Number7);
                    if (TelphoneData == null)
                    {
                        return "号段不存在：" + Number7;
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
                        var liang_Data = db.FindEntity<TelphoneLiangH5Entity>(t => t.Telphone == telphone && t.DeleteMark != 1);//删除过的可以再次导入
                        if (liang_Data != null)
                        {
                            db.Delete<TelphoneLiangH5Entity>(liang_Data.TelphoneID);//删除老的数据
                        }

                        //根据前7位确定城市和运营商
                        string Number7 = telphone.Substring(0, 7);
                        string Province ="", City = "", CityId = "", Operator = "", Brand = "";
                        var TelphoneData = db.FindEntity<TelphoneDataEntity>(t => t.Number7 == Number7);
                        if (TelphoneData != null)
                        {
                            if (string.IsNullOrEmpty(TelphoneData.City))
                            {
                                return "号段城市为空：" + Number7;
                            }
                            else
                            {
                                City = TelphoneData.City.Replace("市", "");
                                CityId = TelphoneData.CityId;
                                Operator = TelphoneData.Operate;
                                Brand = TelphoneData.Brand;
                                Province = TelphoneData.Province;
                            }
                        }
                        else
                        {
                            return "号段不存在：" + Number7;
                        }

                        //价格
                        if (string.IsNullOrEmpty(dtSource.Rows[i][1].ToString()))
                        {
                            return telphone + "价格为空";
                        }
                        decimal Price = Convert.ToDecimal(dtSource.Rows[i][1].ToString());
                        //成本价
                        if (string.IsNullOrEmpty(dtSource.Rows[i][2].ToString()))
                        {
                            return telphone + "成本价为空";
                        }
                        decimal MinPrice = Convert.ToDecimal(dtSource.Rows[i][2].ToString());
                        //利润
                        decimal ChaPrice = Price - MinPrice;

                        //核算价
                        decimal CheckPrice = Convert.ToDecimal(dtSource.Rows[i][3].ToString());

                        //类别
                        string itemName = dtSource.Rows[i][4].ToString();
                        if (string.IsNullOrEmpty(itemName))
                        {
                            return telphone + "类别为空";
                        }
                        string itemNCode = "";
                        var DataItemDetail = db.FindEntity<DataItemDetailEntity>(t => t.ItemName == itemName);
                        if (DataItemDetail != null)
                        {
                            itemNCode = DataItemDetail.ItemValue;
                        }
                        else
                        {
                            return "类型不存在：" + itemName + ",请在数据字典里维护此类型。";
                        }

                        //套餐
                        string Package = dtSource.Rows[i][5].ToString();
                        //状态
                        string existStr = dtSource.Rows[i][6].ToString();

                        if (string.IsNullOrEmpty(existStr))
                        {
                            return telphone + "现卡/代售/秒杀状态为空";
                        }
                        if (existStr != "秒杀" && existStr != "现卡" && existStr != "预售")
                        {
                            return telphone + "现卡/代售/秒杀状态填写错误";
                        }
                        int existMark = 0;
                        if (existStr == "秒杀")
                        {
                            existMark = 2;
                        }
                        else if (existStr == "现卡")
                        {
                            existMark = 1;
                        }
                        else
                        {
                            existMark = 0;
                        }

                        //防止不含推广价此列 报错提示：无法找到列 7
                        decimal? MaxPrice = null;
                        if (columns == 8)
                        {
                            //推广价
                            string maxPriceStr = dtSource.Rows[i][7].ToString();
                            //如果当前列的单元格报错，也会转类型错误
                            if (!string.IsNullOrEmpty(maxPriceStr))
                            {
                                MaxPrice = Convert.ToDecimal(maxPriceStr);
                            }
                        }

                        //添加靓号
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
                return "导入成功："+ commitInt;
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
        /// 批量（删除）
        /// </summary>
        /// <param name="dtSource">实体对象</param>
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
                        var liang_Data = db.FindList<TelphoneLiangH5Entity>(t => t.Telphone == telphone && t.DeleteMark != 1);//删除过的可以再次导入
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
            return "批量删除完成！"+ok;
        }
    }
}
