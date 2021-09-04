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
    public interface IAssemblyResourceProvider : IService
    {
        Type GetType(string url, string postfix);
        string GetPath(Type type, string postfix);
        string GetPath(Assembly asm);
    }

}
