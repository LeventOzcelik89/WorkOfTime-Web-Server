using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWCRM_PresentationOpponentCompany : InfolineTable
    {
        public Guid? PresentationId { get; set;}
        public Guid? OpponentCompanyId { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string OpponentCompany_Title { get; set;}
        public string Presentation_Title { get; set;}
        public string LastStage_Title { get; set;}
        public string LastStage_Color { get; set;}
        public string CurrentPoint { get; set;}
    }
}
