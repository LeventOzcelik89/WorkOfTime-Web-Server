using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    /// <summary>
    /// Görev operasyonu esnasında doldurulan formların cevaplarının tutulduğu tablodur.
    /// </summary>
    public partial class FTM_TaskFormResult : InfolineTable
    {
        /// <summary>
        /// Formun cevaplarının hangi görev için tutulduğu alandır.
        /// </summary>
        public Guid? taskId { get; set;}
        /// <summary>
        /// Formun cevaplarının hangi görev operastonu için tutulduğu alandır.
        /// </summary>
        public Guid? taskOperationId { get; set;}
        /// <summary>
        /// Formun idsisinin tutulduğu alandır.
        /// </summary>
        public Guid? formId { get; set;}
        /// <summary>
        /// Formun cevaplarının tutulduğu json objesidir.
        /// </summary>
        public string jsonResult { get; set;}
    }
}
