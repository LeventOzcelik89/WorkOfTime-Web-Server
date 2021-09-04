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
using System.ComponentModel;

namespace Infoline.WorkOfTime.BusinessAccess
{

    [EnumInfo(typeof(CMP_Company), "type")]
    public enum EnumCMP_CompanyType
    {
        [Description("Benim Carilerim")]
        Benimisletmem = 0,
        [Description("Diğer Cariler")]
        Diger = 1,
    }


    [EnumInfo(typeof(CMP_Company), "taxType")]
    public enum EnumCMP_CompanyTaxType
    {
        [Description("Gerçek Kişi")]
        GercekKisi = 0,
        [Description("Tüzel Kişi")]
        TuzelKisi = 1,
        [Description("Tüzel Kişiliği Olmayan Ticari İşletme")]
        TuzelKisiligiOlmayanİstletme = 2,
    }

    [EnumInfo(typeof(CMP_Company), "isActive")]
    public enum EnumCMP_CompanyIsActive
    {
        [Description("Aktif")]
        Aktif = 0,
        [Description("Pasif")]
        Pasif = 1,
    }


    public class FilterModelCompany
    {
        public string DoingProject { get; set; }
        public string DoingToProject { get; set; }
        public string SellingInvoice { get; set; }
        public string BuyingInvoice { get; set; }
        public string SellingTender { get; set; }
        public string BuyingTender { get; set; }
        //public string MyMaintenance { get; set; }
        //public string MaintenanceServices { get; set; }
    }

    partial class WorkOfTimeDatabase
    {
        public FilterModelCompany GetCMP_CompanyFilterModel()
        {

            using (var db = GetDB())
            {
                return db.ExecuteReader<FilterModelCompany>("Select (STUFF((SELECT ',' + Convert(nvarchar(max), CompanyId) from VWPRJ_Project with (nolock) group by CompanyId " +
                        "FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 1, '')) AS DoingProject," +
                    "(STUFF((SELECT ',' + Convert(nvarchar(max), CorporationId) from VWPRJ_Project with (nolock) group by CorporationId" +
                        " FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 1, '')) AS DoingToProject," +
                    "(STUFF((SELECT ',' + Convert(nvarchar(max), customerId) from VWCMP_Invoice with (nolock) group by customerId" +
                        " FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 1, '')) AS SellingInvoice," +
                    "(STUFF((SELECT ',' + Convert(nvarchar(max), supplierId) from VWCMP_Invoice with (nolock) group by supplierId" +
                        " FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 1, '')) AS BuyingInvoice," +
                    "(STUFF((SELECT ',' + Convert(nvarchar(max), customerId) from VWCMP_Tender with (nolock) group by customerId" +
                        " FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 1, '')) AS SellingTender," +
                    "(STUFF((SELECT ',' + Convert(nvarchar(max), supplierId) from VWCMP_Tender with (nolock) group by supplierId" +
                    " FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 1, '')) AS BuyingTender").FirstOrDefault();
            }
        }

        public CMP_Company[] GetCMP_CompanyByIdsAndStartAndEnd(Guid[] ids, DateTime start, DateTime end)
        {
            using (var db = GetDB())
            {
                return db.Table<CMP_Company>().Where(a => a.id.In(ids) && a.created >= start && a.created <= end).Execute().ToArray();
            }
        }


        public CMP_Company[] GetCMP_CompanyOther(DbTransaction tran = null)
       {
            using (var db = GetDB(tran))
            {
                return db.Table<CMP_Company>().Where(a => a.type == (int)EnumCMP_CompanyType.Diger).Execute().ToArray();
            }
        }
        public CMP_Company GetCMP_CompanyByCode(string code, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<CMP_Company>().Where(a => a.code == code).Execute().FirstOrDefault();
            }
        }

        public CMP_Company GetCMP_CompanyByName(string name, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<CMP_Company>().Where(a => a.code == name).Execute().FirstOrDefault();
            }
        }

        public Guid[] GetCMP_CompanyByType(EnumCMP_CompanyType type, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<CMP_Company>().Where(a => a.type == (int)type).Select(a => new CMP_Company { id = a.id }).Execute().Select(a =>a.id).ToArray();
            }
        }


        public CMP_Company[] GetCMP_CompanyByOwner(DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<CMP_Company>().Where(a => a.type == (int)EnumCMP_CompanyType.Benimisletmem).Execute().ToArray();
            }
        }

        public VMCMP_CompanyNameId[] GetCMP_CompanyByNameId(DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
               return  db.ExecuteReader<VMCMP_CompanyNameId>("select id as id, name + '(' + code + ')' as fullName from CMP_Company with(nolock)").ToArray();
            }
        }

    }

    public class VMCMP_CompanyNameId
    {
        public Guid id { get; set; }
        public string fullName { get; set; }
    }
}
