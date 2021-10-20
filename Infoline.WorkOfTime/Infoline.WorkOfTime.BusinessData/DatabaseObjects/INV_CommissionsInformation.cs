using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class INV_CommissionsInformation : InfolineTable
    {
        public Guid? commissionsId { get; set;}
        public Guid? userId { get; set;}
        public Guid? to { get; set;}
        public Guid? from { get; set;}
        public DateTime? departureDate { get; set;}
        public DateTime? returnDate { get; set;}
        public string hotelName { get; set;}
        public DateTime? hotelEntryDate { get; set;}
        public DateTime? hotelLeaveDate { get; set;}
        public Guid? rentalCarPlace { get; set;}
        public DateTime? rentalCarStartDate { get; set;}
        public DateTime? rentalCarEndDate { get; set;}
        public Guid? shuttleTo { get; set;}
        public Guid? shuttleFrom { get; set;}
        public DateTime? shuttleDepartureDate { get; set;}
        public DateTime? shuttleReturnDate { get; set;}
        public Guid? hotelLocation { get; set;}
        public string note { get; set;}
        public string hotelNote { get; set;}
        public string rentalCarNote { get; set;}
        public string shuttleNote { get; set;}
    }
}
