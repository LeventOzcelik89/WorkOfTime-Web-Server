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
    public class SellerReport
    {
        public Guid DistributorId { get; set; }
        public string DistributorName { get; set; }
        public string productName { get; set; }
        public string CustomerOperatorName { get; set; }
        public DateTime FileNameDate  {get; set; }
        public int activatedCount { get; set; }
        public int salesCount { get; set; }
        public int returnSalesCount { get; set; }
        public int notActivatedCount { get; set; }
    }
        partial class WorkOfTimeDatabase
    {
        public PRD_EntegrationAction[] GetPRD_EntegrationActionBySerialNumbersOrImeis(string[] imeis,DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<PRD_EntegrationAction>().Where(a => a.SerialNo.In(imeis) ||a.Imei.In(imeis)).Execute().ToArray();
            }
        }
        public PRD_EntegrationAction GetPRD_EntegrationActionBySerialNumbersOrImei(string imei, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<PRD_EntegrationAction>().Where(a => a.SerialNo==imei || a.Imei==imei).Execute().FirstOrDefault();
            }
        }

        public VWPRD_EntegrationAction[] GetVWPRD_EntegrationActionByDistrubutorId(Guid id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWPRD_EntegrationAction>().Where(a => a.DistributorId == id).Execute().ToArray();
            }
        }
        public VWPRD_EntegrationAction[] GetVWPRD_EntegrationActionByDistrubutorIdAndDate(Guid id, DateTime startDate, DateTime endDate, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWPRD_EntegrationAction>().Where(a => a.DistributorId == id && a.Invoice_Address <= endDate && a.Invoice_Address >= startDate).Execute().ToArray();
            }
        }

        public SellerReport[] GetVWPRD_EntegrationActionSellerReportByDistIdAndDates(Guid DistributorId, DateTime startDate, DateTime endDate, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                StringBuilder cc = new StringBuilder();
                cc.AppendLine("select c.DistributorId, c.DistributorName, CustomerOperatorName, f.FileNameDate,");
                cc.AppendLine("SUM(CASE WHEN p.id is null THEN 0 ELSE 1 END) as activatedCount,");
                cc.AppendLine("SUM(CASE WHEN c.id is null THEN 0 ELSE 1 END) - Sum(CASE WHEN c.Quantity =1 THEN 0 ELSE 1 END)  as salesCount,");
                cc.AppendLine("SUM(CASE WHEN c.id is null THEN 0 ELSE 1 END) - Sum(CASE WHEN c.Quantity =-1 THEN 0 ELSE 1 END) as returnSalesCount,");
                cc.AppendLine("(SUM(CASE WHEN c.id is null THEN 0 ELSE 1 END)-SUM(CASE WHEN p.id is null THEN 0 ELSE 1 END)) as notActivatedCount");
                cc.AppendLine("from PRD_TitanDeviceActivated as p");
                cc.AppendLine("RIGHT JOIN PRD_EntegrationAction as c on c.Imei = p.IMEI1");
                cc.AppendLine("LEFT JOIN PRD_EntegrationFiles as  f on f.id = c.EntegrationFileId");
                cc.AppendLine(string.Format("where c.DistributorId = '{0}' AND fileNameDate BETWEEN '{1}' AND '{2}'", DistributorId, startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd")));
                cc.AppendLine("Group BY c.DistributorId, c.DistributorName,CustomerOperatorName,f.FileNameDate");
                cc.AppendLine("ORDER BY CustomerOperatorName,c.DistributorName,f.FileNameDate");
                return db.ExecuteReader<SellerReport>(cc.ToString()).ToArray();
            }
        }

        public VWPRD_EntegrationAction[] GetVWPRD_EntegrationActionBySellerId(Guid id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWPRD_EntegrationAction>().Where(a => a.CustomerOperatorId == id).Execute().ToArray();
            }
        }

        public VWPRD_EntegrationAction[] GetVWPRD_EntegrationActionBySellerIdAndDistId(Guid CustomerOperatorId,Guid DistId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWPRD_EntegrationAction>().Where(a => a.CustomerOperatorId == CustomerOperatorId && a.DistributorId == DistId).Execute().ToArray();
            }
        }
    }
}
