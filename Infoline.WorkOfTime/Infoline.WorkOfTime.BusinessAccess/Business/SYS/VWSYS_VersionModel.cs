using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessData;
using Infoline.WorkOfTime.BusinessData.Specific;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Infoline.WorkOfTime.BusinessAccess
{
    public class VWSYS_VersionModel : SYS_Version
    {
        private WorkOfTimeDatabase db { get; set; }
        private DbTransaction trans { get; set; }
        public List<SYS_Version> _versions { get; set; } = new List<SYS_Version>();

        public  List<SYS_Version> GetTenantsForVersionManagement()
        {
            if (_versions.Count() == 0)
            {
                var remoteCon = ConfigurationManager.AppSettings["RemoteConnection"];
                var connection = new Infoline.Framework.Helper.CryptographyHelper().Decrypt(remoteCon);
                using (var db = new InfolineDatabase(connection, DatabaseType.Mssql))
                {
                    _versions = db.ExecuteReader<SYS_Version>("select top 5 * from SYS_Version order by isActive desc,created desc").ToList();
                }
            }
            return _versions;
        }

        public VWSYS_VersionModel Load()
        {
            GetTenantsForVersionManagement();
            this._versions = _versions;
            if (this._versions != null)
            {
                this.B_EntityDataCopyForMaterial(this._versions, true);
            }
            return this;
        }
    }
    public partial class SYS_Version : InfolineTable
    {
        public string versionNumber { get; set; }
        public string versionChange { get; set; }
        public bool? isActive { get; set; }
    }
}
