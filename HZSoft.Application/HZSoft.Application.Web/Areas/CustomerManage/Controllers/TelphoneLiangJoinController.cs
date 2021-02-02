using HZSoft.Application.Entity.CustomerManage;
using HZSoft.Application.Busines.CustomerManage;
using HZSoft.Util;
using HZSoft.Util.WebControl;
using System.Web.Mvc;
using HZSoft.Application.Cache;

namespace HZSoft.Application.Web.Areas.CustomerManage.Controllers
{
    /// <summary>
    /// �� �� 6.1
    /// 
    /// �� ������������Ա
    /// �� �ڣ�2018-09-05 17:58
    /// �� �������ż��˴���
    /// </summary>
    public class TelphoneLiangJoinController : MvcControllerBase
    {
        private TelphoneLiangJoinBLL telphoneliangjoinbll = new TelphoneLiangJoinBLL();
        private OrganizeCache organizeCache = new OrganizeCache();

        #region ��ͼ����
        /// <summary>
        /// ���˴����б�ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult TelphoneLiangJoinIndex()
        {
            return View();
        }
        /// <summary>
        /// ��Դ��������0�������б�ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult TelphoneLiangJoinIndex1()
        {
            return View();
        }
        /// <summary>
        /// ��Ӧ���б�ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult TelphoneLiangJoinIndex2()
        {
            return View();
        }
        /// <summary>
        /// ��ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult TelphoneLiangJoinForm()
        {
            return View();
        }
        /// <summary>
        /// ��ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult TelphoneLiangJoinForm1()
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
            var data = telphoneliangjoinbll.GetPageList(pagination, queryJson);
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
        /// ������Դ��������
        /// </summary>
        /// <param name="pagination">��ҳ����</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�Json</returns>
        [HttpGet]
        public ActionResult GetPageListJson1(Pagination pagination, string queryJson)
        {
            var watch = CommonHelper.TimerStart();
            var data = telphoneliangjoinbll.GetPageList1(pagination, queryJson);
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
        /// ��Ӧ��
        /// </summary>
        /// <param name="pagination">��ҳ����</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�Json</returns>
        [HttpGet]
        public ActionResult GetPageListJson2(Pagination pagination, string queryJson)
        {
            var watch = CommonHelper.TimerStart();
            var data = telphoneliangjoinbll.GetPageList2(pagination, queryJson);
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
            var data = telphoneliangjoinbll.GetList(queryJson);
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
            var data = telphoneliangjoinbll.GetEntity(keyValue);
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
            telphoneliangjoinbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, TelphoneLiangJoinEntity entity)
        {
            telphoneliangjoinbll.SaveForm(keyValue, entity);
            return Success("�����ɹ���");
        }
        #endregion



        /// <summary>
        /// �˵�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        [HttpPost]
        [AjaxOnly]
        public ActionResult UpdateTopOrg(string keyValue)
        {
            var entity = telphoneliangjoinbll.GetEntity(keyValue);
            string msg = "";
            var organize = organizeCache.GetEntityByTel(entity.Telphone);
            if (!string.IsNullOrEmpty(organize.OrganizeId))
            {
                msg = "���ֻ�������Ļ����Ѿ����ڣ��뻻���ֻ����������룡";
            }
            //���������̳����ӣ����¼�
            msg = telphoneliangjoinbll.UpdateTopOrg(entity, 1);
            return Content(msg);
        }


        /// <summary>
        /// �˵�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        [HttpPost]
        [AjaxOnly]
        public ActionResult UpdateCheckState(string keyValue)
        {
            var entity = telphoneliangjoinbll.GetEntity(keyValue);
            string msg = "";
            var organize = organizeCache.GetEntityByTel(entity.Telphone);
            if (!string.IsNullOrEmpty(organize.OrganizeId))
            {
                msg= "���ֻ�������Ļ����Ѿ����ڣ��뻻���ֻ����������룡";
            }
            //���������̳����ӣ����¼�
            msg = telphoneliangjoinbll.UpdateCheckState(entity, 1);
            return Content(msg);
        }

        /// <summary>
        /// �˵�-my
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        [HttpPost]
        [AjaxOnly]
        public ActionResult UpdateCheckStateMy(string keyValue)
        {
            var entity = telphoneliangjoinbll.GetEntity(keyValue);
            string msg = "";
            var organize = organizeCache.GetEntityByTel(entity.Telphone);
            if (!string.IsNullOrEmpty(organize.OrganizeId))
            {
                msg = "���ֻ�������Ļ����Ѿ����ڣ��뻻���ֻ����������룡";
            }
            //���������̳����ӣ����¼�
            msg = telphoneliangjoinbll.UpdateCheckState(entity, 2);
            return Content(msg);
        }

        /// <summary>
        /// ����
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        [HttpPost]
        [AjaxOnly]
        public ActionResult UpdateDeleteState(string keyValue)
        {
            telphoneliangjoinbll.UpdateDeleteState(keyValue, 1);
            return Success("���ϳɹ���");
        }
    }
}
