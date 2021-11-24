using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class CMP_InvoiceConfirmation : InfolineTable
    {
        public Guid? advanceId { get; set;}
        public Guid? userId { get; set;}
        public short? status { get; set;}
        public string description { get; set;}
        public short? ruleOrder { get; set;}
        public short? ruleType { get; set;}
        public Guid? ruleUserId { get; set;}
        public Guid? ruleRoleId { get; set;}
        public string ruleTitle { get; set;}
    }
}
