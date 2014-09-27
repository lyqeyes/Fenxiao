using FenXiao.Model;
using FenXiao.Web.Common;
using FenXiao.Web.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FenXiao.Web.Areas.Marketer.Models;
using Webdiyer.WebControls.Mvc;
using Newtonsoft.Json;

namespace FenXiao.Web.Areas.Marketer.Controllers
{
    public class MSearchController : MarketerControllerBase
    {
        #region 搜索页
        [HttpGet]
        public ActionResult LineSearch(string type = null, DateTime? from = null, DateTime? to = null, int id = 0)
        {
            if (from == null)
            {
                string choosetype = type;
                var list = db.LuXianTypes.Where(a => a.IsDelete == 0 && a.TypeTag.Length == 2).Select(a => a.Id);
                if (type == null)
                {
                    choosetype = "";
                    foreach (var item in list)
                    {
                        choosetype += item + "a";
                    }
                    choosetype = choosetype.Substring(0, choosetype.Length - 1);
                }
                var typelist = choosetype.Split('a');
                List<int> choose = new List<int>();
                foreach (var item in typelist)
                {
                    choose.Add(int.Parse(item));
                }
                if (type == null)
                {
                    var v = db.Product2Type.Select(a => a.Product).Distinct().
                        Where(a => a.State == (int)EnumProduct.zhengchang).OrderByDescending(a => a.Id).ToPagedList(id, PageSize);
                    return View(new LineSearchModel { type = choosetype, Choose = choose, res = v });
                }
                else
                {
                    var choose2 = choose.Except(list);
                    if (choose2.Count() == 0)
                    {
                        var v = db.Product2Type.Select(a => a.Product).Distinct().Where(a => a.State == (int)EnumProduct.zhengchang).OrderByDescending(a => a.Id).ToPagedList(id, PageSize);
                        return View(new LineSearchModel { type = choosetype, Choose = choose, res = v });
                    }
                    else
                    {
                        var v = db.Product2Type.Where(a => choose2.Contains(a.TypeId)).Select(a => a.Product).Distinct().Where(a => a.State == (int)EnumProduct.zhengchang).OrderByDescending(a => a.Id).ToPagedList(id, PageSize);
                        return View(new LineSearchModel { type = choosetype, Choose = choose, res = v });
                    }
                }
            }
            else
            {
                string choosetype = type;
                var list = db.LuXianTypes.Where(a => a.IsDelete == 0 && a.TypeTag.Length == 2).Select(a => a.Id);
                if (type == null)
                {
                    choosetype = "";
                    foreach (var item in list)
                    {
                        choosetype += item + "a";
                    }
                    choosetype = choosetype.Substring(0, choosetype.Length - 1);
                }
                var typelist = choosetype.Split('a');
                List<int> choose = new List<int>();
                foreach (var item in typelist)
                {
                    choose.Add(int.Parse(item));
                }
                if (type == null)
                {
                    var v = db.Product2Type.Select(a => a.Product).Distinct().Where(a => a.State == (int)EnumProduct.zhengchang
                        && a.SendGroupTime > from && a.SendGroupTime < to).OrderByDescending(a => a.Id).ToPagedList(id, PageSize);
                    return View(new LineSearchModel { type = choosetype, Choose = choose, res = v, from = from.ToString(), to = to.ToString() });
                }
                else
                {
                    var choose2 = choose.Except(list);
                    if (choose2.Count() == 0)
                    {
                        var v = db.Product2Type.Select(a => a.Product).Distinct().Where(a => a.State == (int)EnumProduct.zhengchang
                            && a.SendGroupTime > from && a.SendGroupTime < to).OrderByDescending(a => a.Id).ToPagedList(id, PageSize);
                        return View(new LineSearchModel { type = choosetype, Choose = choose, res = v, from = from.ToString(), to = to.ToString() });
                    }
                    else
                    {
                        var v = db.Product2Type.Where(a => choose2.Contains(a.TypeId)).Select(a => a.Product).Distinct().Where(a => a.State == (int)EnumProduct.zhengchang
                            && a.SendGroupTime > from && a.SendGroupTime < to).OrderByDescending(a => a.Id).ToPagedList(id, PageSize);
                        return View(new LineSearchModel { type = choosetype, Choose = choose, res = v, from = from.ToString(), to = to.ToString() });
                    }
                }
            }

            #region 无意义代码
            //var truetypelist = new List<int>();
            //foreach (var item in typelist)
            //{
            //    if (item.Length == 4)
            //    {
            //        truetypelist.Add(int.Parse(item));
            //    }

            //}
            //if (truetypelist.Count == 0)
            //{
            //    return View(new LineSearchModel { Choose = choose, res = db.Products.Where(a=>a.State==(int)EnumProduct.zhengchang).OrderByDescending(a => a.Id).ToPagedList(id,PageSize) });
            //}
            //else
            //{
            //    var pro2type = db.Product2Type.GroupBy(a => a.ProductId).OrderByDescending(a => a.Key).ToList();
            //    List<Product> res = new List<Product>();
            //    foreach (var item in pro2type)
            //    {
            //        var typecount = item.Select(a => a.TypeId).Intersect(truetypelist).Count();
            //        if (typecount == truetypelist.Count)
            //        {
            //            res.Add(item.ElementAt(0).Product);
            //        }
            //        if (res.Count == PageSize)
            //        {
            //            break;
            //        }
            //    }
            //    return View(new LineSearchModel { type = type, Choose = choose, res = res.Where(a=>a.State==(int)EnumProduct.zhengchang).ToList() });
            //} 
            #endregion
        }

        public ActionResult LineTypePage(List<int> Choose = null, string type = null)
        {
            var ProTypes = db.LuXianTypes.
                Where(a => a.IsDelete == 0).
                GroupBy(a => a.TypeTag.Substring(0, 2)).ToList();

            return View(new LineTypeModel { type = type, Choose = Choose, LuXianTypes = ProTypes });
        }

        public ActionResult LineSearchByKey(string key, int id = 0)
        {
            var res = Searcher.Search(key);
            return View(db.Products.OrderByDescending(a => a.Id).Where(a => a.State ==
                (int)EnumProduct.zhengchang && res.Contains(a.Id)).ToPagedList(id, PageSize));
        }
        #endregion

        #region 详情页
        [HttpGet]
        public ActionResult LineDetail(int Id)
        {
            var product = db.Products.Find(Id);
            if (product == null || product.State == (int)EnumProduct.jinyong)
            {
                return HttpNotFound();
            }
            return View(product);
        }
        #endregion

        #region 立即占位
        //填写占位人数页
        [HttpGet]
        public ActionResult ReserveNow(int ProductId)
        {
            ViewBag.ProductId = ProductId;
            return View();
        }
        //ajax提交人数
        [HttpPost]
        public ActionResult ReserveNowAjax(int Id, int ChengRenCount, int ErTongCount)
        {
            AjaxResult ajaxResult = new AjaxResult();

            lock (LockClass.obj)
            {
                var product = db.Products.Find(Id);
                if (product == null)
                {
                    ajaxResult.Ok = 404;
                }
                else if (product.RemainCount < ErTongCount + ChengRenCount)
                {
                    ajaxResult.Ok = 100;
                }
                else if (product.State == (int)EnumProduct.jinyong)
                {
                    ajaxResult.Ok = 300;
                }
                else
                {
                    //线路的数量减少
                    product.RemainCount -= (ErTongCount + ChengRenCount);
                    db.Entry<Product>(product).State = System.Data.Entity.EntityState.Modified;

                    //添加订单
                    OrderForm of = new OrderForm();
                    of.State = (int)EnumOrderForm.ReserveNowOrder;
                    of.ProductId = product.Id;
                    of.CreateUserId = FenXiaoUserContext.Current.UserInfo.Id;
                    of.CreateTime = DateTime.Now;
                    of.ToCompanyId = product.User.CompanyId;                    
                    of.Name = product.Name;
                    of.Note = "";
                    db.Entry<OrderForm>(of).State = System.Data.Entity.EntityState.Added;
                    db.SaveChanges();

                    //更新子线路的数量
                    var childProduct = db.ChildProducts.FirstOrDefault(e => e.ProductId == product.Id && e.CompanyId == FenXiaoUserContext.Current.UserInfo.Company.Id);
                    if (childProduct == null)
                    {
                        childProduct = new ChildProduct();
                        childProduct.CompanyId = FenXiaoUserContext.Current.UserInfo.CompanyId;
                        childProduct.ProductId = product.Id;
                        db.Entry<ChildProduct>(childProduct).State = System.Data.Entity.EntityState.Added;
                    }
                    else
                    {
                        db.Entry<ChildProduct>(childProduct).State = System.Data.Entity.EntityState.Modified;
                    }

                    //添加下单消息
                    var message = new FenXiao.Model.Message();
                    message.CreateTime = DateTime.Now;
                    message.ToCompanyId = product.User.CompanyId;
                    message.IsRead = 0;
                    message.MessageContent = FenXiaoUserContext.Current.UserInfo.Company.CompanyName
                                                                    + "下了线路“" + product.Name + "”共人的占位订单";
                    message.RelatedId = of.Id;
                    message.State = (int)EnumMessage.ReserveNowOrder;
                    db.Entry<FenXiao.Model.Message>(message).State = System.Data.Entity.EntityState.Added;
                    db.SaveChanges();

                    ajaxResult.Ok = 200;
                }
            }
            return Json(ajaxResult);
        }
        #endregion

        #region 直接报名
        //直接报名的首页
        [HttpGet]
        public ActionResult DirectApplyOrder(int ProductId)
        {
            ViewBag.ProductId = ProductId;
            return View();
        }
        //ajax提交人数申请

        [HttpPost]
        public ActionResult DirectApplyOrderAjax(int Id, int ChengRenCount, int ErTongCount)
        {
            AjaxResult ajaxResult = new AjaxResult();

            lock (LockClass.obj)
            {
                var product = db.Products.Find(Id);
                if (product == null)
                {
                    ajaxResult.Ok = 404;
                }
                else if (product.RemainCount < ErTongCount + ChengRenCount)
                {
                    ajaxResult.Ok = 100;
                }
                else if (product.State == (int)EnumProduct.jinyong)
                {
                    ajaxResult.Ok = 300;
                }
                else
                {
                    //线路的数量减少
                    product.RemainCount -= (ErTongCount + ChengRenCount);
                    db.Entry<Product>(product).State = System.Data.Entity.EntityState.Modified;

                    //添加订单
                    OrderForm of = new OrderForm();
                    of.State = (int)EnumOrderForm.ReserveNowOrder;
                    of.ProductId = product.Id;
                    of.CreateUserId = FenXiaoUserContext.Current.UserInfo.Id;
                    of.CreateTime = DateTime.Now;
                    of.ToCompanyId = product.User.CompanyId;
                    of.Name = product.Name;
                    of.Note = "";
                    db.Entry<OrderForm>(of).State = System.Data.Entity.EntityState.Added;
                    db.SaveChanges();

                    //更新子线路的数量
                    var childProduct = db.ChildProducts.FirstOrDefault(e => e.ProductId == product.Id && e.CompanyId == FenXiaoUserContext.Current.UserInfo.Company.Id);
                    if (childProduct == null)
                    {
                        childProduct = new ChildProduct();
                        childProduct.CompanyId = FenXiaoUserContext.Current.UserInfo.CompanyId;
                        childProduct.ProductId = product.Id;
                        db.Entry<ChildProduct>(childProduct).State = System.Data.Entity.EntityState.Added;
                    }
                    else
                    {
                        db.Entry<ChildProduct>(childProduct).State = System.Data.Entity.EntityState.Modified;
                    }

                    //添加下单消息
                    var message = new FenXiao.Model.Message();
                    message.CreateTime = DateTime.Now;
                    message.ToCompanyId = product.User.CompanyId;
                    message.IsRead = 0;
                   message.RelatedId = of.Id;
                    message.State = (int)EnumMessage.ReserveNowOrder;
                    db.Entry<FenXiao.Model.Message>(message).State = System.Data.Entity.EntityState.Added;
                    db.SaveChanges();

                    ajaxResult.Ok = 200;
                    ajaxResult.Msg = "{ \"OrderFormId\":\"" + of.Id + "\", \"MessageId\":\"" + message.Id + "\" }";
                }
            }
            return Json(ajaxResult);
        }
        //报名成员填写信息页
        [HttpGet]
        public ActionResult CreateMember(int OrderFormId, int MessageId)
        {
            ViewBag.MessageId = MessageId;
            ViewBag.OrderFormId = OrderFormId;
            var of = db.OrderForms.FirstOrDefault(e => e.Id == OrderFormId);
            if (of == null)
                return HttpNotFound();
            
            db.Entry<OrderForm>(of).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            ViewBag.OrderForm = of;
            ViewBag.Product = of.Product;
            return View();
        }
        //ajax为订单添加一位成人
        [HttpPost]
        public ActionResult AddOneAdultAjax(int OrderFormId)
        {
            AjaxResult ajaxResult = new AjaxResult();
            var of = db.OrderForms.FirstOrDefault(e => e.Id == OrderFormId);
            if (of == null)
            {
                ajaxResult.Ok = 404;
                return Json(ajaxResult);
            }
            var childProduct = db.ChildProducts.FirstOrDefault(e=>e.ProductId == of.ProductId && e.CompanyId == FenXiaoUserContext.Current.UserInfo.CompanyId);
            if (childProduct == null)
            {
                ajaxResult.Ok = 404;
                return Json(ajaxResult);
            }
            lock (LockClass.obj)
            {
                if(of.Product.Count < 1)
                {
                    ajaxResult.Ok = 100;
                    return Json(ajaxResult);
                }
                //线路的人员数量减一
                of.Product.Count -= 1;
                db.Entry<Product>(of.Product).State = System.Data.Entity.EntityState.Modified;

                db.Entry<OrderForm>(of).State = System.Data.Entity.EntityState.Modified;

                //修改子线路的成人数
                
                db.Entry<ChildProduct>(childProduct).State = System.Data.Entity.EntityState.Modified;

                //添加下单消息
                var message = new FenXiao.Model.Message();
                message.CreateTime = DateTime.Now;
                message.ToCompanyId = of.Product.User.CompanyId;
                message.IsRead = 0;
                message.MessageContent = FenXiaoUserContext.Current.UserInfo.Company.CompanyName
                                                                + "增加了线路“" + of.Product.Name + "1个成人的占位订单";
                message.RelatedId = of.Id;
                message.State = (int)EnumMessage.ReserveNowOrder;
                db.Entry<FenXiao.Model.Message>(message).State = System.Data.Entity.EntityState.Added;

                db.SaveChanges();
                ajaxResult.Ok = 200;
                return Json(ajaxResult);
            }
        }
        //ajax为订单添加一位儿童
        [HttpPost]
        public ActionResult AddOneChildAjax(int OrderFormId)
        {
            AjaxResult ajaxResult = new AjaxResult();
            var of = db.OrderForms.FirstOrDefault(e => e.Id == OrderFormId);
            if (of == null)
            {
                ajaxResult.Ok = 404;
                return Json(ajaxResult);
            }
            var childProduct = db.ChildProducts.FirstOrDefault(e => e.ProductId == of.ProductId && e.CompanyId == FenXiaoUserContext.Current.UserInfo.CompanyId);
            if (childProduct == null)
            {
                ajaxResult.Ok = 404;
                return Json(ajaxResult);
            }
            lock (LockClass.obj)
            {
                if (of.Product.Count < 1)
                {
                    ajaxResult.Ok = 100;
                    return Json(ajaxResult);
                }
                //线路的人员数量减一
                of.Product.Count -= 1;
                db.Entry<Product>(of.Product).State = System.Data.Entity.EntityState.Modified;

                //修改订单的成人数
                db.Entry<OrderForm>(of).State = System.Data.Entity.EntityState.Modified;

                //修改子线路的成人数
                db.Entry<ChildProduct>(childProduct).State = System.Data.Entity.EntityState.Modified;

                //添加下单消息
                var message = new FenXiao.Model.Message();
                message.CreateTime = DateTime.Now;
                message.ToCompanyId = of.Product.User.CompanyId;
                message.IsRead = 0;
                message.MessageContent = FenXiaoUserContext.Current.UserInfo.Company.CompanyName
                                                                + "增加了线路“" + of.Product.Name + "1个儿童的占位订单";
                message.RelatedId = of.Id;
                message.State = (int)EnumMessage.ReserveNowOrder;
                db.Entry<FenXiao.Model.Message>(message).State = System.Data.Entity.EntityState.Added;

                db.SaveChanges();
                ajaxResult.Ok = 200;
                return Json(ajaxResult);
            }
        }
        //ajax保存成员名单
        [HttpPost]
        public ActionResult SaveMemberAjax()
        {
            AjaxResult ajaxResult = new AjaxResult();
            return Json(ajaxResult);
        }
        #endregion

        //当前线路的信息
        public ActionResult ProductInfo(int ProductId)
        {
            var product = db.Products.Find(ProductId);
            return PartialView(product);
        }
    }
}