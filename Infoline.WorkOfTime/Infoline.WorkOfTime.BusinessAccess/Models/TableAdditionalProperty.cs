using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Infoline.WorkOfTime.BusinessAccess
{
    public class TableAdditionalProperty
    {

    }

    public class TableAdditionalPropertySave
    {
        public WorkOfTimeDatabase _db = new WorkOfTimeDatabase();
        public HttpRequestBase Request { get; set; }
        public Guid DataId { get; set; }
        public string DataTable { get; set; }
        public TableAdditionalPropertySave(HttpRequestBase request, Guid DataId, string DataTable)
        {
            Request = request;
            this.DataId = DataId;
            this.DataTable = DataTable;
        }

        public ResultStatus SaveAs(Guid? userId)
        {
            try
            {
                if (!TenantConfig.Tenant.Config.CustomProperty.ContainsKey(this.DataTable))
                {
                    return new ResultStatus { result = true };
                }

                var errorList = new List<string>();

                var keys = TenantConfig.Tenant.Config.CustomProperty[DataTable];
                var list = new List<SYS_TableAdditionalProperty>();
                foreach (var item in keys)
                {
                    var value = Request[item];
                    list.Add(new SYS_TableAdditionalProperty
                    {
                        id = Guid.NewGuid(),
                        dataId = DataId,
                        dataTable = DataTable,
                        propertyKey = item,
                        propertyValue = value,
                        created = DateTime.Now,
                        createdby = userId
                    });
                }

                var deleteList = _db.GetSYS_TableAdditionalPropertyByDataIdAndDataTable(DataId, DataTable);

                var trans = _db.BeginTransaction();
                var rs = _db.BulkDeleteSYS_TableAdditionalProperty(deleteList, trans);
                rs &= _db.BulkInsertSYS_TableAdditionalProperty(list.ToArray(), trans);

                if (rs.result)
                {
                    trans.Commit();
                    return new ResultStatus() { result = true, message = "Başarılı" };
                }
                else
                {
                    trans.Rollback();
                    return new ResultStatus() { result = true, message = "Başarısız " };
                }
            }
            catch (Exception ex)
            {
                return new ResultStatus() { result = true, message = "Başarısız : " + ex.Message };
            }
        }
    }

}