using HZSoft.Application.Entity.CustomerManage;
using HZSoft.Application.Busines.CustomerManage;
using HZSoft.Util;
using HZSoft.Util.WebControl;
using System.Web.Mvc;
using System;
using System.Data;
using System.Linq;

namespace HZSoft.Application.Web.Areas.CustomerManage.Controllers
{
    /// <summary>
    /// �� �� 6.1
    /// 
    /// �� ������������Ա
    /// �� �ڣ�2020-03-05 10:43
    /// �� �������������
    /// </summary>
    public class TelphoneVipShareController : MvcControllerBase
    {
        //ѡ�������
        private TelphoneVipShareBLL telphonevipsharebll = new TelphoneVipShareBLL();
        //����vip����
        private TelphoneLiangVipBLL telphoneliangvipbll = new TelphoneLiangVipBLL();

        #region ��ͼ����
        /// <summary>
        /// �б�ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult TelphoneVipShareIndex()
        {
            return View();
        }
        /// <summary>
        /// ��ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult TelphoneVipShareForm()
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
            var data = telphonevipsharebll.GetPageList(pagination, queryJson);
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
            var data = telphonevipsharebll.GetList(queryJson);
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
            var data = telphonevipsharebll.GetEntity(keyValue);
            return ToJsonResult(data);
        }

        /// �û��б�
        /// </summary>
        /// <param name="roleId">��ɫId</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetVipListJson(string vipId)
        {
            var existMember = telphonevipsharebll.GetVipList(vipId);
            var userdata = telphoneliangvipbll.GetTable();
            userdata.Columns.Add("ischeck", Type.GetType("System.Int32"));
            userdata.Columns.Add("isdefault", Type.GetType("System.Int32"));
            foreach (DataRow item in userdata.Rows)
            {
                string organizeid = item["organizeid"].ToString();
                int ischeck = existMember.Count(t => t.ShareId == organizeid);
                item["ischeck"] = ischeck;
            }
            userdata = DataHelper.DataFilter(userdata, "", "createdate desc");
            return Content(userdata.ToJson());
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
            telphonevipsharebll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, TelphoneVipShareEntity entity)
        {
            telphonevipsharebll.SaveForm(keyValue, entity);
            return Success("�����ɹ���");
        }


        /// <summary>
        /// �����ɫ��Ա
        /// </summary>
        /// <param name="vipId">��ɫId</param>
        /// <param name="shareIds">��ԱId</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveMember(string vipId, string shareIds)
        {
            telphonevipsharebll.SaveMember(vipId, shareIds);
            return Success("����ɹ���");
        }
        #endregion
    }
}
