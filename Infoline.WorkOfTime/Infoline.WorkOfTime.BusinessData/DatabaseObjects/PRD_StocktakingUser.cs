using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class PRD_StocktakingUser : InfolineTable
    {
        public Guid? userId { get; set;}
        public Guid? stocktakingId { get; set;}
    }
}
