using Infoline.Framework.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
namespace Infoline.WorkOfTime.BusinessData
{
    
    public class PRD_ProductProgressPaymentImportExcel
    {
        [ColumnInfoAttribute("Imei**"),Required(ErrorMessage ="Imei numarası alanı zorunludur.")]
        public string imei { get; set; }
        [ColumnInfoAttribute("Satış Tarihi"), Required(ErrorMessage = "Satış tarihi alanı zorunludur.")]
        public DateTime? date { get; set; }
        [ColumnInfoAttribute("Bayi Kodu"), Required(ErrorMessage = "Bayi kodu alanı zorunludur.")]
        public string companyCode { get; set; }
        [ColumnInfoAttribute("Bayi Adı"), Required(ErrorMessage = "Bayi Adı alanı zorunludur.")]
        public string companyName { get; set; }
    }
}