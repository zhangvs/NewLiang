using HZSoft.Application.Entity.CustomerManage;
using HZSoft.Application.IService.CustomerManage;
using HZSoft.Application.Service.CustomerManage;
using HZSoft.Util.WebControl;
using System.Collections.Generic;
using System;

namespace HZSoft.Application.Busines.CustomerManage
{
    /// <summary>
    /// �� �� 6.1
    /// 
    /// �� ������������Ա
    /// �� �ڣ�2020-04-16 09:07
    /// �� ����������
    /// </summary>
    public class OrdersBLL
    {
        private OrdersIService service = new OrdersService();

        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        public IEnumerable<OrdersEntity> GetPageList(Pagination pagination, string queryJson)
        {
            return service.GetPageList(pagination, queryJson);
        }
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<OrdersEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public OrdersEntity GetEntity(int? keyValue)
        {
            return service.GetEntity(keyValue);
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public OrdersEntity GetEntityByOrderSn(string OrderSn)
        {
            return service.GetEntityByOrderSn(OrderSn);
        }

        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        public OrdersEntity GetEntityByTel(string tel)
        {
            return service.GetEntityByTel(tel);
        }
        #endregion

        #region �ύ����
        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="keyValue">����</param>
        public void RemoveForm(int? keyValue)
        {
            try
            {
                service.RemoveForm(keyValue);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        public void SaveForm(int? keyValue, OrdersEntity entity)
        {
            try
            {
                service.SaveForm(keyValue, entity);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public OrdersEntity SaveForm(OrdersEntity entity)
        {
            try
            {
                return service.SaveForm(entity);
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        public void SaveSendForm(int? keyValue, OrdersEntity entity)
        {
            try
            {
                service.SaveSendForm(keyValue, entity);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        public void UpdateSendState(int? keyValue)
        {
            try
            {
                service.UpdateSendState(keyValue);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
