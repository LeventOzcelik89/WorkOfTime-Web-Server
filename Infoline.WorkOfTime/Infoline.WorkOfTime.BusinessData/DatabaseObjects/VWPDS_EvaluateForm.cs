using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWPDS_EvaluateForm : InfolineTable
    {
        public int? formType { get; set;}
        public string formName { get; set;}
        public string formCode { get; set;}
        public bool? status { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string status_Title { get; set;}
        public string formType_Title { get; set;}
        public int? formResult { get; set;}
    }
}
