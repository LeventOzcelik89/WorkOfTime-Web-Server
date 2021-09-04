using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infoline.Framework.Helper
{
    public static class SystemConst
    {
        public enum CachePrefix
        {
            User = 0,
            Roles = 1,
            Firms = 2,
            Cities = 3
        }

        public const string CACHE_KEY__ALL = "all";

        public const string TOKEN__NAME = "ACToken";

        public const int TOKEN__EXPIREDURATION_MINUTES = 720;

        public const string TOKEN__ABONE_ID = "AboneId";
        public const string TOKEN__TABLET_ID = "TabletId";
        public const string TOKEN__APP_VERSION = "AppVersion";
        public const string TOKEN__DATECREATED = "DateCreated";
        public const string TOKEN__USER_ID = "UserId";
        public const string TOKEN__COORDINATE = "Coordinate";

        public const string ERROR_MESSAGE__GENERAL = "Hata oluştu";
        public const string ERROR_MESSAGE__CREDENTIAL = "Kimlik bilgileri hatası";

        public const string REQUEST_PROPERTY__LOG = "log";

        public const string MESSAGE_RESULT__SUCCESS = "İşlem Başarılı";
        public const string MESSAGE_RESULT__DATABASE_ERROR = "Veritabanı Hatası";
        public const string MESSAGE_RESULT__BUSSINESS_ERROR = "Beklenmeyen Hata";
        public const string MESSAGE_RESULT__UNEXPECTED_ERROR = "Beklenmeyen Hata";


        #region BadgeTypeNames

        public const string BadgeType_CKS = "ÇKS";
        public const string BadgeType_TUKAS2014Urun = "TÜKAS 2014 Ürün";
        public const string BadgeType_TUKAS2014Hayvan = "TÜKAS 2014 Hayvan";
        public const string BadgeType_TUKAS2014Ekipman = "TÜKAS 2014 Sensör";
        public const string BadgeType_TUKAS2016Urun = "TÜKAS 2016 Ürün";
        public const string BadgeType_TUKAS2016Hayvan = "TÜKAS 2016 Hayvan";
        public const string BadgeType_TUKAS2016Ekipman = "TÜKAS 2016 Sensör";
        public const string BadgeType_HAYBIS = "HAYBİS";

        #endregion
    }
}
