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
    
    public partial class CustomerInfo
    {
        public long Id { get; set; }
        public long ChildProductId { get; set; }
        public int CreateUserId { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int Sex { get; set; }
        public int CreateTime { get; set; }
        public int IsDelete { get; set; }
    
        public virtual ChildProduct ChildProduct { get; set; }
        public virtual User User { get; set; }
    }
}
