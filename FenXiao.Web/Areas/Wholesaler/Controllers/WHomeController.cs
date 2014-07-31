using FenXiao.Model;
using FenXiao.Web.Areas.Marketer.Models;
using FenXiao.Web.Areas.Wholesaler.Models;
using FenXiao.Web.Common;
using FenXiao.Web.Extension;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace FenXiao.Web.Areas.Wholesaler.Controllers
{
    public class WHomeController : WholesalerControllerBase
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
        public ActionResult MyLuXian()
        {
            var Products = db.Products.Where(a => a.User.CompanyId == LoginInfo.CompanyId).ToList();
            return View(Products);
        }

        public ActionResult MySelledLuXian()
        {
            var Products = db.Products.Where(a => a.User.CompanyId == LoginInfo.CompanyId).ToList();
            return View(Products);
        }

        public ActionResult MyAllLuxian()
        {
            var Products = db.Products.Where(a => a.User.CompanyId == LoginInfo.CompanyId).ToList();
            return View(Products);
        }

        public ActionResult SellingLuXian(int? secho)
        {
            var query = db.Products.Where(a => a.User.CompanyId == LoginInfo.CompanyId&&
                a.State==(int)EnumProduct.zhengchang).Select(a => new 
            {
                a.Id,
                a.Name,
                a.SendGroupTime,
                price = a.ErTongPrice+"/"+a.ChengRenPrice,
                count= a.RemainCount+"/"+a.Count,
                a.State,
                haha=""
            });
            var objs = new List<object>();
            foreach (var city in query)
            {
                objs.Add(GetPropertyList(city).ToArray());
            }
            return Json(new
            {
                sEcho = secho,
                iTotalRecords = query.Count(),
                aaData = objs,
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SelledLuXian(int? secho)
        {
            var query = db.Products.Where(a => a.User.CompanyId == LoginInfo.CompanyId&&
                a.SendGroupTime<DateTime.Now&&a.State==(int)EnumProduct.zhengchang).Select(a => new
            {
                a.Id,
                a.Name,
                a.SendGroupTime,
                price = a.ErTongPrice + "/" + a.ChengRenPrice,
                count = a.RemainCount + "/" + a.Count,
                a.State,
                haha = ""
            });
            var objs = new List<object>();
            foreach (var city in query)
            {
                objs.Add(GetPropertyList(city).ToArray());
            }
            return Json(new
            {
                sEcho = secho,
                iTotalRecords = query.Count(),
                aaData = objs,
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AllLuXian(int? secho)
        {
            var query = db.Products.Where(a => a.User.CompanyId == LoginInfo.CompanyId).Select(a => new
            {
                a.Id,
                a.Name,
                a.SendGroupTime,
                price = a.ErTongPrice + "/" + a.ChengRenPrice,
                count = a.RemainCount + "/" + a.Count,
                a.State,
                haha = ""
            });
            var objs = new List<object>();
            foreach (var city in query)
            {
                objs.Add(GetPropertyList(city).ToArray());
            }
            return Json(new
            {
                sEcho = secho,
                iTotalRecords = query.Count(),
                aaData = objs,
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult MyLuXianDetail(int id)
        {
            var product = db.Products.FirstOrDefault(a => a.Id == id);
            if (product==null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        public string CancelLuXian(int id)
        {
            var pro = db.Products.Find(id);
            if (pro==null)
            {
                return JsonConvert.SerializeObject(new AjaxResult { Ok = 2,Msg="不存在此路线" });
            }
            if (pro.Count==pro.RemainCount)
            {
                pro.State = (int)EnumProduct.jinyong;
                db.Products.Attach(pro);
                db.Entry(pro).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                Searcher.Del(pro);
                return JsonConvert.SerializeObject(new AjaxResult { Ok = 1 });
            }
            else
            {
                return JsonConvert.SerializeObject(new AjaxResult { Ok = 0, Msg = "剩余数量必须等于卖出数量才能禁用" });
            }
        }

        public string RecoverLuXian(int id)
        {
            var pro = db.Products.Find(id);
            if (pro == null)
            {
                return JsonConvert.SerializeObject(new AjaxResult { Ok = 2, Msg = "不存在此路线" });
            }
                pro.State = (int)EnumProduct.zhengchang;
                db.Products.Attach(pro);
                db.Entry(pro).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                Searcher.Add(pro);
                return JsonConvert.SerializeObject(new AjaxResult { Ok = 1 });
        }

        public ActionResult CreateLuXian()
        {
            return View();
        }

        public ActionResult EditLuXian(int id)
        {
            var product = db.Products.FirstOrDefault(a => a.Id == id);
            if (product==null)
            {
                return HttpNotFound();
            }
            string type = "";
            foreach (var item in product.Product2Type.Select(a => a.TypeId).ToList())
            {
                type += item.ToString()+"+";
            }
            TempData["Type"] = type.Substring(0, type.Length - 1);
            TempData["TypeChoose"] = product.Product2Type.Select(a => a.TypeId).ToList();
            return View(product);
        }

        [ValidateInput(false)]
        [HttpPost]
        public string EditLuXian(EditLuXianModel elxm)
        {
            var product = db.Products.FirstOrDefault(a => a.Id == elxm.Id);
            if (product == null)
            {
                return JsonConvert.SerializeObject(new AjaxResult { Ok = 0, Msg = String.Format("找不到商品") });
            }
            if (product.RemainCount>elxm.Count)
            {
                return JsonConvert.SerializeObject(new AjaxResult { Ok=0,Msg=String.Format("数量不能小于已卖数量{0}",product.Count-product.RemainCount)});
            }
            product.FuJian = elxm.fujian;
            product.Count = elxm.Count;
            product.CreateTime = DateTime.Now;
            product.CreateUserId = LoginInfo.UserId;
            product.ErTongPrice = elxm.ertongprice;
            product.ChengRenPrice = elxm.chengrenprice;
            product.Tese = "";
            product.ZhuYiShiXiang = "";
            product.RemainCount = elxm.Count-(product.Count-product.RemainCount);
            product.TripContent = "";
            product.Name = elxm.name;
            product.SuggestionPrice = elxm.suggestprice;
            product.SendGroupTime = DateTime.Parse(elxm.data);
            product.Explain = elxm.shuoming;
            var v = db.Product2Type.Where(a => a.ProductId == product.Id);
            db.Product2Type.RemoveRange(v);
            foreach (var item1 in elxm.type.Split('+'))
            {
                db.Product2Type.Add(
                    new Product2Type
                    {
                        ProductId = product.Id,
                        TypeId = int.Parse(item1),

                    });
            }
            db.SaveChanges();
            Searcher.Del(product);
            Searcher.Add(product);
            return JsonConvert.SerializeObject(new AjaxResult {Ok=1 });
        }

        public ActionResult EditCount(int id)
        {
            var pro = db.Products.Find(id);
            return View(pro);
        }

        [HttpPost]
        public ActionResult EditCount(int count,int id)
        {
            var pro = db.Products.Find(id);
            if (pro.User.CompanyId!=LoginInfo.CompanyId)
            {
                return HttpNotPermission();
            }
            if ((pro.Count-pro.RemainCount)>count)
            {
                ViewBag.error = "修改数量不能低于卖出数量!";
                return View();
            }
            pro.RemainCount = count - (pro.Count - pro.RemainCount);
            pro.Count = count;
            db.Entry(pro).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("MyLuXian");
        }

        public ActionResult LineTypePage(List<int> Choose, string type)
        {
            var ProTypes = db.LuXianTypes.
                Where(a => a.IsDelete == 0).
                GroupBy(a => a.TypeTag.Substring(0, 2)).ToList();

            return View(new LineTypeModel { type = type, Choose = Choose, LuXianTypes = ProTypes });
        }

        public ActionResult LuXianType()
        {
            var groupLunXian = from a in db.LuXianTypes
                               where a.IsDelete == 0
                               group a by a.TypeTag.Substring(0, 2)
                                   into b
                                   select new LuXianTypeModel { Key = b.Key,groups = b};
            return View(groupLunXian.ToList());
        }

        public ActionResult LuXianmanagement(int ProductId)
        {
            var pro = db.Products.FirstOrDefault(a => a.Id == ProductId && a.User.CompanyId == LoginInfo.CompanyId);
            if (pro==null)
            {
                return HttpNotFound();
            }
            return View(pro);
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult CreateLuXian(CreateLuXianModel clxm)
        {
            if (string.IsNullOrEmpty(clxm.name)||
                string.IsNullOrEmpty(clxm.type)||
                string.IsNullOrEmpty(clxm.data))
            {
                return View(clxm);
            }
            var datas = clxm.data.Split('+');
            foreach (var item in datas)
            {
                if (string.IsNullOrEmpty(clxm.fujian))
                {
                    clxm.fujian = "";
                }
                var product = new Product
                {
                    Count = clxm.Count,
                    CreateTime = DateTime.Now,
                    FuJian = clxm.fujian,
                    Explain = clxm.shuoming,
                    CreateUserId = LoginInfo.UserId,
                    Name = clxm.name,
                    ChengRenPrice = clxm.chengrenprice,
                    ErTongPrice = clxm.ertongprice,
                    SendGroupTime = Convert.ToDateTime(item),
                    State = (int)EnumProduct.zhengchang,
                    SuggestionPrice = clxm.suggestprice,
                    Tese = "",
                    ZhuYiShiXiang = "",
                    RemainCount = clxm.Count,
                    TripContent = ""
                    //TripContent = clxm.xingcheng,
                };
                db.Products.Add(product);
                db.SaveChanges();
                Searcher.Add(product);
                foreach (var item1 in clxm.type.Split('+'))
                {
                    db.Product2Type.Add(
                        new Product2Type 
                        {
                            ProductId = product.Id,
                            TypeId = int.Parse(item1),
                            
                        });
                }
                db.SaveChanges();
            }
            return Content("1");
        }


        public ActionResult HandleOrder(int id)
        {
            var order = db.OrderForms.FirstOrDefault(a => a.Id == id);
            if (order == null)
            {
                return HttpNotFound();
            }
            if (order.State == (int)EnumOrderForm.xiadingdan)
            {
                lock (LockClass.objOrder)
                {
                    if (order.State == (int)EnumOrderForm.xiadingdan)
                    {
                        order.State = (int)EnumOrderForm.chulidingdan;
                        db.OrderForms.Attach(order);
                        db.Entry(order).State = System.Data.Entity.EntityState.Modified;
                        db.HandleOrderForms.Add(new HandleOrderForm { CreateTime = DateTime.Now, OrderFormId = order.Id, UserId = LoginInfo.UserId });
                        db.SaveChanges();
                        db.Messages.Add(new FenXiao.Model.Message 
                        {
                            CreateTime = DateTime.Now,
                            IsRead = 0,
                            RelatedId = order.Id,
                            State = (int)EnumMessage.chulidingdan,
                            MessageContent = string.Format("订单{0}儿童{1}，成人{2}个已被处理",order.Product.Name,order.ErTongCount,order.ChengRenCount),
                            ToCompanyId = order.User.CompanyId
                        });
                        db.SaveChanges();
                    }
                }
            }
            return RedirectToAction("LuXianmanagement", new { ProductId=order.ProductId });
        }

        public ActionResult OrderDetial(int id)
        {
            var order = db.OrderForms.Find(id);
            if (order==null)
	        {
                return HttpNotFound();
	        }
            return View(order);
        }

        public ActionResult ReturnOrderDetial(int id)
        {
            var reor = db.ReturnForms.Find(id);
            if (reor==null)
            {
                return HttpNotFound();
            }
            return View(reor);
        }
        public ActionResult HandleReturnPage(int id, string state)
        {
            var rm = db.ReturnForms.Find(id);
            if (rm==null)
            {
                return HttpNotFound();
            }
            if (state=="ok")
            {
                ViewBag.State = "通过";
            }
            else
            {
                ViewBag.State = "拒绝";
            }
            ViewBag.inputstr = state;
            return View(rm);
        }

        public ActionResult HandleReturnOrder(int id, string state, string note)
        {

            var ReturnForm = db.ReturnForms.Find(id);
            if (ReturnForm==null)
            {
                return HttpNotFound();
            }
            if (state == "ok")
            {
                lock (LockClass.obj)
                {
                    if ((int)EnumReturnForm.xiatuidan == ReturnForm.State)
                    {
                        ReturnForm.State = (int)EnumReturnForm.chulidingdan;
                        db.ReturnForms.Attach(ReturnForm);
                        db.Entry(ReturnForm).State = System.Data.Entity.EntityState.Modified;
                        var cp = db.ChildProducts.FirstOrDefault(a => a.ProductId == ReturnForm.ProductId);
                        if (cp == null)
                        {
                            return HttpNotFound();
                        }
                        else
                        {
                            cp.ErTongCount -= ReturnForm.ErTongCount;
                            cp.ChengRenCount -= ReturnForm.ChengRenCount;
                            db.ChildProducts.Attach(cp);
                            db.Entry(cp).State = System.Data.Entity.EntityState.Modified;
                        }
                        db.HandleReturnForms.Add(
                            new HandleReturnForm
                            {
                                CreateTime = DateTime.Now,
                                Reason = note,
                                ReturnFormId = ReturnForm.Id,
                                UserId = LoginInfo.UserId
                            });
                        db.Messages.Add(new FenXiao.Model.Message
                        {
                            CreateTime = DateTime.Now,
                            IsRead = 0,
                            RelatedId = ReturnForm.Id,
                            State = (int)EnumMessage.chulituidan,
                            MessageContent = string.Format("退订单{0}儿童{1}，成人{2}个已被通过",
                            ReturnForm.Product.Name, ReturnForm.ErTongCount, ReturnForm.ChengRenCount),
                            ToCompanyId = ReturnForm.User.CompanyId
                        });
                        db.SaveChanges();
                       
                    }
                }
            }
            else
            {
                lock (LockClass.obj)
                {
                    if ((int)EnumReturnForm.xiatuidan == ReturnForm.State)
                    {
                        ReturnForm.State = (int)EnumReturnForm.quxiaodingdan;
                        db.ReturnForms.Attach(ReturnForm);
                        db.Entry(ReturnForm).State = System.Data.Entity.EntityState.Modified;
                        db.HandleReturnForms.Add(
                            new HandleReturnForm
                            {
                                CreateTime = DateTime.Now,
                                Reason = note,
                                ReturnFormId = ReturnForm.Id,
                                UserId = LoginInfo.UserId
                            });
                        db.Messages.Add(new FenXiao.Model.Message
                        {
                            CreateTime = DateTime.Now,
                            IsRead = 0,
                            RelatedId = ReturnForm.Id,
                            State = (int)EnumMessage.chulituidan,
                            MessageContent = string.Format("退订单{0}儿童{1}，成人{2}个已被拒绝",
                            ReturnForm.Product.Name, ReturnForm.ErTongCount, ReturnForm.ChengRenCount),
                            ToCompanyId = ReturnForm.User.CompanyId
                        });
                        db.SaveChanges();
                    }
                }
            }
            return RedirectToAction("LuXianmanagement", new { ProductId = ReturnForm.ProductId });
        }

        #region  成员管理

        public ActionResult CompanyInfo()
        {
            var userlist = db.Users.Where(a => a.CompanyId == LoginInfo.CompanyId).ToList();
            var user = userlist.FirstOrDefault(a => a.Role.Split('+').Contains(((int)EnumRole.pifa).ToString()));
            if (user != null)
            {
                ViewBag.Name = user.Name;
            }
            return View(userlist);
        }
        public ActionResult Member()
        {
            var userlist = db.Users.Where(a => a.CompanyId == LoginInfo.CompanyId).ToList();
            return View(userlist);
        }
        public ActionResult CreateMember(string CompanyMU, 
            string Count)
        {
            ViewBag.CompanyMU = CompanyMU;
            ViewBag.PeopleCount = Count;
            TempData["PeopleCount"] = Count;
            return View();
        }

        public ActionResult CreateMemberNew()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateMember(CreateMemberModel cmm)
        {
            var user = db.Users.FirstOrDefault(a => a.Email == cmm.Email);
            if (user != null)
            {
                ViewBag.error = "此邮箱已存在";
                return View(cmm);
            }
            db.Users.Add(new User
            {
                CompanyId = LoginInfo.CompanyId,
                CreateTime = DateTime.Now,
                Role = ((int)EnumRole.zipifa).ToString(),
                Email = cmm.Email,
                ImageUrl = "",
                Name = cmm.Name,
                Password = cmm.Password,
                State = (int)EnumUser.zhengchang,
                Phone = cmm.Phone,
            });
            db.SaveChanges();
            return RedirectToAction("Member");
        }
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
        public ActionResult EditMember(EditMemberModel emm)
        {
            var user = db.Users.Find(emm.Id);
            if (user == null)
            {
                return HttpNotFound();
            }
            user.Name = emm.Name;
            user.Phone = emm.Phone;
            user.Email = emm.Email;
            db.Users.Attach(user);
            db.Entry(user).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Member");
        }

        public ActionResult ManageMember(string type, int id)
        {
            var user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            if (LoginInfo.CompanyId != user.CompanyId || !LoginInfo.Role.Contains((int)EnumRole.pifa))
            {
                return HttpNotPermission();
            }
            if (user.Role.Split('+').Contains(((int)EnumRole.pifa).ToString()))
            {
                return RedirectToAction("Member");
            }
            if (type == "dongjie")
            {
                user.State = (int)EnumUser.dongjie;
            }
            else
            {
                user.State = (int)EnumUser.zhengchang;
            }
            db.Users.Attach(user);
            db.Entry(user).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Member");
        }

        public ActionResult ReplyToLingShou()
        {
            return View();
        }

        public ActionResult SubmitReply()
        {
            if (!UserContext.UserInfo.Company.
                 CompanyRole.Split('+').
                 Contains(((int)EnumCompany.zanshilingshou).ToString()))
            {
                lock (LockClass.objApplyToLingShou)
                {
                    if (!UserContext.UserInfo.Company.
                     CompanyRole.Split('+').
                     Contains(((int)EnumCompany.zanshilingshou).ToString()))
                    {
                        db.Applies.Add(new Apply
                        {
                            ApplyRole = (int)EnumCompany.lingshou,
                            CompanyId = UserContext.UserInfo.CompanyId,
                            CreateTime = DateTime.Now,
                            State = (int)EnumApply.Applying
                        });
                        var com = db.Companies.Find(UserContext.UserInfo.CompanyId);
                        com.CompanyRole = ((int)EnumCompany.pifa) + "+" + ((int)EnumCompany.zanshilingshou);
                        db.Companies.Attach(com);
                        db.Entry(com).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                    }
                }
            }
            return RedirectToAction("Member");
        }
        #endregion
    }
}