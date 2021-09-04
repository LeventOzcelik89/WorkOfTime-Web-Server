using HtmlAgilityPack;
using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessAccess;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Routing;
using System.Web.UI.WebControls;
using TuesPechkin;

namespace System.Web.Mvc
{
    public class AllowEveryone : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            return;
        }
    }


    public class ExportPDF : ActionFilterAttribute
    {
        public static IConverter pdfConvert = new ThreadSafeConverter(new PdfToolset(new WinAnyCPUEmbeddedDeployment(new StaticDeployment(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "files", "TuesPechkin2", TenantConfig.Tenant.TenantCode.ToString())))));

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            var httpContext = filterContext.HttpContext;
            var originalFilter = httpContext.Response.Filter;
            var url = httpContext.Request.Url;
            var schemaUrl = url.Scheme + "://" + url.Authority;
            var fileName = DateTime.Now.ToString("yyyyMMdd-HHMMss") + ".pdf";
            httpContext.Response.Filter = new KeywordStream(filterContext.HttpContext, originalFilter, schemaUrl, fileName);
            httpContext.Response.ContentType = MimeMapping.GetMimeMapping(fileName);
            httpContext.Response.AddHeader("content-disposition", " filename=" + fileName);
            //httpContext.Response.AddHeader("content-disposition", "attachment; filename=" + fileName);
            base.OnResultExecuted(filterContext);
        }

        public class KeywordStream : MemoryStream
        {
            StreamWriter stream;
            public string _schema = "";
            public string _filename = "";
            public string[] attributes = new string[] { "href", "src", "url" };
         

            public KeywordStream(HttpContextBase context, Stream s, string schema, string filename)
            {
                stream = new StreamWriter(s);
                _schema = schema;
                _filename = filename;
            }

            public KeywordStream(Stream s, string schema, string filename)
            {
                stream = new StreamWriter(s);
                _schema = schema;
                _filename = filename;
                //language = context.Request.Cookies["language"] != null ? context.Request.Cookies["language"].Value : "TR";

            }

            public override void Close()
            {
                byte[] allBytes = this.ToArray();
                var sb = new StringBuilder();
                for (int i = 0; i < Math.Ceiling(allBytes.Length / 2147000000.0); i++)
                {
                    sb.Append(System.Text.Encoding.UTF8.GetString(allBytes.Skip(i * 2147000000).Take(2147000000).ToArray()));
                }
                var source = sb.ToString();

                var document = new HtmlDocument();
                document.LoadHtml(source);

                var nodes = document.DocumentNode.Descendants().ToList();

                for (int i = 0; i < nodes.Count(); i++)
                {
                    var node = nodes[i];
                    for (int ai = 0; ai < attributes.Length; ai++)
                    {
                        var attribute = attributes[ai];
                        if ((node.Name == "link" || node.Name == "img" || node.Name == "script") && node.Attributes.Contains(attribute))
                        {
                            var value = node.Attributes[attribute].Value;
                            if (!string.IsNullOrEmpty(value) && node.Attributes[attribute].Value.StartsWith("http") == false)
                            {
                                node.Attributes[attribute].Value = _schema + node.Attributes[attribute].Value.TrimStart('~');
                            }
                        }
                    }
                }

                var bytes = GetPdfAsByteArray(_filename, document.DocumentNode.OuterHtml);
                stream.BaseStream.Write(bytes, 0, bytes.Length);
                stream.Flush();
                stream.Close();
                base.Close();
            }

  
        }

        public static byte[] GetPdfAsByteArray(string fileName, string text)
        {
            try
            {
                var document = new HtmlToPdfDocument
                {
                    GlobalSettings =
                    {
                        ProduceOutline = true,
                        DocumentTitle = fileName,
                        PaperSize = PaperKind.Letter,
                        Margins =
                        {
                            All = 0.8,
                            Top = 1.1,
                            Unit = TuesPechkin.Unit.Centimeters
                        }
                    },
                    Objects =
                    {
                        new ObjectSettings
                        {
                            HtmlText = text,
                            WebSettings = new WebSettings{
                                 EnableJavascript = true,
                                 EnablePlugins = true,

                            },
                            ProduceExternalLinks = true,
                            ProduceLocalLinks = true,
                        }
                    },
                };

                return pdfConvert.Convert(document);
            }
            catch
            {
                return null;
            }
        }

    
    }



    public sealed class UserRoleHtmlControl : ActionFilterAttribute
    {
        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {

            var httpContext = filterContext.HttpContext;

            try
            {
                if (filterContext.RequestContext.RouteData.Values.Values.Contains("PrintQrCodes"))
                {
                    return;
                }
            }
            catch { }



            if (httpContext.Response.ContentType.IndexOf("pdf") > -1)
            {
                return;
            }

            if (httpContext.Session == null || httpContext.Session["userStatus"] == null)
            {
                base.OnResultExecuted(filterContext);
                return;
            }


            var originalFilter = httpContext.Response.Filter;
            var pageSecurity = (PageSecurity)httpContext.Session["userStatus"];
            if (originalFilter == null || pageSecurity == null || pageSecurity.UnAuthorizedPage == null)
            {
                base.OnResultExecuted(filterContext);
                return;
            }
            filterContext.HttpContext.Response.Filter = new KeywordStream(filterContext.HttpContext, originalFilter, pageSecurity);
            base.OnResultExecuted(filterContext);
        }

        public class KeywordStream : MemoryStream
        {
            StreamWriter stream;
            public PageSecurity pageSecurity;
            public string[] attributes = new string[] { "href", "data-href", "data-url" };
            public string language = "TR";
            public static Dictionary<string, Dictionary<string, string>> languageSource = new Dictionary<string, Dictionary<string, string>>();
            public static DateTime lastLanguageWrite;
            public string[] pageList;

            public KeywordStream(HttpContextBase context, Stream s, PageSecurity pSecurity)
            {
                stream = new StreamWriter(s);
                pageSecurity = pSecurity;


                language = context.Request.Cookies["language"] != null ? context.Request.Cookies["language"].Value : "TR";

                var languageFile = context.Server.MapPath("/Language/Sources/global" + TenantConfig.Tenant.Config.LanguageCode + ".json");

                try
                {
                    if (System.IO.File.Exists(languageFile))
                    {
                        var lastWriteTime = System.IO.File.GetLastWriteTime(languageFile);
                        if (lastWriteTime != lastLanguageWrite)
                        {
                            lastLanguageWrite = lastWriteTime;
                            string text = System.IO.File.ReadAllText(languageFile);
                            var lngSource = Infoline.Helper.Json.Deserialize<Dictionary<string, Dictionary<string, string>>>(text);
                            languageSource = new Dictionary<string, Dictionary<string, string>>();
                            foreach (var item in lngSource)
                            {
                                var key = cleanString(item.Key);
                                var current = languageSource.Where(a => a.Key == key);

                                if (current.Count() == 0)
                                {
                                    languageSource.Add(key, item.Value);
                                }
                                else
                                {
                                    if (item.Value.Count() > current.Select(a => a.Value.Count()).FirstOrDefault())
                                    {
                                        languageSource[key] = item.Value;
                                    }
                                }
                            }
                        }

                    }
                }
                catch (Exception)
                {

                }
            }


            public override void Close()
            {

                byte[] allBytes = this.ToArray();
                string source = Text.Encoding.UTF8.GetString(allBytes);
                HtmlDocument document = new HtmlDocument();

                var reg = new Regex("(<body(.*)? class=\")");
                var res = reg.Match(source);
                if (res.Success == true)
                {
                    var bodyClass = res.ToString();
                    var newBodyClass = bodyClass.Replace("class=\"", "class=\"lang" + language + " ");
                    source = source.Replace(bodyClass, newBodyClass);
                }


                document.LoadHtml(source);


                List<HtmlNode> nodes = document.DocumentNode.Descendants().ToList();
                for (int i = 0; i < nodes.Count(); i++)
                {
                    var node = nodes[i];
                    for (int ai = 0; ai < attributes.Length; ai++)
                    {
                        var attribute = attributes[ai];
                        if (node.Name != "link" && node.Attributes.Contains(attribute))
                        {
                            var value = node.Attributes[attribute].Value;

                            if (!string.IsNullOrEmpty(value))
                            {
                                foreach (var item in pageSecurity.UnAuthorizedPage)
                                {
                                    var basevalue = value.Split(new string[] { "?" }, StringSplitOptions.RemoveEmptyEntries).First();
                                    var basevalue2 = basevalue;

                                    var pathLArr = basevalue.Split('/');

                                    if (pathLArr.Length == 2)
                                    {
                                        basevalue2 = basevalue + "/Index";
                                    }

                                    if (pathLArr.Length == 3)
                                    {
                                        basevalue2 = basevalue + "/Index";
                                    }

                                    if (pathLArr.Length == 5)
                                    {
                                        basevalue2 = basevalue.Replace("/" + pathLArr[4], "");
                                    }

                                    if (basevalue == item || basevalue2 == item)
                                    {
                                        nodeDetected(node).Remove();
                                    };
                                }
                            }
                        }
                    }

                    if (languageSource.Count() > 0 && language != "TR")
                    {
                        if (node.NodeType == HtmlNodeType.Text)
                        {
                            var text = cleanString(node.InnerText);
                            var current = languageSource.Where(a => a.Key == text);

                            if (current != null && current.Count() > 0)
                            {
                                var selected = current.Select(a => a.Value.Where(c => c.Key == language && c.Value != "").Select(c => c.Value).FirstOrDefault()).FirstOrDefault();
                                if (!string.IsNullOrEmpty(selected))
                                {
                                    node.ParentNode.ReplaceChild(HtmlTextNode.CreateNode(selected), node);
                                }
                            }
                        }
                        changeAttr(node, "placeholder");
                        changeAttr(node, "title");
                        changeAttr(node, "data-original-title");
                    }
                }

        


                var menu = nodes.Where(a => a.Name == "ul" && a.Attributes.Contains("id") && a.Attributes["id"].Value == "side-menu").FirstOrDefault();
                if (menu != null)
                {
                    var li = menu.Descendants().Where(a => a.Name == "li").Select(a => new
                    {
                        node = a,
                        count = a.Descendants().Count(b =>
                        b.Name == "a" && b.Attributes.Contains("href") && (b.Attributes["href"].Value != "#" && b.Attributes["href"].Value != ""))
                    }).Where(a => a.count == 0).ToArray();


                    foreach (var item in li)
                    {
                        item.node.Remove();
                    }
                }


                stream.Write(document.DocumentNode.OuterHtml);
                stream.Flush();
                stream.Close();
                base.Close();
            }

            public void changeAttr(HtmlNode node, string attr)
            {

                if (node.Attributes.Contains(attr) && node.Attributes[attr].Value != null)
                {
                    var current = languageSource.Where(a => a.Key == cleanString(node.Attributes[attr].Value));
                    if (current.Count() > 0)
                    {
                        var selected = current.Select(a => a.Value.Where(c => c.Key == language && c.Value != "").Select(c => c.Value).FirstOrDefault()).FirstOrDefault();
                        if (!string.IsNullOrEmpty(selected))
                        {
                            node.Attributes[attr].Value = selected;
                        }
                    }
                }
            }


            public HtmlNode nodeDetected(HtmlNode node)
            {
                if (node.ParentNode == null)
                {
                    return node;
                }

                if (node.ParentNode.ChildNodes.Where(a => a.NodeType != HtmlNodeType.Text).Count() == 1)
                {
                    return nodeDetected(node.ParentNode);
                }
                else
                {
                    return node;
                }
            }
            public string cleanString(string str)
            {

                str = str.Trim()
                    .Replace(System.Environment.NewLine, " ")
                    .Replace("\r\n", " ")
                    .Replace("\n", " ")
                    .Replace("\r", " ")
                    .Replace("\t", " ")
                    .Replace("&#32;", " ");

                while (str.IndexOf("  ") > 0)
                {
                    str = str.Replace("  ", " ");
                }

                return str;
            }
        }

        

    }
    public sealed class UserRoleControl : AuthorizeAttribute
    {
        public static Dictionary<string, string> CustomAttr = new Dictionary<string, string>();
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var httpContext = filterContext.RequestContext.HttpContext;
            var area = filterContext.RouteData.DataTokens["area"] != null ? "/" + filterContext.RouteData.DataTokens["area"].ToString() : "";
            var controller = filterContext.RouteData.Values["controller"].ToString();
            var action = filterContext.RouteData.Values["action"].ToString();
            var pageURL = string.Format("{0}/{1}/{2}", area, controller, action);


            if (!(filterContext.ActionDescriptor.IsDefined(typeof(AllowEveryone), true) || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowEveryone), true)))
            {

                if (httpContext.Session == null || httpContext.Session["userStatus"] == null)
                {
                    if (filterContext.HttpContext.Request.IsAjaxRequest())
                    {
                        filterContext.Result = new ContentResult
                        {
                            Content = Infoline.Helper.Json.Serialize(new ResultStatus
                            {
                                objects = new FeedBack().Warning("Kullanıcı girişi yapmanız gerekiyor.", false, "/Account/SignIn"),
                                message = "UserControl",
                                result = false,
                            }),
                            ContentType = "application/json",
                            ContentEncoding = Text.Encoding.UTF8,
                        };
                    }
                    else
                    {
                        var url = new Infoline.Framework.Helper.CryptographyHelper().Encrypt(filterContext.HttpContext.Request.RawUrl);
                        filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary
                        {
                            { "action", "SignIn" },
                            { "controller", "Account" },
                            { "area", String.Empty },
                            { "returnUrl", url }
                        });
                    }
                    return;
                }


                var userStatus = (PageSecurity)httpContext.Session["userStatus"];

                if (userStatus.UnAuthorizedPage.Count(a => a.ToUpper(new CultureInfo("en-US", false)) == pageURL.ToUpper(new CultureInfo("en-US", false))) > 0)
                {
                    var kullanici = userStatus.user.FullName + " (" + userStatus.user.loginname + ")";
                    var url = ((System.Web.HttpRequestWrapper)filterContext.HttpContext.Request);
                    var text = "<strong>" + kullanici + " isimli kullanıcı</strong>";
                    text += "<p>" + DateTime.Now.ToString("dd.MM.yyyy HH:mm") + " tarihinde <a href='" + url.Url.AbsoluteUri + "'>" + url.Url.AbsoluteUri + "</a> sayfasında yetki hatası almıştır.</p>";
                    if (url.UrlReferrer != null)
                    {
                        text += "<br/> İlgili sayfaya <a href='" + url.UrlReferrer.AbsoluteUri + "'>" + url.UrlReferrer.AbsoluteUri + " sayfasından gelmiştir.</a>";
                    }

#if !DEBUG
                    Log.Error("Sayfa Yetki Hatası : " + text);
                    new Email().Send((Int16)EmailSendTypes.ZorunluMailler,"sahin.elik@infoline-tr.com", "Sayfa Yetki Hatası", text, true, false, new string[] { "oguz.yavuz@infoline-tr.com", "senol.elik@infoline-tr.com", "ahmet.undemir@infoline-tr.com", "ahmet.temiz@infoline-tr.com" });
#endif
                    if (filterContext.HttpContext.Request.IsAjaxRequest())
                    {
                        filterContext.Result = new ContentResult
                        {
                            Content = Infoline.Helper.Json.Serialize(new ResultStatus
                            {
                                objects = new FeedBack().Warning("Yapmak istediğiniz istek için tarafınıza yetki verilmemiş ! Yöneticiniz ile görüşün.", false),
                                message = "UserControl",
                                result = false,
                            }),
                            ContentType = "application/json",
                            ContentEncoding = Text.Encoding.UTF8,
                        };
                    }
                    else
                    {
                        if (filterContext.IsChildAction == false)
                        {

                            var uri = new Infoline.Framework.Helper.CryptographyHelper().Encrypt(filterContext.HttpContext.Request.RawUrl);
                            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary
                            {
                                { "action", "AccessDenied" },
                                { "controller", "Error" },
                                { "area",String.Empty },
                                { "returnUrl", uri }
                            });

                        }
                    }
                    return;
                }
            }
        }


    }
}