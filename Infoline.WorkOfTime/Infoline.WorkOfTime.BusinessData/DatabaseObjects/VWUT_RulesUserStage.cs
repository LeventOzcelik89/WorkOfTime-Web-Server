using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWUT_RulesUserStage : InfolineTable
    {
        public Guid? rulesId { get; set;}
        public short? order { get; set;}
        public short? type { get; set;}
        public Guid? userId { get; set;}
        public string title { get; set;}
        public Guid? roleId { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string rulesId_Title { get; set;}
        public string userId_Title { get; set;}
        public string roleId_Title { get; set;}
        public string type_Title { get; set;}
        public short? ruleType { get; set;}
    }
}
