//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace FenXiao.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    
    public partial class User
    {
        public User()
        {
            this.HandleApplies = new HashSet<HandleApply>();
            this.HandleReturnForms = new HashSet<HandleReturnForm>();
            this.LoginInfoes = new HashSet<LoginInfo>();
            this.OrderForms = new HashSet<OrderForm>();
            this.Pro2Page = new HashSet<Pro2Page>();
            this.Products = new HashSet<Product>();
            this.ReturnForms = new HashSet<ReturnForm>();
            this.CustomerInfoes = new HashSet<CustomerInfo>();
        }
    
        public int Id { get; set; }
        [Required(ErrorMessage = "*用户名必填项")]
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string Phone { get; set; }
        [Required(ErrorMessage = "*邮箱为必填项")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "*不符合邮箱格式")]
        public string Email { get; set; }
        [Required(ErrorMessage = "*密码为必填项")]
        public string Password { get; set; }
        public string Role { get; set; }
        public System.DateTime CreateTime { get; set; }
        public int CompanyId { get; set; }
        public int State { get; set; }
    
        public virtual Company Company { get; set; }
        public virtual ICollection<HandleApply> HandleApplies { get; set; }
        public virtual ICollection<HandleReturnForm> HandleReturnForms { get; set; }
        public virtual ICollection<LoginInfo> LoginInfoes { get; set; }
        public virtual ICollection<OrderForm> OrderForms { get; set; }
        public virtual ICollection<Pro2Page> Pro2Page { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<ReturnForm> ReturnForms { get; set; }
        public virtual ICollection<CustomerInfo> CustomerInfoes { get; set; }
    }
}
