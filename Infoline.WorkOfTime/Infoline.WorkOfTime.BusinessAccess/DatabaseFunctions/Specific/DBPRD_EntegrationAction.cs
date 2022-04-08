using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infoline.Framework.Database;
using System.Data.SqlClient;
using System.Linq.Expressions;
using System.Data.Common;
using Infoline.WorkOfTime.BusinessData;
using System.Globalization;

namespace Infoline.WorkOfTime.BusinessAccess
{
    partial class WorkOfTimeDatabase
    {
        public PRD_EntegrationAction GetPRD_EntegrationActionByRepetitive(string imei, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<PRD_EntegrationAction>().Where(a => a.Imei == imei && a.Quantity == 1).Execute().OrderByDescending(a => a.created).FirstOrDefault();
            }
        }

        public PRD_EntegrationAction GetPRD_EntegrationActionByImei(string imei, Guid? companyId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<PRD_EntegrationAction>().Where(a => a.Imei == imei).Execute().FirstOrDefault();
            }
        }
        public SellOutReportNewModel[] GetPRD_SellOutProductReportDistrubitorQuery(DateTime startDate, DateTime endDate)
        {
            var data = new List<SellOutReportNewModel>();
            string startDateSql = startDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            string endDateSql = endDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            using (var db = GetDB())
            {
                var EntegrationActionQuery = $@"select * from VWPRD_EntegrationActionSummary where date >= '" + startDateSql + "' and date <= '" + endDateSql + "'";
                var IntegrationActionQuery = $@"select * from VWPRD_InventoryActionSummary where date >= '" + startDateSql + "' and date <= '" + endDateSql + "'";
                var TitanDeviceActivatedQuery = $@"select * from VWPRD_TitanDeviceActivatedSummary where date >= '" + startDateSql + "' and date <= '" + endDateSql + "'";
                var ProductProgressPaymentQuery = $@"select * from VWPRD_ProductProgressPaymentImportSummary where date >= '" + startDateSql + "' and date <= '" + endDateSql + "'";
                var queryExecute = db.ExecuteReader<SellOutReportNewModel>(EntegrationActionQuery.ToString()).ToArray();
                var query2Execute = db.ExecuteReader<SellOutReportNewModel>(IntegrationActionQuery.ToString()).ToArray();
                var query3Execute = db.ExecuteReader<SellOutReportNewModel>(TitanDeviceActivatedQuery.ToString()).ToArray();
                var query4Execute = db.ExecuteReader<SellOutReportNewModel>(ProductProgressPaymentQuery.ToString()).ToArray();
                data.AddRange(queryExecute.GroupBy(v => v.ProductId).Select(a => new SellOutReportNewModel
                {
                    DistributorId = a.Select(b => b.DistributorId).FirstOrDefault(),
                    DistributorName = a.Select(b => b.DistributorName).FirstOrDefault(),
                    SalesCount = queryExecute.Where(b => b.DistributorId == a.Select(v => v.DistributorId).FirstOrDefault()).Sum(c => c.SalesCount),
                    ActivatedData = query3Execute.Where(b => b.DistributorId == a.Select(v => v.DistributorId).FirstOrDefault()).Sum(c => c.ActivatedData),
                    AssignmentCount = query4Execute.Where(b => b.DistributorId == a.Select(v => v.DistributorId).FirstOrDefault()).Sum(c => c.AssignmentCount),
                    DistSalesCount = query2Execute.Where(b => b.DistributorId == a.Select(v => v.DistributorId).FirstOrDefault()).Sum(c => c.DistSalesCount),
                    ProductId = a.Key,
                    ProductName = a.Select(b => GetPRD_ProductById(a.Key).name).FirstOrDefault()
                }));
            }
            return data.ToArray();
        }

        public SellOutReportNewModel[] GetPRD_SellOutReportByDistrubitor(DateTime startDate, DateTime endDate)
        {
            var data = new List<SellOutReportNewModel>();
            string startDateSql = startDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            string endDateSql = endDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            using (var db = GetDB())
            {
                var EntegrationActionQuery = $@"select * from VWPRD_EntegrationActionSummary where date >= '" + startDateSql + "' and date <= '" + endDateSql + "'";
                var IntegrationActionQuery = $@"select * from VWPRD_InventoryActionSummary where date >= '" + startDateSql + "' and date <= '" + endDateSql + "'";
                var TitanDeviceActivatedQuery = $@"select * from VWPRD_TitanDeviceActivatedSummary where date >= '" + startDateSql + "' and date <= '" + endDateSql + "'";
                var ProductProgressPaymentQuery = $@"select * from VWPRD_ProductProgressPaymentImportSummary where date >= '" + startDateSql + "' and date <= '" + endDateSql + "'";
                var queryExecute = db.ExecuteReader<SellOutReportNewModel>(EntegrationActionQuery.ToString()).ToArray();
                var query2Execute = db.ExecuteReader<SellOutReportNewModel>(IntegrationActionQuery.ToString()).ToArray();
                var query3Execute = db.ExecuteReader<SellOutReportNewModel>(TitanDeviceActivatedQuery.ToString()).ToArray();
                var query4Execute = db.ExecuteReader<SellOutReportNewModel>(ProductProgressPaymentQuery.ToString()).ToArray();
                data.AddRange(queryExecute.GroupBy(v => v.DistributorId).Select(a => new SellOutReportNewModel
                {
                    DistributorId = a.Select(b => b.DistributorId).FirstOrDefault(),
                    DistributorName = a.Select(b => b.DistributorName).FirstOrDefault(),
                    SalesCount = queryExecute.Where(b => b.DistributorId == a.Key).Sum(c => c.SalesCount),
                    ActivatedData = query3Execute.Where(b => b.DistributorId == a.Key).Sum(c => c.ActivatedData),
                    AssignmentCount = query4Execute.Where(b => b.DistributorId == a.Key).Sum(c => c.AssignmentCount),
                    DistSalesCount = query2Execute.Where(b => b.DistributorId == a.Key).Sum(c => c.DistSalesCount),

                }));
            }
            return data.ToArray();
        }
    }
}
