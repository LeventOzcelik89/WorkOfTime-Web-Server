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

    public class UT_SectorCard
    {
        public int? ToplamSektorSayisi { get; set; }

        public string IsletmelerdeEnCokKullanilanSektor { get; set; }

        public string EnCokAltSektoruBulunanSektor { get; set; }

        public int? EnCokAltSektoruBulunanSektorunSayisi{ get; set; }
    }



    partial class WorkOfTimeDatabase
    {

        public UT_SectorCard GetUT_SectorCard() {

            using (var db = GetDB())
            {
              return db.ExecuteReader<UT_SectorCard>("Select(select Count(*) from UT_Sector) AS ToplamSektorSayisi,ISNULL((select top 1 dbo.fn_CMP_CompanyCodeNameById(pid) from CMP_Company with (nolock) where pid is not null group by pid  order by COUNT(1) DESC), 'Veri Bulunamadı') as IsletmelerdeEnCokKullanilanSektor,ISNULL((select name from UT_Sector Where id = (select top 1 pid from UT_Sector with (nolock) group by pid  order by COUNT(*) DESC)), 'Veri Bulunamadı') as EnCokAltSektoruBulunanSektor,(select top 1 count(*)  from UT_Sector with (nolock) group by pid  order by COUNT(*) DESC) as EnCokAltSektoruBulunanSektorunSayisi").FirstOrDefault();
            }
        }

        public UT_Sector[] GetUT_SectorByPid(Guid pid, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<UT_Sector>().Where(a => a.pid == pid).Execute().ToArray();
            }
        }

        public UT_Sector[] GetUT_SectorByParentSectors(DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<UT_Sector>().Where(a => a.pid == null).Execute().ToArray();
            }
        }

        public UT_Sector GetUT_SectorByName(string Name, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<UT_Sector>().Where(a => a.name == Name).Execute().FirstOrDefault();
            }
        }


    }
}
