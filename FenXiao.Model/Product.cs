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
    
    public partial class Product
    {
        public Product()
        {
            this.ChildProducts = new HashSet<ChildProduct>();
            this.OrderForms = new HashSet<OrderForm>();
            this.Pro2Page = new HashSet<Pro2Page>();
            this.Product2Type = new HashSet<Product2Type>();
            this.ReturnForms = new HashSet<ReturnForm>();
        }
    
        public int Id { get; set; }
        public int CreateUserId { get; set; }
        public int Count { get; set; }
        public int RemainCount { get; set; }
        public string Name { get; set; }
        public string Tese { get; set; }
        public System.DateTime SendGroupTime { get; set; }
        public double ChengRenPrice { get; set; }
        public double ErTongPrice { get; set; }
        public double SuggestionPrice { get; set; }
        public string FuJian { get; set; }
        public string TripContent { get; set; }
        public string Explain { get; set; }
        public string ZhuYiShiXiang { get; set; }
        public System.DateTime CreateTime { get; set; }
        public int State { get; set; }
        public int IsHot { get; set; }
    
        public virtual ICollection<ChildProduct> ChildProducts { get; set; }
        public virtual ICollection<OrderForm> OrderForms { get; set; }
        public virtual ICollection<Pro2Page> Pro2Page { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<Product2Type> Product2Type { get; set; }
        public virtual ICollection<ReturnForm> ReturnForms { get; set; }
    }
}
