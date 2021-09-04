using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessAccess.Models;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

//Test
namespace Infoline.WorkOfTime.BusinessAccess
{
    public class InvoiceFormTemplateModels : VWCMP_Invoice
    {
        public VWCMP_InvoiceItem[] Items { get; set; }
        public VWCMP_Company Supplier { get; set; }
        public VWCMP_Company Customer { get; set; }
        public string LogoPath { get; set; }
        public int? typeJob { get; set; }
    }

    public class VMCMP_InvoiceModels : VWCMP_Invoice
    {
        private WorkOfTimeDatabase db { get; set; }
        private DbTransaction trans { get; set; }
        private string _siteURL { get; set; } = TenantConfig.Tenant.GetWebUrl();
        private string _tenantName { get; set; } = TenantConfig.Tenant.TenantName;
        public List<VWCMP_InvoiceAction> InvoiceActions { get; set; }
        public List<VWCMP_InvoiceItem> InvoiceItems { get; set; }
        public VWCMP_InvoiceTransform TransformFrom { get; set; }
        public VWPA_Transaction Transaction { get; set; } = new VWPA_Transaction();
        public VWPA_Ledger Ledger { get; set; } = new VWPA_Ledger();
        public VWPA_Account Account { get; set; } = new VWPA_Account();
        public bool? IsTransform { get; set; }
        public bool? IsCopy { get; set; }
        public VWPA_Transaction[] Transactions { get; set; }
        public CMP_Invoice oldInvoice { get; set; }
        public SYS_Files[] files { get; set; }

        public VMCMP_InvoiceModels Load(bool? isTransform, int? direction)
        {
            db = db ?? new WorkOfTimeDatabase();
            var invoice = db.GetVWCMP_InvoiceById(this.id);
            var invoiceVW = db.GetVWCMP_InvoiceById(this.id);

            if (invoice != null)
            {
                this.InvoiceItems = db.GetVWCMP_InvoiceItemByInvoiceId(this.id).OrderBy(a => a.itemOrder).ToList();
                this.InvoiceActions = db.GetVWCMP_InvoiceActionByInvoiceId(this.id).ToList();
                this.files = db.GetSYS_FilesByDataIdAll(this.id).ToArray();

                if (isTransform == true)
                {
                    this.B_EntityDataCopyForMaterial(invoice, true);
                    this.description = "";
                    this.rowNumber = null;
                    this.type = (int)EnumCMP_InvoiceType.IrsaliyesizFatura;
                    this.status = (int)EnumCMP_InvoiceStatus.Odenecek;

                    foreach (var item in this.InvoiceItems)
                    {
                        item.id = Guid.NewGuid();
                        item.description = "";
                    }
                }
                else
                {
                    this.B_EntityDataCopyForMaterial(invoiceVW, true);
                    this.TransformFrom = db.GetVWCMP_InvoiceTransformByIsTransformedTo(this.id).FirstOrDefault();
                    this.Transactions = db.GetVWPA_TransactionByInvoiceId(this.id);
                    if (this.IsCopy == true)
                    {
                        this.status = (short)EnumCMP_InvoiceStatus.Odenecek;
                    }
                }
            }
            else
            {
                //TODO : OĞUZ buralar kontrol edilecek.
                var order = db.GetVWCMP_OrderById(this.id);
                if (order != null)
                {
                    this.B_EntityDataCopyForMaterial(order, true);
                    this.InvoiceItems = db.GetVWCMP_InvoiceItemByInvoiceId(this.id).OrderBy(a => a.itemOrder).ToList();
                }
                else
                {
                    this.id = Guid.NewGuid();
                }
                this.type = (int)EnumCMP_InvoiceType.IrsaliyesizFatura;
            }

            this.status = this.status.HasValue ? this.status.Value : (short)EnumCMP_InvoiceStatus.Odenecek;
            this.IsTransform = isTransform.Value ? isTransform : false;

            if (direction.HasValue)
            {
                this.direction = (short)direction;
            }

            return this;
        }

        public ResultStatus Save(Guid? userId, HttpRequestBase request = null, DbTransaction trans = null)
        {
            db = db ?? new WorkOfTimeDatabase();
            var _trans = trans ?? db.BeginTransaction();
            var isNull = true;
            this.oldInvoice = IsTransform == true ? db.GetCMP_InvoiceById(this.id) : null;

            var rs = new ResultStatus { result = true };
            var invoice = db.GetCMP_InvoiceById(this.id);

            if (invoice != null)
            {
                isNull = false;
            }

            if (this.customerAdress == null || this.supplierAdress == null) { return new ResultStatus { result = false, message = "Fatura kesmek istediğiniz işletmelerin fatura adresleri zorunludur." }; }
            if (this.customerTaxNumber == null || this.supplierTaxNumber == null) { return new ResultStatus { result = false, message = "Fatura kesmek istediğiniz işletmelerin vergi numaraları zorunludur." }; }
            if (this.customerTaxOffice == null || this.supplierTaxOffice == null) { return new ResultStatus { result = false, message = "Fatura kesmek istediğiniz işletmelerin vergi daireleri zorunludur." }; }
            if (this.customerTitle == null || this.supplierTitle == null) { return new ResultStatus { result = false, message = "Fatura kesmek istediğiniz işletmelerin ünvanları zorunludur." }; }
            if (this.rateExchange == null) { return new ResultStatus { result = false, message = "Lütfen güncel kur giriniz." }; }
            if (this.InvoiceItems == null) { return new ResultStatus { result = false, message = "Fatura ekleyebilmek için en az bir ürün girişi yapmalısınız." }; }
            var productControl = this.InvoiceItems.Where(a => a.productId == null).ToArray();
            if (productControl.Count() > 0) { return new ResultStatus { result = false, message = "Ürün seçmek zorunludur, lütfen ürün seçmeyecek olduğunuz satırı silin ya da bir ürün seçiniz." }; }
            if (this.InvoiceItems.Count() == 0) { return new ResultStatus { result = false, message = "Fatura ekleyebilmek için en az bir ürün girişi yapmalısınız." }; }
            if (this.customerId == this.supplierId) { return new ResultStatus { result = false, message = "Kendi işletmenize fatura kesemezsiniz." }; }

			if (this.customerId.HasValue)
			{
                var controlSerialNumber = db.GetCMP_InvoiceBySerialNumberAndCompanyId(this.serialNumber, this.rowNumber, this.customerId.Value);

                if (controlSerialNumber != null) { return new ResultStatus { result = false, message = "Seçilen işletme için bu seri ve sıra numaralı fatura sistemde kayıtlıdır. Lütfen kontrol ediniz." }; }
            }

            if (this.direction == (int)EnumCMP_InvoiceDirectionType.Alis)
            {
                if (this.status == (int)EnumCMP_InvoiceStatus.CalisanOdedi && this.Account.dataId == null) { return new ResultStatus { result = false, message = "Lütfen çalışan seçiniz." }; }
                if (this.status == (int)EnumCMP_InvoiceStatus.Odendi && this.Ledger.accountId == null) { return new ResultStatus { result = false, message = "Lütfen ödeme yapacak hesabı seçiniz." }; }
            }
            else
            {
                if (this.paymentType == (int)EnumCMP_InvoicePaymentType.Pesin && this.Ledger.accountId == null) { return new ResultStatus { result = false, message = "Lütfen ödeme yapılacak hesabı seçiniz." }; }
            }

            if (invoice == null || this.IsCopy == true || !isNull)
            {
                this.created = DateTime.Now;
                this.createdby = userId;
                this.id = Guid.NewGuid();
                rs = this.Insert(_trans);
            }

            if (rs.result)
            {
                new FileUploadSave(request, this.id).SaveAs();
            }

            if (trans == null)
            {
                if (rs.result)
                    _trans.Commit();
                else
                    _trans.Rollback();
            }

            return rs;
        }

        private ResultStatus Insert(DbTransaction trans = null)
        {
            var _trans = trans ?? db.BeginTransaction();

            if (this.direction == (int)EnumCMP_InvoiceDirectionType.Satis)
            {
                this.expiryDate = this.paymentType != (int)EnumCMP_InvoicePaymentType.Vadeli ? null : this.expiryDate;
                this.installmentCount = this.paymentType != (int)EnumCMP_InvoicePaymentType.Taksitli ? null : this.installmentCount;
            }

            foreach (var invoice in this.InvoiceItems)
            {
                invoice.id = Guid.NewGuid();
                invoice.invoiceId = this.id;
                invoice.created = DateTime.Now;
                invoice.createdby = this.createdby;
                invoice.price = invoice.price != null ? invoice.price : 0;
                invoice.quantity = invoice.quantity != null ? invoice.quantity : 0;
                invoice.OTV = invoice.OTV != null ? invoice.OTV : 0;
                invoice.OIV = invoice.OIV != null ? invoice.OIV : 0;
                invoice.KDV = invoice.KDV != null ? invoice.KDV : 0;
                invoice.discount = invoice.discount != null ? invoice.discount : 0;
            }

            var action = new CMP_InvoiceAction
            {
                created = DateTime.Now,
                createdby = this.createdby,
                invoiceId = this.id,
                type = (int)EnumCMP_InvoiceActionType.YeniFatura,
                description = "Fatura oluşturuldu."
            };

            this.discount = this.discount != null ? this.discount : 0;
            this.stopaj = this.stopaj != null ? this.stopaj : 0;
            this.tevkifat = this.tevkifat != null ? this.tevkifat : 0;

            var dbresult = db.InsertCMP_Invoice(new CMP_Invoice().B_EntityDataCopyForMaterial(this), _trans);
            dbresult &= db.InsertCMP_InvoiceAction(action, _trans);
            dbresult &= db.BulkInsertCMP_InvoiceItem(this.InvoiceItems.Select(a => new CMP_InvoiceItem().B_EntityDataCopyForMaterial(a)), _trans);

            if (this.type == (int)EnumCMP_InvoiceType.IrsaliyeliFatura)
            {
                var products = db.GetVWPRD_ProductByProductIds(this.InvoiceItems.Where(a => a.productId.HasValue).Select(a => a.productId.Value).ToArray()).Where(a => a.stockType != (short)EnumPRD_ProductStockType.Stoksuz);

                if (products.Count() > 0)
                {
                    dbresult &= this.InsertWaybill(products, _trans);
                }
            }

            var companyId = this.direction == (int)EnumCMP_InvoiceDirectionType.Alis ? this.supplierId.Value : this.customerId.Value;
            var companyAccount = db.GetVWPA_AccountByDataIdDataTable(companyId, "CMP_Company");

            if (companyAccount == null)
            {
                var currencies = db.GetUT_Currency();
                var TL = currencies.Where(a => a.code == "TL").FirstOrDefault();

                companyAccount = new VWPA_Account()
                {
                    id = Guid.NewGuid(),
                    created = this.created,
                    createdby = this.createdby,
                    code = BusinessExtensions.B_GetIdCode(),
                    currencyId = TL.id,
                    isDefault = true,
                    name = "Ana Hesap",
                    status = true,
                    type = (int)EnumPA_AccountType.Kasa,
                    dataId = companyId,
                    dataTable = "CMP_Company"
                };

                dbresult &= db.InsertPA_Account(new PA_Account().B_EntityDataCopyForMaterial(companyAccount), _trans);
            }

            dbresult &= this.InsertTransactionAccount(companyAccount, _trans);

            if (this.oldInvoice != null)
            {
                dbresult &= this.UpsertInvoiceTransform(_trans);
            }

            if (this.projectId != null)
            {
                var projectInvoice = new PRJ_ProjectInvoice
                {
                    id = Guid.NewGuid(),
                    created = DateTime.Now,
                    createdby = this.createdby,
                    invoiceId = this.id,
                    projectId = projectId.Value
                };

                dbresult &= db.InsertPRJ_ProjectInvoice(projectInvoice, _trans);
            }

            if (trans == null)
            {
                if (dbresult.result)
                    _trans.Commit();
                else
                    _trans.Rollback();
            }

            return dbresult;
        }

        private ResultStatus UpsertInvoiceTransform(DbTransaction trans = null)
        {
            var rs = new ResultStatus { result = true };
            var _trans = trans ?? db.BeginTransaction();

            var listTransform = new List<CMP_InvoiceTransform>();
            var listAction = new List<CMP_InvoiceAction>();

            listTransform.Add(new CMP_InvoiceTransform
            {
                created = DateTime.Now,
                createdby = this.createdby,
                id = Guid.NewGuid(),
                invoiceIdFrom = this.oldInvoice.id,
                invoiceIdTo = this.id
            });

            var newAction = new CMP_InvoiceAction
            {
                id = Guid.NewGuid(),
                created = DateTime.Now,
                createdby = this.createdby,
                invoiceId = this.oldInvoice.id,
                transformInvoiceId = this.id,
            };

            if (this.oldInvoice.type == (int)EnumCMP_InvoiceType.Teklif)
            {
                newAction.description = "Teklifin faturası kesildi.";
                newAction.type = (int)EnumCMP_InvoiceActionType.TeklifFatura;

                rs &= new VMCMP_TenderModels { id = this.oldInvoice.id }.Load(false, (int)EnumCMP_InvoiceDirectionType.Satis).UpdateStatus((int)EnumCMP_TenderStatus.TeklifFatura, this.createdby.Value, _trans);

                var requestControl = db.GetVWCMP_InvoiceTransformByIsTransformedTo(this.oldInvoice.id).FirstOrDefault();

                if (requestControl != null)
                {
                    rs &= new VMCMP_RequestModels { id = requestControl.invoiceIdFrom.Value }.Load(false).UpdateStatus((int)EnumCMP_RequestStatus.FaturasiAlindi, this.createdby.Value, _trans);
                }
            }

            if (this.oldInvoice.type == (int)EnumCMP_InvoiceType.Siparis)
            {
                newAction.description = "Siparişin faturası kesildi.";
                newAction.type = (int)EnumCMP_InvoiceActionType.SiparisFatura;

                rs &= new VMCMP_OrderModels { id = this.oldInvoice.id }.Load(false).UpdateStatus((int)EnumCMP_OrderStatus.SurecTamamlandi, this.createdby.Value, _trans);
            }

            listAction.Add(newAction);

            rs &= db.BulkInsertCMP_InvoiceTransform(listTransform.ToArray(), _trans);
            rs &= db.BulkInsertCMP_InvoiceAction(listAction, _trans);

            if (trans == null)
            {
                if (rs.result)
                    _trans.Commit();
                else
                    _trans.Rollback();
            }

            return rs;
        }

        private ResultStatus InsertTransactionAccount(VWPA_Account companyAccount, DbTransaction trans = null)
        {
            var rs = new ResultStatus { result = true };
            var _trans = trans ?? db.BeginTransaction();

            var invoiceView = db.GetVWCMP_InvoiceById(this.id, _trans);

            var tax = invoiceView.totalTax.HasValue ? invoiceView.totalTax.Value : 0;

            this.Transaction.accountId = companyAccount.id;
            this.Transaction.currencyId = this.currencyId;
            this.Transaction.amount = invoiceView.totalAmount - tax;
            this.Transaction.tax = tax;
            this.Transaction.dataId = invoiceView.id;
            this.Transaction.dataTable = "CMP_Invoice";
            this.Transaction.created = DateTime.Now;
            this.Transaction.createdby = this.createdby;
            this.Transaction.description = "Fatura Ödemesi";
            this.Transaction.invoiceId = this.id;
            this.Transaction.progressDate = this.expiryDate;
            this.Transaction.direction = this.direction == (short)EnumCMP_InvoiceDirectionType.Alis ? (short)EnumPA_TransactionDirection.Cikis : (short)EnumPA_TransactionDirection.Giris;
            this.Transaction.type = this.direction == (short)EnumCMP_InvoiceDirectionType.Alis ? (short)EnumPA_TransactionType.AlisFatura : (short)EnumPA_TransactionType.SatisFatura;

            if (this.Ledger.accountId != null)
            {
                var account = db.GetVWPA_AccountById(this.Ledger.accountId.Value);
                if (this.direction == (int)EnumCMP_InvoiceDirectionType.Satis)
                {
                    this.Ledger.date = this.issueDate;
                }

                var currency = CurrencyExchangeRates.GetExchangeRatesByDate(account.Currency_Code, this.Ledger.date.Value.Date);
                var rate = currency.BanknoteSelling;

                var crossCurrency = CurrencyExchangeRates.GetCrossExchangeRatesByDate(account.Currency_Code, invoiceView.Currency_Code, this.Ledger.date.Value);
                var crossRate = crossCurrency.BanknoteSelling;

                this.Ledger.rateExchange = rate;
                this.Ledger.currencyId = account.currencyId;
                this.Ledger.transactionId = this.Transaction.id;
                this.Ledger.tax = invoiceView.totalTax / crossRate;
                this.Ledger.amount = (invoiceView.totalAmount - invoiceView.totalTax) / crossRate;
                this.Ledger.accountId = this.Ledger.accountId;
                this.Ledger.accountRealtedId = companyAccount.id;
                this.Ledger.crossRate = crossRate;
                this.Ledger.direction = this.direction == (short)EnumCMP_InvoiceDirectionType.Alis ? (short)EnumPA_TransactionDirection.Cikis : (short)EnumPA_TransactionDirection.Giris;
            }

            if (this.direction == (int)EnumCMP_InvoiceDirectionType.Alis)
            {
                this.Transaction.status = (int)EnumPA_TransactionStatus.Odenecek;
                rs &= db.InsertPA_Transaction(new PA_Transaction().B_EntityDataCopyForMaterial(this.Transaction, true), _trans);

                this.Ledger.type = (int)EnumPA_LedgerType.Odeme;
                this.Ledger.description = "Ödeme";

                switch (this.status)
                {
                    case (short)EnumCMP_InvoiceStatus.Odenecek:
                        break;
                    case (short)EnumCMP_InvoiceStatus.Odendi:
                        this.Transaction.progressDate = this.Ledger.date;
                        this.Transaction.status = (int)EnumPA_TransactionStatus.Odendi;
                        var newLedger = new PA_Ledger
                        {
                            accountId = companyAccount.id,
                            accountRealtedId = this.Ledger.accountId,
                            direction = (int)EnumPA_TransactionDirection.Giris,
                            transactionId = this.Transaction.id,
                            currencyId = companyAccount.currencyId,
                            date = this.Ledger.date,
                            type = (int)EnumPA_LedgerType.Tahsilat,
                            description = "Ödeme"
                        };

                        var currency = CurrencyExchangeRates.GetExchangeRatesByDate(companyAccount.Currency_Code, this.Ledger.date.Value.Date);
                        var rate = currency.BanknoteSelling;

                        var crossCurrency = CurrencyExchangeRates.GetCrossExchangeRatesByDate(companyAccount.Currency_Code, invoiceView.Currency_Code, this.Ledger.date.Value);
                        var crossRate = crossCurrency.BanknoteBuying;

                        newLedger.rateExchange = rate;
                        newLedger.amount = (invoiceView.totalAmount - invoiceView.totalTax) / crossRate;
                        newLedger.tax = invoiceView.totalTax / crossRate;
                        newLedger.crossRate = crossRate;

                        rs &= db.InsertPA_Ledger(new PA_Ledger().B_EntityDataCopyForMaterial(this.Ledger, true), _trans);
                        rs &= db.InsertPA_Ledger(newLedger, _trans);

                        rs &= db.InsertCMP_InvoiceAction(new CMP_InvoiceAction
                        {
                            id = Guid.NewGuid(),
                            createdby = this.createdby,
                            created = DateTime.Now,
                            invoiceId = this.id,
                            type = (int)EnumCMP_InvoiceActionType.FaturaOdeme,
                            description = String.Format("{0:N}", invoiceView.totalAmount) + " " + invoiceView.Currency_Symbol + " ödendi."
                        }, _trans);

                        break;
                    case (short)EnumCMP_InvoiceStatus.CalisanOdedi:

                        var account = db.GetVWPA_AccountByDataIdDataTable(this.Account.dataId.Value, "SH_User");

                        if (account == null)
                        {
                            var currencies = db.GetUT_Currency();
                            var TL = currencies.Where(a => a.code == "TL").FirstOrDefault();

                            account = new VWPA_Account()
                            {
                                id = Guid.NewGuid(),
                                created = DateTime.Now,
                                createdby = this.createdby,
                                code = BusinessExtensions.B_GetIdCode(),
                                currencyId = TL.id,
                                isDefault = true,
                                name = "Ana Hesap",
                                status = true,
                                type = (int)EnumPA_AccountType.Kasa,
                                dataId = this.Account.dataId.Value,
                                dataTable = "SH_User",
                                Currency_Code = "TL"
                            };

                            rs &= db.InsertPA_Account(new PA_Account().B_EntityDataCopyForMaterial(account), _trans);
                        }

                        var currency2 = CurrencyExchangeRates.GetExchangeRatesByDate(account.Currency_Code, this.Ledger.date.Value.Date);
                        var rate2 = currency2.BanknoteBuying;

                        var crossCurrency2 = CurrencyExchangeRates.GetCrossExchangeRatesByDate(account.Currency_Code, invoiceView.Currency_Code, this.Ledger.date.Value);
                        var crossRate2 = crossCurrency2.BanknoteBuying;

                        this.Ledger.accountId = account.id;
                        this.Ledger.currencyId = account.currencyId;
                        this.Ledger.date = this.Transaction.date;
                        this.Ledger.rateExchange = rate2;
                        this.Ledger.amount = this.Ledger.amount / crossRate2;
                        this.Ledger.tax = this.Ledger.tax / crossRate2;
                        this.Ledger.crossRate = crossRate2;
                        rs &= db.InsertPA_Ledger(new PA_Ledger().B_EntityDataCopyForMaterial(this.Ledger, true), _trans);

                        rs &= db.InsertCMP_InvoiceAction(new CMP_InvoiceAction
                        {
                            id = Guid.NewGuid(),
                            createdby = this.createdby,
                            created = DateTime.Now,
                            invoiceId = this.id,
                            type = (int)EnumCMP_InvoiceActionType.FaturaOdeme,
                            description = String.Format("{0:N}", invoiceView.totalAmount) + " " + invoiceView.Currency_Symbol + " çalışan ödedi."
                        }, _trans);

                        rs &= db.InsertPA_Transaction(new PA_Transaction
                        {
                            accountId = account.id,
                            amount = invoiceView.totalAmount,
                            currencyId = this.currencyId,
                            date = this.Transaction.date,
                            direction = (int)EnumPA_TransactionDirection.Cikis,
                            invoiceId = this.id,
                            type = (int)EnumPA_TransactionType.AlisFatura
                        }, _trans);

                        break;
                    default:
                        break;
                }
            }
            else
            {
                switch (this.paymentType)
                {
                    case (short)EnumCMP_InvoicePaymentType.Pesin:
                        this.Transaction.date = this.issueDate;
                        this.Transaction.progressDate = this.issueDate;
                        this.Ledger.date = this.issueDate;
                        this.Ledger.type = (int)EnumPA_LedgerType.Tahsilat;

                        if (this.Transaction.accountId.HasValue)
                        {
                            var newLedger = new PA_Ledger
                            {
                                accountId = companyAccount.id,
                                accountRealtedId = this.Ledger.accountId,
                                direction = (int)EnumPA_TransactionDirection.Cikis,
                                transactionId = this.Transaction.id,
                                currencyId = companyAccount.currencyId,
                                date = this.Ledger.date,
                                type = (int)EnumPA_LedgerType.Tahsilat,
                                description = "Tahsilat"
                            };

                            var currency = CurrencyExchangeRates.GetExchangeRatesByDate(companyAccount.Currency_Code, this.Ledger.date.Value.Date);
                            var rate = currency.BanknoteBuying;

                            var crossCurrency = CurrencyExchangeRates.GetCrossExchangeRatesByDate(companyAccount.Currency_Code, invoiceView.Currency_Code, this.Ledger.date.Value);
                            var crossRate = crossCurrency.BanknoteBuying;

                            newLedger.rateExchange = rate;
                            newLedger.amount = (invoiceView.totalAmount - invoiceView.totalTax) / crossRate;
                            newLedger.tax = invoiceView.totalTax / crossRate;
                            newLedger.crossRate = crossRate;

                            rs &= db.InsertPA_Transaction(new PA_Transaction().B_EntityDataCopyForMaterial(this.Transaction, true), _trans);
                            rs &= db.InsertPA_Ledger(new PA_Ledger().B_EntityDataCopyForMaterial(this.Ledger, true), _trans);
                            rs &= db.InsertPA_Ledger(newLedger, _trans);
                            rs &= db.InsertCMP_InvoiceAction(new CMP_InvoiceAction
                            {
                                id = Guid.NewGuid(),
                                createdby = this.createdby,
                                created = DateTime.Now,
                                invoiceId = this.id,
                                type = (int)EnumCMP_InvoiceActionType.FaturaTahsilat,
                                description = String.Format("{0:N}", invoiceView.totalAmount) + " " + invoiceView.Currency_Symbol + " tahsil edildi."
                            }, _trans);
                        }
                        break;
                    case (short)EnumCMP_InvoicePaymentType.Vadeli:
                        this.Transaction.date = this.expiryDate;
                        rs &= db.InsertPA_Transaction(new PA_Transaction().B_EntityDataCopyForMaterial(this.Transaction, true), _trans);
                        break;
                    case (short)EnumCMP_InvoicePaymentType.Taksitli:
                        var listTransaction = new List<PA_Transaction>();
                        var account = db.GetPA_AccountIsDefaultByDataId(this.customerId.Value).FirstOrDefault();
                        for (int i = 0; i < this.installmentCount; i++)
                        {
                            listTransaction.Add(new PA_Transaction
                            {
                                accountId = account.id,
                                amount = (invoiceView.totalAmount - invoiceView.totalTax) / this.installmentCount,
                                tax = invoiceView.totalTax / this.installmentCount,
                                dataId = invoiceView.id,
                                dataTable = "CMP_Invoice",
                                status = (int)EnumPA_TransactionStatus.Odenecek,
                                created = DateTime.Now,
                                createdby = this.createdby,
                                date = this.issueDate.Value.AddMonths(i),
                                currencyId = this.currencyId,
                                direction = (short)EnumPA_TransactionDirection.Giris,
                                invoiceId = this.id,
                                type = (int)EnumPA_TransactionType.SatisFatura
                            });
                        }

                        rs &= db.BulkInsertPA_Transaction(listTransaction.ToArray(), _trans);
                        break;
                }
            }

            if (trans == null)
            {
                if (rs.result)
                    _trans.Commit();
                else
                    _trans.Rollback();
            }

            return rs;
        }

        private ResultStatus InsertWaybill(IEnumerable<VWPRD_Product> products, DbTransaction trans = null)
        {
            var _trans = trans ?? db.BeginTransaction();

            var transactionModel = new VMPRD_TransactionModel
            {
                invoiceId = this.id,
                inputId = this.customerId,
                inputTable = "CMP_Company",
                outputId = this.supplierId,
                outputTable = "CMP_Company",
                created = this.created,
                createdby = this.createdby,
                status = (int)EnumPRD_TransactionStatus.beklemede,
                items = this.InvoiceItems.Where(s => s.productId.HasValue && products.Select(d => d.id).Contains(s.productId.Value)).Select(a => new VMPRD_TransactionItems
                {
                    productId = a.productId,
                    quantity = a.quantity,
                    serialCodes = a.description != null ? a.description : "",
                    unitPrice = a.price,
                }).ToList(),
                date = this.issueDate,
                code = BusinessExtensions.B_GetIdCode(),
                type = this.direction == (int)EnumCMP_InvoiceDirectionType.Satis ? (short)EnumPRD_TransactionType.GidenIrsaliye : (short)EnumPRD_TransactionType.GelenIrsaliye,
            };

            if (this.type == (int)EnumCMP_InvoiceType.Siparis)
            {
                transactionModel.orderId = this.id;
            }

            var dbres = transactionModel.Save(this.createdby, _trans);

            if (dbres.result)
            {
                var storagePersonIds = db.GetSH_UserByRoleId(new Guid(SHRoles.DepoSorumlusu));
                var storagePersons = new List<VWSH_User>();
                if (storagePersonIds.Count() > 0)
                {
                    storagePersons = db.GetVWSH_UserByIds(storagePersonIds).ToList();
                }

                foreach (var user in storagePersons)
                {
                    if (this.direction == (int)EnumCMP_InvoiceDirectionType.Alis)
                    {
                        var text = "<h3>Sayın " + user.FullName + "</h3>";
                        text += "<p>" + this.serialNumber + "/" + this.rowNumber + " kodlu irsaliyeli fatura kesilmiştir. </p>";
                        text += "<p>İrsaliyeleri kontrol ederek şube/depo/kısım girişlerini gerçekleştirmek için <a href='" + _siteURL + "/PRD/VWPRD_Transaction'>tıklayınız.</a> </p>";
                        text += "<p>Bilgilerinize.</p>";
                        new Email().Template("Template1", "satinalma.jpg", _tenantName + " | WorkOfTime | Satın Alma Yönetimi", text).Send((Int16)EmailSendTypes.Fatura, user.email, "Fatura ve İrsaliye Oluşturuldu", true);
                    }
                    else
                    {
                        var text = "<h3>Sayın " + user.FullName + "</h3>";
                        text += "<p>" + this.serialNumber + "/" + this.rowNumber + " kodlu irsaliyeli fatura kesilmiştir. </p>";
                        text += "<p>İrsaliyeleri kontrol ederek şube/depo/kısım çıkışlarını gerçekleştirmek için <a href='" + _siteURL + "/PRD/VWPRD_Transaction'>tıklayınız.</a> </p>";
                        text += "<p>Bilgilerinize.</p>";
                        new Email().Template("Template1", "satinalma.jpg", _tenantName + " | WorkOfTime | Satış Sipariş Yönetimi", text).Send((Int16)EmailSendTypes.Fatura, user.email, "Fatura ve İrsaliye Oluşturuldu", true);
                    }
                }
            }

            if (trans == null)
            {
                if (dbres.result)
                    _trans.Commit();
                else
                    _trans.Rollback();
            }

            return dbres;
        }

        public ResultStatus Delete(PRD_Transaction[] prd_trans, DbTransaction trans = null)
        {
            db = db ?? new WorkOfTimeDatabase();
            var _trans = trans ?? db.BeginTransaction();

            var invoice = db.GetCMP_InvoiceById(this.id);
            var action = db.GetCMP_InvoiceActionByInvoiceId(this.id);
            var transformTo = db.GetCMP_InvoiceTransformByIsTransformedTo(this.id);
            var transformFrom = db.GetCMP_InvoiceTransformByIsTransformedFrom(this.id);
            var items = db.GetCMP_InvoiceItemByInvoiceId(this.id);
            var prdTransactionItem = db.GetPRD_TransactionItemByTransactionIds(prd_trans.Select(a => a.id).ToArray()).ToArray();
            var transactions = db.GetPA_TransactionByInvoiceId(this.id);
            var ledgers = db.GetPA_LedgerByTransactionIds(transactions.Select(a => a.id).ToArray());

            var dbresult = db.BulkDeletePA_Ledger(ledgers, _trans);
            dbresult &= db.BulkDeletePA_Transaction(transactions, _trans);
            dbresult &= db.BulkDeleteCMP_InvoiceTransform(transformFrom, _trans);
            dbresult &= db.BulkDeleteCMP_InvoiceTransform(transformTo, _trans);
            dbresult &= db.BulkDeleteCMP_InvoiceAction(action, _trans);
            dbresult &= db.BulkDeleteCMP_InvoiceItem(items, _trans);
            dbresult &= db.BulkDeletePRD_TransactionItem(prdTransactionItem, _trans);
            dbresult &= db.BulkDeletePRD_Transaction(prd_trans, _trans);
            dbresult &= db.DeleteCMP_Invoice(invoice, _trans);

            if (trans == null)
            {
                if (dbresult.result)
                    _trans.Commit();
                else
                    _trans.Rollback();
            }

            return dbresult;
        }

        public InvoiceFormTemplateModels GetFormTemplate(int? type)
        {
            var invoice = db.GetVWCMP_InvoiceById(this.id);

            var model = new InvoiceFormTemplateModels().B_EntityDataCopyForMaterial(invoice);

            if (type.HasValue)
            {
                model.typeJob = type.Value;
            }

            model.Items = db.GetVWCMP_InvoiceItemByInvoiceId(this.id);

            if (this.customerId.HasValue)
            {
                model.Customer = db.GetVWCMP_CompanyById(this.customerId.Value);
            }

            if (this.supplierId.HasValue)
            {
                model.Supplier = db.GetVWCMP_CompanyById(this.supplierId.Value);
                var file = db.GetSYS_FilesByDataId(this.supplierId.Value);

                if (file != null)
                {
                    model.LogoPath = file.FilePath;
                }
            }

            return model;
        }

        public ResultStatus InsertNote(Guid userId, string note)
        {
            var action = new CMP_InvoiceAction
            {
                id = Guid.NewGuid(),
                created = DateTime.Now,
                createdby = userId,
                invoiceId = this.id,
                description = note,
                type = (short)EnumCMP_InvoiceActionType.NotEkle
            };

            var res = db.InsertCMP_InvoiceAction(action);
            res.objects = db.GetVWCMP_InvoiceActionById(action.id);

            return res;
        }


        public static SimpleQuery UpdateQuery(SimpleQuery query, PageSecurity userStatus)
        {
            BEXP filter = null;

            filter = new BEXP
            {
                Operand1 = new BEXP
                {
                    Operand1 = (COL)"createdby",
                    Operator = BinaryOperator.Equal,
                    Operand2 = (VAL)(userStatus.user.id)
                },

                Operator = BinaryOperator.Or,
                Operand2 = new BEXP
                {
                    Operand1 = (COL)"templateVisibleAllUser",
                    Operator = BinaryOperator.Equal,
                    Operand2 = (VAL)(int)EnumCMP_InvoiceDocumentTemplateVisible.Evet
                }
            };

            query.Filter &= filter;
            return query;

        }
    }
}
