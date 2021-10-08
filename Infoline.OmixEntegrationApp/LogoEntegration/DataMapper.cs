using Infoline.Framework.Database;
using Infoline.OmixEntegrationApp.LogoService;
using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infoline.OmixEntegrationApp.LogoEntegration
{
    class DataMapper : IDataMapper
    {
        public ResultStatus CompanySave(AdClientFind[] param)
        {
            var db = new WorkOfTimeDatabase();
            var result = new ResultStatus { result = true };
            if (param != null)
            {
                var findCompany = db.GetCMP_Company();
                foreach (var item in param)
                {
                    if (item.CariKodu != null)
                    {

                        if (findCompany != null && findCompany.Where(a => a.code == item.CariKodu).Count() > 0)
                        {
                            result = CompanyUpdate(item);
                        }
                        else
                        {
                            result = CompanyInsert(item);
                        }
                    }
                    else
                    {
                        result.result = false;
                        result.message = "CariKodu Alanı Zorunludur.";
                        return result;
                    }
                }
            }
            else
            {
                result.result = false;
                result.message = "Cari Listesi Boş Dönmektedir.";
                return result;
            }
            return result;
        }
        public ResultStatus CompanyInsert(AdClientFind param)
        {
            var db = new WorkOfTimeDatabase();
            var result = new ResultStatus { result = true };
            var findLocation = db.GetUT_Location().Where(a => a.name == param.Il).FirstOrDefault();
            var insertCompany = new CMP_Company
            {
                id = Guid.NewGuid(),
                created = DateTime.Now,
                openAddress = param.Adres,
                code = param.CariKodu,
                name = param.CariUnvan,
                email = param.Eposta1,
                taxOffice = param.VergiDairesi,
                taxNumber = param.VergiNo,
                commercialTitle = param.CariUnvan,
                openAddressLocationId = findLocation != null ? findLocation.id : (Guid?)null,
            };
            result &= db.InsertCMP_Company(insertCompany);
            return result;
        }
        public ResultStatus CompanyUpdate(AdClientFind param)
        {
            var db = new WorkOfTimeDatabase();
            var result = new ResultStatus { result = true };
            var checkCode = db.GetCMP_CompanyByCode(param.CariKodu);
            var validator = CompanyValidator(param, checkCode);
            if (validator.result)
            {
                checkCode.name = param.CariUnvan;
                checkCode.openAddress = param.Adres;
                checkCode.code = param.CariKodu;
                checkCode.email = param.Eposta1;
                checkCode.taxOffice = param.VergiDairesi;
                checkCode.taxNumber = param.VergiNo;
                checkCode.commercialTitle = param.CariUnvan;
                checkCode.changed = DateTime.Now;
                result &= db.UpdateCMP_Company(checkCode);
            }
            return result;
        }
        public ResultStatus ProductSave(AdItemsFindList[] param)
        {
            var db = new WorkOfTimeDatabase();
            var result = new ResultStatus { result = true };
            if (param != null)
            {
                var findProduct = db.GetPRD_Product();
                foreach (var item in param)
                {
                    if (item.UrunKodu != null)
                    {

                        if (findProduct != null && findProduct.Where(a => a.code == item.UrunKodu).Count() > 0)
                        {
                            result = ProductUpdate(item);
                        }
                        else
                        {
                            result = ProductInsert(item);
                        }
                    }
                    else
                    {
                        result.result = false;
                        result.message = "UrunKodu Alanı Zorunludur.";
                        return result;
                    }
                }
            }
            else
            {
                result.result = false;
                result.message = "Cari Listesi Boş Dönmektedir.";
                return result;
            }
            return result;
        }
        public ResultStatus ProductInsert(AdItemsFindList param)
        {
            throw new NotImplementedException();
        }
        public ResultStatus ProductUpdate(AdItemsFindList param)
        {
            throw new NotImplementedException();
        }
        public ResultStatus StorageSave(AdShipFindList[] param)
        {
            var db = new WorkOfTimeDatabase();
            var result = new ResultStatus { result = true };
            if (param != null)
            {
                var findStorage = db.GetCMP_Storage();
                foreach (var item in param)
                {
                    if (item.CariKodu != null)
                    {

                        if (findStorage != null && findStorage.Where(a => a.code == item.CariKodu).Count() > 0)
                        {
                            result = StorageUpdate(item);
                        }
                        else
                        {
                            result = StorageInsert(item);
                        }
                    }
                    else
                    {
                        result.result = false;
                        result.message = "CariKodu Alanı Zorunludur.";
                        return result;
                    }
                }
            }
            else
            {
                result.result = false;
                result.message = "Depo Listesi Boş Dönmektedir.";
                return result;
            }
            return result;
        }
        public ResultStatus StorageInsert(AdShipFindList param)
        {
            var db = new WorkOfTimeDatabase();
            var result = new ResultStatus { result = true };
            var findLocation = db.GetUT_Location().Where(a => a.name == param.SevkIl).FirstOrDefault();
            var findSH_User = db.GetVWSH_User().Where(a => a.FullName == param.SevkIlgiliKisi).FirstOrDefault();
            var insertStorage = new CMP_Storage
            {
                id = Guid.NewGuid(),
                created = DateTime.Now,
                address = param.SevkAdresi,
                code = param.CariKodu,
                name = param.CariUnvan,
                email = param.SevkEposta,
                supervisorId = findSH_User != null ? findSH_User.id : (Guid?)null,
                phone = param.SevkTelefon,
                locationId = findLocation != null ? findLocation.id : (Guid?)null,
            };
            result &= db.InsertCMP_Storage(insertStorage);
            return result;
        }
        public ResultStatus StorageUpdate(AdShipFindList param)
        {
            var db = new WorkOfTimeDatabase();
            var result = new ResultStatus { result = true };
            var checkCode = db.GetCMP_StorageByCode(param.CariKodu);
            var validator = StorageValidator(param, checkCode);
            if (validator.result)
            {
                checkCode.changed = DateTime.Now;
                checkCode.name = param.CariUnvan;
                checkCode.address = param.SevkAdresi;
                checkCode.code = param.CariKodu;
                checkCode.email = param.SevkEposta;
                checkCode.phone = param.SevkTelefon;
                result &= db.UpdateCMP_Storage(checkCode);
            }
            return result;
        }
        public ResultStatus CompanyValidator(AdClientFind param, CMP_Company company)
        {
            var result = new ResultStatus { result = true };
            if (company.email == param.Eposta1 && company.openAddress == param.Adres && company.name == param.CariUnvan && company.taxOffice == param.VergiDairesi && company.taxNumber == param.VergiNo)
            {
                return new ResultStatus
                {
                    result = false,
                    message = "Sistem Güncel",
                };
            }
            return result;
        }
        public ResultStatus StorageValidator(AdShipFindList param, CMP_Storage storage)
        {
            var result = new ResultStatus { result = true };
            //if (company.email == param.SevkEposta && company.address == param.SevkAdresi && company.name == param.CariUnvan && company.phone == param.SevkTelefon)
            //{
            //    return new ResultStatus
            //    {
            //        result = false,
            //        message = "Sistem Güncel",
            //    };
            //}
            return result;
        }
        public ResultStatus ProductValidator(AdShipFindList param, PRD_Product product)
        {
            var result = new ResultStatus { result = true };
            //if (company.email == param.SevkEposta && company.address == param.SevkAdresi && company.name == param.CariUnvan && company.phone == param.SevkTelefon)
            //{
            //    return new ResultStatus
            //    {
            //        result = false,
            //        message = "Sistem Güncel",
            //    };
            //}
            return result;
        }


    }
}
