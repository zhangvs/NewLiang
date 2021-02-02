using HZSoft.Application.Entity.CustomerManage;
using HZSoft.Application.Busines.CustomerManage;
using HZSoft.Util;
using HZSoft.Util.WebControl;
using System.Web.Mvc;
using HZSoft.Application.Busines.SystemManage;
using HZSoft.Application.Code;

namespace HZSoft.Application.Web.Areas.CustomerManage.Controllers
{
    /// <summary>
    /// �� �� 6.1
    /// 
    /// �� ������������Ա
    /// �� �ڣ�2017-09-22 15:43
    /// �� �������붩��
    /// </summary>
    public class TelphoneOrderController : MvcControllerBase
    {
        private TelphoneOrderBLL telphoneorderbll = new TelphoneOrderBLL();
        private CodeRuleBLL codeRuleBLL = new CodeRuleBLL();

        #region ��ͼ����
        /// <summary>
        /// ������������
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult TelphoneOrderIndex()
        {
            return View();
        }
        /// <summary>
        /// Ԥ������
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult TelphoneOrderReserve()
        {
            return View();
        }
        /// <summary>
        /// ������
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult TelphoneOrderSend()
        {
            return View();
        }
        /// <summary>
        /// ��ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult TelphoneOrderForm()
        {
            if (Request["keyValue"] == null)
            {
                ViewBag.OrderCode = codeRuleBLL.GetBillCode("c576c3f7-631d-4108-baaf-1495bdc0d6bb");
                //ViewBag.OrderCode = codeRuleBLL.GetBillCode(SystemInfo.CurrentUserId, "", ((int)CodeRuleEnum.Telphone_OrderCode).ToString());
            }
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
            var data = telphoneorderbll.GetPageList(pagination, queryJson);
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
            var data = telphoneorderbll.GetList(queryJson);
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
            var data = telphoneorderbll.GetEntity(keyValue);
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
            telphoneorderbll.RemoveForm(keyValue);
            return Success("ɾ���ɹ���");
        }
        private Kd100SubscribeDemo kd100 = new Kd100SubscribeDemo();
        private KdApiSubscribeDemo kdn = new KdApiSubscribeDemo();
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveForm(string keyValue, TelphoneOrderEntity entity)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                TelphoneOrderEntity oldentity = telphoneorderbll.GetEntity(keyValue);
                //�Ͽ�ݵ���Ϊ�գ��¿�ݵ��Ų�Ϊ�գ����η����������ݽӿ�
                if (!string.IsNullOrEmpty(entity.Numbers) && string.IsNullOrEmpty(oldentity.Numbers))
                {
                    kdn.orderTracesSubByJson(entity);//�������ѽӿڣ�������ʵ������ϵ��ʽ
                    entity.Sign = 0;
                }
            }
            else
            {
                //���Ľӿ�
                if (!string.IsNullOrEmpty(entity.Numbers))
                {
                    //kd100.orderTracesSubByJson(entity.Express,entity.Numbers);
                    kdn.orderTracesSubByJson(entity);//�������ѽӿڣ�������ʵ������ϵ��ʽ
                    entity.Sign = 0;
                }
                entity.CheckMark = 1;//�ֶ����������ˣ������״̬
            }

            telphoneorderbll.SaveForm(keyValue, entity);
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
        public ActionResult UpdateCheckState(string keyValue)
        {
            telphoneorderbll.UpdateCheckState(keyValue, 1);
            return Success("�˵��ɹ���");
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
            telphoneorderbll.UpdateDeleteState(keyValue, 1);
            return Success("���ϳɹ���");
        }
    }
}
