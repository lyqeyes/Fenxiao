using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FenXiao.Model;
namespace FenXiao.Web.Extension
{
    /// <summary>
    /// 权限检查的Attribute
    /// </summary>
    public class AuthorizeCheckAttribute : Attribute
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="CheckType">检查类型</param>
        public AuthorizeCheckAttribute(EnumAuthorityCheckType CheckType)
        {
            this.CheckType = CheckType;
        }
        /// <summary>
        /// 检查类型
        /// </summary>
        private EnumAuthorityCheckType CheckType { set; get; }

        /// <summary>
        /// 执行检查
        /// </summary>
        /// <param name="id"></param>
        /// <returns>true为有权限，false为无权限</returns>
        public bool Check(int id,int CompanyId)
        {
            using (FenXiaoDBEntities db = new FenXiaoDBEntities())
            {
                switch (CheckType)
                {
                    case EnumAuthorityCheckType.Product:
                        {
                            var product = db.Products.Find(id);
                            if (product == null)
                                return false;
                            if (product.User.CompanyId != CompanyId)
                                return false;
                            else
                                return true;
                        }
                    case EnumAuthorityCheckType.ChildProduct:
                        {
                            var childProduct = db.ChildProducts.Find(id);
                            if (childProduct == null)
                                return false;
                            if (childProduct.CompanyId != CompanyId)
                                return false;
                            else
                                return true;
                        }
                    case EnumAuthorityCheckType.OrderForm:
                        {
                            var orderForm = db.OrderForms.Find(id);
                            if (orderForm == null)
                                return false;
                            if (orderForm.User.CompanyId != CompanyId)
                                return false;
                            else
                                return true;
                        }
                    case EnumAuthorityCheckType.ReturnForm:
                        {
                            var returnForm = db.ReturnForms.Find(id);
                            if (returnForm == null)
                                return false;
                            if (returnForm.User.CompanyId != CompanyId)
                                return false;
                            else
                                return true;
                        }
                    default:
                        {
                            return false;
                        }
                }
            }
        }
    }
}