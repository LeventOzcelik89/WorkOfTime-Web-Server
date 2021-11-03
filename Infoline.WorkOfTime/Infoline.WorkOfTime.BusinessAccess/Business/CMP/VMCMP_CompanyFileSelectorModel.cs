using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
namespace Infoline.WorkOfTime.BusinessAccess.Business
{
    public class VMCMP_CompanyFileSelectorModel : VWCMP_CompanyFileSelector
    {
        private WorkOfTimeDatabase db { get; set; }
        private DbTransaction trans { get; set; }
        public List<string> FileNames { get; set; }
        public VMCMP_CompanyFileSelectorModel Load()
        {
            this.db = this.db ?? new WorkOfTimeDatabase();
            var data = db.GetCMP_CompanyFileSelectorById(this.id);
            if (data != null)
            {
                this.B_EntityDataCopyForMaterial(data, true);
            }
            return this;
        }
        private ResultStatus Validator()
        {
            db = db ?? new WorkOfTimeDatabase();
            var res = new ResultStatus { result = true };
            return res;
        }
        public ResultStatus Insert(Guid? userId)
        {
            db = db ?? new WorkOfTimeDatabase();
            var dbresult = new ResultStatus { result = true };
            if (FileNames.Count > 0)
            {
                foreach (var item in FileNames)
                {
                    dbresult &= db.InsertCMP_CompanyFileSelector(new CMP_CompanyFileSelector
                    {
                        created = DateTime.Now,
                        createdby = userId,
                        customerId = customerId,
                        fileGroupModule = fileGroupModule,
                        fileGroupName = item
                    }, this.trans);
                }
            }
            if (!dbresult.result)
            {
                Log.Error(dbresult.message);
                return new ResultStatus
                {
                    result = false,
                    message = "Çalışan Durum Değişiklikleri oluşturma işlemi başarısız oldu."
                };
            }
            else
            {
                return new ResultStatus
                {
                    result = true,
                    message = "Çalışan Durum Değişiklikleri oluşturma işlemi başarılı şekilde gerçekleştirildi."
                };
            }
        }
        public ResultStatus Delete(DbTransaction transaction = null)
        {
            db = db ?? new WorkOfTimeDatabase();
            trans = transaction ?? db.BeginTransaction();
            var dbresult = db.DeleteCMP_CompanyFileSelector(new CMP_CompanyFileSelector { id = this.id }, trans);
            if (!dbresult.result)
            {
                if (transaction == null) trans.Rollback();
                return new ResultStatus
                {
                    result = false,
                    message = "Çalışan Durum Değişiklikleri silme işlemi başarısız oldu."
                };
            }
            else
            {
                if (transaction == null) trans.Commit();
                return new ResultStatus
                {
                    result = true,
                    message = "Çalışan Durum Değişiklikleri silme işlemi başarılı şekilde gerçekleştirildi."
                };
            }
        }
        public ResultStatus GetAllDataTableFromSysFile()
        {
            db = db ?? new WorkOfTimeDatabase();
            var data = LocalFileProvider._dataTableFileGroup.Keys.Select(x=>new { DataTable = x}).ToList();
            return new ResultStatus { result = true, objects = data };
        }
     
    }


}
