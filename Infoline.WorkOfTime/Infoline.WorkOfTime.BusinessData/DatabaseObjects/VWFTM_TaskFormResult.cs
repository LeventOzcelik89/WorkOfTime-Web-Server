using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWFTM_TaskFormResult : InfolineTable
    {
        public Guid? taskId { get; set;}
        public Guid? taskOperationId { get; set;}
        public Guid? formId { get; set;}
        public string jsonResult { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string task_Name { get; set;}
        public string form_Title { get; set;}
        public Guid? fixtureId { get; set;}
        public Guid? customerId { get; set;}
        public Guid? customerStorageId { get; set;}
        public string customer_Title { get; set;}
        public string fixture_Title { get; set;}
    }
}
