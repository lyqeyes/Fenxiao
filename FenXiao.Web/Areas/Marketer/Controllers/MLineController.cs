using FenXiao.Model;
using FenXiao.Web.Common;
using FenXiao.Web.Extension;
using System;
using System.Linq;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;

namespace FenXiao.Web.Areas.Marketer.Controllers
{
    public class MLineController : MarketerControllerBase
    {
        #region 线路管理
        //线路管理首页
        [HttpGet]
        public ActionResult Line(int id = 0)
        {
            ViewBag.IsOk = db.ChildProducts.Count(e => e.CompanyId == FenXiaoUserContext.Current.UserInfo.CompanyId);
            var list = db.ChildProducts.Where(e =>
                e.CompanyId == FenXiaoUserContext.Current.UserInfo.CompanyId).OrderByDescending(a => a.Id).ToPagedList(id, PageSize);
            return View(list);
        }
        //订单页
        [HttpGet]
        public ActionResult Order(long Id)
        {
            ViewBag.ChildProduct = db.ChildProducts.Find(Id);
            return View();
        }
        //正常订单详情页
        [HttpGet]
        public ActionResult CommonOrderDetail(int Id)
        {
            var orderForm = db.OrderForms.Find(Id);
            return View(orderForm);
        }
        //已取消的订单详情页
        [HttpGet]
        public ActionResult CancelOrderDetail(int Id)
        {
            var returnForm = db.ReturnForms.Find(Id);
            return View(returnForm);
        }
        //已取消的订单
        [HttpGet]
        public ActionResult CancelOrder(int Id)
        {
            var list = db.ReturnForms.Where(e => e.ProductId == Id).OrderByDescending(e => e.CreateTime).ToList();
            return PartialView(list);
        }
        //正常的订单
        [HttpGet]
        public ActionResult CommonOrder(int Id)
        {
            var list = db.OrderForms.Where(e => e.ProductId == Id).OrderByDescending(e => e.CreateTime).ToList();
            return PartialView(list);
        }

        [HttpGet]
        public ActionResult ReturnOrder(int Id)
        {
            ViewBag.ChildProduct = db.ChildProducts.Find(Id);
            return View();
        }
        [HttpPost]
        public string ReturnOrder(int ProductId, int ChildProductId, int ChengRenCount, int ErTongCount)
        {
            //TODO 批发商修改订单与零售商订单间的冲突
            //TODO 零售商与零售商间的冲突
            lock (LockClass.obj)
            {
                var product = db.Products.Find(ProductId);
                var childProduct = db.ChildProducts.Find(ChildProductId);
                if (product == null || childProduct == null)
                {
                    HttpNotFound();
                }
                else if (childProduct.ChengRenCount + childProduct.ErTongCount < ErTongCount + ChengRenCount)
                {
                    return "100";
                }
                else
                {
                    ReturnForm rf = new ReturnForm();
                    rf.ChengRenCount = ChengRenCount;
                    rf.ChengRenPrice = product.ChengRenPrice;
                    rf.ErTongCount = ErTongCount;
                    rf.ErTongPrice = product.ErTongPrice;
                    rf.State = (int)EnumReturnForm.xiatuidan;
                    rf.ProductId = product.Id;
                    rf.CreateUserId = FenXiaoUserContext.Current.UserInfo.Id;
                    rf.CreateTime = DateTime.Now;
                    rf.TotalPrice = rf.ErTongPrice * rf.ErTongCount + rf.ChengRenCount * rf.ChengRenPrice;
                    rf.ToCompanyId = product.User.CompanyId;
                    //TODO 没有信息吗
                    rf.Name = product.Name;
                    rf.Note = Request.Form["Reason"];
                    db.Entry<ReturnForm>(rf).State = System.Data.Entity.EntityState.Added;
                    db.SaveChanges();
                    var message = new FenXiao.Model.Message();
                    message.CreateTime = DateTime.Now;
                    message.ToCompanyId = product.User.CompanyId;
                    message.IsRead = 0;
                    message.MessageContent = FenXiaoUserContext.Current.UserInfo.Company.CompanyName
                                                                    + "退了线路“" + product.Name + "”共" + (rf.ChengRenCount + rf.ErTongCount).ToString() + "人的票";
                    message.RelatedId = rf.Id;
                    message.State = (int)EnumMessage.xiatuidan;
                    db.Entry<FenXiao.Model.Message>(message).State = System.Data.Entity.EntityState.Added;
                    db.SaveChanges();
                    return "200";
                }
                return "0";
            }
        }
        #region 人员管理部分
        //人员管理
        [HttpGet]
        public ActionResult OrderPeople(int Id)
        {
            //TODO id不一致啊
            ViewBag.Id = Id;
            var list = db.CustomerInfoes.Where(e => e.ChildProductId == Id && e.IsDelete == (int)EnumCustomer.zhengchang).ToList();
            return PartialView(list);
        }
        //添加人员
        [HttpPost]
        public ActionResult AddPeople([Bind(Include = "Name,Phone,Email,Sex,ChildProductId")]CustomerInfo customerInfo)
        {
            customerInfo.CreateUserId = FenXiaoUserContext.Current.UserInfo.Id;
            //TODO email 不实用
            //TODO 创建时间类型不对
            //customerInfo.CreateTime = DateTime.Now;
            customerInfo.IsDelete = (int)EnumCustomer.zhengchang;
            db.Entry<CustomerInfo>(customerInfo).State = System.Data.Entity.EntityState.Added;
            db.SaveChanges();
            long Id = db.ChildProducts.Find(customerInfo.ChildProductId).Id;
            TempData["Type"] = 3;
            return RedirectToAction("Order", new { Id = Id });
        }
        //删除人员
        [HttpGet]
        public ActionResult DelPeople(int Id)
        {
            var customerInfo = db.CustomerInfoes.Find(Id);
            customerInfo.IsDelete = (int)EnumCustomer.yishanchu;
            db.Entry<CustomerInfo>(customerInfo).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            TempData["Type"] = 3;
            return RedirectToAction("Order", new { Id = customerInfo.ChildProduct.Id });
        }
        #endregion

        #endregion
    }
}