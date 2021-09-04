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
        /// SH_FilesRole tablosundan, id si gönderilen kaydın bilgilerini döndüren fonksiyondur.
        /// </summary>
        /// <param name="id">SH_FilesRole tablo id'si verilir.</param>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
        /// <returns>Filtre Sonucu SH_FilesRole Objesini geri döndürür.</returns>
        public SH_FilesRole[] GetSH_FilesRoleByRoleId(Guid id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<SH_FilesRole>().Where(a => a.roleid == id).Execute().ToArray();
            }
        }


        public ResultStatus DeleteSH_FilesRoleAll(DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.ExecuteNonQuery("Delete from dbo.SH_FilesRole");
            }
        }


    }
}
