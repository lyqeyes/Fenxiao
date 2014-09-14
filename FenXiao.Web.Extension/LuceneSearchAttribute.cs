using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FenXiao.Web.Extension
{
    /// <summary>
    /// 搜索属性
    /// </summary>
    public class LuceneSearchAttribute : Attribute 
    {
        public LuceneSearchAttribute()
        { }
        private bool isIndex;
        public bool IsIndex
        {
            get { return isIndex; }
            set { isIndex = value; }
        }
        private bool isAdd;
        public bool IsAdd
        {
            get { return isAdd; }
            set { isAdd = value; }
        }
    }
}
