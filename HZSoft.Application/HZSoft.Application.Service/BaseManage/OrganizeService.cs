using HZSoft.Application.Code;
using HZSoft.Application.Entity.AuthorizeManage;
using HZSoft.Application.Entity.BaseManage;
using HZSoft.Application.Entity.CustomerManage;
using HZSoft.Application.Entity.WeChatManage;
using HZSoft.Application.IService.BaseManage;
using HZSoft.Data.Repository;
using HZSoft.Util;
using HZSoft.Util.Extension;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace HZSoft.Application.Service.BaseManage
{
    /// <summary>
    /// 版 本 6.1
    /// 
    /// 创建人：佘赐雄
    /// 日 期：2015.11.02 14:27
    /// 描 述：机构管理
    /// </summary>
    public class OrganizeService : RepositoryFactory<OrganizeEntity>, IOrganizeService
    {
        private Wechat_AgentIService agentService = new Wechat_AgentService();
        #region 获取数据
        /// <summary>
        /// 机构列表
        /// </summary>
        /// <returns></returns>
        public IEnumerable<OrganizeEntity> GetList()
        {
            //return this.BaseRepository().IQueryable().OrderBy(t => t.Category).ThenBy(t => t.CreateDate).ToList();

            string strSql = "select * from Base_Organize where DeleteMark <> 1 ";
            if (!OperatorProvider.Provider.Current().IsSystem && OperatorProvider.Provider.Current().UserId != "3303254b-7cd3-4a25-abd3-bb2542a08df9")//龙哥可以查看到所有号码
            {
                string companyId = OperatorProvider.Provider.Current().CompanyId;
                strSql += " and OrganizeId='" + companyId + "'";
            }
            return this.BaseRepository().FindList(strSql.ToString());
        }

        /// <summary>
        /// 当前机构的所有下级
        /// </summary>
        /// <returns></returns>
        //public IEnumerable<OrganizeEntity> GetListByIds()
        //{
        //    string strSql = "SELECT OrganizeId,Category,ShortName,FullName,OuterPhone,Manager,ParentId,AgreementMark,DeleteMark,CreateDate  FROM Base_Organize  where DeleteMark <> 1 ";
        //    string companyId = OperatorProvider.Provider.Current().CompanyId;
        //    if (!OperatorProvider.Provider.Current().IsSystem && companyId!="a5a962da-57e1-4ad4-87b2-bbdcd1b7cc92")
        //    {
        //        //strSql = @"WITH T
        //        //AS(
        //        //    SELECT OrganizeId,Category,ShortName,FullName,OuterPhone,Manager,ParentId,AgreementMark,DeleteMark,CreateDate  FROM Base_Organize WHERE OrganizeId = '" + OperatorProvider.Provider.Current().CompanyId + "'"+
        //        //    @"UNION ALL
        //        //    SELECT a.OrganizeId,a.Category,a.ShortName,a.FullName,a.OuterPhone,a.Manager,a.ParentId,a.AgreementMark,a.DeleteMark,a.CreateDate  FROM Base_Organize a INNER JOIN T ON a.ParentId = T.OrganizeId
        //        //)
        //        //SELECT OrganizeId,Category,ShortName,FullName,OuterPhone,Manager,ParentId,AgreementMark,DeleteMark,CreateDate  FROM T where DeleteMark <> 1 ";
        //        strSql = $"SELECT OrganizeId,Category,ShortName,FullName,OuterPhone,Manager,ParentId,AgreementMark,DeleteMark,CreateDate  FROM Base_Organize WHERE OrganizeId = '{companyId }' or ParentId='{companyId }' ";
        //    }
        //    strSql += " ORDER BY category,CreateDate ";
        //    return this.BaseRepository().FindList(strSql.ToString());
        //}

        /// <summary>
        /// 获取上级机构所有二级数量
        /// </summary>
        /// <returns></returns>
        public DataTable GetOrgCount(string parentId)
        {
             string strSql = @"WITH T
AS(
    SELECT OrganizeId,ParentId,DeleteMark FROM Base_Organize WHERE OrganizeId = '" + parentId + "'" +
    @"UNION ALL
    SELECT a.OrganizeId,a.ParentId,a.DeleteMark FROM Base_Organize a INNER JOIN T ON a.ParentId = T.OrganizeId
)
SELECT count(1) FROM T where DeleteMark <> 1 ";
            return this.BaseRepository().FindTable(strSql.ToString());
        }
        

        /// <summary>
        /// 机构现售号码数量
        /// </summary>
        /// <param name="organizeId">机构</param>
        /// <returns>返回分页列表</returns>
        public int GetLiangCountByOrg(string organizeId)
        {
            if (string.IsNullOrEmpty(organizeId))
            {
                return 0;
            }
            else
            {
                string strSql = $"select count(1) from Base_Organize where OrganizeId='{organizeId}'";
                return this.BaseRepository().ExecuteBySql(strSql.ToString());
            }

        }


        /// <summary>
        /// 手机端根据传递的org，获取顶级机构id
        /// </summary>
        /// <returns></returns>
        public IEnumerable<OrganizeEntity> GetParentIdByOrgId(string orgid)
        {
            string strSql = @"WITH T
AS(
    SELECT OrganizeId,Img1,Img2,Img3,Img4,ParentId,DeleteMark FROM Base_Organize WHERE OrganizeId = '" + orgid + "'" +
    @"UNION ALL
    SELECT a.OrganizeId,a.Img1,a.Img2,a.Img3,a.Img4,a.ParentId,a.DeleteMark FROM Base_Organize a INNER JOIN T ON a.OrganizeId = T.ParentId
)
SELECT OrganizeId,Img1,Img2,Img3,Img4 FROM T where DeleteMark <> 1 and ParentId='0'";
            return this.BaseRepository().FindList(strSql.ToString());
        }

        /// <summary>
        /// 机构实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public OrganizeEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        //public OrganizeEntity GetEntityByTelAndName(string tel,string name)
        //{
        //    var expression = LinqExtensions.True<OrganizeEntity>();
        //    expression = expression.And(t => t.OuterPhone == tel);
        //    expression = expression.Or(t => t.FullName == name);
        //    return this.BaseRepository().FindEntity(expression);
        //}
        #endregion

        #region 验证数据
        /// <summary>
        /// 公司名称不能重复
        /// </summary>
        /// <param name="fullName">公司名称</param>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        public bool ExistFullName(string fullName, string keyValue)
        {
            var expression = LinqExtensions.True<OrganizeEntity>();
            expression = expression.And(t => t.FullName == fullName);
            if (!string.IsNullOrEmpty(keyValue))
            {
                expression = expression.And(t => t.OrganizeId != keyValue);
            }
            return this.BaseRepository().IQueryable(expression).Count() == 0 ? true : false;
        }
        /// <summary>
        /// 外文名称不能重复
        /// </summary>
        /// <param name="enCode">外文名称</param>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        public bool ExistEnCode(string enCode, string keyValue)
        {
            var expression = LinqExtensions.True<OrganizeEntity>();
            expression = expression.And(t => t.EnCode == enCode);
            if (!string.IsNullOrEmpty(keyValue))
            {
                expression = expression.And(t => t.OrganizeId != keyValue);
            }
            return this.BaseRepository().IQueryable(expression).Count() == 0 ? true : false;
        }
        /// <summary>
        /// 中文名称不能重复
        /// </summary>
        /// <param name="shortName">中文名称</param>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        public bool ExistShortName(string shortName, string keyValue)
        {
            var expression = LinqExtensions.True<OrganizeEntity>();
            expression = expression.And(t => t.ShortName == shortName);
            if (!string.IsNullOrEmpty(keyValue))
            {
                expression = expression.And(t => t.OrganizeId != keyValue);
            }
            return this.BaseRepository().IQueryable(expression).Count() == 0 ? true : false;
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除机构
        /// </summary>
        /// <param name="keyValue">主键</param>
        public void RemoveForm(string keyValue)
        {
            int count = this.BaseRepository().IQueryable(t => t.ParentId == keyValue).Count();
            if (count > 0)
            {
                throw new Exception("当前所选数据有子节点数据！");
            }

            IRepository db = new RepositoryFactory().BaseRepository().BeginTrans();
            var orgEntity = this.BaseRepository().FindEntity(keyValue);
            if (orgEntity==null)
            {
                throw new Exception("当前所选数据为空机构！");
            }
            //先删除部门
            var departmentList = db.FindList<DepartmentEntity>(t => t.OrganizeId == keyValue);
            foreach (var item in departmentList)
            {
                db.Delete(item);
            }
            //删除角色
            var roleList = db.FindList<RoleEntity>(t => t.OrganizeId == keyValue);
            foreach (var item in roleList)
            {
                //删除角色菜单权限
                var authorizeList = db.FindList<AuthorizeEntity>(t => t.ObjectId == item.RoleId);
                foreach (var item1 in authorizeList)
                {
                    db.Delete(item1);
                }
                //删除角色数据权限
                var authorizeDataList = db.FindList<AuthorizeDataEntity>(t => t.ObjectId == item.RoleId);
                foreach (var item2 in authorizeDataList)
                {
                    db.Delete(item2);
                }

                db.Delete(item);
            }
            

            //删除用户
            var userList = db.FindList<UserEntity>(t => t.OrganizeId == keyValue);
            foreach (var item in userList)
            {
                db.Delete(item);
            }

            //删除该机构下级代理的加盟记录
            var joinList = db.FindList<TelphoneLiangJoinEntity>(t => t.OrganizeId == keyValue);
            foreach (var item in joinList)
            {
                db.Delete(item);
            }
            //删除自己的加盟记录
            var joinList0 = db.FindList<TelphoneLiangJoinEntity>(t => t.Telphone == orgEntity.OuterPhone);
            foreach (var item in joinList0)
            {
                db.Delete(item);
            }
            ////删除浏览记录
            //var seeList = db.FindList<TelphoneLiangSeeEntity>(t => t.OrganizeId == keyValue);
            //foreach (var item in joinList)
            //{
            //    db.Delete(item);
            //}
            ////删除分享记录
            //var shareList = db.FindList<TelphoneLiangShareEntity>(t => t.OrganizeId == keyValue);
            //foreach (var item in joinList)
            //{
            //    db.Delete(item);
            //}

            this.BaseRepository().Delete(keyValue);
            db.Commit();
        }

        /// <summary>
        /// 新增下级代理
        /// </summary>
        /// <param name="organizeEntity"></param>
        public void SaveNewAgent(OrganizeEntity organizeEntity)
        {
            IRepository db = new RepositoryFactory().BaseRepository().BeginTrans();
            try
            {
                #region 新增机构
                //父机构
                if(organizeEntity.ParentId == null)
                {
                    organizeEntity.ParentId = "0";
                    //throw new Exception("上级机构不能为空！");
                }

                if (organizeEntity.ParentId != "0")
                {
                    var parentEntity = this.BaseRepository().FindEntity(organizeEntity.ParentId);
                    organizeEntity.Category = parentEntity.Category + 1;

                    //顶级机构
                    IEnumerable<OrganizeEntity> topList = GetParentIdByOrgId(organizeEntity.ParentId);
                    if (topList.Count() > 0 && string.IsNullOrEmpty(organizeEntity.Img1))
                    {
                        OrganizeEntity topEntity = topList.First();
                        organizeEntity.TopOrganizeId = topEntity.OrganizeId;//顶级机构
                    }
                }
                else
                {
                    organizeEntity.Category = 0;
                }
                organizeEntity.Create();
                #endregion

                #region 新增默认管理部门
                DepartmentEntity department = new DepartmentEntity();
                department.OrganizeId = organizeEntity.OrganizeId;
                department.ParentId = "0";
                department.EnCode = organizeEntity.OuterPhone;//账号
                department.FullName = organizeEntity.FullName;
                department.Create();
                db.Insert(department); 
                #endregion

                #region 新增默认靓号角色
                RoleEntity role = new RoleEntity();
                role.OrganizeId = organizeEntity.OrganizeId;
                role.Category = 1;//分类1 - 角色2 - 岗位3 - 职位4 - 工作组
                role.EnCode = organizeEntity.OuterPhone;//账号
                role.FullName = organizeEntity.FullName;
                role.Create();
                db.Insert(role);
                #endregion

                #region 授权功能 
                var AuthorizeList = db.FindList<AuthorizeEntity>(t => t.ObjectId == "6581e298-d4b4-4347-96da-030d82cd247b");
                foreach (AuthorizeEntity item in AuthorizeList)
                {
                    AuthorizeEntity authorizeEntity = new AuthorizeEntity();
                    authorizeEntity.Create();
                    authorizeEntity.Category = 2;  //1 - 部门2 - 角色3 - 岗位4 - 职位5 - 工作组
                    authorizeEntity.ObjectId = role.RoleId;//角色id，角色限定了机构和部门
                    authorizeEntity.ItemType = item.ItemType; //项目类型: 1 - 菜单2 - 按钮3 - 视图4表单
                    authorizeEntity.ItemId = item.ItemId;//项目主键
                    authorizeEntity.SortCode = item.SortCode;
                    db.Insert(authorizeEntity);
                }
                #endregion

                #region 数据权限 就一个
                AuthorizeDataEntity authorizeDataEntity = new AuthorizeDataEntity();
                authorizeDataEntity.Create();
                authorizeDataEntity.AuthorizeType = 4; //授权类型: 1 - 仅限本人2 - 仅限本人及下属3 - 所在部门4 - 所在公司5 - 按明细设置
                authorizeDataEntity.Category = 2; //对象分类: 1 - 部门2 - 角色3 - 岗位4 - 职位5 - 工作组
                authorizeDataEntity.ObjectId = role.RoleId;//角色id，角色限定了机构和部门
                authorizeDataEntity.IsRead = 0;
                authorizeDataEntity.SortCode = 1;
                db.Insert(authorizeDataEntity);
                #endregion
                
                #region 新增默认用户
                UserEntity userEntity = new UserEntity();
                userEntity.Create();
                role.EnCode = organizeEntity.OuterPhone;//账号
                userEntity.Account = organizeEntity.OuterPhone;//登录名为机构名拼音首字母organizeEntity.EnCode
                userEntity.RealName = organizeEntity.Manager;//organizeEntity.FullName
                userEntity.WeChat = organizeEntity.ShortName;//微信昵称
                userEntity.OrganizeId = organizeEntity.OrganizeId;
                userEntity.DepartmentId = department.DepartmentId;
                userEntity.RoleId = role.RoleId;
                userEntity.Gender = 1;
                userEntity.Secretkey = Md5Helper.MD5(CommonHelper.CreateNo(), 16).ToLower();
                userEntity.Password = Md5Helper.MD5(DESEncrypt.Encrypt(Md5Helper.MD5("0000", 32).ToLower(), userEntity.Secretkey).ToLower(), 32).ToLower();
                db.Insert(userEntity);
                #endregion
                
                #region 新增默认用户关系
                UserRelationEntity userRelationEntity = new UserRelationEntity();
                userRelationEntity.Create();
                userRelationEntity.Category = 2;//登录名为机构名拼音首字母
                userRelationEntity.UserId = userEntity.UserId;
                userRelationEntity.ObjectId = userEntity.RoleId;
                db.Insert(userRelationEntity);
                #endregion

                #region 新增代理表
                var agentEntity = agentService.GetEntityByOpenId(organizeEntity.ManagerId);
                if (agentEntity == null)
                {
                    var weChat_Users = db.FindEntity<WeChat_UsersEntity>(t => t.OpenId == organizeEntity.ManagerId);
                    if (weChat_Users != null)
                    {
                        agentEntity = new Wechat_AgentEntity()
                        {
                            OpenId = weChat_Users.OpenId,
                            nickname = weChat_Users.NickName,
                            Sex = weChat_Users.Sex,
                            HeadimgUrl = weChat_Users.HeadimgUrl,
                            Province = weChat_Users.Province,
                            City = weChat_Users.City,
                            Country = weChat_Users.Country,
                            LV = "普通代理",
                            Category = 0,
                            OrganizeId = organizeEntity.OrganizeId,//绑定机构id
                        };
                        agentEntity.Create();//create得不到id，自增
                        agentService.SaveForm(null, agentEntity);//提交数据库之后才能拿到id，复制给机构表中的agentid
                        //更新id，0级机构上级和顶级都是本机构
                        agentEntity.Pid = agentEntity.Id;
                        agentEntity.Tid = agentEntity.Id;
                        agentEntity.Category = 0;//改为0级代理
                        agentService.SaveForm(agentEntity.Id, agentEntity);
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(agentEntity.OrganizeId))
                    {
                        agentEntity.OrganizeId = organizeEntity.OrganizeId;//如果不存在代理机构id，
                        //更新id，0级机构上级和顶级都是本机构
                        agentEntity.Pid = agentEntity.Id;
                        agentEntity.Tid = agentEntity.Id;
                        agentEntity.Category = 0;//改为0级代理
                        db.Update(agentEntity);
                    }
                }
                organizeEntity.AgentId = agentEntity.Id;//绑定代理id
                #endregion

                db.Insert(organizeEntity);
                db.Commit();
            }
            catch (Exception)
            {
                db.Rollback();
                throw;
            }
        }

        /// <summary>
        /// 保存机构表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="organizeEntity">机构实体</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, OrganizeEntity organizeEntity)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                IRepository db = new RepositoryFactory().BaseRepository().BeginTrans();
                try
                {
                    var oldEntity = GetEntity(organizeEntity.OrganizeId);
                    if (oldEntity != null)
                    {
                        if (organizeEntity.OuterPhone != oldEntity.OuterPhone)
                        {
                            //如果电话号码修改，同步修改部门表，用户表
                            var depEntity = db.FindEntity<DepartmentEntity>(t => t.OrganizeId == organizeEntity.OrganizeId && t.EnCode == oldEntity.OuterPhone);
                            if (depEntity != null)
                            {
                                depEntity.EnCode = organizeEntity.OuterPhone;
                                db.Update(depEntity);
                            }
                            //用户表
                            var userEntity = db.FindEntity<UserEntity>(t => t.OrganizeId == organizeEntity.OrganizeId && t.EnCode == oldEntity.OuterPhone);
                            if (userEntity != null)
                            {
                                userEntity.Account = organizeEntity.OuterPhone;//登录账号
                                userEntity.EnCode = organizeEntity.OuterPhone;
                                db.Update(userEntity);
                            }
                        }
                        db.Commit();
                    }
                    organizeEntity.Modify(keyValue);
                    this.BaseRepository().Update(organizeEntity);
                }
                catch (Exception)
                {
                    db.Rollback();
                    throw;
                }
            }
            else
            {
                //新增下级代理权限等
                SaveNewAgent(organizeEntity);

                ////微信提醒模板
                //if (!string.IsNullOrEmpty(organizeEntity.ManagerId))
                //{
                //    WechatHelper.SendToOpenOK(organizeEntity.ManagerId, organizeEntity.FullName, organizeEntity.Description);
                //}
            }
        }
        /// <summary>
        /// 审核加盟申请
        /// </summary>
        /// <param name="organizeEntity">返回新通过的机构信息</param>
        /// <returns></returns>
        public OrganizeEntity SaveReturnEntity(OrganizeEntity organizeEntity)
        {
            //新增下级代理
            SaveNewAgent(organizeEntity);
            return organizeEntity;
        }
        /// <summary>
        /// 修改机构状态
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="State">状态：1-启动；0-禁用</param>
        public void UpdateState(string keyValue, int State)
        {
            OrganizeEntity organizeEntity = new OrganizeEntity();
            organizeEntity.Modify(keyValue);
            organizeEntity.EnabledMark = State;
            this.BaseRepository().Update(organizeEntity);
        }
        /// <summary>
        /// 修改机构协议状态
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="State">状态：1-启动；0-禁用</param>
        public void UpdateAgreementState(string keyValue, int State)
        {
            OrganizeEntity organizeEntity = new OrganizeEntity();
            organizeEntity.AgreementModify(keyValue);
            organizeEntity.AgreementMark = State;
            this.BaseRepository().Update(organizeEntity);
        }


        /// <summary>
        /// 分享+1
        /// </summary>
        /// <param name="keyValue">主键值</param>
        public void UpdateSeeCount(string keyValue)
        {
            OrganizeEntity organizeEntity = GetEntity(keyValue);
            organizeEntity.SeeCount +=1;
            organizeEntity.LastSeeDate = DateTime.Now;
            this.BaseRepository().Update(organizeEntity);
        }
        /// <summary>
        /// 浏览+1
        /// </summary>
        /// <param name="keyValue">主键值</param>
        public void UpdateShareCount(string keyValue)
        {
            OrganizeEntity organizeEntity = GetEntity(keyValue);
            organizeEntity.ShareCount +=1;
            organizeEntity.LastShareDate = DateTime.Now;
            this.BaseRepository().Update(organizeEntity);
        }
        /// <summary>
        /// 加盟+1
        /// </summary>
        /// <param name="keyValue">主键值</param>
        public void UpdateJoinCount(string keyValue)
        {
            OrganizeEntity organizeEntity = GetEntity(keyValue);
            organizeEntity.OrganizeId = keyValue;
            organizeEntity.JoinCount += 1;
            organizeEntity.LastJoinDate = DateTime.Now;
            this.BaseRepository().Update(organizeEntity);
        }


        /// <summary>
        /// 启用管理员账号
        /// </summary>
        /// <param name="keyValue">主键值</param>
        public void AddAdmin(string keyValue)
        {

            IRepository db = new RepositoryFactory().BaseRepository().BeginTrans();
            //先找到当前机构的角色id
            var roleEnity = db.FindEntity<RoleEntity>(t => t.OrganizeId == keyValue);
            if (roleEnity!=null)
            {
                //再给当前角色添加菜单权限
                var authorize = db.FindEntity<AuthorizeEntity>(t => t.ObjectId == roleEnity.RoleId && t.ItemId == "26cba400-c984-4f6b-b025-76d4dbf54184");
                if (authorize == null)
                {
                    AuthorizeEntity authorizeEntity2 = new AuthorizeEntity();
                    authorizeEntity2.Create();
                    authorizeEntity2.Category = 2;  //1 - 部门2 - 角色3 - 岗位4 - 职位5 - 工作组
                    authorizeEntity2.ObjectId = roleEnity.RoleId;//角色id，角色限定了机构和部门
                    authorizeEntity2.ItemType = 1; //项目类型: 1 - 菜单2 - 按钮3 - 视图4表单
                    authorizeEntity2.ItemId = "26cba400-c984-4f6b-b025-76d4dbf54184";//项目主键
                    authorizeEntity2.SortCode = 8;
                    db.Insert(authorizeEntity2);

                    db.Commit();
                }
            }
            //修改平台管理员标识
            OrganizeEntity organizeEntity = GetEntity(keyValue);
            organizeEntity.PTAdminMark = 1;
            this.BaseRepository().Update(organizeEntity);

        }


        #endregion
    }
}
