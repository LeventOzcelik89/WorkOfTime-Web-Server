using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infoline.Framework.Database;
using System.Data.SqlClient;
using System.Linq.Expressions;
using System.Data.Common;
using Infoline.WorkOfTime.BusinessData;
using System.ComponentModel;

namespace Infoline.WorkOfTime.BusinessAccess
{


    [EnumInfo(typeof(CRM_PresentationAction), "type")]
    public enum EnumCRM_PresentationActionType
    {
        [Description("Fırsata Teklif Eklendi"), Generic("icon", "icon-pencil-alt")]
        YeniTeklif = 0,
        [Description("Fırsata Sipariş Eklendi"), Generic("icon", "icon-basket")]
        YeniSipariş = 1,
        [Description("Fırsata Not Eklendi"), Generic("icon", "icon-comment")]
        NotEkle = 2,
        [Description("Fırsat Oluşturuldu"), Generic("icon", "icon-dollar")]
        YeniFırsat = 3,
        [Description("Fırsat Aşaması Belirlendi"), Generic("icon", "icon-flash")]
        AsamaBelirleme = 4,
        [Description("Fırsat Aşaması Güncellendi"), Generic("icon", "icon-flash")]
        AsamaGüncelleme = 5,
        [Description("Fırsata Aktivite Eklendi"), Generic("icon", "icon-users")]
        YeniAktivite = 6,
        [Description("Fırsat Düzenlendi"), Generic("icon", "icon-edit")]
        FırsatDüzenle = 7,
        [Description("Fırsat Aktivitesi Düzenlendi"), Generic("icon", "icon-users")]
        AktiviteDüzenle = 8,
        [Description("Masraf Eklendi"), Generic("icon", "icon-money")]
        MasrafEkle = 9,
        [Description("Masraf Düzenlendi"), Generic("icon", "icon-money")]
        MasrafDüzenlendi = 10,
        [Description("Önem Derecesi Güncellendi"), Generic("icon", "icon-star")]
        OnemGuncellendi = 11,
        [Description("Avans Eklendi"), Generic("icon", "icon-money")]
        AvansEkle = 12,
        [Description("Avans Düzenlendi"), Generic("icon", "icon-money")]
        AvansDüzenlendi = 13,
    }

    partial class WorkOfTimeDatabase
    {
        public CRM_PresentationAction[] GetCRM_PresentationActionByPresentationId(Guid id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<CRM_PresentationAction>().Where(a => a.presentationId == id).Execute().ToArray();
            }
        }
    }
}
