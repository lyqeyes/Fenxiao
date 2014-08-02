using FenXiao.Model;
using FenXiao.Web.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FenXiao.Web.Areas.Wholesaler.Controllers
{
    public class WMessageController : WholesalerControllerBase
    {

        public ActionResult Index()
        {
            var msgs = db.Messages.Where(a => a.ToCompanyId == FenXiaoUserContext.Current.LoginInfo.CompanyId
                && a.IsRead == 0 &&
                (a.State == (int)EnumMessage.xiadingdan ||
                a.State == (int)EnumMessage.xiatuidan ||
                a.State == (int)EnumMessage.chulilingshoushangshenqing)).ToList();
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
                if (message.State == (int)EnumMessage.xiadingdan)
                {
                    return RedirectToAction("OrderDetial", "WHome", new { Area = "Wholesaler", id = message.RelatedId });

                }
                else if (message.State == (int)EnumMessage.xiatuidan)
                {
                    return RedirectToAction("ReturnOrderDetial", "WHome", new { Area = "Wholesaler", id = message.RelatedId });

                }
                else
                {
                    return RedirectToAction("HandleComInfoResult", "WMessage", new { Area = "Wholesaler", id = message.RelatedId });

                }
            }
        }

        public ActionResult HandleComInfoResult(int id)
        {
            var v = db.HandleApplies.Find(id);
            if (v == null)
            {
                return HttpNotFound();
            }
            return View(v);
        }

        public ActionResult Help()
        {
            return View();
        }
    }
}