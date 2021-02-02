using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HZSoft.Application.Web.Utility
{

    public class WhereEntity
    {
        public string page { get; set; }
        public string price { get; set; }
        public string Grade { get; set; }
        public string search { get; set; }
        public string search_is_end { get; set; }
        public string province { get; set; }
        public string City { get; set; }
        public string orderby { get; set; }
        public string ordername { get; set; }
        public string nomore { get; set; }
        public string is_loading { get; set; }
        public string Operator { get; set; }
        public string repeatNumber { get; set; }
        public string except { get; set; }
        public string moreNumber { get; set; }
        public string birthdayNumber { get; set; }

    }

    public class AgentWhereEntity
    {
        public string page { get; set; }
        public string pageCount { get; set; }
        public string search { get; set; }
        public string precise_num { get; set; }//精确查找        
        public string search_is_end { get; set; }
        public string province { get; set; }
        public string city { get; set; }
        public string orderby { get; set; }
        public string ordername { get; set; }
        public string price { get; set; }
        public string regex { get; set; }
        public string isp { get; set; }
        public string isNormal { get; set; }
        public string repeatNumber { get; set; }
        public string exceptNumber { get; set; }
        public string moreNumber { get; set; }
        public string birthdayNumber { get; set; }

    }
}