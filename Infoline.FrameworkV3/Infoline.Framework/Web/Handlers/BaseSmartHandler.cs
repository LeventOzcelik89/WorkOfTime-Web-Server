using GeoAPI.Geometries;
using Infoline.Framework.Database;
using Infoline.Helper;
using Microsoft.SqlServer.Types;
using NetTopologySuite.IO;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using System.Xml.Serialization;

namespace Infoline.Web.SmartHandlers
{
    public abstract class BaseSmartHandler : ISmartHandler
    {

        string name;
        string[] parameters;
        public string Name
        {
            get { return name; }
        }
        public string[] Parameters
        {
            get { return parameters; }
        }

        public BaseSmartHandler(string name) : this(name, new string[0]) { }

        public BaseSmartHandler(string name, string[] parameters)
        {
            this.name = name;
            this.parameters = parameters;
        }

        protected static string HandlerUrl(string name, params object[] parameters)
        {
            return Application.Current.GetService<ISmartHandlerService>().HandlerUrl(name, parameters);
        }

        virtual public void ProcessRequest(HttpContext context, IDictionary<string, object> paramters)
        {

        }
        public static void RenderResponse(HttpContext context, object data)
        {
            data = CheckIsNull(data);
            data = CheckException(context, data);
            string header = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">\n<html xmlns=\"http://www.w3.org/1999/xhtml\">\n<head>\n<title></title>\n<meta http-equiv=\"Content-Type\" content=\"text/html; charset=windows-1254\" />\n<style>\n</style>\n</head>\n<body>\n";
            var bodyStream = new StreamReader(context.Request.InputStream);
            bodyStream.BaseStream.Seek(0, SeekOrigin.Begin);
            var bodyText = bodyStream.ReadToEnd();
            ReturnTypeConverter(context, data, header);
        }
        private static void ReturnTypeConverter(HttpContext context, object d, string header)
        {
            if (context.Request.QueryString["f"] == "txt")
            {
                ReturnTypeTxt(context, d, header);
            }
            else if (context.Request.QueryString["f"] == "xls")
            {
                ReturnTypeXls(context, d, header);
            }
            else if (context.Request.QueryString["f"] == "xml")
            {
                ReturnTypeXml(context, d, header);
            }
            else if (context.Request.QueryString["f"] == "csv")
            {
                ReturnTypeCsv(context, d, header);
            }
            else if (context.Request.QueryString["f"] == "rjson")
            {
                ReturnTypeRjson(context, d, header);
            }
            else if (context.Request.QueryString["f"] == "csvspecial")
            {
                ReturnTypeCsvSpecial(context, d, header);
            }
            else
            {
                ReturnTypeJson(context, d, header);
            }
        }
        protected static void RenderResponse(HttpContext context, DataTable d)
        {
            if (context.Request.QueryString["f"] == "xml" || context.Request.QueryString["f"] == "xml")
            {
                context.Response.ContentType = "application/xml";
                d.TableName = "Item";
                for (int i = 0; i < d.Columns.Count; i++)
                    d.Columns[i].ColumnMapping = MappingType.Attribute;
                using (StringWriter sw = new StringWriter())
                {
                    d.WriteXml(sw, XmlWriteMode.IgnoreSchema);
                    context.Response.Write(sw.ToString().Replace("DocumentElement", "Items"));
                }
            }
            else if (context.Response.ContentType.ToLower() == "image/jpeg")
            {
                //TODO: 
            }
            else
            {
                context.Response.ContentType = "application/json";
                context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(d, new JsonIGeometryConverter(), new JsonSqlGeometryConverter()));
            }

        }
        protected static T ParseRequest<T>(HttpContext context)
        {
            var readedContent = new System.IO.StreamReader(context.Request.InputStream, context.Request.ContentEncoding).ReadToEnd();
            context.Response.ContentType = "application/json";
            return (T)Infoline.Helper.Json.Deserialize<T>(readedContent, new JsonIGeometryConverter(), new JsonSqlGeometryConverter());
        }
        protected static T ParseString<T>(string str, string format = "json")
        {
            if (format == "xml")
            {
                return (T)new System.Xml.Serialization.XmlSerializer(typeof(T)).Deserialize(new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(str ?? "")));
            }
            else
            {
                return (T)Newtonsoft.Json.JsonConvert.DeserializeObject<T>(str);
            }
        }
        protected static T ParseRequestGeometry<T>(HttpContext context, ref string data)
        {
            if (context.Request.QueryString["f"] == "xml")
            {
                context.Request.InputStream.Position = 0;
                var reader = new StreamReader(context.Request.InputStream);
                var text = reader.ReadToEnd();
                return ParseString<T>(text, "xml");
            }
            else
            {
                data += " InputStream Leng:" + context.Request.InputStream.Length.ToString() + ",";

                context.Response.ContentType = "application/json";
                var readedContent = new System.IO.StreamReader(context.Request.InputStream, context.Request.ContentEncoding).ReadToEnd();

                data += " " + readedContent + " ,";



                return (T)Newtonsoft.Json.JsonConvert.DeserializeObject<T>(readedContent);
            }
        }
        private static void ReturnTypeJson(HttpContext context, object d, string header)
        {
            context.Response.ContentType = "application/json";
            context.Response.Write(JsonConvert.SerializeObject(d, new FeatureCollectionJsonConverter(), new FeatureJsonConverter(), new GeometryJsonConverter(), new EnvelopeJsonConverter()));
        }
        private static void ReturnTypeRjson(HttpContext context, object d, string header)
        {
            context.Response.ContentType = "application/json";
            var str = JsonConvert.SerializeObject(d, new JsonIGeometryConverter(), new JsonSqlGeometryConverter());
            Infoline.Framework.Helper.JsonFormatter formatter = new Infoline.Framework.Helper.JsonFormatter();
            str = formatter.PrettyPrint(str);
            context.Response.Write(str);
        }
        private static void ReturnTypeXml(HttpContext context, object d, string header)
        {
            context.Response.Clear();
            context.Response.ContentType = "text/xml, application/xml";
            context.Response.ContentEncoding = System.Text.Encoding.UTF8;

            XmlAttributeOverrides xmlAttributeOverrides = new XmlAttributeOverrides();
            CheckUnnownTypeAndAddXmlAttr(d, xmlAttributeOverrides);

            //TODO: ResultStatus içerisindeji object içerisi Generic List ise hata veriyor Serialize etmiyor. 
            var serializer = new XmlSerializer(d != null ? d.GetType() : typeof(string), xmlAttributeOverrides);
            var stringWriter = new StringWriter();
            using (StringWriter writer = new StringWriter())
            {
                serializer.Serialize(writer, d);
                stringWriter = writer;
            }
            context.Response.Write(stringWriter.ToString());
        }
        private static void ReturnTypeCsv(HttpContext context, object d, string header)
        {
            var _separator = ";";

            context.Response.Clear();
            context.Response.ContentEncoding = System.Text.Encoding.GetEncoding("windows-1254");
            context.Response.Charset = "windows-1254";

            context.Response.ContentType = "text/csv";
            context.Response.AddHeader("Content-Disposition", "attachment;filename=\"" + "csv" + ".csv\"");

            var jsonRes = JsonConvert.SerializeObject(d, new JsonIGeometryConverter(), new JsonSqlGeometryConverter());

            if (!d.GetType().IsArray)
            {
                if (d.GetType().IsAssignableFrom(typeof(string)))
                {
                    context.Response.Write(d);
                }
                else
                {
                    context.Response.Write(jsonRes);
                }
                return;
            }

            var sb = new StringBuilder();
            var dt = JsonConvert.DeserializeObject<DataTable>(jsonRes);
            var headers = "";

            foreach (DataColumn col in dt.Columns)
            {
                headers += _separator + col.ColumnName;
            }

            sb.AppendLine(headers.Substring(1));

            foreach (DataRow row in dt.Rows)
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    sb.Append(row[i] + (i != dt.Columns.Count - 1 ? ";" : ""));
                }
                sb.AppendLine();
            }

            context.Response.Write(sb.ToString());
        }
        private static void ReturnTypeXls(HttpContext context, object d, string header)
        {
            context.Response.Clear();
            context.Response.ContentEncoding = System.Text.Encoding.GetEncoding("windows-1254");
            context.Response.Charset = "windows-1254";

            context.Response.Buffer = true;
            context.Response.ContentType = "application/vnd.ms-excel";
            context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + "xls.xls" + "");


            var jsonRes = JsonConvert.SerializeObject(d, new JsonIGeometryConverter(), new JsonSqlGeometryConverter());

            if (!d.GetType().IsArray)
            {
                if (d.GetType().IsAssignableFrom(typeof(string)))
                {
                    context.Response.Write(d);
                }
                else
                {
                    context.Response.Write(jsonRes);
                }
                return;
            }

            var tw = new System.IO.StringWriter();
            var hw = new System.Web.UI.HtmlTextWriter(tw);

            var dGrid = new DataGrid();
            dGrid.DataSource = d;
            dGrid.DataBind();
            dGrid.RenderControl(hw);

            context.Response.Write(tw);
        }
        private static void ReturnTypeTxt(HttpContext context, object d, string header)
        {
            context.Response.Clear();
            context.Response.ContentEncoding = System.Text.Encoding.GetEncoding("windows-1254");
            context.Response.Charset = "windows-1254";
            context.Response.Buffer = true;

            string filename = string.Format("{0}.txt", d.GetType().Name.Replace("[", "").Replace("]", ""));

            System.IO.StringWriter stream = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(stream);

            DataGrid dgGrid = new DataGrid();
            dgGrid.DataSource = d;
            dgGrid.DataBind();
            dgGrid.RenderControl(hw);

            context.Response.ContentType = "text/plain";
            context.Response.AddHeader("Content-Disposition", "attachment; filename=" + filename + "");
            context.Response.Write(stream);

            stream.Close();
            context.Response.End();
        }

        private static void ReturnTypeCsvSpecial(HttpContext context, object d, string header)
        {
            var _separator = ",";

            context.Response.Clear();
            context.Response.ContentEncoding = System.Text.Encoding.GetEncoding("windows-1254");
            context.Response.Charset = "windows-1254";

            context.Response.ContentType = "text/csv";
            context.Response.AddHeader("Content-Disposition", "attachment;filename=\"" + "csv" + ".csv\"");

            var jsonRes = JsonConvert.SerializeObject(d, new JsonIGeometryConverter(), new JsonSqlGeometryConverter());

            if (!d.GetType().IsArray)
            {
                if (d.GetType().IsAssignableFrom(typeof(string)))
                {
                    context.Response.Write(d);
                }
                else
                {
                    context.Response.Write(jsonRes);
                }
                return;
            }

            var sb = new StringBuilder();
            var dt = JsonConvert.DeserializeObject<DataTable>(jsonRes);
            var headers = "";

            foreach (DataColumn col in dt.Columns)
            {
                headers += _separator + col.ColumnName;
            }
            headers += ";";

            sb.AppendLine(headers.Substring(1));

            foreach (DataRow row in dt.Rows)
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    sb.Append(row[i] + (i != dt.Columns.Count - 1 ? "," : ""));
                }
                sb.AppendLine();
            }

            context.Response.Write(sb.ToString());

        }

        private static void CheckUnnownTypeAndAddXmlAttr(object d, XmlAttributeOverrides xmlAttributeOverrides)
        {
            XmlAttributes xmlAttributes = new XmlAttributes();
            xmlAttributes.XmlIgnore = true;

            //TODO: Çıkarılan Element yerine yeni element ekleme yapılması lazım.
            if (d.GetType().IsArray)
            {
                var dictionaryOfCheck = new Dictionary<string, int>();
                IEnumerable enumerable = d as IEnumerable;
                foreach (var item in enumerable)
                {
                    foreach (var prob in item.GetType().GetProperties())
                    {
                        if ((prob.PropertyType == typeof(IGeometry) || prob.PropertyType == typeof(SqlGeography) || prob.PropertyType == typeof(SqlGeometry)) && dictionaryOfCheck.Where(a => a.Key == prob.Name).FirstOrDefault().Value == 0)
                        {
                            xmlAttributeOverrides.Add(item.GetType(), prob.Name, xmlAttributes);
                            dictionaryOfCheck.Add(prob.Name, 1);
                        }
                    }
                }
            }
            else if (d.GetType() == typeof(ResultStatus))
            {
                try
                {
                    var dictionaryOfCheck = new Dictionary<string, int>();
                    ResultStatus dd = (ResultStatus)d;

                    IEnumerable enumerable = dd.objects as IEnumerable;
                    foreach (var item in enumerable)
                    {
                        foreach (var prob in item.GetType().GetProperties())
                        {
                            if ((prob.PropertyType == typeof(IGeometry) || prob.PropertyType == typeof(SqlGeography) || prob.PropertyType == typeof(SqlGeometry)) && dictionaryOfCheck.Where(a => a.Key == prob.Name).FirstOrDefault().Value == 0)
                            {
                                xmlAttributeOverrides.Add(item.GetType(), prob.Name, xmlAttributes);
                                dictionaryOfCheck.Add(prob.Name, 1);
                            }
                        }
                    }
                }
                catch
                {

                }
            }
            else
            {
                var dictionaryOfCheck = new Dictionary<string, int>();
                foreach (var prob in d.GetType().GetProperties())
                {
                    if (prob.PropertyType == typeof(IGeometry) && dictionaryOfCheck.Where(a => a.Key == prob.Name).FirstOrDefault().Value == 0)
                    {
                        xmlAttributeOverrides.Add(d.GetType(), prob.Name, xmlAttributes);
                        dictionaryOfCheck.Add(prob.Name, 1);
                    }
                }
            }
        }
        private static object CheckIsNull(object data)
        {
            if (data == null || (data.GetType().IsArray && ((Array)data).Length < 1))
                data = new ResultStatus { message = "Data is Null", result = true, objects = null };
            return data;
        }

        private static object CheckException(HttpContext context, object data)
        {
            if (context.Request.QueryString["ResultStatus"] == "1" && data.GetType() != typeof(ResultStatus))
            {
                if (data.GetType().Name.Contains("xception"))
                    data = new ResultStatus { message = data.ToString(), result = false, objects = null };

                else if (data.GetType() == typeof(string) && data.ToString().Contains("unsuccessful"))
                    data = new ResultStatus { message = data.ToString(), result = false, objects = null };
                else
                    data = new ResultStatus { message = "Succesfully", result = true, objects = data };
            }
            return data;
        }
    }

    public class BaseServiceHandler : BaseSmartHandler
    {
        public BaseServiceHandler(string name)
            : base(name, new string[] { "cmd" })
        {

        }

        public override void ProcessRequest(HttpContext context, IDictionary<string, object> paramters)
        {
            var cmd = paramters["cmd"].ToString().ToLower(System.Globalization.CultureInfo.InvariantCulture);

            foreach (var func in this.GetType()
                .GetMethods(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.FlattenHierarchy)
                .Where(a => a.Name.ToLower(System.Globalization.CultureInfo.InvariantCulture) == cmd))
            {
                var ps = func.GetParameters();
                var pvalues = new List<object>();
                var keys = context.Request.QueryString.AllKeys.Select(a => a.ToLower()).ToArray();

                foreach (var p in ps)
                {
                    var pname = p.Name.ToLower();
                    if (keys.Contains(pname))
                    {
                        if (p.ParameterType == typeof(Guid))
                            pvalues.Add(string.IsNullOrEmpty(context.Request.QueryString[pname]) ? Guid.Empty : new Guid(context.Request.QueryString[pname]));
                        else
                            pvalues.Add(Convert.ChangeType(context.Request.QueryString[pname], p.ParameterType));
                    }
                }
                if (pvalues.Count == ps.Length)
                {
                    RenderResponse(context, func.Invoke(this, pvalues.ToArray()));
                    break;
                }
            }
        }
    }
    public class JsonSqlGeometryConverter : Newtonsoft.Json.JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(SqlGeography).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.Value == null) return null;
            var result = SqlGeography.Parse(reader.Value.ToString());
            return result;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value != null)
                serializer.Serialize(writer, value.ToString());
            else serializer.Serialize(writer, value);
        }
    }
    public class JsonIGeometryConverter : Newtonsoft.Json.JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(IGeometry).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.Value == null) return null;
            var wktReader = new WKTReader();
            var result = wktReader.Read(reader.Value.ToString());
            return result;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value != null)
                serializer.Serialize(writer, ((IGeometry)value).AsText());
            else serializer.Serialize(writer, value);
        }
    }

}
