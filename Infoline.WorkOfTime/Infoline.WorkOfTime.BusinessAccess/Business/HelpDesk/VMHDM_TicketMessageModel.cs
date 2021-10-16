using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.Data.Common;
using System.Linq;
using System.Web;

namespace Infoline.WorkOfTime.BusinessAccess
{
    public class VMHDM_TicketMessageModel : VWHDM_TicketMessage
    {
        private WorkOfTimeDatabase db { get; set; }
        private DbTransaction trans { get; set; }
        public VWHDM_Ticket Ticket { get; set; }
        public VWHDM_TicketRequester Requester { get; set; }
        private string _siteURL { get; set; } = TenantConfig.Tenant.GetWebUrl();
        private string _tenantName { get; set; } = TenantConfig.Tenant.TenantName;

        public VMHDM_TicketMessageModel Load()
        {
            this.db = this.db ?? new WorkOfTimeDatabase();
            var ticketMessage = db.GetVWHDM_TicketMessageById(this.id);

            if (ticketMessage != null)
            {
                this.B_EntityDataCopyForMaterial(ticketMessage, true);

                if (this.ticketId.HasValue)
                {
                    this.Ticket = this.db.GetVWHDM_TicketById(this.ticketId.Value);

                    if (this.Ticket != null && this.Ticket.requesterId.HasValue)
                    {
                        this.Requester = this.db.GetVWHDM_TicketRequesterById(this.Ticket.requesterId.Value);
                    }
                }
            }

            return this;
        }

        public ResultStatus Save(Guid userid, HttpRequestBase request = null, DbTransaction trans = null, string files = "")
        {
            this.db = this.db ?? new WorkOfTimeDatabase();
            this.trans = trans ?? db.BeginTransaction();
            var ticketMessage = db.GetVWHDM_TicketMessageById(this.id);

            var res = new ResultStatus { result = true };

            if (this.ticketId.HasValue)
            {
                this.Ticket = this.db.GetVWHDM_TicketById(this.ticketId.Value);

                if (this.Ticket != null && this.Ticket.requesterId.HasValue)
                {
                    this.Requester = this.db.GetVWHDM_TicketRequesterById(this.Ticket.requesterId.Value);
                }
            }

            if (ticketMessage == null)
            {
                this.createdby = userid;
                this.created = DateTime.Now;
                res = Insert(trans, files);
            }
            else
            {
                this.changedby = userid;
                this.changed = DateTime.Now;
                res = Update(trans);
            }

            if (request != null && res.result)
            {
                new FileUploadSave(request, this.id).SaveAs();
            }

            if (trans == null)
            {
                if (res.result)
                    this.trans.Commit();
                else
                    this.trans.Rollback();
            }

            return res;
        }

        private ResultStatus Insert(DbTransaction trans, string files = "")
        {
            var rs = this.db.InsertHDM_TicketMessage(new HDM_TicketMessage().B_EntityDataCopyForMaterial(this, true), trans);

            var ticketAction = new HDM_TicketAction
            {
                id = this.id != null ? this.id : Guid.NewGuid(),
                created = DateTime.Now,
                createdby = this.createdby,
                ticketId = this.ticketId,
                description = this.type == (int)EnumHDM_TicketMessageType.Reply ? "Yardım talebine cevaplandı." :
                              this.type == (int)EnumHDM_TicketMessageType.Forward ? "Yardım talebi mail olarak iletildi." :
                              this.type == (int)EnumHDM_TicketMessageType.Note ? "Yardım talebine not eklendi." : "",
                type = this.type == (int)EnumHDM_TicketMessageType.Reply ? (short)EnumHDM_TicketActionType.Mesaj :
                              this.type == (int)EnumHDM_TicketMessageType.Forward ? (short)EnumHDM_TicketActionType.BaskasinaMail :
                              this.type == (int)EnumHDM_TicketMessageType.Note ? (short)EnumHDM_TicketActionType.Not : (short)EnumHDM_TicketActionType.Mesaj,
            };

            rs &= this.db.InsertHDM_TicketAction(ticketAction, this.trans);

            var managerRol = new Guid(SHRoles.YardimMasaYonetim);
            var personelRol = new Guid(SHRoles.YardimMasaPersonel);
            var customerRole = new Guid(SHRoles.YardimMasaMusteri);
            var talepRol = new Guid(SHRoles.YardimMasaTalep);

            if (this.type == (int)EnumHDM_TicketMessageType.Reply)
            {
                var cc = String.IsNullOrEmpty(this.cc) ? null : this.cc.Split(',');
                var bcc = String.IsNullOrEmpty(this.bcc) ? null : this.bcc.Split(',');

                if (this.Ticket.requesterId == this.createdby)
                {
                    if (this.Ticket.assignUserId.HasValue)
                    {
                        if (!string.IsNullOrEmpty(files))
                        {
                            files = _siteURL + files;
                        }
                        var roles = this.db.GetSH_UserRoleByUserId(this.Ticket.assignUserId.Value);
                        var isPersonelRole = roles.Where(a => a.userid == this.Ticket.assignUserId.Value && a.roleid == personelRol).FirstOrDefault();

                        if(isPersonelRole == null)
                        {
                            isPersonelRole = roles.Where(a => a.userid == this.Ticket.assignUserId.Value && a.roleid == customerRole).FirstOrDefault();
                        }

                        if (isPersonelRole != null)
                        {
                            var assignUser = this.db.GetVWSH_UserById(this.Ticket.assignUserId.Value);

                            var text = "<h3>Sayın " + assignUser.FullName + "</h3>";
                            text += "<p>" + this.Ticket.code + " kodlu yardım talebi için talep eden tarafından aşağıdaki mesaj yazılmıştır. </p>";
                            text += this.content;
                            text += "<p>Kontrolleri gerçekleştirmek için lütfen <a href='" + _siteURL + "/HDM/VWHDM_Ticket/Detail?id=" + this.Ticket.id + "'>tıklayınız.</a> </p>";
                            text += "<p>Bilgilerinize.</p>";
                            new Email().Template("Template1", "teknikserviszimmet.jpg", _tenantName + " | WorkOfTime | Yardım & Destek", text)
                                       .Send((Int16)EmailSendTypes.YardimCozum, assignUser.email, "Yardım & Destek Talebine Mesaj Yazıldı", true, cc, bcc, string.IsNullOrEmpty(files) ? null : new string[] { files });
                        }
                    }
                }
                else
                {
                    var roles = this.db.GetSH_UserRoleByUserId(this.Ticket.requesterId.Value);
                    var isRequesterRole = roles.Where(a => a.userid == this.Ticket.requesterId.Value && (a.roleid == talepRol || a.roleid == managerRol || a.roleid == personelRol)).FirstOrDefault();

                    if (isRequesterRole != null)
                    {
                        var text = "<h3>Sayın " + this.Requester.fullName + "</h3>";
                        text += "<p>" + this.Ticket.code + " kodlu yardım talebinize aşağıdaki mesaj yazılmıştır. </p>";
                        text += this.content;
                        text += "<p>Kontrolleri gerçekleştirmek ve cevap yazmak için lütfen <a href='" + _siteURL + "/HDM/VWHDM_Ticket/Detail?id=" + this.Ticket.id + "'>tıklayınız.</a> </p>";
                        text += "<p>Bilgilerinize.</p>";
                        new Email().Template("Template1", "teknikserviszimmet.jpg", _tenantName + " | WorkOfTime | Yardım & Destek", text)
                                   .Send((Int16)EmailSendTypes.YardimCozum, this.Requester.email, "Yardım & Destek Talebine Mesaj Yazıldı", true, cc, bcc, string.IsNullOrEmpty(files) ? null : new string[] { files });
                    }
                }
            }

            if (this.type == (int)EnumHDM_TicketMessageType.Note)
            {
                if (this.Ticket.requesterId == this.createdby)
                {
                    if (this.Ticket.assignUserId.HasValue)
                    {
                        var roles = this.db.GetSH_UserRoleByUserId(this.Ticket.assignUserId.Value);
                        var isPersonelRole = roles.Where(a => a.userid == this.Ticket.assignUserId.Value && a.roleid == personelRol).FirstOrDefault();

                        if (isPersonelRole != null)
                        {
                            var assignUser = this.db.GetVWSH_UserById(this.Ticket.assignUserId.Value);

                            var text = "<h3>Sayın " + assignUser.FullName + "</h3>";
                            text += "<p>" + this.Ticket.code + " kodlu yardım talebi için aşağıdaki not yazılmıştır. </p>";
                            text += this.content;
                            text += "<p>Kontrolleri gerçekleştirmek için lütfen <a href='" + _siteURL + "/HDM/VWHDM_Ticket/Detail?id=" + this.Ticket.id + "'>tıklayınız.</a> </p>";
                            text += "<p>Bilgilerinize.</p>";
                            new Email().Template("Template1", "teknikserviszimmet.jpg", _tenantName + " | WorkOfTime | Yardım & Destek", text)
                                       .Send((Int16)EmailSendTypes.YardimCozum, assignUser.email, "Yardım & Destek Talebine Mesaj Yazıldı", true, null, null, string.IsNullOrEmpty(files) ? null : new string[] { files });
                        }


                    }
                }
                else
                {
                    var roles = this.db.GetSH_UserRoleByUserId(this.Ticket.requesterId.Value);
                    var isRequesterRole = roles.Where(a => a.userid == this.Ticket.requesterId.Value && a.roleid == talepRol).FirstOrDefault();

                    if (isRequesterRole != null)
                    {
                        var text = "<h3>Sayın " + this.Requester.fullName + "</h3>";
                        text += "<p>" + this.Ticket.code + " kodlu yardım talebinize aşağıdaki not yazılmıştır. </p>";
                        text += this.content;
                        text += "<p>Kontrolleri gerçekleştirmek için lütfen <a href='" + _siteURL + "/HDM/VWHDM_Ticket/Detail?id=" + this.Ticket.id + "'>tıklayınız.</a> </p>";
                        text += "<p>Bilgilerinize.</p>";
                        new Email().Template("Template1", "teknikserviszimmet.jpg", _tenantName + " | WorkOfTime | Yardım & Destek", text)
                                   .Send((Int16)EmailSendTypes.YardimCozum, this.Requester.email, "Yardım & Destek Talebine Mesaj Yazıldı", true, null, null, string.IsNullOrEmpty(files) ? null : new string[] { files });
                    }
                }
            }

            if (this.type == (int)EnumHDM_TicketMessageType.Forward)
            {
                var cc = String.IsNullOrEmpty(this.cc) ? null : this.cc.Split(',');
                var bcc = String.IsNullOrEmpty(this.bcc) ? null : this.bcc.Split(',');
                var forwardUserEmails = String.IsNullOrEmpty(this.mailto) ? null : this.mailto.Split(',');

                foreach (var forwardEmail in forwardUserEmails)
                {
                    var text = "<h3>Merhabalar, </h3>";
                    text += "<p>" + this.Ticket.code + " kodlu yardım talebi ile alakalı size aşağıdaki ileti gönderilmiştir. </p>";
                    text += this.content;
                    text += "<p>Kontrollleri gerçekleştirmek için lütfen <a href='" + _siteURL + "/HDM/VWHDM_Ticket/Detail?id=" + this.Ticket.id + "'>tıklayınız.</a> </p>";
                    text += "<p>Bilgilerinize.</p>";
                    new Email().Template("Template1", "teknikserviszimmet.jpg", _tenantName + " | WorkOfTime | Yardım & Destek", text)
                               .Send((Int16)EmailSendTypes.YardimTalep, forwardEmail, "Yardım & Destek Talebi İletildi", true, cc, bcc, string.IsNullOrEmpty(files) ? null : new string[] { files });
                }
            }

            return rs;
        }

        private ResultStatus Update(DbTransaction trans)
        {
            var rs = this.db.UpdateHDM_TicketMessage(new HDM_TicketMessage().B_EntityDataCopyForMaterial(this, true), false, trans);
            return rs;
        }
    }
}
