using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
namespace Infoline.WorkOfTime.BusinessAccess.Tools.Mapper
{
    public class ReportField
    {
        public string DisplayName { get; set; }
        public string Field { get; set; }
        public bool Checked { get; set; } = true;
        public string GroupName { get; set; }
        public Guid[] RoleIds { get; set; }
    }

    public class ReportGroupFields
    {
        public List<ReportField> reportFields { get; set; } = new List<ReportField>();
        public List<ReportGroup> reportGroups { get; set; } = new List<ReportGroup>();
    }

    public class ReportGroupContainer
    {
        public string Filters { get; set; }
        public ReportGroupFields Fields { get; set; }
    }

    public class ReportGroup
    {
        public string DisplayName { get; set; }
        public List<ReportField> Fields { get; set; } = new List<ReportField>();
    }
    public class MapperBase<T> where T:InfolineTable
    {
        private List<ReportField> items { get; set; } = new List<ReportField>();
        public void Map<TKey>(Expression<Func<T, TKey>> fields, string displayName, string groupName, params string[] roles)
        {
            var props = ExpressionHelper.GetProperties<T, TKey>(fields);
            var repField = new ReportField
            {
                DisplayName = displayName,
                Field = props.Select(a => a.Name).FirstOrDefault(),
                GroupName = String.IsNullOrEmpty(groupName) ? "Diğer Alanlar" : groupName,
                RoleIds = roles.Select(a => a.B_ToGuid()).Where(a => a.HasValue).Select(a => a.Value).ToArray(),
            };

            items.Add(repField);
        }
        public List<ReportField> GetFields()
        {
            return items.ToList();
        }
        public List<string> GetGroups()
        {
            return items.GroupBy(a => a.GroupName).Select(a => a.Key).ToList();
        }
    }
}
