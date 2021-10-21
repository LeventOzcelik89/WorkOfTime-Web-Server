using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Web;

namespace Infoline.WorkOfTime.BusinessAccess
{
    public class VMCMP_StorageCompany : VWCMP_Storage
    {
        public VWCMP_Company CMPCompany { get; set; }
        public string breadCrumps { get; set; }
    }

    public class VWCMP_CompanyDetail : VWCMP_Company
    {
        public int storagesCount { get; set; }
        public int personalsCount { get; set; }
        public int storageCount { get; set; }
        public int tenderCount { get; set; }
        public int invoiceSellingCount { get; set; }
        public int invoiceBuyingCount { get; set; }
        public double invoiceSellingAmount { get; set; }
        public double inoviceBuyingAmount { get; set; }
        public VWCMP_Sector[] sectorList { get; set; }
        public VWCMP_CompanyType[] companyTypeList { get; set; }

    }


    public class VMCMP_CompanyModel : VWCMP_Company
    {
        private WorkOfTimeDatabase db { get; set; }
        private DbTransaction trans { get; set; }
        public VWPA_Account[] VWPA_Accounts { get; set; }
        public Guid[] sector { get; set; }
        public VWSH_User user { get; set; }
        public EnumCMP_CompanyType[] types { get; set; }
        public string breadCrumps { get; set; }

        public Guid[] CMP_TypeIds { get; set; }

        public CMP_Types[] CMP_Types { get; set; }
        public bool? hasRelation { get; set; }


        public VMCMP_CompanyModel Load()
        {
            this.db = this.db ?? new WorkOfTimeDatabase();
            var company = db.GetVWCMP_CompanyById(this.id);

            if (company == null && this.code != null)
            {
                company = db.GetVWCMP_CompanyByCode(this.code);
            }

            if (company != null)
            {
                this.B_EntityDataCopyForMaterial(company, true);
                this.sector = db.GetCMP_SectorByCompanyId(this.id).Select(x => x.sectorId.Value).ToArray();
            }

            this.CMP_TypeIds = db.GetCMP_CompanyTypeByCompanyIdTypesIds(this.id);
            this.code = this.code ?? BusinessExtensions.B_GetIdCode();
            this.type = this.type ?? (Int32)EnumCMP_CompanyType.Diger;
            return this;
        }

        public VWCMP_CompanyDetail LoadCompanyDetail(Guid id)
        {
            this.db = this.db ?? new WorkOfTimeDatabase();
            var company = db.GetVWCMP_CompanyById(id);
            var companyDetail = new VWCMP_CompanyDetail();
            companyDetail = new VWCMP_CompanyDetail().B_EntityDataCopyForMaterial(company);
            var storagesBexp = new BEXP { Operand1 = (COL)"companyId", Operator = BinaryOperator.Equal, Operand2 = (VAL)companyDetail.id };
            var personalBexp = new BEXP { Operand1 = (COL)"companyId", Operator = BinaryOperator.Equal, Operand2 = (VAL)companyDetail.id };
            var storageBexp = new BEXP { Operand1 = (COL)"pid", Operator = BinaryOperator.Equal, Operand2 = (VAL)companyDetail.id };
            var tenderBexp = new BEXP { Operand1 = (COL)"customerId", Operator = BinaryOperator.Equal, Operand2 = (VAL)companyDetail.id };
            var invoiceSellingBexp = new BEXP { Operand1 = (COL)"customerId", Operator = BinaryOperator.Equal, Operand2 = (VAL)companyDetail.id };
            var invoiceBuyingBexp = new BEXP { Operand1 = (COL)"supplierId", Operator = BinaryOperator.Equal, Operand2 = (VAL)companyDetail.id };
            var storagesCount = db.GetCMP_StorageCount(storagesBexp);
            var personalCount = db.GetVWSH_UserCount(personalBexp);
            var storageCount = db.GetCMP_StorageCount(storageBexp);
            var tenderCount = db.GetVWCMP_TenderCount(tenderBexp);
            var invoiceSellingCount = db.GetVWCMP_InvoiceCount(invoiceSellingBexp);
            var invoiceBuyingCount = db.GetVWCMP_InvoiceCount(invoiceBuyingBexp);
            var invoiceSellingAmount = db.GetCMP_CompanyTenderAmountByCustomerId(companyDetail.id, (int)EnumCMP_InvoiceDirectionType.Alis);
            var invoiceBuyingAmount = db.GetCMP_CompanyInvoiceAmountBySuplierId(companyDetail.id, (int)EnumCMP_InvoiceDirectionType.Alis);
            var sectors = db.GetVWCMP_SectorItemByCompanyId(id);
            var companyTypeList = db.GetVWCMP_CompanyTypeByCompanyId(id);
            companyDetail.storagesCount = storagesCount;
            companyDetail.personalsCount = personalCount;
            companyDetail.storageCount = storageCount;
            companyDetail.tenderCount = tenderCount;
            companyDetail.invoiceBuyingCount = invoiceBuyingCount;
            companyDetail.invoiceSellingCount = invoiceSellingCount;
            companyDetail.invoiceSellingAmount = invoiceSellingAmount;
            companyDetail.inoviceBuyingAmount = invoiceBuyingAmount;
            companyDetail.sectorList = sectors;
            companyDetail.companyTypeList = companyTypeList;
            return companyDetail;
        }

        public ResultStatus Save(Guid userid, HttpRequestBase request = null, DbTransaction trans = null)
        {
            db = db ?? new WorkOfTimeDatabase();
            var company = db.GetVWCMP_CompanyById(this.id);
            var res = new ResultStatus { result = true };


            if (company == null)
            {
                this.createdby = userid;
                this.created = DateTime.Now;
                res = Insert(trans);
            }
            else
            {
                this.changedby = userid;
                this.changed = DateTime.Now;
                res = Update(trans);
            }
            if (request != null && res.result)
            {
                new FileUploadSave(request, this.id).SaveAs();
                new TableAdditionalPropertySave(request, this.id, "CMP_Company").SaveAs(this.changedby ?? this.createdby);
            }
            return res;
        }

        private ResultStatus Insert(DbTransaction trans = null)
        {
            var transaction = trans ?? db.BeginTransaction();
            var dbresult = db.InsertCMP_Company(new CMP_Company().B_EntityDataCopyForMaterial(this), this.trans);

            dbresult &= db.InsertCMP_Storage(new CMP_Storage
            {
                id = Guid.NewGuid(),
                created = DateTime.Now,
                createdby = this.createdby,
                code = BusinessExtensions.B_GetIdCode(),
                name = "Ana Şube/Depo/Kısım",
                location = this.location,
                locationId = this.openAddressLocationId,
                companyId = this.id,
                address = this.openAddress
            }, this.trans);

            if (this.sector != null)
            {
                dbresult &= db.BulkInsertCMP_Sector(this.sector.Select(a => new CMP_Sector
                {
                    createdby = this.createdby,
                    companyId = this.id,
                    sectorId = a,
                }), this.trans);
            }

            if (this.CMP_TypeIds != null && this.CMP_TypeIds.Count() > 0)
            {
                var companyKeyList = this.CMP_TypeIds.Select(x => new CMP_CompanyType
                {
                    created = DateTime.Now,
                    createdby = this.createdby,
                    typesId = x,
                    companyId = this.id
                }).ToList();

                dbresult &= db.BulkInsertCMP_CompanyType(companyKeyList, trans);
            }

            var currencies = db.GetUT_Currency();
            var TL = currencies.Where(a => a.code == "TL").FirstOrDefault();
            var account = new PA_Account()
            {
                created = this.created,
                createdby = this.createdby,
                code = BusinessExtensions.B_GetIdCode(),
                currencyId = TL.id,
                isDefault = true,
                name = "Ana Hesap",
                status = true,
                type = (int)EnumPA_AccountType.Kasa,
                dataId = this.id,
                dataTable = "CMP_Company"
            };
            dbresult &= db.InsertPA_Account(account, this.trans);

            if (!dbresult.result)
            {
                if (trans == null) transaction.Rollback();

                return new ResultStatus
                {
                    result = false,
                    message = "İşletme kaydetme işlemi başarısız oldu."
                };
            }
            else
            {
                if (trans == null) transaction.Commit();

                if (this.user != null && this.user.firstname != null && this.user.lastname != null)
                {
                    this.user.CompanyId = this.id;
                    var rs = new VMSH_UserModel().B_EntityDataCopyForMaterial(this.user).Save();
                }

                return new ResultStatus
                {
                    result = true,
                    message = "İşletme kaydetme işlemi başarılı bir şekilde gerçekleştirildi."
                };
            }
        }
        private ResultStatus Update(DbTransaction trans = null)
        {
            var dbresult = new ResultStatus { result = true };
            var transaction = trans ?? db.BeginTransaction();

            var _sectors = db.GetCMP_SectorByCompanyId(this.id);
            if (_sectors != null && _sectors.Count() > 0)
            {
                dbresult &= db.BulkDeleteCMP_Sector(_sectors, this.trans);
            }

            if (this.sector != null)
            {
                dbresult &= db.BulkInsertCMP_Sector(this.sector.Select(a => new CMP_Sector
                {
                    createdby = this.createdby,
                    companyId = this.id,
                    sectorId = a,
                }), this.trans);
            }

            if (this.types != null)
            {
                this.type = this.types.Sum(a => Convert.ToInt32(a));
            }

            dbresult &= db.UpdateCMP_Company(new CMP_Company().B_EntityDataCopyForMaterial(this), false, this.trans);

            var storage = db.GetCMP_StorageByCompanyIdFirst(this.id);
            if (storage != null)
            {
                if (!storage.locationId.HasValue)
                {
                    storage.locationId = this.openAddressLocationId;
                    storage.location = this.location;

                    dbresult &= db.UpdateCMP_Storage(storage, true, this.trans);
                }
            }


            var data = db.GetCMP_CompanyTypeByCompanyId(this.id).ToList();
            dbresult &= db.BulkDeleteCMP_CompanyType(data);

            if (this.CMP_TypeIds != null && this.CMP_TypeIds.Count() > 0)
            {
                var companyKeyList = this.CMP_TypeIds.Select(x => new CMP_CompanyType
                {
                    created = DateTime.Now,
                    createdby = this.createdby,
                    typesId = x,
                    companyId = this.id
                }).ToList();

                dbresult &= db.BulkInsertCMP_CompanyType(companyKeyList, trans);
            }


            if (!dbresult.result)
            {
                if (trans == null) transaction.Rollback();

                return new ResultStatus
                {
                    result = false,
                    message = "İşletme güncelleme işlemi başarısız oldu."
                };
            }
            else
            {
                if (trans == null) transaction.Commit();
                return new ResultStatus
                {
                    result = true,
                    message = "İşletme güncelleme işlemi başarılı şekilde gerçekleştirildi."
                };
            }
        }

        public ResultStatus UpsertStorage(IEnumerable<VWCMP_Storage> items)
        {
            var insertList = new List<CMP_Storage>();
            var updateList = new List<CMP_Storage>();

            var culture = new System.Globalization.CultureInfo("tr-TR", false);

            foreach (var item in items)
            {
                if (!String.IsNullOrEmpty(item.supervisorId_Title))
                {
                    var companyPerson = db.GetVWSH_UserByCompanyId(this.id).Where(a => a.FullName.Trim().ToLower(culture) == item.supervisorId_Title.Trim().ToLower(culture)).FirstOrDefault();

                    if (companyPerson != null)
                    {
                        item.supervisorId = user.id;
                    }
                }

                var storageDb = db.GetVWCMP_StorageByCode(item.code);

                var storage = new CMP_Storage().B_EntityDataCopyForMaterial(item, true);

                if (storageDb != null)
                    updateList.Add(storage);
                else
                    insertList.Add(storage);
            }

            var trans = db.BeginTransaction();

            var rs = db.BulkInsertCMP_Storage(insertList.ToArray(), trans);
            rs &= db.BulkUpdateCMP_Storage(updateList.ToArray(), false, trans);

            if (rs.result)
                trans.Commit();
            else
                trans.Rollback();

            return rs;
        }

        public ResultStatus UpsertPerson(IEnumerable<VWSH_User> persons)
        {
            var errorList = new List<string>();

            var rs = new ResultStatus { result = true };

            foreach (var person in persons)
            {
                var trans = db.BeginTransaction();
                rs = new VMSH_UserModel().B_EntityDataCopyForMaterial(person, true).Save(trans);

                if (rs.result)
                {
                    trans.Commit();
                }
                else
                {
                    errorList.Add(person.firstname + " " + person.lastname + "(" + rs.message + ")");
                    trans.Rollback();
                }
            }

            string errors = "";
            if (errorList.Count() > 0)
            {
                errors = string.Join(", ", errorList);
            }

            return new ResultStatus { result = rs.result, message = errors };
        }

        public ResultStatus Delete(DbTransaction trans = null)
        {
            db = db ?? new WorkOfTimeDatabase();
            var _trans = trans ?? db.BeginTransaction();

            var company = db.GetCMP_CompanyById(this.id);
            var productCompany = db.GetPRD_ProductCompanyByCompanyId(this.id);
            var prd_transaction = db.GetPRD_TransactionByDataId(this.id);
            var isExistInvoice = db.GetCMP_InvoiceByCompanyId(this.id);
            var isExistProject = db.GetPRJ_ProjectByCompanyId(this.id);
            var isExistPresentation = db.GetCRM_PresentationByCompanyId(this.id);
            var isExistTask = db.GetFTM_TaskByCustomerId(this.id);
            var accounts = db.GetPA_AccountByDataId(this.id);
            var ledgerRelateds = db.GetPA_LedgerByRelatedAccountIds(accounts.Select(a => a.id).ToArray()).ToArray();
            var ledgers = db.GetPA_LedgerByAccountIds(accounts.Select(a => a.id).ToArray()).ToArray();

            if (productCompany.Count() > 0)
            {
                return new ResultStatus { result = false, message = "İşletmeye tanımlı ürünler olduğu için işletme silinemez." };
            }

            if (prd_transaction.Count() > 0)
            {
                return new ResultStatus { result = false, message = "Stok hareketi bulunan işletme silinemez." };
            }

            if (isExistInvoice.Count() > 0)
            {
                return new ResultStatus { result = false, message = "İşletmeye ait teklif, fatura veya sipariş olduğu için işletme silinemez." };
            }

            if (isExistProject.Count() > 0)
            {
                return new ResultStatus { result = false, message = "İşletmeye ait proje bulunduğu için işletme silinemez." };
            }

            if (isExistPresentation.Count() > 0)
            {
                return new ResultStatus { result = false, message = "İşletmeye ait potansiyel bulunduğu için işletme silinemez." };
            }

            if (isExistTask.Count() > 0)
            {
                return new ResultStatus { result = false, message = "İşletmenin müşteri olduğu görev bulunduğu için işletme silinemez." };
            }

            if (ledgerRelateds.Count() > 0 || ledgers.Count() > 0)
            {
                return new ResultStatus { result = false, message = "Hesap hareketi bulunan işletme silinemez." };
            }

            var storages = db.GetCMP_StroageByCompanyId(this.id);
            var transactions = db.GetPA_TransactionByAccountIds(accounts.Select(a => a.id).ToArray()).ToArray();

            var companyPersons = db.GetINV_CompanyPersonByCompanyId(this.id);
            var users = db.GetSH_UserByIds(companyPersons.Where(a => a.IdUser.HasValue).Select(a => a.IdUser.Value).ToArray());
            var roles = db.GetSH_UserRoleByUserIds(users.Select(a => a.id).ToArray());
            var companyTypes = db.GetCMP_CompanyTypeByCompanyId(this.id);




            var dbresult = db.BulkDeletePA_Transaction(transactions, _trans);
            dbresult &= db.BulkDeleteCMP_CompanyType(companyTypes, trans);
            dbresult &= db.BulkDeletePA_Account(accounts, _trans);
            dbresult &= db.BulkDeleteSH_UserRole(roles, _trans);
            dbresult &= db.BulkDeleteINV_CompanyPerson(companyPersons, _trans);
            dbresult &= db.BulkDeleteSH_User(users, _trans);
            dbresult &= db.BulkDeleteCMP_Storage(storages, _trans);
            dbresult &= db.DeleteCMP_Company(company, _trans);

            if (trans == null)
            {
                if (dbresult.result)
                    _trans.Commit();
                else
                    _trans.Rollback();
            }

            return dbresult;
        }
    }
}
