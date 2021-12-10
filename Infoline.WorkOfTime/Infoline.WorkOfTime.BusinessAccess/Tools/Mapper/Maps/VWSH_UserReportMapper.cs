using Infoline.WorkOfTime.BusinessData;
using System;

namespace Infoline.WorkOfTime.BusinessAccess.Tools.Mapper.Maps
{
    public class VWSH_UserReportMapper:MapperBase<VWSH_UserReport>
    {
        public VWSH_UserReportMapper()
        {
            Map(a => a.fullName, "Ad Soyad", "Kişisel Bilgiler");
            Map(a => a.Gender_Title, "Cinsiyet", "Kişisel Bilgiler");
            Map(a => a.loginname, "Kullanıcı Adı/Kodu", "Kişisel Bilgiler");
            Map(a => a.email, "Kurumsal Eposta", "Kişisel Bilgiler");
            Map(a => a.companyOfficePhone, "Kurumsal Sabit Telefon", "Kişisel Bilgiler");
            Map(a => a.companyOfficePhoneCode, "Dahili Kod", "Kişisel Bilgiler");
            Map(a => a.companyCellPhone, "Cep Telefonu (Kurumsal)", "Kişisel Bilgiler");
            Map(a => a.personalEmail, "Eposta (Kişisel)", "Kişisel Bilgiler");
            Map(a => a.phone, "Kurumsal Sabit Telefon", "Kişisel Bilgiler");
            Map(a => a.cellphone, "Telefon (Kişisel)", "Kişisel Bilgiler");
            Map(a => a.locationId_Title, "Ülke/İl/İlçe", "Kişisel Bilgiler");
            Map(a => a.address, "Açık Adres", "Kişisel Bilgiler");
            Map(a => a.personLanguages, "Yabancı Diller", "Kişisel Bilgiler");
            Map(a => a.workExperiences, "İş Deneyimleri", "Kişisel Bilgiler");
            Map(a => a.EmergencyPerson, "Acil Durumu Kişisi", "Kişisel Bilgiler");
            Map(a => a.EmergencyPhone, "Acil Durum Telefonu", "Kişisel Bilgiler");
            Map(a => a.City_Title, "İl", "Kişisel Bilgiler");
            Map(a => a.Town_Title, "İlçe", "Kişisel Bilgiler");
            Map(a => a.personCertificates, "Sertifikalar", "Kişisel Bilgiler");



            Map(a => a.IdentificationNumber, "Kimlik Numarası", "Kimlik Bilgileri", SHRoles.IKYonetici);
            Map(a => a.birthday, "Doğum Tarihi", "Kimlik Bilgileri");
            Map(a => a.nationality, "Uyruk", "Kimlik Bilgileri");
        
            Map(a => a.FatherName, "Baba Adı", "Kimlik Bilgileri");
            Map(a => a.MotherName, "Anne Adı", "Kimlik Bilgileri");
           
            Map(a => a.BornLocation_Title, "Doğum Yeri", "Kimlik Bilgileri");
            Map(a => a.MaritalStatus_Title, "Medeni Hal", "Kimlik Bilgileri");
            Map(a => a.IDBloodGroup_Title, "Kan Grubu", "Kimlik Bilgileri");
          



           
            Map(a => a.Military_DoneDate, "Askerlik Terhis Tarihi", "Askerlik Bilgileri");
            Map(a => a.ProbationDetail, "Askerlik Tecil Sebebi", "Askerlik Bilgileri");
            Map(a => a.Probation_Date, "Askerlik Tecil Tarihi", "Askerlik Bilgileri");
            Map(a => a.Military_Title, "Askerlik Durumu", "Askerlik Bilgileri");


            Map(a => a.primarySchool, "İlkokul", "Öğrenim Bilgileri");
            Map(a => a.midSchool, "Ortaokul", "Öğrenim Bilgileri");
            Map(a => a.highSchool, "Lise", "Öğrenim Bilgiler");
            Map(a => a.Degree, "Lisans", "Öğrenim Bilgileri");
            Map(a => a.twoYearDegree, "Ön Lisans", "Öğrenim Bilgileri");
            Map(a => a.Degree, "Lisans", "Öğrenim Bilgileri");
            Map(a => a.MasterDegree, "Yüksek Lisans", "Öğrenim Bilgileri");
            Map(a => a.Phd, "Doktora", "Öğrenim Bilgileri");
            Map(a => a.docent, "Doçent", "Öğrenim Bilgileri");
            Map(a => a.Prof, "Profesör", "Öğrenim Bilgileri");
            Map( a => a.lastGratutedSchool, "Son Mezun Olduğu Okul", "Öğrenim Bilgiler");
            Map( a => a.gratutedSchooles, "Mezun Olduğu Okullar", "Öğrenim Bilgiler");
       


       
            Map(a => a.jobStartDate, "İş Başlangıç Tarihi", "Çalışan Bilgileri");
            Map(a => a.jobEndDate, "İşten Ayrılış Tarihi", "Çalışan Bilgileri");
            Map(a => a.jobLeavingDescription, "İşten Ayrılış Sebebi", "Çalışan Bilgileri");

            
            Map(a => a.Type_Title, "Çalışan Tipi", "Çalışan Bilgileri");
            Map(a => a.currentCompanyName, "Şirket", "Çalışan Bilgileri");
          
            Map(a => a.deparmanTitle, "Departman", "Çalışan Bilgileri");
           
          
            Map(a => a.title, "Ünvan", "Çalışan Bilgileri");
      
     
    
            Map(a => a.manager1_Title, "1. Yönetici", "Çalışan Bilgileri");
            Map(a => a.manager2_Title, "2. Yönetici", "Çalışan Bilgileri");
            Map(a => a.manager3_Title, "3. Yönetici", "Çalışan Bilgileri");
            Map(a => a.manager4_Title, "4. Yönetici", "Çalışan Bilgileri");
            Map(a => a.manager5_Title, "5. Yönetici", "Çalışan Bilgileri");
            Map(a => a.manager6_Title, "6. Yönetici", "Çalışan Bilgileri");
            Map(a => a.salary, "Maaş", "Çalışan Bilgileri");
            Map(a => a.personGroups, "Gruplar", "Çalışan Bilgileri");
            Map(a => a.PersonWorking, "Çalışma Gün Sayısı", "Çalışan Bilgileri");

        }
    }
}
