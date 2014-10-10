using FenXiao.Model;
using FenXiao.Web.Common;
using FenXiao.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;

namespace FenXiao.Web.Areas.Wholesaler.Controllers
{
    public class WMessageController : WholesalerControllerBase
    {

        public ActionResult Index(int id=0)
        {
            var msgs = db.Messages.Where(a => a.ToCompanyId == FenXiaoUserContext.Current.LoginInfo.CompanyId
                &&(a.State == (int)EnumMessage.xiadingdan ||
                a.State == (int)EnumMessage.xiatuidan ||
                a.State == (int)EnumMessage.DirectApplyOrder ||
                a.State == (int)EnumMessage.DirectApplyOrderEditing ||
                a.State == (int)EnumMessage.ReserveNowOrder ||
                a.State == (int)EnumMessage.chulilingshoushangshenqing)).OrderByDescending(a=>a.Id).ToPagedList(id,PageSize);
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
            if (ha==null)
            {
                return HttpNotFound();
            }
            return View(ha);
        }
    }
}