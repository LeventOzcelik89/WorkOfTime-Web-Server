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

        public CompanyInvoiceMonthly[] GetVWCMP_InvoiceBuyingByCompanyId(Guid companyId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.ExecuteReader<CompanyInvoiceMonthly>(" SELECT CONVERT(NVARCHAR(7), created,126) DateMonthYear,"+
                                       "AVG(totalAmountAsTL) TotalAmount "+
                                       "FROM VWCMP_Invoice "+
                                       "WHERE customerId = '"+ companyId +"' and created >= DATEADD(year, -1, GETDATE()) and not(MONTH(created) = MONTH(GETDATE()) and YEAR(created) != YEAR(GETDATE())) "+
                                       "GROUP BY CONVERT(NVARCHAR(7), created, 126) "+
                                       "ORDER BY CONVERT(NVARCHAR(7), created, 126)").ToArray();
            }
        }

        public CompanyInvoiceMonthly[] GetVWCMP_InvoiceSellingByCompanyId(Guid companyId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.ExecuteReader<CompanyInvoiceMonthly>(" SELECT CONVERT(NVARCHAR(7), created,126) DateMonthYear," +
                                       "AVG(totalAmountAsTL) TotalAmount " +
                                       "FROM VWCMP_Invoice " +
                                       "WHERE supplierId = '" + companyId + "' and created >= DATEADD(year, -1, GETDATE()) and not(MONTH(created) = MONTH(GETDATE()) and YEAR(created) != YEAR(GETDATE())) " +
                                       "GROUP BY CONVERT(NVARCHAR(7), created, 126) " +
                                       "ORDER BY CONVERT(NVARCHAR(7), created, 126)").ToArray();
            }
        }

        public VWCMP_Invoice[] GetVWCMP_InvoiceByProjectId(Guid ProjectId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWCMP_Invoice>().Where(a => a.projectId == ProjectId).Execute().ToArray();
            }
        }

        public double GetCMP_CompanyTenderAmountByCustomerId(Guid id, int direction, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWCMP_Invoice>().Where(a => a.customerId == id && a.totalAmountAsTL != null).Execute().Sum(a => a.totalAmountAsTL.Value);
            }
        }

        public double GetCMP_CompanyInvoiceAmountBySuplierId(Guid id, int direction, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWCMP_Invoice>().Where(a => a.supplierId == id && a.totalAmountAsTL != null).Execute().Sum(a => a.totalAmountAsTL.Value);
            }
        }

        public VWCMP_Invoice GetVWCMP_InvoiceByPid(Guid pid, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWCMP_Invoice>().Where(a => a.pid == pid).Execute().FirstOrDefault();
            }
        }

    }

    public class CompanyInvoiceMonthly
    {
        public string DateMonthYear { get; set; }
        public double TotalAmount { get; set; }
    }
}
