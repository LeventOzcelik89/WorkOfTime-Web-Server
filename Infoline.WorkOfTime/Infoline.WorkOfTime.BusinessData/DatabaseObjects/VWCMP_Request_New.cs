using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWCMP_Request_New : InfolineTable
    {
        public short? direction { get; set;}
        public short? type { get; set;}
        public Guid? taskId { get; set;}
        public Guid? pid { get; set;}
        public string description { get; set;}
        public string rowNumber { get; set;}
        public Guid? customerId { get; set;}
        public DateTime? issueDate { get; set;}
        public DateTime? sendingDate { get; set;}
        public string customerAdress { get; set;}
        public short? status { get; set;}
        public Guid? customerStorageId { get; set;}
        public string type_Title { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string status_Title { get; set;}
        public string Customer_Title { get; set;}
        public string CustomerStorage_Title { get; set;}
        public Guid? projectId { get; set;}
        public double? total { get; set;}
        public string projectId_Title { get; set;}
    }
}
