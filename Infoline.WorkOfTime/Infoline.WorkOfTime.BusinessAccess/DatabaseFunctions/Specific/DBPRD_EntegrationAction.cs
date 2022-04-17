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
            var newList = new List<SellOutReportNewModel>();
            using (var db = GetDB())
            {
                var EntegrationActionQuery = $@"select * from VWPRD_EntegrationActionSummary where date >= '" + startDateSql + "' and date <= '" + endDateSql + "'";
                var IntegrationActionQuery = $@"select * from VWPRD_InventoryActionSummary where date >= '" + startDateSql + "' and date <= '" + endDateSql + "'";
                var TitanDeviceActivatedQuery = $@"select * from VWPRD_TitanDeviceActivatedSummary where date >= '" + startDateSql + "' and date <= '" + endDateSql + "'";
                var ProductProgressPaymentQuery = $@"select * from VWPRD_ProductProgressPaymentImportDealarSummary where date >= '" + startDateSql + "' and date <= '" + endDateSql + "'";
                var queryExecute = db.ExecuteReader<SellOutReportNewModel>(EntegrationActionQuery.ToString()).ToArray();
                var query2Execute = db.ExecuteReader<SellOutReportNewModel>(IntegrationActionQuery.ToString()).ToArray();
                var query3Execute = db.ExecuteReader<SellOutReportNewModel>(TitanDeviceActivatedQuery.ToString()).ToArray();
                var query4Execute = db.ExecuteReader<SellOutReportNewModel>(ProductProgressPaymentQuery.ToString()).ToArray();
                list.AddRange(queryExecute);
                list.AddRange(query2Execute);
                list.AddRange(query3Execute);
                list.AddRange(query4Execute);
                var groupList = list.Where(b => b.DistributorName != null).GroupBy(n => n.DistributorId);
                foreach (var dist in groupList)
                {
                    var distrubitorList = list.Where(a => a.DistributorId == dist.Key);
                    var distrubitorGroup = distrubitorList.GroupBy(a => a.ProductId).ToArray();
                    foreach (var product in distrubitorGroup)
                    {
                        newList.Add(new SellOutReportNewModel
                        {
                            SalesCount = queryExecute.Where(a => a.DistributorId == dist.Key && a.ProductId == product.Key).Sum(c => c.SalesCount),
                            DistributorName = queryExecute.Where(a => a.DistributorId == dist.Key).Select(c => c.DistributorName).FirstOrDefault(),
                            DistributorId = dist.Key,
                            ActivatedData = query3Execute.Where(a => a.DistributorId == dist.Key && a.ProductId == product.Key).Sum(c => c.ActivatedData),
                            AssignmentCount = query4Execute.Where(a => a.DistributorId == dist.Key && a.ProductId == product.Key).Sum(c => c.AssignmentCount),
                            DistSalesCount = query2Execute.Where(a => a.DistributorId == dist.Key && a.ProductId == product.Key).Sum(c => c.DistSalesCount),
                            ProductName = GetPRD_ProductById(product.Key).name,
                            ProductId = product.Key,
                        });
                    }
                }
            }
            return newList.ToArray();
        }

        public SellOutReportNewModel[] GetPRD_SellOutReportByDistrubitor(DateTime startDate, DateTime endDate)
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
                var ProductProgressPaymentQuery = $@"select * from VWPRD_ProductProgressPaymentImportDealarSummary where date >= '" + startDateSql + "' and date <= '" + endDateSql + "'";
                var queryExecute = db.ExecuteReader<SellOutReportNewModel>(EntegrationActionQuery.ToString()).ToArray();
                var query2Execute = db.ExecuteReader<SellOutReportNewModel>(IntegrationActionQuery.ToString()).ToArray();
                var query3Execute = db.ExecuteReader<SellOutReportNewModel>(TitanDeviceActivatedQuery.ToString()).ToArray();
                var query4Execute = db.ExecuteReader<SellOutReportNewModel>(ProductProgressPaymentQuery.ToString()).ToArray();
                list.AddRange(queryExecute);
                list.AddRange(query2Execute);
                list.AddRange(query3Execute);
                list.AddRange(query4Execute);
                data.AddRange(list.Where(b => b.DistributorName != null).GroupBy(v => v.DistributorId).Select(a => new SellOutReportNewModel
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
                var ProductProgressPaymentQuery = $@"select * from VWPRD_ProductProgressPaymentImportDealarSummary where DistributorId = '" + DistrubitorId + "' and date >= '" + startDateSql + "' and date <= '" + endDateSql + "'";
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
            var list = new List<SellOutReportNewModel>();
            using (var db = GetDB())
            {
                var EntegrationActionQuery = $@"select * from VWPRD_EntegrationActionSummary where DistributorId = '" + DistrubitorId + "' and date >= '" + startDateSql + "' and date <= '" + endDateSql + "'";
                var TitanDeviceActivatedQuery = $@"select * from VWPRD_TitanDeviceActivatedSummary where DistributorId = '" + DistrubitorId + "' and date >= '" + startDateSql + "' and date <= '" + endDateSql + "'";
                var ProductProgressPaymentDealarQuery = $@"select * from VWPRD_ProductProgressPaymentImportDealarSummary where DistributorId = '" + DistrubitorId + "' and date >= '" + startDateSql + "' and date <= '" + endDateSql + "'";
                var queryExecute = db.ExecuteReader<SellOutReportNewModel>(EntegrationActionQuery.ToString()).ToArray();
                var query3Execute = db.ExecuteReader<SellOutReportNewModel>(TitanDeviceActivatedQuery.ToString()).ToArray();
                var query4Execute = db.ExecuteReader<SellOutReportNewModel>(ProductProgressPaymentDealarQuery.ToString()).ToArray();
                list.AddRange(queryExecute);
                list.AddRange(query3Execute);
                list.AddRange(query4Execute);
                data.AddRange(list.Where(b => b.DealarId != null).GroupBy(v => v.DealarId).Select(a => new SellOutReportNewModel
                {
                    DistributorId = a.Where(b => b.DistributorId == DistrubitorId).Select(c => c.DistributorId).FirstOrDefault(),
                    DistributorName = a.Select(b => b.DistributorName).FirstOrDefault(),
                    DealarId = a.Key,
                    DealarName = a.Key != System.Guid.Empty ? GetCMP_CompanyById(a.Key)?.name : GetCMP_CompanyById(DistrubitorId)?.name,
                    SalesCount = queryExecute.Where(b => b.DealarId == a.Key).Sum(c => c.SalesCount),
                    ActivatedData = query3Execute.Where(b => b.DealarId == a.Key).Sum(c => c.ActivatedData),
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
            var newList = new List<SellOutReportNewModel>();
            using (var db = GetDB())
            {
                var EntegrationActionQuery = $@"select * from VWPRD_EntegrationActionSummary where DistributorId = '" + DistrubitorId + "' and date >= '" + startDateSql + "' and date <= '" + endDateSql + "'";
                var TitanDeviceActivatedQuery = $@"select * from VWPRD_TitanDeviceActivatedSummary where DistributorId = '" + DistrubitorId + "' and date >= '" + startDateSql + "' and date <= '" + endDateSql + "'";
                var ProductProgressPaymentDealarQuery = $@"select * from VWPRD_ProductProgressPaymentImportDealarSummary where DistributorId = '" + DistrubitorId + "' and date >= '" + startDateSql + "' and date <= '" + endDateSql + "'";
                var queryExecute = db.ExecuteReader<SellOutReportNewModel>(EntegrationActionQuery.ToString()).ToArray();
                var query3Execute = db.ExecuteReader<SellOutReportNewModel>(TitanDeviceActivatedQuery.ToString()).ToArray();
                var query4Execute = db.ExecuteReader<SellOutReportNewModel>(ProductProgressPaymentDealarQuery.ToString()).ToArray();
                list.AddRange(queryExecute);
                list.AddRange(query3Execute);
                list.AddRange(query4Execute);
                var groupList = list.Where(b => b.DealarId != null).GroupBy(n => n.DealarId);
                foreach (var dist in groupList)
                {
                    var distrubitorList = list.Where(a => a.DealarId == dist.Key);
                    var distrubitorGroup = distrubitorList.GroupBy(a => a.ProductId).ToArray();
                    foreach (var product in distrubitorGroup)
                    {
                        newList.Add(new SellOutReportNewModel
                        {
                            DistributorName = queryExecute.Where(a => a.DistributorId == DistrubitorId).Select(c => c.DistributorName).FirstOrDefault(),
                            DistributorId = query4Execute.Where(a => a.DistributorId == DistrubitorId).Select(c => c.DistributorId).FirstOrDefault(),
                            DealarId = dist.Key,
                            DealarName = dist.Key != System.Guid.Empty ? GetCMP_CompanyById(dist.Key)?.name : GetCMP_CompanyById(DistrubitorId)?.name,
                            SalesCount = queryExecute.Where(a => a.DealarId == dist.Key && a.DistributorId == DistrubitorId && a.ProductId == product.Key).Sum(c => c.SalesCount),
                            ActivatedData = query3Execute.Where(a => a.DealarId == dist.Key && a.DistributorId == DistrubitorId && a.ProductId == product.Key).Sum(c => c.ActivatedData),
                            AssignmentCount = query4Execute.Where(a => a.DealarId == dist.Key && a.DistributorId == DistrubitorId && a.ProductId == product.Key).Sum(c => c.AssignmentCount),
                            ProductName = GetPRD_ProductById(product.Key).name,
                            ProductId = product.Key,
                        });
                    }
                }

            }

            return newList.ToArray();
        }



        public DistStockReportModel[] GetPRD_DistStockReportByDistrubitor()
        {
            var data = new List<DistStockReportModel>();
            var list = new List<DistStockReportModel>();
            using (var db = GetDB())
            {
                var EntegrationActionQuery = $@"select * from VWPRD_EntegrationActionSummary";
                var IntegrationActionQuery = $@"select * from VWPRD_InventoryActionSummary";
                var queryExecute = db.ExecuteReader<DistStockReportModel>(EntegrationActionQuery.ToString()).ToArray();
                var query2Execute = db.ExecuteReader<DistStockReportModel>(IntegrationActionQuery.ToString()).ToArray();
                list.AddRange(queryExecute);
                list.AddRange(query2Execute);
                var groupList = list.Where(b => b.DistributorName != null).GroupBy(v => v.DistributorId);
                data.AddRange(groupList.Select(a => new DistStockReportModel
                {
                    DistributorId = a.Select(b => b.DistributorId).FirstOrDefault(),
                    DistributorName = a.Select(b => b.DistributorName).FirstOrDefault(),
                    SalesCount = queryExecute.Where(b => b.DistributorId == a.Key).Sum(c => c.SalesCount),
                    DistSalesCount = query2Execute.Where(b => b.DistributorId == a.Key).Sum(c => c.DistSalesCount),
                    TotalStock = (query2Execute.Where(b => b.DistributorId == a.Key).Sum(c => c.DistSalesCount) - queryExecute.Where(b => b.DistributorId == a.Key).Sum(c => c.SalesCount)),
                }));
            }
            return data.ToArray();
        }

        public DistStockReportModel[] GetPRD_StockProductReportDistrubitorQuery()
        {
            var data = new List<DistStockReportModel>();
            var list = new List<DistStockReportModel>();
            var newList = new List<DistStockReportModel>();
            using (var db = GetDB())
            {
                var EntegrationActionQuery = $@"select * from VWPRD_EntegrationActionSummary";
                var IntegrationActionQuery = $@"select * from VWPRD_InventoryActionSummary";
                var queryExecute = db.ExecuteReader<DistStockReportModel>(EntegrationActionQuery.ToString()).ToArray();
                var query2Execute = db.ExecuteReader<DistStockReportModel>(IntegrationActionQuery.ToString()).ToArray();
                list.AddRange(queryExecute);
                list.AddRange(query2Execute);
                var groupList = list.Where(b => b.DistributorName != null).GroupBy(n => n.DistributorId);
                foreach (var dist in groupList)
                {
                    var distrubitorList = list.Where(a => a.DistributorId == dist.Key);
                    var distrubitorGroup = distrubitorList.GroupBy(a => a.ProductId).ToArray();
                    foreach (var product in distrubitorGroup)
                    {
                        newList.Add(new DistStockReportModel
                        {
                            DistributorName = queryExecute.Where(a => a.DistributorId == dist.Key).Select(c => c.DistributorName).FirstOrDefault(),
                            DistributorId = dist.Key,
                            SalesCount = queryExecute.Where(a => a.DistributorId == dist.Key && a.ProductId == product.Key).Sum(c => c.SalesCount),
                            DistSalesCount = query2Execute.Where(a => a.DistributorId == dist.Key && a.ProductId == product.Key).Sum(c => c.DistSalesCount),
                            ProductName = GetPRD_ProductById(product.Key).name,
                            ProductId = product.Key,
                            TotalStock = (query2Execute.Where(a => a.DistributorId == dist.Key && a.ProductId == product.Key).Sum(c => c.DistSalesCount) - queryExecute.Where(a => a.DistributorId == dist.Key && a.ProductId == product.Key).Sum(c => c.SalesCount))
                        });
                    }
                }
            }
            return newList.ToArray();
        }



        public DistStockReportModel[] GetPRD_StockReportByDistrubitorDetail(Guid DistrubitorId)
        {
            var data = new List<DistStockReportModel>();
            var list = new List<DistStockReportModel>();
            using (var db = GetDB())
            {
                var EntegrationActionQuery = $@"select * from VWPRD_EntegrationActionSummary where DistributorId = '" + DistrubitorId + "'";
                var IntegrationActionQuery = $@"select * from VWPRD_ProductProgressPaymentImportDealarSummary where DistributorId = '" + DistrubitorId + "'";
                var queryExecute = db.ExecuteReader<DistStockReportModel>(EntegrationActionQuery.ToString()).ToArray();
                var query2Execute = db.ExecuteReader<DistStockReportModel>(IntegrationActionQuery.ToString()).ToArray();
                list.AddRange(queryExecute);
                list.AddRange(query2Execute);
                data.AddRange(list.Where(b => b.DistributorId != null).GroupBy(v => v.DistributorId).Select(a => new DistStockReportModel
                {
                    DistributorId = a.Select(b => b.DistributorId).FirstOrDefault(),
                    DistributorName = a.Select(b => b.DistributorName).FirstOrDefault(),
                    SalesCount = queryExecute.Where(b => b.DistributorId == a.Key).Sum(c => c.SalesCount),
                    AssignmentCount = query2Execute.Where(b => b.DistributorId == a.Key).Sum(c => c.AssignmentCount),
                    TotalStock = (queryExecute.Where(b => b.DistributorId == a.Key).Sum(c => c.SalesCount) - query2Execute.Where(b => b.DistributorId == a.Key).Sum(c => c.AssignmentCount)),
                }));
            }
            return data.ToArray();
        }

        public DistStockReportModel[] GetPRD_StockReportByDealar(Guid DistrubitorId)
        {
            var data = new List<DistStockReportModel>();
            var list = new List<DistStockReportModel>();
            using (var db = GetDB())
            {
                var EntegrationActionQuery = $@"select * from VWPRD_EntegrationActionSummary where DistributorId = '" + DistrubitorId + "'";
                var Temlik = $@"select * from VWPRD_ProductProgressPaymentImportDealarSummary where DistributorId = '" + DistrubitorId + "'";
                var query2Execute = db.ExecuteReader<DistStockReportModel>(Temlik.ToString()).ToArray();
                var queryExecute = db.ExecuteReader<DistStockReportModel>(EntegrationActionQuery.ToString()).ToArray();
                list.AddRange(queryExecute);
                list.AddRange(query2Execute);
                data.AddRange(list.Where(b => b.DealarId != null).GroupBy(v => v.DealarId).Select(a => new DistStockReportModel
                {
                    DistributorId = a.Where(b => b.DistributorId == DistrubitorId).Select(c => c.DistributorId).FirstOrDefault(),
                    DistributorName = a.Select(b => b.DistributorName).FirstOrDefault(),
                    DealarId = a.Key,
                    DealarName = a.Key != System.Guid.Empty ? GetCMP_CompanyById(a.Key)?.name : GetCMP_CompanyById(DistrubitorId)?.name,
                    SalesCount = queryExecute.Where(b => b.DistributorId == DistrubitorId && b.DealarId == a.Key).Sum(c => c.SalesCount),
                    AssignmentCount = query2Execute.Where(b => b.DistributorId == DistrubitorId && b.DealarId == a.Key).Sum(c => c.AssignmentCount),
                    TotalStock = (queryExecute.Where(b => b.DistributorId == DistrubitorId && b.DealarId == a.Key).Sum(c => c.SalesCount) - query2Execute.Where(b => b.DistributorId == DistrubitorId && b.DealarId == a.Key).Sum(c => c.AssignmentCount))
                }));
            }
            return data.ToArray();
        }

        public DistStockReportModel[] GetPRD_StockProductReportDealarQuery(Guid DistrubitorId)
        {
            var data = new List<DistStockReportModel>();
            var list = new List<DistStockReportModel>();
            var newList = new List<DistStockReportModel>();
            using (var db = GetDB())
            {
                var EntegrationActionQuery = $@"select * from VWPRD_EntegrationActionSummary where DistributorId = '" + DistrubitorId + "'";
                var Temlik = $@"select * from VWPRD_ProductProgressPaymentImportDealarSummary where DistributorId = '" + DistrubitorId + "'";
                var queryExecute = db.ExecuteReader<DistStockReportModel>(EntegrationActionQuery.ToString()).ToArray();
                var query2Execute = db.ExecuteReader<DistStockReportModel>(Temlik.ToString()).ToArray();
                list.AddRange(queryExecute);
                list.AddRange(query2Execute);
                var groupList = list.GroupBy(n => n.DealarId);
                foreach (var dist in groupList)
                {
                    var distrubitorList = list.Where(a => a.DealarId == dist.Key);
                    var distrubitorGroup = distrubitorList.GroupBy(a => a.ProductId).ToArray();
                    foreach (var product in distrubitorGroup)
                    {
                        newList.Add(new DistStockReportModel
                        {
                            SalesCount = queryExecute.Where(a => a.DistributorId == DistrubitorId && a.DealarId == dist.Key && a.ProductId == product.Key).Sum(c => c.SalesCount),
                            AssignmentCount = query2Execute.Where(a => a.DistributorId == DistrubitorId && a.DealarId == dist.Key && a.ProductId == product.Key).Sum(c => c.AssignmentCount),
                            DistributorName = queryExecute.Where(a => a.DealarId == dist.Key).Select(c => c.DistributorName).FirstOrDefault(),
                            DealarId = dist.Key,
                            DealarName = dist.Key != System.Guid.Empty ? GetCMP_CompanyById(dist.Key)?.name : GetCMP_CompanyById(DistrubitorId)?.name,
                            ProductName = GetPRD_ProductById(product.Key).name,
                            ProductId = product.Key,
                            TotalStock = (queryExecute.Where(a => a.DistributorId == DistrubitorId && a.DealarId == dist.Key && a.ProductId == product.Key).Sum(c => c.SalesCount) - query2Execute.Where(a => a.DistributorId == DistrubitorId && a.DealarId == dist.Key && a.ProductId == product.Key).Sum(c => c.AssignmentCount))
                        });
                    }
                }
            }
            return newList.ToArray();
        }
    }
}
