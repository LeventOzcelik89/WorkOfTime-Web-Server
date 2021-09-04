using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class PDS_QuestionResult : InfolineTable
    {
        public double? point { get; set;}
        public string comment { get; set;}
        public Guid? questionFormId { get; set;}
        public Guid? formResultId { get; set;}
        public int? score { get; set;}
    }
}
