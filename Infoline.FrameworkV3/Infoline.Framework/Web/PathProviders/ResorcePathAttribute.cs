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
    [AttributeUsage(AttributeTargets.Assembly, Inherited = false, AllowMultiple = false)]
    public sealed class ResorcePathAttribute : Attribute
    {
        readonly string path;

        public ResorcePathAttribute(string path, string rootpath = "Web")
        {
            this.path = path;
            RootPath = rootpath;


        }

        public string Path
        {
            get { return path; }
        }
        public string RootPath
        {
            get;
            private set;
        }

    }
   
   

}
