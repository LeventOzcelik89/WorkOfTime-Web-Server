using Kendo.Mvc.UI;
using Kendo.Mvc.UI.Fluent;
using System.Collections.Generic;

namespace System.Web.Mvc
{
    public static class AkilliGridBuilder
    {

        public static GridBuilder<T> AkilliGrid<T>(HtmlHelper helper, string name)
            where T : class
        {

            return helper.Kendo().Grid<T>()
                .Selectable(x => x.Mode(GridSelectionMode.Multiple))
                .Name(name)
                .Navigatable()
                .Filterable(filterable =>
                      filterable.Operators(
                          operators =>
                      operators.ForString(str =>
                      str.Clear().Contains("İçeriyor")
                      .StartsWith("İle Başlar")
                      .IsEqualTo("Eşit")
                      .IsNotEqualTo("Eşit Değil")
                      .EndsWith("İle Biter")
                      .DoesNotContain("İçermiyor"))))
                .Sortable()
                .Resizable(a => a.Columns(true))
                .Scrollable(x => x.Height(450))
                .Pageable(x => x.PageSizes(new[] { 10, 100 }).Refresh(true).Messages(m =>
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
                    x.Pdf().HtmlAttributes(new Dictionary<string, object> { { "class", "pull-right hide" }, { "data-original-title", HttpContext.Current.Request.Cookies.Get("language").Value == "ENG"?"Export To Pdf":"Pdf'e Aktar" } });

                    x.Excel().HtmlAttributes(new Dictionary<string, object>() { { "class", "pull-right hide" }, { "data-original-title", HttpContext.Current.Request.Cookies.Get("language").Value == "ENG" ? "Export To Excel" : "Excel'e Aktar " } });

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
                    x.ExcelExport("Kendo_ExcelExport");
                    x.DataBound("Kendo_GridLoad");
                    x.Change("Kendo_GridChange");
                });

        }

    }
}
