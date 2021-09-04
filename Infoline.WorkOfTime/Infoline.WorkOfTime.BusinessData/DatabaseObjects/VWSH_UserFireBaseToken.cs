using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWSH_UserFireBaseToken : InfolineTable
    {
        public Guid? userId { get; set;}
        public string token { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string UserId_Title { get; set;}
    }
}
