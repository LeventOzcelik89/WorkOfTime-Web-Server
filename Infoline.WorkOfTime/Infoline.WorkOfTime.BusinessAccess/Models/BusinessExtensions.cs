using GeoAPI.Geometries;
using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;
using System.Text;

namespace Infoline.WorkOfTime.BusinessAccess
{
    public static class BusinessExtensions
    {

        public static List<Dictionary<string, object>> B_ToDictionaryList<T>(this IEnumerable<T> filledObject) where T : class
        {

            var res = new List<Dictionary<string, object>>();

            foreach (var item in filledObject)
            {

                var copyObject = new Dictionary<string, object>();

                foreach (var prop in item.GetType().GetProperties())
                {
                    copyObject.Add(prop.Name, prop.GetValue(item, null));
                }

                res.Add(copyObject);

            }

            return res;

        }

        public static string B_ToDescription(this Enum item)
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

        public static DateTime RoundSecond(this DateTime item)
        {
            return new DateTime(item.Year, item.Month, item.Day, item.Hour, item.Minute, 0);
        }

        public static Empty B_ConvertType<Empty>(this object item)
            //  where Full : class
            where Empty : class, new()
        {

            var copyObject = new Empty();

            foreach (var prop in item.GetType().GetProperties())
            {
                var value = prop.GetValue(item, null);
                var beCopyProp = copyObject.GetType().GetProperty(prop.Name);

                if (beCopyProp == null || !beCopyProp.PropertyType.IsAssignableFrom(prop.PropertyType))
                {
                    continue;
                }

                if (beCopyProp.CanWrite)
                {
                    beCopyProp.SetValue(copyObject, value);
                }

            }

            return copyObject;

        }

        public static IEnumerable<Empty> B_ConvertType<Empty>(this IEnumerable<object> filledObject)
            where Empty : class, new()
        {

            var res = new List<Empty>();

            foreach (var item in filledObject)
            {
                var copyObject = new Empty();

                foreach (var prop in item.GetType().GetProperties())
                {
                    var value = prop.GetValue(item, null);
                    var beCopyProp = copyObject.GetType().GetProperty(prop.Name);

                    if (beCopyProp == null || !beCopyProp.PropertyType.IsAssignableFrom(prop.PropertyType))
                    {
                        continue;
                    }

                    beCopyProp.SetValue(copyObject, value);

                }

                res.Add(copyObject);

            }

            return res.ToArray();

        }

        public static List<Dictionary<string, object>> B_ToDictionaryList<TSource, TKey>(this IEnumerable<TSource> filledObject, Expression<Func<TSource, TKey>> fields)
        {

            var props = ExpressionHelper.GetProperties<TSource, TKey>(fields);
            //  address, birthday, city

            var res = new List<Dictionary<string, object>>();

            foreach (var item in filledObject)
            {

                var copyObject = new Dictionary<string, object>();

                foreach (var prop in item.GetType().GetProperties().Where(a => props.Select(b => b.Name).Contains(a.Name)))
                {
                    copyObject.Add(prop.Name, prop.GetValue(item));
                }

                res.Add(copyObject);

            }

            return res;

        }

        public static IEnumerable<Dictionary<string, object>> B_ToDictionaryList(this IEnumerable<InfolineTable> filledObject)
        {

            var res = new List<Dictionary<string, object>>();

            foreach (var item in filledObject)
            {

                var copyObject = new Dictionary<string, object>();

                foreach (var prop in item.GetType().GetProperties())
                {
                    copyObject.Add(prop.Name, prop.GetValue(item, null));
                }

                res.Add(copyObject);

            }

            return res;

        }

        public static T B_EntityDataCopyForMaterial<T, T2>(this T entity, T2 copyData, bool infolineBase = true)
        {

            var model = copyData.GetType().GetProperties();
            if (!infolineBase)
            {
                model = model.Where(prop => !new InfolineTable().GetType().GetProperties().Select(x => x.Name).Contains(prop.Name)).ToArray();
            }

            foreach (var prop in model)
            {
                try
                {
                    var value = prop.GetValue(copyData, null);
                    if (value == null) continue;

                    var isThere = entity.GetType().GetProperties().Where(x => x.Name == prop.Name).FirstOrDefault();
                    if (isThere != null)
                    {
                        if (prop.PropertyType.GenericTypeArguments.Length > 0)
                        {

                            isThere.SetValue(entity, Convert.ChangeType(value, prop.PropertyType.GenericTypeArguments[0], CultureInfo.InvariantCulture));

                        }
                        else
                        {
                            if (prop.PropertyType == typeof(IGeometry))
                            {
                                isThere.SetValue(entity, value);
                            }
                            else
                            {
                                isThere.SetValue(entity, Convert.ChangeType(value, prop.PropertyType, CultureInfo.InvariantCulture));
                            }
                        }
                    }
                }
                catch { }

            }
            return entity;
        }

        public static T EntityFill<T, T2>(this T viewDATA, T2 databaseDATA, bool infolineBASE = true, bool force = false)
        {

            foreach (var databaseProp in databaseDATA.GetType().GetProperties().Where(a => infolineBASE ? true : !new InfolineTable().GetType().GetProperties().Select(b => b.Name).Contains(a.Name)).ToArray())
            {
                try
                {
                    var value = databaseProp.GetValue(databaseDATA, null);
                    if (value == null) continue;

                    var viewProp = viewDATA.GetType().GetProperty(databaseProp.Name);
                    if (viewProp != null)
                    {
                        if (!force || (force == false && viewProp.GetValue(viewDATA) != null))
                        {
                            if (databaseProp.PropertyType.GenericTypeArguments.Length > 0)
                            {
                                viewProp.SetValue(databaseDATA, Convert.ChangeType(value, databaseProp.PropertyType.GenericTypeArguments[0], CultureInfo.InvariantCulture));
                            }
                        }
                        else
                        {
                            if ((!force || (force == false && viewProp.GetValue(viewDATA) != null)))
                            {
                                if (databaseProp.PropertyType == typeof(IGeometry))
                                {
                                    viewProp.SetValue(databaseProp, value);
                                }
                                else
                                {
                                    viewProp.SetValue(databaseProp, Convert.ChangeType(value, databaseProp.PropertyType, CultureInfo.InvariantCulture));
                                }
                            }
                        }
                    }
                }
                catch (Exception) { }
            }

            return viewDATA;

        }

        public static T AppendObjectToOther<T, T2>(this T baseObject, T2 beCopyObject)
        {
            foreach (var beCopyProp in beCopyObject.GetType().GetProperties().Where(prop => !new InfolineTable().GetType().GetProperties().Select(x => x.Name).Contains(prop.Name)))
            {
                var value = beCopyProp.GetValue(beCopyObject, null);
                var baseObjectProp = baseObject.GetType().GetProperties().Where(x => x.Name == beCopyProp.Name).FirstOrDefault();

                if (baseObjectProp == null || !baseObjectProp.GetType().IsAssignableFrom(beCopyProp.GetType()))
                {
                    continue;
                }

                if (baseObjectProp != null)
                {
                    if (beCopyProp.PropertyType.GenericTypeArguments.Length > 0)
                    {
                        baseObjectProp.SetValue(baseObject, value);
                    }
                    else
                    {
                        baseObjectProp.SetValue(baseObject, value);
                    }
                }

            }
            return baseObject;
        }

        public static T[] AppendObjectsToOther<T, T2>(this T baseObject, T2[] beCopyObject)
            where T : class
            where T2 : class
        {
            var res = new List<T>();
            foreach (var item in beCopyObject)
            {
                res.Add(baseObject.AppendObjectToOther(item));
            }

            return res.ToArray();
        }

        /// <summary>
        ///     db.getSH_User().FirstOrDefault().PrependObjectToOther ( new VW_SHUser() )
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="baseObject"></param>
        /// <param name="beCopyObject"></param>
        /// <returns></returns>
        public static T2 PrependObjectToOther<T, T2>(this T baseObject, T2 beCopyObject)
        {
            foreach (var baseObjectProp in baseObject.GetType().GetProperties().Where(prop => !new InfolineTable().GetType().GetProperties().Select(x => x.Name).Contains(prop.Name)))
            {
                var value = baseObjectProp.GetValue(baseObject, null);
                var beCopyProp = beCopyObject.GetType().GetProperties().Where(x => x.Name == baseObjectProp.Name).FirstOrDefault();

                if (beCopyProp == null || !beCopyProp.GetType().IsAssignableFrom(baseObjectProp.GetType()))
                {
                    continue;
                }

                if (beCopyProp != null)
                {
                    if (baseObjectProp.PropertyType.GenericTypeArguments.Length > 0)
                    {
                        beCopyProp.SetValue(beCopyObject, value);
                    }
                    else
                    {
                        beCopyProp.SetValue(beCopyObject, value);
                    }
                }

            }
            return beCopyObject;
        }

        /// <summary>
        ///     db.getSH_User().PrependObjectToOther ( new VW_SHUser() )
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="baseObject"></param>
        /// <param name="beCopyObject"></param>
        /// <returns></returns>
        public static T2[] PrependObjectsToOther<T, T2>(this T[] baseObject, T2 beCopyObject) where T2 : new()
        {
            var res = new List<T2>();
            foreach (var item in baseObject)
            {
                res.Add(item.PrependObjectToOther(new T2()));
            }

            return res.ToArray();
        }

        private static Random random = new Random();
        private static string RandomNumber(int length)
        {
            string randomValue = "12345647890";
            string result = "";
            for (var i = 0; i < length; i++)
            {
                result += randomValue[random.Next(randomValue.Length)];
            }
            return result;
        }


        public static string B_GetIdCode()
        {
            DateTime d = DateTime.Now;
            return string.Format("{0:yyyyMMdd}", d) + RandomNumber(6);
        }


        public static Guid? B_ToGuid(this string item)
        {
            Guid TryParse;

            if (!String.IsNullOrEmpty(item) && Guid.TryParse(item.Trim(), out TryParse))
            {
                return TryParse;
            }

            return null;

        }

        public static int? B_ToInt32(this string item)
        {

            Int32 tryParse;

            if (!String.IsNullOrEmpty(item) && Int32.TryParse(item.Trim(), NumberStyles.Any, new CultureInfo("tr-TR"), out tryParse))
            {
                return Convert.ToInt32(item);
            }

            return 0;

        }

        public static T B_RemoveGeographies<T>(this T item) where T : class
        {

            if (item == null)
                return item;

            foreach (var propertyInfo in item.GetType().GetProperties().Where(a => a.PropertyType.IsAssignableFrom(typeof(IGeometry))).ToList())
            {
                propertyInfo.SetValue(item, null);
            }

            return item;

        }

        public static T[] B_RemoveGeographies<T>(this T[] items) where T : new()
        {

            if (items == null)
                return items;

            foreach (var item in items)
            {
                foreach (var propertyInfo in item.GetType().GetProperties().Where(a => a.PropertyType.IsAssignableFrom(typeof(IGeometry))).ToList())
                {
                    propertyInfo.SetValue(item, null);
                }
            }

            return items;

        }

        public static IGeometry B_DGSToGeography(double LatDegree, double LatMinute, double LatSeconds, double LongDegree, double LongMinute, double LongSeconds)
        {

            var _lat = (LatDegree + LatMinute / 60 + LatSeconds / 3600);
            var _long = (LongDegree + LongMinute / 60 + LongSeconds / 3600);

            _lat = Math.Round(_lat * 1e7) / 1e7;

            var c = new NetTopologySuite.IO.WKTReader();
            return c.Read(string.Format("POINT ({0} {1})", _long.ToString().Replace(",", "."), _lat.ToString().Replace(",", ".")));

        }

        public static double GetRemainingExcuse(this VWSH_User item)
        {

            if (item.JobStartDate != null)
            {
                if (item.JobStartDate.Value.Year == DateTime.Now.Year)
                {
                    var month = (DateTime.Now.Month - item.JobStartDate.Value.Month);
                    if (TenantConfig.Tenant.TenantCode == 1192)
                    {
                        return 60 - item.PermitExcuseUsed;
                    }
                    return (month < 7 ? 14 : 27) - item.PermitExcuseUsed;
                }
                else
                {
                    if (TenantConfig.Tenant.TenantCode == 1192)
                    {
                        return 60 - item.PermitExcuseUsed;
                    }
                    return 27 - item.PermitExcuseUsed;
                }
            }

            return 0;

        }

        public static string GetStringFromUrl(string url)
        {

            var wrGETURL = WebRequest.Create(url);
            wrGETURL.Method = "GET";
            wrGETURL.Timeout = 35000;
            wrGETURL.ContentType = "APPLICATION/JSON";
            Stream objStream;
            string text;

            try
            {
                objStream = wrGETURL.GetResponse().GetResponseStream();

                using (StreamReader objReader = new StreamReader(objStream))
                {
                    text = objReader.ReadToEnd();
                }

                return text;

            }
            catch (Exception)
            {
                return null;
            }

        }

        public static ResultStatus<string> GetRemoteString(string url)
        {
            try
            {
                using (var webClient = new WebClient())
                {

                    webClient.Encoding = Encoding.UTF8;

                    var resp = webClient.DownloadString(url);

                    return new ResultStatus<string>
                    {
                        result = true,
                        objects = resp
                    };

                }

            }
            catch (Exception ex)
            {
                return new ResultStatus<string>
                {
                    message = ex.Message
                };
            }

        }

    }

    public class KeyValue
    {
        public string Key { get; set; }
        public object Value { get; set; }
        public object Value2 { get; set; }
        public object Tooltip { get; set; }
        public string Color { get; set; }
    }


    public class KeyValueFiveObject
    {
        public string Key { get; set; }
        public object Value { get; set; }
        public object Value2 { get; set; }
        public object Value3 { get; set; }
        public object Value4 { get; set; }
        public object Value5 { get; set; }

    }

    public class KeyValues
    {
        public string Key { get; set; }
        public double?[] Values { get; set; }
    }
}
