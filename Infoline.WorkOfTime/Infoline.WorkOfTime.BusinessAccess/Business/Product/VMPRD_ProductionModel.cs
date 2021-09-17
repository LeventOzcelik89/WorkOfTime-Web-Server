using Infoline.Framework.Database;
using Infoline.Helper;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Web;

namespace Infoline.WorkOfTime.BusinessAccess
{
    public class VMPRD_ProductionStage : VWPRD_ProductionStage
    {
        public int Count { get; set; }
        
    }

    public class VMPRD_ProductionModel : VWPRD_Production
    {
        private WorkOfTimeDatabase db { get; set; }
        private DbTransaction trans { get; set; }
        public VWCMP_Company productionCompany { get; set; }
        public VWCMP_Storage prodictionStorage { get; set; }
        public VWCMP_Company centerCompany { get; set; }
        public VWCMP_Storage centerStorage { get; set; }
        public List<Guid> assignableUsers { get; set; }
        public List<VWPRD_ProductionUser> productionUsers { get; set; } = new List<VWPRD_ProductionUser>();
        public List<VWPRD_ProductionOperation> productionOperations { get; set; } = new List<VWPRD_ProductionOperation>();
        public List<VMPRD_ProductionStage> productionStages { get; set; } = new List<VMPRD_ProductionStage>();
        public List<VWPRD_ProductMateriel> productMaterials { get; set; } = new List<VWPRD_ProductMateriel>();
        public List<VWPRD_ProductionProduct> productionProducts { get; set; } = new List<VWPRD_ProductionProduct>();
        public List<VWPRD_TransactionItem> wastageProducts { get; set; } = new List<VWPRD_TransactionItem>();
        public List<VWPRD_TransactionItem> producedProducts { get; set; } = new List<VWPRD_TransactionItem>();
        public List<VWPRD_TransactionItem> transactionItems { get; set; } = new List<VWPRD_TransactionItem>();
        public List<VWPRD_ProductionProduct> productionProductList { get; set; } = new List<VWPRD_ProductionProduct>();
        public int Total { get; set; } = 0;
        public int Amount { get; set; } = 1;

        public Guid? productionSchemaId { get; set; }
        public string materialString { get; set; }

        public VMPRD_ProductionModel Load()
        {
            db = db ?? new WorkOfTimeDatabase();
            var production = db.GetVWPRD_ProductionById(this.id);

            if (production != null)
            {
                this.B_EntityDataCopyForMaterial(production, true);
                productionUsers = db.GetVWPRD_ProductionUserByProductionId(this.id).ToList();
                productionProducts = db.GetVWPRD_ProductionProductByProductId(this.id).ToList().OrderBy(x => x.type == (int)EnumPRD_ProductionProductsType.SonradanEklenen).ToList();
                productionOperations = db.GetVWPRD_ProductionOperationByProductionId(this.id).ToList();
                productionStages = db.GetVWPRD_ProductionStagesByProductionId(this.id).B_ConvertType<VMPRD_ProductionStage>().ToList();
                assignableUsers = productionUsers.Where(a => a.userId.HasValue).Select(a => a.userId.Value).ToList();
                productionSchemaId = productionStages.Count() > 0 ? productionStages.Where(a => a.productionSchemaId.HasValue).Select(x => x.productionSchemaId.Value).FirstOrDefault() : Guid.NewGuid();
                foreach (var stages in productionStages.Where(x => x != null))
                {
                    stages.Count = productionOperations.Where(a => a.dataId == stages.id && a.description != null).Sum(p => {
                        var count = 0;
                        int.TryParse(p.description.Split('_')[1], out count);

                        return count;
                    });
                    Total += stages.Count;
                }
            }
            this.code = !String.IsNullOrEmpty(this.code) ? this.code : BusinessExtensions.B_GetIdCode();
            return this;
        }

        public ResultStatus Save(Guid? userId, HttpRequestBase request = null, DbTransaction trans = null)
        {
            db = db ?? new WorkOfTimeDatabase();
            var production = db.GetVWPRD_ProductionById(this.id);
            var rs = new ResultStatus { result = true };

            if (production == null)
            {
                this.createdby = userId;
                this.created = DateTime.Now;
                rs = Insert(userId, trans);
            }
            else
            {
                this.changedby = userId;
                this.changed = DateTime.Now;
                rs = Update(userId, trans);
            }

            if (rs.result)
            {
                if (request != null)
                {
                    new FileUploadSave(request, this.id).SaveAs();
                }
            }

            return rs;
        }

        private ResultStatus Insert(Guid? userId, DbTransaction trans = null)
        {
            db = db ?? new WorkOfTimeDatabase();
            var transaction = trans ?? db.BeginTransaction();
            var production = new PRD_Production().B_EntityDataCopyForMaterial(this, true);
            var validatorResult = Validator();
            if (!validatorResult.result) return validatorResult;

            var createOperation = new VWPRD_ProductionOperation
            {
                id = Guid.NewGuid(),
                createdby = userId,
                created = DateTime.Now,
                productionId = this.id,
                dataId = this.id,
                dataTable = "PRD_Production",
                status = (int)EnumPRD_ProductionOperationStatus.UretimEmriVerildi,
                description = this.description,
            };

            productionOperations.Add(createOperation);

            if (assignableUsers != null && assignableUsers.Count() > 0)
            {
                var users = db.GetVWSH_UserByIds(this.assignableUsers.ToArray());

                if (users.Count() > 0)
                {
                    productionUsers.AddRange(users.Select(x => new VWPRD_ProductionUser
                    {
                        id = Guid.NewGuid(),
                        createdby = this.createdby,
                        productionId = this.id,
                        userId = x.id
                    }));

                    productionOperations.Add(new VWPRD_ProductionOperation
                    {
                        id = Guid.NewGuid(),
                        createdby = userId,
                        created = DateTime.Now.AddSeconds(1),
                        productionId = this.id,
                        status = (int)EnumPRD_ProductionOperationStatus.PersonelAtamasiYapildi,
                        description = string.Join(",", users.Select(a => a.FullName)) + " kullanıcıları üretime dahil oldu."
                    });
                }
            }


            var productionSchemaStages = db.GetVWPRD_ProductionSchemaStageByStageSchemaId(this.productionSchemaId.Value);
            var rs = new ResultStatus { result = true };

            productionStages.AddRange(productionSchemaStages.Select(x => new VWPRD_ProductionStage
            {
                id = Guid.NewGuid(),
                createdby = this.createdby,
                code = x.code,
                name = x.name,
                productionId = this.id,
                stageNumber = x.stageNum,
                productionSchemaId = this.productionSchemaId
            }).B_ConvertType<VMPRD_ProductionStage>());

            if (!string.IsNullOrEmpty(this.materialString))
            {
                var materials = Infoline.Helper.Json.Deserialize<List<VWPRD_ProductMateriel>>(this.materialString);
                if (materials.Count() > 0)
                {
                    var productionProducts = new List<PRD_ProductionProduct>();

                    productionProducts.AddRange(materials.Select(x => new PRD_ProductionProduct
                    {
                        id = Guid.NewGuid(),
                        createdby = userId,
                        productId = x.productId,
                        price = x.price,
                        materialId = x.materialId,
                        quantity = x.quantity,
                        productionId = this.id,
                        totalQuantity = x.totalQuantity,
                        type = (int)EnumPRD_ProductionProductsType.RecetedenGelen,
                        transactionType = (int)EnumPRD_TransactionType.HarcamaBildirimi
                    }));

                    rs &= db.BulkInsertPRD_ProductionProduct(productionProducts);
                }
            }

            rs = db.InsertPRD_Production(new PRD_Production().B_EntityDataCopyForMaterial(this), this.trans);
            rs &= db.BulkInsertPRD_ProductionOperation(productionOperations.Select(a => new PRD_ProductionOperation().B_EntityDataCopyForMaterial(a, true)), this.trans);
            rs &= db.BulkInsertPRD_ProductionUser(productionUsers.Select(a => new PRD_ProductionUser().B_EntityDataCopyForMaterial(a, true)), this.trans);
            rs &= db.BulkInsertPRD_ProductionStage(productionStages.Select(a => new PRD_ProductionStage().B_EntityDataCopyForMaterial(a, true)), this.trans);


            if (rs.result == true)
            {
                if (trans == null) transaction.Commit();
                return new ResultStatus { result = true, message = this.code + " kodlu üretim emri başarıyla oluşturuldu." };
            }
            else
            {
                if (trans == null) transaction.Rollback();
                return new ResultStatus { result = false, message = "Üretim emri oluşturma işlemi başarısız." };
            }
        }

        private ResultStatus Update(Guid? userId, DbTransaction trans = null)
        {
            db = db ?? new WorkOfTimeDatabase();
            var transaction = trans ?? db.BeginTransaction();
            this.assignableUsers = this.assignableUsers ?? new List<Guid>();
            var rs = new ResultStatus { result = true };

            var insertUsers = new List<PRD_ProductionUser>();
            var users = new List<VWSH_User>();
            var currentUserDatas = db.GetPRD_ProductionUsersByProductionId(this.id);
            var deletedUsers = currentUserDatas.Where(a => !this.assignableUsers.Contains(a.userId.Value));
            var newInsertUsers = this.assignableUsers.Where(a => !currentUserDatas.Select(c => c.userId).Contains(a)).ToArray();
            var productionProducts = db.GetPRD_ProductionProductByProductionId(this.id);
            if (productionProducts.Count() > 0)
            {
                rs &= db.BulkDeletePRD_ProductionProduct(productionProducts);
            }

            if (newInsertUsers.Count() > 0)
            {
                users = db.GetVWSH_UserByIds(newInsertUsers).ToList();
                var currentUsers = db.GetPRD_ProductionUsersByProductionId(this.id);
                rs &= db.BulkDeletePRD_ProductionUser(currentUsers, trans);
            }

            if (users.Count() > 0)
            {
                var now = DateTime.Now;
                productionOperations.Add(new VWPRD_ProductionOperation
                {
                    createdby = userId,
                    created = DateTime.Now,
                    productionId = this.id,
                    status = (int)EnumPRD_ProductionOperationStatus.PersonelAtamasiYapildi,
                    description = string.Join(",", users.Select(a => a.FullName)) + " kullanıcılarına atama yapıldı."
                });

                insertUsers = assignableUsers.Select(a => new PRD_ProductionUser
                {
                    createdby = userId,
                    created = (DateTime.Now),
                    userId = a,
                    productionId = this.id,
                }).ToList();
            }


            if (!string.IsNullOrEmpty(this.materialString))
            {
                var materials = Infoline.Helper.Json.Deserialize<List<VWPRD_ProductMateriel>>(this.materialString);
                if (materials.Count() > 0)
                {
                    var productionProductDatas = new List<PRD_ProductionProduct>();

                    productionProductDatas.AddRange(materials.Select(x => new PRD_ProductionProduct
                    {
                        id = Guid.NewGuid(),
                        createdby = userId,
                        productId = x.productId,
                        price = x.price,
                        materialId = x.materialId,
                        quantity = x.quantity,
                        productionId = this.id,
                        totalQuantity = x.totalQuantity,
                        type = (int)EnumPRD_ProductionProductsType.RecetedenGelen
                    }));

                    rs &= db.BulkInsertPRD_ProductionProduct(productionProductDatas);
                }
            }


            rs &= db.BulkInsertPRD_ProductionOperation(productionOperations.Select(x => new PRD_ProductionOperation().B_EntityDataCopyForMaterial(x, true)), trans);
            rs &= db.BulkDeletePRD_ProductionUser(deletedUsers, trans);
            rs &= db.BulkInsertPRD_ProductionUser(insertUsers, trans);

            var production = new PRD_Production().B_EntityDataCopyForMaterial(this, true);
            rs &= db.UpdatePRD_Production(production, false, transaction);

            if (rs.result == true)
            {
                if (trans == null) transaction.Commit();
                return new ResultStatus { result = true, message = "Üretim emri başarıyla güncellendi." };
            }
            else
            {
                if (trans == null) transaction.Rollback();
                return new ResultStatus { result = false, message = "Ürün emri güncelleme işlemi başarısız." };
            }
        }

        public ResultStatus Delete(Guid? userId, DbTransaction _trans = null)
        {
            db = db ?? new WorkOfTimeDatabase();
            this.trans = _trans ?? this.db.BeginTransaction();
            var production = db.GetPRD_ProductionById(this.id);
            if (production == null)
            {
                return new ResultStatus { result = false, message = "Üretim emri silinmiş." };
            }

            var _productionOperations = db.GetPRD_ProductionOperationByProductionId(production.id);
            var _productionUsers = db.GetPRD_ProductionUsersByProductionId(production.id);
            var _productionStages = db.GetPRD_ProductionStageByProductionId(production.id);
            var _productionProduct = db.GetPRD_ProductionProductByProductionId(production.id);

            if (_productionOperations.Count() > 0)
            {
                var dataIds = _productionOperations.Where(x => x.dataId.HasValue).Select(x => x.dataId.Value).ToArray();
                var transactionItems = db.GetVWPRD_TransactionItemByTransactionIds(dataIds);
                if (transactionItems.Count() > 0)
                {
                    return new ResultStatus
                    {
                        result = false,
                        message = "Üretim başladığı için silinemez. Lütfen üretim içerisinden stok işlemlerini temizledikten sonra tekrar silme işlemini gerçekleştiriniz."
                    };
                }
            }

            var rs = db.BulkDeletePRD_ProductionUser(_productionUsers, trans);
            rs &= db.BulkDeletePRD_ProductionOperation(_productionOperations, trans);
            rs &= db.BulkDeletePRD_ProductionStage(_productionStages, trans);
            rs &= db.BulkDeletePRD_ProductionProduct(_productionProduct, trans);
            rs &= db.DeletePRD_Production(production, trans);


            if (rs.result == true)
            {
                if (trans == null) trans.Commit();
                return new ResultStatus
                {
                    result = true,
                    message = "Üretim emri silme başarılı."
                };
            }
            else
            {
                if (trans == null) trans.Rollback();
                return new ResultStatus
                {
                    result = false,
                    message = rs.message
                };
            }
        }

        public static SimpleQuery UpdateQuery(SimpleQuery query, PageSecurity userStatus)
        {
            BEXP filter = null;

            if (!userStatus.AuthorizedRoles.Contains(new Guid(SHRoles.UretimYonetici)) && !userStatus.AuthorizedRoles.Contains(new Guid(SHRoles.SistemYonetici)))
            {
                filter |= new BEXP
                {
                    Operand1 = (COL)"assignableUserIds",
                    Operator = BinaryOperator.Like,
                    Operand2 = (VAL)string.Format("%{0}%", userStatus.user.id.ToString())
                };
            }

            query.Filter &= filter;
            return query;

        }

        public ResultStatus Validator()
        {
            if (!this.productionSchemaId.HasValue)
            {
                return new ResultStatus { message = "Üretim şeması zorunludur." };
            }
            if (!this.productionCompanyId.HasValue)
            {
                return new ResultStatus { result = false, message = "Üretimin yapılacağı şirket zorunludur." };
            }

            if (!this.productionStorageId.HasValue)
            {
                return new ResultStatus { result = false, message = "Ürünlerin yerinin deposu zorunludur." };
            }

            if (!this.centerCompanyId.HasValue)
            {
                return new ResultStatus { result = false, message = "Malzemelerin alınacağı şirket zorunludur." };
            }

            if (!this.centerStorageId.HasValue)
            {
                return new ResultStatus { result = false, message = "Malzemelerin alınacağı depo zorunludur." };
            }

            if (!this.productId.HasValue)
            {
                return new ResultStatus { result = false, message = "Üretim emri verilecek ürün zorunludur." };
            }

            if (!this.quantity.HasValue)
            {
                return new ResultStatus { result = false, message = "Üretim adedi zorunludur." };
            }

            //if (this.productMaterials.Count() <= 0)
            //{
            //	return new ResultStatus { result = false, message = "Ürünün reçetesi bulunmamaktadır." };
            //}

            return new ResultStatus { result = true };
        }


        public void InsertProductionProducts(VWPRD_ProductMateriel[] materiels, Guid? productionId, Guid? userId)
        {
            if (materiels.Count() <= 0)
            {
                return;
            }

            var db = new WorkOfTimeDatabase();
            var productionProducts = new List<PRD_ProductionProduct>();

            productionProducts.AddRange(materiels.Select(x => new PRD_ProductionProduct
            {
                id = Guid.NewGuid(),
                createdby = userId,
                productId = x.productId,
                price = x.price,
                materialId = x.materialId,
                quantity = x.quantity,
                productionId = productionId,
                totalQuantity = x.totalQuantity,
                type = (int)EnumPRD_ProductionProductsType.RecetedenGelen
            }));

            db.BulkInsertPRD_ProductionProduct(productionProducts);
        }

        public VWPRD_StockSummary[] ProductStocksByProductIdsAndStorageId(Guid[] productIds, Guid storageId)
        {
            var db = new WorkOfTimeDatabase();
            var stockSummary = db.GetVWPRD_StockSummaryByProductIdsAndStockId(productIds, storageId);
            return stockSummary;
        }

        public ResultStatus GetProductionProductAndTransaction(Guid productionId, DbTransaction _trans = null)
        {
            var db = new WorkOfTimeDatabase();
            var beginTrans = _trans ?? db.BeginTransaction();
            var data = new VMPRD_ProductionModel { id = productionId }.Load();
            var productionOperations = data.productionOperations.ToList();
            if (productionOperations.Where(x => x.status == (int)EnumPRD_ProductionOperationStatus.HarcamaBildirildi).Count() > 0)
            {
                var transactionItems = db.GetVWPRD_TransactionItemByTransactionIds(productionOperations.Where(a => a.dataId.HasValue).Select(x => x.dataId.Value).ToArray());
                foreach (var productionItem in data.productionProducts)
                {
                    var transactionItem = transactionItems.Where(x => x.productId == productionItem.materialId).FirstOrDefault();
                    if (transactionItem != null)
                    {
                        var product = data.productionProducts.Where(x => x.materialId == transactionItem.productId).FirstOrDefault();
                        if (product != null)
                        {
                            if (productionProductList.Where(x => x.materialId == product.materialId).Count() > 0)
                            {
                                continue;
                            }

                            var transaction = transactionItems.Where(x => x.productId == product.materialId && x.transactionType == (int)EnumPRD_TransactionType.HarcamaBildirimi).ToList();
                            var amountSpent = transaction.Select(x => x.quantity).Sum();
                            product.amountSpent = amountSpent;
                            productionProductList.Add(product);
                        }
                    }
                    else
                    {
                        productionProductList.Add(productionItem);
                    }

                }
            }

            if (productionOperations.Where(x => x.status == (int)EnumPRD_ProductionOperationStatus.FireBildirimiYapildi).Count() > 0)
            {
                var operationDatas = productionOperations.Where(x => x.status == (int)EnumPRD_ProductionOperationStatus.FireBildirimiYapildi).ToList();
                var transactionIds = operationDatas.Where(a => a.dataId.HasValue).Select(a => a.dataId.Value).ToArray();
                if (transactionIds.Count() > 0)
                {
                    data.wastageProducts = db.GetVWPRD_TransactionItemByTransactionIds(transactionIds).ToList();
                }
            }

            if (productionOperations.Where(x => x.status == (int)EnumPRD_ProductionOperationStatus.BitenUrunBildirimi).Count() > 0)
            {
                var operationDatas = productionOperations.Where(x => x.status == (int)EnumPRD_ProductionOperationStatus.BitenUrunBildirimi).ToList();
                var transactionIds = operationDatas.Where(a => a.dataId.HasValue).Select(a => a.dataId.Value).ToArray();
                if (transactionIds.Count() > 0)
                {
                    data.producedProducts = db.GetVWPRD_TransactionItemByTransactionIds(transactionIds).ToList();
                }
            }

            data.productionProducts = data.productionProducts.Where(a => a.transactionType == (int)EnumPRD_TransactionType.HarcamaBildirimi).ToList();

            return new ResultStatus
            {
                result = true,
                objects = data
            };
        }


    }
}
