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
    
    public partial class Company
    {
        public Company()
        {
            this.Applies = new HashSet<Apply>();
            this.ChildProducts = new HashSet<ChildProduct>();
            this.Messages = new HashSet<Message>();
            this.OrderForms = new HashSet<OrderForm>();
            this.Pro2Page = new HashSet<Pro2Page>();
            this.Users = new HashSet<User>();
            this.ReturnForms = new HashSet<ReturnForm>();
        }
    
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string YingYeZhiZhaoImg { get; set; }
        public string CompanyRole { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string CompanyAddress { get; set; }
        public string CompanyPhone { get; set; }
        public System.DateTime CreateTime { get; set; }
        public int State { get; set; }
        public string LianXiRen { get; set; }
        public string Phone { get; set; }
        public string ZuoJi { get; set; }
        public string AjiLuXingShe { get; set; }
        public string JingYingXuKe { get; set; }
        public string FaRenShenFenZhengImg { get; set; }
        public string RongYuChengHao { get; set; }
        public string LvXingSheZeRenXian { get; set; }
        public string RenShenYiWaiXian { get; set; }
    
        public virtual ICollection<Apply> Applies { get; set; }
        public virtual ICollection<ChildProduct> ChildProducts { get; set; }
        public virtual ICollection<Message> Messages { get; set; }
        public virtual ICollection<OrderForm> OrderForms { get; set; }
        public virtual ICollection<Pro2Page> Pro2Page { get; set; }
        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<ReturnForm> ReturnForms { get; set; }
    }
}
