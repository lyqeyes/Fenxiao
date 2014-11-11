using FenXiao.Model;
using FenXiao.Web.Common;
using FenXiao.Web.Extension;
using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;

namespace FenXiao.Web.Areas.Marketer.Controllers
{
    /// <summary>
    /// 公司管理
    /// </summary>
    public class MCompanyController : MarketerControllerBase
    {
        //内容头部的公司信息
        [HttpGet]
        public ActionResult CompanyInfo()
        {
            ViewBag.Count = db.Users.Count(e => e.CompanyId == FenXiaoUserContext.Current.UserInfo.CompanyId) - 1;
            ViewBag.IsAll = FenXiaoUserContext.Current.UserInfo.Role.Split(',').Contains(((int)EnumRole.lingshou).ToString());
            var userList = db.Users.Where(e => e.CompanyId == FenXiaoUserContext.Current.UserInfo.CompanyId);
            foreach (var user in userList)
            {
                if (user.Role.Split(',').Contains(((int)EnumRole.lingshou).ToString()))
                {
                    ViewBag.Name = user.Name;
                    break;
                }
            }
            return PartialView();
        }

        //公司管理首页
        [HttpGet]
        public ActionResult Index(int id = 0)
        {
            var userlist = db.Users.Where(a => a.CompanyId == LoginInfo.CompanyId).OrderBy(a => a.Id).ToPagedList(id, PageSize);
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
            return RedirectToAction("Index");
        }
        
        //编辑子账户
        [HttpGet]
        public ActionResult EditMember(int id, int ReferIndex=0)
        {
            var user = db.Users.Find(id);
            ViewBag.ReferIndex = ReferIndex;
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }
        [HttpPost]
        public ActionResult EditMember([Bind(Include = "Password,Name,Phone,Email,Id")]User newUser, int ReferIndex=0)
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
            return RedirectToAction("Index", new { id = ReferIndex });
        }

        //修改子账户状态
        [HttpGet]
        public ActionResult EditMemberState(int id, string Type, int ReferIndex=0)
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
            return RedirectToAction("Index", new { id = ReferIndex});
        }

        //修改公司信息
        public ActionResult EditCompany()
        {
            var com = db.Companies.FirstOrDefault(a => a.Id == this.LoginInfo.CompanyId);
            if (com == null)
            {
                return HttpNotFound();
            }
            return View(com);
        }

        [HttpPost]
        public ActionResult EditCompany(TempCompany tempCompany)
        {
            if (!this.LoginInfo.Role.Contains((int)EnumRole.lingshou))
            {
                return HttpNotPermission();
            }
            var com = db.Companies.FirstOrDefault(a => a.Id == this.LoginInfo.CompanyId);
            if (com == null)
            {
                return HttpNotFound();
            }
            tempCompany.State = 1;
            tempCompany.CompanyId = this.LoginInfo.CompanyId;
            tempCompany.CreateTime = DateTime.Now;
            tempCompany.CompanyPhone = tempCompany.ZuoJi;
            db.TempCompanies.Add(tempCompany);
            db.SaveChanges();
            Response.AppendHeader("Cache-Control", "no-cache");
            return RedirectPermanent("~/Marketer/MError/EditShenhe");
        }
        //申请成为批发商
        //[HttpGet]
        //public ActionResult ApplyPiFa()
        //{
        //    return View();
        //}
        //[HttpPost]
        //public ActionResult ApplyPiFa(string LvXingSheZeRenXian, string RenShenYiWaiXian, string AjiLuXingShe, string RongYuChengHao)
        //{
        //    lock (LockClass.objApplyToPiFa)
        //    {
        //        if (FenXiaoUserContext.Current.UserInfo.Company.CompanyRole.Contains(((int)EnumCompany.zanshipifa).ToString()))
        //        {
        //            return View();
        //        }
        //        var apply = new Apply();
        //        apply.LvXingSheZeRenXian = LvXingSheZeRenXian;
        //        apply.RenShenYiWaiXian = RenShenYiWaiXian;
        //        apply.AjiLuXingShe = AjiLuXingShe;
        //        apply.RongYuChengHao = RongYuChengHao;
        //        apply.CreateTime = DateTime.Now;
        //        apply.CompanyId = FenXiaoUserContext.Current.UserInfo.CompanyId;
        //        apply.ApplyRole = (int)EnumCompany.pifa;
        //        apply.State = (int)EnumApply.Applying;
        //        db.Entry<Apply>(apply).State = System.Data.Entity.EntityState.Added;
        //        FenXiaoUserContext.Current.UserInfo.Company.CompanyRole += ("," + ((int)EnumCompany.zanshipifa).ToString());
        //        db.Entry<Company>(FenXiaoUserContext.Current.UserInfo.Company).State = System.Data.Entity.EntityState.Modified;
        //        db.SaveChanges();
        //    }
        //    return RedirectToAction("Index");
        //}

        #region 申请成为批发商
        public ActionResult ReplyToPiFa()
        {
            return View();
        }

        public ActionResult SubmitReply()
        {
            if (!UserContext.UserInfo.Company.
                 CompanyRole.Split(',').
                 Contains(((int)EnumCompany.zanshipifa).ToString()))
            {
                lock (LockClass.objApplyToLingShou)
                {
                    if (!UserContext.UserInfo.Company.
                     CompanyRole.Split(',').
                     Contains(((int)EnumCompany.zanshipifa).ToString()))
                    {
                        db.Applies.Add(new Apply
                        {
                            ApplyRole = (int)EnumCompany.pifa,
                            CompanyId = UserContext.UserInfo.CompanyId,
                            CreateTime = DateTime.Now,
                            State = (int)EnumApply.Applying
                        });
                        var com = db.Companies.Find(UserContext.UserInfo.CompanyId);
                        com.CompanyRole = ((int)EnumCompany.lingshou) + "," + ((int)EnumCompany.zanshipifa);
                        db.Companies.Attach(com);
                        db.Entry(com).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                    }
                }
            }
            return RedirectToAction("Index");
        }
        #endregion

        #region 消息管理
        //消息列表
        public ActionResult Message(int id = 0)
        {
            var msgs = db.Messages.Where(a => a.ToCompanyId == FenXiaoUserContext.Current.LoginInfo.CompanyId
                 &&
                (a.State == (int)EnumMessage.chulituidan ||
                a.State == (int)EnumMessage.chulipifashangshenqing)||
                a.State==(int)EnumMessage.CompanyEdit).OrderByDescending(a => a.CreateTime).ToPagedList(id, PageSize);
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
                MessageContext.IsRead(message.Id);
                if (message.State == (int)EnumMessage.chulituidan)
                {
                    return RedirectToAction("CancelOrderDetail", "MLine", new { Area = "Marketer", id = message.RelatedId });

                }
                else if (message.State == (int)EnumMessage.chulipifashangshenqing)
                {
                    return RedirectToAction("HandleComInfoResult", "MCompany", new { Area = "Marketer", id = message.RelatedId });
                }
                else
                {
                    return RedirectToAction("HandleCompanyEditResult", "MCompany", new { Area = "Marketer", id = message.Id });
                }
            }
        }

        //公司申请信息结果
        public ActionResult HandleComInfoResult(int id)
        {
            var v = db.HandleApplies.Find(id);
            if (v == null)
            {
                return HttpNotFound();
            }
            return View(v);
        }
        //公司编辑的审核结果
        public ActionResult HandleCompanyEditResult(int id)
        {
            var message = db.Messages.FirstOrDefault(a => a.Id == id);
            if (message == null)
            {
                return HttpNotFound();
            }
            return View(message);
        }
        #endregion

        #region 导出 客户信息
        [HttpGet]
        public ActionResult Report()
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
            var childProductList = db.ChildProducts.Where(e => e.CompanyId == this.LoginInfo.CompanyId).ToList();
            foreach(var item in childProductList)
            {
                var list = db.CustomerInfoes.Where(e => e.ChildProductId == item.Id && e.State == (int)EnumCustomer.ZhengChang).ToList();
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
                    switch (list[i].ZhengZhaoType)
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
            }

            // 写入到客户端 
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms, "application/vnd.ms-excel", DateTime.Now.ToString("yyyyMMdd") + ".xls");
        }
        #endregion
    }
}