using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWPRD_Transaction : InfolineTable
    {
        public string searchField { get; set;}
        public DateTime? date { get; set;}
        public string code { get; set;}
        public short? status { get; set;}
        public short? type { get; set;}
        public string description { get; set;}
        public Guid? invoiceId { get; set;}
        public Guid? orderId { get; set;}
        public Guid? outputId { get; set;}
        public string outputTable { get; set;}
        public Guid? outputCompanyId { get; set;}
        public Guid? inputId { get; set;}
        public string inputTable { get; set;}
        public Guid? inputCompanyId { get; set;}
        public DateTime? startDate { get; set;}
        public DateTime? endDate { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string status_Title { get; set;}
        public string type_Title { get; set;}
        public string invoiceId_Title { get; set;}
        public string orderId_Title { get; set;}
        public string inputId_Title { get; set;}
        public string project_Title { get; set;}
        public string outputId_Title { get; set;}
        public string inputCompanyId_Title { get; set;}
        public string outputCompanyId_Title { get; set;}
    }
}
