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
        /// FTM_TaskTemplateInventories tablosundan, id si gönderilen kaydın bilgilerini döndüren fonksiyondur.
        /// </summary>
        /// <param name="id">FTM_TaskTemplateInventories tablo id'si verilir.</param>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
        /// <returns>Filtre Sonucu FTM_TaskTemplateInventories Objesini geri döndürür.</returns>
        public FTM_TaskTemplateInventories[] GetFTM_TaskTemplateInventoriesByTaskTemplateId(Guid taskTemplateId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<FTM_TaskTemplateInventories>().Where(a => a.taskTemplateId == taskTemplateId).Execute().ToArray();
            }
        }

    }
}
