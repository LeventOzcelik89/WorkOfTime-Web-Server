﻿using Infoline.Framework.Database;
using Infoline.Helper;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Web;

namespace Infoline.WorkOfTime.BusinessAccess
{

    public class VMPRD_ProductSummary
    {
        public double? storageStockCount { get; set; }
        public double? personStockCount { get; set; }
        public double? totalStockCount { get; set; }
        public double? totalPrice { get; set; }
    }

    public class VMPRD_ProductModel : VWPRD_Product
    {

        private WorkOfTimeDatabase db { get; set; }
        private DbTransaction trans { get; set; }
        public List<Guid> supplierCompanyIds { get; set; } = new List<Guid>();

        public VMPRD_ProductModel Load()
        {

            db = db ?? new WorkOfTimeDatabase();
            var product = db.GetVWPRD_ProductById(this.id);

            if (product != null)
            {
                this.B_EntityDataCopyForMaterial(product, true);
                var companys = db.GetPRD_ProductCompanyByProductId(this.id);
                this.supplierCompanyIds = companys.Where(a => a.type == (int)EnumPRD_ProductCompanyType.supplier).Select(a => a.companyId.Value).ToList();
            }
            else
            {
                this.code = BusinessExtensions.B_GetIdCode();
                this.barcodeType = this.barcodeType ?? (int)EnumPRD_ProductbarcodeType.EAN13;
                this.unitId = this.unitId ?? db.GetUT_UnitByName("adet")?.id;
                this.type = this.type ?? (int)EnumPRD_ProductType.Diger;
                this.stockType = this.stockType ?? (int)EnumPRD_ProductStockType.Stoksuz;
                this.status = this.status ?? (short?)EnumsProperties.EnumToArrayGeneric<EnumPRD_ProductStatus>().Sum(a => Convert.ToInt32(a.Key));
            }


            this.currentSellingCurrencyId = this.currentSellingCurrencyId ?? new Guid("7EC29DCA-9073-4028-90F2-D5613357117A");
            this.currentBuyingCurrencyId = this.currentBuyingCurrencyId ?? new Guid("7EC29DCA-9073-4028-90F2-D5613357117A");

            return this;
        }
        public ResultStatus Save(Guid? userId, HttpRequestBase request = null, DbTransaction trans = null)
        {

            db = db ?? new WorkOfTimeDatabase();
            var product = db.GetVWPRD_ProductById(this.id);
            var rs = new ResultStatus { result = true };
            this.barcodeType = this.barcodeType ?? (int)EnumPRD_ProductbarcodeType.EAN13;
            this.type = this.type ?? (int)EnumPRD_ProductType.Hizmet;
            this.stockType = this.stockType ?? (int)EnumPRD_ProductStockType.Stoksuz;
            var code = db.GetPRD_ProductByCode(this.code);

            if (product == null)
            {
                if (code != null) return new ResultStatus { result = false, message = "Aynı ürün kodu ile ürün eklenemez." };

                this.createdby = userId;
                this.created = DateTime.Now;
                rs = Insert(trans);
            }
            else
            {
                if (code != null && (product.id != code.id && code.code == this.code)) return new ResultStatus { result = false, message = "Aynı ürün kodu ile ürün eklenemez." };

                this.changedby = userId;
                this.changed = DateTime.Now;
                rs = Update(trans);
            }

            if (rs.result)
            {
                if (request != null)
                {
                    new FileUploadSave(request, this.id).SaveAs();
                    new TableAdditionalPropertySave(request, this.id, "PRD_Product").SaveAs(userId);
                }
            }

            return rs;
        }

        private ResultStatus Insert(DbTransaction trans = null)
        {
            db = db ?? new WorkOfTimeDatabase();
            var transaction = trans ?? db.BeginTransaction();
            var product = new PRD_Product().B_EntityDataCopyForMaterial(this, true);

            if (product.type == (int)EnumPRD_ProductType.Hizmet)
            {
                product.barcode = null;
                product.barcodeType = null;
                product.stockType = (short)EnumPRD_ProductStockType.Stoksuz;
                product.isCriticalStock = false;
                product.criticalStock = null;
            }


            var productCompanys = this.supplierCompanyIds.Select(a => new PRD_ProductCompany
            {
                created = DateTime.Now,
                createdby = this.createdby,
                productId = this.id,
                companyId = a,
                type = (int)EnumPRD_ProductCompanyType.supplier,
            }).ToList();


            var priceBuying = new PRD_ProductPrice
            {
                created = DateTime.Now,
                createdby = this.createdby,
                productId = this.id,
                price = this.currentBuyingPrice,
                currencyId = this.currentBuyingCurrencyId,
                type = (short)EnumPRD_ProductPriceType.Alis,
            };

            var priceSelling = new PRD_ProductPrice
            {
                created = DateTime.Now,
                createdby = this.createdby,
                productId = this.id,
                price = this.currentSellingPrice,
                currencyId = this.currentSellingCurrencyId,
                type = (short)EnumPRD_ProductPriceType.Satis,
            };


            if (product.isCriticalStock == true)
            {
                product.criticalStock = product.criticalStock == null ? 0 : product.criticalStock;
            }

            var rs = db.InsertPRD_Product(product, transaction);

            if (priceBuying.price != null && priceBuying.price != 0 && this.currentBuyingCurrencyId.HasValue)
            {
                rs &= db.InsertPRD_ProductPrice(priceBuying, transaction);
            }
            if (priceSelling.price != null && priceSelling.price != 0 && this.currentSellingCurrencyId.HasValue)
            {
                rs &= db.InsertPRD_ProductPrice(priceSelling, transaction);
            }
            rs &= db.BulkInsertPRD_ProductCompany(productCompanys, transaction);


            if (rs.result == true)
            {
                if (trans == null) transaction.Commit();
                return new ResultStatus { result = true, message = "Ürün Ekleme işlemi başarılı." };
            }
            else
            {
                if (trans == null) transaction.Rollback();
                return new ResultStatus { result = false, message = "Ürün Ekleme işlemi başarısız." };
            }
        }
        private ResultStatus Update(DbTransaction trans = null)
        {
            db = db ?? new WorkOfTimeDatabase();
            var transaction = trans ?? db.BeginTransaction();
            var product = new PRD_Product().B_EntityDataCopyForMaterial(this, true);
            var productCompanysOld = db.GetPRD_ProductCompanyByProductId(this.id);
            var productPriceBuyingOld = db.GetPRD_ProductPriceByProductId(this.id, EnumPRD_ProductPriceType.Alis);
            var productPriceSellingOld = db.GetPRD_ProductPriceByProductId(this.id, EnumPRD_ProductPriceType.Satis);
            var productCompanysNew = this.supplierCompanyIds.Select(a => new PRD_ProductCompany
            {
                created = DateTime.Now,
                createdby = this.changedby,
                productId = this.id,
                companyId = a,
                type = (int)EnumPRD_ProductCompanyType.supplier,
            }).ToList();

            if (product.type == (int)EnumPRD_ProductType.Hizmet)
            {
                product.barcode = null;
                product.barcodeType = null;
                product.stockType = (short)EnumPRD_ProductStockType.Stoksuz;
            }


            if (product.isCriticalStock == true)
            {
                product.criticalStock = product.criticalStock == null ? 0 : product.criticalStock;
            }

            var rs = db.UpdatePRD_Product(product, true, transaction);
            rs &= db.BulkDeletePRD_ProductCompany(productCompanysOld, transaction);
            rs &= db.BulkInsertPRD_ProductCompany(productCompanysNew, transaction);

            if (this.currentBuyingPrice != null && this.currentBuyingPrice != 0 && this.currentBuyingCurrencyId.HasValue)
            {
                if (productPriceBuyingOld == null || (productPriceBuyingOld.currencyId != this.currentBuyingCurrencyId) || (productPriceBuyingOld.price != this.currentBuyingPrice))
                {
                    rs &= db.InsertPRD_ProductPrice(new PRD_ProductPrice
                    {
                        created = DateTime.Now,
                        createdby = this.changedby,
                        productId = this.id,
                        price = this.currentBuyingPrice,
                        currencyId = this.currentBuyingCurrencyId,
                        type = (short)EnumPRD_ProductPriceType.Alis,
                    }, transaction);
                }
            }

            if (this.currentSellingPrice != null && this.currentSellingPrice != 0 && this.currentSellingCurrencyId.HasValue)
            {
                if (productPriceSellingOld == null || (productPriceSellingOld.currencyId != this.currentSellingCurrencyId) || (productPriceSellingOld.price != this.currentSellingPrice))
                {
                    rs &= db.InsertPRD_ProductPrice(new PRD_ProductPrice
                    {
                        created = DateTime.Now,
                        createdby = this.changedby,
                        productId = this.id,
                        price = this.currentSellingPrice,
                        currencyId = this.currentSellingCurrencyId,
                        type = (short)EnumPRD_ProductPriceType.Satis,
                    }, transaction);
                }
            }

            if (rs.result == true)
            {
                if (trans == null) transaction.Commit();
                return new ResultStatus { result = true, message = "Ürün Güncelleme işlemi başarılı." };
            }
            else
            {
                if (trans == null) transaction.Rollback();
                return new ResultStatus { result = false, message = "Ürün Güncelleme işlemi başarısız." };
            }
        }
        public ResultStatus Delete(DbTransaction trans = null)
        {
            db = db ?? new WorkOfTimeDatabase();
            var _product = db.GetPRD_ProductById(this.id);
            if (_product == null)
            {
                return new ResultStatus { result = false, message = "Ürün zaten silinmiş." };
            }
            ///TODO:Şahin
            //var productAction = db.GetPRD_ActionProductByProductIdCount(_product.id);
            //if (productAction > 0)
            //{
            //    return new ResultStatus { result = false, message = "Ürüne bağlı stok hareketi bulunduğundan silinemez." };
            //}

            var inventory = db.GetPRD_InventoryByProductIdCount(_product.id);
            if (inventory > 0)
            {
                return new ResultStatus { result = false, message = "Ürüne bağlı envanterler bulunduğundan silinemez." };
            }

            var invoiceItem = db.GetCMP_InvoiceItemByProductIdCount(_product.id);
            if (invoiceItem > 0)
            {
                return new ResultStatus { result = false, message = "Ürün Fatura,Teklif veya Siparişlerde Kullanıldığından silinemez." };
            }

            var dbres = new ResultStatus { result = true };
            var transaction = trans ?? db.BeginTransaction();

            var priceBuying = db.GetPRD_ProductPriceByProductIdAll(_product.id);
            dbres &= db.BulkDeletePRD_ProductPrice(priceBuying, trans);

            var pointSelling = db.GetPRD_ProductPointSellingByProductId(_product.id);
            dbres &= db.BulkDeletePRD_ProductPointSelling(pointSelling, trans);

            var productCompany = db.GetPRD_ProductCompanyByProductId(_product.id);
            dbres &= db.BulkDeletePRD_ProductCompany(productCompany, trans);

            var product = new PRD_Product().B_EntityDataCopyForMaterial(_product, true);
            dbres &= db.DeletePRD_Product(product, trans);

            if (dbres.result == true)
            {
                if (trans == null) transaction.Commit();
                return new ResultStatus
                {
                    result = true,
                    message = "Ürün silme işlemi başarıslı oldu."
                };
            }
            else
            {
                if (trans == null) transaction.Rollback();
                return new ResultStatus
                {
                    result = false,
                    message = "Ürün silme işlemi başarısız oldu."
                };
            }
        }

        /*Custom Metodlar*/
        public ResultStatus UpsertPointSelling(PRD_ProductPointSelling productPointSelling, DbTransaction trans = null)
        {
            if (!productPointSelling.productId.HasValue)
            {
                return new ResultStatus { result = false, message = "Ürün Zorunludur." };
            }

            if (!productPointSelling.productId.HasValue || !productPointSelling.startDate.HasValue || !productPointSelling.endDate.HasValue)
            {
                return new ResultStatus { result = false, message = "Zorunlu alanları doldurunuz." };
            }

            db = db ?? new WorkOfTimeDatabase();
            var dbres = new ResultStatus { result = true };

            var data = db.GetPRD_ProductPointSellingById(productPointSelling.id);
            var control = db.GetPRD_ProductPointSellingByProductAndCompanyAndDateControl(productPointSelling.productId.Value, productPointSelling.startDate.Value, productPointSelling.endDate.Value);
            var transaction = trans ?? db.BeginTransaction();
            if (data != null)
            {
                if (control != null && control.id != productPointSelling.id)
                {
                    return new ResultStatus
                    {
                        result = false,
                        message = string.Format("{0} - {1} tarihleri arasında seçmiş olduğunuz ürüne ait puan girilmiştir.", string.Format("{0:dd.MM.yyyy}", control.startDate),
                        string.Format("{0:dd.MM.yyyy}", control.endDate))
                    };
                }

                productPointSelling.changed = DateTime.Now;
                dbres &= db.UpdatePRD_ProductPointSelling(productPointSelling, true, transaction);
            }
            else
            {

                if (control != null)
                {
                    return new ResultStatus
                    {
                        result = false,
                        message = string.Format("{0} - {1} tarihleri arasında seçmiş olduğunuz ürüne ait puan girilmiştir.", string.Format("{0:dd.MM.yyyy}", control.startDate),
                        string.Format("{0:dd.MM.yyyy}", control.endDate))
                    };
                }
                productPointSelling.created = DateTime.Now;
                dbres &= db.InsertPRD_ProductPointSelling(productPointSelling, transaction);
            }


            if (dbres.result == true)
            {
                if (trans == null) transaction.Commit();
                return new ResultStatus
                {
                    result = true,
                    message = "Ürün puan güncelleme işlemi başarılı oldu."
                };
            }
            else
            {
                if (trans == null) transaction.Rollback();
                return new ResultStatus
                {
                    result = false,
                    message = "Ürün puan güncelleme işlemi başarısız oldu."
                };
            }

        }
        public ResultStatus UpsertProductMateriel(PRD_ProductMateriel productMaterial, DbTransaction trans = null)
        {

            if (!productMaterial.productId.HasValue || !productMaterial.materialId.HasValue || !productMaterial.quantity.HasValue)
            {
                return new ResultStatus { result = false, message = "Zorunlu alanları doldurunuz." };
            }

            if (productMaterial.productId.Value == productMaterial.materialId.Value)
            {
                return new ResultStatus { result = false, message = "Ürün ve materyal aynı olamaz." };
            }

            db = db ?? new WorkOfTimeDatabase();
            var transaction = trans ?? db.BeginTransaction();
            var dbres = new ResultStatus { result = true };
            var oldProductMaterial = db.GetPRD_ProductMaterielById(productMaterial.id);
            var control = db.GetPRD_ProductMaterielByProductIdAndMaterialId(productMaterial.productId.Value, productMaterial.materialId.Value);

            if (oldProductMaterial != null)
            {
                if (control != null && control.id != oldProductMaterial.id)
                {
                    return new ResultStatus { result = false, message = "Aynı ürüne ait materyal ürün bulunmaktadır." };
                }
                productMaterial.changed = DateTime.Now;
                dbres &= db.UpdatePRD_ProductMateriel(productMaterial, true, transaction);

            }
            else
            {
                if (control != null)
                {
                    return new ResultStatus { result = false, message = "Aynı ürüne ait materyal ürün bulunmaktadır." };
                }

                productMaterial.created = DateTime.Now;
                dbres &= db.InsertPRD_ProductMateriel(productMaterial, transaction);
            }

            if (dbres.result == true)
            {
                if (trans == null) transaction.Commit();
                return new ResultStatus
                {
                    result = true,
                    message = "Ürün materyal ekleme işlemi başarılı oldu."
                };
            }
            else
            {
                if (trans == null) transaction.Rollback();
                return new ResultStatus
                {
                    result = false,
                    message = "Ürün materyal ekleme işlemi başarısız oldu."
                };
            }
        }
        /*Custom Metodlar*/



        //Kaldırılacak
        public static SimpleQuery UpdateQuery(SimpleQuery query, PageSecurity userStatus)
        {
            var db = new WorkOfTimeDatabase();
            BEXP filter = null;

            filter = new BEXP
            {
                Operand1 = new BEXP
                {
                    Operand1 = (COL)"status",
                    Operator = BinaryOperator.Equal,
                    Operand2 = (VAL)1
                },

                Operator = BinaryOperator.Or,
                Operand2 = new BEXP
                {
                    Operand1 = (COL)"status",
                    Operator = BinaryOperator.Equal,
                    Operand2 = (VAL)3
                }
            };

            query.Filter &= filter;

            return query;

        }


    }
}