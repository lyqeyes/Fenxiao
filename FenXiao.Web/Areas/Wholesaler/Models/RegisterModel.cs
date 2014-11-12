using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FenXiao.Web.Areas.Wholesaler.Models
{
    public class RegisterModel
    {
        public string reg_type { get; set; }
        public string reg_name { get; set; }
        public string reg_tel { get; set; }
        //营业执照号码
        public string reg_license { get; set; }

        public string reg_zerenxian { get; set; }
        public string reg_renshenxian { get; set; }
        public string reg_rongyuchenghao { get; set; }
        //旅行社经营许可号码
        public string reg_permit { get; set; }
        public string reg_paddress { get; set; }

        public string reg_pname { get; set; }
        public string reg_pjob { get; set; }
        public string reg_email { get; set; }
        public string reg_password { get; set; }
        public string reg_pid { get; set; }
        public string reg_ptel { get; set; }


        public string reg_clicenseurl { get; set; }
        public string reg_ccardurl { get; set; }
        public string reg_jingyingurl { get; set; }
        public string reg_zerenbaoxianurl { get; set; }
        public string reg_renshenbaoxianurl { get; set; }
        public string reg_rongyuzhengshuurl { get; set; }
    }
}