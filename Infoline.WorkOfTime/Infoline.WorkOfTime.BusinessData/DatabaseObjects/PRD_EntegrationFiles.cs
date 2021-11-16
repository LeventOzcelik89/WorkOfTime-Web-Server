using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class PRD_EntegrationFiles : InfolineTable
    {
        /// <summary>
        /// Ftp'deki dosya adı 
        /// </summary>
        public string FileName { get; set;}
        /// <summary>
        /// Ftp Dosya Tarihi 
        /// </summary>
        public DateTime? FileNameDate { get; set;}
        public string FileTypeName { get; set;}
        /// <summary>
        /// Ftpdeki dosyanın oluşturma tarihi 
        /// </summary>
        public DateTime? CreateDateInFtp { get; set;}
        /// <summary>
        /// Distribütör Cari Karşılığı
        /// </summary>
        public Guid? DistributorId { get; set;}
        /// <summary>
        /// Distribütör Adı 
        /// </summary>
        public string DistributorName { get; set;}
        /// <summary>
        /// Ajanın çalışma tarihi tarihi 
        /// </summary>
        public DateTime? ProcessTime { get; set;}
        /// <summary>
        /// Dosya okurken hata oldu mu ?
        /// </summary>
        public bool? ResultFilesReading { get; set;}
        /// <summary>
        /// Dosya okurken hata olduysa hata mesajı 
        /// </summary>
        public string ResultFilesErrorMessage { get; set;}
    }
}
