using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessAccess.Mobile;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Web;
namespace Infoline.WorkOfTime.BusinessAccess
{
    public class VMHDM_TicketIndexModel
    {
        public List<Guid?> IssueIds { get; set; }
        public SH_User[] PersonelUsers { get; set; }
    }
    public class VWHDM_TicketMessageFiles : VWHDM_TicketMessage
    {
        public SYS_Files[] files { get; set; }
    }
    public class VMHDM_TicketModel : VWHDM_Ticket
    {
        private WorkOfTimeDatabase db { get; set; }
        private DbTransaction trans { get; set; }
        private string _siteURL { get; set; } = TenantConfig.Tenant.GetWebUrl();
        private string _tenantName { get; set; } = TenantConfig.Tenant.TenantName;
        public VWHDM_TicketRequester Requester { get; set; }
        public VWHDM_TicketAction[] Actions { get; set; }
        public VWHDM_Issue Issue { get; set; }
        public VWSH_User[] IssueManagers { get; set; }
        public VWSH_User AssignUser { get; set; }
        public VWHDM_TicketMessageFiles[] Messages { get; set; }
        public VMHDM_TicketModel Load(short? status = null)
        {
            this.db = this.db ?? new WorkOfTimeDatabase();
            var ticket = this.db.GetVWHDM_TicketById(this.id);
            if (ticket != null)
            {
                this.B_EntityDataCopyForMaterial(ticket, true);
                if (status.HasValue) { this.status = status; }
                this.Actions = this.db.GetVWHDM_TicketActionByTicketId(this.id);
                var Messages = this.db.GetVWHDM_TicketMessageByTicketId(this.id);
                var filesList = new List<VWHDM_TicketMessageFiles>();
                for (int i = 0; i < Messages.Length; i++)
                {
                    var filesData = new VWHDM_TicketMessageFiles().B_EntityDataCopyForMaterial(Messages[i]);
                    var files = this.db.GetSYS_FilesByDataIdAll(filesData.id);
                    if (files.Count() > 0)
                    {
                        filesData.files = files;
                    }
                    filesList.Add(filesData);
                }
                this.Messages = filesList.ToArray();
                if (this.requesterId.HasValue)
                {
                    this.Requester = this.db.GetVWHDM_TicketRequesterById(this.requesterId.Value);
                }
                if (this.assignUserId.HasValue)
                {
                    this.AssignUser = this.db.GetVWSH_UserById(this.assignUserId.Value);
                }
            }
            else
            {
                if (this.suggestionId.HasValue)
                {
                    var suggestion = this.db.GetHDM_SuggestionById(this.suggestionId.Value);
                    this.title = suggestion.title;
                    if (suggestion.assignUserId.HasValue)
                    {
                        this.assignUserId = suggestion.assignUserId.Value;
                    }
                }
            }
            if (this.suggestionId.HasValue && !this.issueId.HasValue)
            {
                var suggestion = this.db.GetHDM_SuggestionById(this.suggestionId.Value);
                this.issueId = suggestion.issueId;
            }
            if (this.issueId.HasValue)
            {
                this.Issue = this.db.GetVWHDM_IssueById(this.issueId.Value);
                var mainIssueId = this.Issue.mainId.HasValue ? this.Issue.mainId.Value : this.Issue.id;
                var issueManagerIds = this.db.GetVWHDM_IssueUserByIssueId(mainIssueId).Where(a => a.userId.HasValue).Select(a => a.userId.Value).ToArray();
                if (issueManagerIds.Count() > 0)
                    this.IssueManagers = this.db.GetVWSH_UserByIds(issueManagerIds);
            }
            this.priority = this.priority ?? (int)EnumHDM_TicketPriority.Orta;
            this.status = this.status ?? (int)EnumHDM_TicketStatus.Open;
            this.code = this.code ?? BusinessExtensions.B_GetIdCode();
            return this;
        }
        public ResultStatus Save(Guid userid, int? actionType, HttpRequestBase request = null, DbTransaction trans = null)
        {
            this.db = this.db ?? new WorkOfTimeDatabase();
            this.trans = trans ?? this.db.BeginTransaction();
            var ticket = this.db.GetVWHDM_TicketById(this.id);
            if (AssignUser == null && this.assignUserId != null)
            {
                AssignUser = db.GetVWSH_UserById(this.assignUserId.Value);
            }
            var rs = IsExistTicketProperty();
            if (rs.result == false)
            {
                return rs;
            }
            if (ticket == null)
            {
                this.createdby = userid;
                this.created = DateTime.Now;
                rs = Insert(actionType);
            }
            else
            {
                //Todo burada aynı action mı kontrolü yapılabilir.
                //if(this.Actions.Count() > 0 && this.Actions.OrderByDescending(x => x.created).Select(x => x.type).FirstOrDefault() == actionType)
                //{
                //    rs.result = false;
                //    rs.message = "Süreç durumu aynı durumu güncellenemez.";
                //}
                this.changedby = userid;
                this.changed = DateTime.Now;
                rs = Update(actionType);
            }
            if (request != null && rs.result)
            {
                new FileUploadSave(request, this.id).SaveAs();
            }
            if (trans == null)
            {
                if (rs.result)
                    this.trans.Commit();
                else
                    this.trans.Rollback();
            }
            return rs;
        }
        private ResultStatus IsExistTicketProperty()
        {
            db = db ?? new WorkOfTimeDatabase();
            var rs = new ResultStatus { result = true };
            return rs;
        }
        private ResultStatus Insert(int? actionType)
        {
            var rs = InsertRequester();
            rs &= InsertAssignUserRole();
            rs &= this.db.InsertHDM_Ticket(new HDM_Ticket().B_EntityDataCopyForMaterial(this, true), this.trans);
            if (actionType.HasValue)
            {
                rs &= InsertAction(this.createdby.Value, actionType.Value);
            }
            if (this.assignUserId.HasValue)
            {
                rs &= InsertAction(this.createdby.Value, (int)EnumHDM_TicketActionType.PersonelAtama);
            }
            return rs;
        }
        private ResultStatus Update(int? actionType)
        {
            var rs = new ResultStatus { result = true };
            if (actionType.HasValue)
            {
                rs &= InsertAction(this.changedby.Value, actionType.Value);
            }
            rs &= this.db.UpdateHDM_Ticket(new HDM_Ticket().B_EntityDataCopyForMaterial(this, true), false, this.trans);
            rs &= InsertAssignUserRole();
            return rs;
        }
        private ResultStatus InsertAssignUserRole()
        {
            var rs = new ResultStatus { result = true };
            if (this.assignUserId.HasValue)
            {
                var user = this.db.GetSH_UserById(this.assignUserId.Value);
                if (user != null)
                {
                    var userRoleControl = this.db.GetSH_UserRoleByUserIdRoleId(user.id, new Guid(SHRoles.YardimMasaPersonel));
                    if (userRoleControl.Count() == 0)
                    {
                        var newUserRole = new SH_UserRole
                        {
                            id = Guid.NewGuid(),
                            created = DateTime.Now,
                            createdby = this.createdby,
                            roleid = new Guid(SHRoles.YardimMasaPersonel),
                            userid = user.id
                        };
                        rs &= this.db.InsertSH_UserRole(newUserRole, this.trans);
                    }
                }
            }
            return rs;
        }
        public static SimpleQuery UpdateQuery(SimpleQuery query, PageSecurity userStatus)
        {
            BEXP filter = null;
            var data = query.Filter.ToString();
            if (!data.Contains("assignUserId"))
            {
                filter &= new BEXP
                {
                    Operand1 = (COL)"assignUserId",
                    Operator = BinaryOperator.Equal,
                    Operand2 = (VAL)string.Format("{0}", userStatus.user.id.ToString())
                };
            }
            query.Filter &= filter;
            return query;
        }
        public static SimpleQuery UpdateQueryPersonal(SimpleQuery query, PageSecurity userStatus)
        {
            BEXP filter = null;
            var data = query.Filter.ToString();
            if (!data.Contains("requesterId"))
            {
                filter &= new BEXP
                {
                    Operand1 = (COL)"requesterId",
                    Operator = BinaryOperator.Equal,
                    Operand2 = (VAL)string.Format("{0}", userStatus.user.id.ToString())
                };
            }
            query.Filter &= filter;
            return query;
        }
        private ResultStatus InsertRequester()
        {
            var rs = new ResultStatus { result = true };
            var ticketRequester = new HDM_TicketRequester().B_EntityDataCopyForMaterial(this.Requester);
            if (this.requesterId.HasValue)
            {
                var dbrequesterControl = this.db.GetHDM_TicketRequesterById(this.requesterId.Value, this.trans);
                if (dbrequesterControl == null)
                {
                    ticketRequester.created = DateTime.Now;
                    ticketRequester.createdby = this.createdby;
                    ticketRequester.id = this.requesterId.Value;
                    rs &= this.db.InsertHDM_TicketRequester(ticketRequester, this.trans);
                }
                else
                {
                    dbrequesterControl.changed = DateTime.Now;
                    dbrequesterControl.changedby = this.createdby;
                    dbrequesterControl.email = ticketRequester.email;
                    dbrequesterControl.phone = ticketRequester.phone;
                    dbrequesterControl.fullName = ticketRequester.fullName;
                    dbrequesterControl.company = ticketRequester.company;
                    rs &= this.db.UpdateHDM_TicketRequester(dbrequesterControl, false, this.trans);
                }
            }
            else
            {
                ticketRequester.created = DateTime.Now;
                ticketRequester.createdby = this.createdby;
                this.requesterId = ticketRequester.id;
                rs &= this.db.InsertHDM_TicketRequester(ticketRequester, this.trans);
            }
            return rs;
        }
        private ResultStatus InsertAction(Guid userid, int type)
        {
            var action = new HDM_TicketAction
            {
                id = Guid.NewGuid(),
                created = DateTime.Now,
                createdby = userid,
                ticketId = this.id,
                type = (short)type,
                ticketStatus = this.status,
            };
            var ticket = this.db.GetVWHDM_TicketById(this.id);
            var managerRol = new Guid(SHRoles.YardimMasaYonetim);
            var personelRol = new Guid(SHRoles.YardimMasaPersonel);
            var customerRol = new Guid(SHRoles.YardimMasaMusteri);
            var talepRol = new Guid(SHRoles.YardimMasaTalep);
            switch (type)
            {
                case (int)EnumHDM_TicketActionType.YeniTalep:
                    action.description = "Talep oluşturuldu";
                    if (!String.IsNullOrEmpty(this.Requester.email) && this.requesterId.HasValue)
                    {
                        var roles = this.db.GetSH_UserRoleByUserId(this.requesterId.Value);
                        var isRequesterRole = roles.Where(a => a.userid == this.requesterId.Value && a.roleid == talepRol).FirstOrDefault();
                        if (isRequesterRole != null)
                        {
                            var text = "<h3>Sayın " + this.Requester.fullName + "</h3>";
                            text += "<p>" + this.code + " kodlu yardım talebiniz oluşturulmuştur. Talebiniz en kısa süre içinde çözümlenip, tarafınıza bildirilecektir. </p>";
                            text += "<p>Talebin durumunu görüntülemek ve işlem yapmak için <a href='" + _siteURL + "/HDM/VWHDM_Ticket/Detail?id=" + this.id + "'>tıklayınız.</a> </p>";
                            text += "<p>Bilgilerinize.</p>";
                            new Email().Template("Template1", "teknikserviszimmet.jpg", _tenantName + " | WorkOfTime | Yardım Destek Yönetimi", text).Send((Int16)EmailSendTypes.YardimCozum, this.Requester.email, "Yardım Talebi Oluşturuldu", true);
                        }
                    }
                    if (this.AssignUser != null && !String.IsNullOrEmpty(this.AssignUser.email) && this.assignUserId.HasValue)
                    {
                        var roles = this.db.GetSH_UserRoleByUserId(this.assignUserId.Value);
                        var isPersonelRole = roles.Where(a => a.userid == this.assignUserId.Value && (a.roleid == personelRol || a.roleid == customerRol)).FirstOrDefault();
                        if (isPersonelRole != null)
                        {
                            var text = "<h3>Sayın " + this.AssignUser.FullName + "</h3>";
                            text += "<p>Tarafınıza " + this.code + " kodlu yardım talebi oluşturulmuştur. Talebi en kısa süre içinde çözümlemeniz gerekmektedir. </p>";
                            text += "<p>Talep ile ilgili işlem yapmak için <a href='" + _siteURL + "/HDM/VWHDM_Ticket/Detail?id=" + this.id + "'>tıklayınız.</a> </p>";
                            text += "<p>Bilgilerinize.</p>";
                            new Email().Template("Template1", "teknikserviszimmet.jpg", _tenantName + " | WorkOfTime | Yardım Destek Yönetimi", text).Send((Int16)EmailSendTypes.YardimTalep, this.AssignUser.email, "Yardım Talebi Oluşturuldu", true);
                        }
                    }
                    break;
                case (int)EnumHDM_TicketActionType.PersonelAtama:
                    action.description = "Personel ataması yapıldı";
                    break;
                case (int)EnumHDM_TicketActionType.BaskaPersonelAta:
                    action.description = "Görevli personel değiştirildi";
                    if (this.AssignUser != null && !String.IsNullOrEmpty(this.AssignUser.email) && this.assignUserId.HasValue)
                    {
                        var roles = this.db.GetSH_UserRoleByUserId(this.assignUserId.Value);
                        var isPersonelRole = roles.Where(a => a.userid == this.assignUserId.Value && (a.roleid == personelRol || a.roleid == customerRol)).FirstOrDefault();
                        if (isPersonelRole != null)
                        {
                            var text = "<h3>Sayın " + this.AssignUser.FullName + "</h3>";
                            text += "<p>Tarafınıza " + this.code + " kodlu yardım talebi oluşturulmuştur. Talebi en kısa süre içinde çözümlemeniz gerekmektedir. </p>";
                            text += "<p>Talep ile ilgili işlem yapmak için <a href='" + _siteURL + "/HDM/VWHDM_Ticket/Detail?id=" + this.id + "'>tıklayınız.</a> </p>";
                            text += "<p>Bilgilerinize.</p>";
                            new Email().Template("Template1", "teknikserviszimmet.jpg", _tenantName + " | WorkOfTime | Yardım Destek Yönetimi", text).Send((Int16)EmailSendTypes.YardimTalep, this.AssignUser.email, "Yardım Talebi Oluşturuldu", true);
                        }
                    }
                    break;
                case (int)EnumHDM_TicketActionType.Beklemede:
                    this.status = (int)EnumHDM_TicketStatus.Pending;
                    action.ticketStatus = (int)EnumHDM_TicketStatus.Pending;
                    action.description = "Talep beklemeye alındı";
                    break;
                case (int)EnumHDM_TicketActionType.Degerlendirme:
                    this.status = ticket.status;
                    action.description = "Talebin çözümü değerlendirildi";
                    break;
                case (int)EnumHDM_TicketActionType.IsBaslangic:
                    this.status = (int)EnumHDM_TicketStatus.InProgress;
                    action.ticketStatus = (int)EnumHDM_TicketStatus.InProgress;
                    action.description = "Talebin çözümü için çalışmaya başlandı";
                    break;
                case (int)EnumHDM_TicketActionType.Kapatildi:
                    this.status = (int)EnumHDM_TicketStatus.Closed;
                    action.ticketStatus = (int)EnumHDM_TicketStatus.Closed;
                    action.description = "Talep çözümlenerek kapatıldı";
                    if (!String.IsNullOrEmpty(this.Requester.email) && this.requesterId.HasValue)
                    {
                        var roles = this.db.GetSH_UserRoleByUserId(this.requesterId.Value);
                        var isRequesterRole = roles.Where(a => a.userid == this.requesterId.Value && a.roleid == talepRol).FirstOrDefault();
                        if (isRequesterRole != null)
                        {
                            var text = "<h3>Sayın " + this.Requester.fullName + "</h3>";
                            text += "<p>" + this.code + " kodlu yardım talebiniz için çözüm sağlanmıştır. </p>";
                            text += "<p>Talebiniz ile ilgili değerlendirmede bulunmak için <a href='" + _siteURL + "/HDM/VWHDM_Ticket/Evaluate?id=" + this.id + "'>tıklayınız.</a> </p>";
                            text += "<p>Bilgilerinize.</p>";
                            new Email().Template("Template1", "teknikserviszimmet.jpg", _tenantName + " | WorkOfTime | Yardım Destek Yönetimi", text).Send((Int16)EmailSendTypes.YardimCozum, this.Requester.email, "Yardım Talebi Kapatıldı", true);
                        }
                    }
                    break;
                case (int)EnumHDM_TicketActionType.Onay:
                    this.status = (int)EnumHDM_TicketStatus.Closed;
                    action.type = (int)EnumHDM_TicketActionType.Kapatildi;
                    action.description = "Talep, talep eden tarafından kapatıldı";
                    break;
                case (int)EnumHDM_TicketActionType.Red:
                    action.description = "Talebin çözümü reddedildi";
                    this.status = (int)EnumHDM_TicketStatus.InProgress;
                    action.ticketStatus = (int)EnumHDM_TicketStatus.InProgress;
                    if (this.AssignUser != null && !String.IsNullOrEmpty(this.AssignUser.email) && this.assignUserId.HasValue)
                    {
                        var roles = this.db.GetSH_UserRoleByUserId(this.assignUserId.Value);
                        var isPersonelRole = roles.Where(a => a.userid == this.assignUserId.Value && (a.roleid == personelRol || a.roleid == customerRol)).FirstOrDefault();
                        if (isPersonelRole != null)
                        {
                            var text = "<h3>Sayın " + this.AssignUser.FullName + "</h3>";
                            text += "<p>" + this.code + " kodlu yardım talebi için çözümünüz reddedilmiştir. </p>";
                            text += "<p>Talebe tekrar çözüm sağlamak için <a href='" + _siteURL + "/HDM/VWHDM_Ticket/Detail?id=" + this.id + "'>tıklayınız.</a> </p>";
                            text += "<p>Bilgilerinize.</p>";
                            new Email().Template("Template1", "teknikserviszimmet.jpg", _tenantName + " | WorkOfTime | Yardım Destek Yönetimi", text).Send((Int16)EmailSendTypes.YardimCozum, this.AssignUser.email, "Yardım Talebi Çözüm Reddedildi", true);
                        }
                    }
                    break;
                case (int)EnumHDM_TicketActionType.CozumSaglandi:
                    this.status = (int)EnumHDM_TicketStatus.Done;
                    action.ticketStatus = (int)EnumHDM_TicketStatus.Done;
                    action.description = "Talep için çözüm sağlandı";
                    if (this.IssueManagers != null && this.IssueManagers.Count() > 0)
                    {
                        foreach (var manager in this.IssueManagers)
                        {
                            if (!String.IsNullOrEmpty(manager.email))
                            {
                                var text = "<h3>Sayın " + manager.FullName + "</h3>";
                                text += "<p>" + this.code + " kodlu yardım talebi için çözüm sağlanmıştır. </p>";
                                text += "<p>Talebin çözümü onaylamak ve talebi kapatmak için <a href='" + _siteURL + "/HDM/VWHDM_Ticket/Detail?id=" + this.id + "'>tıklayınız.</a> </p>";
                                text += "<p>Bilgilerinize.</p>";
                                new Email().Template("Template1", "teknikserviszimmet.jpg", _tenantName + " | WorkOfTime | Yardım Destek Yönetimi", text).Send((Int16)EmailSendTypes.YardimCozum, manager.email, "Yardım Talebi Çözüm Sağlandı", true);
                            }
                        }
                    }
                    break;
                case (int)EnumHDM_TicketActionType.Vazgec:
                    this.status = (int)EnumHDM_TicketStatus.Cancelled;
                    action.ticketStatus = (int)EnumHDM_TicketStatus.Cancelled;
                    action.description = "Talep gereksiz olarak işaretlendi.";
                    break;
                default:
                    break;
            }
            var rs = this.db.InsertHDM_TicketAction(action, this.trans);
            return rs;
        }
        public string GetPassingTime(DateTime? time)
        {
            if (this.created.HasValue && time.HasValue)
            {
                var fark = (time - this.created);
                var result = (fark.Value.Days != 0 ? fark.Value.Days + " Gün " : "");
                result += (fark.Value.Hours != 0 ? fark.Value.Hours + " Saat " : "");
                result += (fark.Value.Minutes != 0 ? fark.Value.Minutes + " Dakika " : "");
                result += (fark.Value.Seconds != 0 ? fark.Value.Seconds + " Saniye " : "");
                return result;
            }
            else
            {
                return "-";
            }
        }
        public ResultStatus Delete(Guid[] authorizedRoles)
        {
            this.db = this.db ?? new WorkOfTimeDatabase();
            this.trans = trans ?? db.BeginTransaction();
            var ticket = this.db.GetHDM_TicketById(this.id);
            if (ticket == null)
            {
                return new ResultStatus { message = "Yardım talebi zaten silinmiş durumda.", result = false };
            }
            var actions = this.db.GetHDM_TicketActionByTicketId(this.id);
            var messages = this.db.GetHDM_TicketMessageByTicketId(this.id);
            if (!authorizedRoles.Contains(new Guid(SHRoles.YardimMasaYonetim)) && !authorizedRoles.Contains(new Guid(SHRoles.SistemYonetici)))
            {
                return new ResultStatus { message = "Yardım talebi silme yetkiniz bulunmuyor.", result = false };
            }
            var dbresult = db.BulkDeleteHDM_TicketAction(actions, this.trans);
            dbresult &= db.BulkDeleteHDM_TicketMessage(messages, this.trans);
            dbresult &= db.DeleteHDM_Ticket(ticket, this.trans);
            if (this.trans != null)
            {
                if (dbresult.result)
                    this.trans.Commit();
                else
                    this.trans.Rollback();
            }
            return dbresult;
        }
        public SummaryHeadersTicket GetMyTicketSummary(Guid userId)
        {
            this.Load();
            return db.GetDBVWHDM_GetMyTicketSummary(userId);
        }
        public SummaryHeadersTicket GetMyMasterManagerTicketSummary(Guid userId)
        {
            this.Load();
            return db.GetDBVWHDM_GetMyMasterManagerTicketSummary(userId);
        }
        public SummaryHeadersTicket GetMyManagerTicketSummary(Guid userId)
        {
            this.Load();
            return db.GetDBVWHDM_GetMyManagerTicketSummary(userId);
        }
        public ResultStatus<Splitted<VWHDM_Ticket>> MySummaryTickets(Guid userid)
        {
            var db = new WorkOfTimeDatabase();
            var datas = db.GetDBVWHDM_TicketMyWaiting(userid);
            var onaylanan = datas;
            return new ResultStatus<Splitted<VWHDM_Ticket>>
            {
                result = true,
                objects = new Splitted<VWHDM_Ticket>
                {
                    Waiting = onaylanan
                }
            };
        }
        public ResultStatus<Splitted<VWHDM_Ticket>> MySummaryManagerTickets(Guid userid)
        {
            var db = new WorkOfTimeDatabase();
            var datas = db.GetDBVWHDM_TicketMyManagerWaiting(userid);
            var onaylanan = datas;
            return new ResultStatus<Splitted<VWHDM_Ticket>>
            {
                result = true,
                objects = new Splitted<VWHDM_Ticket>
                {
                    Waiting = onaylanan
                }
            };
        }
    }
}
