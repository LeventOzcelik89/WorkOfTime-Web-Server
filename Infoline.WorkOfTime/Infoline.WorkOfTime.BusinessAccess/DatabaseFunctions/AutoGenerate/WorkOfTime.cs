using Infoline.Framework.Database;
using System.Data.Common;

namespace Infoline.WorkOfTime.BusinessAccess
{
    public partial class WorkOfTimeDatabase
    {
        public string ConnectionString { get; private set; }
        public DatabaseType DatabaseType { get; private set; }

        public WorkOfTimeDatabase()
        {
            DatabaseType = (DatabaseType)TenantConfig.Tenant.DBType;
            ConnectionString = TenantConfig.Tenant.GetConnectionString();
            ConnectionString = ConnectionString.Replace("10.100.0.223", "46.221.52.221");
        }

        public WorkOfTimeDatabase(string conn)
        {
            this.ConnectionString = conn;
        }

        public WorkOfTimeDatabase(string conn, DatabaseType type)
        {
            this.DatabaseType = type;
            this.ConnectionString = conn;
            ConnectionString = ConnectionString.Replace("10.100.0.223", "46.221.52.221");
        }

        public InfolineDatabase GetDB(DbTransaction tran = null)
        {
            return new InfolineDatabase(ConnectionString, DatabaseType, tran);
        }

        public DbTransaction BeginTransaction()
        {
            return new InfolineDatabase(ConnectionString, DatabaseType).BeginTransection();
        }
    }

}
