using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infoline.WorkOfTime.BusinessAccess
{
    [EnumInfo(typeof(INV_CompanyPersonCalendar), "Type")]
    public enum EnumINV_CompanyPersonCalendarType
    {
        //  INV_PermitOffical
        [Generic("color", "A4243B" , "desc", "Resmi izinler")]
        [Description("Resmi İzin")] 
        Resmi = 0,

        //  INV_Permit
        [Generic("color", "D8973C", "desc", "Genel izinler")]
        [Description("Genel İzinler")]  
        Genel = 1,

        //  INV_CompanyPerson
        [Generic("color", "00BEFF", "desc", "İş Giriş/Çıkışları")]
        [Description("İş Giriş-Çıkış")] 
        IsGirisCikis = 2,

        //  INV_CompanyPersonDepartments
        [Generic("color", "ED6A5E", "desc", "Departman Giriş/Çıkışları")]
        [Description("Departman Giriş-Çıkışları")] 
        DepartmanGirisCikis = 3,

        //  SH_User
        [Generic("color", "048A81", "desc", "Doğum günleri")]
        [Description("Doğum Günleri")]   
        DogumGun = 4,

        //  PRJ_Project
        [Generic("color", "FF0000", "desc", "Proje")]
        [Description("Proje")]                    
        Proje = 5,

        //  INV_CommissionsPersons
        [Generic("color", "ff9966", "desc", "Görevlendirme")]
        [Description("Görevlendirme")] 
        Gorevlendirme = 6,

        //  INV_Permit
        [Generic("color", "33cc33", "desc", "Sağlık raporu")]
        [Description("Sağlık Raporu")]                  //  #33cc33
        Saglik = 7,

        //Insertde Kullanılacak  Burası INV_CompanyPersonCalendar
        [Generic("color", "3F51B5", "desc", "Hatırlatma")]
        [Description("Hatırlatma")]  
        Hatirlatma = 100,
        [Generic("color", "6276E6", "desc", "Not")]
        [Description("Not")]
        Not = 101,
        [Generic("color", "44CFD4", "desc", "Toplantı")]
        [Description("Toplantı")]   
        Toplanti = 102,
        [Generic("color", "ff8800")]
        [Description("Duyuru")]                         //  
        Duyuru = 103,
        [Generic("color", "ff6a9d")]
        [Description("Tebrik")]
        Tebrik = 104,
        [Generic("color", "23465c")]
        [Description("Sunum")]
        Sunum = 105,
        [Generic("color", "5e4474")]
        [Description("Etkinlik")]
        Etkinlik = 106,
        [Generic("color", "#5b3729")]
        [Description("İş Mülakatı")]
        IsGorusmesi = 15,
        [Generic("color", "e5d5c1")]
        [Description("Gant")]
        Gant = 20,
        [Generic("color", "ffd700")]
        [Description("Satış Toplantısı")]
        SatisToplantisi = 8,
        [Generic("color", "c5e6a6")]
        [Description("Yeni Proje Personeli")]
        NewProjectPerson = 10,
        [Generic("color", "fc0330")]
        [Description("Sertifika")]
        Sertifika = 11,
        [Generic("color", "41676b", "desc", "Eğitim")]
        [Description("Eğitim")]
        Eğitim = 12

        //[Generic("color", "1ba1e2")]
        //[Description("Outlook")]
        //Outlook = 50

        //Saha Görevleri = 58
    }

    partial class WorkOfTimeDatabase
    {
        public INV_CompanyPersonCalendar[] GetINV_CompanyPersonCalendarScheduled(DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.ExecuteReader<INV_CompanyPersonCalendar>("SELECT id,StartDate FROM INV_CompanyPersonCalendar with(nolock) where StartDate > GETDATE() and StartDate < DATEADD(day,1,GETDATE())").ToArray();
            }
        }

    }

}
