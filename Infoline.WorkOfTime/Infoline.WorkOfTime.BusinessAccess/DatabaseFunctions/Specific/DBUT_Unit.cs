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

namespace Infoline.WorkOfTime.BusinessAccess
{

    public class UT_UnitCard
    {
        public int? ToplamBirimTanimiSayisi { get; set; }

        public string SonEklenenBirimIsmi { get; set; }

        public DateTime SonEklenenKaydinTarihi { get; set; }

    }


    partial class WorkOfTimeDatabase
    {

        public UT_UnitCard GetUT_UnitCard()
        {

            using (var db = GetDB())
            {
                return db.ExecuteReader<UT_UnitCard>("Select(select Count(*) from UT_Unit) as ToplamBirimTanimiSayisi,(SELECT top 1 name FROM UT_Unit WITH(NOLOCK) order by created desc) as SonEklenenBirimIsmi,(SELECT top 1 created FROM UT_Unit WITH (NOLOCK)order by created desc) as SonEklenenKaydinTarihi").FirstOrDefault();
            }
        }


        public UT_Unit GetUT_UnitByName(string name)
        {
            using (var db = GetDB())
            {
                return db.Table<UT_Unit>().Where(a => a.name.Contains(name)).Execute().FirstOrDefault();
            }
        }

        public UT_Unit GetUT_UnitByNameByUpdateName(Guid id, string name, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<UT_Unit>().Where(a => a.id != id && a.name == name).OrderBy(a => a.created).Execute().FirstOrDefault();
            }
        }

    }
}
