using GeoAPI.Geometries;
using Infoline.Framework.Database;
using Infoline.Helper;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace Infoline.WorkOfTime.BusinessAccess
{
    public class OwnerInfo
    {
        public string DataTable { get; set; }
        public Guid? DataId { get; set; }
        public Guid? CompanyId { get; set; }
        public string CompanyIdTitle { get; set; }
        public IGeometry Location { get; set; }
        public string Text { get; set; }
        public string Adress { get; set; }
    }

    public class PrintInfo
    {
        public VWSH_User user { get; set; }
        public string logo { get; set; }

    }

    public class VMPRD_TransactionItems : VWPRD_TransactionItem
    {
        public string brand_Title { get; set; }
        public string categoryId_Title { get; set; }
        public string description { get; set; }
        public short? stockType { get; set; }
        public string currencyTitle { get; set; }
    }

    public class VMPRD_TransactionModel : VWPRD_Transaction
    {
        public List<VMPRD_TransactionItems> items { get; set; } = new List<VMPRD_TransactionItems>() { };
        private WorkOfTimeDatabase db { get; set; }
        private DbTransaction trans { get; set; }
        private List<VWPRD_Product> products { get; set; }
        private List<VWPRD_Inventory> inventories { get; set; }

        public string inputId_Adress { get; set; }
        public string outputId_Adress { get; set; }
        public PrintInfo printInfo { get; set; } = new PrintInfo { user = new VWSH_User { }, logo = "" };
        public bool hasUpdate { get; set; }
        public string tenderIds { get; set; }
        public Guid? taskId { get; set; }
        public VMPRD_TransactionModel Load()
        {
            this.db = this.db ?? new WorkOfTimeDatabase();
            var transaction = db.GetVWPRD_TransactionById(this.id);
            if (transaction != null)
            {
                this.B_EntityDataCopyForMaterial(transaction, true);
                if (TenantConfig.Tenant.TenantCode == 1187)
                {
                    this.description = this.project_Title;
                }
                this.items = db.GetVWPRD_TransactionItemByTransactionId(transaction.id).OrderBy(a => a.created).ToList().Select(a => new VMPRD_TransactionItems().B_EntityDataCopyForMaterial(a)).ToList();
                this.hasUpdate = true;
            }
            else
            {
                this.hasUpdate = false;
                this.code = string.IsNullOrEmpty(this.code) ? BusinessExtensions.B_GetIdCode() : this.code;
                this.date = this.date ?? DateTime.Now;
                this.type_Title = this.type != null ? ((EnumPRD_TransactionType)this.type).B_ToDescription() : "";
                this.status_Title = this.status != null ? ((EnumPRD_TransactionStatus)this.status).B_ToDescription() : "";
                //Sipariş altından irsaliye
                if (this.orderId.HasValue)
                {
                    var db = new WorkOfTimeDatabase();
                    var invoice = db.GetCMP_InvoiceById(this.orderId.Value);
                    var invoiceItems = db.GetVWCMP_InvoiceItemByInvoiceId(this.orderId.Value);
                    var products = db.GetPRD_ProductByIds(invoiceItems.Where(a => a.productId.HasValue).Select(a => a.productId.Value).ToArray()).Where(s => s.stockType != (short)EnumPRD_ProductStockType.Stoksuz);
                    this.outputId = invoice.supplierStorageId;
                    this.outputTable = "CMP_Storage";
                    this.inputCompanyId = invoice.customerId;
                    this.inputId = invoice.customerStorageId;
                    this.inputTable = "CMP_Storage";
                    this.type = (short)(invoice.direction == (int)EnumCMP_InvoiceDirectionType.Alis ? EnumPRD_TransactionType.GelenIrsaliye : EnumPRD_TransactionType.GidenIrsaliye);
                    this.type_Title = invoice.direction == (int)EnumCMP_InvoiceDirectionType.Alis ? EnumPRD_TransactionType.GelenIrsaliye.B_ToDescription() : EnumPRD_TransactionType.GidenIrsaliye.B_ToDescription();

                    var items = invoiceItems.Where(s => s.productId.HasValue && products.Select(d => d.id).Contains(s.productId.Value)).ToArray();

                    foreach (var item in items)
                    {
                        this.items.Add(new VMPRD_TransactionItems
                        {
                            productId = item.productId,
                            quantity = item.quantity,
                            unitPrice = item.price,
                        });
                    }
                }
                //Sipariş altından irsaliye
                var outputInfo = this.GetInfo(this.outputId, this.outputTable, this.outputCompanyId);
                var inputInfo = this.GetInfo(this.inputId, this.inputTable, this.inputCompanyId);

                this.inputId_Adress = inputInfo.Adress;
                this.inputId_Title = inputInfo.Text;
                this.inputCompanyId = inputInfo.CompanyId;
                this.inputCompanyId_Title = inputInfo.CompanyIdTitle;
                this.inputId_Title = inputInfo.Text;
                this.outputId_Adress = outputInfo.Adress;
                this.outputId_Title = outputInfo.Text;
                this.outputCompanyId = outputInfo.CompanyId;
                this.outputCompanyId_Title = outputInfo.CompanyIdTitle;
                this.outputId_Title = outputInfo.Text;




                if (this.createdby.HasValue)
                {
                    var user = db.GetSH_UserById(this.createdby.Value);
                    this.createdby_Title = user.firstname + " " + user.lastname;
                }
            }



            if (this.type == (int)EnumPRD_TransactionType.ZimmetAlma)
            {
                this.printInfo.user = db.GetVWSH_UserById(this.outputId.Value);
            }

            if (this.type == (int)EnumPRD_TransactionType.ZimmetVerme)
            {
                this.printInfo.user = db.GetVWSH_UserById(this.inputId.Value);
            }

            if (this.items != null && this.items.Count() > 0)
            {
                var products = db.GetVWPRD_ProductByIds(this.items.GroupBy(a => a.productId).Where(a => a.Key != null).Select(a => a.Key.Value).ToArray());
                foreach (var item in this.items)
                {
                    item.productId_Title = products.Where(x => x.id == item.productId).Select(a => a.fullName).FirstOrDefault();
                    item.brand_Title = products.Where(x => x.id == item.productId).Select(a => a.brandId_Title).FirstOrDefault();
                    item.categoryId_Title = products.Where(x => x.id == item.productId).Select(a => a.categoryId_Title).FirstOrDefault();
                    item.unitId_Title = products.Where(x => x.id == item.productId).Select(a => a.unitId_Title).FirstOrDefault();
                    item.description = products.Where(x => x.id == item.productId).Select(a => a.description).FirstOrDefault();
                    item.stockType = products.Where(x => x.id == item.productId).Select(x => x.stockType).FirstOrDefault();
                    item.currencyTitle = products.Where(x => x.id == item.productId).Select(x => x.currentSellingCurrencyId_Title).FirstOrDefault();
                }
            }



            if (this.inputId.HasValue && this.inputTable == "CMP_Storage")
            {
                this.printInfo.logo = db.GetVWCMP_StorageById((Guid)this.inputId)?.companyId_Image;
            }

            if (this.outputId.HasValue && this.outputTable == "CMP_Storage")
            {
                this.printInfo.logo = db.GetVWCMP_StorageById((Guid)this.outputId)?.companyId_Image;
            }


            if (this.items.Count() == 0)
            {
                this.items.Add(new VMPRD_TransactionItems { });
            }
            return this;
        }
        public ResultStatus Save(Guid? userId, DbTransaction _trans = null, Guid? inventoryId = null)
        {
            this.db = this.db ?? new WorkOfTimeDatabase();
            this.trans = _trans ?? db.BeginTransaction();
            var transaction = db.GetPRD_TransactionById(this.id);
            this.items = this.items.Where(a => a.productId.HasValue && a.quantity > 0).ToList();
            this.code = string.IsNullOrEmpty(this.code) ? BusinessExtensions.B_GetIdCode() : this.code;
            this.status = this.status ?? (int)EnumPRD_TransactionStatus.beklemede;
            this.date = this.date ?? DateTime.Now;
            if (this.type == null) return new ResultStatus { result = false, message = "İşlem tipi seçilmeli." };
            if (this.items.Count() == 0) return new ResultStatus { result = false, message = "Ürün kalemi girilmedi." };
            if (this.type == (int)EnumPRD_TransactionType.Transfer && (this.inputId == this.outputId)) return new ResultStatus { result = false, message = "Çıkış Yapılacak şube/depo/kısım ile Giriş Yapılacak şube/depo/kısım birbirinden farklı olmalıdır." };

            var productids = this.items.Select(a => a.productId.Value).ToArray();
            var serials = this.items.Where(a => a.serialCodes != null).SelectMany(a => a.serialCodes.Split(',').Select(c => c.ToLower())).ToArray();
            this.products = db.GetVWPRD_ProductByIds(productids).ToList();
            this.inventories = db.GetVWPRD_InventoryBySerialCodesAndIds(productids, serials).ToList();

            var control = this.items.Where(a => a.serialCodes != null).GroupBy(a => a.productId).Select(a => new { productId = a.Key, serialCodes = a.SelectMany(c => c.serialCodes.Split(',')).GroupBy(g => g.ToLower()).ToArray() }).ToArray();
            var controlText = control.Where(a => a.serialCodes.Count(c => c.Count() > 1) > 0).Select(a => string.Format("{0} ürünü için {1} serinumaraları", this.products.Where(c => c.id == a.productId).Select(c => c.code + " | " + c.name).FirstOrDefault(), string.Join(",", a.serialCodes.Where(c => c.Count() > 1).Select(g => g.Key)))).ToArray();

            if (controlText.Count() > 0)
                return new ResultStatus { result = false, message = string.Join(",", controlText) + " birden fazla girilmiştir." };
            if (((this.type >= 0 && this.type < 10) || (this.type == 11)) && this.status == (int)EnumPRD_TransactionStatus.islendi && this.inputId == null)
                return new ResultStatus { result = false, message = "Giriş deposu seçilmedi." };
            if (((this.type >= 10 && this.type < 20) || (this.type == 1)) && this.status == (int)EnumPRD_TransactionStatus.islendi && this.outputId == null)
                return new ResultStatus { result = false, message = "Çıkış deposu seçilmedi." };

            var rs = new ResultStatus { result = true };
            if (transaction == null)
            {
                this.created = DateTime.Now;
                this.createdby = userId;
                rs = this.Insert(inventoryId);
            }
            else
            {
                this.created = transaction.created;
                this.createdby = transaction.createdby;
                this.changed = DateTime.Now;
                this.changedby = userId;
                rs = this.Update(transaction);
            }

            if (rs.result == true)
            {
                if (_trans == null) trans.Commit();
                rs.message = ((EnumPRD_TransactionType)(this.type)).B_ToDescription() + " başarılı bir şekilde kaydedildi.";
            }
            else
            {
                if (_trans == null) trans.Rollback();
                rs.message = ((EnumPRD_TransactionType)(this.type)).B_ToDescription() + " kaydedilirken sorunlar oluştu.Mesaj : " + rs.message;
            }

            return rs;
        }
        private ResultStatus Update(PRD_Transaction transaction)
        {
            var errorList = new List<string>();
            var inputInfo = GetInfo(this.inputId, this.inputTable, this.inputCompanyId);
            var outputInfo = GetInfo(this.outputId, this.outputTable, this.outputCompanyId);


            this.inputId_Title = inputInfo.Text;
            this.inputCompanyId = this.inputCompanyId ?? inputInfo.CompanyId;
            this.inputCompanyId_Title = inputInfo.CompanyIdTitle;
            this.inputId = inputInfo.DataId;
            this.inputTable = inputInfo.DataTable;
            this.outputId_Title = outputInfo.Text;
            this.outputCompanyId = this.outputCompanyId ?? outputInfo.CompanyId;
            this.outputCompanyId_Title = outputInfo.CompanyIdTitle;
            this.outputId = outputInfo.DataId;
            this.outputTable = outputInfo.DataTable;



            var DBResult = new ResultStatus { result = true };
            var DBTransactionItems = db.GetPRD_TransactionItemByTransactionId(this.id);
            var DBproduct = db.GetPRD_ProductByIds(DBTransactionItems.Where(a => a.productId.HasValue).Select(a => a.productId.Value).ToArray());


            var PRDTransaction = new PRD_Transaction().B_EntityDataCopyForMaterial(this, true);
            var PRDTransactionItems = this.items.Select(a => new PRD_TransactionItem
            {
                id = a.id,
                productId = a.productId,
                quantity = a.quantity,
                serialCodes = a.serialCodes,
                unitPrice = a.unitPrice,
                created = this.created,
                createdby = this.createdby,
                changed = this.changed,
                changedby = this.changedby,
                transactionId = this.id,
            });


            if (this.status == (int)EnumPRD_TransactionStatus.beklemede || DBproduct.Count(a => a.stockType == (int)EnumPRD_ProductStockType.SeriNoluTakip) == 0)
            {
                DBResult &= db.UpdatePRD_Transaction(PRDTransaction, false, this.trans);
                DBResult &= db.BulkDeletePRD_TransactionItem(DBTransactionItems, this.trans);
                DBResult &= db.BulkInsertPRD_TransactionItem(PRDTransactionItems, this.trans);
            }
            else
            {

                if (transaction.status == (int)EnumPRD_TransactionStatus.islendi)
                {
                    //TODO: Bir ara burası geliştirilecek.
                    DBResult.result = false;
                    DBResult.message = "Ürün kalemlerinde seri numaralı takipli ürün olan işlemler düzenlenemez.";
                    return DBResult;

                    //var PRDInventories = new List<PRD_Inventory>();
                    //var PRDInventoryActions = new List<PRD_InventoryAction>();





                }
                else
                {

                    var PRDInventories = new List<PRD_Inventory>();
                    var PRDInventoryActions = new List<PRD_InventoryAction>();

                    foreach (var item in this.items)
                    {
                        var product = this.products.Where(a => a.id == item.productId).FirstOrDefault();
                        if (product == null) continue;
                        if (this.status == (int)EnumPRD_TransactionStatus.islendi && product.stockType == (int)EnumPRD_ProductStockType.SeriNoluTakip)
                        {
                            errorList.AddRange(this.Validator(item));
                            foreach (var serialcode in (item.serialCodes ?? "").Split(','))
                            {
                                var winventory = this.inventories.Where(a => a.productId == item.productId && a.serialcode == serialcode).FirstOrDefault();

                                var inventory = winventory != null ? new PRD_Inventory().B_EntityDataCopyForMaterial(winventory) : new PRD_Inventory
                                {
                                    created = DateTime.Now,
                                    createdby = this.createdby,
                                    productId = item.productId,
                                    code = BusinessExtensions.B_GetIdCode(),
                                    serialcode = serialcode,
                                };

                                if (winventory == null)
                                {
                                    PRDInventories.Add(inventory);
                                }

                                PRDInventoryActions.Add(new PRD_InventoryAction
                                {
                                    createdby = this.createdby,
                                    created = DateTime.Now,
                                    inventoryId = inventory.id,
                                    transactionId = this.id,
                                    transactionItemId = item.id,
                                    dataId = outputId,
                                    dataTable = outputTable,
                                    dataCompanyId = outputCompanyId,
                                    location = outputInfo.Location,
                                    type = (short)GetOutputType((EnumPRD_TransactionType)this.type)
                                });
                                PRDInventoryActions.Add(new PRD_InventoryAction
                                {
                                    createdby = this.createdby,
                                    created = DateTime.Now.AddSeconds(1),
                                    inventoryId = inventory.id,
                                    transactionId = this.id,
                                    transactionItemId = item.id,
                                    dataId = inputId,
                                    dataTable = inputTable,
                                    dataCompanyId = inputCompanyId,
                                    location = inputInfo.Location,
                                    type = (short)GetInputType((EnumPRD_TransactionType)this.type)
                                });
                            }
                        }
                    }

                    if (errorList.Count() > 0)
                    {
                        DBResult.result = false;
                        DBResult.message = "<ul style='margin:10px 0 0 0;padding:0;'>" + string.Join("", errorList.Select(a => "<li>" + a + "</li>")) + "</ul>";
                        return DBResult;
                    }

                    DBResult &= db.UpdatePRD_Transaction(PRDTransaction, false, this.trans);
                    DBResult &= db.BulkDeletePRD_TransactionItem(DBTransactionItems, this.trans);
                    DBResult &= db.BulkInsertPRD_TransactionItem(PRDTransactionItems, this.trans);
                    DBResult &= db.BulkInsertPRD_Inventory(PRDInventories, this.trans);
                    DBResult &= db.BulkInsertPRD_InventoryAction(PRDInventoryActions, this.trans);

                }
            }


            return DBResult;
        }
        private ResultStatus Insert(Guid? inventoryId)
        {

            var errorList = new List<string>();
            var inputInfo = GetInfo(this.inputId, this.inputTable, this.inputCompanyId);
            var outputInfo = GetInfo(this.outputId, this.outputTable, this.outputCompanyId);


            this.inputId_Title = inputInfo.Text;
            this.inputCompanyId = inputInfo.CompanyId;
            this.inputCompanyId_Title = inputInfo.CompanyIdTitle;
            this.inputId = inputInfo.DataId;
            this.inputTable = inputInfo.DataTable;
            this.outputId_Title = outputInfo.Text;
            this.outputCompanyId = outputInfo.CompanyId;
            this.outputCompanyId_Title = outputInfo.CompanyIdTitle;
            this.outputId = outputInfo.DataId;
            this.outputTable = outputInfo.DataTable;


            var DBResult = new ResultStatus { result = true };
            var PRDTransaction = new PRD_Transaction().B_EntityDataCopyForMaterial(this);
            var PRDTransactionItems = new List<PRD_TransactionItem>();
            var PRDInventories = new List<PRD_Inventory>();
            var PRDInventoryActions = new List<PRD_InventoryAction>();




            for (int i = 0; i < this.items.Count(); i++)
            {
                var item = this.items[i];
                var product = this.products.Where(a => a.id == item.productId).FirstOrDefault();
                if (product == null) continue;
                var transItem = new PRD_TransactionItem
                {
                    created = new DateTime(this.created.Value.Ticks).AddMilliseconds(i),
                    createdby = this.createdby,
                    productId = item.productId,
                    transactionId = this.id,
                    quantity = item.quantity,
                    unitPrice = item.unitPrice,
                    serialCodes = item.serialCodes,
                };

                //Sadece Stoklar işlendi ise envanterler yaratılır aksi takdirde yaratılmaz
                if (this.status == (int)EnumPRD_TransactionStatus.islendi && product.stockType == (int)EnumPRD_ProductStockType.SeriNoluTakip)
                {
                    errorList.AddRange(this.Validator(item));
                    foreach (var serialcode in (item.serialCodes ?? "").Split(','))
                    {
                        var winventory = this.inventories.Where(a => a.productId == item.productId && a.serialcode == serialcode).FirstOrDefault();

                        var inventory = winventory != null ? new PRD_Inventory().B_EntityDataCopyForMaterial(winventory) : new PRD_Inventory
                        {
                            id = inventoryId.HasValue ? inventoryId.Value : Guid.NewGuid(),
                            created = DateTime.Now,
                            createdby = this.createdby,
                            productId = item.productId,
                            code = BusinessExtensions.B_GetIdCode(),
                            serialcode = serialcode,
                            type = inventoryId.HasValue ? (Int16)EnumPRD_InventoryType.Sokum : (Int16)EnumPRD_InventoryType.Diger
                        };

                        if (winventory == null)
                        {
                            PRDInventories.Add(inventory);
                        }


                        PRDInventoryActions.Add(new PRD_InventoryAction
                        {
                            createdby = this.createdby,
                            created = DateTime.Now,
                            inventoryId = inventory.id,
                            transactionId = this.id,
                            transactionItemId = transItem.id,
                            dataId = outputId,
                            dataTable = outputTable,
                            dataCompanyId = outputCompanyId,
                            location = outputInfo.Location,
                            type = (short)GetOutputType((EnumPRD_TransactionType)this.type)
                        });


                        PRDInventoryActions.Add(new PRD_InventoryAction
                        {
                            createdby = this.createdby,
                            created = DateTime.Now.AddSeconds(1),
                            inventoryId = inventory.id,
                            transactionId = this.id,
                            transactionItemId = transItem.id,
                            dataId = inputId,
                            dataTable = inputTable,
                            dataCompanyId = inputCompanyId,
                            location = inputInfo.Location,
                            type = (short)GetInputType((EnumPRD_TransactionType)this.type)
                        });
                    }
                }
                PRDTransactionItems.Add(transItem);


            }


            if (this.orderId.HasValue)
            {
                DBResult &= new VMCMP_OrderModels { id = this.orderId.Value }.Load(false).UpdateStatus((int)EnumCMP_OrderStatus.IrsaliyeKesildi, this.createdby.Value, this.trans);
            }


            if (errorList.Count() > 0)
            {
                DBResult.result = false;
                DBResult.message = "<ul style='margin:10px 0 0 0;padding:0;'>" + string.Join("", errorList.Select(a => "<li>" + a + "</li>")) + "</ul>";
                return DBResult;
            }




            DBResult &= db.InsertPRD_Transaction(PRDTransaction, this.trans);
            DBResult &= db.BulkInsertPRD_TransactionItem(PRDTransactionItems, this.trans);
            DBResult &= db.BulkInsertPRD_Inventory(PRDInventories, this.trans);
            DBResult &= db.BulkInsertPRD_InventoryAction(PRDInventoryActions, this.trans);

            if (!string.IsNullOrEmpty(this.tenderIds))
            {
                var tenders = this.tenderIds.Split(',').Select(x => Guid.Parse(x)).ToArray();
                DBResult &= db.BulkInsertCMP_TenderTransaction(tenders.Select(c => new CMP_TenderTransaction
                {
                    created = DateTime.Now,
                    createdby = this.createdby,
                    tenderId = c,
                    transactionId = this.id
                }), this.trans);

                DBResult &= db.BulkInsertCMP_InvoiceAction(tenders.Select(c => new CMP_InvoiceAction
                {
                    type = (int)EnumCMP_TenderStatus.TeklifIrsaliye,
                    invoiceId = c,
                    created = DateTime.Now,
                    createdby = this.createdby,
                    description = "İrsaliye Kesildi"
                }), this.trans);

                foreach (var tender in tenders)
                {
                    DBResult &= new VMCMP_TenderModels { id = tender }.Load(false, (int)EnumCMP_InvoiceDirectionType.Alis).UpdateStatus((int)EnumCMP_TenderStatus.TeklifIrsaliye, this.createdby.Value, this.trans);
                }

            }

            if (DBResult.result == true)
            {
                EmailSender();
            }

            return DBResult;
        }
        public ResultStatus Delete(DbTransaction _trans = null)
        {
            this.db = this.db ?? new WorkOfTimeDatabase();
            var transaction = db.GetPRD_TransactionById(this.id);
            if (transaction != null)
            {
                this.trans = _trans ?? db.BeginTransaction();
                var transactionItems = db.GetPRD_TransactionItemByTransactionId(transaction.id);
                var inventoryActions = db.GetPRD_InventoryActionByTransactionId(transaction.id);

                var rs = new ResultStatus { result = true };
                if (inventoryActions.Count() > 0)
                {
                    var inventories = db.GetVWPRD_InventoryByIds(inventoryActions.GroupBy(a => a.inventoryId.Value).Select(a => a.Key).ToArray());
                    var deleteinventories = inventories.Where(a => a.firstTransactionId == transaction.id).Select(a => new PRD_Inventory { id = a.id }).ToArray();
                    var deleteactions = inventoryActions.Where(a => inventories.Select(c => c.lastTransactionItemId).Contains(a.transactionItemId)).ToArray();
                    rs &= db.BulkDeletePRD_InventoryAction(deleteactions, trans);
                    rs &= db.BulkDeletePRD_Inventory(deleteinventories, trans);
                }
                rs &= db.BulkDeletePRD_TransactionItem(transactionItems, trans);
                rs &= db.DeletePRD_Transaction(transaction, trans);


                if (rs.result == true)
                {
                    if (_trans == null) trans.Commit();
                    return new ResultStatus { message = "İşlem başarıyla silindi.", result = true, };
                }
                else
                {
                    if (_trans == null) trans.Rollback();
                    return new ResultStatus { message = "İşlem silinirken sorunlar oluştu.", result = false, };
                }

            }
            else
            {
                return new ResultStatus { message = "İşlem kaydı bulunamadı.", result = false, };
            }
        }
        private List<string> Validator(VWPRD_TransactionItem item)
        {
            var errorList = new List<string>();
            var product = this.products.Where(a => a.id == item.productId).FirstOrDefault();
            var type = (EnumPRD_TransactionType)this.type;
            var serialCodes = (item.serialCodes ?? "").Split(',');
            if (serialCodes.Count(a => a != "" && a != null) != item.quantity)
            {
                errorList.Add(string.Format("{0} ürünü için girilen serinumarası miktarı {1} ile girilen miktar {2} uyuşmamaktadır.", product.fullName, serialCodes.Count(), item.quantity));
            }

            var maintanceInventory = new List<string>();
            var unvalidInventory = new List<string>();
            var existInventory = new List<string>();
            var unkownInventory = new List<string>();
            foreach (var serialcode in serialCodes)
            {
                var winventory = this.inventories.Where(a => a.productId == item.productId && a.serialcode == serialcode).FirstOrDefault();

                //Kontroller Yapılacak duruma göre

                if (winventory != null && winventory.firstActionType == (int)EnumPRD_InventoryActionType.BakimEnvanteri)
                {
                    maintanceInventory.Add(winventory.serialcode);
                }


                switch (type)
                {
                    case EnumPRD_TransactionType.GelenIrsaliye:
                    case EnumPRD_TransactionType.Kiralama:
                        if (winventory != null && winventory.lastActionDataCompanyId != this.outputCompanyId) unvalidInventory.Add(serialcode);
                        break;
                    case EnumPRD_TransactionType.ZimmetAlma:
                        if (winventory == null) unkownInventory.Add(serialcode);
                        if (winventory != null && winventory.lastActionDataId != this.outputId) unvalidInventory.Add(serialcode);
                        break;
                    case EnumPRD_TransactionType.AcilisFisi:
                    case EnumPRD_TransactionType.UretimFisi:
                    case EnumPRD_TransactionType.SayimFazlasi:
                        if (winventory != null && winventory.lastActionDataId != this.outputId) existInventory.Add(serialcode);
                        break;
                    case EnumPRD_TransactionType.GidenIrsaliye:
                    case EnumPRD_TransactionType.ZimmetVerme:
                    case EnumPRD_TransactionType.KiralayaVerme:
                    case EnumPRD_TransactionType.SarfFisi:
                    case EnumPRD_TransactionType.FireFisi:
                    case EnumPRD_TransactionType.SayimEksigi:
                    case EnumPRD_TransactionType.Transfer:
                        if (winventory == null) unvalidInventory.Add(serialcode);
                        if (winventory != null && winventory.lastActionDataId != this.outputId) unvalidInventory.Add(serialcode);
                        break;
                    default:
                        break;
                }

            }

            if (existInventory.Count() > 0)
            {
                errorList.Add(string.Format("<strong>{0}</strong> ürünü için <strong>{1}</strong> girilen serinumaraları  <strong>{2}</strong> zaten farklı carilerde kayıtlı.", product.fullName, string.Join(",", existInventory), this.outputId_Title));
            }

            if (unvalidInventory.Count() > 0)
            {
                errorList.Add(string.Format("<strong>{0}</strong> ürünü için <strong>{1}</strong> girilen serinumaraları  <strong>{2}</strong> çıkış carisinde bulunmamaktadır.", product.fullName, string.Join(",", unvalidInventory), this.outputId_Title));
            }

            if (unkownInventory.Count() > 0)
            {
                errorList.Add(string.Format("<strong>{0}</strong> ürünü için <strong>{1}</strong> girilen serinumaraları stoklarda bulunmamaktadır. Sadece stoklarda bulunan serinumaraları ile işlem yapabilirsiniz.", product.fullName, string.Join(",", unkownInventory)));
            }

            if (maintanceInventory.Count() > 0)
            {
                errorList.Add(string.Format("<strong>{0}</strong> ürünü için <strong>{1}</strong> girilen serinumaraları bakım envanteri olarak kayıtlıdır.Bu envanterler üzerinde işlem yapamazsınız.", product.fullName, string.Join(",", maintanceInventory)));
            }
            return errorList;
        }
        public void EmailSender()
        {
            var warehouseSuperVisor = new List<VWSH_User>();
            var storage = new List<VWCMP_Storage>();
            if (this.inputId.HasValue && this.inputTable == "CMP_Storage")
            {
                var input = db.GetVWCMP_StorageById(this.inputId.Value);
                storage.Add(input);
                if (input != null && input.supervisorId.HasValue)
                {
                    warehouseSuperVisor.Add(db.GetVWSH_UserById(input.supervisorId.Value));
                }
            }
            if (this.outputId.HasValue && this.outputTable == "CMP_Storage")
            {
                var output = db.GetVWCMP_StorageById(this.outputId.Value);
                storage.Add(output);
                if (output != null && output.supervisorId.HasValue)
                {
                    warehouseSuperVisor.Add(db.GetVWSH_UserById(output.supervisorId.Value));
                }
            }

            string url = TenantConfig.Tenant.GetWebUrl();
            var tenantName = TenantConfig.Tenant.TenantName;

            var productTable = string.Format($"<table id=\"sub\" cellpadding=\"0\" cellspacing =\"0\" style=\"width:100% !important;margin-bottom: 20px;\">"
                          + "<tr>"
                          + "<td style =\"padding-left:5px;padding-right:5px;border-color:gray;text-align:left;border-style:solid; border-width:1px 1px 1px 1px;background:#bbb;\"> ÜRÜN </td>"
                           + "<td style =\"padding-left:5px;padding-right:5px;border-color:gray;text-align:left;border-style:solid; border-width:1px 1px 1px 1px;background:#bbb;\"> SERİ NUMARALARI	</td>"
                            + "<td style =\"padding-left:5px;padding-right:5px;border-color:gray;text-align:left;border-style:solid; border-width:1px 1px 1px 1px;background:#bbb;\"> MİKTAR  </td>"
                         + "</tr>");
            foreach (var product in this.products)
            {
                var serialCode = this.items.Where(x => x.productId == product.id).Select(x => x.serialCodes).FirstOrDefault();
                var quantity = this.items.Where(x => x.productId == product.id).Select(x => x.quantity).FirstOrDefault();
                productTable += string.Format($"<tr>"
                                 + " <td style =\"padding-left:5px;padding-right:5px;border-color:gray;border-style:solid;border-width:0px 1px 1px 1px;word-break:break-word!important;\">{0}</td>"
                                 + " <td style =\"padding-left:5px;padding-right:5px;border-color:gray;border-style:solid;border-width:0px 1px 1px 1px;word-break:break-word!important;\">{1}</td>"
                                 + " <td style =\"padding-left:5px;padding-right:5px;border-color:gray;border-style:solid;border-width:0px 1px 1px 1px;word-break:break-word!important;\">{2}</td>"
                                + "</tr>", product.fullName, serialCode != null ? serialCode : "-", quantity + " " + product.unitId_Title);
            }
            productTable += "</table>";
            var notifaction = new Notification();
            if (this.type == (int)EnumPRD_TransactionType.ZimmetVerme)
            {
                var user = db.GetSH_UserById(this.inputId.Value);

                if (user != null && !string.IsNullOrEmpty(user.email))
                {
                    var personName = user.firstname + " " + user.lastname;
                    if (this.status == (int)EnumPRD_TransactionStatus.islendi)
                    {

                        var notify = string.Format("Merhaba {0}, envanter(ler) üstünüze zimmetlenmiştir.", personName);
                        var contentUser = string.Format(@"<h3>Merhaba {0},</h3> " + "{1} tarihinde "
                        + "aşağıda bulunan envanter(ler) tarafınıza zimmetlenmiştir."
                        + "<br></br> Envanter(ler)in zimmetini <a href='{2}/PRD/VWPRD_Transaction/Detail?id={3}'> buraya tıklayarak </a> kontrol edebilirsiniz."
                        + "<br></br>Bu envanter(ler)i teslim almadıysanız depo sorumlusu ile iletişime geçiniz.<br></br>"
                        + productTable
                        + "<br></br> Bilgilerinize.<br></br> İyi Çalışmalar.", personName, String.Format("{0:dd/MM/yyyy HH:mm}", this.date), url, this.id);
                        new Email().Template("Template1", "personelzimmet.jpg", "Personel Zimmet Verme İşlemi", contentUser)
                        .Send((Int16)EmailSendTypes.Zimmet, user.email, tenantName + " | WORKOFTIME | Personel Zimmet Verme İşlemi", true);
                        notifaction.NotificationSend(user.id, "Personel Zimmet Verme İşlemi", notify);
                    }

                    var title = this.status == (int)EnumPRD_TransactionStatus.beklemede ? "Onayınızı Bekleyen Personel Zimmet Verme İşlemi" : "Personel Zimmet Verme İşlemi";
                    if (warehouseSuperVisor.Count() > 0)
                    {
                        var fullName = warehouseSuperVisor.Select(x => x.FullName).FirstOrDefault();
                        var mail = warehouseSuperVisor.Select(x => x.email).FirstOrDefault();
                        var id = warehouseSuperVisor.Select(x => x.id).FirstOrDefault();
                        var notify = "Onayınızı bekleyen zimmet verme işlemi var";
                        var contentSuperVisor = string.Format(@"<h3>Merhaba {0},</h3> " + "{1} tarihinde <span style=\"font-weight:bold\">{2}</span> içerisinde bulunan aşağı tablodaki envanter(ler) <span style=\"font-weight:bold\">{3}</span> adlı personele zimmetlenmiştir."
                          + "<br></br> Envanterin zimmetini <a href='{4}/PRD/VWPRD_Transaction/Detail?id={5}'> buraya tıklayarak </a> kontrol edebilirsiniz.<br></br>"
                          + productTable
                          + "<br> </br> Bilgilerinize.<br></br> İyi Çalışmalar.", fullName, String.Format("{0:dd/MM/yyyy HH:mm}", this.date), this.outputId_Title, personName, url, this.id);
                        notifaction.NotificationSend(id, "Personel Zimmet Verme İşlemi", notify);
                        new Email().Template("Template1", "personelzimmet.jpg", title, contentSuperVisor)
                            .Send((Int16)EmailSendTypes.Zimmet, mail, tenantName + " | WORKOFTIME | Personel Zimmet Verme İşlemi", true);
                    }
                }
            }
            else if (this.type == (int)EnumPRD_TransactionType.ZimmetAlma)
            {
                var user = db.GetSH_UserById(this.outputId.Value);

                if (user != null && !string.IsNullOrEmpty(user.email))
                {
                    if (this.status == (int)EnumPRD_TransactionStatus.islendi)
                    {
                        var personName = user.firstname + " " + user.lastname;
                        var contentUser = string.Format(@"<h3>Merhaba {0},</h3> " + "{1} tarihinde "
                       + "aşağıda bulunan envanter(ler) tarafınıza zimmetinizden çıkarılmıştır."
                        + "<br></br> Envanterin zimmetini <a href='{2}/PRD/VWPRD_Transaction/Detail?id={3}'> buraya tıklayarak </a> kontrol edebilirsiniz."
                        + " Bu envanter(ler) hala üzerinize zimmetli ise depo sorumlusu ile iletişime geçiniz.<br></br>"
                        + productTable
                        + "<br> </br> Bilgilerinize.<br></br> İyi Çalışmalar.", personName, String.Format("{0:dd/MM/yyyy HH:mm}", this.date), url, this.id);
                        var notify = "Zimmetinizden çıkartılan envanter(ler) var";
                        notifaction.NotificationSend(user.id, "Personel Zimmet Alma İşlemi", notify);
                        new Email().Template("Template1", "personelzimmet.jpg", "Personel Zimmet Alma İşlemi", contentUser)
                            .Send((Int16)EmailSendTypes.Zimmet, user.email, tenantName + " | WORKOFTIME | Personel Zimmet Alma İşlemi", true);
                    }

                    var title = this.status == (int)EnumPRD_TransactionStatus.beklemede ? "Onayınızı Bekleyen Personel Zimmet Alma İşlemi" : "Personel Zimmet Alma İşlemi";
                    if (warehouseSuperVisor != null)
                    {
                        var notify = "Depoya kaldırılan envanter(ler) var";
                        var fullName = warehouseSuperVisor.Select(x => x.FullName).FirstOrDefault();
                        var mail = warehouseSuperVisor.Select(x => x.email).FirstOrDefault();
                        var id = warehouseSuperVisor.Select(x => x.id).FirstOrDefault();
                        var contentSuperVisor = string.Format(@"<h3>Merhaba {0},</h3>" + "{1} tarihinde "
                         + "asağı tabloda bulunan envanter(ler) {2} kişisinden zimmeti alınarak   <span style=\"font-weight:bold\">{3}</span> adlı deponuza eklenmiştir."
                         + "<br></br> Envanterin zimmetini <a href='{4}/PRD/VWPRD_Transaction/Detail?id={5}'> buraya tıklayarak </a> kontrol edebilirsiniz."
                         + "<br></br> Deponuzun durumunu <a href='{6}/CMP/VWCMP_Storage/Detail?id={7}'> buraya tıklayarak </a> kontrol edebilirsiniz.<br></br>"
                           + productTable
                         + "<br> </br> Bilgilerinize.<br></br> İyi Çalışmalar.", fullName, String.Format("{0:dd/MM/yyyy HH:mm}", this.date != null ? this.date : DateTime.Now), this.outputId_Title, this.inputId_Title, url, this.id, url, this.inputId);
                        notifaction.NotificationSend(id, "Personel Zimmet Alma İşlemi", notify);
                        new Email().Template("Template1", "personelzimmet.jpg", title, contentSuperVisor)
                            .Send((Int16)EmailSendTypes.Zimmet, mail, tenantName + " | WORKOFTIME | Personel Zimmet Alma İşlemi", true);
                    }
                }
            }
            else
            {
                var operationName = EnumsProperties.GetDescriptionFromEnumValue((EnumPRD_TransactionType)this.type);
                if (warehouseSuperVisor.Count() > 0 && this.type != (int)EnumPRD_TransactionType.Transfer)
                {
                    var fullName = warehouseSuperVisor.Select(x => x.FullName).FirstOrDefault();
                    var mail = warehouseSuperVisor.Select(x => x.email).FirstOrDefault();
                    var contentUser = "<h3>Merhaba {0},</h3> "
                                  + "Sorumlusu olduğunuz depo ile ilgili <strong>{1}</strong> tarihinde aşağıda bulunan tablodaki envanter(ler) ile ilgili olarak <strong>{2}</strong>  tarafından <strong>{3} işlemi</strong> gerçekleştirilmiştir."
                                  + "<br></br> Stok işlemini dilerseniz <a href='{4}/PRD/VWPRD_Transaction/Detail?id={5}'> buraya tıklayarak </a>kontrol edebilirsiniz.";
                    if (this.status == (int)EnumPRD_TransactionStatus.beklemede)
                    {
                        contentUser += "<br></br> Transfer Fişi ile ilgili onay işleminizi gerçekleştirmeniz gerekmektedir.";
                    }
                    contentUser += "<br></br>"
                               + productTable
                               + "<br> </br> Bilgilerinize.<br></br> İyi Çalışmalar.";
                    contentUser = string.Format(contentUser, fullName, String.Format("{0:dd/MM/yyyy HH:mm}", this.date), this.createdby_Title, operationName, url, this.id);

                    new Email().Template("Template1", "depozimmet.jpg", operationName + " İşlemi", contentUser)
                    .Send((Int16)EmailSendTypes.Zimmet, mail, tenantName + " | WORKOFTIME | " + operationName + " İşlemi", true);
                }
                else if (warehouseSuperVisor.Count() > 0 && this.type == (int)EnumPRD_TransactionType.Transfer)
                {

                    foreach (var item in warehouseSuperVisor)
                    {
                        var fullName = item.FullName;
                        var mail = item.email;
                        var inputStorage = storage.Where(x => x.id == inputId).Select(x => x.fullName).FirstOrDefault();
                        var outputStorage = storage.Where(x => x.id == outputId).Select(x => x.fullName).FirstOrDefault();
                        var outputStoragePerson = storage.Where(x => x.id == outputId).Select(x => x.supervisorId).FirstOrDefault();

                        if (this.createdby.HasValue)
                        {
                            var user = db.GetSH_UserById(this.createdby.Value); ;
                            this.createdby_Title = user.firstname + " " + user.lastname;
                        }

                        var storages = string.Format($"<table id=\"sub\" cellpadding=\"0\" cellspacing =\"0\" style=\"width:100% !important;margin-bottom: 20px;\">"
                         + "<tr>"
                         + "<td style =\"padding-left:5px;padding-right:5px;border-color:gray;text-align:left;border-style:solid; border-width:1px 1px 1px 1px;background:#bbb;\"> Çıkış Yapılacak Depo </td>"
                         + "<td style =\"padding-left:5px;padding-right:5px;border-color:gray;text-align:left;border-style:solid; border-width:1px 1px 1px 1px;background:#bbb;\"> Giriş Yapılacak Depo </td>"
                         + "</tr>"
                         + "<tr>"
                         + " <td style =\"padding-left:5px;padding-right:5px;border-color:gray;border-style:solid;border-width:0px 1px 1px 1px;word-break:break-word!important;\">{0}</td>"
                         + " <td style =\"padding-left:5px;padding-right:5px;border-color:gray;border-style:solid;border-width:0px 1px 1px 1px;word-break:break-word!important;\">{1}</td>"
                         + "</tr>"
                         + "</table>", outputStorage, inputStorage);

                        var contentUser = "<h3>Merhaba {0},</h3> "
                                   + "Sorumlusu olduğunuz depo ile ilgili <strong>{1}</strong> tarihinde aşağıda bulunan tablodaki envanter(ler) ile ilgili olarak <strong>{2}</strong>  tarafından <strong>{3} işlemi</strong> gerçekleştirilmiştir."
                                   + "<br></br> Stok işlemini dilerseniz <a href='{4}/PRD/VWPRD_Transaction/Detail?id={5}'> buraya tıklayarak </a>kontrol edebilirsiniz.";
                        if (outputStoragePerson != null && this.status == (int)EnumPRD_TransactionStatus.beklemede)
                        {
                            if (item.id == outputStoragePerson)
                            {
                                contentUser += "<br></br> Transfer Fişi ile ilgili onay işleminizi gerçekleştirmeniz gerekmektedir.";
                            }
                        }
                        contentUser += "<br></br>"
                                   + productTable + "</br>"
                                   + "<h4>Depo İşlem Bilgileri</h4>"
                                   + storages
                                   + "<br> </br> Bilgilerinize.<br></br> İyi Çalışmalar.";
                        contentUser = string.Format(contentUser, fullName, String.Format("{0:dd/MM/yyyy HH:mm}", this.date), this.createdby_Title, operationName, url, this.id);

                        new Email().Template("Template1", "depozimmet.jpg", operationName + " İşlemi", contentUser)
                        .Send((Int16)EmailSendTypes.Zimmet, mail, tenantName + " | WORKOFTIME | " + operationName + " İşlemi", true);
                    }
                }
            }
        }
        private EnumPRD_InventoryActionType? GetInputType(EnumPRD_TransactionType type)
        {
            switch (type)
            {
                case EnumPRD_TransactionType.GelenIrsaliye:
                    return EnumPRD_InventoryActionType.Depoda;
                case EnumPRD_TransactionType.ZimmetAlma:
                    return EnumPRD_InventoryActionType.Depoda;
                case EnumPRD_TransactionType.Kiralama:
                    return EnumPRD_InventoryActionType.Depoda;
                case EnumPRD_TransactionType.AcilisFisi:
                    return EnumPRD_InventoryActionType.Depoda;
                case EnumPRD_TransactionType.UretimFisi:
                    return EnumPRD_InventoryActionType.Depoda;
                case EnumPRD_TransactionType.SayimFazlasi:
                    return EnumPRD_InventoryActionType.Depoda;



                case EnumPRD_TransactionType.GidenIrsaliye:
                    return EnumPRD_InventoryActionType.CikisYapildi;
                case EnumPRD_TransactionType.ZimmetVerme:
                    return EnumPRD_InventoryActionType.Personelde;
                case EnumPRD_TransactionType.KiralayaVerme:
                    return EnumPRD_InventoryActionType.KirayaVerildi;
                case EnumPRD_TransactionType.SarfFisi:
                    return EnumPRD_InventoryActionType.Harcandi;
                case EnumPRD_TransactionType.FireFisi:
                    return EnumPRD_InventoryActionType.Harcandi;
                case EnumPRD_TransactionType.SayimEksigi:
                    return EnumPRD_InventoryActionType.Kayboldu;
                case EnumPRD_TransactionType.Transfer:
                    return EnumPRD_InventoryActionType.Depoda;
                case EnumPRD_TransactionType.HarcamaBildirimi:
                    return EnumPRD_InventoryActionType.Harcandi;
                case EnumPRD_TransactionType.UretimBildirimi:
                    return EnumPRD_InventoryActionType.Uretildi;
                default:
                    return null;
            }
        }
        private EnumPRD_InventoryActionType? GetOutputType(EnumPRD_TransactionType type)
        {
            switch (type)
            {
                case EnumPRD_TransactionType.GelenIrsaliye:
                    return EnumPRD_InventoryActionType.IrsaliyeliGiris;
                case EnumPRD_TransactionType.ZimmetAlma:
                    return EnumPRD_InventoryActionType.ZimmetIade;
                case EnumPRD_TransactionType.Kiralama:
                    return EnumPRD_InventoryActionType.Kiralandi;
                case EnumPRD_TransactionType.AcilisFisi:
                    return EnumPRD_InventoryActionType.StokGiris;
                case EnumPRD_TransactionType.UretimFisi:
                    return EnumPRD_InventoryActionType.Uretildi;
                case EnumPRD_TransactionType.SayimFazlasi:
                    return EnumPRD_InventoryActionType.SayimFazlasi;



                case EnumPRD_TransactionType.GidenIrsaliye:
                    return EnumPRD_InventoryActionType.IrsaliyeliCikis;
                case EnumPRD_TransactionType.ZimmetVerme:
                    return EnumPRD_InventoryActionType.ZimmetVerildi;
                case EnumPRD_TransactionType.KiralayaVerme:
                    return EnumPRD_InventoryActionType.StokCikis;
                case EnumPRD_TransactionType.SarfFisi:
                    return EnumPRD_InventoryActionType.StokCikis;
                case EnumPRD_TransactionType.FireFisi:
                    return EnumPRD_InventoryActionType.StokCikis;
                case EnumPRD_TransactionType.SayimEksigi:
                    return EnumPRD_InventoryActionType.SayimEksigi;
                case EnumPRD_TransactionType.Transfer:
                    return EnumPRD_InventoryActionType.Transfer;
                case EnumPRD_TransactionType.HarcamaBildirimi:
                    return EnumPRD_InventoryActionType.Harcandi;
                case EnumPRD_TransactionType.UretimBildirimi:
                    return EnumPRD_InventoryActionType.Uretildi;  
                default:
                    return null;
            }
        }
        private OwnerInfo GetInfo(Guid? dataId, string dataTable, Guid? dataCompanyId)
        {
            OwnerInfo result = new OwnerInfo();
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
                    case "CMP_CompanyCars":
                        var cmpCars = db.GetVWCMP_CompanyCarsById(dataId.Value);
                        result = new OwnerInfo
                        {
                            CompanyId = cmpCars?.companyId,
                            CompanyIdTitle = cmpCars?.companyId_Title,
                            Location = null,
                            Text = cmpCars?.name,
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
    }
}