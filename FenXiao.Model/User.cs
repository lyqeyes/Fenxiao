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
    
    public partial class User
    {
        public User()
        {
            this.CustomerInfoes = new HashSet<CustomerInfo>();
            this.HandleApplies = new HashSet<HandleApply>();
            this.HandleReturnForms = new HashSet<HandleReturnForm>();
            this.LoginInfoes = new HashSet<LoginInfo>();
            this.OrderForms = new HashSet<OrderForm>();
            this.Pro2Page = new HashSet<Pro2Page>();
            this.Products = new HashSet<Product>();
            this.ReturnForms = new HashSet<ReturnForm>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public System.DateTime CreateTime { get; set; }
        public int CompanyId { get; set; }
        public int State { get; set; }
    
        public virtual Company Company { get; set; }
        public virtual ICollection<CustomerInfo> CustomerInfoes { get; set; }
        public virtual ICollection<HandleApply> HandleApplies { get; set; }
        public virtual ICollection<HandleReturnForm> HandleReturnForms { get; set; }
        public virtual ICollection<LoginInfo> LoginInfoes { get; set; }
        public virtual ICollection<OrderForm> OrderForms { get; set; }
        public virtual ICollection<Pro2Page> Pro2Page { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<ReturnForm> ReturnForms { get; set; }
    }
}
