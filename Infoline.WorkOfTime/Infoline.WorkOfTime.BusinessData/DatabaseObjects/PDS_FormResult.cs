using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class PDS_FormResult : InfolineTable
    {
        public string comment { get; set;}
        public int? status { get; set;}
        public Guid? formPermitTaskUserId { get; set;}
        public Guid? evaluateFormId { get; set;}
        public Guid? evaluatorId { get; set;}
        public Guid? evaluatedUserId { get; set;}
    }
}
