using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;

namespace Infoline.WorkOfTime.BusinessAccess
{
    public class TranscriptData
    {
        public string email { get; set; }
        public DateTime date { get; set; }
    }

    public class FBU_OgrenciDersProgrami
    {
        public string ogrenci_no { get; set; }
        public string ders_kodu { get; set; }
        public string ders_adi { get; set; }
        public string derslik_kodu { get; set; }
        public string gun { get; set; }
        public string baslangic_saat { get; set; }
        public string bitis_saat { get; set; }
        public string hoca_kodu { get; set; }
        public string hoca_adi { get; set; }
    }
    public class FBU_OgrenciDersProgramiResult
    {
        public int err { get; set; }
        public List<FBU_OgrenciDersProgrami> result { get; set; }
    }
    public class FBU_OgrenciDers
    {
        public string ogrenci_no { get; set; }
        public string ders_kodu { get; set; }
        public string ders_adi { get; set; }
        public int section { get; set; }
        public string sinifi { get; set; }
        public string kredi { get; set; }
        public string harf { get; set; }
        public string hoca_kodu { get; set; }
        public string hoca_adi { get; set; }
        public string sezon { get; set; }
        public string donem { get; set; }
        public int kyp { get; set; }
    }
    public class FBU_OgrenciDersResult
    {
        public int err { get; set; }
        public List<FBU_OgrenciDers> result { get; set; }
    }
    public class FBU_AkademikTakvim
    {
        public string sezon { get; set; }
        public string donem { get; set; }
        public string takvim_adi { get; set; }
        public string etkinlik { get; set; }
        public string baslangic_tarihi { get; set; }
        public string bitis_tarihi { get; set; }
    }
    public class FBU_AkademikTakvimResult
    {
        public int err { get; set; }
        public List<FBU_AkademikTakvim> result { get; set; }
    }
    public class FBU_OgrenciSinavProgrami
    {
        public int id { get; set; }
        public string sezon { get; set; }
        public string donem { get; set; }
        public int ders_id { get; set; }
        public string section { get; set; }
        public string sinav_tipi { get; set; }
        public string derslik_kodu { get; set; }
        public string tarih { get; set; }
        public string baslangic_saat { get; set; }
        public string bitis_saat { get; set; }
        public int ders_section { get; set; }
        public string ders_kodu { get; set; }
        public string ders_adi { get; set; }
        public string hoca_kodu { get; set; }
        public string hoca_adi { get; set; }
        public string fakulte_kodu { get; set; }
        public string fakulte_adi { get; set; }
        public string program_kodu { get; set; }
        public string program_adi { get; set; }
        public string gozetmen { get; set; }
        public string gozetmenler { get; set; }
        public string[] derslik { get; set; }
    }
    public class FBU_OgrenciSinavProgramiResult
    {
        public int err { get; set; }
        public List<FBU_OgrenciSinavProgrami> result { get; set; }
    }
    public class FBU_OgrenciSinavSonuc
    {
        public string ogrenci_no { get; set; }
        public string ad { get; set; }
        public string soyad { get; set; }
        public string ders_kodu { get; set; }
        public string ders_adi { get; set; }
        public double? final_notu { get; set; }
        public List<FBU_Sınavlar> sinavlar { get; set; }
    }
    public class FBU_Sınavlar
    {
        public string sinav_adi { get; set; }
        public double? puan { get; set; }
    }
    public class FBU_OgrenciSinavSonucResult
    {
        public int err { get; set; }
        public List<FBU_OgrenciSinavSonuc> result { get; set; }
    }
    public class FBU_Duyuru
    {
        public string konu { get; set; }
        public string detay { get; set; }
    }
    public class FBU_DuyuruResult
    {
        public int err { get; set; }
        public List<FBU_Duyuru> result { get; set; }
    }
    public class FBU_OgrenciBilgi
    {
        public string ogrenci_no { get; set; }
        public string ad { get; set; }
        public string soyad { get; set; }
        public string dogum_tarihi { get; set; }
        public string cinsiyet { get; set; }
        public string tckn { get; set; }
        public string pasaport_no { get; set; }
        public string ulke_kodu { get; set; }
        public string il_kodu { get; set; }
        public string ilce_kodu { get; set; }
        public string ikamet_adresi { get; set; }
        public string eposta_adresi { get; set; }
        public string son_mezun_oldugu_kurum { get; set; }
        public string son_mezun_oldugu_kurum_il_kodu { get; set; }
        public string uluslararasi_ogrenci { get; set; }
        public string cep_tel { get; set; }
        public string foto { get; set; }
    }
    public class FBU_OgrenciBilgiResult
    {
        public int err { get; set; }
        public List<FBU_OgrenciBilgi> result { get; set; }
    }
   
    public class FBU_OISModel
    {
        private static List<TranscriptData> _listData = new List<TranscriptData>();
        private string baseuri { get; set; } = "https://ois.fbu.edu.tr/api/crm";
        private string username { get; set; } = "intranet.api";
        private string password { get; set; } = "Kh*v63d4hX68b5@Z";
        private string email { get; set; }

        private Dictionary<string, string> derslerDemoData { get; } = new Dictionary<string, string>
        {
                    { "ILT102","İletişim Kuramları" },
                    { "HIT102","Halkla İlişkilerin Temelleri" },
                    { "HIT106","Medya Çalışmaları" },
                    { "HIT104","Etkili İletişim Becerileri" },
                    { "HIT108","Haber Analizi ve Editörlüğü" },
                    { "TRD102","Türk Dili II" },
                    { "ENG112","İngilizce II" },
        };

        private Dictionary<string, string> duyuruDemoData { get; } = new Dictionary<string, string>
        {
                    { "Öğretim Görevlisi ve Araştırma Görevlisi Giriş Sınavları","COVID-19 salgını nedeniyle ertelenen aşağıdaki Öğretim Görevlisi ve Araştırma Görevlisi kadrolarına başvuran adayların giriş sınavları, 20 Nisan 2020 Pazartesi günü saat 14:00’da Fenerbahçe Üniversitesi Ataşehir Yerleşkesinde yapılacaktır." },
                    { "Akademik İlan","Üniversitemizin ekte belirtilen akademik birimlerine öğretim üyeleri ve öğretim elemanları alınacaktır." },
                    { "FBU Araştırma Görevlisi Kadrosu Ön Değerlendirme Sonuçları","Fenerbahçe Üniversitesi Öğretim Görevlisi ve Araştırma Görevlisi kadrolarına başvuran adayların ön değerlendirme sonuçları ekte sunulmuş olup,  01.04.2020 tarihinde yapılacak olan giriş sınavları yeni koronavirüs salgını nedeniyle ileriki bir tarihe ertelenmiştir." },
                    { "FBU Araştırma Görevlisi Ön Değerlendirme Sonuçları","Fenerbahçe Üniversitesi Öğretim Görevlisi ve Araştırma Görevlisi Kadrosuna başvuran adayların ön değerlendirme sonuçları ekte sunulmuş olup,  23.03.2020 tarihinde yapılacak olan giriş sınavları yeni koronavirüs salgını nedeniyle ileriki bir tarihe ertelenmiştir." },
                    { "Bahar Dönemi Servis Güzergahı","Fenerbahçe Üniversitesi 2020 Bahar Döneminde eğitimine devam eden öğrencilerimize ilgili yerleşke - duraklar arası güzergahında ücretsiz servis hizmeti başlatılacaktır." },
                    { "İNGİLİZCE 1 VE İNGİLİZCE 2 DERSİ MUAFİYET SINAVI HAKKINDA","İngilizce 1 ve İngilizce 2 dersinden muaf olmak isteyen Üniversitemiz fakülte öğrencileri için yapılacak olan Muafiyet Sınavı ile ilgili bilgiler aşağıda yer almaktadır. Söz konusu sınavdan 70 ve üzeri skor elde eden öğrenciler İngilizce 1 ve İngilizce 2 derslerinden muaf olacaklardır" },
                    { "Spor Bilimleri Fakültesi Özel Yetenek Sınav Sonuçları açıklandı.","Fenerbahçe Üniversitesi Spor Bilimleri Fakültesi Özel Yetenek Sınav Sonuçları açıklanmıştır." },
        };

        private Dictionary<string, string> hocalarDemoData { get; } = new Dictionary<string, string>
        {
                    { "1900005239","Ece BABAN" },
                    { "1900005265","Elif Başak SARIOĞLU" },
                    { "1900005258","Özlem ÖZDEMİR" },
                    { "1900005340","Murat Korhan VAROL" },
                    { "2000005342","Egemen SÖNMEZ" },
                    { "1900005321","Şemse NAZİK" },
                    { "1900005322","Şemse NAZİK" },
        };

        private string[] sinavDemoData { get; } = new string[] { "Vize", "Bütünleme", "Final", "Ödev ve Derse Katılım" };

        private Dictionary<string, string> takvimDemoData { get; } = new Dictionary<string, string>
        {
            {"19 Ağustos 2019-23 Ağustos 2019", "YKS ile Yerleşen Öğrencilerin Kayıtları" },
            {"25 Eylül 2019", "Yabancı Uyruklu Öğrencilerin Kayıtlarının Başlaması" },
            {"30 Eylül 2019-07 Ekim 2019", "Muafiyet Başvuruları" },
            {"03 Ekim 2019", "İngilizce Muafiyet Sınavı" },
            {"03 Ekim 2019-04 Ekim 2019", "Güz Dönemi Ders Seçimi" },
            {"07 Ekim 2019", "Güz Dönemi Derslerinin Başlaması" },
            {"07 Ekim 2019-11 Ekim 2019", "Güz Dönemi Ders Ekleme-Bırakma Haftası" },
            {"25 Kasım 2019-29 Kasım 2019", "Ara Sınavlar" },
            {"06 Aralık 2019", "Güz Dönemi Ara Sınav Notlarının Sisteme Girişinin Son Günü" },
            {"23 Aralık 2019-27 Aralık 2019", "Güz Dönemi Ara Sınav Mazeret Sınavları" },
            {"10 Ocak 2020", "Güz Dönemi Derslerinin Bitimi" },
            {"13 Ocak 2020-22 Ocak 2020", "Güz Dönemi Final Sınavları" },
            {"24 Ocak 2020", "Güz Dönemi Final Sınav Notlarının Sisteme Girişinin Son Günü" },
            {"27 Ocak 2020-31 Ocak 2020", "Güz Dönemi Bütünleme Sınavları" },
            {"02 Şubat 2020", "Bütünleme Notlarının İlan Edilmesi" }
        };

        public FBU_OISModel(string _email)
        {
            _email = _email.Split('@').FirstOrDefault();
            email = _email + "@stu.fbu.edu.tr";
        }

        public List<FBU_OgrenciDersProgrami> GetOgrenciDersProgrami()
        {
            var data = GetData("ogrencidersprogrami");
            var resultData = Helper.Json.Deserialize<FBU_OgrenciDersProgramiResult>(data).result;
            return resultData;
        }

        public List<FBU_OgrenciDers> GetOgrenciDers()
        {
            var data = GetData("ogrenciders");
            var resultData = Helper.Json.Deserialize<FBU_OgrenciDersResult>(data).result;
            return resultData;
        }

        public List<FBU_AkademikTakvim> GetAkademikTakvim()
        {
            var data = GetData("akademiktakvim");
            var resultData = Helper.Json.Deserialize<FBU_AkademikTakvimResult>(data).result;
            return resultData;
        }

        public List<FBU_OgrenciSinavProgrami> GetOgrenciSinavProgrami()
        {
            var data = GetData("ogrencisinavprogrami");
            var resultData = Helper.Json.Deserialize<FBU_OgrenciSinavProgramiResult>(data).result;
            return resultData;
        }

        public List<FBU_OgrenciSinavSonuc> GetOgrenciSinavSonuc()
        {
            var data = GetData("ogrencisinavsonuc");
            var resultData = Helper.Json.Deserialize<FBU_OgrenciSinavSonucResult>(data).result;
            return resultData;
        }

        public List<FBU_Duyuru> GetOgrenciDuyuru()
        {
            var data = GetData("duyuru");
            var resultData = Helper.Json.Deserialize<FBU_DuyuruResult>(data).result;
            return resultData;
        }

        public ResultStatus<FBU_OgrenciBilgi> GetOgrenciBilgi()
        {
            var data = GetData("ogrenci");
            var resultData = Helper.Json.Deserialize<FBU_OgrenciBilgiResult>(data);
            return new ResultStatus<FBU_OgrenciBilgi>
            {
                objects = resultData.result.FirstOrDefault(),
                result = resultData.result.Count() > 0,
                message = resultData.result.Count() > 0 ? "" : "öğrenci bilgilerine erişilemiyor.",
            };
        }

        public ResultStatus SendTranskriptRequest()
        {

            var ogrencibilgi = GetOgrenciBilgi();

            if (ogrencibilgi.result == false)
            {
                return new ResultStatus { result = false, message = "Öğrenci bilgilerine erişilemiyor" };
            }

            if (FBU_OISModel._listData.Any(a => a.email == this.email && a.date.ToShortDateString() == DateTime.Now.ToShortDateString()))
            {
                return new ResultStatus { result = false, message = "Aynı gün içinde birkez transkript talebinde bulunabilirsiniz." };
            }

            FBU_OISModel._listData.Add(new TranscriptData
            {
                email = this.email,
                date = DateTime.Now,
            });

            var text = @"<h3>Sayın {0},</h3> 
                <p><u>{1}</u> isimli öğrenci {2} tarihinde, transkript talebinde bulunmuştur. Bilgileri;</p>
                <p>Tc Numarası : {3}</p>
                <p>Öğrenci Numarası : {4}</p>
                <p>Telefon Numarası : {5}</p>
                <p>E-posta Adresi : {6}</p>
                <p>Bilgilerinize.<br>İyi Çalışmalar.</p>";

            var ogrenci = ogrencibilgi.objects;
            var mesaj = string.Format(text, "", ogrenci.ad + " " + ogrenci.soyad, string.Format("{0:dd.MM.yyyy HH:mm}", DateTime.Now), ogrenci.tckn, ogrenci.ogrenci_no, ogrenci.cep_tel, ogrenci.eposta_adresi);


            try
            {
                new Email().Template("Template1", "demo.jpg", "Transkript Talebi Hakkında", mesaj).Send((Int16)EmailSendTypes.Dosya, "alisan.yildirim@fbu.edu.tr", "FBU Transkript Talebi Hakkında..", false);
                return new ResultStatus { result = true, message = "Transkript talebiniz alınmıştır." };
            }
            catch (Exception ex)
            {
                return new ResultStatus { result = false, message = "Transkript talebiniz alınırken sorun oluştu." + ex.Message };
            }



        }

        public object GetAllDataByStudent(string email)
        {
            var res = new FBU_OISModel(email);
            return new
            {
                ogrenciBilgi = res.GetOgrenciBilgi(),
                dersProgrami = res.GetOgrenciDersProgrami(),
                dersler = res.GetOgrenciDers(),
                akademikTakvim = res.GetAkademikTakvim(),
                sinavProgrami = res.GetOgrenciSinavProgrami(),
                sinavSonuc = res.GetOgrenciSinavSonuc(),
                duyuru = res.GetOgrenciDuyuru(),
            };
        }

        private string GetData(string method)
        {
            using (var client = new WebClient())
            {
                var data = new NameValueCollection();
                data["api_username"] = username;
                data["api_password"] = password;
                data["method"] = method;

                if (!String.IsNullOrEmpty(this.email))
                {
                    data["eposta_adresi"] = this.email;
                }

                var response = client.UploadValues(this.baseuri, "POST", data);
                string responseInString = Encoding.UTF8.GetString(response);

                return responseInString;
            }
        }


    }

}
