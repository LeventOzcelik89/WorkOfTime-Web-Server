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

    public class UT_CurrencyCard
    {
        public int? ToplamParaBirimiSayisi { get; set; }

        public string SonEklenenParaBirimi { get; set; }

        public string SonEklenenDovizKodu { get; set; }

    }



    partial class WorkOfTimeDatabase
    {

        public UT_CurrencyCard GetUT_CurrencyCard() {

            using (var db = GetDB())
            {
              return db.ExecuteReader<UT_CurrencyCard>("select ISNULL((select count(*) from UT_Currency with (nolock)), 0) as ToplamParaBirimiSayisi, (select top 1 name from UT_Currency with (nolock)order by created desc)  as SonEklenenParaBirimi, ISNULL((select top 1 code from UT_Currency with (nolock)order by created desc) ,0) as SonEklenenDovizKodu").FirstOrDefault();
            }
        }

        public UT_Currency GetUT_CurrencyIdByCode(String Code, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<UT_Currency>().Where(a => a.code == Code).Execute().FirstOrDefault();
            }
        }


    }
}
