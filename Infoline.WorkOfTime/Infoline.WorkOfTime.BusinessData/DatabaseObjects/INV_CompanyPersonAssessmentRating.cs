using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class INV_CompanyPersonAssessmentRating : InfolineTable
    {
        public Guid? assessmentId { get; set;}
        public string question { get; set;}
        public int? answer { get; set;}
    }
}
