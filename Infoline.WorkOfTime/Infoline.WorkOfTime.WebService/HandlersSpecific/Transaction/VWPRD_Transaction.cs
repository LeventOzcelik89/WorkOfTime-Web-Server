using Infoline.Framework.Database;
using Infoline.Web.SmartHandlers;
using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData.Specific;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Infoline.WorkOfTime.WebService.HandlersSpecific
{
    [Export(typeof(ISmartHandler))]
    public partial class VWPRD_TransactionHandler : BaseSmartHandler
    {
        public VWPRD_TransactionHandler()
            : base("VWPRD_Transaction")
        {

        }

        [HandleFunction("VWPRD_Transaction/Upsert")]
        public void VWPRD_TransactionUpsert(HttpContext context)
        {
            try
            {
                var db = new WorkOfTimeDatabase();
                var model = ParseRequest<VMPRD_TransactionModel>(context);
                var rs = model.Save(CallContext.Current.UserId);
                rs.message = Regex.Replace(rs.message, "<.*?>", String.Empty);
                RenderResponse(context, rs);
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }

        [HandleFunction("VWPRD_Transaction/Delete")]
        public void VWPRD_TransactionDelete(HttpContext context)
        {
            try
            {
                var db = new WorkOfTimeDatabase();
                var model = ParseRequest<VMPRD_TransactionModel>(context);
                var rs = model.Delete();
                RenderResponse(context, rs);
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }


        [HandleFunction("VWPRD_TransactionItem/GetByTransactionId")]
        public void VWPRD_TransactionItemGetById(HttpContext context)
        {
            try
            {
                var db = new WorkOfTimeDatabase();
                var id = context.Request["id"];
                var data = db.GetVWPRD_TransactionItemByTransactionId(new Guid((string)id));
                RenderResponse(context, data);
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }



        [HandleFunction("VWPRD_Transaction/GetByQrCode")]
        public void GetVWPRD_TransactionGetByQrCode(HttpContext context)
        {
            try
            {
                var qrCode = context.Request["QRCode"];
                var userid = CallContext.Current != null ? CallContext.Current.UserId : Guid.NewGuid();

                var db = new WorkOfTimeDatabase();
                if (string.IsNullOrEmpty(qrCode))
                {
                    RenderResponse(context, new ResultStatus { result = false, message = "" });
                    return;
                }

                var inventory = db.GetVWPRD_InventoryByCode(qrCode);
                if (inventory == null)
                {
                    var data = db.GetVWPRD_InventoryBySerialCode(qrCode);
                    RenderResponse(context, new ResultStatus { result = true, objects = data });
                    return;
                }

                RenderResponse(context, new ResultStatus { result = true, objects = inventory });
                return;
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus { result = false, message = ex.Message.ToString() });
                return;
            }
        }


        [HandleFunction("VWPRD_Transaction/GetPageInfo")]
        public void VWPRD_TransactionGetPageInfo(HttpContext context)
        {
            try
            {
                var now = DateTime.Now;
                var startOfWeek = now.AddDays(((int)(now.DayOfWeek) * -1) + 1).Date;
                var endOfWeek = startOfWeek.AddDays(7).Date;
                var startOfMonth = new DateTime(now.Year, now.Month, 1).Date;
                var endOfMonth = startOfMonth.AddMonths(1).Date;
                var startOfLastMonth = new DateTime(now.Year, now.Month, 1).AddMonths(-1).Date;

                var model = new PageModel { SearchProperty = "searchField" };


                model.Filters.Add(new PageFilter
                {
                    Title = "Fire Durumuna Göre",
                    Items = new List<PageFilterItem> {
                        new PageFilterItem{
                            Title = "Onay Bekleyen Fire Bildirimleri",
                            Filter = new BEXP { Operand1 = (COL)"status", Operator = BinaryOperator.Equal, Operand2 = (VAL)(int)EnumPRD_TransactionStatus.beklemede }.GetSerializeObject(),
                        },
                        new PageFilterItem{
                            Title = "Onaylanan Fire Bildirimleri",
                         Filter = new BEXP { Operand1 = (COL)"status", Operator = BinaryOperator.Equal, Operand2 = (VAL)(int)EnumPRD_TransactionStatus.islendi }.GetSerializeObject(),
                        }
                    },
                });


                model.Filters.Add(new PageFilter
                {
                    Title = "Sipariş Tarihine Göre",
                    Items = new List<PageFilterItem> {
                        new PageFilterItem{
                            Title = "Bugün",
                            Filter = CreateFilter(now.ToString("yyyy-MM-dd"),now.AddDays(1).ToString("yyyy-MM-dd")).GetSerializeObject()
                        },
                        new PageFilterItem{
                            Title = "Dün",
                            Filter = CreateFilter(now.AddDays(-1).ToString("yyyy-MM-dd"),now.ToString("yyyy-MM-dd")).GetSerializeObject()
                        },
                         new PageFilterItem{
                            Title = "Bu Hafta",
                            Filter = CreateFilter(startOfWeek.ToString("yyyy-MM-dd"),endOfWeek.ToString("yyyy-MM-dd")).GetSerializeObject()
                        },
                         new PageFilterItem{
                            Title = "Geçen Hafta",
                            Filter = CreateFilter(startOfWeek.AddDays(-7).ToString("yyyy-MM-dd"),startOfWeek.ToString("yyyy-MM-dd")).GetSerializeObject()
                        },
                          new PageFilterItem{
                            Title = "Bu Ay",
                            Filter = CreateFilter(startOfMonth.ToString("yyyy-MM-dd"),endOfMonth.ToString("yyyy-MM-dd")).GetSerializeObject()
                        },
                          new PageFilterItem{
                            Title = "Geçen Ay",
                            Filter = CreateFilter(startOfLastMonth.ToString("yyyy-MM-dd"),startOfMonth.ToString("yyyy-MM-dd")).GetSerializeObject()
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

        public BEXP CreateFilter(string startDate, string endDate)
        {
            var filter = new BEXP
            {
                Operand1 = new BEXP
                {
                    Operand1 = (COL)"created",
                    Operator = BinaryOperator.GreaterThanOrEqual,
                    Operand2 = (VAL)startDate
                },

                Operator = BinaryOperator.And,
                Operand2 = new BEXP
                {
                    Operand1 = (COL)"created",
                    Operator = BinaryOperator.LessThanOrEqual,
                    Operand2 = (VAL)endDate
                }
            };

            return filter;
        }

        [HandleFunction("VWPRD_Transaction/GetAllData")]
        public void VWPRD_TransactionGetAllData(HttpContext context)
        {
            try
            {
                var id = context.Request["id"];
                var res = new VMPRD_TransactionModel { id = new Guid(id) }.Load();
                RenderResponse(context, new ResultStatus { result = true, objects = res });
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus { result = false, message = ex.Message });
            }
        }

        [HandleFunction("VWPRD_Product/GetInventoryCodes")]
        public void VWPRD_ProductGetInventoryCodes(HttpContext context)
        {
            try
            {
                var db = new WorkOfTimeDatabase();
                var lastActionDataId = context.Request["lastActionDataId"];
                var productId = context.Request["productId"];
                var lastActionDataCompanyId = context.Request["lastActionDataCompanyId"];
                
                    
                if (string.IsNullOrEmpty(productId))
                {
                    RenderResponse(context, new ResultStatus { result = false, message = "Obje bulunamadı" });
                    return;
                }

                Guid? lastActionData = null;

                if (!string.IsNullOrEmpty(lastActionDataId))
                {
                    lastActionData = new Guid(lastActionDataId);
                }

                Guid? lastActionCompany = null;

                if (!string.IsNullOrEmpty(lastActionDataCompanyId))
                {
                    lastActionCompany = new Guid(lastActionDataCompanyId);
                }


                var res = db.GetVWPRD_InventoryByProductIdLastActionId(Guid.Parse(productId), lastActionCompany, lastActionData).OrderBy(a => a);

                RenderResponse(context, new ResultStatus { result = true, objects = res });
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus { result = false, message = ex.Message });
                return;
            }
        }
    }
}
