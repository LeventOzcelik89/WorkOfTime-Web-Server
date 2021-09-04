using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessAccess.Models;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace Infoline.WorkOfTime.BusinessAccess
{
    public class VMSH_UserModel : VWSH_User
    {
        public Guid? hrPersonId { get; set; }
        public List<Guid> Roles { get; set; } = new List<Guid> { new Guid(SHRoles.Personel) };
        public double? Salary { get; set; }

        public VWSH_PersonInformation VWSH_PersonInformation { get; set; }
        public VWINV_CompanyPersonAvailability[] VWINV_CompanyPersonAvailabilities { get; set; }
        public VWPA_Account[] VWPA_Accounts { get; set; }
        public WorkOfTimeDatabase db { get; set; }
        public DbTransaction trans { get; set; }
        public List<SYS_BlockMail> blockMailList { get; set; }
        public ResultStatus Save(DbTransaction _trans = null)
        {
            db = db ?? new WorkOfTimeDatabase();
            trans = _trans ?? db.BeginTransaction();
            var user = db.GetVWSH_UserById(this.id);
            if (this.CompanyId == null)
            {
                return new ResultStatus { result = false, message = "Personel işletmesi zorunlu alandır." };
            }

            var company = db.GetVWCMP_CompanyById(this.CompanyId.Value);
            var result = new ResultStatus { result = true };
            if (company.type == (int)EnumCMP_CompanyType.Benimisletmem)
            {
                this.type = (int)EnumSH_UserType.MyPerson;
                result &= user == null ? this.Insert() : this.Update(user);
            }
            else
            {
                this.type = (int)EnumSH_UserType.OtherPerson;
                result &= this.UpsertOther(user);
            }


            if (_trans == null)
            {
                if (result.result == true)
                {
                    trans.Commit();
                    new ManagersCalculator().Run();
                }
                else
                {
                    trans.Rollback();
                }
            }

            return result;
        }

        private ResultStatus UpsertOther(VWSH_User userEx)
        {
            var password = Guid.NewGuid().ToString().Substring(0, 8);
            var dbUser = userEx ?? db.GetVWSH_UserByCompanyIdAndName(this.CompanyId.Value, this.firstname, this.lastname, this.email);
            var rs = new ResultStatus { result = true };

            if (dbUser != null)
            {
                dbUser.changed = DateTime.Now;
                dbUser.changedby = this.createdby;
                dbUser.title = this.title;
                dbUser.firstname = this.firstname;
                dbUser.lastname = this.lastname;
                dbUser.phone = this.phone;
                dbUser.cellphone = this.cellphone;
                dbUser.email = this.email;
                dbUser.status = true;

                rs &= db.UpdateSH_User(new SH_User().B_EntityDataCopyForMaterial(dbUser, true), false, trans);
            }
            else
            {
                this.code = BusinessExtensions.B_GetIdCode();
                this.status = true;
                this.loginname = Guid.NewGuid().ToString().Substring(0, 8);
                this.password = db.GetMd5Hash(db.GetMd5Hash(password));
                var user = new SH_User().B_EntityDataCopyForMaterial(this, true);

                var companyPerson = new INV_CompanyPerson
                {
                    created = this.created,
                    createdby = this.createdby,
                    CompanyId = this.CompanyId,
                    Title = this.title,
                    Level = 0,
                    IdUser = user.id,
                    JobStartDate = DateTime.Now,
                };

                rs &= db.InsertSH_User(user, trans);
                rs &= db.InsertINV_CompanyPerson(companyPerson, trans);
            }

            var rolesEx = db.GetSH_UserRoleByUserId(this.id);
            var rolesNew = this.Roles.GroupBy(a => a).Select(a => new SH_UserRole
            {
                created = this.created,
                createdby = this.createdby,
                roleid = a.Key,
                userid = this.id,
            }).ToList();

            rs &= db.BulkDeleteSH_UserRole(rolesEx, this.trans);
            rs &= db.BulkInsertSH_UserRole(rolesNew, this.trans);

            if (rs.result == true)
            {
                return new ResultStatus { result = true, message = "Sorumlu kaydetme işlemi başarıyla gerçekleşti." };
            }
            else
            {
                return new ResultStatus { result = false, message = "Sorumlu kaydetme işlemi başarısız." };
            }
        }

        private ResultStatus Insert()
        {
            var validate = ValidateLoginName(this.loginname);
            if (validate.result == false) return validate;
            validate = ValidateEmail(this.email);
            if (validate.result == false) return validate;

            var password = Guid.NewGuid().ToString().Substring(0, 8);
            this.code = BusinessExtensions.B_GetIdCode();
            this.type = (int)EnumSH_UserType.MyPerson;
            this.loginname = this.loginname.Trim().ToUpper();
            this.firstname = this.firstname.Trim().ToUpper();
            this.lastname = this.lastname.Trim().ToUpper();
            this.password = db.GetMd5Hash(db.GetMd5Hash(password));
            this.title = this.title;
            this.Roles.Add(new Guid(SHRoles.Personel));
            this.status = true;

            var user = new SH_User().B_EntityDataCopyForMaterial(this, true);
            var companyPerson = new INV_CompanyPerson
            {
                created = this.created,
                createdby = this.createdby,
                JobStartDate = this.JobStartDate,
                CompanyId = this.CompanyId,
                Title = this.CompanyPerson_Title,
                Level = this.CompanyPerson_Level,
                IdUser = user.id,
            };

            var companyPersonSallary = new INV_CompanyPersonSalary
            {
                created = this.created,
                createdby = this.createdby,
                StartDate = this.JobStartDate,
                EndDate = this.JobStartDate.HasValue ? this.JobStartDate.Value.AddDays(365) : DateTime.Now.AddDays(365),
                Salary = 0,
                IdUser = user.id,
            };

            var roles = this.Roles.GroupBy(a => a).Select(a => new SH_UserRole
            {
                created = this.created,
                createdby = this.createdby,
                roleid = a.Key,
                userid = user.id,
            }).ToList();

            var personInfo = new SH_PersonInformation
            {
                created = this.created,
                createdby = this.createdby,
                UserId = this.id,
                IdentificationNumber = this.loginname,
                Gender = this.VWSH_PersonInformation?.Gender,
                PersonalMail = this.VWSH_PersonInformation?.PersonalMail
            };

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
                dataTable = "SH_User"
            };


            var rs = db.InsertSH_User(user, this.trans);
            rs &= db.InsertSH_PersonInformation(personInfo, this.trans);
            rs &= db.InsertINV_CompanyPerson(companyPerson, this.trans);
            rs &= db.InsertINV_CompanyPersonSalary(companyPersonSallary, this.trans);
            rs &= db.InsertPA_Account(account, this.trans);
            rs &= db.BulkInsertSH_UserRole(roles, this.trans);
            if (this.DepartmentId != null)
            {
                var companyPersonDepartments = new INV_CompanyPersonDepartments
                {
                    created = this.created,
                    createdby = this.createdby,
                    StartDate = this.JobStartDate,
                    IsBasePosition = true,
                    DepartmentId = this.DepartmentId,
                    Title = this.title,
                    IdUser = user.id,
                };

                rs &= db.InsertINV_CompanyPersonDepartments(companyPersonDepartments, this.trans);
            }


            if (rs.result == true)
            {

                return new ResultStatus { result = true, message = "Personel kaydetme işlemi başarıyla gerçekleşti." };
            }
            else
            {
                return new ResultStatus { result = false, message = "Personel kaydetme işlemi başarısız." };
            }

        }

        private ResultStatus Update(VWSH_User userEx)
        {
            db = db ?? new WorkOfTimeDatabase();

            var validate = ValidateEmail(this.email);
            if (validate.result == false) return validate;

            var user = new SH_User();
            user.id = this.id;
            user.address = this.address;
            user.birthday = this.birthday;
            user.cellphone = this.cellphone;
            user.code = this.code ?? BusinessExtensions.B_GetIdCode();
            user.changed = this.changed;
            user.changedby = this.changedby;
            user.created = userEx.created;
            user.createdby = userEx.createdby;
            user.companyCellPhone = this.companyCellPhone;
            user.companyCellPhoneCode = this.companyCellPhoneCode;
            user.companyOfficePhone = this.companyOfficePhone;
            user.companyOfficePhoneCode = this.companyOfficePhoneCode;
            user.email = this.email;
            user.firstname = this.firstname.Trim().ToUpper();
            user.locationId = this.locationId;
            user.lastname = this.lastname.Trim().ToUpper();
            user.loginname = this.loginname.Trim();
            user.phone = this.phone;
            user.password = userEx.password;
            user.status = this.status;
            user.title = this.title;
            user.type = (int)EnumSH_UserType.MyPerson;


            this.Roles.Add(new Guid(SHRoles.Personel));
            var rolesEx = db.GetSH_UserRoleByUserId(id);
            var rolesNew = this.Roles.GroupBy(a => a).Select(a => new SH_UserRole
            {
                created = this.created,
                createdby = this.createdby,
                roleid = a.Key,
                userid = user.id,
            }).ToList();



            var rs = db.UpdateSH_User(user, true, this.trans);
            rs &= db.BulkDeleteSH_UserRole(rolesEx, this.trans);
            rs &= db.BulkInsertSH_UserRole(rolesNew, this.trans);


            if (this.VWSH_PersonInformation != null)
            {
                var information = db.GetSH_PersonInformationById(this.VWSH_PersonInformation.id);
                if (information != null)
                {
                    information.Gender = this.VWSH_PersonInformation.Gender;
                    information.PersonalMail = this.VWSH_PersonInformation.PersonalMail;
                    rs &= db.UpdateSH_PersonInformation(information, false, this.trans);
                }
            }

            if (TenantConfig.Tenant.TenantCode == 1139)
            {
                var companyPerson = new INV_CompanyPerson();
                if (this.CompanyPersonId.HasValue)
                {
                    companyPerson = db.GetINV_CompanyPersonById(this.CompanyPersonId.Value);
                }
                else
                {
                    companyPerson = db.GetINV_CompanyPersonByUserId(this.id).Where(x => x.JobEndDate == null || x.JobEndDate >= DateTime.Now).FirstOrDefault();
                }

                if (companyPerson.id != null)
                {
                    companyPerson.JobStartDate = this.JobStartDate;
                    rs &= db.UpdateINV_CompanyPerson(companyPerson, true, this.trans);
                }
            }

            if (userEx.CompanyPersonDepartmentId.HasValue)
            {
                var department = db.GetINV_CompanyPersonDepartmentsById(userEx.CompanyPersonDepartmentId.Value);
                department.Title = this.title;
                rs &= db.UpdateINV_CompanyPersonDepartments(department, true, this.trans);
            }

            if (rs.result == true)
            {
                return new ResultStatus { result = true, message = "Personel güncelleme işlemi başarıyla gerçekleşti." };
            }
            else
            {
                return new ResultStatus { result = false, message = "Personel güncelleme işlemi başarısız." };
            }
        }

        public ResultStatus Delete(DbTransaction _trans = null)
        {
            db = db ?? new WorkOfTimeDatabase();
            var hasDebit = db.GetVWPRD_StockSummaryHasDebitByStockId(this.id);
            if (hasDebit.Count() > 0)
            {
                return new ResultStatus { result = false, message = "Üzerinde zimmet olan personellerin silme işlemi gerçekleştirilemez." };
            }
            this.trans = _trans ?? db.BeginTransaction();

            var companies = db.GetINV_CompanyPersonByUserId(this.id);
            var deparmants = db.GetINV_CompanyPersonDepartmentsByIdUser(this.id);
            var salaries = db.GetINV_CompanyPersonSalaryByIdUserAll(this.id);
            var roles = db.GetSH_UserRoleByUserIds(new Guid[] { this.id });

            var accounts = db.GetPA_AccountByDataId(this.id);
            var transactions = db.GetPA_TransactionByAccountIds(accounts.Select(a => a.id).ToArray()).ToArray();
            var accountLedgers = db.GetPA_LedgerByAccountIds(accounts.Select(a => a.id).ToArray()).ToArray();
            var relatedAccountLedgers = db.GetPA_LedgerByRelatedAccountIds(accounts.Select(a => a.id).ToArray()).ToArray();

            var rs = db.DeleteSH_User(this.id, this.trans);
            rs &= db.BulkDeleteINV_CompanyPerson(companies, this.trans);
            rs &= db.BulkDeleteINV_CompanyPersonDepartments(deparmants, this.trans);
            rs &= db.BulkDeleteINV_CompanyPersonSalary(salaries, this.trans);
            rs &= db.BulkDeleteSH_UserRole(roles, this.trans);

            rs &= db.BulkDeletePA_Ledger(accountLedgers, this.trans);
            rs &= db.BulkDeletePA_Ledger(relatedAccountLedgers, this.trans);
            rs &= db.BulkDeletePA_Transaction(transactions, this.trans);
            rs &= db.BulkDeletePA_Account(accounts, this.trans);

            if (rs.result == true)
            {
                if (_trans == null)
                {
                    this.trans.Commit();
                }
                new ManagersCalculator().Run();
                return new ResultStatus { result = true, message = "Personel silme işlemi başarıyla gerçekleşti." };
            }
            else
            {
                if (_trans == null)
                {
                    this.trans.Rollback();
                }
                return new ResultStatus { result = false, message = "Personel silme işlemi başarısız." };
            }
        }

        public ResultStatus SendPassword()
        {

            string url = TenantConfig.Tenant.GetWebUrl();
            var password = Guid.NewGuid().ToString().Substring(0, 8);

            var user = db.GetSH_UserById(this.id);

            if (string.IsNullOrEmpty(user.email))
            {
                return new ResultStatus { result = false, message = "Şifre bilgisi gönderilebilmesi için e-posta adresi gerekmektedir.Lütfen bir e-posta adresi tanılayınız." };
            }

            user.password = db.GetMd5Hash(db.GetMd5Hash(password));
            var tenantName = TenantConfig.Tenant.TenantName;
            var tenantCode = TenantConfig.Tenant.TenantCode;

            var rs = db.UpdateSH_User(user, true);
            if (rs.result == true)
            {

                if (user.type == (int)EnumSH_UserType.MyPerson)
                {
                    var mesajIcerigi = string.Format(@"<h3>Merhaba!</h3> <p> {2} | WorkOfTime Sistemi üzerinde IK Yöneticiniz şifrenizi Sıfırladı.Aşağıdaki bilgilerle oturum açabilirsiniz</p>
                        <p>Sisteme <u> Kimlik Numaranız</u> ve <u>Şifreniz</u> ile giriş sağlayabilirsiniz.</p>
                        <p><strong>Yeni Şifreniz : <strong><span style='color: #ed5565;'>{0}</span></p>
                        <p> Giriş yapmak için lütfen <a href = '{1}/Account/SignIn' > Buraya tıklayınız! </a></p>", password, url, tenantName);


                    new Email().Template("Template1", "userMailFoto.jpg", "Şifre Sıfırlama Bildirimi", mesajIcerigi)
                              .Send((Int16)EmailSendTypes.ZorunluMailler, user.email, string.Format("{0} | {1}", tenantName + " | WORKOFTIME", "Şifre Sıfırlama Bildirimi"), true);

                }
                else
                {
                    var serviceDomain = TenantConfig.Tenant.GetWebServiceUrl();

                    var mesajIcerigi = string.Format(@"<h3>Merhaba!</h3> <p> {2} | WorkOfTime sistemi üzerinde oturum acabileceğiniz üyelik bilgileri aşagıdaki gibidir.</p>
                        <p><strong>Müşteri Kodu : <strong><span style='color: #ed5565;'>{6}</span></p>
                        <p><strong>Kullanıcı Adı : <strong><span style='color: #ed5565;'>{3}</span></p>
                        <p><strong>Şifre : <strong><span style='color: #ed5565;'>{0}</span></p>
                        <p><strong>E-Posta : <strong><span style='color: #ed5565;'>{4}</span></p>
                        <p> Web üzerinden giriş yapmak için lütfen <a href='{1}/Account/SignIn'> Buraya tıklayınız! </a></p>
                        ", password, url, tenantName, user.loginname, user.email, serviceDomain,tenantCode);

                    new Email().Template("Template1", "userMailFoto.jpg", "Üyelik Bildirimi", mesajIcerigi)
                              .Send((Int16)EmailSendTypes.ZorunluMailler, user.email, string.Format("{0} | {1}", tenantName, "Üyelik Bildirimi"), true);
                }


                return new ResultStatus { result = true, message = "Şifre gönderme işlemi başarıyla gerçekleşti." };
            }
            else
            {
                return new ResultStatus { result = false, message = "Şifre gönderme işlemi başarısız." };
            }
        }

        public ResultStatus Dismissal()
        {
            db = db ?? new WorkOfTimeDatabase();

            var user = db.GetVWSH_UserById(id);
            if (user == null)
            {
                return new ResultStatus { result = false, message = "Kullanıcı bulunamadı." };
            }

            //İşten çıkarılma işlemlerinde değişiklik yapılması istendiğinden kaynaklı kaldırıldı.
            //if (user.JobEndDate != null && user.JobEndDate < DateTime.Now)
            //{
            //    return new ResultStatus { result = false, message = "Personel zaten işten çıkarılmış." };
            //}

            if (user.IsWorking == false)
            {
                return new ResultStatus
                {
                    result = false,
                    message = "Seçilen personel zaten işten çıkarılmıştır.",
                };
            }
            var envanterKontrol = db.GetVWPRD_StockSummaryByStockId(user.id).Where(x => x.quantity >0).ToList();
            var zimmetDusme = db.GetVWPRD_StockSummaryByStockId(user.id).Where(a => a.quantity < 0);
            if (envanterKontrol.Count() != zimmetDusme.Count())
            {
                return new ResultStatus
                {
                    result = false,
                    message = "Personel'e zimmetli envanter bulunmaktadır.İşten çıkarma işlemi için envanterleri personel'in zimmetin'den çıkarınız.",
                };
            }


            var companyPersonSalary = db.GetINV_CompanyPersonSalaryByIdUserAll(user.id).Where(a => a.EndDate == null || a.EndDate > DateTime.Now).ToArray();
            foreach (var salary in companyPersonSalary)
            {
                salary.EndDate = this.JobEndDate;
            }

            var companyPersonDepartments = db.GetINV_CompanyPersonDepartmentsByIdUser(user.id).Where(a => a.EndDate == null || a.EndDate > DateTime.Now).ToArray();
            foreach (var per in companyPersonDepartments)
            {
                per.EndDate = this.JobEndDate;
            }

            if (true)
            {

            }
            var companyPerson = db.GetINV_CompanyPersonById(user.CompanyPersonId.Value);
            companyPerson.JobEndDate = this.JobEndDate;
            companyPerson.JobLeaving = this.JobLeaving;
            companyPerson.JobLeavingDescription = this.JobLeavingDescription;
            companyPerson.changed = this.changed;
            companyPerson.changedby = this.changedby;


            var trans = db.BeginTransaction();
            var rs = db.BulkUpdateINV_CompanyPersonDepartments(companyPersonDepartments, false, trans);
            rs &= db.BulkUpdateINV_CompanyPersonSalary(companyPersonSalary, false, trans);
            rs &= db.UpdateINV_CompanyPerson(companyPerson, false, trans);

            if (rs.result == true)
            {
                trans.Commit();
                new ManagersCalculator().Run();
                return new ResultStatus { result = true, message = "Personel işten çıkarma işlemi başarılı." };
            }
            else
            {
                trans.Rollback();
                return new ResultStatus { result = false, message = "Personel işten çıkarma işlemi başarısız." };
            }

        }

        public ResultStatus ChangePassword(VWSH_UserChangePassword item, Guid userId)
        {
            var db = new WorkOfTimeDatabase();
            var user = db.GetSH_UserById(userId);

            if (user == null)
            {
                return new ResultStatus { result = false, message = "Kullanıcı Bulunamadı." };
            }

            var oldPasswordCrypt = db.GetMd5Hash(db.GetMd5Hash(item.oldPassword));

            if (string.IsNullOrEmpty(item.oldPassword))
            {
                return new ResultStatus { result = false, message = "Lütfen önce ki şifrenizi giriniz." };
            }

            if (user.password == oldPasswordCrypt)
            {
                if (!string.IsNullOrEmpty(item.newPassword) && !string.IsNullOrEmpty(item.newPasswordAgain))
                {
                    if (item.newPassword == item.newPasswordAgain)
                    {
                        user.password = db.GetMd5Hash(db.GetMd5Hash(item.newPassword));
                        var result = db.UpdateSH_User(user);
                        if (result.result)
                        {
                            return new ResultStatus { result = true, message = "Şifre güncelleme işlemi başarılı." };
                        }
                        else
                        {
                            return new ResultStatus { result = false, message = "Şifre güncelleme işlemi başarısız." };
                        }
                    }
                    else
                    {
                        return new ResultStatus { result = false, message = "Şifreler eşleşmemektedir." };
                    }
                }
                else
                {
                    return new ResultStatus { result = false, message = "Lütfen yeni şifrenizi giriniz." };
                }
            }

            return new ResultStatus { result = false };
        }

        public VMSH_UserModel Load()
        {
            db = db ?? new WorkOfTimeDatabase();
            var user = db.GetVWSH_UserById(this.id);
            var rolesDB = db.GetSH_UserRoleByUserId(id);

            if (user != null)
            {
                this.B_EntityDataCopyForMaterial(user, true);
            }

            if (this.hrPersonId.HasValue)
            {
                var data = db.GetHR_PersonById(this.hrPersonId.Value);
                this.firstname = data.Name;
                this.lastname = data.SurName;
                this.loginname = data.IdentifyNumber;
                this.email = data.Email;
                this.birthday = data.Birthday;
                this.locationId = null;
                this.cellphone = data.Phone;
                this.address = data.Adress;
                this.locationId = data.LocationId;
            }

            this.code = this.code ?? BusinessExtensions.B_GetIdCode();
            this.blockMailList = db.GetSYS_BlockMailByUserId(this.id).ToList();
            this.VWSH_PersonInformation = db.GetVWSH_PersonInformationByUserId(this.id) ?? new VWSH_PersonInformation { UserId = this.id, IdentificationNumber = this.loginname };
            this.VWINV_CompanyPersonAvailabilities = new INV_CompanyPersonAvailabilityModel(this.id, new VWINV_CompanyPersonAvailability[] { }).GetDailySchemaByPerson();
            this.Roles = rolesDB.Where(x => x.roleid.HasValue).Select(a => a.roleid.Value).ToList();
            return this;
        }

        public ResultStatus ValidateLoginName(string loginname)
        {
            var res = db.GetSH_UserByLoginName(loginname);
            return new ResultStatus
            {
                result = (res == null),
                message = (res == null) ? "" : "Girilen kullanıcı adı sistemde zaten mevcut."
            };
        }

        public ResultStatus ValidateEmail(string email)
        {
            var res = db.GetSH_UserInfosByEmail(email);
			if (res.Count() > 1)
			{
                return new ResultStatus
                {
                    result = (res == null),
                    message = (res == null) ? "" : "Girilen Email adresi sistemde zaten mevcut."
                };
            }

            return new ResultStatus
            {
                result = true
            };
        }


        public string FormatHour(double? deger)
        {
            if (deger == null || deger == 0)
            {
                return "0 saat";
            }

            var mntUsed = deger % 1;
            var minuteUsed = Math.Floor((60 * mntUsed.Value));
            var hourUsed = Math.Floor(deger.Value);
            var txtUsed = "";
            txtUsed += hourUsed != 0 ? hourUsed + " saat " : "";
            txtUsed += minuteUsed != 0 ? minuteUsed + " dakika" : "";
            return txtUsed;
        }

    }

    public class VMSHUserAndFileResume : VWSH_User
    {
        public string file { get; set; }
    }

    public class VWSH_UserChangePassword : VWSH_User
    {
        public string oldPassword { get; set; }
        public string newPassword { get; set; }
        public string newPasswordAgain { get; set; }
    }

    public class VMPersonALLInformation : VWSH_User
    {
        public VWSH_PersonSchools[] PersonSchools { get; set; }
        public VWSH_PersonCertificate[] PersonCertificate { get; set; }
        public VWSH_PersonLanguages[] PersonLanguages { get; set; }
        public VWSH_PersonWorkExperience[] PersonWorkExperience { get; set; }
        public VWSH_PersonCompetencies[] PersonCompetencies { get; set; }
        public VWSH_PersonInformation PersonInformation { get; set; }
    }

    public class VWPersonCertificateModel
    {
        public Guid Id { get; set; }
        public string PersonName { get; set; }
        public string CertificateName { get; set; }
        public string CertificateTimeText { get; set; }
        public string CertificateStatus { get; set; }
        public string CertificateEndDate { get; set; }
    }
}
