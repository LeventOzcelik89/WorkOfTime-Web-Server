using Infoline.Framework.Database;
using Infoline.Framework.Helper;
using Infoline.Helper;
using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData;
using Infoline.WorkOfTime.BusinessData.Specific;
using Infoline.WorkOfTime.WebProject.Areas.CRM;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace Infoline.WorkOfTime.Controllers
{
    [AllowEveryone]
    public class GeneralController : Controller
    {


        public ContentResult img(Guid id)
        {

            var db = new WorkOfTimeDatabase();
            var profil = db.GetSysFilesFilePathByDataTableAndFileGroupAndDataId("SH_User", "Profil Resmi", id);


            var mark = new System.Drawing.Bitmap(Server.MapPath("/Content/Custom/img/PersonsBackImage/mark.png"));
            var profilFoto = new System.Drawing.Bitmap(Server.MapPath(profil.FilePath));

            //  kalite bozulmadan resize
            //  var profil2 = new Bitmap(profilFoto, 40, 40);


            using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(mark))
            {
                g.DrawImage(profilFoto, new System.Drawing.Rectangle(18, 8, 40, 40));
            }




            using (var ms = new MemoryStream())
            {

                mark.Save(ms, ImageFormat.Bmp);

                Response.Clear();
                Response.ContentType = "image/png";
                Response.AddHeader("content-disposition", "attachment; qr.png");
                Response.BinaryWrite(ms.ToArray());
                Response.End();

            }

            return Content(null);

        }

        public JsonResult GetEnums([DataSourceRequest] DataSourceRequest request)
        {
            var db = new WorkOfTimeDatabase();
            var condition = KendoToExpression.Convert(request);
            var result = db.GetSYS_Enums(condition).Select(a => new { Id = a.enumKey, enumDescription = a.enumDescription });
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetVWCMP_CompanyByCompanyId(Guid id)
        {
            var db = new WorkOfTimeDatabase();
            return Json(db.GetVWCMP_CompanyById(id).RemoveGeographies(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetVWSH_User_OurCompanies()
        {
            var db = new WorkOfTimeDatabase();
            var users = db.GetVWSH_UserMyPerson();

            return Json(users.Select(a => new { Id = a.id, Name = a.FullName }).OrderBy(a => a.Name), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetVWSH_User_WithRoles()
        {
            var db = new WorkOfTimeDatabase();
            var roleUser = db.GetSH_UserRoleByRoleIds(new string[] { SHRoles.SatisPersoneli, SHRoles.SatisOnaylayici, SHRoles.SistemYonetici });
            var userIds = roleUser.Where(x => x.userid.HasValue).Select(x => x.userid.Value).ToArray();
            var userList = db.GetSH_UserByIds(userIds.Count() > 0 ? userIds : new Guid[] { Guid.Empty });
            return Json(userList.Select(a => new { Id = a.id, Name = a.firstname + " " + a.lastname }).OrderBy(a => a.Name), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetVWFTM_TaskCustomers()
        {
            var db = new WorkOfTimeDatabase();
            var task = db.GetVWFTM_TaskJustCustomerQuery();
            var userStatus = (PageSecurity)Session["userStatus"];
            if (userStatus.AuthorizedRoles.Contains(new Guid(SHRoles.SahaGorevYonetici)) || userStatus.AuthorizedRoles.Contains(new Guid(SHRoles.SahaGorevOperator)))
            {
                var authoritys = db.GetVWFTM_TaskAuthorityByUserId(userStatus.user.id);
                if (authoritys.Count() > 0)
                    task = task.Where(x => authoritys.Where(f => f.customerId.HasValue).Select(f => f.customerId.Value).ToArray().Contains(x.customerId.Value)).ToArray();
            }

            var rs = task.GroupBy(a => a.customer_Title).Select(c => new
            {
                Name = c.Key,
                Id = c.Select(f => f.customerId).FirstOrDefault()
            });
            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetINV_PermitTypeName()
        {
            var db = new WorkOfTimeDatabase();
            var result = db.GetINV_PermitType();
            return Json(result.Select(a => new { Id = a.id, Name = a.Name, create = a.created }).OrderBy(x => x.Name).ToArray(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetExchangeRatesByDate(DateTime date)
        {
            if (date.Date > DateTime.Now.Date)
            {
                return Json(new ResultStatus { result = false, message = "Lütfen gelecek tarih girmeyiniz." });
            }

            else if (date.Date == DateTime.Now.Date)
            {
                return Json(CurrencyExchangeRates.GetAllCurrenciesTodaysExchangeRates(), JsonRequestBehavior.AllowGet);
            }

            else
            {
                return Json(CurrencyExchangeRates.GetAllCurrenciesHistoricalExchangeRates(date), JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetEnumProjectTechnicalType()
        {
            var result = EnumsProperties.EnumToArrayValues<EnumPRJ_ProjectProjectProjectTechnicalType>()
                .OrderBy(a => a.Value).Select(c => new { Id = c.Key, Name = c.Value });
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult FilterMenuTaskType()
        {
            var result = EnumsProperties.EnumToArrayValues<EnumFTM_TaskType>().Select(c => new { Id = c.Key, Name = c.Value }).OrderBy(a => a.Name).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult FilterMenuTaskStatus()
        {
            var result = EnumsProperties.EnumToArrayValues<EnumFTM_TaskOperationStatus>()
                .Select(c => new { Id = c.Key, Name = c.Value }).OrderBy(a => a.Name).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult FilterMenuTaskPriority()
        {
            var result = EnumsProperties.EnumToArrayValues<EnumFTM_TaskPriority>().Select(c => new { Id = c.Key, Name = c.Value }).OrderBy(a => a.Name).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult FilterMenuInventoryLastStatus()
        {
            var result = EnumsProperties.EnumToArrayValues<EnumPRD_InventoryActionType>().Where(a => a.Key.toInt32() == 0 || a.Key.toInt32() == 1 || a.Key.toInt32() == 2 || a.Key.toInt32() == 3 || a.Key.toInt32() == 4 | a.Key.toInt32() == 5).Select(c => new { Id = c.Key, Name = c.Value }).OrderBy(a => a.Name).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult FavoritesAdd(string data, bool? status, string search, string Description)
        {
            var db = new WorkOfTimeDatabase();
            var feedBack = new FeedBack();
            var results = new ResultStatus();
            results.result = true;
            var userStatus = (PageSecurity)Session["userStatus"];
            if (userStatus == null)
            {
                return Json(new ResultStatusUI
                {
                    Result = false,

                }, JsonRequestBehavior.AllowGet);
            }
            char[] controlParameter = { '/', ' ' };
            var dataControl = data.Split(controlParameter).Where(c => c != "").ToArray();
            var area = "";
            var controller = "";
            var method = "";

            //burada gelen search urlin parametresini vermektedir.Parametre alan url ler favorilere eklenebilsin isteği bilal bey tarafından gerçekleşti.
            //TODO:Oğuz
            //if (search != "" && search != null)
            //{
            //    return Json(new ResultStatusUI
            //    {
            //        Result = false,
            //        FeedBack = feedBack.Warning("Anasayfa"),
            //    }, JsonRequestBehavior.AllowGet);
            //}

            if (dataControl.Count() == 0)
            {
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedBack.Warning("Anasayfa"),
                }, JsonRequestBehavior.AllowGet);
            }
            else if (dataControl.Count() == 1)
            {
                controller = dataControl[0];
                method = "Index";
            }
            else if (dataControl.Count() == 2)
            {
                controller = dataControl[0];
                method = dataControl[1];
            }
            else
            {
                area = dataControl[0];
                controller = dataControl[1];
                method = dataControl[2];
            }
            var datas = db.GetFVR_FavoritesControl(userStatus.user.id, area, controller, method);
            if (datas == null)
            {
                var act = "/" + area + "/" + controller + "/" + method + search;
                if (act.StartsWith("//"))
                {
                    act = act.Substring(1);
                }

                else if (act.StartsWith("/"))
                {

                }
                else
                {
                    act = "/" + act;
                }

                var newFvr = new FVR_Favorites
                {
                    id = Guid.NewGuid(),
                    created = DateTime.Now,
                    createdby = userStatus.user.id,
                    userId = userStatus.user.id,
                    Area = area,
                    Controller = controller,
                    Method = method,
                    Count = 1,
                    Status = status,
                    Action = act,
                    Description = Description
                };
                results &= db.InsertFVR_Favorites(newFvr);
            }
            else
            {
                datas.Count = datas.Count + 1;
                datas.changed = DateTime.Now;
                datas.changedby = userStatus.user.id;
                datas.Status = status;
                datas.Description = Description != "" ? Description : datas.Description != null ? datas.Description : db.GetSH_Pages().Where(c => c.Action == datas.Action).Select(b => b.Description).FirstOrDefault();
                results &= db.UpdateFVR_Favorites(datas);
            }
            if (results.result == false)
            {
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedBack.Warning("İşlem başarısız oldu."),
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new ResultStatusUI
            {
                Result = true,
                FeedBack = feedBack.Success("Sık Kullanılanlar güncellendi."),
            }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetFavorite(string data)
        {
            var db = new WorkOfTimeDatabase();
            var feedBack = new FeedBack();
            var results = new ResultStatus();
            results.result = true;
            var userStatus = (PageSecurity)Session["userStatus"];
            if (userStatus == null)
            {
                return Json(new ResultStatusUI
                {
                    Result = false,

                }, JsonRequestBehavior.AllowGet);
            }
            char[] controlParameter = { '/', ' ' };
            var dataControl = data.Split(controlParameter).Where(c => c != "").ToArray();
            var area = "";
            var controller = "";
            var method = "";
            if (dataControl.Count() == 0)
            {
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedBack.Warning("Anasayfa"),
                }, JsonRequestBehavior.AllowGet);
            }
            else if (dataControl.Count() == 1)
            {
                controller = dataControl[0];
                method = "Index";
            }
            else if (dataControl.Count() == 2)
            {
                controller = dataControl[0];
                method = dataControl[1];
            }
            else
            {
                area = dataControl[0];
                controller = dataControl[1];
                method = dataControl[2];
            }
            var datas = db.GetFVR_FavoritesControl(userStatus.user.id, area, controller, method);

            if (datas == null)
            {
                return Json(new ResultStatusUI
                {
                    Result = true,
                    Object = false
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new ResultStatusUI
                {
                    Result = true,
                    Object = datas.Status
                }, JsonRequestBehavior.AllowGet);
            }

        }
        public JsonResult GetAllMyFavorites(Guid? userId)
        {
            var db = new WorkOfTimeDatabase();
            var feedBack = new FeedBack();
            var userStatus = (PageSecurity)Session["userStatus"];
            var data = db.GetFVR_FavoritesByUserId(userId != null ? userId.Value : userStatus.user.id);

            var dataFavoriler = data.Where(c => c.Status == true).OrderByDescending(v => v.created).Take(7).ToArray();
            var dataSikKullanilanlar = data.Where(c => c.Status == false || c.Status == null).OrderByDescending(v => v.Count).Take(7).ToArray();
            var pages = db.GetSH_Pages();
            var resultFav = new Dictionary<string, object>();
            var resultFavorites = new Dictionary<string, object>();
            foreach (var item in dataFavoriler)
            {
                if (resultFav.Where(v => v.Key == item.Description).Count() > 0)
                {
                    continue;
                }
                if (item.Description != null)
                {
                    resultFav.Add(item.Description, item);
                }
                else
                {
                    var page = pages.Where(c => c.Action == item.Action).Select(b => b.Description).FirstOrDefault();
                    if (page != null)
                    {
                        if (resultFav.Where(x => x.Key == page).Count() > 0) { continue; }
                        resultFav.Add(page, item);
                    }
                }
            }

            foreach (var item2 in dataSikKullanilanlar)
            {
                if (resultFavorites.Where(v => v.Key == item2.Description).Count() > 0)
                {
                    continue;
                }
                if (item2.Description != null)
                {
                    resultFavorites.Add(item2.Description, item2);
                }
                else
                {
                    var page = pages.Where(c => c.Action == item2.Action).Select(b => b.Description).FirstOrDefault();
                    if (page != null)
                    {

                        if (resultFavorites.Where(x => x.Key == page).Count() > 0) { continue; }
                        resultFavorites.Add(page, item2);
                    }
                }
            }

            return Json(new ResultStatusUI
            {
                Result = true,
                FeedBack = feedBack.Success("İşlem başarı ile gerçekleşti."),
                Object = new { resultFavorites = resultFavorites.ToArray(), resultFav = resultFav.ToArray() }
            }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult UpdateFavoriteText(Guid id, string desc)
        {
            var db = new WorkOfTimeDatabase();
            var data = db.GetFVR_FavoritesById(id);
            data.Description = desc;
            var dbres = db.UpdateFVR_Favorites(data);
            return Json(new ResultStatusUI
            {
                Result = dbres.result,
            }, JsonRequestBehavior.AllowGet);
        }
        public ContentResult GetSH_UsersForPresentationById(Guid id)
        {
            var res = new CRM_Model().GetSH_UsersForPresentationById(id);
            return Content(Helper.Json.Serialize(res), "application/json");
        }
        public ContentResult GetCRM_ManagerStageCodeAndName()
        {
            var res = new WorkOfTimeDatabase().GetCRM_ManagerStage().OrderBy(a => a.Code).Select(a => new { Id = a.id, Name = a.Code + " - " + a.Name, color = a.color }).OrderBy(a => a.Name).ToArray();
            return Content(Infoline.Helper.Json.Serialize(res), "application/json");
        }

        public JsonResult GetCRM_ManagerStageName()
        {
            var db = new WorkOfTimeDatabase();
            var res = db.GetCRM_ManagerStage();
            return Json(res.Select(c => new { Id = c.id, Name = c.Name }).OrderBy(c => c.Name).ToArray(), JsonRequestBehavior.AllowGet);
        }

        public ContentResult GetPresentationViewById(Guid id)
        {
            var db = new WorkOfTimeDatabase();
            var presentation = db.GetVWCRM_PresentationById(id);
            return Content(Helper.Json.Serialize(presentation), "application/json");
        }
        public ContentResult GetContactViewById(Guid id)
        {
            var db = new WorkOfTimeDatabase();
            var contact = db.GetVWCRM_ContactById(id);
            var users = db.GetVWCRM_ContactUsersById(id);
            var data = new { Data = contact, Users = users };
            return Content(Helper.Json.Serialize(data), "application/json");
        }
        public ContentResult GetInvoiceDirectionEnums()
        {
            var data = EnumsProperties.EnumToArrayValues<EnumCMP_InvoiceDirectionType>().ToArray();
            return Content(Helper.Json.Serialize(data), "application/json");
        }
        public ContentResult GetInvoiceDiscountTypeEnums()
        {
            var data = EnumsProperties.EnumToArrayValues<EnumCMP_InvoiceItemDiscountType>().ToArray();
            return Content(Helper.Json.Serialize(data), "application/json");
        }
        public ContentResult GetInvoicePaymentTypeEnums()
        {
            var data = EnumsProperties.EnumToArrayValues<EnumCMP_InvoicePaymentType>().ToArray();
            return Content(Helper.Json.Serialize(data), "application/json");
        }
        public ContentResult GetInvoiceTypeEnums()
        {
            var data = EnumsProperties.EnumToArrayValues<EnumCMP_InvoiceType>().ToArray();
            return Content(Helper.Json.Serialize(data), "application/json");
        }
        public ContentResult GetInvoiceStatusEnums()
        {
            var data = EnumsProperties.EnumToArrayValues<EnumCMP_InvoiceStatus>().ToArray();
            return Content(Helper.Json.Serialize(data), "application/json");
        }
        public ContentResult GetVWUT_LocationByLocationInterSect(string location)
        {
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWUT_LocationGetByPoint(location);
            return Content(Helper.Json.Serialize(data), "application/json");
        }
        public ContentResult GetSuggestionsByText(string text)
        {
            var db = new WorkOfTimeDatabase();
            var suggestions = db.GetVWHDM_SuggestionByText(text);

            foreach (var suggestion in suggestions)
            {
                suggestion.content = HttpUtility.HtmlEncode(suggestion.content);
            }

            return Content(Helper.Json.Serialize(suggestions), "application/json");
        }
        public ContentResult GetSuggestionsByIssueId(Guid issueId)
        {
            var db = new WorkOfTimeDatabase();
            var suggestions = db.GetVWHDM_Suggestion();
            var issues = db.GetHDM_Issue().ToList();
            var filteredIssueIds = GetIssueIdsByParentIssueId(issueId, issues, new List<Guid>());
            var filteredSuggestions = suggestions.Where(a => a.issueId.HasValue && filteredIssueIds.Contains(a.issueId.Value)).ToArray();

            foreach (var suggestion in filteredSuggestions)
            {
                suggestion.content = HttpUtility.HtmlEncode(suggestion.content);
            }

            return Content(Helper.Json.Serialize(filteredSuggestions), "application/json");
        }
        public List<Guid> GetIssueIdsByParentIssueId(Guid parentIssueId, List<HDM_Issue> IssueList, List<Guid> IssueIds)
        {
            IssueIds.Add(parentIssueId);
            var selectedIssues = IssueList.Where(a => a.pid == parentIssueId).ToList();

            if (selectedIssues.Count() > 0)
            {
                foreach (var selectedIssue in selectedIssues)
                {
                    GetIssueIdsByParentIssueId(selectedIssue.id, IssueList, IssueIds);
                }
            }

            return IssueIds;
        }
        public ContentResult GetSysFilesByDataTableAndFileGroupAndDataId(string DataTable, string FileGroup, Guid DataId)
        {
            var db = new WorkOfTimeDatabase();
            var data = db.GetSysFilesFilePathByDataTableAndFileGroupAndDataId(DataTable, FileGroup, DataId);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }
        public JsonResult GetUserRole()
        {
            var db = new WorkOfTimeDatabase();
            var userRoles = db.GetVWSH_UserRole();
            var users = db.GetVWSH_UserByIds(userRoles.Where(a => a.userid.HasValue).Select(a => a.userid.Value).ToArray());

            var rs = userRoles.GroupBy(a => a.Role_Title).Select(c => new
            {
                name = c.Key,
                users = users.Where(b => c.GroupBy(g => g.userid).Select(g => g.Key).ToArray().Contains(b.id)).Select(d => new
                {
                    id = d.id,
                    fullName = d.FullName,
                    photo = d.ProfilePhoto,
                })
            }).OrderBy(d => d.name);

            return Json(rs, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetSH_UserRoleByUserId(Guid userId)
        {
            var db = new WorkOfTimeDatabase();
            var userRoles = db.GetSH_UserRoleByUserId(userId);
            return Json(userRoles.Where(x => x.roleid.HasValue).Select(x => x.roleid.Value).ToArray(), JsonRequestBehavior.AllowGet);
        }
        public ContentResult DataSourceEmailsCompanyAndUsers()
        {
            var db = new WorkOfTimeDatabase();
            var list = new List<string>();
            var users = db.GetVWSH_UserEmails().ToList();
            var companies = db.GetVWCMP_CompanyEmails().ToList();
            list.AddRange(users);
            list.AddRange(companies);
            return Content(Infoline.Helper.Json.Serialize(list.Distinct().ToArray()), "application/json");
        }
        public void SetUser(Guid id)
        {
            var userStatus = (PageSecurity)Session["userStatus"];
            if (userStatus.user.id == Guid.Empty || (Request.Url.AbsoluteUri.IndexOf("localhost") > -1 || Request.Url.AbsoluteUri.IndexOf("developer.workoftime.com") > -1 || Request.Url.AbsoluteUri.IndexOf("demo.workoftime.com") > -1))
            {
                var db = new WorkOfTimeDatabase();
                var user = db.GetSH_UserById(id);
                Session["userStatus"] = db.GetUserPageSecurityByUserid(user.id, Guid.Empty);
            }
        }
        public ContentResult SendCMP_Company(string name, Guid id)
        {
            var userStatus = (PageSecurity)Session["userStatus"];
            var db = new WorkOfTimeDatabase();

            var elem = new CMP_Company
            {
                id = id,
                createdby = userStatus.user.id,
                code = BusinessExtensions.B_GetIdCode(),
                name = name,
                type = (int)EnumCMP_CompanyType.Diger,
            };

            var res = db.InsertCMP_Company(elem);
            return Content(Infoline.Helper.Json.Serialize(new ResultStatus { result = res.result, objects = elem.id }), "application/json");
        }
        public ContentResult InsertStorages()
        {
            var db = new WorkOfTimeDatabase();
            var companies = db.GetCMP_Company();
            var storageList = new List<CMP_Storage>();

            foreach (var com in companies.Where(a => a.type == (int)EnumCMP_CompanyType.Diger))
            {
                storageList.Add(new CMP_Storage
                {
                    id = Guid.NewGuid(),
                    created = DateTime.Now,
                    createdby = com.createdby,
                    code = BusinessExtensions.B_GetIdCode(),
                    name = "Ana Şube/Depo/Kısım",
                    location = com.location,
                    locationId = com.openAddressLocationId,
                    companyId = com.id,
                    address = com.openAddress
                });
            }

            var res = db.BulkInsertCMP_Storage(storageList.ToArray());

            return Content(Infoline.Helper.Json.Serialize(new ResultStatus { result = res.result }), "application/json");
        }
        public ContentResult GetTime()
        {
            return Content(Infoline.Helper.Json.Serialize(DateTime.Now), "application/json");
        }

        public ContentResult GetIdCode()
        {
            return Content(Infoline.Helper.Json.Serialize(BusinessExtensions.B_GetIdCode()), "application/json");
        }

        public ContentResult GetFiles()
        {
            var link = Server.MapPath("/");
            var files1 = "\\Views\\Files";

            var asa = Directory.GetFiles(link, "*", SearchOption.AllDirectories).Where(a => a.Contains(files1));
            var files = Directory.GetFiles(link, "*", SearchOption.AllDirectories)
                .Where(a => a.ToLowerInvariant().IndexOf("\\log\\") < 0)
                .Where(a => a.ToLowerInvariant().IndexOf("\\files\\") < 0)
                .Where(a => a.ToLowerInvariant().IndexOf("\\language\\") < 0)
                .Where(a => a.ToLowerInvariant().IndexOf("microsoft.sqlserver.types.dll") < 0);
            files = files.Concat(asa);
            var info = files.ToDictionary(a => a.Replace(link, ""), a => new FileInfo(a).LastWriteTime);
            return Content(Infoline.Helper.Json.Serialize(info), "application/json");
        }
        public FileResult GetFile(string path)
        {
            var link = Server.MapPath("/");
            var fullPath = Path.Combine(link, path);
            return File(fullPath, System.Net.Mime.MediaTypeNames.Application.Octet, Path.GetFileName(fullPath));
        }
        public ContentResult UpdateCompanyCode()
        {
            var db = new WorkOfTimeDatabase();

            var companies = db.GetCMP_Company().Where(a => a.code == null);

            var list = new List<CMP_Company>();

            foreach (var company in companies)
            {
                company.code = BusinessExtensions.B_GetIdCode();
                list.Add(company);
                Thread.Sleep(100);
            }

            var res = db.BulkUpdateCMP_Company(list.ToArray());
            return Content(Infoline.Helper.Json.Serialize(new ResultStatus { result = res.result }), "application/json");
        }
        public ContentResult SetAccountCompany()
        {
            var userStatus = (PageSecurity)Session["userStatus"];
            var db = new WorkOfTimeDatabase();

            var companies = db.GetVWCMP_Company();
            var users = db.GetVWSH_UserMyPerson();

            var currencies = db.GetUT_Currency();

            var TL = currencies.Where(a => a.code == "TL").FirstOrDefault();

            var list = new List<PA_Account>();

            foreach (var company in companies)
            {
                list.Add(new PA_Account
                {
                    created = DateTime.Now,
                    createdby = userStatus.user.id,
                    code = BusinessExtensions.B_GetIdCode(),
                    currencyId = TL.id,
                    isDefault = true,
                    name = "Ana Hesap",
                    status = true,
                    type = (int)EnumPA_AccountType.Kasa,
                    dataId = company.id,
                    dataTable = "CMP_Company"
                });

                Thread.Sleep(100);
            }

            foreach (var user in users)
            {
                list.Add(new PA_Account
                {
                    created = DateTime.Now,
                    createdby = userStatus.user.id,
                    code = BusinessExtensions.B_GetIdCode(),
                    currencyId = TL.id,
                    isDefault = true,
                    name = "Ana Hesap",
                    status = true,
                    type = (int)EnumPA_AccountType.Kasa,
                    dataId = user.id,
                    dataTable = "SH_User"
                });

                Thread.Sleep(100);
            }

            var res = db.BulkInsertPA_Account(list.ToArray());
            return Content(Infoline.Helper.Json.Serialize(new ResultStatus { result = res.result }), "application/json");
        }
        public ContentResult SetDefaultAccountAndStorage()
        {
            var userStatus = (PageSecurity)Session["userStatus"];
            var db = new WorkOfTimeDatabase();

            var companies = db.GetCMP_Company();
            var users = db.GetVWSH_UserMyPerson();
            var account = db.GetPA_Account();
            var currencies = db.GetUT_Currency();
            var storage = db.GetCMP_Storage();
            var TL = currencies.Where(a => a.code == "TL").FirstOrDefault();

            var listAccount = new List<PA_Account>();
            var listStorage = new List<CMP_Storage>();
            var listReport = new List<String>();
            foreach (var company in companies)
            {
                var hasAccount = account.Where(x => x.dataId == company.id && x.dataTable == "CMP_Company").FirstOrDefault();
                var hasStorage = storage.Where(x => x.companyId == company.id).FirstOrDefault();
                if (hasAccount == null)
                {
                    listAccount.Add(new PA_Account
                    {
                        created = DateTime.Now,
                        createdby = userStatus.user.id,
                        code = BusinessExtensions.B_GetIdCode(),
                        currencyId = TL.id,
                        isDefault = true,
                        name = "Ana Hesap",
                        status = true,
                        type = (int)EnumPA_AccountType.Kasa,
                        dataId = company.id,
                        dataTable = "CMP_Company"
                    });
                    listReport.Add(company.name + " adlı şirkete banka hesabı ");
                }
                if (hasStorage == null)
                {
                    listStorage.Add(new CMP_Storage
                    {
                        id = Guid.NewGuid(),
                        created = DateTime.Now,
                        createdby = company.createdby,
                        code = BusinessExtensions.B_GetIdCode(),
                        name = "Ana Şube/Depo/Kısım",
                        location = company.location,
                        locationId = company.openAddressLocationId,
                        companyId = company.id,
                        address = company.openAddress
                    });
                    listReport.Add(company.name + " adlı şirkete depo ");
                }
            }

            foreach (var user in users)
            {
                var hasAccount = account.Where(x => x.dataId == user.id && x.dataTable == "SH_User").FirstOrDefault();
                if (hasAccount == null)
                {
                    listAccount.Add(new PA_Account
                    {
                        created = DateTime.Now,
                        createdby = userStatus.user.id,
                        code = BusinessExtensions.B_GetIdCode(),
                        currencyId = TL.id,
                        isDefault = true,
                        name = "Ana Hesap",
                        status = true,
                        type = (int)EnumPA_AccountType.Kasa,
                        dataId = user.id,
                        dataTable = "SH_User"
                    });
                    listReport.Add(user.firstname + " " + user.lastname + " adlı personele banka hesabı ");
                }
            }
            var trans = db.BeginTransaction();
            var res = db.BulkInsertPA_Account(listAccount.ToArray(), trans);
            res &= db.BulkInsertCMP_Storage(listStorage.ToArray(), trans);
            var report = "";
            if (listReport.Count > 0)
            {
                report = String.Join(",", listReport) + " tanımlandı";
            }

            if (!res.result)
            {
                trans.Rollback();
                return Content(Infoline.Helper.Json.Serialize(new ResultStatus { result = res.result, message = "Başarısız İşlem Gerçekleşti" }), "application/json");
            }

            trans.Commit();
            return Content(Infoline.Helper.Json.Serialize(new ResultStatus { result = res.result, message = listAccount.Count() + " adet hesap eklendi. " + listStorage.Count() + " şirkete depo tanımlandı." + report }), "application/json");
        }
        public JsonResult GetRandomUserPush(int sayi)
        {
            var db = new WorkOfTimeDatabase();
            var feedback = new FeedBack();

            if (sayi > 5000)
            {
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Warning("Maksimum 5.000 adet ekleme gerçekleştirebilirsiniz.")
                }, JsonRequestBehavior.AllowGet);
            }
            var userStatus = (PageSecurity)Session["userStatus"];
            var data = new List<HR_Person>();
            var dataKeywords = new List<HR_PersonKeywords>();
            var dataPosition = new List<HR_PersonPosition>();
            var sysDataS = new List<SYS_Files>();
            WebClient client = new WebClient();
            client.Encoding = Encoding.UTF8;
            string JsonData = client.DownloadString(new Uri("https://randomuser.me/api/?nat=tr&results=" + sayi));
            var Result = Infoline.Helper.Json.Deserialize<MyUserPushData>(JsonData);
            Random random = new Random();
            var city = db.GetUT_Location().ToArray();
            var keywords = db.GetHR_Keywords().ToArray();
            var position = db.GetHR_Position().ToArray();

            for (int i = 0; i < Result.results.Count(); i++)
            {
                var randsayi = random.Next(1, 81);
                var RandomCity = city[randsayi].id;
                var randSonsayi = random.Next(1, 3);
                var newData = new HR_Person
                {
                    id = Guid.NewGuid(),
                    created = DateTime.Now,
                    createdby = userStatus.user.id,
                    LocationId = RandomCity,
                    Birthday = Result.results[i].dob.date,
                    Education = random.Next(1, 4),
                    Email = Result.results[i].email.TurkceReplaceForMail(),
                    Phone = "0" + Result.results[i].phone.TurkceReplaceForMail().Replace("(", "").Replace(")", ""),
                    ExprienceYear = random.Next(0, 5),
                    Name = Result.results[i].name.first.ToUpper(),
                    SurName = Result.results[i].name.last.ToUpper(),
                    MilitaryStatus = Result.results[i].gender == "male" ? (Int32)EnumHR_PersonMilitaryStatus.Yapildi : (Int32)EnumHR_PersonMilitaryStatus.Muaf,
                };
                data.Add(newData);

                var files = new SYS_Files
                {
                    id = Guid.NewGuid(),
                    created = DateTime.Now,
                    createdby = userStatus.user.id,
                    DataId = newData.id,
                    FileGroup = "Fotoğraf",
                    DataTable = "HR_Person",
                    FileExtension = "jpg",
                    FilePath = Result.results[i].picture.large
                };
                sysDataS.Add(files);

                for (int a = 0; a < 5; a++)
                {
                    var randKey = random.Next(1, keywords.Count());
                    var RandomKey = keywords[randKey].id;

                    var newdataKeywords = new HR_PersonKeywords
                    {
                        id = Guid.NewGuid(),
                        created = DateTime.Now,
                        createdby = userStatus.user.id,
                        HrPersonId = newData.id,
                        HrKeywordsId = RandomKey
                    };
                    dataKeywords.Add(newdataKeywords);
                }

                for (int b = 0; b < 2; b++)
                {
                    var randPos = random.Next(1, position.Count());
                    var RandomPos = position[randPos].id;
                    var newdataPosition = new HR_PersonPosition
                    {
                        id = Guid.NewGuid(),
                        created = DateTime.Now,
                        createdby = userStatus.user.id,
                        HrPersonId = newData.id,
                        HrPositionId = RandomPos
                    };
                    dataPosition.Add(newdataPosition);
                }
            }

            var trans = db.BeginTransaction();
            var dbres = db.BulkInsertHR_Person(data, trans);
            dbres &= db.BulkInsertHR_PersonKeywords(dataKeywords, trans);
            dbres &= db.BulkInsertHR_PersonPosition(dataPosition, trans);
            dbres &= db.BulkInsertSYS_Files(sysDataS, trans);

            if (dbres.result == false)
            {
                trans.Rollback();
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Error("Cv havuzuna kullanıcı kaydı gerçekleştirirken hata oluştu.")

                }, JsonRequestBehavior.AllowGet);
            }

            trans.Commit();

            return Json(new ResultStatusUI
            {
                Result = true,
                FeedBack = feedback.Success("Cv havuzuna kayıt atma işlemi başarı ile gerçekleşti.")

            }, JsonRequestBehavior.AllowGet);
        }
        public string RemoteConnectionValue(string catalog, string host, string user, string pass)
        {
            var remoteValue = "Data Source=" + host + ";Initial Catalog=" + catalog + ";User ID=" + user + ";Password=" + pass + "";
            var cryp = new CryptographyHelper().Encrypt(remoteValue);
            return cryp;
        }

        public JsonResult GetGroupUsers(Guid id)
        {
            var db = new WorkOfTimeDatabase();
            var users = db.GetSH_GroupUsersByGroupId(id);
            return Json(users, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetProductMaterials(Guid productId)
        {

            var db = new WorkOfTimeDatabase();
            var materials = db.GetVWPRD_ProductMaterialByProductId(productId).ToList().OrderBy(x => x.materialId_Title);
            return Json(materials, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetProductionProducts(Guid productId, Guid productionId)
        {
            var db = new WorkOfTimeDatabase();
            var materials = db.GetVWPRD_ProductionProductByProductIdAndProductionId(productId, productionId);
            return Json(materials, JsonRequestBehavior.AllowGet);
        }

        public ContentResult GetYears()
        {
            var maxYear = DateTime.Now.Year + 1;
            var minYear = DateTime.Now.Year - 30;
            var years = new List<object>();
            for (int i = maxYear; i >= minYear; i--)
            {
                years.Add(new { id = i, title = i });
            }
            return Content(Infoline.Helper.Json.Serialize(years), "application/json");
        }

        public ContentResult GetMonths()
        {
            var months = new List<object>();
            months.Add(new { id = 1, title = "Ocak" });
            months.Add(new { id = 2, title = "Şubat" });
            months.Add(new { id = 3, title = "Mart" });
            months.Add(new { id = 4, title = "Nisan" });
            months.Add(new { id = 5, title = "Mayıs" });
            months.Add(new { id = 6, title = "Haziran" });
            months.Add(new { id = 7, title = "Temmuz" });
            months.Add(new { id = 8, title = "Ağustos" });
            months.Add(new { id = 9, title = "Eylül" });
            months.Add(new { id = 10, title = "Ekim" });
            months.Add(new { id = 11, title = "Kasım" });
            months.Add(new { id = 12, title = "Aralık" });

            return Content(Infoline.Helper.Json.Serialize(months), "application/json");
        }


        public ContentResult GetTimes()
        {
            var months = new List<object>();
            months.Add(new { id = "00.00", title = "00.00" });
            months.Add(new { id = "00.30", title = "00.30" });
            months.Add(new { id = "01.00", title = "01.00" });
            months.Add(new { id = "01.30", title = "01.30" });
            months.Add(new { id = "02.00", title = "02.00" });
            months.Add(new { id = "02.30", title = "02.30" });
            months.Add(new { id = "03.00", title = "03.00" });
            months.Add(new { id = "03.30", title = "03.30" });
            months.Add(new { id = "04.00", title = "04.00" });
            months.Add(new { id = "04.30", title = "04.30" });
            months.Add(new { id = "05.00", title = "05.00" });
            months.Add(new { id = "05.30", title = "05.30" });
            months.Add(new { id = "06.00", title = "06.00" });
            months.Add(new { id = "06.30", title = "06.30" });
            months.Add(new { id = "07.00", title = "07.00" });
            months.Add(new { id = "07.30", title = "07.30" });
            months.Add(new { id = "08.00", title = "08.00" });
            months.Add(new { id = "08.30", title = "08.30" });
            months.Add(new { id = "09.00", title = "09.00" });
            months.Add(new { id = "09.30", title = "09.30" });
            months.Add(new { id = "10.00", title = "10.00" });
            months.Add(new { id = "10.30", title = "10.30" });
            months.Add(new { id = "11.00", title = "11.00" });
            months.Add(new { id = "11.30", title = "11.30" });
            months.Add(new { id = "12.00", title = "12.00" });
            months.Add(new { id = "12.30", title = "12.30" });
            months.Add(new { id = "13.00", title = "13.00" });
            months.Add(new { id = "13.30", title = "13.30" });
            months.Add(new { id = "14.00", title = "14.00" });
            months.Add(new { id = "14.30", title = "14.30" });
            months.Add(new { id = "15.00", title = "15.00" });
            months.Add(new { id = "15.30", title = "15.30" });
            months.Add(new { id = "16.00", title = "16.00" });
            months.Add(new { id = "16.30", title = "16.30" });
            months.Add(new { id = "17.00", title = "17.00" });
            months.Add(new { id = "17.30", title = "17.30" });
            months.Add(new { id = "18.00", title = "18.00" });
            months.Add(new { id = "18.30", title = "18.30" });
            months.Add(new { id = "19.00", title = "19.00" });
            months.Add(new { id = "19.30", title = "19.30" });
            months.Add(new { id = "20.00", title = "20.00" });
            months.Add(new { id = "20.30", title = "20.30" });
            months.Add(new { id = "21.00", title = "21.00" });
            months.Add(new { id = "21.30", title = "21.30" });
            months.Add(new { id = "22.00", title = "22.00" });
            months.Add(new { id = "22.30", title = "22.30" });
            months.Add(new { id = "23.00", title = "23.00" });
            months.Add(new { id = "23.30", title = "23.30" });



            return Content(Infoline.Helper.Json.Serialize(months), "application/json");
        }

        public ContentResult GetDays()
        {
            var months = new List<object>();
            months.Add(new { id = 1, title = "Pazartesi" });
            months.Add(new { id = 2, title = "Salı" });
            months.Add(new { id = 3, title = "Çarşamba" });
            months.Add(new { id = 4, title = "Perşembe" });
            months.Add(new { id = 5, title = "Cuma" });
            months.Add(new { id = 6, title = "Cumartesi" });
            months.Add(new { id = 7, title = "Pazar" });

            return Content(Infoline.Helper.Json.Serialize(months), "application/json");
        }
    }
}