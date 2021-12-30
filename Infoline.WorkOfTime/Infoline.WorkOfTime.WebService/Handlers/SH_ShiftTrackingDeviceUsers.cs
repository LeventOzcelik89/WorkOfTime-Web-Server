using Infoline.Framework.Database;
using Infoline.Web.SmartHandlers;
using Infoline.WorkOfTime.BusinessAccess;using Infoline.WorkOfTime.BusinessData;using System;
using System.ComponentModel.Composition;
using System.Web;

namespace Infoline.WorkOfTime.WebService
{
    [Export(typeof(ISmartHandler))]
    public partial class SH_ShiftTrackingDeviceUsersHandler : BaseSmartHandler
    {
        public SH_ShiftTrackingDeviceUsersHandler()
            : base("SH_ShiftTrackingDeviceUsers")
        {

        }

        /// <summary>
        /// SH_ShiftTrackingDeviceUsers tablosundaki kayıtları listelemek için kullanılan Web Servisdir..
        /// </summary>
        /// <param name="context">Opsiyonel olarak [Condition] nesnesi alır. Parametreleri ise;
        /// Filter = [ Field, Operator, Value ] dizi şeklinde alır.
        ///     Field =         Tablodaki kolon adı
        ///     Operator =
        ///         Equal               Eşittir
        ///         NotEqual            Eşit Değildir
        ///         GreaterThan         Büyüktür
        ///         GreaterThanOrEqual  Büyüktür veya Eşittir
        ///         LessThan            Küçüktür
        ///         LessThanOrEqual     Küçüktür veya Eşittir
        ///         In                  İçerir, Dizi formatında
        ///     Sort =          Field ve Type olarak 2 property alır
        ///         Field = Tablodaki kolon adı
        ///         Type = "ASC" ve "DESC" değerlerini alır
        ///     Fields =        Tablodan çekilecek alanlar belirtilir. Dizi şeklinde string olarak giriş yapılır. Örn : ["id","Name"]. Yazılmaz ise tüm kolonlar gelir.
        ///     StartIndex =    Kayıtların alınmaya başlanacağı sırayı belirtir. Örn 100 değeri girilir ise ilk 100 kayıt alınmaz. Kayıtlar 101. kayıttan itibaren gelmeye başlar.
        ///     Count =         Kayıtların toplamda en fazla kaç tane geleceğinin belirtildiği propertydir. Örn : 200 Denir ise en fazla 200 satır kayıt gelir.
        /// </param>
        [HandleFunction("SH_ShiftTrackingDeviceUsers/GetAll")]
        public void SH_ShiftTrackingDeviceUsersGetAll(HttpContext context)
        {
            try
            {
                var c = ParseRequest<Condition>(context);
                var cond = c != null ? CondtionToQuery.Convert(c) : new SimpleQuery();
                var db = new WorkOfTimeDatabase();
                var data = db.GetSH_ShiftTrackingDeviceUsers(cond);
                RenderResponse(context, data);
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }

        /// <summary>
        /// SH_ShiftTrackingDeviceUsers tablosundan id ye göre tekil kayıt çekmek için kullanılan Web Servisdir..
        /// </summary>
        /// <param name="context">
        /// [id] parametresini alır. Guid.
        /// </param>
        [HandleFunction("SH_ShiftTrackingDeviceUsers/GetById")]
        public void SH_ShiftTrackingDeviceUsersGetById(HttpContext context)
        {
            try
            {
                var db = new WorkOfTimeDatabase();
                var id = context.Request["id"];
                var data = db.GetSH_ShiftTrackingDeviceUsersById(new Guid((string)id));
                RenderResponse(context, data);
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }

        /// <summary>
        /// SH_ShiftTrackingDeviceUsers Tablosuna kayıt ekleme işlemi için kullanılan Web Servisdir..
        /// </summary>
        /// <param name="context">
        /// Parametre olarak SH_ShiftTrackingDeviceUsers nesnesinden alır. ( JSON formatında )
        /// </param>
        [HandleFunction("SH_ShiftTrackingDeviceUsers/Insert")]
        public void SH_ShiftTrackingDeviceUsersInsert(HttpContext context)
        {
            try
            {
            var db = new WorkOfTimeDatabase();
            var data = ParseRequest<SH_ShiftTrackingDeviceUsers>(context);
            var dbresult = db.InsertSH_ShiftTrackingDeviceUsers(data);


            RenderResponse(context, dbresult);
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }

        /// <summary>
        /// SH_ShiftTrackingDeviceUsers Tablosunda kayıt güncellemek için kullanılan Web Servisdir.
        /// </summary>
        /// <param name="context">
        /// Parametre olarak SH_ShiftTrackingDeviceUsers nesnesinden alır. ( JSON formatında )
        /// </param>
        [HandleFunction("SH_ShiftTrackingDeviceUsers/Update")]
        public void SH_ShiftTrackingDeviceUsersUpdate(HttpContext context)
        {
            try
            {

                var db = new WorkOfTimeDatabase();
                var data = ParseRequest<SH_ShiftTrackingDeviceUsers>(context);
                var dbresult = db.UpdateSH_ShiftTrackingDeviceUsers(data);

                RenderResponse(context, dbresult);
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }

        /// <summary>
        /// SH_ShiftTrackingDeviceUsers Tablosundan kayıt silmek için kullanılan servistir.
        /// </summary>
        /// <param name="context">
        /// tekil parametre olarak id ( Guid ) alanıda gönderilebilir.
        /// SH_ShiftTrackingDeviceUsers Nesnesi de gönderilebilir.
        /// </param>
        [HandleFunction("SH_ShiftTrackingDeviceUsers/Delete")]
        public void SH_ShiftTrackingDeviceUsersDelete(HttpContext context)
        {
            try
            {
                var dbresult = new ResultStatus();
                var db = new WorkOfTimeDatabase();
                var id = context.Request["id"];
                if (id != null)
                {
                    dbresult = db.DeleteSH_ShiftTrackingDeviceUsers(new Guid((string)id));
                }
                else
                {
                    var item = ParseRequest<SH_ShiftTrackingDeviceUsers>(context);
                    dbresult = db.DeleteSH_ShiftTrackingDeviceUsers(item);
                }


                RenderResponse(context, dbresult);

            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }

    }
}
