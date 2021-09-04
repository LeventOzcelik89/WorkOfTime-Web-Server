using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Web;

namespace Infoline.Data
{
    public class PageProperties
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string Sort { get; set; }
        public string SortType { get; set; }
        public int PageCount { get; set; }
        public int TotalItemCount { get; set; }
        public bool IsFirstPage { get; set; }
        public bool IsLastPage { get; set; }
        public int FirstItemOnPage { get; set; }
        public int LastItemOnPage { get; set; }
        public bool HasNextPage { get; set; }
        public bool HasPrevPage { get; set; }
    }
}
