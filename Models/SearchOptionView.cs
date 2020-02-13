using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace hosungnotes.Models
{
    public class SearchOptionView
    {
        public string SltOpt { get; set; }

        public string TxtSearch { get; set; }

        public string ChkSearchIn { get; set; }

        public string UserPeriod { get; set; }

        public string FromDate { get; set; }

        public string ToDate { get; set; }

        public int? SearchRange { get; set; }

        public int PageSize { get; set; }
    }
}