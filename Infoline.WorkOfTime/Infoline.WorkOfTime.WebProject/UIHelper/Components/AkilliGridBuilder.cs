using Kendo.Mvc.UI;
using Kendo.Mvc.UI.Fluent;
using System.Collections.Generic;
namespace System.Web.Mvc
{
    public class KendoGridOptions
    {
        public bool Excel_ExportHiddenColumns { get; set; } = true;
        public bool Excel_ExportWithTemplate { get; set; } = false;
    }
    public static class AkilliGridBuilder
    {
        public static GridBuilder<T> AkilliGrid<T>(HtmlHelper helper, string name, KendoGridOptions options = null)
             where T : class
        {
            options = options ?? new KendoGridOptions { };
            return helper.Kendo().Grid<T>()
                .HtmlAttributes(new Dictionary<string, object> {
                    { "data-exporthiddens", options.Excel_ExportHiddenColumns.ToString() }
                })
                .Selectable(x => x.Mode(GridSelectionMode.Multiple))
                .Name(name)
                .Navigatable()
                  .Filterable(filterable =>
                      filterable.Operators(
                          operators =>
                      operators
                      .ForString(str =>
                          str.Clear().Contains("İçeriyor")
                          .StartsWith("İle Başlar")
                          .IsEqualTo("Eşit")
                          .IsNotEqualTo("Eşit Değil")
                          .EndsWith("İle Biter")
                          .DoesNotContain("İçermiyor")
                      )
                      .ForDate(dt =>
                            dt.Clear().IsGreaterThan("Büyüktür")
                            .IsGreaterThanOrEqualTo("Büyük yada Eşit")
                            .IsLessThan("Küçüktür")
                            .IsLessThanOrEqualTo("Küçük yada Eşit"))))
                .Sortable(a => a.SortMode(GridSortMode.MultipleColumn))
                .Resizable(a => a.Columns(true))
                .Scrollable(x => x.Height(450))
                //.NoRecords(a => a.Template("Görüntülenecek bir kayıt bulunamadı."))
                .Pageable(x => x.PageSizes(new[] { 10, 25, 100 }).Refresh(true).Messages(m =>
                 {
                     m.Refresh("Yenile")
                      .AllPages("Tümünü getir")
                      .Last("Son sayfaya git")
                      .First("İlk sayfaya git")
                      .Previous("Bir önceki sayfaya git")
                      .Next("Bir sonraki sayfaya git")
                      .ItemsPerPage("Sayfa başına ürün");
                 }))
               .ToolBar(x =>
               {
                   x.Pdf().HtmlAttributes(new Dictionary<string, object> { { "class", "pull-right hide" }, { "data-original-title", "Pdf'e Aktar" } });

                   x.Excel().HtmlAttributes(new Dictionary<string, object>() { { "class", "pull-right hide" }, { "data-original-title", "Excel'e Aktar " } });

               })
                 .Excel(x =>
                    x.AllPages(true).FileName((helper.ViewData["Title"] + "-" + DateTime.Now) + ".xlsx")
                )
                .Pdf(p =>
                    p.FileName((helper.ViewData["Title"] + "-" + DateTime.Now) + ".pdf")
                    .AllPages()
                    .Title(string.Format("{0}", helper.ViewData["Title"]))
                    .Landscape()
                    .Margin(1, 1, 1, 1)
                 )
                .Events(x =>
                {
                    x.ExcelExport(options.Excel_ExportWithTemplate ? "Kendo_ExcelExportTemplate" : "Kendo_ExcelExport");
                    x.DataBound("Kendo_GridLoad");
                    x.Change("Kendo_GridChange");
                });
        }
    }
}
