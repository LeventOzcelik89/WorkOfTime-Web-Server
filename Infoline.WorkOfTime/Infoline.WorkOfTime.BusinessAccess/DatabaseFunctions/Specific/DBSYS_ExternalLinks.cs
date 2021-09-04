using System.ComponentModel;
using Infoline.WorkOfTime.BusinessData;
using System.Data.Common;
using System;
using System.Linq;
using Infoline.Framework.Database;

namespace Infoline.WorkOfTime.BusinessAccess
{
    partial class WorkOfTimeDatabase
    {

        public SYS_ExternalLinks GetSYS_ExternalLinksByUrl(string url ,DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<SYS_ExternalLinks>().Where(x => x.Url == url).OrderByDesc(a => a.created).Execute().FirstOrDefault();
            }
        }


        public SYS_ExternalLinks GetSYS_ExternalLinksByUrlAndId(Guid id, string url, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<SYS_ExternalLinks>().Where(x => x.Url == url && x.id != id).OrderByDesc(a => a.created).Execute().FirstOrDefault();
            }
        }
    }
}
