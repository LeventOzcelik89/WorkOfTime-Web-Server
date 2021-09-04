using Infoline.Framework.Database;
using Infoline.ProjectManagement.BusinessData;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Infoline.ProjectManagement.BusinessAccess.Models
{


    public class VMCMP_Company : VWCMP_Company
    {
        public Guid[] sectorIds { get; set; }
    }

   

    public class VWCMP_CompanyModel
    {
        public WorkOfTimeDatabase _db = new WorkOfTimeDatabase();
        private CMP_Company _company { get; set; }
        private CMP_Sector[] _sectors { get; set; }
        private CMP_Storage _storage { get; set; }

        public VWCMP_CompanyModel()
        {

        }

        public VWCMP_CompanyModel(Guid companyId)
        {
            _storage = _db.GetCMP_StroageItemByCompanyId(companyId);
            _sectors = _db.GetCMP_SectorByCompanyId(companyId);
        }

        public ResultStatus Insert(VMCMP_Company model)
        {
            if (string.IsNullOrEmpty(model.code))
            {
                model.code = BusinessExtensions.B_GetIdCode("CMP");
            }
            var trans = _db.BeginTransaction();
            var dbresult = _db.InsertCMP_Company(new CMP_Company().B_EntityDataCopyForMaterial(model), trans);

            dbresult &= _db.InsertCMP_Storage(new CMP_Storage
            {
                id = Guid.NewGuid(),
                created = DateTime.Now,
                createdby = model.createdby,
                code = BusinessExtensions.B_GetIdCode("STO"),
                name = "Ana Depo",
                location = model.location,
                locationId = model.openAddressLocationId,
                companyId = model.id,
                address = model.openAddress
            }, trans);

            if (model.sectorIds != null)
            {
                dbresult &= _db.BulkInsertCMP_Sector(model.sectorIds.Select(a => new CMP_Sector
                {
                    createdby = model.createdby,
                    companyId = model.id,
                    sectorId = a,
                }), trans);
            }

            if (!dbresult.result)
            {
                trans.Rollback();
                return new ResultStatus
                {
                    result = false,
                    message = "İşletme kaydetme işlemi başarısız oldu."
                };
            }

            trans.Commit();

            return new ResultStatus
            {
                result = true,
                message = "İşletme kaydetme işlemi başarılı bir şekilde gerçekleştirildi."
            };

        }


        public ResultStatus Update(VMCMP_Company model)
        {
            var dbresult = new ResultStatus();
            dbresult.result = true;
            model.changed = DateTime.Now;
            var trans = _db.BeginTransaction();
          
            dbresult &= _db.BulkDeleteCMP_Sector(_sectors, trans);
            if (model.sectorIds != null)
            {
                dbresult &= _db.BulkInsertCMP_Sector(model.sectorIds.Select(a => new CMP_Sector
                {
                    createdby = model.createdby,
                    companyId = model.id,
                    sectorId = a,
                }), trans);
            }
            
            dbresult &= _db.UpdateCMP_Company(new CMP_Company().B_EntityDataCopyForMaterial(model), true, trans);

            if (_storage == null)
            {
                _storage = new CMP_Storage
                {
                    id = Guid.NewGuid(),
                    created = DateTime.Now,
                    createdby = model.changedby,
                    code = BusinessExtensions.B_GetIdCode("STO"),
                    name = "Ana Depo",
                    location = model.location,
                    locationId = model.openAddressLocationId,
                    companyId = model.id,
                    address = model.openAddress
                };

                dbresult &= _db.InsertCMP_Storage(_storage, trans);
            }

            if (!_storage.locationId.HasValue)
            {
                _storage.changed = DateTime.Now;
                _storage.changedby = model.changedby;
                _storage.locationId = model.openAddressLocationId;
                _storage.address = model.openAddress;
                _storage.location = model.location;

                dbresult &= _db.UpdateCMP_Storage(_storage, true, trans);
            }
            
            if (!dbresult.result)
            {
                trans.Rollback();
                return new ResultStatus
                {
                    result = false,
                    message = "İşletme güncelleme işlemi başarısız oldu."
                };
            }

            trans.Commit();
            return new ResultStatus
            {
                result = true,
                message = "İşletme güncelleme işlemi başarılı şekilde gerçekleştirildi."
            };
        }



        public VMCMP_Company Get()
        {
            return new VMCMP_Company().B_EntityDataCopyForMaterial(_company, true);
        }
    }
}
