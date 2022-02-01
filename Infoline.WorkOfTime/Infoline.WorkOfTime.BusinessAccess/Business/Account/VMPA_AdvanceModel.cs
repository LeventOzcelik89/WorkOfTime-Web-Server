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
    public class VMPA_AdvanceModel : VWPA_Advance
    {
        private WorkOfTimeDatabase db { get; set; }
        private DbTransaction trans { get; set; }
        public VWPA_Ledger Ledger { get; set; }
        public PA_Ledger[] Ledgers { get; set; }
        public VWPA_Account Account { get; set; }
        public SYS_Files[] Files { get; set; }
        public bool? IsCopy { get; set; }
        public Guid newId { get; set; }
        public VWPA_AdvanceHistory[] AdvanceHistory { get; set; } = new VWPA_AdvanceHistory[0];
        public DateTime? payBackDate { get; set; }
        public string statusDescription { get; set; }
        public class VWPA_AdvanceHistory
        {
            public string description { get; set; }
            public DateTime date { get; set; }
            public short? status { get; set; }
            public string userId_Title { get; set; }
            public string ruleTitle { get; set; }
            public short? ruleOrder { get; set; }
            public short? ruleType { get; set; }
            public Guid? userId { get; set; }
        }
        public VMPA_AdvanceModel Load()
        {
            this.db = this.db ?? new WorkOfTimeDatabase();
            var advance = db.GetVWPA_AdvanceById(this.id);
            if (advance != null)
            {
                this.B_EntityDataCopyForMaterial(advance, true);
                var advanceConfirmation = db.GetVWPA_AdvanceConfirmationByAdvanceId(this.id);
                if (advanceConfirmation.Count() > 0)
                {
                    this.statusDescription = advanceConfirmation.Where(x => x.status == 3 && x.description != null).Select(a => a.description).FirstOrDefault();
                    AdvanceHistory = advanceConfirmation.Where(a => a.confirmationUserIds != null).Select(x => new VWPA_AdvanceHistory
                    {
                        date = x.created.Value,
                        description = x.description,
                        ruleTitle = x.ruleTitle,
                        userId_Title = x.confirmationUserIds_Titles,
                        status = x.status,
                        ruleType = x.ruleType,
                        ruleOrder = x.ruleOrder,
                        userId = x.userId
                    }).ToArray();
                }
                //if (this.IsCopy == true)
                //{
                //	Files = db.GetSYS_FilesByDataIdAll(this.id);
                //	this.id = Guid.NewGuid();
                //	this.newId = this.id;
                //	db.BulkInsertSYS_Files(Files.Select(x => new SYS_Files { DataId = this.id, FilePath = x.FilePath, id = Guid.NewGuid(), FileGroup = x.FileGroup, DataTable = x.DataTable, FileExtension = x.FileExtension }));
                //}
            }
            else
            {
                this.status = (int)EnumPA_AdvanceStatus.Odenecek;
            }
            return this;
        }
        public VMPA_AdvanceModel LoadMobile()
        {
            this.db = this.db ?? new WorkOfTimeDatabase();
            var advance = db.GetVWPA_AdvanceById(this.id);
            if (advance != null)
            {
                this.B_EntityDataCopyForMaterial(advance, true);
                var advanceConfirmation = db.GetVWPA_AdvanceConfirmationByAdvanceId(this.id);
                if (advanceConfirmation.Count() > 0)
                {
                    this.statusDescription = advanceConfirmation.Where(x => x.status == 0 && x.description != null).Select(a => a.description).FirstOrDefault();
                    AdvanceHistory = advanceConfirmation.Where(a => a.confirmationUserIds != null).Select(x => new VWPA_AdvanceHistory
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
            else
            {
                this.status = (int)EnumPA_AdvanceStatus.Odenecek;
            }
            return this;
        }
        public ResultStatus Save(Guid userId, HttpRequestBase request = null, DbTransaction trans = null)
        {
            db = db ?? new WorkOfTimeDatabase();
            var advance = db.GetVWPA_AdvanceById(this.id);
            var res = new ResultStatus { result = true };
            if (this.status == (int)EnumPA_AdvanceStatus.Odendi)
            {
                if (!this.Ledger.date.HasValue) { return new ResultStatus { result = false, message = "Ödeme yapılan tarihi seçmelisiniz. Lütfen kontrol ediniz!" }; }
                if (!this.Ledger.accountId.HasValue) { return new ResultStatus { result = false, message = "Ödenen hesabı seçmelisiniz. Lütfen kontrol ediniz!" }; }
            }
            if (this.IsCopy == true || advance == null)
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
                var getChanged = db.GetPA_AdvanceById(this.id);
                if (getChanged != null)
                {
                    this.createdby = getChanged.createdby;
                }
                res = Update(trans);
            }
            if (this.direction == 1 || this.direction == -1 || this.direction == 0)
            {
                var advanceConfirmations = db.GetVWPA_AdvanceConfirmationByAdvanceId(this.id);
                UpdateDataControl(advanceConfirmations, this.statusDescription);
            }
            return res;
        }
        private ResultStatus Insert(DbTransaction trans = null)
        {
            var transaction = trans ?? db.BeginTransaction();
            var dbresult = new ResultStatus { result = true };
            if (this.dataTable == "CRM_Presentation")
            {
                var currency = db.GetUT_CurrencyById(this.currencyId.Value);
                var color = this.direction == -1 ? "#1ab394" : "#f8ac59";
                var status = this.direction == -1 ? " (Onaylandı) " : " (Onay bekleniyor) ";
                dbresult &= db.InsertCRM_PresentationAction(new CRM_PresentationAction()
                {
                    created = DateTime.Now,
                    createdby = this.createdby,
                    type = (int)EnumCRM_PresentationActionType.AvansEkle,
                    description = "Potansiyel/Fırsata " + this.amount + currency.symbol + " avans eklendi." + status,
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
            this.tax = this.tax.HasValue ? this.tax.Value : 0;
            dbresult &= db.InsertPA_Advance(new PA_Advance().B_EntityDataCopyForMaterial(this), transaction);
            if (this.status == (int)EnumPA_AdvanceStatus.Odendi)
            {
                dbresult &= InsertLedger(transaction);
            }
            if (trans == null)
            {
                if (dbresult.result)
                {
                    dbresult &= InsertConfirmation(this.createdby.Value, transaction);
                    var getAllConfirmation = db.GetVWPA_AdvanceConfirmationByAdvanceId(this.id);
                    if (getAllConfirmation.Count() == 0)
                    {
                        var getExpens = db.GetPA_AdvanceById(this.id);
                        getExpens.direction = (short)EnumPA_AdvanceDirection.Cikis;
                        dbresult &= getExpens.B_ConvertType<VMPA_AdvanceModel>().Save(this.createdby.Value, null, transaction);
                    }
                    transaction.Commit();
                }
                else
                {
                    transaction.Rollback();
                }
            }
            var message = "Avans talep oluşturma işlemi ";
            return new ResultStatus
            {
                result = dbresult.result,
                message = dbresult.result ? message + "başarılı bir şekilde gerçekleştirildi." : dbresult.message
            };
        }
        private ResultStatus Update(DbTransaction trans = null)
        {
            var transaction = trans ?? db.BeginTransaction();
            var dbresult = new ResultStatus { result = true };
            if (this.direction == -1)
            {
                this.date = DateTime.Now.AddMonths(1);
            }
            this.tax = this.tax.HasValue ? this.tax.Value : 0;
            var confirmations = db.GetVWPA_AdvanceConfirmationByAdvanceId(this.id);
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
                        dbresult &= db.UpdatePA_AdvanceConfirmation(new PA_AdvanceConfirmation().B_EntityDataCopyForMaterial(confirmation), true, transaction);
                    }
                    dbresult &= db.UpdatePA_AdvanceConfirmation(new PA_AdvanceConfirmation().B_EntityDataCopyForMaterial(refreshConfirm), true, transaction);
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
                        confirm.status = (Int16)EnumPA_AdvanceConfirmationStatus.Onay;
                        confirm.description = this.statusDescription;
                    }
                    else if (this.direction == 3)
                    {
                        this.direction = 3;
                        confirm.status = (Int16)EnumPA_AdvanceConfirmationStatus.YenidenTalep;
                        confirm.description = this.statusDescription;
                    }
                    else if (this.direction == 0)
                    {
                    }
                    else
                    {
                        this.direction = 2;
                        confirm.status = (Int16)EnumPA_AdvanceConfirmationStatus.Red;
                        confirm.description = this.statusDescription;
                    }
                    dbresult &= db.UpdatePA_AdvanceConfirmation(new PA_AdvanceConfirmation().B_EntityDataCopyForMaterial(confirm), true, transaction);
                }
            }
            dbresult &= db.UpdatePA_Advance(new PA_Advance().B_EntityDataCopyForMaterial(this), false, transaction);
            if (this.status == (int)EnumPA_TransactionStatus.Odendi || this.status == (int)EnumPA_AdvanceStatus.CalisanOdedi)
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
            var message = "Avans düzenleme işlemi ";
            message = this.direction == 0 ? "Avans düzenleme işlemi " : this.direction == -1 ? "Avans onaylama işlemi " : this.direction == 3 ? "Avans düzeltme talebi " : " Avans reddetme işlemi ";
            var pA_Advance = db.GetPA_AdvanceById(this.id);
            if (pA_Advance != null)
            {
                var currency = db.GetUT_CurrencyById(this.currencyId.Value);
                if (pA_Advance.dataTable == "CRM_Presentation")
                {
                    var color = this.direction == 0 ? "#3343a4" : this.direction == -1 ? "#1ab394" : "#ed5565";
                    dbresult &= db.InsertCRM_PresentationAction(new CRM_PresentationAction()
                    {
                        created = DateTime.Now,
                        createdby = this.createdby,
                        type = (int)EnumCRM_PresentationActionType.AvansDüzenlendi,
                        description = "Potansiyel/Fırsat " + message + " gerçekleştirildi. " + "Avans " + this.amount + " " + currency.symbol + ".",
                        presentationId = pA_Advance.dataId ?? new Guid(),
                        color = color
                    }, trans);
                }
            }
            if (dbresult.result && this.direction == (int)EnumPA_AdvanceDirection.Cikis)
            {
                string[] roles = new string[3] { SHRoles.OnMuhasebe, SHRoles.MuhasebeAlis, SHRoles.MuhasebeAlis };
                var userIds = db.GetSH_UserRoleByRoleIds(roles).Where(a => a.userid.HasValue).Select(x => x.userid.Value).ToArray();
                if (userIds.Count() > 0)
                {
                    var users = db.GetVWSH_UserByIds(userIds);
                    var confirming = db.GetVWSH_UserById(this.changedby.Value);
                    foreach (var user in users)
                    {
                        var text = "<h3>Sayın " + user.FullName + ",</h3>";
                        text += "<p>" + this.createdby_Title + " kişisinin Avans Talebi " + confirming.FullName + " tarafından onaylanmıştır.</p>";
                        if (!string.IsNullOrEmpty(this.description))
                        {
                            text += "<p>Açıklaması : " + this.description + "</p>";
                        }
                        text += "<p>Ödemeniz beklenmektedir.</p>";
                        text += "<p>Bilgilerinize.</p>";
                        new Email().Template("Template1", "bos.png", TenantConfig.Tenant.TenantName + " | Avans Onayı ", text).Send((Int16)EmailSendTypes.AvansOnay, user.email, "Avans Onayı", true);
                    }
                }
            }
            if (this.direction == 2)
            {
                var url = TenantConfig.Tenant.GetWebUrl();
                var user = db.GetVWSH_UserById(this.createdby.Value);
                var text = "<h3>Sayın " + user.FullName + ",</h3>";
                text += "<p>Avans talebiniz reddedilmiştir.</p>";
                if (!string.IsNullOrEmpty(this.statusDescription))
                {
                    text += "<p>Açıklaması : " + this.statusDescription + "</p>";
                }
                text += "<p><a href='" + url + "/PA/VWPA_Advance/Detail?id=" + this.id + "'>Detaya gitmek için tıklayınız.</a> </p>";
                text += "<p>Bilgilerinize.</p>";
                new Email().Template("Template1", "bos.png", TenantConfig.Tenant.TenantName + " | Avans Talebi Reddi ", text).Send((Int16)EmailSendTypes.AvansOnay, user.email, "Avans Talebi Reddi", true);
            }
            if (this.direction == 3)
            {
                var url = TenantConfig.Tenant.GetWebUrl();
                var user = db.GetVWSH_UserById(this.createdby.Value);
                var text = "<h3>Sayın " + user.FullName + ",</h3>";
                text += "<p>Avans talebinize yeniden düzeltme istenmiştir.</p>";
                if (!string.IsNullOrEmpty(this.statusDescription))
                {
                    text += "<p>Açıklaması : " + this.statusDescription + "</p>";
                }
                text += "<p><a href='" + url + "/PA/VWPA_Advance/Detail?id=" + this.id + "'>Detaya gitmek için tıklayınız.</a> </p>";
                text += "<p>Bilgilerinize.</p>";
                new Email().Template("Template1", "bos.png", TenantConfig.Tenant.TenantName + " | Avans Talebi Düzeltme ", text).Send((Int16)EmailSendTypes.AvansOnay, user.email, "Avans Talebi Düzeltme", true);
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
            if (this.status == (int)EnumPA_AdvanceStatus.Odendi)
            {
                var account = db.GetVWPA_AccountById(this.Ledger.accountId.Value);
                var newLedger = new VMPA_LedgerModel
                {
                    advanceId = this.id,
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
                newLedger.accountId = this.accountId;
                newLedger.accountRealtedId = account.id;
                newLedger.direction = (short)(this.direction.Value * -1);
                dbresult &= newLedger.Save(this.createdby.Value, null, transaction);
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
            var advance = db.GetPA_AdvanceById(this.id);
            var ledger = db.GetPA_LedgerByAdvanceId(this.id);
            if (advance == null)
            {
                return new ResultStatus
                {
                    result = false,
                    message = "Avans Kaydı Bulunamadı."
                };
            }
            var dbresult = db.BulkDeletePA_Ledger(ledger, _trans);
            dbresult &= db.DeletePA_Advance(advance, _trans);
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
                message = dbresult.result ? "Avans silme işlemi başarılı bir şekilde gerçekleştirildi." : "Avans silme işlemi başarısız oldu."
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
            }
            query.Filter &= filter;
            return query;
        }
        public SummaryHeadersAdvance GetAdvanceSummary(Guid userId)
        {
            this.Load();
            return db.GetVWPA_AdvanceByUserIdCounts(userId);
        }
        public SummaryHeadersAdvance GetListAdvanceSummary(Guid userId)
        {
            this.Load();
            return db.GetVWPA_AdvancesByUserId(userId);
        }
        public SummaryHeadersAdvance GetListMyApprovedAdvanceSummary(Guid userId)
        {
            this.Load();
            return db.GetVWPA_AdvancesApprovedByUserId(userId);
        }
        public SummaryHeadersAdvance GetRequestAdvanceSummary(Guid userId)
        {
            this.Load();
            return db.GetVWPA_AdvancesRequestByUserId(userId);
        }
        public ResultStatus InsertConfirmation(Guid userId, DbTransaction trans = null)
        {
            var dbresult = new ResultStatus { result = true };
            var _trans = trans ?? db.BeginTransaction();
            this.db = this.db ?? new WorkOfTimeDatabase();
            var advanceCofirmations = new List<PA_AdvanceConfirmation>();
            //Kullanıcıya ait avans kurallarını çektim.
            var rulesUser = db.GetVWUT_RulesUserByUserIdAndType(userId, (Int16)EnumUT_RulesType.Advance);
            var rulesUserStages = new VWUT_RulesUserStage[0];
            if (rulesUser == null)
            {
                var defaultRule = db.GetUT_RulesByTypeIsDefault((Int16)EnumUT_RulesType.Advance);
                rulesUserStages = db.GetVWUT_RulesUserStageByRulesId(defaultRule.id);
            }
            else
            {
                rulesUserStages = db.GetVWUT_RulesUserStageByRulesId(rulesUser.rulesId.Value);
            }
            var shuser = db.GetVWSH_UserById(userId);
            if (rulesUserStages.Count() > 0)
            {
                var getManagers = db.GetINV_CompanyPersonDepartmentsByIdUserAndTypeCurrentWork(userId, (int)EnumINV_CompanyDepartmentsType.Organization);
                if (getManagers.Count() == 0)
                {
                    return new ResultStatus { message = "Kullanıcının ana departmanı yoktur" };
                }
                var getLimitation = rulesUserStages.Where(x => x.limit.HasValue);
                foreach (var rulesStage in rulesUserStages.OrderBy(a => a.ruleType == (int)EnumUT_RulesUserStage.SonOnaylayici ? 10000 : a.order))
                {

                    //yönetici departmanda var mı ? 
                    var isInDepartman = shuser.Manager1 == rulesStage.userId ? true
                              : shuser.Manager2 == rulesStage.userId ? true
                              : shuser.Manager3 == rulesStage.userId ? true
                              : shuser.Manager4 == rulesStage.userId ? true
                              : shuser.Manager5 == rulesStage.userId ? true
                              : shuser.Manager6 == rulesStage.userId ? true
                              : false;
                    Guid? assingUser = null;
                    switch ((EnumUT_RulesUserStage)rulesStage.type)
                    {
                        case EnumUT_RulesUserStage.Manager1:
                        case EnumUT_RulesUserStage.Manager2:
                        case EnumUT_RulesUserStage.Manager3:
                        case EnumUT_RulesUserStage.Manager4:
                        case EnumUT_RulesUserStage.Manager5:
                        case EnumUT_RulesUserStage.Manager6:
                            assingUser = (
                     rulesStage.type == (Int16)EnumUT_RulesUserStage.Manager1 ? shuser?.Manager1 :
                     rulesStage.type == (Int16)EnumUT_RulesUserStage.Manager2 ? shuser?.Manager2 :
                     rulesStage.type == (Int16)EnumUT_RulesUserStage.Manager3 ? shuser?.Manager3 :
                     rulesStage.type == (Int16)EnumUT_RulesUserStage.Manager4 ? shuser?.Manager4 :
                     rulesStage.type == (Int16)EnumUT_RulesUserStage.Manager5 ? shuser?.Manager5 :
                     rulesStage.type == (Int16)EnumUT_RulesUserStage.Manager6 ? shuser?.Manager6 :
                      null);
                            //  eğer yöneticiler son onaylayıcı veya rolebaglu veya secim ise devam 
                            var isUserExistBefore = rulesUserStages.Where(x => (x.type != rulesStage.type)
                             && x.userId.HasValue
                             && x.userId == assingUser);
                            if (isUserExistBefore.Count() > 0)
                            {
                                continue;
                            }
                            break;
                        case EnumUT_RulesUserStage.RoleBagliSecim:
                            if (!rulesStage.roleId.HasValue)
                            {
                                return new ResultStatus { message = "Kural aşaması için belirtilen rol yoktur!" };
                            }
                            var getRole = db.GetVWSH_RoleById(rulesStage.roleId.Value);
                            if (getRole == null)
                            {
                                return new ResultStatus { message = "Kural aşaması için belirtilen rol yoktur!" };
                            }
                            var getRoleUsers = db.GetVWSH_UserByRoleId(getRole.id.ToString());
                            if (getRoleUsers == null)
                            {
                                return new ResultStatus { message = "Kural aşaması için onaylacak kullanıcı yoktur!." };
                            }
                            var apprvList = new List<VWSH_User>();
                            foreach (var user in getRoleUsers)
                            {
                                //onaylacak kişi departmanda var mı ? 
                                var hasRoleInDepartman = shuser.Manager1 == user.id ? true
                                    : shuser.Manager2 == user.id ? true
                                    : shuser.Manager3 == user.id ? true
                                    : shuser.Manager4 == user.id ? true
                                    : shuser.Manager5 == user.id ? true
                                    : shuser.Manager6 == user.id ? true
                                    : false;
                                if (hasRoleInDepartman)
                                {
                                    apprvList.Add(user);//eğer varsa listeye ekle
                                }
                            }
                            var isApprv = apprvList.Where(x => x.id != shuser.id);//onaylayacak kişileri isteği onaylayacak kişi olmayanları getir.
                            if (apprvList.Count() > 0 && isApprv.Count() > 0)//eğer onaylayacak kişi varsa 
                            {
                                assingUser = isApprv.FirstOrDefault().id;//ilkini getir
                            }
                            else
                            {
                                var roleUser = getRoleUsers.Where(x => x.id != shuser.id);//eğer onaylayacak kimse yoksa kendi olmayanı getir
                                if (roleUser.Count() > 0)
                                {
                                    assingUser = roleUser.FirstOrDefault().id;//ilkini al 
                                }
                            }
                            isUserExistBefore = rulesUserStages.Where(x => (x.type != rulesStage.type)
                             && x.userId.HasValue
                             && x.userId == assingUser);
                            if (isUserExistBefore.Count() > 0)
                            {
                                continue;
                            }
                            break;
                        case EnumUT_RulesUserStage.SecimeBagliKullanici:
                            assingUser = rulesStage.userId;
                            isUserExistBefore = rulesUserStages.Where(x => (x.type != rulesStage.type)
                              && x.userId.HasValue
                              && x.userId == assingUser);
                            if (isUserExistBefore.Count() > 0)
                            {
                                continue;
                            }
                            break;
                        case EnumUT_RulesUserStage.SonOnaylayici:
                            //son onaylacak kişi, istek yapanın son kullanıcılarında yoksa bu adımı geç
                            if (!isInDepartman)
                            {
                                continue;
                            }
                            //son onaylayacak kullanıcı kişinin son adımlarında varsa son onaylayacak kullanıcıyı ekle
                            assingUser = rulesStage.userId;
                            break;
                        default:
                            break;
                    }
                    if (assingUser == null)
                    {
                        continue;//eğer onaylayacak kimsesi yoksa bunu veri tabanına ekleme;
                    }
                    if (assingUser == shuser.id)
                    {
                        continue;
                    }
                    var isUsedBefore = advanceCofirmations.Where(x => x.userId == assingUser).OrderByDescending(x => x.ruleOrder);
                    if (isUsedBefore.Count() > 0)
                    {
                        advanceCofirmations.Remove(isUsedBefore.FirstOrDefault());
                    }
                    advanceCofirmations.Add(new PA_AdvanceConfirmation
                    {
                        created = this.created,
                        createdby = userId,
                        advanceId = this.id,
                        ruleType = rulesStage.type,
                        ruleOrder = rulesStage.order,
                        ruleUserId = rulesStage.userId,
                        ruleRoleId = rulesStage.roleId,
                        ruleTitle = rulesStage.title,
                        userId = assingUser
                    });
                }
            }
            else
            {
                return new ResultStatus
                {
                    result = false,
                    message = "Hiç bir avans kuralı bulunamadı."
                };
            }
            dbresult &= db.BulkInsertPA_AdvanceConfirmation(advanceCofirmations, _trans);
            return new ResultStatus
            {
                result = dbresult.result,
                message = dbresult.result ? "Kayıt başarılı bir şekilde gerçekleştirildi." : dbresult.message
            };
        }
        public void UpdateDataControl(VWPA_AdvanceConfirmation[] confirmations, string statusDescription)
        {
            if (this.direction == 0 || this.direction == -1 || this.direction == 1)
            {
                db = db ?? new WorkOfTimeDatabase();
                var getTenantUrl = TenantConfig.Tenant.GetWebUrl();
                var notification = new Notification();
                var notNullOrder = confirmations.Where(x => x.status != null).OrderByDescending(a => a.ruleOrder).FirstOrDefault();
                var findUncommited = confirmations.Where(x => x.status == null && x.confirmationUserIds == null).ToList();
                foreach (var confirmation in findUncommited)
                {
                    if (confirmation.confirmationUserIds == null)
                    {
                        confirmation.status = (Int16)EnumPA_TransactionConfirmationStatus.Onay;
                        confirmation.description = "Otomatik Onay";
                        db.UpdatePA_AdvanceConfirmation(new PA_AdvanceConfirmation().B_EntityDataCopyForMaterial(confirmation));
                    }
                }
                if (notNullOrder == null)
                {
                    var isUserExist = confirmations.OrderBy(x => x.ruleOrder).FirstOrDefault(x => x.confirmationUserIds != null);
                    if (isUserExist != null)
                    {
                        var users = db.GetVWSH_UserByIds(isUserExist.confirmationUserIds.Split(',').Select(a => Guid.Parse(a)).ToArray());
                        var getAdvance = db.GetPA_AdvanceById(this.id);
                        if (getAdvance != null)
                        {
                            this.createdby = getAdvance.createdby;
                            var createdUser = db.GetVWSH_UserById(this.createdby.Value);
                            foreach (var user in users)
                            {
                                var text = "<h3>Sayın " + user.FullName + ",</h3>";
                                text += "<p>" + createdUser.FullName + " kişisi avans talebinde bulunmuştur.</p>";
                                if (!string.IsNullOrEmpty(this.description))
                                {
                                    text += "<p>Açıklaması : " + this.description + "</p>";
                                }
                                text += "<p><a href='" + getTenantUrl + "/PA/VWPA_Advance/Detail?id=" + this.id + "'>Detaya gitmek için tıklayınız.</a> </p>";
                                text += "<p>Bilgilerinize.</p>";
                                new Email().Template("Template1", "bos.png", TenantConfig.Tenant.TenantName + " | Avans Talebi ", text).Send((Int16)EmailSendTypes.AvansOnay, user.email, "Avans Talebi", true);
                            }
                        }
                    }
                }
                else
                {
                    var findNotCommited = confirmations.Where(x => x.status == null && x.confirmationUserIds != null).OrderBy(x => x.ruleOrder).ToList();
                    var findWillCommit = findNotCommited.Where(x => x.confirmationUserIds != null && x.ruleOrder == notNullOrder.ruleOrder + 1).FirstOrDefault();
                    if (findWillCommit != null)
                    {
                        var users = db.GetVWSH_UserByIds(findWillCommit.confirmationUserIds.Split(',').Select(a => Guid.Parse(a)).ToArray());
                        var getAdvance = db.GetPA_AdvanceById(this.id);
                        if (getAdvance != null)
                        {
                            this.createdby = getAdvance.createdby;
                            var createdUser = db.GetVWSH_UserById(this.createdby.Value);
                            foreach (var user in users)
                            {
                                var text = "<h3>Sayın " + user.FullName + ",</h3>";
                                text += "<p>" + createdUser.FullName + " kişisi avans talebinde bulunmuştur.</p>";
                                if (!string.IsNullOrEmpty(this.description))
                                {
                                    text += "<p>Açıklaması : " + this.description + "</p>";
                                }
                                text += "<p><a href='" + getTenantUrl + "/PA/VWPA_Advance/Detail?id=" + this.id + "'>Detaya gitmek için tıklayınız.</a> </p>";
                                text += "<p>Bilgilerinize.</p>";
                                new Email().Template("Template1", "bos.png", TenantConfig.Tenant.TenantName + " | Avans Talebi ", text).Send((Int16)EmailSendTypes.AvansOnay, user.email, "Avans Talebi", true);
                            }
                        }
                    }
                }
            }
            //var control = false;
            //var mailControl = false;
            //var notNullOrder = confirmations.Where(x => x.status != null).OrderByDescending(a => a.ruleOrder).FirstOrDefault();
            //if (this.direction != 3)
            //{
            //    confirmations = confirmations.Where(x => x.status == null).ToArray();
            //}
            //this.db = this.db ?? new WorkOfTimeDatabase();
            //for (int i = 0; i < confirmations.Count(); i++)
            //{
            //    if (confirmations[i].confirmationUserIds == null && !control)
            //    {
            //        if (this.direction == 2)
            //        {
            //            confirmations[i].status = (Int16)EnumPA_AdvanceConfirmationStatus.Red;
            //            confirmations[i].description = statusDescription;
            //            if (confirmations[i].createdby.HasValue)
            //            {
            //                var url = TenantConfig.Tenant.GetWebUrl();
            //                var user = db.GetVWSH_UserById(confirmations[i].createdby.Value);
            //                var text = "<h3>Sayın " + user.FullName + ",</h3>";
            //                text += "<p>Avans talebiniz reddedilmiştir.</p>";
            //                if (!string.IsNullOrEmpty(this.statusDescription))
            //                {
            //                    text += "<p>Açıklaması : " + this.statusDescription + "</p>";
            //                }
            //                text += "<p><a href='" + url + "/PA/VWPA_Advance/Detail?id=" + confirmations[i].advanceId + "'>Detaya gitmek için tıklayınız.</a> </p>";
            //                text += "<p>Bilgilerinize.</p>";
            //                new Email().Template("Template1", "bos.png", TenantConfig.Tenant.TenantName + " | Avans Talebi Reddi ", text).Send((Int16)EmailSendTypes.AvansOnay, user.email, "Avans Talebi Reddi", true);
            //            }
            //        }
            //        else if (this.direction != 3)
            //        {
            //            confirmations[i].status = (Int16)EnumPA_AdvanceConfirmationStatus.Onay;
            //            confirmations[i].description = "Otomatik Onay";
            //            if (confirmations.Count() == 1)
            //            {
            //                if (confirmations[i].advanceId.HasValue)
            //                {
            //                    var advance = db.GetPA_AdvanceById(confirmations[i].advanceId.Value);
            //                    if (advance != null)
            //                    {
            //                        advance.direction = -1;
            //                        db.UpdatePA_Advance(advance);
            //                    }
            //                }
            //            }
            //        }
            //        else if (confirmations.Count(c => c.status == 3) > 0)
            //        {
            //            continue;
            //        }
            //        db.UpdatePA_AdvanceConfirmation(new PA_AdvanceConfirmation().B_EntityDataCopyForMaterial(confirmations[i]));
            //        UpdateDataControl(confirmations, "");
            //    }
            //    else
            //    {
            //        if (notNullOrder != null)
            //        {
            //            if (notNullOrder.confirmationUserIds == confirmations[i].confirmationUserIds && this.direction != 3)
            //            {
            //                confirmations[i].status = (Int16)EnumPA_AdvanceConfirmationStatus.Onay;
            //                confirmations[i].description = "Otomatik Onay";
            //                db.UpdatePA_AdvanceConfirmation(new PA_AdvanceConfirmation().B_EntityDataCopyForMaterial(confirmations[i]));
            //                if (confirmations[i].advanceId.HasValue)
            //                {
            //                    var advance = db.GetPA_AdvanceById(confirmations[i].advanceId.Value);
            //                    if (advance != null)
            //                    {
            //                        advance.direction = -1;
            //                        db.UpdatePA_Advance(advance);
            //                    }
            //                }
            //            }
            //        }
            //        else
            //        {
            //            if (!mailControl)
            //            {
            //                mailControl = true;
            //                var users = db.GetVWSH_UserByIds(confirmations[i].confirmationUserIds.Split(',').Select(a => Guid.Parse(a)).ToArray());
            //                if (this.createdby.HasValue)
            //                {
            //                    var createdUser = db.GetVWSH_UserById(this.createdby.Value);
            //                    var url = TenantConfig.Tenant.GetWebUrl();
            //                    foreach (var user in users)
            //                    {
            //                        var text = "<h3>Sayın " + user.FullName + ",</h3>";
            //                        text += "<p>" + createdUser.FullName + " kişisi avans talebinde bulunmuştur.</p>";
            //                        if (!string.IsNullOrEmpty(this.description))
            //                        {
            //                            text += "<p>Açıklaması : " + this.description + "</p>";
            //                        }
            //                        text += "<p><a href='" + url + "/PA/VWPA_Advance/Detail?id=" + confirmations[i].advanceId + "'>Detaya gitmek için tıklayınız.</a> </p>";
            //                        text += "<p>Bilgilerinize.</p>";
            //                        new Email().Template("Template1", "bos.png", TenantConfig.Tenant.TenantName + " | Avans Talebi ", text).Send((Int16)EmailSendTypes.AvansOnay, user.email, "Avans Talebi", true);
            //                    }
            //                }
            //            }
            //            control = true;
            //        }
            //    }
            //}
        }
        public static SimpleQuery MyAdvanceQuery(SimpleQuery query, PageSecurity userStatus, int? direction)
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
                        Operand2 = (VAL)15
                    },
                    Operand2 = new BEXP
                    {
                        Operand1 = new BEXP
                        {
                            Operand1 = (COL)"rejectedUserIds",
                            Operator = BinaryOperator.Like,
                            Operand2 = (VAL)userStatus.user.id
                        },
                        Operand2 = new BEXP
                        {
                            Operand1 = new BEXP
                            {
                                Operand1 = (COL)"confirmationUserIds",
                                Operator = BinaryOperator.Like,
                                Operand2 = (VAL)userStatus.user.id
                            },
                            Operand2 = new BEXP
                            {
                                Operand1 = (COL)"confirmUserIds",
                                Operator = BinaryOperator.Like,
                                Operand2 = (VAL)userStatus.user.id
                            },
                            Operator = BinaryOperator.Or
                        },
                        Operator = BinaryOperator.Or
                    },
                    Operator = BinaryOperator.Or
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
                            Operand2 = (VAL)15
                        },
                        Operator = BinaryOperator.Or
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
        public static SimpleQuery MyAdvanceApprovedQuery(SimpleQuery query, PageSecurity userStatus, int? direction)
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
                        Operand2 = (VAL)15
                    },
                    Operator = BinaryOperator.Or,
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
                if (direction == (int)EnumPA_TransactionDirection.Talep || direction == 3)
                {
                    filter &= new BEXP
                    {
                        Operand1 = new BEXP
                        {
                            Operand1 = new BEXP
                            {
                                Operand1 = (COL)"confirmationStatus",
                                Operator = BinaryOperator.IsNull,
                                Operand2 = (VAL)null
                            },
                            Operand2 = new BEXP
                            {
                                Operand1 = (COL)"confirmationStatus",
                                Operator = BinaryOperator.Equal,
                                Operand2 = (VAL)1
                            },
                            Operator = BinaryOperator.Or
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
                                Operand1 = (COL)"direction",
                                Operator = BinaryOperator.Equal,
                                Operand2 = (VAL)(int)direction
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
                                Operand2 = (VAL)15
                            },
                            Operator = BinaryOperator.Or
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
                                Operand2 = (VAL)15
                            },
                            Operator = BinaryOperator.Or
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
        public VWINV_CompanyPersonDepartments GetCompanyPerson(VWINV_CompanyPersonDepartments[] companyPerson, Guid? ruleStage)
        {
            return companyPerson.Where(x => x.Manager1 == ruleStage ||
                                x.Manager2 == ruleStage ||
                                x.Manager3 == ruleStage ||
                                x.Manager4 == ruleStage ||
                                x.Manager5 == ruleStage ||
                                x.Manager6 == ruleStage).FirstOrDefault();
        }
    }
}
