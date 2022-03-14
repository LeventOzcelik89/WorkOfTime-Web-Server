using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infoline.WorkOfTime.BusinessAccess.Models
{
    public class SellOutReportModel
    {
        public string Name { get; set; }
        public int Count { get; set; }
        public Guid? Id { get; set; }
        public string Types { get; set; }
        public int SellingCount { get; set; }

    }

    public class SellOutBasedProductModel
    {
        public string Name { get; set; }
        public int SalesCount { get; set; }
        public int ActivatedCount { get; set; }
    }

    public class SellOutReportsModel
    {
        public Guid DistributorId { get; set; }
        public string dataCompanyId_Title { get; set; }
        public int DistSalesCount { get; set; }
        public int SalesCount { get; set; }
        public int ActivatedData { get; set; }
        public string Types { get; set; }
    }
}
