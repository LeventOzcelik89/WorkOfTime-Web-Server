using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Web.Hosting;
using System.IO;
using System.Reflection;
using System.Linq;
using System.ComponentModel.Composition;
using System.Collections.Generic;
using System.Web.Caching;
using System.Diagnostics;
namespace Infoline.Web.PathProviders
{

  
  
    [Export(typeof(IService))]
    [ExportMetadata("ServiceType", typeof(IAssemblyResourceProvider))]
    public class AssemblyResourceProvider : System.Web.Hosting.VirtualPathProvider, IAssemblyResourceProvider
    {
        Dictionary<string, ResourceAssembly> paths = new Dictionary<string, ResourceAssembly>();

        public string GetPath(Assembly asm)
        {
            var ret = paths.Values.FirstOrDefault(a => a.Assembly == asm);
            if (ret == null)
                throw new Exception("Add [assembly: Mira.Web.ResorcePath] Attribute to Assembly " + asm.ToString());
            return string.Concat(ret.Attribute.Path, "/");
        }
        public string GetPath(Type type, string postfix)
        {
            var asm = type.Assembly;
            var ret = paths.Values.FirstOrDefault(a => a.Assembly == asm);
            if (ret != null)
            {
                var fn = type.FullName;
                var ix = fn.IndexOf(".Web.") + 4;
                if (ix > 4)
                    return string.Concat("/", ret.Attribute.Path,
                        fn.Substring(ix, fn.Length > postfix.Length && fn.EndsWith(postfix) ? fn.Length - postfix.Length - ix : fn.Length - ix).Replace(".", "/"));

            }
            return null;
        }
        public Type GetType(string url, string postfix)
        {
            var path = url;

            var root = GetRoot(ref path);
            if (root != null)
            {
                ResourceAssembly res = null;
                if (paths.TryGetValue(root.ToLower(), out res))
                {
                    var tname = string.Format("{0}.{1}.{2}{3}",
                     res.Assembly.FullName.Split(',').FirstOrDefault(),
                     res.Attribute.RootPath,
                     path.Replace("/", ".").Replace(".aspx", ""), postfix);
                    return res.Assembly.GetType(tname, false, true);
                }
            }
            return null;
        }

        public AssemblyResourceProvider()
        {
            paths = AppDomain.CurrentDomain.GetAssemblies().Where(a => !a.GlobalAssemblyCache)
                 .Select(a => new ResourceAssembly { Assembly = a, Attribute = a.GetCustomAttributes(typeof(ResorcePathAttribute), true).OfType<ResorcePathAttribute>().FirstOrDefault() })
                 .Where(a => a.Attribute != null).ToDictionary(a => a.Attribute.Path.ToLower(), a => a);
            if (System.Web.Hosting.HostingEnvironment.IsHosted)
            {
                System.Web.Hosting.HostingEnvironment.RegisterVirtualPathProvider(this);
            }

        }
        static string GetRoot(ref string path)
        {
            string root = null;

            int ix = path.IndexOf('/'), start = 0;

            if (ix == 0 && path.Length > 0)
            {
                start = 1;
                ix = path.IndexOf('/', 1);
            }
            if (ix != -1 && path.Length > ix + 2)
            {
                root = path.Substring(start, ix - start).ToLower();
                path = path.Substring(ix + 1);
            }
            return root;

        }
        Resource ConvertPath(string path)
        {
            if (path.StartsWith("~"))
                path = path.Substring(1);

            string root = GetRoot(ref path);
            if (root != null)
            {
                ResourceAssembly asm;
                var res = new Resource();
                if (root == "app_resource")
                {
                    string[] parts = path.Split('/');
                    res.ResourceName = parts[1];
                    res.Asm = System.Reflection.Assembly.LoadFile(Path.Combine(HttpRuntime.BinDirectory, parts[0]));
                }
                else
                    if (paths.TryGetValue(root, out asm))
                    {

                        res.Asm = asm.Assembly;
                        res.ResourceName =
                        string.Format("{0}.{2}.{1}", asm.Assembly.FullName.Split(',').FirstOrDefault(),
                           path.Replace('/', '.')
                           , asm.Attribute.RootPath);
                    }
                if (res.Asm != null && res.ResourceName != null)
                {
                    res.ResourceName = res.Asm.GetManifestResourceNames().Where(a => a.ToLower() == res.ResourceName.ToLower()).FirstOrDefault();
                    if (res.ResourceName != null)
                        return res;

                }

            }

            return null;

        }
        private bool IsAppResourcePath(string virtualPath)
        {
            String checkPath =
               VirtualPathUtility.ToAppRelative(virtualPath);
            return checkPath.StartsWith("~/App_Resource/", StringComparison.InvariantCultureIgnoreCase);
        }

        public override bool FileExists(string virtualPath)
        {
            return base.FileExists(virtualPath) || ConvertPath(virtualPath) != null;

        }
        public override VirtualFile GetFile(string virtualPath)
        {
            if (base.FileExists(virtualPath))
                return base.GetFile(virtualPath);
            var res = ConvertPath(virtualPath);
            if (res != null)
            {
                return new AssemblyResourceVirtualFile(virtualPath, res);
            }
            else
                return base.GetFile(virtualPath);
        }
        public override System.Web.Caching.CacheDependency GetCacheDependency(string virtualPath, System.Collections.IEnumerable virtualPathDependencies, DateTime utcStart)
        {
            if (ConvertPath(virtualPath) != null)
                return new PermenantDep();
            else
                return base.GetCacheDependency(virtualPath, virtualPathDependencies, utcStart);
        }
        public override string GetCacheKey(string virtualPath)
        {
            return base.GetCacheKey(virtualPath);
        }
        public override string GetFileHash(string virtualPath, System.Collections.IEnumerable virtualPathDependencies)
        {
            return base.GetFileHash(virtualPath, virtualPathDependencies);
        }
        class PermenantDep : CacheDependency
        {
            public PermenantDep()
            {

            }
        }
    }
    class Resource
    {
        public Assembly Asm { get; set; }
        public string ResourceName { get; set; }
    }
    class AssemblyResourceVirtualFile : VirtualFile
    {
        Resource res;
        public AssemblyResourceVirtualFile(string virtualPath, Resource res)
            : base(virtualPath)
        {
            this.res = res;
        }


        public override System.IO.Stream Open()
        {
            return res.Asm.GetManifestResourceStream(res.ResourceName);

        }
    }
    class ResourceAssembly
    {
        public Assembly Assembly { get; set; }
        public ResorcePathAttribute Attribute { get; set; }
    }

}
