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
        public List<Guid?> PidIds { get; set; } = new List<Guid?>();
        public void GetAllChildsIds(Guid id)
        {
            var db = new WorkOfTimeDatabase();
            var findStorage = db.GetCMP_StorageFromPidById(id);
            if (findStorage.Length > 0)
            {
                this.PidIds.AddRange(findStorage.Select(x => (Guid?)x.id));
                foreach (var item in findStorage)
                {
                    GetAllChildsIds(item.id);
                }
            }
        }
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
        public string CmpTypeSearch { get; set; }
        public string applicant { get; set; }
        public VWSH_User companyUser { get; set; } = new VWSH_User();
        public Guid[] userIds { get; set; }


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
                var companyUser = db.GetINV_CompanyPersonByCompanyIdFirst(this.id);
                if (companyUser != null)
                {
                    var user = db.GetSH_UserById(companyUser.IdUser.Value);
                    if (user != null)
                    {
                        var userModel = new VMSH_UserModel { id = user.id }.Load();
                        this.companyUser = userModel;
                    }

                }
                this.B_EntityDataCopyForMaterial(company, true);
                this.sector = db.GetCMP_SectorByCompanyId(this.id).Select(x => x.sectorId.Value).ToArray();
            }
            if (string.IsNullOrEmpty(CmpTypeSearch))
            {
                this.CMP_TypeIds = db.GetCMP_CompanyTypeByCompanyIdTypesIds(this.id);
            }
            else
            {
                var searched = db.GetCMP_TypesLike(CmpTypeSearch);
                if (searched != null)
                {
                    this.CMP_TypeIds = searched.Select(x => x.id).ToArray();
                }
                else
                {
                    var id = Guid.NewGuid();
                    db.InsertCMP_Types(new BusinessData.CMP_Types
                    {
                        id = id,
                        createdby = Guid.Empty,
                        typeName = CmpTypeSearch
                    });
                    this.CMP_TypeIds = new Guid[] { id };
                }

            }
            this.userIds = db.GetCMP_CompanyManagingByCompanyIdUserIds(this.id);
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
            if (this.userIds != null && this.userIds.Count() > 0)
            {
                var userRoles = this.userIds.Select(x => new SH_UserRole {
                    created = DateTime.Now,
                    createdby = this.createdby,
                    userid = x,
                    roleid = Guid.Parse("00000000-0000-0000-0000-420000000000"),
                });
                dbresult &= db.BulkInsertSH_UserRole(userRoles,trans);
                var companyManaging = this.userIds.Select(x => new CMP_CompanyManaging
                {
                    created = DateTime.Now,
                    createdby = this.createdby,
                    userId = x,
                    companyId = this.id
                }).ToList();
                dbresult &= db.BulkInsertCMP_CompanyManaging(companyManaging, trans);
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

            var managingUsers = db.GetCMP_CompanyManagingByCompanyId(this.id).ToList();
            dbresult &= db.BulkDeleteCMP_CompanyManaging(managingUsers);

            if (this.userIds != null && this.userIds.Count() > 0)
            {
                foreach (var item in userIds)
                {
                    var findUserRoles = db.GetSH_UserRoleByUserIdRoleId(item, Guid.Parse("00000000-0000-0000-0000-420000000000"));
                    dbresult &= db.BulkDeleteSH_UserRole(findUserRoles);
                }
                var userRoles = this.userIds.Select(x => new SH_UserRole
                {
                    created = DateTime.Now,
                    createdby = this.createdby,
                    userid = x,
                    roleid = Guid.Parse("00000000-0000-0000-0000-420000000000"),
                });
                dbresult &= db.BulkInsertSH_UserRole(userRoles, trans);
                var companyManaging = this.userIds.Select(x => new CMP_CompanyManaging
                {
                    created = DateTime.Now,
                    createdby = this.createdby,
                    userId = x,
                    companyId = this.id
                }).ToList();

                dbresult &= db.BulkInsertCMP_CompanyManaging(companyManaging, trans);
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
        public ResultStatus InsertCustomerDealer()
        {
            db = db ?? new WorkOfTimeDatabase();
            VWCMP_Company company = null;
            if (this.code == null)
            {
                return new ResultStatus { message = "Bayi kodu boş geçilemez." };
            }
            company = db.GetCMP_CompanyByCodeBayi(this.code);
            if (company != null)
            {
                return new ResultStatus { message = "Bayi kodu sistemde bulunmaktadır.Lütfen başka bir kod ile işlemlerinize devam ediniz." };
            }
            if (this.taxNumber == null)
            {
                return new ResultStatus { message = "Vergi Numarası Boş Geçilemez.", result = false };
            }
            company = db.GetCMP_CompanyByTaxNumberBayi(this.taxNumber);
            if (company != null)
            {
                return new ResultStatus { message = "Vergi numarası sistemde bulunmaktadır.Lütfen başka bir vergi numarası ile işlemlerinize devam ediniz.", result = false };
            }
            var email = db.GetSH_UserByEmail(this.companyUser.email);
            if (email != null && email.Count() > 0)
            {
                return new ResultStatus { result = false, message = "Girmiş olduğunuz email adresi sistemde kullanılıyor.Lütfen başka bir email ile işleminize devam edin." };
            }
            this.CMP_TypeIds = new Guid[] { Guid.Parse("FCB3FA74-0C52-B122-065D-EB929108CBCC") };
            this.isActive = (int)EnumCMP_CompanyIsActive.Pasif;
            this.type = (short)EnumCMP_CompanyType.Diger;

            var result = this.Save(Guid.Parse("00000000-0000-0000-0000-000000000000"));
            var userModel = new VMSH_UserModel();
            userModel.CompanyId = this.id;
            userModel.loginname = this.companyUser.loginname;
            userModel.title = this.companyUser.title;
            userModel.firstname = this.companyUser.firstname;
            userModel.lastname = this.companyUser.lastname;
            userModel.phone = this.companyUser.phone;
            userModel.email = this.companyUser.email;
            userModel.companyCellPhone = this.companyUser.companyCellPhone;
            userModel.companyCellPhoneCode = this.companyUser.companyCellPhoneCode;
            userModel.companyOfficePhone = this.companyUser.companyOfficePhone;
            userModel.companyOfficePhoneCode = this.companyUser.companyOfficePhoneCode;
            userModel.birthday = this.companyUser.birthday;
            userModel.type = (int)EnumSH_UserType.CompanyPerson;
            userModel.Roles = new List<Guid> { new Guid(SHRoles.HakEdisBayiPersoneli) };
            result &= userModel.Save();
            if (result.result)
            {

                SendFirstCustomerMail(userModel);
                var Iks = db.GetVWSH_UserByRoleId(SHRoles.IKYonetici);
                if (Iks != null && Iks.Count() > 0)
                {
                    foreach (var item in Iks)
                    {
                        SendFirstCustomerMailToIK(item, this.name, userModel);
                    }
                }
            }
            return result;
        }
        private void SendFirstCustomerMail(VWSH_User user)
        {
            db = db ?? new WorkOfTimeDatabase();

            string url = TenantConfig.Tenant.GetWebUrl();
            var tenantName = TenantConfig.Tenant.TenantName;
            var mesajIcerigi = $"<h3>Merhaba!</h3> <p>{tenantName} |  WorkOfTime sistemi üzerinde kayıt isteğiniz başarıyla alınmıştır. Süreciniz onaylandığı zaman sizi tekrar bilgilendireceğiz</p>";
            new Email().Template("Template1", "userMailFoto.jpg", "Bayi Kayıt İsteği", mesajIcerigi)
                      .Send((Int16)EmailSendTypes.ZorunluMailler, user.email, string.Format("{0} | {1}", tenantName, "Bayi Kayıt İsteği"), true);
        }

        private void SendFirstCustomerMailToIK(VWSH_User user, string companyName, VWSH_User companyUser)
        {
            db = db ?? new WorkOfTimeDatabase();

            string url = TenantConfig.Tenant.GetWebUrl();
            var tenantName = TenantConfig.Tenant.TenantName;
            var mesajIcerigi = $"<h3>Sayın {user.FullName},</h3></br> <p>{tenantName} | WorkOfTime sistemi üzerinde bayi üyelik başvurusu yapmıştır.</p> <p>" +
                 $"<b>{this.companyUser.firstname} {this.companyUser.firstname}</b></p>" +
                $"<b>{companyName}</b></p>" +
                 $"<p> Detaylar için <a href='{url}/SH/VWSH_User/CompanyPersonIndex?userId={companyUser.id}'> Buraya tıklayınız!</a></p>";

            new Email().Template("Template1", "userMailFoto.jpg", "Bayi Kayıt İsteği", mesajIcerigi)
                      .Send((Int16)EmailSendTypes.ZorunluMailler, this.email, string.Format("{0} | {1}", tenantName, "Bayi Kayıt İsteği"), true);
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
            var companyManaging = db.GetCMP_CompanyManagingByCompanyId(this.id);


            var dbresult = db.BulkDeletePA_Transaction(transactions, _trans);
            dbresult &= db.BulkDeleteCMP_CompanyType(companyTypes, trans);
            dbresult &= db.BulkDeleteCMP_CompanyManaging(companyManaging, trans);
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
        public VMCMP_CompanyModel CompanyModel(PageSecurity userStatus)
        {
            db = db ?? new WorkOfTimeDatabase();
            if (userStatus.AuthorizedRoles.Contains(new Guid(SHRoles.HakEdisBayiPersoneli)))
            {
                var db = new WorkOfTimeDatabase();
                if (userStatus.user.CompanyId.HasValue)
                {
                    var company = db.GetCMP_CompanyById(userStatus.user.CompanyId.Value);
                    if (company != null)
                    {
                        var model = new VMCMP_CompanyModel { id = company.id }.Load();
                        return model;
                    }
                }
            }
            return new VMCMP_CompanyModel();
        }
        public VWPA_Account AccountModel(PageSecurity userStatus)
        {
            db = db ?? new WorkOfTimeDatabase();
            if (userStatus.AuthorizedRoles.Contains(new Guid(SHRoles.HakEdisBayiPersoneli)))
            {
                var db = new WorkOfTimeDatabase();
                if (userStatus.user.CompanyId.HasValue)
                {
                    var company = db.GetCMP_CompanyById(userStatus.user.CompanyId.Value);
                    if (company != null)
                    {
                        var account = db.GetPA_AccountByDataIdAndType(company.id);
                        if (account != null)
                        {
                            return account;
                        }
                    }
                }
            }
            return null;
        }
        public string CheckUserCompanyGeneralInfo(PageSecurity userStatus = null)
        {
            if (userStatus.AuthorizedRoles.Contains(new Guid(SHRoles.HakEdisBayiPersoneli)))
            {

                var db = new WorkOfTimeDatabase();
                if (userStatus.user.CompanyId.HasValue)
                {
                    var company = db.GetCMP_CompanyById(userStatus.user.CompanyId.Value);

                    if (company == null)
                    {
                        new FeedBack().Warning("Size Atanmış İşletme Kayıtlarımızda Yok", true, null, 1);
                    }
                    if (string.IsNullOrEmpty(company.taxNumber) || string.IsNullOrEmpty(company.email) || string.IsNullOrEmpty(company.phone) || string.IsNullOrEmpty(company.invoiceAddress) || string.IsNullOrEmpty(company.commercialTitle) || string.IsNullOrEmpty(company.name) || string.IsNullOrEmpty(company.taxOffice))
                    {
                        return "Bayi bilgilerinizde eksik alanlar bulunmaktadır.";
                    }
                }
                else
                {
                    new FeedBack().Warning("Herhangi bir işletmeye ait değilsiniz!", true, null, 1);
                    return "Herhangi bir işletmeye ait değilsiniz";
                }


            }
            return "";
        }
        public string CheckUserCompanyAccountInfo(PageSecurity userStatus = null)
        {
            if (userStatus.AuthorizedRoles.Contains(new Guid(SHRoles.HakEdisBayiPersoneli)))
            {

                var db = new WorkOfTimeDatabase();
                if (userStatus.user.CompanyId.HasValue)
                {
                    var company = db.GetCMP_CompanyById(userStatus.user.CompanyId.Value);

                    if (company == null)
                    {
                        new FeedBack().Warning("Size Atanmış İşletme Kayıtlarımızda Yok", true, null, 1);
                    }
                    var account = db.GetPA_AccountByDataIdAndType(company.id);
                    if (account == null)
                    {
                        return "Banka hesap bilgilerinde eksik alanlar bulunmaktadır.";
                    }
                }
                else
                {
                    new FeedBack().Warning("Herhangi bir işletmeye ait değilsiniz!", true, null, 1);
                    return "Herhangi bir işletmeye ait değilsiniz";
                }


            }
            return "";
        }
    }
}
