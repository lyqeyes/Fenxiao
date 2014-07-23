using FenXiao.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FenXiao.Web.Extension
{
    public class UserContext
    {
        protected IAuthCookie authCookie;

        public UserContext(IAuthCookie authCookie)
        {
            this.authCookie = authCookie;
        }

        public UserLoginInfo LoginInfo
        {
            get
            {
                if (authCookie.CompanyId == default(int) || authCookie.UserId == default(int))
                    return null;

                var loginInfo = DateSourceContext.Current.LoginInfoes.FirstOrDefault(a => a.UserId == authCookie.UserId 
                    && authCookie.CompanyId == a.User.CompanyId);
                if (loginInfo!=null)
                {
                    List<int> role = new List<int>();
                    var v = loginInfo.User.Role.Split(',');
                    foreach (var item in v)
                    {
                        role.Add(int.Parse(item));
                    }
                    UserLoginInfo userlogininfo = new UserLoginInfo
                    {
                        CompanyId = loginInfo.User.CompanyId,
                        Email = loginInfo.User.Email,
                        ImageUrl = loginInfo.User.ImageUrl,
                        UserId = loginInfo.UserId,
                        UserName = loginInfo.User.Name,
                        Role = role,
                        Companytype = loginInfo.CompanyRole
                    };
                    return userlogininfo;
                }
                return null;
               
            }
        }

        public User UserInfo
        {
            get 
            {
                if (this.LoginInfo==null)
                {
                    return null;
                }
                return DateSourceContext.Current.Users.FirstOrDefault(a=>a.Email==LoginInfo.Email);
            }
        }
    }
}
