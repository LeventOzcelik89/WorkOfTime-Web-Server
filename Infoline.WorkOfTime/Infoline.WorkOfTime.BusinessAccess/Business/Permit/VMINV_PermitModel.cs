using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using static Infoline.WorkOfTime.BusinessAccess.WorkOfTimeDatabase;

namespace Infoline.WorkOfTime.BusinessAccess
{
    public class VMINV_PermitModel : VWINV_Permit
    {
        public static Guid yillikIzin = new Guid("F8547488-3215-1235-5126-EF2323CBBCB2");

        public static Guid mazeretIzin = new Guid("AF40ADF4-9963-4790-A9DE-1D74C32B61C1");
        public PageSecurity userStatus { get; set; }
        public VWSH_User User { get; set; }
        public CalculateReturn Calc { get; set; }
        public string Url { get; set; }
        public string LogoPath { get; set; }
        public VWUT_RulesUser VWUT_RulesUser { get; set; }
        public string ReturnPartialViewPage { get; set; }
        public List<VWUT_RulesUserStage> RulesUserStage = new List<VWUT_RulesUserStage>();
        public WorkOfTimeDatabase db { get; set; }
        public DbTransaction trans { get; set; }
        public INV_PermitType permitType { get; set; }
        public INV_PermitConfirmation[] permitConfirmation { get; set; }
        public bool hasApprove { get; set; }
        public Guid? ruleId { get; set; }

        public static INV_PermitOffical[] officalPermit = checkOfficialPermit(new WorkOfTimeDatabase().GetINV_PermitOffical());

        public static INV_PermitOffical[] checkOfficialPermit(INV_PermitOffical[] officalPermitm)
        {
            foreach (var offical in officalPermitm)
            {
                if (offical.StartDate.HasValue)
                {
                    var newstart = new DateTime(offical.StartDate.Value.Ticks);
                    var start = TenantConfig.Tenant.Config.WorkingTimes[newstart.DayOfWeek].allowTimes.OrderBy(a => a.Start).Select(a => a.Start).FirstOrDefault();
                    offical.StartDate = new DateTime(newstart.Year, newstart.Month, newstart.Day, start.Hours, start.Minutes, 0);
                }

                if (offical.EndDate.HasValue)
                {
                    var newend = new DateTime(offical.EndDate.Value.Ticks);
                    var end = TenantConfig.Tenant.Config.WorkingTimes[newend.DayOfWeek].allowTimes.OrderBy(a => a.End).Reverse().Select(a => a.End).FirstOrDefault();
                    offical.EndDate = new DateTime(newend.Year, newend.Month, newend.Day, end.Hours, end.Minutes, 0);
                }
            }

            return officalPermitm;
        }

        private DateTime? CommencementDate(INV_Permit item)
        {
            DateTime returnDate = new DateTime(item.EndDate.Value.Ticks);

            do
            {
                var timespan = new TimeSpan(returnDate.Hour, returnDate.Minute, returnDate.Second);
                var current = TenantConfig.Tenant.Config.WorkingTimes[returnDate.DayOfWeek];
                var offical = officalPermit.Where(a => a.StartDate <= returnDate && a.EndDate >= returnDate);

                if (offical.Count() == 0 && current.isWorking && current.allowTimes.Count(a => a.Start <= timespan && a.End > timespan) > 0)
                {
                    break;
                }

                returnDate = returnDate.AddMinutes(15);
            } while (true);


            return returnDate;
        }

        public VMINV_PermitModel Load(PageSecurity userStatus)
        {
            db = db ?? new WorkOfTimeDatabase();
            var permit = db.GetVWINV_PermitById(this.id);
            this.userStatus = userStatus;

            if (permit == null)
            {
                this.VWUT_RulesUser = RulesControl(userStatus.user.id);
                if (this.VWUT_RulesUser != null && this.VWUT_RulesUser.rulesId.HasValue)
                {
                    var listData = db.GetVWUT_RulesUserStageByRulesId(this.VWUT_RulesUser.rulesId.Value);
                    this.RulesUserStage.AddRange(listData);
                }
                var now = DateTime.Now;
                var start = this.CommencementDate(new DateTime(now.Year, now.Month, now.Day, 0, 0, 0));
                this.PermitCode = BusinessExtensions.B_GetIdCode();
                this.StartDate = start;
                var endTime = TenantConfig.Tenant.Config.WorkingTimes[start.DayOfWeek].allowTimes.OrderBy(a => a.End).Select(a => a.End).LastOrDefault();
                this.EndDate = new DateTime(start.Year, start.Month, start.Day, endTime.Hours, endTime.Minutes, 0);
                this.IdUser = userStatus.user.id;
                this.PermitTypeId = INV_PermitCalculator.yillikIzin;
                this.AccessPhone = userStatus.user.phone;
                this.ArriveAdress = userStatus.user.address;
                this.ReturnPartialViewPage = ReturnPagesNew(this, "Insert");
            }
            else
            {
                this.B_EntityDataCopyForMaterial(permit);
                this.Calc = this.Calculate();
                this.permitConfirmation = db.GetINV_PermitConfirmation().Where(x => x.permitId == this.id && x.userId == this.IdUser).ToArray();
                this.ReturnPartialViewPage = ReturnPagesNew(this, "Update");
            }

            if (this.IdUser.HasValue)
            {
                this.User = db.GetVWSH_UserById(this.IdUser.Value);
            }

            if (this.VWUT_RulesUser == null)
            {
                var defaultRule = db.GetUT_RulesByTypeIsDefault((Int32)EnumUT_RulesType.Permit);
                this.RulesUserStage = db.GetVWUT_RulesUserStageByRulesId(defaultRule.id).ToList();
            }

            if (this.RulesUserStage.Count() > 0)
            {
                this.ruleId = this.RulesUserStage.FirstOrDefault()?.rulesId;
            }

            return this;
        }

        public VWUT_RulesUser RulesControl(Guid userId)
        {
            var ruleUser = db.GetVWUT_RulesUserByUserIdAndType(userId, (Int32)EnumUT_RulesType.Permit);
            return ruleUser;
        }

        public string ReturnPagesNew(VMINV_PermitModel model, string page, bool? modal = false)
        {
            if (modal == true)
            {
                return "~/Areas/INV/Views/VWINV_Permit/HrInsert/" + page + "/" + page + ".cshtml";
            }
            else
            {
                return "~/Areas/INV/Views/VWINV_Permit/" + page + "/" + page + "New.cshtml";
            }
        }

        public bool UpdateButtonControl(Guid userId, string staffToApprove, bool? lasthasApprove)
        {
            db = db ?? new WorkOfTimeDatabase();
            if (staffToApprove == null)
            {
                return false;
            }
            var userRoles = db.GetSH_UserRoleByUserIds(staffToApprove.Split(',').Select(a => Guid.Parse(a)).ToArray());
            if (userRoles.Count(x => x.roleid == new Guid(SHRoles.IKYonetici)) == userRoles.Count() && lasthasApprove == true)
            {
                return true;
            }

            var approvedPerson = staffToApprove.Split(',').Select(a => Guid.Parse(a)).Reverse().FirstOrDefault();
            if (userId == approvedPerson && (lasthasApprove == true || lasthasApprove == false))
            {
                return true;
            }

            return false;
        }

        public ResultStatus Save(PageSecurity userStatus, HttpRequestBase request = null, DbTransaction _trans = null)
        {
            db = db ?? new WorkOfTimeDatabase();
            var res = new ResultStatus { result = true };
            trans = _trans ?? db.BeginTransaction();
            this.userStatus = userStatus;
            var permit = db.GetVWINV_PermitById(this.id);

            if (permit == null)
            {
                this.created = DateTime.Now;
                this.createdby = this.userStatus.user.id;
                res = this.Insert();
            }
            else
            {
                this.B_EntityDataCopyForMaterial(permit);
                this.changed = DateTime.Now;
                this.changedby = this.userStatus.user.id;
                res = this.Update();
            }


            if (res.result)
            {
                new FileUploadSave(request, this.id).SaveAs();
            }

            if (_trans == null)
            {
                if (res.result)
                    trans.Commit();
                else
                    trans.Rollback();
            }

            return res;
        }

        public ResultStatus Insert()
        {
            var result = new ResultStatus { result = true };
            result = this.InsertNew();
            return result;
        }

        public ResultStatus Update()
        {
            var result = new ResultStatus { result = true };

            //var endStage = db.GetVWUT_RulesUserStageByRulesId(this.ruleId.Value).Reverse().FirstOrDefault();
            var newConfirmation = new INV_PermitConfirmation
            {
                // hasApprove = this.hasApprove,
                permitId = this.id,
                ruleId = this.ruleId,
                userId = this.userStatus.user.id,
                created = DateTime.Now,
                createdby = this.userStatus.user.id,
                ruleStageId = this.userStatus.user.id
            };


            result &= db.InsertINV_PermitConfirmation(newConfirmation, trans);
            result &= db.UpdateINV_Permit(new INV_Permit().B_EntityDataCopyForMaterial(this, true), true, trans);


            if (result.result)
            {
                result.message = "İzin talebi güncelleme işlemi başarılı";
            }
            else
            {
                result.message = "İzin talebi güncelleme işlemi başarısız oldu.";
            }

            return result;
        }

        public ResultStatus InsertNew()
        {
            if (!this.PermitTypeId.HasValue)
            {
                return new ResultStatus { result = false, message = "İzin tipi bulunamadı." };
            }

            if (!this.IdUser.HasValue)
            {
                return new ResultStatus { result = false, message = "Herhangi bir firmada çalışmıyorsunuz." };
            }

            this.User = db.GetVWSH_UserById(this.IdUser.Value);

            var calc = this.Calculate();
            this.ApproveStatus = (int)EnumINV_PermitApproveStatus.TalepEdildi;
            this.TotalDays = calc.TotalDay;
            this.TotalHours = calc.TotalHour;

            var validate = this.ValidateNew();

            if (validate.result == false)
            {
                return new ResultStatus
                {
                    result = false,
                    message = validate.message
                };
            }

            var res = db.InsertINV_Permit(new INV_Permit().B_EntityDataCopyForMaterial(this));

            if (!res.result)
            {
                return new ResultStatus
                {
                    result = false,
                    message = "İzin talebi başarısız."
                };
            }

            var permitType = db.GetINV_PermitTypeById(this.PermitTypeId.Value);
            string created = string.Format("{0:dd.MM.yyyy HH:mm}", this.created);
            string start = string.Format("{0:dd.MM.yyyy HH:mm}", this.StartDate);
            string end = string.Format("{0:dd.MM.yyyy HH:mm}", this.EndDate);
            string url = TenantConfig.Tenant.GetWebUrl();
            var tenantName = TenantConfig.Tenant.TenantName;
            var projects = db.GetVWINV_CompanyPersonDepartmentsByIdUserAndType(this.IdUser.Value, EnumINV_CompanyDepartmentsType.Matrix).Where(a => a.Manager1.HasValue).Select(a => "" + a.Project_Title + " =>  Yöneticisi : " + a.Manager1_Title);

            var text = "<h3>Sayın {0},</h3>";
            text += "<p><u>{1}</u> isimli personel {2} tarihinde, {3} - {4} tarihleri arasında {5} {6} talebinde bulunmuştur.</p>";
            text += projects.Count() > 0 ? "<p>Çalıştığı proje(ler) ve yönetici(leri) : " + string.Join(" , ", projects) + "  </p>" : "";
            text += "Onaya gitmek için lütfen <a href='{7}/INV/VWINV_Permit/UpdateNew?id={8}'>Buraya tıklayınız! </a><p>Bilgilerinize.</p>";


            var manager = new VWSH_User();
            if (RulesUserStage.Count(x => x.type == (Int32)EnumUT_RulesUserStage.Manager1) > 0)
            {
                manager = db.GetVWSH_UserById(this.Manager1Approval.Value);
            }

            else if (RulesUserStage.Count(x => x.type == (Int32)EnumUT_RulesUserStage.Manager2) > 0)
            {
                manager = db.GetVWSH_UserById(this.Manager2Approval.Value);
            }

            if (manager.id != null)
            {
                var mesaj = string.Format(text, manager.firstname + " " + manager.lastname, this.User.firstname + " " + this.User.lastname, created, start, end, calc.Text, permitType.Name, url, this.id);
                new Email().Template("Template1", "izinMailFoto.jpg", "İzin Talebi Onayı Hakkında", mesaj).Send((Int16)EmailSendTypes.IzinOnaylari, manager.email, string.Format("{0} | {1}", tenantName + " | WORKOFTIME", "İzin Talebi Onayı Hakkında.."), true);
            }

            return new ResultStatus
            {
                result = true,
                message = "İzin talebi başarı ile gerçekleştirildi."
            };
        }



        public CalculateReturn Calculate()
        {
            if (this.StartDate == null || this.EndDate == null)
            {
                return new CalculateReturn { Text = "false" };
            }

            DateTime startDate = new DateTime(this.StartDate.Value.Ticks);
            DateTime endDate = new DateTime(this.EndDate.Value.Ticks);
            INV_PermitType type = null;
            if (this.PermitTypeId.HasValue)
            {
                type = new WorkOfTimeDatabase().GetINV_PermitTypeById(this.PermitTypeId.Value);
            }
            var result = new CalculateReturn { CommencementDate = this.CommencementDate(new INV_Permit().B_EntityDataCopyForMaterial(this)), TotalDay = 0, TotalHour = 0 };

            do
            {
                if (startDate >= endDate)
                {
                    break;
                }

                var timespan = new TimeSpan(startDate.Hour, startDate.Minute, startDate.Second);
                var current = TenantConfig.Tenant.Config.WorkingTimes[startDate.DayOfWeek];
                var offical = officalPermit.Where(a => a.StartDate <= startDate && a.EndDate >= startDate);

                if (type != null && type.CanHourly == true)
                {
                    if (offical.Count() == 0 && current.isWorking && current.allowTimes.Count(a => a.Start <= timespan && a.End > timespan) > 0)
                    {
                        result.TotalHour = result.TotalHour + 0.25;
                    }
                    startDate = startDate.AddMinutes(15);
                }
                else
                {
                    if (offical.Count() == 0 && current.isWorking)
                    {
                        result.TotalDay = result.TotalDay + 1;
                    }
                    startDate = startDate.AddDays(1);
                }
            } while (true);


            if (type != null && type.CanHourly == false)
            {
                result.TotalHour = result.TotalDay * 9;
            }
            else
            {
                result.TotalDay = result.TotalHour / 9;
            }


            var minute = result.TotalHour.Value % 1;
            var day = Math.Floor(result.TotalHour.Value / 9);
            var hour = Math.Floor(result.TotalHour.Value - (day * 9));

            result.Text += day > 0 ? day + " Gün " : "";
            result.Text += hour > 0 ? hour + " Saat " : "";
            result.Text += minute > 0 ? (minute * 60) + " Dakika " : "";

            return result;
        }

        public DateTime CommencementDate(DateTime date)
        {
            return CommencementDate(new INV_Permit { EndDate = date }).Value;
        }

        public ResultStatus ValidateNew()
        {
            var db = new WorkOfTimeDatabase();
            if (this.PermitTypeId.HasValue)
            {
                this.permitType = new WorkOfTimeDatabase().GetINV_PermitTypeById(this.PermitTypeId.Value);
            }
            var result = new ResultStatus
            {
                result = false,
            };

            if (this.permitType == null)
            {
                result.message = "İzin tipi bulunamadı.";
                return result;
            }

            if ((this.permitType.CanHourly == true && (this.TotalHours == null || this.TotalHours == 0)) || (this.permitType.CanHourly == false && (this.TotalDays == null || this.TotalDays == 0)))
            {
                result.message = "Seçtiğiniz başlangıç ve bitiş tarihleri'ni kontrol ediniz.";
                return result;
            }

            //Tanımlı izinler
            if (this.permitType.PaidPermitDay.HasValue && this.TotalDays > this.permitType.PaidPermitDay.Value)
            {
                result.message = string.Format("{0} izni maksimum {1} gündür.{2} günlük tarih aralığı seçtiniz tarih aralığınız kontrol ediniz.", this.permitType.Name, this.permitType.PaidPermitDay.Value, this.TotalDays);
                return result;
            }

            if (this.StartDate >= DateTime.Now)
            {
                var permitsStandBy = db.GetVWINV_PermitStandByByIdUser(this.IdUser.Value);
                if (permitsStandBy.Count() > 3)
                {
                    result.message = string.Format("Personelin onayda bekleyen izin talepleri bulunmaktadır.Bu talepler cevaplanmadan yeni talep'de bulunulamaz.");
                    return result;
                }

                var controlCommission = db.GetVWINV_CommissionsPersonsByControlDate(this.IdUser.Value, this.StartDate.Value, this.EndDate.Value);
                if (controlCommission != null)
                {
                    result.message = string.Format("{0} ile {1} tarihleri arasında göreviniz bulunmaktadır. Farklı bir tarih aralığını seçerek tekrar deneyiniz.", string.Format("{0:dd.MM.yyyy HH:mm}", controlCommission.StartDate), string.Format("{0:dd.MM.yyyy HH:mm}", controlCommission.EndDate));
                    return result;
                }

                var controlPermit = db.GetINV_PermitByControlDate(this.IdUser.Value, this.StartDate.Value, this.EndDate.Value);
                if (controlPermit != null)
                {
                    result.message = string.Format("{0} ile {1} tarihleri arasında zaten izin talebiniz bulunmaktadır.", string.Format("{0:dd.MM.yyyy HH:mm}", controlPermit.StartDate), string.Format("{0:dd.MM.yyyy HH:mm}", controlPermit.EndDate));
                    return result;
                }
            }
            else
            {
                var controlPermit = db.GetINV_PermitApprovedPermit(this.IdUser.Value, this.StartDate.Value, this.EndDate.Value);
                if (controlPermit != null)
                {
                    result.message = string.Format("Personelin {0} ile {1} tarihleri arasında zaten kayıtlı izni bulunmaktadır.", string.Format("{0:dd.MM.yyyy HH:mm}", controlPermit.StartDate), string.Format("{0:dd.MM.yyyy HH:mm}", controlPermit.EndDate));
                    return result;
                }
            }

            var userRoles = db.GetSH_UserRoleByUserId(this.userStatus.user.id);
            //IK GELECEĞE AVANS İZİN GİRMEK İSTERSE NİYE DİYE SORMUYORUZ İZİN VERİYORUZ (Tuba Hanımın özel isteği üzerine)
            if (userRoles.Count(x => x.roleid == new Guid(SHRoles.IKYonetici)) > 0 && this.IdUser != this.userStatus.user.id)
            {
                return new ResultStatus
                {
                    result = true,
                    message = "Başarılı"
                };
            }

            if (!this.User.JobStartDate.HasValue)
            {
                return new ResultStatus
                {
                    result = false,
                    message = "İş başlangıç tarihi bulunmamaktadır."
                };
            }

            /*İzin Müsaitlik validasyon*/
            if (this.PermitTypeId == yillikIzin)
            {

                var timeSpan = this.StartDate - this.User.JobStartDate;
                var totalYear = timeSpan.Value.TotalDays / 365;
                var totalPermit = 0;
                for (int i = 1; i < totalYear; i++)
                {
                    totalPermit += (i < 6 ? 14 : (i < 15 ? 20 : 26));
                }

                if (totalPermit > 0)
                {
                    if (this.TotalDays > this.User.PermitYearlyUsable)
                    {
                        result.message = string.Format("Personelin kalan yıllık izin hakkı {1} gün'dür.Almaya çalışılan izin {2} gündür ve kalan izin hakkı bu izin süresini karşılamamaktadır.", string.Format("{0:dd.MM.yyyy HH:mm}", this.StartDate), this.User.PermitYearlyUsable, this.TotalDays);
                        return result;
                    }

                }
                else
                {
                    result.message = string.Format("{0} tarihinde personel'in yıllık izin hakkı bulunmamaktadır.", string.Format("{0:dd.MM.yyyy HH:mm}", this.StartDate));
                    return result;
                }
            }

            /*İzin Müsaitlik Validasyon*/

            if (this.PermitTypeId == mazeretIzin)
            {

                if (this.User.PermitYearlyUsable > 0 && this.TotalHours >= 8.5)
                {
                    result.message = string.Format("Personelin yıllık izin hakkı bulunduğundan tam gün mazeret izni kullanamaz.Yıllık izin olarak talepte bulununuz.");
                    return result;

                }
                if (this.TotalHours > this.User.PermitExcuseUsable)
                {
                    result.message = string.Format("Personelin {0} yılı için kalan mazeret izin hakkı {1} saat'dir.Almaya çalışılan izin {2} saatdir ve kalan izin hakkı bu izin süresini karşılamamaktadır.", this.StartDate.Value.Year, this.User.PermitExcuseUsable, this.TotalHours);
                    return result;
                }
            }
            return new ResultStatus
            {
                result = true,
                message = "Başarılı"
            };

        }

        public SummaryHeadersPermitNew GetMyPermitSummary(Guid userId)
        {
            db = new WorkOfTimeDatabase();
            return db.GetVWINV_PermitMyPermitCountFilter(userId);
        }

        public SummaryHeadersPermitNew GetRequestPermitSummary(Guid userId)
        {
            db = new WorkOfTimeDatabase();
            return db.GetVWINV_PermitRequestPermitCountFilter(userId);
        }
    }
}
