using Infoline.WorkOfTime.BusinessAccess;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.Collections.Specialized;
using Newtonsoft.Json;

using Infoline.Framework.Database;
using System.Net.Http;
using Infoline.Web.Utility;
using System.IO;

namespace Infoline.WorkOfTime.WebProject.Areas.SYS.Controllers
{
    public class LanguageController : Controller
    {
        public static Dictionary<string, Dictionary<string, string>> Global = new Dictionary<string, Dictionary<string, string>>();

        public string cleanString(string str)
        {

            str = str.Trim().Replace(System.Environment.NewLine, " ").Replace("\r\n", " ").Replace("\n", " ").Replace("\r", " ").Replace("\t", " ");

            while (str.IndexOf("  ") > 0)
            {
                str = str.Replace("  ", " ");
            }

            return str;
        }

        private void staticLoad()
        {
            var sourcedirectory = Server.MapPath("~/Language/Sources/");
            var recoverydirectory = Server.MapPath("~/Language/Recovery/");
            if (!System.IO.Directory.Exists(sourcedirectory))
            {
                System.IO.Directory.CreateDirectory(sourcedirectory);
            }
            if (!System.IO.Directory.Exists(recoverydirectory))
            {
                System.IO.Directory.CreateDirectory(recoverydirectory);
            }

            if (Global != null && Global.Count() == 0)
            {
                if (!System.IO.File.Exists(Server.MapPath("/Language/Sources/global" + TenantConfig.Tenant.Config.LanguageCode + ".json")))
                {
                    System.IO.File.WriteAllText(Server.MapPath("/Language/Sources/global" + TenantConfig.Tenant.Config.LanguageCode + ".json"), "{}");
                }
                string text = System.IO.File.ReadAllText(Server.MapPath("/Language/Sources/global" + TenantConfig.Tenant.Config.LanguageCode + ".json"));
                Global = Infoline.Helper.Json.Deserialize<Dictionary<string, Dictionary<string, string>>>(text);
            }

        }

        public class Translation
        {
            public int code { get; set; }
            public string lang { get; set; }
            public List<string> text { get; set; }
        }

        private static readonly HttpClient client = new HttpClient();

        [PageInfo("Dil Tanımlamaları", SHRoles.Cevirmen)]
        public ActionResult Index()
        {
            staticLoad();
            return View();
        }

        [AllowEveryone]
        public ContentResult GetPages()
        {

            var data = new LanguageSearch().GetAllPages()
                .Where(a => System.IO.File.Exists(Server.MapPath(String.Concat((string.IsNullOrEmpty(a.Area) ? "~" : "~/Areas/" + a.Area), "/Views/", a.Controller, "/", a.Action, ".cshtml"))))
                .Where(a => a.Attributes.IndexOf("HttpPost") == -1)
                .OrderBy(a => a.Area)
                .ToArray();

            var result = data.Where(x => !string.IsNullOrEmpty(x.Description)).Select(a => new
            {
                Area = a.Area,
                Controller = a.Controller,
                Action = a.Action,
                Value = a.Area + "-" + a.Controller + "-" + a.Action + ".json",
                Text = "/" + (string.IsNullOrEmpty(a.Area) ? "" : a.Area + "/") + a.Controller + "/" + a.Action,
                Description = a.Description,
                DescAll = a.Description +" (" +  "/" + (string.IsNullOrEmpty(a.Area) ? "" : a.Area + "/") + a.Controller + "/" + a.Action + ")"
            }).OrderBy(a => a.Description).ToArray();
            return Content(Infoline.Helper.Json.Serialize(result), "application/json");
        }

        [HttpPost]
        public Guid? GetParam(string Controller)
        {

            Guid? id = null;
            var db = new WorkOfTimeDatabase();

            try
            {
                id = db.GetTableToId(Controller);
            }
            catch (Exception ) { }

            return id;
        }

        [AllowEveryone]
        [HttpPost]
        public JsonResult Save(string json, string fileName)
        {

            try
            {

                var sourcedirectory = Server.MapPath("~/Language/Sources/");
                var recoverydirectory = Server.MapPath("~/Language/Recovery/");

                var pageSourcePath = sourcedirectory + fileName;
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(pageSourcePath))
                {
                    file.Write(json);
                }

                staticLoad();
                /*Global.json güncelleniyor*/
                var texts = Infoline.Helper.Json.Deserialize<Dictionary<string, Dictionary<string, string>>>(json);
                foreach (var item in texts)
                {
                    if (Global.Count(a => a.Key == item.Key) == 0)
                    {
                        Global.Add(item.Key, item.Value);
                    }
                    else
                    {
                        foreach (var item2 in item.Value)
                        {
                            if (Global[item.Key].Count(a => a.Key == item2.Key) == 0)
                            {
                                Global[item.Key].Add(item2.Key, item2.Value);
                            }
                            else
                            {
                                Global[item.Key][item2.Key] = item2.Value;
                            }
                        }
                    }
                }

                var globalSourcePath = sourcedirectory + "global" + TenantConfig.Tenant.Config.LanguageCode + ".json";
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(globalSourcePath))
                {
                    var globaljson = Infoline.Helper.Json.Serialize(Global);
                    file.Write(globaljson);
                }

                /*Global.json güncelleniyor*/


                ///*Yedek*/
                if (System.IO.File.Exists(pageSourcePath))
                {
                    System.IO.File.Copy(pageSourcePath, recoverydirectory + fileName.Substring(0, fileName.Length - 5) + "_" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss") + ".json");
                }

                if (System.IO.File.Exists(globalSourcePath))
                {
                    System.IO.File.Copy(globalSourcePath, recoverydirectory + "global_" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss") + ".json");
                }
                ///*Yedek*/


                return Json(true, JsonRequestBehavior.AllowGet);


            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return Json(false, JsonRequestBehavior.AllowGet);
            }


        }
    }
}