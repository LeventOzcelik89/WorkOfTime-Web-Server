using Infoline.WorkOfTime.BusinessData;
using System.ComponentModel;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Infoline.Framework.Database;
using System.Data.SqlClient;
using System.Linq.Expressions;
using System.Data.Common;


namespace Infoline.WorkOfTime.BusinessAccess
{
    [EnumInfo(typeof(UT_Location), "type")]
    public enum EnumUT_LocationType
    {
        [Description("Ülke")]
        Ulke = 0,
        [Description("Şehir")]
        Sehir = 1,
        [Description("İlçe")]
        İlce = 2,
        [Description("Köy/Mahalle")]
        KoyMahalle = 3
    }

    public class UT_LocationCard
    {
        public int? ToplamUlkeSayisi { get; set; }

        public int? ToplamSehirSayisi { get; set; }

        public int? ToplamIlceSayisi { get; set; }

        public int? ToplamMahalleSayisi { get; set; }


    }

    partial class WorkOfTimeDatabase
    {

        public UT_LocationCard GetUT_LocationCard()
        {

            using (var db = GetDB())
            {
                return db.ExecuteReader<UT_LocationCard>("Select(select COUNT(*) from UT_Location WITH (NOLOCK) where type = 0) as ToplamUlkeSayisi,(select COUNT(*) from UT_Location WITH (NOLOCK) where type = 1) as ToplamSehirSayisi, (select COUNT(*) from UT_Location WITH (NOLOCK) where type = 2) as ToplamIlceSayisi").FirstOrDefault();
            }
        }
        public UT_Location[] GetUT_LocationByTown(string town, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<UT_Location>().Where(a => a.type == (int)EnumUT_LocationType.İlce && a.name.ToLower() == town.ToLower()).Execute().ToArray();
            }
        }

        public UT_Location GetUT_LocationByCity(string city, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<UT_Location>().Where(a => a.type == (int)EnumUT_LocationType.Sehir && a.name.ToLower() == city.ToLower()).Execute().FirstOrDefault();
            }
        }

        public UT_Location[] GetUT_LocationCityAndTownInTR(DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<UT_Location>().Where(a => (a.type == (int)EnumUT_LocationType.Sehir || a.type == (int)EnumUT_LocationType.İlce) && a.code == "TR").Execute().ToArray();
            }
        }

        public UT_Location[] GetUT_LocationByPid(Guid pid, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<UT_Location>().Where(a => a.pid == pid).Execute().ToArray();
            }
        }
    }
}
