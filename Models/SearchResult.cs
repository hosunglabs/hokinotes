using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace hosungnotes.Models
{
    public class SearchResult
    {
        public SearchResult() { }
        public bool IsValid
        {
            get;
            set;
        }

        public long BoardTotalCount
        {
            get;
            set;
        }

        public List<BoardT> lstBoard
        {
            get;
            set;
        }

        public string ServerError
        {
            get;
            set;
        }

        public String SearchText { get; set; }
    }
}