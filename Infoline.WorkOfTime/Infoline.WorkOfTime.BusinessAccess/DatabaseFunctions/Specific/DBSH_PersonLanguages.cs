using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infoline.Framework.Database;
using System.Data.SqlClient;
using System.Linq.Expressions;
using System.Data.Common;
using Infoline.WorkOfTime.BusinessData;

namespace Infoline.WorkOfTime.BusinessAccess
{
    [EnumInfo(typeof(SH_PersonLanguages), "Languages")]
    public enum EnumSH_PersonLanguagesLanguages
    {

        [Description("Afganca")]
        Afganca = 0,
        [Description("Almanca")]
        Almanca = 1,
        [Description("Amharca")]
        Amharca = 2,
        [Description("Arapça")]
        Arapca = 3,
        [Description("Arnavutça")]
        Arnavutca = 4,
        [Description("Azerice")]
        Azerice = 5,
        [Description("Belarusça")]
        Belarusca = 6,
        [Description("Bengali")]
        Bengali = 7,
        [Description("Boşnakça")]
        Bosnakca = 8,
        [Description("Bulgarca")]
        Bulgarca = 9,
        [Description("Burma")]
        Burma = 10,
        [Description("Çeçence")]
        Cecence = 11,
        [Description("Çekçe")]
        Cekce = 12,
        [Description("Çince")]
        Cince = 13,
        [Description("Danca")]
        Danca = 14,
        [Description("Darice")]
        Darice = 15,
        [Description("Endonezyaca")]
        Endonezyaca = 16,
        [Description("Ermenice")]
        Ermenice = 17,
        [Description("Estonca")]
        Estonca = 18,
        [Description("Farsça")]
        Farsca = 19,
        [Description("Filipince")]
        Filipince = 20,
        [Description("Fince")]
        Fince = 21,
        [Description("Flemenkçe")]
        Flemenkçe = 22,
        [Description("Fransızca")]
        Fransizca = 23,
        [Description("Guarani")]
        Guarani = 24,
        [Description("Gürcüce")]
        Gurcuce = 25,
        [Description("Hırvatça")]
        Hırvatca = 26,
        [Description("Hintçe")]
        Hintce = 27,
        [Description("İbranice")]
        Ibranice = 28,
        [Description("İngilizce")]
        Ingilizce = 29,
        [Description("İrlandaca")]
        Irlandaca = 30,
        [Description("İskoçça")]
        Iskocca = 31,
        [Description("İspanyolca")]
        Ispanyolca = 32,
        [Description("İsveççe")]
        Isvecce = 33,
        [Description("İtalyanca")]
        Italyanca = 34,
        [Description("İzlandaca")]
        Izlandaca = 35,
        [Description("Japonca")]
        Japonca = 36,
        [Description("Katalanca")]
        Katalanca = 37,
        [Description("Kazakça")]
        Kazakca = 38,
        [Description("Kırgızca")]
        Kirgizca = 39,
        [Description("Korece")]
        Korece = 40,
        [Description("Kürtçe")]
        Kurtce = 41,
        [Description("Latince")]
        Latince = 42,
        [Description("Lazca")]
        Lazca = 43,
        [Description("Lehçe")]
        Lehce = 44,
        [Description("Letonyaca")]
        Letonyaca = 45,
        [Description("Litvanca")]
        Litvanca = 46,
        [Description("Macarca")]
        Macarca = 47,
        [Description("Makedonca")]
        Makedonca = 48,
        [Description("Malay Dili")]
        MalayDili = 49,
        [Description("Maltaca")]
        Maltaca = 50,
        [Description("Moğolca")]
        Mogolca = 51,
        [Description("Moldovaca")]
        Moldovaca = 52,
        [Description("Norveççe")]
        Norvecce = 53,
        [Description("Osmanlıca")]
        Osmanlica = 54,
        [Description("Özbekçe")]
        Ozbekce = 55,
        [Description("Peştuca")]
        Pestuca = 56,
        [Description("Portekizce")]
        Portekizce = 57,
        [Description("Quechua")]
        Quechua = 58,
        [Description("Romence")]
        Romence = 59,
        [Description("Rusça")]
        Rusca = 60,
        [Description("Sanskritçe")]
        Sanskritce = 61,
        [Description("Sırpça")]
        Sırpca = 62,
        [Description("Slovakça")]
        Slovakca = 63,
        [Description("Slovence")]
        Slovence = 64,
        [Description("Tacikçe")]
        Tacikce = 65,
        [Description("Tamil")]
        Tamil = 66,
        [Description("Tatarca")]
        Tatarca = 67,
        [Description("Tay")]
        Tay = 68,
        [Description("Tigrinya")]
        Tigrinya = 69,
        [Description("Türkçe")]
        Turkce = 70,
        [Description("Türkmence")]
        Turkmence = 71,
        [Description("Ukrayna Dili")]
        UkraynaDili = 72,
        [Description("Vietnamca")]
        Vietnamca = 73,
        [Description("Yunanca")]
        Yunanca = 74,
        [Description("Urduca")]
        Urduca = 75,
        [Description("Diğer")]
        Diger = 76,
    }

    [EnumInfo(typeof(SH_PersonLanguages), "Reads")]
    public enum EnumSH_PersonLanguagesReads
    {

        [Description("Başlangıç")]
        Baslangic = 0,
        [Description("Temel")]
        Temel = 1,
        [Description("Orta")]
        Orta = 3,
        [Description("İyi")]
        Iyi = 4,
        [Description("İleri")]
        Ileri = 5
    }

    partial class WorkOfTimeDatabase
    {

        public SH_PersonLanguages GetSH_PersonLanguagesByUserIdAndLanguage(Guid userId, int language, DbTransaction tran = null)
        {
            using (var db = GetDB(tran)) 

            {
                return db.Table<SH_PersonLanguages>().Where(x => x.UserId == userId && x.Languages == language).Execute().FirstOrDefault();
            }
        }

        public SH_PersonLanguages GetSH_PersonLanguagesByIdAndUserIdAndLanguage(Guid id, Guid userId, int language, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<SH_PersonLanguages>().Where(x => x.id != id && x.UserId == userId && x.Languages == language).Execute().FirstOrDefault();
            }
        }
    }
}
