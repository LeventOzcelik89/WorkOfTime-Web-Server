using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
namespace Infoline.WorkOfTime.WebProject.Areas.INV.Controllers
{
    public class VWINV_CompanyPersonCalendarController : Controller
    {
        [PageInfo("Gündem Sayfası Methodu", SHRoles.Personel)]
        public JsonResult DataSource([DataSourceRequest] DataSourceRequest request, bool? filtre)
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var condition = KendoToExpression.Convert(request);
            var page = request.Page;
            request.Filters = new FilterDescriptor[0];
            request.Sorts = new SortDescriptor[0];
            request.Page = 1;

            var data = db.GetVWINV_CompanyPersonCalendar(condition).RemoveGeographies().ToDataSourceResult(request);
            data.Total = db.GetVWINV_CompanyPersonCalendarCount(condition.Filter);

            if (userStatus != null && userStatus.user.IsWorking == true && filtre.HasValue && filtre == true)
            {
                var companyPersonCalenderPersonsIds = db.GetINV_CompanyPersonCalendarPersonsByIdUser(userStatus.user.id).Select(x => x.IDPersonCalendar);
                if (companyPersonCalenderPersonsIds.Count() == 0)
                {
                    data.Data = (data.Data as IEnumerable<VWINV_CompanyPersonCalendar>).Where(x => x.id == System.UIHelper.Guid.Null);
                    data.Total = (data.Data as IEnumerable<VWINV_CompanyPersonCalendar>).Count();
                }
                else
                {
                    if (userStatus != null && userStatus.user.IsWorking == true)
                    {
                        data.Data = (data.Data as IEnumerable<VWINV_CompanyPersonCalendar>).Where(x => companyPersonCalenderPersonsIds.Contains(x.id)).ToArray();
                        data.Total = (data.Data as IEnumerable<VWINV_CompanyPersonCalendar>).Count();
                    }
                }
            }

            return Json(data, JsonRequestBehavior.AllowGet);
        }


        [PageInfo("Gündem Sayfası Detayı", SHRoles.Personel)]
        public ActionResult Detail(Guid id)
        {
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWINV_CompanyPersonCalendarById(id);
            return View(data);
        }

        [PageInfo("İş Kazası Eğitim Sayfası Detayı", SHRoles.SistemYonetici, SHRoles.ISGSorumlusu, SHRoles.SahaGorevYonetici, SHRoles.SahaGorevOperator, SHRoles.ProjePersonel, SHRoles.ProjeYonetici)]
        public ActionResult DetailWorkAccident(Guid id)
        {
            var db = new WorkOfTimeDatabase();
            var workAccident = db.GetVWSH_WorkAccidentCalendarById(id);
            var data = db.GetVWINV_CompanyPersonCalendarById(workAccident.companyPersonCalendarId.Value);

            var r = db.INV_CompanyPersonCalendarPersonsByIDPersonCalendarId(data.id).Select(s => s.IdUser).ToArray();
            ViewBag.Persons = r;

            return View(data);
        }

        [PageInfo("Gündem Sayfası Veri Ekleme", SHRoles.Personel)]
        public ActionResult Insert(DateTime? StartDate, DateTime? EndDate)
        {
            return View(new VWINV_CompanyPersonCalendar
            {
                id = Guid.NewGuid(),
                StartDate = StartDate,
                EndDate = EndDate
            });
        }

        [PageInfo("İş Kazası Eğitim Sayfası Veri Ekleme", SHRoles.SistemYonetici, SHRoles.ISGSorumlusu)]
        public ActionResult InsertWorkAccident(int Type, Guid workAccidentId)
        {
            ViewBag.workAccidentId = workAccidentId;
            return View(new VWINV_CompanyPersonCalendar
            {
                id = Guid.NewGuid(),
                Type = Type

            });
        }


        [PageInfo("İş Kazası Eğitim Sayfası Veri Ekleme", SHRoles.SistemYonetici, SHRoles.ISGSorumlusu)]
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult InsertWorkAccident(INV_CompanyPersonCalendar item, bool? mailForParticipants)
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            item.created = DateTime.Now;
            item.createdby = userStatus.user.id;

            var persons = Request["IdUsers"];
            if (persons == null)
            {
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Warning("Herhangi bir personel seçimi yapılmadı.")
                }, JsonRequestBehavior.AllowGet);
            }

            var personList = persons.Split(',').Select(a => new INV_CompanyPersonCalendarPersons
            {
                IdUser = a.ToGuid(),
                created = DateTime.Now,
                createdby = userStatus.user.id,
                IDPersonCalendar = item.id
            }).ToArray();

            var trans = db.BeginTransaction();
            var dbres1 = db.InsertINV_CompanyPersonCalendar(item, trans);
            var dbres2 = db.BulkInsertINV_CompanyPersonCalendarPersons(personList, trans);

            if (!dbres1.result || !dbres2.result)
            {
                trans.Rollback();
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Error(dbres1.message + "\n" + dbres2.message)
                }, JsonRequestBehavior.AllowGet);
            }

            var rsFile = new FileUploadSave(Request, item.id).SaveAs();

            if (mailForParticipants == true)
            {
                var stringType = Infoline.Helper.EnumsProperties.EnumToArrayGeneric<EnumINV_CompanyPersonCalendarType>().Where(x => Convert.ToInt32(x.Key)==item.Type).FirstOrDefault().Value;
                
                var emailUsers = string.Join(";", db.GetVWSH_UserByIds(persons.Split(',').Select(x => new Guid(x)).ToArray()).Where(c => !string.IsNullOrEmpty(c.email)).Select(x => x.email).ToArray());

                if (item.Type == (Int32)EnumINV_CompanyPersonCalendarType.Toplanti || item.Type == (Int32)EnumINV_CompanyPersonCalendarType.Hatirlatma)
                {
                    new Email().SendCalendar(emailUsers, item.Title, item.Description, item.StartDate.Value, item.EndDate.Value);
                }
                else
                {
                    var NewEndDate = item.Type == 100 || item.Type == 101 || item.Type == 103 || item.Type == 104 ? null : "- " + String.Format("{0:dd/MM/yyyy HH:mm}", item.EndDate.ToString());
                    var tenantName = TenantConfig.Tenant.TenantName;
                    var mesajIcerigi = string.Format(@"<h3>Merhaba,</h3> 
                     <p>{0}  {1} tarihinde '{2}' etkinliği oluşturulmuştur. 
                     </p> <p>{3}</p><p>Bilgilerinize.<br>İyi Çalışmalar.</p>",
                          String.Format("{0:dd/MM/yyyy HH:mm}", item.StartDate), String.Format("{0:dd/MM/yyyy HH:mm}", NewEndDate), stringType, item.Description);

                    new Email().Template("Template1", null, item.Title, mesajIcerigi).Send((Int16)EmailSendTypes.DuyuruEtkinlik, emailUsers, string.Format("{0} | {1}", tenantName + " | WORKOFTIME", item.Title), true);
                }
            }


            //SH_WorkAccidentCalendar kayıt ekleme
            var workAccidentCalender = new SH_WorkAccidentCalendar
            {
                id = Guid.NewGuid(),
                created = DateTime.Now,
                createdby = userStatus.user.id,
                workAccidentId = Guid.Parse(Request["workAccidentId"].ToString()),
                companyPersonCalendarId = item.id
            };

            dbres1 = db.InsertSH_WorkAccidentCalendar(workAccidentCalender, trans);
            if (dbres1.result)
            {
                trans.Commit();
                return Json(new ResultStatusUI
                {
                    Result = true,
                    FeedBack = feedback.Success("Takvim Etkinliği başarılı bir şekilde kaydedildi.\n" + item.Title)
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                trans.Rollback();
                return Json(new ResultStatusUI
                {
                    Result = true,
                    FeedBack = feedback.Warning("Takvim Etkinliği Eklenemedi.\n" + item.Title)
                }, JsonRequestBehavior.AllowGet);
            }


        }

        [HttpPost, ValidateAntiForgeryToken]
        [PageInfo("Gündem Sayfası Veri Ekleme", SHRoles.Personel)]
        public JsonResult Insert(INV_CompanyPersonCalendar item, bool? mailForParticipants)
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            item.created = DateTime.Now;
            item.createdby = userStatus.user.id;

            var persons = Request["IdUsers"];
            if (persons == null)
            {
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Warning("Herhangi bir personel seçimi yapılmadı.")
                }, JsonRequestBehavior.AllowGet);
            }

            var personList = persons.Split(',').Select(a => new INV_CompanyPersonCalendarPersons
            {
                IdUser = a.ToGuid(),
                created = DateTime.Now,
                createdby = userStatus.user.id,
                IDPersonCalendar = item.id
            }).ToArray();

            var trans = db.BeginTransaction();
            var dbres1 = db.InsertINV_CompanyPersonCalendar(item, trans);
            var dbres2 = db.BulkInsertINV_CompanyPersonCalendarPersons(personList, trans);

            if (!dbres1.result || !dbres2.result)
            {
                trans.Rollback();
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Error(dbres1.message + "\n" + dbres2.message)
                }, JsonRequestBehavior.AllowGet);
            }

            trans.Commit();
            var rsFile = new FileUploadSave(Request, item.id).SaveAs();

            if (mailForParticipants == true)
            {
                var stringType = ((EnumINV_CompanyPersonCalendarType)item.Type).ToDescription();
                var emailUsers = string.Join(";", db.GetVWSH_UserByIds(persons.Split(',').Select(x => new Guid(x)).ToArray()).Where(c => !string.IsNullOrEmpty(c.email)).Select(x => x.email).ToArray());

                if (item.Type == (Int32)EnumINV_CompanyPersonCalendarType.Toplanti || item.Type == (Int32)EnumINV_CompanyPersonCalendarType.Hatirlatma)
                {
                    new Email().SendCalendar(emailUsers, item.Title, item.Description, item.StartDate.Value, item.EndDate.Value);
                }
                else
                {
                    var NewEndDate = item.Type == 100 || item.Type == 101 || item.Type == 103 || item.Type == 104 ? null : "- " + String.Format("{0:dd/MM/yyyy HH:mm}", item.EndDate.ToString());
                    var tenantName = TenantConfig.Tenant.TenantName;
                    var mesajIcerigi = string.Format(@"<h3>Merhaba,</h3> 
                     <p>{0}  {1} tarihinde '{2}' etkinliği oluşturulmuştur. 
                     </p> <p>{3}</p><p>Bilgilerinize.<br>İyi Çalışmalar.</p>",
                          String.Format("{0:dd/MM/yyyy HH:mm}", item.StartDate), String.Format("{0:dd/MM/yyyy HH:mm}", NewEndDate), stringType, item.Description);

                    new Email().Template("Template1", (item.Type == 106 ? 103 : item.Type) + ".jpg", item.Title, mesajIcerigi).Send((Int16)EmailSendTypes.DuyuruEtkinlik, emailUsers, string.Format("{0} | {1}", tenantName + " | WORKOFTIME", item.Title), true);
                }
            }

            return Json(new ResultStatusUI
            {
                Result = true,
                FeedBack = feedback.Success("Takvim Etkinliği başarılı bir şekilde kaydedildi.\n" + item.Title)
            }, JsonRequestBehavior.AllowGet);

        }


        [PageInfo("Gündem Sayfası Güncelleme", SHRoles.Personel)]
        public ActionResult Update(Guid id)
        {
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWINV_CompanyPersonCalendarById(id);
            var r = db.INV_CompanyPersonCalendarPersonsByIDPersonCalendarId(id).Select(s => s.IdUser).ToArray();
            ViewBag.Persons = r;

            return View(data);
        }


        [HttpPost, ValidateAntiForgeryToken]
        [PageInfo("Gündem Sayfası Veri Güncelleme", SHRoles.Personel)]
        public JsonResult Update(INV_CompanyPersonCalendar item, bool? mailForParticipants)
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            item.created = DateTime.Now;
            item.createdby = userStatus.user.id;

            var persons = Request["IdUsers"];
            if (persons == null)
            {
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Warning("Herhangi bir personel seçimi yapılmadı.")
                }, JsonRequestBehavior.AllowGet);
            }

            var personList = persons.Split(',').Select(a => new INV_CompanyPersonCalendarPersons
            {
                IdUser = a.ToGuid(),
                created = DateTime.Now,
                createdby = item.createdby,
                IDPersonCalendar = item.id
            }).ToArray();

            var trans = db.BeginTransaction();

            var rsDelete = db.BulkDeleteINV_CompanyPersonCalendarPersons(db.INV_CompanyPersonCalendarPersonsByIDPersonCalendarId(item.id), trans);

            var dbres1 = db.UpdateINV_CompanyPersonCalendar(item, false, trans);
            var dbres2 = db.BulkInsertINV_CompanyPersonCalendarPersons(personList, trans);

            if (!dbres1.result || !dbres2.result)
            {
                trans.Rollback();
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Error(dbres1.message + "\n" + dbres2.message)
                }, JsonRequestBehavior.AllowGet);
            }

            if (mailForParticipants == true)
            {
                var stringType = ((EnumINV_CompanyPersonCalendarType)item.Type).ToDescription();
                var emailUsers = string.Join(";", db.GetVWSH_UserByIds(persons.Split(',').Select(x => new Guid(x)).ToArray()).Select(x => x.email).ToArray());
                if (item.Type == (Int32)EnumINV_CompanyPersonCalendarType.Toplanti)
                {
                    new Email().SendCalendar(emailUsers, item.Title, item.Description, item.StartDate.Value, item.EndDate.Value);
                }
                else
                {
                    var NewEndDate = item.Type == 100 || item.Type == 101 || item.Type == 103 || item.Type == 104 ? null : "- " + String.Format("{0:dd/MM/yyyy HH:mm}", item.EndDate.ToString());
                    var tenantName = TenantConfig.Tenant.TenantName;
                    var mesajIcerigi = string.Format(@"<h3>Merhaba,</h3> 
                     <p>{0}  {1} tarihinde '{2}' etkinliği oluşturulmuştur. 
                     </p> <p>{3}</p><p>Bilgilerinize.<br>İyi Çalışmalar.</p>",
                          String.Format("{0:dd/MM/yyyy HH:mm}", item.StartDate), String.Format("{0:dd/MM/yyyy HH:mm}", NewEndDate), stringType, item.Description);

                    new Email().Template("Template1", (item.Type == 106 ? 103 : item.Type) + ".jpg", item.Title, mesajIcerigi).Send((Int16)EmailSendTypes.DuyuruEtkinlik, emailUsers, string.Format("{0} | {1}", tenantName + " | WORKOFTIME", item.Title), true);
                }
            }

            trans.Commit();
            var rsFile = new FileUploadSave(Request, item.id).SaveAs();

            return Json(new ResultStatusUI
            {
                Result = false,
                FeedBack = feedback.Success("Takvim Etkinliği güncelleme işlemi tamamlandır.\n" + item.Title, false, Request.UrlReferrer.AbsoluteUri, timeout: 1)
            }, JsonRequestBehavior.AllowGet);

        }

        [PageInfo("İş Kazası Eğitim Sayfası Veri Güncelleme", SHRoles.SistemYonetici, SHRoles.ISGSorumlusu)]
        public ActionResult UpdateWorkAccident(Guid id)
        {
            var db = new WorkOfTimeDatabase();
            var workAccident = db.GetVWSH_WorkAccidentCalendarById(id);
            var data = db.GetVWINV_CompanyPersonCalendarById(workAccident.companyPersonCalendarId.Value);

            var r = db.INV_CompanyPersonCalendarPersonsByIDPersonCalendarId(data.id).Select(s => s.IdUser).ToArray();
            ViewBag.Persons = r;

            return View(data);
        }

        [PageInfo("İş Kazası Eğitim Sayfası Veri Güncelleme", SHRoles.SistemYonetici, SHRoles.ISGSorumlusu)]
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult UpdateWorkAccident(INV_CompanyPersonCalendar item, bool? mailForParticipants)
        {
            var db = new WorkOfTimeDatabase();

            var control = db.GetINV_CompanyPersonCalendarById(item.id);
            if(control == null)
            {
                var accident = db.GetSH_WorkAccidentCalendarById(item.id);
                item.id = accident.companyPersonCalendarId.Value;
            }
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            item.changed = DateTime.Now;
            item.changedby = userStatus.user.id;

            var persons = Request["IdUsers"];
            if (persons == null)
            {
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Warning("Herhangi bir personel seçimi yapılmadı.")
                }, JsonRequestBehavior.AllowGet);
            }

            var personList = persons.Split(',').Select(a => new INV_CompanyPersonCalendarPersons
            {
                IdUser = a.ToGuid(),
                created = DateTime.Now,
                createdby = item.changedby,
                IDPersonCalendar = item.id
            }).ToArray();

            var trans = db.BeginTransaction();

            var tmpUsers = db.INV_CompanyPersonCalendarPersonsByIDPersonCalendarId(item.id);
            var rsDelete = db.BulkDeleteINV_CompanyPersonCalendarPersons(tmpUsers, trans);

            var dbres1 = db.UpdateINV_CompanyPersonCalendar(item, false, trans);
            var dbres2 = db.BulkInsertINV_CompanyPersonCalendarPersons(personList, trans);

            if (!dbres1.result || !dbres2.result)
            {
                trans.Rollback();
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Error(dbres1.message + "\n" + dbres2.message)
                }, JsonRequestBehavior.AllowGet);
            }

            if (mailForParticipants == true)
            {
                var stringType = Infoline.Helper.EnumsProperties.EnumToArrayGeneric<EnumINV_CompanyPersonCalendarType>().Where(x => Convert.ToInt32(x.Key) == item.Type).FirstOrDefault().Value;
                var emailUsers = string.Join(";", db.GetVWSH_UserByIds(persons.Split(',').Select(x => new Guid(x)).ToArray()).Select(x => x.email).ToArray());
                if (item.Type == (Int32)EnumINV_CompanyPersonCalendarType.Toplanti)
                {
                    new Email().SendCalendar(emailUsers, item.Title, item.Description, item.StartDate.Value, item.EndDate.Value);
                }
                else
                {
                    var NewEndDate = item.Type == 100 || item.Type == 101 || item.Type == 103 || item.Type == 104 ? null : "- " + String.Format("{0:dd/MM/yyyy HH:mm}", item.EndDate.ToString());
                    var tenantName = TenantConfig.Tenant.TenantName;
                    var mesajIcerigi = string.Format(@"<h3>Merhaba,</h3> 
                     <p>{0}  {1} tarihinde '{2}' etkinliği oluşturulmuştur. 
                     </p> <p>{3}</p><p>Bilgilerinize.<br>İyi Çalışmalar.</p>",
                          String.Format("{0:dd/MM/yyyy HH:mm}", item.StartDate), String.Format("{0:dd/MM/yyyy HH:mm}", NewEndDate), stringType, item.Description);

                    new Email().Template("Template1", null, item.Title, mesajIcerigi).Send((Int16)EmailSendTypes.DuyuruEtkinlik, emailUsers, string.Format("{0} | {1}", tenantName + " | WORKOFTIME", item.Title), true);
                }
            }

            trans.Commit();
            var rsFile = new FileUploadSave(Request, item.id).SaveAs();

            return Json(new ResultStatusUI
            {
                Result = false,
                FeedBack = feedback.Success("Takvim Etkinliği güncelleme işlemi tamamlandır.\n" + item.Title, false, Request.UrlReferrer.AbsoluteUri, timeout: 1)
            }, JsonRequestBehavior.AllowGet);

        }


        [HttpPost]
        [PageInfo("Gündem Sayfası Veri Silme", SHRoles.Personel)]
        public JsonResult Delete(string[] id)
        {
            var db = new WorkOfTimeDatabase();
            var feedback = new FeedBack();

            var item = id.Select(a => new INV_CompanyPersonCalendar { id = new Guid(a) });

            var dbresult = db.BulkDeleteINV_CompanyPersonCalendar(item);

            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Silme işlemi başarılı") : feedback.Error("Silme işlemi başarılı")
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [PageInfo("Takvim", SHRoles.Personel)]
        public ActionResult Calendar(int? Type)
        {

            return View(new VWINV_CompanyCalendar() { Type = Type });

        }

        [PageInfo("Benim Şirket Gündemim (Ajanda)", SHRoles.Personel)]
        public ActionResult MyCalendar(int? Type)
        {

            return View(new VWINV_CompanyCalendar() { Type = Type });

        }

        [PageInfo("Ajanda Verilerinin Okunduğu Method", SHRoles.Personel)]
        public JsonResult SchedulerDataSource()
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];

            var date = DateTime.Now;
            var start = DateTime.Now;
            var end = DateTime.Now;
            if (TenantConfig.Tenant.TenantCode == 1169)
            {
                 start = date.AddDays(-1);
                 end = date.AddDays(3);
            }
            else
            {
                 start = date.AddDays(30);
                 end = date.AddDays(30);
            }

            var data = new CalendarModel().Load(userStatus.user.id, start, end, null);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [PageInfo("Ajanda Verilerinin Okunduğu Method", SHRoles.Personel)]
        public JsonResult CertificateSchedulerDataSource()
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];

            var start = new DateTime(DateTime.Now.Year - 2, 12, 31);
            var end = new DateTime(DateTime.Now.Year + 2, 12, 31);

            var data = new CalendarModel().CertificateLoad(userStatus.user.id, start, end, null);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [PageInfo("Genel Takvim Verilerinin Okunduğu Method", SHRoles.Personel)]
        public JsonResult CalendarDataSource(DateTime start, DateTime end, int? type, string email = "")
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];

            if (start == new DateTime(2018, 12, 31, 00, 00, 00))
            {
                start = new DateTime(2019, 01, 01, 00, 00, 00);
            }

            var data = new CalendarModel().Load(userStatus.user.id, start, end, type, email);

            return Json(data, JsonRequestBehavior.AllowGet);
        }


        [PageInfo("", SHRoles.Personel)]
        public ContentResult DescriptionTypeGroup()
        {
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWINV_CompanyPersonCalendar().Where(c => c.Type == (Int32)EnumINV_CompanyPersonCalendarType.Toplanti).GroupBy(x => x.Description);
            var veri = data.Where(x => x.Key != null).Select(c => c.Key).ToArray();
            return Content(Infoline.Helper.Json.Serialize(veri), "application/json");
        }
    }
}
