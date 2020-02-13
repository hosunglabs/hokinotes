using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Principal;

namespace hosungnotes
{
    interface IUserPrincipal : IPrincipal
    {
        string UserIdBase64 { get; set; }
        string UserId { get; set; }
        int UserSeq { get; set; }
        string PasswdBase64 { get; set; }
        string KName { get; set; }
        string Lang { get; set; }
        string Style { get; set; }
    }
}