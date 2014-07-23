using FenXiao.Model;
using FenXiao.Web.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FenXiao.Web.Common
{
    public class FenXiaoUserContext : UserContext
    {
        public FenXiaoUserContext()
            : base(FenXiaoCookieContext.Current)
        {
        }

        public FenXiaoUserContext(IAuthCookie authCookie)
            : base(authCookie)
        {
        }

        public static FenXiaoUserContext Current
        {
            get
            {
                return new FenXiaoUserContext();
            }
        }

        public string CompanyType
        {
            get 
            {
                switch (this.LoginInfo.Companytype)
                {
                    case (int)EnumCompany.lingshou:
                        return "零售商";
                    case (int)EnumCompany.pifa:
                        return "批发商";
                    default:
                        return "管理";
                }
            }
        }
    }
}