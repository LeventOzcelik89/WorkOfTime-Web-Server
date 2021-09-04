using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.ModelBinding;
using System.Web.Mvc;


namespace System.Web.Mvc
{
    public class UIBoolBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            if (value != null)
            {
                if (value.AttemptedValue == "1")
                {
                    return true;
                }
                else if (value.AttemptedValue == "0")
                {
                    return false;
                }
            }
            return base.BindModel(controllerContext, bindingContext);
        }
    }
}