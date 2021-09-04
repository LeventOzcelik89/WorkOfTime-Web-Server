using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWCMP_TenderTransaction : InfolineTable
    {
        public string searchField { get; set;}
        public Guid? tenderId { get; set;}
        public Guid? transactionId { get; set;}
        public short? type { get; set;}
        public short? status { get; set;}
        public string code { get; set;}
        public Guid? inputId { get; set;}
        public Guid? outputId { get; set;}
        public string status_Title { get; set;}
        public string type_Title { get; set;}
        public string invoiceId_Title { get; set;}
        public string orderId_Title { get; set;}
        public string inputId_Title { get; set;}
        public string project_Title { get; set;}
        public string outputId_Title { get; set;}
        public string inputCompanyId_Title { get; set;}
        public string outputCompanyId_Title { get; set;}
        public string rowNumber { get; set;}
        public Guid? requestId { get; set;}
    }
}
