using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWHDM_Ticket : InfolineTable
    {
        public string code { get; set;}
        public short? evaluation { get; set;}
        public short? status { get; set;}
        public short? priority { get; set;}
        public short? channel { get; set;}
        public Guid? issueId { get; set;}
        public Guid? suggestionId { get; set;}
        public string title { get; set;}
        public string content { get; set;}
        public Guid? requesterId { get; set;}
        public Guid? assignUserId { get; set;}
        public DateTime? dueDate { get; set;}
        public string evaluateDescription { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string issueId_Title { get; set;}
        public string suggestionId_Title { get; set;}
        public string requesterId_Title { get; set;}
        public string assignUserId_Title { get; set;}
        public string status_Title { get; set;}
        public string channel_Title { get; set;}
        public string priority_Title { get; set;}
        public string assignUser_Photo { get; set;}
        public string requester_Photo { get; set;}
        public string createdby_Photo { get; set;}
        public int? replyCount { get; set;}
        public int? forwardCount { get; set;}
        public int? noteCount { get; set;}
        public string searchField { get; set;}
    }
}
