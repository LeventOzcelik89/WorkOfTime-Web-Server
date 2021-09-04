using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    /// <summary>
    /// Görev formlarınn tutulduğu tablodur.
    /// </summary>
    public partial class FTM_TaskForm : InfolineTable
    {
        /// <summary>
        /// Form isminin tutulduğu alandır
        /// </summary>
        public string name { get; set;}
        /// <summary>
        /// Form kodunun tutulduğu alandır
        /// </summary>
        public string code { get; set;}
        /// <summary>
        /// Form tipinin tutulduğu alandır(Arıza,Bakım,Kalibrasyon)
        /// </summary>
        public short? type { get; set;}
        /// <summary>
        /// Form sorularının tutulduğu json objesidir.
        /// </summary>
        public string json { get; set;}
        public short? isActive { get; set;}
    }
}
