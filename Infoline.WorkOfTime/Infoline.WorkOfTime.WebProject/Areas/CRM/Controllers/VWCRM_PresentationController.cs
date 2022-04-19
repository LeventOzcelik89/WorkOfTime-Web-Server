using GeoAPI.Geometries;
using Infoline.Framework.Database;
using Infoline.Helper;
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
    public class VWCRM_PresentationController : Controller
    {
        [PageInfo("Potansiyel Fırsatlar", SHRoles.CRMYonetici, SHRoles.SatisPersoneli, SHRoles.CRMBayiPersoneli, SHRoles.CagriMerkezi)]
        public ActionResult Index()
        {
            var db = new WorkOfTimeDatabase();
            var data = db.GetCRM_ManagerStage();
            return View(data);
        }

        [PageInfo("Potansiyel/Fırsat Methodu", SHRoles.Personel, SHRoles.CRMBayiPersoneli, SHRoles.CagriMerkezi)]
        public ContentResult DataSource([DataSourceRequest] DataSourceRequest request)
        {
            var userStatus = (PageSecurity)Session["userStatus"];
            var db = new WorkOfTimeDatabase();
            if (userStatus.AuthorizedRoles.Contains(new Guid(SHRoles.CRMBayiPersoneli)))
            {
                var cc = KendoToExpression.Convert(request);
                request.Page = 1;
                cc = UpdateQuery(cc, userStatus);
                var datam = db.GetVWCRM_Presentation(cc).RemoveGeographies().ToDataSourceResult(request);
                datam.Total = db.GetVWCRM_PresentationCount(cc.Filter);
                return Content(Infoline.Helper.Json.Serialize(datam), "application/json");
            }
            if (userStatus.AuthorizedRoles.Contains(new Guid(SHRoles.SatisPersoneli)))
            {
                var cc = KendoToExpression.Convert(request);
                request.Page = 1;
                cc = UpdateQueryManaging(cc, userStatus);
                var datam = db.GetVWCRM_Presentation(cc).RemoveGeographies().ToDataSourceResult(request);
                datam.Total = db.GetVWCRM_PresentationCount(cc.Filter);
                return Content(Infoline.Helper.Json.Serialize(datam), "application/json");
            }
            request = UpdateRequest(request);
            var condition = KendoToExpression.Convert(request);
            request.Page = 1;
            var data = db.GetVWCRM_Presentation(condition).RemoveGeographies().ToDataSourceResult(request);
            data.Total = db.GetVWCRM_PresentationCount(condition.Filter);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }

        [PageInfo("Potansiyel/Fırsat Veri Methodu", SHRoles.Personel, SHRoles.CRMBayiPersoneli, SHRoles.CagriMerkezi)]
        public ContentResult DataSourceDropDown([DataSourceRequest] DataSourceRequest request)
        {
            var userStatus = (PageSecurity)Session["userStatus"];
            var db = new WorkOfTimeDatabase();
            if (userStatus.AuthorizedRoles.Contains(new Guid(SHRoles.CRMBayiPersoneli)))
            {
                var cc = KendoToExpression.Convert(request);
                cc = UpdateQuery(cc, userStatus);
                var datam = db.GetVWCRM_Presentation(cc);
                return Content(Infoline.Helper.Json.Serialize(datam), "application/json");
            }
            if (userStatus.AuthorizedRoles.Contains(new Guid(SHRoles.SatisPersoneli)))
            {
                var cc = KendoToExpression.Convert(request);
                cc = UpdateQueryManaging(cc, userStatus);
                var datam = db.GetVWCRM_Presentation(cc);
                return Content(Infoline.Helper.Json.Serialize(datam), "application/json");
            }
            request = UpdateRequest(request);
            var condition = KendoToExpression.Convert(request);
            var data = db.GetVWCRM_Presentation(condition);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }

        [PageInfo("Potansiyel/Fırsat Toplam Veri Methodu", SHRoles.Personel, SHRoles.CRMBayiPersoneli, SHRoles.CagriMerkezi)]
        public int DataSourceCount([DataSourceRequest] DataSourceRequest request)
        {
            var userStatus = (PageSecurity)Session["userStatus"];
            var db = new WorkOfTimeDatabase();
            if (userStatus.AuthorizedRoles.Contains(new Guid(SHRoles.CRMBayiPersoneli)))
            {
                var cc = KendoToExpression.Convert(request);
                cc = UpdateQuery(cc, userStatus);
                var cnt = db.GetVWCRM_PresentationCount(cc.Filter);
                return cnt;
            }
            if (userStatus.AuthorizedRoles.Contains(new Guid(SHRoles.SatisPersoneli)))
            {
                var cc = KendoToExpression.Convert(request);
                cc = UpdateQueryManaging(cc, userStatus);
                var cnt = db.GetVWCRM_PresentationCount(cc.Filter);
                return cnt;
            }

            request = UpdateRequest(request);
            var condition = KendoToExpression.Convert(request);
            var count = db.GetVWCRM_PresentationCount(condition.Filter);
            return count;
        }

        [PageInfo("Potansiyel/Fırsat Ekleme", SHRoles.SatisPersoneli, SHRoles.CRMYonetici, SHRoles.CRMBayiPersoneli, SHRoles.CagriMerkezi)]
        public ActionResult Insert(VMCRM_PresentationModel model)
        {
            var userStatus = (PageSecurity)Session["userStatus"];
            model.SalesPersonId = userStatus.user.id;
            var res = model.Load();
            if (userStatus.AuthorizedRoles.Contains(new Guid(SHRoles.CRMBayiPersoneli)))
            {
                res.ChannelCompanyId = userStatus.user.CompanyId;
                return PartialView("~/Areas/CRM/Views/VWCRM_Presentation/SellerInsert.cshtml", res);
            }
            else if (userStatus.AuthorizedRoles.Contains(new Guid(SHRoles.CagriMerkezi)))
            {
                return PartialView("~/Areas/CRM/Views/VWCRM_Presentation/CallCenterInsert.cshtml", res);
            }
            else
            {
                return View(res);
            }
        }

        [PageInfo("Potansiyel/Fırsat Ekleme Methodu", SHRoles.SatisPersoneli, SHRoles.CRMYonetici, SHRoles.CRMBayiPersoneli, SHRoles.CagriMerkezi)]
        [HttpPost, ValidateAntiForgeryToken]
        public ContentResult Insert(VMCRM_PresentationModel item, DateTime? AppointmentDate, string AbsoluteUri, bool? isPost)
        {
            var feedback = new FeedBack();
            var userStatus = (PageSecurity)Session["userStatus"];
            var db = new WorkOfTimeDatabase();
            item.created = DateTime.Now;
            item.createdby = userStatus.user.id;
            var trans = db.BeginTransaction();
            var res = item.Save(userStatus.user.id, trans);

            if (userStatus.AuthorizedRoles.Contains(new Guid(SHRoles.CagriMerkezi)))
            {
                if (item.CustomerCompanyId.HasValue && item.ChannelCompanyId.HasValue)
                {
                    var control = db.GetCMP_CompanyRelationByCustomerIdAndCompanyId(item.CustomerCompanyId.Value, item.ChannelCompanyId.Value);
                    if (control == null)
                    {
                        res &= db.InsertCMP_CompanyRelation(new CMP_CompanyRelation
                        {
                            created = DateTime.Now,
                            companyId = item.ChannelCompanyId,
                            customerId = item.CustomerCompanyId,
                            createdby = userStatus.user.id
                        }, trans);
                    }
                }
            }

            if (res.result)
            {
                res.objects = db.GetVWCRM_PresentationById(item.id);
                trans.Commit();
            }
            else
            {
                trans.Rollback();
            }

            if (res.result && (item.LastContact != null && item.LastContact.ContactType.HasValue))
            {
                new FileUploadSave(Request, item.LastContact.id).SaveAs();
            }

            return Content(Infoline.Helper.Json.Serialize(new ResultStatusUI
            {
                Result = res.result,
                FeedBack = res.result ? feedback.Success(res.message, false, (AbsoluteUri.EndsWith("AgileBoard") ? "" : AbsoluteUri)) : feedback.Warning(res.message),
                Object = res.objects
            }), "application/json");

        }

        [PageInfo("Potansiyel/Fırsat Güncelleme", SHRoles.SatisPersoneli, SHRoles.CRMYonetici, SHRoles.CRMBayiPersoneli, SHRoles.CagriMerkezi)]
        public ActionResult Update(VMCRM_PresentationModel model)
        {
            var userStatus = (PageSecurity)Session["userStatus"];
            model.Load();
            if (userStatus.AuthorizedRoles.Contains(new Guid(SHRoles.CRMBayiPersoneli)))
            {
                model.ChannelCompanyId = userStatus.user.CompanyId;
                return PartialView("~/Areas/CRM/Views/VWCRM_Presentation/SellerUpdate.cshtml", model);
            }
            else if (userStatus.AuthorizedRoles.Contains(new Guid(SHRoles.CagriMerkezi)))
            {
                return PartialView("~/Areas/CRM/Views/VWCRM_Presentation/CallCenterUpdate.cshtml", model);
            }
            else
            {
                return View(model);
            }
        }

        [PageInfo("Potansiyel/Fırsat Güncelleme Methodu", SHRoles.SatisPersoneli, SHRoles.CRMYonetici, SHRoles.CRMBayiPersoneli, SHRoles.CagriMerkezi)]
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Update(VMCRM_PresentationModel item, bool? isPost)
        {
            var feedback = new FeedBack();
            var userStatus = (PageSecurity)Session["userStatus"];
            var res = item.Save(userStatus.user.id);
            return Json(new ResultStatusUI
            {
                Result = res.result,
                FeedBack = res.result ? feedback.Success(res.message, false) : feedback.Warning(res.message)

            }, JsonRequestBehavior.AllowGet);
        }

        [PageInfo("Potansiyel/Fırsat Detayı", SHRoles.SatisPersoneli, SHRoles.CRMYonetici, SHRoles.CRMBayiPersoneli, SHRoles.CagriMerkezi)]
        public ActionResult Detail(Guid id, bool? isAddContact)
        {
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWCRM_PresentationById(id);
            var contacts = db.GetCRM_ContactByPresentationId(id);

            ViewBag.ContactIds = contacts.Select(a => a.id).ToArray();
            ViewBag.Products = db.GetVWCRM_PresentationProductsByPresentationId(id);
            ViewBag.Company = data.CustomerCompanyId.HasValue ? db.GetVWCMP_CompanyById(data.CustomerCompanyId.Value) : null;
            ViewBag.EnumProperties = EnumsProperties.EnumToArrayGeneric<EnumCRM_PresentationActionType>().ToArray();

            var model = new VMVWCRM_Presentation().B_EntityDataCopyForMaterial(data);
            model.Actions = db.GetVWCRM_PresentationActionByPresentationId(id);

            var tender = db.GetVWCMP_TenderByPresentationIdLast(model.id);
            if (tender != null)
            {
                model.LastTender = tender;
            }

            model.isAddContact = isAddContact;

            return View(model);
        }


        [PageInfo("Potansiyel/Fırsat Detayı", SHRoles.SatisPersoneli, SHRoles.CRMYonetici, SHRoles.CRMBayiPersoneli, SHRoles.CagriMerkezi)]
        public ActionResult DetailLocation(Guid id)
        {
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWCRM_PresentationActionById(id);
            var model = new VMVWCRM_PresentationAction().B_EntityDataCopyForMaterial(data);
            model.Presentation = db.GetVWCRM_PresentationByPresentationActionId(data.presentationId.Value);
            return View(model);
        }


        [HttpPost]
        [PageInfo("Potansiyel/Fırsat Silme", SHRoles.CRMYonetici, SHRoles.CagriMerkezi)]
        public JsonResult Delete(Guid id)
        {
            var db = new WorkOfTimeDatabase();
            var feedback = new FeedBack();
            var dbres = new ResultStatus { result = true };
            var _contactUsers = new List<CRM_ContactUser>();
            //Potansiyel
            var presentation = db.GetCRM_PresentationById(id);
            if (presentation == null)
            {
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Warning("Potansiyel/Fırsat bulunamadı.")
                }, JsonRequestBehavior.AllowGet);
            }
            var trans = db.BeginTransaction();
            //Potansiyelin Toplantıları
            var _contact = db.GetCRM_ContactByPresentationId(presentation.id);
            if (_contact.Count() > 0)
            {
                foreach (var _con in _contact)
                {
                    var _contactUser = db.GetCRM_ContactUserByContactId(_con.id);
                    if (_contactUser.Count() > 0)
                    {
                        foreach (var _cntUs in _contactUser)
                        {
                            //Toplantıdaki personeller listeye ekleniyor.
                            _contactUsers.Add(_cntUs);
                        }
                    }


                    if (_contactUsers.Count() > 0)
                    {
                        //Toplantılardaki personeller siliniyor.
                        dbres &= db.BulkDeleteCRM_ContactUser(_contactUsers, trans);
                    }

                }

                var contactFiles = db.GetSYS_FilesInDataId(_contact.Select(c => c.id).ToArray());
                if (contactFiles.Count() > 0)
                {
                    //Toplantı dosyaları siliniyor.
                    dbres &= db.BulkDeleteSYS_Files(contactFiles, trans);
                }

                //Toplantı siliniyor
                dbres &= db.BulkDeleteCRM_Contact(_contact, trans);
            }

            //Ürünler
            var _products = db.GetCRM_PresentationProductsByPresentationId(presentation.id);
            if (_products.Count() > 0)
            {
                //Potansiyelin ürünleri siliniyor.
                dbres &= db.BulkDeleteCRM_PresentationProducts(_products, trans);
            }

            var _opponentCompany = db.GetCRM_PresentationOpponentCompanyByPresentationId(presentation.id);
            if (_opponentCompany.Count() > 0)
            {
                dbres &= db.BulkDeleteCRM_PresentationOpponentCompany(_opponentCompany, trans);
            }

            var presentationActions = db.GetCRM_PresentationActionByPresentationId(presentation.id);

            if (presentationActions.Count() > 0)
            {
                dbres &= db.BulkDeleteCRM_PresentationAction(presentationActions, trans);
            }

            var presentationFiles = db.GetSYS_FilesByDataIdArray(presentation.id).ToArray();
            if (presentationFiles.Count() > 0)
            {
                //Potansiyel/Fırsat resimleri siliniyor.
                dbres &= db.BulkDeleteSYS_Files(presentationFiles, trans);
            }

            //Potansiyel siliniyor.
            dbres &= db.DeleteCRM_Presentation(presentation, trans);

            if (dbres.result == false)
            {
                trans.Rollback();
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Warning("Potansiyel/Fırsat silme işlemi başarısız oldu.")
                }, JsonRequestBehavior.AllowGet);

            }

            trans.Commit();
            return Json(new ResultStatusUI
            {
                Result = true,
                FeedBack = feedback.Success("Potansiyel/Fırsat silme işlemi başarıyla gerçekleşti")
            }, JsonRequestBehavior.AllowGet);
        }



        [PageInfo("Tüm CRM Haritası", SHRoles.CRMYonetici)]
        public ActionResult Map()
        {
            return View();
        }

        [PageInfo("Potansiyel/Fırsat Haritası Verileri", SHRoles.CRMYonetici)]
        public ContentResult GetMapData()
        {
            var db = new WorkOfTimeDatabase();
            var presentations = db.GetVWCRM_Presentation();
            var contacts = db.GetVWCRM_Contact();
            var customers = db.GetVWCMP_CompanyByTypeOther().ToArray();

            var products = db.GetVWCRM_PresentationProductsByPresentationIds(presentations.Select(a => a.id).ToArray());
            var opponents = db.GetVWCRM_PresentationOpponentCompanyByPresentationIds(presentations.Select(a => a.id).ToArray());

            var contactUsers = db.GetVWCRM_ContactUserByContactIds(contacts.Select(a => a.id).ToArray());

            var stages = db.GetVWCRM_ManagerStage();

            var model = new VMCRM_PresentationForMap
            {
                Contacts = contacts,
                Customers = customers,
                Presentations = presentations,
                Products = products,
                OpponentCompany = opponents,
                ContactUsers = contactUsers,
                Stages = stages
            };

            var data = new
            {
                Customer = customers.Select(a => new
                {
                    Data = a,
                    Presentations = presentations.Where(c => c.CustomerCompanyId == a.id).Select(c => new
                    {
                        Data = c,
                        OpponentCompany = opponents.Where(d => d.PresentationId == c.id),
                        Products = products.Where(d => d.PresentationId == c.id),
                        Contacts = contacts.Where(d => d.PresentationId == c.id).Select(s => new
                        {
                            Data = s,
                            Users = contactUsers.Where(x => x.id == s.id)
                        })
                    })
                }),
                Stages = stages
            };


            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }

        [PageInfo("CRM Haritam", SHRoles.SatisPersoneli, SHRoles.CRMYonetici)]
        public ActionResult MyMap()
        {
            var db = new WorkOfTimeDatabase();
            return View();
        }

        [PageInfo("Potansiyel/Fırsatlarım Haritası Verileri", SHRoles.SatisPersoneli, SHRoles.CRMYonetici)]
        public ContentResult GetMyMapData()
        {
            var userStatus = (PageSecurity)Session["userStatus"];
            var db = new WorkOfTimeDatabase();
            var personIds = userStatus.ChildPersons.Where(a => a.IdUser != null).Select(a => a.IdUser.Value).Distinct().ToArray();
            var presentations = db.GetVWCRM_PresentationBySalesPersonIds(personIds).ToList();
            presentations.AddRange(db.GetVWCRM_PresentationByCreated(userStatus.user.id));

            var contacts = db.GetVWCRM_ContactByPresentationIds(presentations.Select(a => a.id).ToArray());
            var customers = db.GetVWCMP_CompanyByIds(presentations.Where(a => a.CustomerCompanyId.HasValue).Select(a => a.CustomerCompanyId.Value).ToArray());
            var products = db.GetVWCRM_PresentationProductsByPresentationIds(presentations.Select(a => a.id).ToArray());
            var opponents = db.GetVWCRM_PresentationOpponentCompanyByPresentationIds(presentations.Select(a => a.id).ToArray());
            var contactUsers = db.GetVWCRM_ContactUserByContactIds(contacts.Select(a => a.id).ToArray());
            var stages = db.GetVWCRM_ManagerStage();
            var model = new VMCRM_PresentationForMap
            {
                Contacts = contacts,
                Customers = customers,
                Presentations = presentations.ToArray(),
                Products = products,
                OpponentCompany = opponents,
                ContactUsers = contactUsers,
                Stages = stages
            };

            var data = new
            {
                Customer = customers.Select(a => new
                {
                    Data = a,
                    Presentations = presentations.Where(c => c.CustomerCompanyId == a.id).Select(c => new
                    {
                        Data = c,
                        OpponentCompany = opponents.Where(d => d.PresentationId == c.id),
                        Products = products.Where(d => d.PresentationId == c.id),
                        Contacts = contacts.Where(d => d.PresentationId == c.id).Select(s => new
                        {
                            Data = s,
                            Users = contactUsers.Where(x => x.id == s.id)
                        })
                    })
                }),
                Stages = stages
            };

            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }


        [PageInfo("Potansiyel/Fırsat Not Ekleme Metodu", SHRoles.CRMYonetici, SHRoles.SatisPersoneli, SHRoles.CRMBayiPersoneli, SHRoles.CagriMerkezi)]
        public ContentResult InsertNote(Guid presentationId, string note, IGeometry location)

        {
            var userStatus = (PageSecurity)Session["userStatus"];
            var dbresult = new VMCRM_PresentationModel { id = presentationId }.Load().InsertNote(userStatus.user.id, note, location);
            return Content(Infoline.Helper.Json.Serialize(dbresult), "application/json");
        }

        [PageInfo("Potansiyel/Fırsat Aşama Güncelle", SHRoles.SatisPersoneli, SHRoles.CRMYonetici, SHRoles.CRMBayiPersoneli, SHRoles.CagriMerkezi)]
        public ActionResult UpdateState(Guid id)
        {
            var db = new WorkOfTimeDatabase();
            var data = db.GetCRM_PresentationById(id);
            return View(data);
        }

        [HttpPost]
        [PageInfo("Potansiyel/Fırsat Aşaması Güncelleme", SHRoles.SatisPersoneli, SHRoles.CRMYonetici, SHRoles.CRMBayiPersoneli, SHRoles.CagriMerkezi)]
        public JsonResult UpdateState(CRM_Presentation item, IGeometry location)
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            item.changedby = userStatus.user.id;

            if (!item.PresentationStageId.HasValue)
            {
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = new FeedBack().Warning("Aşama seçmeden işleme devam edilemez.")
                }, JsonRequestBehavior.AllowGet);
            }

            var presentation = db.GetCRM_PresentationById(item.id);
            if (presentation.PresentationStageId != item.PresentationStageId)
            {
                presentation.changedby = userStatus.user.id;
                presentation.PresentationStageId = item.PresentationStageId;
                var trans = db.BeginTransaction();

                var dbresult = db.UpdateCRM_Presentation(item, false, trans);
                var newStage = db.GetCRM_ManagerStageById(item.PresentationStageId.Value);
                dbresult &= db.InsertCRM_PresentationAction(new CRM_PresentationAction
                {
                    created = DateTime.Now,
                    createdby = userStatus.user.id,
                    presentationId = item.id,
                    color = newStage != null ? newStage.color : "",
                    type = (short)EnumCRM_PresentationActionType.AsamaGüncelleme,
                    description = "Aşama Güncellendi. Yeni aşama : " + (newStage != null ? newStage.Name : ""),
                    location = location
                }, trans);

                if (dbresult.result == false)
                {
                    trans.Rollback();
                    return Json(new ResultStatusUI
                    {
                        Result = false,
                        FeedBack = new FeedBack().Error(dbresult.message)
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    trans.Commit();
                }
            }

            return Json(new ResultStatusUI
            {
                Result = true,
                Object = presentation,
                FeedBack = new FeedBack().Success("Potansiyel/Fırsat kaydı güncellendi...")
            }, JsonRequestBehavior.AllowGet);

        }

        [PageInfo("Önem Derecesi Güncelleme Metodu", SHRoles.CRMYonetici, SHRoles.SatisPersoneli, SHRoles.CRMBayiPersoneli, SHRoles.CagriMerkezi)]
        public JsonResult UpdatePriorityLevel(Guid id, int priorityLevel)
        {
            var db = new WorkOfTimeDatabase();
            var trans = db.BeginTransaction();
            var item = db.GetCRM_PresentationById(id);
            var oldPriorityLevel = item.PriorityLevel;
            var userStatus = (PageSecurity)Session["userStatus"];
            item.changed = DateTime.Now;
            item.changedby = userStatus.user.id;
            item.PriorityLevel = priorityLevel;
            var dbResult = db.UpdateCRM_Presentation(item, false, trans);

            dbResult &= db.InsertCRM_PresentationAction(new CRM_PresentationAction
            {
                id = Guid.NewGuid(),
                created = DateTime.Now,
                createdby = userStatus.user.id,
                description = "Önem derecesi " + priorityLevel.ToString() + " olarak güncellendi. Önceki önem derecesi : " + oldPriorityLevel,
                presentationId = item.id,
                type = (int)EnumCRM_PresentationActionType.OnemGuncellendi,
            }, trans);

            if (dbResult.result)
                trans.Commit();
            else
                trans.Rollback();

            return Json(new ResultStatusUI
            {
                Result = dbResult.result,
                FeedBack = dbResult.result ? new FeedBack().Success("Önem derecesi güncellendi.") : new FeedBack().Warning("Önem derecesi güncellenirken sorun oluştu. " + dbResult.message)
            }, JsonRequestBehavior.AllowGet);
        }

        [PageInfo("Satış ve Müşteri Raporu", SHRoles.CRMYonetici)]
        public ActionResult SalesReport()
        {
            return View();
        }

        [PageInfo("Crm satış ve müşteri raporu methodu", SHRoles.CRMYonetici, SHRoles.IdariPersonelYonetici)]
        public JsonResult GetSalesReportData(SalesFilter filter)
        {
            var data = new VMCRM_PresentationModel().GetReportDate(filter);
            return Json(new ResultStatusUI
            {
                Result = data.result,
                Object = data.objects
            }, JsonRequestBehavior.AllowGet);
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
                        new FilterDescriptor { Value = userStatus.user.id, Operator = FilterOperator.IsEqualTo, Member = "SalesPersonId" }
                    }
                };
                foreach (var item in userStatus.ChildPersons.Where(a => a.IdUser.HasValue && a.IdUser != userStatus.user.id).GroupBy(a => a.IdUser))
                {
                    composit = new CompositeFilterDescriptor
                    {
                        LogicalOperator = FilterCompositionLogicalOperator.Or,
                        FilterDescriptors = new FilterDescriptorCollection{
                            composit,
                            new FilterDescriptor { Value = item.Key, Operator = FilterOperator.IsEqualTo, Member = "createdby" },
                        }
                    };
                    composit = new CompositeFilterDescriptor
                    {
                        LogicalOperator = FilterCompositionLogicalOperator.Or,
                        FilterDescriptors = new FilterDescriptorCollection{
                            composit,
                            new FilterDescriptor { Value = userStatus.user.id, Operator = FilterOperator.IsEqualTo, Member = "SalesPersonId" },
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


        public static SimpleQuery UpdateQuery(SimpleQuery query, PageSecurity userStatus)
        {
            BEXP filter = null;
            filter |= new BEXP
            {
                Operand1 = (COL)"ChannelCompanyId",
                Operator = BinaryOperator.Equal,
                Operand2 = (VAL)userStatus.user.CompanyId
            };
            query.Filter &= filter;
            return query;
        }

        public static SimpleQuery UpdateQueryManaging(SimpleQuery query, PageSecurity userStatus)
        {
            BEXP filter = null;
            filter |= new BEXP
            {
                Operand1 = (COL)"ManagingUserIds",
                Operator = BinaryOperator.Like,
                Operand2 = (VAL)("%" + userStatus.user.id + "%").ToString()
            };
            query.Filter &= filter;
            return query;
        }

        [PageInfo("Satış Duvarı", SHRoles.CRMYonetici, SHRoles.SatisPersoneli, SHRoles.SatisFatura, SHRoles.SatisOnaylayici, SHRoles.CRMBayiPersoneli, SHRoles.CagriMerkezi)]
        public ActionResult AgileBoard(VWAgileBoardDashboardModel item)
        {
            var userStatus = (PageSecurity)Session["userStatus"];
            item.Load(userStatus.user.id);
            return View(item);
        }

    }
}
