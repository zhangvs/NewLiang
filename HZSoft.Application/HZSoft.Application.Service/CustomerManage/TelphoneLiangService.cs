using HZSoft.Application.Code;
using HZSoft.Application.Entity.BaseManage;
using HZSoft.Application.Entity.CustomerManage;
using HZSoft.Application.Entity.SystemManage;
using HZSoft.Application.IService.BaseManage;
using HZSoft.Application.IService.CustomerManage;
using HZSoft.Application.Service.BaseManage;
using HZSoft.Cache.Redis;
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
    public class TelphoneLiangService : RepositoryFactory<TelphoneLiangEntity>, TelphoneLiangIService
    {
        private IOrganizeService orgService = new OrganizeService();
        private TelphoneLiangVipIService vipService = new TelphoneLiangVipService();
        private TelphoneVipShareIService vipShareService = new TelphoneVipShareService();
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        public IEnumerable<TelphoneLiangEntity> GetPageList(Pagination pagination, string queryJson)
        {
            var queryParam = queryJson.ToJObject();
            string strSql = "select * from TelphoneLiang where DeleteMark <> 1 and EnabledMark <> 1";
            //������̨
            if (!queryParam["OrganizeId"].IsEmpty())
            {
                string OrganizeId = queryParam["OrganizeId"].ToString();
                strSql += " and OrganizeId = '" + OrganizeId + "'";
            }
            else
            {
                if (!OperatorProvider.Provider.Current().IsSystem && OperatorProvider.Provider.Current().UserId != "3303254b-7cd3-4a25-abd3-bb2542a08df9")//������Բ鿴�����к���
                {
                    string companyId = OperatorProvider.Provider.Current().CompanyId;
                    //һ���������Բ鿴�ϼ�0���ĺ���⣬��Ϊ�Լ���Ա��
                    strSql += " and OrganizeId IN( select OrganizeId from Base_Organize where OrganizeId='" + companyId
                        + "' or OrganizeId =(SELECT ParentId FROM Base_Organize WHERE OrganizeId='" + companyId + "'))";
                }
            }
            strSql += GetSql(queryJson);
            return this.BaseRepository().FindList(strSql.ToString(), pagination);
        }

        /// <summary>
        /// ������������ҳ��
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public IEnumerable<TelphoneLiangEntity> GetPageListJoin(Pagination pagination, string queryJson)
        {
            var queryParam = queryJson.ToJObject();
            string strSql = "select * from TelphoneLiang where DeleteMark <> 1 and EnabledMark <> 1";
            //������̨
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
                    var organizeData = orgService.GetEntity(companyId);
                    if (!string.IsNullOrEmpty(organizeData.OrganizeId))
                    {
                        //string inOrg = GetInOrg(companyId, organizeData.ParentId, organizeData.TopOrganizeId);
                        List<string> vipList= vipService.GetVipOrgList(companyId, organizeData.ParentId, organizeData.TopOrganizeId);
                        string inOrg = GetOtherOrg(vipList);//�Զ������ȣ�����ƽ̨���
                        if (!string.IsNullOrEmpty(inOrg))
                        {
                            strSql += " and OrganizeId IN(" + inOrg + ")";
                        }
                    }
                }
            }
            strSql += GetSql(queryJson);
            return this.BaseRepository().FindList(strSql.ToString(), pagination);
        }

        /// <summary>
        /// ����jsonƴ��sql�������������������
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        private string GetSql(string queryJson)
        {
            string strSql = "";
            var queryParam = queryJson.ToJObject();
            
            //���ݱ��
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
            //��������
            if (!queryParam["City"].IsEmpty())
            {
                string City = queryParam["City"].ToString();
                strSql += " and City like '%" + City + "%'";
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
                if (jgqj.Length > 1)
                {
                    if (!string.IsNullOrEmpty(jgqj[1]))
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
                if (!string.IsNullOrEmpty(jgqj[1]))
                {
                    strSql += " and MaxPrice <= '" + jgqj[1] + "'";
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
                strSql += " and Operator = '" + Operator + "'";
            }
            return strSql;
        }


            /// <summary>
            /// ������صĻ�������������ƽ̨
            /// </summary>
            /// <param name="organizeId"></param>
            /// <param name="pid"></param>
            /// <param name="top"></param>
            /// <returns></returns>
            private string GetInOrg(string organizeId, string pid, string top)
        {
            string inOrg = "";
            string shareOrg = RedisCache.Get<string>("ShareOrg");
            bool isJoin = false;
            //�ж�vip�������
            //1.������
            if (vipService.GetVipByOrganizeId(organizeId))
            {
                if (!string.IsNullOrEmpty(shareOrg))
                {
                    if (shareOrg.Contains(organizeId))
                    {
                        isJoin = true;
                        shareOrg = shareOrg.Replace("'" + organizeId + "',", "").Replace("'" + organizeId + "'", "");
                    }
                }
                inOrg += "'" + organizeId + "',";
            }
            else
            {
                organizeId = "";
            }
            //2.����ֱ���ϼ�
            if (!string.IsNullOrEmpty(pid) && organizeId != pid)
            {
                if (vipService.GetVipByOrganizeId(pid))
                {
                    if (!string.IsNullOrEmpty(shareOrg))
                    {
                        if (shareOrg.Contains(pid))
                        {
                            isJoin = true;
                            shareOrg = shareOrg.Replace("'" + pid + "',", "").Replace("'" + pid + "'", "");
                        }
                        inOrg += "'" + pid + "',";
                    }
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
                if (vipService.GetVipByOrganizeId(top))
                {
                    if (!string.IsNullOrEmpty(shareOrg))
                    {
                        if (shareOrg.Contains(top))
                        {
                            isJoin = true;
                            shareOrg = shareOrg.Replace("'" + top + "',", "").Replace("'" + top + "'", "");
                        }
                    }
                    inOrg += "'" + top + "',";
                }
                else
                {
                    top = "";
                }
            }
            else
            {
                top = "";
            }

            //�������ȫ�����ڣ����ؿ�
            if (string.IsNullOrEmpty(inOrg))
            {
                return null;
            }

            //�Ƿ���빲��ƽ̨
            if (isJoin)
            {
                inOrg += shareOrg;
            }

            return inOrg.Trim(',');
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
            //���û���Զ���ѡ��������������ж��Ƿ���빲��ƽ̨
            if (string.IsNullOrEmpty(shareOrg))
            {
                //2.����ƽ̨
                shareOrg = RedisCache.Get<string>("ShareOrg");
                bool isJoin = false;
                if (!string.IsNullOrEmpty(shareOrg))
                {
                    foreach (var item in vipList)
                    {
                        //���Ҳ�����˹���ƽ̨���ѱ������ȥ��
                        if (shareOrg.Contains(item))
                        {
                            shareOrg = shareOrg.Replace("'" + item + "',", "").Replace("'" + item + "'", "");
                            isJoin = true;//���빲��ƽ̨
                        }
                    }
                }
                //û�м����shareOrg����Ϊ��
                if (!isJoin)
                {
                    shareOrg = "";
                }
            }

            return shareOrg.Trim(',');
        }

        /// <summary>
        /// H5�˲�ѯ��ť��Listҳ�棬��ҳ����
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public IEnumerable<TelphoneLiangEntity> GetPageListH5LX(Pagination pagination, string queryJson)
        {
            //����ǰ̨H5
            List<string> viplist = null;
            //������sql
            string vipOrg = "";
            string ownOrgSql = "";//Ĭ��Ϊ��
            //����ƽ̨����sql
            string shareOrg = "";
            string shareOrgSql = "";//Ĭ��Ϊ��

            var queryParam = queryJson.ToJObject();
            if (queryParam["OrganizeIdH5"].IsEmpty())
            {
                return null;
            }

            string organizeId = queryParam["OrganizeIdH5"].ToString();
            string pid = queryParam["pid"].ToString();
            string top = queryParam["top"].ToString();

            string allOrg = "";
            //���˳�vip����
            viplist = vipService.GetVipOrgList(organizeId, pid, top);
            foreach (var item in viplist)
            {
                vipOrg += "'" + item + "',";
            }
            //��������ж�Ϊ�գ�����null��vipȫ�����ڲ�����
            if (vipOrg == "")
            {
                return null;
            }

            //1.���������ϵ
            vipOrg = vipOrg.Trim(',');
            ownOrgSql = " and OrganizeId IN(" + vipOrg + ")";
            string ownWhere = ownOrgSql + GetSql(queryJson);

            //2.����ƽ̨����������
            string shareSql = "";
            shareOrg = GetOtherOrg(viplist);
            if (!shareOrg.IsEmpty())
            {
                allOrg = vipOrg + "," + shareOrg;
                shareOrgSql = " and OrganizeId IN(" + shareOrg + ")";
                string shareWhere = shareOrgSql + GetSql(queryJson);
                shareSql = @" UNION all
 SELECT TelphoneID,Telphone,City,Operator,Price,MaxPrice,Grade,ExistMark,CASE ExistMark WHEN '2' THEN 'ƽ̨��ɱ' WHEN '1' THEN 'ƽ̨�ֿ�' ELSE 'ƽ̨Ԥ��' END Description,Package,EnabledMark,OrganizeId OrganizeId FROM TelphoneLiang
 WHERE  SellMark<>1 AND DeleteMark<>1 and EnabledMark <> 1 and ExistMark=1 "//ƽֻ̨���ֿ���
    + shareWhere;
            }
            else
            {
                allOrg = vipOrg;
            }

            //��������0��
            string strSql = @" SELECT * FROM (
 SELECT TelphoneID,Telphone,City,Operator,Price,MaxPrice,Grade,ExistMark,CASE ExistMark WHEN '2' THEN '��Ӫ��ɱ' WHEN '1' THEN '��Ӫ�ֿ�' ELSE '��ӪԤ��' END Description,Package,EnabledMark,OrganizeId FROM TelphoneLiang 
 WHERE SellMark<>1 AND DeleteMark<>1 and EnabledMark <> 1 " + ownWhere//�����������ɱ���ֿ���Ԥ��
//+ otherOrgSql//���۹���ʡ��
+ shareSql
                + " )t ";


            //���ջ�������
            string[] orderbyOrg = allOrg.Split(',');
            string orderbySql = " case ";
            for (int i = 0; i < orderbyOrg.Length; i++)
            {
                orderbySql += " when OrganizeId=" + orderbyOrg[i] + " then " + i;
            }
            orderbySql += " end ASC,";
            pagination.sidx = orderbySql + pagination.sidx + " " + pagination.sord;

            return this.BaseRepository().FindList(strSql.ToString(), pagination);
        }

        /// <summary>
        /// H5�˲�ѯ��ť��Listҳ�棬��ҳ����
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public IEnumerable<TelphoneLiangEntity> GetPageListH5(Pagination pagination, string queryJson)
        {
            //����ǰ̨H5
            List<string> viplist = null;
            //������sql
            string vipOrg = "";
            string ownOrgSql = "";//Ĭ��Ϊ��
            //����ƽ̨����sql
            string shareOrg = "";
            string shareOrgSql = "";//Ĭ��Ϊ��

            var queryParam = queryJson.ToJObject();
            if (queryParam["OrganizeIdH5"].IsEmpty())
            {
                return null;
            }

            string organizeId = queryParam["OrganizeIdH5"].ToString();
            string pid = queryParam["pid"].ToString();
            string top = queryParam["top"].ToString();

            string allOrg = "";
            //���˳�vip����
            viplist = vipService.GetVipOrgList(organizeId, pid, top);
            foreach (var item in viplist)
            {
                vipOrg += "'" + item + "',";
            }
            //��������ж�Ϊ�գ�����null��vipȫ�����ڲ�����
            if (vipOrg == "")
            {
                return null;
            }

            //1.���������ϵ
            vipOrg = vipOrg.Trim(',');
            ownOrgSql = " and OrganizeId IN(" + vipOrg + ")";
            string ownWhere = ownOrgSql + GetSql(queryJson);

            //2.���ۻ������֧
            string otherWhere = ownWhere.Replace("OrganizeId", "CreateOrganizeId");
            string otherOrgSql = @" UNION all
 SELECT TelphoneID,Telphone,City,Operator,Price,MaxPrice,Grade,CASE ExistMark WHEN '2' THEN '������ɱ' WHEN '1' THEN '�����ֿ�' ELSE '����Ԥ��' END Description,Package,EnabledMark,CreateOrganizeId OrganizeId FROM TelphoneLiangOther
 WHERE EnabledMark = 1  "
+ otherWhere;

            //3.����ƽ̨����������
            string shareSql = "";
            shareOrg = GetOtherOrg(viplist);
            if (!shareOrg.IsEmpty())
            {
                allOrg = vipOrg + "," + shareOrg;
                shareOrgSql = " and OrganizeId IN(" + shareOrg + ")";
                string shareWhere = shareOrgSql + GetSql(queryJson);
                shareSql = @" UNION all
 SELECT TelphoneID,Telphone,City,Operator,Price,MaxPrice,Grade,CASE ExistMark WHEN '2' THEN 'ƽ̨��ɱ' WHEN '1' THEN 'ƽ̨�ֿ�' ELSE 'ƽ̨Ԥ��' END Description,Package,EnabledMark,OrganizeId OrganizeId FROM TelphoneLiang
 WHERE  SellMark<>1 AND DeleteMark<>1 and EnabledMark <> 1 "// and Grade IN (0,1,2,3,11,12,4,5,6,9,13,14,15,17,18,19,20,903,902,901,904,905,906,907,908,909,910,911,912,913,914,915,1301,1302,1303,1304,1308)
    + shareWhere;
            }
            else
            {
                allOrg = vipOrg;
            }

            //��������0��
            string strSql = @" SELECT * FROM (
 SELECT TelphoneID,Telphone,City,Operator,Price,MaxPrice,Grade,CASE ExistMark WHEN '2' THEN '��Ӫ��ɱ' WHEN '1' THEN '��Ӫ�ֿ�' ELSE '��ӪԤ��' END Description,Package,EnabledMark,OrganizeId FROM TelphoneLiang 
 WHERE SellMark<>1 AND DeleteMark<>1 and EnabledMark <> 1 " + ownWhere
//+ otherOrgSql//���۹���ʡ��
+ shareSql
                + " )t ";


            //���ջ�������
            string[] orderbyOrg = allOrg.Split(',');
            string orderbySql = " case ";
            for (int i = 0; i < orderbyOrg.Length; i++)
            {
                orderbySql += " when OrganizeId=" + orderbyOrg[i] + " then " + i;
            }
            orderbySql += " end ASC,";
            pagination.sidx = orderbySql+ pagination.sidx+" "+ pagination.sord;

            return this.BaseRepository().FindList(strSql.ToString(), pagination);
        }
// Select * From (Select ROW_NUMBER() Over (Order By  case  when OrganizeId='485b4733-c958-4f46-94dd-3ff8ce444d2c' then 0 when OrganizeId='a5a962da-57e1-4ad4-87b2-bbdcd1b7cc92' then 1 when OrganizeId='2d418431-d2cd-41d7-a2f9-a749ca4ad907' then 2 end ASC,TelphoneID desc) As rowNum, * From ( SELECT * FROM (
// SELECT TelphoneID,Telphone,City,Operator,Price,Grade,CASE ExistMark WHEN '2' THEN '��Ӫ��ɱ' WHEN '1' THEN '��Ӫ�ֿ�' ELSE '��ӪԤ��' END Description,Package,EnabledMark,OrganizeId FROM TelphoneLiang 
// WHERE SellMark<>1 AND DeleteMark<>1 and EnabledMark <> 1  and OrganizeId IN('485b4733-c958-4f46-94dd-3ff8ce444d2c') and Grade in (1,2) and SellMark = 0 
// UNION all
// SELECT TelphoneID,Telphone,City,Operator,Price,Grade,CASE ExistMark WHEN '2' THEN '������ɱ' WHEN '1' THEN '�����ֿ�' ELSE '����Ԥ��' END Description,Package,EnabledMark,CreateOrganizeId OrganizeId FROM TelphoneLiangOther
// WHERE EnabledMark = 1 and CreateOrganizeId IN('485b4733-c958-4f46-94dd-3ff8ce444d2c') and Grade in (1,2) and SellMark = 0 
// UNION all
// SELECT TelphoneID,Telphone,City,Operator,Price,Grade,CASE ExistMark WHEN '2' THEN 'ƽ̨��ɱ' WHEN '1' THEN 'ƽ̨�ֿ�' ELSE 'ƽ̨Ԥ��' END Description,Package,EnabledMark,OrganizeId OrganizeId FROM TelphoneLiang
// WHERE  SellMark<>1 AND DeleteMark<>1 and EnabledMark <> 1 and Grade IN (0,1,2,3,11,12,4,5,6,9,13,14,15,17,18,19,20,903,902,901,904,905,906,907,908,909,910,911,912,913,914,915,1301,1302,1303,1304,1308)  and OrganizeId IN('a5a962da-57e1-4ad4-87b2-bbdcd1b7cc92','2d418431-d2cd-41d7-a2f9-a749ca4ad907') and Grade in (1,2) and SellMark = 0 )t ) As T ) As N Where rowNum > 0 And rowNum <= 40
        /// <summary>
        /// ��ҳ���־�Ʒ�Ƽ�����ɱ
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<TelphoneLiangEntity> GetList(string queryJson)
        {
            //����ǰ̨H5
            List<string> viplist = null;
            //������sql
            string vipOrg = "";
            string ownOrgSql = "";//Ĭ��Ϊ��
            //����ƽ̨����sql
            string shareOrg = "";
            string shareOrgSql = "";//Ĭ��Ϊ��

            var queryParam = queryJson.ToJObject();
            if (queryParam["OrganizeIdH5"].IsEmpty())
            {
                return null;
            }

            string organizeId = queryParam["OrganizeIdH5"].ToString();
            string pid = queryParam["pid"].ToString();
            string top = queryParam["top"].ToString();
            //���˳�vip����
            viplist = vipService.GetVipOrgList(organizeId, pid, top);
            foreach (var item in viplist)
            {
                vipOrg += "'" + item + "',";
            }
            //��������ж�Ϊ�գ�����null��vipȫ�����ڲ�����
            if (vipOrg == "")
            {
                return null;
            }


            //1.���������ϵ
            vipOrg = vipOrg.Trim(',');
            ownOrgSql = " and OrganizeId IN(" + vipOrg + ")";
            string ownWhere = ownOrgSql + GetSql(queryJson);

            //2.���ۻ������֧
            string otherWhere = ownWhere.Replace("OrganizeId", "CreateOrganizeId");
            string otherOrgSql = @" UNION all
 SELECT TelphoneID,Telphone,City,Operator,Price,Grade,CASE ExistMark WHEN '2' THEN '������ɱ' WHEN '1' THEN '�����ֿ�' ELSE '����Ԥ��' END Description,Package,EnabledMark,CreateOrganizeId OrganizeId FROM TelphoneLiangOther
 WHERE EnabledMark = 1  "
+ otherWhere;

            //3.����ƽ̨����������
            string shareSql = "";
            shareOrg = GetOtherOrg(viplist);
            if (!shareOrg.IsEmpty())
            {
                shareOrgSql = " and OrganizeId IN(" + shareOrg + ")";
                string shareWhere = shareOrgSql + GetSql(queryJson);
                shareSql = @" UNION all
 SELECT TelphoneID,Telphone,City,Operator,Price,Grade,CASE ExistMark WHEN '2' THEN 'ƽ̨��ɱ' WHEN '1' THEN 'ƽ̨�ֿ�' ELSE 'ƽ̨Ԥ��' END Description,Package,EnabledMark,OrganizeId OrganizeId FROM TelphoneLiang
 WHERE  SellMark<>1 AND DeleteMark<>1 and EnabledMark <> 1 "// and Grade IN (0,1,2,3,11,12,4,5,6,9,13,14,15,17,18,19,20,903,902,901,904,905,906,907,908,909,910,911,912,913,914,915,1301,1302,1303,1304,1308)
    + shareWhere;
            }

            //��������0��
            string strSql = @" SELECT TOP(10) * FROM (
 SELECT TelphoneID,Telphone,City,Operator,Price,Grade,CASE ExistMark WHEN '2' THEN '��Ӫ��ɱ' WHEN '1' THEN '��Ӫ�ֿ�' ELSE '��ӪԤ��' END Description,Package,EnabledMark,OrganizeId FROM TelphoneLiang 
 WHERE SellMark<>1 AND DeleteMark<>1 and EnabledMark <> 1 " + ownWhere
//+ otherOrgSql//���۹���ʡ��
+ shareSql
                + " )t  ORDER BY Description desc,price ";//ORDER BY EnabledMark, right(Telphone,1),Description,price desc

            return this.BaseRepository().FindList(strSql.ToString());
        }

        /// <summary>
        /// �������ۺ�������
        /// </summary>
        /// <param name="organizeId">����</param>
        /// <returns>���ط�ҳ�б�</returns>
        public DataTable GetLiangCountByOrg(string organizeId)
        {
            if (string.IsNullOrEmpty(organizeId))
            {
                return null;
            }
            else
            {
                string strSql = $"select count(1) from TelphoneLiang where DeleteMark <> 1 and EnabledMark <> 1 and SellMark <> 1 and OrganizeId='{organizeId}'";
                return this.BaseRepository().FindTable(strSql.ToString());
            }

        }

        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public TelphoneLiangEntity GetEntity(int? keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        /// <summary>
        /// ��ȡ�����б����ã�
        /// </summary>
        /// <param name="organizeId">���Ź�˾</param>
        /// <param name="Grade">��ѯ����</param>
        /// <param name="city"></param>
        /// <param name="ExistMark"></param>
        /// <returns>�����б�</returns>
        public IEnumerable<TelphoneLiangEntity> GetGrade(string organizeId, string Grade, string city, int? ExistMark)
        {
            if (string.IsNullOrEmpty(organizeId))
            {
                return null;
            }
            var organizeData = orgService.GetEntity(organizeId);
            if (!string.IsNullOrEmpty(organizeData.OrganizeId))
            {
                string orgSql = "";
                //��������
                string gradeSql = "";
                if (!Grade.IsEmpty())
                {
                    gradeSql += " and Grade = '" + Grade + "'";
                }
                //״̬����
                string existSql = "";
                if (!ExistMark.IsEmpty())
                {
                    existSql += " and ExistMark = " + ExistMark;
                }
                //��������
                string citySql = "";
                if (!string.IsNullOrEmpty(city))
                {
                    if (city.Contains("0000"))
                    {
                        citySql += " and cityid like '" + city.Substring(0, 2) + "%'";
                    }
                    else if (city != "0")
                    {
                        citySql += " and cityid = '" + city + "'";
                    }
                }
                //1.������ 2.����ֱ���ϼ� 3.��������
                //orgSql = "and OrganizeId IN('" + organizeId + "','" + organizeData.ParentId + "','" + organizeData.TopOrganizeId + "')";
                //�ж�vip�������
                //1.������
                if (vipService.GetVipByOrganizeId(organizeId))
                {
                    organizeId = "'" + organizeId + "',";
                }
                else
                {
                    organizeId = "";
                }
                //2.����ֱ���ϼ�
                string pid = "";
                if (!string.IsNullOrEmpty(organizeData.ParentId) && organizeData.ParentId != organizeData.OrganizeId)
                {
                    if (vipService.GetVipByOrganizeId(organizeData.ParentId))
                    {
                        pid = "'" + organizeData.ParentId + "',";
                    }
                }
                // 3.��������
                string top = "";
                if (!string.IsNullOrEmpty(organizeData.TopOrganizeId) && organizeData.TopOrganizeId != organizeData.OrganizeId && organizeData.TopOrganizeId != organizeData.ParentId)
                {
                    if (vipService.GetVipByOrganizeId(organizeData.TopOrganizeId))
                    {
                        top = "'" + organizeData.TopOrganizeId + "',";
                    }
                }
                string inOrg = organizeId + pid + top;
                //�������ȫ�����ڣ����ؿ�
                if (string.IsNullOrEmpty(inOrg))
                {
                    return null;
                }
                orgSql = " and OrganizeId IN(" + inOrg + ")";
                orgSql = orgSql.Replace(",)", ")");


                //4.������ۻ���vip����û�е���
                string otherOrgSql = @" UNION all
 SELECT Telphone,City,Operator,Price,Grade,CASE ExistMark WHEN '2' THEN '��ɱ' WHEN '1' THEN '�ֿ�' ELSE 'Ԥ��' END Description,Package,EnabledMark FROM TelphoneLiangOther
 WHERE EnabledMark = 1 "
+ orgSql.Replace("OrganizeId", "CreateOrganizeId")
+ gradeSql
+ existSql
+ citySql;

                string strSql = @" SELECT * FROM (
 SELECT Telphone,City,Operator,Price,Grade,CASE ExistMark WHEN '2' THEN '��ɱ' WHEN '1' THEN '�ֿ�' ELSE 'Ԥ��' END Description,Package,EnabledMark FROM TelphoneLiang 
 WHERE SellMark<>1 AND DeleteMark<>1 and EnabledMark <> 1 "
                    + orgSql
                    + gradeSql
                    + existSql
                    + citySql
                    + otherOrgSql
                    + " )t ORDER BY EnabledMark, right(Telphone,1),Description,price desc";
                return this.BaseRepository().FindList(strSql.ToString());

            }
            else
            {
                return null;
            }
        }

        
        /// <summary>
        /// ���������Զ���ȫ���ϰ汾���ڣ��°��������ϣ�
        /// </summary>
        /// <param name="telphone"></param>
        /// <param name="organizeId"></param>
        /// <param name="city"></param>
        /// <returns></returns>
        public IEnumerable<TelphoneLiangEntity> GetList(string telphone, string organizeId,string city)
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
                if (!string.IsNullOrEmpty(city))
                {
                    if (city.Contains("0000"))
                    {
                        citySql += " and cityid like '" + city.Substring(0, 2) + "%'";
                    }
                    else if (city != "0")
                    {
                        citySql += " and cityid = '" + city + "'";
                    }
                }

                string orgSql = "";
                //�ж�vip�������
                //1.������
                if (vipService.GetVipByOrganizeId(organizeId))
                {
                    organizeId = "'" + organizeId + "',";
                }
                else
                {
                    organizeId = "";
                }
                //2.����ֱ���ϼ�
                string pid = "";
                if (!string.IsNullOrEmpty(organizeData.ParentId) && organizeData.ParentId != organizeData.OrganizeId)
                {
                    if (vipService.GetVipByOrganizeId(organizeData.ParentId))
                    {
                        pid = "'" + organizeData.ParentId + "',";
                    }
                }
                // 3.��������
                string top = "";
                if (!string.IsNullOrEmpty(organizeData.TopOrganizeId) && organizeData.TopOrganizeId != organizeData.OrganizeId && organizeData.TopOrganizeId != organizeData.ParentId)
                {
                    if (vipService.GetVipByOrganizeId(organizeData.TopOrganizeId))
                    {
                        top = "'" + organizeData.TopOrganizeId + "',";
                    }
                }

                string inOrg = organizeId + pid + top;
                //�������ȫ�����ڣ����ؿ�
                if (string.IsNullOrEmpty(inOrg))
                {
                    return null;
                }
                orgSql = " and OrganizeId IN(" + inOrg + ")";
                orgSql = orgSql.Replace(",)", ")");

                //�������ȫ�����ڣ����ؿ�
                if (string.IsNullOrEmpty(organizeId) && string.IsNullOrEmpty(pid) && string.IsNullOrEmpty(top))
                {
                    return null;
                }
                //4.�������ۻ���
                string otherOrgSql = "";
                //������ۻ���vip����û�е���
                otherOrgSql = @" UNION all
 SELECT Telphone,City,Grade,ExistMark,price,EnabledMark FROM TelphoneLiangOther
 WHERE EnabledMark = 1 "
+ orgSql.Replace("OrganizeId", "CreateOrganizeId")
+ citySql
+ telSql;
                
                //������룬��ʾǰ20������
                string strSql = @" SELECT TOP 20 * FROM (
 SELECT Telphone,City,Grade,ExistMark,price,EnabledMark FROM TelphoneLiang
  WHERE SellMark<>1 AND DeleteMark<>1 and EnabledMark <> 1 "
                    + orgSql
                    + citySql
                    + telSql
                    + otherOrgSql
                    + " )t ORDER BY EnabledMark,Grade,ExistMark desc,price desc";

                return this.BaseRepository().FindList(strSql.ToString());
            }
            else
            {
                return null;
            }
            

        }

        /// <summary>
        /// �ֻ��˵�������б�����ʱ��,ֱ��ȫ����������Ϊ��Ȼ���Ե�����룬�ͱ�Ȼ��Ȩ�޷�Χ�ڣ������ظ�У��ú����Ƿ����ڱ�������Χ
        /// </summary>
        /// <param name="organizeId">��ǰ����</param>
        /// <param name="telphone">�ֻ���</param>
        /// <returns></returns>
        public IEnumerable<TelphoneLiangEntity> GetEntityByTel(string organizeId, string telphone)
        {
            if (string.IsNullOrEmpty(organizeId))
            {
                return null;
            }
            string strSql = "select TelphoneID,Telphone,City,Operator,MinPrice,Price,ChaPrice,Grade,CASE ExistMark WHEN '2' THEN '��ɱ' WHEN '1' THEN '�ֿ�' ELSE 'Ԥ��' END Description,Package,OrganizeId from TelphoneLiang"
               + " where DeleteMark <> 1 and EnabledMark <> 1 and SellMark <> 1";
            if (!telphone.IsEmpty())
            {
                strSql += " and Telphone='" + telphone + "'";
            }
            return this.BaseRepository().FindList(strSql);

        }
        public TelphoneLiangEntity GetEntityByOrgTel(string telphone)
        {
            return this.BaseRepository().FindEntity(t => t.Telphone == telphone);
        }

        /// <summary>
        /// �ֻ��˵����ѯ��ť�����ʱ�򣬷�ֹ��ѯ�����������ĺ��룬��������жϣ��ϰ汾���ڣ��°��������ϣ�
        /// </summary>
        /// <param name="organizeId">��ǰ����</param>
        /// <param name="telphone">�ֻ���</param>
        /// <returns></returns>
        public IEnumerable<TelphoneLiangEntity> GetEntityByOrgTel(string organizeId, string telphone)
        {

            if (string.IsNullOrEmpty(organizeId))
            {
                return null;
            }
            var organizeData = orgService.GetEntity(organizeId);
            if (!string.IsNullOrEmpty(organizeData.OrganizeId))
            {
                string telSql = "";
                if (!telphone.IsEmpty())
                {
                    telSql += " and Telphone='" + telphone + "'";
                }
                string orgSql = "";
                //1.������ 2.����ֱ���ϼ� 3.��������
                //orgSql = "and OrganizeId IN('" + organizeId + "','" + organizeData.ParentId + "','" + organizeData.TopOrganizeId + "')";
                //�ж�vip�������
                //1.������
                if (vipService.GetVipByOrganizeId(organizeId))
                {
                    organizeId = "'" + organizeId + "',";
                }
                else
                {
                    organizeId = "";
                }
                //2.����ֱ���ϼ�
                string pid = "";
                if (!string.IsNullOrEmpty(organizeData.ParentId) && organizeData.ParentId != organizeData.OrganizeId)
                {
                    if (vipService.GetVipByOrganizeId(organizeData.ParentId))
                    {
                        pid = "'" + organizeData.ParentId + "',";
                    }
                }
                // 3.��������
                string top = "";
                if (!string.IsNullOrEmpty(organizeData.TopOrganizeId) && organizeData.TopOrganizeId != organizeData.OrganizeId && organizeData.TopOrganizeId != organizeData.ParentId)
                {
                    if (vipService.GetVipByOrganizeId(organizeData.TopOrganizeId))
                    {
                        top = "'" + organizeData.TopOrganizeId + "',";
                    }
                }
                string inOrg = organizeId + pid + top;
                //�������ȫ�����ڣ����ؿ�
                if (string.IsNullOrEmpty(inOrg))
                {
                    return null;
                }
                orgSql = " and OrganizeId IN(" + inOrg + ")";
                orgSql = orgSql.Replace(",)", ")");
                //4.�������ۻ���
                string otherOrgSql = "";
                //������ۻ���vip����û�е���
                otherOrgSql = @" UNION all
 SELECT TelphoneID,Telphone,City,Operator,MinPrice,Price,ChaPrice,Grade,CASE ExistMark WHEN '2' THEN '��ɱ' WHEN '1' THEN '�ֿ�' ELSE 'Ԥ��' END Description,Package,OrganizeId FROM TelphoneLiangOther
 WHERE EnabledMark = 1 "
+ orgSql.Replace("OrganizeId", "CreateOrganizeId")
+ telSql;

                string strSql = @" 
 SELECT TelphoneID,Telphone,City,Operator,MinPrice,Price,ChaPrice,Grade,CASE ExistMark WHEN '2' THEN '��ɱ' WHEN '1' THEN '�ֿ�' ELSE 'Ԥ��' END Description,Package,OrganizeId FROM TelphoneLiang 
 WHERE SellMark<>1 AND DeleteMark<>1 and EnabledMark <> 1 "
                    + telSql
                    + orgSql
                    + otherOrgSql;
                return this.BaseRepository().FindList(strSql.ToString());

            }
            else
            {
                return null;
            }
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
                    TelphoneLiangEntity entity = this.BaseRepository().FindEntity(id);
                    entity.Modify(custIds[i]);
                    entity.DeleteMark = 1;//��ɾ��
                    this.BaseRepository().Update(entity);

                    //ɾ�����۱���ͬ����
                    var otherList = db.FindList<TelphoneLiangOtherEntity>(t => t.Telphone == entity.Telphone);
                    foreach (var item in otherList)
                    {
                        db.Delete(item);
                    }
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
                    TelphoneLiangEntity entity = this.BaseRepository().FindEntity(id);
                    entity.Modify(custIds[i]);
                    entity.SellMark = 0;//����״̬
                    this.BaseRepository().Update(entity);

                    //�ϼܴ��۱���ͬ����
                    var otherList = db.FindList<TelphoneLiangOtherEntity>(t => t.Telphone == entity.Telphone);
                    foreach (var item in otherList)
                    {
                        item.SellMark = 0;//����״̬
                        db.Update(item);
                    }
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
                    TelphoneLiangEntity entity = this.BaseRepository().FindEntity(id);
                    entity.Modify(custIds[i]);
                    entity.ExistMark = 1;//�ֿ�
                    this.BaseRepository().Update(entity);

                    //�ֿ����۱���ͬ����
                    var otherList = db.FindList<TelphoneLiangOtherEntity>(t => t.Telphone == entity.Telphone);
                    foreach (var item in otherList)
                    {
                        item.ExistMark = 1;//�ֿ�
                        db.Update(item);
                    }
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
                    TelphoneLiangEntity entity = this.BaseRepository().FindEntity(id);
                    entity.Modify(custIds[i]);
                    entity.ExistMark = 2;//��ɱ
                    this.BaseRepository().Update(entity);

                    //�ֿ����۱���ͬ����
                    var otherList = db.FindList<TelphoneLiangOtherEntity>(t => t.Telphone == entity.Telphone);
                    foreach (var item in otherList)
                    {
                        item.ExistMark = 2;//��ɱ
                        db.Update(item);
                    }
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
        public void SaveForm(int? keyValue, TelphoneLiangEntity entity)
        {
            if (!string.IsNullOrEmpty(keyValue.ToString()))
            {
                entity.Modify(keyValue);
                this.BaseRepository().Update(entity);
            }
            else
            {
                //�жϺ�������
                string greaterMsg = IsGreater(1);
                if (!string.IsNullOrEmpty(greaterMsg))
                {
                    throw new Exception(greaterMsg);
                }

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
        #endregion

        #region �жϺ�������
        /// <summary>
        /// ����������
        /// </summary>
        /// <param name="rowsCount"></param>
        /// <returns></returns>
        public string IsGreater(int rowsCount)
        {
            if (OperatorProvider.Provider.Current().IsSystem)
            {
                return "";
            }
            IRepository db = new RepositoryFactory().BaseRepository().BeginTrans();
            string companyid = OperatorProvider.Provider.Current().CompanyId;
            var vipEntity = db.FindEntity<TelphoneLiangVipEntity>(t => (t.OrganizeId == companyid || t.Description.Contains(companyid)) && t.VipEndDate > DateTime.Now);
            if (vipEntity==null)
            {
                return "δ��ͨ�����ܻ�����ѵ��ڣ�����ϵ����Ա��ͨ��";
            }
            if (string.IsNullOrEmpty(vipEntity.Id))
            {
                return "δ��ͨ�����ܻ�����ѵ��ڣ�����ϵ����Ա��ͨ��";
            }
            else
            {
                int uploadMax = Convert.ToInt32(vipEntity.UploadMax);//�������ϴ�����
                DataTable dt= GetLiangCountByOrg(companyid);
                int nowLiangCount = Convert.ToInt32(dt.Rows[0][0].ToString());
                if (nowLiangCount + rowsCount > uploadMax)
                {
                    return $"���ϴ�����{nowLiangCount}�������ϴ�����{rowsCount}�� �ѳ�������{uploadMax}������ϵ����Ա׷���ײͰ���";
                }
                else
                {
                    return "";
                }
            }
        }
        #endregion

        /// <summary>
        /// ������������
        /// </summary>
        /// <param name="dtSource">ʵ�����</param>
        /// <returns></returns>
        public string BatchAddEntity(DataTable dtSource)
        {
            int rowsCount = dtSource.Rows.Count;
            //�жϺ�������
            string greaterMsg = IsGreater(rowsCount);
            if (!string.IsNullOrEmpty(greaterMsg))
            {
                return greaterMsg;
            }

            IRepository db = new RepositoryFactory().BaseRepository().BeginTrans();

            int columns = dtSource.Columns.Count;
            string cf = "";
            for (int i = 0; i < rowsCount; i++)
            {
                try
                {
                    string telphone = dtSource.Rows[i][0].ToString();
                    if (telphone.Length == 11)
                    {
                        var liang_Data = db.FindEntity<TelphoneLiangEntity>(t => t.Telphone == telphone && t.DeleteMark!=1);//ɾ�����Ŀ����ٴε���
                        if (liang_Data != null)
                        {
                            cf+= telphone + ",";
                        }

                        //����ǰ7λȷ�����к���Ӫ��
                        string Number7 = telphone.Substring(0, 7);
                        string City = "",CityId = "", Operator = "", Brand = "";
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
                            return "���Ͳ����ڣ�" + itemName+",���������ֵ���ά�������͡�";
                        }

                        //�ײ�
                        string Package = dtSource.Rows[i][5].ToString();
                        //״̬
                        string existStr = dtSource.Rows[i][6].ToString();

                        if (string.IsNullOrEmpty(existStr))
                        {
                            return telphone + "�ֿ�/����/��ɱ״̬Ϊ��";
                        }
                        if (existStr!= "��ɱ" && existStr != "�ֿ�" && existStr != "Ԥ��")
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
                        if (columns==8)
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
                        TelphoneLiangEntity entity = new TelphoneLiangEntity()
                        {
                            Telphone = telphone,
                            Price = Price,
                            MaxPrice = MaxPrice,
                            MinPrice = MinPrice,
                            ChaPrice = ChaPrice,
                            CheckPrice= CheckPrice,
                            City = City,
                            CityId = CityId,
                            Operator = Operator,
                            Grade = itemNCode,
                            Package = Package,
                            Brand = Brand,
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
                    LogHelper.AddLog(ex.Message);
                    return ex.Message;
                }

            }
            db.Commit();
            if (cf!="")
            {
                LogHelper.AddLog("�����ظ�������룺" + cf);
                return "�����ظ�������룺" + cf;
            }
            else
            {
                return "����ɹ�";
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

                        //string companyid = OperatorProvider.Provider.Current().CompanyId;
                        var entity = db.FindEntity<TelphoneLiangEntity>(t => t.Telphone == telphone && t.SellMark == 0 && t.EnabledMark == 0 && t.DeleteMark == 0 );//&& t.OrganizeId == companyid
                        if (entity != null)
                        {
                            //var userEntity = db.FindEntity<UserEntity>(t => t.RealName == name);
                            //if (userEntity != null)
                            //{
                            //    entity.SellerName = userEntity.RealName;
                            //    entity.SellerId = userEntity.UserId;
                            //}
                            //else
                            //{
                            //    return name + " �����ڸ��û���";
                            //}
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
                        //ɾ�����۱���ͬ����
                        var otherList = db.FindList<TelphoneLiangOtherEntity>(t => t.Telphone == telphone);
                        foreach (var item in otherList)
                        {
                            db.Delete(item);
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
                        if (chaPrice<0)
                        {
                            return telphone + " �ɱ��۸����ۼۣ��۸�˳��ߵ���";
                        }

                        //�����
                        decimal checkPrice = 0;
                        if (telphonePrice.Length==4)
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


                        //string companyid = OperatorProvider.Provider.Current().CompanyId;
                        var entity = db.FindEntity<TelphoneLiangEntity>(t => t.Telphone == telphone && t.SellMark == 0 && t.EnabledMark == 0 && t.DeleteMark == 0 );//&& t.OrganizeId == companyid//һ���������Բ���
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
        /// ������ɾ����
        /// </summary>
        /// <param name="dtSource">ʵ�����</param>
        /// <returns></returns>
        public string BatchDeleteEntity(DataTable dtSource)
        {
            int rowsCount = dtSource.Rows.Count;
            IRepository db = new RepositoryFactory().BaseRepository().BeginTrans();

            int columns = dtSource.Columns.Count;
            for (int i = 0; i < rowsCount; i++)
            {
                try
                {
                    string telphone = dtSource.Rows[i][0].ToString();
                    if (telphone.Length == 11)
                    {
                        var liang_Data = db.FindEntity<TelphoneLiangEntity>(t => t.Telphone == telphone && t.DeleteMark != 1);//ɾ�����Ŀ����ٴε���
                        if (liang_Data != null)
                        {
                            db.Delete(liang_Data);
                        }
                    }

                }
                catch (Exception ex)
                {
                    LogHelper.AddLog(ex.Message);
                    return ex.Message;
                }

            }
            db.Commit();
            return "����ɾ����ɣ�";
        }
    }
}
