using Infoline.WorkOfTime.BusinessData;
using System;
using System.ComponentModel;
using System.Data.Common;
using System.Linq;

namespace Infoline.WorkOfTime.BusinessAccess
{

    [EnumInfo(typeof(CSM_Student), "grade")]
    public enum EnumCSM_StudentGrade
    {
        [Description("9. Sınıf"), KeyValue("sort", "0")]
        Sinif9 = 0,
        [Description("10. Sınıf"), KeyValue("sort", "1")]
        Sinif10 = 1,
        [Description("11. Sınıf"), KeyValue("sort", "2")]
        Sinif11 = 2,
        [Description("12. Sınıf"), KeyValue("sort", "3")]
        Sinif12 = 3,
        [Description("Mezun"), KeyValue("sort", "4")]
        Mezun = 4,
        [Description("Bilinmiyor"), KeyValue("sort", "6")]
        Bilinmiyor = 5,
        [Description("Üniversite"), KeyValue("sort", "5")]
        Universite = 6,
    }


    [EnumInfo(typeof(CSM_Student), "source")]
    public enum EnumCSM_StudentSource
    {
        [Description("Fuar")]
        Fuar = 0,
        [Description("Web Site")]
        WebSite = 1,
        [Description("Arama")]
        Arama = 2,
        [Description("Diğer")]
        Diger = 3,
    }

    [EnumInfo(typeof(CSM_Student), "isAllowContact")]
    public enum EnumCSM_StudentIsAllowContact
    {
        [Description("Hayır")]
        Hayir = 0,
        [Description("Evet")]
        Evet = 1,
    }

    partial class WorkOfTimeDatabase
    {
        public CSM_Student GetCSM_StudentByPhone(string phone, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<CSM_Student>().Where(a => a.phone == phone).Execute().FirstOrDefault();
            }
        }
        public CSM_Student GetCSM_StudentByEmail(string email, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<CSM_Student>().Where(a => a.email == email).Execute().FirstOrDefault();
            }
        }

    }
}
