using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessData;
using Infoline.WorkOfTime.BusinessAccess.Models;
using System;
using System.Data.Common;
using System.Web;
using System.Linq;
using System.Collections.Generic;
namespace Infoline.WorkOfTime.BusinessAccess
{
    public class VMPA_TransactionModel : VWPA_Transaction
    {
        private WorkOfTimeDatabase db { get; set; }
        private DbTransaction trans { get; set; }
        public VWPA_Ledger Ledger { get; set; }
        public PA_Ledger[] Ledgers { get; set; }
        public VWPA_Account Account { get; set; }
        public SYS_Files[] Files { get; set; }
        public bool? IsCopy { get; set; }
        public Guid newId { get; set; }
        public VWPA_TransactionHistory[] TransactionHistory { get; set; }
        public DateTime? payBackDate { get; set; }
        public string statusDescription { get; set; }
        public bool hasTask { get; set; }
        public bool? isService { get; set; }
        public bool? isCorrection { get; set; }
        public class VWPA_TransactionHistory
        {
            public string description { get; set; }
            public DateTime date { get; set; }
            public short? status { get; set; }
            public string userId_Title { get; set; }
            public string ruleTitle { get; set; }
            public short? ruleOrder { get; set; }
            public short? ruleType { get; set; }
        }
        public VMPA_TransactionModel Load()
        {
            this.db = this.db ?? new WorkOfTimeDatabase();
            var transaction = db.GetVWPA_TransactionById(this.id);
            if (transaction != null)
            {
                this.B_EntityDataCopyForMaterial(transaction, true);
                if (this.type == (int)EnumPA_TransactionType.Masraf)
                {
                    var transactionConfirmation = db.GetVWPA_TransactionConfirmationByTransactionId(this.id);
                    if (transactionConfirmation.Count() > 0)
                    {
                        this.statusDescription = transactionConfirmation.Where(x => x.status == 3 || x.status == 0 && x.description != null).Select(a => a.description).FirstOrDefault();
                        TransactionHistory = transactionConfirmation.Where(a => a.confirmationUserIds != null).Select(x => new VWPA_TransactionHistory
                        {
                            date = x.created.Value,
                            description = x.description,
                            ruleTitle = x.ruleTitle,
                            userId_Title = x.confirmationUserIds_Titles,
                            status = x.status,
                            ruleType = x.ruleType,
                            ruleOrder = x.ruleOrder
                        }).ToArray();
                    }
                    //if (this.IsCopy == true)
                    //{
                    //    Files = db.GetSYS_FilesByDataIdAll(this.id);
                    //    db.BulkInsertSYS_Files(Files.Select(x => new SYS_Files { DataId = this.id, FilePath = x.FilePath, id = Guid.NewGuid(), FileGroup = x.FileGroup, DataTable = x.DataTable, FileExtension = x.FileExtension }));
                    //}
                }
            }
            else
            {
                if (this.type == (int)EnumPA_TransactionType.Masraf)
                {
                    this.status = (int)EnumPA_TransactionStatus.Odenecek;
                }
            }
            return this;
        }
        public VMPA_TransactionModel LoadMobile()
        {
            this.db = this.db ?? new WorkOfTimeDatabase();
            var transaction = db.GetVWPA_TransactionById(this.id);
            if (transaction != null)
            {
                this.B_EntityDataCopyForMaterial(transaction, true);
                if (this.type == (int)EnumPA_TransactionType.Masraf)
                {
                    var transactionConfirmation = db.GetVWPA_TransactionConfirmationByTransactionId(this.id);
                    if (transactionConfirmation.Count() > 0)
                    {
                        this.statusDescription = transactionConfirmation.Where(x => x.status == 0 && x.description != null).Select(a => a.description).FirstOrDefault();
                        TransactionHistory = transactionConfirmation.Where(a => a.confirmationUserIds != null).Select(x => new VWPA_TransactionHistory
                        {
                            date = x.created.Value,
                            description = x.description,
                            ruleTitle = x.ruleTitle,
                            userId_Title = x.confirmationUserIds_Titles,
                            status = x.status,
                            ruleType = x.ruleType,
                            ruleOrder = x.ruleOrder
                        }).ToArray();
                    }
                    Files = db.GetSYS_FilesByDataIdAll(this.id);
                    this.id = Guid.NewGuid();
                    this.newId = this.id;
                    db.BulkInsertSYS_Files(Files.Select(x => new SYS_Files { DataId = this.id, FilePath = x.FilePath, id = Guid.NewGuid(), FileGroup = x.FileGroup, DataTable = x.DataTable, FileExtension = x.FileExtension }));
                }
            }
            else
            {
                if (this.type == (int)EnumPA_TransactionType.Masraf)
                {
                    this.status = (int)EnumPA_TransactionStatus.Odenecek;
                }
            }
            return this;
        }
        public ResultStatus Save(Guid userId, HttpRequestBase request = null, DbTransaction trans = null)
        {
            db = db ?? new WorkOfTimeDatabase();
            if (this.ProjectId != null)
            {
                this.dataId = new Guid(this.ProjectId);
                this.dataTable = "PRJ_Project";
            }
            //if (this.dataId.HasValue && isService != null && !isService.Value )
            //{
            //	this.id = Guid.NewGuid();
            //}
            var transaction = db.GetVWPA_TransactionById(this.id);
            var res = new ResultStatus { result = true };
            if (this.status == (int)EnumPA_TransactionStatus.Odendi)
            {
                if (!this.Ledger.date.HasValue) { return new ResultStatus { result = false, message = "Ödeme yapılan tarihi seçmelisiniz. Lütfen kontrol ediniz!" }; }
                if (!this.Ledger.accountId.HasValue) { return new ResultStatus { result = false, message = "Ödenen hesabı seçmelisiniz. Lütfen kontrol ediniz!" }; }
            }
            else if (this.status == (int)EnumPA_TransactionStatus.Odenecek)
            {
                if (!this.date.HasValue && this.type != (int)EnumPA_TransactionType.Masraf) { return new ResultStatus { result = false, message = "Ödeme yapılacak tarihi seçmelisiniz. Lütfen kontrol ediniz!" }; }
            }
            else
            {
                if (!this.Account.dataId.HasValue) { return new ResultStatus { result = false, message = "Ödeme yapılan personeli seçmelisiniz. Lütfen kontrol ediniz!" }; }
            }
            if (this.IsCopy == true || transaction == null)
            {
                this.created = DateTime.Now;
                this.createdby = userId;
                if (this.IsCopy == true)
                {
                    this.id = this.newId;
                }
                res = Insert(trans);
            }
            else
            {
                this.changed = DateTime.Now;
                this.changedby = userId;
                res = Update(trans);
            }
            if (this.type == (Int16)EnumPA_TransactionType.Masraf)
            {
                var getTenantUrl = TenantConfig.Tenant.GetWebUrl();
                var notification= new Notification();
                var transactionConfirmations = db.GetVWPA_TransactionConfirmationByTransactionId(this.id);
                UpdateDataControl(transactionConfirmations, this.statusDescription, userId);
                if (this.direction == 2)// red ise
                {
                    if (this.createdby.HasValue)
                    {
                        var user = db.GetVWSH_UserById(this.createdby.Value);//işlemi oluşturan kişi
                        var getDeclineUser = db.GetVWSH_UserById(userId); // işlemi yapan kişi 
                        var text = "<h3>Sayın " + user.FullName + ",</h3>";
                        text += "<p>Masraf talebiniz reddedilmiştir.</p>";
                        if (!string.IsNullOrEmpty(this.statusDescription))
                        {
                            text += "<p>Açıklaması : " + this.statusDescription + "</p>";
                        }
                        text += "<div><a href='" + getTenantUrl + "/PA/VWPA_Transaction/IndexRequest" + "'>Detaya gitmek için tıklayınız.</a> </div>";
                        text += "<p>Bilgilerinize.</p>";
                        new Email().Template("Template1", "bos.png", TenantConfig.Tenant.TenantName + " | Masraf Talebi Reddi ", text).Send((Int16)EmailSendTypes.MasrafOnay, user.email, "Masraf Talebi Reddi", true);
                        notification.NotificationSend(user.id, "Masraf talebiniz reddedilmiştir", "Masraf talebiniz" + getDeclineUser.FullName + " tarafından reddedilmiştir");
                    }
                    
                }
                else if (this.direction == 3)// yeniden talep 
                {
                    if (this.createdby.HasValue)
                    {
                        var user = db.GetVWSH_UserById(this.createdby.Value);//işlemi oluşturan kişi
                        var getDeclineUser = db.GetVWSH_UserById(userId); // işlemi yapan kişi 
                        var text = "<h3>Sayın " + user.FullName + ",</h3>";
                        text += "<p>Masraf talebinize düzenleme istenmektedir.</p>";
                        if (!string.IsNullOrEmpty(this.statusDescription))
                        {
                            text += "<p>Açıklaması : " + this.statusDescription + "</p>";
                        }
                        text += "<div><a href='" + getTenantUrl + "/PA/VWPA_Transaction/IndexRequest" + "'>Detaya gitmek için tıklayınız.</a> </div>";
                        text += "<p>Bilgilerinize.</p>";
                        new Email().Template("Template1", "bos.png", TenantConfig.Tenant.TenantName + " | Masraf Talebi Düzenleme ", text).Send((Int16)EmailSendTypes.MasrafOnay, user.email, "Masraf Talebi Düzenleme", true);
                        notification.NotificationSend(user.id, "Masraf talebiniz reddedilmiştir", "Masraf talebiniz" + getDeclineUser.FullName + " tarafından düzenleme talebi istenmiştir");
                    }
                   
                }
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
            var dbresult = new ResultStatus { result = true };
            if (this.type == (int)EnumPA_TransactionType.Masraf)
            {
                var personManager = db.GetVWINV_CompanyPersonDepartmentsByIdUser(this.createdby.Value).Where(a => a.Manager1.HasValue);
                if (personManager.Count() == 0)
                {
                    this.direction = -1;
                }
                if (this.dataTable == "CRM_Presentation")
                {
                    var currency = db.GetUT_CurrencyById(this.currencyId.Value);
                    var color = this.direction == -1 ? "#1ab394" : "#f8ac59";
                    var status = this.direction == -1 ? " (Onaylandı) " : " (Onay bekleniyor) ";
                    dbresult &= db.InsertCRM_PresentationAction(new CRM_PresentationAction()
                    {
                        created = DateTime.Now,
                        createdby = this.createdby,
                        type = (int)EnumCRM_PresentationActionType.MasrafEkle,
                        description = "Potansiyel/Fırsata " + this.amount + currency.symbol + " masraf eklendi." + status,
                        presentationId = dataId,
                        color = color
                    }, trans);
                }
                var account = db.GetVWPA_AccountByDataId(this.createdby.Value);
                if (account != null)
                {
                    this.accountId = account.id;
                }
                else
                {
                    var currencies = db.GetUT_Currency();
                    var TL = currencies.Where(a => a.code == "TL").FirstOrDefault();
                    this.accountId = Guid.NewGuid();
                    dbresult &= db.InsertPA_Account(new PA_Account()
                    {
                        id = this.accountId.Value,
                        created = DateTime.Now,
                        createdby = this.createdby,
                        code = BusinessExtensions.B_GetIdCode(),
                        currencyId = TL.id,
                        isDefault = true,
                        name = "Ana Hesap",
                        status = true,
                        type = (int)EnumPA_AccountType.Kasa,
                        dataId = this.createdby,
                        dataTable = "SH_User"
                    }, transaction);
                }
            }
            this.tax = this.tax.HasValue ? this.tax.Value : 0;
            dbresult &= db.InsertPA_Transaction(new PA_Transaction().B_EntityDataCopyForMaterial(this), transaction);
            if (this.status == (int)EnumPA_TransactionStatus.Odendi || this.status == (int)EnumPA_TransactionStatus.CalisanOdedi)
            {
                dbresult &= InsertLedger(transaction);
            }
            if (trans == null)
            {
                if (dbresult.result)
                {
                    if (this.type == (Int16)EnumPA_TransactionType.Masraf)
                    {
                        dbresult &= InsertConfirmation(this.createdby.Value, transaction);
                    }
                    transaction.Commit();
                }
                else
                {
                    transaction.Rollback();
                }
            }
            var message = this.type == (int)EnumPA_TransactionType.Masraf ? "Masraf talebi " : "Gider ekleme işlemi ";
            return new ResultStatus
            {
                result = dbresult.result,
                message = dbresult.result ? message + "başarılı bir şekilde gerçekleştirildi." : message + "başarısız oldu."
            };
        }
        private ResultStatus Update(DbTransaction trans = null)
        {
            var transaction = trans ?? db.BeginTransaction();
            var dbresult = new ResultStatus { result = true };
            if (this.type == (int)EnumPA_TransactionType.Masraf && this.direction == -1)
            {
                this.date = DateTime.Now.AddMonths(1);
            }
            this.tax = this.tax.HasValue ? this.tax.Value : 0;
            if (this.type == (int)EnumPA_TransactionType.Masraf)
            {
                var confirmations = db.GetVWPA_TransactionConfirmationByTransactionId(this.id);
                if (confirmations.Count() > 0)
                {
                    var refreshConfirm = confirmations.Where(x => x.status == 3).FirstOrDefault();
                    if (refreshConfirm != null)
                    {
                        this.direction = 0;
                        refreshConfirm.status = null;
                        foreach (var confirmation in confirmations)
                        {
                            confirmation.status = null;
                            dbresult &= db.UpdatePA_TransactionConfirmation(new PA_TransactionConfirmation().B_EntityDataCopyForMaterial(confirmation), true, transaction);
                        }
                        dbresult &= db.UpdatePA_TransactionConfirmation(new PA_TransactionConfirmation().B_EntityDataCopyForMaterial(refreshConfirm), true, transaction);
                    }
                    else
                    {
                        var confirm = confirmations.Where(x => x.status == null).FirstOrDefault();
                        if (this.direction == -1)
                        {
                            if (confirmations.Count(x => x.status == null) == 1)
                            {
                                this.direction = -1;
                            }
                            else
                            {
                                this.direction = 0;
                            }
                            confirm.status = (Int16)EnumPA_TransactionConfirmationStatus.Onay;
                            confirm.description = this.statusDescription;
                        }
                        else if (this.direction == 3)
                        {
                            if (this.isCorrection.HasValue && this.isCorrection.Value)
                            {
                                this.direction = 0;
                                confirm.status = null;
                                var transactionConfirmation = db.GetPA_TransactionConfirmationByTransactionId(this.id);
                                db.BulkDeletePA_TransactionConfirmation(transactionConfirmation);
                                var transactionCofirmations = new List<PA_TransactionConfirmation>();
                                var rulesUser = db.GetVWUT_RulesUserByUserIdAndType(this.createdby.Value, (Int16)EnumUT_RulesType.Transaction);
                                var shuser = db.GetVWSH_UserById(this.createdby.Value);
                                if (rulesUser != null)
                                {
                                    var rulesUserStages = db.GetVWUT_RulesUserStageByRulesId(rulesUser.rulesId.Value);
                                    if (rulesUserStages.Count() > 0)
                                    {
                                        var tabAmount = rulesUserStages.Where(a => this.amount == a.limit || this.amount <= a.limit).FirstOrDefault();
                                        if (tabAmount == null)
                                        {
                                            transactionCofirmations.AddRange(rulesUserStages.Select(a => new PA_TransactionConfirmation
                                            {
                                                created = this.created,
                                                createdby = this.createdby,
                                                transactionId = this.id,
                                                ruleType = a.type,
                                                ruleOrder = a.order,
                                                ruleUserId = a.userId,
                                                ruleRoleId = a.roleId,
                                                ruleTitle = a.title,
                                                userId = (a.type == (Int16)EnumUT_RulesUserStage.Manager1 ? shuser?.Manager1 :
                                                a.type == (Int16)EnumUT_RulesUserStage.Manager2 ? shuser?.Manager2 :
                                                a.type == (Int16)EnumUT_RulesUserStage.Manager3 ? shuser?.Manager3 :
                                                a.type == (Int16)EnumUT_RulesUserStage.Manager4 ? shuser?.Manager4 :
                                                a.type == (Int16)EnumUT_RulesUserStage.Manager5 ? shuser?.Manager5 :
                                                a.type == (Int16)EnumUT_RulesUserStage.Manager6 ? shuser?.Manager6 :
                                                a.type == (Int16)EnumUT_RulesUserStage.SecimeBagliKullanici ? a.userId : null)
                                            }));
                                        }
                                        else
                                        {
                                            foreach (var rulesUserStage in rulesUserStages.Where(a => a.order <= tabAmount.order).ToList())
                                            {
                                                confirm = new VWPA_TransactionConfirmation
                                                {
                                                    created = this.created,
                                                    createdby = this.createdby,
                                                    transactionId = this.id,
                                                    ruleType = rulesUserStage.type,
                                                    ruleOrder = rulesUserStage.order,
                                                    ruleUserId = rulesUserStage.userId,
                                                    ruleRoleId = rulesUserStage.roleId,
                                                    ruleTitle = rulesUserStage.title,
                                                    userId = (rulesUserStage.type == (Int16)EnumUT_RulesUserStage.Manager1 ? shuser?.Manager1 :
                                                    rulesUserStage.type == (Int16)EnumUT_RulesUserStage.Manager2 ? shuser?.Manager2 :
                                                    rulesUserStage.type == (Int16)EnumUT_RulesUserStage.Manager3 ? shuser?.Manager3 :
                                                    rulesUserStage.type == (Int16)EnumUT_RulesUserStage.Manager4 ? shuser?.Manager4 :
                                                    rulesUserStage.type == (Int16)EnumUT_RulesUserStage.Manager5 ? shuser?.Manager5 :
                                                    rulesUserStage.type == (Int16)EnumUT_RulesUserStage.Manager6 ? shuser?.Manager6 :
                                                    rulesUserStage.type == (Int16)EnumUT_RulesUserStage.SecimeBagliKullanici ? rulesUserStage.userId : null)
                                                };
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    var defaultRule = db.GetUT_RulesByTypeIsDefault((Int16)EnumUT_RulesType.Transaction);
                                    if (defaultRule == null)
                                    {
                                        return new ResultStatus
                                        {
                                            result = false,
                                            message = "Hiç bir masraf kuralı bulunamadı."
                                        };
                                    }
                                    else
                                    {
                                        var rulesUserStages = db.GetVWUT_RulesUserStageByRulesId(defaultRule.id);
                                        if (rulesUserStages.Count() > 0)
                                        {
                                            var tabAmount = rulesUserStages.Where(a => this.amount == a.limit || this.amount <= a.limit).FirstOrDefault();
                                            if (tabAmount == null)
                                            {
                                                transactionCofirmations.AddRange(rulesUserStages.Select(a => new PA_TransactionConfirmation
                                                {
                                                    created = this.created,
                                                    createdby = this.createdby,
                                                    transactionId = this.id,
                                                    ruleType = a.type,
                                                    ruleOrder = a.order,
                                                    ruleUserId = a.userId,
                                                    ruleRoleId = a.roleId,
                                                    ruleTitle = a.title
                                                }));
                                            }
                                            else
                                            {
                                                foreach (var rulesUserStage in rulesUserStages.Where(a => a.order <= tabAmount.order).ToList())
                                                {
                                                    transactionCofirmations.Add(new PA_TransactionConfirmation
                                                    {
                                                        created = this.created,
                                                        createdby = this.createdby,
                                                        transactionId = this.id,
                                                        ruleType = rulesUserStage.type,
                                                        ruleOrder = rulesUserStage.order,
                                                        ruleUserId = rulesUserStage.userId,
                                                        ruleRoleId = rulesUserStage.roleId,
                                                        ruleTitle = rulesUserStage.title
                                                    });
                                                }
                                            }
                                        }
                                    }
                                }
                                dbresult &= db.BulkInsertPA_TransactionConfirmation(transactionCofirmations, transaction);
                            }
                            else
                            {
                                this.direction = 3;
                                confirm.status = (Int16)EnumPA_TransactionConfirmationStatus.YenidenTalep;
                                confirm.description = this.statusDescription;
                            }
                        }
                        else
                        {
                            this.direction = 2;
                            confirm.status = (Int16)EnumPA_TransactionConfirmationStatus.Red;
                            confirm.description = this.statusDescription;
                        }
                        dbresult &= db.UpdatePA_TransactionConfirmation(new PA_TransactionConfirmation().B_EntityDataCopyForMaterial(confirm), true, transaction);
                    }
                }
            }
            dbresult &= db.UpdatePA_Transaction(new PA_Transaction().B_EntityDataCopyForMaterial(this), false, transaction);
            if (this.status == (int)EnumPA_TransactionStatus.Odendi || this.status == (int)EnumPA_TransactionStatus.CalisanOdedi)
            {
                dbresult &= InsertLedger(transaction);
            }
            if (trans == null)
            {
                if (dbresult.result)
                    transaction.Commit();
                else
                    transaction.Rollback();
            }
            var message = "Gider düzenleme işlemi ";
            if (this.type == (int)EnumPA_TransactionType.Masraf)
            {
                message = this.direction == 0 ? "Masraf düzenleme işlemi " : this.direction == -1 ? "Masraf onaylama işlemi " : " Masraf reddetme işlemi ";
                var pA_Transaction = db.GetPA_TransactionById(this.id);
                if (pA_Transaction != null)
                {
                    var currency = db.GetUT_CurrencyById(this.currencyId.Value);
                    if (pA_Transaction.dataTable == "CRM_Presentation")
                    {
                        var color = this.direction == 0 ? "#3343a4" : this.direction == -1 ? "#1ab394" : "#ed5565";
                        dbresult &= db.InsertCRM_PresentationAction(new CRM_PresentationAction()
                        {
                            created = DateTime.Now,
                            createdby = this.createdby,
                            type = (int)EnumCRM_PresentationActionType.MasrafDüzenlendi,
                            description = "Potansiyel/Fırsat " + message + " gerçekleştirildi. " + "Masraf " + this.amount + " " + currency.symbol + ".",
                            presentationId = pA_Transaction.dataId ?? new Guid(),
                            color = color
                        }, trans);
                    }
                }
            }
            if (dbresult.result && this.direction == (int)EnumPA_TransactionDirection.Cikis)
            {
                var notify = new Notification();
                string[] roles = new string[3] { SHRoles.OnMuhasebe, SHRoles.MuhasebeAlis, SHRoles.MuhasebeAlis };
                var userIds = db.GetSH_UserRoleByRoleIds(roles).Where(a => a.userid.HasValue).Select(x => x.userid.Value).ToArray();
                if (userIds.Count() > 0)
                {
                    var users = db.GetVWSH_UserByIds(userIds);
                    var confirming = db.GetVWSH_UserById(this.changedby.Value);
                    var createdUser = db.GetVWSH_UserById(this.createdby.Value);
                    foreach (var user in users)
                    {
                        var text = "<h3>Sayın " + user.FullName + ",</h3>";
                        text += "<p>" + this.createdby_Title + " kişisinin Masraf Talebi " + confirming.FullName + " tarafından onaylanmıştır.</p>";
                        if (!string.IsNullOrEmpty(this.description))
                        {
                            text += "<p>Açıklaması : " + this.description + "</p>";
                        }
                        text += "<p>Ödemeniz beklenmektedir.</p>";
                        text += "<p>Bilgilerinize.</p>";
                        new Email().Template("Template1", "bos.png", TenantConfig.Tenant.TenantName + " | Masraf Onayı ", text).Send((Int16)EmailSendTypes.MasrafOnay, user.email, "Masraf Onayı", true);
                        notify.NotificationSend(user.id, createdUser.FullName + " kişisi masraf talebinde bulunmuştur", "");
                    }
                }
            }
            return new ResultStatus
            {
                result = dbresult.result,
                message = dbresult.result ? message + "başarılı bir şekilde gerçekleştirildi." : message + "başarısız oldu."
            };
        }
        private ResultStatus InsertLedger(DbTransaction trans = null)
        {
            var transaction = trans ?? db.BeginTransaction();
            var dbresult = new ResultStatus { result = true };
            var currency = db.GetUT_CurrencyById(this.currencyId.Value);
            var rate = CurrencyExchangeRates.GetExchangeRatesByDate(currency.code, this.Ledger.date.Value);
            if (this.status == (int)EnumPA_TransactionStatus.Odendi)
            {
                var account = db.GetVWPA_AccountById(this.Ledger.accountId.Value);
                var newLedger = new VMPA_LedgerModel
                {
                    transactionId = this.id,
                    date = this.Ledger.date,
                    created = this.created,
                    createdby = this.createdby,
                    type = (int)EnumPA_LedgerType.Odeme,
                    isBalanceFixing = false,
                    tax = this.tax,
                    amount = this.amount,
                    currencyId = this.currencyId,
                    Currency_Code = currency.code,
                    Currency_Symbol = currency.symbol
                };
                if (this.type == (int)EnumPA_TransactionType.Vergi || this.type == (int)EnumPA_TransactionType.BankaGideri)
                {
                    newLedger.accountId = account.id;
                    newLedger.direction = this.direction;
                }
                else
                {
                    newLedger.accountId = this.accountId;
                    newLedger.accountRealtedId = account.id;
                    newLedger.direction = (short)(this.direction.Value * -1);
                }
                dbresult &= newLedger.Save(this.createdby.Value, null, transaction);
            }
            if (this.status == (int)EnumPA_TransactionStatus.CalisanOdedi)
            {
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
                    dbresult &= db.InsertPA_Account(new PA_Account().B_EntityDataCopyForMaterial(account), transaction);
                }
                dbresult &= new VMPA_LedgerModel
                {
                    accountId = this.Account.dataId,
                    transactionId = this.id,
                    date = this.Ledger.date,
                    accountRealtedId = this.accountId,
                    created = this.created,
                    createdby = this.createdby,
                    type = (int)EnumPA_LedgerType.Odeme,
                    isBalanceFixing = false,
                    direction = this.direction,
                    tax = this.tax,
                    amount = this.amount,
                    currencyId = account.currencyId,
                    Currency_Code = account.Currency_Code,
                    Currency_Symbol = account.Currency_Symbol
                }.Save(this.createdby.Value, null, transaction);
                dbresult &= db.InsertPA_Transaction(new PA_Transaction
                {
                    accountId = account.id,
                    tax = this.Ledger.tax,
                    amount = this.Ledger.amount,
                    currencyId = account.currencyId,
                    date = this.Ledger.date,
                    direction = (int)EnumPA_TransactionDirection.Cikis,
                    type = (int)EnumPA_TransactionType.FisFatura
                }, transaction);
            }
            if (trans == null)
            {
                if (dbresult.result)
                    transaction.Commit();
                else
                    transaction.Rollback();
            }
            return dbresult;
        }
        public ResultStatus Delete(DbTransaction trans = null)
        {
            db = db ?? new WorkOfTimeDatabase();
            var _trans = trans ?? db.BeginTransaction();
            var transaction = db.GetPA_TransactionById(this.id);
            var ledger = db.GetPA_LedgerByTransactionId(this.id);
            if (transaction == null)
            {
                return new ResultStatus
                {
                    result = false,
                    message = "Masraf Kaydı Bulunamadı."
                };
            }
            var dbresult = db.BulkDeletePA_Ledger(ledger, _trans);
            dbresult &= db.DeletePA_Transaction(transaction, _trans);
            if (trans == null)
            {
                if (dbresult.result == true)
                    _trans.Commit();
                else
                    _trans.Rollback();
            }
            return new ResultStatus
            {
                result = dbresult.result,
                message = dbresult.result ? "Masraf silme işlemi başarılı bir şekilde gerçekleştirildi." : "Masraf silme işlemi başarısız oldu."
            };
        }
        public static SimpleQuery UpdateQuery(SimpleQuery query, PageSecurity userStatus)
        {
            BEXP filter = null;
            if (!userStatus.AuthorizedRoles.Contains(new Guid(SHRoles.SistemYonetici)))
            {
                filter |= new BEXP
                {
                    Operand1 = (COL)"createdby",
                    Operator = BinaryOperator.Like,
                    Operand2 = (VAL)string.Format("%{0}%", userStatus.user.id.ToString())
                };
                filter |= new BEXP
                {
                    Operand1 = (COL)"type",
                    Operator = BinaryOperator.Equal,
                    Operand2 = (VAL)(int)EnumPA_TransactionType.Masraf
                };
            }
            query.Filter &= filter;
            return query;
        }
        public SummaryHeadersTransaction GetTransactionSummary(Guid userId)
        {
            this.Load();
            return db.GetVWPA_TransactionRequestsCountFilter(userId);
        }
        public SummaryHeadersTransaction GetListTransactionSummary(Guid userId)
        {
            this.Load();
            return db.GetVWPA_TransactionsByUserId(userId);
        }
        public SummaryHeadersTransaction GetMyApprovedTransactionSummary(Guid userId)
        {
            this.Load();
            return db.GetVWPA_TransactionsMyApprovedByUserId(userId);
        }
        public ResultStatus InsertConfirmation(Guid userId, DbTransaction trans = null)
        {
            var dbresult = new ResultStatus { result = true };
            var _trans = trans ?? db.BeginTransaction();
            this.db = this.db ?? new WorkOfTimeDatabase();
            var transactionCofirmations = new List<PA_TransactionConfirmation>();
            //Kullanıcıya ait masraf kurallarını çektim.
            var rulesUser = db.GetVWUT_RulesUserByUserIdAndType(userId, (Int16)EnumUT_RulesType.Transaction);
            var shuser = db.GetVWSH_UserById(userId);
            if (rulesUser != null)
            {
                var rulesUserStages = db.GetVWUT_RulesUserStageByRulesId(rulesUser.rulesId.Value);
                if (rulesUserStages.Count() > 0)
                {
                    var tabAmount = rulesUserStages.Where(a => this.amount == a.limit || this.amount <= a.limit).FirstOrDefault();
                    if (tabAmount == null)
                    {
                        transactionCofirmations.AddRange(rulesUserStages.Select(a => new PA_TransactionConfirmation
                        {
                            created = this.created,
                            createdby = userId,
                            transactionId = this.id,
                            ruleType = a.type,
                            ruleOrder = a.order,
                            ruleUserId = a.userId,
                            ruleRoleId = a.roleId,
                            ruleTitle = a.title,
                            userId = (a.type == (Int16)EnumUT_RulesUserStage.Manager1 ? shuser?.Manager1 :
                            a.type == (Int16)EnumUT_RulesUserStage.Manager2 ? shuser?.Manager2 :
                            a.type == (Int16)EnumUT_RulesUserStage.Manager3 ? shuser?.Manager3 :
                            a.type == (Int16)EnumUT_RulesUserStage.Manager4 ? shuser?.Manager4 :
                            a.type == (Int16)EnumUT_RulesUserStage.Manager5 ? shuser?.Manager5 :
                            a.type == (Int16)EnumUT_RulesUserStage.Manager6 ? shuser?.Manager6 :
                            a.type == (Int16)EnumUT_RulesUserStage.SecimeBagliKullanici ? a.userId : null)
                        }));
                    }
                    else
                    {
                        foreach (var rulesUserStage in rulesUserStages.Where(a => a.order <= tabAmount.order).ToList())
                        {
                            transactionCofirmations.Add(new PA_TransactionConfirmation
                            {
                                created = this.created,
                                createdby = userId,
                                transactionId = this.id,
                                ruleType = rulesUserStage.type,
                                ruleOrder = rulesUserStage.order,
                                ruleUserId = rulesUserStage.userId,
                                ruleRoleId = rulesUserStage.roleId,
                                ruleTitle = rulesUserStage.title,
                                userId = (rulesUserStage.type == (Int16)EnumUT_RulesUserStage.Manager1 ? shuser?.Manager1 :
                                rulesUserStage.type == (Int16)EnumUT_RulesUserStage.Manager2 ? shuser?.Manager2 :
                                rulesUserStage.type == (Int16)EnumUT_RulesUserStage.Manager3 ? shuser?.Manager3 :
                                rulesUserStage.type == (Int16)EnumUT_RulesUserStage.Manager4 ? shuser?.Manager4 :
                                rulesUserStage.type == (Int16)EnumUT_RulesUserStage.Manager5 ? shuser?.Manager5 :
                                rulesUserStage.type == (Int16)EnumUT_RulesUserStage.Manager6 ? shuser?.Manager6 :
                                rulesUserStage.type == (Int16)EnumUT_RulesUserStage.SecimeBagliKullanici ? rulesUserStage.userId : null)
                            });
                        }
                    }
                }
            }
            else
            {
                //Kullanıcının masraf kuralı yok ise varsayılanı çektim.
                var defaultRule = db.GetUT_RulesByTypeIsDefault((Int16)EnumUT_RulesType.Transaction);
                if (defaultRule == null)
                {
                    return new ResultStatus
                    {
                        result = false,
                        message = "Hiç bir masraf kuralı bulunamadı."
                    };
                }
                else
                {
                    var rulesUserStages = db.GetVWUT_RulesUserStageByRulesId(defaultRule.id);
                    if (rulesUserStages.Count() > 0)
                    {
                        var tabAmount = rulesUserStages.Where(a => this.amount == a.limit || this.amount <= a.limit).FirstOrDefault();
                        if (tabAmount == null)
                        {
                            transactionCofirmations.AddRange(rulesUserStages.Select(a => new PA_TransactionConfirmation
                            {
                                created = this.created,
                                createdby = userId,
                                transactionId = this.id,
                                ruleType = a.type,
                                ruleOrder = a.order,
                                ruleUserId = a.userId,
                                ruleRoleId = a.roleId,
                                ruleTitle = a.title
                            }));
                        }
                        else
                        {
                            foreach (var rulesUserStage in rulesUserStages.Where(a => a.order <= tabAmount.order).ToList())
                            {
                                transactionCofirmations.Add(new PA_TransactionConfirmation
                                {
                                    created = this.created,
                                    createdby = userId,
                                    transactionId = this.id,
                                    ruleType = rulesUserStage.type,
                                    ruleOrder = rulesUserStage.order,
                                    ruleUserId = rulesUserStage.userId,
                                    ruleRoleId = rulesUserStage.roleId,
                                    ruleTitle = rulesUserStage.title
                                });
                            }
                        }
                    }
                }
            }
            dbresult &= db.BulkInsertPA_TransactionConfirmation(transactionCofirmations, _trans);
            return new ResultStatus
            {
                result = dbresult.result,
                message = dbresult.result ? "Kayıt başarılı bir şekilde gerçekleştirildi." : "Kayıt başarısız oldu."
            };
        }
        public void UpdateDataControl(VWPA_TransactionConfirmation[] confirmations, string statusDescription, Guid userId)
        {
            if (this.direction == 0 || this.direction == -1 || this.direction == 1)
            {
                db = db ?? new WorkOfTimeDatabase();
                var getTenantUrl = TenantConfig.Tenant.GetWebUrl();
                var notification = new Notification();
                var notNullOrder = confirmations.Where(x => x.status != null).OrderByDescending(a => a.ruleOrder).FirstOrDefault();//en son onaylayan kişi
                var findUncommited = confirmations.Where(x => x.status == null && x.confirmationUserIds == null).ToList();
                foreach (var confirmation in findUncommited)
                {
                    if (confirmation.confirmationUserIds == null)
                    {
                        confirmation.status = (Int16)EnumPA_TransactionConfirmationStatus.Onay;
                        confirmation.description = "Otomatik Onay";
                        db.UpdatePA_TransactionConfirmation(new PA_TransactionConfirmation().B_EntityDataCopyForMaterial(confirmation));
                    }

                }
                if (notNullOrder == null)
                {
                    var users = db.GetVWSH_UserByIds(confirmations.OrderBy(x => x.ruleOrder).FirstOrDefault().confirmationUserIds.Split(',').Select(a => Guid.Parse(a)).ToArray());
                    var getTrans = db.GetPA_TransactionById(this.id);
                    if (getTrans != null)
                    {
                        if (this.direction != 3 || this.direction != 2)
                        {
                            this.createdby = getTrans.createdby;
                            var createdUser = db.GetVWSH_UserById(this.createdby.Value);
                            foreach (var user in users)
                            {
                                var text = "<h3>Sayın " + user.FullName + ",</h3>";
                                text += "<p>" + createdUser.FullName + " kişisi masraf talebinde bulunmuştur.</p>";
                                if (!string.IsNullOrEmpty(this.description))
                                {
                                    text += "<p>Açıklaması : " + this.description + "</p>";
                                }
                                text += "<div><a href='" + getTenantUrl + "/PA/VWPA_Transaction/IndexRequest" + "'>Detaya gitmek için tıklayınız.</a> </div>";
                                text += "<p>Bilgilerinize.</p>";
                                new Email().Template("Template1", "bos.png", TenantConfig.Tenant.TenantName + " | Masraf Onayı ", text).Send((Int16)EmailSendTypes.MasrafOnay, user.email, "Masraf Onayı", true);
                                notification.NotificationSend(user.id, "Onayınızı bekleyen masraf talebi var", createdUser.FullName + " kişisi masraf talebinde bulunmuştur");
                            }
                        }
                    }
                }
                else
                {
                    var findNotCommited = confirmations.Where(x => x.status == null && x.confirmationUserIds != null).ToList();
                    foreach (var confirmation in findUncommited)
                    {
                        if (confirmation.confirmationUserIds != null && confirmation.ruleOrder == notNullOrder.ruleOrder + 1)
                        {
                            var users = db.GetVWSH_UserByIds(confirmation.confirmationUserIds.Split(',').Select(a => Guid.Parse(a)).ToArray());
                            var getTrans = db.GetPA_TransactionById(this.id);
                            if (getTrans != null)
                            {
                                if (this.direction != 3 || this.direction != 2)
                                {
                                    this.createdby = getTrans.createdby;
                                    var createdUser = db.GetVWSH_UserById(this.createdby.Value);
                                    foreach (var user in users)
                                    {
                                        var text = "<h3>Sayın " + user.FullName + ",</h3>";
                                        text += "<p>" + createdUser.FullName + " kişisi masraf talebinde bulunmuştur.</p>";
                                        if (!string.IsNullOrEmpty(this.description))
                                        {
                                            text += "<p>Açıklaması : " + this.description + "</p>";
                                        }
                                        text += "<div><a href='" + getTenantUrl + "/PA/VWPA_Transaction/IndexRequest" + "'>Detaya gitmek için tıklayınız.</a> </div>";
                                        text += "<p>Bilgilerinize.</p>";
                                        new Email().Template("Template1", "bos.png", TenantConfig.Tenant.TenantName + " | Masraf Onayı ", text).Send((Int16)EmailSendTypes.MasrafOnay, user.email, "Masraf Onayı", true);
                                        notification.NotificationSend(user.id, "Onayınızı bekleyen masraf talebi var", createdUser.FullName + " kişisi masraf talebinde bulunmuştur");
                                    }
                                }
                            }

                        }
                    }






                }
            }
           


                //if (this.direction==0||this.direction==-1||this.direction==1)//red ve yeniden talep değilse 
                //{
                //    confirmations = confirmations.Where(x => x.status == null).ToArray();

                //    foreach (var confirmation in confirmations)
                //    {
                //        if (string.IsNullOrEmpty(confirmation.confirmationUserIds))
                //        {
                //            confirmation.status = (Int16)EnumPA_TransactionConfirmationStatus.Onay;
                //            confirmation.description = "Otomatik Onay";
                //            if (confirmations.Count() == 1)
                //            {
                //                if (confirmation.transactionId.HasValue)
                //                {
                //                    var transaction = db.GetPA_TransactionById(confirmation.transactionId.Value);
                //                    if (transaction != null)
                //                    {
                //                        transaction.direction = -1;
                //                        db.UpdatePA_Transaction(transaction);
                //                    }
                //                }
                //            }
                //            db.UpdatePA_TransactionConfirmation(new PA_TransactionConfirmation().B_EntityDataCopyForMaterial(confirmation));
                //            UpdateDataControl(confirmations, "", userId);
                //        }
                //        else
                //        {
                //            if (notNullOrder!=null)
                //            {
                //                if (notNullOrder.confirmationUserIds!=null)
                //                {
                //                    if (notNullOrder.confirmationUserIds.Split(',').Where(x => x.Contains(confirmation.confirmationUserIds)).Count() > 0)
                //                    {
                //                        confirmation.status = (Int16)EnumPA_TransactionConfirmationStatus.Onay;
                //                        confirmation.description = "Otomatik Onay";
                //                        db.UpdatePA_TransactionConfirmation(new PA_TransactionConfirmation().B_EntityDataCopyForMaterial(confirmation));

                //                    }
                //                    else if (!mailControl)
                //                    {
                //                        mailControl = true;
                //                        var users = db.GetVWSH_UserByIds(confirmation.confirmationUserIds.Split(',').Select(a => Guid.Parse(a)).ToArray());
                //                        var getTrans = db.GetPA_TransactionById(this.id);
                //                        if (getTrans != null)
                //                        {
                //                            if (this.direction != 3 || this.direction != 2)
                //                            {
                //                                this.createdby = getTrans.createdby;
                //                                var createdUser = db.GetVWSH_UserById(this.createdby.Value);
                //                                foreach (var user in users)
                //                                {
                //                                    var text = "<h3>Sayın " + user.FullName + ",</h3>";
                //                                    text += "<p>" + createdUser.FullName + " kişisi masraf talebinde bulunmuştur.</p>";
                //                                    if (!string.IsNullOrEmpty(this.description))
                //                                    {
                //                                        text += "<p>Açıklaması : " + this.description + "</p>";
                //                                    }
                //                                    text += "<div><a href='" + getTenantUrl + "/PA/VWPA_Transaction/IndexRequest" + "'>Detaya gitmek için tıklayınız.</a> </div>";
                //                                    text += "<p>Bilgilerinize.</p>";
                //                                    new Email().Template("Template1", "bos.png", TenantConfig.Tenant.TenantName + " | Masraf Onayı ", text).Send((Int16)EmailSendTypes.MasrafOnay, user.email, "Masraf Onayı", true);
                //                                    notification.NotificationSend(user.id, "Onayınızı bekleyen masraf talebi var", createdUser.FullName + " kişisi masraf talebinde bulunmuştur");
                //                                }
                //                            }

                //                        }
                //                    }

                //                }
                //                else if (!mailControl)
                //                {
                //                    mailControl = true;
                //                    var users = db.GetVWSH_UserByIds(confirmation.confirmationUserIds.Split(',').Select(a => Guid.Parse(a)).ToArray());
                //                    var getTrans = db.GetPA_TransactionById(this.id);
                //                    if (getTrans != null)
                //                    {
                //                        if (this.direction == 0 || this.direction == -1 || this.direction == 1)
                //                        {
                //                            this.createdby = getTrans.createdby;
                //                            var createdUser = db.GetVWSH_UserById(this.createdby.Value);
                //                            foreach (var user in users)
                //                            {
                //                                var text = "<h3>Sayın " + user.FullName + ",</h3>";
                //                                text += "<p>" + createdUser.FullName + " kişisi masraf talebinde bulunmuştur.</p>";
                //                                if (!string.IsNullOrEmpty(this.description))
                //                                {
                //                                    text += "<p>Açıklaması : " + this.description + "</p>";
                //                                }
                //                                text += "<div><a href='" + getTenantUrl + "/PA/VWPA_Transaction/IndexRequest" + "'>Detaya gitmek için tıklayınız.</a> </div>";
                //                                text += "<p>Bilgilerinize.</p>";
                //                                new Email().Template("Template1", "bos.png", TenantConfig.Tenant.TenantName + " | Masraf Onayı ", text).Send((Int16)EmailSendTypes.MasrafOnay, user.email, "Masraf Onayı", true);
                //                                notification.NotificationSend(user.id, "Onayınızı bekleyen masraf talebi var", createdUser.FullName + " kişisi masraf talebinde bulunmuştur");
                //                            }
                //                        }

                //                    }
                //                }
                //            }
                //            else
                //            {
                //                 if (!mailControl)
                //                {
                //                    mailControl = true;
                //                    var users = db.GetVWSH_UserByIds(confirmation.confirmationUserIds.Split(',').Select(a => Guid.Parse(a)).ToArray());
                //                    var getTrans = db.GetPA_TransactionById(this.id);
                //                    if (getTrans != null)
                //                    {
                //                        if (this.direction == 0 || this.direction == -1 || this.direction == 1)
                //                        {
                //                            this.createdby = getTrans.createdby;
                //                            var createdUser = db.GetVWSH_UserById(this.createdby.Value);
                //                            foreach (var user in users)
                //                            {
                //                                var text = "<h3>Sayın " + user.FullName + ",</h3>";
                //                                text += "<p>" + createdUser.FullName + " kişisi masraf talebinde bulunmuştur.</p>";
                //                                if (!string.IsNullOrEmpty(this.description))
                //                                {
                //                                    text += "<p>Açıklaması : " + this.description + "</p>";
                //                                }
                //                                text += "<div><a href='" + getTenantUrl + "/PA/VWPA_Transaction/IndexRequest" + "'>Detaya gitmek için tıklayınız.</a> </div>";
                //                                text += "<p>Bilgilerinize.</p>";
                //                                new Email().Template("Template1", "bos.png", TenantConfig.Tenant.TenantName + " | Masraf Onayı ", text).Send((Int16)EmailSendTypes.MasrafOnay, user.email, "Masraf Onayı", true);
                //                                notification.NotificationSend(user.id, "Onayınızı bekleyen masraf talebi var", createdUser.FullName + " kişisi masraf talebinde bulunmuştur");
                //                            }
                //                        }

                //                    }
                //                }
                //            }
                //        }
                //    }

                //}

            




        }
        public static SimpleQuery MyTransactionQuery(SimpleQuery query, PageSecurity userStatus, int? direction)
        {
            BEXP filter = null;
            if (!direction.HasValue)
            {
                filter = new BEXP
                {
                    Operand1 = new BEXP
                    {
                        Operand1 = (COL)"type",
                        Operator = BinaryOperator.Equal,
                        Operand2 = (VAL)(int)EnumPA_TransactionType.Masraf
                    },
                    Operator = BinaryOperator.And,
                    Operand2 = new BEXP
                    {
                        Operand1 = (COL)"createdby",
                        Operator = BinaryOperator.Equal,
                        Operand2 = (VAL)userStatus.user.id
                    }
                };
            }
            else
            {
                filter &= new BEXP
                {
                    Operand1 = new BEXP
                    {
                        Operand1 = new BEXP
                        {
                            Operand1 = (COL)"direction",
                            Operator = BinaryOperator.Equal,
                            Operand2 = (VAL)(int)direction
                        },
                        Operand2 = new BEXP
                        {
                            Operand1 = (COL)"type",
                            Operator = BinaryOperator.Equal,
                            Operand2 = (VAL)(int)EnumPA_TransactionType.Masraf
                        },
                        Operator = BinaryOperator.And
                    },
                    Operand2 = new BEXP
                    {
                        Operand1 = new BEXP
                        {
                            Operand1 = (COL)"createdby",
                            Operator = BinaryOperator.Equal,
                            Operand2 = (VAL)userStatus.user.id
                        },
                        Operand2 = new BEXP
                        {
                            Operand1 = (COL)"createdby",
                            Operator = BinaryOperator.Equal,
                            Operand2 = (VAL)userStatus.user.id
                        },
                        Operator = BinaryOperator.Or
                    },
                    Operator = BinaryOperator.And
                };
            }
            query.Filter &= filter;
            return query;
        }
        public static SimpleQuery MyTransactionApprovedQuery(SimpleQuery query, PageSecurity userStatus, int? direction)
        {
            BEXP filter = null;
            string userId = userStatus.user.id.ToString();
            if (!direction.HasValue)
            {
                filter = new BEXP
                {
                    Operand1 = new BEXP
                    {
                        Operand1 = (COL)"type",
                        Operator = BinaryOperator.Equal,
                        Operand2 = (VAL)(int)EnumPA_TransactionType.Masraf
                    },
                    Operand2 = new BEXP
                    {
                        Operand1 = new BEXP
                        {
                            Operand1 = (COL)"hasReject",
                            Operator = BinaryOperator.NotEqual,
                            Operand2 = (VAL)1
                        },
                        Operand2 = new BEXP
                        {
                            Operand1 = new BEXP
                            {
                                Operand1 = (COL)"confirmationUserIds",
                                Operator = BinaryOperator.Like,
                                Operand2 = (VAL)userId
                            },
                            Operand2 = new BEXP
                            {
                                Operand1 = (COL)"rejectedUserIds",
                                Operator = BinaryOperator.IsNull,
                                Operand2 = (VAL)null
                            },
                            Operator = BinaryOperator.And
                        },
                        Operator = BinaryOperator.And
                    },
                    Operator = BinaryOperator.And
                };
            }
            else
            {
                if (direction == (int)EnumPA_TransactionDirection.Talep || direction == 3)
                {
                    filter &= new BEXP
                    {
                        Operand1 = new BEXP
                        {
                            Operand1 = new BEXP
                            {
                                Operand1 = (COL)"direction",
                                Operator = BinaryOperator.Equal,
                                Operand2 = (VAL)(int)direction
                            },
                            Operand2 = new BEXP
                            {
                                Operand1 = (COL)"type",
                                Operator = BinaryOperator.Equal,
                                Operand2 = (VAL)(int)EnumPA_TransactionType.Masraf
                            },
                            Operator = BinaryOperator.And
                        },
                        Operand2 = new BEXP
                        {
                            Operand1 = new BEXP
                            {
                                Operand1 = (COL)"confirmationUserIds",
                                Operator = BinaryOperator.Like,
                                Operand2 = (VAL)userId
                            },
                            Operand2 = new BEXP
                            {
                                Operand1 = (COL)"confirmationStatus",
                                Operator = BinaryOperator.Equal,
                                Operand2 = (VAL)1
                            },
                            Operator = BinaryOperator.And
                        },
                        Operator = BinaryOperator.And
                    };
                }
                else if (direction == -1)
                {
                    filter &= new BEXP
                    {
                        Operand1 = new BEXP
                        {
                            Operand1 = new BEXP
                            {
                                Operand1 = (COL)"direction",
                                Operator = BinaryOperator.Equal,
                                Operand2 = (VAL)(int)direction
                            },
                            Operand2 = new BEXP
                            {
                                Operand1 = (COL)"type",
                                Operator = BinaryOperator.Equal,
                                Operand2 = (VAL)(int)EnumPA_TransactionType.Masraf
                            },
                            Operator = BinaryOperator.And
                        },
                        Operand2 = new BEXP
                        {
                            Operand1 = new BEXP
                            {
                                Operand1 = (COL)"confirmUserIds",
                                Operator = BinaryOperator.Like,
                                Operand2 = (VAL)userId
                            },
                            Operand2 = new BEXP
                            {
                                Operand1 = (COL)"confirmationStatus",
                                Operator = BinaryOperator.Equal,
                                Operand2 = (VAL)1
                            },
                            Operator = BinaryOperator.And
                        },
                        Operator = BinaryOperator.Or
                    };
                }
                else if (direction == 2)
                {
                    filter &= new BEXP
                    {
                        Operand1 = new BEXP
                        {
                            Operand1 = new BEXP
                            {
                                Operand1 = (COL)"direction",
                                Operator = BinaryOperator.Equal,
                                Operand2 = (VAL)(int)direction
                            },
                            Operand2 = new BEXP
                            {
                                Operand1 = (COL)"type",
                                Operator = BinaryOperator.Equal,
                                Operand2 = (VAL)(int)EnumPA_TransactionType.Masraf
                            },
                            Operator = BinaryOperator.And
                        },
                        Operand2 = new BEXP
                        {
                            Operand1 = new BEXP
                            {
                                Operand1 = (COL)"rejectedUserIds",
                                Operator = BinaryOperator.Like,
                                Operand2 = (VAL)userId
                            },
                            Operand2 = new BEXP
                            {
                                Operand1 = (COL)"confirmationStatus",
                                Operator = BinaryOperator.Equal,
                                Operand2 = (VAL)0
                            },
                            Operator = BinaryOperator.And
                        },
                        Operator = BinaryOperator.And
                    };
                }
            }
            query.Filter &= filter;
            return query;
        }
    }
}
