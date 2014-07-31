using FenXiao.Model;
using FenXiao.Web.Common;
using FenXiao.Web.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FenXiao.Web.Areas.Marketer.Models;
using System.Data.Entity.Core.Objects;
using System.Data.Entity;
using Webdiyer.WebControls.Mvc;
using Newtonsoft.Json; 
namespace FenXiao.Web.Areas.Marketer.Controllers
{
    public class MHomeController : MarketerControllerBase
    {
        #region 线路检索
        //搜索页
        [HttpGet]
        public ActionResult LineSearch(string type = null, DateTime? from = null, DateTime? to = null, int id = 0)
        {
            if (from==null)
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
                        var v = db.Product2Type.Select(a => a.Product).Distinct().Where(a => a.State == (int)EnumProduct.zhengchang).OrderByDescending(a => a.Id).ToPagedList(id,PageSize);
                        return View(new LineSearchModel { type = choosetype, Choose = choose, res = v });
                    }
                    else
                    {
                        var v = db.Product2Type.Where(a => choose2.Contains(a.TypeId)).Select(a => a.Product).Distinct().Where(a => a.State == (int)EnumProduct.zhengchang).OrderByDescending(a => a.Id).ToPagedList(id,PageSize);
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
                    return View(new LineSearchModel { type = choosetype, Choose = choose, res = v ,from = from.ToString(),to = to.ToString()});
                }
                else
                {
                    var choose2 = choose.Except(list);
                    if (choose2.Count() == 0)
                    {
                        var v = db.Product2Type.Select(a => a.Product).Distinct().Where(a => a.State == (int)EnumProduct.zhengchang
                            && a.SendGroupTime > from && a.SendGroupTime < to).OrderByDescending(a => a.Id).ToPagedList(id,PageSize);
                        return View(new LineSearchModel { type = choosetype, Choose = choose, res = v, from = from.ToString(), to = to.ToString() });
                    }
                    else
                    {
                        var v = db.Product2Type.Where(a => choose2.Contains(a.TypeId)).Select(a => a.Product).Distinct().Where(a => a.State == (int)EnumProduct.zhengchang
                            && a.SendGroupTime > from && a.SendGroupTime < to).OrderByDescending(a => a.Id).ToPagedList(id,PageSize);
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
                    foreach(var item in TempList)
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
        #endregion

        #region 线路管理
        //线路管理首页
        [HttpGet]
        public ActionResult Line(int id=0)
        {
            ViewBag.IsOk = db.ChildProducts.Count(e => e.CompanyId == FenXiaoUserContext.Current.UserInfo.CompanyId);
            var list = db.ChildProducts.Where(e =>
                e.CompanyId == FenXiaoUserContext.Current.UserInfo.CompanyId).OrderByDescending(a=>a.Id).ToPagedList(id,PageSize);
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

        #region 公司管理
        //内容头部的公司信息
        [HttpGet]
        public ActionResult CompanyInfo()
        {
            ViewBag.Count = db.Users.Count(e => e.CompanyId == FenXiaoUserContext.Current.UserInfo.CompanyId) - 1;
            ViewBag.IsAll = FenXiaoUserContext.Current.UserInfo.Role.Split('+').Contains(((int)EnumRole.lingshou).ToString());
            var userList = db.Users.Where(e => e.CompanyId == FenXiaoUserContext.Current.UserInfo.CompanyId);
            foreach (var user in userList)
            {
                if (user.Role.Split('+').Contains(((int)EnumRole.lingshou).ToString()))
                {
                    ViewBag.Name = user.Name;
                    break;
                }
            }
            return PartialView();
        }

        //公司管理首页
        [HttpGet]
        public ActionResult Member()
        {
            //TODO  一个公司有经销商与零售商两种身份怎么去处理管理员的显示？
            var userlist = db.Users.Where(a => a.CompanyId == LoginInfo.CompanyId).ToList();
            return View(userlist);
        }

        //创建子账户
        [HttpGet]
        public ActionResult CreateMember()
        {
            return View();
        }
        [HttpPost]

        public ActionResult CreateMember([Bind(Include = "Password,Name,Phone,Email")]User newUser)
        {
            var user = db.Users.FirstOrDefault(a => a.Email == newUser.Email);
            if (user != null)
            {
                //TODO 没有处理异常
                ViewBag.error = "此邮箱已存在";
                return View(newUser);
            }
            db.Users.Add(new User
            {
                CompanyId = LoginInfo.CompanyId,
                CreateTime = DateTime.Now,
                Role = ((int)EnumRole.zilingshou).ToString(),
                Email = newUser.Email,
                ImageUrl = "",
                Name = newUser.Name,
                Password = newUser.Password,
                State = (int)EnumUser.zhengchang,
                Phone = newUser.Phone,
            });
            db.SaveChanges();
            return RedirectToAction("Member");
        }
        //编辑子账户
        [HttpGet]
        public ActionResult EditMember(int id)
        {
            var user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }
        [HttpPost]
        public ActionResult EditMember([Bind(Include = "Password,Name,Phone,Email,Id")]User newUser)
        {
            var user = db.Users.Find(newUser.Id);
            if (user == null)
            {
                return HttpNotFound();
            }
            user.Name = newUser.Name;
            user.Phone = newUser.Phone;
            user.Email = newUser.Email;
            user.Password = newUser.Password;
            db.Entry<User>(user).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Member");
        }

        //修改子账户状态
        [HttpGet]
        public ActionResult EditMemberState(int id, string Type)
        {
            var user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            switch (Type)
            {
                case "恢复":
                    {
                        user.State = (int)EnumUser.zhengchang;
                        break;
                    }
                case "冻结":
                    {
                        user.State = (int)EnumUser.dongjie;
                        break;
                    }
                default:
                    {
                        return HttpNotFound();
                    }
            }
            db.Entry<User>(user).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Member");
        }

        //修改公司信息
        //TODO 缺少修改公司信息页面
        [HttpGet]
        public ActionResult EditCompanyInfo(int Id)
        {
            return View();
        }
        [HttpPost]
        public ActionResult EditCompanyInfo(Company company)
        {
            return View();
        }

        //申请成为批发商
        [HttpGet]
        public ActionResult ApplyPiFa()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ApplyPiFa(string LvXingSheZeRenXian, string RenShenYiWaiXian, string AjiLuXingShe, string RongYuChengHao)
        {
            lock (LockClass.objApplyToPiFa)
            {
                if (FenXiaoUserContext.Current.UserInfo.Company.CompanyRole.Contains(((int)EnumCompany.zanshipifa).ToString()))
                {
                    return View();
                }
                var apply = new Apply();
                apply.LvXingSheZeRenXian = LvXingSheZeRenXian;
                apply.RenShenYiWaiXian = RenShenYiWaiXian;
                apply.AjiLuXingShe = AjiLuXingShe;
                apply.RongYuChengHao = RongYuChengHao;
                apply.CreateTime = DateTime.Now;
                apply.CompanyId = FenXiaoUserContext.Current.UserInfo.CompanyId;
                apply.ApplyRole = (int)EnumCompany.pifa;
                apply.State = (int)EnumApply.Applying;
                db.Entry<Apply>(apply).State = System.Data.Entity.EntityState.Added;
                FenXiaoUserContext.Current.UserInfo.Company.CompanyRole += ("+" + ((int)EnumCompany.zanshipifa).ToString());
                db.Entry<Company>(FenXiaoUserContext.Current.UserInfo.Company).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Member");
        }
        #endregion

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
    }
}