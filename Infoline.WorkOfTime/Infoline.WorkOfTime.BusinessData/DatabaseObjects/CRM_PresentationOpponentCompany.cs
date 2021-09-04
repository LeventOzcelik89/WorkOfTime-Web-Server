using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class CRM_PresentationOpponentCompany : InfolineTable
    {
        public Guid? PresentationId { get; set;}
        public Guid? OpponentCompanyId { get; set;}
    }
}
