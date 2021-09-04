using Infoline.WorkOfTime.BusinessData;
using Infoline.WorkOfTime.WebProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Infoline.Web.Utility
{
    class LanguageSearch
    {

        public ProjectPages[] GetAllPages()
        {
            var projectName = Assembly.GetExecutingAssembly().FullName.Split(',')[0];
            Assembly asm = Assembly.GetAssembly(typeof(MvcApplication));

            var model = asm.GetTypes()
                .SelectMany(t => t.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public))
                .Where(d => d.ReturnType.Name == "ActionResult")
                .Select(n => new ProjectPages
                {
                    Controller = n.DeclaringType != null ? n.DeclaringType.Name.Replace("Controller", "") : null,
                    Action = n.Name,
                    Area = n.DeclaringType.Namespace.ToString().Replace(projectName + ".", "").Replace("Areas.", "").Replace(".Controllers", "").Replace("Controllers", ""),
                    Attributes = String.Join(",", n.GetCustomAttributes(true).Select(a => a.GetType().Name.Replace("Attribute", "")))
                })
                .ToArray();

            var list = new List<SH_Pages>();
            var controllers = AppDomain.CurrentDomain.GetAssemblies()
                .Where(assembly => assembly.FullName.StartsWith("Infoline.WorkOfTime.WebProject"))
                .Select(a => a.GetTypes().Where(c => c.Name.EndsWith("Controller"))).FirstOrDefault();

            if (controllers == null)
            {
                //return list;
            }


            foreach (var cntrlr in controllers)
            {
                var ctrlrattr = cntrlr.GetCustomAttributes(false).Select(o => (((Attribute)o).TypeId)).Select(o => ((Type)o).Name).ToList();
                var methods = cntrlr.GetMethods().Where(a => a.DeclaringType.FullName.StartsWith("Infoline.WorkOfTime.WebProject")).ToArray();
                foreach (var mthd in methods.GroupBy(a => a.Name))
                {
                    string area = cntrlr.FullName.IndexOf("Areas.") > -1 ? cntrlr.FullName.Split(new string[] { "Areas." }, StringSplitOptions.RemoveEmptyEntries).LastOrDefault().Split('.').FirstOrDefault() : null;
                    string controller = cntrlr.Name.Replace("Controller", "");
                    string method = mthd.Key;
                    string action = String.Format("{0}/{1}/{2}", string.IsNullOrEmpty(area) ? "" : "/" + area, controller, method);
                    var attr = mthd.SelectMany(a => a.GetCustomAttributes(false).Select(o => (((Attribute)o).TypeId)).Select(o => ((Type)o).Name)).ToList();

                    var page = new SH_Pages
                    {
                        id = Guid.NewGuid(),
                        created = DateTime.Now,
                        Action = action,
                        Area = area,
                        Controller = controller,
                        Method = method,
                        AllowEveryone = ctrlrattr.Contains("AllowEveryone") || attr.Contains("AllowEveryone")
                    };

                    if (attr.IndexOf("PageInfo") > -1)
                    {
                        var pageInfoList = mthd.SelectMany(a => a.GetCustomAttributes(typeof(PageInfo), false)).Select(a => (PageInfo)a).ToArray();
                        page.Description = pageInfoList.Where(a => a.Description != null).Select(a => a.Description).FirstOrDefault();
                    }

                    list.Add(page);
                }
            }


            foreach (var mod in model)
            {
                var rr = (!string.IsNullOrEmpty(mod.Area) ? "/" : "") + mod.Area + "/" + mod.Controller + "/" + mod.Action;
                var res = list.Where(x => x.Action == rr).FirstOrDefault();
                if (res != null)
                {
                    if (!string.IsNullOrEmpty(res.Description))
                    {
                        mod.Description = res.Description;
                    }
                }

            }

            return model;
        }



    }

    public class ProjectPages
    {
        public string Controller { get; set; }
        public string Action { get; set; }
        public string Area { get; set; }
        public string Attributes { get; set; }
        public string Description { get; set; }
    }
}
