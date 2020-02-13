using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace hosungnotes.Models
{
    public class BoardT 
        {
        public long Seq
        {
            get;
            set;
        }
        public string LabelName
        {
            get;
            set;
        }
        public string Subject
        {
            get;
            set;
        }

        public string Comment
        {
            get;
            set;
        }     
        
        public DateTime RegDate
        {
            get;
            set;
        }


        public BoardT() { }


    }
}