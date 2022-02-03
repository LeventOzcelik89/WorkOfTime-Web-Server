using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWPRD_Inventory : InfolineTable
    {
        public int isItSalable { get; set;}
        public string fullName { get; set;}
        public string fullNameProduct { get; set;}
        public string searchField { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string productId_Title { get; set;}
        public string productId_Image { get; set;}
        public Guid? productId { get; set;}
        public string code { get; set;}
        public string serialcode { get; set;}
        public short? type { get; set;}
        public string firstActionType_Title { get; set;}
        public string lastActionType_Title { get; set;}
        public string firstActionDataId_Title { get; set;}
        public string firstActionDataCompanyId_Title { get; set;}
        public string lastActionDataId_Title { get; set;}
        public string lastActionDataCompanyId_Title { get; set;}
        public IGeometry  location { get; set;}
        public short? firstActionType { get; set;}
        public short? lastActionType { get; set;}
        public Guid? firstActionDataId { get; set;}
        public Guid? firstActionDataCompanyId { get; set;}
        public Guid? firstActionId { get; set;}
        public Guid? lastActionId { get; set;}
        public Guid? lastActionDataId { get; set;}
        public Guid? lastActionDataCompanyId { get; set;}
        public Guid? firstTransactionId { get; set;}
        public Guid? lastTransactionId { get; set;}
        public Guid? firstTransactionItemId { get; set;}
        public Guid? lastTransactionItemId { get; set;}
        public string lastActionCompanyTitles { get; set;}
    }
}
