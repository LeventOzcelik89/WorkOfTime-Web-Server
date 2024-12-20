﻿using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class SV_Customer : InfolineTable
    {
        /// <summary>
        /// Müşteri Adı
        /// </summary>
        public string name { get; set;}
        /// <summary>
        /// Müşteri Soyadı
        /// </summary>
        public string lastName { get; set;}
        /// <summary>
        /// Müşteri Telefon Numarası
        /// </summary>
        public string phoneNumber { get; set;}
        public string email { get; set;}
        /// <summary>
        /// Müşteri 2. telefon numarası
        /// </summary>
        public string otherPhoneNumber { get; set;}
        /// <summary>
        /// Açık Adres Alanı
        /// </summary>
        public Guid? openLocationId { get; set;}
        /// <summary>
        /// Açık Adres Detay
        /// </summary>
        public string Address { get; set;}
        public string code { get; set;}
        public IGeometry  location { get; set;}
    }
}
