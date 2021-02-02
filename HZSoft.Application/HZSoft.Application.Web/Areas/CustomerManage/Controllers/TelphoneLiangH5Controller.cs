using HZSoft.Application.Entity.CustomerManage;
using HZSoft.Application.Busines.CustomerManage;
using HZSoft.Util;
using HZSoft.Util.WebControl;
using System.Web.Mvc;
using System.Web;
using System;
using System.IO;
using System.Data;
using HZSoft.Util.Offices;

namespace HZSoft.Application.Web.Areas.CustomerManage.Controllers
{
    /// <summary>
    /// �� �� 6.1
    /// 
    /// �� ������������Ա
    /// �� �ڣ�2020-06-07 14:11
    /// �� ����ͷ�����ſ�
    /// </summary>
    public class TelphoneLiangH5Controller : MvcControllerBase
    {
        private TelphoneLiangH5BLL telphoneliangh5bll = new TelphoneLiangH5BLL();

        #region ��ͼ����
        /// <summary>
        /// �б�ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult TelphoneLiangH5Index()
        {
            return View();
        }
        /// <summary>
        /// ��ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult TelphoneLiangH5Form()
        {
            return View();
        }

        /// <summary>
        /// ������������
        /// </summary>
        /// <returns></returns>
        public ActionResult TelphoneLiangH5Import()
        {
            return View();
        }
        /// <summary>
        /// ������������
        /// </summary>
        /// <returns></returns>
        public ActionResult TelphoneLiangH5Delete()
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
            var data = telphoneliangh5bll.GetPageList(pagination, queryJson);
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
            var data = telphoneliangh5bll.GetList(queryJson);
            return ToJsonResult(data);
        }
        /// <summary>
        /// ��ȡʵ�� 
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns>���ض���Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(int? keyValue)
        {
            var data = telphoneliangh5bll.GetEntity(keyValue);
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
        public ActionResult RemoveForm(string keyValues)
        {
            telphoneliangh5bll.RemoveForm(keyValues);
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
        public ActionResult SaveForm(int? keyValue, TelphoneLiangH5Entity entity)
        {
            telphoneliangh5bll.SaveForm(keyValue, entity);
            return Success("�����ɹ���");
        }
        #endregion

        #region ������������
        public FileResult GetFile()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "Resource/ExcelTemplate/";
            string fileName = "������������ģ��.xlsx";
            return File(path + fileName, "application/ms-excel", fileName);
        }

        [HttpPost]
        public ActionResult TelphoneLiangH5Import(HttpPostedFileBase filebase)
        {
            HttpPostedFileBase file = Request.Files["files"];
            string FileName;
            string savePath;
            if (file == null || file.ContentLength <= 0)
            {
                ViewBag.error = "�ļ�����Ϊ��";
                return View();
            }
            else
            {
                string filename = Path.GetFileName(file.FileName);
                int filesize = file.ContentLength;//��ȡ�ϴ��ļ��Ĵ�С��λΪ�ֽ�byte
                string fileEx = System.IO.Path.GetExtension(filename);//��ȡ�ϴ��ļ�����չ��
                string NoFileName = System.IO.Path.GetFileNameWithoutExtension(filename);//��ȡ����չ�����ļ���
                int Maxsize = 4000 * 1024;//�����ϴ��ļ������ռ��СΪ4M
                string FileType = ".xls,.xlsx";//�����ϴ��ļ��������ַ���

                FileName = NoFileName + DateTime.Now.ToString("yyyyMMddhhmmss") + fileEx;
                if (!FileType.Contains(fileEx))
                {
                    ViewBag.error = "�ļ����Ͳ��ԣ�ֻ�ܵ���xls��xlsx��ʽ���ļ�";
                    return View();
                }
                if (filesize >= Maxsize)
                {
                    ViewBag.error = "�ϴ��ļ�����4M�������ϴ�";
                    return View();
                }
                string path = AppDomain.CurrentDomain.BaseDirectory + "Resource/ExcelData/";
                savePath = Path.Combine(path, FileName);
                file.SaveAs(savePath);
            }

            //excelתDataTable
            DataTable dtSource = ExcelHelper.ExcelImport(savePath);

            //��������TelphoneWash��
            //SqlBulkCopyByDatatable("TelphoneWash", dtSource);

            //һ���в���
            //����������ƣ�����ʱ������ع�
            ViewBag.error = telphoneliangh5bll.BatchAddEntity(dtSource);

            System.Threading.Thread.Sleep(2000);
            return View();
        }
        #endregion




        /// <summary>
        /// �����ϼ�
        /// </summary>
        /// <returns></returns>
        public ActionResult UpForm(string keyValues)
        {
            telphoneliangh5bll.UpForm(keyValues);
            return Success("�ϼܳɹ���");
        }

        /// <summary>
        /// �����ֿ�
        /// </summary>
        /// <returns></returns>
        public ActionResult ExistForm(string keyValues)
        {
            telphoneliangh5bll.ExistForm(keyValues);
            return Success("״̬���޸�Ϊ�ֿ���");
        }

        /// <summary>
        /// ������ɱ
        /// </summary>
        /// <returns></returns>
        public ActionResult MiaoShaForm(string keyValues)
        {
            telphoneliangh5bll.MiaoShaForm(keyValues);
            return Success("״̬���޸�Ϊ��ɱ��");
        }

        /// <summary>
        /// �����¼�
        /// </summary>
        /// <returns></returns>
        public ActionResult DownTelphone(string downTelphones)
        {
            string returnMsg = telphoneliangh5bll.DownTelphone(downTelphones);
            return Success(returnMsg);
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        public ActionResult PriceTelphone(string priceTelphones)
        {
            string returnMsg = telphoneliangh5bll.PriceTelphone(priceTelphones);
            return Success(returnMsg);
        }


        #region ����ɾ������
        public FileResult GetFileDelete()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "Resource/ExcelTemplate/";
            string fileName = "��������ɾ��ģ��.xlsx";
            return File(path + fileName, "application/ms-excel", fileName);
        }

        [HttpPost]
        public ActionResult TelphoneLiangH5Delete(HttpPostedFileBase filebase)
        {
            HttpPostedFileBase file = Request.Files["files"];
            string FileName;
            string savePath;
            if (file == null || file.ContentLength <= 0)
            {
                ViewBag.error = "�ļ�����Ϊ��";
                return View();
            }
            else
            {
                string filename = Path.GetFileName(file.FileName);
                int filesize = file.ContentLength;//��ȡ�ϴ��ļ��Ĵ�С��λΪ�ֽ�byte
                string fileEx = System.IO.Path.GetExtension(filename);//��ȡ�ϴ��ļ�����չ��
                string NoFileName = System.IO.Path.GetFileNameWithoutExtension(filename);//��ȡ����չ�����ļ���
                int Maxsize = 4000 * 1024;//�����ϴ��ļ������ռ��СΪ4M
                string FileType = ".xls,.xlsx";//�����ϴ��ļ��������ַ���

                FileName = NoFileName + DateTime.Now.ToString("yyyyMMddhhmmss") + fileEx;
                if (!FileType.Contains(fileEx))
                {
                    ViewBag.error = "�ļ����Ͳ��ԣ�ֻ�ܵ���xls��xlsx��ʽ���ļ�";
                    return View();
                }
                if (filesize >= Maxsize)
                {
                    ViewBag.error = "�ϴ��ļ�����4M�������ϴ�";
                    return View();
                }
                string path = AppDomain.CurrentDomain.BaseDirectory + "Resource/ExcelData/";
                savePath = Path.Combine(path, FileName);
                file.SaveAs(savePath);
            }

            //excelתDataTable
            DataTable dtSource = ExcelHelper.ExcelImport(savePath);
            //��������TelphoneWash��
            //SqlBulkCopyByDatatable("TelphoneWash", dtSource);
            //һ���в���

            //����������ƣ�����ʱ������ع�
            ViewBag.error = telphoneliangh5bll.BatchDeleteEntity(dtSource);

            System.Threading.Thread.Sleep(2000);
            return View();
        }
        #endregion
    }
}
