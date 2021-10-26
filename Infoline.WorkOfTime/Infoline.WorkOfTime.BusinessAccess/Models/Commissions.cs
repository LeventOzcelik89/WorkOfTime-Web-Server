using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Infoline.WorkOfTime.BusinessAccess.Mobile
{
    public class Commissions
    {
        public ResultStatus CommissionsInsert(INV_CommissionModel item, Guid userId)
        {
            var db = new WorkOfTimeDatabase();
            item.created = DateTime.Now;
            item.createdby = userId;
            item.CommissionCode = BusinessExtensions.B_GetIdCode();

            var dbresult = new List<ResultStatus>();
            item.ApproveStatus = (Int32)EnumINV_CommissionsApproveStatus.Bekliyor;

            var totals = INV_PermitCalculator.Calculate(new INV_Permit
            {
                StartDate = item.StartDate,
                EndDate = item.EndDate,
                PermitTypeId = INV_PermitCalculator.mazeretIzin
            });

            item.TotalDays = totals.TotalDay;
            item.TotalHours = totals.TotalHour;

            if (item.IdUsers == null)
            {
                return new ResultStatus { result = false, message = "Hiç Personel seçimi yapılmadı !..." };
            }

            if (item.StartDate == item.EndDate)
            {
                return new ResultStatus { result = false, message = "Tarih aralığını kontrol edin." };
            }



            //if (item.TravelInformation == (Int32)EnumINV_CommissionsTravelInformation.Arac && item.IdCompanyCar.HasValue)
            //{
            //    return new ResultStatus { result = false, message = "Görevlendirmeyi araç ile gerçekleştirdiğinizden dolayı araç seçmelisiniz." };
            //}



            var personList = item.IdUsers.Select(x => new INV_CommissionsPersons
            {
                id = Guid.NewGuid(),
                created = DateTime.Now,
                createdby = userId,
                IdCommisions = item.id,
                IdUser = x,
                IsOwner = userId == x ? Convert.ToBoolean((Int32)EnumINV_CommissionsPersonsIsOwner.Evet) : Convert.ToBoolean((Int32)EnumINV_CommissionsPersonsIsOwner.Hayir),
            });


            if (personList.Where(x => x.IsOwner == true).Count() == 0)
            {
                return new ResultStatus { result = false, message = "Kendinizi seçmeden görevlendirme talep edemezsiniz.Lütfen kendinizi ekleyerek tekrar talep gerçekleştiriniz." };
            }

            var owner = personList.Where(a => a.IsOwner == true).Select(a => a.IdUser.Value).FirstOrDefault();


            var personelPosition = db.GetVWINV_CompanyPersonDepartmentsByIdUserAndType(owner, EnumINV_CompanyDepartmentsType.Organization).FirstOrDefault();
            if (personelPosition != null && personelPosition.Manager1.HasValue)
            {
                item.Manager1Approval = personelPosition.Manager1;
            }
            else
            {
                item.ApproveStatus = (Int32)EnumINV_CommissionsApproveStatus.Onaylandi;
                item.Manager1ApprovalDate = DateTime.Now;
            }



            var personelsInfo = db.GetVWSH_UserByIds(personList.Select(x => x.IdUser.Value).ToArray());


            foreach (var person in personList)
            {
                var personName = personelsInfo.Where(a => a.id == person.IdUser.Value).FirstOrDefault();
                var permitControl = db.GetINV_PermitByControlDate(person.IdUser.Value, item.StartDate, item.EndDate);

                if (permitControl != null)
                {
                    return new ResultStatus { result = false, message = string.Format("{0} isimli personelin {1} - {2} tarihleri arasında izin talebi mevcuttur. !", personName.firstname + " " + personName.lastname, string.Format("{0:dd.MM.yyyy HH:mm}", permitControl.StartDate), string.Format("{0:dd.MM.yyyy HH:mm}", permitControl.EndDate)) };
                }

                var commissionControl = db.GetVWINV_CommissionsPersonsByControlDate(person.IdUser.Value, item.StartDate, item.EndDate);

                if (commissionControl != null)
                {
                    return new ResultStatus { result = false, message = string.Format("{0} isimli personelin {1} - {2} tarihleri arasında görev talebi mevcuttur. !", personName.firstname + " " + personName.lastname, string.Format("{0:dd.MM.yyyy HH:mm}", commissionControl.StartDate), string.Format("{0:dd.MM.yyyy HH:mm}", commissionControl.EndDate)) };
                }
            }



            var trans = db.BeginTransaction();
            if (item.IdProject != null && item.IdProject.Count() > 0)
            {
                var projectList = item.IdProject.Select(x => new INV_CommissionsProjects
                {
                    id = Guid.NewGuid(),
                    created = DateTime.Now,
                    createdby = userId,
                    IdCommissions = item.id,
                    IdProject = x
                }).ToArray();

                var yuzdelik = Math.Round(Convert.ToDouble(100) / Convert.ToDouble(projectList.Count()), 1);
                foreach (var tile in projectList)
                {
                    tile.Percentile = yuzdelik;
                }

                dbresult.Add(db.BulkInsertINV_CommissionsProjects(projectList, trans));

            }

            dbresult.Add(db.InsertINV_Commissions(item, trans));
            dbresult.Add(db.BulkInsertINV_CommissionsPersons(personList, trans));


            if (dbresult.Count(a => a.result == false) > 0)
            {
                trans.Rollback();
                return new ResultStatus { result = false, message = "Görev formu kaydetme işlemi başarısız. Lütfen Daha Sonra Tekrar Deneyiniz." };
            }
            trans.Commit();

            var notification = new Notification();
            var ownerInfo = personelsInfo.Where(a => a.id == owner).FirstOrDefault();
            var created = string.Format("{0:dd.MM.yyyy HH:mm}", item.created);
            var start = string.Format("{0:dd.MM.yyyy HH:mm}", item.StartDate);
            var end = string.Format("{0:dd.MM.yyyy HH:mm}", item.EndDate);
            var personels = string.Join(", ", personelsInfo.Select(x => x.firstname + " " + x.lastname).ToArray());

            var siteName = TenantConfig.Tenant.GetWebUrl();

            if (item.Manager1Approval.HasValue)
            {
                var companyPerson = db.GetVWSH_UserById(item.Manager1Approval.Value);
                var notifyText = "Sayın {0}, {1} isimli personel {2} tarihinde, {3} - {4} tarihleri arasında";

                var text = "<h3>Sayın {0},</h3>";
                text += "<p><u>{1}</u> isimli personel {2} tarihinde, {3} - {4} tarihleri arasında ";
                if (personelsInfo.Count() > 1)
                {
                    text += personels + " personelleri için ";
                    notifyText += personels + " personelleri için";
                }
                text += "görevlendirme talebinde bulunmuştur.</p> ";
                notifyText += "görevlendirme talebinde bulunmuştur";
                text += "Onaya gitmek için lütfen <a href='{5}/INV/VWINV_Commissions/Update?id={6}'>Buraya tıklayınız! </a><p>Bilgilerinize.</p>";
                var mesaj = string.Format(text, companyPerson.FullName, ownerInfo.FullName, created, start, end, siteName, item.id);
                var notify = string.Format(notifyText,companyPerson.FullName,ownerInfo.FullName,created,start,end);
                new Email().Template("Template1", "gorevMailFoto.jpg", "Görev Talep Onayı Hakkında", mesaj)
                               .Send((Int16)EmailSendTypes.GorevlendirmeOnaylari, companyPerson.email, "Görevlendirme Onayı Hakkında..", true);
                notification.NotificationSend(companyPerson.id, "Görev Talep Onayı Hakkında",notify);
            }
            else
            {

                foreach (var person in personelsInfo)
                {
                    var notifyText = "Sayın {0},";
                    var text = "<h3>Sayın {0},</h3>";
                    if (ownerInfo.id == person.id)
                    {
                        notifyText += "{1} tarihinde, {2} - {3} tarihleri arasında  oluşturduğunuz görevlendirme talebi sistem tarafından otomatik onaylanmıştır.";
                        text += "<p>{1} tarihinde, {2} - {3} tarihleri arasında  oluşturduğunuz görevlendirme talebi sistem tarafından otomatik onaylanmıştır.</p>";
                    }
                    else
                    {
                        notifyText += ownerInfo.FullName + " tarafından adınıza {1} tarihinde, {2} - {3} tarihleri arasında oluşturduğu görevlendirme talebi sistem tarafından otomatik onaylanmıştır.";
                        text += "<p> " + ownerInfo.FullName + " tarafından adınıza {1} tarihinde, {2} - {3} tarihleri arasında oluşturduğu görevlendirme talebi sistem tarafından otomatik onaylanmıştır.</p>";
                    }

                    text += "Islak imzalı görevlendirme formunu yüklemek için lütfen <a href='{4}/INV/VWINV_Commissions/MyIndex'>Buraya tıklayınız! </a><p>Bilgilerinize.</p>";

                    var mesaj = string.Format(text, person.FullName, created, start, end, siteName);
                    var notify = string.Format(notifyText,person.FullName,created,start,end);
                    new Email().Template("Template1", "gorevMailFoto.jpg", "Görev Talep Onayı Hakkında", mesaj)
                              .Send((Int16)EmailSendTypes.GorevlendirmeSurecTamamlama, person.email, "Görevlendirme Onayı Hakkında..", true);
                    notification.NotificationSend(person.id, "Görev Talep Onayı Hakkında",notify);
                }

            }

            return new ResultStatus { result = true, message = "Görev kaydetme işlemi başarı ile gerçekleşti" };
        }

        public ResultStatus CommissionsUpdate(INV_Commissions item, Guid userId)
        {
            var db = new WorkOfTimeDatabase();
            var notification = new Notification();
            item.changed = DateTime.Now;
            item.changedby = userId;
            var commision = db.GetVWINV_CommissionsById(item.id);
            if (commision == null)
            {
                return new ResultStatus { result = false, message = "Görevlendirme bulunamadı.Silinmiş olabilir." };
            }

            commision.changed = DateTime.Now;
            commision.changedby = userId;
         

            if (commision.ApproveStatus == (Int32)EnumINV_CommissionsApproveStatus.Onaylandi || commision.ApproveStatus == (Int32)EnumINV_CommissionsApproveStatus.Reddedildi)
            {
                commision.Manager1ApprovalDate = DateTime.Now;
            }
            var persons = db.GetVWINV_CommissionsPersonCommissionIds(item.id);

            var url = TenantConfig.Tenant.GetWebUrl();
            string pattern = @"<[^>]*?>";
            var insankaynaklariAll = db.GetSH_UserByRoleId(SHRoles.IKYonetici);
            //Yönetici Onayı
            if (userId == commision.Manager1Approval && commision.ApproveStatus == (Int32)EnumINV_CommissionsApproveStatus.Bekliyor)
            {
                commision.ApproveStatus = item.ApproveStatus;

                foreach (var insankaynaklari in insankaynaklariAll)
                {
                    commision.Manager1ApprovalDate = DateTime.Now;
                    commision.ApproveStatus = item.ApproveStatus;
                    var text = "<h3>Sayın İnsan Kaynakları Yöneticisi,</h3>";
                    text += "<p>{0} - {1} tarihleri arasında  {2} adlı personel(ler) görevlendirme gerçekleştirecektir.</p>";
                    text += "<p>Seyahati gerçekleştireceği adres : {3} </p>";
                    text += "<p>Bilgilerinize.</p>";
                    var notifyText = "Sayın İnsan Kaynakları Yöneticisi, {0} - {1} tarihleri arasında  {2} adlı personel(ler) görevlendirme gerçekleştirecektir. ";
                    var asUser = persons.Where(x => x.IdUser != null).Select(x => x.IdUser.Value).ToArray();
                    var userMail = db.GetVWSH_UserByIds(asUser);
                    var userss = string.Join(", ", userMail.Select(x => x.FullName));
                    var siteName = url;
                    var notify = string.Format(notifyText, string.Format("{0:dd.MM.yyyy HH:mm}", commision.StartDate), string.Format("{0:dd.MM.yyyy HH:mm}", commision.EndDate), userss);
                    var mesaj = string.Format(text, string.Format("{0:dd.MM.yyyy HH:mm}", commision.StartDate), string.Format("{0:dd.MM.yyyy HH:mm}", commision.EndDate), userss, string.IsNullOrEmpty(commision.ToAdress) ? "Belirtilmemiş" : commision.ToAdress, siteName);
                    new Email().Template("Template1", "gorevMailFoto.jpg", "Görevlendirmeye Çıkacak Personeller Hakkında", mesaj).Send((Int16)EmailSendTypes.GorevlendirmeOnaylari, insankaynaklari.email, "Görevlendirmeye Çıkacak Personeller Hakkında", true);
                    notification.NotificationSend(insankaynaklari.id, "Görevlendirmeye Çıkacak Personeller Hakkında",notify);
                }
            }


            //Islak İmza Yükleme
            if (persons.Count(a => a.IdUser == userId && !string.IsNullOrEmpty(a.Files)) > 0 && commision.ApproveStatus == (Int32)EnumINV_CommissionsApproveStatus.Onaylandi)
            {
                //File işlemi sor
                //var rsFile = new FileUploadSave(Request).SaveAs();
                persons = db.GetVWINV_CommissionsPersonCommissionIds(item.id);
                if (persons.Count(a => string.IsNullOrEmpty(a.Files)) == 0)
                {
                    commision.ApproveStatus = (Int32)EnumINV_CommissionsApproveStatus.IslakImzaYuklendi;
                    //    commision.ApproveStatus_Title = ((EnumINV_CommissionsApproveStatus)commision.ApproveStatus).ToDescription();
                }
            }


            var dbresult = db.UpdateINV_Commissions(new INV_Commissions().B_EntityDataCopyForMaterial(commision, true));

            if (!dbresult.result)
            {
                return new ResultStatus { result = false, message = "Görev yanıtlandırma işlemi başarısız.Lütfen sistem yöneticinizle görüşünüz." };
            }


            var personsInfo = db.GetVWSH_UserByIds(persons.Select(a => a.IdUser.Value).ToArray());
            var owner = persons.Where(a => a.IsOwner == true).FirstOrDefault();
            var ownerInfo = personsInfo.Where(a => a.id == owner.IdUser).FirstOrDefault();
            var created = string.Format("{0:dd.MM.yyyy HH:mm}", commision.created);
            var start = string.Format("{0:dd.MM.yyyy HH:mm}", commision.StartDate);
            var end = string.Format("{0:dd.MM.yyyy HH:mm}", commision.EndDate);


            if (ownerInfo == null)
            {
                return new ResultStatus { result = false, message = "Görev talebini yapan personel bulunamadı." };
            }



            if (commision.ApproveStatus == (Int32)EnumINV_CommissionsApproveStatus.Reddedildi && userId == commision.Manager1Approval)
            {

                var personels = String.Join(", ", personsInfo.Select(a => a.FullName).ToArray());

                var text = "<h3>Sayın {0},</h3>";
                text += "<p>{1} tarihinde, {2} - {3} tarihleri arasında oluşturduğunuz görevlendirme talebi yöneticiniz {5} tarafından reddedildi.</p>";
                text += "<p>Bilgilerinize.</p>";
                var mesaj = string.Format(text, ownerInfo.FullName, created, start, end, personels, commision.Manager1Approval_Title);

                var notify= Regex.Replace(text,pattern,"");
                notify = string.Format(notify, ownerInfo.FullName, created, start, end, personels, commision.Manager1Approval_Title);
                new Email().Template("Template1", "gorevMailFoto.jpg", "Görev Talep Onayı Hakkında", mesaj)
                        .Send((Int16)EmailSendTypes.GorevlendirmeOnaylari, ownerInfo.email, "Görevlendirme Onayı Hakkında..", true);
                notification.NotificationSend(ownerInfo.id,"Görev Talep Onayı Hakkında",notify);
            }
            if (commision.ApproveStatus == (Int32)EnumINV_CommissionsApproveStatus.Onaylandi && userId == commision.Manager1Approval)
            {
                foreach (var person in personsInfo)
                {
                    var text = "<h3>Sayın {0},</h3>";
                    if (ownerInfo.id == person.id)
                    {
                        text += "<p>{1} tarihinde, {2} - {3} tarihleri arasında  oluşturduğunuz görevlendirme talebi onaylanmıştır.</p>";
                    }
                    else
                    {
                        text += "<p> " + ownerInfo.FullName + " tarafından adınıza {1} tarihinde, {2} - {3} tarihleri arasında oluşturduğu görevlendirme talebi onaylanmıştır.</p>";
                    }
                    var notify = Regex.Replace(text, pattern, "");
                    notify = string.Format(notify+ " Islak imzalı görevlendirme formunu yüklemeniz beklenmektedir.", person.FullName, created, start, end);
                    notification.NotificationSend(person.id, "Görev Talep Onayı Hakkında", notify);
                    text += "Islak imzalı görevlendirme formunu yüklemek için lütfen <a href='{4}/INV/VWINV_Commissions/MyIndex'>Buraya tıklayınız! </a><p>Bilgilerinize.</p>";
                    var mesaj = string.Format(text, person.FullName, created, start, end, url);
                    new Email().Template("Template1", "gorevMailFoto.jpg", "Görev Talep Onayı Hakkında", mesaj)
                       .Send((Int16)EmailSendTypes.GorevlendirmeSurecTamamlama, person.email, "Görevlendirme Onayı Hakkında..", true);               
                }
                var genelMudurAll = db.GetSH_UserByRoleId(SHRoles.IdariPersonelYonetici);
                if (ownerInfo != null)
                {
                    foreach (var genelMudur in genelMudurAll)
                    {
                        var baslik = "<h3>Sayın {0},</h3>";
                        baslik += "<p>{1} tarihinde, {2} - {3} tarihleri arasında {4} isimli personelin görevlendirme talebi onaylanmıştır.</p> ";
                       
                        var notify = Regex.Replace(baslik, pattern, "");
                        notify = string.Format(notify, (genelMudur.firstname != null ? genelMudur.firstname : " ") + " " + (genelMudur.lastname != null ? genelMudur.lastname : " "), created, start, end,ownerInfo.FullName);
                        baslik += "Görevlendirme formunu görüntülemek için lütfen <a href='{5}/INV/VWINV_Commissions/Index'>Buraya tıklayınız! </a><p>Bilgilerinize.</p>";
                        var mesajText = string.Format(baslik, (genelMudur.firstname != null ? genelMudur.firstname : " ") + " " + (genelMudur.lastname != null ? genelMudur.lastname : " "), created, start, end, ownerInfo.FullName, url);
                        new Email().Template("Template1", "gorevMailFoto.jpg", "Görev Talebi Bilgilendirmesi Hakkında", mesajText)
                          .Send((Int16)EmailSendTypes.GorevlendirmeOnaylari, genelMudur.email, "Görev Talebi Bilgilendirmesi Hakkında..", true);
                        notification.NotificationSend(genelMudur.id, "Görev Talebi Bilgilendirmesi Hakkında", notify);
                    }
               }
            }

            return new ResultStatus { result = dbresult.result, message = "Görev yanıtlama işlemi başarılı" };
        }

        public ResultStatus<Splitted<VMINV_CommissionsPersons>> MyCommissionsAbout(Guid userid)
        {
            var db = new WorkOfTimeDatabase();
            var datas = db.GetVWINV_CommissionsPersons();
            var projects = db.GetVWINV_CommissionsProjects();
            var res = new List<VMINV_CommissionsPersons>();
            foreach (var data in datas)
            {
                var item = new VMINV_CommissionsPersons().B_EntityDataCopyForMaterial(data, true);
                var proj = projects.Where(c => c.IdCommissions == data.id).ToArray();
                item.ComissionsPersons = datas.Where(a => a.id == data.id).ToArray();
                item.CommissionsProjects = proj;
                res.Add(item);
            }

            var _bekleyen = res.Where(a => a.Manager1Approval == userid && a.ApproveStatus == (Int32)EnumINV_CommissionsApproveStatus.Bekliyor && a.IsOwner == true).ToArray();
            var _onayladigim = res.Where(a => a.Manager1Approval == userid && a.IsOwner == true && (a.ApproveStatus == (Int32)EnumINV_CommissionsApproveStatus.Onaylandi || a.ApproveStatus == (Int32)EnumINV_CommissionsApproveStatus.IslakImzaYuklendi)).ToArray();
            var _reddettigim = res.Where(a => a.Manager1Approval == userid && a.ApproveStatus == (Int32)EnumINV_CommissionsApproveStatus.Reddedildi && a.IsOwner == true).ToArray();


            return new ResultStatus<Splitted<VMINV_CommissionsPersons>>
            {
                result = true,
                objects = new Splitted<VMINV_CommissionsPersons>
                {
                    Approved = _onayladigim,
                    Waiting = _bekleyen,
                    Declined = _reddettigim
                }
            };
        }

        public SummaryHeadersCommission GetMyAboutCommissionsSummary(Guid userid)
        {
            var db = new WorkOfTimeDatabase();
            return db.GetVWINV_CommissionsPersonsAboutByIdUserCounts(userid);
        }

        public ResultStatus<Splitted<VMINV_CommissionsPersons>> MBMyCommissions(Guid userid)
        {
            var db = new WorkOfTimeDatabase();
            var datas = db.GetVWINV_CommissionsPersonsByJustUserId(userid);
            var _persons = db.GetVWINV_CommissionsPersons();
            var projects = db.GetVWINV_CommissionsProjects();
            var res = new List<VMINV_CommissionsPersons>();
            foreach (var data in datas)
            {
                var item = new VMINV_CommissionsPersons().B_EntityDataCopyForMaterial(data, true);
                var proj = projects.Where(c => c.IdCommissions == data.id).ToArray();
                item.ComissionsPersons = _persons.Where(a => a.id == data.id).ToArray();
                item.CommissionsProjects = proj;
                res.Add(item);
            }

            var bekleyen = res.Where(c => c.ApproveStatus == (Int32)EnumINV_CommissionsApproveStatus.Bekliyor || c.ApproveStatus == (Int32)EnumINV_CommissionsApproveStatus.Onaylandi).ToArray();
            var onaylanan = res.Where(c => c.ApproveStatus == (Int32)EnumINV_CommissionsApproveStatus.IslakImzaYuklendi).ToArray();
            var reddedilen = res.Where(c => c.ApproveStatus == (Int32)EnumINV_CommissionsApproveStatus.Reddedildi).ToArray();

            return new ResultStatus<Splitted<VMINV_CommissionsPersons>>
            {
                result = true,
                objects = new Splitted<VMINV_CommissionsPersons>
                {
                    Approved = onaylanan,
                    Waiting = bekleyen,
                    Declined = reddedilen
                }
            };
        }

        public SummaryHeadersCommission GetMyCommissionsSummary(Guid userid)
        {
            var db = new WorkOfTimeDatabase();
            return db.GetVWINV_CommissionsPersonsByIdUserCounts(userid);
        }

        public ResultStatus CommisCalculate(INV_Commissions data)
        {
            var calc = INV_CommissionsCalculator.Calculate(data);
            return new ResultStatus { result = true, objects = calc };
        }
    }

    public class VMINV_CommissionsPersons : VWINV_CommissionsPersons
    {
        public VWINV_CommissionsPersons[] ComissionsPersons { get; set; }
        public VWINV_CommissionsProjects[] CommissionsProjects { get; set; }
    }


    public class VMINV_CommissionsProjectsAndPersons
    {
        public VWINV_CommissionsPersons[] ComissionsPersons { get; set; }
        public VWINV_CommissionsProjects[] CommissionsProjects { get; set; }
    }

    public class INV_CommissionModel : INV_Commissions
    {
        public Guid[] IdUsers { get; set; }

        public Guid[] IdProject { get; set; }
    }

  
}

