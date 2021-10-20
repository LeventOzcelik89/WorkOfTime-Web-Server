using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWINV_CommissionsInformation : InfolineTable
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
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string Information_Title { get; set;}
        public string TravelInformation_Title { get; set;}
        public string ApproveStatus_Title { get; set;}
        public int? travelInformation { get; set;}
        public int? requestForAccommodation { get; set;}
        public string from_Title { get; set;}
        public string to_Title { get; set;}
        public string shuttleFrom_Title { get; set;}
        public string shuttleTo_Title { get; set;}
        public string rentalCarPlace_Title { get; set;}
        public string hotelLocation_Title { get; set;}
        public string airportFrom_Title { get; set;}
        public string airportTo_Title { get; set;}
        public string flightInfoFiles { get; set;}
        public string hotelInfoFiles { get; set;}
        public string rentalCarInfoFiles { get; set;}
        public string shuttleInfoFiles { get; set;}
        public string busInfoFiles { get; set;}
        public string companyCarFiles { get; set;}
    }
}
