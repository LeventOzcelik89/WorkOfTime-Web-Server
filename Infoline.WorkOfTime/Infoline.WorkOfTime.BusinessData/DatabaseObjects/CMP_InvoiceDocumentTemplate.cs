using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class CMP_InvoiceDocumentTemplate : InfolineTable
    {
        public string name { get; set;}
        public string cover { get; set;}
        public string description { get; set;}
        public string tenderConditions { get; set;}
        public short? templateVisibleAllUser { get; set;}
    }
}
