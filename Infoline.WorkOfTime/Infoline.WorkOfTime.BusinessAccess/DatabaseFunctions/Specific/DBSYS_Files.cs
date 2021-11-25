using Infoline.WorkOfTime.BusinessData;
using System.Data.Common;
using System;
using System.Linq;
using Infoline.Framework.Database;

namespace Infoline.WorkOfTime.BusinessAccess
{
    partial class WorkOfTimeDatabase
    {

        public SYS_Files GetSYS_FilesByDataId(Guid Id ,DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<SYS_Files>().Where(x => x.DataId == Id).OrderByDesc(a => a.created).Execute().FirstOrDefault();
            }
        }


        public SYS_Files[] GetSYS_FilesInDataId(Guid[] Ids, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<SYS_Files>().Where(x => x.DataId.In(Ids)).OrderByDesc(a => a.created).Execute().ToArray();
            }
        }

        public SYS_Files[] GetSYS_FilesByDataIdAll(Guid Id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<SYS_Files>().Where(x => x.DataId == Id).OrderByDesc(a => a.created).Execute().ToArray();
            }
        }

        public SYS_Files[] GetSYS_FilesByDataIdAndDataTableAll(Guid Id,string dataTable, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<SYS_Files>().Where(x => x.DataId == Id && x.DataTable == dataTable).OrderBy(a => a.FileGroup).Execute().ToArray();
            }
        }

        public SYS_Files[] GetSYS_FilesByDataIdArray(Guid Id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<SYS_Files>().Where(x => x.DataId == Id).OrderByDesc(a => a.created).Execute().ToArray();
            }
        }
        public SYS_Files[] GetSYS_FilesByDataTableAndFileGroup(string dataTable,string fileGroup, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<SYS_Files>().Where(x => x.DataTable == dataTable && x.FileGroup == fileGroup).OrderByDesc(a => a.created).Execute().ToArray();
            }
        }


        public SYS_Files GetSysFilesFilePathByDataTableAndFileGroupAndDataId(string DataTable, string FileGroup, Guid DataId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<SYS_Files>().Where(x => x.DataTable == DataTable && x.FileGroup == FileGroup && x.DataId == DataId)
                    .Select(a => new { 
                        FilePath = a.FilePath
                    })
                    .Execute<SYS_Files>().FirstOrDefault();
            }
        }
        public object[] GetSysFilesGroupedByDataTable(DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<SYS_Files>().Execute().GroupBy(x => x.DataTable).Select(x => new { value = x.Key }).ToArray();
            }
        }
        public object[] GetSYS_FilesByFileGroup(DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<SYS_Files>().Execute().GroupBy(x=>x.FileGroup).Select(x=>new {file=x.Key,value=x.GroupBy(a=>a.DataTable).Select(b=>b.Key).FirstOrDefault() }).ToArray();
            }
        }
    }
}
