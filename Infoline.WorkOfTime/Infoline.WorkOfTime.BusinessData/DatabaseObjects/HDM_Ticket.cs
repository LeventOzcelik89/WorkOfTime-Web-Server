using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    /// <summary>
    /// Yardım taleplerinin tutulduğu tablodur.
    /// </summary>
    public partial class HDM_Ticket : InfolineTable
    {
        /// <summary>
        /// Talep kodunun tutulduğu alandır.
        /// </summary>
        public string code { get; set;}
        /// <summary>
        /// Talebin değerlendirme puanının tutulduğu alandır.
        /// </summary>
        public short? evaluation { get; set;}
        /// <summary>
        /// Talebin durumunun tutulduğu alandır.
        /// </summary>
        public short? status { get; set;}
        /// <summary>
        /// Talebin önceliğinin tutulduğu alandır.
        /// </summary>
        public short? priority { get; set;}
        /// <summary>
        /// Talebin nerden geldiğinin tutulduğu alandır.
        /// </summary>
        public short? channel { get; set;}
        /// <summary>
        /// Talebin konusunun tutulduğu alandır.
        /// </summary>
        public Guid? issueId { get; set;}
        /// <summary>
        /// Talebin sorununun tutulduğu alandır.
        /// </summary>
        public Guid? suggestionId { get; set;}
        /// <summary>
        /// Talep başlığının tutulduğu alandır.
        /// </summary>
        public string title { get; set;}
        /// <summary>
        /// Talep içeriğinin tutulduğu alandır.
        /// </summary>
        public string content { get; set;}
        /// <summary>
        /// Talebi yapan kişinin tutulduğu alandır.
        /// </summary>
        public Guid? requesterId { get; set;}
        /// <summary>
        /// Görevli kişinin tutulduğu alandır.
        /// </summary>
        public Guid? assignUserId { get; set;}
        public DateTime? dueDate { get; set;}
        public string evaluateDescription { get; set;}
    }
}
