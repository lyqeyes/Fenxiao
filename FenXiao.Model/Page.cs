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
    
    public partial class Page
    {
        public Page()
        {
            this.Pro2Page = new HashSet<Pro2Page>();
        }
    
        public System.Guid Id { get; set; }
        public string Audio { get; set; }
        public string Image { get; set; }
        public System.DateTime CreateTime { get; set; }
        public int Kind { get; set; }
        public int State { get; set; }
        public string Name { get; set; }
    
        public virtual ICollection<Pro2Page> Pro2Page { get; set; }
    }
}
