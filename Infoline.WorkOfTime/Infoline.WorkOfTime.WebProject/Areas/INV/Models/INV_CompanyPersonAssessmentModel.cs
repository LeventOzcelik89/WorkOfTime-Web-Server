using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Infoline.WorkOfTime.WebProject.Areas.INV.Models
{
    public class INV_CompanyPersonAssessmentModel
    {
        public Dictionary<int, string[]> QuestionsByAssessmentType = new Dictionary<int, string[]>()
        {
            {2, new string[] { "İşe uyum göstermek", "Görevin gerektirdiği bilgi ve beceri düzeyine sahip olmak",
                "Verilen iş emirlerini yerine getirmek", "Hatasız iş yapmak, iş kalitesi", "Gelişime açıklık",
                "İşin gerektirdiği performans düzeyine sahip olmak", "Görev ve sorumluluk bilincinde olmak",
                "Çalışma arkadaşları ve diğer bölümler ile iletişim", "İşyeri kurallarına uymak", "Çalışma saatlerine özen göstermek",
                "Güvenilirlik", "İşçi sağlığı ve iş güvenliği konusunda bilinçli olmak" } },
            {6,new string[] { "İşe uyum göstermek", "Görevin gerektirdiği bilgi ve beceri düzeyine sahip olmak",
                "Verilen iş emirlerini yerine getirmek", "Hatasız iş yapmak, iş kalitesi", "Sorun çözme ve insiyatif kullanmak",
                "İşin gerektirdiği performans düzeyine sahip olmak", "Görev ve sorumluluk bilincinde olmak", "İşe ve kuruma bağlılık",
                "Çalışma arkadaşları ve diğer bölümler ile iletişim", "İşyeri kurallarına uymak", "Çalışma saatlerine özen göstermek",
                "Güvenilirlik", "Raporlama", "İşçi sağlığı ve iş güvenliği konusunda bilinçli olmak",
                "Yöneticilik Becerileri (Yönetici Pozisyonunda ise)" } },
            {12, new string[] { "İşe uyum göstermek", "Görevin gerektirdiği bilgi ve beceri düzeyine sahip olmak",
                "Verilen iş emirlerini yerine getirmek", "Hatasız iş yapmak, iş kalitesi", "Sorun çözme ve insiyatif kullanmak",
                "İşin gerektirdiği performans düzeyine sahip olmak", "Görev ve sorumluluk bilincinde olmak", "İşe ve kuruma bağlılık",
                "Çalışma arkadaşları ve diğer bölümler ile iletişim", "İşyeri kurallarına uymak", "Çalışma saatlerine özen göstermek",
                "Güvenilirlik", "Raporlama", "İşçi sağlığı ve iş güvenliği konusunda bilinçli olmak",
                "Yöneticilik Becerileri (Yönetici Pozisyonunda ise)" } }

        };
    }
}