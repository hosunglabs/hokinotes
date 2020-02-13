using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Principal;

namespace hosungnotes
{
    public class GWUserPrincipal : IUserPrincipal
    {
        public IIdentity Identity { get; private set; }
        public bool IsInRole(string role)
        {
            return false;
        }
        public GWUserPrincipal(string userSeq)
        {
            this.Identity = new GenericIdentity(userSeq);
        }

        public string UserIdBase64 { get; set; }
        public string UserId { get; set; }
        public int UserSeq { get; set; }
        public string PasswdBase64 { get; set; }
        public string KName { get; set; }
        public string Lang { get; set; }
        public string Style { get; set; }
    }
       
}