using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FenXiao.Web.Common
{
    public class TypeHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="choose"></param>
        /// <param name="id"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static string ConvertToTypeString(List<int>choose,int id,int index)
        {
            choose[index] = id;
            string res = "";
            foreach (var item in choose)
            {
                res += item.ToString() + "+";
            }
            return res.Substring(0, res.Length - 1);
        }
    }
}