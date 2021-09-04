using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWSH_PersonReferences : InfolineTable
    {
        public Guid? UserId { get; set;}
        public string ReferenceUserName { get; set;}
        public string ReferencePosition { get; set;}
        public string ReferenceMail { get; set;}
        public string ReferencePhone { get; set;}
        public string ReferenceWorkingCompany { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string User_Title { get; set;}
    }
}
