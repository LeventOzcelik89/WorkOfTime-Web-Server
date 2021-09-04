using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class PDS_EvaluateForm : InfolineTable
    {
        public int? formType { get; set;}
        public string formName { get; set;}
        public string formCode { get; set;}
        public bool? status { get; set;}
    }
}
