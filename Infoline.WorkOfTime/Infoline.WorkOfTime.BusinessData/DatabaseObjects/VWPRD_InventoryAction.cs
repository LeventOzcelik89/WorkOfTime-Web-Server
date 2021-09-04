using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWPRD_InventoryAction : InfolineTable
    {
        public Guid? transactionId { get; set;}
        public Guid? transactionItemId { get; set;}
        public Guid? inventoryId { get; set;}
        public short? type { get; set;}
        public Guid? dataId { get; set;}
        public string dataTable { get; set;}
        public Guid? dataCompanyId { get; set;}
        public IGeometry  location { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string transactionId_Title { get; set;}
        public string inventoryId_Title { get; set;}
        public string serialCode { get; set;}
        public string dataCompanyId_Title { get; set;}
        public string type_Title { get; set;}
        public string dataId_Title { get; set;}
    }
}
