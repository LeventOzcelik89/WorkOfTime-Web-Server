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
    [EnumInfo(typeof(EnumUT_SchoolType), "type")]
    public enum EnumUT_SchoolType
    {
        [Description("İlk Öğretim (İlk Okul)")]
        ilkogretim = 0,
        [Description("Orta Öğretim  (Lise)")]
        ortaogretim = 1,
        [Description("Yüksek Öğretim (Üniversite)")]
        yuksekogretim = 2,
    }


    partial class WorkOfTimeDatabase
    {
    }
}
