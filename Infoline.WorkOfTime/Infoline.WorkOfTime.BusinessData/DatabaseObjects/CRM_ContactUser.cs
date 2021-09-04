using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class CRM_ContactUser : InfolineTable
    {
        public Guid? ContactId { get; set;}
        public Guid? UserId { get; set;}
        public int? UserType { get; set;}
    }
}
