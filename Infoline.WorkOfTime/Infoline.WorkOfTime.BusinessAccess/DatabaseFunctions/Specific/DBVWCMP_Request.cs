using Infoline.WorkOfTime.BusinessData;
using System;
using System.ComponentModel;
using System.Data.Common;
using System.Linq;

namespace Infoline.WorkOfTime.BusinessAccess
{

    [EnumInfo(typeof(CMP_Invoice), "requestStatus")]
    public enum EnumCMP_RequestStatus
    {
        [Description("Talep Onayı Bekleniyor"), Generic("icon", "fa fa-spinner", "color", "rgb(248, 172, 89)")]
        YoneticiOnayiBekleniyor = 0,
        [Description("Teklif Toplanması Bekleniyor"), Generic("icon", "icon-hourglass-1", "color", "rgb(248, 172, 89)")]
        TeklifToplanmasiBekleniyor = 1,
        [Description("Teklif Onayı Bekleniyor"), Generic("icon", "icon-back-in-time", "color", "rgb(28, 132, 198)")]
        TeklifToplandiOnayBekleniyor = 2,
        [Description("Yeni Teklif Toplanması Bekleniyor"), Generic("icon", "icon-hourglass-1", "color", "rgb(248, 172, 89)")]
        YeniTeklifToplanmasiBekleniyor = 3,
        [Description("Tedarik Edilmesi Bekleniyor (Tekliflerden biri Onaylandı)"), Generic("icon", "icon-cart", "color", "rgb(35, 198, 200)")]
        TeklifOnaylandi = 4,
        [Description("Tedarik Edildi (Faturası Alındı)"), Generic("icon", "fa fa-check-square-o", "color", "rgb(26, 179, 148)")]
        FaturasiAlindi = 5,
        [Description("Talep Reddedildi"), Generic("icon", "fa fa-times-circle", "color", "rgb(237, 85, 101)")]
        TalepReddedildi = 6,
        [Description("Talep İptal Edildi"), Generic("icon", "icon-cancel-outline", "color", "rgb(150, 17, 17, 1)")]
        TalepIptalEdildi = 7
    }

    partial class WorkOfTimeDatabase
    {
        public VWCMP_Request GetVWCMP_RequestByTaskId(Guid taskId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<VWCMP_Request>().Where(a => a.taskId == taskId).Execute().FirstOrDefault();
            }
        }
    }
}
