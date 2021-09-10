using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infoline.WorkOfTime.BusinessData.Specific
{
    public class VWPRD_CompanyBasedPriceDetailDto
    {
        public Guid? id { get; set; }
        public Guid? companyBasedPriceId { get; set; }
        public double? minCondition { get; set; }
        public short? type { get; set; }      
        public double? discount { get; set; }
        public int? monthCount { get; set; }
        [DataType(DataType.Date)]
        public DateTime? startDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime? endDate { get; set; }
        public double? price { get; set; }
        public string createdby_Title { get; set; }
        public string changedby_Title { get; set; }
        public string companyId_Title { get; set; }
        public Guid? companyId { get; set; }
        public string productId_Title { get; set; }
        public Guid? productId { get; set; }
        public string categoryId_Title { get; set; }
        public Guid? categoryId { get; set; }
        public short? conditionType { get; set; }
        public short? sellingType { get; set; }
        public double? basePrice{ get; set; }
        public double afterPrice { get; set; }
        public bool isPrice { get; set; }
    }
}
