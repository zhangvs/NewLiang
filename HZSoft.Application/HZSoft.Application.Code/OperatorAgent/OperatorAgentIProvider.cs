﻿
namespace HZSoft.Application.Code
{
    /// <summary>
    /// 版 本 6.1
    /// 
    /// 创建人：佘赐雄
    /// 日 期：2015.10.10
    /// 描 述：当前操作者回话接口
    /// </summary>
    public interface OperatorAgentIProvider
    {
        /// <summary>
        /// 写入登录信息
        /// </summary>
        /// <param name="user">成员信息</param>
        void AddCurrent(OperatorAgent user);
        /// <summary>
        /// 获取当前用户
        /// </summary>
        /// <returns></returns>
        OperatorAgent Current();
        /// <summary>
        /// 删除当前用户
        /// </summary>
        void EmptyCurrent();
        /// <summary>
        /// 是否过期
        /// </summary>
        /// <returns></returns>
        bool IsOverdue();
        /// <summary>
        /// 是否已登录
        /// </summary>
        /// <returns></returns>
        int IsOnLine();
    }
}