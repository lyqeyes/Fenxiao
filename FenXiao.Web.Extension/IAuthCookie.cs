using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FenXiao.Web.Extension
{
    /// <summary>
    /// Cookie接口
    /// </summary>
    public interface IAuthCookie
    {
        int UserExpiresHours { get; set; }

        string UserName { get; set; }

        string Email { get; set; }

        string ImageUrl { get; set; }

        int UserId { get; set; }

        int CompanyId { get; set; }

        string Role { get; set; }
    }
}
