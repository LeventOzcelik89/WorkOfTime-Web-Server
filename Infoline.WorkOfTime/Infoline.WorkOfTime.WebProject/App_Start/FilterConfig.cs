﻿using System.Web.Mvc;

namespace Infoline.WorkOfTime.WebProject
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new UserRoleControl());
            filters.Add(new UserRoleHtmlControl());
        }
    }
}
