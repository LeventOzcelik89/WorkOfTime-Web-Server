using Infoline.Framework.Database;
using Infoline.Helper;
using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessAccess.Models;
using Infoline.WorkOfTime.BusinessData;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Infoline.WorkOfTime.WebProject.Areas.PA.Controllers
{
    public class VWPA_TransactionController : Controller
    {
        [PageInfo("Gelir Listesi", SHRoles.OnMuhasebe)]
        public ActionResult IndexIncome()
        {
            return View();
        }

        [PageInfo("Gider Listesi", SHRoles.OnMuhasebe)]
        public ActionResult IndexExpense()
        {
            return View();
        }

        [PageInfo("Nakit Akış Raporu", SHRoles.OnMuhasebe)]
        public ActionResult Report()
        {
            var db = new WorkOfTimeDatabase();

            var model = new VMPA_TransactionReport
            {
                Transactions = db.GetVWPA_Transaction(),
                Ledgers = db.GetVWPA_Ledger()
            };

            return View(model);
        }

        [PageInfo("Ödeme Planı Methodu", SHRoles.Personel)]
        public ContentResult DataSource([DataSourceRequest] DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);

            var page = request.Page;
            request.Filters = new FilterDescriptor[0];
            request.Sorts = new SortDescriptor[0];
            request.Page = 1;
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWPA_Transaction(condition).RemoveGeographies().ToDataSourceResult(request);
            data.Total = db.GetVWPA_TransactionCount(condition.Filter);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }
        // TO DO : ÜNDEMİR - DÜZELTİLECEK.
        [PageInfo("Ödeme Planı Methodu", SHRoles.Personel)]
        public ContentResult DataSourceSpesific([DataSourceRequest] DataSourceRequest request, Guid? dataId, Guid? corporationId)
        {
            var condition = KendoToExpression.Convert(request);
            var db = new WorkOfTimeDatabase();

            if (dataId.HasValue && corporationId.HasValue)
            {
                var tasks = db.GetFTM_TaskByCustomerId(corporationId.Value);

                BEXP filter = null;

                if (tasks.Count() > 0)
                {
                    var taskIds = tasks.Select(x => (Guid?)x.id).ToList();
                    filter &= new BEXP
                    {
                        Operand1 = new BEXP
                        {
                            Operand1 = new BEXP
                            {
                                Operand1 = (COL)"type",
                                Operator = BinaryOperator.Equal,
                                Operand2 = (VAL)(int)EnumPA_TransactionType.Masraf
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
                                Operand1 = (COL)"dataId",
                                Operator = BinaryOperator.Equal,
                                Operand2 = (VAL)dataId.Value
                            },
                            Operand2 = new BEXP
                            {
                                Operand1 = (COL)"dataId",
                                Operator = BinaryOperator.In,
                                Operand2 = new ARR { Values = taskIds.Select(a => (VAL)a).ToArray() }
                            },
                            Operator = BinaryOperator.Or
                        },
                        Operator = BinaryOperator.And
                    };
                }
                else
                {
                    filter &= new BEXP
                    {
                        Operand1 = (COL)"type",
                        Operator = BinaryOperator.Equal,
                        Operand2 = (VAL)(int)EnumPA_TransactionType.Masraf
                    };
                    filter &= new BEXP
                    {
                        Operand1 = (COL)"dataId",
                        Operator = BinaryOperator.Equal,
                        Operand2 = (VAL)dataId.Value
                    };
                }

                condition.Filter &= filter;
            }

            var page = request.Page;
            request.Filters = new FilterDescriptor[0];
            request.Sorts = new SortDescriptor[0];
            request.Page = 1;
            var data = db.GetVWPA_Transaction(condition).RemoveGeographies().ToDataSourceResult(request);
            data.Total = db.GetVWPA_TransactionCount(condition.Filter);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }



        [PageInfo("Şirket Kasa ve Banka Hesap Tanımları Methodu", SHRoles.Personel)]
        public int DataSourceCount([DataSourceRequest] DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);
            var db = new WorkOfTimeDatabase();
            var total = db.GetVWPA_TransactionCount(condition.Filter);
            return total;
        }

        [PageInfo("Ödeme Planı Veri Methodu", SHRoles.Personel)]
        public ContentResult DataSourceDropDown([DataSourceRequest] DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);

            var db = new WorkOfTimeDatabase();
            var data = db.GetVWPA_Transaction(condition);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }

        [PageInfo("Gider Detay Sayfası", SHRoles.Personel)]
        public ActionResult DetailExpense(Guid id)
        {
            var model = new VMPA_TransactionModel { id = id }.Load();
            model.direction = (int)EnumPA_TransactionDirection.Cikis;
            return View(model);
        }

        [PageInfo("Gelir Detay Sayfası", SHRoles.OnMuhasebe)]
        public ActionResult DetailIncome(Guid id)
        {
            var model = new VMPA_TransactionModel { id = id }.Load();
            model.direction = (int)EnumPA_TransactionDirection.Giris;
            return View(model);
        }

        [PageInfo("Masraf Talepleri (Onay)", SHRoles.Personel)]
        public ActionResult IndexRequest()
        {
            return View();
        }

        [PageInfo("Tüm Masraf Talepleri", SHRoles.MuhasebeAlis, SHRoles.MuhasebeSatis, SHRoles.OnMuhasebe)]
        public ActionResult IndexAllRequest(Guid? id)
        {
            ViewBag.TransactionId = id;
            return View();
        }
        [AllowEveryone]
        [PageInfo("Idlere Göre Tüm Masraf  Talepleri ", SHRoles.MuhasebeAlis, SHRoles.MuhasebeSatis, SHRoles.OnMuhasebe)]
        public ActionResult IndexRequestByIds(string ids)
        {
            var returning = new List<Guid?>();
            if (!string.IsNullOrEmpty(ids))
            {
                foreach (var item in ids.Split(','))
                {
                    returning.Add(Guid.Parse(item));
                }
            }
            

            return View(returning);
        }

        [PageInfo("Masraf Taleplerim", SHRoles.Personel)]
        public ActionResult IndexMyRequest(Guid? id)
        {
            ViewBag.TransactionId = id;
            return View();
        }

        [PageInfo("Gider Ekleme Sayfası", SHRoles.Personel)]
        public ActionResult InsertExpense(VMPA_TransactionModel item)
        {
            var userStatus = (PageSecurity)Session["userStatus"];
            if (item.dataId.HasValue)
            {
                item.id = Guid.NewGuid();
            }
            item.Load();
            item.type_Title = item.type_Title ?? Infoline.Helper.EnumsProperties.EnumToArrayGeneric<EnumPA_TransactionType>().Where(a => Convert.ToInt16(a.Key) == item.type).Select(a => a.Value).FirstOrDefault();
            item.direction = item.type == (int)EnumPA_TransactionType.Masraf ? (short)EnumPA_TransactionDirection.Talep : (short)EnumPA_TransactionDirection.Cikis;

            return View(item);
        }

        [PageInfo("Gider Düzenleme Sayfası", SHRoles.Personel)]
        public ActionResult UpdateExpense(Guid id, bool? IsCopy = false)
        {
            var db = new WorkOfTimeDatabase();
            var model = new VMPA_TransactionModel { id = id, IsCopy = IsCopy }.Load();
            model.Ledgers = db.GetPA_LedgerByTransactionId(id);
            model.status = (int)EnumPA_TransactionStatus.Odenecek;
            return View(model);
        }

        [PageInfo("Gelir Ekleme Sayfası", SHRoles.OnMuhasebe)]
        public ActionResult InsertIncome(VMPA_TransactionModel item)
        {
            item.Load();
            item.direction = (int)EnumPA_TransactionDirection.Giris;
            return View(item);
        }

        [HttpPost, ValidateAntiForgeryToken]
        [PageInfo("Ödeme Planı Ekleme Sayfası", SHRoles.Personel)]
        public JsonResult Insert(VMPA_TransactionModel item)
        {
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();

            var dbresult = item.Save(userStatus.user.id, Request);

            return Json(new ResultStatusUI
            {
                Result = dbresult.result,
                Object = item.id,
                FeedBack = dbresult.result ? feedback.Success(dbresult.message) : feedback.Warning(dbresult.message)
            }, JsonRequestBehavior.AllowGet);
        }

        [PageInfo("Ödeme Planı Güncelleme Sayfası", SHRoles.Personel)]
        public ActionResult Update(Guid id)
        {
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWPA_TransactionById(id);
            return View(data);
        }

        [HttpPost, ValidateAntiForgeryToken]
        [PageInfo("Ödeme Planı Güncelleme Sayfası", SHRoles.Personel)]
        public JsonResult Update(VMPA_TransactionModel item)
        {
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            var dbresult = item.Save(userStatus.user.id, Request);

            return Json(new ResultStatusUI
            {
                Result = dbresult.result,
                Object = item.id,
                FeedBack = dbresult.result ? feedback.Success(dbresult.message) : feedback.Warning(dbresult.message)
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [PageInfo("Ödeme Onaylama Methodu", SHRoles.Personel)]
        public JsonResult AllApprovalTrans([DataSourceRequest] DataSourceRequest request, string ids)
        {
            var db = new WorkOfTimeDatabase();
            var feedBack = new FeedBack();
            var userStatus = (PageSecurity)Session["userStatus"];
            var condition = KendoToExpression.Convert(request);
            condition.Take = 99999;
            condition.Skip = 0;
            var transactions = new List<VWPA_Transaction>();
            if (string.IsNullOrEmpty(ids))
            {
                transactions = db.GetVWPA_Transaction(condition).ToList();
            }
            else
            {
                var transIds = ids.Split(',').Select(a => Guid.Parse(a)).ToArray();
                transactions = db.GetVWPA_TransactionByIds(transIds).ToList();
            }

            transactions = transactions.Where(x => (x.confirmationStatus == (Int16)EnumPA_TransactionConfirmationStatus.Onay || x.confirmationStatus == null) || (x.confirmUserIds.Contains(userStatus.user.id.ToString()) && x.direction == (Int16)EnumPA_TransactionDirection.Talep && x.type == (Int16)EnumPA_TransactionType.Masraf)).ToList();

            if (transactions.Count() == 0)
            {
                return Json(new ResultStatusUI { Result = false, FeedBack = feedBack.Warning("Onaylanacak masraf talebi bulunamadı.") }, JsonRequestBehavior.AllowGet);
            }

            var res = new ResultStatus { result = true };
            foreach (var transaction in transactions)
            {
                transaction.direction = -1;
                transaction.dataId = null;
                res &= new VMPA_TransactionModel().B_EntityDataCopyForMaterial(transaction).Save(userStatus.user.id);
            }

            return Json(new ResultStatusUI { Result = res.result, FeedBack = res.result ? feedBack.Success("Masraf talepleri başarı ile onaylandı") : feedBack.Warning("Masraf onaylama işlemi başarısız oldu.") }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [PageInfo("Ödeme Planı Silme İşlemi", SHRoles.OnMuhasebe)]
        public JsonResult Delete(Guid id)
        {
            var feedback = new FeedBack();
            var rs = new VMPA_TransactionModel { id = id }.Load().Delete();

            return Json(new ResultStatusUI
            {
                Result = rs.result,
                FeedBack = rs.result ? feedback.Success("Silme işlemi başarılı") : feedback.Error("Silme işlemi başarılı")
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [PageInfo("Excel'den Cari Ekleme", SHRoles.IKYonetici)]
        public JsonResult Import(string model)
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            var culture = new System.Globalization.CultureInfo("tr-TR", false);

            var currencies = db.GetUT_Currency();
            var companies = db.GetCMP_Company().ToList();
            var accounts = db.GetPA_Account().ToList();
            var locations = db.GetUT_LocationCityAndTownInTR();
            var TL = currencies.Where(a => a.code == "TL").FirstOrDefault();

            var excelTransactions = Helper.Json.Deserialize<PA_TransactionLedgerExcel[]>(model);

            var enumArray = EnumsProperties.EnumToArrayGeneric<EnumPA_TransactionLedgerExcel>().ToArray();

            var defaultAccount = db.GetVWPA_AccountMy().FirstOrDefault();

            var existError = new List<ExcelResult>();
            var excelResult = new ExcelResult
            {
                status = true,
                rowNumber = 0
            };

            foreach (var excelTrans in excelTransactions.Select((pa_trans, index) => new { pa_trans = pa_trans, index = index }))
            {
                excelResult.rowNumber++;

                var item = excelTrans.pa_trans;

                if (String.IsNullOrEmpty(item.companyCode))
                {
                    excelResult.status = false;
                    excelResult.message += " İşletme kodu alanı zorunludur";
                }

                if (String.IsNullOrEmpty(item.actionType))
                {
                    excelResult.status = false;
                    excelResult.message += " İşlem Tipi alanı zorunludur";
                }

                if (item.credit.HasValue && item.debt.HasValue)
                {
                    excelResult.status = false;
                    excelResult.message += " Borç-Alacaktan sadece biri girilmelidir";
                }

                if (!item.credit.HasValue && !item.debt.HasValue)
                {
                    excelResult.status = false;
                    excelResult.message += " Borç-Alacaktan bir tanesi girilmelidir";
                }

                if (String.IsNullOrEmpty(item.currency))
                {
                    excelResult.status = false;
                    excelResult.message += " Para Birimi alanı zorunludur";
                }

                if (!item.date.HasValue)
                {
                    excelResult.status = false;
                    excelResult.message += " Tarih alanı zorunludur";
                }

                var enumType = enumArray.Where(a => a.Value.ToLower(culture).Contains(item.actionType.ToLower(culture)) || a.Key.ToLower(culture).Contains(item.actionType.ToLower(culture))).FirstOrDefault();

                if (enumType == null)
                {
                    excelResult.status = false;
                    excelResult.message += " İşlem Tipi yanlış girilmiş olabilir.";
                }

                if (excelResult.status)
                {
                    var trans = db.BeginTransaction();
                    var res = new ResultStatus { result = true };

                    var company = companies.Where(a => a.code.Trim().ToLower(culture) == item.companyCode.Trim().ToLower(culture)).FirstOrDefault();

                    if (company == null)
                    {
                        company = new CMP_Company
                        {
                            id = Guid.NewGuid(),
                            created = DateTime.Now,
                            createdby = userStatus.user.id,
                            code = item.companyCode,
                            commercialTitle = item.companyName,
                            name = item.companyName,
                            taxType = (short)EnumCMP_CompanyTaxType.TuzelKisi,
                            type = (short)EnumCMP_CompanyType.Diger
                        };

                        companies.Add(company);
                        res &= db.InsertCMP_Company(company, trans);
                    }

                    var account = accounts.Where(a => a.dataId == company.id).FirstOrDefault();

                    if (account == null)
                    {
                        account = new PA_Account()
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
                        };

                        accounts.Add(account);
                        res &= db.InsertPA_Account(account, trans);
                    }

                    var enumKey = Convert.ToInt16(enumType.Key);

                    var currency = currencies.Where(a => a.code.ToLower(culture) == item.currency.ToLower(culture) || a.name.ToLower(culture) == item.currency.ToLower(culture)).FirstOrDefault();

                    var crossRate = 1.00;
                    if (currency != null)
                    {
                        var code = currency.code == "TL" ? "TRY" : currency.code;
                        crossRate = CurrencyExchangeRates.GetExchangeRatesByDate(code, item.date.Value).BanknoteSelling;
                    }

                    var transaction = new PA_Transaction
                    {
                        id = Guid.NewGuid(),
                        created = DateTime.Now,
                        createdby = userStatus.user.id,
                        accountId = account.id,
                        currencyId = currency != null ? currency.id : TL.id,
                        dataId = company.id,
                        dataTable = "CMP_Company",
                        description = item.description,
                        progressDate = item.expiryDate,
                        date = item.date,
                        status = (short)EnumPA_TransactionStatus.Odenecek,
                        amount = item.credit.HasValue ? item.credit.Value : item.debt.Value,
                        tax = 0
                    };

                    var ledgerIn = new PA_Ledger
                    {
                        id = Guid.NewGuid(),
                        created = DateTime.Now,
                        createdby = userStatus.user.id,
                        accountId = defaultAccount.id,
                        accountRealtedId = account.id,
                        currencyId = currency != null ? currency.id : TL.id,
                        date = item.date,
                        amount = item.credit.HasValue ? item.credit.Value : item.debt.Value,
                        description = item.description,
                        direction = item.credit.HasValue ? (short)EnumPA_TransactionDirection.Giris : (short)EnumPA_TransactionDirection.Cikis,
                        crossRate = crossRate,
                        rateExchange = crossRate,
                        tax = 0
                    };

                    var ledgerOut = new PA_Ledger
                    {
                        id = Guid.NewGuid(),
                        created = DateTime.Now,
                        createdby = userStatus.user.id,
                        accountId = account.id,
                        accountRealtedId = defaultAccount.id,
                        currencyId = currency != null ? currency.id : TL.id,
                        date = item.date,
                        amount = item.credit.HasValue ? item.credit.Value : item.debt.Value,
                        description = item.description,
                        direction = item.debt.HasValue ? (short)EnumPA_TransactionDirection.Giris : (short)EnumPA_TransactionDirection.Cikis,
                        crossRate = crossRate,
                        rateExchange = crossRate,
                    };

                    switch (enumKey)
                    {
                        case (short)EnumPA_TransactionLedgerExcel.AlacakDekont:
                            transaction.direction = (short)EnumPA_TransactionDirection.Cikis;
                            transaction.type = (short)EnumPA_TransactionType.AlacakDekont;
                            break;
                        case (short)EnumPA_TransactionLedgerExcel.BorcDekont:
                            transaction.direction = (short)EnumPA_TransactionDirection.Giris;
                            transaction.type = (short)EnumPA_TransactionType.BorcDekont;
                            break;
                        case (short)EnumPA_TransactionLedgerExcel.AlisFatura:
                            transaction.direction = (short)EnumPA_TransactionDirection.Cikis;
                            transaction.type = (short)EnumPA_TransactionType.AlisFatura;
                            break;
                        case (short)EnumPA_TransactionLedgerExcel.SatisFatura:
                            transaction.direction = (short)EnumPA_TransactionDirection.Giris;
                            transaction.type = (short)EnumPA_TransactionType.SatisFatura;
                            break;
                        case (short)EnumPA_TransactionLedgerExcel.CekGiris:
                            transaction.direction = (short)EnumPA_TransactionDirection.Cikis;
                            transaction.type = (short)EnumPA_TransactionType.Cek;
                            break;
                        case (short)EnumPA_TransactionLedgerExcel.CekCikis:
                            transaction.direction = (short)EnumPA_TransactionDirection.Giris;
                            transaction.type = (short)EnumPA_TransactionType.Cek;
                            break;
                        case (short)EnumPA_TransactionLedgerExcel.SatisİadeFatura:
                            transaction.direction = (short)EnumPA_TransactionDirection.Cikis;
                            transaction.type = (short)EnumPA_TransactionType.SatisİadeFatura;
                            break;
                        case (short)EnumPA_TransactionLedgerExcel.SenetCikis:
                            transaction.direction = (short)EnumPA_TransactionDirection.Giris;
                            transaction.type = (short)EnumPA_TransactionType.Senet;
                            break;
                        case (short)EnumPA_TransactionLedgerExcel.SenetGiris:
                            transaction.direction = (short)EnumPA_TransactionDirection.Cikis;
                            transaction.type = (short)EnumPA_TransactionType.Senet;
                            break;
                        case (short)EnumPA_TransactionLedgerExcel.Tahsil:
                            ledgerIn.type = (short)EnumPA_LedgerType.Tahsilat;
                            ledgerOut.type = (short)EnumPA_LedgerType.Odeme;
                            break;
                        case (short)EnumPA_TransactionLedgerExcel.Odeme:
                            ledgerIn.type = (short)EnumPA_LedgerType.Odeme;
                            ledgerOut.type = (short)EnumPA_LedgerType.Tahsilat;
                            break;
                        default:
                            break;
                    }

                    if (transaction.type.HasValue)
                    {
                        res &= db.InsertPA_Transaction(transaction, trans);
                    }

                    if (ledgerIn.type.HasValue)
                    {
                        res &= db.InsertPA_Ledger(ledgerIn, trans);
                    }

                    if (ledgerOut.type.HasValue)
                    {
                        res &= db.InsertPA_Ledger(ledgerOut, trans);
                    }

                    if (res.result)
                    {
                        trans.Commit();
                    }
                    else
                    {
                        trans.Rollback();
                        excelResult.status = false;
                        excelResult.message += " Bir sorun oluştu";
                    }
                }

                existError.Add(excelResult);
                excelResult = new ExcelResult
                {
                    status = true,
                    rowNumber = excelResult.rowNumber
                };
            }

            if (existError.Where(a => a.status == false).Count() == excelTransactions.Length)
            {
                return Json(new ResultStatusUI
                {
                    Result = true,
                    FeedBack = feedback.Error("Kaydetme işlemi başarısız."),
                    Object = existError,
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new ResultStatusUI
                {
                    Result = true,
                    FeedBack = feedback.Success("Kaydetme işlemi başarılı." + (existError.Where(a => a.status == false).Count() > 0 ? "Bazı kayıtlarda problem oluştu." : "")),
                    Object = existError.Where(a => a.status == false).ToArray(),
                }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}


