using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWPRD_StocktakingUser : InfolineTable
    {
        public Guid? userId { get; set;}
        public Guid? stocktakingId { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string stocktakingId_Title { get; set;}
        public string userId_Title { get; set;}
    }
}
