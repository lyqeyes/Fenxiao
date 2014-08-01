using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Yeanzhi.Framework.Utility;

namespace FenXiao.Web.Extension
{
    public class CookieContext : IAuthCookie
    {
        public CookieContext()
        {
        }

        /// <summary>
        /// Cache或者Cookie的Key前缀
        /// </summary>
        public virtual string KeyPrefix
        {
            get
            {
                return "Context_";
            }
        }

        public void Set(string key, string value, int expiresHours = 0)
        {
            if (expiresHours > 0)
                Cookie.Save(KeyPrefix + key, value, expiresHours);
            else
                Cookie.Save(KeyPrefix + key, value);
        }

        #region IAuthCookie
        private int userExpiresHours = 10;
        public virtual int UserExpiresHours
        {
            get
            {
                return userExpiresHours;
            }
            set
            {
                userExpiresHours = value;
            }
        }

        public virtual string UserName
        {
            get
            {
                return HttpUtility.UrlDecode(Cookie.GetValue(KeyPrefix + "UserName"));
            }
            set
            {
                Cookie.Save(KeyPrefix + "UserName", HttpUtility.UrlEncode(value), UserExpiresHours);
            }
        }

        public virtual string Email
        {
            get
            {
                return Cookie.GetValue(KeyPrefix + "Email");
            }
            set
            {
                Cookie.Save(KeyPrefix + "Email", value, UserExpiresHours);
            }
        }

        public string ImageUrl
        {
            get
            {
                return HttpUtility.UrlDecode(Cookie.GetValue(KeyPrefix + "ImageUrl"));
            }
            set
            {
                Cookie.Save(KeyPrefix + "ImageUrl", HttpUtility.UrlEncode(value), UserExpiresHours);
            }

        }

        public int UserId
        {
            get
            {
                int userid = 0;
                int.TryParse(Cookie.GetValue(KeyPrefix + "UserId"), out userid);
                return userid;
            }
            set
            {
                Cookie.Save(KeyPrefix + "UserId", value.ToString(), UserExpiresHours);
            }
        }

        public int CompanyId
        {
            get
            {
                int CompanyId = 0;
                int.TryParse(Cookie.GetValue(KeyPrefix + "CompanyId"), out CompanyId);
                return CompanyId;
            }
            set
            {
                Cookie.Save(KeyPrefix + "CompanyId", value.ToString(), UserExpiresHours);
            }
        }

        public string Role
        {
            get
            {
                return Cookie.GetValue(KeyPrefix + "Role");
            }
            set
            {
                Cookie.Save(KeyPrefix + "Role", value.ToString(), UserExpiresHours);
            }
        }

        public int CompanyRole
        {
            get
            {
                int CompanyRole = -1;
                int.TryParse(Cookie.GetValue(KeyPrefix + "CompanyRole"), out CompanyRole);
                return CompanyRole;
            }
            set
            {
                Cookie.Save(KeyPrefix + "CompanyRole", value.ToString(), UserExpiresHours);
            }
        }
        #endregion


    }
}
