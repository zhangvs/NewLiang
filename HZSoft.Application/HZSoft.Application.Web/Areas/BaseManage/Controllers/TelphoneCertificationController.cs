using HZSoft.Application.Entity.BaseManage;
using HZSoft.Application.Busines.BaseManage;
using HZSoft.Util;
using HZSoft.Util.WebControl;
using System.Web.Mvc;
using System.IO;
using System.Web;
using Ionic.Zip;
using System;
using System.Net;

namespace HZSoft.Application.Web.Areas.BaseManage.Controllers
{
    /// <summary>
    /// �� �� 6.1
    /// 
    /// �� ������������Ա
    /// �� �ڣ�2017-10-12 17:30
    /// �� ����TelphoneCertification
    /// </summary>
    public class TelphoneCertificationController : MvcControllerBase
    {
        private TelphoneCertificationBLL telphonecertificationbll = new TelphoneCertificationBLL();

        #region ��ͼ����
        /// <summary>
        /// �б�ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult TelphoneCertificationIndex()
        {
            return View();
        }
        /// <summary>
        /// ��ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult TelphoneCertificationForm()
        {
            return View();
        }
        #endregion

        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ����</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�Json</returns>
        [HttpGet]
        public ActionResult GetPageListJson(Pagination pagination, string queryJson)
        {
            var watch = CommonHelper.TimerStart();
            var data = telphonecertificationbll.GetPageList(pagination, queryJson);
            var jsonData = new
            {
                rows = data,
                total = pagination.total,
                page = pagination.page,
                records = pagination.records,
                costtime = CommonHelper.TimerEnd(watch)
            };
            return ToJsonResult(jsonData);
        }
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�Json</returns>
        [HttpGet]
        public ActionResult GetListJson(string queryJson)
        {
            var data = telphonecertificationbll.GetList(queryJson);
            return ToJsonResult(data);
        }
        /// <summary>
        /// ��ȡʵ�� 
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns>���ض���Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = telphonecertificationbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }
        #endregion

        #region �ύ����
        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult RemoveForm(string keyValue)
        {
            telphonecertificationbll.RemoveForm(keyValue);
            return Success("ɾ���ɹ���");
        }
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveForm(string keyValue, TelphoneCertificationEntity entity)
        {
            telphonecertificationbll.SaveForm(keyValue, entity);
            return Success("�����ɹ���");
        }
        #endregion

        /// <summary>
        /// ����ͼƬ
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public ActionResult Export(string keyValue)
        {
            var data = telphonecertificationbll.GetEntity(keyValue);
            if (data!=null)
            {
                using (ZipFile zip = new ZipFile(System.Text.Encoding.Default))
                {
                    zip.AddFile(Server.MapPath(data.photo_z), "");
                    zip.AddFile(Server.MapPath(data.photo_b), "");
                    zip.AddFile(Server.MapPath(data.photo_s), "");

                    Stream ms = new MemoryStream();
                    zip.Save(ms);
                    ms.Seek(0, SeekOrigin.Begin);//��ms�����ã��ú�����ÿ��Ի�ȡ����ȷ������������ܻ��ȡ�����ļ�
                    return File(ms, "application/zip", data.mobileNumber + data.custName + ".zip");

                    //zip.Save(Server.MapPath("~/Resource/ZIP/" + data.mobileNumber + data.custName + ".zip"));
                    //return File(Server.MapPath("~/Resource/ZIP/" + data.mobileNumber + data.custName + ".zip"),
                    //              "application/zip");
                }
            }
            else
            {
                return Success("δѡ��");
            }

        }
        


    }
}
