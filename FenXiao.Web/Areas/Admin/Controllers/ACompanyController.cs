using FenXiao.Model;
using FenXiao.Web.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;
namespace FenXiao.Web.Areas.Admin.Controllers
{
    public class ACompanyController : AdminControllerBase
    {
        //
        // GET: /Admin/ACompany/
        public ActionResult AllCompany(int? id)
        {
            List<Company> list = db.Companies.OrderByDescending(a => a.CreateTime).ToList();
            return View(list.ToPagedList(id ?? 1, 25));
        }
        public ActionResult CompanySearch()
        {
            //..检索
            return View("AllCompany");
        }

        public ActionResult CompanyDetail(int id)
        {
            Company com = db.Companies.FirstOrDefault(a => a.Id == id);
            if (com != null)
            {
                return View(com);
            }
            else
                return View() ;
        }
        public ActionResult CompanyAccounts(int? id, int comId)
        {
            var accounts = db.Users.Where(a => a.CompanyId == comId).OrderBy(a => a.Role);
            return View(accounts.ToPagedList(id??1,25));
        }
        public ActionResult CompanyChangeState(int id)
        {
            var company = db.Companies.FirstOrDefault(a => a.Id == id);
            if (company != null)
            {
                company.State = company.State == 1 ? 0 : 1;
                db.Entry(company).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("AllCompany");
        }
        public ActionResult AccountChangeState(int id)
        {
            var acc = db.Users.FirstOrDefault(a => a.Id == id);
            acc.State = acc.State == 1 ? 0 : 1;
            db.Entry(acc).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("CompanyAccounts", new { comId = acc.CompanyId });
        }
	}
}