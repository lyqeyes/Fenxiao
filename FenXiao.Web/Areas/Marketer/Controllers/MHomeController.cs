using FenXiao.Model;
using FenXiao.Web.Common;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FenXiao.Web.Models; 
namespace FenXiao.Web.Areas.Marketer.Controllers
{
    public class MHomeController : MarketerControllerBase
    {
        public ActionResult Index() 
        {
            return View();
        }

        #region 消息管理
        //消息列表
        public ActionResult Message()
        {
            var msgs = db.Messages.Where(a => a.ToCompanyId == FenXiaoUserContext.Current.LoginInfo.CompanyId
                && a.IsRead == 0 &&
                (a.State == (int)EnumMessage.chulidingdan ||
                a.State == (int)EnumMessage.chulituidan ||
                a.State == (int)EnumMessage.chulipifashangshenqing)).ToList();
            return View(msgs);
        }

        public ActionResult Read(int id)
        {
            var message = db.Messages.Find(id);
            if (message == null)
            {
                return HttpNotFound();
            }
            else
            {
                message.IsRead = 1;
                db.Entry(message).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                if (message.State == (int)EnumMessage.chulidingdan)
                {
                    return RedirectToAction("CommonOrderDetail", "MHome", new { Area = "Marketer", id = message.RelatedId });

                }
                else if (message.State == (int)EnumMessage.chulituidan)
                {
                    return RedirectToAction("CancelOrderDetail", "MHome", new { Area = "Marketer", id = message.RelatedId });

                }
                else
                {
                    return RedirectToAction("HandleComInfoResult", "MHome", new { Area = "Marketer", id = message.RelatedId });

                }
            }
        }

        //公司申请信息结果
        public ActionResult HandleComInfoResult(int id)
        {
            var v = db.HandleApplies.Find(id);
            if (v==null)
            {
                return HttpNotFound();
            }
            return View(v);
        }
        #endregion

        public ActionResult Help()
        {
            List<HelpModel> hms = new List<HelpModel>();
            var list = db.HelpTypes.OrderByDescending(a => a.Weight).ToList();
            foreach (var item in list)
            {
                HelpModel hm = new HelpModel() { };
                hm.Name = item.Name;
                hm.HelpArticles = item.HelpArticles.OrderByDescending(a => a.Weight).ToList();
                hms.Add(hm);
            }
            return View(hms);
        }

        public ActionResult HelpContent(int id)
        {
            var ha = db.HelpArticles.Find(id);
            if (ha == null)
            {
                return HttpNotFound();
            }
            return View(ha);
        }
    }
}