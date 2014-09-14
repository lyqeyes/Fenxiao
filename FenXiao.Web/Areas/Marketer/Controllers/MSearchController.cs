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
        //搜索页
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

        //详情页
        [HttpGet]
        public ActionResult LineDetail(int Id)
        {
            var product = db.Products.Find(Id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }
        //下订单
        [HttpGet]
        public ActionResult CreateOrder(int ProductId)
        {
            var product = db.Products.Find(ProductId);
            return View(product);
        }
        [HttpPost]
        public string CreateOrder(int Id, int ChengRenCount, int ErTongCount)
        {
            //TODO 批发商修改订单与零售商订单间的冲突
            //TODO 零售商与零售商间的冲突
            lock (LockClass.obj)
            {
                var product = db.Products.Find(Id);
                if (product == null)
                {
                    HttpNotFound();
                }
                else if (product.RemainCount < ErTongCount + ChengRenCount)
                {
                    return "100";
                }
                else if (product.State == (int)EnumProduct.jinyong)
                {
                    return "300";
                }
                else
                {
                    product.RemainCount -= (ErTongCount + ChengRenCount);
                    db.Entry<Product>(product).State = System.Data.Entity.EntityState.Modified;
                    OrderForm of = new OrderForm();
                    of.ChengRenCount = ChengRenCount;
                    of.ChengRenPrice = product.ChengRenPrice;
                    of.ErTongCount = ErTongCount;
                    of.ErTongPrice = product.ErTongPrice;
                    of.State = (int)EnumOrderForm.xiadingdan;
                    of.ProductId = product.Id;
                    of.CreateUserId = FenXiaoUserContext.Current.UserInfo.Id;
                    of.CreateTime = DateTime.Now;
                    of.TotalPrice = of.ErTongPrice * of.ErTongCount + of.ChengRenCount * of.ChengRenPrice;
                    of.ToCompanyId = product.User.CompanyId;
                    //TODO 没有信息吗
                    of.Name = product.Name;
                    of.Note = "";
                    db.Entry<OrderForm>(of).State = System.Data.Entity.EntityState.Added;
                    db.SaveChanges();
                    var childProduct = db.ChildProducts.FirstOrDefault(e => e.ProductId == product.Id);
                    if (childProduct == null)
                    {
                        childProduct = new ChildProduct();
                        childProduct.ErTongCount = of.ErTongCount;
                        childProduct.ChengRenCount = of.ChengRenCount;
                        childProduct.CompanyId = FenXiaoUserContext.Current.UserInfo.CompanyId;
                        childProduct.ProductId = product.Id;
                        db.Entry<ChildProduct>(childProduct).State = System.Data.Entity.EntityState.Added;
                    }
                    else
                    {
                        childProduct.ErTongCount += of.ErTongCount;
                        childProduct.ChengRenCount += of.ChengRenCount;
                        db.Entry<ChildProduct>(childProduct).State = System.Data.Entity.EntityState.Modified;
                    }
                    var message = new FenXiao.Model.Message();
                    message.CreateTime = DateTime.Now;
                    message.ToCompanyId = product.User.CompanyId;
                    message.IsRead = 0;
                    message.MessageContent = FenXiaoUserContext.Current.UserInfo.Company.CompanyName
                                                                    + "下了线路“" + product.Name + "”共" + (of.ChengRenCount + of.ErTongCount).ToString() + "人的票";
                    message.RelatedId = of.Id;
                    message.State = (int)EnumMessage.xiadingdan;
                    db.Entry<FenXiao.Model.Message>(message).State = System.Data.Entity.EntityState.Added;
                    db.SaveChanges();

                    //创建相关成员列表
                    string str = Request.Form["CustomerList"];
                    var TempList = JsonConvert.DeserializeObject<List<CustomerTempModel>>(str);
                    var customerInfoList = new List<CustomerInfo>();
                    CustomerInfo customerInfo;
                    foreach (var item in TempList)
                    {
                        customerInfo = new CustomerInfo();
                        customerInfo.IsDelete = (int)EnumCustomer.zhengchang;
                        customerInfo.ChildProductId = childProduct.Id;
                        customerInfo.OrderId = of.Id;
                        customerInfo.CreateUserId = FenXiaoUserContext.Current.UserInfo.Id;
                        customerInfo.Name = item.Name;
                        customerInfo.Email = item.Email;
                        customerInfo.Phone = item.Phone;
                        if (item.Sex == "男")
                            customerInfo.Sex = 0;
                        else
                            customerInfo.Sex = 1;
                        customerInfoList.Add(customerInfo);
                    }
                    db.CustomerInfoes.AddRange(customerInfoList);
                    db.SaveChanges();
                    return "200";
                }
                return "0";
            }
        }
    }
}