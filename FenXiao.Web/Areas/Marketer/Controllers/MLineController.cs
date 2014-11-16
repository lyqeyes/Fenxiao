using FenXiao.Model;
using FenXiao.Web.Common;
using FenXiao.Web.Extension;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;

namespace FenXiao.Web.Areas.Marketer.Controllers
{
    public class MLineController : MarketerControllerBase
    {
        //线路管理首页
        [HttpGet]
        public ActionResult Line(int id = 0)
        {
            var list = db.ChildProducts.Where(e =>
                e.CompanyId == FenXiaoUserContext.Current.UserInfo.CompanyId).OrderByDescending(a => a.Id).ToPagedList(id, PageSize);
            return View(list);
        }
        [HttpGet]
        public ActionResult LineByKey(string ProductName,int id = 0)
        {
            var res = Searcher.Search(ProductName);
            var list = db.ChildProducts.Where(e =>
               e.CompanyId == FenXiaoUserContext.Current.UserInfo.CompanyId && res.Contains(e.ProductId)).OrderByDescending(a => a.Id).ToPagedList(id, PageSize);
            return View(list);
        }
        [HttpGet]
        public ActionResult LineByRange(DateTime? From = null, DateTime? To = null, int PorductId = -1,int id = 0)
        {
            ViewBag.From = From;
            ViewBag.To = To;
            ViewBag.PorductId = PorductId;
            if(From != null && To != null)
            {
                var list = db.ChildProducts.Where(e =>
               e.CompanyId == FenXiaoUserContext.Current.UserInfo.CompanyId && e.Product.SendGroupTime > From && e.Product.SendGroupTime < To).OrderByDescending(a => a.Id).ToPagedList(id, PageSize);
                return View(list);
            }
            else if(PorductId != -1)
            {
                var list = db.ChildProducts.Where(e =>
               e.CompanyId == FenXiaoUserContext.Current.UserInfo.CompanyId && e.ProductId == PorductId).OrderByDescending(a => a.Id).ToPagedList(id, PageSize);
                return View(list);
            }
            else
            {
                return RedirectToAction("Line", "MLine", new { Area = "Marketer" });
            }            
        }
        //订单页
        [HttpGet]
        public ActionResult Order(int Id)
        {
            var childProduct = db.ChildProducts.Find(Id);
            ViewBag.ChildProduct = childProduct;
            this.PermissionCompanyId = childProduct.CompanyId;
            return View();
        }

        #region 发起退单
        [HttpGet]
        public ActionResult ReturnOrder(int Id)
        {
            ViewBag.ChildProduct = db.ChildProducts.Find(Id);
            ViewBag.ZhanWeiCount = ViewBag.ChildProduct.ZhanWeiCount;
            ViewBag.list = db.CustomerInfoes.Where(e => e.ChildProductId == Id && e.State == (int)EnumCustomer.ZhengChang).ToList();
            this.PermissionCompanyId = ViewBag.ChildProduct.CompanyId;
            return View();
        }
        [HttpPost]
        public string ReturnOrder(int ProductId, int ChildProductId, int AllCount)
        {
            lock (LockClass.GetChildProductLock(ChildProductId))
            {
                var product = db.Products.Find(ProductId);
                var childProduct = db.ChildProducts.Find(ChildProductId);
                if (product == null || childProduct == null)
                {
                    HttpNotFound();
                }
                else if((childProduct.ZhanWeiCount-childProduct.ZhanWeiLockCount) < AllCount)
                {
                    return "100";
                }
                else
                {
                    //产生退单
                    ReturnForm rf = new ReturnForm();
                    rf.State = (int)EnumReturnForm.xiatuidan;
                    rf.ProductId = product.Id;
                    rf.CreateUserId = FenXiaoUserContext.Current.UserInfo.Id;
                    rf.CreateTime = DateTime.Now;
                    rf.ToCompanyId = product.User.CompanyId;
                    rf.AllCount = AllCount;
                    rf.Name = product.Name;
                    rf.CustomerList = Request.Form["CustomerIdList"];
                    rf.Note = Request.Form["Reason"];
                    db.Entry<ReturnForm>(rf).State = System.Data.Entity.EntityState.Added;
                    int Temp = 0;
                    //修改成员状态
                    if (rf.CustomerList != null && rf.CustomerList != "")
                    {
                        foreach (var Id in rf.CustomerList.Split(','))
                        {
                            Temp++;
                            var customerInfo = db.CustomerInfoes.Find(int.Parse(Id));
                            customerInfo.State = (int)EnumCustomer.DaiShanChu;
                            db.Entry<CustomerInfo>(customerInfo).State = System.Data.Entity.EntityState.Modified;
                        }
                    }
                    childProduct.ZhanWeiLockCount += (AllCount-Temp);
                    childProduct.ZhanWeiCount -= (AllCount - Temp);
                    db.SaveChanges();
                    var message = new FenXiao.Model.Message();
                    message.CreateTime = DateTime.Now;
                    message.ToCompanyId = product.User.CompanyId;
                    message.IsRead = 0;
                    message.MessageContent = FenXiaoUserContext.Current.UserInfo.Company.CompanyName
                                                                    + "退了线路“" + product.Name + "”共" + (rf.AllCount).ToString() + "人的票";
                    message.RelatedId = rf.Id;
                    message.State = (int)EnumMessage.xiatuidan;
                    db.Entry<FenXiao.Model.Message>(message).State = System.Data.Entity.EntityState.Added;
                    db.SaveChanges();
                    return "200";
                }
                return "0";
            }
        }

        #endregion 

        #region 订单页
        //正常的订单
        [HttpGet]
        public ActionResult CommonOrder(int Id)
        {
            var list = db.OrderForms.Where(e => e.ProductId == Id).OrderByDescending(e => e.CreateTime).ToList(); 
            return PartialView(list);
        }

        //正常订单详情页
        [HttpGet]
        public ActionResult CommonOrderDetail(int Id)
        {
            var orderForm = db.OrderForms.Find(Id);
            return View(orderForm);
        }
        #endregion

        #region 退单页
        //已取消的订单
        [HttpGet]
        public ActionResult CancelOrder(int Id)
        {
            var list = db.ReturnForms.Where(e => e.ProductId == Id).OrderByDescending(e => e.CreateTime).ToList();
            return PartialView(list);
        }

        //已取消的订单详情页
        [HttpGet]
        public ActionResult CancelOrderDetail(int Id)
        {
            var returnForm = db.ReturnForms.Find(Id);
            return View(returnForm);
        }      
        #endregion

        #region 成员页
        //人员管理
        [HttpGet]
        public ActionResult OrderPeople(int Id)
        {
            ViewBag.Id = Id;
            ViewBag.ZhanWeiCount = db.ChildProducts.Find(Id).ZhanWeiCount;
            ViewBag.ZhanWeiLockCount = db.ChildProducts.Find(Id).ZhanWeiLockCount;
            var list = db.CustomerInfoes.Where(e => e.ChildProductId == Id && e.State == (int)EnumCustomer.ZhengChang).ToList();
            ViewBag.list = db.CustomerInfoes.Where(e => e.ChildProductId == Id && e.State == (int)EnumCustomer.DaiShanChu).ToList();            
            return PartialView(list);
        }
        //删除人员
        [HttpPost]
        public ActionResult DelPeopleAjax(int Id)
        {
            AjaxResult ajaxResult = new AjaxResult();
            var customerInfo = db.CustomerInfoes.Find(Id);
            if (customerInfo == null)
            {
                ajaxResult.Ok = 100;
            }
            else if(customerInfo.State == (int)EnumCustomer.DaiShanChu)
            {
                ajaxResult.Ok = 50;
            }
            else
            {
                customerInfo.ChildProduct.ZhanWeiCount += 1;
                db.Entry<ChildProduct>(customerInfo.ChildProduct).State = System.Data.Entity.EntityState.Modified;
                customerInfo.State = (int)EnumCustomer.YiShanChu;
                db.Entry<CustomerInfo>(customerInfo).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                ajaxResult.Ok = 200;
            }            
            return Json(ajaxResult);
        }
        //保存修改
        [HttpPost]
        public ActionResult SavePeopleAjax(int Id, int ChildProductId)
        {
            //异步提交结果
            AjaxResult ajaxResult = new AjaxResult();
            //拿到子线路信息
            var cp = db.ChildProducts.Find(ChildProductId);
            if (cp == null)
            {
                ajaxResult.Ok = 100;
                return Json(ajaxResult);
            }
            //拿到成员信息并添加至数据库
            var customerInfo = JsonConvert.DeserializeObject<CustomerInfo>(Request.Form["CustomerInfo"]);
            //添加新的成员
            if(Id < 0)
            {
                if(cp.ZhanWeiCount < 1)
                {
                    ajaxResult.Ok = 100;
                    return Json(ajaxResult);
                }
                //修改子线路信息
                cp.ZhanWeiCount -= 1;
                db.Entry<ChildProduct>(cp).State = System.Data.Entity.EntityState.Modified;

                customerInfo.CreateTime = DateTime.Now;
                customerInfo.CreateUserId = FenXiaoUserContext.Current.UserInfo.Id;
                customerInfo.ChildProductId = cp.Id;
                customerInfo.State = (int)EnumCustomer.ZhengChang;               
                db.Entry<CustomerInfo>(customerInfo).State = System.Data.Entity.EntityState.Added;
                db.SaveChanges();
            }
            //修改已有成员
            else
            {
                var temp = db.CustomerInfoes.Find(Id);
                temp.Birthday = customerInfo.Birthday;
                temp.Birthplace = customerInfo.Birthplace;
                temp.CreateTime = DateTime.Now;
                temp.CreateUserId = FenXiaoUserContext.Current.UserInfo.Id;
                temp.DaoQiTime = customerInfo.DaoQiTime;
                temp.FenFang = customerInfo.FenFang;
                temp.Name = customerInfo.Name;
                temp.Note = customerInfo.Note;
                temp.Phone = customerInfo.Phone;
                temp.PinYinMing = customerInfo.PinYinMing;
                temp.PinYinXing = customerInfo.PinYinXing;
                temp.QianFaPlace = customerInfo.QianFaPlace;
                temp.QiangFaTime = customerInfo.QiangFaTime;
                temp.Sex = customerInfo.Sex;
                temp.Type = customerInfo.Type;
                temp.ZhengZhaoNumber = customerInfo.ZhengZhaoNumber;
                temp.ZhengZhaoType = customerInfo.ZhengZhaoType;
                db.Entry<CustomerInfo>(temp).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }        

            //所有操作正常，返回状态200
            ajaxResult.Ok = 200;
            return Json(ajaxResult);
        }
        //报表导出
        [HttpGet]
        public ActionResult Report(int Id)
        {
            //创建Excel文件的对象
            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
            NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("Sheet1"); //添加一个sheet

            //给sheet1添加第一行的头部标题
            NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);
            row1.CreateCell(0).SetCellValue("团号");
            row1.CreateCell(1).SetCellValue("姓名");
            row1.CreateCell(2).SetCellValue("拼音姓");
            row1.CreateCell(3).SetCellValue("拼音名");
            row1.CreateCell(4).SetCellValue("性别");
            row1.CreateCell(5).SetCellValue("生日");
            row1.CreateCell(6).SetCellValue("出生地");
            row1.CreateCell(7).SetCellValue("证照类型");
            row1.CreateCell(8).SetCellValue("证照号");
            row1.CreateCell(9).SetCellValue("签发地");
            row1.CreateCell(10).SetCellValue("签发时间");
            row1.CreateCell(11).SetCellValue("到期时间");
            row1.CreateCell(12).SetCellValue("成员类别");
            row1.CreateCell(13).SetCellValue("电话");
            row1.CreateCell(14).SetCellValue("分房");
            row1.CreateCell(15).SetCellValue("备注");
            //获取数据
            var list = db.CustomerInfoes.Where(e => e.ChildProductId == Id && e.State== (int)EnumCustomer.ZhengChang).ToList();
             //将数据逐步写入sheet1各个行
            for (int i = 0; i < list.Count; i++)
            {
                NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(i + 1);

                rowtemp.CreateCell(0).SetCellValue(list[i].ChildProduct.ProductId.ToString());
                rowtemp.CreateCell(1).SetCellValue(list[i].Name);
                rowtemp.CreateCell(2).SetCellValue(list[i].PinYinXing);
                rowtemp.CreateCell(3).SetCellValue(list[i].PinYinMing);
                if (list[i].Sex == 0)
                {
                    rowtemp.CreateCell(4).SetCellValue("男");
                }
                else
                {
                    rowtemp.CreateCell(4).SetCellValue("女");
                }
                rowtemp.CreateCell(5).SetCellValue(list[i].Birthday);
                rowtemp.CreateCell(6).SetCellValue(list[i].Birthplace);
                switch(list[i].ZhengZhaoType)
                {
                    case 0:
                        {
                            rowtemp.CreateCell(7).SetCellValue("身份证");
                            break;
                        }
                    case 1:
                        {
                            rowtemp.CreateCell(7).SetCellValue("护照");
                            break;
                        }
                    case 2:
                        {
                            rowtemp.CreateCell(7).SetCellValue("军人证");
                            break;
                        }
                    case 3:
                        {
                            rowtemp.CreateCell(7).SetCellValue("赴台证");
                            break;
                        }
                    case 4:
                        {
                            rowtemp.CreateCell(7).SetCellValue("港澳证");
                            break;
                        }
                    default:
                        {
                            rowtemp.CreateCell(7).SetCellValue("身份证");
                            break;
                        }
                }
                rowtemp.CreateCell(8).SetCellValue(list[i].ZhengZhaoNumber);
                rowtemp.CreateCell(9).SetCellValue(list[i].QianFaPlace);
                rowtemp.CreateCell(10).SetCellValue(list[i].QiangFaTime);
                rowtemp.CreateCell(11).SetCellValue(list[i].DaoQiTime);
                if (list[i].Sex == 0)
                {
                    rowtemp.CreateCell(12).SetCellValue("成人");
                }
                else
                {
                    rowtemp.CreateCell(12).SetCellValue("儿童");
                }            
                rowtemp.CreateCell(13).SetCellValue(list[i].Phone);
                rowtemp.CreateCell(14).SetCellValue(list[i].FenFang);
                rowtemp.CreateCell(15).SetCellValue(list[i].Note);
            }

            // 写入到客户端 
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms, "application/vnd.ms-excel", DateTime.Now.ToString("yyyyMMdd") + ".xls");
        }
        #endregion

        #region 微信营销页
        [HttpGet]
        public ActionResult WeChat(int Id)
        {
            var pages = db.Pro2Page.Where(a =>
               (a.Page.State == (int)EnumPageState.publicState
               || (a.CompanyId == this.LoginInfo.CompanyId))
               &&a.ProductId == Id).
               OrderByDescending(a => a.Id).ToList();
            return PartialView(pages);
        }
        #endregion
    }
}