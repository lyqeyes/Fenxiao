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
    
    public partial class OrderForm
    {
        public OrderForm()
        {
            this.CustomerInfoes = new HashSet<CustomerInfo>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public int CreateUserId { get; set; }
        public int ProductId { get; set; }
        public int ToCompanyId { get; set; }
        public string Note { get; set; }
        public System.DateTime CreateTime { get; set; }
        public int State { get; set; }
        public int AllCount { get; set; }
    
        public virtual Company Company { get; set; }
        public virtual ICollection<CustomerInfo> CustomerInfoes { get; set; }
        public virtual User User { get; set; }
        public virtual Product Product { get; set; }
    }
}
