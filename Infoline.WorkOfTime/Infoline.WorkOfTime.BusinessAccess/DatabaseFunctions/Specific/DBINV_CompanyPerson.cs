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

    [EnumInfo(typeof(INV_CompanyPerson), "JobLeaving")]
    public enum EnumINV_CompanyPersonJobLeaving
    {

        [Description("Deneme süreli iş sözleşmesinin işverence feshi")]
        IsverenFeshi = 0,
        [Description("Deneme süreli iş sözleşmesinin işçi tarafından feshi")]
        IsciFeshi = 1,
        [Description("Belirsiz süreli iş sözleşmesinin işçi tarafından feshi (istifa)")]
        BelirsizIstifa = 2,
        [Description("Belirsiz süreli iş sözleşmesinin işveren tarafından haklı sebep bildirilmeden feshi")]
        BelirsizFeshi = 3,
        [Description("Belirli süreli iş sözleşmesinin sona ermesi")]
        IsSonaErmesi = 4,
        [Description("Emeklilik nedeniyle")]
        EmeklilikNedeniyle = 5,
        [Description("Ölüm")]
        Olum = 6,
        [Description("Askerlik")]
        Askerlik = 7,
        [Description("Kadın personelin evlenmesi")]
        KadinPersonelEvlilik = 8,
        [Description("Sözleşme sona ermeden sigortalının aynı işverene ait diğer işyerine nakli")]
        AyniOfisDegisikligi = 9,
        [Description("İşyerinin kapanması")]
        IsyerininKapanmasi = 10,
        [Description("İşin sona ermesi")]
        IsinSonaErmesi = 11,
        [Description("Statü değişikliği")]
        StatuDegisikligi = 12,
        [Description("Diğer nedenler")]
        DigerNedenler = 13,
        [Description("İşçi tarafından zorunlu nedenle fesih")]
        IsciZorunluFesih = 14,
        [Description("İşçi tarafından sağlık nedeniyle fesih")]
        IsciSaglikFesih = 15,
        [Description("İşçi tarafından işverenin ahlak ve iyiniyet kurallarına aykırı davranışı nedeni ile fesih")]
        IsciAhlakFesih = 16,
        [Description("Disiplin kurulu kararı ile fesih")]
        DisiplinKuruluFesih = 17,
        [Description("İşveren tarafından zorunlu nedenlerle ve tutukluluk nedeniyle fesih")]
        IsverenTutuklukFesih = 18,
        [Description("İşveren tarafından sağlık nedeni ile fesih")]
        IsverenSaglikFesih = 19,
        [Description("İşveren tarafından işçinin ahlak ve iyiniyet kurallarına aykırı davranışı nedeni ile fesih")]
        IsverenAhlakFesih = 20,
        [Description("İşyerinin devri, işin veya işyerinin niteliğinin değişmesi nedeniyle fesih")]
        IsinDevri = 21,
        [Description("Doğum Nedeniyle İşten Ayrılma")]
        DoğumAyrilma = 22,
        [Description("İşletme Değişikliği")]
        IsletmeDegisikligi = 23

    }

    public partial class WorkOfTimeDatabase
    {
        public INV_CompanyPerson[] GetINV_CompanyPersonByUserId(Guid userId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<INV_CompanyPerson>().Where(a => a.IdUser == userId).OrderBy(a => a.created).Execute().ToArray();
            }
        }

        public INV_CompanyPerson[] GetINV_CompanyPersonByCompanyId(Guid companyId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<INV_CompanyPerson>().Where(x => x.CompanyId == companyId && x.JobStartDate <= DateTime.Now && x.JobEndDate >= DateTime.Now).OrderByDesc(a => a.created).Execute().ToArray();
            }
        }
        public INV_CompanyPerson[] GetINV_CompanyPersonByCompanyIds(Guid[] ids, DbTransaction tran = null)
        {
            using (var db = GetDB())
            {
                return db.Table<INV_CompanyPerson>().Where(a => a.CompanyId.In(ids)).Execute().ToArray();
            }
        }

    }

}
