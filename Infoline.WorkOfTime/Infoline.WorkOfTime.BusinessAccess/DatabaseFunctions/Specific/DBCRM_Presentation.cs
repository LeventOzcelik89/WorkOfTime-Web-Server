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
    [EnumInfo(typeof(CRM_Presentation), "PriorityLevel")]
    public enum EnumCRM_PresentationPriorityLevel
    {
        [Description("Önem Derecesi 1")]
        PriorityLevel1 = 1,
        [Description("Önem Derecesi 2")]
        PriorityLevel2 = 2,
        [Description("Önem Derecesi 3")]
        PriorityLevel3 = 3,
        [Description("Önem Derecesi 4")]
        PriorityLevel4 = 4,
        [Description("Önem Derecesi 5")]
        PriorityLevel5 = 5,
    }

    [EnumInfo(typeof(CRM_Presentation), "PlaceofArrival")]
    public enum EnumCRM_PresentationPlaceofArrival
    {
        [Description("Web")]
        Web = 1,
        [Description("Sosyal Medya")]
        SosyalMedya = 2,
        [Description("Google Reklamları")]
        GoogleReklamlari = 3,
        [Description("Çağrı Merkezi")]
        CagriMerkezi = 4,
        [Description("Şirket Telefonu")]
        SirketTelefonu = 5,
        [Description("Bayi Referansı")]
        BayiReferansi = 6,
        [Description("Danışma Referansı")]
        DanismaReferansi = 7,
    }

    partial class WorkOfTimeDatabase
    {
        public CRM_Presentation[] GetCRM_PresentationByCompanyId(Guid id)
        {
            using (var db = GetDB())
            {
                return db.Table<CRM_Presentation>().Where(a => a.ChannelCompanyId == id || a.CustomerCompanyId == id).Execute().ToArray();
            }
        }


    }


}
