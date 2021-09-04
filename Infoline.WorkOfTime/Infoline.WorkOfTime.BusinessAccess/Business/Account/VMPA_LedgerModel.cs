using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessAccess.Models;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.Data.Common;
using System.Linq;
using System.Web;


namespace Infoline.WorkOfTime.BusinessAccess
{
    public class VMPA_TransactionReport
    {
        public VWPA_Transaction[] Transactions { get; set; }
        public VWPA_Ledger[] Ledgers { get; set; }
    }

    public class VMPA_Ledger
    {
        public VWPA_Ledger[] Ledgers { get; set; }
        public VWPA_Account[] AccountIds { get; set; }
    }

    public class VMPA_LedgerModel : VWPA_Ledger
    {
        private WorkOfTimeDatabase db { get; set; }
        private DbTransaction trans { get; set; }
        public VWPA_Transaction Transaction { get; set; }
        public VWPA_Advance Advance { get; set; }

        public PA_Ledger LedgerRelated { get; set; }
        private string _siteURL { get; set; } = TenantConfig.Tenant.GetWebUrl();
        private string _tenantName { get; set; } = TenantConfig.Tenant.TenantName;

        public VMPA_LedgerModel Load()
        {
            this.db = this.db ?? new WorkOfTimeDatabase();
            var ledger = db.GetVWPA_LedgerById(this.id);

            if (ledger != null)
            {
                this.B_EntityDataCopyForMaterial(ledger);
                if (ledger.transactionId.HasValue)
                {
                    this.Transaction = db.GetVWPA_TransactionById(ledger.transactionId.Value);
                }

                if (ledger.advanceId.HasValue)
                {
                    this.Advance = db.GetVWPA_AdvanceById(ledger.advanceId.Value);
                }
            }
            else
            {
                if (this.accountId.HasValue)
                {
                    var account = db.GetVWPA_AccountById(this.accountId.Value);
                    this.currencyId = account.currencyId;
                    this.Currency_Symbol = account.Currency_Symbol;
                    this.Currency_Code = account.Currency_Code;
                    this.amount = 0;
                }

                if (this.transactionId.HasValue)
                {
                    this.Transaction = db.GetVWPA_TransactionById(this.transactionId.Value);
                    this.amount = (double)this.Transaction.debt;

                    if (this.Transaction != null)
                    {
                        this.accountRealtedId = this.Transaction.accountId;
                    }

                    if (this.Transaction.invoiceId.HasValue)
                    {
                        var invoice = db.GetVWCMP_InvoiceById(this.Transaction.invoiceId.Value);
                        this.currencyId = invoice.currencyId;
                        this.Currency_Code = invoice.Currency_Code;
                        this.Currency_Symbol = invoice.Currency_Symbol;
                    }
                    else
                    {
                        this.currencyId = this.Transaction.currencyId;
                        this.Currency_Code = this.Transaction.Currency_Code;
                        this.Currency_Symbol = this.Transaction.Currency_Symbol;
                    }
                }

                if (this.advanceId.HasValue)
                {
                    this.Advance = db.GetVWPA_AdvanceById(this.advanceId.Value);
                    this.amount = (double)this.Advance.debt;

                    if (this.Advance != null)
                    {
                        this.accountRealtedId = this.Advance.accountId;
                    }

                    if (this.Advance.invoiceId.HasValue)
                    {
                        var invoice = db.GetVWCMP_InvoiceById(this.Advance.invoiceId.Value);
                        this.currencyId = invoice.currencyId;
                        this.Currency_Code = invoice.Currency_Code;
                        this.Currency_Symbol = invoice.Currency_Symbol;
                    }
                    else
                    {
                        this.currencyId = this.Advance.currencyId;
                        this.Currency_Code = this.Advance.Currency_Code;
                        this.Currency_Symbol = this.Advance.Currency_Symbol;
                    }
                }


                this.direction = this.type == (int)EnumPA_LedgerType.Tahsilat || this.type == (int)EnumPA_LedgerType.ParaGiris ||
                    this.type == (int)EnumPA_LedgerType.BakiyeSabitleme || this.type == (int)EnumPA_LedgerType.AcilisBakiye ?
                    (short)EnumPA_TransactionDirection.Giris : (short)EnumPA_TransactionDirection.Cikis;

                this.date = DateTime.Now;
            }

            this.id = Guid.NewGuid();
            return this;
        }

        public ResultStatus Save(Guid userId, HttpRequestBase request = null, DbTransaction trans = null)
        {
            db = db ?? new WorkOfTimeDatabase();
            var ledger = db.GetVWPA_LedgerById(this.id);
            var res = new ResultStatus { result = true };

            this.direction = this.type == (int)EnumPA_LedgerType.Tahsilat || this.type == (int)EnumPA_LedgerType.ParaGiris ||
                this.type == (int)EnumPA_LedgerType.BakiyeSabitleme || this.type == (int)EnumPA_LedgerType.AcilisBakiye ?
                (short)EnumPA_TransactionDirection.Giris : (short)EnumPA_TransactionDirection.Cikis;

            this.tax = this.tax.HasValue ? this.tax : 0;
            this.type = this.type == (int)EnumPA_LedgerType.CalisanaOdeme ? (int)EnumPA_LedgerType.Odeme : this.type;

            var rateCur = CurrencyExchangeRates.GetExchangeRatesByDate(this.Currency_Code, this.date.Value);
            this.rateExchange = rateCur.BanknoteSelling;

            if (this.transactionId.HasValue)
            {
                this.Transaction = db.GetVWPA_TransactionById(this.transactionId.Value);
            }

            if (this.advanceId.HasValue)
            {
                this.Advance = db.GetVWPA_AdvanceById(this.advanceId.Value);
            }

            if (this.accountRealtedId.HasValue)
            {
                this.LedgerRelated = new PA_Ledger
                {
                    id = Guid.NewGuid(),
                    accountId = this.accountRealtedId,
                    accountRealtedId = this.accountId,
                    created = DateTime.Now,
                    createdby = userId,
                    date = this.date,
                    description = this.description,
                    direction = (short)(this.direction.Value * -1),
                    transactionId = this.transactionId,
                    advanceId = this.advanceId,
                    type = this.type == (int)EnumPA_LedgerType.ParaCikis ? (short)EnumPA_LedgerType.ParaGiris :
                       this.type == (int)EnumPA_LedgerType.ParaGiris ? (short)EnumPA_LedgerType.ParaCikis : this.type
                };

                var relatedAccount = db.GetVWPA_AccountById(this.accountRealtedId.Value);
                this.LedgerRelated.currencyId = relatedAccount.currencyId;
                var relatedRate = CurrencyExchangeRates.GetExchangeRatesByDate(relatedAccount.Currency_Code, this.date.Value);

                if (this.transactionId.HasValue)
                {
                    var rateCross = CurrencyExchangeRates.GetCrossExchangeRatesByDate(relatedAccount.Currency_Code, this.Transaction.Currency_Code, this.date.Value);
                    this.LedgerRelated.crossRate = rateCross.BanknoteSelling;
                }

                if (this.advanceId.HasValue)
                {
                    var rateCross = CurrencyExchangeRates.GetCrossExchangeRatesByDate(relatedAccount.Currency_Code, this.Advance.Currency_Code, this.date.Value);
                    this.LedgerRelated.crossRate = rateCross.BanknoteSelling;
                }

                if (this.LedgerRelated.crossRate.HasValue)
                {
                    this.LedgerRelated.amount = this.amount / this.LedgerRelated.crossRate;
                    this.LedgerRelated.tax = this.tax / this.LedgerRelated.crossRate;
                }
                else
                {
                    this.LedgerRelated.amount = (this.amount * this.rateExchange) / relatedRate.BanknoteSelling;
                    this.LedgerRelated.tax = (this.tax * this.rateExchange) / relatedRate.BanknoteSelling;
                }

                this.LedgerRelated.rateExchange = relatedRate.BanknoteSelling;
            }

            var account = db.GetVWPA_AccountById(this.accountId.Value);
            var rate = CurrencyExchangeRates.GetExchangeRatesByDate(account.Currency_Code, this.date.Value);
            this.currencyId = account.currencyId;

            if (this.transactionId.HasValue)
            {
                var rateCross = CurrencyExchangeRates.GetCrossExchangeRatesByDate(account.Currency_Code, this.Transaction.Currency_Code, this.date.Value);
                this.crossRate = rateCross.BanknoteSelling;
            }

            if (this.advanceId.HasValue)
            {
                var rateCross = CurrencyExchangeRates.GetCrossExchangeRatesByDate(account.Currency_Code, this.Advance.Currency_Code, this.date.Value);
                this.crossRate = rateCross.BanknoteSelling;
            }

            this.Currency_Code = account.Currency_Code;

            if (this.crossRate.HasValue)
            {
                this.amount = this.amount / this.crossRate;
                this.tax = this.tax / this.crossRate;
            }
            else
            {
                this.amount = (this.amount * this.rateExchange) / rate.BanknoteSelling;
                this.tax = (this.tax * this.rateExchange) / rate.BanknoteSelling;
            }

            this.rateExchange = rate.BanknoteSelling;

            if (ledger == null)
            {
                this.created = DateTime.Now;
                this.createdby = userId;
                this.date = this.date;
                res &= Insert(trans);
            }
            else
            {
                this.changed = DateTime.Now;
                this.changedby = userId;
                res &= Update(trans);
            }

            if (res.result && request != null)
            {
                new FileUploadSave(request, this.id).SaveAs();
            }

            return res;
        }

        private ResultStatus Insert(DbTransaction trans = null)
        {
            var transaction = trans ?? db.BeginTransaction();

            this.crossRate = this.crossRate.HasValue ? this.crossRate.Value : this.rateExchange;
            var rs = db.InsertPA_Ledger(new PA_Ledger().B_EntityDataCopyForMaterial(this), transaction);

            if (this.accountRealtedId.HasValue)
            {
                this.LedgerRelated.crossRate = this.LedgerRelated.crossRate.HasValue ? this.LedgerRelated.crossRate.Value : this.LedgerRelated.rateExchange;
                rs &= db.InsertPA_Ledger(this.LedgerRelated, transaction);

                if (this.Transaction != null && this.Transaction.invoiceId.HasValue)
                {
                    rs &= db.InsertCMP_InvoiceAction(new CMP_InvoiceAction
                    {
                        id = Guid.NewGuid(),
                        createdby = this.createdby,
                        created = DateTime.Now,
                        description = String.Format("{0:N}", (this.amount + this.tax) * this.crossRate) + " " + this.Transaction.Currency_Symbol + " tahsil edildi.",
                        type = (int)EnumCMP_InvoiceActionType.FaturaTahsilat,
                        invoiceId = this.Transaction.invoiceId
                    }, transaction);

                    var userRoles = db.GetVWSH_UserRoleByUserId(this.createdby.Value);
                    var roleIds = userRoles.Select(a => a.roleid).ToArray();

                    var invoice = db.GetCMP_InvoiceById(this.Transaction.invoiceId.Value);
                    if (!roleIds.Contains(new Guid(SHRoles.SatisPersoneli)))
                    {
                        var transformInvoice = db.GetVWCMP_InvoiceTransformByIsTransformedTo(this.Transaction.invoiceId.Value).FirstOrDefault();

                        if (transformInvoice != null && transformInvoice.invoiceIdFrom.HasValue)
                        {
                            var orderOrTender = db.GetCMP_InvoiceById(transformInvoice.invoiceIdFrom.Value);

                            if (orderOrTender != null && orderOrTender.createdby.HasValue)
                            {
                                var str = orderOrTender.type == (int)EnumCMP_InvoiceType.Teklif ? "satış teklifinizin" : "siparişinizin";

                                var user = db.GetSH_UserById(orderOrTender.createdby.Value);
                                var text = "<h3>Sayın " + user.firstname + " " + user.lastname + "</h3>";
                                text += "<p>" + orderOrTender.rowNumber + " kodlu " + str + " faturasından tahsilat yapılmıştır. </p>";
                                text += "<p>Faturayı kontrol etmek için <a href='" + _siteURL + "/CMP/VWCMP_Invoice/DetailSelling?id=" + this.Transaction.invoiceId.Value + "'>tıklayınız.</a> </p>";
                                text += "<p>Bilgilerinize.</p>";
                                new Email().Template("Template1", "satinalma.jpg", _tenantName + " | WorkOfTime | Satış Sipariş Yönetimi", text).Send((Int16)EmailSendTypes.SatinAlma, user.email, "Faturadan Tahsilat Yapıldı", true);
                            }
                        }
                    }

                    if (!roleIds.Contains(new Guid(SHRoles.OnMuhasebe)))
                    {
                        var muhasebePersonelIds = db.GetSH_UserRoleByRoleId(new Guid(SHRoles.OnMuhasebe)).Where(a => a.userid.HasValue).Select(a => a.userid.Value).ToArray();
                        var users = db.GetSH_UserByIds(muhasebePersonelIds);

                        foreach (var user in users)
                        {
                            var text = "<h3>Sayın " + user.firstname + " " + user.lastname + "</h3>";
                            text += "<p>" + invoice.serialNumber + "/" + invoice.rowNumber + " seri numaralı faturadan tahsilat yapılmıştır. </p>";
                            text += "<p>Faturayı kontrol etmek için <a href='" + _siteURL + "/CMP/VWCMP_Invoice/DetailSelling?id=" + this.Transaction.invoiceId.Value + "'>tıklayınız.</a> </p>";
                            text += "<p>Bilgilerinize.</p>";
                            new Email().Template("Template1", "satinalma.jpg", _tenantName + " | WorkOfTime | Satış Sipariş Yönetimi", text).Send((Int16)EmailSendTypes.SatinAlma,user.email, "Faturadan Tahsilat Yapıldı", true);
                        }
                    }

                }

                if (this.Advance != null && this.Advance.invoiceId.HasValue)
                {
                    rs &= db.InsertCMP_InvoiceAction(new CMP_InvoiceAction
                    {
                        id = Guid.NewGuid(),
                        createdby = this.createdby,
                        created = DateTime.Now,
                        description = String.Format("{0:N}", (this.amount + this.tax) * this.crossRate) + " " + this.Advance.Currency_Symbol + " tahsil edildi.",
                        type = (int)EnumCMP_InvoiceActionType.FaturaTahsilat,
                        invoiceId = this.Advance.invoiceId
                    }, transaction);

                    var userRoles = db.GetVWSH_UserRoleByUserId(this.createdby.Value);
                    var roleIds = userRoles.Select(a => a.roleid).ToArray();

                    var invoice = db.GetCMP_InvoiceById(this.Advance.invoiceId.Value);
                    if (!roleIds.Contains(new Guid(SHRoles.SatisPersoneli)))
                    {
                        var transformInvoice = db.GetVWCMP_InvoiceTransformByIsTransformedTo(this.Advance.invoiceId.Value).FirstOrDefault();

                        if (transformInvoice != null && transformInvoice.invoiceIdFrom.HasValue)
                        {
                            var orderOrTender = db.GetCMP_InvoiceById(transformInvoice.invoiceIdFrom.Value);

                            if (orderOrTender != null && orderOrTender.createdby.HasValue)
                            {
                                var str = orderOrTender.type == (int)EnumCMP_InvoiceType.Teklif ? "satış teklifinizin" : "siparişinizin";

                                var user = db.GetSH_UserById(orderOrTender.createdby.Value);
                                var text = "<h3>Sayın " + user.firstname + " " + user.lastname + "</h3>";
                                text += "<p>" + orderOrTender.rowNumber + " kodlu " + str + " faturasından tahsilat yapılmıştır. </p>";
                                text += "<p>Faturayı kontrol etmek için <a href='" + _siteURL + "/CMP/VWCMP_Invoice/DetailSelling?id=" + this.Advance.invoiceId.Value + "'>tıklayınız.</a> </p>";
                                text += "<p>Bilgilerinize.</p>";
                                new Email().Template("Template1", "satinalma.jpg", _tenantName + " | WorkOfTime | Satış Sipariş Yönetimi", text).Send((Int16)EmailSendTypes.SatinAlma, user.email, "Faturadan Tahsilat Yapıldı", true);
                            }
                        }
                    }

                    if (!roleIds.Contains(new Guid(SHRoles.OnMuhasebe)))
                    {
                        var muhasebePersonelIds = db.GetSH_UserRoleByRoleId(new Guid(SHRoles.OnMuhasebe)).Where(a => a.userid.HasValue).Select(a => a.userid.Value).ToArray();
                        var users = db.GetSH_UserByIds(muhasebePersonelIds);

                        foreach (var user in users)
                        {
                            var text = "<h3>Sayın " + user.firstname + " " + user.lastname + "</h3>";
                            text += "<p>" + invoice.serialNumber + "/" + invoice.rowNumber + " seri numaralı faturadan tahsilat yapılmıştır. </p>";
                            text += "<p>Faturayı kontrol etmek için <a href='" + _siteURL + "/CMP/VWCMP_Invoice/DetailSelling?id=" + this.Transaction.invoiceId.Value + "'>tıklayınız.</a> </p>";
                            text += "<p>Bilgilerinize.</p>";
                            new Email().Template("Template1", "satinalma.jpg", _tenantName + " | WorkOfTime | Satış Sipariş Yönetimi", text).Send((Int16)EmailSendTypes.SatinAlma, user.email, "Faturadan Tahsilat Yapıldı", true);
                        }
                    }

                }

            }

            if (trans == null)
            {
                if (rs.result)
                    transaction.Commit();
                else
                    transaction.Rollback();
            }

            return rs;
        }

        private ResultStatus Update(DbTransaction trans = null)
        {
            var transaction = trans ?? db.BeginTransaction();
            var res = new ResultStatus { result = true };

            if (trans == null)
            {

            }

            return res;
        }
    }
}
