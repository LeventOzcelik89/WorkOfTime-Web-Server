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
using System.ComponentModel;

namespace Infoline.WorkOfTime.BusinessAccess
{

    [EnumInfo(typeof(HDM_TicketMessage), "type")]
    public enum EnumHDM_TicketMessageType
    {
        [Description("Mesaj")]
        Reply = 0,
        [Description("İleti")]
        Forward = 1,
        [Description("Not")]
        Note = 2,
    }

    partial class WorkOfTimeDatabase
    {
        public HDM_TicketMessage[] GetHDM_TicketMessageByTicketId(Guid ticketId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<HDM_TicketMessage>().Where(a => a.ticketId == ticketId).Execute().ToArray();
            }
        }
    }
}
