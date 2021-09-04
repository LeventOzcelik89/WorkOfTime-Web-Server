using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infoline.WorkOfTime.BusinessData.Specific
{
    public class PageModel
    {
        public string Title { get; set; }
        public List<PageFilter> Filters { get; set; } = new List<PageFilter>();
        public List<PageOrder> Orders { get; set; } = new List<PageOrder>();
        public string SearchProperty { get; set; }
    }


    public class PageFilter
    {
        public string Title { get; set; }
        public List<PageFilterItem> Items { get; set; } = new List<PageFilterItem>();
    }

    public class PageFilterItem
    {
        public string Title { get; set; }
        public object Filter { get; set; }
    }




    public class PageOrder
    {
        public string Title { get; set; }
        public string Column { get; set; }

    }




}
