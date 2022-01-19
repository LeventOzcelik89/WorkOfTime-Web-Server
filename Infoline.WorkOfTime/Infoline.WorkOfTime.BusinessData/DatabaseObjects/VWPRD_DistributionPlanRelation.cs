using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWPRD_DistributionPlanRelation : InfolineTable
    {
        public Guid? transactionId { get; set;}
        public Guid? distributionPlanId { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string searchField { get; set;}
        public string code { get; set;}
        public string status_Title { get; set;}
        public string type_Title { get; set;}
        public string inputId_Title { get; set;}
        public string outputId_Title { get; set;}
        public string invoiceId_Title { get; set;}
        public string orderId_Title { get; set;}
        public string description { get; set;}
    }
}
