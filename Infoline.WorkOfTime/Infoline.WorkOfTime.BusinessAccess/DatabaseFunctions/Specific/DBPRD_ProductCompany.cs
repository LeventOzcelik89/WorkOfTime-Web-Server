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

    [EnumInfo(typeof(PRD_ProductCompany), "type")]
    public enum EnumPRD_ProductCompanyType
    {
        [Description("Tedarikçi"), Generic("icon", "fa fa-bookmark", "color", "#1c84c6")]
        supplier = 0,
        [Description("Garanti"), Generic("icon", "fa fa-bookmark", "color", "#F8AC59")]
        warranty = 1,
    }


    partial class WorkOfTimeDatabase
    {

        public PRD_ProductCompany[] GetPRD_ProductCompanyByProductId(Guid id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<PRD_ProductCompany>().Where(a => a.productId == id).Execute().ToArray();
            }
        }

        public PRD_ProductCompany[] GetPRD_ProductCompanyByCompanyId(Guid id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<PRD_ProductCompany>().Where(a => a.companyId == id).Execute().ToArray();
            }
        }

    }
}
