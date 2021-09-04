using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWFTM_TaskTemplateUser : InfolineTable
    {
        public Guid? taskTemplateId { get; set;}
        public Guid? userId { get; set;}
        public string verifyCode { get; set;}
        public bool? status { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string taskTemplateId_Title { get; set;}
        public string userId_Title { get; set;}
        public string photo { get; set;}
    }
}
