using FenXiao.Web.Areas.Admin.Models;
using FenXiao.Web.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;
using FenXiao.Model;

namespace FenXiao.Web.Areas.Admin.Controllers
{
    public class AInfoChangeCheckController : AdminControllerBase
    {
        // GET: Admin/AInfoChangeCheck
        public ActionResult Index(int id = 1)
        {
            var tempcompanylist = db.TempCompanies.OrderByDescending(a => a.CreateTime);
            var page = tempcompanylist.ToPagedList(id,PageSize);
            return View(page);
        }

        public ActionResult TempCompanyDetail(int id)
        {
            var newcompany = db.TempCompanies.FirstOrDefault(a => a.Id == id);
            var oldcompany = db.Companies.FirstOrDefault(a => a.Id == newcompany.CompanyId);
            InfoCheckModel result = new InfoCheckModel()
            {
                TempCompanyId = newcompany.Id,
                CompanyNew = newcompany,
                CompanyOld = oldcompany
            };
            return View(result);
        }
        public ActionResult DealCheck(int TempCompanyId,string Choice, string reason = "无")
        {
            var tempcompany = db.TempCompanies.FirstOrDefault(a => a.Id == TempCompanyId);
            if (tempcompany == null)
                return RedirectToAction("index");
            var oldcompany = db.Companies.FirstOrDefault(a => a.Id == tempcompany.CompanyId);
            if (Choice == "yes")
            {
                //1.发送消息
                db.Messages.Add(new FenXiao.Model.Message
                {
                    CreateTime = DateTime.Now,
                    IsRead = 0,
                    MessageContent = "您修改公司资料的申请已通过.",
                    State = (int)EnumMessage.CompanyEdit,
                    RelatedId = 0,
                    ToCompanyId = tempcompany.CompanyId
                });

                //2.更新company
                oldcompany.CompanyName = tempcompany.CompanyName;
                oldcompany.CompanyAddress = tempcompany.CompanyAddress;
                oldcompany.CompanyName = tempcompany.CompanyName;
                oldcompany.CompanyPhone = tempcompany.CompanyPhone;
                oldcompany.LianXiRen = tempcompany.LianXiRen;
                oldcompany.Phone = tempcompany.Phone;
                oldcompany.YingYeZhiZhao = tempcompany.YingYeZhiZhao;
                oldcompany.YingYeZhiZhaoImg = tempcompany.YingYeZhiZhaoImg;
                oldcompany.FaRenShenFenZhengImg = tempcompany.FaRenShenFenZhengImg;
                db.Entry(oldcompany).State = System.Data.Entity.EntityState.Modified;

                //3.删除tempcompany
                db.TempCompanies.Remove(tempcompany);
                db.SaveChanges();
            }
            //申请不通过
            else
            {
                db.Messages.Add(new FenXiao.Model.Message
                {
                    CreateTime = DateTime.Now,
                    IsRead = 0,
                    MessageContent = "您修改公司资料的请求被管理员拒绝. 原因是:" + reason,
                    State = (int)EnumMessage.CompanyEdit,
                    RelatedId = 0,
                    ToCompanyId = tempcompany.CompanyId
                });

                db.TempCompanies.Remove(tempcompany);
                db.SaveChanges();
            }
            return RedirectToAction("index");
        }
    }
}