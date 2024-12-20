﻿using Infoline.Framework.Database;
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
        public VWPRD_Transaction Transaction { get; set; }
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
        public List<VWPRD_ProductionProduct> productionProductList { get; set; } = new List<VWPRD_ProductionProduct>();
        public VWPRD_Product ProductDetail { get; set; }
        public int Total { get; set; } = 0;
        public Guid? productionSchemaId { get; set; }
        public string inputId_Adress { get; set; }
        public string outputId_Adress { get; set; }
        public string tenderIds { get; set; }
        public int amount { get; set; }
        public double OutOfStock { get; set; }
        public string ProductSerialCodes { get; set; }
        public bool expensReport { get; set; }
        public VMPRD_ProductionModel Load()
        {
            db = db ?? new WorkOfTimeDatabase();
            var production = db.GetVWPRD_ProductionById(this.id);
            if (production != null)
            {
                this.B_EntityDataCopyForMaterial(production, true);
                ProductDetail = db.GetVWPRD_ProductById(this.productId.Value);
                productionUsers = db.GetVWPRD_ProductionUserByProductionId(this.id).ToList();
                productionProducts = db.GetVWPRD_ProductionProductByProductId(this.id).ToList().OrderBy(x => x.materialId_Title).OrderBy(x => x.type == (int)EnumPRD_ProductionProductsType.SonradanEklenen).ToList();
                productionOperations = db.GetVWPRD_ProductionOperationByProductionId(this.id).ToList();
                productionStages = db.GetVWPRD_ProductionStagesByProductionId(this.id).B_ConvertType<VMPRD_ProductionStage>().ToList();
                assignableUsers = productionUsers.Where(a => a.userId.HasValue).Select(a => a.userId.Value).ToList();
                productionSchemaId = productionStages.Count() > 0 ? productionStages.Where(a => a.productionSchemaId.HasValue).Select(x => x.productionSchemaId.Value).FirstOrDefault() : Guid.NewGuid();
                foreach (var stages in productionStages.Where(x => x != null))
                {
                    stages.Count = productionOperations.Where(a => a.dataId == stages.id && a.description != null).Sum(p =>
                    {
                        var count = 0;
                        int.TryParse(p.description.Split('_')[1], out count);
                        return count;
                    });
                    Total += stages.Count;
                }
            }
            if (Transaction != null)
            {
                var outputInfo = this.GetInfo(this.Transaction.outputId, this.Transaction.outputTable, this.Transaction.outputCompanyId);
                var inputInfo = this.GetInfo(this.Transaction.inputId, this.Transaction.inputTable, this.Transaction.inputCompanyId);
                this.inputId_Adress = inputInfo.Adress;
                this.Transaction.inputId_Title = inputInfo.Text;
                this.Transaction.inputCompanyId = inputInfo.CompanyId;
                this.Transaction.inputCompanyId_Title = inputInfo.CompanyIdTitle;
                this.Transaction.inputId_Title = inputInfo.Text;
                this.outputId_Adress = outputInfo.Adress;
                this.Transaction.outputId_Title = outputInfo.Text;
                this.Transaction.outputCompanyId = outputInfo.CompanyId;
                this.Transaction.outputCompanyId_Title = outputInfo.CompanyIdTitle;
                this.Transaction.outputId_Title = outputInfo.Text;
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
            var rs = new ResultStatus { result = true };
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
            rs = db.InsertPRD_Production(new PRD_Production().B_EntityDataCopyForMaterial(this), this.trans);
            rs &= db.BulkInsertPRD_ProductionOperation(productionOperations.Select(a => new PRD_ProductionOperation().B_EntityDataCopyForMaterial(a, true)), this.trans);
            rs &= db.BulkInsertPRD_ProductionUser(productionUsers.Select(a => new PRD_ProductionUser().B_EntityDataCopyForMaterial(a, true)), this.trans);
            rs &= db.BulkInsertPRD_ProductionStage(productionStages.Select(a => new PRD_ProductionStage().B_EntityDataCopyForMaterial(a, true)), this.trans);
            if (productMaterials.Count > 0)
            {
                var productionProducts = new List<PRD_ProductionProduct>();
                productionProducts.AddRange(productMaterials.Select(x => new PRD_ProductionProduct
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
                    transactionType = (int)EnumPRD_TransactionType.HarcamaBildirimi,
                    currencyId = x.currencyId,
                    unitId = x.unitId,
                }));
                rs &= db.BulkInsertPRD_ProductionProduct(productionProducts);
            }
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
        public ResultStatus InsertForSpendProducts(Guid userId)
        {
            db = db ?? new WorkOfTimeDatabase();
            trans = trans ?? db.BeginTransaction();
            var outputInfo = this.GetInfo(this.Transaction.outputId, this.Transaction.outputTable, this.Transaction.outputCompanyId);
            this.Transaction.inputCompanyId = null;
            this.Transaction.inputId = null;
            this.Transaction.inputTable = null;
            this.Transaction.outputId_Title = outputInfo.Text;
            this.Transaction.outputCompanyId = outputInfo.CompanyId;
            this.Transaction.outputCompanyId_Title = outputInfo.CompanyIdTitle;
            this.Transaction.outputId = outputInfo.DataId;
            this.Transaction.outputTable = outputInfo.DataTable;
            var DBResult = new ResultStatus { result = true };
            var listOfTransItems = new List<VMPRD_TransactionItems>();
            foreach (var item in productionProducts)
            {
                var findProduct = db.GetVWPRD_ProductionProductByMetarialIdAndProductionId(item.productId.Value, id);
                if (findProduct != null)
                {
                    listOfTransItems.Add(new VMPRD_TransactionItems
                    {
                        productId = item.productId,
                        quantity = item.quantity,
                        serialCodes = item.serialCodes != null ? item.serialCodes : "",
                        unitPrice = findProduct.price,
                    });
                }
            }
            var transModel = new VMPRD_TransactionModel
            {
                inputId = this.Transaction.inputId,
                inputTable = this.Transaction.inputTable,
                outputId = this.Transaction.outputId,
                outputTable = this.Transaction.outputTable,
                created = this.created,
                createdby = this.createdby,
                status = (int)EnumPRD_TransactionStatus.islendi,
                items = listOfTransItems,
                date = DateTime.Now,
                code = BusinessExtensions.B_GetIdCode(),
                type = (short)EnumPRD_TransactionType.HarcamaBildirimi,
                id = Guid.NewGuid()
            };
            DBResult &= transModel.Save(userId, trans);
            DBResult &= db.InsertPRD_ProductionOperation(new PRD_ProductionOperation
            {
                createdby = userId,
                created = DateTime.Now,
                productionId = this.id,
                dataId = transModel.id,
                dataTable = "PRD_Transaction",
                status = (int)EnumPRD_ProductionOperationStatus.HarcamaBildirildi,
                userId = userId,
            }, trans);
            var sendedList = new List<PRD_ProductionProduct>();
            foreach (var product in productionProducts)
            {
                if (product.id != null)
                {
                    var getProduct = db.GetPRD_ProductionProductByMetarialIdAndProductionId(product.productId.Value, this.id);
                    if (getProduct == null)
                    {
                        var findProduct = db.GetVWPRD_ProductById(product.productId.Value);
                        if (findProduct != null)
                        {
                            var newProduct = new PRD_ProductionProduct
                            {
                                id = Guid.NewGuid(),
                                createdby = userId,
                                productId = findProduct.id,
                                price = product.price,
                                materialId = findProduct.id,
                                quantity = product.quantity,
                                productionId = this.id,
                                totalQuantity = product.totalQuantity,
                                type = (int)EnumPRD_ProductionProductsType.SonradanEklenen,
                                transactionType = (int)EnumPRD_TransactionType.HarcamaBildirimi,
                                currencyId = findProduct.currentSellingCurrencyId,
                                unitId = findProduct.unitId,
                                amountSpent = product.quantity
                            };
                            DBResult &= db.InsertPRD_ProductionProduct(newProduct);
                        }
                    }
                    else
                    {
                        if (product.quantity != null)
                        {
                            if (getProduct.amountSpent != null)
                            {
                                getProduct.amountSpent += product.quantity;
                            }
                            else
                            {
                                getProduct.amountSpent = product.quantity;
                            }
                            sendedList.Add(getProduct);
                        }
                    }
                }
            }
            DBResult &= db.BulkUpdatePRD_ProductionProduct(sendedList);
            if (DBResult.result)
            {
                trans.Commit();
            }
            else
            {
                trans.Rollback();
            }
            return DBResult;
        }
        public ResultStatus InsertForFinishedProducts(Guid userId)
        {
            this.db = this.db ?? new WorkOfTimeDatabase();
            trans = trans ?? db.BeginTransaction();
            var transaction = db.GetPRD_TransactionById(Transaction.id);
            var SpendProduct = this.producedProducts.Where(x => x.quantity > 0 && x.productId != null).ToList();
            this.code = string.IsNullOrEmpty(this.code) ? BusinessExtensions.B_GetIdCode() : this.code;
            this.Transaction.status = this.Transaction.status ?? (int)EnumPRD_TransactionStatus.beklemede;
            this.Transaction.date = this.Transaction.date ?? DateTime.Now;
            if (Transaction.type == null)
            {
                return new ResultStatus { result = false, message = "İşlem tipi seçilmeli." };
            }
            if (this.Transaction.type == (int)EnumPRD_TransactionType.Transfer && (this.Transaction.inputId == this.Transaction.outputId)) return new ResultStatus { result = false, message = "Çıkış Yapılacak şube/depo/kısım ile Giriş Yapılacak şube/depo/kısım birbirinden farklı olmalıdır." };
            var inputInfo = GetInfo(this.Transaction.inputId, this.Transaction.inputTable, this.Transaction.inputCompanyId);
            var outputInfo = GetInfo(this.Transaction.outputId, this.Transaction.outputTable, this.Transaction.outputCompanyId);
            this.Transaction.inputId_Title = inputInfo.Text;
            this.Transaction.inputCompanyId = inputInfo.CompanyId;
            this.Transaction.inputCompanyId_Title = inputInfo.CompanyIdTitle;
            this.Transaction.inputId = inputInfo.DataId;
            this.Transaction.inputTable = inputInfo.DataTable;
            this.Transaction.outputCompanyId = outputInfo.CompanyId;
            this.Transaction.outputId = outputInfo.DataId;
            this.Transaction.outputTable = outputInfo.DataTable;
            this.ProductDetail = db.GetVWPRD_ProductById(this.ProductDetail.id);
            var DBResult = new ResultStatus { result = true };
            var PRDTransaction = new PRD_Transaction().B_EntityDataCopyForMaterial(this.Transaction);
            #region harcama bildirimi
            if (expensReport)
            {
                if (productionProducts.Count > 0)
                {
                    var listOfTransItems = new List<VMPRD_TransactionItems>();
                    foreach (var item in productionProducts)
                    {
                        var findProduct = db.GetVWPRD_ProductionProductByMetarialIdAndProductionId(item.productId.Value, id);
                        if (findProduct != null)
                        {
                            listOfTransItems.Add(new VMPRD_TransactionItems
                            {
                                productId = item.productId,
                                quantity = item.quantity,
                                serialCodes = item.serialCodes != null ? item.serialCodes : "",
                                unitPrice = findProduct.price,
                            });
                        }
                    }
                    var transModelForExpens = new VMPRD_TransactionModel
                    {
                        inputId = null,
                        inputTable = null,
                        outputId = this.Transaction.inputId,
                        outputTable = this.Transaction.inputTable,
                        created = this.created,
                        createdby = this.createdby,
                        status = (int)EnumPRD_TransactionStatus.islendi,
                        items = listOfTransItems,
                        date = DateTime.Now,
                        code = BusinessExtensions.B_GetIdCode(),
                        type = (short)EnumPRD_TransactionType.HarcamaBildirimi,
                        id = Guid.NewGuid()
                    };
                    var sendedList = new List<PRD_ProductionProduct>();
                    foreach (var product in productionProducts)
                    {
                        if (product.id != null)
                        {
                            var getProduct = db.GetPRD_ProductionProductByMetarialIdAndProductionId(product.productId.Value, id);
                            if (getProduct == null)
                            {
                                var findProduct = db.GetVWPRD_ProductById(product.productId.Value);
                                if (findProduct != null)
                                {
                                    var newProduct = new PRD_ProductionProduct
                                    {
                                        id = Guid.NewGuid(),
                                        createdby = userId,
                                        productId = findProduct.id,
                                        price = product.price,
                                        materialId = findProduct.id,
                                        quantity = product.quantity,
                                        productionId = this.id,
                                        totalQuantity = product.totalQuantity,
                                        type = (int)EnumPRD_ProductionProductsType.SonradanEklenen,
                                        transactionType = (int)EnumPRD_TransactionType.HarcamaBildirimi,
                                        currencyId = findProduct.currentSellingCurrencyId,
                                        unitId = findProduct.unitId,
                                        amountSpent = product.quantity
                                    };
                                    DBResult &= db.InsertPRD_ProductionProduct(newProduct);
                                }
                            }
                            else
                            {
                                if (product.quantity != null)
                                {
                                    if (getProduct.amountSpent != null)
                                    {
                                        getProduct.amountSpent += product.quantity;
                                    }
                                    else
                                    {
                                        getProduct.amountSpent = product.quantity;
                                    }
                                    sendedList.Add(getProduct);
                                }
                            }
                        }
                    }
                    DBResult &= db.BulkUpdatePRD_ProductionProduct(sendedList);
                    DBResult &= transModelForExpens.Save(userId, trans);
                    DBResult &= db.InsertPRD_ProductionOperation(new PRD_ProductionOperation
                    {
                        createdby = userId,
                        created = DateTime.Now,
                        productionId = this.id,
                        dataId = transModelForExpens.id,
                        dataTable = "PRD_Transaction",
                        status = (int)EnumPRD_ProductionOperationStatus.HarcamaBildirildi,
                        userId = userId,
                    }, trans);
                }
            }
            #endregion

            #region Biten Ürün Bildirimi 
            var transModel = new VMPRD_TransactionModel
            {
                inputId = this.Transaction.outputId,
                inputTable = this.Transaction.outputTable,
                outputId = null,
                outputTable = null,
                created = this.created,
                createdby = this.createdby,
                status = (int)EnumPRD_TransactionStatus.islendi,
                items = new List<VMPRD_TransactionItems> { new VMPRD_TransactionItems {
                    productId=ProductDetail.id,
                    serialCodes=ProductSerialCodes,
                    quantity=amount,
                    unitPrice=ProductDetail.currentSellingPrice
                } },
                date = DateTime.Now,
                code = BusinessExtensions.B_GetIdCode(),
                type = (short)EnumPRD_TransactionType.UretimBildirimi,
                id = Guid.NewGuid()
            };
            DBResult &= transModel.Save(userId, trans);
            DBResult &= db.InsertPRD_ProductionOperation(new PRD_ProductionOperation
            {
                createdby = userId,
                created = DateTime.Now,
                productionId = this.id,
                dataId = transModel.id,
                dataTable = "PRD_Transaction",
                status = (int)EnumPRD_ProductionOperationStatus.BitenUrunBildirimi,
                userId = userId,
            }, trans);
            #endregion
            
            if (DBResult.result)
            {
                trans.Commit();
            }
            else
            {
                trans.Rollback();
            }
            return DBResult;
        }
        public ResultStatus InsertForFireNotification(Guid userId)
        {
            db = db ?? new WorkOfTimeDatabase();
            trans = trans ?? db.BeginTransaction();
            var transaction = trans ?? db.BeginTransaction();
            var errorList = new List<string>();
            var outputInfo = this.GetInfo(this.Transaction.outputId, this.Transaction.outputTable, this.Transaction.outputCompanyId);
            var inputInfo = this.GetInfo(this.Transaction.inputId, this.Transaction.inputTable, this.Transaction.inputCompanyId);
            this.Transaction.inputId_Title = null;
            this.Transaction.inputCompanyId = null;
            this.Transaction.inputCompanyId_Title = null;
            this.Transaction.inputId = null;
            this.Transaction.inputTable = null;
            this.Transaction.outputId_Title = outputInfo.Text;
            this.Transaction.outputCompanyId = outputInfo.CompanyId;
            this.Transaction.outputCompanyId_Title = outputInfo.CompanyIdTitle;
            this.Transaction.outputId = outputInfo.DataId;
            this.Transaction.outputTable = outputInfo.DataTable;
            var DBResult = new ResultStatus { result = true };
            var PRDTransaction = new PRD_Transaction().B_EntityDataCopyForMaterial(this.Transaction);
            var transModel = new VMPRD_TransactionModel
            {
                inputId = this.Transaction.inputId,
                inputTable = this.Transaction.inputTable,
                outputId = this.Transaction.outputId,
                outputTable = this.Transaction.outputTable,
                created = this.created,
                createdby = this.createdby,
                status = (int)EnumPRD_TransactionStatus.islendi,
                items = this.productionProducts.Where(s => s.productId.HasValue).Select(a => new VMPRD_TransactionItems
                {
                    productId = a.productId,
                    quantity = a.quantity,
                    serialCodes = a.serialCodes != null ? a.serialCodes : "",
                    unitPrice = a.price,
                }).ToList(),
                date = DateTime.Now,
                code = BusinessExtensions.B_GetIdCode(),
                type = (short)EnumPRD_TransactionType.FireFisi,
                id = Guid.NewGuid()
            };
            DBResult &= transModel.Save(userId, trans);
            var sendedList = new List<PRD_ProductionProduct>();
            foreach (var product in productionProducts)
            {
                if (product.id != null)
                {
                    var getProduct = db.GetPRD_ProductionProductByMetarialIdAndProductionId(product.productId.Value, id);
                    if (getProduct == null)
                    {
                        var findProduct = db.GetVWPRD_ProductById(product.productId.Value);
                        if (findProduct != null)
                        {
                            var newProduct = new PRD_ProductionProduct
                            {
                                id = Guid.NewGuid(),
                                createdby = userId,
                                productId = findProduct.id,
                                price = product.price,
                                materialId = findProduct.id,
                                quantity = product.quantity,
                                productionId = this.id,
                                totalQuantity = product.totalQuantity,
                                type = (int)EnumPRD_ProductionProductsType.SonradanEklenen,
                                transactionType = (int)EnumPRD_TransactionType.HarcamaBildirimi,
                                currencyId = findProduct.currentSellingCurrencyId,
                                unitId = findProduct.unitId,
                                amountSpent = product.quantity
                            };
                            DBResult &= db.InsertPRD_ProductionProduct(newProduct);
                        }
                    }
                    else
                    {
                        if (product.quantity != null)
                        {
                            if (getProduct.amountSpent != null)
                            {
                                getProduct.amountSpent += product.quantity;
                            }
                            else
                            {
                                getProduct.amountSpent = product.quantity;
                            }
                            sendedList.Add(getProduct);
                        }
                    }
                }
            }
            DBResult &= db.BulkUpdatePRD_ProductionProduct(sendedList);
            DBResult &= db.InsertPRD_ProductionOperation(new PRD_ProductionOperation
            {
                createdby = userId,
                created = DateTime.Now,
                productionId = this.id,
                dataId = transModel.id,
                dataTable = "PRD_Transaction",
                status = (int)EnumPRD_ProductionOperationStatus.FireBildirimiYapildi,
                description = this.description,
                userId = userId,
            }, trans);
            if (DBResult.result)
            {
                trans.Commit();
            }
            else
            {
                trans.Rollback();
            }
            return DBResult;
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
        public ResultStatus Delete(Guid? userId)
        {
            db = db ?? new WorkOfTimeDatabase();
            trans = db.BeginTransaction();
            var production = db.GetPRD_ProductionById(this.id);
            if (production == null)
            {
                trans.Rollback();
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
                    trans.Rollback();
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
                trans.Commit();
                return new ResultStatus
                {
                    result = true,
                    message = "Üretim emri silme başarılı."
                };
            }
            else
            {
                trans.Rollback();
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
        private OwnerInfo GetInfo(Guid? dataId, string dataTable, Guid? dataCompanyId)
        {
            OwnerInfo result = new OwnerInfo();
            db = new WorkOfTimeDatabase();
            if (dataId != null && dataTable != null)
            {
                switch (dataTable)
                {
                    case "CMP_Storage":
                        var storage = db.GetVWCMP_StorageById(dataId.Value);
                        result = new OwnerInfo
                        {
                            CompanyId = storage?.companyId,
                            CompanyIdTitle = storage?.companyId_Title,
                            Location = storage?.location,
                            Text = storage?.companyId_Title + " | " + storage?.name,
                            Adress = storage?.address,
                            DataId = dataId,
                            DataTable = dataTable,
                        };
                        break;
                    case "SH_User":
                        var user = db.GetVWSH_UserById(dataId.Value);
                        var company = db.GetVWCMP_CompanyById(user.CompanyId ?? Guid.NewGuid());
                        result = new OwnerInfo
                        {
                            CompanyId = company?.id,
                            CompanyIdTitle = company?.fullName,
                            Location = company?.location,
                            Text = company?.fullName + " | " + user?.FullName,
                            Adress = user?.address,
                            DataId = dataId,
                            DataTable = dataTable,
                        };
                        break;
                    case "CMP_Company":
                        var cmp = db.GetVWCMP_CompanyById(dataId.Value);
                        result = new OwnerInfo
                        {
                            CompanyId = cmp?.id,
                            CompanyIdTitle = cmp?.fullName,
                            Location = cmp.location,
                            Text = cmp?.fullName,
                            Adress = ""
                        };
                        break;
                    default:
                        break;
                }
            }
            else
            {
                var company = db.GetVWCMP_CompanyById(dataCompanyId ?? Guid.NewGuid());
                result.CompanyId = dataCompanyId;
                result.CompanyIdTitle = company?.fullName;
                result.Text = company?.fullName;
                result.Adress = company?.openAddress;
                result.Location = company?.location;
                result.DataId = null;
                result.DataTable = null;
            }
            return result;
        }
        public ResultStatus AddUser(Guid userId)
        {
            var db = new WorkOfTimeDatabase();
            var trans = db.BeginTransaction();
            var result = new ResultStatus { result = true };
            var findProduction = db.GetPRD_ProductionById(id);
            var ops = new List<PRD_ProductionOperation>();
            var prodUsers = db.GetVWPRD_ProductionUsersByProductionId(id);
            var deleted = prodUsers.Where(a => !this.assignableUsers.Contains(a.userId.Value)).ToList();
            var added = this.assignableUsers.Where(a => !prodUsers.Select(b => b.userId).Contains(a)).ToArray();
            var addedUsers = db.GetSH_UserByIds(added);
            if (deleted.Count > 0)
            {
                ops.Add(new PRD_ProductionOperation
                {
                    createdby = userId,
                    created = DateTime.Now,
                    productionId = this.id,
                    status = (int)EnumPRD_ProductionOperationStatus.PersonelUretimdenAlindi,
                    description = String.Join(", ", deleted.Select(a => a.userId_Title)) + " kullanıcıları üretimden alındı."
                });
            }
            if (added.Length > 0)
            {
                ops.Add(new PRD_ProductionOperation
                {
                    createdby = userId,
                    created = DateTime.Now,
                    productionId = this.id,
                    status = (int)EnumPRD_ProductionOperationStatus.PersonelAtamasiYapildi,
                    description = String.Join(", ", addedUsers.Select(a => a.firstname + " " + a.lastname)) + " kullanıcıları üretime dahil oldu."
                });
            }
            result = db.BulkDeletePRD_ProductionUser(prodUsers.B_ConvertType<PRD_ProductionUser>(), trans);
            result &= db.BulkInsertPRD_ProductionUser(assignableUsers.Select(a => new PRD_ProductionUser
            {
                userId = a,
                productionId = this.id
            }), trans);
            result &= db.BulkInsertPRD_ProductionOperation(ops, trans);
            if (result.result)
            {
                trans.Commit();
                return result;
            }
            else
            {
                trans.Rollback();
                return result;
            }
        }
    }
}
