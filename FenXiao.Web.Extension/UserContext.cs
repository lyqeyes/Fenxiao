using FenXiao.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FenXiao.Web.Extension
{
    /// <summary>
    /// 当前账户上下文
    /// </summary>
    public class UserContext
    {
        protected IAuthCookie authCookie;

        public UserContext(IAuthCookie authCookie)
        {
            this.authCookie = authCookie;
        }

        /// <summary>
        /// 用户的登录信息
        /// </summary>
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
                    if (loginInfo.CompanyRole!=this.authCookie.CompanyRole)
                    {
                        return null;
                    }
                    else if (loginInfo.LastActivityTime.AddDays(7)<DateTime.Now)
                    {
                        DateSourceContext.Current.LoginInfoes.Remove(loginInfo);
                        DateSourceContext.Current.SaveChanges();
                        return null;
                    }
                    else
                    {
                        loginInfo.LastActivityTime = DateTime.Now;
                        DateSourceContext.Current.SaveChanges();
                    }
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
