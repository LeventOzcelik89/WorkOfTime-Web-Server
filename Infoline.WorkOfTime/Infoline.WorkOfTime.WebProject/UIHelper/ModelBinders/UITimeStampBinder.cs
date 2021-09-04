using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Infoline.WorkOfTime.BusinessAccess;
using Kendo.Mvc.Extensions;

namespace System.Web.Mvc
{
    /// <summary>
    /// TimeStamp convert to DateTİme
    /// </summary>
    public class UITimeStampBinder : IModelBinder
    {
        public string GetRequiredString(ControllerContext controllerContext, string key)
        {
            if (controllerContext.RouteData.Values[key] != null)
            {
                return controllerContext.RouteData.Values[key].ToString();
            }
            return controllerContext.HttpContext.Request[key];
        }
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var request = controllerContext.HttpContext.Request;
            var model = bindingContext.ModelMetadata.Container;

            if (model == null)
            {
                var res = GetRequiredString(controllerContext, bindingContext.ModelName + "_TimeStamp");
                if (res != null)
                {
                    return new DateTime(1970, 1, 1, 0, 0, 0, 0).AddMilliseconds(Convert.ToInt64(res)).ToLocalTime();
                }
                return Convert.ToDateTime(GetRequiredString(controllerContext, bindingContext.ModelName));

            }
            else
            {
                try
                {
                    var param = request.Params.Keys;
                    foreach (var key in param)
                    {
                        if (key != null && key.ToString().EndsWith("_TimeStamp"))
                        {
                            var dt = key.ToString().Replace("_TimeStamp", "");
                            if (dt != null)
                            {
                                var entity = model.GetType().GetProperties().FirstOrDefault(x => x.Name == dt);
                                var value = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddMilliseconds(Convert.ToInt64(request[key.ToString()])).ToLocalTime();

                                if (value != null && entity != null)
                                {
                                    entity.SetValue(model, value);
                                }
                            }
                        }
                        else if (bindingContext.ModelName.Equals(key))
                        {
                            var entity = model.GetType().GetProperties().FirstOrDefault(x => x.Name.Equals(bindingContext.ModelName));
                            entity.SetValue(model, Convert.ToDateTime(request[bindingContext.ModelName]));
                        }
                    }
                }
                catch (Exception ex) { new FeedBack().Error(ex.Message.ToString()); }
            }


            return model;
        }
    }
    //public class InterfaceModelBinder : DefaultModelBinder
    //{
    //    public override object BindModel(ControllerContext controllerContext,
    //        ModelBindingContext bindingContext)
    //    {



    //        if (controllerContext.RequestContext.HttpContext.Request.Form.AllKeys.Where(x => x.EndsWith("_TimeStamp")).Count() > 0)
    //        {


    //            var request = controllerContext.RequestContext.HttpContext.Request.Form;


    //            foreach (var row in controllerContext.RequestContext.HttpContext.Request.Form.AllKeys.ToList())
    //            {

    //                if (!bindingContext.ModelMetadata.AdditionalValues.ContainsKey(row))
    //                {
    //                    Type desiredType = bindingContext.ValueProvider.GetValue(row).AttemptedValue.GetType();
    //                    bindingContext.ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(null, desiredType);

    //                    if (row.EndsWith("_TimeStamp"))
    //                    {
    //                        var dt = row.Replace("_TimeStamp", "");
    //                        var deger = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddMilliseconds(Convert.ToInt64(request[row])).ToLocalTime();
    //                        if (bindingContext.ModelMetadata.AdditionalValues.ContainsKey(dt) == true)
    //                        {
    //                            bindingContext.ModelMetadata.AdditionalValues[dt] = deger;
    //                        }
    //                        else
    //                        {
    //                            bindingContext.ModelMetadata.AdditionalValues.Add(dt, deger);
    //                        }
    //                    }
    //                    else
    //                    {
    //                        bindingContext.ModelMetadata.AdditionalValues.Add(row, request[row]);
    //                    }
    //                }
    //            }
    //        }

    //        return base.BindModel(controllerContext, bindingContext);
    //    }
    //}

    //public class CustomProvider : ValueProviderFactory
    //{
    //    public override IValueProvider GetValueProvider(ControllerContext controllerContext)
    //    {
    //        var ParamsTimeStamp = controllerContext.HttpContext.Request.Params.AllKeys;
    //        var backingStore = new Dictionary<string, object>();
    //        foreach (var key in ParamsTimeStamp)
    //        {
    //            if (key != null && key.EndsWith("_TimeStamp"))
    //            {
    //                var dt = key.Replace("_TimeStamp", "");
    //                if (dt != null)
    //                {
    //                    var nvc = HttpUtility.ParseQueryString(dt);
    //                    foreach (var key2 in nvc)
    //                    {
    //                        var value = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddMilliseconds(Convert.ToInt64(controllerContext.HttpContext.Request[key])).ToLocalTime();
    //                        if (value != null)
    //                        {
    //                            backingStore.Add(key2.ToString(), value);
    //                        }
    //                    }

    //                }
    //            }
    //            else
    //            {
    //                var nvc = HttpUtility.ParseQueryString(key);
    //                foreach (string key2 in nvc)
    //                {
    //                    backingStore.Add(key2, nvc[key2]);
    //                }
    //            }
    //        }


    //        return new DictionaryValueProvider<object>(backingStore, CultureInfo.CurrentCulture);
    //    }
    //}
}


