﻿using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWPRD_StockSummary 
    {
        public Guid? id { get; set;}
        public DateTime? created { get; set;}
        public double? quantity { get; set;}
        public short? status { get; set;}
        public Guid? stockId { get; set;}
        public string stockTable { get; set;}
        public Guid? stockCompanyId { get; set;}
        public string stockId_Title { get; set;}
        public string stockCompanyId_Title { get; set;}
        public Guid? productId { get; set;}
        public string productId_Title { get; set;}
        public string productId_Image { get; set;}
        public string unitId_Title { get; set;}
        public short? stockType { get; set;}
        public string categoryId_Title { get; set;}
        public string searchField { get; set;}
    }
}