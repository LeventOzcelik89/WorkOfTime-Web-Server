using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class CRM_PresentationAction : InfolineTable
    {
        /// <summary>
        /// İşlemin açıklamasının tutulduğu alandır.
        /// </summary>
        public string description { get; set;}
        /// <summary>
        /// İşlemin yapıldığı fırsatın tutulduğu alandır.
        /// </summary>
        public Guid? presentationId { get; set;}
        /// <summary>
        /// İşlemin tipi tutulur.
        /// </summary>
        public short? type { get; set;}
        public string color { get; set;}
        /// <summary>
        /// CRM_Contact tablosunun id'sinin tutulduğu alandır.
        /// </summary>
        public Guid? contactId { get; set;}
        public IGeometry  location { get; set;}
    }
}
