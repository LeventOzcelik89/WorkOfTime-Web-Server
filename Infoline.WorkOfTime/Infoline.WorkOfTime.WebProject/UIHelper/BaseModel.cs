using GeoAPI.Geometries;
using Infoline.Helper;
using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData;
using Kendo.Mvc.UI.Fluent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using static Infoline.WorkOfTime.WebProject.Areas.INV.Models.INV_PermitModel;

namespace System.Web.Mvc
{
    public enum GridSelectorType
    {
        [Description("radio")]
        Radio = 0,
        [Description("checkbox")]
        Checkbox = 1
    }

    public static class BaseModel
    {

        private static IDictionary<string, object> ReplaceAttributes(ref IDictionary<string, object> Keys, Validations.ValidationUI param)
        {

            foreach (var pr in param.GetType().GetProperties().Where(a => a.GetValue(param) != null).ToList())
            {

                var nm = pr.Name.ToString().Replace("_", "-");
                var val = pr.GetValue(param).GetType().IsAssignableFrom(typeof(bool))
                    ? nm
                    : pr.GetValue(param);

                if (Keys[nm] != null)
                {
                    Keys[nm] = val;
                }
                else
                {
                    Keys.Add(nm, val);
                }

            }

            return Keys;

        }

        public static string AppendAttribute(this string item, string Key, object Value)
        {

            var pat = Key + "=\"(.*?)\"";

            if (Regex.Match(item, pat).Success)
            {
                item = Regex.Replace(item, "" + Key + "=\"(.*?)\"", Key += "=\"" + Value + "\"", RegexOptions.IgnoreCase);
            }
            else
            {
                if (item.StartsWith("<input"))
                {
                    item = item.Insert(6, " " + Key + "=\"" + Value + "\"");
                }
                else if (item.StartsWith("<div"))
                {
                    item = item.Insert(4, " " + Key + "=\"" + Value + "\"");
                }
                else if (item.StartsWith("<textarea"))
                {
                    item = item.Insert(9, " " + Key + "=\"" + Value + "\"");
                }
                else if (item.StartsWith("<select"))
                {
                    item = item.Insert(7, " " + Key + "=\"" + Value + "\"");
                }

            }

            return item;

        }

        public static string AppendAttributes(this string item, Validations.ValidationUI validate)
        {
            if (validate == null)
                return item;

            var attr = validate.GetType().GetProperties().Where(a => a.GetValue(validate) != null)
                .ToDictionary(
                k => k.Name.ToString().Replace("_", "-").ToLower(),
                v => v.GetValue(validate).GetType().IsAssignableFrom(typeof(bool)) ? v.Name.ToString().Replace("_", "-").ToLower() : v.GetValue(validate));

            return item.AppendAttributes(attr);

        }

        public static string AppendAttributes(this string item, IDictionary<string, object> attributes)
        {

            if (attributes == null)
                return item;

            foreach (var attr in attributes)
            {
                item = item.AppendAttribute(attr.Key.ToString().ToLower(), attr.Value);
            };

            return item;

        }

        public static string AsAttribute(this string item, string attribute)
        {
            return " " + attribute + "=\"" + item + "\"";
        }

        public static string AsAttribute(this bool item, string attribute)
        {
            return " " + attribute + "=\"" + item.ToString().ToLower() + "\"";
        }

        //  Validations To Kendo

        public static DatePickerBuilder Validate(this DatePickerBuilder item, Validations.ValidationUI param)
        {
            var hAttr = ((Kendo.Mvc.UI.DatePicker)item).HtmlAttributes;
            ReplaceAttributes(ref hAttr, param);
            return item;
        }

        public static DropDownListBuilder Validate(this DropDownListBuilder item, Validations.ValidationUI param)
        {
            var hAttr = ((Kendo.Mvc.UI.DropDownList)item).HtmlAttributes;
            ReplaceAttributes(ref hAttr, param);
            return item;
        }


        public static RadioButtonGroupBuilder Validate(this RadioButtonGroupBuilder item, Validations.ValidationUI param)
        {
            var hAttr = ((RadioButtonGroup)item).HtmlAttributes;
            ReplaceAttributes(ref hAttr, param);
            return item;
        }

        public static MvcHtmlString Validate(this MvcHtmlString item, Validations.ValidationUI param)
        {

            var hstr = item.ToHtmlString();

            foreach (var pr in param.GetType().GetProperties().Where(a => a.GetValue(param) != null).ToList())
            {

                var nm = pr.Name.ToString().Replace("_", "-");
                var pat = nm + "=\"(.*?)\"";

                var _val = pr.GetValue(param).GetType().IsAssignableFrom(typeof(bool)) && (bool)pr.GetValue(param) == true
                    ? nm
                    : pr.GetValue(param);

                if (pr.GetValue(param).GetType().IsAssignableFrom(typeof(bool)) && (bool)pr.GetValue(param) == false)
                    continue;

                hstr = AppendAttribute(hstr, nm, _val);

            }

            return new MvcHtmlString(hstr);

        }

        public static AutoCompleteBuilder Validate(this AutoCompleteBuilder item, Validations.ValidationUI param)
        {
            var hAttr = ((Kendo.Mvc.UI.AutoComplete)item).HtmlAttributes;
            ReplaceAttributes(ref hAttr, param);
            return item;
        }

        public static CheckBoxBuilder Validate(this CheckBoxBuilder item, Validations.ValidationUI param)
        {
            var hAttr = ((Kendo.Mvc.UI.CheckBox)item).HtmlAttributes;
            ReplaceAttributes(ref hAttr, param);
            return item;
        }

        public static ColorPickerBuilder Validate(this ColorPickerBuilder item, Validations.ValidationUI param)
        {
            var hAttr = ((Kendo.Mvc.UI.ColorPicker)item).HtmlAttributes;
            ReplaceAttributes(ref hAttr, param);
            return item;
        }

        public static ComboBoxBuilder Validate(this ComboBoxBuilder item, Validations.ValidationUI param)
        {
            var hAttr = ((Kendo.Mvc.UI.ComboBox)item).HtmlAttributes;
            ReplaceAttributes(ref hAttr, param);
            return item;
        }

        public static NumericTextBoxBuilder<long> Validate(this NumericTextBoxBuilder<long> item, Validations.ValidationUI param)
        {
            var hAttr = ((Kendo.Mvc.UI.NumericTextBox<long>)item).HtmlAttributes;
            ReplaceAttributes(ref hAttr, param);
            return item;
        }

        public static NumericTextBoxBuilder<int> Validate(this NumericTextBoxBuilder<int> item, Validations.ValidationUI param)
        {
            var hAttr = ((Kendo.Mvc.UI.NumericTextBox<int>)item).HtmlAttributes;
            ReplaceAttributes(ref hAttr, param);
            return item;
        }

        public static NumericTextBoxBuilder<short> Validate(this NumericTextBoxBuilder<short> item, Validations.ValidationUI param)
        {
            var hAttr = ((Kendo.Mvc.UI.NumericTextBox<short>)item).HtmlAttributes;
            ReplaceAttributes(ref hAttr, param);
            return item;
        }

        public static NumericTextBoxBuilder<double> Validate(this NumericTextBoxBuilder<double> item, Validations.ValidationUI param)
        {
            var hAttr = ((Kendo.Mvc.UI.NumericTextBox<double>)item).HtmlAttributes;
            ReplaceAttributes(ref hAttr, param);
            return item;
        }

        public static DateTimePickerBuilder Validate(this DateTimePickerBuilder item, Validations.ValidationUI param)
        {
            var hAttr = ((Kendo.Mvc.UI.DateTimePicker)item).HtmlAttributes;
            ReplaceAttributes(ref hAttr, param);
            return item;
        }

        public static EditorBuilder Validate(this EditorBuilder item, Validations.ValidationUI param)
        {
            var hAttr = ((Kendo.Mvc.UI.Editor)item).HtmlAttributes;
            ReplaceAttributes(ref hAttr, param);
            return item;
        }

        public static MaskedTextBoxBuilder Validate(this MaskedTextBoxBuilder item, Validations.ValidationUI param)
        {
            var hAttr = ((Kendo.Mvc.UI.MaskedTextBox)item).HtmlAttributes;
            ReplaceAttributes(ref hAttr, param);
            return item;
        }

        public static MultiSelectBuilder Validate(this MultiSelectBuilder item, Validations.ValidationUI param)
        {
            var hAttr = ((Kendo.Mvc.UI.MultiSelect)item).HtmlAttributes;
            ReplaceAttributes(ref hAttr, param);
            return item;
        }

        public static RadioButtonBuilder Validate(this RadioButtonBuilder item, Validations.ValidationUI param)
        {
            var hAttr = ((Kendo.Mvc.UI.RadioButton)item).HtmlAttributes;
            ReplaceAttributes(ref hAttr, param);
            return item;
        }

        public static RangeSliderBuilder<double> Validate(this RangeSliderBuilder<double> item, Validations.ValidationUI param)
        {
            var hAttr = ((Kendo.Mvc.UI.RangeSlider<double>)item).HtmlAttributes;
            ReplaceAttributes(ref hAttr, param);
            return item;
        }

        public static RecurrenceEditorBuilder Validate(this RecurrenceEditorBuilder item, Validations.ValidationUI param)
        {
            var hAttr = ((Kendo.Mvc.UI.RecurrenceEditor)item).HtmlAttributes;
            ReplaceAttributes(ref hAttr, param);
            return item;
        }

        public static SliderBuilder<double> Validate(this SliderBuilder<double> item, Validations.ValidationUI param)
        {
            var hAttr = ((Kendo.Mvc.UI.Slider<double>)item).HtmlAttributes;
            ReplaceAttributes(ref hAttr, param);
            return item;
        }

        public static TimePickerBuilder Validate(this TimePickerBuilder item, Validations.ValidationUI param)
        {
            var hAttr = ((Kendo.Mvc.UI.TimePicker)item).HtmlAttributes;
            ReplaceAttributes(ref hAttr, param);
            return item;
        }

        public static TimezoneEditorBuilder Validate(this TimezoneEditorBuilder item, Validations.ValidationUI param)
        {
            var hAttr = ((Kendo.Mvc.UI.TimezoneEditor)item).HtmlAttributes;
            ReplaceAttributes(ref hAttr, param);
            return item;
        }

        public static GridBoundColumnBuilder<T> GridSelector<T>(this GridBoundColumnBuilder<T> item, GridSelectorType type) where T : class
        {

            item.Column.Title = String.Empty;
            item.Column.ClientTemplate = "<input type=\"" + type.ToDescription() + "\" data-event=\"GridSelector\" />";
            item.Column.Width = "45px";
            item.Column.Sortable = false;
            item.Column.Filterable = false;
            item.Column.HtmlAttributes.Add("style", "max-width: 45px !important;");
            item.Column.HeaderHtmlAttributes.Add("style", "max-width: 45px !important;");

            return item;

        }

        public static GridBoundColumnBuilder<T> DataColumn<T>(this GridBoundColumnBuilder<T> item, Expression<Func<T, object>> expression) where T : class
        {

            MemberExpression body = expression.Body as MemberExpression;
            if (body == null)
            {
                UnaryExpression ubody = (UnaryExpression)expression.Body;
                body = ubody.Operand as MemberExpression;
            }

            item.Column.ClientTemplate = item.Column.ClientTemplate.AppendAttribute("data-" + body.Member.Name, "#=data." + body.Member.Name + "#");

            return item;
        }

    }

    public static class Extensions
    {

        public static string VersionCode
        {
            get
            {
                if (System.Configuration.ConfigurationManager.AppSettings["VersionCode"] != null)
                {
                    return System.Configuration.ConfigurationManager.AppSettings["VersionCode"].ToString();
                }

                return "1989Kl";
            }
        }

        public static object GetPropertyValue<T>(this T obj, string name) where T : class
        {
            Type t = typeof(T);
            return t.GetProperty(name).GetValue(obj, null);
        }

        public static string DateFormatShort(bool? kendoGrid = false)
        {
            var vl = DateTimeFormatInfo.CurrentInfo.ShortDatePattern;

            if (kendoGrid == true)
            {
                return "{0:" + vl + "}";
            }

            return vl;
        }

        public static string DateFormatFull(bool? kendoGrid = false)
        {
            var vl = DateFormatShort() + " " + DateTimeFormatInfo.CurrentInfo.ShortTimePattern;

            if (kendoGrid == true)
            {
                return "{0:" + vl + "}";
            }

            return vl;
        }

        public static bool IsValidGuid(this string str)
        {
            Guid guid;

            if (String.IsNullOrEmpty(str)) { return false; }

            return Guid.TryParse(str, out guid);
        }


        public static T RemoveGeographies<T>(this T item) where T : class
        {
            return BusinessExtensions.B_RemoveGeographies<T>(item);
        }

        public static T[] RemoveGeographies<T>(this T[] items) where T : new()
        {
            return BusinessExtensions.B_RemoveGeographies<T>(items);
        }

        public static string ToDescription(this Enum item)
        {
            MemberInfo[] memberInfos = item.GetType().GetMembers(BindingFlags.Public | BindingFlags.Static);

            var res = memberInfos.Where(a => a.Name == item.ToString()).FirstOrDefault();

            if (res != null)
            {
                return res.GetCustomAttributesData().Select(a => a.ConstructorArguments).FirstOrDefault().FirstOrDefault().Value.ToString();
            }
            else
            {
                return item.ToString();
            }

        }


        public static Guid? ToGuid(this string item)
        {
            return BusinessExtensions.B_ToGuid(item);

        }


        public static double? ToDouble(this string item)
        {
            double TryParse;

            if (!String.IsNullOrEmpty(item) && Double.TryParse(item.Trim().Replace(".", ","), NumberStyles.Any, new CultureInfo("tr-TR"), out TryParse))
            {
                return TryParse;
            }

            return null;

        }


        public static CultureInfo Culture(string lang = "tr-TR")
        {
            return CultureInfo.GetCultureInfo(lang);
        }

        public static T EntityDataCopyForMaterial<T, T2>(this T entity, T2 copyData, bool infolineBase = true)
        {
            return BusinessExtensions.B_EntityDataCopyForMaterial<T, T2>(entity, copyData, infolineBase);
        }



        public static string TurkceReplaceForMail(this string title)
        {
            title = title.Trim().ToLower();
            title = Regex.Replace(title, "ş", "s");
            title = Regex.Replace(title, "ı", "i");
            title = Regex.Replace(title, "ö", "o");
            title = Regex.Replace(title, "ü", "u");
            title = Regex.Replace(title, "ç", "c");
            title = Regex.Replace(title, "ğ", "g");
            title = Regex.Replace(title, "-", "");
            title = title.Trim();
            title = Regex.Replace(title, " ", "-");
            return title;
        }

        public static KeyValue[] ToOrderByDescColorGreen(this KeyValue[] param)
        {
            var seriesColor = new[] { "#9CCC65", "#8BC34A", "#7CB342", "#689F38", "#558B2F", "#33691E", "#81C784", "#66BB6A", "#4CAF50", "#43A047", "#388E3C", "#2E7D32", "#1B5E20", "#4DD0E1", "#26A69A", "#009688", "#00897B", "#00796B", "#00695C", "#004D40" }.Reverse().ToArray();
            for (int i = 0; i < param.Length; i++)
            {
                param[i].Color = seriesColor[i];
            }
            return param.OrderByDescending(x => x.Value).ToArray();
        }



        public static ChartKeyValueDesc[] ToOrderByDescColorGreen(this ChartKeyValueDesc[] param)
        {
            var seriesColor = new[] { "#9CCC65", "#8BC34A", "#7CB342", "#689F38", "#558B2F", "#33691E", "#81C784", "#66BB6A", "#4CAF50", "#43A047", "#388E3C", "#2E7D32", "#1B5E20", "#4DD0E1", "#26A69A", "#009688", "#00897B", "#00796B", "#00695C", "#004D40" }.Reverse().ToArray();
            for (int i = 0; i < param.Length; i++)
            {
                param[i].Color = seriesColor[i];
            }
            return param.OrderByDescending(x => x.Value).ToArray();
        }

        public static Empty ConvertType<Empty>(this object item)
            //  where Full : class
            where Empty : class, new()
        {
            return BusinessExtensions.B_ConvertType<Empty>(item);
        }

        public static IEnumerable<Empty> ConvertType<Empty>(this IEnumerable<object> filledObject)
            where Empty : class, new()
        {
            return BusinessExtensions.B_ConvertType<Empty>(filledObject);
        }

        public static string NewCode
        {
            get
            {
                return BusinessExtensions.B_GetIdCode();
            }
        }



        public static string HtmlRaw(this DateTime? date)
        {
            if (date.HasValue)
            {
                return date.Value.ToString("dd.MM.yyyy HH:mm");
            }
            else
            {
                return "-";
            }
        }

        public static string HtmlRaw(this DateTime? date, string format)
        {
            if (date.HasValue)
            {
                return date.Value.ToString(format);
            }
            else
            {
                return "-";
            }
        }

        public static string HtmlRaw(this DateTime? date, string format, string defaultValue)
        {
            if (date.HasValue)
            {
                return date.Value.ToString(format);
            }
            else
            {
                return defaultValue;
            }
        }




    }
}