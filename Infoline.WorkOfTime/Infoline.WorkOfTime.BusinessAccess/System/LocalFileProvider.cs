using Infoline.Framework.Database;
using Infoline.Framework.Helper;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Infoline.WorkOfTime.BusinessAccess
{
    public class LocalFileProvider
    {

        private readonly string _dataTable;
        //TO DO:
        private readonly string _libraryName = HttpContext.Current.Server.MapPath("~/Files"); //Başka bir dizine eklemek istiyorsanız burayı şiyapcaksınız

        public static Dictionary<string, FileBase[]> _dataTableFileGroup = new Dictionary<string, FileBase[]>()
        {
            {
                "SH_User", new[]
                {
                    new FileBase("Profil Resmi",".jpg,.jpeg,.png",1, new string[] {SHRoles.IKYonetici  },
                    new string[] {SHRoles.IKYonetici},
                     new string[] {SHRoles.IKYonetici }),
                    new FileBase("Nüfus Cüzdanı",  ".jpg,.jpeg,*.doc,*.docx,.png,*.pdf",  2, new string[] {SHRoles.IKYonetici  },
                    new string[] {SHRoles.IKYonetici},
                     new string[] {SHRoles.IKYonetici }),
                    new FileBase("Sürücü Belgesi",  ".jpg,.jpeg,*.png,*.doc,*.docx,.pdf",  5, new string[] {SHRoles.IKYonetici  },
                    new string[] {SHRoles.IKYonetici},
                     new string[] {SHRoles.IKYonetici }),
                    new FileBase("Sağlık Raporu",  ".jpg,.jpeg,*.png,*.doc,*.docx,.pdf",  99, new string[] {SHRoles.IKYonetici  },
                    new string[] {SHRoles.IKYonetici},
                     new string[] {SHRoles.IKYonetici }),
                    new FileBase("Özgeçmiş",".jpg,.jpeg,*.doc,*.docx,.png,.pdf",  1, new string[] {SHRoles.IKYonetici  },
                    new string[] {SHRoles.IKYonetici},
                     new string[] {SHRoles.IKYonetici }),
                    new FileBase("Diploma",  ".jpg,.jpeg,.png,*.doc,*.docx,.pdf",  5, new string[] {SHRoles.IKYonetici  },
                    new string[] {SHRoles.IKYonetici},
                     new string[] {SHRoles.IKYonetici }),
                    new FileBase("Personel Sözleşmeleri",  ".jpg,.jpeg,.png,*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.rar",  99, new string[] {SHRoles.IKYonetici  },
                    new string[] {SHRoles.IKYonetici},
                     new string[] {SHRoles.IKYonetici }),
                    new FileBase("İş Başvuru Formu",  ".jpg,.jpeg,.png,*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.rar",  2, new string[] {SHRoles.IKYonetici  },
                    new string[] {SHRoles.IKYonetici},
                     new string[] {SHRoles.IKYonetici }),
                    new FileBase("SGK İşe Giriş Bildirgesi",  ".jpg,.jpeg,.png,*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.rar",  1, new string[] {SHRoles.IKYonetici  },
                    new string[] {SHRoles.IKYonetici},
                     new string[] {SHRoles.IKYonetici }),
                    new FileBase("Evlilik Cüzdan Fotokopisi",  ".jpg,.jpeg,.png,*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.rar",  1, new string[] {SHRoles.IKYonetici  },
                    new string[] {SHRoles.IKYonetici},
                     new string[] {SHRoles.IKYonetici }),
                    new FileBase("Adli Sicil Kaydı Fotokopisi",  ".jpg,.jpeg,.png,*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.rar",  1, new string[] {SHRoles.IKYonetici  },
                    new string[] {SHRoles.IKYonetici},
                     new string[] {SHRoles.IKYonetici }),
                    new FileBase("Banka Hesap Cüzdanı Fotokopisi",  ".jpg,.jpeg,.png,*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.rar",  1, new string[] {SHRoles.IKYonetici  },
                    new string[] {SHRoles.IKYonetici},
                     new string[] {SHRoles.IKYonetici }),
                    new FileBase("Sağlık Raporu",  ".jpg,.jpeg,.png,*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.rar",  99, new string[] {SHRoles.IKYonetici  },
                    new string[] {SHRoles.IKYonetici},
                     new string[] {SHRoles.IKYonetici }),
                    new FileBase("İşyeri Hekimi/Periyodik Muayene Formu",  ".jpg,.jpeg,.png,*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.rar",  99, new string[] {SHRoles.IKYonetici  },
                    new string[] {SHRoles.IKYonetici},
                     new string[] {SHRoles.IKYonetici }),
                    new FileBase("Görev Tanımı",  ".jpg,.jpeg,.png,*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.rar",  5, new string[] {SHRoles.IKYonetici  },
                    new string[] {SHRoles.IKYonetici},
                     new string[] {SHRoles.IKYonetici }),
                    new FileBase("Askerlik Durum Belgesi",  ".jpg,.jpeg,.png,*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.rar",  1, new string[] {SHRoles.IKYonetici  },
                    new string[] {SHRoles.IKYonetici},
                     new string[] {SHRoles.IKYonetici }),
                    new FileBase("Kan Grubu Kartı",  ".jpg,.jpeg,.png,*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.rar",  1, new string[] {SHRoles.IKYonetici  },
                    new string[] {SHRoles.IKYonetici},
                     new string[] {SHRoles.IKYonetici }),
                    new FileBase("Aile Durum Bildirimi Formu (AGİ Formu)",  ".jpg,.jpeg,.png,*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.rar",  99, new string[] {SHRoles.IKYonetici  },
                    new string[] {SHRoles.IKYonetici},
                     new string[] {SHRoles.IKYonetici }),
                    new FileBase("Bordro Alımı Taahhütnamesi",  ".jpg,.jpeg,.png,*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.rar",  1, new string[] {SHRoles.IKYonetici  },
                    new string[] {SHRoles.IKYonetici},
                     new string[] {SHRoles.IKYonetici }),
                    new FileBase("Vukuatlı Nüfus Sureti",  ".jpg,.jpeg,.png,*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.rar",  1, new string[] {SHRoles.IKYonetici  },
                    new string[] {SHRoles.IKYonetici},
                     new string[] {SHRoles.IKYonetici }),
                    new FileBase("Disiplin Yönetmeliği",  ".jpg,.jpeg,.png,*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.rar",  1, new string[] {SHRoles.IKYonetici  },
                    new string[] {SHRoles.IKYonetici},
                     new string[] {SHRoles.IKYonetici }),
                    new FileBase("İcatlar ve Özgün Çalışmaların Temliki",  ".jpg,.jpeg,.png,*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.rar",  6, new string[] {SHRoles.IKYonetici  },
                    new string[] {SHRoles.IKYonetici},
                     new string[] {SHRoles.IKYonetici }),
                    new FileBase("Şirket Ahlak Kuralları",  ".jpg,.jpeg,.png,*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.rar",  1, new string[] {SHRoles.IKYonetici  },
                    new string[] {SHRoles.IKYonetici},
                     new string[] {SHRoles.IKYonetici }),
                    new FileBase("İş Güvenliği Şartnamesi ve Taahhütnamesi",  ".jpg,.jpeg,.png,*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.rar",  5, new string[] {SHRoles.IKYonetici  },
                    new string[] {SHRoles.IKYonetici},
                     new string[] {SHRoles.IKYonetici }),
                    new FileBase("Fazla Mesai Muvafakatnamesi",  ".jpg,.jpeg,.png,*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.rar", 99, new string[] {SHRoles.IKYonetici  },
                    new string[] {SHRoles.IKYonetici},
                     new string[] {SHRoles.IKYonetici }),
                    new FileBase("Psikoteknik Belgesi",  ".jpg,.jpeg,.png,*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.rar",  1, new string[] {SHRoles.IKYonetici  },
                    new string[] {SHRoles.IKYonetici},
                     new string[] {SHRoles.IKYonetici }),
                      new FileBase("İkametgah (Yerleşim Yeri)",  ".jpg,.jpeg,.png,*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.rar",  1, new string[] {SHRoles.IKYonetici  },
                    new string[] {SHRoles.IKYonetici},
                     new string[] {SHRoles.IKYonetici }),
                         new FileBase("Hedef Karşılaştırma Formları",  ".jpg,.jpeg,.png,*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.rar",  99, new string[] {SHRoles.IKYonetici  },
                    new string[] {SHRoles.IKYonetici},
                     new string[] {SHRoles.IKYonetici }),
                    new FileBase("SRC Belgesi",  ".jpg,.jpeg,.png,*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.rar",  1, new string[] {SHRoles.IKYonetici  },
                    new string[] {SHRoles.IKYonetici},
                     new string[] {SHRoles.IKYonetici }),
                    new FileBase("Engelli/Özürlü Belgesi",  ".jpg,.jpeg,.png,*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.rar", 1, new string[] {SHRoles.IKYonetici  },
                    new string[] {SHRoles.IKYonetici},
                     new string[] {SHRoles.IKYonetici }),
                    new FileBase("İşten Ayrılış Bildirgesi",  ".jpg,.jpeg,.png,*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.rar", 5, new string[] {SHRoles.IKYonetici  },
                    new string[] {SHRoles.IKYonetici},
                     new string[] {SHRoles.IKYonetici }),
                    new FileBase("Personel İşten Çıkış Görüşmesi",  ".jpg,.jpeg,.png,*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.rar", 5, new string[] {SHRoles.IKYonetici  },
                    new string[] {SHRoles.IKYonetici},
                     new string[] {SHRoles.IKYonetici }),
                    new FileBase("Personel İşten Ayrılış Bilgi Formu",  ".jpg,.jpeg,.png,*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.rar", 5, new string[] {SHRoles.IKYonetici  },
                    new string[] {SHRoles.IKYonetici},
                     new string[] {SHRoles.IKYonetici }),
                       new FileBase("Personel Savunma",  ".jpg,.jpeg,.png,*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.rar", 99, new string[] {SHRoles.IKYonetici  },
                    new string[] {SHRoles.IKYonetici},
                     new string[] {SHRoles.IKYonetici }),
                    new FileBase("Hukuki Evrak ve Yazışmaları",".jpg,.jpeg,.png,*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.rar", 99, new string[] {SHRoles.IKYonetici  },
                    new string[] {SHRoles.IKYonetici},
                     new string[] {SHRoles.IKYonetici }),
                    new FileBase("Diğer",".jpg,.jpeg,.png,*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.rar", 99, new string[] {SHRoles.IKYonetici  },
                    new string[] {SHRoles.IKYonetici},
                     new string[] {SHRoles.IKYonetici }),
                    new FileBase("İSG Temel Eğitim Sertifikası",".jpg,.jpeg,.png,*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.rar", 99, new string[] {SHRoles.IKYonetici  },
                    new string[] {SHRoles.IKYonetici},
                     new string[] {SHRoles.IKYonetici }),
                    new FileBase("İSG Yüksekte Çalışma Eğitim Sertifikası",".jpg,.jpeg,.png,*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.rar", 99, new string[] {SHRoles.IKYonetici  },
                    new string[] {SHRoles.IKYonetici},
                     new string[] {SHRoles.IKYonetici }),
                    new FileBase("Son Aya Ait SGK Hizmet Dökümü",".jpg,.jpeg,.png,*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.rar", 99, new string[] {SHRoles.IKYonetici  },
                    new string[] {SHRoles.IKYonetici},
                     new string[] {SHRoles.IKYonetici }),
                    new FileBase("Mesleki Yeterlilik Belgesi",".jpg,.jpeg,.png,*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.rar", 99, new string[] {SHRoles.IKYonetici  },
                    new string[] {SHRoles.IKYonetici},
                     new string[] {SHRoles.IKYonetici }),
                    new FileBase("Geçici Görevlendirme Belgesi",".jpg,.jpeg,.png,*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.rar", 99, new string[] {SHRoles.IKYonetici  },
                    new string[] {SHRoles.IKYonetici},
                     new string[] {SHRoles.IKYonetici }),
                    new FileBase("KKD Zimmet Formu",".jpg,.jpeg,.png,*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.rar", 99, new string[] {SHRoles.IKYonetici  },
                    new string[] {SHRoles.IKYonetici},
                     new string[] {SHRoles.IKYonetici }),
                }
            },

            {
                "INV_Permit", new[]
                {
                     new FileBase("İzin Talep Dosyası","*.jpg,.jpeg,.png,*.pdf",3,
                         new string[]{SHRoles.Personel },
                         new string[]{ SHRoles.Personel},
                         new string[]{ SHRoles.Personel}),
                    new FileBase("Islak İmzalı İzin Dosyası","*.jpg,.jpeg,.png,*.pdf",1,
                         new string[]{ SHRoles.Personel,SHRoles.IKYonetici },
                         new string[]{ SHRoles.IKYonetici},
                         new string[]{ SHRoles.Personel}),
                    new FileBase("Sağlık Raporu Dosyası","*.jpg,.jpeg,.png,*.pdf",99,
                         new string[]{ SHRoles.IKYonetici },
                         new string[]{ SHRoles.IKYonetici},
                         new string[]{ SHRoles.Personel})
                }
            },
            {
                "INV_CommissionsPersons", new[]
                {
                   new FileBase("Islak İmzalı Görev Dosyası","*.jpg,.jpeg,.png,*.pdf",1,
                         new string[]{SHRoles.Personel },
                         new string[]{ SHRoles.Personel},
                         new string[]{ SHRoles.Personel}),
                }
            },
              {
                "INV_CommissionsInformation", new[]
                {
                   new FileBase("Uçuş Bilgi Dosyaları","*.jpg,.jpeg,.png,*.pdf",99,
                         new string[]{SHRoles.Personel,SHRoles.IdariPersonelYonetici },
                         new string[]{ SHRoles.Personel,SHRoles.IdariPersonelYonetici},
                         new string[]{ SHRoles.Personel,SHRoles.IdariPersonelYonetici}),
                    new FileBase("Konaklama Bilgi Dosyaları","*.jpg,.jpeg,.png,*.pdf",99,
                         new string[]{ SHRoles.Personel,SHRoles.IdariPersonelYonetici },
                         new string[]{ SHRoles.Personel,SHRoles.IdariPersonelYonetici},
                         new string[]{ SHRoles.Personel,SHRoles.IdariPersonelYonetici}),
                    new FileBase("Kiralık Araç Bilgi Dosyaları","*.jpg,.jpeg,.png,*.pdf",99,
                         new string[]{ SHRoles.Personel,SHRoles.IdariPersonelYonetici },
                         new string[]{ SHRoles.Personel,SHRoles.IdariPersonelYonetici},
                         new string[]{ SHRoles.Personel,SHRoles.IdariPersonelYonetici}),
                      new FileBase("Shuttle Bilgi Dosyaları","*.jpg,.jpeg,.png,*.pdf",99,
                         new string[]{ SHRoles.Personel,SHRoles.IdariPersonelYonetici },
                         new string[]{ SHRoles.Personel,SHRoles.IdariPersonelYonetici},
                         new string[]{ SHRoles.Personel,SHRoles.IdariPersonelYonetici}),
                    new FileBase("Otobüs Bilgi Dosyaları","*.jpg,.jpeg,.png,*.pdf",99,
                         new string[]{ SHRoles.Personel,SHRoles.IdariPersonelYonetici },
                         new string[]{ SHRoles.Personel,SHRoles.IdariPersonelYonetici},
                         new string[]{ SHRoles.Personel,SHRoles.IdariPersonelYonetici}),
                      new FileBase("Şirket Aracı Bilgi Dosyaları","*.jpg,.jpeg,.png,*.pdf",99,
                         new string[]{ SHRoles.Personel,SHRoles.IdariPersonelYonetici },
                         new string[]{ SHRoles.Personel,SHRoles.IdariPersonelYonetici},
                         new string[]{ SHRoles.Personel,SHRoles.IdariPersonelYonetici})
                }
            },
            {
                "SH_PersonCertificate", new[]
                {
                    new FileBase("Sertifika Dosyası", "*.doc,*.docx,*.xls,*.png,*.pdf,*.jpg,*.jpeg", 99, new string[] {SHRoles.IKYonetici }, new string[] {SHRoles.IKYonetici }, new string[] {SHRoles.IKYonetici }),
                }
            },
            {
                "SH_PersonLanguages", new[]
                {
                    new FileBase("Sertifika Dosyası", "*.doc,*.docx,*.xls,*.png,*.pdf,*.jpg,*.jpeg", 99, new string[] {SHRoles.IKYonetici }, new string[] {SHRoles.IKYonetici }, new string[] {SHRoles.IKYonetici }),
                }
            },
             {
                "INV_CompanyPersonAssessment", new[]
                {
                    new FileBase("Islak İmzalı Değerlendirme Dosyası","*.doc,*.docx,*.xls,*.png,*.pdf,*.jpg,*.jpeg", 1, new string[] {SHRoles.IKYonetici  },
                    new string[] {SHRoles.IKYonetici},
                     new string[] {SHRoles.IKYonetici }),
                }
            },
            {
                "PRJ_Project", new[]
                {
                    new FileBase("İhale Öncesi Toplantı Kararları (Direktörlükler Toplantısı)  ", "*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.jpg,*.jpeg,*.png,*.msg",99,new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici}),
                    new FileBase("İhale Sonrası Toplantı Kararları (Direktörlükler Toplantısı) ", "*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.jpg,*.jpeg,*.png,*.msg",99,new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici}),
                    new FileBase("Sözleşme Dosyası                                             ", "*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.jpg,*.jpeg,*.png,*.msg",99,new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici}),
                    new FileBase("Resmi Yazışmalar                                             ", "*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.jpg,*.jpeg,*.png,*.msg",99,new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici}),
                    new FileBase("Proje Beratı                                                 ", "*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.jpg,*.jpeg,*.png,*.msg",99,new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici}),
                    new FileBase("Proje Başvuru Dosyası                                        ", "*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.jpg,*.jpeg,*.png,*.msg",99,new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici}),
                    new FileBase("Proje Zaman Planı                                            ", "*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.jpg,*.jpeg,*.png,*.msg",99,new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici}),
                    new FileBase("İdari Zaman Planı                                            ", "*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.jpg,*.jpeg,*.png,*.msg",99,new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici}),
                    new FileBase("Teknik İş Planı                                              ", "*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.jpg,*.jpeg,*.png,*.msg",99,new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici}),
                    new FileBase("Analiz ve Tasarım Dokümanı                                   ", "*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.jpg,*.jpeg,*.png,*.msg",99,new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici}),
                    new FileBase("Veri Tabanı Tasarım Dokümanı                                 ", "*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.jpg,*.jpeg,*.png,*.msg",99,new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici}),
                    new FileBase("Veri Tabanı Sözlüğü                                          ", "*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.jpg,*.jpeg,*.png,*.msg",99,new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici}),
                    new FileBase("Web Servis Tanım Dokümanı                                    ", "*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.jpg,*.jpeg,*.png,*.msg",99,new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici}),
                    new FileBase("Kullanım Klavuzları                                          ", "*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.jpg,*.jpeg,*.png,*.msg",99,new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici}),
                    new FileBase("Kaynak Kod Dökümanı                                          ", "*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.jpg,*.jpeg,*.png,*.msg",99,new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici}),
                    new FileBase("Kurulum Dökümanı                                             ", "*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.jpg,*.jpeg,*.png,*.msg",99,new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici}),
                    new FileBase("Eğitim Planı Dökümanı                                        ", "*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.jpg,*.jpeg,*.png,*.msg",99,new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici}),
                    new FileBase("Eğitim İçerik Dökümanı                                       ", "*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.jpg,*.jpeg,*.png,*.msg",99,new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici}),
                    new FileBase("Test Planı                                                   ", "*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.jpg,*.jpeg,*.png,*.msg",99,new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici}),
                    new FileBase("Test Sonuç Raporu                                            ", "*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.jpg,*.jpeg,*.png,*.msg",99,new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici}),
                    new FileBase("Şartname                                                     ", "*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.jpg,*.jpeg,*.png,*.msg",99,new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici}),
                    new FileBase("Şartname Cevapları Dokümanı                                  ", "*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.jpg,*.jpeg,*.png,*.msg",99,new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici}),
                    new FileBase("İhale Dosyaları                                              ", "*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.jpg,*.jpeg,*.png,*.msg",99,new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici}),
                    new FileBase("Hakediş Başvuru Yazısı                                       ", "*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.jpg,*.jpeg,*.png,*.msg",99,new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici}),
                    new FileBase("Maliyet Analizi                                              ", "*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.jpg,*.jpeg,*.png,*.msg",99,new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici}),
                    new FileBase("Fiyat Teklifi                                                ", "*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.jpg,*.jpeg,*.png,*.msg",99,new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici}),
                    new FileBase("İdari Yazışmalar                                             ", "*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.jpg,*.jpeg,*.png,*.msg",99,new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici}),
                    new FileBase("Toplantı tutanakları                                         ", "*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.jpg,*.jpeg,*.png,*.msg",99,new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici}),
                    new FileBase("Haftalık İlerleme Raporu                                     ", "*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.jpg,*.jpeg,*.png,*.msg",99,new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici}),
                    new FileBase("Genel İlerleme Raporu                                        ", "*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.jpg,*.jpeg,*.png,*.msg",99,new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici}),
                    new FileBase("Teknik Çizim ve Dosyalar                                     ", "*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.jpg,*.jpeg,*.png,*.msg",99,new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici}),
                    new FileBase("Faaliyet Raporu                                              ", "*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.jpg,*.jpeg,*.png,*.msg",99,new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici}),
                    new FileBase("SPICE- GTD Gereksinim Tanımları Dokümanı                     ", "*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.jpg,*.jpeg,*.png,*.msg",99,new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici}),
                    new FileBase("SPICE- KDD Kullanım Durumu Dokümanı                          ", "*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.jpg,*.jpeg,*.png,*.msg",99,new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici}),
                    new FileBase("SPICE- Pazar Analizi                                         ", "*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.jpg,*.jpeg,*.png,*.msg",99,new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici}),
                    new FileBase("SPICE- Toplantı Tutanağı Formu                               ", "*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.jpg,*.jpeg,*.png,*.msg",99,new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici}),
                    new FileBase("SPICE- Kodlama Standardı                                     ", "*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.jpg,*.jpeg,*.png,*.msg",99,new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici}),
                    new FileBase("SPICE- MPP Proje Zaman Plan                                  ", "*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.jpg,*.jpeg,*.png,*.msg",99,new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici}),
                    new FileBase("SPICE- FMT Fiziksel Mimari Tasarım Raporu                    ", "*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.jpg,*.jpeg,*.png,*.msg",99,new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici}),
                    new FileBase("SPICE- FVM Fiziksel Veri Modeli                              ", "*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.jpg,*.jpeg,*.png,*.msg",99,new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici}),
                    new FileBase("SPICE- KAT Kullanıcı Arayüz Tasarım Raporu                   ", "*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.jpg,*.jpeg,*.png,*.msg",99,new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici}),
                    new FileBase("SPICE- YTT Yazılım Tasarım Tanımları                         ", "*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.jpg,*.jpeg,*.png,*.msg",99,new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici}),
                    new FileBase("SPICE- TDD Test Durumları Dokümanı                           ", "*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.jpg,*.jpeg,*.png,*.msg",99,new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici}),
                    new FileBase("SPICE- Gereksinimlerin İzlenebilirliği Formu                 ", "*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.jpg,*.jpeg,*.png,*.msg",99,new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici}),
                    new FileBase("SPICE- Müşteri Memnuniyeti Anketi                            ", "*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.jpg,*.jpeg,*.png,*.msg",99,new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici}),
                    new FileBase("SPICE- Proje Kategorizasyonu ve Metrikleri Formu             ", "*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.jpg,*.jpeg,*.png,*.msg",99,new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici}),
                    new FileBase("SPICE- Düzeltici Faaliyet İsteği Formu                       ", "*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.jpg,*.jpeg,*.png,*.msg",99,new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici}),
                    new FileBase("SPICE- İteratif Yazılım Yaşam Döngüsü                        ", "*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.jpg,*.jpeg,*.png,*.msg",99,new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici}),
                    new FileBase("SPICE- Uyarlama Tablosu Formu                                ", "*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.jpg,*.jpeg,*.png,*.msg",99,new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici}),
                    new FileBase("SPICE- Süreç Formu                                           ", "*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.jpg,*.jpeg,*.png,*.msg",99,new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici}),
                    new FileBase("SPICE- Tasarım Kriterleri                                    ", "*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.jpg,*.jpeg,*.png,*.msg",99,new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici}),
                    new FileBase("SPICE- Kılavuz Formu                                         ", "*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.jpg,*.jpeg,*.png,*.msg",99,new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici}),
                    new FileBase("SPICE- Konfigürasyon Maddeleri Listesi                       ", "*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.jpg,*.jpeg,*.png,*.msg",99,new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici}),
                    new FileBase("SPICE- Şablon Formu                                          ", "*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.jpg,*.jpeg,*.png,*.msg",99,new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici}),
                    new FileBase("SPICE- Kod Gözden Geçirme                                    ", "*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.jpg,*.jpeg,*.png,*.msg",99,new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici}),
                    new FileBase("SPICE- Denetim Soruları                                      ", "*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.jpg,*.jpeg,*.png,*.msg",99,new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici}),
                    new FileBase("SPICE- Fiziksel Konfigürasyon Denetimi Soru Listesi          ", "*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.jpg,*.jpeg,*.png,*.msg",99,new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici}),
                    new FileBase("SPICE- Fonksiyonel Konfigürasyon Denetimi Soru Listesi       ", "*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.jpg,*.jpeg,*.png,*.msg",99,new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici}),
                    new FileBase("SPICE- Denetim Raporu                                        ", "*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.jpg,*.jpeg,*.png,*.msg",99,new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici}),
                    new FileBase("SPICE- DFİ Takip Formu                                       ", "*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.jpg,*.jpeg,*.png,*.msg",99,new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici}),
                    new FileBase("SPICE- Personel Listesi                                      ", "*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.jpg,*.jpeg,*.png,*.msg",99,new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici}),
                    new FileBase("SPICE- Gereksinim Gözden Geçirme Kontrol Listesi             ", "*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.jpg,*.jpeg,*.png,*.msg",99,new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici}),
                    new FileBase("SPICE- PDR Proje Durum Raporu                                ", "*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.jpg,*.jpeg,*.png,*.msg",99,new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici}),
                    new FileBase("SPICE- DP Denetim Planı                                      ", "*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.jpg,*.jpeg,*.png,*.msg",99,new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici}),
                    new FileBase("SPICE- Konfigürasyon Durum Raporu                            ", "*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.jpg,*.jpeg,*.png,*.msg",99,new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici}),
                    new FileBase("SPICE- KGP Kalite Güvence Planı                              ", "*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.jpg,*.jpeg,*.png,*.msg",99,new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici}),
                    new FileBase("SPICE- Konfigürasyon Yönetim Planı                           ", "*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.jpg,*.jpeg,*.png,*.msg",99,new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici}),
                    new FileBase("SPICE- PKR Proje Kapanış Raporu                              ", "*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.jpg,*.jpeg,*.png,*.msg",99,new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici}),
                    new FileBase("SPICE- PYP Proje Yönetim Planı                               ", "*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.jpg,*.jpeg,*.png,*.msg",99,new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici}),
                    new FileBase("SPICE- RN Sürüm Notları                                      ", "*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.jpg,*.jpeg,*.png,*.msg",99,new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici}),
                    new FileBase("SPICE- TSR Test Sonuç Raporu                                 ", "*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.jpg,*.jpeg,*.png,*.msg",99,new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici}),
                    new FileBase("SPICE- SYP Sürüm Yönetim Planı                               ", "*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.jpg,*.jpeg,*.png,*.msg",99,new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici}),
                    new FileBase("SPICE- TP Test Planı                                         ", "*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.jpg,*.jpeg,*.png,*.msg",99,new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici}),
                    new FileBase("SPICE- DP Doğrulama Planı                                    ", "*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.jpg,*.jpeg,*.png,*.msg",99,new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici}),
                    new FileBase("SPICE- EP Entegrasyon Planı                                  ", "*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.jpg,*.jpeg,*.png,*.msg",99,new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici}),
                    new FileBase("SPICE- Eğitim Planı                                          ", "*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.jpg,*.jpeg,*.png,*.msg",99,new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici}),
                    new FileBase("SPICE- RYP Risk Yönetim Planı                                ", "*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.jpg,*.jpeg,*.png,*.msg",99,new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici}),
                    new FileBase("SPICE- Süreçler / Des / Doğrulama                            ", "*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.jpg,*.jpeg,*.png,*.msg",99,new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici}),
                    new FileBase("SPICE- Süreçler / Des / Kalite Güvence                       ", "*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.jpg,*.jpeg,*.png,*.msg",99,new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici}),
                    new FileBase("SPICE- Süreçler / Des / Dokümanların Kontrolü                ", "*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.jpg,*.jpeg,*.png,*.msg",99,new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici}),
                    new FileBase("SPICE- Süreçler / Des / Problem Yönetimi                     ", "*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.jpg,*.jpeg,*.png,*.msg",99,new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici}),
                    new FileBase("SPICE- Süreçler / Des / Değişiklik Yönetimi                  ", "*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.jpg,*.jpeg,*.png,*.msg",99,new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici}),
                    new FileBase("SPICE- Süreçler / Muh / Gereksinim Toplama Süreci            ", "*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.jpg,*.jpeg,*.png,*.msg",99,new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici}),
                    new FileBase("SPICE- Süreçler / Muh / Tasarım Süreci                       ", "*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.jpg,*.jpeg,*.png,*.msg",99,new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici}),
                    new FileBase("SPICE- Süreçler / Muh / Yazılım Geliştirme Süreci            ", "*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.jpg,*.jpeg,*.png,*.msg",99,new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici}),
                    new FileBase("SPICE- Süreçler / Muh / Entegrasyon Süreci                   ", "*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.jpg,*.jpeg,*.png,*.msg",99,new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici}),
                    new FileBase("SPICE- Süreçler / Muh / Test Süreci                          ", "*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.jpg,*.jpeg,*.png,*.msg",99,new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici}),
                    new FileBase("SPICE- Süreçler / Muh / Proje Yönetimi Süreci                ", "*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.jpg,*.jpeg,*.png,*.msg",99,new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici}),
                    new FileBase("SPICE- Süreçler / Muh / onfigürasyon Yönetimi Süreci         ", "*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.jpg,*.jpeg,*.png,*.msg",99,new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici}),
                    new FileBase("SPICE- Süreçler / Muh / Risk Yönetimi Süreci                 ", "*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.jpg,*.jpeg,*.png,*.msg",99,new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici}),
                    new FileBase("SPICE- Süreçler / Muh / Sürüm Yönetimi Süreci                ", "*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.jpg,*.jpeg,*.png,*.msg",99,new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici}),
                    new FileBase("SPICE- Süreçler / Muh / Yazılım Gereksinim Yönetimi Süreci   ", "*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.jpg,*.jpeg,*.png,*.msg",99,new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici}),
                    new FileBase("İş Bitirme Dosyası                                           ", "*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.jpg,*.jpeg,*.png,*.msg",99,new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici},new string[]{SHRoles.ProjeYonetici}),

                }
            },
            {
                "HR_Person", new[]
                {
                    new FileBase("Fotoğraf", "*.doc,*.docx,*.xls,*.png,*.pdf,*.jpg,*.jpeg",1,
                        new string [] {SHRoles.IKYonetici,SHRoles.PersonelTalebi,SHRoles.IsGorusmesiMulakat },
                        new string [] {SHRoles.IKYonetici,SHRoles.PersonelTalebi},
                        new string [] {SHRoles.IKYonetici,SHRoles.PersonelTalebi,SHRoles.IsGorusmesiMulakat }),
                    new FileBase("CV","*.doc,*.docx,*.xls,*.png,*.pdf,*.jpg,*.jpeg",5,
                        new string [] {SHRoles.IKYonetici,SHRoles.PersonelTalebi,SHRoles.IsGorusmesiMulakat },
                        new string [] {SHRoles.IKYonetici,SHRoles.PersonelTalebi },
                        new string [] {SHRoles.IKYonetici,SHRoles.PersonelTalebi,SHRoles.IsGorusmesiMulakat }),
                    new FileBase("Ek","*.doc,*.docx,*.xls,*.png,*.pdf,*.jpg,*.jpeg",5,
                        new string [] {SHRoles.IKYonetici,SHRoles.PersonelTalebi,SHRoles.IsGorusmesiMulakat },
                        new string [] {SHRoles.IKYonetici,SHRoles.PersonelTalebi },
                        new string [] {SHRoles.IKYonetici,SHRoles.PersonelTalebi,SHRoles.IsGorusmesiMulakat })
                }
            },

              {
                "CRM_Contact", new[]
                {
                    new FileBase("Toplantı Dosyası",
                                 "*.csv, *.jpeg, *.ppt, *.pptx, *.msg, *.doc,*.docx,*.xls,*.png,*.pdf,*.jpg,*.jpeg,*.xlsx,*.rar,*.gif,*.jpeg,*.mp3",
                                 99,
                                 new string[]{ SHRoles.CRMYonetici,SHRoles.SatisPersoneli,SHRoles.BayiPersoneli,SHRoles.CagriMerkezi },
                                 new string[]{ SHRoles.CRMYonetici,SHRoles.BayiPersoneli,SHRoles.CagriMerkezi },
                                 new string[]{ SHRoles.CRMYonetici,SHRoles.SatisPersoneli, SHRoles.BayiPersoneli }
                                 ),
                    new FileBase("Kartvizitler",
                                 "*.jpeg, *.ppt, *.pptx, *.msg, *.doc,*.docx,*.xls,*.png,*.pdf,*.jpg,*.jpeg,*.xlsx,*.rar,*.gif,*.jpeg",
                                 99,
                                 new string[]{ SHRoles.CRMYonetici,SHRoles.SatisPersoneli,SHRoles.BayiPersoneli,SHRoles.CagriMerkezi },
                                 new string[]{ SHRoles.CRMYonetici,SHRoles.SatisPersoneli,SHRoles.BayiPersoneli,SHRoles.CagriMerkezi },
                                 new string[]{ SHRoles.CRMYonetici,SHRoles.SatisPersoneli, SHRoles.BayiPersoneli }
                                 ),
                    new FileBase("Diğer Dosyalar",
                                 "*.csv, *.jpeg, *.ppt, *.pptx, *.msg, *.doc,*.docx,*.xls,*.png,*.pdf,*.jpg,*.jpeg,*.xlsx,*.rar,*.gif,*.jpeg,*.mp3",
                                 99,
                                 new string[]{ SHRoles.CRMYonetici,SHRoles.SatisPersoneli,SHRoles.BayiPersoneli,SHRoles.CagriMerkezi },
                                 new string[]{ SHRoles.CRMYonetici,SHRoles.SatisPersoneli,SHRoles.BayiPersoneli,SHRoles.CagriMerkezi },
                                 new string[]{ SHRoles.CRMYonetici,SHRoles.SatisPersoneli, SHRoles.BayiPersoneli }
                                 )
                }
            },
            {
                "CRM_Presentation", new [] {
                    new FileBase("Sözleşme Dosyaları",
                                 "*.csv, *.jpeg, *.ppt, *.pptx, *.msg, *.png, *.jpg,*.jpeg, *.doc, *.docx, *.xls, *.xlsx, *.rar, *.zip, *.msg, *.pdf, *.ppt, *.pptx, *.mp4, *.avi",
                                 99,
                                 new string[]{ SHRoles.CRMYonetici,SHRoles.SatisPersoneli ,SHRoles.BayiPersoneli,SHRoles.CagriMerkezi},
                                 new string[]{ SHRoles.CRMYonetici ,SHRoles.BayiPersoneli,SHRoles.CagriMerkezi},
                                 new string[] {SHRoles.CRMYonetici,SHRoles.SatisPersoneli, SHRoles.BayiPersoneli }
                                 ),
                    new FileBase("Şirket Evrakları",
                                 "*.csv, *.jpeg, *.ppt, *.pptx, *.msg, *.png, *.jpg,*.jpeg, *.doc, *.docx, *.xls, *.xlsx, *.rar, *.zip, *.msg, *.pdf, *.ppt, *.pptx, *.mp4, *.avi",
                                 99,
                                 new string[]{ SHRoles.CRMYonetici,SHRoles.SatisPersoneli,SHRoles.BayiPersoneli,SHRoles.CagriMerkezi },
                                 new string[]{ SHRoles.CRMYonetici,SHRoles.BayiPersoneli,SHRoles.CagriMerkezi },
                                 new string[] {SHRoles.CRMYonetici,SHRoles.SatisPersoneli, SHRoles.BayiPersoneli }
                                 ),
                    new FileBase("Teklifler",
                                 "*.csv, *.jpeg, *.ppt, *.pptx, *.msg, *.png, *.jpg, *.jpeg, *.doc, *.docx, *.xls, *.xlsx, *.rar, *.zip, *.msg, *.pdf, *.ppt, *.pptx, *.mp4, *.avi",
                                 99,
                                 new string[]{ SHRoles.CRMYonetici,SHRoles.SatisPersoneli,SHRoles.BayiPersoneli,SHRoles.CagriMerkezi },
                                 new string[]{ SHRoles.CRMYonetici,SHRoles.BayiPersoneli,SHRoles.CagriMerkezi },
                                 new string[] {SHRoles.CRMYonetici,SHRoles.SatisPersoneli, SHRoles.BayiPersoneli }
                                 )
                }
            },
            {
                "SH_PersonSchools", new[]
                {
                    new FileBase("Eğitim Dosyası", "*.doc,*.docx,*.xls,*.png,*.pdf,*.jpg ,*.jpeg,*.xlsx,*.rar", 99,new string [] {SHRoles.IKYonetici },new string [] {SHRoles.IKYonetici },new string [] {SHRoles.IKYonetici }),
                }
            },
             {
                "CMP_CompanyMyPersonDocument", new[]
                {
                    new FileBase("Teknik El Kitabı", "*.doc,*.docx,*.xls,*.png,*.pdf,*.jpg,*.jpeg", 1,new string [] {SHRoles.IKYonetici },new string [] {SHRoles.IKYonetici },new string [] {SHRoles.Personel }),
                    new FileBase("Çalışan El Kitabı", "*.doc,*.docx,*.xls,*.png,*.pdf,*.jpg,*.jpeg", 1,new string [] {SHRoles.IKYonetici },new string [] {SHRoles.IKYonetici },new string [] {SHRoles.Personel }),
                    new FileBase("Kişisel Verilerin Korunması Yönetmeliği",  "*.doc,*.docx,*.xls,*.png,*.pdf,*.jpg,*.jpeg,*.xlsx,*.rar",  5,new string [] {SHRoles.IKYonetici },new string [] {SHRoles.IKYonetici },new string [] {SHRoles.Personel }),
                    new FileBase("İzin Yönetmeliği",  "*.doc,*.docx,*.xls,*.png,*.pdf,*.jpg,*.jpeg,*.xlsx,*.rar",  5,new string [] {SHRoles.IKYonetici },new string [] {SHRoles.IKYonetici },new string [] {SHRoles.Personel }),
                    new FileBase("Ücret Avansı Yönetmeliği",  "*.doc,*.docx,*.xls,*.png,*.pdf,*.jpg,*.jpeg,*.xlsx,*.rar",  5,new string [] {SHRoles.IKYonetici },new string [] {SHRoles.IKYonetici },new string [] {SHRoles.Personel }),
                    new FileBase("Zimmet Yönetmeliği",  "*.doc,*.docx,*.xls,*.png,*.pdf,*.jpg,*.jpeg,*.xlsx,*.rar",  5,new string [] {SHRoles.IKYonetici },new string [] {SHRoles.IKYonetici },new string [] {SHRoles.Personel }),
                    new FileBase("Yetkinlik Bazlı Performans Değerlendirme Yönetmeliği",  "*.doc,*.docx,*.xls,*.png,*.pdf,*.jpg,*.jpeg,*.xlsx,*.rar",  5,new string [] {SHRoles.IKYonetici },new string [] {SHRoles.IKYonetici },new string [] {SHRoles.Personel }),
                    new FileBase("Eğitim Yönetmeliği",  "*.doc,*.docx,*.xls,*.png,*.pdf,*.jpg,*.jpeg,*.xlsx,*.rar",  5,new string [] {SHRoles.IKYonetici },new string [] {SHRoles.IKYonetici },new string [] {SHRoles.Personel }),
                    new FileBase("İş Gücü Planlama Seçme ve Yerleştirme Yönetmeliği",  "*.doc,*.docx,*.xls,*.png,*.pdf,*.jpg,*.jpeg,*.xlsx,*.rar",  5,new string [] {SHRoles.IKYonetici },new string [] {SHRoles.IKYonetici },new string [] {SHRoles.Personel }),
                    new FileBase("Ücret Yönetimi ve Sosyal İmkanlar Yönetmeliği",  "*.doc,*.docx,*.xls,*.png,*.pdf,*.jpg,*.jpeg,*.xlsx,*.rar",  5,new string [] {SHRoles.IKYonetici },new string [] {SHRoles.IKYonetici },new string [] {SHRoles.Personel }),
                    new FileBase("İş Etiği Kuralları Yönetmeliği",  "*.doc,*.docx,*.xls,*.png,*.pdf,*.jpg,*.jpeg,*.xlsx,*.rar",  5,new string [] {SHRoles.IKYonetici },new string [] {SHRoles.IKYonetici },new string [] {SHRoles.Personel }),
                    new FileBase("Görev ve Seyehat Yönetmeliği",  "*.doc,*.docx,*.xls,*.png,*.pdf,*.jpg,*.jpeg,*.xlsx,*.rar",  5,new string [] {SHRoles.IKYonetici },new string [] {SHRoles.IKYonetici },new string [] {SHRoles.Personel }),
                    new FileBase("Araç Kullanım Yönetmeliği",  "*.doc,*.docx,*.xls,*.png,*.pdf,*.jpg,*.jpeg,*.xlsx,*.rar",  5,new string [] {SHRoles.IKYonetici },new string [] {SHRoles.IKYonetici },new string [] {SHRoles.Personel }),
                    new FileBase("Kılık ve Kıyafet Yönetmeliği",  "*.doc,*.docx,*.xls,*.png,*.pdf,*.jpg,*.jpeg,*.xlsx,*.rar",  5,new string [] {SHRoles.IKYonetici },new string [] {SHRoles.IKYonetici },new string [] {SHRoles.Personel }),
                    new FileBase("Disiplin Kuralları Yönetmeliği",  "*.doc,*.docx,*.xls,*.png,*.pdf,*.jpg,*.jpeg,*.xlsx,*.rar",  5,new string [] {SHRoles.IKYonetici },new string [] {SHRoles.IKYonetici },new string [] {SHRoles.Personel }),
                    new FileBase("Staj Yönetmeliği",  "*.doc,*.docx,*.xls,*.png,*.pdf,*.jpg,*.jpeg,*.xlsx,*.rar",  5,new string [] {SHRoles.IKYonetici },new string [] {SHRoles.IKYonetici },new string [] {SHRoles.Personel }),
                    new FileBase("İş Sağlığı ve Güvenliği Yönetmeliği",  "*.doc,*.docx,*.xls,*.png,*.pdf,*.jpg,*.jpeg,*.xlsx,*.rar",  5,new string [] {SHRoles.IKYonetici },new string [] {SHRoles.IKYonetici },new string [] {SHRoles.Personel }),
                    new FileBase("İşten Ayrılma Yönetmeliği",  "*.doc,*.docx,*.xls,*.png,*.pdf,*.jpg,*.jpeg,*.xlsx,*.rar",  5,new string [] {SHRoles.IKYonetici },new string [] {SHRoles.IKYonetici },new string [] {SHRoles.Personel }),
                    new FileBase("Koronavirüs(Covid-19) Korunma Talimatları",  "*.doc,*.docx,*.xls,*.png,*.pdf,*.jpg,*.jpeg,*.xlsx,*.rar",  5,new string [] {SHRoles.IKYonetici },new string [] {SHRoles.IKYonetici },new string [] {SHRoles.Personel }),
                    new FileBase("Şirket ve Marka Logoları",  "*.doc,*.docx,*.xls,*.png,*.pdf,*.jpg,*.jpeg,*.xlsx,*.rar",  5,new string [] {SHRoles.IKYonetici },new string [] {SHRoles.IKYonetici },new string [] {SHRoles.Personel }),
                    new FileBase("Antetli Kağıtlar",  "*.doc,*.docx,*.xls,*.png,*.pdf,*.jpg,*.jpeg,*.xlsx,*.rar",  5,new string [] {SHRoles.IKYonetici },new string [] {SHRoles.IKYonetici },new string [] {SHRoles.Personel }),
                    new FileBase("Teklif Formatları",  "*.doc,*.docx,*.xls,*.png,*.pdf,*.jpg,*.jpeg,*.xlsx,*.rar",  5,new string [] {SHRoles.IKYonetici },new string [] {SHRoles.IKYonetici },new string [] {SHRoles.Personel }),
                    new FileBase("Kurumsal Kimlik Kılavuzu",  "*.doc,*.docx,*.xls,*.png,*.pdf,*.jpg,*.jpeg,*.xlsx,*.rar",  5,new string [] {SHRoles.IKYonetici },new string [] {SHRoles.IKYonetici },new string [] {SHRoles.Personel }),
                    new FileBase("Sunumlar",  "*.doc,*.docx,*.xls,*.png,*.pdf,*.jpg,*.jpeg,*.xlsx,*.rar",  5,new string [] {SHRoles.IKYonetici },new string [] {SHRoles.IKYonetici },new string [] {SHRoles.Personel }),
                    new FileBase("Mail formatları",  "*.doc,*.docx,*.xls,*.png,*.pdf,*.jpg,*.jpeg,*.xlsx,*.rar",  5,new string [] {SHRoles.IKYonetici },new string [] {SHRoles.IKYonetici },new string [] {SHRoles.Personel }),
                }
            },
            {
                "CMP_CompanyMyDocument", new[]
                {
                    new FileBase("Standart Şablonlar-Genel",  "*.csv, *.png, *.jpg, *.jpeg, *.doc, *.docx, *.xls, *.xlsx, *.rar, *.zip, *.pdf, *.ppt, *.pptx",  99,new string [] {SHRoles.IKYonetici },new string [] {SHRoles.IKYonetici },new string [] {SHRoles.IKYonetici }),
                    new FileBase("Standart Şablonlar-Satış",  "*.csv, *.png, *.jpg, *.jpeg, *.doc, *.docx, *.xls, *.xlsx, *.rar, *.zip, *.pdf, *.ppt, *.pptx",  99,new string [] {SHRoles.IKYonetici },new string [] {SHRoles.IKYonetici },new string [] {SHRoles.IKYonetici }),
                    new FileBase("Standart Şablonlar-Teknik",  "*.csv, *.png, *.jpg, *.jpeg, *.doc, *.docx, *.xls, *.xlsx, *.rar, *.zip, *.pdf, *.ppt, *.pptx",  99,new string [] {SHRoles.IKYonetici },new string [] {SHRoles.IKYonetici },new string [] {SHRoles.IKYonetici }),
                    new FileBase("Şirket Evrakları-Genel",  "*.csv, *.png, *.jpg, *.jpeg, *.doc, *.docx, *.xls, *.xlsx, *.rar, *.zip, *.pdf, *.ppt, *.pptx",  99,new string [] {SHRoles.IKYonetici },new string [] {SHRoles.IKYonetici },new string [] {SHRoles.IKYonetici }),
                    new FileBase("Şirket Evrakları-İdari",  "*.csv, *.png, *.jpg, *.jpeg, *.doc, *.docx, *.xls, *.xlsx, *.rar, *.zip, *.pdf, *.ppt, *.pptx",  99,new string [] {SHRoles.IKYonetici },new string [] {SHRoles.IKYonetici },new string [] {SHRoles.IKYonetici }),
                    new FileBase("Kalite Belgeleri",  "*.csv, *.png, *.jpg, *.jpeg, *.doc, *.docx, *.xls, *.xlsx, *.rar, *.zip, *.pdf, *.ppt, *.pptx",  99,new string [] {SHRoles.IKYonetici },new string [] {SHRoles.IKYonetici },new string [] {SHRoles.IKYonetici }),
                    new FileBase("Marka Tescil Belgeleri",  "*.csv, *.png, *.jpg, *.jpeg, *.doc, *.docx, *.xls, *.xlsx, *.rar, *.zip, *.pdf, *.ppt, *.pptx",  99,new string [] {SHRoles.IKYonetici },new string [] {SHRoles.IKYonetici },new string [] {SHRoles.IKYonetici }),
                    new FileBase("Patent Belgeleri",  "*.csv, *.png, *.jpg, *.jpeg, *.doc, *.docx, *.xls, *.xlsx, *.rar, *.zip, *.pdf, *.ppt, *.pptx",  99,new string [] {SHRoles.IKYonetici },new string [] {SHRoles.IKYonetici },new string [] {SHRoles.IKYonetici }),
                    new FileBase("Yerli Malı Belgeleri",  "*.csv, *.png, *.jpg, *.jpeg, *.doc, *.docx, *.xls, *.xlsx, *.rar, *.zip, *.pdf, *.ppt, *.pptx",  99,new string [] {SHRoles.IKYonetici },new string [] {SHRoles.IKYonetici },new string [] {SHRoles.IKYonetici }),
                    new FileBase("Ruhsatlar",  "*.csv, *.png, *.jpg, *.jpeg, *.doc, *.docx, *.xls, *.xlsx, *.rar, *.zip, *.pdf, *.ppt, *.pptx",  99,new string [] {SHRoles.IKYonetici },new string [] {SHRoles.IKYonetici },new string [] {SHRoles.IKYonetici }),
                    new FileBase("Hizmet Alım Sözleşmeleri",  "*.csv, *.png, *.jpg, *.jpeg, *.doc, *.docx, *.xls, *.xlsx, *.rar, *.zip, *.pdf, *.ppt, *.pptx",  99,new string [] {SHRoles.IKYonetici },new string [] {SHRoles.IKYonetici },new string [] {SHRoles.IKYonetici }),
                    new FileBase("Kiralama Sözleşmeleri",  "*.csv, *.png, *.jpg, *.jpeg, *.doc, *.docx, *.xls, *.xlsx, *.rar, *.zip, *.pdf, *.ppt, *.pptx",  99,new string [] {SHRoles.IKYonetici },new string [] {SHRoles.IKYonetici },new string [] {SHRoles.IKYonetici }),
                    new FileBase("Kurumlar Vergisi Beyannamesi",  "*.csv, *.png, *.jpg, *.jpeg, *.doc, *.docx, *.xls, *.xlsx, *.rar, *.zip, *.pdf, *.ppt, *.pptx",  99,new string [] {SHRoles.IKYonetici },new string [] {SHRoles.IKYonetici },new string [] {SHRoles.IKYonetici }),
                    new FileBase("Sanayi Sicil Belgeleri",  "*.csv, *.png, *.jpg, *.jpeg, *.doc, *.docx, *.xls, *.xlsx, *.rar, *.zip, *.pdf, *.ppt, *.pptx",  99,new string [] {SHRoles.IKYonetici },new string [] {SHRoles.IKYonetici },new string [] {SHRoles.IKYonetici }),
                    new FileBase("Kapasite Raporları",  "*.csv, *.png, *.jpg, *.jpeg, *.doc, *.docx, *.xls, *.xlsx, *.rar, *.zip, *.pdf, *.ppt, *.pptx",  99,new string [] {SHRoles.IKYonetici },new string [] {SHRoles.IKYonetici },new string [] {SHRoles.IKYonetici }),
                    new FileBase("Vekaletnameler",  "*.csv, *.png, *.jpg, *.jpeg, *.doc, *.docx, *.xls, *.xlsx, *.rar, *.zip, *.pdf, *.ppt, *.pptx",  99,new string [] {SHRoles.IKYonetici },new string [] {SHRoles.IKYonetici },new string [] {SHRoles.IKYonetici }),
                    new FileBase("Diğer Belgeler",  "*.csv, *.png, *.jpg, *.jpeg, *.doc, *.docx, *.xls, *.xlsx, *.rar, *.zip, *.pdf, *.ppt, *.pptx",  99,new string [] {SHRoles.IKYonetici },new string [] {SHRoles.IKYonetici },new string [] {SHRoles.IKYonetici }),
                }
            },
            {
            "CMP_CompanyMyWorkHealthDocument", new[]
                {
                    new FileBase {fileGroup = "İSG Sözleşmeleri", fileExtensions = "*.doc,*.docx,*.xls,*.png,*.pdf,*.jpg,*.jpeg,*.xlsx,*.rar", count = 99},
                    new FileBase {fileGroup = "Yıllık Planlar", fileExtensions = "*.doc,*.docx,*.xls,*.png,*.pdf,*.jpg,*.jpeg,*.xlsx,*.rar", count = 99},
                    new FileBase {fileGroup = "Öneri Tespit Nüshaları", fileExtensions = "*.doc,*.docx,*.xls,*.png,*.pdf,*.jpg,*.jpeg,*.xlsx,*.rar", count = 99},
                    new FileBase {fileGroup = "Eğitim Kayıtları", fileExtensions = "*.doc,*.docx,*.xls,*.png,*.pdf,*.jpg,*.jpeg,*.xlsx,*.rar", count = 99},
                    new FileBase {fileGroup = "Risk Değerlendirme", fileExtensions = "*.doc,*.docx,*.xls,*.png,*.pdf,*.jpg,*.jpeg,*.xlsx,*.rar", count = 99},
                    new FileBase {fileGroup = "Acil Durum Planları", fileExtensions = "*.doc,*.docx,*.xls,*.png,*.pdf,*.jpg,*.jpeg,*.xlsx,*.rar", count = 99},
                    new FileBase {fileGroup = "Ortam Ölçüm Ve Kontrol Belgeleri", fileExtensions = "*.doc,*.docx,*.xls,*.png,*.pdf,*.jpg,*.jpeg,*.xlsx,*.rar", count = 99},
                    new FileBase {fileGroup = "İş Kazası", fileExtensions = "*.doc,*.docx,*.xls,*.png,*.pdf,*.jpg,*.jpeg,*.xlsx,*.rar", count = 99},
                    new FileBase {fileGroup = "Sağlık-Güvenlik Planı(Varsa)", fileExtensions = "*.doc,*.docx,*.xls,*.png,*.pdf,*.jpg,*.jpeg,*.xlsx,*.rar", count = 99},
                    new FileBase {fileGroup = "İSG Kurul Kayıtları(Varsa)", fileExtensions = "*.doc,*.docx,*.xls,*.png,*.pdf,*.jpg,*.jpeg,*.xlsx,*.rar", count = 99},
                    new FileBase {fileGroup = "Personel Sertifikaları", fileExtensions = "*.doc,*.docx,*.xls,*.png,*.pdf,*.jpg,*.jpeg,*.xlsx,*.rar", count = 99},
                }
            },
            {
                "FTM_TaskOperation", new[]
                {
                    new FileBase ("Görev Dosyası", "*.csv, *.jpeg, *.ppt, *.pptx, *.msg, *.doc,*.docx,*.xls,*.png,*.pdf,*.jpg,*.jpeg,*.xlsx,*.rar,*.gif,*.jpeg,*.mp3,*.mp4", 99,
                                 new string[]{ SHRoles.SahaGorevOperator,SHRoles.SahaGorevYonetici, SHRoles.SahaGorevPersonel, SHRoles.SahaGorevMusteri,SHRoles.BayiGorevPersoneli },
                                 new string[]{ SHRoles.SahaGorevOperator,SHRoles.SahaGorevYonetici, SHRoles.SahaGorevPersonel, SHRoles.SahaGorevMusteri,SHRoles.BayiGorevPersoneli },
                                 new string[]{ SHRoles.SahaGorevOperator,SHRoles.SahaGorevYonetici, SHRoles.SahaGorevPersonel, SHRoles.SahaGorevMusteri,SHRoles.BayiGorevPersoneli  }),
                }
            },
            {
                "PRD_ProductionOperation", new[]
                {
                    new FileBase ("Üretim Dosyası", "*.jpeg, *.ppt, *.pptx,*.doc,*.docx,*.xls,*.png,*.pdf,*.jpg,*.jpeg,*.xlsx,*.rar,*.gif,*.jpeg,*.mp3,*.mp4", 99,
                                 new string[]{ SHRoles.Personel,SHRoles.Personel, SHRoles.Personel, SHRoles.Personel, SHRoles.Personel },
                                 new string[]{ SHRoles.Personel, SHRoles.Personel, SHRoles.Personel, SHRoles.Personel, SHRoles.Personel },
                                 new string[]{ SHRoles.Personel, SHRoles.Personel, SHRoles.Personel, SHRoles.Personel, SHRoles.Personel  }),
                }
            },
            {
                "CMP_Invoice", new[]
                {
                    new FileBase ("Fatura Dosyası", "*.csv, *.jpeg, *.ppt, *.pptx, *.msg, *.doc,*.docx,*.xls,*.png,*.pdf,*.jpg,*.jpeg,*.xlsx,*.rar,*.gif,*.jpeg,*.mp3", 99,
                                 new string[]{ SHRoles.OnMuhasebe, SHRoles.MuhasebeSatis, SHRoles.MuhasebeAlis, SHRoles.SatisFatura },
                                 new string[]{ SHRoles.OnMuhasebe, SHRoles.MuhasebeSatis, SHRoles.MuhasebeAlis, SHRoles.SatisFatura },
                                 new string[]{ SHRoles.OnMuhasebe, SHRoles.MuhasebeSatis, SHRoles.MuhasebeAlis, SHRoles.SatisFatura }),
                    new FileBase ("Teklif Koşul Dosyası", "*.csv, *.jpeg, *.ppt, *.pptx, *.msg, *.doc,*.docx,*.xls,*.png,*.pdf,*.jpg,*.jpeg,*.xlsx,*.rar,*.gif,*.jpeg,*.mp3", 99,
                                 new string[]{ SHRoles.SatinAlmaPersonel, SHRoles.SatisPersoneli, SHRoles.SatisPersoneli, SHRoles.CRMYonetici  },
                                 new string[]{ SHRoles.SatinAlmaPersonel, SHRoles.SatisPersoneli, SHRoles.SatisPersoneli, SHRoles.CRMYonetici  },
                                 new string[]{ SHRoles.OnMuhasebe, SHRoles.SatinAlmaOnaylayici, SHRoles.SatisOnaylayici, SHRoles.SatinAlmaPersonel, SHRoles.SatisPersoneli, SHRoles.CRMYonetici }
                        ),
                    new FileBase ("Teklif Dosyası", "*.csv, *.jpeg, *.ppt, *.pptx, *.msg, *.doc,*.docx,*.xls,*.png,*.pdf,*.jpg,*.jpeg,*.xlsx,*.rar,*.gif,*.jpeg,*.mp3", 99,
                                 new string[]{ SHRoles.SatinAlmaPersonel, SHRoles.SatisPersoneli, SHRoles.SatisPersoneli, SHRoles.CRMYonetici,SHRoles.BayiPersoneli,SHRoles.CagriMerkezi },
                                 new string[]{ SHRoles.SatinAlmaPersonel,  SHRoles.SatisPersoneli, SHRoles.SatisPersoneli, SHRoles.CRMYonetici,SHRoles.BayiPersoneli,SHRoles.CagriMerkezi },
                                 new string[]{ SHRoles.OnMuhasebe, SHRoles.SatinAlmaOnaylayici, SHRoles.SatisOnaylayici, SHRoles.SatinAlmaPersonel, SHRoles.SatisPersoneli, SHRoles.CRMYonetici, SHRoles.BayiPersoneli }
                        ),
                    new FileBase ("İrsaliye Dosyası", "*.csv, *.jpeg, *.ppt, *.pptx, *.msg, *.doc,*.docx,*.xls,*.png,*.pdf,*.jpg,*.jpeg,*.xlsx,*.rar,*.gif,*.jpeg,*.mp3", 99,
                                 new string[]{ SHRoles.DepoSorumlusu },
                                 new string[]{ SHRoles.DepoSorumlusu },
                                 new string[]{ SHRoles.DepoSorumlusu }
                        ),
                    new FileBase ("Sipariş Dosyası",  "*.csv, *.jpeg, *.ppt, *.pptx, *.msg, *.doc,*.docx,*.xls,*.png,*.pdf,*.jpg,*.jpeg,*.xlsx,*.rar,*.gif,*.jpeg,*.mp3", 99,
                                 new string[]{ SHRoles.OnMuhasebe, SHRoles.SatisPersoneli, SHRoles.CRMYonetici, SHRoles.SatisPersoneli,SHRoles.BayiPersoneli,SHRoles.CagriMerkezi },
                                 new string[]{ SHRoles.OnMuhasebe, SHRoles.SatisPersoneli, SHRoles.CRMYonetici, SHRoles.SatisPersoneli,SHRoles.BayiPersoneli,SHRoles.CagriMerkezi },
                                 new string[]{ SHRoles.OnMuhasebe, SHRoles.SatisPersoneli, SHRoles.SatisOnaylayici, SHRoles.CRMYonetici, SHRoles.SatisPersoneli, SHRoles.BayiPersoneli }
                        ),
                    new FileBase ("Talep Dosyası",  "*.csv, *.jpeg, *.ppt, *.pptx, *.msg, *.doc,*.docx,*.xls,*.png,*.pdf,*.jpg,*.jpeg,*.xlsx,*.rar,*.gif,*.jpeg,*.mp3", 99,
                                 new string[]{ SHRoles.SatinAlmaTalebi, SHRoles.SatinAlmaPersonel },
                                 new string[]{ SHRoles.SatinAlmaTalebi, SHRoles.SatinAlmaPersonel },
                                 new string[]{ SHRoles.SatinAlmaTalebi, SHRoles.SatinAlmaPersonel, SHRoles.SatisOnaylayici, SHRoles.SatinAlmaOnaylayici, SHRoles.ProjeYonetici }
                        ),
                }
            },
            {
                "PRD_Product", new[]
                {
                    new FileBase("Ürün Görseli","*.jpeg,*.png,*.jpg,*.jpeg",1,
                        new string[]{ SHRoles.StokYoneticisi, SHRoles.SatinAlmaTalebi, SHRoles.DepoSorumlusu, SHRoles.SatinAlmaPersonel, SHRoles.SatisPersoneli, SHRoles.OnMuhasebe, SHRoles.SatisPersoneli },
                        new string[]{ SHRoles.StokYoneticisi, SHRoles.SatinAlmaTalebi,SHRoles.DepoSorumlusu, SHRoles.SatinAlmaPersonel, SHRoles.SatisPersoneli, SHRoles.OnMuhasebe, SHRoles.SatisPersoneli },
                        new string[]{ SHRoles.Personel, SHRoles.SahaGorevMusteri }
                    ),
                     new FileBase("Ürün Dökümanları","*.csv, *.jpeg, *.ppt, *.pptx, *.msg, *.doc,*.docx,*.xls,*.png,*.pdf,*.jpg,*.jpeg,*.xlsx,*.rar,*.gif,*.jpeg,*.mp3",99,
                        new string[]{SHRoles.StokYoneticisi, SHRoles.SatinAlmaTalebi, SHRoles.DepoSorumlusu,SHRoles.SatinAlmaPersonel, SHRoles.SatisPersoneli, SHRoles.OnMuhasebe, SHRoles.SatisPersoneli },
                        new string[]{SHRoles.StokYoneticisi, SHRoles.SatinAlmaTalebi, SHRoles.DepoSorumlusu,SHRoles.SatinAlmaPersonel, SHRoles.SatisPersoneli, SHRoles.OnMuhasebe, SHRoles.SatisPersoneli },
                        new string[]{SHRoles.Personel, SHRoles.SahaGorevMusteri }
                    ),
                     new FileBase("Eski Görev Dosyaları", "*.csv,*.ppt,*.pptx,*.doc,*.docx,*.xls,*.xlsx,*.pdf",20000,
                                                                                 new string[]{ SHRoles.Personel ,SHRoles.Personel, SHRoles.Personel},
                                                                                 new string[]{ SHRoles.Personel ,SHRoles.Personel, SHRoles.Personel},
                                                                                 new string[]{ SHRoles.Personel,SHRoles.Personel, SHRoles.Personel}),
                }
            },
            {
                "CMP_Company", new[]
                {
                    new FileBase("İşletme Logosu", "*.jpeg,*.png,*.jpg,*.jpeg",  1,
                                                                                 new string[]{ SHRoles.Personel ,SHRoles.BayiPersoneli,SHRoles.CagriMerkezi},
                                                                                 new string[]{ SHRoles.Personel ,SHRoles.BayiPersoneli,SHRoles.CagriMerkezi},
                                                                                 new string[]{ SHRoles.Personel,SHRoles.BayiPersoneli,SHRoles.CagriMerkezi}),
                    new FileBase("İşletme Diğer Dosyalar", "*.jpeg,*.png,*.jpg,*.jpeg",  99,
                                                                                 new string[]{ SHRoles.Personel },
                                                                                 new string[]{ SHRoles.Personel },
                                                                                 new string[]{ SHRoles.Personel})
                }
            },
             {
                "CMP_CompanyCars", new[]
                {
                    new FileBase("Dosyalar", "*.csv, *.jpeg, *.ppt, *.pptx, *.doc,*.docx,*.xls,*.png,*.pdf,*.jpg,*.jpeg,*.xlsx,*.rar",  99,
                                                                                 new string[]{ SHRoles.Personel },
                                                                                 new string[]{ SHRoles.Personel },
                                                                                 new string[]{ SHRoles.Personel})
                }
            },
            {
                "PRD_Action", new []{
                    new FileBase("İşlem Dosyası", "*.csv, *.jpeg, *.ppt, *.pptx, *.doc,*.docx,*.xls,*.png,*.pdf,*.jpg,*.jpeg,*.xlsx,*.rar",  1,
                                                                                 new string[]{ SHRoles.DepoSorumlusu, SHRoles.StokYoneticisi, SHRoles.SatisPersoneli },
                                                                                 new string[]{ SHRoles.DepoSorumlusu, SHRoles.StokYoneticisi, SHRoles.SatisPersoneli },
                                                                                 new string[]{ SHRoles.Personel}),
                }
            },
            {
                "PRD_Transaction", new []{
                    new FileBase("İşlem Dosyası", "*.jpeg,*.doc,*.docx,*.png,*.pdf,*.jpg,*.jpeg",  5,
                                                                                new string[]{ SHRoles.DepoSorumlusu, SHRoles.StokYoneticisi, SHRoles.SatisPersoneli,SHRoles.IKYonetici },
                                                                                new string[]{ SHRoles.DepoSorumlusu, SHRoles.StokYoneticisi, SHRoles.SatisPersoneli,SHRoles.IKYonetici },
                                                                                new string[]{ SHRoles.Personel}),
                }
            },
            {
                "INV_CompanyPersonCalendar", new []{
                    new FileBase("Takvim Dosyası", "*.csv, *.jpeg, *.ppt, *.pptx, *.doc,*.docx,*.xls,*.png,*.pdf,*.jpg,*.jpeg,*.xlsx,*.rar",  99,
                                                                                 new string[]{ SHRoles.Personel},new string[]{ SHRoles.Personel},new string[]{ SHRoles.Personel}),
                }
            },
            {
                "PA_Ledger", new []{
                    new FileBase("Dekont Dosyası", "*.csv, *.jpeg, *.ppt, *.pptx, *.doc,*.docx,*.xls,*.png,*.pdf,*.jpg,*.jpeg,*.xlsx,*.rar",  99,
                                                                                 new string[]{ SHRoles.OnMuhasebe, SHRoles.MuhasebeSatis, SHRoles.MuhasebeAlis, SHRoles.SatisFatura },
                                                                                 new string[]{ SHRoles.OnMuhasebe, SHRoles.MuhasebeSatis, SHRoles.MuhasebeAlis, SHRoles.SatisFatura },
                                                                                 new string[]{ SHRoles.OnMuhasebe, SHRoles.MuhasebeSatis, SHRoles.MuhasebeAlis, SHRoles.SatisFatura }),
                }
            },
            {
                "PA_Transaction", new []{
                    new FileBase("Vergi Dosyası", "*.csv, *.jpeg, *.ppt, *.pptx, *.doc,*.docx,*.xls,*.png,*.pdf,*.jpg,*.jpeg,*.xlsx,*.rar",  99,
                                                                                 new string[]{ SHRoles.OnMuhasebe},new string[]{ SHRoles.OnMuhasebe},new string[]{ SHRoles.OnMuhasebe}),
                    new FileBase("Fiş/Fatura Dosyası", "*.csv, *.jpeg, *.ppt, *.pptx, *.doc,*.docx,*.xls,*.png,*.pdf,*.jpg,*.jpeg,*.xlsx,*.rar",  99,
                                                                                 new string[]{ SHRoles.OnMuhasebe},new string[]{ SHRoles.OnMuhasebe},new string[]{ SHRoles.OnMuhasebe}),
                    new FileBase("Banka Gideri Dosyası", "*.csv, *.jpeg, *.ppt, *.pptx, *.doc,*.docx,*.xls,*.png,*.pdf,*.jpg,*.jpeg,*.xlsx,*.rar",  99,
                                                                                 new string[]{ SHRoles.OnMuhasebe},new string[]{ SHRoles.OnMuhasebe},new string[]{ SHRoles.OnMuhasebe}),
                    new FileBase("Masraf Dosyası", "*.csv, *.jpeg, *.ppt, *.pptx, *.doc,*.docx,*.xls,*.png,*.pdf,*.jpg,*.jpeg,*.xlsx,*.rar",  99,
                                                                                 new string[]{ SHRoles.Personel },
                                                                                 new string[]{ SHRoles.Personel },
                                                                                 new string[]{ SHRoles.Personel }),
                }
            },
            {
                "PA_Advance", new []{
                    new FileBase("Avans Dosyası", "*.csv, *.jpeg, *.ppt, *.pptx, *.doc,*.docx,*.xls,*.png,*.pdf,*.jpg,*.jpeg,*.xlsx,*.rar",  99,
                                                                                 new string[]{ SHRoles.Personel },
                                                                                 new string[]{ SHRoles.Personel },
                                                                                 new string[]{ SHRoles.Personel }),
                }
            },
            {
                "SH_Publications", new []{
                    new FileBase("Yayın Dosyası", "*.csv, *.jpeg, *.ppt, *.pptx, *.doc,*.docx,*.xls,*.png,*.pdf,*.jpg,*.jpeg,*.xlsx,*.rar",  99,
                                                                                 new string[]{ SHRoles.IKYonetici},
                                                                                 new string[]{ SHRoles.IKYonetici},
                                                                                 new string[]{ SHRoles.IKYonetici}),
                }
            },
            {
                "APM_Activity", new []{
                    new FileBase("Aktivite Dosyası", "*.csv, *.jpeg, *.ppt, *.pptx, *.doc,*.docx,*.xls,*.png,*.pdf,*.jpg,*.jpeg,*.xlsx,*.rar",  99,
                                                                                 new string[]{ SHRoles.Personel},
                                                                                 new string[]{ SHRoles.Personel},
                                                                                 new string[]{ SHRoles.Personel}),
                }
            },
            {
                "HDM_Ticket", new []{
                    new FileBase("Yardım Talep Dosyası", "*.csv, *.jpeg, *.ppt, *.pptx, *.doc,*.docx,*.xls,*.png,*.pdf,*.jpg,*.jpeg,*.xlsx,*.rar",  99,
                                                                                 new string[]{ SHRoles.YardimMasaPersonel, SHRoles.YardimMasaMusteri, SHRoles.YardimMasaYonetim, SHRoles.YardimMasaTalep },
                                                                                 new string[]{ SHRoles.YardimMasaPersonel, SHRoles.YardimMasaMusteri, SHRoles.YardimMasaYonetim, SHRoles.YardimMasaTalep},
                                                                                 new string[]{ SHRoles.YardimMasaPersonel, SHRoles.YardimMasaMusteri, SHRoles.YardimMasaYonetim, SHRoles.YardimMasaTalep}),
                }
            },
            {
                "HDM_TicketMessage", new []{
                    new FileBase("Mesaj Ek Dosya", "*.csv, *.jpeg, *.ppt, *.pptx, *.doc,*.docx,*.xls,*.png,*.pdf,*.jpg,*.jpeg,*.xlsx,*.rar",  99,
                                                                                 new string[]{ SHRoles.YardimMasaPersonel, SHRoles.YardimMasaMusteri, SHRoles.YardimMasaYonetim, SHRoles.YardimMasaTalep },
                                                                                 new string[]{ SHRoles.YardimMasaPersonel, SHRoles.YardimMasaMusteri, SHRoles.YardimMasaYonetim, SHRoles.YardimMasaTalep},
                                                                                 new string[]{ SHRoles.YardimMasaPersonel, SHRoles.YardimMasaMusteri, SHRoles.YardimMasaYonetim, SHRoles.YardimMasaTalep}),
                }
            },

             {
                "DOC_DocumentVersion", new []{
                    new FileBase("Doküman Dosyası", "*.csv, *.jpeg, *.ppt, *.pptx, *.doc,*.docx,*.xls,*.png,*.pdf,*.jpg,*.jpeg,*.xlsx,*.rar",  99,
                                                                                 new string[]{ SHRoles.Personel },
                                                                                 new string[]{ SHRoles.Personel},
                                                                                 new string[]{ SHRoles.Personel}),
                }
            },
        };



        public LocalFileProvider(string dataTable, bool role = false)
        {
            if (!role)
            {
                if (string.IsNullOrEmpty(dataTable)) throw new ArgumentNullException("dataTable");
                _dataTable = dataTable;


#if DEBUG == false
         _libraryName = System.Configuration.ConfigurationManager.AppSettings["FilesPath"];
#endif

                if (!Directory.Exists(_libraryName))
                    Directory.CreateDirectory(_libraryName);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="dataTableName">Tablo Adı</param>
        /// <param name="dataId">Data Guid Id</param>
        /// <param name="isRar">Dosya rar ile kayıt yapılsın mı?</param>
        /// <param name="files">Dosyalar</param>
        /// <returns></returns>
        public ResultStatusUI Import(Guid dataId, string fileGroup, HttpPostedFileBase files, bool zip = false)
        {
            try
            {
                var path = Path.Combine(_libraryName, _dataTable, dataId.ToString());
                CreateFolder(path);

                if (files == null || files.ContentLength <= 0)
                {
                    return new ResultStatusUI() { Result = false, Object = "İşlem Başarısız. (Files null)" };
                }

                var extension = Path.GetExtension(files.FileName);

                var dbId = Guid.NewGuid();
                if (extension == null)
                {
                    return new ResultStatusUI() { Result = false, Object = "İşlem Başarısız. (FileName null)" };
                }

                //İmages Name Path
                var fileName = GenerateFileName(path, files.FileName, extension.ToLower());

                var imagesPath = Path.Combine(path, fileName);

                if (Directory.Exists(path))
                {
                    files.SaveAs(imagesPath);
                }

                path = GetZipPath(files, zip, fileName, path, imagesPath);

                var rsFiles = InsertSYS_Files(_dataTable, dataId, path, dbId, fileGroup, Path.GetExtension(files.FileName));

                if (rsFiles.result == false)
                {
                    return new ResultStatusUI
                    {
                        FeedBack = new FeedBack().Error(rsFiles.message, "File Import Hatası.  Exp:(" + rsFiles.message + ")", false),
                        Result = false
                    };
                }

                var dict = new Dictionary<string, object>();
                dict.Add("id", dbId);
                dict.Add("url", rsFiles.objects);
                dict.Add("name", fileName);

                return new ResultStatusUI()
                {
                    Result = true,
                    Object = dict,
                    FeedBack = new FeedBack().Success("Dosya yükleme işlemi başarılı."),
                };

            }
            catch (Exception ex)
            {
                new FeedBack().Error(ex.Message, "File Import Hatası.  Exp:(" + ex.Message + ")", false);
                return new ResultStatusUI() { Result = false, Object = ex.Message };
            }
        }

        public ResultStatus Import(Guid dataId, string fileGroup, string base64File, string fileName)
        {

            try
            {

                var fileProvider = _dataTableFileGroup[_dataTable].Where(a => a.fileGroup == fileGroup).FirstOrDefault();


                if (fileProvider == null)
                {
                    return new ResultStatus()
                    {
                        result = false,
                        message = "Upload etmek istenen tablo(" + _dataTable + ") field(" + fileGroup + ") FileProvider Listesisinde bulunamadı",
                        objects = fileName
                    };
                }


                var path = string.Format("{0}\\{1}\\{2}\\", _libraryName, _dataTable, dataId);

                if (fileProvider.fileExtensions.IndexOf(fileName.Split('.').LastOrDefault().ToLower()) < 0)
                {
                    return new ResultStatus()
                    {
                        result = false,
                        message = "Upload etmek istenen dosya izin verilen formatlarda değil.Kabul edilen dosya formatları : " + fileProvider.fileExtensions,
                        objects = fileName
                    };
                }

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }


                if (Directory.GetFiles(path).Count() > fileProvider.count)
                {
                    return new ResultStatus()
                    {
                        result = false,
                        message = "Upload etmek istenen dosya grubu için maximum yükleme adetine ulaşıldı.",
                        objects = fileName
                    };
                }



                if (Directory.GetFiles(path).Count(a => a.IndexOf(fileName) > -1) > 0)
                {
                    fileName = DateTime.Now.ToString("dd.MM.yyyy_hh.MM") + "_" + fileName;
                }

                var imagesPath = Path.Combine(path, fileName);
                var rsFiles = InsertSYS_Files(dataId, fileGroup, imagesPath);
                var bytes = Convert.FromBase64String(base64File);
                if (rsFiles.result)
                {
                    using (var imageFile = new FileStream(imagesPath, FileMode.Create))
                    {
                        imageFile.Write(bytes, 0, bytes.Length);
                        imageFile.Flush();
                    }
                    return new ResultStatus() { result = true, message = "işlem başarılı.", objects = fileName };
                }
                else
                {
                    rsFiles.objects = fileName;
                    return rsFiles;
                }

            }
            catch (Exception ex)
            {
                return new ResultStatus() { result = false, message = ex.Message, objects = fileName };
            }
        }

        private ResultStatus InsertSYS_Files(Guid dataId, string fileGroup, string path)
        {
            var userStatus = (PageSecurity)HttpContext.Current.Session["userStatus"];
            var pathNew = "/" + path.Substring(path.IndexOf("Files", StringComparison.Ordinal)).Replace("\\", "/");
            var db = new WorkOfTimeDatabase();
            return db.InsertSYS_Files(new SYS_Files
            {
                id = Guid.NewGuid(),
                DataId = dataId,
                FileGroup = fileGroup,
                DataTable = _dataTable,
                created = DateTime.Now,
                createdby = userStatus == null ? Guid.Empty : userStatus.user.id,
                FileExtension = pathNew.Split('.').LastOrDefault(),
                FilePath = pathNew,
            });
        }

        public int FileCount(Guid dataId, string dataTable, string group)
        {
            using (var db = new WorkOfTimeDatabase().GetDB(null))
            {
                return db.Table<SYS_Files>().Where(a => a.DataId == dataId && a.DataTable == dataTable && a.FileGroup == group).Count();
            }
        }


        public FileBase[] GETFileBaseByDataId(Guid dataId)
        {
            var userStatus = (PageSecurity)HttpContext.Current.Session["userStatus"];
            var fileGroups = _dataTableFileGroup.Where(x => x.Key == _dataTable).FirstOrDefault();

            using (var db = new WorkOfTimeDatabase().GetDB(null))
            {
                var result = db.Table<VWSYS_Files>().Where(x => x.DataId == dataId).Execute().ToArray();
                return fileGroups.Value.Where(x => userStatus.FilesRole.Select(s => s.fileGroup).Contains(x.fileGroup) && x.fileGroup != "Sağlık Raporu").Select(a => new FileBase
                {
                    fileExtensions = a.fileExtensions,
                    fileGroup = a.fileGroup,
                    files = result.Where(z => z.FileGroup == a.fileGroup).ToArray(),
                    count = a.count
                }).ToArray();
            }
        }

        public FileBase[] GETFileBaseInDataId(Guid[] dataIds)
        {
            var userStatus = (PageSecurity)HttpContext.Current.Session["userStatus"];
            var fileGroups = _dataTableFileGroup.Where(x => x.Key == _dataTable).FirstOrDefault();

            using (var db = new WorkOfTimeDatabase().GetDB(null))
            {
                var result = db.Table<VWSYS_Files>().Where(x => x.DataTable == _dataTable && x.DataId.In(dataIds)).Execute().ToArray();

                return fileGroups.Value.Where(x => userStatus.FilesRole.Select(s => s.fileGroup).Contains(x.fileGroup) && x.fileGroup != "Sağlık Raporu").Select(a => new FileBase
                {
                    fileExtensions = a.fileExtensions,
                    fileGroup = a.fileGroup,
                    files = result.Where(z => z.FileGroup == a.fileGroup).ToArray(),
                    count = a.count
                }).ToArray();

            }
        }

        public VWSYS_Files[] GETSYS_FilesByDataId(Guid dataId)
        {
            using (var db = new WorkOfTimeDatabase().GetDB(null))
            {
                return db.Table<VWSYS_Files>().Where(x => x.DataId == dataId).Execute().ToArray();
            }
        }

        public VWSYS_Files[] GETSYS_FilesByDataIdAndFileGroup(Guid dataId, string fileGroup)
        {
            using (var db = new WorkOfTimeDatabase().GetDB(null))
            {
                return db.Table<VWSYS_Files>().Where(x => x.DataId == dataId && x.FileGroup == fileGroup).Execute().ToArray();
            }
        }
        public ResultStatusUI Delete(Guid id)
        {
            try
            {
                var db = new WorkOfTimeDatabase();
                var data = db.GetSYS_FilesById(id);
                var path = HttpContext.Current.Server.MapPath(data.FilePath);
                if (File.Exists(path) == true)
                {
                    File.Delete(path);
                }
                var rsFiles = db.DeleteSYS_Files(data);
                new FeedBack().Success(rsFiles.message);
            }
            catch (Exception ex)
            {
                new FeedBack().Error(ex.Message, "File Silme Hatası.  Exp:(" + ex.Message + ")", false);
                return new ResultStatusUI() { Result = false, Object = ex.Message };
            }
            return new ResultStatusUI() { Result = true, Object = "Silme İşlem başarılı." };
        }

        public ResultStatusUI BulkDelete(SYS_Files[] entity)
        {
            FeedBack FeedBack;
            try
            {
                var db = new WorkOfTimeDatabase();
                foreach (var path in entity.Select(data => HttpContext.Current.Server.MapPath(data.FilePath)).Where(path => File.Exists(path) == true))
                {
                    File.Delete(path);
                }
                var rsDelete = db.BulkDeleteSYS_Files(entity);
                FeedBack = new FeedBack().Success(rsDelete.message);
            }
            catch (Exception ex)
            {
                new FeedBack().Error(ex.Message, "File Silme Hatası.  Exp:(" + ex.Message + ")", false);
                return new ResultStatusUI() { Result = false, Object = ex.Message };
            }
            return new ResultStatusUI() { Result = true, Object = "Silme İşlem başarılı.", FeedBack = FeedBack };
        }



        private static ResultStatus InsertSYS_Files(string dataTableName, Guid dataId, string path, Guid dbId, string fileGroup, string fileExtension)
        {
            path = "/" + path.Substring(path.IndexOf("Files", StringComparison.Ordinal));
            var userStatus = (PageSecurity)HttpContext.Current.Session["userStatus"];
            var db = new WorkOfTimeDatabase();
            var sysfiles = new SYS_Files()
            {
                DataId = dataId,
                created = DateTime.Now,
                FilePath = path,
                id = dbId,
                DataTable = dataTableName,
                FileGroup = fileGroup,
                FileExtension = fileExtension.Replace(".", ""),
                createdby = userStatus.user.id
            };
            var rsFiles = db.InsertSYS_Files(sysfiles);
            rsFiles.objects = path;
            return rsFiles;
        }
        private static string GetZipPath(HttpPostedFileBase files, bool zip, string fileName, string path, string imagesPath)
        {
            if (zip)
            {
                var zipFileName =
                    fileName.Replace(files.FileName.Substring(files.FileName.IndexOf(".", StringComparison.Ordinal)), "");
                path = Path.Combine(path, string.Format("{0}.zip", zipFileName));
                Zip.Created(path, new[] { imagesPath }, true);
            }
            else
            {
                path = imagesPath;
            }
            return path;
        }
        private static string GenerateFileName(string path, string fileName, string extension)
        {
            fileName = (fileName.Replace(extension, "")).Replace(" ", "-").Replace("/", "-").Replace(".", "-");
            if (!Directory.Exists(path + fileName))
            {
                var pathFileName = path + fileName + "___" + DateTime.Now.ToString("s").Replace("-", "_").Replace("T", "_").Replace(":", "_") + extension;
                var fl = fileName + "___" + DateTime.Now.ToString("s").Replace("-", "_").Replace("T", "_").Replace(":", "_") + extension;
                if (pathFileName.Length > 260)
                {
                    return DateTime.Now.ToString("s").Replace("-", "_").Replace("T", "_").Replace(":", "_") + "___" + extension;
                }
                return fl;
            }
            return fileName + extension;
        }





        private static void CreateFolder(string path)
        {
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
            }
            catch (Exception ex)
            {
                new FeedBack().Error(ex.Message, "CreateFolder Hatası Exp:(" + ex.Message + ")", false);
            }
        }

    }
    public class FileBase
    {
        public int count { get; set; }
        public string fileGroup { get; set; }
        public string fileExtensions { get; set; }
        public string[] insert { get; set; } = new string[] { };
        public string[] delete { get; set; } = new string[] { };
        public string[] preview { get; set; } = new string[] { };
        public VWSYS_Files[] files { get; set; }
        public FileBase()
        {

        }
        public FileBase(string FileGroup, string FileExtensions, int Count, string[] Insert = null, string[] Delete = null, string[] Preview = null)
        {
            fileGroup = FileGroup;
            fileExtensions = FileExtensions;
            count = Count;
            insert = Insert ?? new string[] { };
            delete = Delete ?? new string[] { };
            preview = Preview ?? new string[] { };
        }


    }
    public class FileUploadSave
    {
        public HttpRequestBase Request { get; set; }
        public Guid? DataId { get; set; }
        public string DataTable { get; set; }
        public FileUploadSave(HttpRequestBase request, Guid? DataId = null, string DataTable = null)
        {
            Request = request;
            this.DataId = DataId;
            this.DataTable = DataTable;
        }

        public ResultStatusUI SaveAs()
        {
            try
            {
                var errorList = new List<string>();
                var files = Request.Files;
                for (int i = 0; i < files.Count; i++)
                {
                    var file = files[i];
                    var key = files.AllKeys[i];
                    var items = key.Split('|');
                    var table = DataTable ?? items[0];
                    var group = items[1];
                    var dataid = DataId ?? new Guid(items[2]);
                    var fileProvider = new LocalFileProvider(table);
                    fileProvider.Import(dataid, group, file);
                }
            }
            catch (Exception ex)
            {
                new FeedBack().Error(ex.Message.ToString());
                return new ResultStatusUI() { Result = false, FeedBack = new FeedBack().Warning("İşlem ") };
            }
            return new ResultStatusUI() { Result = true, FeedBack = new FeedBack().Success("Başarılı") };
        }
    }
}