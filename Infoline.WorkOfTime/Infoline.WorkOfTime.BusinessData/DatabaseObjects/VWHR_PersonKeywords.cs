using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWHR_PersonKeywords : InfolineTable
    {
        public Guid? HrPersonId { get; set;}
        public Guid? HrKeywordsId { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string HrPersonId_Title { get; set;}
        public string HrKeywords_Title { get; set;}
    }
}
