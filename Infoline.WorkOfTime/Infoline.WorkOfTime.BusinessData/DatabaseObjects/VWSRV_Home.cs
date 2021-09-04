using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWSRV_Home 
    {
        public string CardKey { get; set;}
        public string CardName { get; set;}
        public string TotalText { get; set;}
        public string Note { get; set;}
        public string NoteText { get; set;}
        public int? Total { get; set;}
    }
}
