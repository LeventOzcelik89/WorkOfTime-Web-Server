using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWCMP_InvoiceDocumentTemplate : InfolineTable
    {
        public short? templateVisibleAllUser { get; set;}
        public string name { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string templateVisibleAllUser_Title { get; set;}
    }
}
