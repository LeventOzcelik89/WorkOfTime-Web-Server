using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWSH_ShiftTracking : InfolineTable
    {
        public Guid? userId { get; set;}
        public Guid? companyId { get; set;}
        public short? shiftTrackingStatus { get; set;}
        public IGeometry  location { get; set;}
        public Guid? qrCodeData { get; set;}
        public DateTime? timestamp { get; set;}
        public Guid? tableId { get; set;}
        public string tableName { get; set;}
        public string qrCodeDataText { get; set;}
        public Guid? shiftTrackingDeviceId { get; set;}
        public int? passType { get; set;}
        public string deviceUserId { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string UserId_Title { get; set;}
        public string CompanyId_Title { get; set;}
        public string table_Title { get; set;}
        public string ShiftTrackingStatus_Title { get; set;}
    }
}
