using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace hosungnotes
{
    [Serializable]
    public class GWPrincipalSerializeModel
    {
        public string UserId { get; set; }
        public string UserIdBase64 { get; set; }
        public int UserSeq { get; set; }
        public string PasswdBase64 { get; set; }
        public string KName { get; set; }
        public string Lang { get; set; }
        public string Style { get; set; }

    }
}