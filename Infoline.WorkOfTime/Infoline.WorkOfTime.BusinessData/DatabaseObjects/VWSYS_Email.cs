using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWSYS_Email : InfolineTable
    {
        public string SendingMail { get; set;}
        public string SendingTitle { get; set;}
        public string SendingMessage { get; set;}
        public bool? SendingIsBodyHtml { get; set;}
        public string Result { get; set;}
        public bool? Status { get; set;}
    }
}
