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
            var insertCompany = new CMP_Company
            {
                id = Guid.NewGuid(),
                createdby = Guid.Empty,
                created = DateTime.Now,
                openAddress = param.Adres,
                code = param.CariKodu,
                name = param.CariUnvan,
                email = param.Eposta1,
                taxOffice = param.VergiDairesi,
                taxNumber = param.VergiNo,
                commercialTitle = param.CariUnvan,
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
                checkCode.changedby = Guid.Empty;
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
                        //    if (findProduct != null && findProduct.Where(a => a.code == item.UrunKodu).Count() > 0)
                        //    {
                        //        result = ProductUpdate(item);
                        //    }
                        if (findProduct != null && findProduct.Where(a => a.code == item.UrunKodu).Count() == 0)
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
            var db = new WorkOfTimeDatabase();
            var result = new ResultStatus { result = true };
            short? stockType = 0;
            short? type = 0;
            if (param.StokTakipTipi == "Takip Yapılmıyor")
            {
                stockType = (int)EnumPRD_ProductStockType.Stoksuz;
            }
            else if (param.StokTakipTipi == "Seri Numarası Takibi Yapılıyor")
            {
                stockType = (int)EnumPRD_ProductStockType.SeriNoluTakip;
            }
            else
            {
                stockType = (int)EnumPRD_ProductStockType.NormalTakip;
            }

            if (param.MalzemeTuruAciklamasi== "Hammadde")
            {
                type = (int)EnumPRD_ProductType.Hammadde;
            }
            else if(param.MalzemeTuruAciklamasi== "Sabit Kıymet")
            {
                type = (int)EnumPRD_ProductType.TicariMal;
            }
            else if(param.MalzemeTuruAciklamasi == "Yarı Mamul")
            {
                type = (int)EnumPRD_ProductType.YariMamul;
            }
            else if (param.MalzemeTuruAciklamasi == "Mamul")
            {
                type = (int)EnumPRD_ProductType.TicariMal;
            }
            else
            {
                type = (int)EnumPRD_ProductType.Diger;
            }


            var insertUnit = new UT_Unit
            {
                id = Guid.NewGuid(),
                createdby = Guid.Empty,
                created = DateTime.Now,
                name = param.Birim,
            };
            var insertCategory = new PRD_Category
            {
                id = Guid.NewGuid(),
                createdby = Guid.Empty,
                created = DateTime.Now,
                name = param.Kategori,
                code = param.MalzemeTuruKodu,
            };
            var insertBrand = new PRD_Brand
            {
                id = Guid.NewGuid(),
                createdby = Guid.Empty,
                created = DateTime.Now,
                name = param.MarkaAciklamasi,
                code = param.MarkaKodu,
            };
            var checkUnit = db.GetUT_UnitByName(param.Birim);

            var insertProduct = new PRD_Product
            {
                id = Guid.NewGuid(),
                name = param.UrunAciklamasi1,
                description = param.UrunAciklamasi2,
                code = param.UrunKodu,
                categoryId = insertCategory != null && insertCategory.name != "" ? insertCategory.id : (Guid?)null,
                brandId = insertBrand != null && insertBrand.name != "" ? insertBrand.id : (Guid?)null,
                unitId = checkUnit != null ? checkUnit.id : (Guid?)null,
                created = DateTime.Now,
                createdby = Guid.Empty,
                stockType = stockType,
                type= type,
            };
            var insertProductPrice = new PRD_ProductPrice
            {
                created = DateTime.Now,
                createdby = Guid.Empty,
                id = Guid.NewGuid(),
                price = param.BirimFiyat,
                productId = insertProduct != null ? insertProduct.id : (Guid?)null,
            };
            if (checkUnit == null)
            {
                result &= db.InsertUT_Unit(insertUnit);
            }
            if (insertCategory.name != "")
            {
                result &= db.InsertPRD_Category(insertCategory);
            }
            if (insertBrand.name != "")
            {
                result &= db.InsertPRD_Brand(insertBrand);
            }
            result &= db.InsertPRD_Product(insertProduct);
            result &= db.InsertPRD_ProductPrice(insertProductPrice);
            return result;
        }
        //public ResultStatus ProductUpdate(AdItemsFindList param)
        //{
        //    var db = new WorkOfTimeDatabase();
        //    var result = new ResultStatus { result = true };
        //    var checkCode = db.GetPRD_ProductByCode(param.UrunKodu);
        //    var validator = ProductValidator(param, checkCode);
        //    if (validator.result)
        //    {
        //        var updateUnit = new UT_Unit
        //        {
        //            id = Guid.NewGuid(),
        //            changedby = Guid.Empty,
        //            changed = DateTime.Now,
        //            name = param.Birim,
        //        };
        //        var updateCategory = new PRD_Category
        //        {
        //            id = Guid.NewGuid(),
        //            changedby = Guid.Empty,
        //            changed = DateTime.Now,
        //            name = param.Kategori,
        //            code = param.MalzemeTuruKodu,
        //        };
        //        var updateBrand = new PRD_Brand
        //        {
        //            id = Guid.NewGuid(),
        //            changedby = Guid.Empty,
        //            changed = DateTime.Now,
        //            name = param.MarkaAciklamasi,
        //            code = param.MarkaKodu,
        //        };
        //        checkCode.brandId = updateBrand != null ? updateBrand.id : (Guid?)null;
        //        checkCode.categoryId = updateCategory != null ? updateCategory.id : (Guid?)null;
        //        checkCode.unitId = updateUnit != null ? updateUnit.id : (Guid?)null;
        //        checkCode.name = param.UrunAciklamasi1;
        //        checkCode.description = param.UrunAciklamasi2;
        //        checkCode.code = param.UrunKodu;
        //        checkCode.changed = DateTime.Now;
        //        checkCode.changedby = Guid.Empty;
        //    }
        //    return result;
        //}
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
            var findSH_User = db.GetVWSH_User().Where(a => a.FullName == param.SevkIlgiliKisi).FirstOrDefault();
            var insertStorage = new CMP_Storage
            {
                id = Guid.NewGuid(),
                createdby = Guid.Empty,
                created = DateTime.Now,
                address = param.SevkAdresi,
                code = param.CariKodu,
                name = param.CariUnvan,
                email = param.SevkEposta,
                supervisorId = findSH_User != null ? findSH_User.id : (Guid?)null,
                phone = param.SevkTelefon,
                postCode = param.SevkPostaKodu,
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
                checkCode.changedby = Guid.Empty;
                checkCode.name = param.CariUnvan;
                checkCode.address = param.SevkAdresi;
                checkCode.code = param.CariKodu;
                checkCode.email = param.SevkEposta;
                checkCode.phone = param.SevkTelefon;
                checkCode.postCode = param.SevkPostaKodu;
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
            if (storage.email == param.SevkEposta && storage.address == param.SevkAdresi && storage.name == param.CariUnvan && storage.phone == param.SevkTelefon && storage.postCode == param.SevkPostaKodu)
            {
                return new ResultStatus
                {
                    result = false,
                    message = "Sistem Güncel",
                };
            }
            return result;
        }
        public ResultStatus ProductValidator(AdItemsFindList param, PRD_Product product)
        {
            var result = new ResultStatus { result = true };
            if (product.name == param.UrunAciklamasi1 && product.description == param.UrunAciklamasi2)
            {
                return new ResultStatus
                {
                    result = false,
                    message = "Sistem Güncel",
                };
            }
            return result;
        }


    }
}
