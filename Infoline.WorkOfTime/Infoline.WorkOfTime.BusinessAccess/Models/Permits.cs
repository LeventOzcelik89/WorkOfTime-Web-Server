using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.Linq;
namespace Infoline.WorkOfTime.BusinessAccess.Mobile
{
    public class Permits
    {
        public ResultStatus PermitInsert(INV_Permit data, Guid userId)
        {
            var db = new WorkOfTimeDatabase();
            var calc = INV_PermitCalculator.Calculate(data);
            data.created = DateTime.Now;
            data.createdby = userId;
            data.PermitCode = BusinessExtensions.B_GetIdCode();
            data.TotalDays = calc.TotalDay;
            data.TotalHours = calc.TotalHour;
            var personel = db.GetVWSH_UserById(data.IdUser.Value);
            var IKYonetici = db.GetSH_UserByRoleId(SHRoles.IKYonetici).OrderBy(a => a.CompanyId == personel.CompanyId).FirstOrDefault();
            var validate = INV_PermitCalculator.Validate(data, personel, IKYonetici);
            var isIK = IKYonetici.id == userId ? true : false;
            if (validate.result == false)
            {
                return new ResultStatus { result = false, message = validate.message.ToString() };
            }
            var compPersonDept = db.GetVWINV_CompanyPersonDepartmentsByIdUserAndType(data.IdUser.Value, EnumINV_CompanyDepartmentsType.Organization).FirstOrDefault();
            if (compPersonDept == null)
            {
                return new ResultStatus { result = false, message = "Yönetici ataması yapılmamış.lütfen sistem yöneticinizle görüşünüz." };
            }
            if (IKYonetici == null)
            {
                return new ResultStatus { result = false, message = "IK Yöneticisi ataması yapılmamış.lütfen sistem yöneticinizle görüşünüz." };
            }
            data.IkApproval = IKYonetici.id;
            data.Manager1Approval = compPersonDept != null ? compPersonDept.Manager1 : null;
            data.Manager2Approval = compPersonDept != null ? compPersonDept.Manager2 : null;
            if (!data.Manager1Approval.HasValue)
            {
                data.Manager1ApprovalDate = DateTime.Now;
                data.Manager2ApprovalDate = DateTime.Now;
                data.ApproveStatus = (int)EnumINV_PermitApproveStatus.Yonetici2Onay;
            }
            var dbresult = db.InsertINV_Permit(data);
            if (!dbresult.result)
            {
                return new ResultStatus { result = false, message = "İzin talebi başarısız." };
            }
            var notification = new Notification();
            var permitType = db.GetINV_PermitTypeById(data.PermitTypeId.Value);
            string created = string.Format("{0:dd.MM.yyyy HH:mm}", data.created);
            string start = string.Format("{0:dd.MM.yyyy HH:mm}", data.StartDate);
            string end = string.Format("{0:dd.MM.yyyy HH:mm}", data.EndDate);
            var url = TenantConfig.Tenant.GetWebUrl();
            var tenantName = TenantConfig.Tenant.TenantName;
            var projects = db.GetVWINV_CompanyPersonDepartmentsByIdUserAndType(data.IdUser.Value, EnumINV_CompanyDepartmentsType.Matrix).Where(a => a.Manager1.HasValue).Select(a => "" + a.Project_Title + " =>  Yöneticisi : " + a.Manager1_Title);
            var text = "<h3>Sayın {0},</h3>";
            text += "<p><u>{1}</u> isimli personel {2} tarihinde, {3} - {4} tarihleri arasında {5} {6} talebinde bulunmuştur.</p>";
            text += projects.Count() > 0 ? "<p>Çalıştığı proje(ler) ve yönetici(leri) : " + string.Join(" , ", projects) + "  </p>" : "";
            text += "Onaya gitmek için lütfen <a href='{7}/INV/VWINV_Permit/Update?id={8}'>Buraya tıklayınız! </a><p>Bilgilerinize.</p>";
            var notifyText = "Sayın {0}, {1} isimli personel {2} tarihinde, {3} - {4} tarihleri arasında {5} {6} talebinde bulunmuştur.";

            if (dbresult.result)
            {
                if (data.Manager1Approval.HasValue)
                {
                    var manager1 = db.GetVWSH_UserById(data.Manager1Approval.Value);
                    var mesaj = string.Format(text, manager1.firstname + " " + manager1.lastname, personel.firstname + " " + personel.lastname, created, start, end, calc.Text, permitType.Name, url, data.id);
                    var notify = string.Format(notifyText, manager1.firstname + " " + manager1.lastname, personel.firstname +" " + personel.lastname,created,start,end,calc.Text,permitType.Name);
                    new Email().Template("Template1", "izinMailFoto.jpg", "İzin Talebi Onayı Hakkında", mesaj)
                     .Send((Int16)EmailSendTypes.IzinOnaylari, manager1.email, string.Format("{0} | {1}", tenantName + " | WORKOFTIME", "İzin Talebi Onayı Hakkında.."), true);
                    notification.NotificationSend(manager1.id, "İzin Talebi Onayı Hakkında",notify);
                    
                }
                else
                {
                    if (IKYonetici != null)
                    {
                        var mesaj = string.Format(text, IKYonetici.firstname + " " + IKYonetici.lastname, personel.firstname + " " + personel.lastname, created, start, end, calc.Text, permitType.Name, string.Join(" , ", projects), url, data.id);
                        var notify = string.Format(notifyText, IKYonetici.firstname + " " + IKYonetici.lastname, personel.firstname + " " + personel.lastname, created, start, end, calc.Text, permitType.Name);
                        new Email().Template("Template1", "izinMailFoto.jpg", "İzin Talebi Onayı Hakkında", mesaj)
                           .Send((Int16)EmailSendTypes.IzinOnaylari, IKYonetici.email, string.Format("{0} | {1}", tenantName + " | WORKOFTIME", "İzin Talebi Onayı Hakkında.."), true);
                        notification.NotificationSend(IKYonetici.id, "İzin Talebi Onayı Hakkında", notify);
                    }
                }
            }
            return new ResultStatus { result = true, message = "İzin talebi başarı ile gerçekleştirildi." };
        }
        public ResultStatus Calculate(INV_Permit data)
        {
            var calc = INV_PermitCalculator.Calculate(data);
            return new ResultStatus { result = true, objects = calc };
        }
        public ResultStatus<Splitted<VWINV_Permit>> MyPermitsAbout(Guid userid)
        {
            var db = new WorkOfTimeDatabase();
            var asdata = db.GetVWINV_Permit();
            var bekleyen = asdata.Where(a =>
                (a.Manager1Approval == userid && a.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.TalepEdildi) ||
                (a.Manager2Approval == userid && a.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.Yonetici1Onay) ||
                (a.IkApproval == userid && a.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.Yonetici2Onay)
                ).ToArray();
            var onayladigim = asdata.Where(a =>
                 (a.Manager1Approval == userid && (a.ApproveStatus == ((Int32)EnumINV_PermitApproveStatus.Yonetici1Onay) ||
                 a.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.Yonetici2Onay || a.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.IslakImzaYuklendi || a.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.IkKontrol)) ||
                 (a.Manager2Approval == userid && (a.ApproveStatus == ((Int32)EnumINV_PermitApproveStatus.Yonetici2Onay) ||
                 a.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.IslakImzaYuklendi || a.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.IkKontrol)) ||
                 (a.IkApproval == userid && (a.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.IkKontrol || a.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.IslakImzaYuklendi))).ToArray();
            var reddettigim = asdata.Where(a =>
                             (a.Manager1Approval == userid && a.ApproveStatus == ((Int32)EnumINV_PermitApproveStatus.Yonetici1Red)) ||
                             (a.Manager2Approval == userid && a.ApproveStatus == ((Int32)EnumINV_PermitApproveStatus.Yonetici2Red)) ||
                             (a.IkApproval == userid && a.ApproveStatus == ((Int32)EnumINV_PermitApproveStatus.IkKontrolRed))).ToArray();
            return new ResultStatus<Splitted<VWINV_Permit>>
            {
                result = true,
                objects = new Splitted<VWINV_Permit>
                {
                    Approved = onayladigim,
                    Waiting = bekleyen,
                    Declined = reddettigim
                }
            };
        }
        public SummaryHeadersPermit GetMyAboutPermitsSummary(Guid userid)
        {
            var db = new WorkOfTimeDatabase();
            return db.GetVWINV_PermitAboutByIdUserCounts(userid);
        }
        public ResultStatus<Splitted<VWINV_Permit>> MyPermits(Guid userId)
        {
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWINV_PermitByIdUser(userId);
            var bekleyen = data.Where((c => c.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.TalepEdildi || c.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.Yonetici1Onay || c.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.Yonetici2Onay || c.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.IkKontrol)).OrderByDescending(x => x.StartDate).ToArray();
            var onaylanan = data.Where((c => c.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.IslakImzaYuklendi || c.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.GecmisIzin || c.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.SaglikRaporu || c.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.AvansIzin)).OrderByDescending(x => x.StartDate).ToArray();
            var reddedilen = data.Where((c => c.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.IkKontrolRed || c.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.Yonetici1Red || c.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.Yonetici2Red)).OrderByDescending(x => x.StartDate).ToArray();
            return new ResultStatus<Splitted<VWINV_Permit>>
            {
                result = true,
                objects = new Splitted<VWINV_Permit>
                {
                    Approved = onaylanan,
                    Waiting = bekleyen,
                    Declined = reddedilen
                }
            };
        }
        public SummaryHeadersPermit GetMyPermitsSummary(Guid userid)
        {
            var db = new WorkOfTimeDatabase();
            return db.GetVWINV_PermitByIdUserCounts(userid);
        }
        public ResultStatus PermitUpdate(INV_Permit item, Guid userId)
        {
            var db = new WorkOfTimeDatabase();
            var notification = new Notification();
            if (item == null)
            {
                return new ResultStatus { result = false, message = "Boş nesne gönderemezsiniz." };
            }
            if (CallContext.Current == null)
            {
                return new ResultStatus { result = false, message = "Kullanıcı girişi yapmanız gerekmektedir." };
            }
            if (item.id == null)
            {
                return new ResultStatus { result = false, message = "Id alanı boş olamaz." };
            }
            var permit = db.GetVWINV_PermitById(item.id);
            var user = db.GetVWSH_UserById(permit.IdUser.Value);
            permit.changed = DateTime.Now;
            permit.changedby = userId;
            permit.ApproveStatus = item.ApproveStatus;
            permit.ApproveDetail = item.ApproveDetail;
            if (permit.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.Yonetici1Onay || permit.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.Yonetici1Red)
            {
                permit.Manager1ApprovalDate = DateTime.Now;
                if (permit.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.Yonetici1Onay && !permit.Manager2Approval.HasValue)
                {
                    permit.ApproveStatus = (Int32)EnumINV_PermitApproveStatus.Yonetici2Onay;
                    permit.Manager2ApprovalDate = DateTime.Now;
                }
            }
            if (permit.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.Yonetici2Onay || permit.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.Yonetici2Red)
            {
                permit.Manager2ApprovalDate = DateTime.Now;
            }
            if (permit.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.IkKontrol || permit.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.IkKontrolRed)
            {
                permit.IkApprovalDate = DateTime.Now;
            }
            var insankaynaklari = db.GetSH_UserByRoleId(SHRoles.IKYonetici).OrderBy(a => a.CompanyId == user.CompanyId).FirstOrDefault();
            var dbresults = db.UpdateINV_Permit(new INV_Permit().B_EntityDataCopyForMaterial(permit, true));
            if (!dbresults.result)
            {
                return new ResultStatus { result = false, message = dbresults.message };
            }
            var permitUser = db.GetVWSH_UserById(permit.IdUser.Value);
            var start = string.Format("{0:dd.MM.yyyy HH:mm}", permit.StartDate);
            var end = string.Format("{0:dd.MM.yyyy HH:mm}", permit.EndDate);
            var changed = string.Format("{0:dd.MM.yyyy HH:mm}", permit.changed);
            var calc = INV_PermitCalculator.Calculate(new INV_Permit().B_EntityDataCopyForMaterial(permit, true));
            var url = TenantConfig.Tenant.GetWebUrl();
            var tenantName = TenantConfig.Tenant.TenantName;
            if (permit.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.IslakImzaYuklendi && permit.IdUser == userId & insankaynaklari != null)
            {
                var text = @"<h3>Sayın {0},</h3> 
                <p><u>{1}</u> isimli personel {2} tarihinde, {3} - {4} tarihleri arasında onaylanan izni için ıslak imzalı dosyasını yüklemiştir. </p>
                <p>Kontrol etmek için lütfen <a href='{5}/INV/VWINV_Permit/Detail?id={6}'>Buraya tıklayınız! </a></p>
                <p>Bilgilerinize.<br>İyi Çalışmalar.</p>";
                var mesaj = string.Format(text, permit.IkApproval_Title, permit.Person_Title, changed, start, end, url, permit.id);
                var notify = string.Format("Sayın {0}, {1} isimli personel {2} tarihinde, {3} - {4} tarihleri arasında onaylanan izni için ıslak imzalı dosyası yüklenmiştir.", permit.IkApproval_Title, permit.Person_Title, changed, start, end);
                new Email().Template("Template1", "working.jpg", "İzin Talebi Islak İmzalı Dosyası Hakkında", mesaj)
                .Send((Int16)EmailSendTypes.IzinOnaylari, insankaynaklari.email, string.Format("{0} | {1}", tenantName + " | WORKOFTIME", "İzin Talebi Islak İmzalı Dosyası Hakkında.."), true);
                notification.NotificationSend(insankaynaklari.id, "İzin Talebi Islak İmzalı Dosyası Hakkında", notify);
                return new ResultStatus { result = true, message = "Dosya yükleme işleminiz başarı ile gerçekleşti." };
            }
            if (permit.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.Yonetici1Onay)
            {
                var manager2 = db.GetVWSH_UserById(permit.Manager2Approval.Value);
                var text = @"<h3>Sayın {0},</h3>
                <p><u>{1}</u> isimli personel {2} tarihinde, {3} - {4} tarihleri arasında {5} {6} talebinde bulunmuştur.</p>
                <p>Onaya g itmek için lütfen <a href='{7}/INV/VWINV_Permit/Update?id={8}'>Buraya tıklayınız! </a><br/>Bilgilerinize.</p>";
                var notify = string.Format("Sayın {0}, {1} isimli personel {2} tarihinde, {3} - {4} tarihleri arasında {5} {6} talebinde bulunmuştur.", permit.Manager2Approval_Title, permit.Person_Title, changed, start, end, calc.Text, permit.PermitType_Title);
                var mesaj = string.Format(text, permit.Manager2Approval_Title, permit.Person_Title, changed, start, end, calc.Text, permit.PermitType_Title, url, permit.id);
                new Email().Template("Template1", "izinMailFoto.jpg", "İzin Talebi Onayı Hakkında", mesaj)
                  .Send((Int16)EmailSendTypes.IzinOnaylari, manager2.email, string.Format("{0} | {1}", tenantName + " | WORKOFTIME", "İzin Talebi Onayı Hakkında.."), true);
                notification.NotificationSend(manager2.id, "İzin Talebi Onayı Hakkında", notify);
            }
            if (permit.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.Yonetici2Onay & insankaynaklari != null)
            {
                var text = @"<h3>Sayın {0},</h3>
                <p><u>{1}</u> isimli personel {2} tarihinde, {3} - {4} tarihleri arasında {5} {6} talebinde bulunmuştur.</p>
                <p>Yöneticileri <strong>{7}</strong> ve <strong>{8}</strong> tarafından onaylanmıştır.Sistem üzerinden kontrolünüz beklenmektedir.</p>
                <p>Onaya gitmek için lütfen <a href='{9}/INV/VWINV_Permit/Update?id={10}'>Buraya tıklayınız! </a><br/>Bilgilerinize.</p>";
                var mesaj = string.Format(text, permit.IkApproval_Title, permit.Person_Title, changed, start, end, calc.Text, permit.PermitType_Title, permit.Manager1Approval_Title, permit.Manager2Approval_Title, url, permit.id);
                var notify = string.Format("Sayın {0}, {1} isimli personel {2} tarihinde, {3} - {4} tarihleri arasında {5} {6} talebinde bulunmuştur.", permit.IkApproval_Title, permit.Person_Title, changed, start, end, calc.Text, permit.PermitType_Title);
                new Email().Template("Template1", "izinMailFoto.jpg", "İzin Talebi Onayı Hakkında", mesaj)
               .Send((Int16)EmailSendTypes.IzinOnaylari, insankaynaklari.email, string.Format("{0} | {1}", tenantName + " | WORKOFTIME", "İzin Talebi Onayı Hakkında.."), true);
                notification.NotificationSend(insankaynaklari.id, "İzin Talebi Onayı Hakkında", notify);
            }
            if (permit.ApproveStatus==(Int32)EnumINV_PermitApproveStatus.Yonetici1Red)
            {
                var text = @"<h3>Sayın {0},</h3>
                <p>{1} tarihinde talep ettiğiniz <u> {2} - {3} </u> tarihleri arasındaki {4} yöneticiniz tarafından <font color='red'>reddedilmiştir.</font></p>
                <p>Bilgilerinize.<br>İyi Çalışmalar.</p>";
                var notify = string.Format("Sayın {0}, {1} tarihinde talep ettiğiniz {2} - {3} tarihleri arasındaki {4} tarafından reddedilmiştir.", permit.Person_Title, changed, start, end, calc.Text);
                var mesaj = string.Format(text, permit.Person_Title, changed, start, end, calc.Text);
                new Email().Template("Template1", "izinMailFoto.jpg", "İzin Talebi Onayı Hakkında", mesaj)
                  .Send((Int16)EmailSendTypes.IzinOnaylari, permitUser.email, string.Format("{0} | {1}", tenantName + " | WORKOFTIME", "İzin Talebi Hakkında.."), true);
                notification.NotificationSend(permit.id, "İzin Talebi Onayı Hakkında", notify);
            }
            if (permit.ApproveStatus==(Int32)EnumINV_PermitApproveStatus.Yonetici2Red)
            {
                var text = @"<h3>Sayın {0},</h3>
                <p>{1} tarihinde talep ettiğiniz <u> {2} - {3} </u> tarihleri arasındaki {4} yöneticiniz tarafından <font color='red'>reddedilmiştir.</font></p>
                <p>Bilgilerinize.<br>İyi Çalışmalar.</p>";
                var notify = string.Format("Sayın {0}, {1} tarihinde talep ettiğiniz {2} - {3} tarihleri arasındaki {4} tarafından reddedilmiştir.", permit.Person_Title, changed, start, end, permit.PermitType_Title);
                var mesaj = string.Format(text, permit.Person_Title, changed, start, end, permit.PermitType_Title);
                new Email().Template("Template1", "izinMailFoto.jpg", "İzin Talebi Onayı Hakkında", mesaj)
                  .Send((Int16)EmailSendTypes.IzinOnaylari, permitUser.email, string.Format("{0} | {1}", tenantName + " | WORKOFTIME", "İzin Talebi Hakkında.."), true);
                notification.NotificationSend(permit.id, "İzin Talebi Onayı Hakkında", notify);
            }
            if (permit.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.IkKontrolRed)
            {
                var text = @"<h3>Sayın {0},</h3>
                <p>{1} tarihinde talep ettiğiniz <u> {2} - {3} </u> tarihleri arasındaki {4} {5}, insan kaynakları tarafından <font color='red'>reddedilmiştir.</font></p>
                <p>Bilgilerinize.<br>İyi Çalışmalar.</p>";
                var mesaj = string.Format(text, permit.Person_Title, changed, start, end, calc.Text, permit.PermitType_Title);
                var notify = string.Format("Sayın {0}, {1} tarihinde talep ettiğiniz {2} - {3} tarihleri arasındaki {4} {5}, insan kaynakları tarafından reddedilmiştir", permit.Person_Title, changed, start, end, calc.Text, permit.PermitType_Title);
                new Email().Template("Template1", "izinMailFoto7-.jpg", "İzin Talebi Onayı Hakkında", mesaj)
                 .Send((Int16)EmailSendTypes.IzinOnaylari, permitUser.email, string.Format("{0} | {1}", tenantName + " | WORKOFTIME", "İzin Talebi Hakkında.."), true);
                notification.NotificationSend(permitUser.id, "İzin Talebi Onayı Hakkında", notify);
            }
            if (permit.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.IkKontrol)
            {
                var text = @"<h3>Sayın {0},</h3> 
                    <p>{1} tarihinde talep ettiğiniz <u> {2} - {3} </u> tarihleri arasındaki {4} {5}  yöneticileriniz {6} ve insan kaynakları tarafından onaylanmıştır.</p>
                    <p>İzin sürecinizin devam edebilmesi için ıslak imzalı izin formunuzu yüklemelisiniz. Yüklemek için <a href='{7}/INV/VWINV_Permit/MyIndex'>Buraya tıklayınız! </a><br/>Bilgilerinize.<br>İyi Çalışmalar.</p>";
                var notify = string.Format("Sayın {0}, {1} tarihinde talep ettiğiniz {2} - {3} tarihleri arasındaki {4} {5}  izniniz onaylanmıştır. İzin sürecinizin devam edebilmesi için ıslak imzalı izin formunuzu yüklemelisiniz.", permit.Person_Title, changed, start, end, calc.Text, permit.PermitType_Title);
                var mesaj = string.Format(text, permit.Person_Title, changed, start, end, calc.Text, permit.PermitType_Title, permit.Manager1Approval_Title + "," + permit.Manager2Approval_Title, url);
                new Email().Template("Template1", "izinMailFoto.jpg", "İzin Talebi Onayı Hakkında", mesaj)
                    .Send((Int16)EmailSendTypes.IzinSurecTamamlama, permitUser.email, string.Format("{0} | {1}", tenantName + " | WORKOFTIME", "İzin Talebi Hakkında.."), true);
                notification.NotificationSend(permitUser.id, "İzin Talebi Onayı Hakkında", notify);
            }
            return new ResultStatus { result = true, message = "İzin yanıtlama işlemi başarılı" };
        }
    }
}
