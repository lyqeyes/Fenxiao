using FenXiao.Web.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Yeanzhi.System;
using Webdiyer.WebControls.Mvc;
using FenXiao.Model;

namespace FenXiao.Web.Areas.Admin.Controllers
{
    public class AManagementController : AdminControllerBase
    {
        #region 大类部分
        public ActionResult DaLei(int?id)
        {            
            var daleis = db.LuXianTypes.Where(a => a.TypeTag.Length == 2).ToList().OrderByDescending(a => a.Weight);
            return View(daleis.ToPagedList(id??1,25));
        }
        public ActionResult DaleiSearch()
        {
            //...检索
            return View("DaLei");
        }

        
        public string CreateDaLei(string name, int weight)
        {
            if (string.IsNullOrEmpty(name) || weight == 0)
            {
                return JsonConvert.SerializeObject(new ResultJson
                {
                    Ok = 0,
                    Msg = "大类名或权重为空"
                });
            }
            //添加大类
            var newOne = new FenXiao.Model.LuXianType
            {
                CreateTime = DateTime.Now,
                IsDelete = 0,
                TypeName = name,
                TypeTag = GetTypeVaule(),
                Weight = weight
            };
            db.LuXianTypes.Add(newOne);
            db.SaveChanges();

            //对应到所有商品
            var productlist = db.Products;
            if (productlist != null)
            {
                foreach (var item in productlist)
                {
                    db.Product2Type.Add(new Product2Type() {
                        TypeId = newOne.Id,
                        ProductId = item.Id
                    });
                }
            }
            db.SaveChanges();

            return JsonConvert.SerializeObject(new ResultJson
                {
                    Ok = 1,
                    Result = "1"
                });
        }
        public string EditDaLei(string editname, int editweight, int editId)
        {
            if (string.IsNullOrEmpty(editname) || editweight == 0)
            {
                return JsonConvert.SerializeObject(new ResultJson
                {
                    Ok = 0,
                    Msg = "大类名或权重为空"
                });
            }
            var editOne = db.LuXianTypes.FirstOrDefault(a => a.Id == editId);
            editOne.TypeName = editname;
            editOne.Weight = editweight;
            db.Entry(editOne).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return JsonConvert.SerializeObject(new ResultJson
                {
                    Ok = 1,
                    Result = "1"
                });
        }


        public string DelDaLei()
        {
            return "";
        } 
        #endregion

        #region 小类部分
        public ActionResult XiaoLei(int? id, string tag)
        {
            ViewBag.tag = tag;
            var xiaoleis = db.LuXianTypes.Where(a => a.TypeTag.Length == 4 && a.TypeTag.Contains(tag)).OrderByDescending(a => a.Weight);
            return View(xiaoleis.ToPagedList(id ?? 1, 25));
        }

        public string CreateXiaoLei(string name, int weight, string tag)
        {
            if (string.IsNullOrEmpty(name) || weight == 0)
            {
                return JsonConvert.SerializeObject(new ResultJson
                {
                    Ok = 0,
                    Msg = "名称或权重为空"
                });
            }
            if (string.IsNullOrEmpty(tag))
            {
                return JsonConvert.SerializeObject(new ResultJson
                {
                    Ok = 0,
                    Msg = "大类tag错误"
                });
            }
            db.LuXianTypes.Add(new FenXiao.Model.LuXianType
            {
                CreateTime = DateTime.Now,
                IsDelete = 0,
                TypeName = name,
                TypeTag = GetOneTypeVaule(tag),
                Weight = weight
            });
            db.SaveChanges();
            return JsonConvert.SerializeObject(new ResultJson
            {
                Ok = 1,
                Result = "1"
            });
        }

        public string DelXiaoLei()
        {
            return "";
        } 
        #endregion

        #region 大类小类的禁用与恢复操作
        //大类 小类的禁用操作都是这个
        public ActionResult LuxianTypeChangeState(int id)
        {
            var type = db.LuXianTypes.FirstOrDefault(a => a.Id == id);

            //情况1: 大类的禁用
            if (type.IsDelete == 0 && type.TypeTag.Length == 2)
            {
                //获取该大类以及其下所有小类
                var typelist = from d in db.LuXianTypes
                               where d.TypeTag.StartsWith(type.TypeTag)
                               select d;
                //禁用所有
                foreach (var item in typelist)
                {
                    item.IsDelete = 1;
                    db.Entry(item).State = System.Data.Entity.EntityState.Modified;
                }

                //获取该大类以及其下所有小类的ID
                var typeidlist = from d in typelist
                                 select d.Id;
                //获取所有和该大类关联的路线
                var product2typelist = from d in db.Product2Type
                                       where typeidlist.Contains(d.TypeId)
                                       select d;
                //删除他们之间的关联关系
                foreach (var item in product2typelist)
                {
                    db.Product2Type.Remove(item);
                }
            }
            //情况2:大类的恢复, 操作与大类添加一样
            else if (type.IsDelete == 1 && type.TypeTag.Length == 2)
            {
                var typelist = from d in db.LuXianTypes
                               where d.TypeTag.StartsWith(type.TypeTag)
                               select d;
                //恢复所有
                foreach (var item in typelist)
                {
                    item.IsDelete = 0;
                    db.Entry(item).State = System.Data.Entity.EntityState.Modified;
                }
                var productlist = db.Products;
                if (productlist != null)
                {
                    foreach (var item in productlist)
                    {
                        db.Product2Type.Add(new Product2Type()
                        {
                            TypeId = type.Id,
                            ProductId = item.Id
                        });
                    }
                }
            }
            //情况3:小类的禁用
            else if (type.IsDelete == 0 && type.TypeTag.Length == 4)
            {
                type.IsDelete = 1;
                var tag = type.TypeTag.Substring(0, 2);
                var dalei = db.LuXianTypes.FirstOrDefault(a => a.TypeTag == tag);
                if (dalei != null)
                {
                    var list = db.Product2Type.Where(a => a.TypeId == type.Id);
                    foreach (var item in list)
                    {
                        item.TypeId = dalei.Id;
                        db.Entry(item).State = System.Data.Entity.EntityState.Modified;
                    }
                }
            }
            //情况4:小类的恢复
            else
            {
                //...小类恢复无其它操作
                type.IsDelete = 0;
            }
            db.Entry(type).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            if (type.TypeTag.Length == 2)
            {
                return RedirectToAction("DaLei");
            }
            else
            {
                return RedirectToAction("XiaoLei", new { tag = type.TypeTag.Substring(0, 2) });
            }
        } 
        #endregion

        #region 创建编码的相关方法
        private string GetTypeVaule()
        {
            var temp = GenerateCheckCode(2);
            var v = db.LuXianTypes.FirstOrDefault(a => a.TypeTag == temp);
            if (v != null)
            {
                temp = GetTypeVaule();
            }
            return temp;
        }

        private string GetOneTypeVaule(string id)
        {
            var temp = id + GenerateCheckCode(2);
            var v = db.LuXianTypes.FirstOrDefault(a => a.TypeTag == temp);
            if (v != null)
            {
                temp = GetOneTypeVaule(id);
            }
            return temp;
        }

        private string GenerateCheckCode(int codeCount)
        {
            int number;
            char code;
            string checkCode = String.Empty;
            System.Random random = new Random();
            for (int i = 0; i < codeCount; i++)
            {
                number = random.Next();
                if (number % 2 == 0)
                    code = (char)('0' + (char)(number % 10));
                else
                    code = (char)('a' + (char)(number % 26));
                checkCode += code.ToString();
            }
            return checkCode;
        } 
        #endregion
    }
}