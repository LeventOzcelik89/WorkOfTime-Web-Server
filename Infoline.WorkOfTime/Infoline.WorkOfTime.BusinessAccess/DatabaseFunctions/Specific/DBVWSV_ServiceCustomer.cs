using Infoline.WorkOfTime.BusinessData;
using System;
using System.ComponentModel;
using System.Data.Common;
using System.Linq;
namespace Infoline.WorkOfTime.BusinessAccess
{
    partial class WorkOfTimeDatabase
    {
        public VWSV_Customer GetVWSV_CustomerByPhoneNumber(string phoneNumber, DbTransaction transaction = null)
        {
            using (var db = GetDB(transaction))
            {
                return db.Table<VWSV_Customer>().Where(x => x.phoneNumber == phoneNumber).Execute().FirstOrDefault();
            }
        }
        
    }
}
