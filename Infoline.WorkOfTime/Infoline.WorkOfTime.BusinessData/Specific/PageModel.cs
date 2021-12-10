using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infoline.WorkOfTime.BusinessData.Specific
{
    public class PageModel
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public List<PageFilterGroup> FilterGroups { get; set; } = new List<PageFilterGroup>();
        public List<PageOrder> Orders { get; set; } = new List<PageOrder>();
        public string SearchProperty { get; set; }
        public void AddOrder(string title, string column)
        {

            this.Orders.Add(new PageOrder()
            {
                Column = column,
                Title = title
            });
        }
    }
    public class PageModel<T>
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public List<PageFilterGroup> FilterGroups { get; set; } = new List<PageFilterGroup>();
        public List<PageOrder> Orders { get; set; } = new List<PageOrder>();
        public string SearchProperty { get; set; }
        [JsonIgnore]
        public Expression<Func<T, bool>> GridFilter { get; set; }

        public void SetGridFilter(Expression<Func<T, bool>> filter = null)
        {
            GridFilter = filter;
        }

        public PageModel(string pageName = null, string pageTitle = null)
        {
            Name = pageName;
        }
        public void AddOrder(string title, string column)
        {

            this.Orders.Add(new PageOrder()
            {
                Column = column,
                Title = title
            });
        }
    }

    public class PageFilter
    {

        public string Filter { get; set; }
        public string Color { get; set; }
        [JsonIgnore]
        public string ActiveClass { get; set; }
        public string Icon { get; set; }
        public bool IsActive { get; set; } = false;
        public int Count { get; set; } = 0;
        public string MobileIcon { get; set; }
        public bool MobileShowCard { get; set; }
        public string MobileColor { get; set; }

        public string GetFilterButton(string gridName, string switchType, string category)
        {

            var classes = IsActive == true ? "active " + ActiveClass : "btn-default";
            var str = $"<button type=\"button\" data-switch=\"{switchType}\" data-category=\"{category}\" data-grid=\"{gridName}\" data-active=\"{ActiveClass}\" data-query=\"{Filter}\"  class=\"btn btn-sm {classes}\"><i class=\"{Icon}\"></i> {Title} ({Count}) </button>";
            return str;

        }
        public string Title { get; set; }
        public List<PageFilterItem> Items { get; set; } = new List<PageFilterItem>();
    }

    public class PageFilterItem
    {
        public string Title { get; set; }
        public object Filter { get; set; }
    }


    public class PageFilterGroup
    {
        public string Title { get; set; }
        public List<PageFilter> Filters { get; set; } = new List<PageFilter>();
        [JsonIgnore]
        public string Class { get; set; }
        public bool MobileShowCard { get; set; }




    }

    public class PageOrder
    {
        public string Title { get; set; }
        public string Column { get; set; }

    }




}
