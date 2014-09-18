using FenXiao.Model;
using FenXiao.Web.Areas.Admin.Models;
using FenXiao.Web.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;
namespace FenXiao.Web.Areas.Admin.Controllers
{
    public class ACompanyController : AdminControllerBase
    {
        private List<string> GetPropertyList(object obj)
        {
            var propertyList = new List<string>();
            var properties = obj.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            foreach (var property in properties)
            {
                object o = property.GetValue(obj, null);
                propertyList.Add(o == null ? "" : o.ToString());
            }
            return propertyList;
        }
        //
        // GET: /Admin/ACompany/
        public ActionResult AllCompany(int? id)
        {
            //List<Company> list = db.Companies.OrderByDescending(a => a.CreateTime).ToList();
            //return View(list.ToPagedList(id ?? 1, 25));
            return View();
        }
        public ActionResult GetAllCompany(int? secho)
        {
            var query_temp = db.Companies.Select(a => new { a.Id, a.CompanyName,a.CompanyRole, a.Province,a.City, a.CompanyAddress,a.CompanyPhone,a.State }).ToList();
            var query = new List<Companydatamodel>();
            foreach (var item in query_temp)
            {
                if (item.CompanyRole.Split(',').Contains("0") && item.CompanyRole.Split(',').Contains("1"))
                {
                    query.Add(new Companydatamodel()
                    {
                        Id = item.Id,
                        CompanyName = item.CompanyName,
                        CompanyRole = "批发商 & 零售商",
                        City = item.Province + ',' + item.City,
                        CompanyAddress = item.CompanyAddress,
                        CompanyPhone = item.CompanyPhone,
                        State = item.State
                    });
                }
                else if (item.CompanyRole.Split(',').Contains("0"))
                {
                    query.Add(new Companydatamodel()
                    {
                        Id = item.Id,
                        CompanyName = item.CompanyName,
                        CompanyRole = "零售商",
                        City = item.Province + ',' + item.City,
                        CompanyAddress = item.CompanyAddress,
                        CompanyPhone = item.CompanyPhone,
                        State = item.State
                    });
                }
                else {
                    query.Add(new Companydatamodel()
                    {
                        Id = item.Id,
                        CompanyName = item.CompanyName,
                        CompanyRole = "批发商",
                        City = item.Province + ',' + item.City,
                        CompanyAddress = item.CompanyAddress,
                        CompanyPhone = item.CompanyPhone,
                        State = item.State
                    });
                }
            }
            var objs = new List<object>();
            foreach (var item in query)
            {
                objs.Add(GetPropertyList(item).ToArray());
            }
            return Json(new
            {
                sEcho = secho,
                iTotalRecords = query.Count(),
                aaData = objs,
            }, JsonRequestBehavior.AllowGet);
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