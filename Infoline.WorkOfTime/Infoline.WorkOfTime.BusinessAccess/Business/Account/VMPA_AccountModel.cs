using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Infoline.WorkOfTime.BusinessAccess
{
    public class PA_AccountSummary
    {
        public double SumInput { get; set; } = 0;
        public double SumOutput { get; set; } = 0;
        public double Balance { get; set; } = 0;
    }

    public class VMPA_AccountModel : VWPA_Account
    {
        private WorkOfTimeDatabase db { get; set; }
        private DbTransaction trans { get; set; }
        public VWPA_Ledger Ledger { get; set; }
        public VWPA_Transaction Transaction { get; set; }
        public bool isOpenBalance { get; set; }
        public VWCMP_Company Company { get; set; }
        public VWSH_User User { get; set; }
        public PA_AccountSummary Summary { get; set; } = new PA_AccountSummary();

        public VMPA_AccountModel Load()
        {
            this.db = this.db ?? new WorkOfTimeDatabase();
            var account = db.GetVWPA_AccountById(this.id) ?? db.GetVWPA_AccountByCode(this.code);
            var currencyItem = db.GetUT_CurrencyIdByCode("TL");

            if (currencyItem != null)
            {
                this.currencyId = currencyItem.id;
            }

            if (account != null)
            {
                this.B_EntityDataCopyForMaterial(account, true);

                if (this.dataTable == "CMP_Company" && this.dataId.HasValue)
                {
                    this.Company = db.GetVWCMP_CompanyById(this.dataId.Value);
                }
                if (this.dataTable == "SH_User" && this.dataId.HasValue)
                {
                    this.User = db.GetVWSH_UserById(this.dataId.Value);
                }

                var ledgers = db.GetVWPA_LedgerbyAccountId(this.id);

                if (ledgers.Count() > 0)
                {
                    this.Summary.SumInput = (double)ledgers.Sum(a => a.inputAmount);
                    this.Summary.SumOutput = (double)ledgers.Sum(a => a.outputAmount);
                    this.Summary.Balance = this.Summary.SumInput - this.Summary.SumOutput;
                }
            }
            else
            {
                this.code = this.code ?? BusinessExtensions.B_GetIdCode();
                this.iban = this.iban ?? "TR";
            }

            return this;
        }

        public ResultStatus Save(Guid userId, HttpRequestBase request = null, DbTransaction trans = null)
        {
            db = db ?? new WorkOfTimeDatabase();
            var account = db.GetVWPA_AccountById(this.id) ?? db.GetVWPA_AccountByCode(this.code);
            var res = new ResultStatus { result = true };

            var _code = db.GetVWPA_AccountByCode(this.code);

            if (_code != null && _code.id != this.id)
            {
                return new ResultStatus { result = false, message = "Kod Daha Önce Kayıt Edilmiş. Lütfen Başka Bir Kod ile Deneyiniz." };
            }

            if (this.type == (int)EnumPA_AccountType.Banka)
            {
                if (this.iban.Length != 26)
                {
                    return new ResultStatus { result = false, message = "IBAN 26 karakter olmalıdır." };
                }

                var _iban = db.GetVWPA_AccountByIBAN(this.iban);
                if ((_iban != null && account != null && this.iban != account.iban) || (_iban != null && account == null))
                {
                    return new ResultStatus { result = false, message = "IBAN Numarası Daha Önce Kayıt Edilmiş. Lütfen Başka Bir İban Numarası ile Deneyiniz." };
                }
            }

            if (account == null)
            {
                this.created = DateTime.Now;
                this.createdby = userId;
                res = Insert(trans);
            }
            else
            {
                this.changed = DateTime.Now;
                this.changedby = userId;
                res = Update(trans);
            }

            return res;

        }

        private ResultStatus Insert(DbTransaction trans = null)
        {
            var transaction = trans ?? db.BeginTransaction();
            var dbresult = db.InsertPA_Account(new PA_Account().B_EntityDataCopyForMaterial(this), transaction);

            if (this.isDefault == true)
            {
                if (this.dataId.HasValue)
                {
                    var accounts = db.GetPA_AccountIsDefaultByDataId(this.dataId.Value).Where(a => a.id != this.id);
                    foreach (var item in accounts)
                    {
                        item.isDefault = false;
                    }

                    dbresult &= db.BulkUpdatePA_Account(accounts, false, transaction);
                }
            }

            if (this.Ledger.amount.HasValue && this.Ledger.amount != 0)
            {
                this.Ledger.tax = 0;
                this.Ledger.accountId = this.id;
                this.Ledger.created = this.created;
                this.Ledger.createdby = this.createdby;
                this.Ledger.type = (int)EnumPA_LedgerType.AcilisBakiye;
                this.Ledger.currencyId = this.currencyId;
                this.Ledger.direction = (int)EnumPA_TransactionDirection.Giris;
                dbresult &= db.InsertPA_Ledger(new PA_Ledger().B_EntityDataCopyForMaterial(this.Ledger), transaction);
            }

            if (trans == null)
            {
                if (dbresult.result)
                    transaction.Commit();
                else
                    transaction.Rollback();
            }

            return new ResultStatus
            {
                result = dbresult.result,
                message = dbresult.result ? "Hesap kaydetme işlemi başarılı bir şekilde gerçekleştirildi." : "Hesap kaydetme işlemi başarısız oldu."
            };
        }

        private ResultStatus Update(DbTransaction trans = null)
        {
            var transaction = trans ?? db.BeginTransaction();

            var dbresult = db.UpdatePA_Account(new PA_Account().B_EntityDataCopyForMaterial(this), true, transaction);

            if (this.isDefault == true)
            {
                if (this.dataId.HasValue)
                {
                    var accounts = db.GetPA_AccountIsDefaultByDataId(this.dataId.Value).Where(a => a.id != this.id);
                    foreach (var item in accounts)
                    {
                        item.isDefault = false;
                    }

                    dbresult &= db.BulkUpdatePA_Account(accounts, false, transaction);
                }
            }

            if (trans == null)
            {
                if (dbresult.result)
                    transaction.Commit();
                else
                    transaction.Rollback();
            }

            return new ResultStatus
            {
                result = dbresult.result,
                message = dbresult.result ? "Hesap güncelleme işlemi başarılı bir şekilde gerçekleştirildi." : "Hesap güncelleme işlemi başarısız oldu."
            };
        }

        public ResultStatus Delete(DbTransaction trans = null)
        {
            var _trans = trans ?? db.BeginTransaction();

            var account = db.GetPA_AccountById(this.id);
            var transaction = db.GetPA_TransactionByAccountId(this.id);
            var ledger = db.GetPA_LedgerByAccountId(this.id);

            var resultString = transaction.Count() > 0 || ledger.Count() > 0 ? "Hesaba ait işlemler bulunduğu için hesap silinemez." : "";
            resultString = String.IsNullOrEmpty(resultString) && account.isDefault == true ? "Hesap, işletme için varsayılan hesap olduğundan silinemez. Lütfen silmeden önce varsayılan hesabı değiştirin." : resultString;

            if (String.IsNullOrEmpty(resultString))
            {
                var dbresult = db.DeletePA_Account(id, _trans);

                if (trans == null)
                {
                    if (dbresult.result)
                        _trans.Commit();
                    else
                        _trans.Rollback();
                }


                return new ResultStatus
                {
                    result = dbresult.result,
                    message = dbresult.result ? "Hesap silme işlemi başarılı bir şekilde gerçekleştirildi." : "Hesap silme işlemi başarısız oldu."
                };
            }
            else
            {
                return new ResultStatus { result = false, message = resultString };
            }
        }
    }
}
