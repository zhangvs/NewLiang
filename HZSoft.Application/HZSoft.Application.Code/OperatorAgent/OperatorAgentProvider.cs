using HZSoft.Cache.Factory;
using HZSoft.Util;
using System;

namespace HZSoft.Application.Code
{
    /// <summary>
    /// 版 本 6.1
    /// 
    /// 创建人：佘赐雄
    /// 日 期：2015.10.10
    /// 描 述：当前操作者回话
    /// </summary>
    public class OperatorAgentProvider : OperatorAgentIProvider
    {
        #region 静态实例
        /// <summary>
        /// 当前提供者
        /// </summary>
        public static OperatorAgentIProvider Provider
        {
            get { return new OperatorAgentProvider(); }
        }
        /// <summary>
        /// 给app调用
        /// </summary>
        public static string AppId
        {
            set;
            get;
        }
        #endregion

        /// <summary>
        /// 秘钥
        /// </summary>
        private string LoginUserKey = "HZSoft_LoginUserKey_2016_V6.1";
        /// <summary>
        /// 登陆提供者模式:Session、Cookie 
        /// </summary>
        private string LoginProvider = Config.GetValue("LoginProvider");
        /// <summary>
        /// 写入登录信息
        /// </summary>
        /// <param name="user">成员信息</param>
        public virtual void AddCurrent(OperatorAgent user)
        {
            try
            {
                if (LoginProvider == "Cookie")
                {
                    WebHelper.WriteCookie(LoginUserKey, DESEncrypt.Encrypt(user.ToJson()));
                }
                else
                {
                    WebHelper.WriteSession(LoginUserKey, DESEncrypt.Encrypt(user.ToJson()));
                }
                CacheFactory.Cache().WriteCache(user.Token, user.Id.ToString(), user.LogTime.AddHours(1));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// 当前用户
        /// </summary>
        /// <returns></returns>
        public virtual OperatorAgent Current()
        {
            try
            {
                OperatorAgent user = new OperatorAgent();
                if (LoginProvider == "Cookie")
                {
                    user = DESEncrypt.Decrypt(WebHelper.GetCookie(LoginUserKey).ToString()).ToObject<OperatorAgent>();
                }
                else if (LoginProvider == "AppClient")
                {
                    user = CacheFactory.Cache().GetCache<OperatorAgent>(AppId);
                }
                else
                {
                    user = DESEncrypt.Decrypt(WebHelper.GetSession(LoginUserKey).ToString()).ToObject<OperatorAgent>();
                }
                return user;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// 删除登录信息
        /// </summary>
        public virtual void EmptyCurrent()
        {
            if (LoginProvider == "Cookie")
            {
                WebHelper.RemoveCookie(LoginUserKey.Trim());
                #region 解决cookie时，设置数据权限较多时无法登陆的bug modify by chengzg 2016.8.19 13:15
                CacheFactory.Cache().RemoveCache(LoginUserKey);
                #endregion
            }
            else
            {
                WebHelper.RemoveSession(LoginUserKey.Trim());
            }
        }
        /// <summary>
        /// 是否过期
        /// </summary>
        /// <returns></returns>
        public virtual bool IsOverdue()
        {
            try
            {
                object str = "";
                AuthorizeDataModel dataAuthorize = null;
                if (LoginProvider == "Cookie")
                {
                    str = WebHelper.GetCookie(LoginUserKey);
                    #region 解决cookie时，设置数据权限较多时无法登陆的bug modify by chengzg 2016.8.19 13:15
                    dataAuthorize = CacheFactory.Cache().GetCache<AuthorizeDataModel>(LoginUserKey);

                    if (dataAuthorize == null)
                    {
                        return true;
                    }
                    #endregion
                }
                else
                {
                    str = WebHelper.GetSession(LoginUserKey);
                }
                if (str != null && str.ToString() != "")
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception)
            {
                return true;
            }
        }
        /// <summary>
        /// 是否已登录
        /// </summary>
        /// <returns></returns>
        public virtual int IsOnLine()
        {
            OperatorAgent user = new OperatorAgent();
            if (LoginProvider == "Cookie")
            {
                user = DESEncrypt.Decrypt(WebHelper.GetCookie(LoginUserKey).ToString()).ToObject<OperatorAgent>();
            }
            else
            {
                user = DESEncrypt.Decrypt(WebHelper.GetSession(LoginUserKey).ToString()).ToObject<OperatorAgent>();
            }
            object token = CacheFactory.Cache().GetCache<string>(user.Id.ToString());
            if (token == null)
            {
                return -1;//过期
            }
            if (user.Token == token.ToString())
            {
                return 1;//正常
            }
            else
            {
                return 0;//已登录
            }
        }
    }
}
