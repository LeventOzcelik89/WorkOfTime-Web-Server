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

    [BusinessAccess.EnumInfo(typeof(PRD_InventoryAction), "type")]
    public enum EnumPRD_InventoryActionType
    {
        //Konum Hareketleri
        [Description("Depoda"), Generic("icon", "icon-warehouse")]
        Depoda = 0,
        [Description("Personelde"), Generic("icon", "icon-user-6")]
        Personelde = 1,
        [Description("Kiraya Verildi"), Generic("icon", "icon-loop")]
        KirayaVerildi = 2,
        [Description("Çıkış Yapıldı"), Generic("icon", " icon-loop-alt")]
        CikisYapildi = 3,
        [Description("Harcandı/Tükendi"), Generic("icon", "icon-trash-4")]
        Harcandi = 4,
        [Description("Kayboldu"), Generic("icon", "icon-question")]
        Kayboldu = 5,


        //Log Hareketleri
        [Description("Stok Giriş"), Generic("icon", "icon-plus-5")]
        StokGiris = 20,
        [Description("Stok Çıkış"), Generic("icon", "icon-minus-4")]
        StokCikis = 21,
        [Description("İrsaliyeli Giriş"), Generic("icon", "icon-level-up-1")]
        IrsaliyeliGiris = 22,
        [Description("İrsaliyeli Çıkış"), Generic("icon", "icon-level-down-1")]
        IrsaliyeliCikis = 23,
        [Description("Sayım Fazlası"), Generic("icon", "icon-plus-squared")]
        SayimFazlasi = 24,
        [Description("Sayım Eksigi"), Generic("icon", "icon-minus-squared")]
        SayimEksigi = 25,
        [Description("Zimmet İade Alındı"), Generic("icon", "icon-user-add-1")]
        ZimmetIade = 26,
        [Description("Zimmet Verildi"), Generic("icon", "icon-user-delete")]
        ZimmetVerildi = 27,
        [Description("Kiralandı"), Generic("icon", "icon-loop-outline")]
        Kiralandi = 28,
        [Description("Üretildi"), Generic("icon", "icon-wpforms")]
        Uretildi = 29,
        [Description("Depolar Arası Transfer"), Generic("icon", " icon-updown-circle")]
        Transfer = 30,
        [Description("Bakım Envanteri"), Generic("icon", "icon-cogs")]
        BakimEnvanteri = 101,
        [Description("Cihaz Değişti"), Generic("icon", "icon-phone")]
        CihazDegisimi = 102,
        [Description("Teknik Servise Geldi"), Generic("icon", "icon-cogs")]
        TeknikServiceGeldi = 103,
    }






    partial class WorkOfTimeDatabase
    {
        public PRD_InventoryAction[] GetPRD_InventoryActionByInventoryId(Guid inventoryId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<PRD_InventoryAction>().Where(a => a.inventoryId == inventoryId).Execute().ToArray();
            }
        }
        public PRD_InventoryAction[] GetPRD_InventoryActionByDataId(Guid dataId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<PRD_InventoryAction>().Where(a => a.dataId == (dataId)).Execute().ToArray();
            }
        }
        public PRD_InventoryAction[] GetPRD_InventoryActionByTransactionId(Guid transactionId)
        {
            using (var db = GetDB())
            {
                return db.Table<PRD_InventoryAction>().Where(a => a.transactionId == transactionId).Execute().ToArray();
            }
        }

    }
}
