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
        public System.DateTime CreateTime { get; set; }
        public int Type { get; set; }
        public string Name { get; set; }
        public string PinYinXing { get; set; }
        public string PinYinMing { get; set; }
        public int Sex { get; set; }
        public string Birthday { get; set; }
        public string Birthplace { get; set; }
        public int ZhengZhaoType { get; set; }
        public string ZhengZhaoNumber { get; set; }
        public string QianFaPlace { get; set; }
        public string QiangFaTime { get; set; }
        public string DaoQiTime { get; set; }
        public string Phone { get; set; }
        public string Note { get; set; }
        public string FenFang { get; set; }
        public int State { get; set; }
    
        public virtual ChildProduct ChildProduct { get; set; }
        public virtual User User { get; set; }
    }
}
