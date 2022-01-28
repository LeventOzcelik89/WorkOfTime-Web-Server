using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.Infrastructure.Implementation;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Infoline.WorkOfTime.WebProject.Areas.CRM.Controllers
{
    public class VWCRM_ContactController : Controller
    {
        [PageInfo("Aktivite ve Randevular", SHRoles.CRMYonetici, SHRoles.SatisPersoneli)]
        public ActionResult Index(string ids)
        {
            var _ids = new List<Guid>();
            if (!String.IsNullOrEmpty(ids))
            {
                _ids = ids.Split(',').Select(a => new Guid(a)).ToList();
            }
            return View(_ids);
        }


        [PageInfo("Aktivite/Randevu Methodu", SHRoles.CRMYonetici, SHRoles.SatisPersoneli, SHRoles.CRMBayiPersoneli, SHRoles.CagriMerkezi)]
        public ContentResult DataSource([DataSourceRequest] DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);
            request.Page = 1;
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWCRM_Contact(condition).RemoveGeographies().ToDataSourceResult(request);
            data.Total = db.GetVWCRM_ContactCount(condition.Filter);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }

        [AllowEveryone]
        [PageInfo("Toplantı İlgililer Grid Metodu")]
        public ContentResult DataSourceDropDownForRelations([DataSourceRequest] DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWCRM_ContactRelationTables(condition);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }


        [PageInfo("Aktivite/Randevu Veri Methodu", SHRoles.Personel)]
        public int DataSourceCount([DataSourceRequest] DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);
            var db = new WorkOfTimeDatabase();
            var count = db.GetVWCRM_ContactCount(condition.Filter);
            return count;
        }

        [PageInfo("Aktivite/Randevu Methodu", SHRoles.CRMYonetici, SHRoles.SatisPersoneli)]
        public ContentResult DataSourceUser([DataSourceRequest] DataSourceRequest request)
        {
            request = UpdateRequest(request);
            var condition = KendoToExpression.Convert(request);
            request.Page = 1;
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWCRM_ContactUser(condition).RemoveGeographies().ToDataSourceResult(request);
            data.Total = db.GetVWCRM_ContactUserCount(condition.Filter);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }


        [PageInfo("Aktivite/Randevu Methodu", SHRoles.CRMYonetici, SHRoles.SatisPersoneli)]
        public int DataSourceUserCount([DataSourceRequest] DataSourceRequest request)
        {
            request = UpdateRequest(request);
            var condition = KendoToExpression.Convert(request);
            var db = new WorkOfTimeDatabase();
            var count = db.GetVWCRM_ContactUserCount(condition.Filter);
            return count;
        }


        [PageInfo("Aktivite/Randevu Veri Methodu", SHRoles.Personel)]
        public ContentResult DataSourceDropDown([DataSourceRequest] DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);

            var db = new WorkOfTimeDatabase();
            var data = db.GetVWCRM_Contact(condition);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }



        [PageInfo("Aktivite/Randevu Detayı", SHRoles.CRMYonetici, SHRoles.SatisPersoneli, SHRoles.CRMBayiPersoneli)]
        public ActionResult Detail(Guid id)
        {
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWCRM_ContactById(id);
            ViewBag.IdUsers = db.GetCRM_ContactUserByContactId(data.id).Select(c => c.UserId).ToArray();

            return View(data);
        }




        [PageInfo("Aktivite/Randevu Ekleme", SHRoles.SatisPersoneli, SHRoles.CRMYonetici, SHRoles.CRMBayiPersoneli)]
        public ActionResult Insert(VWCRM_Contact item, DateTime? date, bool all = false)
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var list = new List<Guid?> { userStatus.user.id };

            if (item.PresentationId.HasValue)
            {
                var pre = db.GetVWCRM_PresentationById(item.PresentationId.Value);
                if (pre != null)
                {
                    list.Add(pre.SalesPersonId);
                    item.ChannelCompanyId = pre.ChannelCompanyId;
                    item.PresentationStageId = pre.PresentationStageId;
                }
            }

            if (date.HasValue)
            {
                item.ContactStartDate = date;
            }
            ViewBag.SalesPersonId = list.Distinct().Select(a => a.Value);
            ViewBag.All = all;

            return View(item);
        }

        [PageInfo("Aktivite/Randevu Ekleme", SHRoles.SatisPersoneli, SHRoles.CRMYonetici, SHRoles.CRMBayiPersoneli)]
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Insert(VWCRM_Contact item, Guid[] IdUsers, DateTime? AppointmentDate, bool mailForParticipants, Guid? RelationId)
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            IdUsers = IdUsers ?? new Guid[] { };
            item.created = DateTime.Now;
            item.createdby = userStatus.user.id;

            if (!RelationId.HasValue)
            {
                if (item.PresentationId == null)
                {
                    return Json(new ResultStatusUI
                    {
                        Result = false,
                        FeedBack = feedback.Warning("Bağlı potansiyel fırsat bulunamadı.")
                    }, JsonRequestBehavior.AllowGet);
                }
            }

            if (RelationId.HasValue)
            {
                var cmp = db.GetCMP_CompanyById(RelationId.Value);
                if (cmp != null)
                {
                    item.customerId = RelationId;
                }
                else
                {
                    item.PresentationId = RelationId;
                }
            }


            var trans = db.BeginTransaction();
            var dbresult = db.InsertCRM_Contact(new CRM_Contact().EntityDataCopyForMaterial(item, true), trans);

            if (item.PresentationId.HasValue)
            {
                dbresult &= db.InsertCRM_PresentationAction(new CRM_PresentationAction
                {
                    created = DateTime.Now,
                    createdby = userStatus.user.id,
                    description = "Yeni aktivite/randevu eklendi.",
                    presentationId = item.PresentationId.Value,
                    type = (short)EnumCRM_PresentationActionType.YeniAktivite,
                    contactId = item.id,
                }, trans);
            }
            dbresult &= db.BulkInsertCRM_ContactUser(IdUsers.Select(a => new CRM_ContactUser
            {
                id = Guid.NewGuid(),
                created = DateTime.Now,
                createdby = userStatus.user.id,
                UserId = a,
                ContactId = item.id,
                UserType = GetUserByType(a)
            }), trans);

            if (item.PresentationStageId != null && item.PresentationId.HasValue)
            {
                var presetation = db.GetCRM_PresentationById(item.PresentationId.Value);
                if (presetation.PresentationStageId != item.PresentationStageId)
                {
                    var oldStage = db.GetCRM_ManagerStageById(presetation.PresentationStageId.Value);
                    var newstage = db.GetCRM_ManagerStageById(item.PresentationStageId.Value);
                    if (oldStage != null && newstage != null)
                    {
                        presetation.changedby = userStatus.user.id;
                        presetation.changed = DateTime.Now;
                        presetation.PresentationStageId = item.PresentationStageId;
                        dbresult &= db.UpdateCRM_Presentation(presetation, false, trans);
                        dbresult &= db.InsertCRM_PresentationAction(new CRM_PresentationAction
                        {
                            created = DateTime.Now,
                            createdby = userStatus.user.id,
                            presentationId = item.PresentationId,
                            color = newstage.color,
                            type = (short)EnumCRM_PresentationActionType.AsamaGüncelleme,
                            description = "Aşama Güncellendi.  " + oldStage.Name + " => " + newstage.Name
                        }, trans);
                    }
                }
            }

            var hata = "";
            if (dbresult.result == false)
            {
                trans.Rollback();
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Warning("Aktivite/Randevu tanımlama işlemi başarısız oldu.")
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                new FileUploadSave(Request).SaveAs();
                trans.Commit();
            }

            try
            {


                var oldPresentation = db.GetVWCRM_PresentationById(item.PresentationId.Value);
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
                switch ((EnumCRM_ContactContactStatus)item.ContactStatus)
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
                            string.Format("{0:dd.MM.yyyy}", item.ContactStartDate),
                            string.Format("{0:dd.MM.yyyy}", item.ContactEndDate),
                            channelText != null ? channelText + " " + txtKatil : "",
                            urlMail,
                            oldPresentation.id,
                            Infoline.Helper.EnumsProperties.GetDescriptionFromEnumValue((EnumCRM_ContactContactStatus)item.ContactStatus),
                            (item.ContactStartDate.Value.Hour < 10 ? "0" + item.ContactStartDate.Value.Hour : item.ContactStartDate.Value.Hour.ToString())
                            + ":" + (item.ContactStartDate.Value.Minute < 10 ? "0" + item.ContactStartDate.Value.Minute : item.ContactStartDate.Value.Minute.ToString()),
                            (item.ContactEndDate.Value.Hour < 10 ? "0" + item.ContactEndDate.Value.Hour : item.ContactEndDate.Value.Hour.ToString())
                            + ":" + (item.ContactEndDate.Value.Minute < 10 ? "0" + item.ContactEndDate.Value.Minute : item.ContactEndDate.Value.Minute.ToString()),
                            ownerText != null ? ownerText + " " + txtKatil : "",
                            customerText != null ? customerText + " " + txtKatil : "",
                            (item.Description != null ? item.Description : " - "),
                            oldPresentation.Stage_Title,
                             item.ContactType == (int)EnumCRM_ContactContactType.Diger ? "randevu tipi belirtilmemiş" : Infoline.Helper.EnumsProperties.GetDescriptionFromEnumValue((EnumCRM_ContactContactType)item.ContactType)
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
                                        string.Format("{0:dd.MM.yyyy}", item.ContactStartDate),
                                        string.Format("{0:dd.MM.yyyy}", item.ContactEndDate),
                                        channelText + " " + txtKatil,
                                        urlMail,
                                        oldPresentation.id,
                                        Infoline.Helper.EnumsProperties.GetDescriptionFromEnumValue((EnumCRM_ContactContactStatus)item.ContactStatus),
                                        (item.ContactStartDate.Value.Hour < 10 ? "0" + item.ContactStartDate.Value.Hour : item.ContactStartDate.Value.Hour.ToString())
                                        + ":" + (item.ContactStartDate.Value.Minute < 10 ? "0" + item.ContactStartDate.Value.Minute : item.ContactStartDate.Value.Minute.ToString()),
                                        (item.ContactEndDate.Value.Hour < 10 ? "0" + item.ContactEndDate.Value.Hour : item.ContactEndDate.Value.Hour.ToString())
                                        + ":" + (item.ContactEndDate.Value.Minute < 10 ? "0" + item.ContactEndDate.Value.Minute : item.ContactEndDate.Value.Minute.ToString()),
                                        ownerText,
                                        customerText,
                                        (item.Description != null ? item.Description : " - "),
                                         item.ContactType == (int)EnumCRM_ContactContactType.Diger ? "randevu tipi belirtilmemiş" : Infoline.Helper.EnumsProperties.GetDescriptionFromEnumValue((EnumCRM_ContactContactType)item.ContactType));
                        new Email().Template("Template1", "104.jpg", "Randevu Hakkında", message)
                         .Send((Int16)EmailSendTypes.Toplanti, otherPerson, string.Format("{0} | {1}", tenantName + " | WORKOFTIME", "Yeni Potansiyel Randevusu Hakkında"), true);
                    }
                }
            }
            catch (Exception ex)
            {
                new Email().Send((Int16)EmailSendTypes.Toplanti, "ahmet.undemir@infoline-tr.com", "Toplantı Hata", ex.Message);
            }
            return Json(new ResultStatusUI
            {
                Result = true,
                Object = Helper.Json.Serialize(item),
                FeedBack = feedback.Success("Aktivite/Randevu tanımlama işlemi başarı ile gerçekleşti. " + hata, false)
            }, JsonRequestBehavior.AllowGet);
        }


        [PageInfo("Aktivite/Randevu Güncelleme", SHRoles.SatisPersoneli, SHRoles.CRMYonetici, SHRoles.CRMBayiPersoneli)]
        public ActionResult Update(Guid id)
        {
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWCRM_ContactById(id);
            ViewBag.IdUsers = db.GetCRM_ContactUserByContactId(data.id).Select(c => c.UserId).ToArray();
            return View(data);
        }

        [PageInfo("Aktivite/Randevu Güncelleme Methodu", SHRoles.SatisPersoneli, SHRoles.CRMYonetici, SHRoles.CRMBayiPersoneli)]
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Update(VWCRM_Contact item, Guid[] IdUsers, bool mailForParticipants)
        {
            var feedback = new FeedBack();
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];

            if (!item.PresentationId.HasValue)
            {
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Warning("Bağlı potansiyel fırsat bulunamadı.")
                }, JsonRequestBehavior.AllowGet);
            }

            item.changed = DateTime.Now;
            item.changedby = userStatus.user.id;


            var oldContact = db.GetCRM_ContactById(item.id);
            var contactUsers = db.GetCRM_ContactUserByContactId(item.id);
            var itemNew = new CRM_Contact().EntityDataCopyForMaterial(item, true);
            var stringList = new List<String>();
            var newDictionary = new CRM_Model().CrmContactDictionary.ToList();

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

            var changes = new List<String>();
            foreach (var stList in stringList)
            {
                changes.Add(newDictionary.Where(c => c.Key == stList).Select(c => c.Value).FirstOrDefault());
            }

            var changeUser = contactUsers.Where(c => c.UserId.HasValue && !IdUsers.Contains(c.UserId.Value)).ToArray();
            if (changeUser.Count() > 0)
            {
                changes.Add("Katılımcılar");
            }



            var trans = db.BeginTransaction();
            var dbRes = db.UpdateCRM_Contact(new CRM_Contact().EntityDataCopyForMaterial(item, true), false, trans);

            var statusDescription = Helper.EnumsProperties.GetDescriptionFromEnumValue((EnumCRM_ContactContactStatus)item.ContactStatus);

            dbRes &= db.InsertCRM_PresentationAction(new CRM_PresentationAction
            {
                created = DateTime.Now,
                createdby = userStatus.user.id,
                description = "Aktivite/Randevu düzenlendi. (" + statusDescription + ")",
                presentationId = item.PresentationId.Value,
                contactId = item.id,
                type = (short)EnumCRM_PresentationActionType.AktiviteDüzenle,
            }, trans);

            var stageId = Request["PresentationStageId"].ToGuid();
            var oldPresentation = db.GetCRM_PresentationById(item.PresentationId.Value);

            if (oldPresentation != null && item.PresentationStageId != null && oldPresentation.PresentationStageId != item.PresentationStageId)
            {
                var newStage = db.GetCRM_ManagerStageById(stageId.Value);
                var oldState = db.GetCRM_ManagerStageById(oldPresentation.PresentationStageId.Value);
                dbRes &= db.InsertCRM_PresentationAction(new CRM_PresentationAction
                {
                    created = DateTime.Now,
                    createdby = userStatus.user.id,
                    presentationId = item.PresentationId.Value,
                    color = newStage != null ? newStage.color : "",
                    type = (short)EnumCRM_PresentationActionType.AsamaGüncelleme,
                    description = "Yeni aşama : " + newStage.Name + " ||  Eski aşama :  " + oldState.Name
                }, trans);
                oldPresentation.PresentationStageId = item.PresentationStageId;
                oldPresentation.changedby = userStatus.user.id;
                oldPresentation.changed = DateTime.Now;
                dbRes &= db.UpdateCRM_Presentation(oldPresentation, false, trans);
                changes.Add("Potansiyel Aşaması");
            }

            var lastChanges = string.Join(", ", changes);

            dbRes &= db.BulkDeleteCRM_ContactUser(contactUsers, trans);
            dbRes &= db.BulkInsertCRM_ContactUser(IdUsers.Select(a => new CRM_ContactUser
            {
                id = Guid.NewGuid(),
                created = DateTime.Now,
                createdby = userStatus.user.id,
                UserId = a,
                ContactId = item.id,
                UserType = GetUserByType(a)
            }), trans);


            if (dbRes.result == false)
            {
                trans.Rollback();
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Warning("Aktivite/Randevu güncelleme işlemi başarısız oldu.")
                });
            }
            else
            {
                var rsFile = new FileUploadSave(Request).SaveAs();
                trans.Commit();
            }


            try
            {


                string urlMail = TenantConfig.Tenant.GetWebUrl();
                var tenantName = TenantConfig.Tenant.TenantName;

                if (item.PresentationId != null && lastChanges != "")
                {
                    var _presentation = db.GetCRM_PresentationById(item.PresentationId.Value);
                    if (_presentation.SalesPersonId != null)
                    {
                        var _salesUser = db.GetSH_UserById(_presentation.SalesPersonId.Value);
                        if (item.changedby != null)
                        {
                            var _changePerson = db.GetSH_UserById(item.changedby.Value);

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
                                if (lastChanges != "")
                                {
                                    var text = "<h3>Merhaba, </h3>";
                                    text += "<p>{0} potansiyelinde, güncelleme olmuştur.İlgili güncelleme {1} isimli personel tarafından gerçekleştirilmiştir.</p>";
                                    text += "<p>{4} alan(lar)ı güncellenmiştir.</p>";
                                    text += "<p>Potansiyelin detayına gitmek için lütfen <a href='{2}/CRM/VWCRM_Presentation/Detail?id={3}'> Buraya tıklayınız! </a></p>";
                                    text += "<p> Bilgilerinize. </p>";
                                    var mesaj = string.Format(text, _presentation.Name, _changePerson.firstname + " " + _changePerson.lastname, urlMail, _presentation.id, lastChanges);
                                    new Email().Template("Template1", "104.jpg", "Potansiyel Güncellemesi Hakkında", mesaj)
                                     .Send((Int16)EmailSendTypes.Toplanti, users, string.Format("{0} | {1}", tenantName + " | WORKOFTIME", "Randevu Güncellemesi Hakkında"), true);
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
            }
            return Json(new ResultStatusUI
            {
                Result = true,
                Object = Helper.Json.Serialize(item),
                FeedBack = feedback.Success("Aktivite/Randevu güncelleme işlemi başarı ile gerçekleşti.", false, Request.UrlReferrer.AbsolutePath == "/CRM/VWCRM_Contact/Update" ? Url.Action("Index", "VWCRM_Contact", new { area = "CRM" }) : null)
            });

        }


        [HttpPost]
        [PageInfo("Aktivite/Randevu Silme", SHRoles.CRMYonetici)]
        public JsonResult Delete(string[] id)
        {
            var db = new WorkOfTimeDatabase();
            var feedback = new FeedBack();
            var item = id.Select(a => new CRM_Contact { id = new Guid(a) });
            var _contactUser = db.GetCRM_ContactUserByContactId(new Guid(id[0]));
            var _file = db.GetSYS_FilesByDataIdArray(new Guid(id[0]));


            var trans = db.BeginTransaction();
            var dbresult = db.BulkDeleteCRM_Contact(item, trans);
            dbresult &= db.BulkDeleteCRM_ContactUser(_contactUser, trans);
            dbresult &= db.BulkDeleteSYS_Files(_file, trans);

            if (dbresult.result == false)
            {
                trans.Rollback();
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Error("Aktivite/Randevu silme işlemi başarısız oldu.")
                }, JsonRequestBehavior.AllowGet);
            }

            trans.Commit();
            return Json(new ResultStatusUI
            {
                Result = true,
                FeedBack = feedback.Success("Aktivite/Randevu silme işlemi başarı ile gerçekleşti.")
            }, JsonRequestBehavior.AllowGet);
        }


        [PageInfo("Toplantı takvimi", SHRoles.SatisPersoneli, SHRoles.IdariPersonelYonetici)]
        public ActionResult ContactCalendar()
        {
            var model = new VMCRM_ContactModel();
            return View(model);

        }

        [PageInfo("Toplantı takvimi", SHRoles.SatisPersoneli, SHRoles.IdariPersonelYonetici)]
        public ContentResult GetContactCalendarData(Guid? userId)
        {
            var userStatus = (PageSecurity)Session["userStatus"];
            var res = new List<object>();
            var db = new WorkOfTimeDatabase();

            if (userStatus.AuthorizedRoles.Contains(new Guid(SHRoles.SatisPersoneli)))
            {
                if (!userStatus.AuthorizedRoles.Contains(new Guid(SHRoles.SistemYonetici)))
                {
                    if (!userStatus.AuthorizedRoles.Contains(new Guid(SHRoles.IdariPersonelYonetici)))
                    {
                        userId = userStatus.user.id;
                    }
                }
            }

            var data = new VMCRM_ContactModel().CalendarDatas(userId);
            var years = data.Where(x => x.ContactEndDate.HasValue).GroupBy(a => a.ContactEndDate.Value.Year).Select(a => a.Key).OrderBy(a => a).ToArray();

            foreach (var year in years)
            {

                var yearData = data.Where(a => a.ContactEndDate.HasValue && a.ContactEndDate.Value.Year == year).ToArray();

                res.Add(new
                {
                    Year = year,
                    Data = yearData
                });

            }

            return Content(Infoline.Helper.Json.Serialize(new ResultStatus { result = true, objects = res }), "application/json");
        }

        public int GetUserByType(Guid userId)
        {
            var db = new WorkOfTimeDatabase();
            var _shuser = db.GetVWSH_UserById(userId);
            return _shuser.type.HasValue ? _shuser.type.Value : (int)EnumSH_UserType.MyPerson;

        }

        public DataSourceRequest UpdateRequest(DataSourceRequest request)
        {
            var userStatus = (PageSecurity)Session["userStatus"];
            if (userStatus.AuthorizedRoles.Contains(new Guid(SHRoles.SatisPersoneli)) && !userStatus.AuthorizedRoles.Contains(new Guid(SHRoles.CRMYonetici)))
            {
                var composit = new CompositeFilterDescriptor
                {
                    LogicalOperator = FilterCompositionLogicalOperator.Or,
                    FilterDescriptors = new FilterDescriptorCollection
                    {
                        new FilterDescriptor { Value = userStatus.user.id, Operator = FilterOperator.IsEqualTo, Member = "createdby" },
                        new FilterDescriptor { Value = userStatus.user.id, Operator = FilterOperator.IsEqualTo, Member = "UserId" }
                    }
                };
                foreach (var item in userStatus.ChildPersons.Where(a => a.IdUser.HasValue && a.IdUser != userStatus.user.id).GroupBy(a => a.IdUser))
                {
                    composit = new CompositeFilterDescriptor
                    {
                        LogicalOperator = FilterCompositionLogicalOperator.Or,
                        FilterDescriptors = new FilterDescriptorCollection{
                            composit,
                            new FilterDescriptor { Value = userStatus.user.id, Operator = FilterOperator.IsEqualTo, Member = "createdby" },
                        }
                    };
                    composit = new CompositeFilterDescriptor
                    {
                        LogicalOperator = FilterCompositionLogicalOperator.Or,
                        FilterDescriptors = new FilterDescriptorCollection{
                            composit,
                            new FilterDescriptor { Value = userStatus.user.id, Operator = FilterOperator.IsEqualTo, Member = "UserId" },
                        }
                    };
                }
                if (request.Filters != null && request.Filters.Count() > 0)
                {
                    composit = new CompositeFilterDescriptor
                    {
                        LogicalOperator = FilterCompositionLogicalOperator.And,
                        FilterDescriptors = new FilterDescriptorCollection
                        {
                            composit,
                            request.Filters[0]
                        }
                    };
                }
                request.Filters = new List<IFilterDescriptor>() { composit };
            }
            return request;
        }
    }
}
