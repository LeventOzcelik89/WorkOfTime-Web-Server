using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using System.Web.UI;

namespace System.Web.Mvc
{
    public class GridSearch : FactoryBase
    {

        public enum Colors
        {
            red = 0,
            green = 1,
            blue = 2,
            yellow = 3,
            navy = 4,
        }

        public IDictionary<string, object> HtmlAttributes { get; set; }
        public string _Color { get; set; }
        public string _IconClass { get; set; }
        public string _Value { get; set; }
        public string _Text { get; set; }
        public string _Link { get; set; }
        public string _Grid { get; set; }
        public string _GridFilter { get; set; }
        public string _Name { get; set; }
        public string _PlaceHolder { get; set; }
        public string _NameButton { get; set; }

        public string _GridSearchField { get; set; }

        public string Render()
        {

            var sb = new StringBuilder();

            sb.AppendLine("<style type=\"text/css\">");
            sb.AppendLine(".panel-custom {border:0 !important;border-radius:0 !important;}");
            sb.AppendLine(".panel-custom .panel-body {border:0 !important;background-color: #3f51b5 !important;border-radius:0 !important;padding:15px !important;}");
            sb.AppendLine("</style>");



            sb.AppendLine("<div class=\"panel panel-custom\">");
            sb.AppendLine("<div class=\"panel-body\">");
            sb.AppendLine("<div class=\"row\">");
            sb.AppendLine("<div class=\"col-md-11\">");
            sb.AppendLine("<input id=" + this._Name + " class=\"form-control\"  placeholder=\"Lütfen " + this._PlaceHolder + "  Giriniz...\" style=\"color: #000;\" />");
            sb.AppendLine("</div>");
            sb.AppendLine("<div class=\"col-md-1\">");
            sb.AppendLine("<a href=\"#\" id =" + this._NameButton + " class=\"btn btn-block btn-default\" style=\"color:black;height: 40px;line-height:2;padding:5px;\" ><i class=\"fa icon-search\"></i></a>");
            sb.AppendLine("</div>");
            sb.AppendLine("</div>");
            sb.AppendLine("</div>");
            sb.AppendLine("</div>");



            if (!String.IsNullOrEmpty(this._Grid))
            {

                sb.AppendLine("<script type=\"text/javascript\">");
                sb.AppendLine("$(document)");
                sb.AppendLine(".on('click', '#" + this._NameButton + "', function(e) {");
                sb.AppendLine("e.preventDefault();");
                sb.AppendLine("var valueSearch = $('#" + this._Name + "').val();");
                sb.AppendLine("if(valueSearch == null || valueSearch == \"\" ){ valueSearch = \"\" }");
                sb.AppendLine("var newFilter = { field: '" + this._GridSearchField + "', operator: 'contains', value: valueSearch };");
                sb.AppendLine("var dataSource = $('#" + this._Grid + "').data('kendoGrid').dataSource;");




                sb.AppendLine("var filters = null;");
                sb.AppendLine("if (dataSource.filter() != null)");
                sb.AppendLine("{filters = dataSource.filter().filters;}");
                sb.AppendLine("if (filters == null)");
                sb.AppendLine("{filters = [newFilter];}");
                sb.AppendLine("else{");
                sb.AppendLine("var isNew = true;");
                sb.AppendLine("var index = 0;");
                sb.AppendLine("for (index = 0; index < filters.length; index++){");
                sb.AppendLine("if (filters[index].field == '" + this._GridSearchField + "'){isNew = false;break;}}");
                sb.AppendLine("if (isNew){filters.push(newFilter);}else{ filters[index] = newFilter;}}");

                sb.AppendLine("$.each(filters, function(i, item) {");
                sb.AppendLine("if (item.field == '" + this._GridSearchField + "' && item.value == \"\"){filters.pop();}})");

                sb.AppendLine("dataSource.filter(filters);");
                sb.AppendLine("return false;");
                sb.AppendLine("})");
                sb.AppendLine(".on('keyup', '#" + this._Name + "', function(e) {");
                sb.AppendLine("if ($('#" + this._Name + "').val().length > 3) {");
                sb.AppendLine("$('#" + this._NameButton + "').trigger('click');");
                sb.AppendLine("} else if (e.keyCode == 8){");
                sb.AppendLine("$('#" + this._NameButton + "').trigger('click');}");
                sb.AppendLine("})");
                sb.AppendLine("</script>");

            }

            return sb.ToString();
        }

        public GridSearch(ViewContext viewContext, ViewDataDictionary viewData = null) : base(viewContext, viewData)
        {
            //  this.ViewData = ViewData;
            //  this.ViewContext = ViewContext;

            this._Text = "";
            this._Value = "";
            this._Color = Colors.red.ToString();
            this._Link = "#";
            this._IconClass = "fa fa-comments";
            this._Name = "GridSearch_" + Guid.NewGuid().ToString().Substring(0, 8);
            this._NameButton = "GridSearchButton_" + Guid.NewGuid().ToString().Substring(0, 8);

            this.HtmlAttributes = new Dictionary<string, object>();

        }

        protected override void WriteHtml(HtmlTextWriter writer)
        {

        }

    }
}