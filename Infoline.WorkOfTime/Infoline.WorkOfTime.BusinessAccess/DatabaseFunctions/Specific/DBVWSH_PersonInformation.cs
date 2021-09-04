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
    partial class WorkOfTimeDatabase
    {
        /// <summary>
        /// VWSH_PersonInformation tablosundan, id si gönderilen kaydın bilgilerini döndüren fonksiyondur.
        /// </summary>
        /// <param name="id">VWSH_PersonInformation tablo id'si verilir.</param>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
        /// <returns>Filtre Sonucu VWSH_PersonInformation Objesini geri döndürür.</returns>
        public VWSH_PersonInformation GetVWSH_PersonInformationByUserId(Guid id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<VWSH_PersonInformation>().Where(a => a.UserId== id).Execute().FirstOrDefault();
            }
        }

        public VWSH_PersonInformation[] GetVWSH_PersonInformationByUserIds(Guid[] ids, DbTransaction tran = null)
        {
            using (var db = GetDB())
            {
                return db.Table<VWSH_PersonInformation>().Where(x => x.UserId.In(ids)).Execute().ToArray();
            }
        }

    }
}
