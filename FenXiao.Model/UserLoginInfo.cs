using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FenXiao.Model
{
    /// <summary>
    /// 用户登录信息
    /// </summary>
    public class UserLoginInfo
    {
        public string UserName { get; set; }

        public string Email { get; set; }

        public string ImageUrl { get; set; }

        public int UserId { get; set; }

        public int CompanyId { get; set; }

        public int Companytype { get; set; }

        public List<int> Role { get; set; }
    }
}
