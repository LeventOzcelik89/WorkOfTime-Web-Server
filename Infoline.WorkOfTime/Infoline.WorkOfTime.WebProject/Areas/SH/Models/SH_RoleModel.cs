using Infoline.Framework.Database;
using Infoline.Framework.Helper;
using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData;
using Infoline.WorkOfTime.WebProject.Areas.INV.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Infoline.WorkOfTime.WebProject.Areas.SH.Models
{

        public class DataSend
        {
            public VWSH_PagesRole[] pagesRole { get; set; }
            public List<TreeViewModel> model { get; set; }
        }
        public class TreeViewModel
        {
            public Guid id { get; set; }
            public string text { get; set; }
            public bool expanded { get; set; }
            public bool selected { get; set; }
            public TreeViewModel[] items { get; set; }
            public bool @checked { get; set; }
            public string spriteCssClass { get; set; }
            public bool? enabled { get; set; }

        }

        public class FileTreeViewModel
        {
            public string id { get; set; }
            public string text { get; set; }
            public bool expanded { get; set; }
            public List<FileTreeViewModel> items { get; set; }
            public string spriteCssClass { get; set; }
            public bool @checked { get; set; }
        }

        public class DataSendFile
        {
            public List<FileTreeViewModel> filesRole { get; set; }
            public List<FileTreeViewModel> model { get; set; }
        }

}