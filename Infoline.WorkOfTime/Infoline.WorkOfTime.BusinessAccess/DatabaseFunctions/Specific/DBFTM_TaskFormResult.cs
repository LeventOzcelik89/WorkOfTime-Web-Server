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
        /// FTM_TaskOperation taskId filtresi sonucunda gelen kayıtları çeken fonksiyondur. Sayfa giridnde kullanılmaktadır.
        /// </summary>
        /// <param name="taskId">TaskId gönderilir.</param>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
        /// <returns>Filtre Sonucu FTM_TaskOperation dizi objesini geri döndürür.</returns>
        public FTM_TaskFormResult[] GetFTM_TaskFormResultByTaskId(Guid taskId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<FTM_TaskFormResult>().Where(a => a.taskId == taskId).Execute().ToArray();
            }
        }

        public FTM_TaskUser[] GetFTM_TaskUserByTaskId(Guid taskId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<FTM_TaskUser>().Where(a => a.taskId == taskId).Execute().ToArray();
            }
        }
 
        public FTM_TaskUserHelper[] GetFTM_TaskUserHelperByTaskId(Guid taskId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<FTM_TaskUserHelper>().Where(a => a.taskId == taskId).Execute().ToArray();
            }
        }
    }
}
