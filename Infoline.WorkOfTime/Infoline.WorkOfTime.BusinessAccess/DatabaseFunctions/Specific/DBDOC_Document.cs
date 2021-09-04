using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infoline.Framework.Database;
using System.Data.SqlClient;
using System.Linq.Expressions;
using System.Data.Common;
using Infoline.WorkOfTime.BusinessData;
using System.ComponentModel;

namespace Infoline.WorkOfTime.BusinessAccess
{
    [EnumInfo(typeof(DOC_Document), "type")]
    public enum EnumDOC_DocumentType
    {
        [Description("Yönetmelik"), Generic("icon", "icon-folder-2", "color", "","EN", "Policy")]
        Document = 0,
        [Description("Prosedür"), Generic("icon", "icon-book-open", "color", "", "EN", "Procedure")]
        Policy = 1,
        [Description("El Kitapları ve Yönergeler"), Generic("icon", "icon-book-1", "color", "", "EN", "Handbooks and Guidelines")]
        HandBooks = 2,
        [Description("Form"), Generic("icon", "icon-wpforms", "color", "", "EN", "Forms")]
        Form = 3,
        [Description("Islak İmzalı Doküman"), Generic("icon", "icon-pencil-alt", "color", "", "EN", "Signed Documents")]
        SignedDocument = 4
    }

    partial class WorkOfTimeDatabase
    {

    }
}
