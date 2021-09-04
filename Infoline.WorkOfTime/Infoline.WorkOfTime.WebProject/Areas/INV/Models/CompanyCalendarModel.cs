using Infoline.WorkOfTime.BusinessAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Infoline.WorkOfTime.WebProject.Areas.INV.Models
{

    public class CompanyCalendarModel
    {
        public Dictionary<Enum, string> CalendarHeaDictionary = new Dictionary<Enum, string>()
        {
            {EnumINV_CompanyPersonCalendarType.Resmi, "#A4243B|Resmi İzin"},
            {EnumINV_CompanyPersonCalendarType.Genel,"#D8973C|Genel İzinler" },
            {EnumINV_CompanyPersonCalendarType.Saglik,"#33cc33|Sağlık Raporu" },
            {EnumINV_CompanyPersonCalendarType.IsGirisCikis,"#00BEFF|İş Giriş-Çıkış" },
            {EnumINV_CompanyPersonCalendarType.DepartmanGirisCikis,"#ED6A5E|Departman Giriş-Çıkışları" },
            {EnumINV_CompanyPersonCalendarType.DogumGun,"#048A81|Doğum Günleri" },
            {EnumINV_CompanyPersonCalendarType.Proje,"#FF0000|Proje" },
            {EnumINV_CompanyPersonCalendarType.Gorevlendirme,"#ff9966|Görevlendirme" },
            {EnumINV_CompanyPersonCalendarType.Hatirlatma,"#3F51B5|Hatırlatma" },
            {EnumINV_CompanyPersonCalendarType.Not,"#6276E6|Not" },
            {EnumINV_CompanyPersonCalendarType.Toplanti,"#44CFD4|Toplantı" },
            {EnumINV_CompanyPersonCalendarType.Duyuru,"#ff8800|Duyuru" },
            {EnumINV_CompanyPersonCalendarType.Tebrik,"#ff6a9d|Tebrik" },
            {EnumINV_CompanyPersonCalendarType.Sunum,"#23465c|Sunum" },
            {EnumINV_CompanyPersonCalendarType.SatisToplantisi,"#ffd700|Satış Toplantısı" },
            {EnumINV_CompanyPersonCalendarType.Etkinlik, "#5e4474|Etkinlik" },
            {EnumINV_CompanyPersonCalendarType.IsGorusmesi, "#5b3729|İş Mülakatı" },
            {EnumINV_CompanyPersonCalendarType.Gant, "#e5d5c1|Gant" },
            {EnumINV_CompanyPersonCalendarType.NewProjectPerson, "#c5e6a6|Yeni Proje Personeli" },
            //{EnumINV_CompanyPersonCalendarType.Outlook,"#1ba1e2" },
        };
    }


    public class MyCalendar
    {
        public Guid id { get; set; }
        public DateTime? start { get; set; }
        public DateTime? end { get; set; }
        public int? type { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string katilimcilar { get; set; }
        public string color { get; set; }
    }

}