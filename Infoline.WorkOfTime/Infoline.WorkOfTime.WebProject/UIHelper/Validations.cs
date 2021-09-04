namespace System.Web.Mvc
{

    public struct Validations
    {
        public static ValidationUI UserName(bool? required = null)
        {
            return new ValidationUI
            {
                required = (required == true),
                pattern = "[a-zA-Z0-9._-]+",
                data_error = "Yanlızca sayı ve türkçe karakter içermeyen harf girişi gerçekleştirebilirsiniz.. ( 6 - 30 Karakter )",
                minlength = 6,
                maxlength = 30
            };
        }

        public static ValidationUI Text09(bool? required = null, Int32? minLength = 0, Int32? maxLength = 50)
        {
            return new ValidationUI
            {
                required = (required == true),
                pattern = "[a-zA-Z0-9]+",
                data_error = "Lütfen Özel karakter kullanmadan ve birleşik giriş yapınız..",
                minlength = minLength,
                maxlength = maxLength
            };
        }

        public static ValidationUI Text09Space(bool? required = null, Int32? minLength = 0, Int32? maxLength = 50)
        {
            return new ValidationUI
            {
                required = (required == true),
                pattern = "[a-zA-Z0-9 ]+",
                //data_pattern_error = "Lütfen Özel karakter kullanmadan giriş yapınız...",
                data_error = "Lütfen Özel karakter kullanmadan giriş yapınız...",
                minlength = minLength,
                maxlength = maxLength

            };
        }

        public static ValidationUI TextEverywhere(bool? required = null, Int32? minLength = 0, Int32? maxLength = 50)
        {
            return new ValidationUI
            {
                required = (required == true),
                pattern = "[a-zA-Z0-9 ÇŞĞÜÖİçşğüöıâÂ:;|\\-_/.,&*'?+()’<>= ]+",
                data_pattern_error = "Lütfen Özel karakter kullanmadan giriş gerçekleştirin...",
                minlength = minLength,
                maxlength = maxLength
            };
        }

        public static ValidationUI TextProduct(bool? required = null, Int32? minLength = 0, Int32? maxLength = null)
        {
            return new ValidationUI
            {
                required = (required == true),
                pattern = "[a-zA-Z0-9 ÇŞĞÜÖİçşğüöıâÂ:'/\\-_.,*+()& ]+",
                data_pattern_error = "Lütfen Özel karakter kullanmadan giriş gerçekleştirin...",
                minlength = minLength,
                maxlength = maxLength
            };
        }
        public static ValidationUI MalzemeAdi(bool? required = null, Int32? minLength = 0, Int32? maxLength = 50)
        {
            return new ValidationUI
            {
                required = (required == true),
                pattern = "[a-zA-Z0-9 ÇŞĞÜÖİçşğüöıâÂ\\-/*]+",
                data_pattern_error = "Lütfen Özel karakter kullanmadan giriş gerçekleştirin...",
                minlength = minLength,
                maxlength = maxLength
            };
        }

        public static ValidationUI Unvan(bool? required = null, Int32? minLength = 0, Int32? maxLength = 50)
        {
            return new ValidationUI
            {
                required = (required == true),
                pattern = "[a-zA-Z0-9 ÇŞĞÜÖİçşğüöı.,/-]+",
                data_pattern_error = "Lütfen Özel karakter kullanmadan giriş gerçekleştirin...",
                minlength = minLength,
                maxlength = maxLength
            };
        }

        public static ValidationUI TextTurkce(bool? required = null, Int32? minLength = null, Int32? maxLength = null)
        {
            return new ValidationUI
            {
                required = (required == true),
                pattern = "[a-zA-ZÇŞĞÜÖİâÂçşğüöı]+",
                data_error = "Lütfen Yalnızca Alfabetik Giriş Yapınız..",
                minlength = minLength,
                maxlength = maxLength
            };
        }

        public static ValidationUI TextInternational(bool? required = null, Int32? minLength = null, Int32? maxLength = null)
        {
            return new ValidationUI
            {
                required = (required == true),
                pattern = "[A-Z]+",
                data_error = "Lütfen Yalnızca Büyük Harflerle Alfabetik Giriş Yapınız..",
                minlength = minLength,
                maxlength = maxLength
            };
        }


        public static ValidationUI TextTurkceBuyuk(bool? required = null, Int32? minLength = null, Int32? maxLength = null)
        {
            return new ValidationUI
            {
                required = (required == true),
                pattern = "[A-ZÇŞĞÜÖâÂİ .]+",
                data_error = "Lütfen Yalnızca Büyük Harflerle Alfabetik Giriş Yapınız..",
                minlength = minLength,
                maxlength = maxLength
            };
        }

        public static ValidationUI TextTurkceSpace(bool? required = null, Int32? minLength = null, Int32? maxLength = null)
        {
            return new ValidationUI
            {
                required = (required == true),
                pattern = "[a-zA-ZÇŞĞÜÖâÂİçşğüöı .]+",
                data_error = "Lütfen Yalnızca Alfabetik Giriş Yapınız..",
                minlength = minLength,
                maxlength = maxLength
            };
        }

        public static ValidationUI TextTurkce09(bool? required = null, Int32? minLength = 0, Int32? maxLength = 50)
        {
            return new ValidationUI
            {
                required = (required == true),
                pattern = "[a-zA-Z0-9ÇŞĞÜÖİçşğüâÂöı]+",
                data_pattern_error = "Lütfen Özel karakter kullanmadan birleşik giriş gerçekleştirin...",
                minlength = minLength,
                maxlength = maxLength
            };
        }

        public static ValidationUI TextTurkceSpace09(bool? required = null, Int32? minLength = null, Int32? maxLength = null)
        {
            return new ValidationUI
            {
                required = (required == true),
                pattern = "[a-zA-Z0-9 ÇŞĞÜÖİçşğüöıâÂ]+",
                //data_pattern_error = "Lütfen Özel karakter kullanmadan giriş gerçekleştirin...",
                data_error = "Lütfen Özel karakter kullanmadan giriş gerçekleştirin...",
                minlength = minLength,
                maxlength = maxLength
            };
        }

        public static ValidationUI Text09Double(bool? required = null, Int32? minLength = null, Int32? maxLength = null)
        {
            return new ValidationUI
            {
                required = (required == true),
                pattern = "[a-zA-Z0-9 ÇŞĞÜÖİçşğüöıâÂ,.-:;]+",
                //data_pattern_error = "Lütfen Özel karakter kullanmadan giriş gerçekleştirin...",
                data_error = "Lütfen Özel karakter kullanmadan giriş gerçekleştirin...",
                minlength = minLength,
                maxlength = maxLength
            };
        }

        public static ValidationUI IP(bool? required = null, string error = "Geçerli bir adres girilmedi.")
        {
            return new ValidationUI
            {
                pattern = "\\b\\d{1,3}.\\b\\d{1,3}.\\b\\d{1,3}.\\b\\d{1,3}",
                data_error = error,
                required = (required == true),
                maxlength = 15
            };
        }

        public static ValidationUI NumberOnly(bool? required = null, Int32? minLength = 0, Int32? maxLength = 12, string error = "Sadece sayı girişi gerçekleştiriniz..")
        {
            return new ValidationUI
            {
                pattern = "[0-9]+",
                data_error = error,
                required = (required == true),
                minlength = minLength,
                maxlength = maxLength
            };
        }

        public static ValidationUI Range(Int32? Min, Int32? Max, bool? required = null)
        {
            return new ValidationUI
            {
                type = "number",
                min = Min,
                max = Max,
                required = (required == true),
            };
        }

        public static ValidationUI TelefonNo(bool? required = null)
        {
            return new ValidationUI
            {
                maxlength = 13,
                minlength = 11,
                pattern = "([+][0-9])?(0)[0-9]{10}",
                data_error = "Telefon Numarası Sayısal Karakterlerden Oluşmalıdır. ( 0 ile beraber )",
                required = (required == true),
            };
        }
        public static ValidationUI FaksNo(bool? required = null)
        {
            return new ValidationUI
            {
                maxlength = 13,
                minlength = 11,
                pattern = "([+][0-9])?(0)[0-9]{10}",
                data_error = "Faks Numarası Sayısal Karakterlerden Oluşmalıdır. ( 0 ile beraber )",
                required = (required == true),
            };
        }

        public static ValidationUI TelefonNo2(bool? required = null)
        {
            return new ValidationUI
            {
                maxlength = 13,
                minlength = 10,
                pattern = "([+][0-9])?[0-9]{10}",
                data_error = "Telefon Numarası Sayısal Karakterlerden Oluşmalıdır.(0 olmadan)",
                required = (required == true),
            };
        }

        public static ValidationUI VergiNo(bool? required = null)
        {
            return new ValidationUI
            {
                minlength = 10,
                maxlength = 11,
                pattern = "[0-9]+",
                data_error = "Vergi Numarası En Az 10 En Fazla 11 Sayısal Karakterden Oluşmalıdır.",
                required = (required == true),
            };
        }

        public static ValidationUI IhaleNo(bool? required = null)
        {
            return new ValidationUI
            {
                minlength = 10,
                maxlength = 11,
                pattern = "[0-9]*[.][0-9]*[.][0-9]*",
                data_error = "İhale Numarası En Az 10 En Fazla 11 Sayısal Karakterden Oluşmalıdır.",
                required = (required == true),
            };
        }


        public static ValidationUI TCKimlik = new ValidationUI
        {
            maxlength = 20,
            //minlength = 11,
            //pattern = "(([1-9]{1})([0-9]{9})([0,2,4,6,8]{1}))",
            pattern = "[0-9]+",
            data_error = "Geçerli bir kimlik numarası girilmedi.",
            required = true,
        };

        public static ValidationUI TCKimlikReq(bool? required = true)
        {
            return new ValidationUI
            {
                //Kazakistan Kimlik noları ile uymadığı için zorunluluklar kaldırıldı.

                maxlength = 20,
                //minlength = 11,
                //pattern = "(([1-9]{1})([0-9]{9})([0,2,4,6,8]{1}))",
                pattern = "[0-9]+",
                data_error = "Geçerli bir kimlik numarası girilmedi.",
                required = required,
            };
        }

        public static ValidationUI Required = new ValidationUI { required = true, };

        public static ValidationUI False = new ValidationUI {required=false };

        public static ValidationUI Yuzde(bool? required = null)
        {
            return new ValidationUI
            {
                type = "number",
                pattern = "(^(?:100|[1-9]?[0-9])$)",
                maxlength = 3,
                data_error = "Lütfen 0 ile 100 arasında bir değer giriniz.",
                data_pattern_error = "Lütfen 0 ile 100 arasında bir değer giriniz.",
                required = (required == true)
            };
        }

        public static ValidationUI Number(bool? required = null, Int32? minLength = 0, Int32? maxLength = 12,string data_Error= "Geçerli bir sayı girilmedi.")
        {
            return new ValidationUI
            {
                pattern = "[-+]?[0-9]*(?:.|,)?[0-9]+",
                data_error = data_Error,
                required = (required == true),
                minlength = minLength,
                maxlength = maxLength
            };
        }

        public static ValidationUI EMail(bool? required = null)
        {
            return new ValidationUI
            {
                type = "email",
                data_error = "Geçerli bir mail adresi girilmedi",
                maxlength = 100,
                minlength = 5,
                required = (required == true)
            };
        }

        public static ValidationUI Password(bool? required = null)
        {
            return new ValidationUI
            {
                autocomplete = "off",
                type = "password",
                maxlength = 14,
                minlength = 6,
                pattern = "[0-9a-zA-Z.,*!-_&]+",
                data_pattern_error = "Geçerli bir şifre girilmedi. ( 6 - 14 Karakter )",
                required = (required == true)
            };
        }

        public static ValidationUI Password2(bool? required = null)
        {
            return new ValidationUI
            {
                autocomplete = "off",
                type = "password",
                maxlength = 100,
                minlength = 0,
                pattern = "[0-9a-zA-Z.,*!-_&]+",
                data_pattern_error = "Geçerli bir şifre girilmedi. ",
                required = (required == true)
            };
        }

        public static ValidationUI PasswordMatch(string Match, bool? required = null)
        {
            return new ValidationUI
            {
                data_error = "Malesef Şifreler eşleşmiyor...",
                data_pattern_error = "Malesef Şifreler eşleşmiyor...",
                data_match = Match,
                required = (required == true),
                type = Password(required).type,
                pattern = Password(required).pattern,
                maxlength = Password(required).maxlength,
                minlength = Password(required).minlength,
                autocomplete = Password(required).autocomplete,
            };
        }

        public static ValidationUI Adres(bool? required = null, int? minLength = 10, int? maxLength = 250)
        {
            return new ValidationUI
            {
                pattern = "[0-9a-zA-âÂZÇŞĞÜÖİçşğüöı \\-/.,;:]+",
                minlength = minLength == null ? 10 : minLength,
                required = (required == true)
            };
        }

        public static ValidationUI Detail(bool? required = null, int? minLength = 10, int? maxLength = 250)
        {
            return new ValidationUI
            {
                pattern = "[0-9a-zA-Z âÂZÇŞĞÜÖİçşğüöı\\-/.,;:]+",
                minlength = minLength == null ? 10 : minLength,
                maxlength = 250,
                required = (required == true)
            };
        }

        public static ValidationUI URLNew(bool? required = null)
        {
            return new ValidationUI
            {
                pattern = "(http:\\/\\/|https:\\/\\/|)([a-zA-Z0-1.\\-\\_]+)([.])([a-zA-Z0-1.\\-\\_]+)",
                maxlength = 255,
                required = (required == true),
                data_error = "Lütfen bir URL giriniz.(Örn: http://www.workoftime.com)",
            };
        }
        public static ValidationUI URL(bool? required = null)
        {
            return new ValidationUI
            {
                maxlength = 255,
                type = "url",
                required = (required == true),
                data_error = "Lütfen bir URL giriniz.(Örn: http://www.workoftime.com)",

            };
        }
        public static ValidationUI MeasurementLinearity(bool? required = null, Int32? minLength = 0, Int32? maxLength = 100)
        {
            return new ValidationUI
            {
                required = (required == true),
                pattern = "[0-9%+-]+",
                data_pattern_error = "Lütfen Sayı %,+,- gibi Alanları Tercih Ediniz..",
                minlength = minLength,
                maxlength = maxLength
            };
        }
        public static ValidationUI OnlyDay(bool? required = null, Int32? minLength = 1, Int32? maxLength = 2, string error = "1 - 31 Arasında Değer Giriniz")
        {
            return new ValidationUI
            {
                pattern = @"([1-9]|[12]\d|3[1])",
                data_error = error,
                required = (required == true),
                minlength = minLength,
                maxlength = maxLength
            };
        }
        public static ValidationUI OnlyMounth(bool? required = null, Int32? minLength = 1, Int32? maxLength = 2, string error = "1 - 12 Arasında Değer Giriniz")
        {
            return new ValidationUI
            {
                pattern = "^(0?[1-9]|1[012])$",
                data_error = error,
                required = (required == true),
                minlength = minLength,
                maxlength = maxLength
            };
        }
        public static ValidationUI AccesableUser(bool? required = null, Int32? minLength = 0, Int32? maxLength = 100)
        {
            return new ValidationUI
            {
                required = (required == true),
                pattern = "[0-9a-zA-Z--_@]+",
                data_pattern_error = "Lütfen '.', '-', '_', '@' gibi Alanları Tercih Ediniz..",
                minlength = minLength,
                maxlength = maxLength
            };
        }
        public static ValidationUI MailMessage(bool? required = null, int? minLength = 0)
        {
            return new ValidationUI
            {
                pattern = "[0-9a-zA-Z âÂZÇŞĞÜÖİçşğüöı\\-/.,;:]+",
                maxlength = 255,
                minlength = minLength == null ? 0 : minLength,
                required = (required == true)
            };
        }
        public static ValidationUI TextName(bool? required = null, Int32? minLength = null, Int32? maxLength = null)
        {
            return new ValidationUI
            {
                required = (required == true),
                pattern = "[a-zA-Z0-9 ÇŞĞÜÖİçşğüöı-]+",
                //data_pattern_error = "Lütfen Özel karakter kullanmadan giriş gerçekleştirin...",
                data_error = "Lütfen Özel karakter kullanmadan giriş gerçekleştirin...",
                minlength = minLength,
                maxlength = maxLength
            };
        }
        public class ValidationUI
        {
            public string pattern { get; set; }
            public Int32? maxlength { get; set; }
            public Int32? minlength { get; set; }
            public string type { get; set; }
            public bool? required { get; set; }
            public string data_match { get; set; }
            public string data_error { get; set; }
            public string data_required_error { get { return "Lütfen Bu Alanı Doldurun.."; } }
            public string data_pattern_error { get; set; }
            public string autocomplete { get; set; }    //      on  -  off
            public Int32? min { get; set; }
            public Int32? max { get; set; }
        }
        
    }

}