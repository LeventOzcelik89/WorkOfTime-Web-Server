using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWDOC_DocumentFlow : InfolineTable
    {
        public Guid? documentId { get; set;}
        public short? type { get; set;}
        public Guid? organizationUnitId { get; set;}
        public Guid? userId { get; set;}
        public short? order { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string userId_Title { get; set;}
        public string organizationUnitId_Title { get; set;}
        public string type_Title { get; set;}
    }
}
