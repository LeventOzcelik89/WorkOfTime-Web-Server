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
        public SellOutReportNewModel[] GetPRD_SellOutReportDistrubitorQuery(DateTime startDate, DateTime endDate)
        {
            var data = new List<SellOutReportNewModel>();
            using (var db = GetDB())
            {
                var query = $@"Select DistributorId, Count(*) as DistSalesCount, SUM(CASE WHEN ActivationDate is not null THEN 1 ELSE 0 END) as ActivatedData
                           FROM VWPRD_EntegrationAction
                           Where
                           Invoice_Address >= '{startDate.ToString("yyyy-MM-dd")}' and
                           Invoice_Address <= '{endDate.ToString("yyyy-MM-dd")}' and
                           Quantity = 1
                           Group by DistributorId";
                var query2 = $@"Select dataCompanyId as DistributorId, dataCompanyId_Title, Count(*) as SalesCount From VWPRD_InventoryAction
                            Where
                            type = 3 and
                           created >= '{startDate.ToString("yyyy-MM-dd")}' and
                           created <= '{endDate.ToString("yyyy-MM-dd")}'
                           Group by dataCompanyId_Title, dataCompanyId
                           order by Count(*) desc";
                var query3 = $@"select pid,pid_title, Count(*) as AssignmentCount from VWPRD_ProductProgressPaymentImport
					            Where
                                created >= '{startDate.ToString("yyyy-MM-dd")}' and
                                created <= '{endDate.ToString("yyyy-MM-dd")}'
				                group by pid,pid_Title";
                var queryExecute = db.ExecuteReader<SellOutReportNewModel>(query.ToString()).ToArray();
                var query2Execute = db.ExecuteReader<SellOutReportNewModel>(query2.ToString()).ToArray();
                var query3Execute = db.ExecuteReader<SellOutReportNewModel>(query3.ToString()).ToArray();
                data.AddRange(query2Execute.Select(a => new SellOutReportNewModel
                {
                    ActivatedData = queryExecute.Where(b => b.DistributorId == a.DistributorId).Select(c => c.ActivatedData).FirstOrDefault(),
                    DistSalesCount = queryExecute.Where(b => b.DistributorId == a.DistributorId).Select(c => c.DistSalesCount).FirstOrDefault(),
                    SalesCount = a.SalesCount,
                    dataCompanyId_Title = a.dataCompanyId_Title,
                    Types = (GetVWCMP_CompanyById(a.DistributorId) != null ? GetVWCMP_CompanyById(a.DistributorId).CMPTypes_Title : null) == "Distribütör" ? "Distribütör" : "Bayi",
                    DistributorId = a.DistributorId,
                    pid_Title= GetVWCMP_CompanyById(a.DistributorId)!=null ? query3Execute.Where(d => GetVWCMP_CompanyById(a.DistributorId).pid == d.pid).Select(c=>c.pid_Title).FirstOrDefault(): null,
                    AssignmentCount = GetVWCMP_CompanyById(a.DistributorId)!=null ? query3Execute.Where(d => GetVWCMP_CompanyById(a.DistributorId).pid == d.pid).Select(t => t.AssignmentCount).FirstOrDefault() : 0,
                }));
            }
            return data.ToArray();
        }
        public SellOutReportNewModel[] GetPRD_SellOutProductReportDistrubitorQuery(DateTime startDate, DateTime endDate)
        {
            var data = new List<SellOutReportNewModel>();
            using (var db = GetDB())
            {
                var query = $@"Select DistributorId, Count(*) as DistSalesCount, SUM(CASE WHEN ActivationDate is not null THEN 1 ELSE 0 END) as ActivatedData
                           FROM VWPRD_EntegrationAction
                           Where
                           Invoice_Address >= '{startDate.ToString("yyyy-MM-dd")}' and
                           Invoice_Address <= '{endDate.ToString("yyyy-MM-dd")}' and
                           Quantity = 1
                           Group by DistributorId";
                var query2 = $@"Select dataCompanyId as DistributorId, dataCompanyId_Title, Count(*) as SalesCount From VWPRD_InventoryAction
                            Where
                            type = 3 and
                           created >= '{startDate.ToString("yyyy-MM-dd")}' and
                           created <= '{endDate.ToString("yyyy-MM-dd")}'
                           Group by dataCompanyId_Title, dataCompanyId
                           order by Count(*) desc";
                var query3 = $@"select pid , pid_Title ,productId,productId_Title, Count(*) as counts from VWPRD_ProductProgressPaymentImport
	                            Where
                                created >= '{startDate.ToString("yyyy-MM-dd")}' and
                                created <= '{endDate.ToString("yyyy-MM-dd")}'
	                            group by pid , pid_Title,productId,productId_Title
	                            order by pid_Title desc";
                var queryExecute = db.ExecuteReader<SellOutReportNewModel>(query.ToString()).ToArray();
                var query2Execute = db.ExecuteReader<SellOutReportNewModel>(query2.ToString()).ToArray();
                var query3Execute = db.ExecuteReader<SellOutReportNewModel>(query3.ToString()).ToArray();
                data.AddRange(query2Execute.Select(a => new SellOutReportNewModel
                {

                    ActivatedData = queryExecute.Where(b => b.DistributorId == a.DistributorId).Select(c => c.ActivatedData).FirstOrDefault(),
                    DistSalesCount = queryExecute.Where(b => b.DistributorId == a.DistributorId).Select(c => c.DistSalesCount).FirstOrDefault(),
                    SalesCount = a.SalesCount,
                    dataCompanyId_Title = a.dataCompanyId_Title,
                    Types = (GetVWCMP_CompanyById(a.DistributorId) != null ? GetVWCMP_CompanyById(a.DistributorId).CMPTypes_Title : null) == "Distribütör" ? "Distribütör" : "Bayi",
                    DistributorId = a.DistributorId,
                    pid_Title = GetVWCMP_CompanyById(a.DistributorId) != null ? query3Execute.Where(d => GetVWCMP_CompanyById(a.DistributorId).pid == d.pid).Select(c => c.pid_Title).FirstOrDefault() : null,
                    AssignmentCount = GetVWCMP_CompanyById(a.DistributorId) != null ? query3Execute.Where(d => GetVWCMP_CompanyById(a.DistributorId).pid == d.pid).Select(t => t.AssignmentCount).FirstOrDefault() : 0,
                }));
            }
            return data.ToArray();
        }
    }
}
