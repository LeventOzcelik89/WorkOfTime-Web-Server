using Infoline.Framework.Database;
using Infoline.Framework.Helper;
using Infoline.WorkOfTime.BusinessAccess.Models;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Infoline.WorkOfTime.BusinessAccess
{
    public class VMPRD_EntegrationActionModel : VWPRD_EntegrationAction
    {
        private WorkOfTimeDatabase db { get; set; }
        private DbTransaction trans { get; set; }
        public ResultStatus DistDataSourceReport { get; set; }
        public ResultStatus DistAndProductDataSourceReport { get; set; }
        public ResultStatus SellOutReportData(DateTime startDate, DateTime endDate)
        {
            db = db ?? new WorkOfTimeDatabase();
            var getData = db.GetPRD_SellOutReportDistrubitorQuery(startDate, endDate);

            return new ResultStatus
            {
                result = true,
                objects = getData
            };
        }
        public ResultStatus SellOutProductReportData(DateTime startDate, DateTime endDate)
        {
            db = db ?? new WorkOfTimeDatabase();
            var getData = db.GetPRD_SellOutProductReportDistrubitorQuery(startDate, endDate);

            return new ResultStatus
            {
                result = true,
                objects = getData
            };
        }
    }
    public class SellOutReportNewModel
    {
        public Guid DistributorId { get; set; }
        public string dataCompanyId_Title { get; set; }
        public int DistSalesCount { get; set; }
        public int SalesCount { get; set; }
        public int ActivatedData { get; set; }
        public string Types { get; set; }
        public int AssignmentCount { get; set; }
        public Guid pid { get; set; }
        public string pid_Title { get; set; }
        public Guid productId { get; set; }
        public string productId_Title { get; set; }

    }
}
