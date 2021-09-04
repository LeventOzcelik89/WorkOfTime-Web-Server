using GeoAPI.Geometries;
using Infoline.Framework.Database;
using Infoline.Framework.Database.Mssql;
using Infoline.WorkOfTime.BusinessAccess;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Infoline.WorkOfTime.Tools
{

    public class ClassInfo
    {
        public string Schema { get; set; }
        public string Alias { get; set; }
        public string Name { get; set; }
        internal string ViewName { get { return "VW" + Name; } }
        internal string AreaName { get { return Name.Split('_').FirstOrDefault(); } }
        public string Description { get; set; }
        public List<PropInfo> Properties { get; set; } = new List<PropInfo>();
        public ResultStatus Create()
        {
            return new ResultStatus();
        }
        public ResultStatus Alter()
        {
            return new ResultStatus();
        }
    }

    public class PropInfo
    {
        public string Alias { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Type Type { get; set; }
        public bool IsPrimaryKey { get; set; }
        public string RelationTable { get; set; }
        public string RelationColumn { get; set; }
        public string[] RelationPreviewColumns { get; set; }
        public string RelationName
        {
            get
            {
                return RelationTable != null || Enum != null ? Name + "_Title" : Name;
            }
        }
        public Type Enum { get; set; }
        public string Default { get; set; }
        public bool Nullable { get; set; }
        public int? Length { get; set; }
        public string Validation { get; set; }
    }

    public class DisplayInfo : Attribute
    {
        public string Alias { get; set; }
        public string Description { get; set; }
        public DisplayInfo(string Alias, string Description)
        {
            this.Alias = Alias;
            this.Description = Description;
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class RelationInfo : Attribute
    {
        public string Table { get; set; }
        public string Column { get; set; }
        public string[] PreviewColumns { get; set; }

        public RelationInfo(string TableName, string ColumnName, string[] PreviewColumns)
        {
            Table = TableName;
            Column = ColumnName;
            this.PreviewColumns = PreviewColumns;
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class LengthInfo : Attribute
    {
        public int? Length { get; set; }
        public LengthInfo(int Length)
        {
            this.Length = Length;
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class EnumInfo : Attribute
    {
        public Type Enum { get; set; }
        public EnumInfo(Type enumm)
        {
            Enum = enumm;
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class DefaultInfo : Attribute
    {
        public string Default { get; set; }
        public DefaultInfo(EnumDefaults Default)
        {
            this.Default = Default.ToString();
        }

        public DefaultInfo(object Default)
        {
            this.Default = Default.ToString();
        }

        public enum EnumDefaults
        {
            getdate,
            newid
        }

    }

    public class ValidationInfo : Attribute
    {
        public string Validation { get; set; }
        public ValidationInfo(string validation)
        {
            Validation = validation;
        }
    }

    public class IsPrimary : Attribute
    {
        public IsPrimary()
        {
        }
    }

    public class MVCInfo : Attribute
    {
        public bool Grid { get; set; }
        public bool Upsert { get; set; }
        public MVCInfo(bool grid = true, bool upsert = true)
        {
            Grid = grid;
            Upsert = upsert;
        }
    }

    public static class MyExtension
    {
        public static IEnumerable<T> ToOrderBy<T>(this IEnumerable<T> item, Expression<Func<T, object>> equalsColumnExp, params string[] _params)
        {
            var liste = new List<T>();
            try
            {
                var expression = (MemberExpression)equalsColumnExp.Body;
                var equalsColumn = expression.Member.Name;

                if (item == null || item.Count() == 0 || typeof(T).GetProperty(equalsColumn) == null)
                {
                    return item ?? new List<T>();
                }

                foreach (var param in _params)
                {
                    var prm = item.Where(m => param.Equals(m.GetType().GetProperty(equalsColumn).GetValue(m, null).ToString()));
                    liste.AddRange(prm);
                }

                liste.AddRange(item.Where(m => !_params.Contains(m.GetType().GetProperty(equalsColumn).GetValue(m, null).ToString())));

            }
            catch
            {
                return item;
            }

            return liste;
        }


        public static string ToCapitalizeInvariant(this string item)
        {
            return string.Join("", item.ToCharArray().Select((a, i) => (i == 0 ? a.ToString().ToUpperInvariant() : a.ToString().ToLowerInvariant())));
        }

        public static string ToCapitalize(this string item)
        {
            return string.Join("", item.ToCharArray().Select((a, i) => (i == 0 ? a.ToString().ToUpper() : a.ToString().ToLower())));
        }


        public static string ToCapitalize(this string item, CultureInfo culture)
        {


            return string.Join("", item.ToCharArray().Select((a, i) => (i == 0 ? a.ToString().ToUpper(culture) : a.ToString().ToLower(culture))));
        }


    }

    public class CodeGenerator
    {
        public string[] NotColumns { get; set; } = new string[] { "created", "createdby", "changed", "changedby" };
        public string Dbname { get; set; }
        public bool OverWrite { get; set; }
        public string SolutionName { get; set; }
        public string ProjectName { get { return SolutionName + ".WebProject"; } }
        private List<ClassInfo> TableList { get; set; } = new List<ClassInfo>();
        private readonly ITypeMapper typeMapper = new MssqlTypeMapper();
        public CodeGenerator(Type[] classNames, string schemaName, string solutionName, string DBName)
        {
            Dbname = DBName;
            SolutionName = solutionName;
            var createClass = Assembly.GetExecutingAssembly().GetTypes().Where(a => classNames.Select(c => c.Name).Contains(a.Name));

            foreach (var classs in createClass)
            {
                var tableInfo = new ClassInfo
                {
                    Schema = schemaName,
                    Name = classs.Name,
                    Alias = classs.Name,
                    Description = classs.Name
                };

                if (classs.GetCustomAttribute<DisplayInfo>() != null)
                {
                    tableInfo.Alias = classs.GetCustomAttribute<DisplayInfo>().Alias;
                    tableInfo.Description = classs.GetCustomAttribute<DisplayInfo>().Description;
                }



                foreach (var item in classs.GetProperties().OrderBy(a => a.MetadataToken))
                {
                    var prop = new PropInfo
                    {
                        Name = item.Name,
                        Type = item.PropertyType,
                        Alias = item.Name,
                        Description = item.Name,
                        IsPrimaryKey = item.GetCustomAttribute<IsPrimary>() != null
                    };
                    prop.Nullable = (item.PropertyType.IsGenericType && item.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>)) || prop.Type == typeof(string) || prop.Type == typeof(IGeometry);
                    prop.Length = item.GetCustomAttribute<LengthInfo>()?.Length;
                    prop.Default = item.GetCustomAttribute<DefaultInfo>()?.Default;

                    if (item.GetCustomAttribute<DisplayInfo>() != null)
                    {
                        prop.Alias = item.GetCustomAttribute<DisplayInfo>().Alias;
                        prop.Description = item.GetCustomAttribute<DisplayInfo>().Description;
                    }

                    if (item.GetCustomAttribute<RelationInfo>() != null)
                    {
                        prop.RelationTable = item.GetCustomAttribute<RelationInfo>().Table;
                        prop.RelationColumn = item.GetCustomAttribute<RelationInfo>().Column;
                        prop.RelationPreviewColumns = item.GetCustomAttribute<RelationInfo>().PreviewColumns;
                    }


                    if (item.GetCustomAttribute<EnumInfo>() != null)
                    {
                        prop.Enum = item.GetCustomAttribute<EnumInfo>().Enum;
                    }


                    tableInfo.Properties = tableInfo.Properties.ToList();
                    tableInfo.Properties.Add(prop);
                }

                TableList.Add(tableInfo);
            }
        }
        public string CreateTables()
        {

            var queryList = new List<string>();



            foreach (var table in TableList)
            {
                var columns = string.Join("," + System.Environment.NewLine, table.Properties.Select(
                     p => string.Format("\t [{0}] {1} {2} {3}", p.Name, typeMapper.GetSqlType(p.Type, p.Length), (p.Nullable ? "NULL" : "NOT NULL"), (string.IsNullOrEmpty(p.Default) ? "" : "CONSTRAINT [DF_" + table.Name + "_" + p.Name + "] DEFAULT (" + p.Default + "())"))
                 ));
                queryList.Add(string.Format("CREATE TABLE [{0}].[{1}](\n {2} \n )", table.Schema, table.Name, columns));
                queryList.Add(string.Format("EXEC sp_addextendedproperty 'MS_Description', N'{0}','SCHEMA', N'{1}','TABLE', N'{2}'", table.Description.Replace('\'', ' '), table.Schema, table.Name));
                foreach (var prop in table.Properties)
                {
                    queryList.Add(string.Format("EXEC sp_addextendedproperty 'MS_Description', N'{0}','SCHEMA', N'{1}','TABLE', N'{2}','COLUMN', N'{3}'", prop.Description.Replace('\'', ' '), table.Schema, table.Name, prop.Name));
                }
            }



            foreach (var table in TableList)
            {
                var columnsPrimary = string.Join(",", table.Properties.Where(a => a.IsPrimaryKey).Select(a => "[" + a.Name + "]"));
                queryList.Add(string.Format("ALTER TABLE [{0}].[{1}] ADD CONSTRAINT [PK_{1}] PRIMARY KEY CLUSTERED ({2}) WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON[PRIMARY]", table.Schema, table.Name, columnsPrimary));

                var viewColumns = "";
                foreach (var prop in table.Properties)
                {
                    if (prop.RelationTable != null)
                    {
                        if (prop.RelationPreviewColumns.Length > 0)
                        {
                            var columnSelect = string.Join("+' | '+", prop.RelationPreviewColumns.Select(a => "ISNULL([" + a + "],'')"));
                            var columnFNName = string.Join("", prop.RelationPreviewColumns.Select(a => a.ToCapitalizeInvariant()));
                            viewColumns += string.Format("\n\t,[{0}].[fn_{1}{2}By{3}](t.[{4}]) as {5}", table.Schema, prop.RelationTable, columnFNName, prop.RelationColumn.ToCapitalizeInvariant(), prop.Name, prop.RelationName);
                            queryList.Add(string.Format("CREATE FUNCTION [{0}].[fn_{1}{2}By{3}](@id uniqueidentifier) RETURNS nvarchar(500) AS BEGIN RETURN ISNULL((SELECT TOP 1 {4} FROM [{0}].[{1}] WITH (NOLOCK) WHERE id = @id), NULL) END", table.Schema, prop.RelationTable, columnFNName, prop.RelationColumn.ToCapitalizeInvariant(), columnSelect));
                        }
                        queryList.Add(string.Format("ALTER TABLE [{0}].[{1}] ADD CONSTRAINT [FK_{1}_{2}_{3}_{4}] FOREIGN KEY ([{2}]) REFERENCES [{0}].[{3}] ([{4}]) ON DELETE NO ACTION ON UPDATE NO ACTION", table.Schema, table.Name, prop.Name, prop.RelationTable, prop.RelationColumn));

                    }

                    if (prop.Enum != null)
                    {
                        viewColumns += string.Format("\n\t,[{0}].[fn_EnumKeyToDescription]('{1}','{2}',t.[{2}]) as {3}", table.Schema, table.Name, prop.Name, prop.RelationName);
                    }
                }
                queryList.Add(string.Format("CREATE view [{0}].[{1}] as \n\t SELECT \n\t t.*{2} \n\t FROM {3} as t WITH (NOLOCK)", table.Schema, table.ViewName, viewColumns, table.Name));
            }



            var db = new WorkOfTimeDatabase();

            foreach (var query in queryList)
            {
                using (var dt = db.GetDB())
                {

                    var rs = dt.ExecuteNonQuery(query);
                    if (rs.result == false)
                    {
                        System.Diagnostics.Debug.WriteLine(rs.message);
                    }

                }
            }



            return string.Join("\n GO \n", queryList);
        }

        public void CreatePages(string path, bool overWrite = false)
        {
            OverWrite = overWrite;
            foreach (var item in TableList)
            {
                var areaName = item.Name.Split('_').FirstOrDefault();
                var baseRoot = Path.Combine(path, "Areas", areaName);
                CreateAreaRegistration(item, baseRoot);
                CreateWebConfig(baseRoot);
                CreateController(item, baseRoot);
                CreateIndex(item, baseRoot);
                CreateUpsert(item, baseRoot, "Insert");
                CreateUpsert(item, baseRoot, "Update");
                CreateDetail(item, baseRoot);
            }
        }

        #region Controller
        private void CreateController(ClassInfo info, string baseRoot)
        {

            var root = Path.Combine(baseRoot, "Controllers");
            var sb = new StringBuilder();
            sb.Clear();
            sb.AppendLine(string.Format("using {0}.BusinessData;", SolutionName));
            sb.AppendLine(string.Format("using {0}.BusinessAccess;", SolutionName));
            sb.AppendLine("using Kendo.Mvc;");
            sb.AppendLine("using Kendo.Mvc.Extensions;");
            sb.AppendLine("using Kendo.Mvc.UI;");
            sb.AppendLine("using System;");
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine("using System.Linq;");
            sb.AppendLine("using System.Web.Mvc;");
            sb.AppendLine("");
            sb.AppendFormat("namespace {0}.Areas.{1}.Controllers\r\n", ProjectName, info.AreaName);
            sb.AppendLine("{");
            sb.AppendFormat("\tpublic class {0}Controller : Controller\r\n", info.ViewName);
            sb.AppendLine("\t{");
            sb.AppendLine(GetMethod_Constructer());
            sb.AppendLine();
            sb.AppendLine(GetMethod_DataSource(Dbname, info.ViewName));
            sb.AppendLine();
            sb.AppendLine(GetMethod_DataSourceDropDown(Dbname, info.ViewName));
            sb.AppendLine();
            sb.AppendLine(GetMethod_Detail(Dbname, info.ViewName));
            sb.AppendLine();
            sb.AppendLine(GetMethod_Insert_Open(info.ViewName));
            sb.AppendLine();
            sb.AppendLine(GetMethod_Insert_Commit(Dbname, info.Name));
            sb.AppendLine();
            sb.AppendLine(GetMethod_Update_Open(Dbname, info.ViewName));
            sb.AppendLine();
            sb.AppendLine(GetMethod_Update_Commit(Dbname, info.Name));
            sb.AppendLine();
            sb.AppendLine(GetMethod_Delete(Dbname, info.Name));
            sb.AppendLine();
            sb.AppendLine("\t}");
            sb.AppendLine("}");
            CreateFile(sb.ToString(), root, info.ViewName + "Controller.cs");
        }
        private string GetMethod_Constructer()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("\t\tpublic ActionResult Index()");
            sb.AppendLine("\t\t{");
            sb.AppendLine("\t\t    return View();");
            sb.AppendLine("\t\t}");
            return sb.ToString();
        }
        private string GetMethod_DataSource(string dbname, string viewName)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("\t\tpublic ContentResult DataSource([DataSourceRequest]DataSourceRequest request)");
            sb.AppendLine("\t\t{");
            sb.AppendLine("\t\t    var condition = KendoToExpression.Convert(request);\r\n");
            sb.AppendLine("\t\t    var page = request.Page;");
            sb.AppendLine("\t\t    request.Filters = new FilterDescriptor[0];");
            sb.AppendLine("\t\t    request.Sorts = new SortDescriptor[0];");
            sb.AppendLine("\t\t    request.Page = 1;");
            sb.AppendFormat("\t\t    var db = new {0}Database();\r\n", dbname);
            sb.AppendFormat("\t\t    var data = db.Get{0}(condition).RemoveGeographies().ToDataSourceResult(request);\r\n", viewName);
            sb.AppendFormat("\t\t    data.Total = db.Get{0}Count(condition.Filter);\r\n", viewName);
            sb.AppendLine("\t\t    return Content(Infoline.Helper.Json.Serialize(data), \"application/json\");");
            sb.AppendLine("\t\t}");
            return sb.ToString();
        }
        private string GetMethod_DataSourceDropDown(string dbname, string viewName)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("\t\tpublic ContentResult DataSourceDropDown([DataSourceRequest]DataSourceRequest request)");
            sb.AppendLine("\t\t{");
            sb.AppendLine("\t\t    var condition = KendoToExpression.Convert(request);\r\n");
            sb.AppendFormat("\t\t    var db = new {0}Database();\r\n", dbname);
            sb.AppendFormat("\t\t    var data = db.Get{0}(condition);\r\n", viewName);
            sb.AppendLine("\t\t    return Content(Infoline.Helper.Json.Serialize(data), \"application/json\");");
            sb.AppendLine("\t\t}");
            return sb.ToString();
        }
        private string GetMethod_Insert_Open(string viewName)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("\t\tpublic ActionResult Insert()");
            sb.AppendLine("\t\t{");
            sb.AppendFormat("\t\t    var data = new {0} {{ id = Guid.NewGuid() }};\r\n", viewName);
            sb.AppendLine("\t\t    return View(data);");
            sb.AppendLine("\t\t}");
            return sb.ToString();
        }
        private string GetMethod_Insert_Commit(string dbname, string tableName)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("\t\t[HttpPost, ValidateAntiForgeryToken]");
            sb.AppendFormat("\t\tpublic JsonResult Insert({0} item)\r\n", tableName);
            sb.AppendLine("\t\t{");
            sb.AppendFormat("\t\t    var db = new {0}Database();\r\n", dbname);
            sb.AppendLine("\t\t    var userStatus = (PageSecurity)Session[\"userStatus\"];");
            sb.AppendLine("\t\t    var feedback = new FeedBack();");
            sb.AppendLine("\t\t    item.created = DateTime.Now;");
            sb.AppendLine("\t\t    item.createdby = userStatus.user.id;");
            sb.AppendFormat("\t\t    var dbresult = db.Insert{0}(item);\r\n", tableName);
            sb.AppendLine("\t\t    var result = new ResultStatusUI");
            sb.AppendLine("\t\t    {");
            sb.AppendLine("\t\t        Result = dbresult.result,");
            sb.AppendLine("\t\t        FeedBack = dbresult.result ? feedback.Success(\"Kaydetme işlemi başarılı\") : feedback.Warning(\"Kaydetme işlemi başarısız\")");
            sb.AppendLine("\t\t    };");
            sb.AppendLine("\t\t");
            sb.AppendLine("\t\t    return Json(result, JsonRequestBehavior.AllowGet);");
            sb.AppendLine("\t\t}");
            return sb.ToString();
        }
        private string GetMethod_Update_Open(string dbname, string viewName)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("\t\tpublic ActionResult Update(Guid id)");
            sb.AppendLine("\t\t{");
            sb.AppendFormat("\t\t    var db = new {0}Database();\r\n", dbname);
            sb.AppendFormat("\t\t    var data = db.Get{0}ById(id);\r\n", viewName);
            sb.AppendLine("\t\t    return View(data);");
            sb.AppendLine("\t\t}");
            return sb.ToString();
        }
        private string GetMethod_Update_Commit(string dbname, string tableName)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("\t\t[HttpPost, ValidateAntiForgeryToken]");
            sb.AppendFormat("\t\tpublic JsonResult Update({0} item)\r\n", tableName);
            sb.AppendLine("\t\t{");
            sb.AppendFormat("\t\t    var db = new {0}Database();\r\n", dbname);
            sb.AppendLine("\t\t    var userStatus = (PageSecurity)Session[\"userStatus\"];");
            sb.AppendLine("\t\t    var feedback = new FeedBack();");
            sb.AppendLine("\t\t");
            sb.AppendLine("\t\t    item.changed = DateTime.Now;");
            sb.AppendLine("\t\t    item.changedby = userStatus.user.id;");
            sb.AppendLine("\t\t");
            sb.AppendFormat("\t\t    var dbresult = db.Update{0}(item);\r\n", tableName);
            sb.AppendLine("\t\t    var result = new ResultStatusUI");
            sb.AppendLine("\t\t    {");
            sb.AppendLine("\t\t        Result = dbresult.result,");
            sb.AppendLine("\t\t        FeedBack = dbresult.result ? feedback.Success(\"Güncelleme işlemi başarılı\") : feedback.Warning(\"Güncelleme işlemi başarısız\")");
            sb.AppendLine("\t\t    };");
            sb.AppendLine("\t\t");
            sb.AppendLine("\t\t    return Json(result, JsonRequestBehavior.AllowGet);");
            sb.AppendLine("\t\t}");
            return sb.ToString();
        }
        private string GetMethod_Detail(string dbname, string viewName)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("\t\tpublic ActionResult Detail(Guid id)");
            sb.AppendLine("\t\t{");
            sb.AppendFormat("\t\t    var db = new {0}Database();\r\n", dbname);
            sb.AppendFormat("\t\t    var data = db.Get{0}ById(id);\r\n", viewName);
            sb.AppendLine("\t\t    return View(data);");
            sb.AppendLine("\t\t}");

            return sb.ToString();
        }
        private string GetMethod_Delete(string dbname, string tableName)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("\t\t[HttpPost]");
            sb.AppendLine("\t\tpublic JsonResult Delete(string[] id)");
            sb.AppendLine("\t\t{");
            sb.AppendFormat("\t\t    var db = new {0}Database();\r\n", dbname);
            sb.AppendLine("\t\t    var feedback = new FeedBack();");
            sb.AppendLine("\t\t");
            sb.AppendFormat("\t\t    var item = id.Select(a => new {0} {{ id = new Guid(a) }});\r\n", tableName);
            sb.AppendLine("\t\t");
            sb.AppendFormat("\t\t    var dbresult = db.BulkDelete{0}(item);\r\n", tableName);
            sb.AppendLine("\t\t");
            sb.AppendLine("\t\t    var result = new ResultStatusUI");
            sb.AppendLine("\t\t    {");
            sb.AppendLine("\t\t        Result = dbresult.result,");
            sb.AppendLine("\t\t        FeedBack = dbresult.result ? feedback.Success(\"Silme işlemi başarılı\") : feedback.Error(\"Silme işlemi başarılı\")");
            sb.AppendLine("\t\t    };");
            sb.AppendLine("\t\t");
            sb.AppendLine("\t\t    return Json(result, JsonRequestBehavior.AllowGet);");
            sb.AppendLine("\t\t}");
            return sb.ToString();
        }
        #endregion
        private void CreateIndex(ClassInfo info, string baseRoot)
        {
            var root = Path.Combine(baseRoot, "Views", info.ViewName);
            var sb = new StringBuilder();

            sb.AppendLine("@{");
            sb.AppendLine("    ViewBag.Title = \"" + info.Alias + "\";");
            sb.AppendLine("    Layout = \"~/Views/Shared/_Layout.cshtml\";");
            sb.AppendLine("}");
            sb.AppendLine();
            sb.AppendLine();
            sb.AppendLine("<div class=\"row\">");
            sb.AppendLine("     <div class=\"col-md-2\">");
            sb.AppendLine("         <div class=\"ibox\">");
            sb.AppendLine("             <div class=\"ibox-content mailbox-content\">");
            sb.AppendLine("                 <div class=\"file-manager\">");
            sb.AppendLine("                     <a class=\"btn btn-block btn-success\" data-task=\"Insert\" data-href=\"/" + info.AreaName + "/" + info.ViewName + "/Insert\" data-modal=\"true\" data-method=\"get\">");
            sb.AppendLine("                         <i class=\"fa fa-plus-circle\"></i> " + info.Alias + " Oluştur");
            sb.AppendLine("                     </a>");
            sb.AppendLine("                 </div>");
            sb.AppendLine("             </div>");
            sb.AppendLine("         </div>");
            sb.AppendLine("     </div>");
            sb.AppendLine("     <div class=\"col-md-10\">");
            sb.AppendLine("         <div class=\"ibox\">");
            sb.AppendLine("             <div class=\"ibox-title\">");
            sb.AppendLine("                 <h5>"+ info.Alias +"</h5>");
            sb.AppendLine("                 <div class=\"ibox-tools\">");
            sb.AppendLine("                     <a class=\"collapse-link\">");
            sb.AppendLine("                         <i class=\"fa fa-chevron-up\"></i>");
            sb.AppendLine("                     </a>");
            sb.AppendLine("                 </div>");
            sb.AppendLine("             </div>");
            sb.AppendLine("             <div class=\"ibox-content\">");

            sb.AppendLine("@(Html.Akilli()");
            sb.AppendFormat("    .Grid<{0}.BusinessData.{1}>(\"{1}\")\r\n", SolutionName, info.ViewName);
            sb.AppendFormat("    .DataSource(x => x.Ajax().Read(r => r.Action(\"DataSource\", \"{0}\", new {{ area = \"{1}\" }})).PageSize(25))\r\n", info.ViewName, info.AreaName);
            sb.AppendLine("    .Columns(x =>");
            sb.AppendLine("    {");

            foreach (var item in info.Properties.Where(a => new string[] { "created", "createdby", "changed", "changedby" }.Contains(a.Name) == false))
            {
                if (item.Name != "id")
                {
                    sb.AppendFormat("        x.Bound(y => y.{0}).Title(\"{1}\").Width(130)", item.RelationName, item.Alias);
                    if (item.Type == typeof(DateTime) || item.Type == typeof(DateTime?))
                        sb.Append(".Format(Extensions.DateFormatShort(true))");

                    sb.Append(";");
                }

                sb.AppendLine();
            }

            sb.AppendLine("    })");
            sb.AppendLine("    .Scrollable(x => x.Height(650))");
            sb.AppendLine("    .Selectable(x => x.Mode(GridSelectionMode.Single))");
            sb.AppendLine("    .ToolBar(x =>");
            sb.AppendLine("    {");
            sb.AppendFormat("        x.Custom().Text(\"<i data-original-title='" + info.Alias + " Düzenle' class='fa fa-edit'></i>\").HtmlAttributes(new Dictionary<string, object>() {{ {{ \"data-show\", \"single\" }}, {{ \"data-method\", \"GET\" }} }}).Url(Url.Action(\"Update\", \"{0}\", new {{ area = \"{1}\" }}));", info.ViewName, info.AreaName); sb.AppendLine();
            sb.AppendFormat("        x.Custom().Text(\"<i data-original-title='" + info.Alias + " Detayı' class='fa fa-info-circle'></i>\").HtmlAttributes(new Dictionary<string, object>() {{ {{ \"data-show\", \"single\" }}, {{ \"data-default\", \"\" }} }}).Url(Url.Action(\"Detail\", \"{0}\", new {{ area = \"{1}\" }}));", info.ViewName, info.AreaName); sb.AppendLine();
            sb.AppendFormat("        x.Custom().Text(\"<i data-original-title='" + info.Alias + " Sil' class='fa fa-trash'></i>\").HtmlAttributes(new Dictionary<string, object>() {{ {{ \"data-ask\", \"\" }} }}).Url(Url.Action(\"Delete\", \"{0}\", new {{ area = \"{1}\" }}));", info.ViewName, info.AreaName); sb.AppendLine();
            sb.AppendLine("    }))");

            sb.AppendLine("             </div>");
            sb.AppendLine("         </div>");
            sb.AppendLine("     </div>");
            sb.AppendLine("</div>");

            CreateFile(sb.ToString(), root, "Index.cshtml");
        }
        private void CreateUpsert(ClassInfo info, string baseRoot, string action)
        {
            var root = Path.Combine(baseRoot, "Views", info.ViewName);
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("@model {0}.BusinessData.{1}\r\n", SolutionName, info.ViewName);
            sb.AppendLine("@{");
            sb.AppendLine("    ViewBag.Title = \"" + info.Alias + " " + (action == "Insert" ? "Ekle" : "Güncelle") + "\";");
            sb.AppendLine("    Layout = \"~/Views/Shared/_Layout.cshtml\";");
            sb.AppendLine("}");
            sb.AppendLine("");
            sb.AppendFormat("@using(Html.BeginForm(\"{0}\", \"{1}\", FormMethod.Post, new Dictionary<string, object>() {{  \r\n", action, info.ViewName);
            sb.AppendLine("    { \"class\", \"form-horizontal\" },");
            sb.AppendLine("    { \"role\", \"form\" },");
            sb.AppendLine("    { \"data-selector\", \"modalContainer\" },");
            sb.AppendLine("    { \"data-formType\", \"Ajax\" }");
            sb.AppendLine("}))");
            sb.AppendLine("{");
            sb.AppendLine("    @Html.AntiForgeryToken()");
            sb.AppendLine("    @Html.HiddenFor(model => model.id)");
            sb.AppendLine("");
            sb.AppendLine("");



            foreach (var item in info.Properties.Where(a => new string[] { "created", "createdby", "changed", "changedby" }.Contains(a.Name) == false))
            {
                if (item.Name != "id")
                {

                    sb.AppendLine("    <div class=\"form-group\">");
                    sb.AppendLine("        <div class=\"col-md-4\">");
                    sb.AppendFormat("            <label class=\"control-label label-md\" for=\"{0}\">{1}</label>\r\n", item.Name, item.Alias);
                    sb.AppendLine("        </div>");
                    sb.AppendLine("        <div class=\"col-md-8\">");
                    sb.Append(GetRazorInputByType(item, info.Name));
                    sb.AppendLine("        </div>");
                    sb.AppendLine("    </div>");
                    sb.AppendLine("");
                    sb.AppendLine("");
                }

                sb.AppendLine();
            }

            sb.AppendLine("    <div class=\"buttons\">");
            sb.AppendLine("        <button class=\"btn btn-md btn-danger pull-left\" data-task=\"modalClose\">Geri</button>");
            sb.AppendLine("        <button class=\"btn btn-md btn-success pull-right\" type=\"submit\">Kaydet</button>");
            sb.AppendLine("    </div>");
            sb.AppendLine("");
            sb.AppendLine("}");

            CreateFile(sb.ToString(), root, action + ".cshtml");
        }
        private void CreateDetail(ClassInfo info, string baseRoot)
        {
            var root = Path.Combine(baseRoot, "Views", info.ViewName);
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("@model {0}.BusinessData.{1}\r\n", SolutionName, info.ViewName);
            sb.AppendLine("@{");
            sb.AppendLine("    ViewBag.Title = \"" + info.Alias + " Detay\";");
            sb.AppendLine("    Layout = \"~/Views/Shared/_Layout.cshtml\";");
            sb.AppendLine("}");
            sb.AppendLine("");
            CreateFile(sb.ToString(), root, "Detail.cshtml");
        }
        private void CreateAreaRegistration(ClassInfo info, string baseRoot)
        {

            var root = Path.Combine(baseRoot);
            var str = @"using System.Web.Mvc;
                namespace {0}.Areas.{1}
                {{
                    public class {1}AreaRegistration : AreaRegistration
                    {{
                        public override string AreaName
                        {{
                            get
                            {{
                                return ""{1}"";
                            }}
                        }}

                        public override void RegisterArea(AreaRegistrationContext context)
                        {{
                            context.MapRoute(
                                ""{1}_default"",
                                ""{1}/{{controller}}/{{action}}/{{id}}"",
                                new {{ action = ""Index"", id = UrlParameter.Optional }}
                            );
                        }}
                    }}
                }}";


            CreateFile(string.Format(str, ProjectName, info.AreaName), root, info.AreaName + "AreaRegistration.cs");
        }
        private void CreateWebConfig(string baseRoot)
        {
            var root = Path.Combine(baseRoot, "Views");
            var str = @"<?xml version=""1.0""?>
                <configuration>
                  <configSections>
                    <sectionGroup name=""system.web.webPages.razor"" type=""System.Web.WebPages.Razor.Configuration.RazorWebSectionGroup, System.Web.WebPages.Razor, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31BF3856AD364E35"">
                      <section name=""host"" type=""System.Web.WebPages.Razor.Configuration.HostSection, System.Web.WebPages.Razor, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31BF3856AD364E35"" requirePermission=""false"" />
                      <section name=""pages"" type=""System.Web.WebPages.Razor.Configuration.RazorPagesSection, System.Web.WebPages.Razor, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31BF3856AD364E35"" requirePermission=""false"" />
                    </sectionGroup>
                  </configSections>

                  <system.web.webPages.razor>
                    <host factoryType=""System.Web.Mvc.MvcWebRazorHostFactory, System.Web.Mvc, Version=5.2.3.0, Culture=neutral, PublicKeyToken=31BF3856AD364E35"" />
                    <pages pageBaseType=""System.Web.Mvc.WebViewPage"">
                      <namespaces>
                        <add namespace=""System.Web.Mvc"" />
                        <add namespace=""System.Web.Mvc.Ajax"" />
                        <add namespace=""System.Web.Mvc.Html"" />
                        <add namespace=""System.Web.Routing"" />
                        <add namespace=""System.Web.Optimization"" />
                        <add namespace=""Kendo.Mvc.UI"" />
                        <add namespace=""{0}"" />
                        <add namespace=""{1}"" />
                        <add namespace=""{2}"" />
                      </namespaces>
                    </pages>
                  </system.web.webPages.razor>

                  <appSettings>
                    <add key=""webpages:Enabled"" value=""false"" />
                  </appSettings>

                  <system.webServer>
                    <handlers>
                      <remove name=""BlockViewHandler""/>
                      <add name=""BlockViewHandler"" path=""*"" verb=""*"" preCondition=""integratedMode"" type=""System.Web.HttpNotFoundHandler"" />
                    </handlers>
                  </system.webServer>
                </configuration>";



            CreateFile(string.Format(str, ProjectName, ProjectName.Replace("WebProject", "BusinessData"), ProjectName.Replace("WebProject", "BusinessAccess")), root, "Web.config");
        }
        private string GetRazorInputByType(PropInfo prop, string TableName)
        {
            StringBuilder sb = new StringBuilder();

            if (prop.Type == typeof(bool) || prop.Type == typeof(bool?))
            {
                sb.AppendLine("\t\t\t @(Html.Kendo().CheckBoxFor(a=>a." + prop.Name + ").Checked(true))");
            }
            else if ((prop.Type == typeof(Guid) || prop.Type == typeof(Guid?)) && prop.RelationTable != null)
            {
                sb.AppendLine("\t\t\t  @(");
                sb.AppendLine("\t\t\t      Html.Akilli()");
                sb.AppendLine("\t\t\t      .DropDownListFor(model => model." + prop.Name + ")");
                sb.AppendLine("\t\t\t      .OptionLabel(\"Lütfen " + prop.Alias + " seçiniz..\")");
                sb.AppendLine("\t\t\t      .DataTextField(\"" + prop.RelationPreviewColumns.FirstOrDefault() + "\")");
                sb.AppendLine("\t\t\t      .DataValueField(\"" + prop.RelationColumn + "\")");
                sb.AppendLine("\t\t\t      .Action(b => b.Action(\"DataSourceDropDown\", \"VW" + prop.RelationTable + "\", new { area = \"" + prop.RelationTable.Split('_').FirstOrDefault() + "\" }))");
                sb.AppendLine("\t\t\t      .Sort(x => x.Add(\"" + prop.RelationPreviewColumns.FirstOrDefault() + "\").Ascending())");
                sb.AppendLine("\t\t\t      .Validate(Validations.Required)");
                sb.AppendLine("\t\t\t      .Execute()");
                sb.AppendLine("\t\t\t)");
            }
            else if ((typeof(int) == prop.Type || typeof(int?) == prop.Type || typeof(short) == prop.Type || typeof(short?) == prop.Type) && prop.Enum != null)
            {
                sb.AppendLine("\t\t\t @(");
                sb.AppendLine("\t\t\t     Html.Akilli().");
                sb.AppendLine("\t\t\t     DropDownListFor(model => model." + prop.Name + ").");
                sb.AppendLine("\t\t\t     OptionLabel(\"Lütfen " + prop.Alias + " seçiniz..\").");
                sb.AppendLine("\t\t\t     DataTextField(\"enumDescription\").");
                sb.AppendLine("\t\t\t     DataValueField(\"Id\").");
                sb.AppendLine("\t\t\t     Action(b => b.Action(\"GetEnums\", \"General\", new { area = string.Empty })).");
                sb.AppendLine("\t\t\t     Filter<Infoline.WorkOfTime.BusinessData.SYS_Enums>(a => a.tableName == \"" + TableName + "\" && a.fieldName == \"" + prop.Name + "\").");
                sb.AppendLine("\t\t\t     Sort(x => x.Add(\"enumDescription\").Ascending()).");
                sb.AppendLine("\t\t\t     Validate(Validations.Required).");
                sb.AppendLine("\t\t\t     Execute(\"enumKey\")");
                sb.AppendLine("\t\t\t )");
            }
            else if (typeof(DateTime) == prop.Type || typeof(DateTime?) == prop.Type)
            {
                sb.AppendLine("\t\t\t @(");
                sb.AppendLine("\t\t\t      Html.Akilli().");
                sb.AppendLine("\t\t\t      DatePickerFor(model => model." + prop.Name + ").");
                sb.AppendLine("\t\t\t      Placeholder(\"Lütfen " + prop.Alias + " girin..\").");
                sb.AppendLine("\t\t\t      Format(Extensions.DateFormatShort()).");
                sb.AppendLine("\t\t\t      Validate(Validations.Required)");
                sb.AppendLine("\t\t\t )");
            }
            else if (typeof(IGeometry) == prop.Type)
            {

                sb.AppendLine("\t\t\t @( ");
                sb.AppendLine("\t\t\t     Html.Akilli().MapInputFor(a => a." + prop.Name + ").");
                sb.AppendLine("\t\t\t     HelpText(\"Lütfen " + prop.Alias + " seçiniz.\").");
                sb.AppendLine("\t\t\t     HtmlAttributes(new Dictionary<string, object>()");
                sb.AppendLine("\t\t\t     {");
                sb.AppendLine("\t\t\t         {\"class\", \"form-control\"},");
                sb.AppendLine("\t\t\t         {\"placeholder\", \"Lütfen " + prop.Alias + " Giriniz..\"}");
                sb.AppendLine("\t\t\t     }).");
                sb.AppendLine("\t\t\t     Opened(true).");
                sb.AppendLine("\t\t\t     Validate(Validations.Required).");
                sb.AppendLine("\t\t\t     ZoomLevel(5).");
                sb.AppendLine("\t\t\t     Navigation(false).");
                sb.AppendLine("\t\t\t     Searchable(true)");
                sb.AppendLine("\t\t\t )");
            }
            else
            {
                sb.AppendLine("\t\t\t @Html.TextBoxFor(model => model." + prop.Name + ", new Dictionary<string, object>()");
                sb.AppendLine("\t\t\t {");
                sb.AppendLine("\t\t\t     {\"class\", \"form-control\"},");
                sb.AppendLine("\t\t\t     {\"placeholder\", \"Lütfen " + prop.Alias + " giriniz..\"}");
                sb.AppendLine("\t\t\t }).Validate(Validations.TextEverywhere(true))");
            }

            return sb.ToString();
        }

        private void CreateFile(string content, string path, string fileName)
        {
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            var filePath = Path.Combine(path, fileName);
            FileStream fs = null;
            try
            {
                if (!File.Exists(filePath))
                {
                    fs = new FileStream(filePath, FileMode.CreateNew);
                    using (StreamWriter writer = new StreamWriter(fs, Encoding.UTF8))
                    {
                        writer.Write(content);
                    }
                }
                else
                {
                    File.Delete(filePath);
                    fs = new FileStream(filePath, FileMode.CreateNew);
                    using (StreamWriter writer = new StreamWriter(fs, Encoding.UTF8))
                    {
                        writer.Write(content);
                    }
                }
            }
            finally
            {
                if (fs != null)
                    fs.Dispose();
            }
        }

    }







}
