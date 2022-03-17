using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWPRD_Stocktaking : InfolineTable
    {
        public string code { get; set;}
        public DateTime? date { get; set;}
        public Guid? storageId { get; set;}
        public Guid? responsibleUserId { get; set;}
        public string description { get; set;}
        public short? status { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string storageId_Title { get; set;}
        public string responsibleUserId_Title { get; set;}
        public string status_Title { get; set;}
    }
}
