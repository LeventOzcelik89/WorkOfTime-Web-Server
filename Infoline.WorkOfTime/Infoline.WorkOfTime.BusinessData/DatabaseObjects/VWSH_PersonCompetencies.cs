using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWSH_PersonCompetencies : InfolineTable
    {
        public Guid? UserId { get; set;}
        public Guid? CompetenciesId { get; set;}
        public int? CompetenciesLevel { get; set;}
        public string created_Title { get; set;}
        public string changedby_Title { get; set;}
        public string User_Title { get; set;}
        public string Competencies_Title { get; set;}
        public string CompetenciesLevel_Title { get; set;}
    }
}
