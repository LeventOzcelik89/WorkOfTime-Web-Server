using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infoline.WorkOfTime.BusinessAccess
{
    public class StockReportModel
    {
        private WorkOfTimeDatabase db { get; set; }
        public int TotalStorage { get; set; }
        public int TotalStockTracking { get; set; }
        public int TotalSerialNumber { get; set; }
        public double? TotalStockPiece { get; set; }
        public double? TotalStockKg { get; set; }
        public double? TotalStockPackage { get; set; }
        public VWPRD_Inventory[] PieceInventories { get; set; }
        public VWPRD_Inventory[] KgInventories { get; set; }
        public VWPRD_Inventory[] PackageInventories { get; set; }
        public PRD_InventoryAction[] PieceInventoryActions { get; set; }
        public PRD_InventoryAction[] KgInventoryActions { get; set; }
        public PRD_InventoryAction[] PackageInventoryActions { get; set; }
        public StockReportData[] PieceStockData { get; set; }
        public StockReportData[] KgStockData { get; set; }
        public StockReportData[] PackageStockData { get; set; }
        public PieClass[] CategoryGroup { get; set; }
        public PieClass[] SizeGroup { get; set; }

        public StockReportData[] PieceActionData { get; set; }
        public StockReportData[] KgActionData { get; set; }
        public StockReportData[] PackagActionData { get; set; }

        public Guid piece = new Guid("8F6E4C58-47AB-445C-B3C6-6DF642AF1DAC");
        public Guid kg = new Guid("7139C11B-1335-4EA1-BCCE-E8F3B9FE669E");
        public Guid package = new Guid("0F16117B-1E5A-48A8-A143-D211F9FE8E25");

        public StockReportModel Load()
        {
            this.db = this.db ?? new WorkOfTimeDatabase();
            var unitIds = new Guid[]
            {
                this.piece,
                this.kg,
                this.package
            };
            var storages = db.GetVWCMP_StorageOwner();
            this.TotalStorage = storages.Count();
            var products = db.GetVWPRD_Product();

            this.TotalStockTracking = products.Where(x => x.stockType == (Int32)EnumPRD_ProductStockType.NormalTakip).Count();
            this.TotalSerialNumber = products.Where(x => x.stockType == (Int32)EnumPRD_ProductStockType.SeriNoluTakip).Count();

            if (storages.Count() > 0)
            {
                var summaries = db.GetVWPRD_StockSummaryByStockIds(storages.Select(x => x.id).ToArray());
                this.CategoryGroup = summaries.Where(x => x.productId_Title.IndexOf("Kabak Çekirdeği") > -1).GroupBy(x => x.productId_Title.Split('|').LastOrDefault()).Select(v => new PieClass
                {
                    name = v.Key,
                    y = v.Select(y => y.quantity).Sum()
                }).OrderByDescending(v => v.y).Take(10).ToArray();

                if (TenantConfig.Tenant.TenantCode == 1137)
                {
                    this.SizeGroup = summaries.Where(x => x.productId_Title.IndexOf("Kabak Çekirdeği") > -1 && x.productId_Title.IndexOf("mm /") > -1).GroupBy(x => x.productId_Title.Split('|').FirstOrDefault().Split('(').Skip(1).FirstOrDefault().Replace(")", "")).Select(v => new PieClass
                    {
                        name = v.Key,
                        y = v.Select(y => y.quantity).Sum()
                    }).OrderByDescending(v => v.y).ToArray();
                }
                var pieceSummaries = summaries.Where(x => x.unitId_Title == "ADET").ToArray();

                this.TotalStockPiece = pieceSummaries.Select(x => x.quantity).Sum();
                this.PieceStockData = pieceSummaries.GroupBy(a => new { a.stockId_Title , a.stockCompanyId_Title}).Select(v => new StockReportData
                {
                    unitId_Title = "ADET",
                    stockId_Title = v.Key.stockCompanyId_Title + "<br> " + v.Key.stockId_Title,
                    quantity = v.Select(b => b.quantity).Sum(),
                }).OrderByDescending(f => f.quantity).Take(10).ToArray();

                var kgSummaries = summaries.Where(x => x.unitId_Title == "KG").ToArray();

                this.TotalStockKg = kgSummaries.Select(x => x.quantity).Sum();
                this.KgStockData = kgSummaries.GroupBy(a => new { a.stockId_Title , a.stockCompanyId_Title }).Select(v => new StockReportData
                {
                    unitId_Title = "KG",
                    stockId_Title = v.Key.stockCompanyId_Title + "<br> " + v.Key.stockId_Title,
                    quantity = v.Select(b => b.quantity).Sum(),
                }).OrderByDescending(f => f.quantity).Take(10).ToArray();


                var packageSummaries = summaries.Where(x => x.unitId_Title == "PAKET").ToArray();

                this.TotalStockPackage = packageSummaries.Select(x => x.quantity).Sum();
                this.PackageStockData = packageSummaries.GroupBy(a => new { a.stockId_Title , a.stockCompanyId_Title }).Select(v => new StockReportData
                {
                    unitId_Title = "PAKET",
                    stockId_Title = v.Key.stockCompanyId_Title + " <br>" + v.Key.stockId_Title,
                    quantity = v.Select(b => b.quantity).Sum(),
                }).OrderByDescending(f => f.quantity).Take(10).ToArray();

                var stockActions = db.GetVWPRD_StockActionByStockIds(storages.Select(x => x.id).ToArray());

                var pieceActions = stockActions.Where(x => x.unitId_Title == "ADET").ToArray();
                this.PieceActionData = pieceActions.GroupBy(a => new { a.stockId_Title, a.stockCompanyId_Title }).Select(v => new StockReportData
                {
                    unitId_Title = "ADET",
                    stockId_Title = v.Key.stockCompanyId_Title + " <br>" + v.Key.stockId_Title,
                    quantity = v.Count()
                }).OrderByDescending(f => f.quantity).Take(10).ToArray();

                var kgActions = stockActions.Where(x => x.unitId_Title == "KG").ToArray();
                this.KgActionData = kgActions.GroupBy(a => new { a.stockId_Title, a.stockCompanyId_Title }).Select(v => new StockReportData
                {
                    unitId_Title = "KG",
                    stockId_Title = v.Key.stockCompanyId_Title + " <br>" + v.Key.stockId_Title,
                    quantity = v.Count()
                }).OrderByDescending(f => f.quantity).Take(10).ToArray();

                var packageActions = stockActions.Where(x => x.unitId_Title == "PAKET").ToArray();
                this.PackagActionData = packageActions.GroupBy(a => new { a.stockId_Title, a.stockCompanyId_Title }).Select(v => new StockReportData
                {
                    unitId_Title = "PAKET",
                    stockId_Title = v.Key.stockCompanyId_Title + " <br>" + v.Key.stockId_Title,
                    quantity = v.Count()
                }).OrderByDescending(f => f.quantity).Take(10).ToArray();
            }
            return this;
        }

        public class StockReportData
        {
            public string unitId_Title { get; set; }
            public string stockId_Title { get; set; }
            public double? quantity { get; set; }
            public string productId_Title { get; set; }
        }

        public class PieClass
        {
            public double? y { get; set; }
            public string name { get; set; }
        }
    }
}
