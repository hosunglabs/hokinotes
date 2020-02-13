using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Principal;

namespace hosungnotes
{
    public class WebUser
    {
        public static GWUserPrincipal UserPrincipal
        {
            get
            {
                return (System.Web.HttpContext.Current.User as GWUserPrincipal);
            }
        }
    }
}