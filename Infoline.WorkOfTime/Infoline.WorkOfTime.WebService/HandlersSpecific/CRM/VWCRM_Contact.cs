using Infoline.Framework.Database;
using Infoline.Web.SmartHandlers;
using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData;
using Infoline.WorkOfTime.BusinessData.Specific;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web;

namespace Infoline.WorkOfTime.WebService.HandlersSpecific
{
    [Export(typeof(ISmartHandler))]
    public partial class VWCRM_ContactHandler : BaseSmartHandler
    {
        public VWCRM_ContactHandler()
            : base("VWCRM_Contact")
        {

        }

        [HandleFunction("VWCRM_Contact/GetAll")]
        public void VWCRM_ContactGetAll(HttpContext context)
        {
            try
            {
                var c = ParseRequest<Condition>(context);
                var cond = c != null ? CondtionToQuery.Convert(c) : new SimpleQuery();
                var db = new WorkOfTimeDatabase();
                var data = db.GetVWCRM_Contact(cond);
                RenderResponse(context, data);
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }

        public int GetUserByType(Guid userId)
        {
            var db = new WorkOfTimeDatabase();
            var _shuser = db.GetVWSH_UserById(userId);
            return _shuser.type.HasValue ? _shuser.type.Value : (int)EnumSH_UserType.MyPerson;

        }

        [HandleFunction("VWCRM_Contact/Insert")]
        public void VWCRM_ContactInsert(HttpContext context)
        {
            try
            {
                var model = ParseRequest<VMCRM_ContactModel>(context);
                var userId = CallContext.Current.UserId;
                var db = new WorkOfTimeDatabase();
                var feedback = new FeedBack();
                model.created = DateTime.Now;
                model.createdby = CallContext.Current.UserId;

                if (!model.RelationId.HasValue)
                {
                    if (model.PresentationId == null)
                    {
                        RenderResponse(context, new ResultStatus { result = true, message = "Bağlı potansiyel fırsat bulunamadı." });
                        return;
                    }
                }

                if (model.RelationId.HasValue)
                {
                    var cmp = db.GetCMP_CompanyById(model.RelationId.Value);
                    if (cmp != null)
                    {
                        model.customerId = model.RelationId;
                    }
                    else
                    {
                        model.PresentationId = model.RelationId;
                    }
                }


                var trans = db.BeginTransaction();
                var data = db.GetCRM_ContactById(model.id);
                var tempCopy = new CRM_Contact().B_EntityDataCopyForMaterial(model, true);
                var dbresult = data == null ? db.InsertCRM_Contact(tempCopy, trans) : db.UpdateCRM_Contact(tempCopy);

                if (model.PresentationId.HasValue)
                {
                    dbresult &= db.InsertCRM_PresentationAction(new CRM_PresentationAction
                    {
                        created = DateTime.Now,
                        createdby = CallContext.Current.UserId,
                        description = data == null ? "Yeni aktivite/randevu eklendi." : "Aktivite/randevu güncellendi.",
                        presentationId = model.PresentationId.Value,
                        type = data == null ? (short)EnumCRM_PresentationActionType.YeniAktivite : (short)EnumCRM_PresentationActionType.AktiviteDüzenle,
                        contactId = model.id,
                        location = model.location
                        
                    }, trans);
                }
                var conctactUsers = db.GetCRM_ContactUserByContactId(model.id);
                dbresult &= db.BulkDeleteCRM_ContactUser(conctactUsers);
                dbresult &= db.BulkInsertCRM_ContactUser(model.Users.Select(a => new CRM_ContactUser
                {
                    id = Guid.NewGuid(),
                    created = DateTime.Now,
                    createdby = a.UserId,
                    UserId = a.UserId,
                    ContactId = model.id,
                    UserType = GetUserByType(a.UserId)
                }), trans);

                var statusDescription = Helper.EnumsProperties.GetDescriptionFromEnumValue((EnumCRM_ContactContactStatus)model.ContactStatus);
                dbresult &= db.InsertCRM_ContactAction(new CRM_ContactAction
                {
                    created = DateTime.Now,
                    createdby = CallContext.Current.UserId,
                    description = data == null ? "Yeni aktivite/randevu eklendi. (" + statusDescription + ") (" + DateTime.Now + ")" : "Aktivite/Randevu düzenlendi. (" + statusDescription + ") (" + DateTime.Now + ")",
                    ContactId = model.id,
                    location = model.location,
                }, trans);

                if (model.PresentationStageId != null && model.PresentationId.HasValue)
                {
                    var presetation = db.GetCRM_PresentationById(model.PresentationId.Value);
                    if (presetation.PresentationStageId != model.PresentationStageId)
                    {
                        var oldStage = db.GetCRM_ManagerStageById(presetation.PresentationStageId.Value);
                        var newstage = db.GetCRM_ManagerStageById(model.PresentationStageId.Value);
                        if (oldStage != null && newstage != null)
                        {
                            presetation.changedby = CallContext.Current.UserId;
                            presetation.changed = DateTime.Now;
                            presetation.PresentationStageId = model.PresentationStageId;
                            dbresult &= db.UpdateCRM_Presentation(presetation, false, trans);
                            dbresult &= db.InsertCRM_PresentationAction(new CRM_PresentationAction
                            {
                                created = DateTime.Now,
                                createdby = CallContext.Current.UserId,
                                presentationId = model.PresentationId,
                                color = newstage.color,
                                type = (short)EnumCRM_PresentationActionType.AsamaGüncelleme,
                                description = "Aşama Güncellendi.  " + oldStage.Name + " => " + newstage.Name,
                                location = model.location
                            }, trans);
                        }
                    }
                }

                var hata = "";
                if (dbresult.result == false)
                {
                    trans.Rollback();
                    RenderResponse(context, new ResultStatus { message = "Aktivite/Randevu tanımlama işlemi başarısız oldu.", result = false });
                    return;
                }
                else
                {
                    trans.Commit();
                }

                try
                {


                    var oldPresentation = db.GetVWCRM_PresentationById(model.PresentationId.Value);
                    string urlMail = TenantConfig.Tenant.GetWebUrl();
                    var tenantName = TenantConfig.Tenant.TenantName;
                    string customerText = "";
                    string channelText = "";
                    string ownerText = "";
                    var listMyPersonMails = new List<string>();
                    var listOtherPersonMails = new List<string>();
                    if (model.Users != null && model.Users.Count() > 0 && oldPresentation != null)
                    {
                        if (oldPresentation.ChannelCompanyId.HasValue)
                        {
                            var channelUsers = db.GetVWSH_UserByCompanyIdAndUserIds(oldPresentation.ChannelCompanyId.Value, model.Users.Select(x => x.UserId).ToArray());
                            channelText = string.Join(", ", channelUsers.Select(t => t.FullName).ToArray());
                            //if (mailForParticipants)
                            //{
                            //    var mail = channelUsers.Select(x => x.email).ToList();
                            //    if (mail != null) { listOtherPersonMails.AddRange(mail); };
                            //}
                        }
                        if (oldPresentation.CustomerCompanyId.HasValue)
                        {
                            var customerUsers = db.GetVWSH_UserByCompanyIdAndUserIds(oldPresentation.CustomerCompanyId.Value, model.Users.Select(x => x.UserId).ToArray());
                            customerText = string.Join(", ", customerUsers.Select(t => t.FullName).ToArray());
                            //if (mailForParticipants)
                            //{
                            //    var mail = customerUsers.Select(x => x.email).ToList();
                            //    if (mail != null) { listOtherPersonMails.AddRange(mail); };
                            //}
                        }
                        var owner = db.GetVWSH_UserMyPersonByIds(model.Users.Select(x => x.UserId).ToArray());
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
                    switch ((EnumCRM_ContactContactStatus)model.ContactStatus)
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
                                string.Format("{0:dd.MM.yyyy}", model.ContactStartDate),
                                string.Format("{0:dd.MM.yyyy}", model.ContactEndDate),
                                channelText != null ? channelText + " " + txtKatil : "",
                                urlMail,
                                oldPresentation.id,
                                Infoline.Helper.EnumsProperties.GetDescriptionFromEnumValue((EnumCRM_ContactContactStatus)model.ContactStatus),
                                (model.ContactStartDate.Value.Hour < 10 ? "0" + model.ContactStartDate.Value.Hour : model.ContactStartDate.Value.Hour.ToString())
                                + ":" + (model.ContactStartDate.Value.Minute < 10 ? "0" + model.ContactStartDate.Value.Minute : model.ContactStartDate.Value.Minute.ToString()),
                                (model.ContactEndDate.Value.Hour < 10 ? "0" + model.ContactEndDate.Value.Hour : model.ContactEndDate.Value.Hour.ToString())
                                + ":" + (model.ContactEndDate.Value.Minute < 10 ? "0" + model.ContactEndDate.Value.Minute : model.ContactEndDate.Value.Minute.ToString()),
                                ownerText != null ? ownerText + " " + txtKatil : "",
                                customerText != null ? customerText + " " + txtKatil : "",
                                (model.Description != null ? model.Description : " - "),
                                oldPresentation.Stage_Title,
                                 model.ContactType == (int)EnumCRM_ContactContactType.Diger ? "randevu tipi belirtilmemiş" : Infoline.Helper.EnumsProperties.GetDescriptionFromEnumValue((EnumCRM_ContactContactType)model.ContactType)
                                );
                        new Email().Template("Template1", "potansiyel.jpg", "Yeni Potansiyel Randevusu Hakkında", mesaj)
                         .Send((Int16)EmailSendTypes.Toplanti, myPerson, string.Format("{0} | {1}", tenantName + " | WORKOFTIME", "Yeni Potansiyel Randevusu Hakkında"), true);
                    }
                }
                catch (Exception ex)
                {
                    new Email().Send((Int16)EmailSendTypes.Toplanti, "ahmet.undemir@infoline-tr.com", "Toplantı Hata", ex.Message);
                }
                //var rs = model.Save(userId);
                RenderResponse(context, new ResultStatus { result = dbresult.result, message = dbresult.result ? "Toplantı kaydetme işlemi başarılı" : "Toplantı kaydetme işlemi başarısız oldu." });
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }


        [HandleFunction("VWCRM_Contact/ParticipantsInsert")]
        public void VWCRM_ContactParticipantsInsert(HttpContext context)
        {
            try
            {
                var model = ParseRequest<VMCRM_ContactModel>(context);
                var userId = CallContext.Current.UserId;
                var rs = model.ParticipantsInsert();
                RenderResponse(context, rs);
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }

        [HandleFunction("VWCRM_Contact/Update")]
        public void VWCRM_ContactUpdate(HttpContext context)
        {
            try
            {
                var model = ParseRequest<VMCRM_ContactModel>(context);
                var userId = CallContext.Current.UserId;
                var rs = model.Save(userId);
                RenderResponse(context, rs);
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }



        [HandleFunction("VWCRM_Contact/GetById")]
        public void VWCRM_ContactGetById(HttpContext context)
        {
            try
            {
                var db = new WorkOfTimeDatabase();
                var id = context.Request["id"];
                var data = new VMCRM_ContactModel { id = new Guid((string)id) }.Load();
                RenderResponse(context, new ResultStatus
                {
                    result = true,
                    objects = data,
                });
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }

        [HandleFunction("VWCRM_Contact/GetByIdParticipants")]
        public void VWCRM_ContactGetParticipants(HttpContext context)
        {
            try
            {
                var db = new WorkOfTimeDatabase();
                var model = ParseRequest<Participant>(context);
                var data = new VMCRM_ContactModel { id = model.Id }.GetParticipants(model.Name);
                RenderResponse(context, new ResultStatus
                {
                    result = true,
                    objects = data,
                });
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }

        [HandleFunction("VWCRM_Contact/GetCount")]
        public void VWCRM_ContactGetCount(HttpContext context)
        {
            try
            {
                var c = ParseRequest<Condition>(context);
                var cond = c != null ? CondtionToQuery.Convert(c) : new SimpleQuery();
                var db = new WorkOfTimeDatabase();
                var data = db.GetVWCRM_ContactCount(cond.Filter);
                RenderResponse(context, data);
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }

        [HandleFunction("VWCRM_Contact/GetPageInfo")]
        public void VWCRM_ContactGetPageInfo(HttpContext context)
        {
            var now = DateTime.Now;
            var startOfWeek = now.AddDays(((int)(now.DayOfWeek) * -1) + 1).Date;
            var endOfWeek = startOfWeek.AddDays(7).Date;
            var startOfMonth = new DateTime(now.Year, now.Month, 1).Date;
            var endOfMonth = startOfMonth.AddMonths(1).Date;
            var startOfLastMonth = new DateTime(now.Year, now.Month, 1).AddMonths(-1).Date;

            try
            {
                var model = new PageModel { SearchProperty = "searchField" };

                model.Filters.Add(new PageFilter
                {
                    Title = "Aktivite/Randevu Tipine Göre",
                    Items = Infoline.Helper.EnumsProperties.EnumToArrayGeneric<EnumCRM_ContactContactType>().Select(a => new PageFilterItem
                    {
                        Title = a.Value,
                        Filter = new BEXP { Operand1 = (COL)"ContactType", Operator = BinaryOperator.Equal, Operand2 = (VAL)a.Key }.GetSerializeObject(),
                    }).ToList(),
                });

                model.Filters.Add(new PageFilter
                {
                    Title = "Aktivite/Randevu Durumuna Göre",
                    Items = Infoline.Helper.EnumsProperties.EnumToArrayGeneric<EnumCRM_ContactContactStatus>().Select(a => new PageFilterItem
                    {
                        Title = a.Value,
                        Filter = new BEXP { Operand1 = (COL)"ContactStatus", Operator = BinaryOperator.Equal, Operand2 = (VAL)a.Key }.GetSerializeObject(),
                    }).ToList(),
                });

                model.Filters.Add(new PageFilter
                {
                    Title = "Başlangıç Tarihine Göre",
                    Items = new List<PageFilterItem> {
                        new PageFilterItem{
                            Title = "Bugün",
                            Filter = new BEXP { Operand1 = new BEXP { Operand1 = (COL)"ContactStartDate", Operator = BinaryOperator.GreaterThan, Operand2 = (VAL)now.ToString("yyyy-MM-dd") }, Operator = BinaryOperator.And, Operand2 = new BEXP { Operand1 = (COL)"ContactStartDate", Operator = BinaryOperator.LessThan, Operand2 = (VAL)now.AddDays(1).ToString("yyyy-MM-dd") } }.GetSerializeObject(),
                        },
                        new PageFilterItem{
                            Title = "Bu Hafta",
                            Filter = new BEXP { Operand1 = new BEXP { Operand1 = (COL)"ContactStartDate", Operator = BinaryOperator.GreaterThan, Operand2 = (VAL)startOfWeek.ToString("yyyy-MM-dd") }, Operator = BinaryOperator.And, Operand2 = new BEXP { Operand1 = (COL)"ContactStartDate", Operator = BinaryOperator.LessThan, Operand2 = (VAL)endOfWeek.ToString("yyyy-MM-dd") } }.GetSerializeObject(),
                        },
                         new PageFilterItem{
                            Title = "Bu Ay",
                            Filter = new BEXP { Operand1 = new BEXP { Operand1 = (COL)"ContactStartDate", Operator = BinaryOperator.GreaterThan, Operand2 = (VAL)startOfMonth.ToString("yyyy-MM-dd") }, Operator = BinaryOperator.And, Operand2 = new BEXP { Operand1 = (COL)"ContactStartDate", Operator = BinaryOperator.LessThan, Operand2 = (VAL)endOfMonth.ToString("yyyy-MM-dd") } }.GetSerializeObject(),
                        },
                         new PageFilterItem{
                            Title = "Gelecek",
                            Filter =  new BEXP { Operand1 = (COL)"ContactStartDate", Operator = BinaryOperator.GreaterThan, Operand2 = (VAL)DateTime.Now.ToString("yyyy/MM/dd") }.GetSerializeObject(),
                        },
                          new PageFilterItem{
                            Title = "Dün",
                            Filter = new BEXP { Operand1 = new BEXP { Operand1 = (COL)"ContactStartDate", Operator = BinaryOperator.GreaterThan, Operand2 = (VAL)now.AddDays(-1).ToString("yyyy-MM-dd") }, Operator = BinaryOperator.And, Operand2 = new BEXP { Operand1 = (COL)"ContactStartDate", Operator = BinaryOperator.LessThan, Operand2 = (VAL)now.ToString("yyyy-MM-dd") } }.GetSerializeObject(),
                        },
                          new PageFilterItem{
                            Title = "Geçen Hafta",
                            Filter = new BEXP { Operand1 = new BEXP { Operand1 = (COL)"ContactStartDate", Operator = BinaryOperator.GreaterThan, Operand2 = (VAL)startOfWeek.AddDays(-7).ToString("yyyy-MM-dd") }, Operator = BinaryOperator.And, Operand2 = new BEXP { Operand1 = (COL)"ContactStartDate", Operator = BinaryOperator.LessThan, Operand2 = (VAL)startOfWeek.ToString("yyyy-MM-dd") } }.GetSerializeObject(),
                        },
                          new PageFilterItem{
                            Title = "Geçen Ay",
                            Filter = new BEXP { Operand1 = new BEXP { Operand1 = (COL)"ContactStartDate", Operator = BinaryOperator.GreaterThan, Operand2 = (VAL)startOfLastMonth.ToString("yyyy-MM-dd") }, Operator = BinaryOperator.And, Operand2 = new BEXP { Operand1 = (COL)"ContactStartDate", Operator = BinaryOperator.LessThan, Operand2 = (VAL)startOfMonth.ToString("yyyy-MM-dd") } }.GetSerializeObject(),
                        }
                    },
                });


                RenderResponse(context, model);
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }



        [HandleFunction("VWCRM_Contact/Delete")]
        public void VWCRM_ContactDelete(HttpContext context)
        {
            try
            {
                var model = ParseRequest<VMCRM_ContactModel>(context);
                var userId = CallContext.Current.UserId;
                var rs = model.Delete();
                RenderResponse(context, rs);
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }



        [HandleFunction("VWCRM_ContactAction/GetAll")]
        public void VWCRM_ContactActionGetAll(HttpContext context)
        {
            try
            {
                var c = ParseRequest<Condition>(context);
                var cond = c != null ? CondtionToQuery.Convert(c) : new SimpleQuery();
                var db = new WorkOfTimeDatabase();
                var data = db.GetVWCRM_ContactAction(cond);
                RenderResponse(context, data);
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }

    }
}
