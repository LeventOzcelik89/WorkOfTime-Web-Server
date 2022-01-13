using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Infoline.WorkOfTime.BusinessAccess
{
    public class INV_PermitModelBusiness
    {
        public WorkOfTimeDatabase _db = new WorkOfTimeDatabase();
        private string _siteURL { get; set; } = TenantConfig.Tenant.GetWebUrl();
        public ResultStatus Insert(INV_Permit item)
        {
            var feedback = new FeedBack();
            if (!item.PermitTypeId.HasValue)
            {
                return new ResultStatus { result = false, message = "İzin tipi bulunamadı." };
            }
            var calc = INV_PermitCalculator.Calculate(item);
            item.created = DateTime.Now;
            item.ApproveStatus = (int)EnumINV_PermitApproveStatus.TalepEdildi;
            item.TotalDays = calc.TotalDay;
            item.TotalHours = calc.TotalHour;
            item.PermitCode = BusinessExtensions.B_GetIdCode();
            if (!item.IdUser.HasValue)
            {
                return new ResultStatus { result = false, message = "Herhangi bir firmada çalışmıyorsunuz." };
            }
            var personel = _db.GetVWSH_UserById(item.IdUser.Value);
            if (personel == null)
            {
                return new ResultStatus { result = false, message = "Personel bulunamadı." };
            }
            var IKYonetici = _db.GetSH_UserByRoleId(SHRoles.IKYonetici).OrderBy(a => a.CompanyId == personel.CompanyId).FirstOrDefault();
            var validate = INV_PermitCalculator.Validate(item, personel, IKYonetici);
            if (validate.result == false)
            {
                return new ResultStatus
                {
                    result = false,
                    message = validate.message
                };
            }
            var compPersonDept = _db.GetVWINV_CompanyPersonDepartmentsByIdUserAndType(item.IdUser.Value, EnumINV_CompanyDepartmentsType.Organization).FirstOrDefault() ?? new VWINV_CompanyPersonDepartments();
            item.IkApproval = IKYonetici.id;
            item.Manager1Approval = compPersonDept.Manager1;
            item.Manager2Approval = compPersonDept.Manager2;
            if (!item.Manager1Approval.HasValue)
            {
                item.Manager1ApprovalDate = DateTime.Now;
                item.ApproveStatus = (int)EnumINV_PermitApproveStatus.Yonetici1Onay;
            }
            if (!item.Manager2Approval.HasValue && !item.Manager1Approval.HasValue)
            {
                item.Manager2ApprovalDate = DateTime.Now;
                item.ApproveStatus = (int)EnumINV_PermitApproveStatus.Yonetici2Onay;
            }
            if (!item.IkApproval.HasValue && !item.Manager2Approval.HasValue && !item.Manager1Approval.HasValue)
            {
                item.IkApprovalDate = DateTime.Now;
                item.ApproveStatus = (int)EnumINV_PermitApproveStatus.IkKontrol;
            }
            var dbresult = _db.InsertINV_Permit(item);
            if (!dbresult.result)
            {
                return new ResultStatus
                {
                    result = false,
                    message = "İzin talebi başarısız."
                };
            }
            var permitType = _db.GetINV_PermitTypeById(item.PermitTypeId.Value);
            string created = string.Format("{0:dd.MM.yyyy HH:mm}", item.created);
            string start = string.Format("{0:dd.MM.yyyy HH:mm}", item.StartDate);
            string end = string.Format("{0:dd.MM.yyyy HH:mm}", item.EndDate);
            string url = TenantConfig.Tenant.GetWebUrl();
            var tenantName = TenantConfig.Tenant.TenantName;
            var projects = _db.GetVWINV_CompanyPersonDepartmentsByIdUserAndType(item.IdUser.Value, EnumINV_CompanyDepartmentsType.Matrix).Where(a => a.Manager1.HasValue).Select(a => "" + a.Project_Title + " =>  Yöneticisi : " + a.Manager1_Title);
            var text = "<h3>Sayın {0},</h3>";
            text += "<p><u>{1}</u> isimli personel {2} tarihinde, {3} - {4} tarihleri arasında {5} {6} talebinde bulunmuştur.</p>";
            text += projects.Count() > 0 ? "<p>Çalıştığı proje(ler) ve yönetici(leri) : " + string.Join(" , ", projects) + "  </p>" : "";
            text += "Onaya gitmek için lütfen <a href='{7}/INV/VWINV_Permit/Update?id={8}'>Buraya tıklayınız! </a><p>Bilgilerinize.</p>";
            var nofityText = "Sayın {0}, {1} isimli personel {2} tarihinde, {3} - {4} tarihleri arasında {5} {6} talebinde bulunmuştur.";
            var notification = new Notification();
            switch ((EnumINV_PermitApproveStatus)item.ApproveStatus)
            {
                case EnumINV_PermitApproveStatus.TalepEdildi:
                    var manager1 = _db.GetVWSH_UserById(item.Manager1Approval.Value);
                    var notify = string.Format(nofityText, manager1.firstname + " " + manager1.lastname, personel.firstname + " " + personel.lastname, created, start, end, calc.Text, permitType.Name);
                    var mesaj = string.Format(text, manager1.firstname + " " + manager1.lastname, personel.firstname + " " + personel.lastname, created, start, end, calc.Text, permitType.Name, url, item.id);
                    new Email().Template("Template1", "izinMailFoto.jpg", "İzin Talebi Onayı Hakkında", mesaj).Send((Int16)EmailSendTypes.IzinOnaylari, manager1.email, string.Format("{0} | {1}", tenantName + " | WORKOFTIME", "İzin Talebi Onayı Hakkında.."), true);
                    notification.NotificationSend(manager1.id,item.createdby, "İzin Talebi Onayı Hakkında", notify);
                    break;
                case EnumINV_PermitApproveStatus.Yonetici1Onay:
                case EnumINV_PermitApproveStatus.Yonetici1Red:
                    var manager2 = _db.GetVWSH_UserById(item.Manager2Approval.Value);
                    var notify2 = String.Format(nofityText, manager2.firstname + " " + manager2.lastname, personel.firstname + " " + personel.lastname, created, start, end, calc.Text, permitType.Name);
                    var mesaj2 = string.Format(text, manager2.firstname + " " + manager2.lastname, personel.firstname + " " + personel.lastname, created, start, end, calc.Text, permitType.Name, url, item.id);
                    new Email().Template("Template1", "izinMailFoto.jpg", "İzin Talebi Onayı Hakkında", mesaj2).Send((Int16)EmailSendTypes.IzinOnaylari, manager2.email, string.Format("{0} | {1}", tenantName + " | WORKOFTIME", "İzin Talebi Onayı Hakkında.."), true);
                    notification.NotificationSend(manager2.id, item.createdby, "İzin Talebi Onayı Hakkında", notify2);
                    break;
                case EnumINV_PermitApproveStatus.Yonetici2Onay:
                case EnumINV_PermitApproveStatus.Yonetici2Red:
                    var notify3 = String.Format(nofityText, IKYonetici.firstname + " " + IKYonetici.lastname, personel.firstname + " " + personel.lastname, created, start, end, calc.Text, permitType.Name);
                    var mesaj3 = string.Format(text, IKYonetici.firstname + " " + IKYonetici.lastname, personel.firstname + " " + personel.lastname, created, start, end, calc.Text, permitType.Name, string.Join(" , ", projects), url, item.id);
                    new Email().Template("Template1", "izinMailFoto.jpg", "İzin Talebi Onayı Hakkında", mesaj3).Send((Int16)EmailSendTypes.IzinOnaylari, IKYonetici.email, string.Format("{0} | {1}", tenantName + " | WORKOFTIME", "İzin Talebi Onayı Hakkında.."), true);
                    notification.NotificationSend(IKYonetici.id, item.createdby, "İzin Talebi Onayı Hakkında", notify3);
                    break;
                case EnumINV_PermitApproveStatus.IkKontrol:
                    var textmy = @"<h3>Sayın {0},</h3> 
                    <p>{1} tarihinde talep ettiğiniz <u> {2} - {3} </u> tarihleri arasındaki {4} {5} yöneticileriniz ve insan kaynakları tarafından onaylanmıştır.</p>
                    <p>İzin sürecinizin devam edebilmesi için ıslak imzalı izin formunuzu yüklemelisiniz. Yüklemek için <a href='{6}/INV/VWINV_Permit/MyIndex'>Buraya tıklayınız! </a><br/>Bilgilerinize.<br>İyi Çalışmalar.</p>";
                    var notify4 = string.Format("Sayın {0}, {1} tarihinde talep ettiğiniz {2} - {3}  tarihleri arasındaki {4} {5} yöneticileriniz ve insan kaynakları tarafından onaylanmıştır. İzin sürecinizin devam edebilmesi için ıslak imzalı izin formunuzu yüklemelisiniz", personel.firstname + " " + personel.lastname, DateTime.Now, start, end, calc.Text, permitType.Name);
                    var mesaj4 = string.Format(textmy, personel.firstname + " " + personel.lastname, DateTime.Now, start, end, calc.Text, permitType.Name, url);
                    new Email().Template("Template1", "izinMailFoto.jpg", "İzin Talebi Onayı Hakkında", mesaj4).Send((Int16)EmailSendTypes.IzinSurecTamamlama, personel.email, string.Format("{0} | {1}", tenantName + " | WORKOFTIME", "İzin Talebi Hakkında.."), true);
                    notification.NotificationSend(personel.id, item.createdby, "İzin Talebi Onayı Hakkında", mesaj4);
                    break;
                case EnumINV_PermitApproveStatus.IkKontrolRed:
                    var textmy1 = @"<h3>Sayın {0},</h3> 
                    <p>{1} tarihinde talep ettiğiniz <u> {2} - {3} </u> tarihleri arasındaki {4} {5} yöneticileriniz ve insan kaynakları tarafından reddedilmiştir.</p>
              </br>Blgilerinize.<br>İyi Çalışmalar.</p>";
                    var notify5 = string.Format("Sayın {0}, {1} tarihinde talep ettiğiniz {2} - {3}  tarihleri arasındaki {4} {5} yöneticileriniz ve insan kaynakları tarafından reddedilmiştir. ", personel.firstname + " " + personel.lastname, DateTime.Now, start, end, calc.Text, permitType.Name);
                    var mesaj5= string.Format(textmy1, personel.firstname + " " + personel.lastname, DateTime.Now, start, end, calc.Text, permitType.Name, url);
                    new Email().Template("Template1", "izinMailFoto.jpg", "İzin Talebi Onayı Hakkında", mesaj5).Send((Int16)EmailSendTypes.IzinSurecTamamlama, personel.email, string.Format("{0} | {1}", tenantName + " | WORKOFTIME", "İzin Talebi Hakkında.."), true);
                    notification.NotificationSend(personel.id, item.createdby, "İzin Talebi Onayı Hakkında", mesaj5);
                    break;
                case EnumINV_PermitApproveStatus.IslakImzaYuklendi:
                    break;
                case EnumINV_PermitApproveStatus.GecmisIzin:
                    break;
                case EnumINV_PermitApproveStatus.SaglikRaporu:
                    break;
                case EnumINV_PermitApproveStatus.AvansIzin:
                    break;
                default:
                    break;
            }
            return new ResultStatus
            {
                result = true,
                message = "İzin talebi başarı ile gerçekleştirildi."
            };
        }
    }
    public class VWINV_PermitForm : VWINV_Permit
    {
        public VWSH_User User { get; set; }
        public VWINV_CompanyPersonDepartments Department { get; set; }
        public CalculateReturn Calc { get; set; }
        public string Url { get; set; }
        public string LogoPath { get; set; }
        public List<VWUT_RulesUserStage> RulesUserStage { get; set; }
    }
}
