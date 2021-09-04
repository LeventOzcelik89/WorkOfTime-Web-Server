using Infoline.WorkOfTime.BusinessData;
using System;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using Infoline.Framework.Database;

namespace Infoline.WorkOfTime.BusinessAccess
{





    partial class WorkOfTimeDatabase
    {
        /// <summary>
        /// SYS_Enums tablosundan tüm kayıtları çeken fonksiyondur
        /// </summary>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
        /// <returns>SYS_Enums dizi objesini geri döndürür.</returns>
        public SYS_Enums[] GetSYS_EnumsByWhere(Expression<Func<SYS_Enums, bool>> condition, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<SYS_Enums>().Where(condition).OrderByDesc(a => a.created).Execute().ToArray();
            }
        }


        public ResultStatus AllDeleteSYS_Enums(DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.ExecuteNonQuery("Delete from dbo.SYS_Enums");
            }
        }


    }
}
