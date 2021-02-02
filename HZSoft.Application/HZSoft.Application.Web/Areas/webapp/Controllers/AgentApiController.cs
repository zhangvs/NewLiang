using HZSoft.Application.Busines.BaseManage;
using HZSoft.Application.Busines.CustomerManage;
using HZSoft.Application.Code;
using HZSoft.Application.Entity.BaseManage;
using HZSoft.Application.Entity.CustomerManage;
using HZSoft.Application.Entity.WeChatManage;
using HZSoft.Application.Web.Utility;
using HZSoft.Util;
using HZSoft.Util.WebControl;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HZSoft.Application.Web.Areas.webapp.Controllers
{
    [HandlerWX2AuthorizeAttribute(LoginMode.Enforce)]
    public class AgentApiController : Controller
    {
        private Wechat_AgentBLL agentBll = new Wechat_AgentBLL();
        private ComissionLogBLL comissionLogBll = new ComissionLogBLL();
        private WithdrawLogBLL withdrawLogBLL = new WithdrawLogBLL();
        //
        // GET: /webapp/AgentApi/

        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 下级代理明细佣金汇总
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public ActionResult comissionSummary(string start, string end)
        {
            //return Content("{\"code\":200,\"msg\":\"success\",\"data\":[{\"name\":\"\u601d\u79d1\u83b1-\u667a\u80fd\u79d1\u6280-\u5f20\u5f66\u5c71\",\"contact\":\"\u672a\u586b\u5199\",\"profit\":0}]}");//, "text/json"
            var agentEntity = agentBll.GetEntityByOpenId(CurrentWxUser.OpenId);
            if (agentEntity != null)
            {
                var agentList = agentBll.GetList("{\"Pid\":\"" + agentEntity.Id + "\",\"StartTime\":\"" + start + "\",\"EndTime\":\"" + end + "\"}");
                var root = new ReturnJson { code = 200, msg = "success", data = agentList };
                return Json(root);
            }
            else
            {
                var root = new ReturnJson { code = 401, msg = "fail" };
                return Json(root);
            }
        }
        /// <summary>
        /// 下级代理佣金明细
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="status"></param>
        /// <param name="child_id"></param>
        /// <returns></returns>
        public ActionResult comissionLog(int page, int pagesize, string start, string end,int? status,int? child_id)
        {
            var agentEntity = agentBll.GetEntityByOpenId(CurrentWxUser.OpenId);
            if (agentEntity != null)
            {
                Pagination pagination = new Pagination()
                {
                    rows = pagesize,
                    page = page,
                    sidx = "created_at",
                    sord = "desc"
                };
                if (string.IsNullOrEmpty(child_id.ToString()))
                {
                    child_id = agentEntity.Id;//
                }
                JObject queryJson = new JObject { { "StartTime", start },
                        { "EndTime", end },
                        { "status", status },
                        { "agent_id", child_id }
                    };
                var entityList = comissionLogBll.GetPageList(pagination, queryJson.ToString());
                decimal? sum0 = 0, sum1 = 0, sum2 = 0;
                foreach (var item in entityList)
                {
                    if (item.status == 0)
                    {
                        sum0 += item.profit;//未付款
                    }
                    if (item.status == 1)
                    {
                        sum1 += item.profit;//已支付
                    }
                    if (item.status == 2)
                    {
                        sum2 += item.profit;//已入账
                    }
                }
                List<stats> ss = new List<stats>();
                ss.Add(new stats() { status = 0, sum = sum0 });
                ss.Add(new stats() { status = 1, sum = sum1 });
                ss.Add(new stats() { status = 2, sum = sum2 });


                dynamic logsdata = new ExpandoObject();
                logsdata.data = entityList;
                //logsdata.current_page = page;

                dynamic objmatch = new ExpandoObject();
                objmatch.stats = ss;
                objmatch.logs = logsdata;

                dynamic obj = new ExpandoObject();
                obj.code = 200;
                obj.msg = "success";
                obj.data = objmatch;

                string json = JsonConvert.SerializeObject(obj);
                return Content(json, "text/json");
            }
            else
            {
                var root = new ReturnJson { code = 401, msg = "fail" };
                return Json(root);
            }
            //return Content("{\"code\":200,\"msg\":\"success\",\"data\":{\"stats\":[{\"status\":0,\"sum\":88.08},{\"status\":1,\"sum\":0},{\"status\":2,\"sum\":0}],\"logs\":{\"current_page\":1,\"data\":[{\"agent_id\":333,\"profit\":\"41.44\",\"invited_agent_id\":0,\"status\":0,\"indirect\":0,\"phonenum\":\"17155390444\",\"created_at\":\"2020-10-07 14:55:14\",\"agent_name\":\"\u5317\u9f99\u514b\u5ddd\"},{\"agent_id\":333,\"profit\":\"46.64\",\"invited_agent_id\":0,\"status\":0,\"indirect\":0,\"phonenum\":\"16523720009\",\"created_at\":\"2020-09-26 18:17:28\",\"agent_name\":\"\u5317\u9f99\u514b\u5ddd\"}],\"first_page_url\":\"https://shop.jnlxsm.net/agentApi/comissionLog?page=1\",\"from\":1,\"last_page\":1,\"last_page_url\":\"https://shop.jnlxsm.net/agentApi/comissionLog?page=1\",\"next_page_url\":null,\"path\":\"https://shop.jnlxsm.net/agentApi/comissionLog\",\"per_page\":\"40\",\"prev_page_url\":null,\"to\":2,\"total\":2}}}");
        }
        /// <summary>
        /// 下级代理，下拉列表
        /// </summary>
        /// <returns></returns>
        public ActionResult getAgents()
        {
            ReturnJson root = null;
            var agentEntity = agentBll.GetEntityByOpenId(CurrentWxUser.OpenId);
            if (agentEntity != null)
            {
                var agentList = agentBll.GetList("{\"Pid\":\"" + agentEntity.Id + "\"}");//
                root = new ReturnJson { code = 200, msg = "success", data = agentList };
            }
            else
            {
                root = new ReturnJson { code = 401, msg = "fail" };
            }
            return Json(root);
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        public ActionResult userInfo()
        {
            ReturnJson root = null;
            var agentEntity = agentBll.GetEntityByOpenId(CurrentWxUser.OpenId);
            if (agentEntity!=null)
            {
                List<Wechat_AgentEntity> matchList = new List<Wechat_AgentEntity>();
                matchList.Add(agentEntity);
                root = new ReturnJson { code = 200, msg = "success", data = matchList };
            }
            else
            {
                root = new ReturnJson { code = 401, msg = "fail" };
            }
            return Json(root);
        }


        /// <summary>
        /// 提现记录
        /// </summary>
        /// <returns></returns>
        public ActionResult withdrawLogs(int page, int pagesize, string start, string end, int? status)
        {
            var agentEntity = agentBll.GetEntityByOpenId(CurrentWxUser.OpenId);
            if (agentEntity != null)
            {
                Pagination pagination = new Pagination()
                {
                    rows = pagesize,
                    page = page,
                    sidx = "id",
                    sord = "desc"
                };
                JObject queryJson = new JObject { { "StartTime", start },
                        { "EndTime", end },
                        { "status", status },
                        { "agent_id", agentEntity.Id.ToString()}// 
                    };
                var withdrawList = withdrawLogBLL.GetPageList(pagination, queryJson.ToJson());

                JObject obj = new JObject();
                JObject obj1 = new JObject();

                obj1["data"] = JArray.FromObject(withdrawList);
                obj1["current_page"] = pagination.page;
                obj1["last_page"] = pagination.total;
                obj1["total"] = pagination.records;

                obj["code"] = 200;
                obj["success"] = "success";
                obj["data"] = obj1;//嵌套

                IsoDateTimeConverter timeConverter = new IsoDateTimeConverter { DateTimeFormat = "yyyy'-'MM'-'dd" };
                //json显示日期带T问题的解决方法
                //此问题是由Newtonsoft.Json转换json导致的；
                //Newtonsoft.Json产生的默认日期时间格式为： IsoDateTimeConverter 格式
                string root = JsonConvert.SerializeObject(obj, Formatting.Indented, timeConverter);

                return Content(root, "text/json");
            }
            else
            {
                var root = new ReturnJson { code = 401, msg = "fail" };
                return Json(root);
            }
            
        }


        /// <summary>
        /// 保存个人资料
        /// </summary>
        /// <returns></returns>
        public ActionResult saveUserInfo(string realname,string contact,string alipay)
        {
            //状态实体vo类实例化
            ReturnJson returnJson = new ReturnJson();
            //声明空字符串图片路径变量准备接收传输过来的值
            string banner = "";
            //try...catch捕捉程序异常错误
            try
            {
                var agentEntity = agentBll.GetEntityByOpenId(CurrentWxUser.OpenId);
                if (agentEntity != null)
                {

                    if (Request.Files.Count > 0)
                    {
                        string extension = string.Empty;
                        HttpPostedFileBase file = Request.Files[0] as HttpPostedFileBase;
                        if (file.FileName.Length > 0)
                        {
                            if (file.FileName.IndexOf('.') > -1)
                            {
                                //原来也可以这用获取扩展名
                                //extension = file.FileName.Remove(0, file.FileName.LastIndexOf('.'));
                                string filePath = "/Resource/DocumentFile/Agent/";
                                //创建路径
                                CreateFilePath(filePath);
                                if (file.FileName.ToString() != "")
                                {
                                    string absoluteSavePath = System.Web.HttpContext.Current.Server.MapPath("~" + filePath);
                                    var pathLast = Path.Combine(absoluteSavePath, file.FileName);
                                    file.SaveAs(pathLast);
                                    banner = filePath + file.FileName;
                                }
                            }
                        }
                    }
                    Wechat_AgentEntity entity = new Wechat_AgentEntity()
                    {
                        realname = realname,
                        contact = contact,
                        alipay = alipay,
                        banner = banner
                    };
                    agentBll.SaveForm(agentEntity.Id, entity);

                    returnJson.msg = "success";
                    returnJson.code = 200;
                    return Json(returnJson);
                }
                else
                {
                    var root = new ReturnJson { code = 401, msg = "fail" };
                    return Json(root);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 当存储的文件路径不存在时，创建文件路径
        /// </summary>
        /// <param name="savePath">保存路径（非绝对路径）</param>
        public static void CreateFilePath(string savePath)
        {
            string Absolute_savePath = System.Web.HttpContext.Current.Server.MapPath("~" + savePath);
            if (!Directory.Exists(Absolute_savePath))
                Directory.CreateDirectory(Absolute_savePath);
        }
       
    }
}
