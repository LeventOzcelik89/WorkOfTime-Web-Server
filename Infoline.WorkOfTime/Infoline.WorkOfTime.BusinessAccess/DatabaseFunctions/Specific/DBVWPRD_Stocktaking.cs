using Infoline.WorkOfTime.BusinessData;
using System.Collections.Generic;
using System.ComponentModel;
using System;
using System.Data.Common;

namespace Infoline.WorkOfTime.BusinessAccess
{
    public class SummaryHeadersStocktaking
    {
        public int AllStocktakingsCount { get; set; }
        public string AllStocktakingsFilter { get; set; }
        public int StartedStocktakingsCount { get; set; }
        public string StartedStocktakingsFilter { get; set; }
        public int ProcessedStocktakingsCount { get; set; }
        public string ProcessedStocktakingsFilter { get; set; }
        public int CompletedStocktakingsCount { get; set; }
        public string CompletedStocktakingsFilter { get; set; }
        public HeadersStocktaking headerFilters { get; set; }
    }

    public class HeadersStocktaking
    {
        public string title { get; set; }
        public List<HeadersStocktakingItem> Filters { get; set; }
    }

    public class HeadersStocktakingItem
    {
        public string title { get; set; }
        public int count { get; set; }
        public string filter { get; set; }
        public bool isActive { get; set; }
    }

    [EnumInfo(typeof(PRD_Stocktaking), "status")]
    public enum EnumPRD_StocktakingStatus
    {
        [Description("Sayım Başladı"), Generic("color", "EF5352")]
        SayimBasladi = 0,
        [Description("Sayım İşlemi Sona Erdi"), Generic("color", "1ab394")]
        SayimIslemiSonaErdi = 1,
        [Description("Stoklara İşlendi"), Generic("color", "F8AC59")]
        StoklaraIslendi = 2,
        [Description("Sayım Bitti"), Generic("color", "F8AC59")]
        SayimBitti = 3,
    }

    partial class WorkOfTimeDatabase
    {
        public SummaryHeadersStocktaking GetVWPRD_StocktakingPageInfo(Guid userId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                var headers = new SummaryHeadersStocktaking();
                headers.headerFilters = new HeadersStocktaking();
                headers.headerFilters.title = "Durumuna Göre";
                headers.headerFilters.Filters = new List<HeadersStocktakingItem>();
                headers.headerFilters.Filters.Add(new HeadersStocktakingItem
                {
                    title = "Sayım Başladı",
                    filter = "{'Filter':{'Operand1': 'status', 'Operator': 'Equal', 'Operand2':" + (int)EnumPRD_StocktakingStatus.SayimBasladi + "}}",
                    count = db.Table<VWPRD_Stocktaking>().Where(a => a.status == (int)EnumPRD_StocktakingStatus.SayimBasladi).Count(),
                    isActive = true
                });

                headers.headerFilters.Filters.Add(new HeadersStocktakingItem
                {
                    title = "Sayım İşlemi Sona Erdi",
                    filter = "{'Filter':{'Operand1': 'status', 'Operator': 'Equal', 'Operand2':" + (int)EnumPRD_StocktakingStatus.SayimIslemiSonaErdi + "}}",
                    count = db.Table<VWPRD_Stocktaking>().Where(a => a.status == (int)EnumPRD_StocktakingStatus.SayimIslemiSonaErdi).Count(),
                    isActive = true
                });

                headers.headerFilters.Filters.Add(new HeadersStocktakingItem
                {
                    title = "Stoklara İşlendi",
                    filter = "{'Filter':{'Operand1': 'status', 'Operator': 'Equal', 'Operand2':" + (int)EnumPRD_StocktakingStatus.StoklaraIslendi + "}}",
                    count = db.Table<VWPRD_Stocktaking>().Where(a => a.status == (int)EnumPRD_StocktakingStatus.StoklaraIslendi).Count(),
                    isActive = true
                });

                headers.headerFilters.Filters.Add(new HeadersStocktakingItem
                {
                    title = "Sayım Bitti",
                    filter = "{'Filter':{'Operand1': 'status', 'Operator': 'Equal', 'Operand2':" + (int)EnumPRD_StocktakingStatus.SayimBitti + "}}",
                    count = db.Table<VWPRD_Stocktaking>().Where(a => a.status == (int)EnumPRD_StocktakingStatus.SayimBitti).Count(),
                    isActive = true
                });

                return headers;
            }
        }
    }

}


