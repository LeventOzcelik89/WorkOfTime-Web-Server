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
    [EnumInfo(typeof(CMP_Invoice), "type")]
    public enum EnumCMP_InvoiceType
    {
        [Description("Proforma (Teklif)")]
        Teklif = 0,
        [Description("İrsaliyeli Fatura")]
        IrsaliyeliFatura = 1,
        [Description("İrsaliyesiz Fatura")]
        IrsaliyesizFatura = 2,
        [Description("Irsaliye")]
        Irsaliye = 3,
        [Description("Sipariş")]
        Siparis = 4,
        [Description("Talep")]
        Talep = 5,
    }

    [EnumInfo(typeof(CMP_Invoice), "direction")]
    public enum EnumCMP_InvoiceDirectionType
    {
        [Description("Alış")]
        Alis = 0,
        [Description("Satış")]
        Satis = 1,
    }
    [EnumInfo(typeof(CMP_Invoice), "status")]
    public enum EnumCMP_InvoiceStatus
    {
        [Description("Ödendi")]
        Odendi = 0,
        [Description("Ödenecek")]
        Odenecek = 1,
        [Description("Çalışan Cebinden Ödedi")]
        CalisanOdedi = 2,
    }

    [EnumInfo(typeof(CMP_Invoice), "paymentType")]
    public enum EnumCMP_InvoicePaymentType
    {
        [Description("Peşin")]
        Pesin = 0,
        [Description("Vadeli")]
        Vadeli = 1,
        [Description("Taksitli")]
        Taksitli = 2,
    }

    [EnumInfo(typeof(CMP_Invoice), "tevkifat")]
    public enum EnumCMP_InvoiceTevkifat
    {
        [Description("10/10")]
        Yuzde100 = 6,
        [Description("9/10")]
        Yuzde90 = 5,
        [Description("7/10")]
        Yuzde70 = 4,
        [Description("5/10")]
        Yuzde50 = 3,
        [Description("3/10")]
        Yuzde30 = 2,
        [Description("2/10")]
        Yuzde20 = 1,
        [Description("0/10")]
        Yuzde0 = 0,
    }

    [EnumInfo(typeof(CMP_Invoice), "stopaj")]
    public enum EnumCMP_InvoiceStopaj
    {
        [Description("20%")]
        Yuzde20 = 5,
        [Description("17%")]
        Yuzde17 = 4,
        [Description("15%")]
        Yuzde15 = 3,
        [Description("10%")]
        Yuzde10 = 2,
        [Description("3%")]
        Yuzde3 = 1,
        [Description("0%")]
        Yuzde0 = 0,
    }


    partial class WorkOfTimeDatabase
    {

        public CMP_Invoice GetCMP_InvoiceBySerialNumber(string serialNumber, string rowNumber, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<CMP_Invoice>().Where(a => a.serialNumber.ToLower() == serialNumber.ToLower() && a.rowNumber == rowNumber).Execute().FirstOrDefault();
            }
        }

        public CMP_Invoice GetCMP_InvoiceBySerialNumberAndCompanyId(string serialNumber, string rowNumber, Guid customerId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<CMP_Invoice>().Where(a => a.serialNumber.ToLower() == serialNumber.ToLower() && a.rowNumber == rowNumber && a.customerId == customerId).Execute().FirstOrDefault();
            }
        }

        public CMP_Invoice[] GetCMP_InvoiceByCompanyId(Guid companyId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<CMP_Invoice>().Where(a => a.customerId == companyId || a.supplierId == companyId).Execute().ToArray();
            }
        }

        public CMP_Invoice[] GetCMP_InvoiceByIds(Guid[] Ids, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<CMP_Invoice>().Where(a => a.id.In(Ids)).Execute().ToArray();
            }
        }

        public VWCMP_Invoice[] GetVWCMP_InvoiceByIds(Guid[] Ids, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<VWCMP_Invoice>().Where(a => a.id.In(Ids)).Execute().ToArray();
            }
        }
    }
}
