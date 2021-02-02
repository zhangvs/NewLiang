using HZSoft.Application.Entity.CustomerManage;
using HZSoft.Application.IService.CustomerManage;
using HZSoft.Application.Service.CustomerManage;
using HZSoft.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;

namespace HZSoft.Application.Busines.CustomerManage
{
    /// <summary>
    /// �� �� 6.1
    /// 
    /// �� ������������Ա
    /// �� �ڣ�2020-06-07 14:11
    /// �� ����ͷ�����ſ�
    /// </summary>
    public class TelphoneLiangH5BLL
    {
        private TelphoneLiangH5IService service = new TelphoneLiangH5Service();

        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        public IEnumerable<TelphoneLiangH5Entity> GetPageList(Pagination pagination, string queryJson)
        {
            return service.GetPageList(pagination, queryJson);
        }
        public IEnumerable<TelphoneLiangH5Entity> GetPageListH5LX(Pagination pagination, string queryJson)
        {
            return service.GetPageListH5LX(pagination, queryJson);
        }
        public IEnumerable<TelphoneLiangH5Entity> GetPageListH5LX_JS(Pagination pagination, string queryJson)
        {
            return service.GetPageListH5LX_JS(pagination, queryJson);
        }
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<TelphoneLiangH5Entity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public TelphoneLiangH5Entity GetEntity(int? keyValue)
        {
            return service.GetEntity(keyValue);
        }
        #endregion

        #region �ύ����
        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="keyValue">����</param>
        public void RemoveForm(string keyValues)
        {
            try
            {
                service.RemoveForm(keyValues);
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
        public void SaveForm(int? keyValue, TelphoneLiangH5Entity entity)
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


        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <returns></returns>
        public string BatchAddEntity(DataTable dtSource)
        {
            try
            {
                string returnMsg = service.BatchAddEntity(dtSource);
                return returnMsg;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }


        /// <summary>
        /// �ϼ�����
        /// </summary>
        /// <param name="keyValue">����</param>
        public void UpForm(string keyValues)
        {
            try
            {
                service.UpForm(keyValues);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// �ֿ�����
        /// </summary>
        /// <param name="keyValue">����</param>
        public void ExistForm(string keyValues)
        {
            try
            {
                service.ExistForm(keyValues);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// ��ɱ����
        /// </summary>
        /// <param name="keyValue">����</param>
        public void MiaoShaForm(string keyValues)
        {
            try
            {
                service.MiaoShaForm(keyValues);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// �����¼�
        /// </summary>
        /// <returns></returns>
        public string DownTelphone(string downTelphones)
        {
            try
            {
                return service.DownTelphone(downTelphones);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        public string PriceTelphone(string priceTelphones)
        {
            try
            {
                return service.PriceTelphone(priceTelphones);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// ����ɾ��
        /// </summary>
        /// <returns></returns>
        public string BatchDeleteEntity(DataTable dtSource)
        {
            try
            {
                string returnMsg = service.BatchDeleteEntity(dtSource);
                return returnMsg;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        #endregion
    }
}
