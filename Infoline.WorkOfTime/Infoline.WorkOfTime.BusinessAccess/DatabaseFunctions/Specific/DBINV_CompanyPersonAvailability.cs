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


    [EnumInfo(typeof(INV_CompanyPersonAvailability), "type")]
    public enum EnumINV_CompanyPersonAvailabilityType
    {
        [Description("Proje")]
        Proje = 0,
        [Description("Bakım")]
        Bakim = 1,
    }



    partial class WorkOfTimeDatabase
    {
        /// <summary>
        /// Gönderilen proje id için personel kullanım yüzdelerini verir.
        /// </summary>
        /// <param name="IdProject"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public INV_CompanyPersonAvailability[] GETINV_CompanyPersonAvailabilityByProjectId(Guid IdProject, DbTransaction tran = null)
        {
            using (var dB = GetDB(tran))
            {
                return dB.Table<INV_CompanyPersonAvailability>().Where(x => x.IdProject == IdProject).Execute().ToArray();
            }
        }
        public INV_CompanyPersonAvailability[] GETINV_CompanyPersonAvailabilityByProjectId(Guid IdProject, int type, DbTransaction tran = null)
        {
            using (var dB = GetDB(tran))
            {
                return dB.Table<INV_CompanyPersonAvailability>().Where(x => x.IdProject == IdProject && x.type == type).Execute().ToArray();
            }
        }

    }
}
