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
    [EnumInfo(typeof(CMP_CompanyCars), "isHire")]
    public enum EnumCMP_CompanyCarsisHire
    {
        [Description("Hayır")]
        Hayir = 0,
        [Description("Evet")]
        Evet = 1,
    }

    [EnumInfo(typeof(CMP_CompanyCars), "fuelType")]
    public enum EnumCMP_CompanyCarsfuelType
    {
        [Description("Benzin")]
        Benzin = 0,
        [Description("Dizel")]
        Dizel = 1,
        [Description("LPG")]
        LPG = 2,
        [Description("Elektrik(Hibrit)")]
        Elektrik = 3,
    }
    partial class WorkOfTimeDatabase
    {
        public CMP_CompanyCars GetCMP_CompanyCarsByPlate(string plate)
        {
            using (var db = GetDB())
            {
                return db.Table<CMP_CompanyCars>().Where(a => a.plate == plate).Execute().FirstOrDefault();
            }
        }

        public CMP_CompanyCars GetCMP_CompanyCarsPlateByUpdate(Guid id, string plate)
        {
            using (var db = GetDB())
            {
                return db.Table<CMP_CompanyCars>().Where(a => a.id != id && a.plate == plate).Execute().FirstOrDefault();
            }
        }
    }
}
