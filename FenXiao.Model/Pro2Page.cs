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
    
    public partial class Pro2Page
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public System.Guid PageId { get; set; }
        public int UserId { get; set; }
        public System.DateTime CreateTime { get; set; }
        public int CompanyId { get; set; }
    
        public virtual Company Company { get; set; }
        public virtual Page Page { get; set; }
        public virtual User User { get; set; }
        public virtual Product Product { get; set; }
    }
}
