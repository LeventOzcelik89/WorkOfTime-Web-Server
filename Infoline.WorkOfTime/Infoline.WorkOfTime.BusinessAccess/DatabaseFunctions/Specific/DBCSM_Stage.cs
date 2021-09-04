using Infoline.WorkOfTime.BusinessData;
using System.ComponentModel;
using System.Data.Common;
using System.Linq;

namespace Infoline.WorkOfTime.BusinessAccess
{

    [EnumInfo(typeof(CSM_Stage), "status")]
    public enum EnumCSM_StageStatus
    {
        [Description("Yeni Form")]
        FormDoldurdu = 0,
        [Description("Olumlu")]
        Olumlu = 1,
        [Description("Olumsuz")]
        Olumsuz = 2,
        [Description("Devam Ediyor")]
        DevamEdiyor = 3,
        [Description("Yeni Randevu")]
        Randevu = 4,
    }


    partial class WorkOfTimeDatabase
    {

        public CSM_Stage CSM_StageByStatus(short status, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<CSM_Stage>().Where(a => a.status == status).Execute().FirstOrDefault();
            }
        }
    }
}
