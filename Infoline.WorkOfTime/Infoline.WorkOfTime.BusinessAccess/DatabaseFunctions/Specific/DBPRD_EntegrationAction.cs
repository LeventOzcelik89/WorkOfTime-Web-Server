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
            var list = new List<SellOutReportNewModel>();
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
                foreach (var dist in queryExecute.GroupBy(n => n.DistributorId))
                {
                    var distrubitorList = queryExecute.Where(a => a.DistributorId == dist.Key);
                    foreach (var product in distrubitorList)
                    {
                        list.Add(new SellOutReportNewModel
                        {
                            SalesCount = queryExecute.Where(a => a.DistributorId == dist.Key && a.ProductId == product.ProductId).Sum(c => c.SalesCount),
                            DistributorName = queryExecute.Where(a => a.DistributorId == dist.Key).Select(c => c.DistributorName).FirstOrDefault(),
                            DistributorId = dist.Key,
                            ActivatedData = query3Execute.Where(a => a.DistributorId == dist.Key && a.ProductId == product.ProductId).Sum(c => c.ActivatedData),
                            AssignmentCount = query4Execute.Where(a => a.DistributorId == dist.Key && a.ProductId == product.ProductId).Sum(c => c.AssignmentCount),
                            DistSalesCount = query2Execute.Where(a => a.DistributorId == dist.Key && a.ProductId == product.ProductId).Sum(c => c.DistSalesCount),
                            ProductName = GetPRD_ProductById(product.ProductId).name,
                            ProductId = product.ProductId,
                        });
                    }
                }
            }
            return list.ToArray();
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

        public SellOutReportNewModel[] GetPRD_SellOutReportByDistrubitorDetail(DateTime startDate, DateTime endDate, Guid DistrubitorId)
        {
            var data = new List<SellOutReportNewModel>();
            string startDateSql = startDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            string endDateSql = endDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            using (var db = GetDB())
            {
                var EntegrationActionQuery = $@"select * from VWPRD_EntegrationActionSummary where DistributorId = '" + DistrubitorId + "' and date >= '" + startDateSql + "' and date <= '" + endDateSql + "'";
                var IntegrationActionQuery = $@"select * from VWPRD_InventoryActionSummary where DistributorId = '" + DistrubitorId + "' and date >= '" + startDateSql + "' and date <= '" + endDateSql + "'";
                var TitanDeviceActivatedQuery = $@"select * from VWPRD_TitanDeviceActivatedSummary where DistributorId = '" + DistrubitorId + "' and date >= '" + startDateSql + "' and date <= '" + endDateSql + "'";
                var ProductProgressPaymentQuery = $@"select * from VWPRD_ProductProgressPaymentImportSummary where DistributorId = '" + DistrubitorId + "' and date >= '" + startDateSql + "' and date <= '" + endDateSql + "'";
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

        public SellOutReportNewModel[] GetPRD_SellOutReportByDealar(DateTime startDate, DateTime endDate, Guid DistrubitorId)
        {
            var data = new List<SellOutReportNewModel>();
            string startDateSql = startDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            string endDateSql = endDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            using (var db = GetDB())
            {
                var EntegrationActionQuery = $@"select * from VWPRD_EntegrationActionSummary where DistributorId = '" + DistrubitorId + "' and date >= '" + startDateSql + "' and date <= '" + endDateSql + "'";
                var IntegrationActionQuery = $@"select * from VWPRD_InventoryActionSummary where DistributorId = '" + DistrubitorId + "' and date >= '" + startDateSql + "' and date <= '" + endDateSql + "'";
                var TitanDeviceActivatedQuery = $@"select * from VWPRD_TitanDeviceActivatedSummary where DistributorId = '" + DistrubitorId + "' and date >= '" + startDateSql + "' and date <= '" + endDateSql + "'";
                var ProductProgressPaymentDealarQuery = $@"select * from VWPRD_ProductProgressPaymentImportDealarSummary where DistributorId = '" + DistrubitorId + "' and date >= '" + startDateSql + "' and date <= '" + endDateSql + "'";
                var queryExecute = db.ExecuteReader<SellOutReportNewModel>(EntegrationActionQuery.ToString()).ToArray();
                var query3Execute = db.ExecuteReader<SellOutReportNewModel>(TitanDeviceActivatedQuery.ToString()).ToArray();
                var query4Execute = db.ExecuteReader<SellOutReportNewModel>(ProductProgressPaymentDealarQuery.ToString()).ToArray();
                data.AddRange(query4Execute.GroupBy(v => v.DealarId).Select(a => new SellOutReportNewModel
                {
                    DistributorId = a.Where(b => b.DistributorId == DistrubitorId).Select(c => c.DistributorId).FirstOrDefault(),
                    DistributorName = a.Select(b => b.DistributorName).FirstOrDefault(),
                    DealarId = a.Key,
                    DealarName = GetCMP_CompanyById(a.Key).name,
                    SalesCount = queryExecute.Where(b => b.DistributorId == a.Key).Sum(c => c.SalesCount),
                    ActivatedData = query3Execute.Where(b => b.DistributorId == a.Key).Sum(c => c.ActivatedData),
                    AssignmentCount = query4Execute.Where(b => b.DealarId == a.Key).Sum(c => c.AssignmentCount),
                }));
            }
            return data.ToArray();
        }

        public SellOutReportNewModel[] GetPRD_SellOutProductReportDealarQuery(DateTime startDate, DateTime endDate, Guid DistrubitorId)
        {
            var data = new List<SellOutReportNewModel>();
            string startDateSql = startDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            string endDateSql = endDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            var list = new List<SellOutReportNewModel>();
            using (var db = GetDB())
            {
                var EntegrationActionQuery = $@"select * from VWPRD_EntegrationActionSummary where DistributorId = '" + DistrubitorId + "' and date >= '" + startDateSql + "' and date <= '" + endDateSql + "'";
                var IntegrationActionQuery = $@"select * from VWPRD_InventoryActionSummary where DistributorId = '" + DistrubitorId + "' and date >= '" + startDateSql + "' and date <= '" + endDateSql + "'";
                var TitanDeviceActivatedQuery = $@"select * from VWPRD_TitanDeviceActivatedSummary where DistributorId = '" + DistrubitorId + "' and date >= '" + startDateSql + "' and date <= '" + endDateSql + "'";
                var ProductProgressPaymentDealarQuery = $@"select * from VWPRD_ProductProgressPaymentImportDealarSummary where DistributorId = '" + DistrubitorId + "' and date >= '" + startDateSql + "' and date <= '" + endDateSql + "'";
                var queryExecute = db.ExecuteReader<SellOutReportNewModel>(EntegrationActionQuery.ToString()).ToArray();
                var query3Execute = db.ExecuteReader<SellOutReportNewModel>(TitanDeviceActivatedQuery.ToString()).ToArray();
                var query4Execute = db.ExecuteReader<SellOutReportNewModel>(ProductProgressPaymentDealarQuery.ToString()).ToArray();
                foreach (var dist in query4Execute.GroupBy(n => n.DealarId))
                {
                    var distrubitorList = query4Execute.Where(a => a.DealarId == dist.Key);
                    foreach (var product in distrubitorList.GroupBy(v=>v.ProductId))
                    {
                        list.Add(new SellOutReportNewModel
                        {
                            SalesCount = queryExecute.Where(a => a.DistributorId == dist.Key && a.ProductId == product.Key).Sum(c => c.SalesCount),
                            DistributorName = queryExecute.Where(a => a.DistributorId == dist.Key).Select(c => c.DistributorName).FirstOrDefault(),
                            DistributorId = query4Execute.Where(a => a.DealarId == dist.Key).Select(c => c.DistributorId).FirstOrDefault(),
                            DealarId = dist.Key,
                            DealarName = GetCMP_CompanyById(dist.Key).name,
                            ActivatedData = query3Execute.Where(a => a.DistributorId == dist.Key && a.ProductId == product.Key).Sum(c => c.ActivatedData),
                            AssignmentCount = query4Execute.Where(a => a.DealarId == dist.Key && a.ProductId == product.Key).Sum(c => c.AssignmentCount),
                            ProductName = GetPRD_ProductById(product.Key).name,
                            ProductId = product.Key,
                        });
                    }
                }
            }
            return list.ToArray();
        }
    }
}
