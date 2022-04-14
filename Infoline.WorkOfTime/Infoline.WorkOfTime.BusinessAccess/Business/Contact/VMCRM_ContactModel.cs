using GeoAPI.Geometries;
using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Web;

namespace Infoline.WorkOfTime.BusinessAccess
{
    public class Users
    {
        public Guid UserId { get; set; }
        public string FullName { get; set; }
    }

    public class Participants
    {
        public Guid? UserId { get; set; }
        public string UserName { get; set; }
        public int? Type { get; set; }
    }

    public class Participant
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }

    public class VMCRM_ContactModel : VWCRM_Contact
    {
        private WorkOfTimeDatabase db { get; set; }
        private DbTransaction trans { get; set; }
        public Users[] Users { get; set; }
        public bool mailForParticipants { get; set; }
        public List<Participants> Participants { get; set; }
        public Guid? RelationId { get; set; }
        public List<VWCRM_ContactAction> contactActions { get; set; }
        public IGeometry location { get; set; }


        public VMCRM_ContactModel Load()
        {
            this.db = this.db ?? new WorkOfTimeDatabase();
            var contact = db.GetVWCRM_ContactById(this.id);
            var participantList = new List<Participants>();
            this.contactActions = db.GetVWCRM_ContactActionByContactId(this.id).ToList();


            if (contact != null)
            {
                this.B_EntityDataCopyForMaterial(contact, true);
                var data = db.GetVWCRM_ContactUserByContactId(this.id);
                if (data.Length > 0)
                {
                    foreach (var contactUser in data)
                    {
                        participantList.Add(new Participants
                        {
                            Type = contactUser.UserType,
                            UserId = contactUser.UserId,
                            UserName = contactUser.User_Title
                        });
                    }

                    Participants = participantList;
                }
            }
            return this;
        }

        public VWCRM_ContactUser[] CalendarDatas(Guid? userId)
        {
            this.db = this.db ?? new WorkOfTimeDatabase();
            var datas = new List<VWCRM_ContactUser>();
            var contactUsers = db.GetVWCRM_ContactUser();
            if (userId.HasValue)
            {
                contactUsers = contactUsers.Where(x => x.UserId == userId.Value).ToArray();
            }
            datas.AddRange(contactUsers.Select(x => new VWCRM_ContactUser().B_EntityDataCopyForMaterial(x)).ToList());
            return datas.ToArray();
        }

        public List<Participants> GetParticipants(string userName)
        {
            this.db = this.db ?? new WorkOfTimeDatabase();
            var participants = GetSH_UsersForPresentationById(this.id);
            var participantList = new List<Participants>();

            if (participants != null)
            {
                if (!string.IsNullOrEmpty(userName))
                {
                    var username = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(userName);
                    participants = participants.Where(x => x.UserName.Contains(username.ToLower()) || x.UserName.Contains(username.ToUpper()) || x.UserName.Contains(username)).ToList();
                }

                foreach (var participantData in participants)
                {
                    participantList.Add(new Participants
                    {
                        Type = participantData.Type,
                        UserId = participantData.UserId,
                        UserName = participantData.UserName
                    });
                }
            }
            return participantList.OrderBy(x => x.UserName).ToList();
        }

        public ResultStatus Save(Guid userid, HttpRequestBase request = null, DbTransaction trans = null)
        {
            db = db ?? new WorkOfTimeDatabase();
            var contact = db.GetVWCRM_ContactById(this.id);
            var res = new ResultStatus { result = true };


            if (contact == null)
            {
                this.createdby = userid;
                this.created = DateTime.Now;
                res = Insert(trans);
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
                new TableAdditionalPropertySave(request, this.id, "CRM_Contact").SaveAs(this.changedby ?? this.createdby);
            }
            return res;
        }

        private ResultStatus Insert(DbTransaction trans = null)
        {
            var transaction = trans ?? db.BeginTransaction();

            var dbresult = db.InsertCRM_Contact(new CRM_Contact().B_EntityDataCopyForMaterial(this), this.trans);


            dbresult &= db.InsertCRM_PresentationAction(new CRM_PresentationAction
            {
                created = DateTime.Now,
                createdby = this.createdby,
                description = "Yeni aktivite/randevu eklendi.",
                presentationId = this.PresentationId.Value,
                type = (short)EnumCRM_PresentationActionType.YeniAktivite,
                contactId = this.id
            }, this.trans);


            dbresult &= db.BulkInsertCRM_ContactUser(Users.Select(a => new CRM_ContactUser
            {
                id = Guid.NewGuid(),
                created = DateTime.Now,
                createdby = this.createdby,
                UserId = a.UserId,
                ContactId = this.id,
                UserType = GetUserByType(a.UserId)
            }), this.trans);


            if (this.PresentationStageId != null)
            {
                var presetation = db.GetCRM_PresentationById(this.PresentationId.Value);
                if (presetation.PresentationStageId != this.PresentationStageId)
                {
                    var oldStage = db.GetCRM_ManagerStageById(presetation.PresentationStageId.Value);
                    var newstage = db.GetCRM_ManagerStageById(this.PresentationStageId.Value);
                    if (oldStage != null && newstage != null)
                    {
                        presetation.changedby = this.changedby;
                        presetation.changed = DateTime.Now;
                        presetation.PresentationStageId = this.PresentationStageId;
                        dbresult &= db.UpdateCRM_Presentation(presetation, false, trans);
                        dbresult &= db.InsertCRM_PresentationAction(new CRM_PresentationAction
                        {
                            created = DateTime.Now,
                            createdby = this.createdby,
                            presentationId = this.PresentationId,
                            color = newstage.color,
                            type = (short)EnumCRM_PresentationActionType.AsamaGüncelleme,
                            description = "Aşama Güncellendi.  " + oldStage.Name + " => " + newstage.Name
                        }, trans);
                    }
                }
            }

            var IdUsers = Users.Select(x => x.UserId).ToArray();

            var oldPresentation = db.GetVWCRM_PresentationById(this.PresentationId.Value);
            string urlMail = TenantConfig.Tenant.GetWebUrl();
            var tenantName = TenantConfig.Tenant.TenantName;
            string customerText = "";
            string channelText = "";
            string ownerText = "";
            var listMyPersonMails = new List<string>();
            var listOtherPersonMails = new List<string>();
            if (IdUsers != null && IdUsers.Count() > 0 && oldPresentation != null)
            {
                if (oldPresentation.ChannelCompanyId.HasValue)
                {
                    var channelUsers = db.GetVWSH_UserByCompanyIdAndUserIds(oldPresentation.ChannelCompanyId.Value, IdUsers);
                    channelText = string.Join(", ", channelUsers.Select(t => t.FullName).ToArray());
                    if (mailForParticipants)
                    {
                        var mail = channelUsers.Select(x => x.email).ToList();
                        if (mail != null) { listOtherPersonMails.AddRange(mail); };
                    }
                }
                if (oldPresentation.CustomerCompanyId.HasValue)
                {
                    var customerUsers = db.GetVWSH_UserByCompanyIdAndUserIds(oldPresentation.CustomerCompanyId.Value, IdUsers);
                    customerText = string.Join(", ", customerUsers.Select(t => t.FullName).ToArray());
                    if (mailForParticipants)
                    {
                        var mail = customerUsers.Select(x => x.email).ToList();
                        if (mail != null) { listOtherPersonMails.AddRange(mail); };
                    }
                }
                var owner = db.GetVWSH_UserMyPersonByIds(IdUsers);
                ownerText = string.Join(", ", owner.Select(t => t.FullName).ToArray());
                listMyPersonMails.AddRange(owner.Select(x => x.email).ToList());
                var salesPerson1 = db.GetVWSH_UserById(oldPresentation.SalesPersonId.Value);
                if (salesPerson1 != null)
                {
                    if (salesPerson1.Manager1.HasValue)
                    {
                        var manager = db.GetVWSH_UserById(salesPerson1.Manager1.Value);
                        if (manager.email != null) { listMyPersonMails.Add(manager.email); };

                    }
                }
            }
            var myPerson = string.Join(";", listMyPersonMails.ToArray());
            var otherPerson = string.Join(";", listOtherPersonMails.ToArray());
            string txtKatil = " katılacaktır.";
            switch ((EnumCRM_ContactContactStatus)this.ContactStatus)
            {
                case EnumCRM_ContactContactStatus.ToplantiPlanlandi:
                    txtKatil = " katılacaktır.";
                    break;
                case EnumCRM_ContactContactStatus.ToplantiGerceklesti:
                    txtKatil = " katıldı.";
                    break;
                case EnumCRM_ContactContactStatus.ToplantiIptal:
                    txtKatil = " yer alıyordu.";
                    break;
            }

            if (!string.IsNullOrEmpty(myPerson))
            {
                var text = "<h3>Merhaba, </h3>";
                text += "<p>{0} firmasına açılan potansiyel randevusu hakkında bilgilendirme; </p>";
                text += "<p>{1} {7} ile {2} {8} tarihleri arasında {13} olan {6}</p>";
                text += "<p>Firmamızdan {9}</p>";
                if (channelText != "")
                {
                    text += "<p>Yanımızda {3}</p>";
                }
                if (customerText != "")
                {
                    text += "<p>Müşteri tarafında {10}</p>";
                }
                text += "<p>Randevu hakkında : {11} </p>";
                text += "<p>Potansiyelin en son aşaması : {12} </p>";
                text += "<p>Görüşme detaylarına gitmek için lütfen <a href='{4}/CRM/VWCRM_Presentation/Detail?id={5}'> Buraya tıklayınız! </a></p>";
                text += "<p> Bilgilerinize. </p>";

                var mesaj = string.Format(text,
                        oldPresentation.CustomerCompany_Title,
                        string.Format("{0:dd.MM.yyyy}", this.ContactStartDate),
                        string.Format("{0:dd.MM.yyyy}", this.ContactEndDate),
                        channelText != null ? channelText + " " + txtKatil : "",
                        urlMail,
                        oldPresentation.id,
                        Infoline.Helper.EnumsProperties.GetDescriptionFromEnumValue((EnumCRM_ContactContactStatus)this.ContactStatus),
                        (this.ContactStartDate.Value.Hour < 10 ? "0" + this.ContactStartDate.Value.Hour : this.ContactStartDate.Value.Hour.ToString())
                        + ":" + (this.ContactStartDate.Value.Minute < 10 ? "0" + this.ContactStartDate.Value.Minute : this.ContactStartDate.Value.Minute.ToString()),
                        (this.ContactEndDate.Value.Hour < 10 ? "0" + this.ContactEndDate.Value.Hour : this.ContactEndDate.Value.Hour.ToString())
                        + ":" + (this.ContactEndDate.Value.Minute < 10 ? "0" + this.ContactEndDate.Value.Minute : this.ContactEndDate.Value.Minute.ToString()),
                        ownerText != null ? ownerText + " " + txtKatil : "",
                        customerText != null ? customerText + " " + txtKatil : "",
                        (this.Description != null ? this.Description : " - "),
                        oldPresentation.Stage_Title,
                         this.ContactType == (int)EnumCRM_ContactContactType.Diger ? "randevu tipi belirtilmemiş" : Infoline.Helper.EnumsProperties.GetDescriptionFromEnumValue((EnumCRM_ContactContactType)this.ContactType)
                        );
                new Email().Template("Template1", "potansiyel.jpg", "Yeni Potansiyel Randevusu Hakkında", mesaj)
                 .Send((Int16)EmailSendTypes.Toplanti, myPerson, string.Format("{0} | {1}", tenantName + " | WORKOFTIME", "Yeni Potansiyel Randevusu Hakkında"), true);
            }
            if (mailForParticipants && customerText != "")
            {
                var participants = string.Join(";", db.GetSH_UserByIds(IdUsers).Where(x => x.type == (int)EnumCMP_CompanyType.Diger).Select(c => c.email));
                if (participants != null && participants != "")
                {
                    var textPart = "<h3>Merhaba, </h3>";
                    textPart += "<p>{0} firmanızla olan toplantımız hakkında bilgilendirme; </p>";
                    textPart += "<p>{1} {7} ile {2} {8} tarihleri arasında {12} olan {6}</p>";
                    textPart += "<p>Firmamızdan {9}</p>";
                    if (channelText != "")
                    {
                        textPart += "<p>Yanımızda {3}</p>";
                    }
                    textPart += "<p>Tarafınızdan {10}</p>";
                    textPart += "<p>Randevu hakkında : {11} </p>";
                    textPart += "<p>Görüşme detaylarına gitmek için lütfen <a href='{4}/CRM/VWCRM_Presentation/Detail?id={5}'> Buraya      tıklayınız! </a></p>";
                    textPart += "<p> Bilgilerinize. </p>";

                    var message = string.Format(textPart,
                                    oldPresentation.CustomerCompany_Title,
                                    string.Format("{0:dd.MM.yyyy}", this.ContactStartDate),
                                    string.Format("{0:dd.MM.yyyy}", this.ContactEndDate),
                                    channelText + " " + txtKatil,
                                    urlMail,
                                    oldPresentation.id,
                                    Infoline.Helper.EnumsProperties.GetDescriptionFromEnumValue((EnumCRM_ContactContactStatus)this.ContactStatus),
                                    (this.ContactStartDate.Value.Hour < 10 ? "0" + this.ContactStartDate.Value.Hour : this.ContactStartDate.Value.Hour.ToString())
                                    + ":" + (this.ContactStartDate.Value.Minute < 10 ? "0" + this.ContactStartDate.Value.Minute : this.ContactStartDate.Value.Minute.ToString()),
                                    (this.ContactEndDate.Value.Hour < 10 ? "0" + this.ContactEndDate.Value.Hour : this.ContactEndDate.Value.Hour.ToString())
                                    + ":" + (this.ContactEndDate.Value.Minute < 10 ? "0" + this.ContactEndDate.Value.Minute : this.ContactEndDate.Value.Minute.ToString()),
                                    ownerText,
                                    customerText,
                                    (this.Description != null ? this.Description : " - "),
                                     this.ContactType == (int)EnumCRM_ContactContactType.Diger ? "randevu tipi belirtilmemiş" : Infoline.Helper.EnumsProperties.GetDescriptionFromEnumValue((EnumCRM_ContactContactType)this.ContactType));
                    new Email().Template("Template1", "104.jpg", "Randevu Hakkında", message)
                     .Send((Int16)EmailSendTypes.Toplanti, otherPerson, string.Format("{0} | {1}", tenantName + " | WORKOFTIME", "Yeni Potansiyel Randevusu Hakkında"), true);
                }
            }


            if (!dbresult.result)
            {
                if (trans == null) transaction.Rollback();

                return new ResultStatus
                {
                    result = false,
                    message = "Aktivite/Randevu tanımlama işlemi başarısız oldu."
                };
            }
            else
            {
                return new ResultStatus
                {
                    result = true,
                    message = "Aktivite/Randevu tanımlama işlemi başarılı bir şekilde gerçekleştirildi."
                };
            }
        }

        private ResultStatus Update(DbTransaction trans = null)
        {
            var feedback = new FeedBack();
            var db = new WorkOfTimeDatabase();
            var IdUsers = Users.Select(x => x.UserId).ToArray();

            if (!this.PresentationId.HasValue)
            {
                return new ResultStatus
                {
                    result = false,
                    message = "Bağlı potansiyel fırsat bulunamadı."
                };
            }

            this.changed = DateTime.Now;
            this.changedby = this.createdby;


            var oldContact = db.GetCRM_ContactById(this.id);
            var contactUsers = db.GetCRM_ContactUserByContactId(this.id);
            var itemNew = new CRM_Contact().B_EntityDataCopyForMaterial(this, true);
            var stringList = new List<String>();

            foreach (var prop in itemNew.GetType().GetProperties())
            {
                if (prop.Name == "created" || prop.Name == "createdby" || prop.Name == "changed" || prop.Name == "changedby") { }
                else
                {
                    if (prop.GetValue(itemNew) == null || prop.GetValue(oldContact) == null)
                    {
                        if (prop.GetValue(itemNew) != null)
                        {
                            stringList.Add(prop.Name);
                        }
                        if (prop.GetValue(oldContact) != null)
                        {
                            stringList.Add(prop.Name);
                        }
                    }

                    if (prop.GetValue(itemNew) != null && prop.GetValue(oldContact) != null)
                    {
                        if (prop.GetValue(itemNew).ToString() != prop.GetValue(oldContact).ToString())
                        {
                            stringList.Add(prop.Name);
                        }
                    }
                }
            }



            var _trans = db.BeginTransaction();
            var dbRes = db.UpdateCRM_Contact(new CRM_Contact().B_EntityDataCopyForMaterial(this, true), false, trans);

            var statusDescription = Helper.EnumsProperties.GetDescriptionFromEnumValue((EnumCRM_ContactContactStatus)this.ContactStatus);

            dbRes &= db.InsertCRM_PresentationAction(new CRM_PresentationAction
            {
                created = DateTime.Now,
                createdby = this.createdby,
                description = "Aktivite/Randevu düzenlendi. (" + statusDescription + ")",
                presentationId = this.PresentationId.Value,
                contactId = this.id,
                type = (short)EnumCRM_PresentationActionType.AktiviteDüzenle,
                location = this.location
            }, trans);

            var stageId = this.PresentationStageId;
            var oldPresentation = db.GetCRM_PresentationById(this.PresentationId.Value);

            if (oldPresentation != null && this.PresentationStageId != null && oldPresentation.PresentationStageId != this.PresentationStageId)
            {
                var newStage = db.GetCRM_ManagerStageById(stageId.Value);
                var oldState = db.GetCRM_ManagerStageById(oldPresentation.PresentationStageId.Value);
                dbRes &= db.InsertCRM_PresentationAction(new CRM_PresentationAction
                {
                    created = DateTime.Now,
                    createdby = this.createdby,
                    presentationId = this.PresentationId.Value,
                    color = newStage != null ? newStage.color : "",
                    type = (short)EnumCRM_PresentationActionType.AsamaGüncelleme,
                    description = "Yeni aşama : " + newStage.Name + " ||  Eski aşama :  " + oldState.Name
                }, trans);
                oldPresentation.PresentationStageId = this.PresentationStageId;
                oldPresentation.changedby = this.changedby;
                oldPresentation.changed = DateTime.Now;
                dbRes &= db.UpdateCRM_Presentation(oldPresentation, false, trans);
            }


            dbRes &= db.BulkDeleteCRM_ContactUser(contactUsers, trans);
            dbRes &= db.BulkInsertCRM_ContactUser(IdUsers.Select(a => new CRM_ContactUser
            {
                id = Guid.NewGuid(),
                created = DateTime.Now,
                createdby = this.createdby,
                UserId = a,
                ContactId = this.id,
                UserType = GetUserByType(a)
            }), trans);

            dbRes &= db.InsertCRM_ContactAction(new CRM_ContactAction
            {
                created = DateTime.Now,
                createdby = this.createdby,
                description = "Aktivite/Randevu düzenlendi. (" + statusDescription + ")",
                ContactId = this.id,
                location = this.location
            }, trans);


            if (dbRes.result == false)
            {
                trans.Rollback();

                return new ResultStatus
                {
                    result = false,
                    message = "Aktivite/Randevu güncelleme işlemi başarısız oldu."
                };
            }

            try
            {


                string urlMail = TenantConfig.Tenant.GetWebUrl();
                var tenantName = TenantConfig.Tenant.TenantName;

                if (this.PresentationId != null)
                {
                    var _presentation = db.GetCRM_PresentationById(this.PresentationId.Value);
                    if (_presentation.SalesPersonId != null)
                    {
                        var _salesUser = db.GetSH_UserById(_presentation.SalesPersonId.Value);
                        if (this.changedby != null)
                        {
                            var _changePerson = db.GetSH_UserById(this.changedby.Value);

                            if (_presentation != null && _salesUser != null && _changePerson != null)
                            {
                                var compPersonDept = db.GetVWSH_UserById(_salesUser.id);
                                var list = new List<Guid>();
                                if (compPersonDept != null)
                                {
                                    if (compPersonDept.Manager1.HasValue)
                                    {
                                        list.Add(compPersonDept.Manager1.Value);
                                    }
                                    list.Add(compPersonDept.id);
                                }
                                var users = string.Join(";", db.GetSH_UserByIds(list.ToArray()).Select(c => c.email));
                                if (mailForParticipants)
                                {
                                    users += string.Join(";", db.GetSH_UserByIds(IdUsers).Where(x => x.type == (int)EnumCMP_CompanyType.Diger).Select(c => c.email));
                                    var a = db.GetSH_UserByIds(IdUsers);
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
            }
            return new ResultStatus
            {
                result = true,
                message = "Aktivite/Randevu güncelleme işlemi başarı ile gerçekleşti."
            };

        }

        public ResultStatus Delete(DbTransaction trans = null)
        {
            var db = new WorkOfTimeDatabase();
            var item = db.GetCRM_ContactById(this.id);
            var _contactUser = db.GetCRM_ContactUserByContactId(this.id);
            var _contactAction = db.GetCRM_ContactActionByContactId(this.id);
            var _file = db.GetSYS_FilesByDataIdArray(this.id);

            var _trans = db.BeginTransaction();
            var dbresult = db.DeleteCRM_Contact(item, trans);
            dbresult &= db.BulkDeleteCRM_ContactUser(_contactUser, trans);
            dbresult &= db.BulkDeleteCRM_ContactAction(_contactAction, trans);
            dbresult &= db.BulkDeleteSYS_Files(_file, trans);

            if (dbresult.result == false)
            {
                _trans.Rollback();

                return new ResultStatus
                {
                    result = false,
                    message = "Aktivite/Randevu silme işlemi başarısız oldu."
                };
            }

            _trans.Commit();

            return new ResultStatus
            {
                result = true,
                message = "Aktivite/Randevu silme işlemi başarı ile gerçekleşti."
            };
        }

        public List<Participants> GetSH_UsersForPresentationById(Guid id)
        {
            var result = new List<Participants>();

            var db = new WorkOfTimeDatabase();
            var companies = db.GetVWCMP_CompanyMyCompanies();
            var customerCompanies = db.GetVWCMP_CompanyOtherCompanies();
            var companyIds = companies.Select(a => a.id).ToArray();
            var _presentation = db.GetCRM_PresentationById(id);

            if (companies.Count() > 0)
            {
                result.AddRange(db.GetVWSH_UserByCompanyIds(companyIds).Select(a => new Participants
                {
                    UserId = a.id,
                    UserName = a.FullName,
                    Type = (int)EnumCRM_ContactUserUserType.OwnerUser
                }));
            }
            if(customerCompanies.Count() > 0)
            {
                result.AddRange(db.GetVWSH_UserByCompanyIds(customerCompanies.Select(x => x.id).ToArray()).Select(a => new Participants
                {
                    UserId = a.id,
                    UserName = a.FullName,
                    Type = (int)EnumCRM_ContactUserUserType.CustomerUser
                }));
            }

            if (_presentation != null)
            {
                if (_presentation.CustomerCompanyId.HasValue)
                {
                    result.AddRange(db.GetVWSH_UserByCompanyId(_presentation.CustomerCompanyId.Value).Select(a => new Participants
                    {
                        UserId = a.id,
                        UserName = a.FullName,
                        Type = (int)EnumCRM_ContactUserUserType.CustomerUser
                    }));
                }

                if (_presentation.ChannelCompanyId.HasValue)
                {
                    result.AddRange(db.GetVWSH_UserByCompanyId(_presentation.ChannelCompanyId.Value).Select(a => new Participants
                    {
                        UserId = a.id,
                        UserName = a.FullName,
                        Type = (int)EnumCRM_ContactUserUserType.ChannelUser
                    }));
                }
            }
            return result.OrderBy(a => a.UserName).ToList();

        }

        public ResultStatus ParticipantsInsert()
        {
            var db = new WorkOfTimeDatabase();

            var presentation = db.GetCRM_PresentationById(this.PresentationId.Value);

            foreach (var userData in this.Users)
            {
                string firstName = "";
                string lastName = "";
                if (userData.FullName != "")
                {
                    var nameCount = userData.FullName.Split();
                    if (nameCount.Count() > 0)
                    {
                        if (nameCount.Count() == 3)
                        {
                            firstName = nameCount[0];
                            lastName = nameCount[1] + " " + nameCount[2];
                        }
                        else if (nameCount.Count() == 2)
                        {
                            firstName = nameCount[0];
                            lastName = nameCount[1];
                        }
                        else if (nameCount.Count() == 1)
                        {
                            firstName = nameCount[0];
                        }
                        else
                        {
                            firstName = userData.FullName;
                        }
                    }
                }


                var user = new SH_User
                {
                    id = userData.UserId,
                    createdby = this.createdby,
                    firstname = firstName,
                    lastname = lastName,
                    code = BusinessExtensions.B_GetIdCode(),
                    type = (int)EnumSH_UserType.OtherPerson,
                    loginname = firstName,
                    password = db.GetMd5Hash(db.GetMd5Hash("123456")),
                    status = false
                };
                var customerCompanyId = "";
                if (presentation == null)
                {
                    customerCompanyId = this.PresentationId.Value.ToString();
                }  else
                {
                    customerCompanyId = presentation.CustomerCompanyId.ToString();
                }
                var compPerson = new INV_CompanyPerson
                {
                    createdby = this.createdby,
                    IdUser = user.id,
                    CompanyId = new Guid(customerCompanyId),
                    JobStartDate = DateTime.Now,
                    Title = "CRM",
                    Level = 1
                };

                var trans = db.BeginTransaction();

                var dbres = db.InsertSH_User(user, trans);
                dbres &= db.InsertINV_CompanyPerson(compPerson, trans);

                if (dbres.result == true)
                {
                    trans.Commit();
                }
                else
                {
                    trans.Rollback();
                }
            }
            return new ResultStatus
            {
                result = true,
                message = "Aktivite/Randevu Katılımcı Ekleme Başarılı."
            };
        }


        public int GetUserByType(Guid userId)
        {
            var db = new WorkOfTimeDatabase();
            var _shuser = db.GetVWSH_UserById(userId);
            return _shuser.type.HasValue ? _shuser.type.Value : (int)EnumSH_UserType.MyPerson;

        }
    }


    public  class CRM_ContactReports
    {
        public int planned { get; set; }
        public int happening { get; set; }
        public int canceled { get; set; }
        public int unplannedHappening { get; set; }
        public int Total { get; set; }
        public string createdByTitle { get; set; }
    }

    public class CRM_ContactContactTypeReports
    {
        public int faceToFace { get; set; }
        public int phone { get; set; }
        public int videoConferencing { get; set; }
        public int written { get; set; }
        public int food { get; set; }
        public int other { get; set; }
        public int Totals { get; set; }
        public string createdByTitleForContactType { get; set; }
    }

}
