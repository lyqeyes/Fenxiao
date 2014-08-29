using FenXiao.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FenXiao.Web.Models
{
    public class HelpModel
    {
        public string Name { get; set; }
        public List<HelpArticle> HelpArticles { get; set; }
    }
}