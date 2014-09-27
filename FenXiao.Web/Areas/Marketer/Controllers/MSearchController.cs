﻿using FenXiao.Model;
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

        #region 立即占位（还未加锁）
        //填写占位人数页
        [HttpGet]
        public ActionResult ReserveNow(int ProductId)
        {
            ViewBag.ProductId = ProductId;
            return View();
        }
        //ajax提交人数
        [HttpPost]
        public ActionResult ReserveNowAjax(int Id, int AllCount)
        {
            //异步结果
            AjaxResult ajaxResult = new AjaxResult();
            var product = db.Products.Find(Id);
            if (product == null)
            {
                ajaxResult.Ok = 404;
            }
            else if (product.RemainCount < AllCount)
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
                product.RemainCount -= AllCount;
                db.Entry<Product>(product).State = System.Data.Entity.EntityState.Modified;

                //添加订单
                OrderForm of = new OrderForm();
                of.State = (int)EnumOrderForm.ReserveNowOrder;
                of.ProductId = product.Id;
                of.CreateUserId = FenXiaoUserContext.Current.UserInfo.Id;
                of.CreateTime = DateTime.Now;
                of.ToCompanyId = product.User.CompanyId;
                of.Name = product.Name;
                of.AllCount = AllCount;
                of.Note = "";
                db.Entry<OrderForm>(of).State = System.Data.Entity.EntityState.Added;

                //更新子线路的数量
                var childProduct = db.ChildProducts.FirstOrDefault(e => e.ProductId == product.Id && e.CompanyId == FenXiaoUserContext.Current.UserInfo.Company.Id);
                if (childProduct == null)
                {
                    //不存在的话新建一个子线路
                    childProduct = new ChildProduct();
                    childProduct.CompanyId = FenXiaoUserContext.Current.UserInfo.CompanyId;
                    childProduct.ProductId = product.Id;
                    childProduct.AllCount = AllCount;
                    childProduct.EditCount = 0;
                    childProduct.ZhanWeiCount = AllCount;
                    db.Entry<ChildProduct>(childProduct).State = System.Data.Entity.EntityState.Added;
                }
                else
                {
                    //存在的话修改子线路信息
                    childProduct.AllCount += AllCount;
                    childProduct.ZhanWeiCount += AllCount;
                    db.Entry<ChildProduct>(childProduct).State = System.Data.Entity.EntityState.Modified;
                }
                //将线路信息、子线路信息、订单信息保存到数据库
                db.SaveChanges();

                //添加占位下单消息
                string content =FenXiaoUserContext.Current.UserInfo.Company.CompanyName
                                                                + "下了线路“" + product.Name + "”共" + AllCount + "人的占位订单";
                MessageHelper.Add(of.Id, product.User.CompanyId, content, EnumMessage.ReserveNowOrder);

                //所有操作正常，返回状态200
                ajaxResult.Ok = 200;
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
        public ActionResult DirectApplyOrderAjax(int Id, int AllCount)
        {
            //异步结果
            AjaxResult ajaxResult = new AjaxResult();
            var product = db.Products.Find(Id);
            if (product == null)
            {
                ajaxResult.Ok = 404;
            }
            else if (product.RemainCount < AllCount)
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
                product.RemainCount -= AllCount;
                db.Entry<Product>(product).State = System.Data.Entity.EntityState.Modified;

                //添加订单
                OrderForm of = new OrderForm();
                of.State = (int)EnumOrderForm.DirectApplyOrderEditing;
                of.ProductId = product.Id;
                of.CreateUserId = FenXiaoUserContext.Current.UserInfo.Id;
                of.CreateTime = DateTime.Now;
                of.ToCompanyId = product.User.CompanyId;
                of.Name = product.Name;
                of.AllCount = AllCount;
                of.Note = "";
                db.Entry<OrderForm>(of).State = System.Data.Entity.EntityState.Added;

                //更新子线路的数量
                var childProduct = db.ChildProducts.FirstOrDefault(e => e.ProductId == product.Id && e.CompanyId == FenXiaoUserContext.Current.UserInfo.Company.Id);
                if (childProduct == null)
                {
                    //不存在的话新建一个子线路
                    childProduct = new ChildProduct();
                    childProduct.CompanyId = FenXiaoUserContext.Current.UserInfo.CompanyId;
                    childProduct.ProductId = product.Id;
                    childProduct.AllCount = AllCount;
                    childProduct.EditCount = AllCount;
                    childProduct.ZhanWeiCount = 0;
                    db.Entry<ChildProduct>(childProduct).State = System.Data.Entity.EntityState.Added;
                }
                else
                {
                    //存在的话修改子线路信息
                    childProduct.AllCount += AllCount;
                    childProduct.EditCount += AllCount;
                    db.Entry<ChildProduct>(childProduct).State = System.Data.Entity.EntityState.Modified;
                }
                //将线路信息、子线路信息、订单信息保存到数据库
                db.SaveChanges();

                //添加占位下单消息
                string content = FenXiaoUserContext.Current.UserInfo.Company.CompanyName
                                                                + "下了线路“" + product.Name + "”共" + AllCount + "人的直接订单，正在编辑";
                MessageHelper.Add(of.Id, product.User.CompanyId, content, EnumMessage.DirectApplyOrderEditing);                

                //所有操作正常，返回状态200
                ajaxResult.Ok = 200;
                ajaxResult.Msg = "{\"OrderFormId\":"+of.Id+"}";
            }
            return Json(ajaxResult);
        }

        //报名成员填写信息页
        [HttpGet]
        public ActionResult Member(int OrderFormId)
        {
            var of = db.OrderForms.FirstOrDefault(e => e.Id == OrderFormId);
            if (of == null)
                return HttpNotFound();
            return View(of);
        }

        //ajax保存成员名单
        [HttpPost]
        public ActionResult SaveMemberAjax()
        {
            AjaxResult ajaxResult = new AjaxResult();
            return Json(ajaxResult);
        }
        #endregion

        #region 当前线路的信息
        public ActionResult ProductInfo(int ProductId)
        {
            var product = db.Products.Find(ProductId);
            return PartialView(product);
        }
        #endregion
    }
}